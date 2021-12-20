using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal interface ICustomerRepository
    {
        bool GetCustomerEmail(string email);
        
        bool AddNewCustomer(
            Guid customerId,
            string? firstName,
            string? lastName,
            string? address1,
            string? city,
            string? state,
            string? zip,
            string email);
    }
}
