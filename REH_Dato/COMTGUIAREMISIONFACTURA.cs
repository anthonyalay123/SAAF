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
    
    public partial class COMTGUIAREMISIONFACTURA
    {
        public int IdGuiaRemisionFactura { get; set; }
        public string CodigoEstado { get; set; }
        public int IdGuiaRemision { get; set; }
        public int IdOrdenPagoFactura { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public string Tipo { get; set; }
        public string Numero { get; set; }
        public string Factura { get; set; }
    
        public virtual COMTGUIAREMISION COMTGUIAREMISION { get; set; }
        public virtual COMTORDENPAGOFACTURA COMTORDENPAGOFACTURA { get; set; }
    }
}
