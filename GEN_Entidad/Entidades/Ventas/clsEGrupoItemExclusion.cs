using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Ventas
{
    public class GrupoItemExclusion
    {
        public GrupoItemExclusion()
        {
            GrupoItemExclusionDetalle = new List<GrupoItemExclusionDetalle>();
        }
        public int IdGrupoItemExclusion { get; set; }
        public string CodigoEstado { get; set; }
        public short ItmsGrpCod { get; set; }
        public string ItmsGrpNam { get; set; }
        public ICollection<GrupoItemExclusionDetalle> GrupoItemExclusionDetalle { get; set; }
        public string Det { get; set; }
        public string Del { get; set; }
    }

    public class GrupoItemExclusionDetalle
    {
        public int IdGrupoItemExclusionDetalle { get; set; }
        public int IdGrupoItemExclusion { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public string Del { get; set; }
    }
}
