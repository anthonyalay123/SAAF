using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
   public class UsuarioDepartamentoAsignado : Maestro
    {
        public int idUsuarioDepartamentoAsignado { get; set; }
        public string CodigoUsuario { get; set; }
        public string CodigoDepartamento { get; set; }
        public int idMenu { get; set; }
        public string codigoEstado { get; set; }
    }
}
