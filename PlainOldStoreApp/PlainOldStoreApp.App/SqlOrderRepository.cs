using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal class SqlOrderRepository : IOrderRepository
    {
        private readonly string _connectionString;
        internal SqlOrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Order> AddAllOrders(Guid customerId, int storeId, List<Order> orders)
        {
            using SqlConnection sqlConnection = new(_connectionString);

            decimal orderTotal = 0;
            foreach (Order order in orders)
            {
                orderTotal += order.ProductPrice;
            }

            sqlConnection.Open();

            using SqlCommand sqlCommand = new(
                @"INSERT INTO Posa.OrdersInvoice
                (
                    CustomerID,
                    StoreID,
                    OrderTotal
                )
                VALUES
                (
                    @customerId
                    @storeID
                    @orderTotal);", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@customerId", customerId);
            sqlCommand.Parameters.AddWithValue("@storeID", storeId);
            sqlCommand.Parameters.AddWithValue("@orderTotal", orderTotal);

            sqlConnection.Close();

            sqlConnection.Open();
            using SqlCommand sqlReadCommand = new(
                @"SELECT OrdersInvoiceID FROM Posa.OrdesInvoice
                    WHERE CustomerID = @customerId
                    AND StoreID = @storeID
                    AND OrderTotal = @orderTotal;");

            sqlReadCommand.Parameters.AddWithValue("@customerId", customerId);
            sqlReadCommand.Parameters.AddWithValue("@storeID", storeId);
            sqlReadCommand.Parameters.AddWithValue("@orderTotal", orderTotal);

            using SqlDataReader reader = sqlReadCommand.ExecuteReader();
            int ordersInvoiceId = 0;
            while (reader.Read())
            {
                ordersInvoiceId = reader.GetInt32(0);
            }

            sqlConnection.Close();

            string sqlString =
                @"INSERT INTO Posa.CustomerOrders
                (
                    OrdersInvoiceID,
                    ProductID,
                    ProductPrice,
                    Quantity
                )
                VALUES
                (
                    @ordersInvoiceId,
                    @productId,
                    @productPrice,
                    @quantity);";

            sqlConnection.Open();

            foreach (Order order in orders)
            {
                using SqlCommand sqlCommandOrders = new(sqlString, sqlConnection);

                sqlCommandOrders.Parameters.AddWithValue("@ordersInvoiceId", ordersInvoiceId);
                sqlCommandOrders.Parameters.AddWithValue("@productId", order.ProductId);
                sqlCommandOrders.Parameters.AddWithValue("@productPrice", order.ProductPrice);
                sqlCommandOrders.Parameters.AddWithValue("@quantity", order.Quantity);
            }

            sqlConnection.Close();

            throw new NotImplementedException();
        }
    }
}
