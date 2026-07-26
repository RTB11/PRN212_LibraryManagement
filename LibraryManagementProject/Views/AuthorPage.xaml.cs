using LibraryManagementProject.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibraryManagementProject.Views
{
    public partial class AuthorPage : Page
    {
        private readonly LibraryContext _context = new();
        private Author? selectedAuthor;

        public AuthorPage()
        {
            InitializeComponent();
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            lvAuthors.ItemsSource = _context.Authors.ToList();
        }

        private void lvAuthors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedAuthor = lvAuthors.SelectedItem as Author;

            if (selectedAuthor != null)
            {
                txtAuthorName.Text = selectedAuthor.AuthorName;
                txtBiography.Text = selectedAuthor.Biography;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = txtAuthorName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Author name is required!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtAuthorName.Focus();
                return;
            }

            bool exists = _context.Authors.Any(a => a.AuthorName != null && a.AuthorName.ToLower() == name.ToLower());
            if (exists)
            {
                MessageBox.Show("Author with this name already exists in the system!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtAuthorName.Focus();
                return;
            }

            Author author = new Author
            {
                AuthorName = name,
                Biography = txtBiography.Text.Trim()
            };

            _context.Authors.Add(author);
            _context.SaveChanges();
            MessageBox.Show("Add author successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            LoadAuthors();
            ClearForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAuthor == null)
            {
                MessageBox.Show("Please select an author to update!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string name = txtAuthorName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Author name is required!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtAuthorName.Focus();
                return;
            }

            bool exists = _context.Authors.Any(a => a.AuthorName != null && a.AuthorName.ToLower() == name.ToLower() && a.AuthorId != selectedAuthor.AuthorId);
            if (exists)
            {
                MessageBox.Show("Author with this name already exists in the system!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtAuthorName.Focus();
                return;
            }

            selectedAuthor.AuthorName = name;
            selectedAuthor.Biography = txtBiography.Text.Trim();

            _context.SaveChanges();
            MessageBox.Show("Update author successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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