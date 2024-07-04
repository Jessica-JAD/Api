using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    [Table("ALMPEDSD")]
    public partial class SolicitudLineaDetalle
    {
        //LLave foránea de  Lineas
        [Column("id_linped")]
        public int SolicitudLineaId { get; set; }

        [ForeignKey(nameof(SolicitudLineaId))]
        public virtual SolicitudLinea SolicitudLinea { get; set; }

        //LLave foránea de Moneda
        [Column("id_emone")]
        public int EmpresaMonedaId { get; set; }

        [ForeignKey(nameof(EmpresaMonedaId))]
        public virtual EmpresaMoneda EmpresaMoneda { get; set; }

        [Column("precio_arti_orig")]
        public decimal Precio { get; set; }

        [Column("importe_arti_orig")]
        public decimal Importe { get; set; }

        [Column("precio_arti_dest")]
        public decimal PrecioDestino { get; set; }

        [Column("importe_arti_dest")]
        public decimal ImporteDestino { get; set; }
    }

    public class StockFiltering : PaginadoParam
    {
        public string Codigo { get; set; }

        public string Descripcion { get; set; }
    }

}
