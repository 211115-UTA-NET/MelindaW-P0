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
        /// <summary>
        /// Queries the PosaDatabase and returns the products for a store
        /// </summary>
        /// <param name="storeLocation"></param>
        /// <returns>List<Products></Products></returns>
        public List<Product> GetAllStoreProducts(int storeLocation)
        {
            List<Product> products = new();

            using SqlConnection connection = new SqlConnection(_connectionString);
            
            using SqlCommand sqlCommand = new(
                @"SELECT Inventory.ProductID, ProductName, ProductDescription, ProductPrice, Quantity, Inventory.StoreID
                FROM Posa.Inventory
                INNER JOIN Posa.Products ON Inventory.ProductID=Products.ProductID
                WHERE Inventory.StoreID=@storeId;", connection);

            sqlCommand.Parameters.AddWithValue("@storeId", storeLocation);
            try
            {
                connection.Open();
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
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
            }
            finally
            {
                connection.Close();
            }
            return products;
        }
    }
}
