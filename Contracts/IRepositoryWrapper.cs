using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IEmpresaMonedaRepository EmpresaMonedas { get; }

        IAlmacenRepository Almacenes { get; }

        ISeccionRepository Secciones { get; }

        IAlmacenSeccionRepository AlmacenSecciones { get; }

        IUnidadMedidaRepository UnidadesMedida { get; }

        IConversionRepository Conversiones { get; }

        IArticuloRepository Articulos { get; }

        ISolicitudRepository Solicitudes { get; }

        void Save();

        Task SaveAsync();
    }
}
