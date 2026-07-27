using LibraryManagementProject.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            FilterMembers();
        }

        private void FilterMembers()
        {
            if (_context == null || lvMembers == null)
                return;

            var query = _context.Members.AsQueryable();

            if (txtSearch != null && !string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                string keyword = txtSearch.Text.Trim();
                query = query.Where(m => (m.FullName != null && m.FullName.Contains(keyword)) ||
                                         (m.Email != null && m.Email.Contains(keyword)) ||
                                         (m.Phone != null && m.Phone.Contains(keyword)) ||
                                         (m.Address != null && m.Address.Contains(keyword)) ||
                                         m.MemberId.ToString().Contains(keyword));
            }

            lvMembers.ItemsSource = query.ToList();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterMembers();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            FilterMembers();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearch != null)
                txtSearch.Text = string.Empty;

            FilterMembers();
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
