using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRDashboard.Domain.Dtos;
using QRDashboard.Domain.Dtos.Response;
using QRDashboard.Domain.Entities;
using QRDashboard.Domain.Interfaces;

namespace QRDashboard.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;

        public UsuarioController(IUsuarioService usuarioService, IRolService rolService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _rolService = rolService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var sesion = HttpContext.Session.GetInt32("Sesion");
            var typeUser = HttpContext.Session.GetInt32("TypeUser");
            if(sesion == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else if(typeUser != 1)
            {
                ViewData["Unauthorizate"] = "No tienes permisos para acceder a esta pantalla, solo los Administradores pueden";
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<DtoUsuario> dtoUsuarioLista = _mapper.Map<List<DtoUsuario>>(await _usuarioService.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = dtoUsuarioLista });
        }

        //SELECT INPUT JQUERY
        [HttpGet]
        public async Task<IActionResult> ListaRoles()
        {
            List<DtoRol> dtoListaRoles = _mapper.Map<List<DtoRol>>(await _rolService.Lista());
            return StatusCode(StatusCodes.Status200OK, dtoListaRoles);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<DtoUsuario> gResponse = new GenericResponse<DtoUsuario>();

            try
            {
                DtoUsuario dtoUsuario = JsonConvert.DeserializeObject<DtoUsuario>(modelo);
                string nombreFoto = "";
                Stream fotoStream = null;

                if (foto != null)
                {
                    var fotoName = dtoUsuario.Username;
                    //string nombre_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(fotoName, extension);
                    fotoStream = foto.OpenReadStream();
                }

                Usuario usuarioCreado = await _usuarioService.Crear(_mapper.Map<Usuario>(dtoUsuario), fotoStream, "Fotos_Perfil", nombreFoto);

                dtoUsuario = _mapper.Map<DtoUsuario>(usuarioCreado);
                gResponse.Status = true;
                gResponse.Object = dtoUsuario;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status201Created, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<DtoUsuario> gResponse = new GenericResponse<DtoUsuario>();

            try
            {
                DtoUsuario dtoUsuario = JsonConvert.DeserializeObject<DtoUsuario>(modelo);
                string nombreFoto = "";
                Stream fotoStream = null;

                if (foto != null)
                {
                    var fotoName = dtoUsuario.Username;
                    // string nombre_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(fotoName, extension);
                    fotoStream = foto.OpenReadStream();
                }

                Usuario usuarioEditado = await _usuarioService.Editar(_mapper.Map<Usuario>(dtoUsuario), fotoStream, "Fotos_Perfil", nombreFoto);

                dtoUsuario = _mapper.Map<DtoUsuario>(usuarioEditado);
                gResponse.Status = true;
                gResponse.Object = dtoUsuario;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int idUsuario)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Status = await _usuarioService.Eliminar(idUsuario);
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}