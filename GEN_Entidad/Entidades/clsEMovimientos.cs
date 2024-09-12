using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
    public class Movimientos
    {
        public int IdMovimiento { get; set; }
        public string CodigoEstado { get; set; }
        public int IdPersona { get; set; }
        public int IdEmpleadoContrato { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public string CodigoTipoMovimiento { get; set; }
        public string TipoMovimiento { get; set; }
        public System.DateTime Fecha { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public decimal Valor { get; set; }
        public string Observacion { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
    }

    public class MovimientosGrid
    {
        public int IdMovimiento { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string CodigoTipoMovimiento { get; set; }
        public string Cedula { get; set; }
        public string Empleado { get; set; }
        public decimal Valor { get; set; }
        public string Observacion { get; set; }
        public string Eliminar { get; set; }
        public int IdNomina { get; set; }
        public int IdReferencial { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string CodigoRubro { get; set; }
        public string Rubro { get; set; }
        public string CodigoTipoPrestamo { get; set; }
        public string TipoPrestamo { get; set; }

    }

    public class MovimientosExcel
    {
        public string Cedula { get; set; }
        public string Empleado { get; set; }
        public string Observacion { get; set; }
        public decimal Valor { get; set; }     
    }

    public class MovimientosExport
    {
        public DateTime FechaIngreso { get; set; }
        public string TipoMovimiento { get; set; }
        public string Cedula { get; set; }
        public string Empleado { get; set; }
        public decimal Valor { get; set; }
        public string Observacion { get; set; }

    }

    public class SpConsultaMovimientosResumen
    {
        public string Cedula { get; set; }
        public string Empleado { get; set; }
        public string TipoMovimiento { get; set; }
        public decimal SaldoPlanilla { get; set; }
        public decimal ValorDescontar { get; set; }
        public decimal Saldo { get; set; }
    }

    public class TablaAmortizacionPI
    {
        public int Id { get; set; }
        public int Cuota { get; set; }
        [MaxLength(4)]
        public int Año { get; set; }
        [MaxLength(2)]
        public int Mes { get; set; }
        public string Rol { get; set; }
        public string Periodo { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }
        public string Estado { get; set; }
        public string Del { get; set; }
        public int AñoMes { get { return (Año * 100) + Mes; } }
    }

    public class SpCapacidadEndeudamiento
    {
        public decimal Ingresos { get; set; }
        public decimal Egresos { get; set; }
        public decimal CapacidadEndeudamiento { get; set; }
    }

    public class PrestamoInterno
    {
        public int IdPrestamoInterno { get; set; }
        public string CodigoEstado { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public int IdEmpleadoContrato { get; set; }
        public int IdPersona { get; set; }
        public string DesPersona { get; set; }
        public DateTime FechaInicioContrato { get; set; }
        public string Ciudad { get; set; }
        public decimal Ingresos { get; set; }
        public decimal Egresos { get; set; }
        public decimal CapacidadEndeudamiento { get; set; }
        public decimal ValorCredito { get; set; }
        public int Plazo { get; set; }
        public string CodigoMotivoPrestamoInterno { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaInicioPago { get; set; }
        public string UsuarioIngreso { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioAprobacion { get; set; }
        public Nullable<DateTime> FechaAprobacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public string CodigoTipoPrestamo { get; set; }

        public ICollection<PrestamoInternoDetalle> PrestamoInternoDetalle { get; set; }
    }

    public class PrestamoInternoDetalle
    {
        public int IdPrestamoInternoDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int IdPrestamoInterno { get; set; }
        public int Cuota { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string CodigoTipoRol { get; set; }
        public decimal Valor { get; set; }
        public string CodigoEstadoCuota { get; set; }
        public string Observacion { get; set; }
        public string UsuarioIngreso { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public int IdPeriodo { get; set; }
        public string CodigoPeriodo { get; set; }
        public string Descripcion { get; set; }
        public string CodigoRubro { get; set; }

        public PrestamoInterno PrestamoInterno { get; set; }
    }
    
}
