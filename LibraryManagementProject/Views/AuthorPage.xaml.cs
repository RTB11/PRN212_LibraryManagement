using LibraryManagementProject.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibraryManagementProject.Views
{
    public partial class AuthorPage : Page
    {

        private readonly LibraryContext _context = new();

        private Author selectedAuthor;

        public AuthorPage()
        {
            InitializeComponent();

            LoadAuthors();
        }

        private void LoadAuthors()
        {
            lvAuthors.ItemsSource =
                _context.Authors.ToList();
        }
        private void lvAuthors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedAuthor =
                lvAuthors.SelectedItem as Author;


            if (selectedAuthor != null)
            {
                txtAuthorName.Text =
                    selectedAuthor.AuthorName;


                txtBiography.Text =
                    selectedAuthor.Biography;
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtAuthorName.Text))
            {
                MessageBox.Show("Author name is required");
                return;
            }


            Author author = new Author();

            author.AuthorName = txtAuthorName.Text;

            author.Biography = txtBiography.Text;

            _context.Authors.Add(author);
            _context.SaveChanges();
            MessageBox.Show("Add successfully");

            LoadAuthors();
            ClearForm();

        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (selectedAuthor == null)
            {
                MessageBox.Show("Please select author");
                return;
            }

            selectedAuthor.AuthorName =
                txtAuthorName.Text;
            selectedAuthor.Biography =
                txtBiography.Text;

            _context.SaveChanges();
            MessageBox.Show("Update successfully");
            LoadAuthors();

        } 

        private void ClearForm()
        {
            selectedAuthor = null;

            txtAuthorName.Clear();
            txtBiography.Clear();

        }

    }
}