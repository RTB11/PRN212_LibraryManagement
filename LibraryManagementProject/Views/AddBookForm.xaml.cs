using LibraryManagementProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagementProject.Views
{
    public partial class AddBookWindow : Window
    {
        private readonly LibraryContext _context = new();
        private List<Author> _allAuthors = new();
        private Book? _book;

        public AddBookWindow(Book? book = null)
        {
            InitializeComponent();

            LoadData();

            _book = book;

            if (_book != null)
            {
                LoadBook();
                Title = "Update Book";
            }
            else
            {
                Title = "Add Book";
            }
        }

        private void LoadData()
        {
            _allAuthors = _context.Authors.ToList();
            cbAuthor.ItemsSource = _allAuthors;

            cbCategory.ItemsSource = _context.Categories.ToList();
        }

        private void LoadBook()
        {
            if (_book == null)
                return;

            txtTitle.Text = _book.Title;

            txtIsbn.Text = _book.Isbn;

            cbAuthor.SelectedValue = _book.AuthorId;

            cbCategory.SelectedValue = _book.CategoryId;

            txtPublishYear.Text =
                _book.PublishYear?.ToString() ?? "";

            txtPrice.Text = _book.Price.ToString() ?? "";

            txtQuantity.Text =
                _book.Quantity.ToString();
        }

        private void cbAuthor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up ||
                e.Key == Key.Down ||
                e.Key == Key.Enter ||
                e.Key == Key.Escape ||
                e.Key == Key.Tab)
            {
                return;
            }

            string searchText = cbAuthor.Text;

            var filtered = string.IsNullOrWhiteSpace(searchText)
                ? _allAuthors
                : _allAuthors.Where(a => a.AuthorName != null &&
                                          a.AuthorName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                             .ToList();

            cbAuthor.ItemsSource = filtered;
            cbAuthor.IsDropDownOpen = true;

            if (cbAuthor.Template.FindName("PART_EditableTextBox", cbAuthor) is TextBox textBox)
            {
                textBox.Text = searchText;
                textBox.CaretIndex = searchText.Length;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            string authorName = cbAuthor.Text.Trim();
            Author? author = null;

            if (cbAuthor.SelectedItem is Author selectedAuth && selectedAuth.AuthorName.Equals(authorName, StringComparison.OrdinalIgnoreCase))
            {
                author = selectedAuth;
            }
            else
            {
                author = _context.Authors.FirstOrDefault(a => a.AuthorName.ToLower() == authorName.ToLower());
            }

            if (author == null)
            {
                var dialogResult = MessageBox.Show(
                    $"Author '{authorName}' does not exist in the database. Do you want to add this author?",
                    "Add New Author",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    author = new Author
                    {
                        AuthorName = authorName
                    };
                    _context.Authors.Add(author);
                    _context.SaveChanges();

                    _allAuthors.Add(author);
                    cbAuthor.ItemsSource = _allAuthors;
                }
                else
                {
                    return;
                }
            }

            Book book;

            // ADD
            if (_book == null)
            {
                book = new Book();
                book.Status = true;
            }
            // UPDATE
            else
            {
                book = _context.Books
                    .FirstOrDefault(x => x.BookId == _book.BookId);

                if (book == null)
                {
                    MessageBox.Show("Book not found!");
                    return;
                }
            }

            bool exists = _context.Books.Any(x =>
                        x.Isbn == txtIsbn.Text.Trim()
                        && (_book == null || x.BookId != _book.BookId));

            if (exists)
            {
                MessageBox.Show("ISBN already exists!");
                return;
            }

            book.Title = txtTitle.Text.Trim();
            book.Isbn = txtIsbn.Text.Trim();
            book.AuthorId = author.AuthorId;
            book.CategoryId = (int)cbCategory.SelectedValue;
            book.PublishYear = int.Parse(txtPublishYear.Text);
            book.Quantity = int.Parse(txtQuantity.Text);
            book.Price = decimal.Parse(txtPrice.Text);

            if (_book == null)
            {
                book.AvailableQuantity = book.Quantity;
            }

            if (_book == null)
            {
                _context.Books.Add(book);
            }

            try
            {
                _context.SaveChanges();

                if (_book == null)
                {
                    MessageBox.Show(
                        "Add book successfully!",
                        "Success");
                }
                else
                {
                    MessageBox.Show(
                        "Update book successfully!",
                        "Success");
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error");
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter title");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtIsbn.Text))
            {
                MessageBox.Show("Please enter ISBN");
                return false;
            }

            if (string.IsNullOrWhiteSpace(cbAuthor.Text))
            {
                MessageBox.Show("Please enter or select author");
                return false;
            }

            if (cbCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select category");
                return false;
            }

            if (!int.TryParse(
                txtPublishYear.Text,
                out int year))
            {
                MessageBox.Show(
                    "Please enter valid publish year");
                return false;
            }

            if (year < 0)
            {
                MessageBox.Show(
                    "Publish year cannot be negative");
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Please enter a valid price");
                return false;
            }

            if (!int.TryParse(
                txtQuantity.Text,
                out int quantity))
            {
                MessageBox.Show(
                    "Please enter valid quantity");
                return false;
            }

            if (quantity < 0)
            {
                MessageBox.Show(
                    "Quantity cannot be negative");
                return false;
            }

            return true;
        }
    }
}