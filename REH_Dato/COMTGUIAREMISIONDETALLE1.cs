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
    
    public partial class COMTGUIAREMISIONDETALLE1
    {
        public int IdGuiaRemisionDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int IdGuiaRemision { get; set; }
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int CantidadOriginal { get; set; }
        public int CantidadTomada { get; set; }
        public int Saldo { get; set; }
        public int Cantidad { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public string AlmacenOrigen { get; set; }
        public string AlmacenDestino { get; set; }
    
        public virtual COMTGUIAREMISION1 COMTGUIAREMISION { get; set; }
    }
}
