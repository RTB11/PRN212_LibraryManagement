using LibraryManagementProject.Model;
using Microsoft.EntityFrameworkCore;
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
            FilterAuthors();
        }

        private void FilterAuthors()
        {
            if (_context == null || lvAuthors == null)
                return;

            var query = _context.Authors
                                .Include(a => a.Books)
                                .ThenInclude(b => b.Category)
                                .AsQueryable();

            if (txtSearch != null && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string keyword = txtSearch.Text.Trim();
                query = query.Where(a => (a.AuthorName != null && a.AuthorName.Contains(keyword)) ||
                                         a.AuthorId.ToString().Contains(keyword));
            }

            lvAuthors.ItemsSource = query.ToList();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterAuthors();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            FilterAuthors();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearch != null)
                txtSearch.Text = string.Empty;

            FilterAuthors();
        }

        private void lvAuthors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedAuthor = lvAuthors.SelectedItem as Author;

            if (selectedAuthor != null)
            {
                txtAuthorName.Text = selectedAuthor.AuthorName;
                txtBookCount.Text = selectedAuthor.BookCount.ToString();
                lbAuthorBooks.ItemsSource = selectedAuthor.Books?.ToList();
            }
            else
            {
                txtAuthorName.Clear();
                txtBookCount.Clear();
                lbAuthorBooks.ItemsSource = null;
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
                AuthorName = name
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

            _context.SaveChanges();
            MessageBox.Show("Update author successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadAuthors();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            selectedAuthor = null;
            lvAuthors.SelectedItem = null;
            txtAuthorName.Clear();
            txtBookCount.Clear();
            lbAuthorBooks.ItemsSource = null;
        }
    }
}