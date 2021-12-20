using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.App
{
    internal class SqlStoreRepository : IStoreRepository
    {
        private readonly string _connectionString;

        internal SqlStoreRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Dictionary<int, string> RetriveStores()
        {
            Dictionary<int, string> stores = new Dictionary<int, string>();
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            using SqlCommand sqlCommand = new(@"SELECT * FROM Posa.Stores", connection);
            
            using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                stores.Add(sqlDataReader.GetInt32(0), sqlDataReader.GetString(1));
            }

            connection.Close();
            return stores;
        }
    }
}
