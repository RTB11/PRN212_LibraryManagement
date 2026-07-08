using LibraryManagementProject.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibraryManagementProject.Views
{
    public partial class CategoryPage : Page
    {

        private readonly LibraryContext _context = new();

        private Category selectedCategory;


        public CategoryPage()
        {
            InitializeComponent();

            LoadCategories();
        }

        private void LoadCategories()
        {
            lvCategories.ItemsSource =
                _context.Categories.ToList();
        }

        private void lvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCategory =
                lvCategories.SelectedItem as Category;


            if (selectedCategory != null)
            {
                txtCategoryName.Text =
                    selectedCategory.CategoryName;


                txtDescription.Text =
                    selectedCategory.Description;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Category name is required");
                return;
            }
            Category category = new Category();

            category.CategoryName = txtCategoryName.Text;
            category.Description = txtDescription.Text;


            _context.Categories.Add(category);
            _context.SaveChanges();
            MessageBox.Show("Add successfully");

            LoadCategories();

            txtCategoryName.Clear();
            txtDescription.Clear();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Category name is required");
                return;
            }



            if (selectedCategory == null)
            {
                Category category = new Category();

                category.CategoryName =
                    txtCategoryName.Text;


                category.Description =
                    txtDescription.Text;


                _context.Categories.Add(category);

            }
            else
            {
                selectedCategory.CategoryName =
                    txtCategoryName.Text;


                selectedCategory.Description =
                    txtDescription.Text;
            }

            _context.SaveChanges();


            MessageBox.Show("Saved successfully");


            LoadCategories();

            btnAdd_Click(null, null);

        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (selectedCategory == null)
            {
                MessageBox.Show("Please select category");
                return;
            }

            _context.Categories.Remove(selectedCategory);


            _context.SaveChanges();
            MessageBox.Show("Deleted successfully");


            LoadCategories();

            btnAdd_Click(null, null);

        }

    }
}