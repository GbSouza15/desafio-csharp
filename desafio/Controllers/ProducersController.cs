using desafio.Models;
using desafio.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace desafio.Controllers
{
    public class ProducersController : Controller
    {
        private readonly IProducerRepository _producerRepository;
        public ProducersController(IProducerRepository producerRepository)
        {
            _producerRepository = producerRepository;
        }

        public IActionResult Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            int userId = userIdClaim != null && int.TryParse(userIdClaim.Value, out int result) ? result : 0;

            List<ProducerModel> producers = _producerRepository.GetProducers(userId);
            return View(producers);
        }

        public IActionResult FormAdd()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddProducer(ProducerModel producer)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                int userId = userIdClaim != null && int.TryParse(userIdClaim.Value, out int result) ? result : 0;

                string dataValidValue = HttpContext.Request.Form["dataValid"]; 

                if (dataValidValue == "true")
                {
                    _producerRepository.Add(userId, producer);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["MessageError"] = "CPF inválido. Por favor, insira um CPF válido.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["MessageError"] = $"Erro ao adicionar produtor. {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult UpdateProducer(ProducerModel producer)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                int userId = userIdClaim != null && int.TryParse(userIdClaim.Value, out int result) ? result : 0;

                string dataValidValue = HttpContext.Request.Form["dataValid"];

                if (dataValidValue == "true")
                {
                    _producerRepository.UpdateProducer(userId, producer);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["MessageError"] = "CPF inválido. Por favor, insira um CPF válido.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["MessageError"] = $"Erro ao atualizar produtor. {ex.Message}";
                return RedirectToAction("Index");
            }
        }

    }
}
