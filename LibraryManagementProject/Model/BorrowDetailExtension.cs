using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementProject.Model;

public partial class BorrowDetail
{
    [NotMapped]
    public bool IsSelected { get; set; }
}
