using LibraryManagementProject.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibraryManagementProject.Views
{
    public partial class BookListPageManager : Page
    {
        readonly LibraryContext _context = new LibraryContext();

        private Book? _book;

        public BookListPageManager()
        {
            InitializeComponent();
            LoadCategories();
            LoadBooks();
        }

        private void LoadBooks()
        {
            FilterBooks();
        }

        private void LoadCategories()
        {
            var categories = _context.Categories.OrderBy(c => c.CategoryName).ToList();

            categories.Insert(0, new Category
            {
                CategoryId = 0,
                CategoryName = "All"
            });

            cbCategory.ItemsSource = categories;
            cbCategory.DisplayMemberPath = "CategoryName";
            cbCategory.SelectedValuePath = "CategoryId";
            cbCategory.SelectedIndex = 0;
        }

        private void FilterBooks()
        {
            if (_context == null || cbCategory == null || lvBooks == null)
                return;

            var query = _context.Books
                                .Include(b => b.Author)
                                .Include(b => b.Category)
                                .AsQueryable();

            if (cbCategory.SelectedValue is int categoryId && categoryId != 0)
            {
                query = query.Where(b => b.CategoryId == categoryId);
            }

            if (txtSearch != null && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string keyword = txtSearch.Text.Trim();
                query = query.Where(b => (b.Title != null && b.Title.Contains(keyword)) ||
                                         (b.Author != null && b.Author.AuthorName != null && b.Author.AuthorName.Contains(keyword)) ||
                                         (b.Isbn != null && b.Isbn.Contains(keyword)) ||
                                         (b.Publisher != null && b.Publisher.Contains(keyword)));
            }

            lvBooks.ItemsSource = query.ToList();
        }

        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterBooks();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterBooks();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            FilterBooks();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearch != null)
                txtSearch.Text = string.Empty;

            if (cbCategory != null)
                cbCategory.SelectedIndex = 0;

            FilterBooks();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow window = new AddBookWindow();

            if (window.ShowDialog() == true)
            {
                LoadBooks();
            }
            else
            {
                LoadBooks();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (lvBooks.SelectedItem is not Book book)
            {
                MessageBox.Show("Please select a book!");
                return;
            }

            AddBookWindow window = new AddBookWindow(book);

            if (window.ShowDialog() == true)
            {
                LoadBooks();
            }
        }

        private void lvBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _book = lvBooks.SelectedItem as Book;
        }
    }
}
