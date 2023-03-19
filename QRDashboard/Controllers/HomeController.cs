using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QRDashboard.Domain.Dtos;
using QRDashboard.Domain.Dtos.Response;
using QRDashboard.Domain.Entities;
using QRDashboard.Domain.Interfaces;

namespace QRDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;

        public HomeController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }
        
        public IActionResult Index()
        {
            var sesion = HttpContext.Session.GetInt32("Sesion");
            if(sesion == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPerfil()
        {
            GenericResponse<DtoUsuario> response = new GenericResponse<DtoUsuario>();
            try
            {
                int? idSesion = HttpContext.Session.GetInt32("Sesion");
                DtoUsuario usuario = _mapper.Map<DtoUsuario>(await _usuarioService.GetById(idSesion.Value));

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

        [HttpPost]
        public async Task<IActionResult> GuardarPerfil([FromBody] DtoUsuario modelo)
        {
            GenericResponse<DtoUsuario> response = new GenericResponse<DtoUsuario>();
            try
            {
                int? idSesion = HttpContext.Session.GetInt32("Sesion");
                Usuario entity = _mapper.Map<Usuario>(modelo);

                entity.IdUser = idSesion.Value;
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

        public IActionResult Privacy()
        {
            return View();
        }
    }
}