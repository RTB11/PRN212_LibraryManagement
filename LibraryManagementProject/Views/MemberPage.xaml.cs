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

        public MemberPage()
        {
            InitializeComponent();
            LoadMembers();
        }


        private void LoadMembers()
        {
            lvMembers.ItemsSource = _context.Members.ToList();
        }

    }
}
