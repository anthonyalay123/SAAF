using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.REH
{
    public class BandejaPermisoHoras
    {
        public bool Sel { get; set; }
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Empleado { get; set; }
        public string TipoPermiso { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string Observacion { get; set; }
        public string Estado { get; set; }
        public string Aprobaciones { get; set; }
        public string Aprobadores { get; set; }
        public string Ver { get; set; }
        public string Imprimir { get; set; }
    }
}
