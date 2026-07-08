using System;
using System.Collections.Generic;

namespace LibraryManagementProject.Model;

public partial class BorrowDetail
{
    public int BorrowDetailId { get; set; }

    public int BorrowId { get; set; }

    public int BookId { get; set; }

    public int? Quantity { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public decimal? FineAmount { get; set; }

    public string? Status { get; set; }

    public string? Note { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual BorrowRecord Borrow { get; set; } = null!;
}
