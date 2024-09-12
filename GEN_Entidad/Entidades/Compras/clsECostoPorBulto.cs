using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Compras
{
    public class CostoPorBulto
    {
        public int IdCostoPorBulto { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoTipoTransporte{ get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public decimal Costo { get; set; }
        public string Del { get; set; }
    }
}
