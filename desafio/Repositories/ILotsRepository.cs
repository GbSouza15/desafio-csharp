using desafio.Models;
using System.Collections.Generic;

namespace desafio.Repositories
{
    public interface ILotsRepository
    {
        List<LotsModel> GetLots(int userId);
        LotsModel GetLotbyId(int userId, int id);
        LotsModel UpdateLots(int userId, int id, LotsModel lotsModel);
        LotsModel Add(int userId, LotsModel lot);
    }
}
