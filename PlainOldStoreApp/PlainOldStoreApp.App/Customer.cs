using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal class Customer
    {
        private readonly Guid customerId = Guid.NewGuid();
        internal string? FirstName { get; }
        internal string? LastName { get; }
        internal string? Address1 { get; }
        internal string? City { get; }
        internal string? State { get; }
        internal string? ZipCode { get; }
        internal string Email { get; }
        

        private readonly ICustomerRepository _customerRepository;

        internal Customer (string email, ICustomerRepository customerRepository)
        {
            Email = email;
            _customerRepository = customerRepository;
        }
        internal Customer (
            string firstName,
            string lastName,
            string address1,
            string city,
            string state,
            string zipCode,
            string email,
            ICustomerRepository customerRepository)
        {
            FirstName = firstName;
            LastName = lastName;
            Address1 = address1;
            City = city;
            State = state;
            ZipCode = zipCode;
            Email = email;
            _customerRepository = customerRepository;
        }

        internal bool LookUpEmail()
        {
            bool foundEmail = _customerRepository.GetCustomerEmail(Email);
            return foundEmail;
        }

        internal bool AddCustomer()
        {
            bool isAdd = _customerRepository.AddNewCustomer(customerId, FirstName, LastName, Address1, City, State, ZipCode, Email);
            return isAdd;
        }
    }
}
