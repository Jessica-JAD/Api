using AutoMapper;
using Contracts;
using Entities;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTApi.Controllers
{
    [Route("api/Secciones")]
    [ApiController]
    public class SeccionesController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public SeccionesController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener listado de secciones
        /// </summary>
        /// <remarks>
        /// Se permite aplicar filtrados; los filtros aplicados a CODIGO y DESCRIPCION buscan cualquier resultado que contenga la cadena.
        /// </remarks>
        [Authorize]
        [HttpGet]
        public IActionResult GetAll([FromQuery] SeccionFiltering parametros)
        {
            try
            {
                _logger.LogInfo("Obteniendo secciones según filtro aplicado");

                var secciones = _repository.Secciones.GetAll(parametros);

                _logger.LogInfo($"Retornadas {secciones.Count()} secciones de la base de datos.");

                var seccionesResult = _mapper.Map<IEnumerable<SeccionDTO>>(secciones);

                var cant = seccionesResult.Count();
                Response.Headers.Add("Registros ", JsonConvert.SerializeObject(cant));

                return Ok(seccionesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en acción 'Devolver Secciones': {ex.Message}");
                return StatusCode(500, "Error interno en el servidor API.");
            }
        }
    }
}
