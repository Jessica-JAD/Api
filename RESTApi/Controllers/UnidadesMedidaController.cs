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
    [Route("api/UMs")]
    [ApiController]
    public class UnidadesMedidaController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public UnidadesMedidaController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener listado de unidades de medida
        /// </summary>
        /// <remarks>
        /// Se permite aplicar filtrados; los filtros aplicados a CODIGO y DESCRIPCION buscan cualquier resultado que contenga la cadena.
        /// </remarks>
        [Authorize]
        [HttpGet]
        public IActionResult GetAll([FromQuery] UnidadMedidaFiltering parametros)
        {
            try
            {
                _logger.LogInfo("Obteniendo unidades de medida según filtro aplicado");

                var unidadesmedida = _repository.UnidadesMedida.GetAll(parametros);

                _logger.LogInfo($"Retornadas {unidadesmedida.Count()} unidades de medida de la base de datos.");

                var unidadesmedidaResult = _mapper.Map<IEnumerable<UnidadMedidaDTO>>(unidadesmedida);

                var cant = unidadesmedidaResult.Count();
                Response.Headers.Add("Registros ", JsonConvert.SerializeObject(cant));

                return Ok(unidadesmedidaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en acción 'Devolver Unidades de Medida': {ex.Message}");
                return StatusCode(500, "Error interno en el servidor API.");
            }
        }

        /// <summary>
        /// Obtener listado de unidades de medida con sus conversiones
        /// </summary>
        /// <remarks>
        /// Se permite aplicar filtrados; los filtros aplicados a CODIGO y DESCRIPCION buscan cualquier resultado que contenga la cadena.
        /// </remarks>
        [Authorize]
        [HttpGet("Conversiones")]
        public IActionResult GetAllWithDetails([FromQuery] UnidadMedidaFiltering parametros)
        {
            try
            {
                _logger.LogInfo("Obteniendo unidades de medida con sus conversiones según filtro aplicado");

                var unidadesmedida = _repository.UnidadesMedida.GetAllWithDetails(parametros);

                _logger.LogInfo($"Retornadas {unidadesmedida.Count()} unidades de medida con sus conversiones de la base de datos.");

                var unidadesmedidaResult = _mapper.Map<IEnumerable<UnidadMedidaConversionesDTO>>(unidadesmedida);

                var cant = unidadesmedidaResult.Count();
                Response.Headers.Add("Registros ", JsonConvert.SerializeObject(cant));

                return Ok(unidadesmedidaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en acción 'Devolver Unidades de Medida con sus conversiones': {ex.Message}");
                return StatusCode(500, "Error interno en el servidor API.");
            }
        }
    }
}
