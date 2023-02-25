using Microsoft.AspNetCore.Mvc;

namespace QRDashboard.Controllers
{
    public class ProyectoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
