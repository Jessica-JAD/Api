using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    [Table("ALMSTOCD")]
    public partial class StockLineaDetalle
    {
        //Llave foránea hacia Stock-Lineas
        [Column("id_stocl")]
        public int StockLineaId { get; set; }
        
        [ForeignKey(nameof(StockLineaId))]
        public virtual StockLinea StockLinea { get; set; }

        //Llave foránea hacia Empresa/Moneda
        [Column("id_emone")]
        public int EmpresaMonedaId { get; set; }

        [ForeignKey(nameof(EmpresaMonedaId))]
        public virtual EmpresaMoneda EmpresaMoneda { get; set; }

        [Column("precio_arti")]
        public decimal Precio { get; set; }

        [Column("importe_arti")]
        public decimal Importe { get; set; }
    }
}
