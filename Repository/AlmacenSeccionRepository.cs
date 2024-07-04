using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class AlmacenSeccionRepository : RepositoryBase<AlmacenSeccion>, IAlmacenSeccionRepository
    {
        public AlmacenSeccionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<AlmacenSeccion> GetAll(int IdAlmacen)
        {
            var almacensecciones = FindByCondition(x => x.AlmacenId.Equals(IdAlmacen))
                .Include(a => a.Seccion)
                .ToList()
                .AsQueryable();

            return almacensecciones.OrderBy(s => s.Seccion.Codigo);
        }

    }
}
