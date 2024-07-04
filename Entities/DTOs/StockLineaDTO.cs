using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class StockLineaDTO
    {
        public int Id { get; set; }

        public int ArticuloId { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        public string UMCodigo { get; set; }
    }

    public class StockLineaDetalleDTO : StockLineaDTO
    {
        public decimal? Minimo { get; set; }

        public decimal? Maximo { get; set; }

        public decimal Existencia { get; set; }

        public decimal Precio { get; set; }

        public decimal Importe { get; set; }

        public string MonedaCodigo { get; set; }
    }

}
