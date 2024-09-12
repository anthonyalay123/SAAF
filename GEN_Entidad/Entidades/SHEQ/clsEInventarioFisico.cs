using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.SHEQ
{
    public class InventarioFisico
    {
        //public int IdInventarioFisico { get; set; }
        public int IdBodegaEPP { get; set; }
        public string Observaciones { get; set; }
        public virtual ICollection<InventarioFisicoDetalle> InventarioFisicoDetalle { get; set; }
    }

    public class InventarioFisicoDetalle
    {
        //public int IdInventarioFisico { get; set; }
        public int IdItemEPP { get; set; }
        public int Cantidad { get; set; }
        public int CantidadNuevo { get; set; }
        public string Del { get; set; }
    }

}
