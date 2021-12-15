using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal class Customer
    {
        internal Customer ()
        {

        }
        internal Customer (string firstName, string lastName, string address, string city, string state, int zipCode, string email, Store store)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            City = city;
            State = state;
            ZipCode = zipCode;
            Email = email;
            Store = store;
        }
        internal string FirstName { get; set; }
        internal string LastName { get; set; }
        internal string Address { get; set; }
        internal string City { get; set; }
        internal string State { get; set; }
        internal int ZipCode { get; set; }
        internal string Email{ get; set; }
        internal Store Store { get; set; }
    }
}
