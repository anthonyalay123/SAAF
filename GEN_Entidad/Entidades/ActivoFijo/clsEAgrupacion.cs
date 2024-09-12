using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.ActivoFijo
{
    public class Agrupacion
    {
        public string CodigoAgrupacion { get; set; }
        public string Descripcion { get; set; }
        public string CuentaDepreciacionDebito { get; set; }
        public string CuentaDepreciacionCredito { get; set; }
        public string CuentaBajaDebito { get; set; }
        public string CuentaBajaCredito { get; set; }
        public string CuentaVentaSuperiorDebito { get; set; }
        public string CuentaVentaSuperiorCredito { get; set; }
        public string CuentaVentaInferiorDebito { get; set; }
        public string CuentaVentaInferiorCredito { get; set; }
        public string Del { get; set; }
    }
}
