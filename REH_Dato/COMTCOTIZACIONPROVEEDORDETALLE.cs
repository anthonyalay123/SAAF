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
    
    public partial class COMTCOTIZACIONPROVEEDORDETALLE
    {
        public int IdCotizacionProveedorDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int IdCotizacionProveedor { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public decimal ValorUnitario { get; set; }
        public bool GrabaIva { get; set; }
        public decimal ValorIva { get; set; }
        public decimal Total { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public string ItemCode { get; set; }
        public string ArchivoAdjunto { get; set; }
    
        public virtual COMTCOTIZACIONPROVEEDOR COMTCOTIZACIONPROVEEDOR { get; set; }
    }
}
