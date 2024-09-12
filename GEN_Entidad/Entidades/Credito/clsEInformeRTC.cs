using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Credito
{
    public class InformeRtc
    {
        public InformeRtc()
        {
            this.InformeRtcCultivos = new HashSet<InformeRtcCultivos>();
            this.InformeRtcClientes = new HashSet<InformeRtcClientes>();
            this.InformeRtcCroquis = new HashSet<InformeRtcCroquis>();
            this.InformeRtcDocumentosPendientes = new HashSet<InformeRtcDocumentosPendientes>();
            this.InformeRtcProductos = new HashSet<InformeRtcProductos>();
            this.InformeRtcProveedor = new HashSet<InformeRtcProveedor>();
        }

        public int IdInformeRTC { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoTipoProceso { get; set; }
        public string TipoProceso { get; set; }
        public string CodigoActividadCliente { get; set; }
        public string ActividadCliente { get; set; }
        public int IdRTC { get; set; }
        public string RTC { get; set; }
        public string Zona { get; set; }
        public string CodigoCliente { get; set; }
        public string Cliente { get; set; }
        public string RepresentanteLegal { get; set; }
        public string CodigoTipoPersona { get; set; }
        public string CodigoSector { get; set; }
        public System.DateTime FechaInforme { get; set; }
        public string Ciudad { get; set; }
        public int TiempoEnZonaActividad { get; set; }
        public string ZonasCubrenVentas { get; set; }
        public string TipoCultivo { get; set; }
        public decimal CantidadHectareasCubreVentas { get; set; }
        public string CultivosSiembra { get; set; }
        public decimal CantidadHectareasSembradasCultivo { get; set; }
        public decimal CostoHectarea { get; set; }
        public decimal PotencialVenta { get; set; }
        public decimal CantidadHectareasPropias { get; set; }
        public decimal CantidadHectareasAlquiladas { get; set; }
        public string TieneCodigoSap { get; set; }
        public string GrupoSap { get; set; }
        public decimal TotalHectareas { get; set; }
        public string TieneOtrasActividadesComerciales { get; set; }
        public decimal MontoAnualesVenta { get; set; }
        public string TieneCuencaCorriente { get; set; }
        public string CodigoBanco { get; set; }
        public string Banco { get; set; }
        public string DetalleFormaPago { get; set; }
        public string TrabajadoAntesCliente { get; set; }
        public string Donde { get; set; }
        public int TiempoConocerlo { get; set; }
        public string ComportamientoPago { get; set; }
        public string TieneHistorialAfecor { get; set; }
        public decimal CupoSolicitado { get; set; }
        public decimal VentaEstimadaAnual { get; set; }
        public int PlazoSolicitado { get; set; }
        public decimal Mes1 { get; set; }
        public decimal Mes2 { get; set; }
        public decimal Mes3 { get; set; }
        public decimal Mes4 { get; set; }
        public decimal Mes5 { get; set; }
        public decimal Mes6 { get; set; }
        public decimal Mes7 { get; set; }
        public decimal Mes8 { get; set; }
        public decimal Mes9 { get; set; }
        public decimal Mes10 { get; set; }
        public decimal Mes11 { get; set; }
        public decimal Mes12 { get; set; }
        public decimal VentasSri { get; set; }
        public decimal ComprasSri { get; set; }
        public decimal FacturacionAfecor { get; set; }
        public decimal PorcAfecorSobreCompras { get; set; }
        public decimal PorcAfecorSobreComprasProyectadas { get; set; }
        public decimal MontoCrecimientoAfecor { get; set; }
        public decimal PorcCrecimientoAfecor { get; set; }
        public Nullable<int> IdReferenciaForm { get; set; }
        public Nullable<bool> Completado { get; set; }
        public Nullable<int> IdProcesoCredito { get; set; }
        public string Argumentos { get; set; }
        public string Direccion1Almacen { get; set; }
        public string Direccion1Titular { get; set; }
        public string Direccion2Almacen { get; set; }
        public string Direccion2Titular { get; set; }
        public Nullable<System.DateTime> FechaUltimaSolicitud { get; set; }
        public int TiempoConocerloMes { get; set; }
        public int TiempoEnZonaActividadMes { get; set; }
        public decimal MontoHistorialCompras { get; set; }
        public string Usuario { get; set; }

        public virtual ICollection<InformeRtcCultivos> InformeRtcCultivos { get; set; }
        public virtual ICollection<InformeRtcClientes> InformeRtcClientes { get; set; }
        public virtual ICollection<InformeRtcCroquis> InformeRtcCroquis { get; set; }
        public virtual ICollection<InformeRtcDocumentosPendientes> InformeRtcDocumentosPendientes { get; set; }
        public virtual ICollection<InformeRtcProductos> InformeRtcProductos { get; set; }
        public virtual ICollection<InformeRtcProveedor> InformeRtcProveedor { get; set; }

    }


    public class InformeRtcCultivos
    {
        public int IdInformeRTCUbiCultivos { get; set; }
        public string CodigoEstado { get; set; }
        public int IdInformeRTC { get; set; }
        [MaxLength(300)]
        public string Ubicacion { get; set; }
        public string Del { get; set; }
    }

    public partial class InformeRtcClientes
    {
        public int IdInformeRTCCliente { get; set; }
        public string CodigoEstado { get; set; }
        public int IdInformeRTC { get; set; }
        [MaxLength(300)]
        public string Cliente { get; set; }
        public string Del { get; set; }
    }

    public partial class InformeRtcCroquis
    {
        public int IdInformeRTCCroquis { get; set; }
        public string CodigoEstado { get; set; }
        public int IdInformeRTC { get; set; }
        [MaxLength(200)]
        public string Lugar { get; set; }
        [MaxLength(700)]
        public string Referencia { get; set; }
        [MaxLength(400)]
        public string Coordenadas { get; set; }
        public string Add { get; set; }
        public string NombreOriginal { get; set; }
        public string ArchivoAdjunto { get; set; }
        public string RutaOrigen { get; set; }
        public string RutaDestino { get; set; }
        public string Ver { get; set; }
        public string Del { get; set; }
    }

    public partial class InformeRtcDocumentosPendientes
    {
        public int IdInformeRTCDocumentosPendientes { get; set; }
        public string CodigoEstado { get; set; }
        public int IdInformeRTC { get; set; }
        public int IdCheckList { get; set; }
        public string CheckList { get; set; }
        public System.DateTime FechaEntrega { get; set; }
        [MaxLength(180)]
        public string Compromisos { get; set; }
        public string Del { get; set; }
    }

    public partial class InformeRtcProductos
    {
        public int IdInformeRTCProducto { get; set; }
        public string CodigoEstado { get; set; }
        public int IdInformeRTC { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public short ItmsGrpCod { get; set; }
        public string ItmsGrpNam { get; set; }
        public string Del { get; set; }
    }

    public partial class InformeRtcProveedor
    {
        public int IdInformeRTCProveedor { get; set; }
        public string CodigoEstado { get; set; }
        public int IdInformeRTC { get; set; }
        [MaxLength(380)]
        public string Proveedor { get; set; }
        public decimal MontoEstimadoVenta { get; set; }
        public string Del { get; set; }
    }
}
