using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 27/02/2020
    /// Clase del Objeto Rubro
    /// </summary>
    public class Rubro : Maestro
    {
        public string CodigoTipoRubro { get; set; }
        public string DescripcionTipoRubro { get; set; }
        public string CodigoCategoriaRubro { get; set; }
        public string DescripcionCategoriaRubro { get; set; }
        public bool Aportable { get; set; }
        public bool ImpuestoRenta { get; set; }
        public bool NovedadEditable { get; set; }
        public bool EsEntero { get; set; }
        public string CodigoTipoContabilizacion { get; set; }
        public string CodigoTipoMovimiento { get; set; }
        public int Orden { get; set; }
        public string Formula { get; set; }
    }
}
