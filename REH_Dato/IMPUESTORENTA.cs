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
    
    public partial class IMPUESTORENTA
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int IdPersona { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public decimal SueldoBase { get; set; }
        public decimal SueldoGanado { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal Vacaciones { get; set; }
        public decimal OtrosIngresosIess { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalIngresosAnual { get; set; }
        public decimal Deducibles { get; set; }
        public decimal BaseImponible { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Gravadas { get; set; }
        public decimal NoGravadas { get; set; }
    }
}
