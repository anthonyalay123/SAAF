using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 21/01/2020
    /// Clase donde estarán todos las variables fijas para evitar quemar código
    /// </summary>
    public static class Diccionario
    {
        public static class Correo
        {
            public const string OrdenCompra = "OOC";
            public const string OrdenPagoContabilidad = "OPC";

        }

        public static class TipoAprobacion
        {
            public const string Aprobado = "APR";
            public const string Seguimiento = "SEG";

        }

        public static class Credenciales
        {
            public static class Ftp
            {
                public static string Servidor = "35.215.99.152";
                public static string UrlRoot = "ftp://35.215.99.152/";
                public static string Puerto = "21";
                public static string Usuario = "jordonez@afecor.com";
                public static string Contrasena = "@fec0r$!.*";
                public static bool Cifrado = false;
            }
        }

        public static class CarpetaFtp
        {
            public static string TalentoHumano = "afecor.com/public_html/Talento_Humano/";
            public static string Ventas = "afecor.com/public_html/Ventas/";
        }


        public const string Activo = "A";
        public const string Inactivo = "I";
        public const string Eliminado = "E";
        public const string Pendiente = "P";
        public const string Jubilado = "J";
        public const string Cerrado = "C";
        public const string Aprobado = "S";
        public const string Negado = "N";
        public const string PreAprobado = "R";
        public const string Devengado = "D";
        public const string Corregir = "O";
        public const string Cotizado = "T";
        public const string Generado = "G";
        public const string Rechazado = "H";
        public const string Cargado = "U";
        public const string EnRevision = "V";
        public const string EnResolucion = "W";
        public const string EnAnalisis = "X";
        public const string Finalizado = "F";
        public const string Excepcion = "Y";
        public const string EnEspera = "Z";

        public const string DesActivo = "ACTIVO";
        public const string DesInactivo = "INACTIVO";
        public const string DesEliminado = "ELIMINADO";
        public const string DesPendiente = "PENDIENTE";
        public const string DesJubilado = "JUBILADO";
        public const string DesCerrado = "CERRADO";
        public const string DesAprobado = "APROBADO";
        public const string DesPreAprobado = "PRE-APROBADO";
        public const string DesDevengado = "DEVENGADO";
        public const string DesNegado = "NEGADO";
        public const string DesRechazado = "RECHAZADO";
        public const string DesCorregir = "CORREGIR";
        public const string DesCargado = "CARGADO";
        public const string DesEnRevision = "EN REVISIÓN";
        public const string DesEnResolucion = "EN RESOLUCIÓN";
        public const string DesEnAnalisis = "EN ANÁLISIS";
        public const string DesFinalizado = "FINALIZADO";
        public const string DesExcepcion = "EXCEPCIÓN";
        public const string DesEnEspera = "EN ESPERA";


        public const string EstadoSCAprobado = "APR";
        public const string EstadoSCCorregir = "COR";
        public const string EstadoSCPendiente = "PEN";
        public const string EstadoSCRechazado = "REC";


        public static class TipoMovimiento
        {
            public const string Ingreso = "I";
            public const string DesIngreso = "INGRESO";

            public const string Egreso = "E";
            public const string DesEgreso = "EGRESO";
        }

        /// <summary>
        /// Función que devuelve la descripción del estado
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public static string gsGetDescripcion(string tsCodigo)
        {
            string psValor = string.Empty;
            if (tsCodigo == Activo) psValor = DesActivo;
            if (tsCodigo == Inactivo) psValor = DesInactivo;
            if (tsCodigo == Eliminado) psValor = DesEliminado;
            if (tsCodigo == Pendiente) psValor = DesPendiente;
            if (tsCodigo == Cerrado) psValor = DesCerrado;
            if (tsCodigo == Aprobado) psValor = DesAprobado;
            if (tsCodigo == PreAprobado) psValor = DesPreAprobado;
            if (tsCodigo == Devengado) psValor = DesDevengado;
            if (tsCodigo == Negado) psValor = DesNegado;
            if (tsCodigo == Corregir) psValor = DesCorregir;
            if (tsCodigo == Rechazado) psValor = DesRechazado;
            if (tsCodigo == Cargado) psValor = DesCargado;
            if (tsCodigo == EnRevision) psValor = DesEnRevision;
            if (tsCodigo == EnResolucion) psValor = DesEnResolucion;
            if (tsCodigo == EnAnalisis) psValor = DesEnAnalisis;
            if (tsCodigo == Finalizado) psValor = DesFinalizado;
            if (tsCodigo == Excepcion) psValor = DesExcepcion;
            if (tsCodigo == EnEspera) psValor = DesEnEspera;
            return psValor;
        }

        /// <summary>
        /// Función que devuelve el código del estado
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public static string gsGetCodigo(string tsDescripcion)
        {
            string psValor = string.Empty;
            if (tsDescripcion == DesActivo) psValor = Activo;
            if (tsDescripcion == DesInactivo) psValor = Inactivo;
            if (tsDescripcion == DesEliminado) psValor = Eliminado;
            if (tsDescripcion == DesPendiente) psValor = Pendiente;
            if (tsDescripcion == DesCerrado) psValor = Cerrado;
            if (tsDescripcion == DesAprobado) psValor = Aprobado;
            if (tsDescripcion == DesPreAprobado) psValor = PreAprobado;
            if (tsDescripcion == DesDevengado) psValor = Devengado;
            if (tsDescripcion == DesNegado) psValor = Negado;
            if (tsDescripcion == DesCorregir) psValor = Corregir;
            if (tsDescripcion == DesRechazado) psValor = Rechazado;
            if (tsDescripcion == DesCargado) psValor = Cargado;
            if (tsDescripcion == DesEnRevision) psValor = EnRevision;
            if (tsDescripcion == DesEnResolucion) psValor = EnResolucion;
            if (tsDescripcion == DesEnAnalisis) psValor = EnAnalisis;
            if (tsDescripcion == DesFinalizado) psValor = Finalizado;
            if (tsDescripcion == DesEnEspera) psValor = EnEspera;
            return psValor;
        }

        public const string Seleccione = "0";
        public const string DesSeleccione = "Seleccione...";
        public const string DesTodas = "TODAS";
        public const string DesTodos = "TODOS";

        public const string MsgTituloRegistroGuardado = "Aviso";
        public const string MsgRegistroGuardado = "Guardado Exitosamente.";
        public const string MsgRegistroNoGuardado = "No se pudo Grabar!";


        public const string MsgTituloRegistroAEliminar = "Aviso";
        public const string MsgRegistroAEliminar = "¿Está seguro de Eliminar Registro?";
        public const string MsgRegistroAGuardar = "¿Está seguro de Guardar Cambios?";


        public const string MsgTituloRegistroEliminado = "Aviso";
        public const string MsgRegistroEliminado = "Registro Eliminado.";

        public const string Cargar = "Cargado";
        public const string Descargar = "Descargar";

        public const string ModuloTalentoHumano = "Módulo Talento Humano";

        public const string MsgRegistroAprobar = "Aprobado Exitosamente.";
        public const string MsgRegistroNoAprobado = "No se pudo Aprobar!";
        public const string MsgRegistroAAprobar = "¿Está seguro de Aprobar?";

        public const string MsgRegistroARechazar = "¿Está seguro de Rechazar el Registro?";
        public const string MsgRegistroRechazado = "Registro Rechazado.";


        #region Variables de Config
        public static class Config
        {
            public static string FormatoFechaBD { get; set; }
            public static string FormatoFechaHoraBD { get; set; }
            public static string FormatoFechaSistema { get; set; }
            public static float FontSizeMenu { get; set; }
            public static float FontSizeMenuItem { get; set; }
        }
        #endregion


        public static class Auditoria
        {
            public static class TipoAccion
            {
                public const string Insert = "IN";
                public const string Update = "UP";
            }

        }

        public static class BuscarCodigo
        {
            public static class Tipo
            {
                public const string Primero = "P";
                public const string Anterior = "A";
                public const string Siguiente = "S";
                public const string Ultimo = "U";
            }

        }

        public static class ListaCatalogo
        {
            public const string GrupoCatalogo = "000";

            public const string TipoPersona = "001";
            public static class TipoPersonaClass
            {
                public const string Natural = "NAT";
            }

            public const string TipoIdentificacion = "002";
            public static class TipoIdentificacionClass
            {
                public const string Cedula = "CED";
            }

            public const string NivelEducacion = "003";
            public const string ColorPiel = "004";
            public const string ColorOjos = "005";
            public const string TipoSangre = "006";
            public const string TipoLicencia = "007";
            public const string Genero = "008";
            public const string EstadoCivil = "009";
            public const string TipoCargaFamiliar = "010";
            public const string TipoContacto = "011";
            public const string Parentezco = "012";
            public const string Sucursal = "013";
            public const string Departamento = "014";
            public const string MotivoFinContrato = "015";
            public const string Banco = "016";
            public const string FormaPago = "017";
            public const string TipoCuentaBancaria = "018";
            public const string TipoEmpleado = "019";
            public const string TipoVivienda = "020";
            public const string TipoMaterialVivienda = "021";
            //public const string TipoContrato = "022";
            public const string TipoCategoriaRubro = "023";
            public static class TipoCategoriaRubroClass
            {
                public const string AnticipoFDM = "001";
                public const string Automaticos = "002";
                public const string SeguroSocial = "003";
                public const string Sueldo = "004";
                public const string TotalIngresos = "005";
                public const string TotalEgresos = "006";
                public const string NetoRecibir = "007";
                public const string DiasLaborados = "008";
                public const string SueldoGanado = "009";
                public const string Faltas = "010";
                public const string IngresosIESS = "011";
                public const string IngresosNOIESS = "012";
                public const string ProvisionFondoReserva = "013";
                public const string ProvisionDecimoTercero = "014";
                public const string ProvisionDecimoCuarto = "015";
                public const string ProvisionUtilidades = "016";
            }
            public const string TipoProcesoNomina = "024";
            public const string Region = "025";
            public const string TipoBeneficioSocial = "026";
            public static class TipoBeneficioSocialClass
            {
                public const string ProvisionVacaciones = "PVA";
                public const string ProvisionDecimoTercero = "PDT";
                public const string ProvisionDecimoCuarto = "PDC";
                public const string ProvisionFondoReserva = "PFR";
            }
            public const string TipoDiscapacidad = "027";
            public const string TipoContabilizacion = "028";
            public static class TipoContabilizacionClass
            {
                public const string Empleado = "001";
                public const string Agrupado = "002";
                public const string SinCuenta = "003";
            }
            public const string TipoVacacion = "029";
            public static class TipoVacacionClass
            {
                public const string Gozadas = "001";
                public const string Pagadas = "002";
            }

            public const string TipoItems = "030";
            public const string TipoHoraExtra = "031";
            public static class TipoHoraExtraClass
            {
                public const string Recargo25 = "025";
                public const string Recargo50 = "050";
                public const string Recargo100 = "100";
            }
            public const string Titulo = "032";
            public const string SapFamilia = "033";
            public const string TipoMovimiento = "034";
            public const string TipoCompra = "035";
            public const string MaterialEnvase = "MAT";
            public static class TipoMovimientoClass
            {
                public const string PrestamoQuirografario = "PQ";
                public const string PrestamoHipoticareo = "PH";
                public const string PrestamoInterno = "PI";
                public const string DescuentosProgramados = "DP";
                public const string Anticipos = "AN";
                public const string PrestamoQuirografarioContrapartida = "PQC";
                public const string PrestamoHipoticareoContrapartida = "PHC";
                public const string PrestamoInternoContrapartida = "PIC";
            }

            public const string GrupoFamilia = "036";
            public const string MotivoPrestamoInterno = "037";
            public const string TipoTransporte = "038";
            public const string TipoOrdenPago = "039";
            public const string TipoCodigoComision = "040";
            public const string AgrupacionCobranza = "041";
            public const string ResponsableCobranza = "042";
            public const string TipoPrestamoAnticipoDescuento = "043";
            public const string TipoProducto = "044";
            public const string AgrupacionActivoFijo = "045";
            public const string TipoEnvasado = "046";
            public const string ControlMateriaPrima = "047";
            public const string TipoItemEPP = "048";
            public const string MotivoEgresoInventarioEPP = "049";
            public const string MotivoIngresoInventarioEPP = "050";
            public const string EstadoActivoFijo = "051";
            public const string TipoSolicitudCredito = "052";
            public static class TipoSolicitudCreditoClass
            {
                public const string Nuevo = "NUE";
                public const string Reactivacion = "REA";
                public const string AumentoCupo = "AUM";
                public const string Actualizacion = "ACT";
                public const string AumentoPlazo = "APL";
            }
            public const string ActividadCliente = "053";
            public static class ActividadClienteClass
            {
                public const string Distribuidor = "DIS";
                public const string EmpresaAgricola = "EMA";
                public const string Agricultor = "AGR";
                public const string Fumigadores = "FUM";

            }
            public const string Vivienda = "054";
            public const string Local = "055";
            public const string TipoOtrosActivos = "056";
            public const string TipoRelacionPersona = "057";
            public const string TipoReferenciaPersonal = "058";
            public const string TipoBien = "059";
            public const string TipoItem = "060";
            public const string MotivoEgresoInventario = "061";
            public const string MotivoIngresoInventario = "062";
            public const string EstatusSeguro = "063";
            public const string TipoProcesoCredito = "064";
            public static class TipoProcesoCreditoClass
            {
                public const string ActualizacionDatos = "ACT";
                public const string AumentoPlazo = "APL";
                public const string AumentoCupo = "AUM";
                public const string ClientesNuevos = "NUE";
                public const string ReactivacionCredito = "REA";

            }
            public const string TipoDestinatario = "065";
            public const string PresentacionNombramiento = "066";
            public const string SectorInformeRtc = "067";
            public const string TipoFichaMedica = "068";
            public const string TipoServicioBasico = "069";
            public const string TipoCompania = "070";
            public const string EstadoIMC = "071";
            public const string CaracteristicaVivienda = "072";
            public const string TipoAdministrador = "073";
            public const string Años = "074";
            public const string PagareRefComercial = "075";
            public const string TipoObligacionesSRI = "076";
            public const string TipoMovimientoActivo = "077";
            public const string MotivoSeguimientoCompromiso = "078";

            public const string ConceptoLogistica = "084";

            public const string SubtipoItem = "085";

            public const string MotivoTransferencia = "MOT";
            public const string MotivoGuiasManuales = "086";
            public const string TipoSolicitudAnticipo = "087";
        }

        public static class Tablas
        {

            public static class TipoRubro
            {
                public const string Ingresos = "001";
                public const string Egresos = "002";
                public const string Sistema = "003";
            }

            public static class TipoRol
            {
                public const string PrimeraQuincena = "001";
                public const string FinMes = "002";
                public const string DecimoCuarto = "003";
                public const string DecimoTercero = "004";
                public const string Comisiones = "005";
                public const string Jubilados = "006";
                public const string Utilidades = "007";
            }

            public static class Menu
            {
                public const string Novedad = "Transacciones.frmTrNovedad";
                public const string FondoReserva = "Transacciones.frmTrFondoReserva";
                public const string GenerarPago = "Transacciones.frmTrGeneraPago";
                public const string ConsultaRol = "Transacciones.frmTrConsultaRol";
                public const string EnviarCorreoRol = "Transacciones.frmTrEnviarCorreoRol";
                public const string SolicitudCompra = "Compras.Transaccion.frmTrSolicitudCompras";
                public const string CuentaContable = "Parametrizadores.frmPaCuentaContable";
                public const string CuentaContableRubro = "Parametrizadores.frmPaCuentaContableRubro";
                public const string CuentaContableTipoBS = "Parametrizadores.frmPaCuentaContableTipoBS";
                public const string Cotizacion = "Compras.Transaccion.frmTrCotizacion";
                public const string AprobacionCotizacion = "Compras.Transaccion.frmTrCotizacionAprobacion";
                public const string BandejaSolicitudCompra = "Compras.Transaccion.frmTrBandejaSolicitudCompra";
                public const string BandejaSolicitudCompraAprobadas = "Compras.Transaccion.frmTrSolicitudesCompraAprobadas";
                public const string OrdenCompra = "Compras.Transaccion.frmTrOrdenCompra";
                public const string BandejaCotizacion = "Compras.Transaccion.frmTrBandejaCotizacion";
                public const string OrdenPago = "Compras.Transaccion.frmTrOrdenPago";
                public const string frmTrBandejaOrdenPago = "Compras.Transaccion.frmTrBandejaOrdenPago";
                public const string BandejaPermisoHoras = "TalentoHumano.Transacciones.frmTrBandejaPermisoHoras";
                public const string Proveedores = "Compras.Transaccion.frmPaProveedores";
                public const string frmTrBandejaFactPendPago = "Compras.Transaccion.frmTrBandejaFactPendPago";
                public const string frmTrBandejaFactPendPagoPorAprobar = "Compras.Transaccion.frmTrBandejaFactPendPagoPorAprobar";
                public const string frmTrBandejaFactPendPagoContabilidad = "Compras.Transaccion.frmTrBandejaFactPendPagoContabilidad";
                public const string frmTrBandejaFactPendPagoTesoreria = "Compras.Transaccion.frmTrBandejaFactPendPagoTesoreria";
                public const string frmTrBandejaFactPendPagoFinanciero = "Compras.Transaccion.frmTrBandejaFactPendPagoFinanciero";
                public const string frmTrLiquidacion = "Transacciones.frmTrLiquidacion";
                public const string frmTrBandejaLiquidaciones = "TalentoHumano.Transacciones.frmTrBandejaLiquidaciones";
                public const string frmTrBorradorComisiones = "Cobranza.Transacciones.frmTrBorradorComisiones";
                public const string frmTrConsultarComisiones = "Cobranza.Transacciones.frmTrConsultarComisiones";
                public const string frmTrSolicitudVacacion = "Transacciones.frmTrSolicitudVacacion";
                public const string frmTrBandejaSolicitudVacaciones = "TalentoHumano.Transacciones.frmTrBandejaSolicitudVacaciones";
                public const string frmMaGrupoPago = "Compras.Maestros.frmMaGrupoPago";
                public const string frmTrSolicitudCredito = "Credito.Transacciones.frmTrSolicitudCredito";
                public const string frmTrPlantillaSeguro = "Credito.Transacciones.frmTrPantillaSeguro";
                public const string frmTrTransferenciaStock = "SHEQ.Transacciones.frmTrTransferenciaStock";
                public const string frmTrMovimientoInventario = "SHEQ.Transacciones.frmTrMovimientoInventario";
                public const string frmTrEntregaEPP = "SHEQ.Transacciones.frmTrEntregaEPP";
                public const string frmTrPlantillaSeguroRevision = "Credito.Transacciones.frmTrPantillaRevision";
                public const string frmTrProcesoCredito = "Credito.Transacciones.frmTrProcesoCredito";
                public const string frmTrCambiarEstatus = "Credito.Transacciones.frmTrCambiarEstatus";
                public const string frmTrPantillaRevision = "Credito.Transacciones.frmTrPantillaRevision";
                public const string frmTrRevisionRuc = "Credito.Transacciones.frmTrRevisionRuc";
                public const string frmTrRevisionCedula = "Credito.Transacciones.frmTrRevisionCedula";
                public const string frmTrRevisionNombramiento = "Credito.Transacciones.frmTrRevisionNombramiento";
                public const string frmTrInformeRTC = "Credito.Transacciones.frmTrInformeRTC";
                public const string frmPrFichaMedica = "TalentoHumano.Parametrizadores.frmPrFichaMedica";
                public const string frmTrRevisionPlanillaServicioBasico = "Credito.Transacciones.frmTrRevisionPlanillaServicioBasico";
                public const string frmTrRevisionRefComercial = "Credito.Transacciones.frmTrRevisionRefComercial";
                public const string frmTrRevisionRefBancaria = "Credito.Transacciones.frmTrRevisionRefBancaria";
                public const string frmTrRevisionBienes = "Credito.Transacciones.frmTrRevisionBienes";
                public const string frmTrGuiaRemision = "Compras.Transaccion.frmTrGuiaRemision";
                public const string frmTrRecepcionIngreso = "SHEQ.Transacciones.frmTrRecepcionIngreso";
                public const string frmTrBandejaAprobacionRebate = "Ventas.Transacciones.frmTrBandejaAprobacionRebate";
                public const string frmTrBandejaAprobacionRebateAnual = "Ventas.Transacciones.frmTrBandejaAprobacionRebateAnual";
                public const string SolicitudAnticipo = "Compras.Transaccion.frmTrSolicitudAnticipo";
                public const string LiquidacionAnticipo = "Compras.Transaccion.frmTrLiquidacionAnticipo";
            }

            public static class Transaccion
            {
                public const string Comision = "001";
                public const string CotizacionGanadora = "002";
                public const string OrdenPago = "003";
                public const string PermisoPorHoras = "004";
                public const string FacturaPago = "005";
                public const string Liquidacion = "006";
                public const string SolicitudVacaciones = "007";
                public const string RequerimientoCredito = "008";
                public const string Checklist = "009";
                public const string Ticket = "010";
                public const string Rebate = "011";
                public const string RebateAnual = "012";
                public const string CheckListAdjunto = "013";
                public const string CheckListFechaCompromiso = "014";
                public const string CheckListComentarios = "015";
                public const string OrdenPagoFactura = "016";
                public const string SolicitudAnticipo = "017";
                public const string LiquidacionAnticipo = "018";
            }

            public static class TipoPermiso
            {
                public const string Vacaciones = "001";
                public const string PermisoLactancia = "005";
                public const string VacacionesPagadas = "015";
                public const string AnticipoVacaciones = "016";
            }

            public static class TipoSecuencia
            {
                public const string Nomina = "001";
                public const string MultiCash = "002";
            }

            public static class CheckList
            {
                public const int PlantillaCalifSeguro = 1;
                public const int SolicitudCredito = 2;
                public const int CedulaTitularRepLegal = 3;
                public const int CedulaConyuge = 4;
                public const int RucCertSriRuc = 5;
                public const int NombramientotRepLeg = 6;
                public const int UltImpRtaEEFF = 7;
                public const int EscritConstEmpresa = 8;
                public const int PlanillaAguaLuz = 9;
                public const int PrediosCertfBienes = 10;
                public const int CertfBanco = 11;
                public const int InformeRTC = 12;
                public const int Pagare = 13;
                public const int CertifComercial = 14;
                public const int PlanComercialAcuerdoComercial = 15;

            }

        }

        public static readonly string[] EstadosNoIncluyentesSistema = { Inactivo, Eliminado };

        public static class ButtonGridImage
        {
            public const string trash_16x16 = "images/actions/trash_16x16.png";
            public const string open_16x16 = "images/actions/open_16x16.png";
            public const string download_16x16 = "images/actions/download_16x16.png";
            public const string show_16x16 = "images/actions/show_16x16.png";
            public const string printer_16x16 = "images/print/printer_16x16.png";
            public const string printviadpf_16x16 = "images/print/printviadpf_16x16.png";
            public const string refresh_16x16 = "images/actions/refresh_16x16.png";
            public const string add_16x16 = "images/actions/add_16x16.png";
            public const string showhidecomment_16x16 = "images/comments/showhidecomment_16x16.png";
            public const string deletelist2_16x16 = "images/actions/deletelist2_16x16.png";
            public const string removeitem_16x16 = "images/actions/removeitem_16x16.png";
            public const string converttorange_16x16 = "images/actions/converttorange_16x16.png";
            public const string inserttable_16x16 = "images/richedit/inserttable_16x16.png";
            public const string find_16x16 = "images/find/find_16x16.png";
            public const string zoom_16x16 = "images/zoom/zoom_16x16.png";
            public const string delete_16x16 = "images/edit/delete_16x16.png";
            public const string iconsetsimbolscircled3 = "images/edit/iconsetsimbolscircled3_16x16.png";
            public const string apply_16x16 = "images/actions/apply_16x16.png";
            public const string cancel_16x16 = "images/actions/cancel_16x16.png";
            public const string exporttodpf_16x16 = "images/export/exporttodpf_16x16.png";

        }

    }
}
