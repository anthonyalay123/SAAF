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
    
    public partial class CREPPARAMETRO
    {
        public int Id { get; set; }
        public string CodigoEstado { get; set; }
        public int DiasVigenciaFechaReferenciaComercial { get; set; }
        public int DiasVigenciaFechaPlanillaServiciosBasicos { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public Nullable<decimal> CupoMinimoEnvioSeguro { get; set; }
        public string AsuntoCorregirInforme { get; set; }
        public string TextoCorregirInforme { get; set; }
        public Nullable<bool> EnviarCorreoCorregirRTC { get; set; }
        public Nullable<int> DiasAlarmaResolucion { get; set; }
        public Nullable<int> DiasAlarmaRequerimiento { get; set; }
        public Nullable<int> DiasAlarmaFechaCompromiso { get; set; }
    }
}
