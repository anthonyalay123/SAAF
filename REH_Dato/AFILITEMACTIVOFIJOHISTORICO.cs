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
    
    public partial class AFILITEMACTIVOFIJOHISTORICO
    {
        public int Id { get; set; }
        public string CodigoEstado { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string Agrupacion { get; set; }
        public int IdItemActivoFijo { get; set; }
        public string Item { get; set; }
        public string CodigoTipoActivoFijo { get; set; }
        public string TipoActivoFijo { get; set; }
        public string Tipo { get; set; }
        public string CodigoSucursal { get; set; }
        public string Sucursal { get; set; }
        public string CodigoCentroCosto { get; set; }
        public string CentroCosto { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorResidual { get; set; }
        public decimal ValorDepreciable { get; set; }
        public decimal ValorDepreciado { get; set; }
        public decimal ValorPorDepreciar { get; set; }
        public decimal DepreciacionMensual { get; set; }
        public decimal ValorActual { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
    }
}
