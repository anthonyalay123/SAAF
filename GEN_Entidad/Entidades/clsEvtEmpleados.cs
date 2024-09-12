using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
    public class VtEmpleados
    {
        public int IdPersona { get; set; }
        public int IdEmpleadoContrato { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public string Genero { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public string Region { get; set; }
        public string RutaImagen { get; set; }
        public string CorreoPersonal { get; set; }
        public string CorreoLaboral { get; set; }
        public string TipoEmpleado { get; set; }
        public string Sucursal { get; set; }
        public string CentroCosto { get; set; }
        public string Departamento { get; set; }
        public string CargoLaboral { get; set; }
        public System.DateTime FechaInicioContrato { get; set; }
        public Nullable<System.DateTime> FechaFinContrato { get; set; }
        public string MotivoFinContrato { get; set; }
        public decimal Sueldo { get; set; }
        public string Banco { get; set; }
        public string FormaPago { get; set; }
        public string NumeroCuenta { get; set; }
        public string TipoComision { get; set; }
        public Nullable<decimal> PorcentajeComision { get; set; }
        public Nullable<decimal> PorcentajeComisionAdicional { get; set; }
        public string Discapacidad { get; set; }
        public bool EsJubilado { get; set; }
        public bool AcumulaD13 { get; set; }
        public bool AcumulaD14 { get; set; }
    }

}
