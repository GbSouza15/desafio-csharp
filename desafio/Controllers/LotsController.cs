using desafio.Models;
using desafio.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace desafio.Controllers
{
    public class LotsController : Controller
    {
        private readonly ILotsRepository _lotsRepository;
        private readonly IPerimeterRepository _perimeterRepository;
        private readonly IProducerRepository _producerRepository;
        public LotsController(ILotsRepository lotsRepository, IPerimeterRepository perimeterRepository, IProducerRepository producerRepository)
        {
            _lotsRepository = lotsRepository;
            _perimeterRepository = perimeterRepository;
            _producerRepository = producerRepository;
        }

        public IActionResult Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = userIdClaim != null && int.TryParse(userIdClaim.Value, out int result) ? result : 0;

            List<LotsModel> lots = _lotsRepository.GetLots(userId);
            List<PerimeterModel> perimeters = _perimeterRepository.GetPerimeter(userId);
            List<ProducerModel> producers = _producerRepository.GetProducers(userId);

            ViewBag.Perimeters = perimeters;
            ViewBag.Producers = producers;
            ViewBag.Lots = lots;

            return View();
        }

        public IActionResult FormAdd()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = userIdClaim != null && int.TryParse(userIdClaim.Value, out int result) ? result : 0;

            List<PerimeterModel> perimeters = _perimeterRepository.GetPerimeter(userId);
            List<ProducerModel> producers = _producerRepository.GetProducers(userId);

            ViewBag.Perimeters = perimeters;
            ViewBag.Producers = producers;

            return View();
        }

        [HttpPost]
        public IActionResult AddLots(LotsModel lot)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                int userId = userIdClaim != null && int.TryParse(userIdClaim.Value, out int result) ? result : 0;

                _lotsRepository.Add(userId, lot);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MessageError"] = $"Erro ao adicionar lote. {ex.Message}";
                return RedirectToAction("FormAdd");
            }
            
        }

        [HttpPost]
        public IActionResult UpdateLots(int id, LotsModel lotsModel)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                int userId = userIdClaim != null && int.TryParse(userIdClaim.Value, out int result) ? result : 0;

                _lotsRepository.UpdateLots(userId, id, lotsModel);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MessageError"] = $"Erro ao editar lote. {ex.Message}";
                return RedirectToAction("Index");
            }
            
        }
    }
}
