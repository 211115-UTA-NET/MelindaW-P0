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
                        PlaceOrder.PlaceOrderMenu();
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
    }
}
