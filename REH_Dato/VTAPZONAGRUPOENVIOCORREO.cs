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
    
    public partial class VTAPZONAGRUPOENVIOCORREO
    {
        public int IdZonaGrupoEnvioCorreo { get; set; }
        public string CodigoEstado { get; set; }
        public int IdZonaGrupo { get; set; }
        public string TipoDestinatario { get; set; }
        public int IdPersona { get; set; }
        public string CorreoManual { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
    
        public virtual VTAPZONAGRUPO VTAPZONAGRUPO { get; set; }
    }
}
