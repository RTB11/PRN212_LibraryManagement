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
using System.Windows.Shapes;

namespace LibraryManagementProject.Views
{
    /// <summary>
    /// Interaction logic for AddMemberForm.xaml
    /// </summary>
    public partial class AddMemberForm : Window
    {
        private readonly LibraryContext _context = new();

        private Member? _member;

        public AddMemberForm(Member? member = null)
        {
            InitializeComponent();

            cbGender.SelectedIndex = 0;

            _member = member;

            if (_member != null)
            {
                LoadMember();
                Title = "Update Member";
            }
        }


        private void LoadMember()
        {
            txtFullName.Text = _member.FullName;
            txtEmail.Text = _member.Email;
            txtPhone.Text = _member.Phone;
            txtAddress.Text = _member.Address;

            if (_member.DateOfBirth != null)
            {
                dpDateOfBirth.SelectedDate =
                    _member.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue);
            }

            cbGender.SelectedIndex = _member.Gender == true ? 0 : 1;
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Member member;

            if (_member == null)
            {
                member = new Member();
                member.JoinDate = DateOnly.FromDateTime(DateTime.Today);
                member.Status = true;
            }
            else
            {
                member = _context.Members.Find(_member.MemberId);

                if (member == null)
                {
                    MessageBox.Show("Member not found!");
                    return;
                }
            }

            member.FullName = txtFullName.Text;
            member.Email = txtEmail.Text;
            member.Phone = txtPhone.Text;
            member.Address = txtAddress.Text;

            if (dpDateOfBirth.SelectedDate != null)
            {
                member.DateOfBirth = DateOnly.FromDateTime(dpDateOfBirth.SelectedDate.Value);
            }

            member.Gender =
                ((ComboBoxItem)cbGender.SelectedItem).Tag.ToString() == "True";

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
    
    }
}
