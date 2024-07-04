using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    [Table("ALMUMCON")]
    public partial class Conversion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_umconv")]
        public int Id { get; set; }

        [Column("id_umedorig")]
        public int UnidadMedidaOrigenId { get; set; }

        [ForeignKey(nameof(UnidadMedidaOrigenId))]
        public virtual UnidadMedida UnidadMedidaOrigen { get; set; }

        [Column("id_umeddest")]
        public int UnidadMedidaDestinoId { get; set; }

        [ForeignKey(nameof(UnidadMedidaDestinoId))]
        public virtual UnidadMedida UnidadMedidaDestino { get; set; }

        [Column("cantxumdest")]
        public decimal Factor { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }
}
