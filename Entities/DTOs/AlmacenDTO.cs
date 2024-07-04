using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class AlmacenDTO
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }
    }

    public class AlmacenSeccionesDTO
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        public ICollection<AlmacenSeccionDTO> Secciones { get; set; }
    }
}
