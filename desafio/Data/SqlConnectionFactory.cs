using System;
using System.Data.SqlClient;

namespace desafio.Data
{
    public class SqlConnectionFactory
    {
        public static SqlConnection CreateConnection()
        {
            SqlConnection connection = new SqlConnection("String de conexão aqui.");

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao abrir a conexão: {ex.Message}");
            }

            return connection;
        }
    }
}
