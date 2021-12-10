using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal class Store
    {
        internal int StoreId { get; set; }
        internal string StoreName { get; set; }
        internal string StoreAddress { get; set; }
        internal string StoreCity { get; set; }
        internal string StoreState { get; set; }
        internal int StoreZipCode { get; set; }
    }
}
