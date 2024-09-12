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
    /// Clase del Objeto Periodo
    /// </summary>
    public class Periodo
    {
        public int IdPeriodo { get; set; }
        public string Codigo { get; set; }
        public string CodigoTipoRol { get; set; }
        public string TipoRol { get; set; }
        public int Anio { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime? FechaInicioComi { get; set; }
        public DateTime? FechaFinComi { get; set; }
        public DateTime? FechaInicioHorasExtras { get; set; }
        public DateTime? FechaFinHorasExtras { get; set; }
        public string CodigoEstadoNomina { get; set; }
    }
}
