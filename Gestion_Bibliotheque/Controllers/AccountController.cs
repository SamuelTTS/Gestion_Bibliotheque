using Gestion_Bibliotheque.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestion_Bibliotheque.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext context;
        public AccountController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // On cherche l'utilisateur dans la base MySQL (Docker)
            var user = context.Users.FirstOrDefault(u => u.email == email && u.password == password);

            if (user != null)
            {
                // On enregistre ses infos dans la session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.name);
                HttpContext.Session.SetString("UserEmail", user.email);
                HttpContext.Session.SetString("UserRole", user.role);

                return RedirectToAction("Index", "Livres");
            }

            ViewBag.Error = "Email ou mot de passe incorrect";
            return View();
        }
              
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
