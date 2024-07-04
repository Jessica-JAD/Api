using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("almeasec")]
    public partial class AlmacenSeccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_aeasec")]
        public int Id { get; set; }


        [Column("id_almacen")]
        public int AlmacenId { get; set; }
        [ForeignKey(nameof(AlmacenId))]
        public virtual Almacen Almacen { get; set; }


        [Column("id_seccion")]
        public int SeccionId { get; set; }
        [ForeignKey(nameof(SeccionId))]
        public virtual Seccion Seccion { get; set; }

        [Column("tipo_seccion")]
        public ETiposSeccion IdTipoSeccion { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }
}
