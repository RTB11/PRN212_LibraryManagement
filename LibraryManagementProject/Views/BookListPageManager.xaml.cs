using LibraryManagementProject.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibraryManagementProject.Views
{
  
    public partial class BookListPageManager : Page
    {
        private readonly LibraryContext _context = new();

        private Book? _book;

        public BookListPageManager()
        {
            InitializeComponent();
            LoadCategories();

            LoadBooks();
        }

        private void LoadBooks()
        {
            var books = _context.Books
                                .Include(b => b.Author)
                                .Include(b => b.Category)
                                .ToList();

            lvBooks.ItemsSource = books;
        }

        private void LoadCategories()
        {
            var categories = _context.Categories
                                     .OrderBy(c => c.CategoryName)
                                     .ToList();

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
            var query = _context.Books
                                .Include(b => b.Author)
                                .Include(b => b.Category)
                                .AsQueryable();

            if (cbCategory.SelectedValue is int categoryId && categoryId != 0)
            {
                query = query.Where(b => b.CategoryId == categoryId);
            }

            lvBooks.ItemsSource = query.ToList();
        }

        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            FilterBooks();
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow window = new AddBookWindow();

            window.ShowDialog(); 

            LoadBooks();     
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
