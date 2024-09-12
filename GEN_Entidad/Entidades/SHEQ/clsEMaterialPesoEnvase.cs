using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.SHEQ
{
    public class MaterialPesoEnvase : Maestro
    {
        public string Material { get; set; }
        public decimal BaseQty { get; set; }
        public decimal PesoEnvase { get; set; }
    }
}
