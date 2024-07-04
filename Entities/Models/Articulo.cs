using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ALMNARTI")]
    public partial class Articulo
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_arti")]
        public int Id { get; set; }

        [MaxLength(20)]
        [Column("cod_arti")]
        public string Codigo { get; set; }

        [MaxLength(80)]
        [Column("desc_arti")]
        public string Descripcion { get; set; }

        //Llave foránea de Unidad de Medida Comp
        [Column("id_umcomp")]
        public int UMCompraId { get; set; }

        [ForeignKey(nameof(UMCompraId))]
        public virtual UnidadMedida UMCompra { get; set; }

        //Llave foránea de Unidad de Medida Almacenaje
        [Column("id_umalmac")]
        public int UMAlmacenajeId { get; set; }

        [ForeignKey(nameof(UMAlmacenajeId))]
        public virtual UnidadMedida UMAlmacenaje { get; set; }

        //Lave foránea de Unidad de Medida Seccion
        [Column("id_umsec")]
        public int UMSeccionId { get; set; }
        
        [ForeignKey(nameof(UMSeccionId))]
        public virtual UnidadMedida UMSeccion { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }

    public class ArticulosFiltering : PaginadoParam
    {
        public int IdAlmacen { get; set; }

        public int IdSeccion { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }
    }
}
