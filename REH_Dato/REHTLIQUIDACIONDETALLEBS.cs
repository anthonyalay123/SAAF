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
    
    public partial class REHTLIQUIDACIONDETALLEBS
    {
        public int IdLiquidacionDetalleBS { get; set; }
        public string CodigoEstado { get; set; }
        public int IdLiquidacion { get; set; }
        public string CodigoTipoBS { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string Periodo { get; set; }
        public int Dias { get; set; }
        public decimal Valor { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public Nullable<bool> Provisionado { get; set; }
    
        public virtual REHTLIQUIDACION REHTLIQUIDACION { get; set; }
    }
}
