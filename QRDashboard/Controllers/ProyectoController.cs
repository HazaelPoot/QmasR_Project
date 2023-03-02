﻿using AutoMapper;
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
        public async Task<IActionResult> Index()
        {
            List<VMProyecto> vmProyectoLista = _mapper.Map<List<VMProyecto>>(await _proyectoService.Lista());
            return View(vmProyectoLista);
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

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            VMProyecto vmProyecto = _mapper.Map<VMProyecto>(await _proyectoService.ObtenerPorId(id));
            return StatusCode(StatusCodes.Status200OK, vmProyecto);
        }

        //a este hay que darle cuello
        public async Task<IActionResult> EditModal(int id)
        {
            VMProyecto vmProyecto = _mapper.Map<VMProyecto>(await _proyectoService.ObtenerPorId(id));
            return PartialView("_EditModal", vmProyecto);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<VMProyecto> gResponse = new GenericResponse<VMProyecto>();

            try
            {
                VMProyecto vMProyecto = JsonConvert.DeserializeObject<VMProyecto>(modelo);
                string nombreFoto = "";
                Stream fotoStream = null;

                if (foto != null)
                {
                    string nombre_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(nombre_codigo, extension);
                    fotoStream = foto.OpenReadStream();
                }

                ProyectoQr ProyectoCreado = await _proyectoService.Crear(_mapper.Map<ProyectoQr>(vMProyecto), fotoStream, "Fotos_Proyecto", nombreFoto);

                vMProyecto = _mapper.Map<VMProyecto>(ProyectoCreado);
                gResponse.Status = true;
                gResponse.Obejct = vMProyecto;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Mesaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromForm] IFormFile foto, [FromForm] string modelo)
        {
            GenericResponse<VMProyecto> gResponse = new GenericResponse<VMProyecto>();

            try
            {
                VMProyecto vMProyecto = JsonConvert.DeserializeObject<VMProyecto>(modelo);
                string nombreFoto = "";
                Stream fotoStream = null;

                if (foto != null)
                {
                    string nombre_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(nombre_codigo, extension);
                    fotoStream = foto.OpenReadStream();
                }

                ProyectoQr proyectoEditado = await _proyectoService.Editar(_mapper.Map<ProyectoQr>(vMProyecto), fotoStream, "Fotos_Proyecto", nombreFoto);

                vMProyecto = _mapper.Map<VMProyecto>(proyectoEditado);
                gResponse.Status = true;
                gResponse.Obejct = vMProyecto;
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
