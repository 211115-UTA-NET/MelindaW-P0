using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal interface IOrderRepository
    {
        List<Order> AddAllOrders(Guid customerId, int storeId, List<Order> orders);
    }
}
