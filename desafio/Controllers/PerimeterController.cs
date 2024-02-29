using desafio.Models;
using desafio.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace desafio.Controllers
{
    public class PerimeterController : Controller
    {
        private readonly IPerimeterRepository _perimeterRepository;
        public PerimeterController(IPerimeterRepository perimeterRepository)
        {
            _perimeterRepository = perimeterRepository;
        }

        public IActionResult Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = userIdClaim != null && int.TryParse(userIdClaim.Value, out int result) ? result : 0;

            List<PerimeterModel> perimeter = _perimeterRepository.GetPerimeter(userId);
            return View(perimeter);
        }

        public IActionResult FormAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPerimeter(PerimeterModel perimeter)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = userIdClaim != null && int.TryParse(userIdClaim.Value, out int result) ? result : 0;

            _perimeterRepository.Add(userId, perimeter);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdatePerimeter(PerimeterModel perimeter)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = userIdClaim != null && int.TryParse(userIdClaim.Value, out int result) ? result : 0;

            _perimeterRepository.UpdatePerimeter(userId, perimeter);
            return RedirectToAction("Index");
        }
    }
}
