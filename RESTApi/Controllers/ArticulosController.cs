using AutoMapper;
using Contracts;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RESTApi.Controllers
{
    [Route("api/Articulos")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly RepositoryContext _context;
        private readonly IMapper _mapper;
        private IRepositoryWrapper _repository;
        private ILoggerManager _logger;

        public ArticulosController(IMapper mapper, ILoggerManager logger, IRepositoryWrapper repository, RepositoryContext context)
        {
            _context = context;
            _mapper = mapper;
            _repository = repository;
            _logger = logger;

        }

        /// <summary>
        /// Obtener listado de artículos
        /// </summary>
        /// <remarks>
        /// Se permite aplicar filtrados; los filtros aplicados a CODIGO y DESCRIPCION buscan cualquier resultado que contenga la cadena.
        /// </remarks>

        [Authorize]
        [HttpGet]
        public IActionResult GetAll([FromQuery] ArticulosFiltering parametros)
        {
            if (!((parametros.IdAlmacen > 0) && (parametros.IdSeccion > 0)))
            {
                return BadRequest("Debe especificar valores válidos para el Almacén y la Sección");
            }

            try
            {
                _logger.LogInfo("Obteniendo articulos según filtro aplicado");             
                
                var articulos = _repository.Articulos.GetAll(parametros);
               
                var metadata = new
                {
                    articulos.TotalCount,
                    articulos.PageSize,
                    articulos.CurrentPage,
                    articulos.TotalPages,
                    articulos.HasNext,
                    articulos.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                Response.Headers.Add("Registros ", JsonConvert.SerializeObject(metadata.TotalCount));

                return Ok(articulos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en acción 'Devolver Articulos': {ex.Message}");
                return StatusCode(500, "Error interno en el servidor API.");
            }
        }

        /// <summary>
        /// Obtener detalles de artículos
        /// </summary>
        /// <remarks>
        /// Se permite aplicar filtrados; a partir de un Almacen y una Seccion.
        /// </remarks>
        [Authorize]
        [HttpGet("Detalles")]
        public IActionResult GetDetalles([FromQuery] ArticulosFiltering parametros)
        {
            if (!((parametros.IdAlmacen > 0) && (parametros.IdSeccion > 0)))
            {
                return BadRequest("Debe especificar valores válidos para el Almacén y la Sección");
            }

            try
            {
                _logger.LogInfo("Obteniendo articulos según filtro aplicado");

                var articulos = _repository.Articulos.GetAllDetails(parametros);

                var metadata = new
                {
                    articulos.TotalCount,
                    articulos.PageSize,
                    articulos.CurrentPage,
                    articulos.TotalPages,
                    articulos.HasNext,
                    articulos.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                Response.Headers.Add("Registros ", JsonConvert.SerializeObject(metadata.TotalCount));

                return Ok(articulos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en acción 'Devolver Articulos': {ex.Message}");
                return StatusCode(500, "Error interno en el servidor API.");
            }
        }
    }
}
