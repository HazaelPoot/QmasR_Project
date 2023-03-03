﻿using AutoMapper;
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
            return View(dtoProyectoLista);
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<DtoProyecto> dtoProyectoLista = _mapper.Map<List<DtoProyecto>>(await _proyectoService.Lista());
            return StatusCode(StatusCodes.Status200OK, new { data = dtoProyectoLista });
        }

        //ESTE EL GET EN ARRAY, SOLO DESCOMENTALO Y COMENTA EL DE ARRIBA PARA QUE NO TE DE ERROR DE METODO AMBIGUO
        // [HttpGet]
        // public async Task<IActionResult> Lista()
        // {
        //     List<VMProyecto> vmProyectoLista = _mapper.Map<List<VMProyecto>>(await _proyectoService.Lista());
        //     return StatusCode(StatusCodes.Status200OK, vmProyectoLista);
        // }

        [HttpGet]
        public async Task<IActionResult> ListaCategorias()
        {
            List<DtoCategoria> dtoListaRoles = _mapper.Map<List<DtoCategoria>>(await _categoriaService.Lista());
            return StatusCode(StatusCodes.Status200OK, dtoListaRoles);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            DtoProyecto dtoProyecto = _mapper.Map<DtoProyecto>(await _proyectoService.ObtenerPorId(id));
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
                    string nombre_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(nombre_codigo, extension);
                    fotoStream = foto.OpenReadStream();
                }

                ProyectoQr ProyectoCreado = await _proyectoService.Crear(_mapper.Map<ProyectoQr>(dtoProyecto), fotoStream, "Fotos_Proyecto", nombreFoto);

                dtoProyecto = _mapper.Map<DtoProyecto>(ProyectoCreado);
                gResponse.Status = true;
                gResponse.Obejct = dtoProyecto;
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
            GenericResponse<DtoProyecto> gResponse = new GenericResponse<DtoProyecto>();

            try
            {
                DtoProyecto dtoProyecto = JsonConvert.DeserializeObject<DtoProyecto>(modelo);
                string nombreFoto = "";
                Stream fotoStream = null;

                if (foto != null)
                {
                    string nombre_codigo = Guid.NewGuid().ToString("N");
                    string extension = Path.GetExtension(foto.FileName);
                    nombreFoto = string.Concat(nombre_codigo, extension);
                    fotoStream = foto.OpenReadStream();
                }

                ProyectoQr proyectoEditado = await _proyectoService.Editar(_mapper.Map<ProyectoQr>(dtoProyecto), fotoStream, "Fotos_Proyecto", nombreFoto);

                dtoProyecto = _mapper.Map<DtoProyecto>(proyectoEditado);
                gResponse.Status = true;
                gResponse.Obejct = dtoProyecto;
            }
            catch (Exception ex)
            {
                gResponse.Status = false;
                gResponse.Mesaje = ex.Message;
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
                gResponse.Mesaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }
    }
}
