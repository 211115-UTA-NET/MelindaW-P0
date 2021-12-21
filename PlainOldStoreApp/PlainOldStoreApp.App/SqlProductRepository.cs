using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal class SqlProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        internal SqlProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Product> GetAllStoreProducts(int storeLocation)
        {
            List<Product> products = new();
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            using SqlCommand sqlCommand = new(
                @"SELECT Inventory.ProductID, ProductName, ProductDescription, ProductPrice, Quantity, Inventory.StoreID
                FROM Posa.Inventory
                INNER JOIN Posa.Products ON Inventory.ProductID=Products.ProductID
                WHERE Inventory.StoreID=@storeId;", connection);
            sqlCommand.Parameters.AddWithValue("@storeId", storeLocation);
            using SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                products.Add(new(
                    reader.GetInt32(0),
                    reader.GetString(1), 
                    reader.GetString(2), 
                    reader.GetDecimal(3),
                    reader.GetInt32(4),
                    reader.GetInt32(5)));
            }
            connection.Close();
            return products;
        }
    }
}
