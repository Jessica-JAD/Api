using Contracts;
using Entities;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    public class ArticuloRepository : RepositoryBase<Articulo>, IArticuloRepository
    {
        public ArticuloRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public PagedList<ArticuloDTO> GetAll(ArticulosFiltering parametros)
        {
            var articulos = (from Art in APIRepositoryContext.Articulos
                                join UMC in APIRepositoryContext.UnidadesMedida on Art.UMCompra equals UMC
                                join UMA in APIRepositoryContext.UnidadesMedida on Art.UMAlmacenaje equals UMA
                                join UMS in APIRepositoryContext.UnidadesMedida on Art.UMSeccion equals UMS
                                join St in APIRepositoryContext.Stocks on Art equals St.Articulo
                                join StL in APIRepositoryContext.StocksLinea on St equals StL.Stocks
                                join StLD in APIRepositoryContext.StocksLineasDetalles on StL equals StLD.StockLinea
                                join AlmSec in APIRepositoryContext.AlmacenSecciones on St.AlmacenSeccion equals AlmSec
                                join Alm in APIRepositoryContext.Almacenes on AlmSec.Almacen equals Alm
                                join Secc in APIRepositoryContext.Secciones on AlmSec.Seccion equals Secc
                                join EM in APIRepositoryContext.EmpresaMonedas on StLD.EmpresaMoneda equals EM
                                join M in APIRepositoryContext.Monedas on EM.Moneda equals M
                             where
                                    AlmSec.AlmacenId == parametros.IdAlmacen
                                    && AlmSec.SeccionId == parametros.IdSeccion
                                    && AlmSec.Activo == true
                                    && St.Activo == true
                                    && Art.Activo == true
                                    && Alm.Activo == true
                                    && Secc.Activo == true
                                    && EM.Activo == true && EM.Reservado == true
                                    && Art.Codigo.ToLower().Contains(!string.IsNullOrWhiteSpace(parametros.Codigo) ? parametros.Codigo.Trim().ToLower() : Art.Codigo.ToLower())
                                    && Art.Descripcion.ToLower().Contains(!string.IsNullOrWhiteSpace(parametros.Descripcion) ? parametros.Descripcion.Trim().ToLower() : Art.Descripcion.ToLower())
                             select new ArticuloDTO
                                {
                                    Id = Art.Id,
                                    Codigo = Art.Codigo,
                                    Descripcion = Art.Descripcion.Trim(),
                                    UMId = AlmSec.IdTipoSeccion == ETiposSeccion.SAL ? Art.UMAlmacenajeId : Art.UMSeccionId,
                                    UMCodigo = AlmSec.IdTipoSeccion == ETiposSeccion.SAL ? UMA.Codigo.Trim() : UMS.Codigo.Trim(),
                                    Activo = Art.Activo,
                                })
                                .ToList()
                                .AsQueryable()
                                .AsNoTracking();

            return PagedList<ArticuloDTO>.ToPagedList(articulos.OrderBy(a => a.Codigo),
                parametros.PageNumber,
                parametros.PageSize);
        }

        public PagedList<ArticuloDetallesDTO> GetAllDetails(ArticulosFiltering parametros)
        {
            var articulos = (from Art in APIRepositoryContext.Articulos
                                join UMC in APIRepositoryContext.UnidadesMedida on Art.UMCompra equals UMC
                                join UMA in APIRepositoryContext.UnidadesMedida on Art.UMAlmacenaje equals UMA 
                                join UMS in APIRepositoryContext.UnidadesMedida on Art.UMSeccion equals UMS 
                                join St in APIRepositoryContext.Stocks on Art equals St.Articulo
                                join StL in APIRepositoryContext.StocksLinea on St equals StL.Stocks 
                                join StLD in APIRepositoryContext.StocksLineasDetalles on StL equals StLD.StockLinea
                                join AlmSec in APIRepositoryContext.AlmacenSecciones on St.AlmacenSeccion equals AlmSec
                                join Alm in APIRepositoryContext.Almacenes on AlmSec.Almacen equals Alm 
                                join Secc in APIRepositoryContext.Secciones on AlmSec.Seccion equals Secc 
                                join EM in APIRepositoryContext.EmpresaMonedas on StLD.EmpresaMoneda equals EM 
                                join M in APIRepositoryContext.Monedas on EM.Moneda equals M 
                                where 
                                        AlmSec.AlmacenId == parametros.IdAlmacen
                                    && AlmSec.SeccionId == parametros.IdSeccion
                                    && AlmSec.Activo == true
                                    && St.Activo == true
                                    && Art.Activo == true
                                    && Alm.Activo == true
                                    && Secc.Activo == true
                                    && EM.Activo == true && EM.Reservado == true
                                    && Art.Codigo.ToLower().Contains(!string.IsNullOrWhiteSpace(parametros.Codigo) ? parametros.Codigo.Trim().ToLower() : Art.Codigo.ToLower())
                                    && Art.Descripcion.ToLower().Contains(!string.IsNullOrWhiteSpace(parametros.Descripcion) ? parametros.Descripcion.Trim().ToLower() : Art.Descripcion.ToLower())

                             select new ArticuloDetallesDTO
                            {
                                Id = Art.Id,
                                Codigo = Art.Codigo,
                                Descripcion = Art.Descripcion.Trim(),
                                UMId = AlmSec.IdTipoSeccion == ETiposSeccion.SAL ? Art.UMAlmacenajeId : Art.UMSeccionId,
                                UMCodigo = AlmSec.IdTipoSeccion == ETiposSeccion.SAL ? UMA.Codigo.Trim() : UMS.Codigo.Trim(),
                                Minimo = St.Minimo,
                                Maximo = St.Maximo,
                                Existencia = StL.Existencia,
                                Precio = StLD.Precio,
                                Importe = StLD.Importe,
                                MonedaCodigo = M.Codigo.Trim()
                             })
                             .ToList()
                             .AsQueryable()
                             .AsNoTracking();

            return PagedList<ArticuloDetallesDTO>.ToPagedList(articulos.OrderBy(a => a.Codigo),
                parametros.PageNumber,
                parametros.PageSize);
        }

        public async Task<IEnumerable<Articulo>> GetAllAsync(ArticulosFiltering parametros)
        {
            var articulos = await FindAll()
                   .OrderBy(s => s.Codigo)
                   .ToListAsync();

            //SearchByFilter(ref articulos, parametros.Codigo, parametros.Descripcion, parametros.Activo);

            return articulos;
        }

        // Metodos privados

        private static void SearchByFilter(ref IQueryable<ArticuloDTO> articulos, string Codigo, string Descripcion, EBoolean Activo)
        {
            bool DefActivo = Enum.IsDefined(typeof(EBoolean), Activo);

            // Si no hay valores o los parametros son vacios
            if (!articulos.Any() || (string.IsNullOrWhiteSpace(Codigo) && string.IsNullOrWhiteSpace(Descripcion) && !DefActivo))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(Codigo))
                articulos = articulos.Where(a => a.Codigo.ToLower().Contains(Codigo.Trim().ToLower()));

            if (!string.IsNullOrWhiteSpace(Descripcion))
                articulos = articulos.Where(a => a.Descripcion.ToLower().Contains(Descripcion.Trim().ToLower()));

            if (DefActivo)
            {
                if (Activo == EBoolean.TRUE)
                    articulos = articulos.Where(a => a.Activo.Equals(true));
                else if (Activo == EBoolean.FALSE)
                    articulos = articulos.Where(a => a.Activo.Equals(false));
            }

        }

        private void SearchByFilterDetails(ref IQueryable<ArticuloDetallesDTO> articulos, string Codigo, string Descripcion, EBoolean Activo)
        {
            bool DefActivo = Enum.IsDefined(typeof(EBoolean), Activo);

            // Si no hay valores o los parametros son vacios
            if (!articulos.Any() || (string.IsNullOrWhiteSpace(Codigo) && string.IsNullOrWhiteSpace(Descripcion) && !DefActivo))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(Codigo))
                articulos = articulos.Where(a => a.Codigo.ToLower().Contains(Codigo.Trim().ToLower()));

            if (!string.IsNullOrWhiteSpace(Descripcion))
                articulos = articulos.Where(a => a.Descripcion.ToLower().Contains(Descripcion.Trim().ToLower()));

            if (DefActivo)
            {
                if (Activo == EBoolean.TRUE)
                    articulos = articulos.Where(a => a.Activo.Equals(true));
                else if (Activo == EBoolean.FALSE)
                    articulos = articulos.Where(a => a.Activo.Equals(false));
            }

        }


    }
}
