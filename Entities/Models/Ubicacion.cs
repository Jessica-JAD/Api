using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
    [Table("ALMNUBIC")]
    public partial class Ubicacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_ubicacion")]
        public int Id { get; set; }

        [MaxLength(50)]
        [Column("desc_ubicacion")]
        public string Descripcion { get; set; }

        [MaxLength(2)]
        [Column("estante")]
        public string Estante { get; set; }

        [MaxLength(2)]
        [Column("piso")]
        public string Piso { get; set; }

        [MaxLength(4)]
        [Column("orden")]
        public string Orden { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }
}
