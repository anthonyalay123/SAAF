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
    
    public partial class VTATREBATECLIENTEANUALDETALLE
    {
        public int IdRebateClienteAnualDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int IdRebateClienteAnual { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public string CodeCliente { get; set; }
        public string NameCliente { get; set; }
        public decimal Presupuesto { get; set; }
        public decimal VentaNeta { get; set; }
        public int CantFacturas { get; set; }
        public int CantFacturasPagadas { get; set; }
        public decimal PorcCumplimientoMeta { get; set; }
        public decimal PorcMargenRentabilidad { get; set; }
        public int DiasMora { get; set; }
        public Nullable<decimal> PorcentajeRebate { get; set; }
        public decimal ValorRebate { get; set; }
        public string Observacion { get; set; }
        public string CodigoEstadoRebate { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public Nullable<int> CantFacturasPendientes { get; set; }
        public Nullable<System.DateTime> FechaCobranza { get; set; }
        public Nullable<bool> RegistradoCobranza { get; set; }
        public string UsuarioCobranza { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
        public string UsuarioAprobacion { get; set; }
        public string NombreOriginal { get; set; }
        public string ArchivoAdjunto { get; set; }
    
        public virtual VTATREBATECLIENTEANUAL VTATREBATECLIENTEANUAL { get; set; }
    }
}
