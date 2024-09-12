using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Credito
{
    public class BandejaInformeRTC
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoSolicitud { get; set; }
        public string Cliente { get; set; }
        public string RepresentanteLegal { get; set; }
        public string EstadoRequerimiento { get; set; }
        public string EstadoInforme { get; set; }
        public string Transaccion { get; set; }
        public string Imprimir { get; set; }
        public int IdInf { get; set; }
    }
}
