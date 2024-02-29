using desafio.Data;
using desafio.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace desafio.Repositories
{
    public class LotsRepository : ILotsRepository
    {
        public LotsModel Add(int userId, LotsModel lot)
        {
            using (SqlConnection connection = SqlConnectionFactory.CreateConnection())
            {
                // Verificar se o código do lote já existe
                if (IsCodeInUse(connection, userId, lot.CodeLot))
                {
                    throw new Exception("Já existe um lote cadastrado com esse código.");
                }

                string insertQuery = "INSERT INTO lote (CodigoLote, Id_Perimetro, Id_Produtor, Id_Usuario) VALUES (@CodigoLote, @Id_Perimetro, @Id_Produtor, @Id_Usuario);";

                try
                {
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CodigoLote", lot.CodeLot);
                        command.Parameters.AddWithValue("@Id_Perimetro", lot.IdPerimeter);
                        command.Parameters.AddWithValue("@Id_Produtor", lot.IdProducer);
                        command.Parameters.AddWithValue("@Id_Usuario", userId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao inserir no banco de dados: {ex.Message}");
                    throw new Exception($"Erro ao inserir no banco de dados: {ex.Message}");
                }

                return lot;
            }
        }

        public LotsModel GetLotbyId(int userId, int id)
        {
            LotsModel lots = null;

            using (SqlConnection connection = SqlConnectionFactory.CreateConnection())
            {
                string getByIdQuery = @"
                    SELECT lote.Id, lote.CodigoLote, lote.Id_Perimetro, lote.Id_Produtor, 
                    perimetro.NomePerimetro, produtor.NomeProdutor 
                    FROM lote 
                    JOIN perimetro ON lote.Id_Perimetro = perimetro.Id 
                    JOIN produtor ON lote.Id_Produtor = produtor.Id
                    WHERE lote.Id_Usuario = @UserId AND lote.Id = @Id;";

                try
                {
                    using (SqlCommand command = new SqlCommand(getByIdQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lots = new LotsModel()
                                {
                                    Id = reader.GetInt32(0),
                                    CodeLot = reader.GetString(1),
                                    IdPerimeter = reader.GetInt32(2),
                                    IdProducer = reader.GetInt32(3),
                                    PerimeterName = reader.GetString(4),
                                    ProducerName = reader.GetString(5)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao obter o lote por Id: {ex.Message}");
                    throw new Exception($"Erro ao obter o lote por Id: {ex.Message}");
                }

                return lots;
            }
        }

        public List<LotsModel> GetLots(int userId)
        {
            List<LotsModel> lots = new List<LotsModel>();

            using (SqlConnection connection = SqlConnectionFactory.CreateConnection())
            {
                string getQuery = @"
                    SELECT lote.Id, lote.CodigoLote, lote.Id_Perimetro, lote.Id_Produtor, 
                    perimetro.NomePerimetro, produtor.NomeProdutor 
                    FROM lote 
                    JOIN perimetro ON lote.Id_Perimetro = perimetro.Id 
                    JOIN produtor ON lote.Id_Produtor = produtor.Id
                    WHERE lote.Id_Usuario = @UserId;";

                try
                {
                    using (SqlCommand command = new SqlCommand(getQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                LotsModel lot = new LotsModel()
                                {
                                    Id = reader.GetInt32(0),
                                    CodeLot = reader.GetString(1),
                                    IdPerimeter = reader.GetInt32(2),
                                    IdProducer = reader.GetInt32(3),
                                    PerimeterName = reader.GetString(4),
                                    ProducerName = reader.GetString(5),
                                };
                                lots.Add(lot);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao listar os lotes: {ex.Message}");
                    throw new Exception($"Erro ao listar os lotes: {ex.Message}");
                }
                return lots;
            }
        }

        private bool IsCodeInUse(SqlConnection connection, int userId, string codeLot)
        {
            string checkCodeQuery = "SELECT COUNT(*) FROM lote WHERE CodigoLote = @CodeLot AND Id_Usuario = @UserId";

            using (SqlCommand command = new SqlCommand(checkCodeQuery, connection))
            {
                command.Parameters.AddWithValue("@CodeLot", codeLot);
                command.Parameters.AddWithValue("@UserId", userId);
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        public LotsModel UpdateLots(int userId, int id, LotsModel lots)
        {
            using (SqlConnection connection = SqlConnectionFactory.CreateConnection())
            {
                if (IsCodeInUse(connection, userId, lots.CodeLot))
                {
                    throw new Exception("Já existe um lote cadastrado com esse código.");
                }

                LotsModel existingLots = GetLotbyId(userId, lots.Id);

                if (existingLots != null)
                {
                    existingLots.IdPerimeter = lots.IdPerimeter;
                    existingLots.IdProducer = lots.IdProducer;
                    existingLots.ProducerName = lots.ProducerName;
                    existingLots.PerimeterName = lots.PerimeterName;
                    existingLots.CodeLot = lots.CodeLot;

                    string updateQuery = @"
                    UPDATE lote 
                    SET CodigoLote = @CodigoLote, 
                    Id_Perimetro = @Id_Perimetro, 
                    Id_Produtor = @Id_Produtor 
                    WHERE Id_Usuario = @UserId AND Id = @Id;";

                    try
                    {
                        using (SqlCommand command = new SqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@CodigoLote", existingLots.CodeLot);
                            command.Parameters.AddWithValue("@Id_Perimetro", existingLots.IdPerimeter);
                            command.Parameters.AddWithValue("@Id_Produtor", existingLots.IdProducer);
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@Id", existingLots.Id);
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao atualizar o lote: {ex.Message}");
                        throw new Exception($"Erro ao atualizar o lote: {ex.Message}");
                    }
                }

                return existingLots;
            }
        }
    }
}
