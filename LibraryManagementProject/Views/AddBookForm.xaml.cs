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
        private List<Category> _allCategories = new();
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

            _allCategories = _context.Categories.ToList();
            cbCategory.ItemsSource = _allCategories;
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

            var textBox = (e.OriginalSource as TextBox) ?? (cbAuthor.Template.FindName("PART_EditableTextBox", cbAuthor) as TextBox);
            if (textBox != null)
            {
                textBox.Text = searchText;
                textBox.CaretIndex = searchText.Length;
            }
        }

        private void cbCategory_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up ||
                e.Key == Key.Down ||
                e.Key == Key.Enter ||
                e.Key == Key.Escape ||
                e.Key == Key.Tab)
            {
                return;
            }

            string searchText = cbCategory.Text;

            var filtered = string.IsNullOrWhiteSpace(searchText)
                ? _allCategories
                : _allCategories.Where(c => c.CategoryName != null &&
                                          c.CategoryName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                             .ToList();

            cbCategory.ItemsSource = filtered;
            cbCategory.IsDropDownOpen = true;

            var textBox = (e.OriginalSource as TextBox) ?? (cbCategory.Template.FindName("PART_EditableTextBox", cbCategory) as TextBox);
            if (textBox != null)
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

            string categoryName = cbCategory.Text.Trim();
            Category? category = null;

            if (cbCategory.SelectedItem is Category selectedCat && selectedCat.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase))
            {
                category = selectedCat;
            }
            else
            {
                category = _context.Categories.FirstOrDefault(c => c.CategoryName.ToLower() == categoryName.ToLower());
            }

            if (category == null)
            {
                var dialogResult = MessageBox.Show(
                    $"Category '{categoryName}' does not exist in the database. Do you want to add this category?",
                    "Add New Category",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    category = new Category
                    {
                        CategoryName = categoryName
                    };
                    _context.Categories.Add(category);
                    _context.SaveChanges();

                    _allCategories.Add(category);
                    cbCategory.ItemsSource = _allCategories;
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
            book.CategoryId = category.CategoryId;
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
            string title = txtTitle.Text.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Please enter book title", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTitle.Focus();
                return false;
            }

            string isbn = txtIsbn.Text.Trim();
            if (string.IsNullOrWhiteSpace(isbn))
            {
                MessageBox.Show("Please enter ISBN", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtIsbn.Focus();
                return false;
            }

            bool isbnExists = _context.Books.Any(x => x.Isbn == isbn && (_book == null || x.BookId != _book.BookId));
            if (isbnExists)
            {
                MessageBox.Show("ISBN already exists in the system!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtIsbn.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cbAuthor.Text))
            {
                MessageBox.Show("Please enter or select author", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbAuthor.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cbCategory.Text))
            {
                MessageBox.Show("Please enter or select category", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbCategory.Focus();
                return false;
            }

            if (!int.TryParse(txtPublishYear.Text.Trim(), out int year))
            {
                MessageBox.Show("Please enter a valid publish year", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPublishYear.Focus();
                return false;
            }

            int currentYear = DateTime.Now.Year;
            if (year < 0 || year > currentYear)
            {
                MessageBox.Show($"Publish year must be between 0 and current year ({currentYear})!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPublishYear.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price) || price < 0)
            {
                MessageBox.Show("Please enter a valid non-negative price", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPrice.Focus();
                return false;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Quantity must be a valid positive integer greater than 0!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtQuantity.Focus();
                return false;
            }

            return true;
        }
    }
}