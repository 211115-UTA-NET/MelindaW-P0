using PlainOldStoreApp.App;
using System.Text.RegularExpressions;

namespace PlainOldStoreApp
{
    class Program
    {
        static void Main(String[] args)
        {
            bool isRunning = true;

            Console.WriteLine("Welcome to Plain Old Store!");
            Console.WriteLine("What can I help you with today?");

            while (isRunning)
            {
                Console.WriteLine("Please select one of the following menu options.");
                Console.WriteLine("1. Place Order\n2. Lookup Order\n3. Exit");
                string? slection = Console.ReadLine()?.Trim().ToLower();
                Console.WriteLine();

                switch (slection)
                {
                    case "1":
                    case "place order":
                        PlaceOrder();
                        break;
                    case "2":
                    case "lookup order":
                        break;
                    case "3":
                    case "exit":
                        Console.WriteLine("Have a nice day. Goodbye!");
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please select a valid menu option.");
                        break;
                }
            }

        }
        private static void PlaceOrder()
        {
            bool isOrdering = true;
            while (isOrdering)
            {
                Console.WriteLine("Would you like to place an order for a new or existing customer?");
                Console.WriteLine("1. New Customer\n2. Existing Customer\n3. Done");
                string? selection = Console.ReadLine()?.Trim().ToLower();
                Console.WriteLine();

                switch (selection)
                {
                    case "1":
                    case "new customer":
                        Console.WriteLine("Please enter in the customer's email.");
                        string? email = Console.ReadLine()?.Trim();
                        if(string.IsNullOrWhiteSpace(email))
                        {
                            break;
                        }
                        Tuple<bool, string> emailTuple = Validate.ValidateEmail(email);
                        if (emailTuple.Item1 == false)
                        {
                            break;
                        }
                        Customer customer = new Customer();
                        customer.Email = emailTuple.Item2;
                        Console.WriteLine(customer.Email);
                        //Check if email already exists
                        //If email can not be found add new customer
                        break;
                    case "2":
                    case "existing customer":
                        break;
                    case "3":
                    case "done":
                        Console.WriteLine("Thanks for placing an order.");
                        isOrdering = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please select a valid menu option.");
                        break;
                }
            }
        }
    }
}
