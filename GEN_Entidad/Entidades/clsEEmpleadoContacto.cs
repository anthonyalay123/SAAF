using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Clase Empleado Contacto
    /// 
    /// </summary>
    public class EmpleadoContacto
    {
        public int IdEmpleadoContacto { get; set; }
        public string CodigoEstado { get; set; }
        public int IdPersona { get; set; }
        public string CodigoTipoContacto { get; set; }
        public string CodigoParentezco { get; set; }
        public string NombreCompleto { get; set; }
        public string Direccion { get; set; }
        public string TelefonoConvencional { get; set; }
        public string TelefonoCelular { get; set; }
        public string Correo { get; set; }
        public string Observacion { get; set; }
        public string UsuarioIngreso { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public Empleado Empleado { get; set; }
    }
}
