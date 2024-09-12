using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Ventas
{
    public class GrupoCliente : Maestro
    {
        public int IdGrupoCliente { get; set; }
        public string Nombre { get; set; }
        public List<GrupoClienteDetalle> GrupoClienteDetalle { get; set; }
    }

    public class GrupoClienteDetalle
    {
        public int IdGrupoClienteDetalle { get; set; }
        public int IdGrupoCliente { get; set; }
        public string Cliente { get; set; }
        public string NameCliente { get; set; }
        public string Del { get; set; }
    }
}
