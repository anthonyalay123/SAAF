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
    /// Clase del Objeto Solicitud de Vacación
    /// </summary>
    public class SolicitudVacacion : Maestro
    {
        public int Id { get; set; }
        public string CodigoTipoVacacion { get; set; }
        public int IdPersona { get; set; }
        public int Dias { get; set; }
        public int IdEmpleadoContrato { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string DesPersona { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string DesTipoVacacion { get; set; }
        public string Observacion { get; set; }
        public string Reemplazo { get; set; }
        public bool PagarReemplazo { get; set; }
        public Nullable<int> IdPersonaReemplazo { get; set; }

        public ICollection<SolicitudVacacionDetalle> SolicitudVacacionDetalle { get; set; }
        public ICollection<SolicitudVacacionValor> SolicitudVacacionValor { get; set; }
        public ICollection<SolicitudVacacionSaldos> SolicitudVacacionSaldos { get; set; }
    }

    public class BandejaSolicitudVacaciones
    {
        public bool Sel { get; set; }
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Usuario { get; set; }
        public string Empleado { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Dias { get; set; }
        public string Comentario { get; set; }
        public string Estado { get; set; }
        public string VerComentarios { get; set; }
        public string Ver { get; set; }
        public string Imprimir { get; set; }
    }

    public class BandejaSolicitudVacacionesExcel
    {

        public int No { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string Empleado { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Dias { get; set; }
        public string Estado { get; set; }
    }
}
