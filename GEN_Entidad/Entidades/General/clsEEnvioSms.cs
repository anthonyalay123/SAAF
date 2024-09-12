using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.General
{
    public class EnvioSms
    {
        public int Id { get; set; }
        public int DocEntry { get; set; }
        public int Hora { get; set; }
        public int Time { get; set; }
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public decimal DocTotal { get; set; }
        public string FormaPago { get; set; }
        public string Movil { get; set; }
        public string Destinatario { get; set; }
        public string TextoEmail { get; set; }
        public bool PrevioEnvio { get; set; }
        public bool PostEnvio { get; set; }
        public string MsgError { get; set; }
        public Nullable<System.DateTime> FechaIngreso { get; set; }
    }

}
