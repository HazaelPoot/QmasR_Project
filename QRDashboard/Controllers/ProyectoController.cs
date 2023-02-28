using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRDashboard.Domain.Dtos.Response;
using QRDashboard.Domain.Entities;
using QRDashboard.Domain.Interfaces;
using QRDashboard.Models;

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
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<VMProyecto> vmProyectoLista = _mapper.Map<List<VMProyecto>>(await _proyectoService.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = vmProyectoLista });
        }

        [HttpGet]
        public async Task<IActionResult> ListaCategorias()
        {
            List<VMCategoria> vmListaRoles = _mapper.Map<List<VMCategoria>>(await _categoriaService.Lista());
            return StatusCode(StatusCodes.Status200OK, vmListaRoles);
        }
    }
}
