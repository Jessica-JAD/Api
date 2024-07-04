using AutoMapper;
using Entities;
using Entities.DTOs;

namespace RESTApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Origen, destino
            CreateMap<Almacen, AlmacenDTO>()
                .ReverseMap();
            CreateMap<Almacen, AlmacenSeccionesDTO>()
                .ReverseMap();

            CreateMap<Seccion, SeccionDTO>()
                .ReverseMap();

            CreateMap<AlmacenSeccion, AlmacenSeccionDTO>()
                .ForMember(dest =>
                    dest.Id,
                    opt => opt.MapFrom(src => src.SeccionId))
                .ForMember(dest =>
                    dest.Codigo,
                    opt => opt.MapFrom(src => src.Seccion.Codigo))
                .ForMember(dest =>
                    dest.Descripcion,
                    opt => opt.MapFrom(src => src.Seccion.Descripcion))
                .ForMember(dest =>
                    dest.IdTipoSeccion,
                    opt => opt.MapFrom(src => src.IdTipoSeccion))
                .ReverseMap();

            CreateMap<UnidadMedida, UnidadMedidaDTO>()
                .ReverseMap();
            CreateMap<UnidadMedida, UnidadMedidaConversionesDTO>()
                .ReverseMap();

            CreateMap<Conversion, ConversionDTO>()
                .ForMember(dest =>
                    dest.UMDestinoId,
                    opt => opt.MapFrom(src => src.UnidadMedidaDestinoId))
                .ForMember(dest =>
                    dest.UMDestinoCodigo,
                    opt => opt.MapFrom(src => src.UnidadMedidaDestino.Codigo))
                .ForMember(dest =>
                    dest.Factor,
                    opt => opt.MapFrom(src => src.Factor))
                .ReverseMap();

            CreateMap<Articulo, ArticuloDTO>()
                .ReverseMap();

            CreateMap<Solicitud, SolicitudDTO>()
                .ForMember(dest =>
                    dest.AlmacenOrigenCodigo,
                    opt => opt.MapFrom(src => src.AlmacenSeccionSolicitante.Almacen.Codigo.Trim()))
                .ForMember(dest =>
                    dest.SeccionOrigenCodigo,
                    opt => opt.MapFrom(src => src.AlmacenSeccionSolicitante.Seccion.Codigo.Trim()))
                .ForMember(dest =>
                    dest.AlmacenDestinoCodigo,
                    opt => opt.MapFrom(src => src.AlmacenSeccionDestino.Almacen.Codigo.Trim()))
                .ForMember(dest =>
                    dest.SeccionDestinoCodigo,
                    opt => opt.MapFrom(src => src.AlmacenSeccionDestino.Seccion.Codigo.Trim()))
                .ReverseMap();
            CreateMap<Solicitud, SolicitudLineasDTO>()
                .ForMember(dest =>
                    dest.AlmacenOrigenCodigo,
                    opt => opt.MapFrom(src => src.AlmacenSeccionSolicitante.Almacen.Codigo.Trim()))
                .ForMember(dest =>
                    dest.SeccionOrigenCodigo,
                    opt => opt.MapFrom(src => src.AlmacenSeccionSolicitante.Seccion.Codigo.Trim()))
                .ForMember(dest =>
                    dest.AlmacenDestinoCodigo,
                    opt => opt.MapFrom(src => src.AlmacenSeccionDestino.Almacen.Codigo.Trim()))
                .ForMember(dest =>
                    dest.SeccionDestinoCodigo,
                    opt => opt.MapFrom(src => src.AlmacenSeccionDestino.Seccion.Codigo.Trim()))
                .ReverseMap();
            CreateMap<Solicitud, SolicitudNueva>()
                .ForMember(dest =>
                    dest.AlmacenOrigenId,
                    opt => opt.MapFrom(src => src.AlmacenSeccionSolicitante.AlmacenId))
                .ForMember(dest =>
                    dest.SeccionOrigenId,
                    opt => opt.MapFrom(src => src.AlmacenSeccionSolicitante.SeccionId))
                .ForMember(dest =>
                    dest.AlmacenDestinoId,
                    opt => opt.MapFrom(src => src.AlmacenSeccionDestino.AlmacenId))
                .ForMember(dest =>
                    dest.SeccionDestinoId,
                    opt => opt.MapFrom(src => src.AlmacenSeccionDestino.SeccionId))
                .ReverseMap();

            CreateMap<SolicitudLinea, LineasDTO>()
                .ForMember(dest =>
                    dest.Codigo,
                    opt => opt.MapFrom(src => src.Articulo.Codigo))
                .ForMember(dest =>
                    dest.Descripcion,
                    opt => opt.MapFrom(src => src.Articulo.Descripcion))
                .ForMember(dest =>
                    dest.UM,
                    opt => opt.MapFrom(src => src.Conversion.UnidadMedidaOrigen.Codigo))
                .ForMember(dest =>
                    dest.UMDestino,
                    opt => opt.MapFrom(src => src.ConversionDestino.UnidadMedidaOrigen.Codigo))
                .ForMember(dest =>
                    dest.Precio,
                    opt => opt.MapFrom(src => src.Detalles.Precio))
                .ForMember(dest =>
                    dest.Importe,
                    opt => opt.MapFrom(src => src.Detalles.Importe))
                .ForMember(dest =>
                    dest.PrecioDestino,
                    opt => opt.MapFrom(src => src.Detalles.PrecioDestino))
                .ForMember(dest =>
                    dest.ImporteDestino,
                    opt => opt.MapFrom(src => src.Detalles.ImporteDestino))
                .ForMember(dest =>
                    dest.Moneda,
                    opt => opt.MapFrom(src => src.Detalles.EmpresaMoneda.Moneda.Codigo))
                .ReverseMap();

            CreateMap<SolicitudLinea, LineaNueva>()
                .ReverseMap();
        }
    }
}
