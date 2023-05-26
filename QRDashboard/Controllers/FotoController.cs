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
    public class FotoController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFotoService _fotoService;
        private readonly IProyectoService _proyectoService;

        public FotoController(IFotoService fotoService, IMapper mapper, IProyectoService proyectoService)
        {
            _proyectoService = proyectoService;
            _fotoService = fotoService;
            _mapper = mapper;
        }
        public async Task<ActionResult<IEnumerable<FotosProyecto>>> Index(int idProj)
        {
            var sesion = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(sesion == null)
                return RedirectToAction("Index", "Login");
                
            IEnumerable<DtoFotosProyecto> dtoFotoProj = _mapper.Map<IEnumerable<DtoFotosProyecto>>(await _fotoService.ListByProject(idProj));
            ProyectoQr search = await _proyectoService.GetById(idProj);
            ViewData["TituloProject"] = search.Titulo;
            TempData["IdProj"] = idProj;

            return View(dtoFotoProj);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FotosProyecto>>> ListByProject(int idProj)
        {
            ProyectoQr search = await _proyectoService.GetById(idProj);
            TempData["IdProj"] = idProj;
            if(search == null)
                return BadRequest("El proyecto no existe");

            IEnumerable<DtoFotosProyecto> dtoFotoProj = _mapper.Map<IEnumerable<DtoFotosProyecto>>(await _fotoService.ListByProject(idProj));
            return StatusCode(StatusCodes.Status200OK, new {data = dtoFotoProj});
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<DtoFotosProyecto> dtoFotoProj = _mapper.Map<List<DtoFotosProyecto>>(await _fotoService.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = dtoFotoProj });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            DtoFotosProyecto dtoFotoProj = _mapper.Map<DtoFotosProyecto>(await _fotoService.GetById(id));
            return StatusCode(StatusCodes.Status200OK, dtoFotoProj);
        }

        [Authorize]
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

        [Authorize]
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