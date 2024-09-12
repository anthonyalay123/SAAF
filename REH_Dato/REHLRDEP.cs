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
    
    public partial class REHLRDEP
    {
        public int Id { get; set; }
        public string CodigoEstado { get; set; }
        public int Anio { get; set; }
        public int IdPersona { get; set; }
        public string EsbeneficiarioGalapagos { get; set; }
        public string TieneEnfermedadCatastrofica { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string CodigoEstablecimiento { get; set; }
        public string TipoResidencia { get; set; }
        public string PaisResidencia { get; set; }
        public string AplicaConvenioDobleImposicion { get; set; }
        public string CondicionDiscapacidad { get; set; }
        public decimal PorcentajeDiscapacidad { get; set; }
        public string TipoIdentificacionPersonaDiscapacidad { get; set; }
        public string NUmeroIdentificacionPersonaDiscapacidad { get; set; }
        public decimal Sueldos { get; set; }
        public decimal Sobresueldos { get; set; }
        public decimal Utilidades { get; set; }
        public decimal IngresosOtrosEmpleadores { get; set; }
        public decimal ImpuestoRentaAsumidoEmpleador { get; set; }
        public decimal DecimoTercero { get; set; }
        public decimal DecimoCuarto { get; set; }
        public decimal FondoReserva { get; set; }
        public decimal CompensacionEconomicaSalarioDigno { get; set; }
        public decimal OtrosIngresosRelaciónDependenciaNoConstituyenRenta { get; set; }
        public decimal IngresosGravadosEmpleador { get; set; }
        public string TipoSistemaSalarioNeto { get; set; }
        public decimal AporteIESSEmpleador { get; set; }
        public decimal AporteIESSOtrosEmpleadores { get; set; }
        public decimal GastosVivienda { get; set; }
        public decimal GastosSalud { get; set; }
        public decimal GastosEducacion { get; set; }
        public decimal GastosAlimentacion { get; set; }
        public decimal GastosVestimenta { get; set; }
        public decimal GastosTurismo { get; set; }
        public decimal ExoneracionDiscapacidad { get; set; }
        public decimal ExoneracionTerceraEdad { get; set; }
        public decimal BaseImponible { get; set; }
        public Nullable<decimal> BaseImponibleRebajaDsto { get; set; }
        public decimal ImpuestoRentaCausado { get; set; }
        public Nullable<decimal> TotalConExentos { get; set; }
        public Nullable<decimal> RebajaGastosPersonales { get; set; }
        public Nullable<decimal> ImpRentaRebajaGP { get; set; }
        public decimal ImpuestoRentaRetenidoAsumidoOtrosEmpleadores { get; set; }
        public decimal ImpuestoRentaAsumidoEsteEmpleador { get; set; }
        public decimal ImpuestoRentaRetenido { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public Nullable<int> NumeroCargas { get; set; }
    }
}
