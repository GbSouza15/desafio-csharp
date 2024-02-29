using desafio.Models;

namespace desafio.Repositories
{
    public interface IPerimeterRepository
    {
        List<PerimeterModel> GetPerimeter(int userid);
        PerimeterModel GetPerimeterById(int userId, int id);
        PerimeterModel UpdatePerimeter(int userId, PerimeterModel perimeter);
        PerimeterModel Add(int userId, PerimeterModel perimeter);
    }
}
