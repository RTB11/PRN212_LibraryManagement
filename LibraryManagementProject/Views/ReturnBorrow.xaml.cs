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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryManagementProject.Views
{

    public partial class ReturnBorrow : Page
    {
        private readonly LibraryContext _context = new();

        private BorrowRecord? selectedBorrow;

        public ReturnBorrow()
        {
            InitializeComponent();

            LoadBorrowRecords();

        }

        private void LoadBorrowRecords()
        {
            dgBorrowRecords.ItemsSource = _context.BorrowRecords
                .Include(x => x.Member)
                .Where(x => x.Status == "Borrowing")
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            dgBorrowRecords.ItemsSource = _context.BorrowRecords
                .Include(x => x.Member)
                .Where(x =>
                    x.Status == "Borrowing" &&
                    (
                        x.BorrowCode.ToLower().Contains(keyword) || x.Member.FullName.ToLower().Contains(keyword)
                    ))
                .ToList();
        }

        private void dgBorrowRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBorrow = dgBorrowRecords.SelectedItem as BorrowRecord;

            if (selectedBorrow == null)
                return;

            dgBorrowDetails.ItemsSource = _context.BorrowDetails
                .Include(x => x.Book)
                .Where(x => x.BorrowId == selectedBorrow.BorrowId)
                .ToList();
        }

        private void btnReturnAll_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBorrow == null)
            {
                MessageBox.Show("Please select a borrow record.");
                return;
            }

            if (MessageBox.Show( "Return all books?","Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                return;
            }

            var details = _context.BorrowDetails
                .Include(x => x.Book)
                .Where(x => x.BorrowId == selectedBorrow.BorrowId)
                .ToList();

            foreach (var detail in details)
            {
                if (detail.Status == "Returned")
                    continue;

                detail.Status = "Returned";
                detail.ReturnDate = DateOnly.FromDateTime(DateTime.Now);

                detail.Book.AvailableQuantity += detail.Quantity ?? 1;

                if (DateOnly.FromDateTime(DateTime.Now) > selectedBorrow.DueDate)
                {
                    int lateDays = DateOnly.FromDateTime(DateTime.Now).DayNumber
                                  - selectedBorrow.DueDate.DayNumber;

                    detail.FineAmount = lateDays * 5000;
                }
            }

            selectedBorrow.Status = "Returned";

            _context.SaveChanges();

            MessageBox.Show("Books returned successfully.");

            LoadBorrowRecords();

            dgBorrowDetails.ItemsSource = null;
        }

        private void btnReturnSelected_Click(object sender, RoutedEventArgs e)
        {
            var details = dgBorrowDetails.SelectedItems
                                .Cast<BorrowDetail>()
                                .ToList();

            if (details.Count == 0)
            {
                MessageBox.Show("Please select a book.");
                return;
            }


            foreach (var detail in details)
            {
                if (detail.Status == "Returned")
                    continue;

                detail.Status = "Returned";

                detail.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
                var book = _context.Books
                    .First(x => x.BookId == detail.BookId);

                book.AvailableQuantity += detail.Quantity ?? 1;

                if (DateOnly.FromDateTime(DateTime.Now) > selectedBorrow!.DueDate)
                {
                    int late =
                        DateOnly.FromDateTime(DateTime.Now).DayNumber
                        - selectedBorrow.DueDate.DayNumber;


                    detail.FineAmount = late * 5000;
                }
            }

            bool allReturned = _context.BorrowDetails
                .Where(x => x.BorrowId == selectedBorrow.BorrowId)
                .All(x => x.Status == "Returned");

            if (allReturned)
            {
                selectedBorrow.Status = "Returned";
            }

            _context.SaveChanges();
            dgBorrowRecords_SelectionChanged(null, null);
            LoadBorrowRecords();
            MessageBox.Show("Books returned.");
        }


    }
}
