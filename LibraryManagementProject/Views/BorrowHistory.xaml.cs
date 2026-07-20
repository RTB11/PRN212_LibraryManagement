using LibraryManagementProject.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibraryManagementProject.Views
{
    public partial class BorrowHistory : Page
    {
        private readonly LibraryContext _context = new();

        private BorrowRecord? selectedBorrow;


        public BorrowHistory()
        {
            InitializeComponent();

            LoadBorrowHistory();

            dgBorrowHistory.SelectionChanged += dgBorrowHistory_SelectionChanged;

            btnSearch.Click += btnSearch_Click;

            btnRefresh.Click += btnRefresh_Click;
        }


        private void LoadBorrowHistory()
        {
            dgBorrowHistory.ItemsSource = _context.BorrowRecords
                .Include(x => x.Member)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }



        private void dgBorrowHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBorrow = dgBorrowHistory.SelectedItem as BorrowRecord;


            if (selectedBorrow == null)
            {
                dgBorrowDetails.ItemsSource = null;
                return;
            }


            dgBorrowDetails.ItemsSource = _context.BorrowDetails
                .Include(x => x.Book)
                .Where(x => x.BorrowId == selectedBorrow.BorrowId)
                .ToList();
        }



        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();


            var query = _context.BorrowRecords
                .Include(x => x.Member)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.BorrowCode.Contains(keyword) || x.Member.FullName.Contains(keyword));
            }

            if (cbStatus.SelectedItem is ComboBoxItem item)
            {
                string status = item.Content.ToString();


                if (status != "All")
                {
                    query = query.Where(x => x.Status == status);
                }
            }

            dgBorrowHistory.ItemsSource = query.OrderByDescending(x => x.CreatedAt)
                     .ToList();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";

            cbStatus.SelectedIndex = 0;

            LoadBorrowHistory();

            dgBorrowDetails.ItemsSource = null;
        }

    }
}