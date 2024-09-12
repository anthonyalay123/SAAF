using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Credito
{
    public class CheckListParametro
    {
        public int Id { get; set; }
        public string CodigoTipoSolicitud { get; set; }
        public string CodigoTipoPersona { get; set; }
        public string CodigoContado { get; set; }
        public string CodigoEstatusSeguro { get; set; }
        public int IdCheckList { get; set; }
        public string CheckList { get; set; }
        public string Necesario { get; set; }

        public string Del { get; set; }
    }
}
