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
    
    public partial class REHPPERMISO
    {
        public int Id { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoTipoPermiso { get; set; }
        public int IdPersona { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public System.DateTime FechaFin { get; set; }
        public Nullable<int> IdPersonaCubre { get; set; }
        public string Observacion { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public Nullable<int> IdSolicitudVacacion { get; set; }
    }
}
