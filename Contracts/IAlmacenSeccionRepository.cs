using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IAlmacenSeccionRepository : IRepositoryBase<AlmacenSeccion>
    {
        IEnumerable<AlmacenSeccion> GetAll(int IdAlmacen);
    }
}
