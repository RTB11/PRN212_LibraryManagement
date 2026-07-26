using LibraryManagementProject.Model;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace LibraryManagementProject.Views
{
    public partial class AddMemberForm : Window
    {
        private readonly LibraryContext _context = new();
        private Member? _member;

        public AddMemberForm(Member? member = null)
        {
            InitializeComponent();

            _member = member;

            if (_member != null)
            {
                LoadMember();
                Title = "Update Member";
            }
            else
            {
                Title = "Add Member";
            }
        }

        private void LoadMember()
        {
            txtFullName.Text = _member!.FullName;
            txtEmail.Text = _member.Email;
            txtPhone.Text = _member.Phone;
            txtAddress.Text = _member.Address;

            if (_member.Gender == true)
                cbGender.SelectedIndex = 0;
            else if (_member.Gender == false)
                cbGender.SelectedIndex = 1;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            Member member;

            if (_member == null)
            {
                member = new Member
                {
                    JoinDate = DateOnly.FromDateTime(DateTime.Today),
                    Status = true
                };
            }
            else
            {
                member = _context.Members.Find(_member.MemberId);

                if (member == null)
                {
                    MessageBox.Show("Member not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            member.FullName = txtFullName.Text.Trim();
            member.Email = txtEmail.Text.Trim();
            member.Phone = txtPhone.Text.Trim();
            member.Address = txtAddress.Text.Trim();

            if (cbGender.SelectedItem is ComboBoxItem genderItem && genderItem.Tag != null)
            {
                member.Gender = bool.Parse(genderItem.Tag.ToString() ?? "true");
            }

            if (_member == null)
            {
                _context.Members.Add(member);
                MessageBox.Show("Add member successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Update member successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            _context.SaveChanges();
            DialogResult = true;
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Full Name is required!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFullName.Focus();
                return false;
            }

            string email = txtEmail.Text.Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Email is required!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return false;
            }

            if (!Regex.IsMatch(email, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
            {
                MessageBox.Show("Invalid email format!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return false;
            }

            bool emailExists = _context.Members.Any(m => m.Email != null && m.Email.ToLower() == email.ToLower() && (_member == null || m.MemberId != _member.MemberId));
            if (emailExists)
            {
                MessageBox.Show("Email already exists in the system!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return false;
            }

            string phone = txtPhone.Text.Trim();
            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Phone is required!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPhone.Focus();
                return false;
            }

            if (!Regex.IsMatch(phone, @"^\d{10,11}$"))
            {
                MessageBox.Show("Phone number must contain only digits and be 10-11 digits long!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPhone.Focus();
                return false;
            }

            bool phoneExists = _context.Members.Any(m => m.Phone == phone && (_member == null || m.MemberId != _member.MemberId));
            if (phoneExists)
            {
                MessageBox.Show("Phone number already exists in the system!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPhone.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Address is required!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtAddress.Focus();
                return false;
            }

            if (cbGender.SelectedItem == null)
            {
                MessageBox.Show("Please select gender!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbGender.Focus();
                return false;
            }

            if (dpDateOfBirth.SelectedDate != null && dpDateOfBirth.SelectedDate > DateTime.Today)
            {
                MessageBox.Show("Date of birth cannot be in the future!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                dpDateOfBirth.Focus();
                return false;
            }

            return true;
        }
    }
}