using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal class Order
    {
        internal Guid CustomerId { get; }
        internal int StoreId { get; }
        internal decimal OrderTotal { get; }
        internal int OrdersInvoiceID { get; }
        internal int? ProductId { get; }
        internal decimal ProductPrice { get; }
        internal int? Quantity { get; }
        internal string ProductName { get; }
        internal DateTime DateTime { get; }

        private readonly IOrderRepository _orderRepository;

        internal Order(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        internal Order (string productName, int quantity, decimal productPrice)
        {
            ProductName = productName;
            Quantity = quantity;
            ProductPrice = productPrice;
        }
        internal Order(int? productId, decimal productPrice, int? quantity)
        {             
            ProductId = productId;
            ProductPrice = productPrice;
            Quantity = quantity;
        }
        internal Order(string productName, decimal productPrice, int quantity, DateTime orderdate)
        {
            ProductName=productName;
            ProductPrice=productPrice;
            Quantity=quantity;
            DateTime = orderdate;
        }

        internal Tuple<List<Order>, string> PlaceCustomerOreder(Guid customerId, int storeId, List<Order> orders)
        {
            Tuple<List<Order>, string> getOrders = _orderRepository.AddAllOrders(customerId, storeId, orders);
            return getOrders;
        }

        internal List<Order> GetAllCustomerOrders(string firstname, string lastName)
        {
            List<Order> orders = _orderRepository.GetAllCoustomerOrders(firstname, lastName);
            return orders;
        }

        internal List<Order> GetAllStoreOrders(int storeID)
        {
            List<Order> orders = _orderRepository.GetAllStoreOrders(storeID);
            return orders;
        }
    }
}
