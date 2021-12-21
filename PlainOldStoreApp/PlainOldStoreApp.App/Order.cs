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

        private readonly IOrderRepository _orderRepository;

        internal Order(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        internal Order(int? productId, decimal productPrice, int? quantity)
        {             
            ProductId = productId;
            ProductPrice = productPrice;
            Quantity = quantity;
        }

        internal List<Order> PlaceCustomerOreder(Guid customerId, int storeId, List<Order> orders)
        {
            List<Order> getOrders = _orderRepository.AddAllOrders(customerId, storeId, orders);
            return getOrders;
        }
    }
}
