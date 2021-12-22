using PlainOldStoreApp.App;
using System.Text.RegularExpressions;

namespace PlainOldStoreApp
{
    class Program
    {
        static void Main(String[] args)
        {
            bool isRunning = true;
            string connectionString = File.ReadAllText(
                "C:/Users/melin/OneDrive/Desktop/RevGit/MelindaW-P0/waggonerm-posa-db.txt");
            Console.WriteLine("Welcome to Plain Old Store!");
            Console.WriteLine("What can I help you with today?");
            Console.WriteLine();

            while (isRunning)
            {
                Console.WriteLine("Please select one of the following menu options.");
                Console.WriteLine("1. Place Order\n2. Add Customer\n3. Lookup Order\n4. Exit");
                string? slection = Console.ReadLine()?.Trim().ToLower();
                Console.WriteLine();

                switch (slection)
                {
                    case "1":
                    case "place order":
                        PlaneOldShop.PlaceOrder(connectionString);
                        break;
                    case "2":
                    case "add customer":
                        PlaneOldShop.AddCustomer(connectionString);
                        break;
                    case "3":
                    case "lookup order":
                        PlaneOldShop.LookupOrder(connectionString);
                        break;
                    case "4":
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
    }
}
