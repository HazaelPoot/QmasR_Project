using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRDashboard.Domain.Dtos;
using QRDashboard.Domain.Dtos.Response;
using QRDashboard.Domain.Entities;
using QRDashboard.Domain.Interfaces;

namespace QRDashboard.Controllers
{
    public class ProyectoController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProyectoService _proyectoService;
        private readonly ICategoriaService _categoriaService;

        public ProyectoController(IProyectoService proyectoService, ICategoriaService categoriaService, IMapper mapper)
        {
            _proyectoService = proyectoService;
            _categoriaService = categoriaService;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> Index()
        {
            List<DtoProyecto> dtoProyectoLista = _mapper.Map<List<DtoProyecto>>(await _proyectoService.Lista());
            
            var sesion = HttpContext.Session.GetInt32("Sesion");
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
        public async Task<IActionResult> GetById(int id)
        {
            DtoProyecto dtoProyecto = _mapper.Map<DtoProyecto>(await _proyectoService.GetById(id));
            return StatusCode(StatusCodes.Status200OK, dtoProyecto);
        }

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
                    //string nombre_codigo = Guid.NewGuid().ToString("N");
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
                    //string nombre_codigo = Guid.NewGuid().ToString("N");
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
