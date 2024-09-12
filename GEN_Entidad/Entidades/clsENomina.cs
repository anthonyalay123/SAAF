using System;

namespace GEN_Entidad
{
    public class Nomina
    {
        public int IdNomina { get; set; }
        public string CodigoEstado { get; set; }
        public string Estado { get; set; }
        public int IdPeriodo { get; set; }
        public string CodigoPeriodo { get; set; }
        public string CodigoTipoRol { get; set; }
        public string DescripcionTipoRol { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public decimal Total { get; set; }
        public int Empleados { get; set; }
        public string UsuarioIngreso { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
    }

    public class Comisiones
    {
        public int Id { get; set; }
        public string CodigoTipoRol { get; set; }
        public string DesTipoRol { get; set; }
        public int IdPeriodo { get; set; }
        public string CodigoPeriodo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Total { get; set; }
        public decimal TotalComisiones { get; set; }
        public string CodigoEstado { get; set; }
        public string DesEstado { get; set; }

    }

    public class DetalleComisiones
    {
        public int Id { get; set; }
        public string Empleado { get; set; }
        public string Zona { get; set; }
        public string CodCliente { get; set; }
        public string Cliente { get; set; }
        public string Facturador { get; set; }
        public string CodComision { get; set; }
        public string Factura { get; set; }
        public string Vendedor { get; set; }
        public DateTime FechaEmisiónFact { get; set; }
        public DateTime FechaVenceFact { get; set; }
        public int DiasCrédito { get; set; }
        public int NumDocPagoEnSAP { get; set; }
        public DateTime FechaRegistroPago { get; set; }
        public DateTime FechaEfectivaPago { get; set; }
        public int DiasPago { get; set; }
        public string Banco { get; set; }
        public decimal ValorPago { get; set; }
        public decimal PorcComisión { get; set; }
        public decimal ValorComisión { get; set; }

    }
}

