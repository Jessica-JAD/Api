using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class AlmacenRepository : RepositoryBase<Almacen>, IAlmacenRepository
    {

        public AlmacenRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Almacen> GetAll(AlmacenFiltering parametros)
        {
           var almacenes = FindAll()
                .ToList()
                .AsQueryable();

            SearchByFilter(ref almacenes, parametros.Codigo, parametros.Descripcion, EBoolean.TRUE);

            return almacenes.OrderBy(s => s.Codigo);
        }

        public IEnumerable<Almacen> GetAllWithDetails(AlmacenFiltering parametros)
        {
            var almacenes = FindAll()
                 // Falta q sean ACTIVAS toas las entidades (tanto las Secciones como la relacion AlmacenSeccion)
                 // Falta incluir al Tipo de Seccion "Consumo Discontinuo"
                 .Include(ras => ras.Secciones.Where(s => s.IdTipoSeccion.Equals(ETiposSeccion.SAL))).ThenInclude(s => s.Seccion)
                 .ToList()
                 .AsQueryable();

            SearchByFilter(ref almacenes, parametros.Codigo, parametros.Descripcion, EBoolean.TRUE);

            return almacenes.OrderBy(s => s.Codigo);
        }

        /**************** METODO PRIVADO PARA APLICAR FILTRO  ********************/

        private static void SearchByFilter(ref IQueryable<Almacen> almacenes, string Codigo, string Descripcion, EBoolean Activo)
        {
            bool DefActivo = Enum.IsDefined(typeof(EBoolean), Activo);

            // Si no hay valores o los parametros son vacios
            if (!almacenes.Any() || (string.IsNullOrWhiteSpace(Codigo) && string.IsNullOrWhiteSpace(Descripcion) && !DefActivo))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(Codigo))
                almacenes = almacenes.Where(a => a.Codigo.ToLower().Contains(Codigo.Trim().ToLower()));

            if (!string.IsNullOrWhiteSpace(Descripcion))
                almacenes = almacenes.Where(a => a.Descripcion.ToLower().Contains(Descripcion.Trim().ToLower()));

            if (DefActivo)
            {
                if (Activo == EBoolean.TRUE)
                    almacenes = almacenes.Where(a => a.Activo.Equals(true));
                else if (Activo == EBoolean.FALSE)
                    almacenes = almacenes.Where(a => a.Activo.Equals(false));
            }
        }

    }
}
