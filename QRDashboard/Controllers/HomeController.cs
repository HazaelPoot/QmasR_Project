using AutoMapper;
using System.Security.Claims;
using QRDashboard.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using QRDashboard.Domain.Entities;
using QRDashboard.Domain.Interfaces;
using QRDashboard.Domain.Dtos.Response;
using Microsoft.AspNetCore.Authorization;

namespace QRDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public HomeController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }
        
        public IActionResult Index()
        {
            var sesion = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(sesion == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ObtenerPerfil()
        {
            GenericResponse<DtoUsuario> response = new GenericResponse<DtoUsuario>();
            try
            {
                var idUser = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                DtoUsuario usuario = _mapper.Map<DtoUsuario>(await _usuarioService.GetById(idUser));

                response.Status = true;
                response.Object = usuario;
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GuardarPerfil([FromBody] DtoUsuario modelo)
        {
            GenericResponse<DtoUsuario> response = new GenericResponse<DtoUsuario>();
            try
            {
                var idSesion = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                Usuario entity = _mapper.Map<Usuario>(modelo);
                entity.IdUser = idSesion;
                
                bool result = await _usuarioService.GuardarPerfil(entity);

                response.Status = result;
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        public async Task<IActionResult> CambiarClave([FromBody] DtoPassword dto)
        {
            GenericResponse<bool> response = new GenericResponse<bool>();
            try
            {
                var idSesion = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));        
                bool result = await _usuarioService.CambiarClave(idSesion, dto.claveActual, dto.claveNueva);

                response.Status = result;
            }
            catch(Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}