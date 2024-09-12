using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.REH
{
    public class GestorConsultaPerfil: Maestro
    {
        public int IdGestorConsulta { get; set; }
        public int IdPerfil { get; set; }
        public string DesGestorConsulta { get; set; }
        public string DesPerfil { get; set; }
    }
}
