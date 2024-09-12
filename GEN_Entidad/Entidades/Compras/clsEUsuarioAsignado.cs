using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Compras
{
    public class UsuarioAsignado
    {
        public int idMenu { get; set; }
        public string Menu { get; set; }
        public string codigoUsuarioPrincipal { get; set; }
        public string UsuarioPrincipal { get; set; }
        public string CodigoUsuarioAsignado { get; set; }
        public int IdPersona { get; set; }
        public string UsuarioAsi { get; set; }
        public string CodigoEstado { get; set; }
        public string Estado { get; set; }
    }
}
