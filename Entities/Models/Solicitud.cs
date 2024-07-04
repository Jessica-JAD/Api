using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    [Table("ALMPEDSC")]
    public partial class Solicitud
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_pedsc")]
        public int Id { get; set; }

        //Llaves foránea
        [Column("id_aeasecsol")]
        public int AlmacenSeccionSolicitanteId { get; set; }

        [ForeignKey(nameof(AlmacenSeccionSolicitanteId))]
        public virtual AlmacenSeccion AlmacenSeccionSolicitante { get; set; }

        //Llaves foránea
        [Column("id_aeasecdest")]
        public int AlmacenSeccionDestinoId { get; set; }

        [ForeignKey(nameof(AlmacenSeccionDestinoId))]
        public virtual AlmacenSeccion AlmacenSeccionDestino { get; set; }

        [Column("fecha_ped")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Column("fecha_real")]
        [DataType(DataType.DateTime)]
        public DateTime FechaReal { get; set; }

        [Column("nro_ref")]
        [StringLength(10)]
#nullable enable
        public string? Referencia { get; set; }
#nullable disable

        [Column("tipo_ped")]
        public bool PedidoManual { get; set; }

        [Column("consecutiv")]
        [StringLength(10)]
        public string Consecutivo { get; set; }

        [Column("observacion")]
        [MaxLength(150)]
#nullable enable
        public string? Observaciones { get; set; }
#nullable disable

        [Column("id_oper")]
        public int IdOperador { get; set; }

        [Column("estado")]
        public EPedidosEstados IdEstado { get; set; }

        //[NotMapped]
        [InverseProperty(nameof(SolicitudLinea.Solicitud))]
        public virtual IEnumerable<SolicitudLinea> Lineas { get; set; }

        public Solicitud()
        {
            FechaReal = DateTime.Now;
            PedidoManual = false;
            IdEstado = EPedidosEstados.PP;
        }
    }

    public class SolicitudFiltering
    {
        public int? AlmacenId { get; set; }

        public int? SeccionId { get; set; }

        public string Consecutivo { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public string Observaciones { get; set; }

        public EPedidosEstados Estado { get; set; }
    }

    public class SolicitudFilteringPaging : PaginadoParam
    {
        public int? AlmacenId { get; set; }

        public int? SeccionId { get; set; }

        public string Consecutivo { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public string Observaciones { get; set; }

        public EPedidosEstados Estado { get; set; }
    }
}
