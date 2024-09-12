using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
    public class DescuentoDiscapacitadoIr : Maestro
    {
        public int Anio { get; set; }
        public decimal PorcentajeInicial { get; set; }
        public decimal PorcentajeFinal { get; set; }
        public decimal PorcentajeDescuento { get; set; }
    }
}
