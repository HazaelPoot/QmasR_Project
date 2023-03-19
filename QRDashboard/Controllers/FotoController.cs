using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRDashboard.Domain.Dtos;
using QRDashboard.Domain.Dtos.Response;
using QRDashboard.Domain.Entities;
using QRDashboard.Domain.Interfaces;

namespace QRDashboard.Controllers
{
    public class FotoController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFotoService _fotoService;
        private readonly IProyectoService _proyectoService;

        public FotoController(IFotoService fotoService, IMapper mapper, IProyectoService proyectoService)
        {
            _fotoService = fotoService;
            _proyectoService = proyectoService;
            _mapper = mapper;
        }
        public async Task<ActionResult<IEnumerable<FotosProyecto>>> Index(int idProj)
        {
            IEnumerable<DtoFotosProyecto> dtoFotoProj = _mapper.Map<IEnumerable<DtoFotosProyecto>>(await _fotoService.ListByProject(idProj));
            ProyectoQr search = await _proyectoService.GetById(idProj);
            ViewData["TituloProject"] = search.Titulo;
            TempData["IdProj"] = idProj;

            var sesion = HttpContext.Session.GetInt32("Sesion");
            if(sesion == null)
                return RedirectToAction("Index", "Login");

            return View(dtoFotoProj);
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<DtoFotosProyecto> dtoFotoProj = _mapper.Map<List<DtoFotosProyecto>>(await _fotoService.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = dtoFotoProj });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FotosProyecto>>> ListByProject(int idProj)
        {
            ProyectoQr search = await _proyectoService.GetById(idProj);
            if(search == null)
                return BadRequest("El proyecto no existe");

            IEnumerable<DtoFotosProyecto> dtoFotoProj = _mapper.Map<IEnumerable<DtoFotosProyecto>>(await _fotoService.ListByProject(idProj));
            return StatusCode(StatusCodes.Status200OK, dtoFotoProj);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            DtoFotosProyecto dtoFotoProj = _mapper.Map<DtoFotosProyecto>(await _fotoService.GetById(id));
            return StatusCode(StatusCodes.Status200OK, dtoFotoProj);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<DtoFotosProyecto> gResponse = new GenericResponse<DtoFotosProyecto>();

            try
            {
                DtoFotosProyecto dtoFotoProj = JsonConvert.DeserializeObject<DtoFotosProyecto>(modelo);
                string nombreFoto = "";
                Stream fotoStream = null;
                int? idProj = (int?)TempData["IdProj"];

                if (foto != null)
                {
                    // var fotoName = dtoFotoProj.Titulo;
                    string nombre_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(nombre_codigo, extension);
                    fotoStream = foto.OpenReadStream();
                }

                FotosProyecto FotoCreado = await _fotoService.Crear(_mapper.Map<FotosProyecto>(dtoFotoProj), idProj, fotoStream, "Fotos_Imagen", nombreFoto);

                dtoFotoProj = _mapper.Map<DtoFotosProyecto>(FotoCreado);
                gResponse.Status = true;
                gResponse.Object = dtoFotoProj;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status201Created, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int idImage)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Status = await _fotoService.Eliminar(idImage);
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