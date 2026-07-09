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
