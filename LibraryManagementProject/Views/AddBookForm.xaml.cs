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
            if (_book == null)
                return;

            txtTitle.Text = _book.Title;

            txtIsbn.Text = _book.Isbn;

            cbAuthor.SelectedValue = _book.AuthorId;

            cbCategory.SelectedValue = _book.CategoryId;

            txtPublisher.Text = _book.Publisher;

            txtPublishYear.Text =
                _book.PublishYear?.ToString() ?? "";

            txtQuantity.Text =
                _book.Quantity.ToString();

            txtShelf.Text = _book.Shelf;
        }



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;


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



            book.Title = txtTitle.Text.Trim();


            book.Isbn = txtIsbn.Text.Trim();


            book.AuthorId =
                (int)cbAuthor.SelectedValue;


            book.CategoryId =
                (int)cbCategory.SelectedValue;


            book.Publisher =
                txtPublisher.Text.Trim();


            book.PublishYear =
                int.Parse(txtPublishYear.Text);


            book.Quantity =
                int.Parse(txtQuantity.Text);



            /*
             * Khi thêm sách mới:
             * Available = Quantity
             *
             * Khi update:
             * reset lại tồn kho theo Quantity
             * (có thể thay đổi logic sau)
             */

            book.AvailableQuantity =
                book.Quantity;


            book.Shelf =
                txtShelf.Text.Trim();



            if (_book == null)
            {
                _context.Books.Add(book);

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



            try
            {
                _context.SaveChanges();

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



            if (cbAuthor.SelectedItem == null)
            {
                MessageBox.Show("Please select author");
                return false;
            }



            if (cbCategory.SelectedItem == null)
            {
                MessageBox.Show("Please select category");
                return false;
            }



            if (string.IsNullOrWhiteSpace(txtPublisher.Text))
            {
                MessageBox.Show("Please enter publisher");
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



            if (string.IsNullOrWhiteSpace(txtShelf.Text))
            {
                MessageBox.Show("Please enter shelf");
                return false;
            }


            return true;
        }
    }
}