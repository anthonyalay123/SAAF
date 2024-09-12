using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    public class PersonaDireccion
    {
        public int IdPersonaDireccion { get; set; }
        public string CodigoEstado { get; set; }
        public int IdPersona { get; set; }
        public int IdPais { get; set; }
        public int IdProvincia { get; set; }
        public Nullable<int> IdCanton { get; set; }
        public Nullable<int> IdParroquia { get; set; }
        public string direccion { get; set; }
        public string Referencia { get; set; }
        public string TelfonoConvencional { get; set; }
        public string TelfonoCelular { get; set; }
        public bool Principal { get; set; }
        public string UsuarioIngreso { get; set; }
        public string GeoReferenciacion { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }

        public Persona Persona { get; set; }
    }
}
