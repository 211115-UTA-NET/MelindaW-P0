using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal interface ICustomerRepository
    {
        bool GetCustomerEmail(string? email);

        List<Customer> GetAllCustomer(string? firstName, string? lasName);
        
        Guid AddNewCustomer(
            string? firstName,
            string? lastName,
            string? address1,
            string? city,
            string? state,
            string? zip,
            string email);
    }
}
