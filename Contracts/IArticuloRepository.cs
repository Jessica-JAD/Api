using Entities;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IArticuloRepository : IRepositoryBase<Articulo>
    {
        PagedList<ArticuloDTO> GetAll(ArticulosFiltering parametros);

        PagedList<ArticuloDetallesDTO> GetAllDetails(ArticulosFiltering parametros);

        Task<IEnumerable<Articulo>> GetAllAsync(ArticulosFiltering parametros);
    }
}