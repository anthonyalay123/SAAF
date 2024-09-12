using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.SHEQ
{
    public class EntregaEPP
    {
        public int IdEntregaEPP { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int IdBodega { get; set; }
        public string CodigoEstado { get; set; }
        public int IdEmpleado{ get; set; }
        public string Observaciones{ get; set; }
        public string CentroCosto{ get; set; }
        public string Ver { get; set; }
        public virtual ICollection<EntregaEPPDetalle> EntregaEPPDetalle { get; set; }
    }

    public class EntregaEPPDetalle
    {
        public int IdEntregaEPPDetalle { get; set; }
        public int IdEntregaEPP { get; set; }
        public int Codigo { get { return IdItemEPP; } }
        public int IdItemEPP { get; set; }
        public int IdBodega { get; set; }
        public DateTime FechaEntrega { get; set; }
        public int Cantidad { get; set; }
        public string Del { get; set; }
    }
}
