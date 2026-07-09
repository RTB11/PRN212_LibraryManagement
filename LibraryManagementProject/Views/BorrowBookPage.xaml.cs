using LibraryManagementProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementProject.Views
{
    /// <summary>
    /// Interaction logic for BorrowBookPage.xaml
    /// </summary>
    public partial class BorrowBookPage : Page
    {

        private readonly LibraryContext _context = new();

        private Member? selectedMember;

        private ObservableCollection<Book> borrowBooks = new();

        public BorrowBookPage()
        {
            InitializeComponent();

            dgBorrow.ItemsSource = borrowBooks;

            LoadBooks();
            LoadMembers();
        }

        private void LoadBooks()
        {
            dgBooks.ItemsSource = _context.Books
                                    .Include(b => b.Category)
                                    .Include(b => b.Author)
                                    .ToList();
        }

        private void LoadMembers()
        {
            dgMembers.ItemsSource = _context.Members
                .Where(m => m.Status == true)
                .ToList();
        }

        private void btnSearchMember_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtMemberKeyword.Text.Trim();

            dgMembers.ItemsSource = _context.Members
                .Where(m =>
                    (m.FullName ?? "").Contains(keyword) ||
                    (m.Phone ?? "").Contains(keyword) ||
                    m.MemberId.ToString().Contains(keyword))
                .ToList();
        }




        private void dgMembers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedMember = dgMembers.SelectedItem as Member;

            if (selectedMember != null)
            {
                txtSelectedMember.Text = $"{selectedMember.FullName} (ID: {selectedMember.MemberId})";
            }
            else
            {
                txtSelectedMember.Text = "";
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (dgBooks.SelectedItem is not Book book)
            {
                MessageBox.Show("Please select a book.");
                return;
            }

            if (book.AvailableQuantity <= 0)
            {
                MessageBox.Show("This book is currently unavailable.");
                return;
            }

            if (borrowBooks.Any(b => b.BookId == book.BookId))
            {
                MessageBox.Show("This book has already been added.");
                return;
            }

            borrowBooks.Add(book);
        }


        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (dgBorrow.SelectedItem is Book book)
            {
                borrowBooks.Remove(book);
            }
        }

        private void btnSearchBook_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtBookKeyword.Text.Trim();

            dgBooks.ItemsSource = _context.Books
                .Where(b =>
                    (b.Title ?? "").Contains(keyword) ||
                    (b.Isbn ?? "").Contains(keyword))
                .ToList();
        }

        private void btnBorrow_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMember == null)
            {
                MessageBox.Show("Please select a member.");
                return;
            }

            if (borrowBooks.Count == 0)
            {
                MessageBox.Show("Please select at least one book.");
                return;
            }

            BorrowRecord record = new BorrowRecord
            {
                BorrowCode = GenerateBorrowCode(),
                MemberId = selectedMember.MemberId,
                BorrowDate = DateOnly.FromDateTime(DateTime.Now),
                DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(14)),
                TotalBooks = borrowBooks.Count,
                Status = "Borrowing",
                UserId = UserSession.CurrentUser.UserId,
                CreatedAt = DateTime.Now
            };
            _context.BorrowRecords.Add(record);
            _context.SaveChanges();

            foreach (Book book in borrowBooks)
            {
                BorrowDetail detail = new BorrowDetail
                {
                    BorrowId = record.BorrowId,
                    BookId = book.BookId,
                    Quantity = 1,
                    Status = "Borrowing"
                };

                _context.BorrowDetails.Add(detail);
                book.AvailableQuantity--;
                _context.Books.Update(book);
            }


            _context.SaveChanges();

            MessageBox.Show("Borrow successfully.");
            borrowBooks.Clear();
            selectedMember = null;
            dgMembers.SelectedItem = null;
            txtMemberKeyword.Clear();
            txtBookKeyword.Clear();

            LoadBooks();
            LoadMembers();
        }

        private string GenerateBorrowCode()
        {
            int lastId = _context.BorrowRecords
                         .OrderByDescending(b => b.BorrowId)
                         .Select(b => b.BorrowId)
                         .FirstOrDefault();

            return $"BR{lastId + 1:D4}";
        }
    }
}
