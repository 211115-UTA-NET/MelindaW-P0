using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal interface IOrderRepository
    {
        Tuple<List<Order>, string> AddAllOrders(Guid customerId, int storeId, List<Order> orders);

        List<Order> GetAllStoreOrders(int storeID);

        List<Order> GetAllCoustomerOrders(string firstName, string lastName);
    }
}
