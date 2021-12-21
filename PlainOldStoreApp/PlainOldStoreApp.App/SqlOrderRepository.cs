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
            decimal? orderTotal = 0;
            foreach (Order order in orders)
            {
                orderTotal += order.ProductPrice * order.Quantity;
            }
            Console.WriteLine(orderTotal.ToString());
            Console.WriteLine(customerId);
            Console.WriteLine(storeId);

            using SqlConnection sqlConnection = new(_connectionString);
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
                    @customerId,
                    @storeID,
                    @orderTotal);", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@customerId", customerId);
            sqlCommand.Parameters.AddWithValue("@storeID", storeId);
            sqlCommand.Parameters.AddWithValue("@orderTotal", orderTotal);

            sqlCommand.ExecuteNonQuery();

            sqlConnection.Close();

            sqlConnection.Open();
            using SqlCommand sqlReadCommand = new(
                @"SELECT OrdersInvoiceID FROM Posa.OrdersInvoice
                    WHERE CustomerID = @customerId
                    AND StoreID = @storeID
                    AND OrderTotal = @orderTotal;", sqlConnection);

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

                sqlCommandOrders.ExecuteNonQuery();
            }

            sqlConnection.Close();

            string sqlUpdateString =
                @"UPDATE Posa.Inventory
                SET Quantity = (Quantity - @quantity)
                WHERE StoreID=@storeID
                AND ProductID=@ProductId;";

            sqlConnection.Open();

            foreach (Order order in orders)
            {
                using SqlCommand sqlUpdateCommand = new(sqlUpdateString, sqlConnection);

                sqlUpdateCommand.Parameters.AddWithValue("@quantity", order.Quantity);
                sqlUpdateCommand.Parameters.AddWithValue("@storeID", storeId);
                sqlUpdateCommand.Parameters.AddWithValue("@ProductId", order.ProductId);

                sqlUpdateCommand.ExecuteNonQuery();
            }


            throw new NotImplementedException();
        }
    }
}
