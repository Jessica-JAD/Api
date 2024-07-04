using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{

    [Table("ALMNUMED")]
    public partial class UnidadMedida
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_umed")]
        public int Id { get; set; }

        [MaxLength(20)]
        [Column("cod_umed")]
        public string Codigo { get; set; }

        [MaxLength(50)]
        [Column("desc_umed")]
        public string Descripcion { get; set; }

        [InverseProperty(nameof(Conversion.UnidadMedidaOrigen))]
        public virtual IEnumerable<Conversion> Conversiones { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }

    public class UnidadMedidaFiltering : ModelsFiltering
    {
    }
}
