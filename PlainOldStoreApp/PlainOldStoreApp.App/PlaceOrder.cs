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
                        string connectionString = File.ReadAllText(
                                "C:/Users/melin/OneDrive/Desktop/RevGit/MelindaW-P0/waggonerm-posa-db.txt");
                        string firstName;
                        string lastName;
                        string email = "";
                        Console.WriteLine("Please enter in the customer's name or email.");
                        string? nameOrEmail = Console.ReadLine()?.Trim();
                        Tuple<string, string> nameOrEmailTuple = Validate.ValidateNameOrEmail(nameOrEmail);
                        if (nameOrEmailTuple.Item1 == "false")
                        {
                            break;
                        }
                        ICustomerRepository customerRepository = new SqlCustomerRepository(connectionString);
                        if (nameOrEmailTuple.Item1 == "email")
                        {
                            Customer customerLookUpEmail = new Customer(nameOrEmailTuple.Item2, customerRepository);
                            bool foundEmail = customerLookUpEmail.LookUpEmail();
                            if (foundEmail)
                            {
                                Console.WriteLine("The email you entered is already associated with an account.");
                                Console.WriteLine("Please use that account to order");
                                break;
                            }
                            else
                            {
                                email = nameOrEmailTuple.Item2.ToUpper();
                                Console.WriteLine("Please enter first name and last name.");
                                string? name = Console.ReadLine();
                                Tuple<string, string> fullname = Validate.VaildateName(name);
                                if (string.IsNullOrWhiteSpace(fullname.Item1) || string.IsNullOrWhiteSpace(fullname.Item2))
                                {
                                    break;
                                }
                                firstName = fullname.Item1.ToUpper();
                                lastName = fullname.Item2.ToUpper();
                            }
                        }
                        else
                        {
                            Customer customerLookUpName = new Customer(nameOrEmailTuple.Item1, nameOrEmailTuple.Item2, customerRepository);
                            List<Customer> foundCoustomers = customerLookUpName.LookUpName();
                            string? emailLookUP;
                            Tuple<bool, string> emailTuple;
                            if (foundCoustomers.Count >= 1)
                            {
                                Console.WriteLine("The name you entered is already associated with an account.");
                                Console.WriteLine("Please enter email to see if account exists.");
                                emailLookUP = Console.ReadLine();
                                emailTuple = Validate.ValidateEmail(emailLookUP);
                                if(!emailTuple.Item1)
                                {
                                    break;
                                }
                                foreach (Customer customer in foundCoustomers)
                                {
                                    if (customer.Email == emailTuple.Item2)
                                    {
                                        Console.WriteLine("Please use account associated with email to order.");
                                        email = "anEmailWasFound";
                                        break;
                                    }
                                }
                                if (email == "anEmailWasFound")
                                {
                                    break;
                                }
                                email = emailTuple.Item2;
                            }
                            firstName = nameOrEmailTuple.Item1;
                            lastName = nameOrEmailTuple.Item2;
                            if(email == "")
                            {
                                Console.WriteLine("Please enter an email.");
                                emailLookUP = Console.ReadLine();
                                emailTuple = Validate.ValidateEmail(emailLookUP);
                                email = emailTuple.Item2;
                            }                            
                        }
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
                        Console.WriteLine($"{firstName} {lastName}\n{address1}\n{city}\n{state}\n{zip}\n{email}");
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
                                email,
                                customerRepository);

                        Guid customerId = newCustomer.AddCustomer();

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
                            Console.WriteLine($"Please select the item number for the item you would like to buy from our {store.GetStore(products.StoreID)} location.");
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
                                if (isInt == false || itemSelection < 1 || itemSelection > numberOfProducts)
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
                                if (isInt == false || productAmount < 1 || productAmount > 5)
                                {
                                    Console.WriteLine("Invalid input.");
                                    Console.WriteLine("Please enter a amount between 1 and 5.");
                                    isInt = false;
                                }
                            }
                            int? sum = 0;
                            if (amountOfProductsOrdered.Count > 0)
                            {
                                foreach (Product product in amountOfProductsOrdered)
                                {
                                    if(product.ProductId == itemSelection)
                                    {
                                        sum += product.ProductQuantiy;
                                    }
                                }
                            }
                            if ((sum + productAmount) > 5)
                            {
                                Console.WriteLine("You can not order more than 5 of one item.");
                            }
                            else
                            {
                                foreach (Product product in allStoreProducts)
                                {
                                    if (product.ProductId == itemSelection)
                                    {
                                        if (product.ProductQuantiy == 0)
                                        {
                                            Console.WriteLine("Out of stock.");
                                            break;
                                        }
                                        while (product.ProductQuantiy < productAmount)
                                        {
                                            Console.WriteLine("Not enough inventory to fulfill order.");
                                            Console.WriteLine("Please reduce order amount.");
                                            isInt = int.TryParse(Console.ReadLine(), out productAmount);
                                            if (isInt == false || productAmount < 1 || productAmount > 5)
                                            {
                                                Console.WriteLine("Invalid input.");
                                                Console.WriteLine("Please enter a amount between 1 and 5.");
                                                isInt = false;
                                            }
                                        }
                                        amountOfProductsOrdered.Add(new(itemSelection, productName, product.ProductDescription, product.ProductPrice, productAmount, product.StoreID));
                                        product.ProductQuantiy -= productAmount;
                                    }
                                }
                            }
                            if (amountOfProductsOrdered.Count > 0)
                            {
                                Console.WriteLine("You have selected to order:");
                                foreach (Product product in amountOfProductsOrdered)
                                {
                                    Console.WriteLine($"{product.ProductId}.\t{product.ProductName}\t{product.ProductDescription}\t${product.ProductPrice}\t{product.ProductQuantiy}");
                                }
                            }
                            Console.WriteLine("Would you like to select another item?");
                            Console.WriteLine("Yes(Y) or NO(N)");
                            string? yesOrNO = Console.ReadLine()?.ToLower();
                            if (yesOrNO == "n" || yesOrNO == "no")
                            {
                                isOrderingProducts = false;
                            }
                        }
                        IOrderRepository orderRepository = new SqlOrderRepository(connectionString);
                        Order orders = new Order(orderRepository);
                        List<Order> ordersMade = new List<Order>();
                        foreach (Product product in amountOfProductsOrdered)
                        {
                            ordersMade.Add(new(product.ProductId, product.ProductPrice, product.ProductQuantiy));
                        }
                        List<Order> getOrders = orders.PlaceCustomerOreder(customerId, storeLocation, ordersMade);
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
