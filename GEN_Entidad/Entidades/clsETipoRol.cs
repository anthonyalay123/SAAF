using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 06/02/2020
    /// Clase del Objeto Tipo Rol
    /// </summary>
    public class TipoRol : Maestro
    {
        public string CodigoPeriodo { get; set; }
        public int DiaPago { get; set; }
        public int DiaMaxReverso { get; set; }
        public int Dias { get; set; }
        public int OrdenPagoMensual { get; set; }
        public bool DecimoTercero { get; set; }
        public bool ImpuestoRenta { get; set; }
    }
}
