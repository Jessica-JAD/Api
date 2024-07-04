using Entities;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ISolicitudRepository : IRepositoryBase<Solicitud>
    {
        string APIUser { get; }

        int APIUserId { get; }

        string APIReference { get; }

        string GetConsecutivo(int idAlmacen, DateTime fecha, int idTipoOperacion);

        IEnumerable<Solicitud> GetAll(SolicitudFiltering parametros);

        PagedList<Solicitud> GetAllPages(SolicitudFilteringPaging parametros);

        Solicitud GetById(int Id);

        Solicitud GetByIdWithLines(int Id);

        void CreateSolicitud(Solicitud solicitud);
    }
}
