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
    
    public partial class CRETSOLICITUDCREDITOREFCOMERCIAL
    {
        public int IdSolicitudCreditoRefComercial { get; set; }
        public string CodigoEstado { get; set; }
        public int IdSolicitudCredito { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public string NombreOriginal { get; set; }
        public string ArchivoAdjunto { get; set; }
        public Nullable<System.DateTime> FechaConsulta { get; set; }
        public Nullable<System.DateTime> ClienteDesde { get; set; }
        public Nullable<decimal> Cupo { get; set; }
        public Nullable<int> Plazo { get; set; }
        public Nullable<decimal> Garantia { get; set; }
        public Nullable<decimal> PromedioComprasMensual { get; set; }
        public Nullable<decimal> PromedioComprasAnual { get; set; }
        public Nullable<int> PromedioDiasPagos { get; set; }
        public Nullable<System.DateTime> FechaReferenciaComercial { get; set; }
        public string CodigoGarantia { get; set; }
    
        public virtual CRETSOLICITUDCREDITO CRETSOLICITUDCREDITO { get; set; }
    }
}
