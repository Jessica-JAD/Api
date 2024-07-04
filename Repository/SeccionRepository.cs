using Entities;
using Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Entities.DTOs;

namespace Repository
{
    public class SeccionRepository : RepositoryBase<Seccion>, ISeccionRepository
    {
        public SeccionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

       
        public IEnumerable<Seccion> GetAll(SeccionFiltering parametros)
        {
            var secciones = FindAll()
                .ToList()
                .AsQueryable();

            SearchByFilter(ref secciones, parametros.Codigo, parametros.Descripcion, EBoolean.TRUE);

            return secciones.OrderBy(s => s.Codigo);
        }


        /**************** METODO PRIVADO PARA APLICAR FILTRO  ********************/
       
        private static void SearchByFilter(ref IQueryable<Seccion> secciones, string Codigo, string Descripcion, EBoolean Activo)
        {
            bool DefActivo = Enum.IsDefined(typeof(EBoolean), Activo);

            // Si no hay valores o los parametros son vacios
            if (!secciones.Any() || (string.IsNullOrWhiteSpace(Codigo) && string.IsNullOrWhiteSpace(Descripcion) && !DefActivo))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(Codigo))
                secciones = secciones.Where(a => a.Codigo.ToLower().Contains(Codigo.Trim().ToLower()));

            if (!string.IsNullOrWhiteSpace(Descripcion))
                secciones = secciones.Where(a => a.Descripcion.ToLower().Contains(Descripcion.Trim().ToLower()));

            if (DefActivo)
            {
                if (Activo == EBoolean.TRUE)
                    secciones = secciones.Where(a => a.Activo.Equals(true));
                else if (Activo == EBoolean.FALSE)
                    secciones = secciones.Where(a => a.Activo.Equals(false));
            }
        }

    }
}
