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
    public class ProyectoController : Controller
    {
        private readonly IProyectoService _proyectoService;
        private readonly IMapper _mapper;
        
        public ProyectoController(IProyectoService proyectoService, IMapper mapper)
        {
            _proyectoService = proyectoService;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> Index()
        {
            List<DtoProyecto> dtoProyectoLista = _mapper.Map<List<DtoProyecto>>(await _proyectoService.Lista());
            
            var sesion = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(sesion == null)
                return RedirectToAction("Index", "Login");

            return View(dtoProyectoLista);
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<DtoProyecto> dtoProyectoLista = _mapper.Map<List<DtoProyecto>>(await _proyectoService.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = dtoProyectoLista });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FotosProyecto>>> ListActivate()
        {
            IEnumerable<DtoProyecto> dtoProyectoLista = _mapper.Map<IEnumerable<DtoProyecto>>(await _proyectoService.ListActivate());
            return StatusCode(StatusCodes.Status200OK, new {data = dtoProyectoLista});
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            DtoProyecto dtoProyecto = _mapper.Map<DtoProyecto>(await _proyectoService.GetById(id));
            return StatusCode(StatusCodes.Status200OK, dtoProyecto);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<DtoProyecto> gResponse = new GenericResponse<DtoProyecto>();

            try
            {
                DtoProyecto dtoProyecto = JsonConvert.DeserializeObject<DtoProyecto>(modelo);
                string nombreFoto = "";
                Stream fotoStream = null;

                if (foto != null)
                {
                    var fotoName = dtoProyecto.Titulo;
                    string extension = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(fotoName, extension);
                    fotoStream = foto.OpenReadStream();
                }

                ProyectoQr ProyectoCreado = await _proyectoService.Crear(_mapper.Map<ProyectoQr>(dtoProyecto), fotoStream, "Fotos_Proyecto", nombreFoto);

                dtoProyecto = _mapper.Map<DtoProyecto>(ProyectoCreado);
                gResponse.Status = true;
                gResponse.Object = dtoProyecto;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status201Created, gResponse);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Editar([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<DtoProyecto> gResponse = new GenericResponse<DtoProyecto>();

            try
            {
                DtoProyecto dtoProyecto = JsonConvert.DeserializeObject<DtoProyecto>(modelo);
                string nombreFoto = "";
                Stream fotoStream = null;

                if (foto != null)
                {
                    var fotoName = dtoProyecto.Titulo;
                    string extension = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(fotoName, extension);
                    fotoStream = foto.OpenReadStream();
                }

                ProyectoQr proyectoEditado = await _proyectoService.Editar(_mapper.Map<ProyectoQr>(dtoProyecto), fotoStream, "Fotos_Proyecto", nombreFoto);

                dtoProyecto = _mapper.Map<DtoProyecto>(proyectoEditado);
                gResponse.Status = true;
                gResponse.Object = dtoProyecto;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int idProj)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Status = await _proyectoService.Eliminar(idProj);
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
