using System;
namespace GEN_Entidad
{
    /// <summary>
    /// Clase de Comision
    /// </summary>
    public class ComisionDetalle
    {
        public int IdComisionDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int IdComision { get; set; }
        public int IdPersona { get; set; }
        public decimal Valor { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }

        public Comision Comision { get; set; }
    }
}

