using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class AlmacenSeccionDTO
    {
        public int Id { get; set; } //Id de la Seccion

        public string Codigo { get; set; } //Codigo de la Seccion

        public string Descripcion { get; set; } //Descripción de la Seccion

        public ETiposSeccion IdTipoSeccion { get; set; }
    }
}
