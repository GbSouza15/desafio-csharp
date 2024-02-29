using System;
using System.Data.SqlClient;

namespace desafio.Data
{
    public class CreateTables
    {
        public static void TableCreate(SqlConnection connection)
        {
            if (!TableExists(connection, "usuario"))
            {
                CreateUserTable(connection);
            }

            if (!TableExists(connection, "perimetro"))
            {
                CreatePerimetroTable(connection);
            }

            if (!TableExists(connection, "produtor"))
            {
                CreateProdutorTable(connection);
            }

            if (!TableExists(connection, "lote"))
            {
                CreateLoteTable(connection);
            }
        }

        private static bool TableExists(SqlConnection connection, string tableName)
        {
            string query = $"SELECT 1 FROM sys.tables WHERE name = '{tableName}'";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                return command.ExecuteScalar() != null;
            }
        }

        private static void CreateUserTable(SqlConnection connection)
        {
            string createTableScript = @"
                IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'usuario')
                BEGIN
                    CREATE TABLE usuario (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Username NVARCHAR(100),
                        Email NVARCHAR(50) UNIQUE,
                        Password NVARCHAR(255)
                    );
                END
            ";

            ExecuteSqlCommand(connection, createTableScript);
        }

        private static void CreatePerimetroTable(SqlConnection connection)
        {
            string createTableScript = @"
                IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'perimetro')
                BEGIN
                    CREATE TABLE perimetro (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        NomePerimetro NVARCHAR(100),
                        Id_Usuario INT,
                        FOREIGN KEY (Id_Usuario) REFERENCES usuario(Id)
                    );
                END
            ";

            ExecuteSqlCommand(connection, createTableScript);
        }

        private static void CreateLoteTable(SqlConnection connection)
        {
            string createTableScript = @"
                IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'lote')
                BEGIN
                    CREATE TABLE lote (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        CodigoLote NVARCHAR(50),
                        Id_Perimetro INT,
                        Id_Produtor INT,
                        Id_Usuario INT,
                        FOREIGN KEY (Id_Perimetro) REFERENCES perimetro(Id),
                        FOREIGN KEY (Id_Produtor) REFERENCES produtor(Id),
                        FOREIGN KEY (Id_Usuario) REFERENCES usuario(Id)
                    );
                END
            ";

            ExecuteSqlCommand(connection, createTableScript);
        }

        private static void CreateProdutorTable(SqlConnection connection)
        {
            string createTableScript = @"
                IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'produtor')
                BEGIN
                    CREATE TABLE produtor (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        NomeProdutor NVARCHAR(100),
                        CPF NVARCHAR(14) UNIQUE,
                        Id_Usuario INT,
                        FOREIGN KEY (Id_Usuario) REFERENCES usuario(Id)
                    );
                END
            ";

            ExecuteSqlCommand(connection, createTableScript);
        }

        private static void ExecuteSqlCommand(SqlConnection connection, string commandText)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao executar o comando SQL: {ex.Message}");
            }
        }
    }
}
