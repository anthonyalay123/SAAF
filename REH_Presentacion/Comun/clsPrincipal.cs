using GEN_Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REH_Presentacion
{

    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 21/01/2020
    /// Clase principal donde se encontraran variables globales para el sistema
    /// </summary>
    public static class clsPrincipal
    {
        public static string gsUsuario { get; set; }
        public static string gsDesUsuario { get; set; }
        public static string gsDesDepartamento { get; set; }
        public static string gsCorreo{ get; set; }
        public static string gsTerminal { get; set; }
        public static int gIdPerfil { get; set; }
        public static string gsDesPerfil { get; set; }
        public static decimal gdcTamanoMb { get; set; }
        public static List<MenuAccionPerfil> gsAccionPerfil { get; set; }
        public static decimal gdcIva { get; set; }
        public static TimeSpan HoraInicioNotificacion { get; set; }
        public static TimeSpan HoraFinNotificacion { get; set; }
        public static int MinFrecuenciaNotificacion { get; set; }
        public static string gsRucEmpresa { get; set; }
        public static bool gbEditaProveedorFormaPago { get; set; }
        public static bool gbEditaTipoOrdenPago { get; set; }
        public static bool gbSuperUsuario { get; set; }
        public static bool gbEnviarDesdeCorreoCorporativo { get; set; }
        public static string gsCodigoUsuarioSap { get; set; }

    }
}
