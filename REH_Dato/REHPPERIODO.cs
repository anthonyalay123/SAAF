//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace REH_Dato
{
    using System;
    using System.Collections.Generic;
    
    public partial class REHPPERIODO
    {
        public int IdPeriodo { get; set; }
        public string CodigoEstado { get; set; }
        public string Codigo { get; set; }
        public string CodigoTipoRol { get; set; }
        public int Anio { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public System.DateTime FechaFin { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public string CodigoEstadoNomina { get; set; }
        public Nullable<System.DateTime> FechaInicioComi { get; set; }
        public Nullable<System.DateTime> FechaFinComi { get; set; }
        public Nullable<System.DateTime> FechaInicioHorasExtras { get; set; }
        public Nullable<System.DateTime> FechaFinHorasExtras { get; set; }
        public Nullable<bool> CerradoDiarioProvision { get; set; }
        public Nullable<System.DateTime> FechaInicioPermisosHoras { get; set; }
        public Nullable<System.DateTime> FechaFinPermisosHoras { get; set; }
    }
}
