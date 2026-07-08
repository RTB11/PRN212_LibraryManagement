using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementProject.Model;
using LibraryManagementProject.Repository;

namespace LibraryManagementProject.Service;

public class LoginService
{
    private readonly UserRepository _repository = new();

    public User? Login(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            return null;

        if (string.IsNullOrWhiteSpace(password))
            return null;

        return _repository.Login(username, password);
    }
}
