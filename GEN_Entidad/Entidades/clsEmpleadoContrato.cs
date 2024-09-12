using System;
using System.Collections.Generic;

namespace GEN_Entidad
{
    public class EmpleadoContrato
    {
        public int IdEmpleadoContrato { get; set; }
        public string CodigoEstado { get; set; } 
        public int IdPersona { get; set; }
        public string CodigoSucursal { get; set; }
        public string CodigoDepartamento { get; set; }
        public string CodigoTipoContrato { get; set; }
        public string CodigoCentroCosto { get; set; }
        public Nullable<int> IdPersonaJefe { get; set; }
        public string CodigoCargoLaboral { get; set; }
        public decimal Sueldo { get; set; }
        public Nullable<TimeSpan> InicioHorarioLaboral { get; set; }
        public Nullable<TimeSpan> FinHorarioLaboral { get; set; }
        public DateTime FechaInicioContrato { get; set; }
        public Nullable<DateTime> FechaFinContrato { get; set; }
        public Nullable<DateTime> FechaIngresoMandato8 { get; set; }
        public string CodigoMotivoFinContrato { get; set; }
        public string CodigoTipoComision { get; set; }
        public Nullable<decimal> PorcentajeComision { get; set; }
        public Nullable<decimal> PorcentajeComisionAdicional { get; set; }
        public bool AplicaHorasExtras { get; set; }
        public string CodigoBanco { get; set; }
        public string CodigoFormaPago { get; set; }
        public string CodigoTipoCuentaBancaria { get; set; }
        public string NumeroCuenta { get; set; }
        public bool AplicaIessConyugue { get; set; }
        public bool AcumulaD3 { get; set; }
        public bool AcumulaD4 { get; set; }
        public bool EsJefe { get; set; }
        public bool EsJubilado { get; set; }
        public decimal PorcentajePQ { get; set; }
        public bool AplicaAlimentacion { get; set; }
        public bool AplicaMovilizacion { get; set; }
        public bool DerechoFondoReserva { get; set; }
        public bool SolicitudFondoReserva { get; set; }
        public Nullable<decimal> ValorAlimentacion { get; set; }
        public Nullable<decimal> ValorMovilizacion { get; set; }

        public Empleado Empleado { get; set; }
        public ICollection<EmpleadoContratoCuentaBancaria> EmpleadoContratoCuentaBancaria { get; set; }

    }

    public class EmpleadoContratoCuentaBancaria
    {
        public int IdEmpleadoContratoCuenta { get; set; }
        public int IdEmpleadoContrato { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoTipoRol { get; set; }
        public string CodigoBanco { get; set; }
        public string CodigoFormaPago { get; set; }
        public string CodigoTipoCuentaBancaria { get; set; }
        public string NumeroCuenta { get; set; }
        public string Del { get; set; }

        //public EmpleadoContrato EmpleadoContrato { get; set; }
    }

    public class EmpleadoDescuentoPrestamo
    {
        public string CodigoTipoRol { get; set; }
        public string CodigoRubro { get; set; }
        public bool AplicaDescuentoRol { get; set; }
        public string Del { get; set; }
    }
}
