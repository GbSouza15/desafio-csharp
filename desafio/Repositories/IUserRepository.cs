using desafio.Models;
using System.Collections.Generic;

namespace desafio.Repositories
{
    public interface IUserRepository
    {
        UserModel CreateUser(UserModel user);
        UserModel GetUserById(int userId);
        UserModel GetUserByCredentials(string username, string password);
    }
}
