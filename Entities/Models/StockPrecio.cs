using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{
    [Table("ALMSTOCP")]
        public partial class StockPrecio
        {
            //Llave foránea hacia Stock
            [Column("id_stocc")]
            public int StockId { get; set; }

            [ForeignKey(nameof(StockId))]
            public virtual Stock Stock { get; set; }

            //Llave foránea hacia Empresa/Moneda
            [Column("id_emone")]
            public int EmpresaMonedaId { get; set; }

            [ForeignKey(nameof(EmpresaMonedaId))]
            public virtual EmpresaMoneda EmpresaMoneda { get; set; }

            [Column("precio")]
            public decimal Precio { get; set; }

            [Column("precio_vta")]
            public decimal? PrecioVenta { get; set; }
        }
    }



