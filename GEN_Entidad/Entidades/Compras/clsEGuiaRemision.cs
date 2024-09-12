using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Compras
{
    public class GuiaRemision
    {
        public GuiaRemision()
        {
            GuiaRemisionDetalle = new List<GuiaRemisionDetalle>();
            GuiaRemisionFactura = new List<GuiaRemisionFactura>();
            GuiaRemisionAdjunto = new List<GuiaRemisionAdjunto>();
        }

        public int IdGuiaRemision { get; set; }
        public string Tipo { get; set; }
        public int? DocEntry { get; set; }
        public int? DocNum { get; set; }
        public string NumeroGuia { get; set; }
        public int? FolioNum { get; set; }
        public string CodigoEstado { get; set; }
        public string Local { get; set; }
        public string PuntoEmision { get; set; }
        public string Secuencia { get; set; }
        public string Numero { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaInicioTraslado { get; set; }
        public DateTime FechaTerminoTraslado { get; set; }
        public DateTime FechaContabilizacion { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public DateTime FechaDocumento { get; set; }
        public string CodigoMotivoTraslado { get; set; }
        public string MotivoTraslado { get; set; }
        public string CodigoTransportista { get; set; }
        public string Transportista { get; set; }
        public string IdentificacionTransportista { get; set; }
        public string CodigoTransporte { get; set; }
        public string Transporte { get; set; }
        public string PuntoPartida { get; set; }
        public string CodProveedor { get; set; }
        public string Proveedor { get; set; }
        public string CodCliente { get; set; }
        public string Cliente { get; set; }
        public string IdentificacionCliente { get; set; }
        public string PuntoLlegada { get; set; }
        public string CodigoMotivo { get; set; }

        public decimal Total { get; set; }
        public decimal TotalFlete { get { return NumBultos * ValorBulto; } }
        public string CodAlmacen { get; set; }
        public string TipoTransporte { get; set; }
        public int CodVendedor { get; set; }
        public string Vendedor { get; set; }
        public string CodZonaVendedor { get; set; }
        public string ZonaVendedor { get; set; }
        public string CodZonaTransporte { get; set; }
        public string ZonaTransporte { get; set; }
        public decimal ValorBulto { get; set; }
        public int NumBultos { get; set; }
        public string UsuarioSap { get; set; }
        public string Usuario { get; set; }
        public string CodAlmacenDestino { get; set; }
        public string Prefijo { get; set; }
        public string Comentario { get; set; }

        //public int? IdOrdenPagoFactura { get; set; }
        public bool IngresoManual { get; set; }
        public string Ver { get; set; }
        public string Del { get; set; }
        public string TipoItem { get; set; }
        public string CodigoMotivoGuia { get; set; }
        public int CantidadTotal { get; set; }
        public string GR { get; set; }
        public string Externa { get; set; }
        public ICollection<GuiaRemisionDetalle> GuiaRemisionDetalle { get; set; }
        public ICollection<GuiaRemisionFactura> GuiaRemisionFactura { get; set; }
        public ICollection<GuiaRemisionAdjunto> GuiaRemisionAdjunto { get; set; }
    }

    public class GuiaRemisionDetalle
    {
        public GuiaRemisionDetalle()
        {
            GuiaRemision = new GuiaRemision();
        }

        public int IdGuiaRemisionDetalle { get; set; }
        public int IdGuiaRemision { get; set; }
        public int LineNum { get; set; }
        public string TipoItem { get; set; }
        
        public string ItemCodeString { get { return ItemCode; } }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        public string DescripcionServicio { get; set; }

        public string CodigoAlmacenOrigen { get { return AlmacenOrigen; } }
        public string AlmacenOrigen { get; set; }
        public string NombreAlmacenOrigen { get; set; }

        public string CodigoAlmacenDestino { get { return AlmacenDestino; } }
        public string AlmacenDestino { get; set; }
        public string NombreAlmacenDestino { get; set; }
        public int Cantidad { get; set; }
        public int CantidadOriginal { get; set; }
        public int CantidadTomada { get; set; }
        public int Saldo { get { return CantidadOriginal - CantidadTomada; } }

        public decimal Prorrateo { get; set; }

        public string Del { get; set; }

        public GuiaRemision GuiaRemision { get; set; }

    }

    public class GuiaRemisionAdjunto
    {
        public int IdGuiaRemisionAdjunto { get; set; }
        public int IdGuiaRemision { get; set; }
        public string Add { get; set; }
        public string Descripcion { get; set; }
        public string ArchivoAdjunto { get; set; }
        public string NombreOriginal { get; set; }
        public string RutaDestino { get; set; }
        public string RutaOrigen { get; set; }
        public string Descargar { get; set; }
        public string Visualizar { get; set; }
        public string Del { get; set; }
    }
    public class GuiaRemisionFactura
    {
        public int IdGuiaRemisionFactura { get; set; }
        public int IdGuiaRemision { get; set; }
        public int IdOrdenPagoFactura { get; set; }

        public Factura Factura { get; set; }
        public GuiaRemision GuiaRemision { get; set; }
    }

    public class ImportGuiaRemision
    {
        public string Cliente { get; set; }
        public string Numero { get; set; }
        public decimal ValorPorBulto { get; set; }
        public decimal Total { get; set; }
    }

    public class ListaGuiaRemisionFactura
    {
        public bool Sel { get; set; }
        public string Tipo { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string UsuarioSap { get; set; }
        public int? Id { get; set; }
        public string Proveedor { get; set; }
        public string Transportista { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaEntrega { get; set; }
        public decimal Total { get; set; }
        public string Numero { get; set; }
        public int Bultos { get; set; }
        public string AlmacenOrigen { get; set; }
        public string AlmacenDestino { get; set; }
        public string Zona { get; set; }
        public decimal ValorPorBulto { get; set; }
    }
}
