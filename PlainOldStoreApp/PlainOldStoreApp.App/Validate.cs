using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal class Validate
    {
        internal static Tuple<bool, string> ValidateEmail(string? email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(email);
            if (!match.Success)
            {
                Console.WriteLine("The email you entered is invalid would you like to try again?");
                Console.WriteLine("Yes(Y) or No(N)");
                string? selection = Console.ReadLine()?.ToLower().Trim();
                if (selection == "yes" || selection == "y")
                {
                    Console.WriteLine("Please enter a valid email.");
                    email = Console.ReadLine()?.Trim();
                    return ValidateEmail(email);
                }
                return new Tuple<bool, string>(false, email);
            }
            else
            {
                return new Tuple<bool, string>(true, email);
            }
        }
    }
}
