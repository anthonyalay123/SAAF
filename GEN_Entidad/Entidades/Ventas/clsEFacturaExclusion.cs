using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Ventas
{
    public class FacturaExclusion
    {
        public int IdFacturaExclusion { get; set; }
        public string CodigoEstado { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string TipoDocumento { get; set; }
        public int NumFactura { get; set; }
        public string Del { get; set; }
    }
}
