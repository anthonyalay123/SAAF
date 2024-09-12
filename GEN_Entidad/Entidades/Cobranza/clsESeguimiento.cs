using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Cobranza
{
    public class SeguimientoCompromiso
    {
        public int IdSeguimientoCompromiso { get; set; }
        public string CodigoEstado { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public DateTime FechaGestion { get; set; }
        public string CodCliente { get; set; }
        public string Cliente { get; set; }
        public string CodZona { get; set; }
        public string Zona { get; set; }
        public DateTime FechaPedido { get; set; }
        public int NumCompromiso { get; set; }
        public decimal ValorPedido { get; set; }
        public string Factura { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal Saldo { get; set; }
        public int DiasMora { get; set; }
        public DateTime? FechaCompromiso { get; set; }
        public string Compromiso { get; set; }
        public string CompromisoCumplido { get; set; }
        public string Observaciones { get; set; }
        public string CodigoMotivo { get; set; }
        public string Motivo { get; set; }
        public string Del { get; set; }

    }
}
