using System;
using System.Collections.Generic;

namespace LibraryManagementProject.Model;

public partial class Member
{
    public int MemberId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateOnly? JoinDate { get; set; }

    public bool? Status { get; set; }

    public bool? Gender { get; set; }

    public virtual ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}
