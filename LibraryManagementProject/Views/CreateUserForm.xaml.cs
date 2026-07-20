using LibraryManagementProject.Model;
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

            User user = new User();


            user.Username = txtUsername.Text;

            user.PasswordHash = txtPassword.Password;

            user.FullName = txtFullName.Text;


            user.RoleId = (int)cbRole.SelectedValue;


            user.Status = true;


            _context.Users.Add(user);

            _context.SaveChanges();



            MessageBox.Show("Create user successfully");


            Close();

        }

        private bool Validation()
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Please enter username");
                return false;
            }
            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("Please enter password");
                return false;
            }
            if (string.IsNullOrEmpty(txtFullName.Text))
            {
                MessageBox.Show("Please enter full name");
                return false;
            }
            if (cbRole.SelectedItem == null)
            {
                MessageBox.Show("Please select role");
                return false;
            }
            return true;
        }

    }
}