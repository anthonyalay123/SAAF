using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
    public class Alimentacion
    {
        public int IdAlimentacion { get; set; }
        public string CodigoEstado { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string CodigoTipoRol { get; set; }
        public string DesTipoRol { get; set; }
        public decimal Total { get; set; }
        public string Observacion { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public int Dias { get; set; }

        public ICollection<AlimentacionDetalle> AlimentacionDetalle { get; set; }
    }

    public class AlimentacionDetalle
    {
        public int IdAlimentacionDetalle { get; set; }
        public int IdAlimentacion { get; set; }
        public int IdPersona { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public int Dias { get; set; }
        public decimal ValorAlimentacion { get; set; }
        public decimal Total { get { return Math.Round(Dias * ValorAlimentacion, 2); } }
        public string Observacion { get; set; }
        
        //public Alimentacion Alimentacion { get; set; }
    }
}
