using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementProject.Model;

public partial class Member
{
    public string GenderText
    {
        get
        {
            return Gender == true ? "Male" : "Female";
        }
    }
}