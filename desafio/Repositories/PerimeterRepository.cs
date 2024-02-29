using desafio.Data;
using desafio.Models;
using System;
using System.Data.SqlClient;

namespace desafio.Repositories
{
    public class PerimeterRepository : IPerimeterRepository
    {
        public PerimeterModel Add(int userId, PerimeterModel perimeter)
        {
            using (SqlConnection connection = SqlConnectionFactory.CreateConnection())
            {
                string insertQuery = "INSERT INTO perimetro (NomePerimetro, Id_Usuario) VALUES (@NomePerimetro, @Id_Usuario);";

                try
                {
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NomePerimetro", perimeter.Name);
                        command.Parameters.AddWithValue("@Id_Usuario", userId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao adicionar perímetro. {ex.Message}");
                }

                return perimeter;
            }
        }

        public List<PerimeterModel> GetPerimeter(int userID)
        {
            List<PerimeterModel> perimeters = new List<PerimeterModel>();
            using (SqlConnection connection = SqlConnectionFactory.CreateConnection())
            {
                string getQuery = "SELECT Id, NomePerimetro FROM perimetro WHERE Id_Usuario = @Id_Usuario;";

                try
                {
                    using (SqlCommand command = new SqlCommand (getQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id_Usuario", userID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PerimeterModel perimeter = new PerimeterModel()
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                };
                                perimeters.Add(perimeter);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao listar perímetros. {ex.Message}");
                }
                
                return perimeters;
            }
        }

        public PerimeterModel GetPerimeterById(int userId, int id)
        {
            PerimeterModel perimeter = null;

            using (SqlConnection connection = SqlConnectionFactory.CreateConnection())
            {
                string getByIdQuery = "SELECT Id, NomePerimetro FROM perimetro WHERE Id_Usuario = @Id_Usuario AND Id = @Id";

                try
                {
                    using (SqlCommand command = new SqlCommand(getByIdQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id_Usuario", userId);
                        command.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                perimeter = new PerimeterModel()
                                {
                                    Id= reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao obter perímetro. {ex.Message}");
                }

                return perimeter;
            }
        }

        public PerimeterModel UpdatePerimeter(int userId, PerimeterModel perimeter)
        {
            using (SqlConnection connection = SqlConnectionFactory.CreateConnection())
            {
                PerimeterModel existingPerimeter = GetPerimeterById(userId, perimeter.Id);

                if (existingPerimeter != null)
                {
                    existingPerimeter.Name = perimeter.Name;

                    string updateQuery = "UPDATE perimetro SET NomePerimetro = @NomePerimetro WHERE Id_Usuario = @Id_Usuario AND Id = @Id;";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NomePerimetro", existingPerimeter.Name);
                        command.Parameters.AddWithValue("@Id_Usuario", userId);
                        command.Parameters.AddWithValue("@Id", existingPerimeter.Id);

                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    throw new Exception($"Perímetro não encontrado.");
                }

                return existingPerimeter;
            }
        }
    }
}
