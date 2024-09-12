using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Compras
{
    public class OrdenPago
    {
        public OrdenPago()
        {
            Facturas = new List<Factura>();
            OrdenPagoAdjunto = new List<OrdenPagoAdjunto>();
        }
        public int IdOrdenPago { get; set; }
        public string Observacion { get; set; }
        public string UsuarioAprobacion { get; set; }
        public int IdProveedor { get; set; }
        public string ProveedorNombre { get; set; }
        public string CodigoEstado { get; set; }
        public decimal Valor { get; set; }
        public decimal TotalOrdenCompra { get; set; }
        public string FormaPago { get; set; }
        public string CodigoTipoCompra { get; set; }
        public string CodigoTipoOrdenPago { get; set; }
        public string ComentarioAprobador { get; set; }
        public DateTime Fecha { get; set; }
        public ICollection<Factura> Facturas { get; set; }
        public ICollection<OrdenPagoAdjunto> OrdenPagoAdjunto { get; set; }
    }

    public class Factura
    {
        public Factura()
        {
            GuiaRemisionFactura = new List<GuiaRemisionFactura>();
            GuiaRemision = new List<GuiaRemision>();
        }

        public int IdFactura { get; set; }
        public int IdOrdenPago { get; set; }
        public string CodigoEstado { get; set; }


        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [StringLength(30, ErrorMessage = "El número es demasiado largo")]
        public string NoFactura { get; set; }
        [MaxLength(200)]
        public string Descripcion { get; set; }
        
        public DateTime FechaFactura { get; set; }
        public int Dias { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string TipoTransporte { get; set; }
        public int IdZonaGrupo { get; set; }
        public string ZonaGrupo { get; set; }

        [Range(0, 9999999999999999.99, ErrorMessage = "Maximo 18 digitos")]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Maximo dos decimales.")]
        public decimal Valor { get; set; }
        public string ArchivoAdjunto { get; set; }
        public string RutaOrigen { get; set; }
        public string RutaDestino { get; set; }

        public string AgregarFactura { get; set; }
        public string NombreOriginal { get; set; }
        public string VerFactura { get; set; }

        public string Add { get; set; }
        public string Observacion { get; set; }
        public decimal SaldoOc { get; set; }

        public string Guia { get; set; }
        public string Del { get; set; }

        public ICollection<GuiaRemisionFactura> GuiaRemisionFactura { get; set; }
        public ICollection<GuiaRemision> GuiaRemision { get; set; }

    }
    public class Proveedor
    {
        public bool Sel { get; set; }
        public int IdProveedor { get; set; }
        public string Nombre { get; set; }
        
        public string Observacion { get; set; }
        public decimal Total { get; set; }
        public decimal ValorAnticipo { get; set; }
        public decimal Saldo { get { return Total - ValorAnticipo; } }
        public string Estado { get; set; }
    }

    public class BandejaOrdenPago
    {
        public bool Sel { get; set; }
        public int Id { get; set; }
        public DateTime FechaOrdenPago { get; set; }
        public string Usuario { get; set; }
        public string ReferenciasIDOrdenCompraProveedor { get; set; }
        public string Observacion { get; set; }
        public string Proveedor { get; set; }
        public string Factura { get; set; }

        public decimal Valor { get; set; }
        public decimal TotalOrdenCompra { get; set; }
        public decimal Diferencia { get { return Valor - TotalOrdenCompra; } }
        [MaxLength(100)]
        public string Comentario { get; set; }
        public string Aprobaciones { get; set; }
        public string UsuariosAprobacion { get; set; }
        public string Estado { get; set; }
        public string VerComentarios { get; set; }
        public string Ver { get; set; }
        public string Imprimir { get; set; }
    }

    public class BandejaOrdenPagoExcel
    {
        public int No { get; set; }
        public DateTime FechaOrdenPago { get; set; }
        public string Usuario { get; set; }
        public string Descripcion { get; set; }
        public string Proveedor { get; set; }
        public decimal Total { get; set; }
        public string Aprobo { get; set; }
        public string Estado { get; set; }
        
    }

    public class OrdenPagoAdjunto
    {
        public int IdOrdenPagoAdjunto { get; set; }
        public int IdOrdenPago { get; set; }
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

    public class FacturaDetalleGrid
    {
        public bool Sel { get; set; }
        public int IdOrdenPago { get; set; }
        public int IdOrdenPagoFactura { get; set; }
        public string Identificacion { get; set; }
        public int IdProveedor { get; set; }
        public string Proveedor { get; set; }
        public string NumDocumento { get; set; }
        public int DocNum { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal Valor { get; set; }
        public decimal Abono { get; set; }
        public decimal Saldo { get; set; }
        public decimal ValorPago { get; set; }
        public string Observacion { get; set; }
        public int IdGrupoPago { get; set; }
        public string GrupoPago { get; set; }
        public string ActualizarGrupoPago { get; set; }
        //public string Comentarios { get; set; }
        public string UsuarioAprobadorOP { get; set; }
        public string ComentarioAprobadorOP { get; set; }
        public bool Generado { get; set; }
        public string CodigoEstado { get; set; }
        [MaxLength(200)]
        public string ComentarioTesoreria { get; set; }
        [MaxLength(200)]
        public string Comentario { get; set; }
        public string VerComentarios { get; set; }
        public string Ver { get; set; }
        public string Del { get; set; }
        public int IdFacturaPago { get; set; }
    }

    public class FacturaPorAprobarDetalleGrid
    {
        public bool Sel { get; set; }
        public int IdOrdenPagoFactura { get; set; }
        public int IdOrdenPago { get; set; }
        public string Identificacion { get; set; }
        public string Proveedor { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal Valor { get; set; }
        public decimal Abono { get; set; }
        public decimal Saldo { get; set; }
        public decimal ValorPago { get; set; }
        public string NumDocumento { get; set; }
        public string Observacion { get; set; }
        public bool Generado { get; set; }
        public string CodigoEstado { get; set; }
        public string Comentario { get; set; }
        public string Aprobaciones { get; set; }
        public string Aprobo { get; set; }
        public string UsuarioCreaOP { get; set; }
        public string UsuarioApruebaOP { get; set; }
        public int DocNum { get; set; }
        public string VerComentarios { get; set; }
        public string Del { get; set; }
        public string Ver { get; set; }
        public int IdFacturaPago { get; set; }
        
    }

    public class FacturaPorAprobarDetalleGridExcel
    {
        //public bool Sel { get; set; }
        public string UsuarioCreaOP { get; set; }
        public string UsuarioApruebaOP { get; set; }
        public string Identificacion { get; set; }
        public string Proveedor { get; set; }
        public string NumDocumento { get; set; }
        public int DocNum { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal ValorPago { get; set; }
        public string Observacion { get; set; }

    }

    public class FacturaAprobadasDetalleGrid
    {
        public bool Sel { get; set; }
        //public bool SelDet { get; set; }
        public int IdOrdenPagoFactura { get; set; }
        public int IdOrdenPago { get; set; }
        public int IdProveedor { get; set; }
        public string Identificacion { get; set; }
        public string Proveedor { get; set; }
        public string NumDocumento { get; set; }
        public string Det { get; set; }
        public int DocNum { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal Valor { get; set; }
        public decimal Abono { get; set; }
        public decimal Saldo { get; set; }
        public decimal ValorPagoOriginal { get; set; }
        public decimal ValorPago { get; set; }
        public string ComentarioAprobador { get; set; }
        public DateTime FechaPago { get; set; }
        public string Observacion { get; set; }
        public bool Generado { get; set; }
        public string CodigoEstado { get; set; }
        public int IdGrupoPago { get; set; }
        public string GrupoPago { get; set; }
        public string Ver { get; set; }
        public string CodigoTipoIdentificacion { get; set; }
        public string IdentificacionCuenta { get; set; }
        public string Nombre { get; set; }
        public string CodigoBanco { get; set; }
        public string CodigoFormaPago { get; set; }
        public string CodigoTipoCuentaBancaria { get; set; }
        public string NumeroCuenta { get; set; }
        public string Aprobaciones { get; set; }
        public string Aprobo { get; set; }
        public string Del { get; set; }
        public int IdFacturaPago { get; set; }
        public string IdSemanaPago { get; set; }

        public string Add { get; set; }

        public string ArchivoAdjunto { get; set; }
        public string RutaOrigen { get; set; }
        public string RutaDestino { get; set; }
        public string NombreOriginal { get; set; }
        public string VerOP { get; set; }


        public string Descargar { get; set; }
        public string Visualizar { get; set; }

    }

    public class ArchivoPagoMultiCashTxt
    {
        public string Pa { get; set; }
        public string CuentaEmpresa { get; set; }
        public int Secuencia { get; set; }
        public string NumCompOP { get; set; }
        public string CuentaBenef { get; set; }
        public string Moneda { get; set; }
        public decimal ValorDec { get; set; }
        public string Valor { get; set; }
        public string FormaPago { get; set; }
        public string CodigoIntFin { get; set; }
        public string TipoCuenta { get; set; }
        public string CuentaAcreditar { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string Beneficiario { get; set; }
        public string DireccionOP { get; set; }
        public string CiudadOP { get; set; }
        public string TelefonoOP { get; set; }
        public string LocalidadOP { get; set; }
        public string Referencia { get; set; }
        public string Notificaciones { get; set; }
        public string Cuerpo { get
            {
                return string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}\t{19}\t",
                        Pa, CuentaEmpresa, Secuencia, NumCompOP, CuentaBenef, Moneda, Valor, FormaPago, CodigoIntFin, TipoCuenta, CuentaAcreditar,
                        TipoIdentificacion, Identificacion, Beneficiario, DireccionOP, CiudadOP, TelefonoOP, LocalidadOP, Referencia, Notificaciones);
            } }
    }

    public class SpFacturasPendPagoContabilidad
    {
        public int IdOrdenPago { get; set; }
        public int IdOrdenPagoFactura { get; set; }
        public string Mapeada { get; set; }
        public string IngresadaSap { get; set; }
        public string Departamento { get; set; }
        public string Usuario { get; set; }
        public string CardCode { get; set; }
        public string Proveedor { get; set; }
        public string Factura { get; set; }
        public string Descripcion { get; set; }
        public string ComentarioAprobador { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaFactura { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal Valor { get; set; }
        public string ArchivoAdjunto { get; set; }
        public string NombreOriginal { get; set; }
        public string Estado { get; set; }
        public string Aprobo { get; set; }
        public int FacturaRelSap { get; set; }
        public int DocNum { get; set; }
        public string Relacion { get; set; }
        public string Desligar { get; set; }
        public string VerComentarios { get; set; }
        public string VerFactura { get; set; }
        public string Ver { get; set; }
        public int DocEntry { get; set; }

    }


    public class SpBandejaOrdenPagoContabilizar
    {
        public bool Sel { get; set; }
        public int IdOrdenPagoFactura { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public int IdOrdenPago { get; set; }
        public string CodProveedor { get; set; }
        public string Proveedor { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaFactura { get; set; }
        public string Factura { get; set; }
        public string TipoTransporte { get; set; }
        public decimal Valor { get; set; }
        public string Ver { get; set; }

    }
}


