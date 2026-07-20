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

    public partial class LibrarianDashboard : Page
    {

        private readonly LibraryContext _context = new();

        public LibrarianDashboard()
        {
            InitializeComponent();
            txtWelcome.Text = $"Welcome, {UserSession.CurrentUser.FullName}";
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            txtTotalBooks.Text = _context.Books.Count().ToString();
            txtTotalMembers.Text = _context.Members.Count().ToString();
            txtTotalBorrows.Text = _context.BorrowRecords.Count(x => x.Status == "Borrowing").ToString();
        }


    }
}
