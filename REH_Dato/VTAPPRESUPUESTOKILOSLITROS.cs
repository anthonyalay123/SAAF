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
    
    public partial class VTAPPRESUPUESTOKILOSLITROS
    {
        public int IdPresupuestoKilosLitros { get; set; }
        public string CodigoEstado { get; set; }
        public int Periodo { get; set; }
        public int Mes { get; set; }
        public int IdZona { get; set; }
        public string Zona { get; set; }
        public string Familia { get; set; }
        public short ItmsGrpCod { get; set; }
        public string ItmsGrpNam { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Unidades { get; set; }
        public decimal MedidaConversion { get; set; }
        public decimal Total { get; set; }
        public string TipoProducto { get; set; }
        public string Observacion { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
    }
}
