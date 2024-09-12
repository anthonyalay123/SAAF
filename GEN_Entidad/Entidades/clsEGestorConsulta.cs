using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
    public class GestorConsulta : Maestro
    {
        public int IdGestorConsulta { get; set; }
        public string Nombre { get; set; }
        public bool botonImprimir { get; set; }
        public string DataSet { get; set; }
        public string TituloReporte { get; set; }
        public Nullable<int> FixedColumn { get; set; }
        public List<GestorConsultaDetalle> Detalle { get; set; }
        
    }

    public class GestorConsultaDetalle 
    {
        public int IdDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int IdGestorConsulta { get; set; }
        public int Orden { get; set; }
        public string Nombre { get; set; }
        public string TipoDato { get; set; }
        public string Formato { get; set; }
        public string PlaceHolder { get; set; }
        public int Longitud { get; set; }
        public string Delete { get; set; }
    }

    public class LogAceptaRolEmpleado
    {
        public int IdPersona { get; set; }
        public string Empleado { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string Rol { get; set; }
        public string Enviado { get; set; }
        //public DateTime Fecha { get; set; }
        public string Aceptado { get; set; }
    }
}
