using System;
using System.Collections.Generic;

namespace LibraryManagementProject.Model;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string? Isbn { get; set; }

    public int AuthorId { get; set; }

    public int CategoryId { get; set; }

    public string? Publisher { get; set; }

    public int? PublishYear { get; set; }

    public int Quantity { get; set; }

    public int AvailableQuantity { get; set; }

    public string? Shelf { get; set; }

    public string? ImageUrl { get; set; }

    public bool? Status { get; set; }

    public decimal Price { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<BorrowDetail> BorrowDetails { get; set; } = new List<BorrowDetail>();

    public virtual Category Category { get; set; } = null!;
}
