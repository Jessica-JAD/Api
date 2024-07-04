using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ISeccionRepository : IRepositoryBase<Seccion>
    {
        IEnumerable<Seccion> GetAll(SeccionFiltering parametros);
    }
}
