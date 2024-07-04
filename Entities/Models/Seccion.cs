using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{

    [Table("ALMNSECC")]
    public partial class Seccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_seccion")]
        public int Id { get; set; }

        [MaxLength(4)]
        [Column("cod_seccion")]
        public string Codigo { get; set; }

        [MaxLength(50)]
        [Column("desc_seccion")]
        public string Descripcion { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }

    public class SeccionFiltering : ModelsFiltering
    {
    }
}
