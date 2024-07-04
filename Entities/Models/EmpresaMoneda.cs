using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    [Table("ALMEMONE")]
    public class EmpresaMoneda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_emone")]
        public int Id { get; set; }
       
        [Column("id_moneda")]
        public int MonedaId { get; set; }

        [ForeignKey(nameof(MonedaId))]
        public virtual Moneda Moneda { get; set; }

        [Column("reservado")]
        public bool Reservado { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }

    public class EmpresaMonedaFiltering : ModelsFiltering
    {
    }
    
}
