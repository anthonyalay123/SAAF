using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
    public class PaTransaccion : Maestro
    {
        public int CantidadAutorizacion {get; set;}

    }
    public class UsuarioAprobacion : Maestro
    {
        public int inicio { get; set; }

        public int Fin { get; set; }
        public string NombreUsuario { get; set; }
        public int idMenu { get; set; }
    }
}
