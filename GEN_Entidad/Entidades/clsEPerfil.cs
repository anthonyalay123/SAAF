using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
    public class Perfil : Maestro
    {   public int IdPerfil { get; set; }
        public string CodigoFlujoCompras { get; set; }
        public List<PerfilFlujo> Flujos{ get; set; }
}

    public class PerfilFlujo
    {
        public int IdPerfilFlujo { get; set; }
        public int IdPerfil{ get; set; }
        public string CodigoFlujo   { get; set; }

        public string Del { get; set; }
    }


}
