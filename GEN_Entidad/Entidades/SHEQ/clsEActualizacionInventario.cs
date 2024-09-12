using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.SHEQ
{
    public class ActualizacionInventario
    {
        public int IdActualizacionInventario { get; set; }
        public int IdBodegaEPP { get; set; }
        public string Observaciones { get; set; }
        public DateTime Fecha { get; set; }

        public virtual ICollection<ActualizacionInventarioDetalle> ActualizacionInventarioDetalle { get; set; }
    }

    public class ActualizacionInventarioDetalle
    {
        public int IdActualizacionInventarioDetalle { get; set; }
        public int IdActualizacionInventario { get; set; }
        public int IdItemEPP { get; set; }
        public string Descripcion { get; set; }
        public int IdBodegaEPP { get; set; }
        public int StockAnterior { get; set; }
        public int StockNuevo { get; set; }
        public int StockNuevoAgregar { get; set; }
       
    }

}
