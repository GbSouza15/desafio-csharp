using desafio.Data;
using desafio.Models;
using System;
using System.Data.SqlClient;

namespace desafio.Repositories
{
    public class UserRepository : IUserRepository
    {
        public UserModel CreateUser(UserModel user)
        {
            using (SqlConnection connection = SqlConnectionFactory.CreateConnection())
            {
                if (IsEmailInUse(connection, user.Email))
                {
                    throw new Exception("Email já em uso. Por favor, escolha outro.");
                }

                string createUserQuery = @"
                    INSERT INTO usuario (Username, Email, Password) 
                    VALUES (@Username, @Email, @Password);
                    SELECT SCOPE_IDENTITY();";

                try
                {
                    using (SqlCommand command = new SqlCommand(createUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", user.Username);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Password", user.Password);

                        int userId = Convert.ToInt32(command.ExecuteScalar());

                        user.Id = userId;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao criar o usuário: {ex.Message}");
                }

                return user;
            }
        }

        public UserModel GetUserByCredentials(string email, string password)
        {
            using (SqlConnection connection = SqlConnectionFactory.CreateConnection())
            {
                string getUserByCredentialsQuery = @"
                    SELECT Id, Username, Email, Password
                    FROM usuario
                    WHERE Email = @Email AND Password = @Password;";

                UserModel user = null;

                try
                {
                    using (SqlCommand command = new SqlCommand(getUserByCredentialsQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new UserModel
                                {
                                    Id = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Password = reader.GetString(3)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao obter usuário por credenciais: {ex.Message}");
                }

                if (user == null)
                {
                    throw new Exception("Credenciais inválidas");
                }

                return user;
            }
        }

        public UserModel GetUserById(int userId)
        {
            using (SqlConnection connection = SqlConnectionFactory.CreateConnection())
            {
                string getUserByIdQuery = @"
                    SELECT Id, Username, Email, Password
                    FROM usuario
                    WHERE Id = @UserId;";

                UserModel user = null;

                try
                {
                    using (SqlCommand command = new SqlCommand(getUserByIdQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new UserModel
                                {
                                    Id = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Password = reader.GetString(3)
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao obter usuário por ID: {ex.Message}");
                }

                return user;
            }
        }

        private bool IsEmailInUse(SqlConnection connection, string email)
        {
            string checkEmailQuery = "SELECT COUNT(*) FROM usuario WHERE Email = @Email";

            using (SqlCommand command = new SqlCommand(checkEmailQuery, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }
    }
}
