using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
    public class SpResumenHorasExtras
    {
        public bool Sel { get; set; }
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public string Imprimir { get; set; }

    }
}
