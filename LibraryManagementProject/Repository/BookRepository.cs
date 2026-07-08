using LibraryManagementProject.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementProject.Repository
{
    public class BookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository()
        {
            _context = new LibraryContext();
        }

        public List<Book> GetAll()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToList();
        }
    }
}
