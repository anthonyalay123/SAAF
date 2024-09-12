using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Credito
{
    public class ProcesoCredito
    {
        public ProcesoCredito()
        {
            this.ProcesoCreditoDetalle = new List<ProcesoCreditoDetalle>();
            this.ProcesoCreditoAccionista = new List<ProcesoCreditoAccionista>();
            this.ProcesoCreditoAdmActual = new List<ProcesoCreditoAdmActual>();
            this.ProcesoCreditoBuro = new List<ProcesoCreditoBuro>();
            this.ProcesoCreditoFuncionJudicial = new List<ProcesoCreditoFuncionJudicial>();
            this.ProcesoCreditoSri = new List<ProcesoCreditoSri>();
            this.ProcesoCreditoResolucionAdjunto = new List<ProcesoCreditoResolucionAdjunto>();
            this.ProcesoCreditoCedula = new ProcesoCreditoCedula();
            this.ProcesoCreditoNombramiento = new List<ProcesoCreditoNombramiento>();
            this.ProcesoCreditoRuc = new ProcesoCreditoRuc();
            this.ProcesoCreditoOtrosActivos = new List<ProcesoCreditoOtrosActivos>();
            this.ProcesoCreditoRefBancaria = new List<ProcesoCreditoRefBancaria>();
            this.ProcesoCreditoRefComercial = new List<ProcesoCreditoRefComercial>();
            this.ProcesoCreditoPropiedades = new List<ProcesoCreditoPropiedades>();
        }

        public int IdProcesoCredito { get; set; }
        public string CodigoEstado { get; set; }
        public System.DateTime Fecha { get; set; }
        public string CodigoTipoSolicitud { get; set; }
        public string TipoSolicitud { get; set; }
        public string CodigoCliente { get; set; }
        public string Cliente { get; set; }
        public string CodigoTipoPersona { get; set; }
        public string CodigoContado { get; set; }
        public string CodigoEstatusSeguro { get; set; }
        public string Observacion { get; set; }
        public Nullable<int> IdReferenciaForm { get; set; }
        public decimal CupoSap { get; set; }
        public decimal CupoSolicitado { get; set; }
        public bool Completado { get; set; }
        public string RepresentanteLegal { get; set; }
        public string NomUsuario { get; set; }
        public string ComentarioRevisor { get; set; }
        public string ComentarioResolucion { get; set; }
        public string CodigoTipoServicioBasico { get; set; }
        public string TipoServicioBasico { get; set; }
        public DateTime FechaPlanillaServicioBasico { get; set; }
        public string DireccionPlanillaServicioBasico { get; set; }
        public string ComentarioPlanillaServicioBasico { get; set; }
        public decimal? CupoAprobado { get; set; }
        public int? PlazoAprobado { get; set; }
        public string ResolucionAfecor { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public string ComentarioBuro { get; set; }
        public string ComentarioFuncionJudicial { get; set; }
        public string ComentarioSri { get; set; }
        public string ComentarioSuperIntendenciaCia { get; set; }
        public Nullable<System.DateTime> FechaConsultaSuper { get; set; }
        public string RucSuper { get; set; }
        public string TipoCompaniaSuper { get; set; }
        public Nullable<decimal> CapitalSuscritoSuper { get; set; }
        public string CumplimientoObligacionesSuper { get; set; }
        public string MotivoNoCumplimientoObligacionesSuper { get; set; }
        public string PlazoSap { get; set; }
        public int? PlazoSolicitado { get; set; }

        public int IdRTC { get; set; }
        public string RTC { get; set; }
        public string Zona { get; set; }
        public DateTime FechaUltimaSolicitud { get; set; }

        public string ArchivoAdjunto { get; set; }
        public string RutaOrigen { get; set; }
        public string RutaDestino { get; set; }
        public string NombreOriginal { get; set; }

        public bool EnviarRevision { get; set; }

        public string Identificacion { get; set; }
        public string ComentariosNombramiento { get; set; }
        public string ComentariosCertBancarios { get; set; }
        public string ComentariosBienes { get; set; }
        public string ComentariosRefComerciales { get; set; }

        public int ContadorCerrado { get; set; }

        public bool CerrarRequerimientoPorVigencia { get; set; }

        public ICollection<ProcesoCreditoDetalle> ProcesoCreditoDetalle { get; set; }
        public ICollection<ProcesoCreditoAdjunto> ProcesoCreditoAdjunto { get; set; }
        public ICollection<ProcesoCreditoAccionista> ProcesoCreditoAccionista { get; set; }
        public ICollection<ProcesoCreditoAdmActual> ProcesoCreditoAdmActual { get; set; }
        public ICollection<ProcesoCreditoBuro> ProcesoCreditoBuro { get; set; }
        public ICollection<ProcesoCreditoFuncionJudicial> ProcesoCreditoFuncionJudicial { get; set; }
        public ICollection<ProcesoCreditoSri> ProcesoCreditoSri { get; set; }
        public ICollection<ProcesoCreditoResolucionAdjunto> ProcesoCreditoResolucionAdjunto { get; set; }
        public ProcesoCreditoCedula ProcesoCreditoCedula { get; set; }
        public ICollection<ProcesoCreditoNombramiento> ProcesoCreditoNombramiento { get; set; }
        public ProcesoCreditoRuc ProcesoCreditoRuc { get; set; }
        public ICollection<ProcesoCreditoOtrosActivos> ProcesoCreditoOtrosActivos { get; set; }
        public ICollection<ProcesoCreditoRefBancaria> ProcesoCreditoRefBancaria { get; set; }
        public ICollection<ProcesoCreditoRefComercial> ProcesoCreditoRefComercial { get; set; }
        public ICollection<ProcesoCreditoPropiedades> ProcesoCreditoPropiedades { get; set; }
    }

    public class ProcesoCreditoResolucionAdjunto
    {
        public int IdProcesoCreditoResolucionAdjunto { get; set; }
        public int IdProcesoCredito { get; set; }
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

    public class ProcesoCreditoDetalle
    {
        public int IdProcesoCreditoDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int IdProcesoCredito { get; set; }
        public int IdCheckList { get; set; }
        public string CheckList { get; set; }
        public string Completado { get; set; }
        public string Estado { get { return Diccionario.gsGetDescripcion(CodigoEstado); } }
        public string Transaccion { get; set; }
        public string Revision { get; set; }

        public string ArchivoAdjunto { get; set; }
        public string RutaOrigen { get; set; }
        public string RutaDestino { get; set; }
        public string Add { get; set; }
        public string NombreOriginal { get; set; }
        public string TrazAdj { get; set; }
        public string DelAdj { get; set; }
        public DateTime FechaReferencial { get; set; }
        public DateTime? FechaCompromiso { get; set; }
        public string TrazFec { get; set; }
        public string Ver { get; set; }
        public string Comentarios { get; set; }
        public string TrazCom { get; set; }
        public Nullable<bool> FisicoRecibido { get; set; }
        public Nullable<System.DateTime> FechaFisicoRecibido { get; set; }
        public string VerComentarios { get; set; }
        public string Aprobar { get; set; }
        public string Corregir { get; set; }
        public string EnEspera { get; set; }
        public string Excepcion { get; set; }
        public string DesAprobar { get; set; }
        public decimal MontoReferencial { get; set; }
        public int? Periodo { get; set; }
        public string Del { get; set; }

    }

    public class ProcesoCreditoAdjunto
    {
        public int IdProcesoCreditoAdjunto { get; set; }
        public int IdProcesoCredito { get; set; }
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

    public class ProcesoCreditoDetalleRevision
    {
        public bool Sel { get; set; }
        public int IdProcesoCreditoDetalle { get; set; }
        public int IdProcesoCredito { get; set; }
        public int IdCheckList { get; set; }
    }

    public class ActualizaEstatus
    {
        public int IdPlantillaSeguro { get; set; }
        public int IdProcesoCredito { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Usuario { get; set; }
        public string Zona { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaResolucion { get; set; }


        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string EstatusSeguro { get; set; }
        public string CalificacionSeguro { get; set; }

        public string Resolucion { get; set; }
        public string Ver { get; set; }
        public string Del { get; set; }

    }

    public partial class ProcesoCreditoAccionista
    {
        public int IdProcesoCreditoAccionista { get; set; }
        public string CodigoEstado { get; set; }
        public int IdProcesoCredito { get; set; }
        public string Accionista { get; set; }
        public decimal Capital { get; set; }
        public decimal PorcentajeParticipacionAcciones { get; set; }
        public string Del { get; set; }
    }

    public partial class ProcesoCreditoAdmActual
    {
        public int IdProcesoCreditoAdmActual { get; set; }
        public string CodigoEstado { get; set; }
        public int IdProcesoCredito { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; }
        public System.DateTime FechaNombramiento { get; set; }
        public int Anios { get; set; }
        public string Tipo { get; set; }
        public string Accionista { get; set; }
        public decimal PorcentajeParticipacionAcciones { get; set; }
        public string Del { get; set; }
    }

    public partial class ProcesoCreditoFuncionJudicial
    {
        public int IdProcesoCreditoFuncionJudicial { get; set; }
        public string CodigoEstado { get; set; }
        public int IdProcesoCredito { get; set; }
        public System.DateTime FechaConsulta { get; set; }
        public string DemandasVigentes { get; set; }
        public Nullable<System.DateTime> FechaCaso { get; set; }
        public string Ofendido { get; set; }
        public string DetalleAccion { get; set; }
        public decimal MontoDemanda { get; set; }
        public string Del { get; set; }
    }

    public partial class ProcesoCreditoBuro
    {
        public int IdProcesoCreditoBuro { get; set; }
        public string CodigoEstado { get; set; }
        public int IdProcesoCredito { get; set; }
        public string HabilitadoCtaCte { get; set; }
        public string Nombre { get; set; }
        public System.DateTime FechaConsulta { get; set; }
        public Nullable<System.DateTime> FechaInhabilitado { get; set; }
        public Nullable<int> TiempoInhabilitado { get; set; }
        public int Score { get; set; }
        public decimal PorcentajeProbMora { get; set; }
        public decimal RiesgoComercial { get; set; }
        public decimal RiesgoFinanciero { get; set; }
        public decimal TotalRiesgo { get { return RiesgoComercial + RiesgoFinanciero; } }
        public decimal TotalDeudaVencida { get; set; }
        public int CuotasMensualesCancelTiempo { get; set; }
        public int CuotasMensualesPagadasMora { get; set; }
        public string Del { get; set; }
    }

    public partial class ProcesoCreditoSri
    {
        public int IdProcesoCreditoSri { get; set; }
        public string CodigoEstado { get; set; }
        public int IdProcesoCredito { get; set; }
        public System.DateTime FechaConsulta { get; set; }
        public string ObligacionesPdtes { get; set; }
        public string TipoObligacion { get; set; }
        public decimal ValorTotalObligacion { get; set; }
        public string MotivoObligacionesPdtes { get; set; }
        public string Del { get; set; }
    }

    public partial class ProcesoCreditoCedula
    {
        public int IdProcesoCredito { get; set; }
        public string IdentificacionRepLegal { get; set; }
        public string NombreRepLegal { get; set; }
        public DateTime? FechaCaducidadCedulaRepLegal { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Edad { get { return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) >= new DateTime(DateTime.Now.Year, FechaNacimiento.Month, FechaNacimiento.Day) ? DateTime.Now.Year - FechaNacimiento.Year : (DateTime.Now.Year - FechaNacimiento.Year) - 1; } }
        public string CodigoEstadoCivil { get; set; }
        public string IdentificacionConyuge { get; set; }
        public string NombreConyuge { get; set; }
        public DateTime FechaIngreso { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string ComentarioRevisionCedula { get; set; }

    }

    public class ProcesoCreditoNombramiento
    {
        public int IdProcesoCreditoNombramiento { get; set; }
        public int IdProcesoCredito { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public int Periodo { get; set; }
        public DateTime FechaCaducidad { get { return FechaInscripcion.AddYears(Periodo); } }
        [MaxLength(200)]
        public string Cargo { get; set; }
        public string CodigoPresentacion { get; set; }
        public string Del { get; set; }
    }

    public class ProcesoCreditoRuc
    {
        public ProcesoCreditoRuc()
        {
            this.ProcesoCreditoRucDireccion = new List<ProcesoCreditoRucDireccion>();
        }

        public int IdProcesoCredito { get; set; }
        public string Identificacion { get; set; }
        public string NombreComercial { get; set; }
        public string EstadoRuc { get; set; }
        public string ObligadoLlevarCantidadRuc { get; set; }
        public DateTime FechaIngreso { get; set; }
        public Nullable<System.DateTime> FechaInicioActividadRuc { get; set; }
        public Nullable<System.DateTime> FechaReinicioActividadRuc { get; set; }
        public Nullable<System.DateTime> FechaUltActualizacionRuc { get; set; }
        public string CodigoTipoNegocio { get; set; }
        public string ActividadesSecundarias { get; set; }
        public string OtrasActividades { get; set; }
        public string Comentarios { get; set; }

        public ICollection<ProcesoCreditoRucDireccion> ProcesoCreditoRucDireccion { get; set; }
    }

    public class ProcesoCreditoRucDireccion
    {
        public int IdProcesoCreditoRucDireccion { get; set; }
        public int IdProcesoCredito { get; set; }
        public bool Principal { get; set; }
        [MaxLength(300)]
        public string Direccion { get; set; }
        public string Del { get; set; }
    }

    public class Destinatarios
    {
        public string Tipo { get; set; }
        public string Correo { get; set; }
        public string Persona { get; set; }
        public string Del { get; set; }
    }

    public class ProcesoCreditoOtrosActivos
    {
        public int IdProcesoCreditoOtrosActivos { get; set; }
        public int IdProcesoCredito { get; set; }
        public string CodigoTipoOtrosActivos { get; set; }
        [MaxLength(50)]
        public string Marca { get; set; }
        [MaxLength(50)]
        public string Modelo { get; set; }
        [MaxLength(4)]
        public int Anio { get; set; }
        [MaxLength(18)]
        public decimal AvaluoComercial { get; set; }
        public DateTime FechaPago { get; set; }
        public string DescripcionDocumento { get; set; }
        public string PropietarioBeneficiario { get; set; }

        public string Del { get; set; }
    }

    public class ProcesoCreditoRefBancaria
    {
        public int IdProcesoCreditoRefBancaria { get; set; }
        public int IdProcesoCredito { get; set; }
        public DateTime FechaEmision { get; set; }
        public string CodigoBanco { get; set; }
        public string CodigoTipoCuentaBancaria { get; set; }
        [MaxLength(30)]
        public string NumeroCuenta { get; set; }
        public string Titular { get; set; }
        public Nullable<System.DateTime> FechaApertura { get; set; }
        public decimal SaldosPromedios { get; set; }
        public string ArchivoAdjunto { get; set; }
        public string RutaOrigen { get; set; }
        public string RutaDestino { get; set; }
        public string Add { get; set; }
        public string NombreOriginal { get; set; }
        public string Ver { get; set; }


        public string Del { get; set; }
    }

    public class ProcesoCreditoRefComercial
    {
        public int IdProcesoCreditoRefComercial { get; set; }
        public int IdProcesoCredito { get; set; }
        public DateTime FechaConsulta { get; set; }
        public DateTime ClienteDesde { get; set; }
        public DateTime? FechaReferenciaComercial { get; set; }
        [MaxLength(100)]
        public string Nombre { get; set; }
        [MaxLength(10)]
        public string Telefono { get; set; }
        public string ArchivoAdjunto { get; set; }
        public string RutaOrigen { get; set; }
        public string RutaDestino { get; set; }
        public string Add { get; set; }
        public string NombreOriginal { get; set; }
        public string Ver { get; set; }
        public decimal Cupo { get; set; }
        public int Plazo { get; set; }
        public decimal Garantia { get; set; }
        public string CodigoGarantia { get; set; }
        public decimal PromedioComprasMensual { get; set; }
        public decimal PromedioComprasAnual { get; set; }
        public int PromedioDiasPagos { get; set; }
        public string Del { get; set; }
    }

    public class ProcesoCreditoPropiedades
    {
        public int IdProcesoCreditoPropiedades { get; set; }
        public int IdProcesoCredito { get; set; }
        public string CodigoTipoBien { get; set; }
        [MaxLength(100)]
        public string Direccion { get; set; }
        public string Hipoteca { get; set; }
        [MaxLength(18)]
        public decimal AvaluoComercial { get; set; }
        public DateTime FechaPago { get; set; }
        public string DescripcionDocumento { get; set; }
        public string PropietarioBeneficiario { get; set; }
        public string Del { get; set; }
    }
}
