using desafio.Models;
using desafio.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace desafio.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;

        public LoginController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(UserModel loginCredentials)
        {
            try
            {
                
                    UserModel user = _userRepository.GetUserByCredentials(loginCredentials.Email, loginCredentials.Password);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    };

                    var userIdentity = new ClaimsIdentity(claims, "login");

                    var userPrincipal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(userPrincipal);

                    return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["MessageError"] = $"Erro ao fazer o login. Verifique seu email e senha.";
                return RedirectToAction("Index");
            }
        }
    }
}
