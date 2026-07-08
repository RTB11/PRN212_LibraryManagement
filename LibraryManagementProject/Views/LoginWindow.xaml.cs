using LibraryManagementProject.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace LibraryManagementProject.Views
{

    public partial class LoginWindow : Window
    {
        private readonly LoginService _loginService = new();
        public LoginWindow()
        {
            InitializeComponent();
        }



        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập.");
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu.");
                txtPassword.Focus();
                return;
            }

            var user = _loginService.Login(
                txtUsername.Text.Trim(),
                txtPassword.Password
             );

            if (user == null)
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.");
                return;
            }

            AdminDashboard dashboard = new();
            dashboard.Show();

            Close();
        }


    }
}