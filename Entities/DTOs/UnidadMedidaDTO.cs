using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class UnidadMedidaDTO
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }
    }

    public class UnidadMedidaConversionesDTO
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        public ICollection<ConversionDTO> Conversiones { get; set; }
    }
}
