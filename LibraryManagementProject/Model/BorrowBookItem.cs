using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementProject.Model
{
    public class BorrowBookItem
    {
        public Book Book { get; set; } = null!;

        public string BorrowCondition { get; set; } = "Good";

        public string? BorrowNote { get; set; }


        public int BookId => Book.BookId;

        public string Title => Book.Title;

        public string Isbn => Book.Isbn;

        public string CategoryName =>
            Book.Category?.CategoryName ?? "";

        public string AuthorName =>
            Book.Author?.AuthorName ?? "";
    }
}
