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


    public partial class MemberPage : Page
    {
        private readonly LibraryContext _context = new();

        private Member? _selectedMember;
        
        public MemberPage()
        {
            InitializeComponent();
            LoadMembers();
        }


        private void LoadMembers()
        {
            lvMembers.ItemsSource = _context.Members.ToList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddMemberForm window = new AddMemberForm();

            if (window.ShowDialog() == true)
            {
                LoadMembers();
            }
        }


        private void lvMembers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedMember = lvMembers.SelectedItem as Member;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedMember == null)
            {
                MessageBox.Show("Please select a member!");
                return;
            }

            AddMemberForm window = new AddMemberForm(_selectedMember);

            if (window.ShowDialog() == true)
            {
                LoadMembers();
            }
        }

    }
}
