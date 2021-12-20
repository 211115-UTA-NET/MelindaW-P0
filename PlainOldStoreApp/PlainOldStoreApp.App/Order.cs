using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal class Order
    {
        private int OrderLineID = 0;
        internal int OrderID { get; set; }
        internal int StoreID { get; set; }
        internal Guid CustomerID { get; set; }
        internal DateTime OrderTime { get; set; }
        internal int ProductID { get; set; }
        internal int Quantity { get; set; }
    }
}
