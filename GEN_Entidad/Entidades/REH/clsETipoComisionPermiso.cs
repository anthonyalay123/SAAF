using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.REH
{
   public class TipoComisionPermiso : Maestro
    {
        public string CodigoTipoComision { get; set; }
        public string CodigoTipoPermiso { get; set; }
        public bool DescuentoValorComision { get; set; }
        public string CodCodigoTipoPermiso { get; set; }
        public string CodCodigoTipoComision { get; set; }

    }
}
