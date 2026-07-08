using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementProject.Model;

namespace LibraryManagementProject.Repository;
public class UserRepository
{
    private readonly LibraryContext _context = new();

    public User? Login(string username, string password)
    {
        return _context.Users
            .FirstOrDefault(x =>
                x.Username == username &&
                x.PasswordHash == password &&
                x.Status == true);
    }
}