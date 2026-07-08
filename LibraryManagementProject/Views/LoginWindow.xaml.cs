using LibraryManagementProject.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace LibraryManagementProject.Views
{
    public partial class LoginWindow : Window
    {
        private readonly LibraryContext _context = new();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!");
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!");
                txtPassword.Focus();
                return;
            }

            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u =>
                    u.Username == txtUsername.Text.Trim() &&
                    u.PasswordHash == txtPassword.Password &&
                    u.Status == true);

            if (user == null)
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                return;
            }

            if (user.Role.RoleName == "Admin")
            {
                AdminFrameDashboard dashboard = new AdminFrameDashboard();
                dashboard.Show();
                this.Close();
            }
            else if (user.Role.RoleName == "Librarian")
            {
                MessageBox.Show("Chức năng Librarian đang được phát triển.");
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập.");
            }
        }
    }
}