using Microsoft.AspNetCore.Mvc;

namespace QRDashboard.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // var sesion = HttpContext.Session.GetInt32("Sesion");
            // if(sesion == null)
            //     return RedirectToAction("Index", "Login");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}