using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementProject.Model;
public partial class Book
{
    public string StatusText
    {
        get
        {
            return Status == true ? "Active" : "Inactive";
        }
    }

    [NotMapped]
    public string BorrowCondition { get; set; } = "Good";
}
