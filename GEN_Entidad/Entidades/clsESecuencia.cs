using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
   public class Secuencia : Maestro
    {
        public string NombreTabla { get; set; }
        public int ProximaSecuencia { get; set; }
        public bool Formato { get; set; }
        public string Prefijo { get; set; }
        public int Logitud { get; set; }
    }
}
