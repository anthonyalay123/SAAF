using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
    public class EnvioCorreo : Maestro
    {
        
        
     
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public string Transaccion { get; set; }
        public bool CCUsuario { get; set; }
        public string CCCorreo { get; set; }

    }
}
