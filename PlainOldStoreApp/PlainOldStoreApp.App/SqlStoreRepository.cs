using System.Data.SqlClient;

namespace PlainOldStoreApp.App
{
    internal class SqlStoreRepository : IStoreRepository
    {
        private readonly string _connectionString;

        internal SqlStoreRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Queries the PosaDatabase and returns a list of stores
        /// </summary>
        /// <returns>Dictionary<int, string></int></returns>
        public Dictionary<int, string> RetriveStores()
        {
            Dictionary<int, string> stores = new Dictionary<int, string>();
            using SqlConnection connection = new SqlConnection(_connectionString);
            

            using SqlCommand sqlCommand = new(@"SELECT * FROM Posa.Stores", connection);
            try
            {
                connection.Open();
                using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    stores.Add(sqlDataReader.GetInt32(0), sqlDataReader.GetString(1));
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return stores;
        }
    }
}
