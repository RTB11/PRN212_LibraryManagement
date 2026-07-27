using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementProject.Model;

public partial class Author
{
    [NotMapped]
    public int BookCount
    {
        get
        {
            return Books != null ? Books.Count : 0;
        }
    }
}
