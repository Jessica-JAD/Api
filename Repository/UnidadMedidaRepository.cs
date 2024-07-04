using Entities;
using Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UnidadMedidaRepository : RepositoryBase<UnidadMedida>, IUnidadMedidaRepository
    {
        public UnidadMedidaRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<UnidadMedida> GetAll(UnidadMedidaFiltering parametros)
        {
            var unidadesmedida = FindAll()
                .ToList()
                .AsQueryable();

            SearchByFilter(ref unidadesmedida, parametros.Codigo, parametros.Descripcion, EBoolean.TRUE);

            return unidadesmedida.OrderBy(s => s.Codigo);
        }

        public IEnumerable<UnidadMedida> GetAllWithDetails(UnidadMedidaFiltering parametros)
        {
            var unidadesmedida = FindAll()
                .Include(c => c.Conversiones.Where(s => s.Activo.Equals(true))).ThenInclude(umd => umd.UnidadMedidaDestino)
                .ToList()
                .AsQueryable();

            SearchByFilter(ref unidadesmedida, parametros.Codigo, parametros.Descripcion, EBoolean.TRUE);

            return unidadesmedida.OrderBy(s => s.Codigo);
        }


        /**************** METODO PRIVADO PARA APLICAR FILTRO  ********************/

        private static void SearchByFilter(ref IQueryable<UnidadMedida> unidadesmedida, string Codigo, string Descripcion, EBoolean Activo)
        {
            bool DefActivo = Enum.IsDefined(typeof(EBoolean), Activo);

            // Si no hay valores o los parametros son vacios
            if (!unidadesmedida.Any() || (string.IsNullOrWhiteSpace(Codigo) && string.IsNullOrWhiteSpace(Descripcion) && !DefActivo))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(Codigo))
                unidadesmedida = unidadesmedida.Where(a => a.Codigo.ToLower().Contains(Codigo.Trim().ToLower()));

            if (!string.IsNullOrWhiteSpace(Descripcion))
                unidadesmedida = unidadesmedida.Where(a => a.Descripcion.ToLower().Contains(Descripcion.Trim().ToLower()));

            if (DefActivo)
            {
                if (Activo == EBoolean.TRUE)
                    unidadesmedida = unidadesmedida.Where(a => a.Activo.Equals(true));
                else if (Activo == EBoolean.FALSE)
                    unidadesmedida = unidadesmedida.Where(a => a.Activo.Equals(false));
            }
        }
    }
}
