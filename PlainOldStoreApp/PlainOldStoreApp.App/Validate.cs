using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PlainOldStoreApp.App
{
    internal class Validate
    {
        internal static Tuple<string, string> ValidateNameOrEmail()
        {
            string? tryAgain;
            bool isValidating = true;
            Console.WriteLine("Please enter in the customer's name or email.");
            string? nameOrEmail = Console.ReadLine()?.Trim();
            Console.WriteLine();
            while (isValidating)
            {
                if (string.IsNullOrWhiteSpace(nameOrEmail))
                {
                    Console.WriteLine("You did not enter in a full name or email.");
                    Console.WriteLine("Would you like to try again?");
                    Console.WriteLine("Yes(Y) or No(N)?");
                    tryAgain = Console.ReadLine()?.Trim().ToLower();
                    Console.WriteLine();
                    if (tryAgain == "no" || tryAgain == "n")
                    {
                        return new Tuple<string, string>("false", "");
                    }
                    Console.WriteLine("Please enter full name or email.");
                    nameOrEmail = Console.ReadLine()?.Trim();
                    Console.WriteLine();
                }
                string patternEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                Regex regexEmail = new Regex(patternEmail, RegexOptions.IgnoreCase);
                Match matchEmail = regexEmail.Match(nameOrEmail);

                string patternName = @"^[a-z]+[\w\s]+[a-z]+$";
                Regex regexName = new Regex(patternName, RegexOptions.IgnoreCase);
                Match matchName = regexName.Match(nameOrEmail);
                if (matchEmail.Success)
                {
                    return new Tuple<string, string>("email", nameOrEmail.ToUpper());
                }
                if (matchName.Success)
                {
                    string firstName = nameOrEmail.Split(' ')[0];
                    string lastName = nameOrEmail.Split(' ')[1];
                    return new Tuple<string, string>(firstName.ToUpper(), lastName.ToUpper());
                }
                Console.WriteLine("You did not enter in a name full or email.");
                Console.WriteLine("Would you like to try again?");
                Console.WriteLine("Yes(Y) or No(N)?");
                tryAgain = Console.ReadLine()?.Trim().ToLower();
                Console.WriteLine();
                if (tryAgain == "yes" || tryAgain == "y")
                {
                    return new Tuple<string, string>("false", "");
                }
                Console.WriteLine("Please enter your full name or email.");
                nameOrEmail = Console.ReadLine()?.Trim();
                Console.WriteLine();
            }
            return new Tuple<string, string>("false", "");
        }
        internal static Tuple<bool, string> ValidateEmail(string? email)
        {
            string? tryAgain;
            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("Your did not enter in an email.");
                Console.WriteLine("Would you like to try again?");
                Console.WriteLine("Yes(Y) or No(N)?");
                tryAgain = Console.ReadLine()?.Trim().ToLower();
                Console.WriteLine();
                if (tryAgain == "yes" || tryAgain == "y")
                {
                    Console.WriteLine("Please enter your email.");
                    email = Console.ReadLine()?.Trim();
                    Console.WriteLine();
                    return ValidateEmail(email);
                }
                return new Tuple<bool, string>(false, "");
            }
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(email);
            if (match.Success)
            {
                return new Tuple<bool, string>(true, email.ToUpper());
            }
            Console.WriteLine("Your did not enter in an email.");
            Console.WriteLine("Would you like to try again?");
            Console.WriteLine("Yes(Y) or No(N)?");
            tryAgain = Console.ReadLine()?.Trim().ToLower();
            Console.WriteLine();
            if (tryAgain == "yes" || tryAgain == "y")
            {
                Console.WriteLine("Please enter your email.");
                email = Console.ReadLine()?.Trim();
                Console.WriteLine();
                return ValidateEmail(email);
            }
            return new Tuple<bool, string>(false, email);
        }

        internal static Tuple<string, string> VaildateName(string? name)
        {
            string? tryAgain;
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("You did not enter you fist and last name.");
                Console.WriteLine("Would you like to try again?");
                Console.WriteLine("Yes(Y) or No(N)?");
                tryAgain = Console.ReadLine()?.Trim();
                Console.WriteLine();
                if (tryAgain == "yes" || tryAgain == "y")
                {
                    Console.WriteLine("Please enter your first name and last name.");
                    name = Console.ReadLine()?.Trim();
                    Console.WriteLine();
                    return VaildateName(name);
                }
                else
                {
                    return new Tuple<string, string>("", "");
                }
            }
            string pattern = @"^[a-z]+[\w\s]+[a-z]+$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(name);
            if(match.Success)
            {
                string firstName = name.Split(' ')[0];
                string lastName = name.Split(' ')[1];
                return new Tuple<string, string>(firstName.ToUpper(), lastName.ToUpper());
            }
            Console.WriteLine("You did not enter you first and last name.");
            Console.WriteLine("Would you like to try again?");
            Console.WriteLine("Yes(Y) or No(N)?");
            tryAgain = Console.ReadLine()?.Trim();
            Console.WriteLine();
            if (tryAgain == "yes" || tryAgain == "y")
            {
                Console.WriteLine("Please enter your first name and last name.");
                name = Console.ReadLine()?.Trim();
                Console.WriteLine();
                return VaildateName(name);
            }
            return new Tuple<string, string>("", "");
        }

        internal static string ValidateAddress(string? address1, string? city, string? state, string? zip)
        {
            string userId = File.ReadAllText("C:/Users/melin/OneDrive/Desktop/RevGit/MelindaW-P0/usps-web-tools-api.txt");
            bool isValidating = true;
            string address = "";
            while (isValidating)
            {
                address = "";
                XDocument addressDoc = new XDocument(
                new XElement("AddressValidateRequest",
                    new XAttribute("USERID", userId),
                        new XElement("Address",
                            new XElement("Address1", address1),
                            new XElement("Address2", ""),
                            new XElement("City", city),
                            new XElement("State", state),
                            new XElement("Zip5", zip),
                            new XElement("Zip4", "")
                        )
                    )
                );
                string url = @"https://secure.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + addressDoc;
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Async = true;
                using (XmlReader reader = XmlReader.Create(url, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Text)
                            address += reader.ReadContentAsString() + "\n";
                    }
                }
                if (address.Contains("Invalid") || address.Contains("Not Found") || address.Contains("clsAMS"))
                {
                    Console.WriteLine("Not a valid address");
                    Console.WriteLine("Would you like to try again?");
                    Console.WriteLine("Yes(Y) or No(N)?");
                    string? tryAgain = Console.ReadLine()?.Trim();
                    Console.WriteLine();
                    if (tryAgain == "yes" || tryAgain == "y")
                    {
                        Console.WriteLine("Please enter address 1.");
                        address1 = Console.ReadLine();
                        Console.WriteLine();
                        Console.WriteLine("Please enter your city.");
                        city = Console.ReadLine();
                        Console.WriteLine();
                        Console.WriteLine("Please enter your state.");
                        state = Console.ReadLine();
                        Console.WriteLine();
                        Console.WriteLine("Please enter your zip code.");
                        zip = Console.ReadLine();
                        Console.WriteLine();
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    isValidating = false;
                }
            }
            return address;
        }
    }
}
