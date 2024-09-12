using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Administracion
{
    public class Presentacion
    {
        public int IdPresentacion { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
