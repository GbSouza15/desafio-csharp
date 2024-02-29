using desafio.Models;
using desafio.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace desafio.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserRepository _userRepository;

        public RegisterController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(UserModel newUser)
        {
            try
            {
                _userRepository.CreateUser(newUser);
                return RedirectToAction("Index", "Login");
            } 
            catch (Exception ex)
            {
                TempData["MessageError"] = $"Erro ao registrar. {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
