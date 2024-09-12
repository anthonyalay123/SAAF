using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 26/05/2021
    /// Clase del Objeto Solicitud de Vacación Detalle
    /// </summary>
    public class SolicitudVacacionDetalle
    {
        public int IdSolicitudVacacionDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int IdSolicitudVacacion { get; set; }
        public int IdEmpleadoContrato { get; set; }
        public int Periodo { get; set; }
        public int Dias { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public decimal Valor { get; set; }
        public int DiasSaldo { get; set; }
        public string UsuarioIngreso { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }

        public SolicitudVacacion SolicitudVacacion { get; set; }
    }

    public class SolicitudVacacionSaldos
    {
        public int IdSolicitudVacacionSaldos { get; set; }
        public string CodigoEstado { get; set; }
        public int IdSolicitudVacacion { get; set; }
        public int Periodo { get; set; }
        public decimal Valor { get; set; }
        public SolicitudVacacion SolicitudVacacion { get; set; }
    }

    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 26/05/2021
    /// Clase del Objeto Solicitud de Vacación Detalle Grid
    /// </summary>
    public class SolicitudVacacionDetalleGridView
    {
        public int Periodo { get; set; }
        public int Dias { get; set; }
        public decimal Valor { get; set; }
        public int DiasSaldo { get; set; }
    }
}
