using desafio.Models;
using System.Collections.Generic;

namespace desafio.Repositories
{
    public interface IUserRepository
    {
        // Método para criar um novo usuário
        UserModel CreateUser(UserModel user);

        // Método para buscar um usuário pelo ID
        UserModel GetUserById(int userId);

        // Método para buscar um usuário por credenciais (por exemplo, nome de usuário e senha)
        UserModel GetUserByCredentials(string username, string password);
    }
}
