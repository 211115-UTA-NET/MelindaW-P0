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
    internal class PlaneOldShop
    {
        internal static void PlaceOrder(string connectionString)
        {
            bool isOrdering = true;
            while(isOrdering)
            {
                string email = "";
                Tuple<string, string> nameOrEmailTuple = Validate.ValidateNameOrEmail();
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
                        email = nameOrEmailTuple.Item2.ToUpper();
                        Console.WriteLine("You have an account.");
                        Console.WriteLine("Please continue with your order.");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("No account has that email please lookup by name or create an account.");
                        Console.WriteLine();
                        break;
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
                        Console.WriteLine("We have found an account under that name.");
                        Console.WriteLine("Please enter email to verify your account.");
                        emailLookUP = Console.ReadLine();
                        Console.WriteLine();
                        emailTuple = Validate.ValidateEmail(emailLookUP);
                        if (!emailTuple.Item1)
                        {
                            break;
                        }
                        foreach (Customer customer in foundCoustomers)
                        {
                            if (customer.Email == emailTuple.Item2)
                            {
                                Console.WriteLine("You have an account please continue with your order.");
                                Console.WriteLine();
                                email = "anEmailWasFound";
                            }
                        }
                        if (email != "anEmailWasFound")
                        {
                            Console.WriteLine("No account was found with that email.");
                            break;
                        }
                        email = emailTuple.Item2;
                    }
                    else
                    {
                        Console.WriteLine("No account was found with this name");
                        Console.WriteLine();
                        break;
                    }
                }
                Customer getCustomerID = new Customer(email, customerRepository);
                Guid customerId = getCustomerID.GetCustomerID();
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
                while (!isInt)
                {
                    isInt = int.TryParse(Console.ReadLine(), out storeLocation);
                    if (isInt == false || numberOfStores < 0 || storeLocation > numberOfStores)
                    {
                        Console.WriteLine("Invalid input.");
                        Console.WriteLine("Please choose a store location.");
                        Console.WriteLine();
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
                    Console.WriteLine();
                    Console.WriteLine($"Please select the item number for the item you would like to buy from our {store.GetStore(products.StoreID)} location.");
                    Console.WriteLine();
                    Console.WriteLine(string.Format("   | {0,3} {1,28} {2,-30} | {3,-13} | {4,8}", "Product Name", "|", "Product Description", "Product Price", "Quantity"));

                    foreach (Product product in allStoreProducts)
                    {
                        Console.WriteLine(string.Format("{0,-2} | {1,-39} | {2,-30} | {3:C2} {4,8} {5,8}"
                            , product.ProductId, product.ProductName, product.ProductDescription, product.ProductPrice, "|", product.ProductQuantiy));
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
                            Console.WriteLine();
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
                            Console.WriteLine();
                            isInt = false;
                        }
                    }
                    int? sum = 0;
                    if (amountOfProductsOrdered.Count > 0)
                    {
                        foreach (Product product in amountOfProductsOrdered)
                        {
                            if (product.ProductId == itemSelection)
                            {
                                sum += product.ProductQuantiy;
                            }
                        }
                    }
                    if ((sum + productAmount) > 5)
                    {
                        Console.WriteLine("You can not order more than 5 of one item.");
                        Console.WriteLine();
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
                                    Console.WriteLine();
                                    break;
                                }
                                while (product.ProductQuantiy < productAmount)
                                {
                                    Console.WriteLine("Not enough inventory to fulfill order.");
                                    Console.WriteLine("Please reduce order amount.");
                                    Console.WriteLine();
                                    isInt = int.TryParse(Console.ReadLine(), out productAmount);
                                    if (isInt == false || productAmount < 1 || productAmount > 5)
                                    {
                                        Console.WriteLine("Invalid input.");
                                        Console.WriteLine("Please enter a amount between 1 and 5.");
                                        Console.WriteLine();
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
                        Console.WriteLine();
                        Console.WriteLine("You have selected to order:");
                        foreach (Product product in amountOfProductsOrdered)
                        {
                            Console.WriteLine(string.Format("{0,-2} | {1,-39} | {2,-30} | {3:C2} {4,8} {5,8}",
                                product.ProductId, product.ProductName, product.ProductDescription, product.ProductPrice, "|", product.ProductQuantiy));
                        }
                    }
                    Console.WriteLine();
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
                Tuple<List<Order>, string> getOrders = orders.PlaceCustomerOreder(customerId, storeLocation, ordersMade);

                Console.WriteLine("The order has been submitted.");
                Console.WriteLine("Order Summery:");
                Console.WriteLine();
                foreach (Order order in getOrders.Item1)
                {
                    Console.WriteLine(string.Format("{0,-39} | {1,-30} | {2:C2}", order.ProductName, order.Quantity, order.ProductPrice));
                }
                Console.WriteLine(getOrders.Item2);
                Console.WriteLine();
                Console.WriteLine("Thanks for placing an order.");
                Console.WriteLine();
                break;
            }
        }
        internal static void AddCustomer(string connectionString)
        {
            bool isAdding = true;
            while (isAdding)
            {
                string firstName;
                string lastName;
                string email = "";
                Tuple<string, string> nameOrEmailTuple = Validate.ValidateNameOrEmail();
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
                        Console.WriteLine();
                        break;
                    }
                    else
                    {
                        email = nameOrEmailTuple.Item2.ToUpper();
                        Console.WriteLine("Please enter first name and last name.");
                        string? name = Console.ReadLine();
                        Console.WriteLine();
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
                        Console.WriteLine();
                        emailTuple = Validate.ValidateEmail(emailLookUP);
                        if (!emailTuple.Item1)
                        {
                            break;
                        }
                        foreach (Customer customer in foundCoustomers)
                        {
                            if (customer.Email == emailTuple.Item2)
                            {
                                Console.WriteLine("Please use account associated with email to order.");
                                Console.WriteLine();
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
                    if (email == "")
                    {
                        Console.WriteLine("Please enter an email.");
                        emailLookUP = Console.ReadLine();
                        Console.WriteLine();
                        emailTuple = Validate.ValidateEmail(emailLookUP);
                        email = emailTuple.Item2;
                    }
                }
                Console.WriteLine("Please enter address 1.");
                string? address1 = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Please enter city.");
                string? city = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Please enter state.");
                string? state = Console.ReadLine();
                Console.WriteLine();
                Console.WriteLine("Please enter zip code.");
                string? zip = Console.ReadLine();
                Console.WriteLine();

                string address = Validate.ValidateAddress(address1, city, state, zip);

                if (string.IsNullOrWhiteSpace(address)) { break; }

                address1 = address.Split("\n")[0];
                city = address.Split("\n")[1];
                state = address.Split("\n")[2];
                zip = address.Split("\n")[3] + "-" + address.Split("\n")[4];

                Console.WriteLine("Please verify that the name, email, and address were entered correctly.");
                Console.WriteLine();
                Console.WriteLine($"{firstName} {lastName}\n{address1}\n{city}\n{state}\n{zip}\n{email}");
                Console.WriteLine();
                Console.WriteLine("Correct Yes(Y) or No(N)");
                string? input = Console.ReadLine()?.ToLower().Trim();
                Console.WriteLine();
                if (input == "n" || input == "no")
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

                bool isAdded = newCustomer.AddCustomer();
                if(isAdded)
                {
                    Console.WriteLine("The customer was successfully registered.");
                    Console.WriteLine();
                }
                break;
            }
        }
        internal static void LookupOrder(string connectionString)
        {
            Console.WriteLine("Would you like to lookup all orders of a customer or store?");
            Console.WriteLine("1. Customer");
            Console.WriteLine("2. Store");
            string? selection = Console.ReadLine();
            IOrderRepository orderRepository = new SqlOrderRepository(connectionString);
            switch(selection)
            {
                case "1":
                    Console.WriteLine("Please enter first name and last name.");
                    string? name = Console.ReadLine();
                    Console.WriteLine();
                    Tuple<string, string> fullname = Validate.VaildateName(name);
                    if (string.IsNullOrWhiteSpace(fullname.Item1) || string.IsNullOrWhiteSpace(fullname.Item2))
                    {
                        break;
                    }
                    string firstName = fullname.Item1.ToUpper();
                    string lastName = fullname.Item2.ToUpper();

                    Order allCutomerOrders = new Order(orderRepository);
                    List<Order> orders = allCutomerOrders.GetAllCustomerOrders(firstName, lastName);
                    if (orders.Count == 0)
                    {
                        Console.WriteLine("No orders have been made by this customer.");
                    }
                    else
                    {
                        foreach (Order order in orders)
                        {
                            Console.WriteLine(order.ProductName + " " + order.ProductPrice + " " + order.Quantity + " " + order.DateTime);
                        }
                    }
                    break;
                case "2":
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
                    while (!isInt)
                    {
                        isInt = int.TryParse(Console.ReadLine(), out storeLocation);
                        if (isInt == false || numberOfStores < 0 || storeLocation > numberOfStores)
                        {
                            Console.WriteLine("Invalid input.");
                            Console.WriteLine("Please choose a store location.");
                            Console.WriteLine();
                            isInt = false;
                        }
                    }
                    Order allStoreOrders = new Order(orderRepository);
                    List<Order> ordersFromStore = allStoreOrders.GetAllStoreOrders(storeLocation);
                    if (ordersFromStore.Count == 0)
                    {
                        Console.WriteLine("No orders have been made at this store.");
                    }
                    else
                    {
                        foreach (Order order in ordersFromStore)
                        {
                            Console.WriteLine(order.ProductName + " " + order.ProductPrice + " " + order.Quantity + " " + order.DateTime);
                        }
                    }
                    break;
                default:
                    Console.WriteLine("You did not make a valid selection");
                    break;
            }
        }
    }
}
