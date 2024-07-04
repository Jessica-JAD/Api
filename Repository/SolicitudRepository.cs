using Contracts;
using Entities;
using Entities.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Repository
{
     public class SolicitudRepository : RepositoryBase<Solicitud>, ISolicitudRepository
    {
        public SolicitudRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        private readonly string _APIUser = "APIUser";

        private readonly string _APIReference = "APIStocks";

        public static DateTime FechaMinima
        {
            get {
                string[] fechaFormatos = new[] { "dd/MM/yyyy" };
                CultureInfo proveedor = new CultureInfo("en-US");

                return DateTime.ParseExact("01/01/2022", fechaFormatos, proveedor, DateTimeStyles.AdjustToUniversal);
            }
        }

        public string APIUser
        {
            get { return _APIUser; }
        }

        public int APIUserId
        {
            get { return GetAPIUserId(); }
        }

        public string APIReference
        {
            get { return _APIReference; }
        }

        private int GetAPIUserId()
        {
            var Valor = (from Oper in APIRepositoryContext.Operadores
                         where
                             Oper.Nombre.ToUpper().Equals(_APIUser.ToUpper())
                         select
                            Oper.Id
                        ).FirstOrDefault();

            return Valor;
        }

        public string GetConsecutivo(int idAlmacen, DateTime fecha, int idTipoOperacion)
        {
            var paramIdAlmacen = new SqlParameter
            {
                ParameterName = "id_alm",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = idAlmacen,
            };
            var paramFecha = new SqlParameter
            {
                ParameterName = "fecha_cont",
                SqlDbType = System.Data.SqlDbType.DateTime,
                Value = fecha,
            };
            var paramIdTipoOperac = new SqlParameter
            {
                ParameterName = "id_operac",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = idTipoOperacion,
            };
            var paramConsecutivo = new SqlParameter
            {
                ParameterName = "consecutivo",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Size = 10,
                Direction = System.Data.ParameterDirection.Output,
            };

            var res = APIRepositoryContext.Database.ExecuteSqlRaw(@"EXECUTE sp_Get_Consecutivo @id_alm, @fecha_cont, @id_operac, @consecutivo OUTPUT", paramIdAlmacen, paramFecha, paramIdTipoOperac, paramConsecutivo);

            return (string)paramConsecutivo.Value;
        }

        public IEnumerable<Solicitud> GetAll(SolicitudFiltering parametros)
        {
            var solicitudes = FindAll()
                .Include(s => s.AlmacenSeccionSolicitante).ThenInclude(a => a.Almacen)
                .Include(s => s.AlmacenSeccionSolicitante).ThenInclude(a => a.Seccion)
                .Include(s => s.AlmacenSeccionDestino).ThenInclude(a => a.Almacen)
                .Include(s => s.AlmacenSeccionDestino).ThenInclude(a => a.Seccion)
                /* CONDICIONES OBLIGADAS A APLICAR */
                .Where(s => s.Fecha >= FechaMinima
                    && s.IdOperador.Equals(APIUserId) 
                    && s.Referencia.ToUpper().Equals(APIReference.ToUpper()))
                .ToList()
                .AsQueryable();

            SearchByFilter(ref solicitudes, parametros.AlmacenId, parametros.SeccionId, parametros.Consecutivo, parametros.Observaciones, parametros.Fecha, parametros.Estado);

            return solicitudes.OrderBy(s => s.AlmacenSeccionSolicitante.Almacen.Codigo)
                    .ThenBy(s => s.AlmacenSeccionSolicitante.Seccion.Codigo)
                    .ThenBy(s => s.Consecutivo);
        }

        public PagedList<Solicitud> GetAllPages(SolicitudFilteringPaging parametros)
        {
            var solicitudes = FindAll()
                .Include(s => s.AlmacenSeccionSolicitante).ThenInclude(a => a.Almacen)
                .Include(s => s.AlmacenSeccionSolicitante).ThenInclude(a => a.Seccion)
                .Include(s => s.AlmacenSeccionDestino).ThenInclude(a => a.Almacen)
                .Include(s => s.AlmacenSeccionDestino).ThenInclude(a => a.Seccion)
                /* CONDICIONES OBLIGADAS A APLICAR */
                .Where(s => s.Fecha >= FechaMinima
                    && s.IdOperador.Equals(APIUserId)
                    && s.Referencia.ToUpper().Equals(APIReference.ToUpper()))
                .ToList()
                .AsQueryable();

            SearchByFilter(ref solicitudes, parametros.AlmacenId, parametros.SeccionId, parametros.Consecutivo, parametros.Observaciones, parametros.Fecha, parametros.Estado);

            return PagedList<Solicitud>.ToPagedList(solicitudes.OrderBy(s => s.AlmacenSeccionSolicitante.Almacen.Codigo)
                    .ThenBy(s => s.AlmacenSeccionSolicitante.Seccion.Codigo)
                    .ThenBy(s => s.Consecutivo),
                parametros.PageNumber,
                parametros.PageSize);
        }

        public Solicitud GetById(int Id)
        {
            return FindByCondition(s => s.Id.Equals(Id))
                .Include(s => s.AlmacenSeccionSolicitante).ThenInclude(a => a.Almacen)
                .Include(s => s.AlmacenSeccionSolicitante).ThenInclude(a => a.Seccion)
                .Include(s => s.AlmacenSeccionDestino).ThenInclude(a => a.Almacen)
                .Include(s => s.AlmacenSeccionDestino).ThenInclude(a => a.Seccion)
                /* CONDICIONES OBLIGADAS A APLICAR */
                .Where(s => s.Fecha >= FechaMinima
                    && s.IdOperador.Equals(APIUserId)
                    && s.Referencia.ToUpper().Equals(APIReference.ToUpper()))
                .FirstOrDefault();
        }

        public Solicitud GetByIdWithLines(int Id)
        {
            return FindByCondition(s => s.Id.Equals(Id))
                .Include(s => s.AlmacenSeccionSolicitante).ThenInclude(a => a.Almacen)
                .Include(s => s.AlmacenSeccionSolicitante).ThenInclude(a => a.Seccion)
                .Include(s => s.AlmacenSeccionDestino).ThenInclude(a => a.Almacen)
                .Include(s => s.AlmacenSeccionDestino).ThenInclude(a => a.Seccion)
                .Include(s => s.Lineas).ThenInclude(a => a.Articulo)
                .Include(s => s.Lineas).ThenInclude(a => a.Conversion).ThenInclude(b => b.UnidadMedidaOrigen)
                .Include(s => s.Lineas).ThenInclude(a => a.ConversionDestino).ThenInclude(b => b.UnidadMedidaOrigen)
                .Include(s => s.Lineas).ThenInclude(a => a.Detalles).ThenInclude(b => b.EmpresaMoneda).ThenInclude(c => c.Moneda)
                /* CONDICIONES OBLIGADAS A APLICAR */
                .Where(s => s.IdOperador.Equals(APIUserId)
                    && s.Referencia.ToUpper().Equals(APIReference.ToUpper()))
                .FirstOrDefault();
        }

        public void CreateSolicitud(Solicitud solicitud)
        {
            Create(solicitud);
        }


        /************************
         *   Metodos privados   *
         ************************/
        private static void SearchByFilter(ref IQueryable<Solicitud> solicitudes, int? AlmacenId, int? SeccionId, string Consecutivo, string Observaciones, DateTime Fecha, EPedidosEstados Estado)
        {
            bool DefEstado = Enum.IsDefined(typeof(EPedidosEstados), Estado);

            // Si no hay valores o los parametros son vacios
            if (!solicitudes.Any() || ((AlmacenId.GetValueOrDefault(0) == 0) && (SeccionId.GetValueOrDefault(0) == 0) && string.IsNullOrWhiteSpace(Consecutivo) && string.IsNullOrWhiteSpace(Observaciones) && (Fecha.Date > DateTime.Parse("0001-01-01")) && !DefEstado))
            {
                return;
            }

            if (AlmacenId.GetValueOrDefault(0) > 0)
            {
                solicitudes = solicitudes.Where(s => s.AlmacenSeccionSolicitante.AlmacenId.Equals(AlmacenId));
            }

            if (SeccionId.GetValueOrDefault(0) > 0)
            {
                solicitudes = solicitudes.Where(s => s.AlmacenSeccionSolicitante.SeccionId.Equals(SeccionId));
            }

            if (!string.IsNullOrWhiteSpace(Consecutivo))
                solicitudes = solicitudes.Where(a => a.Consecutivo.ToLower().Contains(Consecutivo.Trim().ToLower()));

            if (!string.IsNullOrWhiteSpace(Observaciones))
                solicitudes = solicitudes.Where(a => a.Referencia.ToLower().Contains(Observaciones.Trim().ToLower()));

            if (Fecha.Date > FechaMinima)
                solicitudes = solicitudes.Where(p => p.Fecha.Equals(Fecha));

            if (DefEstado)
            {
                if (Estado == EPedidosEstados.PP)
                    solicitudes = solicitudes.Where(a => a.IdEstado.Equals(EPedidosEstados.PP));
                else if (Estado == EPedidosEstados.PB)
                    solicitudes = solicitudes.Where(a => a.IdEstado.Equals(EPedidosEstados.PB));
                else if (Estado == EPedidosEstados.PA)
                    solicitudes = solicitudes.Where(a => a.IdEstado.Equals(EPedidosEstados.PA));
                else if (Estado == EPedidosEstados.PM)
                    solicitudes = solicitudes.Where(a => a.IdEstado.Equals(EPedidosEstados.PM));
                else if (Estado == EPedidosEstados.PN)
                    solicitudes = solicitudes.Where(a => a.IdEstado.Equals(EPedidosEstados.PN));
                else if (Estado == EPedidosEstados.PC)
                    solicitudes = solicitudes.Where(a => a.IdEstado.Equals(EPedidosEstados.PC));
            }

        }
    }
} 
