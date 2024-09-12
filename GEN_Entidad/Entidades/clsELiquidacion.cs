using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
    public class Liquidacion
    {
        public int IdLiquidacion { get; set; }
        public string CodigoEstado { get; set; }
        public int IdPersona { get; set; }
        public int IdEmpleadoContrato { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; }
        public DateTime FechaInicioContrato { get; set; }
        public DateTime FechaFinContrato { get; set; }
        public string Sucursal { get; set; }
        public string DesMotivo { get; set; }
        public string Motivo { get; set; }
        public decimal SueldoBase { get; set; }
        public decimal PorcComision { get; set; }
        public decimal UltimoSueldo { get; set; }
        public decimal MejorSueldo { get; set; }
        public decimal Total { get; set; }
        public decimal ValorRolComisiones { get; set; }
        public DateTime FechaCalculo { get; set; }
        public string Observacion { get; set; }
        public string UsuarioIngreso { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public string UsuarioAprobacion { get; set; }
        public Nullable<DateTime> FechaAprobacion { get; set; }
    }

    public class BandejaLiquidacion
    {
        public bool Sel { get; set; }
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string Empleado { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaFiniquito { get; set; }
        public decimal Total { get; set; }
        public string Comentario { get; set; }
        public string Estado { get; set; }
        public string VerComentarios { get; set; }
        public string Ver { get; set; }
        public string Imprimir { get; set; }
    }

    public class BandejaLiquidacionExcel
    {
        
        public int No { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string Empleado { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaFiniquito { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
    }
}
