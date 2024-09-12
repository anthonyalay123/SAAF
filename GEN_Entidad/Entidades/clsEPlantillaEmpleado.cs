using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 17/08/2020
    /// Clase para la carga de empleados
    /// </summary>
    public class PlantillaEmpleado
    {
        public string Identificacion { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string CorreoPersonal { get; set; }
        public string CorreoLaboral { get; set; }
        public string Genero { get; set; }
        public string LugarNacimiento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Region { get; set; }
        public string TipoEmpleado { get; set; }
        public string Sucursal { get; set; }
        public string Departamento { get; set; }
        public string TipoContrato { get; set; }
        public string CentrodeCosto { get; set; }
        public string CargoLaboral { get; set; }
        public decimal Sueldo { get; set; }
        public decimal PorcentajePrimeraQuincena { get; set; }
        public DateTime FechaInicioContrato { get; set; }
        public Nullable<DateTime> FechaFindeContrato { get; set; }
        public string TipodeComision { get; set; }
        public Nullable<decimal> PorcentajedeComision { get; set; }
        public bool AplicaHorasExtras { get; set; }
        public string Banco { get; set; }
        public string FormadePago { get; set; }
        public string TipoCuenta { get; set; }
        public string Cuenta { get; set; }
        public bool AplicaIessConyugue { get; set; }
        public bool AcumulaDecimoTercero { get; set; }
        public bool AcumulaDecimoCuarto { get; set; }
        public bool EsJefe { get; set; }
        public bool EsJubilado { get; set; }
        public decimal ValorAlimentacion { get; set; }
        public decimal ValorMovilizacion { get; set; }
        public string EstadoCivil { get; set; }
        public string EstadoContrato { get; set; }
        public bool SolicitudFondoReserva { get; set; }
        public bool DerechoFondoReserva { get; set; }

    }
}
