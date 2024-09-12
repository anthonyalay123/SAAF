using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Credito
{
    public class SolicitudCredito
    {

        public SolicitudCredito()
        {
            this.ProcesoCredito = new ProcesoCredito();
            //this.SolicitudCreditoOtrosActivos = new List<SolicitudCreditoOtrosActivos>();
            //this.SolicitudCreditoRefBancaria = new List<SolicitudCreditoRefBancaria>();
            //this.SolicitudCreditoRefComercial = new List<SolicitudCreditoRefComercial>();
            this.SolicitudCreditoRefPersonal = new List<SolicitudCreditoRefPersonal>();
            //this.SolicitudCreditoPropiedades = new List<SolicitudCreditoPropiedades>();
            //this.SolicitudCreditoDireccion = new List<SolicitudCreditoDireccion>();
            //this.SolicitudCreditoNombramiento = new List<SolicitudCreditoNombramiento>();
        }

        public int IdSolicitudCredito { get; set; }
        public string CodigoEstado { get; set; }
        public System.DateTime FechaSolicitud { get; set; }
        public string CodigoTipoSolicitud { get; set; }
        public string TipoSolicitud { get; set; }
        public string CodigoActividadCliente { get; set; }
        public string ActividadCliente { get; set; }
        public int IdRTC { get; set; }
        public string RTC { get; set; }
        public string Zona { get; set; }
        public decimal Cupo { get; set; }
        public string Observacion { get; set; }
        public string CodigoCliente { get; set; }
        public string Cliente { get; set; }
        public string Identificacion { get; set; }
        public string IdentificacionRepLegal { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public string CodigoEstadoCivil { get; set; }
        public string Ciudad { get; set; }
        public string DireccionDomicilio { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string CodigoVivienda { get; set; }
        public int Tiempo { get; set; }
        public string Email { get; set; }
        public string NombreConyuge { get; set; }
        public string IdentificacionConyuge { get; set; }
        public string NombreComercial { get; set; }
        public int TiempoActividad { get; set; }
        public string Ruc { get; set; }
        public string CodigoTipoNegocio { get; set; }
        public string CiudadNegocio { get; set; }
        public string DireccionNegocio { get; set; }
        public string TelefonoNegocio { get; set; }
        public string CelularNegocio { get; set; }
        public string CodigoLocal { get; set; }
        public int TiempoNegocio { get; set; }
        public string ActividadAnterior { get; set; }
        public Nullable<int> TiempoActividadAnterior { get; set; }
        public string EmpresaActividadAnterior { get; set; }
        public string CargoActividadAnterior { get; set; }
        public string NombreContactoPago { get; set; }
        public string CargoContactoPago { get; set; }
        public string TelefonoContactoPago { get; set; }
        public string CelularContactoPago { get; set; }
        public string EmailContactoPago { get; set; }
        public string NombreContactoPagoAgricultor { get; set; }
        public string CargoContactoPagoAgricultor { get; set; }
        public string TelefonoContactoPagoAgricultor { get; set; }
        public string CelularContactoPagoAgricultor { get; set; }
        public string EmailContactoPagoAgricultor { get; set; }
        public string EmailFacturaElectronica { get; set; }
        public decimal OtrosIngresos { get; set; }
        public string OrigenOtrosIngresos { get; set; }
        public string EmailRealizaCompras { get; set; }
        public string EmailRecibeCompras { get; set; }
        public Nullable<int> TiempoAgricultor { get; set; }
        public string ClaseCultivo { get; set; }
        public Nullable<int> NumeroHectariasCultivadas { get; set; }
        public string CodigoTerreno { get; set; }
        public Nullable<int> AntiguedadTerreno { get; set; }
        public Nullable<int> MesesCosecha { get; set; }
        public Nullable<decimal> PromedioIngresos { get; set; }
        public string UbicacionTerrenos { get; set; }
        public Nullable<System.DateTime> FechaAprobacion { get; set; }
        public string UsuarioAprobacion { get; set; }
        public string UsuarioIngreso { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string TerminalIngreso { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string TerminalModificacion { get; set; }
        public Nullable<int> IdReferenciaForm { get; set; }
        public bool Completado { get; set; }
        public int IdProcesoCredito { get; set; }
        public decimal CupoSap { get; set; }
        public string EstadoRuc { get; set; }
        public Nullable<System.DateTime> FechaInicioActividadRuc { get; set; }
        public Nullable<System.DateTime> FechaReinicioActividadRuc { get; set; }
        public Nullable<System.DateTime> FechaUltActualizacionRuc { get; set; }
        public string ObligadoLlevarCantidadRuc { get; set; }
        public string IdentificacionRealizaCompras { get; set; }
        public string IdentificacionEmailRecibeCompras { get; set; }
        public Nullable<System.DateTime> FechaCaducidadCedulaRepLegal { get; set; }
        public int Edad { get { return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) >= new DateTime(DateTime.Now.Year, FechaNacimiento.Month, FechaNacimiento.Day) ? DateTime.Now.Year - FechaNacimiento.Year: (DateTime.Now.Year - FechaNacimiento.Year) - 1; } }
        public string OtrasActividades { get; set; }
        public string ActividadesSecundarias     { get; set; }
        public string Comentarios { get; set; }
        public string ComentariosNombramiento { get; set; }
        public string ComentariosCertBancarios { get; set; }
        public string ComentariosRefComerciales { get; set; }
        public string ComentariosBienes { get; set; }
        public Nullable<System.DateTime> FechaUltimaSolicitud { get; set; }
        public int TiempoMes { get; set; }
        public int TiempoActividadMes { get; set; }
        public int TiempoActividadAnteriorMes { get; set; }
        public int TiempoAgricultorMes { get; set; }
        public int TiempoNegocioMes { get; set; }
        public int AntiguedadTerrenoMes { get; set; }
        public string ComentarioRevisionCedula { get; set; }

        //public ICollection<SolicitudCreditoOtrosActivos> SolicitudCreditoOtrosActivos { get; set; }
        //public ICollection<SolicitudCreditoRefBancaria> SolicitudCreditoRefBancaria { get; set; }
        //public ICollection<SolicitudCreditoRefComercial> SolicitudCreditoRefComercial { get; set; }
        public ICollection<SolicitudCreditoRefPersonal> SolicitudCreditoRefPersonal { get; set; }
        //public ICollection<SolicitudCreditoPropiedades> SolicitudCreditoPropiedades { get; set; }
        //public ICollection<SolicitudCreditoDireccion> SolicitudCreditoDireccion { get; set; }
        //public ICollection<SolicitudCreditoNombramiento> SolicitudCreditoNombramiento { get; set; }

        public ProcesoCredito ProcesoCredito { get; set; }
    }

    //public class SolicitudCreditoOtrosActivos
    //{
    //    public int IdSolicitudCreditoOtrosActivos { get; set; }
    //    public int IdSolicitudCredito { get; set; }
    //    public string CodigoTipoOtrosActivos { get; set; }
    //    [MaxLength(50)]
    //    public string Marca { get; set; }
    //    [MaxLength(50)]
    //    public string Modelo { get; set; }
    //    [MaxLength(4)]
    //    public int Anio { get; set; }
    //    [MaxLength(18)]
    //    public decimal AvaluoComercial { get; set; }
    //    public DateTime FechaPago { get; set; }
    //    public string DescripcionDocumento { get; set; }
    //    public string PropietarioBeneficiario { get; set; }
   
    //    public string Del { get; set; }
    //}

    //public class SolicitudCreditoRefBancaria
    //{
    //    public int IdSolicitudCreditoRefBancaria { get; set; }
    //    public int IdSolicitudCredito { get; set; }
    //    public DateTime FechaEmision { get; set; }
    //    public string CodigoBanco { get; set; }
    //    public string CodigoTipoCuentaBancaria { get; set; }
    //    [MaxLength(30)]
    //    public string NumeroCuenta { get; set; }
    //    public string Titular { get; set; }
    //    public Nullable<System.DateTime> FechaApertura { get; set; }
    //    public decimal SaldosPromedios { get; set; }
    //    public string ArchivoAdjunto { get; set; }
    //    public string RutaOrigen { get; set; }
    //    public string RutaDestino { get; set; }
    //    public string Add { get; set; }
    //    public string NombreOriginal { get; set; }
    //    public string Ver { get; set; }


    //    public string Del { get; set; }
    //}

    //public class SolicitudCreditoRefComercial
    //{
    //    public int IdSolicitudCreditoRefComercial { get; set; }
    //    public int IdSolicitudCredito { get; set; }
    //    public DateTime FechaConsulta { get; set; }
    //    public DateTime ClienteDesde { get; set; }
    //    public DateTime? FechaReferenciaComercial { get; set; }
    //    [MaxLength(100)]
    //    public string Nombre { get; set; }
    //    [MaxLength(10)]
    //    public string Telefono { get; set; }
    //    public string ArchivoAdjunto { get; set; }
    //    public string RutaOrigen { get; set; }
    //    public string RutaDestino { get; set; }
    //    public string Add { get; set; }
    //    public string NombreOriginal { get; set; }
    //    public string Ver { get; set; }
    //    public decimal Cupo { get; set; }
    //    public int Plazo { get; set; }
    //    public decimal Garantia { get; set; }
    //    public string CodigoGarantia { get; set; }
    //    public decimal PromedioComprasMensual { get; set; }
    //    public decimal PromedioComprasAnual { get; set; }
    //    public int PromedioDiasPagos { get; set; }
    //    public string Del { get; set; }
    //}

    public class SolicitudCreditoRefPersonal
    {
        public int IdSolicitudCreditoRefPersonal { get; set; }
        public int IdSolicitudCredito { get; set; }
        public string TipoReferenciaPersonal { get; set; }
        public string ReferenciaPersonal { get; set; }
        [MaxLength(100)]
        public string Nombre { get; set; }
        public string CodigoRelacion { get; set; }
        public string Relacion { get; set; }
        [MaxLength(10)]
        public string Telefono { get; set; }
        [MaxLength(50)]
        public string Direccion { get; set; }
        [MaxLength(50)]
        public string Ciudad { get; set; }
        public string Del { get; set; }
    }

    //public class SolicitudCreditoPropiedades
    //{
    //    public int IdSolicitudCreditoPropiedades { get; set; }
    //    public int IdSolicitudCredito { get; set; }
    //    public string CodigoTipoBien { get; set; }
    //    [MaxLength(100)]
    //    public string Direccion { get; set; }
    //    public string Hipoteca { get; set; }
    //    [MaxLength(18)]
    //    public decimal AvaluoComercial { get; set; }
    //    public DateTime FechaPago { get; set; }
    //    public string DescripcionDocumento { get; set; }
    //    public string PropietarioBeneficiario { get; set; }
    //    public string Del { get; set; }
    //}

    //public class SolicitudCreditoDireccion
    //{
    //    public int IdSolicitudCreditoDireccion { get; set; }
    //    public int IdSolicitudCredito { get; set; }
    //    public bool Principal { get; set; }
    //    [MaxLength(300)]
    //    public string Direccion { get; set; }
    //    public string Del { get; set; }
    //}

    //public class SolicitudCreditoNombramiento
    //{
    //    public int IdSolicitudCreditoNombramiento { get; set; }
    //    public int IdSolicitudCredito { get; set; }
    //    public DateTime FechaInscripcion { get; set; }
    //    public int Periodo { get; set; }
    //    public DateTime FechaCaducidad   { get { return FechaInscripcion.AddYears(Periodo); } }
    //    [MaxLength(200)]
    //    public string Cargo { get; set; }
    //    public string CodigoPresentacion { get; set; }
    //    public string Del { get; set; }
    //}
}
