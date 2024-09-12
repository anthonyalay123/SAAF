using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    public class EmpleadoCargaFamiliar
    {
        public int IdEmpleadoCargaFamiliar { get; set; }
        public string CodigoEstado { get; set; }
        public int IdPersona { get; set; }
        public string CodigoTipoCargaFamiliar { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime Fecha { get; set; }
        public string CodigoTipoGenero { get; set; }
        public bool Discapacitado { get; set; }
        public string Adjunto { get; set; }
        public Empleado Empleado { get; set; }
        public bool Sustituto { get; set; }
        public bool Fallecido { get; set; }
        public bool AplicaCargaFamiliar { get; set; }
        public bool CargaFamiliarIR { get; set; }
        public bool EnfermedadCatastrofica { get; set; }
        public string Del { get; set; }
        
    }
}
