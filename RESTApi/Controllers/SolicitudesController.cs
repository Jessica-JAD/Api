using AutoMapper;
using Contracts;
using Entities;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RESTApi.Controllers
{
    [Route("api/Solicitudes")]
    [ApiController]
    public class SolicitudesController : ControllerBase
    {
        private readonly RepositoryContext _context;
        private readonly IMapper _mapper;
        private IRepositoryWrapper _repository;
        private ILoggerManager _logger;

        public SolicitudesController(IMapper mapper, ILoggerManager logger, IRepositoryWrapper repository, RepositoryContext context)
        {
            _context = context;
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Obtener listado de solicitudes
        /// </summary>
        /// <remarks>
        /// Se permite aplicar filtros; los filtros aplicados a CONSECUTIVO y OBSERVACIONES buscan cualquier resultado que contenga la cadena.
        /// </remarks>
        // GET: api/Solicitudes
        [Authorize]
        [HttpGet]
        public IActionResult GetAll([FromQuery] SolicitudFilteringPaging parametros)
        {
            try
            {
                _logger.LogInfo("Obteniendo solicitudes según filtro aplicado");
                var solicitudes = _repository.Solicitudes.GetAllPages(parametros);

                var metadata = new
                {
                    solicitudes.TotalCount,
                    solicitudes.PageSize,
                    solicitudes.CurrentPage,
                    solicitudes.TotalPages,
                    solicitudes.HasNext,
                    solicitudes.HasPrevious
                };

                _logger.LogInfo($"Retornadas {solicitudes.Count()} solicitudes de la base de datos.");

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var solicitudesResult = _mapper.Map<IEnumerable<SolicitudDTO>>(solicitudes);

                return Ok(solicitudesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en acción 'Devolver Solicitudes': {ex.Message}");
                return StatusCode(500, "Error interno en el servidor API.");
            }
        }

        /// <summary>
        /// Obtener datos de una solicitud
        /// </summary>
        /// <remarks>
        /// Debe especificarse el Id de la solicitud.
        /// </remarks>
        // GET: api/Solicitudes/5
        [Authorize]
        [HttpGet("{Id:int:min(1)}", Name = "SolicitudPorId")]
        public IActionResult GetById(int Id)
        {
            var solicitud = _repository.Solicitudes.GetById(Id);
            if (solicitud == null)
            {
                _logger.LogError($"Solicitud con id: {Id}, no se ha encontrado en la base de datos.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Solicitud devuelta con id: {Id}");
                var solicitudResult = _mapper.Map<SolicitudDTO>(solicitud);

                return Ok(solicitudResult);
            }
        }

        /// <summary>
        /// Obtener datos de una solicitud con sus lineas
        /// </summary>
        /// <remarks>
        /// Debe especificarse el Id de la solicitud.
        /// </remarks>
        // GET: api/Solicitudes/5
        [Authorize]
        [HttpGet("{Id:int:min(1)}/details", Name = "SolicitudPorIdLineas")]
        public IActionResult GetByIdWithLines(int Id)
        {
            var solicitud = _repository.Solicitudes.GetByIdWithLines(Id);
            if (solicitud == null)
            {
                _logger.LogError($"Solicitud con id: {Id}, no se ha encontrado en la base de datos.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Solicitud devuelta con id: {Id}");
                var solicitudResult = _mapper.Map<SolicitudLineasDTO>(solicitud);

                return Ok(solicitudResult);
            }
        }

        /// <summary>
        /// Crear una nueva solicitud con sus lineas
        /// </summary>
        /// <remarks>
        /// Debe recibir coo parámetro un objeto con los datos de la solicitud y sus lineas
        /// </remarks>
        // POST: api/Solicitudes
        [Authorize]
        [HttpPost]
        public IActionResult CreateSolicitud([FromBody] SolicitudNueva solicitud)
        {
            try
            {
                if (solicitud == null)
                {
                    _logger.LogError("El objeto Solicitud enviado desde el cliente es nulo.");
                    return BadRequest("Solicitud es nulo");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Objeto inválido enviado desde el cliente.");
                    return BadRequest("Modelo del objecto inválido ");
                }

                //context.Entry(yourObject).State = EntityState.Detached
                    
                var solicitudEntity = _mapper.Map<Solicitud>(solicitud);

                #region Completar los datos de la Cabecera
                // Id q identifica al tipo de operación Pedidos (ver tabla ALMTOPER) 
                int idTipoOperacion = 4;

                // Obtener AlmacenSeccion Origen, segun Ids del Almacen y la Seccion Origen
                var resSoli = _repository.AlmacenSecciones
                    .FindByCondition(ras => ras.AlmacenId.Equals(solicitudEntity.AlmacenSeccionSolicitante.AlmacenId)
                        && ras.SeccionId.Equals(solicitudEntity.AlmacenSeccionSolicitante.SeccionId)
                        && ras.Activo.Equals(true)
                        && (ras.IdTipoSeccion.Equals(ETiposSeccion.SAL) || ras.IdTipoSeccion.Equals(ETiposSeccion.SCD) || ras.IdTipoSeccion.Equals(ETiposSeccion.SGA)))
                    .Include(al => al.Almacen)
                    .Include(s => s.Seccion)
                    .AsNoTracking()
                    .FirstOrDefault();

                // Obtener AlmacenSeccion Destino, segun Ids del Almacen y la Seccion Destino
                var resDest = _repository.AlmacenSecciones
                    .FindByCondition(ras => ras.AlmacenId.Equals(solicitudEntity.AlmacenSeccionDestino.AlmacenId)
                        && ras.SeccionId.Equals(solicitudEntity.AlmacenSeccionDestino.SeccionId)
                        && ras.Activo.Equals(true)
                        && (ras.IdTipoSeccion.Equals(ETiposSeccion.SAL) || ras.IdTipoSeccion.Equals(ETiposSeccion.SCD) || ras.IdTipoSeccion.Equals(ETiposSeccion.SGA)))
                    .Include(al => al.Almacen)
                    .Include(s => s.Seccion)
                    .AsNoTracking()
                    .FirstOrDefault();

                // Obtener EmpresaMoneda base
                var resEmpMon = _repository.EmpresaMonedas
                    .FindByCondition(em => em.Reservado.Equals(true))
                    .AsNoTracking()
                    .FirstOrDefault();

                //Chequear q datos de cabecera existan
                if ((new object[] {resSoli, resDest, resEmpMon}).Any(v => v == null))
                    throw new Exception("Imposible obtener datos de " + (resSoli == null ? "Almacén/Sección origen" : resDest == null ? "Almacén/Sección destino" : "Empresa"));

                // Completar datos de la Solicitud
                solicitudEntity.IdOperador = _repository.Solicitudes.APIUserId;
                solicitudEntity.Referencia = _repository.Solicitudes.APIReference;

                solicitudEntity.AlmacenSeccionSolicitante = null; //resSoli;
                solicitudEntity.AlmacenSeccionSolicitanteId = resSoli.Id;
                solicitudEntity.AlmacenSeccionDestino = null; // resDest;
                solicitudEntity.AlmacenSeccionDestinoId = resDest.Id;
                #endregion

                #region Completar los datos de las lineas
                // Completar datos de las lineas de Solicitud
                foreach (SolicitudLinea linea in solicitudEntity.Lineas)
                {
                    // Obtener datos de Articulo
                    var resArt = _repository.Articulos.FindByCondition(a => a.Id.Equals(linea.ArticuloId))
                        //.Include(umc => umc.UMCompra)
                        //.Include(uma => uma.UMAlmacenaje)
                        //.Include(ums => ums.UMSeccion)
                        .AsNoTracking()
                        .FirstOrDefault();
                    if (resArt == null)
                        throw new Exception(message: $"Artículo {linea.ArticuloId} inexistente.");

                    // Obtener datos de Origen, segun Ids del AlmacenSecion Origen y el articulo; obtener segun la Empresa y la moneda base
                    var resStSol = _context.StocksLineasDetalles
                        .Where(stld => stld.EmpresaMonedaId.Equals(resEmpMon.Id)
                            && stld.StockLinea.Stocks.AlmacenSeccionId.Equals(resDest.Id)
                            && stld.StockLinea.Stocks.ArticuloId.Equals(resArt.Id))
                        .Include(stld => stld.StockLinea)
                        .AsNoTracking()
                        .FirstOrDefault();
                    if (resStSol == null)
                        throw new Exception($"Imposible obtener datos de artículo {resArt.Id} en el origen");
                    else if (!(resStSol.StockLinea.Existencia > 0))
                        throw new Exception($"El artículo {resArt.Id} no tiene existencias en el origen");

                    // Obtener datos de Destino, segun Ids del AlmacenSecion Destino y el articulo; obtener segun la Empresa y la moneda base
                    var resStDes = _context.StocksLineasDetalles
                        .Where(stld => stld.EmpresaMonedaId.Equals(resEmpMon.Id)
                            && stld.StockLinea.Stocks.AlmacenSeccionId.Equals(resDest.Id)
                            && stld.StockLinea.Stocks.ArticuloId.Equals(resArt.Id))
                        .AsNoTracking()
                        .FirstOrDefault();
                    if (resStDes == null)
                        throw new Exception($"Imposible obtener datos de artículo {resArt.Id} en el destino");

                    // Obtener datos de UM origen segun sea el Tipo de Seccion Origen
                    var resUMSol = _repository.UnidadesMedida
                        .FindByCondition(um => um.Id.Equals(resSoli.IdTipoSeccion.Equals(ETiposSeccion.SAL) ? resArt.UMAlmacenajeId : resArt.UMSeccionId))
                        .AsNoTracking()
                        .FirstOrDefault();
                    if (resUMSol == null)
                        throw new Exception($"Imposible obtener UM del artículo {resArt.Id} en el origen");

                    // Obtener datos de UM destino segun sea el Tipo de Seccion Destino
                    var resUMDes = _repository.UnidadesMedida
                        .FindByCondition(um => um.Id.Equals(resDest.IdTipoSeccion.Equals(ETiposSeccion.SAL) ? resArt.UMAlmacenajeId : resArt.UMSeccionId))
                        .AsNoTracking()
                        .FirstOrDefault();
                    if (resUMDes == null)
                        throw new Exception($"Imposible obtener UM del artículo {resArt.Id} en el destino");

                    // Obtener datos en Conversiones de UM (de UM origen a UM destino)
                    var resCUMSol = _repository.Conversiones
                        .FindByCondition(c => c.UnidadMedidaOrigenId.Equals(resUMSol.Id) && c.UnidadMedidaDestinoId.Equals(resUMDes.Id))
                        .AsNoTracking()
                        .FirstOrDefault();
                    if (resCUMSol == null)
                        throw new Exception($"Imposible obtener conversiones entre las UM origen y destino del artículo {resArt.Id}");

                    // Obtener datos en Conversiones de UM (de UM destino a UM origen)
                    var resCUMDes = _repository.Conversiones
                        .FindByCondition(c => c.UnidadMedidaOrigenId.Equals(resUMDes.Id) && c.UnidadMedidaDestinoId.Equals(resUMSol.Id))
                        .AsNoTracking()
                        .FirstOrDefault();
                    if (resCUMDes == null)
                        throw new Exception($"Imposible obtener conversiones entre las UM destino y origen del artículo {resArt.Id}");

                    /* COMPLETO LOS CAMPOS DE LAS LINEAS CON LOS DATOS NECESARIOS */
                    linea.Articulo = null; //resArt;
                    linea.Conversion = null; // resCUMSol;
                    linea.ConversionId = resCUMSol.Id;
                    linea.ConversionDestino = null; // resCUMDes; 
                    linea.ConversionDestinoId = resCUMDes.Id;
                    linea.CantidadDestino = (linea.Cantidad * resCUMDes.Factor);
                    linea.Detalles = new SolicitudLineaDetalle
                    {
                        EmpresaMoneda = null, // resEmpMon,
                        EmpresaMonedaId = resEmpMon.Id,
                        Precio = resStSol.Precio,
                        Importe = (linea.Cantidad * resStSol.Precio),
                        PrecioDestino = resStDes.Precio,
                        ImporteDestino = (linea.CantidadDestino * resStDes.Precio),
                    };
                }
                #endregion

                solicitudEntity.Consecutivo = _repository.Solicitudes.GetConsecutivo(resSoli.AlmacenId, solicitudEntity.Fecha, idTipoOperacion);

                _repository.Solicitudes.CreateSolicitud(solicitudEntity);

                _repository.Save();

                // Asignación para q el mapeo resultante muestre los datos del solicitante y del destino
                solicitudEntity.AlmacenSeccionSolicitante = resSoli;
                solicitudEntity.AlmacenSeccionDestino = resDest;

                var createdSolicitud = _mapper.Map<SolicitudDTO>(solicitudEntity);

                return CreatedAtRoute("SolicitudPorId", new { id = createdSolicitud.Id }, createdSolicitud);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en creación de la Solicitud: {ex.Message}");
                if (ex.InnerException != null)
                    _logger.LogError($"Excepción interna = {ex.InnerException.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}

//Ver https://entityframework.net/knowledge-base/21035405/new-records-inserted-in-foreign-key-table-when-inserting-in-parent-table

//ver https://stackoverflow.com/questions/33938669/restful-api-design-how-to-handle-foreign-keys

//Ver http://www.kamilgrzybek.com/design/rest-api-data-validation/


