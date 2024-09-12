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
    
    public partial class COMTFACTURAPAGO
    {
        public int IdFacturaPago { get; set; }
        public string CodigoEstado { get; set; }
        public int IdProveedor { get; set; }
        public string Identificacion { get; set; }
        public string Proveedor { get; set; }
        public string NumDocumento { get; set; }
        public int DocNum { get; set; }
        public System.DateTime FechaEmision { get; set; }
        public System.DateTime FechaVencimiento { get; set; }
        public decimal Valor { get; set; }
        public decimal Abono { get; set; }
        public decimal Saldo { get; set; }
        public decimal ValorPago { get; set; }
        public string Observacion { get; set; }
        public string Comentario { get; set; }
        public Nullable<int> IdOrdenPagoFactura { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
        public string UsuarioAprobacion { get; set; }
        public Nullable<bool> PagadoTesoreria { get; set; }
        public string UsuarioPagoTesoreria { get; set; }
        public Nullable<System.DateTime> FechaPagoTesoreria { get; set; }
        public Nullable<bool> PagadoFinanciero { get; set; }
        public string UsuarioPagoFinanciero { get; set; }
        public Nullable<System.DateTime> FechaPagoFinanciero { get; set; }
        public string ComentarioTesoreria { get; set; }
        public string ComentarioAprobadorOP { get; set; }
        public string UsuarioAprobadorOP { get; set; }
        public Nullable<int> IdOrdenPago { get; set; }
        public string NombreOriginal { get; set; }
        public string ArchivoAdjunto { get; set; }
        public Nullable<int> IdSemanaPago { get; set; }
        public Nullable<decimal> ValorPagoOriginal { get; set; }
        public string ComentarioAprobador { get; set; }
        public Nullable<int> IdGrupoPagos { get; set; }
        public string GrupoPagos { get; set; }
    }
}
