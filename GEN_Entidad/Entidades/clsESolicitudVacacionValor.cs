using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 31/05/2021
    /// Clase del Objeto Solicitud de Vacación Detalle Valor
    /// </summary>
    public class SolicitudVacacionValor
    {
        public int IdSolicitudVacacionValor { get; set; }
        public string CodigoEstado { get; set; }
        public int IdSolicitudVacacion { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public decimal Valor { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }

        public SolicitudVacacion SolicitudVacacion { get; set; }
    }
}
