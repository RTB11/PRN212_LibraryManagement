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
    /// <summary>
    /// Interaction logic for LibrarianDashboard.xaml
    /// </summary>
    public partial class LibrarianFrame : Window
    {
        public LibrarianFrame()
        {
            InitializeComponent();
            MainFrame.Navigate(new LibrarianDashboard());
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new LibrarianDashboard());
        }

        private void Borrow_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BorrowBookPage());
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReturnBorrow());

        }

        private void BorrowHistory_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BorrowHistory());
        }
    }
}
