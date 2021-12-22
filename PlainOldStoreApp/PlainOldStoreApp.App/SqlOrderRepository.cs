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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="storeId"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public Tuple<List<Order>, string> AddAllOrders(Guid customerId, int storeId, List<Order> orders)
        {
            decimal? orderTotal = 0;
            foreach (Order order in orders)
            {
                orderTotal += order.ProductPrice * order.Quantity;
            }

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
            if (reader.Read())
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
            sqlConnection.Close();

            List<Order> orderItems = new List<Order>();

            string sqlGetOrderString =
                @"SELECT ProductName, Quantity, Posa.CustomerOrders.ProductPrice
                    FROM Posa.CustomerOrders
                    INNER JOIN Posa.Products ON Posa.CustomerOrders.ProductID=Posa.Products.ProductID
                    WHERE OrdersInvoiceID=@ordersInvoiceId;";

            sqlConnection.Open();

            using SqlCommand sqlGetOrderCommand = new(sqlGetOrderString, sqlConnection);

            sqlGetOrderCommand.Parameters.AddWithValue("@ordersInvoiceId", ordersInvoiceId);

            using SqlDataReader readOrder = sqlGetOrderCommand.ExecuteReader();

            while (readOrder.Read())
            {
                orderItems.Add(new(
                    readOrder.GetString(0),
                    readOrder.GetInt32(1),
                    readOrder.GetDecimal(2)));
            }
            sqlConnection.Close ();

            string orderSummery ="";

            string sqlGetOrderSummeryString =
                @"SELECT StoreCity, OrderTime, OrderTotal
                    FROM Posa.OrdersInvoice
                    INNER JOIN Posa.Stores ON Posa.OrdersInvoice.StoreID=Posa.Stores.StoreID
                    WHERE OrdersInvoiceID=@ordersInvoiceID;";

            sqlConnection.Open();

            using SqlCommand sqlGetOrderSummery = new(sqlGetOrderSummeryString, sqlConnection);

            sqlGetOrderSummery.Parameters.AddWithValue("@ordersInvoiceID", ordersInvoiceId);

            using SqlDataReader readSummery = sqlGetOrderSummery.ExecuteReader();

            if(readSummery.Read())
            {
                orderSummery = $"{readSummery.GetString(0)}\t{readSummery.GetDateTime(1)}\t{readSummery.GetDecimal(2)}";
            }

            sqlConnection.Close();

            return new Tuple<List<Order>, string> (orderItems, orderSummery);
        }

        public List<Order> GetAllCoustomerOrders(string fisrtName, string lastName)
        {
            List<Order> allCustomerOrders = new List<Order>();
            
            string sqlGetAllCustomerOrdersString =
                @"SELECT FirstName, LastName, ProductName, Posa.CustomerOrders.ProductPrice, Quantity, Posa.OrdersInvoice.OrderTime
                    FROM Posa.Customer
                    INNER JOIN Posa.OrdersInvoice ON Posa.Customer.CustomerID=Posa.OrdersInvoice.CustomerID
                    INNER JOIN Posa.CustomerOrders ON Posa.OrdersInvoice.OrdersInvoiceID=Posa.OrdersInvoice.OrdersInvoiceID
                    INNER JOIN Posa.Products ON Posa.CustomerOrders.ProductID=Posa.Products.ProductID
                    WHERE FirstName = @firstName
                    AND LastName = @lastName;";

            using SqlConnection sqlConnection = new(_connectionString);
            sqlConnection.Open();
            using SqlCommand sqlGetAllCustomerOrders = new(sqlGetAllCustomerOrdersString, sqlConnection);
            sqlGetAllCustomerOrders.Parameters.AddWithValue("@firstName", fisrtName);
            sqlGetAllCustomerOrders.Parameters.AddWithValue("@lastName", lastName);
            using SqlDataReader dataReader = sqlGetAllCustomerOrders.ExecuteReader();

            while(dataReader.Read())
            {
                allCustomerOrders.Add(new(
                    dataReader.GetString(2),
                    dataReader.GetDecimal(3),
                    dataReader.GetInt32(4),
                    dataReader.GetDateTime(5)));
            }
            sqlConnection.Close();
            return allCustomerOrders;
        }

        public List<Order> GetAllStoreOrders(int storeID)
        {
            List<Order> allStoreOrders = new List<Order>();

            string sqlGetAllStoreOrdersString =
                @"SELECT Posa.OrdersInvoice.StoreID, ProductName, Posa.CustomerOrders.ProductPrice, Quantity, Posa.OrdersInvoice.OrderTime, Posa.CustomerOrders.OrderLineID
                    FROM Posa.OrdersInvoice
                    INNER JOIN Posa.CustomerOrders ON Posa.OrdersInvoice.OrdersInvoiceID=Posa.OrdersInvoice.OrdersInvoiceID
                    INNER JOIN Posa.Products ON Posa.CustomerOrders.ProductID=Posa.Products.ProductID
                    WHERE Posa.OrdersInvoice.StoreID=@storeID;";

            using SqlConnection sqlConnection = new(_connectionString);
            sqlConnection.Open();
            using SqlCommand sqlGetAllStoreOrders = new(sqlGetAllStoreOrdersString, sqlConnection);
            sqlGetAllStoreOrders.Parameters.AddWithValue("@storeID", storeID);
            using SqlDataReader dataReader = sqlGetAllStoreOrders.ExecuteReader();

            while (dataReader.Read())
            {
                allStoreOrders.Add(new(
                    dataReader.GetString(1),
                    dataReader.GetDecimal(2),
                    dataReader.GetInt32(3),
                    dataReader.GetDateTime(4)));
            }
            sqlConnection.Close();
            return allStoreOrders;
        }
    }
}
