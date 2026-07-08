using System;
using System.Collections.Generic;

namespace LibraryManagementProject.Model;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public int RoleId { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

    public virtual Role Role { get; set; } = null!;

    public string StatusText
    {
        get
        {
            return Status == true ? "Active" : "Inactive";
        }
    }
}
