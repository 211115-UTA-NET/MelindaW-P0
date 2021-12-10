using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal class Customer
    {
        internal int CustomerId { get; set; }
        internal string FirstName { get; set; }
        internal string LastName { get; set; }
        internal string Address { get; set; }
        internal string City { get; set; }
        internal string State { get; set; }
        internal int ZipCode { get; set; }
        internal string Email { get; set; }
    }
}
