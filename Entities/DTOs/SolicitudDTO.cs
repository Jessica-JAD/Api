using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DTOs
{
    public class SolicitudDTO
    {
        public int Id { get; set; }

        public string Consecutivo { get; set; }

        public DateTime Fecha { get; set; }

        public string AlmacenOrigenCodigo { get; set; }

        public string SeccionOrigenCodigo { get; set; }

        public string AlmacenDestinoCodigo { get; set; }

        public string SeccionDestinoCodigo { get; set; }

        #nullable enable
        public string? Observaciones { get; set; }
        #nullable disable

        public EPedidosEstados IdEstado { get; set; }
    }

    public class LineasDTO
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        public decimal Cantidad { get; set; }

        public string UM { get; set; }

        public decimal CantidadDestino { get; set; }

        public string UMDestino { get; set; }

        public decimal Precio { get; set; }

        public decimal Importe { get; set; }

        public decimal PrecioDestino { get; set; }

        public decimal ImporteDestino { get; set; }

        public string Moneda { get; set; }
    }


    public class SolicitudLineasDTO
    {
        public int Id { get; set; }

        public string Consecutivo { get; set; }

        public DateTime Fecha { get; set; }

        public string AlmacenOrigenCodigo { get; set; }

        public string SeccionOrigenCodigo { get; set; }

        public string AlmacenDestinoCodigo { get; set; }

        public string SeccionDestinoCodigo { get; set; }

        #nullable enable
        public string? Observaciones { get; set; }
        #nullable disable

        public virtual IEnumerable<LineasDTO> Lineas { get; set; }

        public EPedidosEstados IdEstado { get; set; }
    }


    public class SolicitudNueva : IValidatableObject
    {
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha es requerida.")]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El identificador del almacén origen es requerido.")]
        [Display(Name = "AlmacenOrigenId")]
        public int AlmacenOrigenId { get; set; }

        [Required(ErrorMessage = "El identificador de la sección origen es requerido.")]
        [Display(Name = "SeccionOrigenId")]
        public int SeccionOrigenId { get; set; }

        [Required(ErrorMessage = "El identificador del almacén destino es requerido.")]
        [Display(Name = "AlmacenDestinoId")]
        public int AlmacenDestinoId { get; set; }

        [Required(ErrorMessage = "El identificador de la sección destino es requerido.")]
        [Display(Name = "SeccionDestinoId")]
        public int SeccionDestinoId { get; set; }

        [MaxLength(150)]
#nullable enable
        public string? Observaciones { get; set; }
#nullable disable

        public virtual IEnumerable<LineaNueva> Lineas { get; set; }

        public SolicitudNueva()
        {
            Fecha = DateTime.Today;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errores = new List<ValidationResult>();

            if (!IsValidFecha(Fecha))
            {
                errores.Add(new ValidationResult($"La fecha especificada {Fecha} no es válida."));
            }

            if (!IsValidOrigen(AlmacenOrigenId, SeccionOrigenId))
            {
                errores.Add(new ValidationResult($"El origen no es válido"));
            }

            if (!IsValidDestino(AlmacenDestinoId, SeccionDestinoId))
            {
                errores.Add(new ValidationResult($"El destino no es válido"));
            }

            return errores;
        }

        static bool IsValidFecha(DateTime fecha)
        {
            // Aqui chequear q la fecha sea válida y esté en período contable abierto
            return fecha.Date >= new DateTime(2022, 1, 1).Date;
        }

        static bool IsValidOrigen(int almacenId, int seccionId)
        {
            // Aqui chequear q el Almacen/Seccion del origen sea válido
            return (almacenId > 0 && seccionId > 0);
        }

        static bool IsValidDestino(int almacenId, int seccionId)
        {
            // Aqui chequear q el Almacen/Seccion del destino sea válido
            return (almacenId > 0 && seccionId > 0);
        }
    }

    public class LineaNueva : IValidatableObject
    {
        [Required(ErrorMessage = "El identificador del artículo es requerido.")]
        [Display(Name = "ArticuloId")]
        public int ArticuloId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida.")]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errores = new List<ValidationResult>();

            if (!IsValidArticulo(ArticuloId))
            {
                errores.Add(new ValidationResult($"El artículo con Id = {ArticuloId} no puede ser incluido en la Solicitud."));
            }

            if (!IsValidCantidad(Cantidad))
            {
                errores.Add(new ValidationResult($"La cantidad especificada para el artículo {ArticuloId} no es válida"));
            }

            return errores;
        }

        static bool IsValidArticulo(int articuloId)
        {
            // Aqui chequear q el artículo exista en Stocks para el Almacen/Seccion de la solicitud
            return (articuloId > 0);
        }

        static bool IsValidCantidad(decimal ctd)
        {
            // Aqui chequear q la ctd sea menor o igual a la existencia del artículo en Stocks para el ALmacen/Seccion de la solicitud
            return (ctd > 0);
        }
    }
}
