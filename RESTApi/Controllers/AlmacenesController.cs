using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.DTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace RESTApi.Controllers
{
    //[Authorize]
    [Route("api/Almacenes")]
    [ApiController]
    public class AlmacenesController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public AlmacenesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener listado de almacenes
        /// </summary>
        /// <remarks>
        /// Se permite aplicar filtrados; los filtros aplicados a CODIGO y DESCRIPCION buscan cualquier resultado que contenga la cadena.
        /// </remarks>
        [Authorize]
        [HttpGet]
        public IActionResult GetAll([FromQuery] AlmacenFiltering parametros)
        {
            try
            {
                _logger.LogInfo("Obteniendo almacenes según filtro aplicado");

                var almacenes = _repository.Almacenes.GetAll(parametros);

                _logger.LogInfo($"Retornados {almacenes.Count()} almacenes de la base de datos.");

                var almacenesResult = _mapper.Map<IEnumerable<AlmacenDTO>>(almacenes);

                var cant = almacenesResult.Count();
                Response.Headers.Add("Registros ", JsonConvert.SerializeObject(cant));

                return Ok(almacenesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en acción 'Devolver Almacenes': {ex.Message}");
                return StatusCode(500, "Error interno en el servidor API.");
            }
        }

        /// <summary>
        /// Obtener listado de almacenes con sus secciones
        /// </summary>
        /// <remarks>
        /// Se permite aplicar filtros; los filtros aplicados a CODIGO y DESCRIPCION buscan cualquier resultado que contenga la cadena.
        /// </remarks>
        [Authorize]
        [HttpGet("Secciones")]
        public IActionResult GetAllWithDetails([FromQuery] AlmacenFiltering parametros)
        {
            try
            {
                _logger.LogInfo("Obteniendo almacenes según filtro aplicado");

                var almacenes = _repository.Almacenes.GetAllWithDetails(parametros);

                _logger.LogInfo($"Retornados {almacenes.Count()} almacenes de la base de datos.");

                var almacenesResult = _mapper.Map<IEnumerable<AlmacenSeccionesDTO>>(almacenes);

                var cant = almacenesResult.Count();
                Response.Headers.Add("Registros ", JsonConvert.SerializeObject(cant));

                return Ok(almacenesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en acción 'Devolver Almacenes': {ex.Message}");
                return StatusCode(500, "Error interno en el servidor API.");
            }
        }
    }
}
