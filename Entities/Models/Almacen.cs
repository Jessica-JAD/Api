using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ALMNALMA")]
    public partial class Almacen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_almacen")]
        public int Id { get; set; }

        [MaxLength(3)]
        [Column("cod_almacen")]
        public string Codigo { get; set; } 

        [MaxLength(50)]
        [Column("desc_almacen")]
        public string Descripcion { get; set; }

        [InverseProperty(nameof(AlmacenSeccion.Almacen))]
        public virtual IEnumerable<AlmacenSeccion> Secciones { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }

    public class AlmacenFiltering : ModelsFiltering
    {
    }
}
