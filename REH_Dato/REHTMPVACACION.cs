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
    
    public partial class REHTMPVACACION
    {
        public int Periodo { get; set; }
        public string Cedula { get; set; }
        public string Empleado { get; set; }
        public System.DateTime FechaInicial { get; set; }
        public System.DateTime FechaFinal { get; set; }
        public int DiasNormales { get; set; }
        public int DiasAdicionales { get; set; }
        public int TotalDias { get; set; }
        public int DiasNormalesDevengados { get; set; }
        public int DiasAdicionalesDevengados { get; set; }
        public int TotalDiasDevengados { get; set; }
        public int SaldoDias { get; set; }
        public Nullable<int> DiasLiquidar { get; set; }
        public Nullable<int> DiasGozar { get; set; }
        public string Observacion { get; set; }
    }
}
