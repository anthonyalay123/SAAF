using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.ActivoFijo
{
    public class ItemActivoFijo
    {
        public int IdItemActivoFijo { get; set; }
        public string Codigo { get; set; }
        public string CodigoEstado { get; set; }
        public string Descripcion { get; set; }
        public string CodigoTipoActivoFijo { get; set; }
        public string CodigoSucursal { get; set; }
        public string CodigoCentroCosto { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaCompra { get; set; }
        public DateTime FechaActivacion { get; set; }
        public decimal CostoCompra { get; set; }
        public decimal ValorResidual { get; set; }
        public decimal ValorDepreciable { get; set; }
        public decimal DepreciacionAcumulada { get; set; }
        public decimal CostoActual { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Serie { get; set; }
        public string ProductId { get; set; }
        public Nullable<int> IdPersona { get; set; }
        public string Persona { get; set; }
        public string NumFactura { get; set; }
        public string CodigoEstadoActivoFijo { get; set; }
        public string CodigoAgrupacion { get; set; }
        public string Agrupacion { get; set; }

        public string ArchivoAdjunto { get; set; }
        public string RutaOrigen { get; set; }
        public string RutaDestino { get; set; }
        public string NombreOriginal { get; set; }

        public Nullable<int> IdProveedor { get; set; }
        public string Proveedor { get; set; }

    }

    public class TipoActivoFijo
    {
        public string CodigoTipoActivoFijo { get; set; }
        public string CodigoEstado { get; set; }
        public string Descripcion { get; set; }
        public string Alias { get; set; }
        public bool Depreciable { get; set; }
        public int VidaUtil { get; set; }
        public decimal PorcentajeDepreciacion { get; set; }
        public decimal PorcentajeResidual { get; set; }
        public decimal ValorResidual { get; set; }
    }

    public class MovimientoActivoFijo
    {
        public int IdMovimientoActivoFijo { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoTipoMovimiento { get; set; }
        public string TipoMovimiento { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public int IdItemActivoFijo { get; set; }
        public string CodigoActivoFijo { get; set; }
        public string ActivoFijo { get; set; }
        public decimal CostoCompra { get; set; }
        public decimal ValorResidual { get; set; }
        public decimal ValorDepreciable { get; set; }
        public decimal DepreciacionAcumulada { get; set; }
        public decimal CostoActual { get; set; }
        public string Observacion { get; set; }
        public Nullable<decimal> ValorVenta { get; set; }
    }
}
