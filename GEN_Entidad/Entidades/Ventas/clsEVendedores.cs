using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Ventas
{
    public class VendedorGrupo
    {
        public int IdZonaGrupo { get; set; }
        public int IdVendedorGrupo { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> IdPersona { get; set; }
        public string Det { get; set; }
        public string Del { get; set; }
        public virtual ICollection<VendedorGrupoDetalle> VendedorGrupoDetalle { get; set; }
    }

    public class VendedorGrupoDetalle
    {
        public int IdVendedorGrupoDetalle { get; set; }
        public int IdVendedorGrupo { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public string Del { get; set; }
    }
}
