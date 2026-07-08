using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementProject.Model;
using LibraryManagementProject.Repository;

namespace LibraryManagementProject.Service
{
    public class BookService
    {
        private readonly BookRepository _repository = new();

        public List<Book> GetAllBooks()
        {
            return _repository.GetAll();
        }
    }
}