using LibraryManagementProject.Model;
using System;
using System.Linq;
using System.Windows;

namespace LibraryManagementProject.Views
{
    public partial class CreateUserForm : Window
    {
        private readonly LibraryContext _context = new();

        public CreateUserForm()
        {
            InitializeComponent();
            LoadRoles();
        }

        private void LoadRoles()
        {
            cbRole.ItemsSource = _context.Roles.ToList();
            cbRole.DisplayMemberPath = "RoleName";
            cbRole.SelectedValuePath = "RoleId";
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!Validation())
                return;

            User user = new User
            {
                Username = txtUsername.Text.Trim(),
                PasswordHash = txtPassword.Password,
                FullName = txtFullName.Text.Trim(),
                RoleId = (int)cbRole.SelectedValue,
                Status = true,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            MessageBox.Show("Create user successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private bool Validation()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;
            string fullName = txtFullName.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter username", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsername.Focus();
                return false;
            }

            if (username.Length < 3)
            {
                MessageBox.Show("Username must be at least 3 characters long", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsername.Focus();
                return false;
            }

            if (username.Contains(" "))
            {
                MessageBox.Show("Username cannot contain spaces", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsername.Focus();
                return false;
            }

            bool usernameExists = _context.Users.Any(u => u.Username.ToLower() == username.ToLower());
            if (usernameExists)
            {
                MessageBox.Show("Username already exists! Please choose another username.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter password", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassword.Focus();
                return false;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassword.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Please enter full name", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFullName.Focus();
                return false;
            }

            if (cbRole.SelectedItem == null || cbRole.SelectedValue == null)
            {
                MessageBox.Show("Please select a role", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbRole.Focus();
                return false;
            }

            return true;
        }
    }
}