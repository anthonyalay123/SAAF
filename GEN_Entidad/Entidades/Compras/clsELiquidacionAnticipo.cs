using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Compras
{
    public class LiquidacionAnticipo
    {
        public LiquidacionAnticipo()
        {
            this.LiquidacionAnticipoDetalle = new List<LiquidacionAnticipoDetalle>();
            this.LiquidacionAnticipoSolicitud = new List<LiquidacionAnticipoSolicitud>();
        }

        public int IdLiquidacionAnticipo { get; set; }
        public string CodigoEstado { get; set; }
        public int IdProveedor { get; set; }
        public string Proveedor { get; set; }
        public string CardCode { get; set; }
        public System.DateTime FechaLiquidacion { get; set; }
        public string Observacion { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal TotalAnticipo { get; set; }
        public decimal ValorDevolver { get; set; }
        public decimal ValorReponer { get; set; }
        public string Usuario { get; set; }
        public ICollection<LiquidacionAnticipoDetalle> LiquidacionAnticipoDetalle { get; set; }
        public ICollection<LiquidacionAnticipoSolicitud> LiquidacionAnticipoSolicitud { get; set; }
        
    }

    public partial class LiquidacionAnticipoDetalle
    {
        public int IdLiquidacionAnticipoDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int IdLiquidacionAnticipo { get; set; }
        public DateTime Fecha { get; set; }
        [MaxLength(20)]
        public string NumeroDocumento { get; set; }
        [MaxLength(200)]
        public string Proveedor { get; set; }
        [MaxLength(500)]
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }
        public string Del { get; set; }
        public LiquidacionAnticipo LiquidacionAnticipo { get; set; }
    }

    public partial class LiquidacionAnticipoSolicitud
    {
        public int IdLiquidacionAnticipoSolicitud { get; set; }
        public string CodigoEstado { get; set; }
        public int IdLiquidacionAnticipo { get; set; }
        public int IdSolicitudAnticipo { get; set; }
        public DateTime FechaAnticipo { get; set; }
        public string Proveedor { get; set; }
        public Decimal Valor { get; set; }
        public string Del { get; set; }
        public LiquidacionAnticipo LiquidacionAnticipo { get; set; }
        public SolicitudAnticipo SolicitudAnticipo { get; set; }
    }

    public class SpBandejaLiquidacionAnticipo
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Proveedor { get; set; }
        public DateTime FechaLiquidacion { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal TotalAnticipo { get; set; }
        public string Aprobaciones { get; set; }
        public string Aprobo { get; set; }
        public string Estado { get; set; }
        public string Observacion { get; set; }
        public string Ver { get; set; }
        public string VerComentarios { get; set; }

        //public string ArchivoAdjunto { get; set; }
        //public string RutaOrigen { get; set; }
        //public string RutaDestino { get; set; }
        //public string NombreOriginal { get; set; }
    }

    public class SpListadoLiquidacionAnticipo
    {
        public int Id { get; set; }
        public DateTime FechaLiquidacion { get; set; }
        public string Usuario { get; set; }
        public string Proveedor { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal TotalAnticipo { get; set; }
        public string Observacion { get; set; }
        public string Estado { get; set; }
        public string VerComentarios { get; set; }
        public string Ver { get; set; }
        public string Imprimir { get; set; }
    }

}
