using AutoMapper;
using QRDashboard.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using QRDashboard.Domain.Entities;
using QRDashboard.Domain.Interfaces;
using QRDashboard.Domain.Dtos.Response;

namespace QRDashboard.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IMapper _mapper;
        public CategoriaController(ICategoriaService categoriaService, IMapper mapper)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        //SELECT INPUT JQUERY
        [HttpGet]
        public async Task<IActionResult> ListaCategorias()
        {
            List<DtoCategoria> dtoListaCategoria = _mapper.Map<List<DtoCategoria>>(await _categoriaService.Lista());
            return StatusCode(StatusCodes.Status200OK, dtoListaCategoria);
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
                gResponse.Object = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status201Created, gResponse);
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
                gResponse.Object = modelo;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Message = ex.Message;
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
                gResponse.Message = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}