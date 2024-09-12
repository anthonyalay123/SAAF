using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Cobranza
{
    public class ClienteComision
    {
        public int IdClienteComision { get; set; }
        public string CodigoEstado { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public ICollection<ClienteComisionDetalle> ClienteComisionDetalle { get; set; }
        public string Det { get; set; }
        public string Del { get; set; }
    }

    public class ClienteComisionDetalle
    {
        public int IdClienteComisionDetalle { get; set; }
        public int IdClienteComision { get; set; }
        public string CodigoComision { get; set; }
        public string CodeZona { get; set; }
        public string CodigoToleraciaContado { get; set; }
        public decimal PorcentajeComision { get; set; }
        public string Del { get; set; }
    }

    public class ComisionDetalleBase
    {
        public int IdDet { get; set; }
        public int IdCab { get; set; }
        public string CodigoComision { get; set; }
        public string CodeZona { get; set; }
        public string CodigoToleraciaContado { get; set; }
        public decimal PorcentajeComision { get; set; }
        public string Del { get; set; }
    }

    public class NoAplicaNumDocPagoComision
    {
        public int IdNoAplicaNumDocPagoComision { get; set; }
        public string CodigoEstado { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public int NumDocPago { get; set; }
        public bool Considerar { get; set; }
        public bool ConsiderarCobranza { get; set; }
        public string Del { get; set; }
    }

    public class FacturaComision
    {
        public int IdFacturaComision { get; set; }
        public string CodigoEstado { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string NumFactura { get; set; }
        public ICollection<FacturaComisionDetalle> FacturaComisionDetalle { get; set; }
        public string Det { get; set; }
        public string Del { get; set; }
    }

    public class FacturaComisionDetalle
    {
        public int IdFacturaComisionDetalle { get; set; }
        public int IdFacturaComision { get; set; }
        public string CodigoComision { get; set; }
        public string CodeZona { get; set; }
        public string CodigoToleraciaContado { get; set; }
        public decimal PorcentajeComision { get; set; }
        public string Del { get; set; }
    }

    public class ClientePagoAseguradora
    {
        public int IdClientePagoAseguradora { get; set; }
        public string CodigoEstado { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Del { get; set; }
    }

    public class ClienteCambioZona
    {
        public int IdClienteCambioZona { get; set; }
        public string CodigoEstado { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public string Del { get; set; }
    }

    public class ZonaDiasDocumento
    {
        public int IdZonaDiasDocumento { get; set; }
        public string CodigoEstado { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public ICollection<ZonaDiasDocumentoDetalle> ZonaDiasDocumentoDetalle { get; set; }
        public string Det { get; set; }
        public string Del { get; set; }
    }

    public class ZonaDiasDocumentoDetalle
    {
        public int IdZonaDiasDocumentoDetalle { get; set; }
        public int IdZonaDiasDocumento { get; set; }
        public int DiasDocumento { get; set; }
        public int DiasDocumentoAdd { get; set; }
        public string Del { get; set; }
    }

    public class EmpleadoComision
    {
        public int IdPersona { get; set; }
        public ICollection<EmpleadoComisionZona> EmpleadoComisionZona { get; set; }
        public ICollection<EmpleadoComisionCodigo> EmpleadoComisionCodigo { get; set; }
        public string Det1 { get; set; }
        public string Det2 { get; set; }
        public string Del { get; set; }
    }

    public class EmpleadoComisionZona
    {
        public int IdEmpleadoZonaSap { get; set; }
        public int IdPersona { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public string Del { get; set; }
    }

    public class EmpleadoComisionCodigo
    {
        public int IdCodigoComision { get; set; }
        public int IdPersona { get; set; }
        public string CodigoComision { get; set; }
        public string Del { get; set; }
    }

    public class Vendedor
    {
        public int Id { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public int SlpCodeCurrent { get; set; }
        public string SlpNameCurrent { get; set; }
        public string CodigoGrupoFactura { get; set; }
        public string Del { get; set; }
    }

    public class PrComisiones
    {
        public int Id { get; set; }
        public string CodigoComision { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public string CodigoToleranciaContado { get; set; }
        public string CodigoGrupoFactura { get; set; }
        public decimal PorcentajeComision { get; set; }
        public string Del { get; set; }
    }

    public class ToleranciaContado
    {
        public string CodigoToleranciaContado { get; set; }
        public string Descripcion { get; set; }
        public int Desde { get; set; }
        public int Hasta { get; set; }
        public string Del { get; set; }
    }

    public class DiaNoLaborable
    {
        public int IdDiaNoLaborable { get; set; }
        public string CodigoEstado { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public ICollection<DiaNoLaborableZona> DiaNoLaborableZona { get; set; }
        public string Det { get; set; }
        public string Del { get; set; }
    }

    public class DiaNoLaborableZona
    {
        public int IdDiaNoLaborableZona { get; set; }
        public int IdDiaNoLaborable { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public string Del { get; set; }
    }
}
