using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
    public class RubroFijo
    {
        public int IdRubroFijo { get; set; }
        public string CodigoTipoRol { get; set; }
        public int IdPersona { get; set; }
        public string IdPersonaString { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string CodigoRubro { get; set; }
        public decimal Valor { get; set; }
        public string Observacion { get; set; }
        public string Del { get; set; }
    }

}
