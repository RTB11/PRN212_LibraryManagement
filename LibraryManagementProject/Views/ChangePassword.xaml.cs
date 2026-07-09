using LibraryManagementProject.Model;
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
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        private readonly LibraryContext _context = new();

        public ChangePassword()
        {
            InitializeComponent();
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = txtOldPassword.Password.Trim();
            string newPassword = txtNewPassword.Password.Trim();
            string confirmPassword = txtConfirmPassword.Password.Trim();

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            var user = _context.Users.FirstOrDefault(x => x.UserId == UserSession.CurrentUser.UserId);

            if (user == null)
            {
                MessageBox.Show("User not found.");
                return;
            }

            if (user.PasswordHash != oldPassword)
            {
                MessageBox.Show("Old password is incorrect.");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New passwords do not match.");
                return;
            }

            if (newPassword == oldPassword)
            {
                MessageBox.Show("New password must be different from the old password.");
                return;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show("Password must contain at least 6 characters.");
                return;
            }

            user.PasswordHash = newPassword;

            _context.SaveChanges();

            MessageBox.Show("Password changed successfully.");

            Close();
        }
    }
}
