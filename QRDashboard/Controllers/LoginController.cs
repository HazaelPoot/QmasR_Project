using AutoMapper;
using QRDashboard.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using QRDashboard.Domain.Entities;
using QRDashboard.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace QRDashboard.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public LoginController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var sesion = HttpContext.Session.GetInt32("Sesion");
            if(sesion != null)
                return RedirectToAction("Index", "Home");

            return View();        
        }

        [HttpPost]
        public async Task<IActionResult> Acceso(DtoLogin dto)
        {
            Usuario auth = await _usuarioService.Autenthication(dto.Username, dto.Passw);

            if(auth != null)
            {
                HttpContext.Session.SetInt32("Sesion", auth.IdUser);
                HttpContext.Session.SetString("Nombres", auth.Nombre);
                HttpContext.Session.SetString("Username", auth.Username);
                HttpContext.Session.SetString("Imagen", auth.UrlImagen);
                HttpContext.Session.SetInt32("TypeUser", auth.AdminType);
            }
            else
            {
                ViewData["Message"] = "Usuario o Contraseña Incorrectos";
                return View("Index");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ApiAcceso([FromBody] DtoLogin dto)
        {
            Usuario auth = await _usuarioService.Autenthication(dto.Username, dto.Passw);
            

            if(auth != null)
            {  
                HttpContext.Session.SetInt32("Sesion", auth.IdUser);
                HttpContext.Session.SetString("Nombres", auth.Nombre);
                HttpContext.Session.SetString("Username", auth.Username);
                HttpContext.Session.SetString("Imagen", auth.UrlImagen);
                HttpContext.Session.SetInt32("TypeUser", auth.AdminType);
                
                DtoUsuario dtoUsuario = _mapper.Map<DtoUsuario>(await _usuarioService.GetById(auth.IdUser));
                return StatusCode(StatusCodes.Status200OK, dtoUsuario);
            }

            return StatusCode(StatusCodes.Status404NotFound, new {message = "Username o Contraseña Incorrectos"});
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}