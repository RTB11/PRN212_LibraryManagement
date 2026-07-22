using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementProject.Model;

public partial class BorrowDetail
{
    public int BorrowDetailId { get; set; }

    public int BorrowId { get; set; }

    public int BookId { get; set; }

    public int Quantity { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public decimal? FineAmount { get; set; }

    public string? Status { get; set; }


    // Tình trạng lúc mượn
    public string? BorrowCondition { get; set; }


    // Ghi chú lúc mượn
    public string? BorrowNote { get; set; }


    // Tình trạng lúc trả
    public string? ReturnCondition { get; set; }


    // Ghi chú lúc trả
    public string? ReturnNote { get; set; }



    public virtual Book Book { get; set; } = null!;


    public virtual BorrowRecord Borrow { get; set; } = null!;



    // Chỉ dùng cho DataGrid chọn sách trả
    [NotMapped]
    public bool IsSelected { get; set; }
}