using LibraryManagementProject.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace LibraryManagementProject.Views
{
    public partial class ReturnBorrow : Page
    {

        private readonly LibraryContext _context = new();

        private BorrowRecord? selectedBorrow;

        private ObservableCollection<BorrowDetail> borrowDetails = new();

        public ReturnBorrow()
        {
            InitializeComponent();

            LoadBorrowRecords();
        }



        private void LoadBorrowRecords()
        {
            dgBorrowRecords.ItemsSource =
                _context.BorrowRecords
                .Include(x => x.Member)
                .Where(x => x.Status == "Borrowing")
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }




        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();


            dgBorrowRecords.ItemsSource =
                _context.BorrowRecords
                .Include(x => x.Member)
                .Where(x =>
                    x.Status == "Borrowing" &&
                    (
                        (x.BorrowCode ?? "").ToLower().Contains(keyword)
                        ||
                        (x.Member.FullName ?? "").ToLower().Contains(keyword)
                    )
                )
                .ToList();
        }

        private void dgBorrowRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBorrow = dgBorrowRecords.SelectedItem as BorrowRecord;

            if (selectedBorrow == null)
            {
                dgBorrowDetails.ItemsSource = null;
                return;
            }

            borrowDetails =
                new ObservableCollection<BorrowDetail>(
                    _context.BorrowDetails
                    .Include(x => x.Book)
                    .Where(x => x.BorrowId == selectedBorrow.BorrowId)
                    .ToList()
                );

            dgBorrowDetails.ItemsSource = borrowDetails;
        }




        private void btnReturnAll_Click(object sender, RoutedEventArgs e)
        {

            if (selectedBorrow == null)
            {
                MessageBox.Show("Please select a borrow record.");
                return;
            }

            if (MessageBox.Show(
                "Return all books?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question)
                != MessageBoxResult.Yes)
            {
                return;
            }

            var details =
                _context.BorrowDetails
                .Include(x => x.Book)
                .Where(x => x.BorrowId == selectedBorrow.BorrowId)
                .ToList();

            foreach (var detail in details)
            {
                if (detail.Status == "Returned")
                    continue;

                detail.Status = "Returned";
                detail.ReturnDate = DateOnly.FromDateTime(DateTime.Now);

                detail.Book.AvailableQuantity += detail.Quantity;

                CalculateFine(detail);
            }

            selectedBorrow.Status = "Returned";

            _context.SaveChanges();

            MessageBox.Show("Books returned successfully.");

            LoadBorrowRecords();

            dgBorrowDetails.ItemsSource = null;

            selectedBorrow = null;
        }

        private void btnReturnSelected_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBorrow == null)
            {
                MessageBox.Show("Please select borrow record.");
                return;
            }

            var selectedDetails = borrowDetails
                .Where(x => x.IsSelected)
                .ToList();

            if (!selectedDetails.Any())
            {
                MessageBox.Show("Please select a book.");
                return;
            }

            foreach (var detail in selectedDetails)
            {
                if (detail.Status == "Returned")
                    continue;

                detail.Status = "Returned";
                detail.ReturnDate = DateOnly.FromDateTime(DateTime.Now);

                detail.Book.AvailableQuantity += detail.Quantity;

                CalculateFine(detail);
            }


            if (borrowDetails.All(x => x.Status == "Returned"))
            {
                selectedBorrow.Status = "Returned";

            }

            _context.SaveChanges();
            MessageBox.Show("Books returned.");

            RefreshCurrentBorrow();
        }


        private void RefreshCurrentBorrow()
        {
            LoadBorrowRecords();

            if (selectedBorrow == null)
            {
                dgBorrowDetails.ItemsSource = null;
                return;
            }

            borrowDetails = new ObservableCollection<BorrowDetail>(
                _context.BorrowDetails
                    .Include(x => x.Book)
                    .Where(x => x.BorrowId == selectedBorrow.BorrowId)
                    .ToList());

            dgBorrowDetails.ItemsSource = borrowDetails;
        }


        private void CalculateFine(BorrowDetail detail)
        {
            if (selectedBorrow == null)
                return;

            decimal fine = 0;

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);


            switch (detail.ReturnCondition)
            {
                case "Lost":
                    decimal lostFee = 50000;
                    fine += detail.Book.Price + lostFee;
                    break;

                case "Damaged":
                    fine += 100000;
                    break;
            }


            if (today > selectedBorrow.DueDate)
            {
                int lateDays =
                    today.DayNumber -
                    selectedBorrow.DueDate.DayNumber;

                fine += lateDays * 5000;
            }
            detail.FineAmount = fine;
        }

    }
}