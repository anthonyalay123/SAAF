using System;
using System.Collections.Generic;

namespace GEN_Entidad
{
    /// <summary>
    /// Clase de Comision
    /// </summary>
    public class Comision
    {
        public int IdComision { get; set; }
        public string CodigoEstado { get; set; }
        public int IdPeriodo { get; set; }
        public decimal BaseComision { get; set; }
        public decimal TotalAdministrativo { get; set; }
        public decimal TotalCobranza { get; set; }
        public decimal Total { get; set; }
        public string Observacion { get; set; }
        public string UsuarioIngreso { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }

        public  ICollection<ComisionDetalle> ComisionDetalle { get; set; }
    }
}

