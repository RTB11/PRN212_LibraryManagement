using System;
using System.Collections.Generic;

namespace LibraryManagementProject.Model;

public partial class Author
{
    public int AuthorId { get; set; }

    public string AuthorName { get; set; } = null!;

    public string? Biography { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
