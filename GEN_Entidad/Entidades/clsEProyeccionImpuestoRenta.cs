using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{

    public class ProyeccionImpuestoRenta
    {
        public int IdProyeccionImpuestoRenta { get; set; }
        public string CodigoEstado { get; set; }
        public int IdPersona { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public int Anio { get; set; }
        public decimal Enero { get; set; }
        public decimal Febrero { get; set; }
        public decimal Marzo { get; set; }
        public decimal Abril { get; set; }
        public decimal Mayo { get; set; }
        public decimal Junio { get; set; }
        public decimal Julio { get; set; }
        public decimal Agosto { get; set; }
        public decimal Septiembre { get; set; }
        public decimal Octubre { get; set; }
        public decimal Noviembre { get; set; }
        public decimal Diciembre { get; set; }
        public decimal Utilidades { get; set; }
        public decimal Total { get; set; }
    }

    public class ProyeccionImpuestoRentaGrid
    {
        public int IdProyeccionImpuestoRenta { get; set; }
        public int IdPersona { get; set; }
        public int Periodo { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public decimal Enero { get; set; }
        public decimal Febrero { get; set; }
        public decimal Marzo { get; set; }
        public decimal Abril { get; set; }
        public decimal Mayo { get; set; }
        public decimal Junio { get; set; }
        public decimal Julio { get; set; }
        public decimal Agosto { get; set; }
        public decimal Septiembre { get; set; }
        public decimal Octubre { get; set; }
        public decimal Noviembre { get; set; }
        public decimal Diciembre { get; set; }
        public decimal Utilidades { get; set; }
        public decimal Total
        {
            get
            {
                return Enero + Febrero + Marzo + Abril + Mayo + Junio + Julio + Agosto
+ Septiembre + Octubre + Noviembre + Diciembre + Utilidades;
            }
        }
        public string Del { get; set; }
    }

    public class ProyeccionImpuestoRentaExcel
    {
        //public int Periodo { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public decimal Enero { get; set; }
        public decimal Febrero { get; set; }
        public decimal Marzo { get; set; }
        public decimal Abril { get; set; }
        public decimal Mayo { get; set; }
        public decimal Junio { get; set; }
        public decimal Julio { get; set; }
        public decimal Agosto { get; set; }
        public decimal Septiembre { get; set; }
        public decimal Octubre { get; set; }
        public decimal Noviembre { get; set; }
        public decimal Diciembre { get; set; }
        public decimal Utilidades { get; set; }
        public decimal Total
        {
            get
            {
                return Enero + Febrero + Marzo + Abril + Mayo + Junio + Julio + Agosto
+ Septiembre + Octubre + Noviembre + Diciembre +Utilidades;
            }
        }
    }
}
