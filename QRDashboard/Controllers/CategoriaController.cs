using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using QRDashboard.Domain.Dtos;
using QRDashboard.Domain.Dtos.Response;
using QRDashboard.Domain.Entities;
using QRDashboard.Domain.Interfaces;

namespace QRDashboard.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService, IMapper mapper)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<DtoCategoria> dtoCategoriaLista = _mapper.Map<List<DtoCategoria>>(await _categoriaService.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = dtoCategoriaLista });
        }

        //SELECT INPUT
        [HttpGet]
        public async Task<IActionResult> ListaCategorias()
        {
            List<DtoCategoria> dtoListaCategoria = _mapper.Map<List<DtoCategoria>>(await _categoriaService.Lista());
            return StatusCode(StatusCodes.Status200OK, dtoListaCategoria);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            DtoCategoria dtoCategoria = _mapper.Map<DtoCategoria>(await _categoriaService.GetById(id));
            return StatusCode(StatusCodes.Status200OK, dtoCategoria);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody]DtoCategoria modelo)
        {
            GenericResponse<DtoCategoria> gResponse = new GenericResponse<DtoCategoria>();

            try
            {
                Categorium categoryCreado = await _categoriaService.Crear(_mapper.Map<Categorium>(modelo));
                modelo = _mapper.Map<DtoCategoria>(categoryCreado);

                gResponse.Status = true;
                gResponse.Obejct = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Mesaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody]DtoCategoria modelo)
        {
            GenericResponse<DtoCategoria> gResponse = new GenericResponse<DtoCategoria>();

            try
            {
                Categorium category_editada = await _categoriaService.Editar(_mapper.Map<Categorium>(modelo));
                modelo = _mapper.Map<DtoCategoria>(category_editada);

                gResponse.Status = true;
                gResponse.Obejct = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Mesaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int idcategoria)
        {
            GenericResponse<string> gResponse = new GenericResponse<string>();

            try
            {
                gResponse.Status = await _categoriaService.Eliminar(idcategoria);
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Mesaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}