using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal class SqlCustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        internal SqlCustomerRepository(string connetionString)
        {
            _connectionString = connetionString;
        }
        public bool GetCustomerEmail(string? email)
        {
            string emailSQL = "";

            using SqlConnection connection = new(_connectionString);
            connection.Open();

            using SqlCommand sqlCommand = new(
                @"SELECT Email
                    FROM Posa.Customer
                    WHERE Email=@email",
                connection);

            sqlCommand.Parameters.AddWithValue("@email", email);

            using SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                emailSQL = reader.GetString(0);
            }
            connection.Close();
            if(emailSQL == email)
            {
                return true;
            }
            return false;
        }

        public List<Customer> GetAllCustomer(string? firstName, string? lastName)
        {
            List<Customer> customers = new();
            using SqlConnection connection = new(_connectionString);
            connection.Open();
            using SqlCommand sqlCommand = new(
                @"SELECT * FROM Posa.Customer
                WHERE FirstName = @firstName
                AND LastName = @lastName", connection);
            sqlCommand.Parameters.AddWithValue("@firstName", firstName);
            sqlCommand.Parameters.AddWithValue("@lastName", lastName);

            using SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                customers.Add(new(
                    reader.GetGuid(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7)));
            }
            connection.Close();
            return customers;
        }
        public Guid AddNewCustomer(
            string? firstName,
            string? lastName,
            string? address1,            
            string? city,
            string? state,
            string? zip,
            string email
            )
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();

            string sqlString = @"INSERT INTO Posa.Customer
                (
                    FirstName,
                    LastName,
                    Address1,
                    City,
                    State,
                    Zip,
                    Email
                )
                VALUES
                (
                    @firstName,
                    @lastName, 
                    @address1,
                    @city,
                    @state,
                    @zip,
                    @email);";

            using SqlCommand sqlCommand = new(sqlString, connection);

            sqlCommand.Parameters.AddWithValue("@firstName", firstName);
            sqlCommand.Parameters.AddWithValue("@lastName", lastName);
            sqlCommand.Parameters.AddWithValue("@address1", address1);
            sqlCommand.Parameters.AddWithValue("@city", city);
            sqlCommand.Parameters.AddWithValue("@state", state);
            sqlCommand.Parameters.AddWithValue("@zip", zip);
            sqlCommand.Parameters.AddWithValue("@email", email);

            connection.Close();

            connection.Open();

            using SqlCommand sqlCustomerId = new(
                @"SELECT CustomerID
                FROM Posa.Customer
                WHERE Email = @email", connection);

            sqlCustomerId.Parameters.AddWithValue("@email", email);
            using SqlDataReader reader = sqlCommand.ExecuteReader();
            Guid customerId = new();
            while (reader.Read())
            {
                customerId = reader.GetGuid(0);
            }

            connection.Close();

            return customerId;
        }
    }
}
