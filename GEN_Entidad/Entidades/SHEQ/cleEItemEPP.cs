using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.SHEQ
{
    public class ItemEPP
    {
        public int IdItemEPP { get; set; }
        public string Descripcion { get; set; }
        public decimal? Costo { get; set; }
        public string GrabaIva { get; set; }
    }
}
