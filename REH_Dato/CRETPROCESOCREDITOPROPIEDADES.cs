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
    
    public partial class CRETPROCESOCREDITOPROPIEDADES
    {
        public int IdProcesoCreditoPropiedades { get; set; }
        public string CodigoEstado { get; set; }
        public int IdProcesoCredito { get; set; }
        public string CodigoTipoBien { get; set; }
        public string Direccion { get; set; }
        public string Hipoteca { get; set; }
        public decimal AvaluoComercial { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public Nullable<System.DateTime> FechaPago { get; set; }
        public string DescripcionDocumento { get; set; }
        public string PropietarioBeneficiario { get; set; }
    
        public virtual CRETPROCESOCREDITO CRETPROCESOCREDITO { get; set; }
    }
}
