using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Final_Project.Services
{
    public static class Validation
    {
        public static bool CheckName(string userentry)
        {

            Regex namePattern = new Regex(@"^[A-Z][[a-z]+$");

            if (userentry == null)
            {
                return false;
            }
            else if (namePattern.IsMatch(userentry))
            {
                return true;
            }
                return false;
        }

        public static bool CheckFavorite(string userentry)
        {

            Regex namePattern = new Regex(@"\D+");

            if (userentry == null)
            {
                return false;
            }
            else if (namePattern.IsMatch(userentry))
            {
                return true;
            }
                return false;
        }
    }
}
