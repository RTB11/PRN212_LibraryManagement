using LibraryManagementProject.Model;
using System;
using System.Linq;
using System.Windows;

namespace LibraryManagementProject.Views
{
    public partial class AddBookWindow : Window
    {
        private readonly LibraryContext _context = new();

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
            cbAuthor.ItemsSource = _context.Authors.ToList();
            cbCategory.ItemsSource = _context.Categories.ToList();
        }

        private void LoadBook()
        {
            txtTitle.Text = _book.Title;
            txtIsbn.Text = _book.Isbn;

            cbAuthor.SelectedValue = _book.AuthorId;
            cbCategory.SelectedValue = _book.CategoryId;

            txtPublisher.Text = _book.Publisher;
            txtPublishYear.Text = _book.PublishYear.ToString();
            txtPrice.Text = _book.Price.ToString();
            txtQuantity.Text = _book.Quantity.ToString();
            txtShelf.Text = _book.Shelf;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Book book;

            if (_book == null)
            {
                book = new Book();
                book.CreatedAt = DateTime.Now;
                book.Status = true;
            }
            else
            {
                book = _context.Books.Find(_book.BookId);

                if (book == null)
                {
                    MessageBox.Show("Book not found!");
                    return;
                }
            }

            book.Title = txtTitle.Text;
            book.Isbn = txtIsbn.Text;
            book.AuthorId = (int)cbAuthor.SelectedValue;
            book.CategoryId = (int)cbCategory.SelectedValue;
            book.Publisher = txtPublisher.Text;
            book.PublishYear = int.Parse(txtPublishYear.Text);
            book.Price = decimal.Parse(txtPrice.Text);
            book.Quantity = int.Parse(txtQuantity.Text);
            book.AvailableQuantity = book.Quantity;
            book.Shelf = txtShelf.Text;

            if (_book == null)
            {
                _context.Books.Add(book);
                MessageBox.Show("Add book successfully!");
            }
            else
            {
                MessageBox.Show("Update book successfully!");
            }

            _context.SaveChanges();

            DialogResult = true;
            Close();
        }
    }
}