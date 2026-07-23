using System;
using System.Collections.Generic;

namespace LibraryManagementProject.Model;

public partial class BorrowDetail
{
    public int BorrowDetailId { get; set; }

    public int BorrowId { get; set; }

    public int BookId { get; set; }

    public int Quantity { get; set; }

    public string? BorrowCondition { get; set; }

    public string? BorrowNote { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public string? ReturnCondition { get; set; }

    public string? ReturnNote { get; set; }

    public decimal? FineAmount { get; set; }

    public string? Status { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual BorrowRecord Borrow { get; set; } = null!;
}
