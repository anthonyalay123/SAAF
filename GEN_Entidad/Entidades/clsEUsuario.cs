using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
    /// <summary>
    /// Autor: Guillermo Murillo
    /// Fecha: 1/7/2021
    /// Clase del Objeto Usuario
    /// </summary>
    public class Usuario : Maestro
    {
        public string Correo { get; set; }
        public string Clave { get; set; }
        public int IdPerfil { get; set; }
        public string DesPerfil { get; set; }
        public decimal TamanoMB { get; set; }
        public string CodigoDepartamento { get; set; }
        public int IdPersona { get; set; }
        public bool AprobacionFinalCotizacion { get; set; }
        public bool EditaProveedorFormaPago { get; set; }
        public bool EditaTipoOrdenPago { get; set; }
        public bool SuperUsuario { get; set; }
        public TimeSpan HoraInicioNotificacion { get; set; }
        public TimeSpan HoraFinNotificacion { get; set; }
        public int MinFrecuenciaNotificacion { get; set; }
        public int CantMinCotizaciones { get; set; }
        public decimal MontoMax { get; set; }
        public bool VisualizaZonaOrdenPago { get; set; }
        public bool EnviarDesdeCorreoCorporativo { get; set; }
        public string CorreoCorporativo { get; set; }
        public string ClaveDesdeCorreoCorporativo { get; set; }
        public string CodigoUsuarioSap { get; set; }
        public int? BodegaEPP { get; set; }
        public bool ControlaDuplicidadGuias { get; set; }

    }
}
