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
    
    public partial class REHTLIQUIDACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public REHTLIQUIDACION()
        {
            this.REHTLIQUIDACIONDETALLEBS = new HashSet<REHTLIQUIDACIONDETALLEBS>();
            this.REHTLIQUIDACIONDETALLEROL = new HashSet<REHTLIQUIDACIONDETALLEROL>();
            this.REHTLIQUIDACIONDETALLEVAC = new HashSet<REHTLIQUIDACIONDETALLEVAC>();
            this.REHTLIQUIDACIONRESUMEN = new HashSet<REHTLIQUIDACIONRESUMEN>();
        }
    
        public int IdLiquidacion { get; set; }
        public string CodigoEstado { get; set; }
        public int IdPersona { get; set; }
        public int IdEmpleadoContrato { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; }
        public System.DateTime FechaInicioContrato { get; set; }
        public System.DateTime FechaFinContrato { get; set; }
        public string Sucursal { get; set; }
        public string Motivo { get; set; }
        public decimal SueldoBase { get; set; }
        public decimal PorcComision { get; set; }
        public decimal UltimoSueldo { get; set; }
        public decimal MejorSueldo { get; set; }
        public System.DateTime FechaCalculo { get; set; }
        public string Observacion { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public string UsuarioAprobacion { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
        public string CodigoMotivo { get; set; }
        public decimal Total { get; set; }
        public Nullable<decimal> ValorRolComisiones { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REHTLIQUIDACIONDETALLEBS> REHTLIQUIDACIONDETALLEBS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REHTLIQUIDACIONDETALLEROL> REHTLIQUIDACIONDETALLEROL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REHTLIQUIDACIONDETALLEVAC> REHTLIQUIDACIONDETALLEVAC { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REHTLIQUIDACIONRESUMEN> REHTLIQUIDACIONRESUMEN { get; set; }
    }
}
