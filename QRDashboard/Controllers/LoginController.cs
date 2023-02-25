using Microsoft.AspNetCore.Mvc;

namespace QRDashboard.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
