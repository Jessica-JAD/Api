using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{
    [Table("ALMSTOCC")]
    public partial class Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_stocc")]
        public int Id { get; set; }

        //Llave foránea de Almacen-Seccion
        [Column("id_aeasec")]
        public int AlmacenSeccionId { get; set; }

        [ForeignKey(nameof(AlmacenSeccionId))]
        public virtual AlmacenSeccion AlmacenSeccion { get; set; }

        //Llave foránea de Articulo
        [Column("id_arti")]
        public int ArticuloId { get; set; }
        
        [ForeignKey(nameof(ArticuloId))]
        public virtual Articulo Articulo { get; set; }

        //Llave foránea de Ubicacion
        [Column("id_ubicacion")]
        public int? UbicacionId { get; set; }

        [ForeignKey(nameof(UbicacionId))]
        public virtual Ubicacion Ubicacion{ get; set; }

        [Column("stock_min")]
        public decimal? Minimo { get; set; }

        [Column("stock_max")]
        public decimal? Maximo { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }
    }

}
