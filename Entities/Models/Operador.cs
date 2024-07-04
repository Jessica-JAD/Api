using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    [Table("operador")]
    public partial class Operador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_oper")]
        public int Id { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }
}
