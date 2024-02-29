using desafio.Data;
using desafio.Models;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace desafio.Repositories
{
    public class ProducerRepository : IProducerRepository
    {
        public ProducerModel Add(int userId, ProducerModel producer)
        {
            using (SqlConnection connection = SqlConnectionFactory.CreateConnection())
            {

                if (IsCPFDuplicated(connection, userId, producer.CPF))
                {
                    throw new Exception("CPF já em uso.");
                }

                string insertQuery = "INSERT INTO produtor (NomeProdutor, CPF, Id_Usuario) VALUES (@NomeProdutor, @CPF, @Id_Usuario);";

                try
                {
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NomeProdutor", producer.Name);
                        command.Parameters.AddWithValue("@CPF", producer.CPF);
                        command.Parameters.AddWithValue("@Id_Usuario", userId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao adicionar usuário. {ex.Message}");
                }

                return producer;
            }
        }


        public List<ProducerModel> GetProducers(int userId)
        {
            List<ProducerModel> producers = new List<ProducerModel>();

            using (SqlConnection connection = SqlConnectionFactory.CreateConnection ())
            {
                string getQuery = "SELECT Id, NomeProdutor, CPF FROM produtor WHERE Id_Usuario = @Id_Usuario";

                try
                {
                    using (SqlCommand command = new SqlCommand (getQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id_Usuario", userId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProducerModel producer = new ProducerModel()
                                {
                                   Id = reader.GetInt32(0),
                                   Name = reader.GetString(1),
                                   CPF = reader.GetString(2)
                                };
                                producers.Add(producer);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao listar produtores. {ex.Message}");
                }

                return producers;
            }
        }

        public ProducerModel GetProducersById(int userId, int id)
        {
            ProducerModel producer = null;

            using (SqlConnection connection = SqlConnectionFactory.CreateConnection())
            {

                string getByIdQuery = "SELECT Id, NomeProdutor, CPF FROM produtor WHERE Id_Usuario = @Id_Usuario AND Id = @Id";

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
                                producer = new ProducerModel()
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    CPF = reader.GetString(2)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro obter produtor. {ex.Message}");
                }

                return producer;
            }
        }

        public ProducerModel UpdateProducer(int userId, ProducerModel producerModel)
        {
            using (SqlConnection connection = SqlConnectionFactory.CreateConnection())
            {
                if (IsCPFDuplicated(connection, userId, producerModel.CPF))
                {
                    throw new Exception("CPF já em uso.");
                }

                ProducerModel existingProducer = GetProducersById(userId, producerModel.Id);

                if (existingProducer != null)
                {
                    existingProducer.Name = producerModel.Name;
                    existingProducer.CPF = producerModel.CPF;

                    string updateQuery = "UPDATE produtor SET NomeProdutor = @NomeProdutor, CPF = @CPF WHERE Id_Usuario = @Id_Usuario AND Id = @Id;";

                    try
                    {
                        using (SqlCommand command = new SqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@NomeProdutor", existingProducer.Name);
                            command.Parameters.AddWithValue("@CPF", existingProducer.CPF);
                            command.Parameters.AddWithValue("@Id_Usuario", userId);
                            command.Parameters.AddWithValue("@Id", existingProducer.Id);

                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Erro ao atualizar produtor. {ex.Message}");
                    }
                }

                return existingProducer;
            }
        }

        private bool IsCPFDuplicated(SqlConnection connection, int userId, string cpf)
        {
            string checkCPFQuery = "SELECT COUNT(*) FROM produtor WHERE Id_Usuario = @Id_Usuario AND CPF = @CPF";

            using (SqlCommand command = new SqlCommand(checkCPFQuery, connection))
            {
                command.Parameters.AddWithValue("@Id_Usuario", userId);
                command.Parameters.AddWithValue("@CPF", cpf);
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }
    }
}
