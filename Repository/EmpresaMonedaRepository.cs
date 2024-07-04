using Contracts;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmpresaMonedaRepository : RepositoryBase<EmpresaMoneda>, IEmpresaMonedaRepository
    {
        public EmpresaMonedaRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
