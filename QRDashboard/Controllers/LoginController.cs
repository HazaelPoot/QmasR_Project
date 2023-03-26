using QRDashboard.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using QRDashboard.Domain.Entities;
using QRDashboard.Domain.Interfaces;

namespace QRDashboard.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public LoginController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        public IActionResult Index()
        {
            var sesion = HttpContext.Session.GetInt32("Sesion");
            if(sesion != null)
                return RedirectToAction("Index", "Home");

            return View();        
        }

        public async Task<IActionResult> Acceso(DtoLogin dto)
        {
            Usuario user_econtrado = await _usuarioService.Autenthication(dto.Username, dto.Passw);

            if(user_econtrado != null)
            {
                HttpContext.Session.SetInt32("Sesion", user_econtrado.IdUser);
                HttpContext.Session.SetString("Nombres", user_econtrado.Nombre);
                HttpContext.Session.SetString("Username", user_econtrado.Username);
                HttpContext.Session.SetString("Imagen", user_econtrado.UrlImagen);
                HttpContext.Session.SetInt32("TypeUser", user_econtrado.AdminType);
            }
            else
            {
                ViewData["Message"] = "Usuario o Contraseña Incorrectos";
                return View("Index");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
