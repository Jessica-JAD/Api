using System;
using System.ComponentModel;

namespace Entities
{

    public enum EBoolean
    {
        [Description("Falso")]
        FALSE = 0,

        [Description("Verdadero")]
        TRUE = 1,

        [Description("Indefinido")]
        NONE = 2
    }

    public enum ETiposSeccion : byte
    {
        [Description("Almacenaje")]
        SAL = 1,

        [Description("Punto de Venta")]
        SPV = 2,

        [Description("Consumo Discontinuo")]
        SCD = 3,

        [Description("Gasto")]
        SGA = 4
    }

    public enum EPedidosEstados : byte
    {
        [Description("PP-Pendiente")]
        PP = 1,

        [Description("PB-Borrado")]
        PB = 2,

        [Description("PA-Actualizado")]
        PA = 3,

        [Description("PM-Modificado")]
        PM = 6,

        [Description("PN-No se")]
        PN = 7,

        [Description("PC-Cerrado")]
        PC = 9
    }

    public class ModelsFiltering
    {
//        private EBoolean _activo = EBoolean.NONE;

        public string Codigo { get; set; }

        public string Descripcion { get; set; }
/*
        public EBoolean Activo
        {
            get
            {
                return _activo;
            }
            set
            {
                _activo = (Enum.IsDefined(typeof(EBoolean), value)) ? value : EBoolean.NONE;
            }
        }
*/
    }
}
