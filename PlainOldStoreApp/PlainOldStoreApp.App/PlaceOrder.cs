using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace PlainOldStoreApp.App
{
    internal class PlaceOrder
    {
        internal static void PlaceOrderMenu()
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
                        Tuple<bool, string> emailTuple = Validate.ValidateEmail(email);
                        if (emailTuple.Item1 == false)
                        {
                            break;
                        }
                        string connectionString = File.ReadAllText(
                            "C:/Users/melin/OneDrive/Desktop/RevGit/MelindaW-P0/waggonerm-posa-db.txt");
                        ICustomerRepository customerRepository = new SqlCustomerRepository(connectionString);
                        Customer customerLookUp = new Customer(emailTuple.Item2, customerRepository);
                        bool foundEmail = customerLookUp.LookUpEmail();
                        if(foundEmail)
                        {
                            Console.WriteLine("The email you entered is already associated with an account.");
                        }
                        else
                        {
                            Console.WriteLine("Please enter first name and last name.");
                            string? name = Console.ReadLine();
                            Tuple<string, string> fullname = Validate.VaildateName(name);
                            if (string.IsNullOrWhiteSpace(fullname.Item1) || string.IsNullOrWhiteSpace(fullname.Item2))
                            {
                                break;
                            }
                            string firstName = fullname.Item1.ToUpper() ;
                            string lastName = fullname.Item2.ToUpper(); ;

                            Console.WriteLine("Please enter address 1.");
                            string? address1 = Console.ReadLine();
                            Console.WriteLine("Please enter city.");
                            string? city = Console.ReadLine();
                            Console.WriteLine("Please enter state.");
                            string? state = Console.ReadLine();
                            Console.WriteLine("Please enter zip code.");
                            string? zip = Console.ReadLine();
                            
                            string address = Validate.ValidateAddress(address1, city, state, zip);
                            
                            if (string.IsNullOrWhiteSpace(address)) { break; }

                            address1 = address.Split("\n")[0];
                            city = address.Split("\n")[1];
                            state = address.Split("\n")[2];
                            zip = address.Split("\n")[3] + "-" + address.Split("\n")[4];

                            Console.WriteLine("Please verify that the name, email, and address was entered correctly");
                            Console.WriteLine($"{firstName} {lastName}\n{address1}\n{city}\n{state}\n{zip}\n{emailTuple.Item2}");
                            Console.WriteLine("Correct Yes(Y) or No(N)");
                            string? input = Console.ReadLine()?.ToLower().Trim();
                            if(input == "n" || input == "no")
                            {
                                break;
                            }
                            Customer newCustomer = new Customer(
                                  firstName,
                                  lastName,
                                  address1,
                                  city,
                                  state,
                                  zip,
                                  customerLookUp.Email,
                                  customerRepository);
                            bool isAdd = newCustomer.AddCustomer();
                            if (isAdd == false)
                            { 
                                Console.WriteLine("Something went wrong.");
                                Console.WriteLine("The customer was not added.");
                                break;
                            }
                        }
                        IStoreRepository storeRepository = new SqlStoreRepository(connectionString);
                        Store store = new Store(storeRepository);
                        Dictionary<int, string> stores = store.GetStoresFromDatabase();
                        Console.WriteLine("Please choose a store location.");
                        foreach (var s in stores)
                        {
                            Console.WriteLine(s);
                        }
                        int numberOfStores = stores.Count;
                        int storeLocation = 0;
                        bool isInt = false;
                        while(!isInt)
                        {
                            isInt = int.TryParse(Console.ReadLine(), out storeLocation);
                            if(isInt == false || numberOfStores < 0 || storeLocation > numberOfStores)
                            {
                                Console.WriteLine("Invalid input.");
                                Console.WriteLine("Please choose a store location.");
                                isInt = false;
                            }
                        }
                        IProductRepository productRepository = new SqlProductRepository(connectionString);
                        Product products = new Product(storeLocation, productRepository);
                        List<Product> allStoreProducts = products.GetStoreInventory();

                        bool isOrderingProducts = true;
                        List<Product> amountOfProductsOrdered = new List<Product>();
                        while (isOrderingProducts)
                        {
                            Console.WriteLine($"Please select the item number for the item you would like to buy from our {store.GetStore(products.StoreID)}.");
                            Console.WriteLine($"Item Number\tProduct Name\tProduct Description\tProduct Price\tAmount Available");
                            foreach (Product product in allStoreProducts)
                            {
                                Console.WriteLine($"{product.ProductId}. {product.ProductName}\t\t{product.ProductDescription}\t\t${product.ProductPrice}\t{product.ProductQuantiy}");
                            }
                            int numberOfProducts = allStoreProducts.Count;
                            int itemSelection = 0;
                            isInt = false;
                            while (!isInt)
                            {
                                isInt = int.TryParse(Console.ReadLine(), out itemSelection);
                                if (isInt == false || itemSelection < 0 || itemSelection > numberOfProducts)
                                {
                                    Console.WriteLine("Invalid input.");
                                    Console.WriteLine("Please choose an item number.");
                                    isInt = false;
                                }
                            }
                            string? productName = "";
                            foreach (Product product in allStoreProducts)
                            {
                                if (product.ProductId == itemSelection)
                                {
                                    productName = product.ProductName;
                                }
                            }
                            int productAmount = 0;
                            Console.WriteLine($"How many {productName} would you like?");
                            isInt = false;
                            while (!isInt)
                            {
                                isInt = int.TryParse(Console.ReadLine(), out productAmount);
                                if (isInt == false || productAmount <= 0 || productAmount > 5)
                                {
                                    Console.WriteLine("Invalid input.");
                                    Console.WriteLine("Please enter a amount between 0 and 5.");
                                    isInt = false;
                                }
                            }
                            foreach (Product product in allStoreProducts)
                            {
                                if (product.ProductId == itemSelection)
                                {
                                    amountOfProductsOrdered.Add(new(itemSelection, productName, product.ProductDescription, product.ProductPrice, productAmount, product.StoreID));
                                }
                            }
                            Console.WriteLine("You have selected to order");
                            foreach (Product product in amountOfProductsOrdered)
                            {
                                Console.WriteLine($"{product.ProductId}.\t{product.ProductName}\t{product.ProductDescription}\t${product.ProductPrice}\t{product.ProductQuantiy}");
                            }
                            Console.WriteLine("Would you like to select another item?");
                            Console.WriteLine("Yes(Y) or NO(N");
                            string? yesOrNO = Console.ReadLine()?.ToLower();
                            if (yesOrNO == "n" || yesOrNO == "no")
                            {
                                isOrderingProducts = false;
                            }
                        }
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
