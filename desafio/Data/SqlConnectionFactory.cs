using System;
using System.Data.SqlClient;

namespace desafio.Data
{
    public class SqlConnectionFactory
    {
        public static SqlConnection CreateConnection()
        {
            SqlConnection connection = new SqlConnection("Server=Gabriel;Database=Teste;User Id=sa;Password=gabriel2004;");

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
