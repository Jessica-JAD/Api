using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class ArticuloDTO
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        public int UMId { get; set; }

        public string UMCodigo { get; set; }
     
        public bool Activo { get; set; }
    }

    public class ArticuloDetallesDTO : ArticuloDTO
    {
        public decimal? Minimo { get; set; }

        public decimal? Maximo { get; set; }

        public decimal Existencia { get; set; }

        public decimal Precio { get; set; }

        public decimal Importe { get; set; }

        public string MonedaCodigo { get; set; }
    }
}
