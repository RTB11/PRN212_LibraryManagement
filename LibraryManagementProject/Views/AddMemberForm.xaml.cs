using LibraryManagementProject.Model;
using System;
using System.Text.RegularExpressions;
using System.Windows;

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
        }






        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (!ValidateInput())
                return;



            Member member;



            if (_member == null)
            {
                member = new Member();

                member.JoinDate =
                    DateOnly.FromDateTime(DateTime.Today);

                member.Status = true;
            }
            else
            {
                member = _context.Members
                    .Find(_member.MemberId);



                if (member == null)
                {
                    MessageBox.Show("Member not found!");
                    return;
                }
            }





            member.FullName =
                txtFullName.Text.Trim();


            member.Email =
                txtEmail.Text.Trim();


            member.Phone =
                txtPhone.Text.Trim();


            member.Address =
                txtAddress.Text.Trim();






            if (_member == null)
            {
                _context.Members.Add(member);

                MessageBox.Show("Add member successfully!");
            }
            else
            {
                MessageBox.Show("Update member successfully!");
            }




            _context.SaveChanges();



            DialogResult = true;

            Close();

        }








        private bool ValidateInput()
        {

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Full Name is required!");

                txtFullName.Focus();

                return false;
            }




            string phone =
                txtPhone.Text.Trim();



            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Phone is required!");

                txtPhone.Focus();

                return false;
            }





            if (!Regex.IsMatch(phone, @"^\d{10,11}$"))
            {
                MessageBox.Show(
                    "Phone number must contain only digits and be 10-11 digits long!"
                );

                txtPhone.Focus();

                return false;
            }





            string email =
                txtEmail.Text.Trim();




            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Email is required!");

                txtEmail.Focus();

                return false;
            }





            if (!Regex.IsMatch(
                email,
                @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"))
            {

                MessageBox.Show("Invalid email format!");

                txtEmail.Focus();

                return false;
            }



            return true;
        }

    }
}