using System;
using System.Collections.Generic;

namespace LibraryManagementProject.Model;

public partial class BorrowRecord
{
    public int BorrowId { get; set; }

    public string BorrowCode { get; set; } = null!;

    public int MemberId { get; set; }

    public DateOnly? BorrowDate { get; set; }

    public DateOnly DueDate { get; set; }

    public int? TotalBooks { get; set; }

    public string? Status { get; set; }

    public int UserId { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<BorrowDetail> BorrowDetails { get; set; } = new List<BorrowDetail>();

    public virtual Member Member { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
