using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.SHEQ
{
    public class TransferenciaStock
    {
        public int IdTransferenciaStock { get; set; }
        public DateTime FechaTransferencia { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int IdBodegaEPPOrigen { get; set; }
        public int IdBodegaEPPDestino { get; set; }
        public string Motivo { get; set; }
        public string GrupoMotivo { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoMotivo { get; set; }
        public string Observaciones { get; set; }
        public string Aprobado { get; set; }
        public string Ver { get; set; }
        public virtual ICollection<TransferenciaStockDetalle> TransferenciaStockDetalle { get; set; }
    }

    public class TransferenciaStockDetalle
    {
        public int IdTransferenciaStockDetalle { get; set; }
        public int IdTransferenciaStock { get; set; }
        public int IdItemEPP { get; set; }
        public int? Stock { get; set; }
        public int IdBodegaEPPOrigen { get; set; }
        public int IdBodegaEPPDestino { get; set; }
        public int Cantidad { get; set; }
        public string Del { get; set; }
    }
}
