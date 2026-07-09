using LibraryManagementProject.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibraryManagementProject.Views
{
    public partial class UserPage : Page
    {

        private readonly LibraryContext _context = new();

        private User selectedUser;


        public UserPage()
        {
            InitializeComponent();

            LoadUsers();
            LoadRoles();

            cbStatus.ItemsSource = new string[]
            {
                "Active",
                "Inactive"
            };
        }

        private void LoadUsers()
        {
            lvUsers.ItemsSource = _context.Users
                .Include(x => x.Role)
                .ToList();
        }



        private void LoadRoles()
        {
            cbRole.ItemsSource = _context.Roles.ToList();
            cbRole.DisplayMemberPath = "RoleName";
            cbRole.SelectedValuePath = "RoleId";
        }



        private void lvUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            selectedUser = lvUsers.SelectedItem as User;


            if (selectedUser != null)
            {

                txtUsername.Text = selectedUser.Username;
                txtFullName.Text = selectedUser.FullName;
                cbRole.SelectedValue = selectedUser.RoleId;

                cbStatus.SelectedItem =
                    selectedUser.Status == true
                    ? "Active"
                    : "Inactive";

            }

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

            if (selectedUser == null)
            {
                MessageBox.Show("Please select user");
                return;
            }



            selectedUser.RoleId =
                (int)cbRole.SelectedValue;



            selectedUser.Status =
                cbStatus.SelectedItem.ToString() == "Active";



            _context.SaveChanges();
            MessageBox.Show("Updated successfully");
            FilterUsers();
        }

        private void FilterUsers()
        {
            var query = _context.Users
                .Include(x => x.Role)
                .AsQueryable();

            if (cbFilterStatus.SelectedItem is ComboBoxItem item)
            {
                string status = item.Content.ToString();

                if (status == "Active")
                {
                    query = query.Where(x => x.Status.Value);
                }
                else if (status == "Inactive")
                {
                    query = query.Where(x => !x.Status.Value);
                }
            }

            lvUsers.ItemsSource = query.ToList();
        }

        private void cbFilterStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            FilterUsers();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            CreateUserForm form = new CreateUserForm();

            form.ShowDialog();

            FilterUsers();
        }

    }
}