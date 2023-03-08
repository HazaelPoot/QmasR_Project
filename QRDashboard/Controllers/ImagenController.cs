using Microsoft.AspNetCore.Mvc;

namespace QRDashboard.Controllers
{
    public class ImagenController : Controller
    {
        private readonly ILogger<ImagenController> _logger;

        public ImagenController(ILogger<ImagenController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}