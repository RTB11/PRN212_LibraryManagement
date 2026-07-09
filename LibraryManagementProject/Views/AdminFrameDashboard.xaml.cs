using System.Windows;
using System.Windows.Controls;
using LibraryManagementProject.Views;

namespace LibraryManagementProject.Views
{
    public partial class AdminFrameDashboard : Window
    {
        public AdminFrameDashboard()
        {
            InitializeComponent();
            MainFrame.Navigate(new AdminDashboard());
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AdminDashboard());
        }

        private void Books_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BookListPageManager());
        }

        private void Authors_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AuthorPage());
        }

        private void Categories_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CategoryPage());
        }

        private void Members_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MemberPage());
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserPage());
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to logout?","Logout",
                                                           MessageBoxButton.YesNo,
                                                           MessageBoxImage.Question
                                                       );

            if (result == MessageBoxResult.Yes)
            {
                this.Close();
                LoginWindow login = new LoginWindow();
                login.Show();
            }
        }

        private void PasswordChange_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword changePassword = new ChangePassword();
            changePassword.Show();
        }
    }
}