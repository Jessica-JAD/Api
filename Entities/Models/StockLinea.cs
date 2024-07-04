using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ALMSTOCL")]
    public partial class StockLinea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_stocl")]
        public int Id { get; set; }

        //Llave foránea de Almacen Stock
        [Column("id_stocc")]
        public int StocksId { get; set; }

        [ForeignKey(nameof(StocksId))]
        public virtual Stock Stocks { get; set; }
        
        [Column("existencia")]
        public decimal Existencia { get; set; }
    }
}