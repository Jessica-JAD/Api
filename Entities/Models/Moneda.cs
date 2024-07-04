using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SICLAMON")]
    public partial class Moneda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_mone")]
        public int Id { get; set; }

        [MaxLength(4)]
        [Column("cod_mone")]
        public string Codigo { get; set; }

        [MaxLength(18)]
        [Column("desc_mone")]
        public string Descripcion { get; set; }

        [Column("eliminado")]
        public bool Eliminado { get; set; }
    }

    public class MonedaFiltering : ModelsFiltering
    {
    }
}
