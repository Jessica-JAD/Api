using Entities;
using System.Collections.Generic;

namespace Contracts
{
    public interface IAlmacenRepository : IRepositoryBase<Almacen>
    {
        IEnumerable<Almacen> GetAll(AlmacenFiltering parametros);

        IEnumerable<Almacen> GetAllWithDetails(AlmacenFiltering parametros);
    }
}
