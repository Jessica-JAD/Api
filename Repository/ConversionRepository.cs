using Contracts;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ConversionRepository : RepositoryBase<Conversion>, IConversionRepository
    {
        public ConversionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
