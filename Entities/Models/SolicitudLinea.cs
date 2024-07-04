using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    [Table("ALMPEDSL")]
    public partial class SolicitudLinea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_linped")]
        public int Id { get; set; }

        //LLave foránea de Solicitud
        [Column("id_pedsc")]
        public int SolicitudId { get; set; }

        [ForeignKey(nameof(SolicitudId))]
        public virtual Solicitud Solicitud { get; set; }

        //Llaves foránea de Articulo
        [Column("id_arti")]
        public int ArticuloId { get; set; }

        [ForeignKey(nameof(ArticuloId))]
        public virtual Articulo Articulo { get; set; }

        [Column("ctdad_ped")]
        public decimal Cantidad { get; set; }

        [Column("id_umconvped")]
        public int ConversionId { get; set; }

        [ForeignKey(nameof(ConversionId))]
        public virtual Conversion Conversion { get; set; }

        [Column("ctdad_dest")]
        public decimal CantidadDestino { get; set; }

        [Column("id_umconvdest")]
        public int ConversionDestinoId { get; set; }

        [ForeignKey(nameof(ConversionDestinoId))]
        public virtual Conversion ConversionDestino { get; set; }

        [InverseProperty(nameof(SolicitudLineaDetalle.SolicitudLinea))]
        public virtual SolicitudLineaDetalle Detalles { get; set; }
    }
}
