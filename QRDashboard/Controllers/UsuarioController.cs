using AutoMapper;
using Newtonsoft.Json;
using System.Security.Claims;
using QRDashboard.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using QRDashboard.Domain.Entities;
using QRDashboard.Domain.Interfaces;
using QRDashboard.Domain.Dtos.Response;
using Microsoft.AspNetCore.Authorization;

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
        
        //SELECT INPUT JQUERY
        [HttpGet]
        public async Task<IActionResult> ListaRoles()
        {
            List<DtoRol> dtoListaRoles = _mapper.Map<List<DtoRol>>(await _rolService.Lista());
            return StatusCode(StatusCodes.Status200OK, dtoListaRoles);
        }

        public IActionResult Index()
        {
            var session = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var typeUser = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Role));

            if(session == null)
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
        // [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Lista()
        {
            List<DtoUsuario> dtoUsuarioLista = _mapper.Map<List<DtoUsuario>>(await _usuarioService.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = dtoUsuarioLista });
        }

        [HttpGet]
        // [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            DtoUsuario dtoUsuario = _mapper.Map<DtoUsuario>(await _usuarioService.GetById(id));
            if(dtoUsuario is null)
                return BadRequest($"El Usuario {id} no existe");

            return StatusCode(StatusCodes.Status200OK, dtoUsuario);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
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

        [Authorize(Policy = "Admin")]
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

        [Authorize(Policy = "Admin")]
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