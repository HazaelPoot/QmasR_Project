using AutoMapper;
using System.Security.Claims;
using QRDashboard.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using QRDashboard.Domain.Entities;
using QRDashboard.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace QRDashboard.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAccesoService _accesoService;
        private readonly IMapper _mapper;

        public LoginController(IAccesoService accesoService, IMapper mapper)
        {
            _accesoService = accesoService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var sesion = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(sesion != null)
                return RedirectToAction("Index", "Home");

            return View();        
        }

        [HttpPost]
        public async Task<IActionResult> Acceso(DtoLogin dto)
        {
            try
            {
                Usuario auth = await _accesoService.Authentication(dto.Username, dto.Passw);
                DtoUsuario dtoUsuario = _mapper.Map<DtoUsuario>(await _accesoService.GetUserAuth(auth.IdUser));
                await _accesoService.GenerateClaims(dtoUsuario);
                ViewData["Message"] = null;

                return RedirectToAction("Index", "Home");
            }
            catch(Exception msg)
            {
                ViewData["Message"] = msg.Message;
                return View("Index");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public async Task<IActionResult> ApiAcceso([FromBody] DtoLogin dto)
        {
            try
            {
                Usuario auth = await _accesoService.Authentication(dto.Username, dto.Passw);
                DtoUsuario dtoUsuario = _mapper.Map<DtoUsuario>(await _accesoService.GetUserAuth(auth.IdUser));
                await _accesoService.GenerateClaims(dtoUsuario);

                return StatusCode(StatusCodes.Status200OK, dtoUsuario);
            }
            catch(Exception msg)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { servermessage = msg.Message});
            }
        }

        public async Task<IActionResult> ApiLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return StatusCode(StatusCodes.Status200OK, new {message = "Session Cerrada"});
        }
    }
}