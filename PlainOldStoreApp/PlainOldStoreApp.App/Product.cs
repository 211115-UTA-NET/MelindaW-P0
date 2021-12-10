using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal class Product
    {
        internal int ProductId { get; set; }
        internal string ProductName { get; set; }
        internal decimal Price { get; set; }
        internal string ProductDescription { get; set; }
    }
}
