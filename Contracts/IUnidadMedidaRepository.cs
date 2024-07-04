using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IUnidadMedidaRepository : IRepositoryBase<UnidadMedida>
    {
        IEnumerable<UnidadMedida> GetAll(UnidadMedidaFiltering parametros);

        IEnumerable<UnidadMedida> GetAllWithDetails(UnidadMedidaFiltering parametros);
    }
}
