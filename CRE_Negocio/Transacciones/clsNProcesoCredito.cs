using GEN_Entidad;
using GEN_Entidad.Entidades.Credito;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using static GEN_Entidad.Diccionario;

namespace CRE_Negocio.Transacciones
{
    public class clsNProcesoCredito : clsNBase
    {

        public ProcesoCredito goConsultar(int tId)
        {
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Find<CRETPROCESOCREDITO>().Include(x=>x.CRETPROCESOCREDITODETALLE).Include(x => x.CRETPROCESOCREDITOACCIONISTA)
                .Include(x => x.CRETPROCESOCREDITOADMACTUAL).Include(x => x.CRETPROCESOCREDITOBURO).Include(x=> x.CRETPROCESOCREDITOFUNCIONJUDICIAL)
                .Include(x => x.CRETPROCESOCREDITOSRI).Include(x=>x.CRETPROCESOCREDITORESOLUCIONADJUNTO).Include(x=>x.CRETPROCESOCREDITOCEDULA)
                .Include(x=>x.CRETPROCESOCREDITONOMBRAMIENTO).Include(x=>x.CRETPROCESOCREDITORUC.CRETPROCESOCREDITORUCDIRECCION).Include(x => x.CRETPROCESOCREDITOPROPIEDADES)
                .Include(x=>x.CRETPROCESOCREDITOOTROSACTIVOS).Include(x => x.CRETPROCESOCREDITOREFBANCARIA).Include(x => x.CRETPROCESOCREDITOREFCOMERCIAL)
                .Where(x => x.IdProcesoCredito == tId).ToList();

            return loLlenarDatos(poLista).FirstOrDefault();
        }

        private List<ProcesoCredito> loLlenarDatos(List<CRETPROCESOCREDITO> poLista)
        {
            var poListaReturn = new List<ProcesoCredito>();

            foreach (var item in poLista)
            {
                var poCab = new ProcesoCredito();
                poCab.Fecha = item.FechaIngreso;
                poCab.IdProcesoCredito = item.IdProcesoCredito;
                poCab.CodigoEstado = item.CodigoEstado;
                poCab.CodigoTipoSolicitud = item.CodigoTipoProceso;
                poCab.TipoSolicitud = item.TipoSolicitud;
                poCab.Observacion = item.Observacion;
                poCab.CodigoCliente = item.CodigoCliente;
                poCab.Cliente = item.Cliente;
                poCab.Completado = item.Completado ?? false;
                poCab.CodigoTipoPersona = item.CodigoTipoPersona;
                poCab.CodigoContado = item.CodigoContado;
                poCab.CodigoEstatusSeguro = item.CodigoEstatusSeguro;
                poCab.EnviarRevision = item.EnviarRevision ?? false;
                poCab.CupoSap = item.CupoSap ?? 0M;
                poCab.CupoSolicitado = item.CupoSolicitado ?? 0M;
                poCab.PlazoSolicitado = item.PlazoSolicitado ?? 0;
                poCab.PlazoSap = item.PlazoSap;
                poCab.RepresentanteLegal = item.RepresentanteLegal;
                poCab.ComentarioRevisor = item.ComentarioRevisor;
                poCab.FechaPlanillaServicioBasico = item.FechaPlanillaServicioBasico ?? DateTime.Now;
                poCab.DireccionPlanillaServicioBasico = item.DireccionPlanillaServicioBasico;
                poCab.ComentarioPlanillaServicioBasico = item.ComentarioPlanillaServicioBasico;
                poCab.CodigoTipoServicioBasico = item.CodigoTipoServicioBasico;
                poCab.TipoServicioBasico = item.TipoServicioBasico;
                poCab.CupoAprobado = item.CupoAprobado;
                poCab.PlazoAprobado = item.PlazoAprobado;
                poCab.FechaResolucion = item.FechaResolucion;
                poCab.ResolucionAfecor = item.ResolucionAfecor;
                poCab.IdRTC = item.IdRTC ?? 0;
                poCab.RTC = item.RTC;
                poCab.Zona = item.Zona;

                poCab.ComentarioBuro = item.ComentarioBuro;
                poCab.ComentarioFuncionJudicial = item.ComentarioFuncionJudicial;
                poCab.ComentarioSri = item.ComentarioSri;
                poCab.ComentarioSuperIntendenciaCia = item.ComentarioSuperIntendenciaCia;
                poCab.FechaConsultaSuper = item.FechaConsultaSuper;
                poCab.RucSuper = item.RucSuper;
                poCab.TipoCompaniaSuper = item.TipoCompaniaSuper;
                poCab.CapitalSuscritoSuper = item.CapitalSuscritoSuper;
                poCab.CumplimientoObligacionesSuper = item.CumplimientoObligacionesSuper;
                poCab.MotivoNoCumplimientoObligacionesSuper = item.MotivoNoCumplimientoObligacionesSuper;

                poCab.ComentarioResolucion = item.ComentarioResolucion;
                poCab.Identificacion = item.Identificacion;
                poCab.ComentariosNombramiento = item.ComentariosNombramiento;
                poCab.ComentariosCertBancarios = item.ComentariosCertBancarios;
                poCab.ComentariosRefComerciales = item.ComentariosRefComerciales;
                poCab.ComentariosBienes = item.ComentariosBienes;
                poCab.CerrarRequerimientoPorVigencia = item.CerrarRequerimientoPorVigencia ?? false;

                poCab.ContadorCerrado = loBaseDa.Find<CRETPROCESOCREDITO>().Where(x => x.CodigoEstado == Diccionario.Cerrado && x.IdProcesoCredito != item.IdProcesoCredito && x.CodigoCliente == item.CodigoCliente).Count();

                var pdFechaUltSol = loBaseDa.Find<CRETPROCESOCREDITO>().Where(x => x.CodigoEstado == Diccionario.Finalizado && x.IdProcesoCredito != poCab.IdProcesoCredito && x.CodigoCliente == poCab.CodigoCliente).Select(x => x.Fecha).FirstOrDefault();
                if (pdFechaUltSol != null)
                {
                    if (pdFechaUltSol != DateTime.MinValue)
                    {
                        poCab.FechaUltimaSolicitud = pdFechaUltSol;
                    }
                }

                poCab.NombreOriginal = item.NombreOriginal;
                poCab.ArchivoAdjunto = item.ArchivoAdjunto;
                poCab.RutaDestino = ConfigurationManager.AppSettings["CarpetaProCre"].ToString();
                poCab.NomUsuario = loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoUsuario == item.UsuarioIngreso).Select(x => x.NombreCompleto).FirstOrDefault();

                poCab.ProcesoCreditoDetalle = new List<ProcesoCreditoDetalle>();
                var psLista = new List<string>();
                psLista.Add(Diccionario.Eliminado);
                psLista.Add(Diccionario.Inactivo);
                foreach (var detalle in item.CRETPROCESOCREDITODETALLE.Where(x => !psLista.Contains(x.CodigoEstado)))
                {
                    var poDet = new ProcesoCreditoDetalle();

                    poDet.CodigoEstado = detalle.CodigoEstado;
                    poDet.CheckList = detalle.CheckList;
                    poDet.Completado = detalle.Completado ?? false ? "SI" : "NO";
                    poDet.IdCheckList = detalle.IdCheckList;
                    poDet.IdProcesoCredito = detalle.IdProcesoCredito;
                    poDet.IdProcesoCreditoDetalle = detalle.IdProcesoCreditoDetalle;
                    poDet.ArchivoAdjunto = detalle.ArchivoAdjunto;
                    poDet.NombreOriginal = detalle.NombreOriginal;
                    poDet.FechaReferencial = detalle.FechaReferencial ?? DateTime.Now;
                    poDet.FechaCompromiso = detalle.FechaCompromiso;
                    poDet.RutaDestino = ConfigurationManager.AppSettings["CarpetaProCre"].ToString();
                    poDet.Comentarios = detalle.Comentarios;
                    poDet.MontoReferencial = detalle.MontoReferencial ?? 0;
                    poDet.Periodo = detalle.Periodo;
                    poDet.FechaFisicoRecibido = detalle.FechaFisicoRecibido;
                    poDet.FisicoRecibido = detalle.FisicoRecibido;
                    //poDet.FechaCompromiso
                    poCab.ProcesoCreditoDetalle.Add(poDet);
                }

                poCab.ProcesoCreditoAdjunto = new List<ProcesoCreditoAdjunto>();
                foreach (var detalle in item.CRETPROCESOCREDITOADJUNTO.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var det = new ProcesoCreditoAdjunto();

                    det.IdProcesoCreditoAdjunto = detalle.IdProcesoCreditoAdjunto;
                    det.IdProcesoCredito = detalle.IdProcesoCredito;
                    det.Descripcion = detalle.Descripcion;
                    det.ArchivoAdjunto = detalle.ArchivoAdjunto;
                    det.NombreOriginal = detalle.NombreOriginal;
                    det.RutaDestino = ConfigurationManager.AppSettings["CarpetaProCreAdj"].ToString();
                    poCab.ProcesoCreditoAdjunto.Add(det);
                }

                poCab.ProcesoCreditoAccionista = new List<ProcesoCreditoAccionista>();
                foreach (var detalle in item.CRETPROCESOCREDITOACCIONISTA.Where(x => !psLista.Contains(x.CodigoEstado)))
                {
                    var poDet = new ProcesoCreditoAccionista();

                    poDet.CodigoEstado = detalle.CodigoEstado;
                    poDet.Accionista = detalle.Accionista;
                    poDet.Capital = detalle.Capital;
                    poDet.PorcentajeParticipacionAcciones = detalle.ParticipacionAcciones;
                    poCab.ProcesoCreditoAccionista.Add(poDet);
                }

                poCab.ProcesoCreditoAdmActual = new List<ProcesoCreditoAdmActual>();
                foreach (var detalle in item.CRETPROCESOCREDITOADMACTUAL.Where(x => !psLista.Contains(x.CodigoEstado)))
                {
                    var poObjectItem = new ProcesoCreditoAdmActual();

                    poObjectItem.CodigoEstado = detalle.CodigoEstado;
                    poObjectItem.Nombre = detalle.Nombre;
                    poObjectItem.Cargo = detalle.Cargo;
                    poObjectItem.FechaNombramiento = detalle.FechaNombramiento;
                    poObjectItem.Anios = detalle.Anios;
                    poObjectItem.Tipo = detalle.Tipo;
                    poObjectItem.Accionista = detalle.Accionista;
                    poObjectItem.PorcentajeParticipacionAcciones = detalle.PorcentajeParticipacionAcciones;
                    poCab.ProcesoCreditoAdmActual.Add(poObjectItem);
                }

                poCab.ProcesoCreditoBuro = new List<ProcesoCreditoBuro>();
                foreach (var detalle in item.CRETPROCESOCREDITOBURO.Where(x => !psLista.Contains(x.CodigoEstado)))
                {
                    var poObjectItem = new ProcesoCreditoBuro();

                    poObjectItem.CodigoEstado = detalle.CodigoEstado;
                    poObjectItem.FechaConsulta = detalle.FechaConsulta;
                    poObjectItem.HabilitadoCtaCte = detalle.HabilitadoCtaCte;
                    poObjectItem.FechaInhabilitado = detalle.FechaInhabilitado;
                    poObjectItem.TiempoInhabilitado = detalle.TiempoInhabilitado;
                    poObjectItem.Score = detalle.Score;
                    poObjectItem.PorcentajeProbMora = detalle.PorcentajeProbMora;
                    poObjectItem.RiesgoComercial = detalle.RiesgoComercial;
                    poObjectItem.RiesgoFinanciero = detalle.RiesgoFinanciero;
                    poObjectItem.TotalDeudaVencida = detalle.TotalDeudaVencida;
                    poObjectItem.CuotasMensualesCancelTiempo = detalle.CuotasMensualesCancelTiempo;
                    poObjectItem.CuotasMensualesPagadasMora = detalle.CuotasMensualesPagadasMora;
                    poObjectItem.Nombre = detalle.Nombre;
                    poCab.ProcesoCreditoBuro.Add(poObjectItem);
                }

                poCab.ProcesoCreditoFuncionJudicial = new List<ProcesoCreditoFuncionJudicial>();
                foreach (var detalle in item.CRETPROCESOCREDITOFUNCIONJUDICIAL.Where(x => !psLista.Contains(x.CodigoEstado)))
                {
                    var poObjectItem = new ProcesoCreditoFuncionJudicial();

                    poObjectItem.CodigoEstado = detalle.CodigoEstado;
                    poObjectItem.FechaConsulta = detalle.FechaConsulta;
                    poObjectItem.DemandasVigentes = detalle.DemandasVigentes;
                    poObjectItem.FechaCaso = detalle.FechaCaso;
                    poObjectItem.Ofendido = detalle.Ofendido;
                    poObjectItem.DetalleAccion = detalle.DetalleAccion;
                    poObjectItem.MontoDemanda = detalle.MontoDemanda;
                    poCab.ProcesoCreditoFuncionJudicial.Add(poObjectItem);
                }

                poCab.ProcesoCreditoSri = new List<ProcesoCreditoSri>();
                foreach (var detalle in item.CRETPROCESOCREDITOSRI.Where(x => !psLista.Contains(x.CodigoEstado)))
                {
                    var poObjectItem = new ProcesoCreditoSri();

                    poObjectItem.CodigoEstado = detalle.CodigoEstado;
                    poObjectItem.FechaConsulta = detalle.FechaConsulta;
                    poObjectItem.ObligacionesPdtes = detalle.ObligacionesPdtes;
                    poObjectItem.TipoObligacion = detalle.TipoObligacion;
                    poObjectItem.ValorTotalObligacion = detalle.ValorTotalObligacion ?? 0M;
                    poObjectItem.MotivoObligacionesPdtes = detalle.MotivoObligacionesPdtes;
                    poCab.ProcesoCreditoSri.Add(poObjectItem);
                }

                poCab.ProcesoCreditoResolucionAdjunto = new List<ProcesoCreditoResolucionAdjunto>();
                foreach (var detalle in item.CRETPROCESOCREDITORESOLUCIONADJUNTO.Where(x => !psLista.Contains(x.CodigoEstado)))
                {
                    var poObjectItem = new ProcesoCreditoResolucionAdjunto();

                    poObjectItem.IdProcesoCreditoResolucionAdjunto = detalle.IdProcesoCreditoResolucionAdjunto;
                    poObjectItem.IdProcesoCredito = detalle.IdProcesoCredito;
                    poObjectItem.Descripcion = detalle.Descripcion;
                    poObjectItem.ArchivoAdjunto = detalle.ArchivoAdjunto;
                    poObjectItem.NombreOriginal = detalle.NombreOriginal;
                    poObjectItem.RutaDestino = ConfigurationManager.AppSettings["CarpetaProCreRes"].ToString();
                    poCab.ProcesoCreditoResolucionAdjunto.Add(poObjectItem);
                }

                poCab.ProcesoCreditoResolucionAdjunto = new List<ProcesoCreditoResolucionAdjunto>();
                foreach (var detalle in item.CRETPROCESOCREDITORESOLUCIONADJUNTO.Where(x => !psLista.Contains(x.CodigoEstado)))
                {
                    var poObjectItem = new ProcesoCreditoResolucionAdjunto();

                    poObjectItem.IdProcesoCreditoResolucionAdjunto = detalle.IdProcesoCreditoResolucionAdjunto;
                    poObjectItem.IdProcesoCredito = detalle.IdProcesoCredito;
                    poObjectItem.Descripcion = detalle.Descripcion;
                    poObjectItem.ArchivoAdjunto = detalle.ArchivoAdjunto;
                    poObjectItem.NombreOriginal = detalle.NombreOriginal;
                    poObjectItem.RutaDestino = ConfigurationManager.AppSettings["CarpetaProCreRes"].ToString();
                    poCab.ProcesoCreditoResolucionAdjunto.Add(poObjectItem);
                }

                if (item.CRETPROCESOCREDITOCEDULA != null)
                {
                    var poObjectItem = new ProcesoCreditoCedula();
                    poObjectItem.IdentificacionConyuge = item.CRETPROCESOCREDITOCEDULA.IdentificacionConyuge;
                    poObjectItem.IdentificacionRepLegal = item.CRETPROCESOCREDITOCEDULA.IdentificacionRepLegal;
                    poObjectItem.CodigoEstadoCivil = item.CRETPROCESOCREDITOCEDULA.CodigoEstadoCivil;
                    poObjectItem.FechaCaducidadCedulaRepLegal = item.CRETPROCESOCREDITOCEDULA.FechaCaducidadCedulaRepLegal;
                    poObjectItem.FechaIngreso = item.CRETPROCESOCREDITOCEDULA.FechaIngreso;
                    poObjectItem.FechaModificacion = item.CRETPROCESOCREDITOCEDULA.FechaModificacion;
                    poObjectItem.IdProcesoCredito = item.CRETPROCESOCREDITOCEDULA.IdProcesoCredito;
                    poObjectItem.NombreConyuge = item.CRETPROCESOCREDITOCEDULA.NombreConyuge;
                    poObjectItem.NombreRepLegal = item.CRETPROCESOCREDITOCEDULA.NombreRepLegal;
                    poObjectItem.ComentarioRevisionCedula = item.CRETPROCESOCREDITOCEDULA.ComentarioRevisionCedula;
                    poObjectItem.FechaNacimiento = item.CRETPROCESOCREDITOCEDULA.FechaNacimiento;
                    poCab.ProcesoCreditoCedula = poObjectItem;
                }

                if (item.CRETPROCESOCREDITORUC != null)
                {
                    var poObjectItemRuc = new ProcesoCreditoRuc();

                    poObjectItemRuc.ActividadesSecundarias = item.CRETPROCESOCREDITORUC.ActividadesSecundarias;
                    poObjectItemRuc.CodigoTipoNegocio = item.CRETPROCESOCREDITORUC.CodigoTipoNegocio;
                    poObjectItemRuc.Comentarios = item.CRETPROCESOCREDITORUC.Comentarios;
                    poObjectItemRuc.EstadoRuc = item.CRETPROCESOCREDITORUC.EstadoRuc;
                    poObjectItemRuc.FechaIngreso = item.CRETPROCESOCREDITORUC.FechaIngreso;
                    poObjectItemRuc.FechaInicioActividadRuc = item.CRETPROCESOCREDITORUC.FechaInicioActividadRuc;
                    poObjectItemRuc.FechaReinicioActividadRuc = item.CRETPROCESOCREDITORUC.FechaReinicioActividadRuc;
                    poObjectItemRuc.FechaUltActualizacionRuc = item.CRETPROCESOCREDITORUC.FechaUltActualizacionRuc;
                    poObjectItemRuc.Identificacion = item.CRETPROCESOCREDITORUC.Identificacion;
                    poObjectItemRuc.IdProcesoCredito = item.CRETPROCESOCREDITORUC.IdProcesoCredito;
                    poObjectItemRuc.NombreComercial = item.CRETPROCESOCREDITORUC.NombreComercial;
                    poObjectItemRuc.ObligadoLlevarCantidadRuc = item.CRETPROCESOCREDITORUC.ObligadoLlevarCantidadRuc;
                    poObjectItemRuc.OtrasActividades = item.CRETPROCESOCREDITORUC.OtrasActividades;

                    foreach (var detalle in item.CRETPROCESOCREDITORUC.CRETPROCESOCREDITORUCDIRECCION.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {
                        var poDet = new ProcesoCreditoRucDireccion();
                        poDet.IdProcesoCreditoRucDireccion = detalle.IdProcesoCreditoRucDireccion;
                        poDet.IdProcesoCredito = detalle.IdProcesoCredito;
                        poDet.Principal = detalle.Principal;
                        poDet.Direccion = detalle.Direccion;

                        poObjectItemRuc.ProcesoCreditoRucDireccion.Add(poDet);
                    }

                    poCab.ProcesoCreditoRuc = poObjectItemRuc;
                }

               
                foreach (var detalle in item.CRETPROCESOCREDITONOMBRAMIENTO.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new ProcesoCreditoNombramiento();
                    poDet.IdProcesoCredito = detalle.IdProcesoCredito;
                    poDet.IdProcesoCreditoNombramiento = detalle.IdProcesoCreditoNombramiento;
                    poDet.FechaInscripcion = detalle.FechaInscripcion;

                    poDet.Cargo = detalle.Cargo;
                    poDet.Periodo = detalle.Periodo;
                    poDet.CodigoPresentacion = detalle.CodigoPresentacion;

                    poCab.ProcesoCreditoNombramiento.Add(poDet);
                }


                poCab.ProcesoCreditoOtrosActivos = new List<ProcesoCreditoOtrosActivos>();
                foreach (var detalle in item.CRETPROCESOCREDITOOTROSACTIVOS.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new ProcesoCreditoOtrosActivos();

                    poDet.IdProcesoCreditoOtrosActivos = detalle.IdProcesoCreditoOtrosActivos;
                    poDet.IdProcesoCredito = detalle.IdProcesoCredito;
                    poDet.CodigoTipoOtrosActivos = detalle.CodigoTipoOtrosActivos;
                    poDet.Marca = detalle.Marca;
                    poDet.Modelo = detalle.Modelo;
                    poDet.Anio = detalle.Anio;
                    poDet.AvaluoComercial = detalle.AvaluoComercial;
                    poDet.FechaPago = detalle.FechaPago ?? DateTime.Now;
                    poDet.DescripcionDocumento = detalle.DescripcionDocumento;
                    poDet.PropietarioBeneficiario = detalle.PropietarioBeneficiario;

                    poCab.ProcesoCreditoOtrosActivos.Add(poDet);
                }

                poCab.ProcesoCreditoRefBancaria = new List<ProcesoCreditoRefBancaria>();
                foreach (var detalle in item.CRETPROCESOCREDITOREFBANCARIA.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new ProcesoCreditoRefBancaria();
                    poDet.IdProcesoCreditoRefBancaria = detalle.IdProcesoCreditoRefBancaria;
                    poDet.IdProcesoCredito = detalle.IdProcesoCredito;
                    poDet.CodigoBanco = detalle.CodigoBanco;
                    poDet.CodigoTipoCuentaBancaria = detalle.CodigoTipoCuentaBancaria;
                    poDet.NumeroCuenta = detalle.NumeroCuenta;
                    poDet.FechaApertura = detalle.FechaApertura;
                    poDet.ArchivoAdjunto = detalle.ArchivoAdjunto;
                    poDet.NombreOriginal = detalle.NombreOriginal;
                    poDet.RutaDestino = ConfigurationManager.AppSettings["CarpetaSolCreRefBan"].ToString();
                    poDet.FechaEmision = detalle.FechaEmision ?? DateTime.Now;
                    poDet.Titular = detalle.Titular;
                    poDet.SaldosPromedios = detalle.SaldosPromedios ?? 0M;
                    poCab.ProcesoCreditoRefBancaria.Add(poDet);
                }

                poCab.ProcesoCreditoRefComercial = new List<ProcesoCreditoRefComercial>();
                foreach (var detalle in item.CRETPROCESOCREDITOREFCOMERCIAL.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new ProcesoCreditoRefComercial();
                    poDet.IdProcesoCreditoRefComercial = detalle.IdProcesoCreditoRefComercial;
                    poDet.IdProcesoCredito = detalle.IdProcesoCredito;
                    poDet.Nombre = detalle.Nombre;
                    poDet.Telefono = detalle.Telefono;
                    poDet.ArchivoAdjunto = detalle.ArchivoAdjunto;
                    poDet.NombreOriginal = detalle.NombreOriginal;
                    poDet.RutaDestino = ConfigurationManager.AppSettings["CarpetaSolCreRefCom"].ToString();
                    poDet.ClienteDesde = detalle.ClienteDesde ?? DateTime.Now;
                    poDet.Cupo = detalle.Cupo ?? 0M;
                    poDet.FechaConsulta = detalle.FechaConsulta ?? DateTime.Now;
                    poDet.Garantia = detalle.Garantia ?? 0M;
                    poDet.Plazo = detalle.Plazo ?? 0;
                    poDet.PromedioComprasAnual = detalle.PromedioComprasAnual ?? 0M;
                    poDet.PromedioComprasMensual = detalle.PromedioComprasMensual ?? 0M;
                    poDet.PromedioDiasPagos = detalle.PromedioDiasPagos ?? 0;
                    poDet.CodigoGarantia = detalle.CodigoGarantia ?? "0";
                    poDet.FechaReferenciaComercial = detalle.FechaReferenciaComercial;
                    poCab.ProcesoCreditoRefComercial.Add(poDet);
                }

              
                poCab.ProcesoCreditoPropiedades = new List<ProcesoCreditoPropiedades>();
                foreach (var detalle in item.CRETPROCESOCREDITOPROPIEDADES.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new ProcesoCreditoPropiedades();
                    poDet.IdProcesoCreditoPropiedades = detalle.IdProcesoCreditoPropiedades;
                    poDet.IdProcesoCredito = detalle.IdProcesoCredito;
                    poDet.CodigoTipoBien = detalle.CodigoTipoBien;
                    poDet.Direccion = detalle.Direccion;
                    poDet.Hipoteca = detalle.Hipoteca;
                    poDet.AvaluoComercial = detalle.AvaluoComercial;
                    poDet.FechaPago = detalle.FechaPago ?? DateTime.Now;
                    poDet.DescripcionDocumento = detalle.DescripcionDocumento;
                    poDet.PropietarioBeneficiario = detalle.PropietarioBeneficiario;

                    poCab.ProcesoCreditoPropiedades.Add(poDet);
                }


                poListaReturn.Add(poCab);
            }


            return poListaReturn;
        }

        public List<ProcesoCreditoResolucionAdjunto> goBuscarProcesoCreditoResolucionAdjunto(int tId)
        {

            return loBaseDa.Find<CRETPROCESOCREDITORESOLUCIONADJUNTO>().Where(x => x.IdProcesoCredito == tId && x.CodigoEstado != Diccionario.Eliminado)
               .ToList().Select(x => new ProcesoCreditoResolucionAdjunto
               {
                   IdProcesoCreditoResolucionAdjunto = x.IdProcesoCreditoResolucionAdjunto,
                   IdProcesoCredito = x.IdProcesoCredito,
                   Descripcion = x.Descripcion,
                   ArchivoAdjunto = x.ArchivoAdjunto,
                   NombreOriginal = x.NombreOriginal,
                   RutaDestino = ConfigurationManager.AppSettings["CarpetaProCreRes"].ToString()
               }).ToList();
        }

        public List<ProcesoCreditoDetalleRevision> goConsultarProcesoDetalleRevision(int tId)
        {

            loBaseDa.CreateContext();
            var poListaReturn = new List<ProcesoCreditoDetalleRevision>();
            var poObject = loBaseDa.Find<CRETPROCESOCREDITO>().Include(x=>x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == tId).FirstOrDefault();
            if (poObject != null)
            {
                var poComboCheckList = goConsultarComboCheckList();

                var psLista = new List<string>();
                psLista.Add(Diccionario.Eliminado);
                psLista.Add(Diccionario.Inactivo);
                foreach (var detalle in poComboCheckList)
                {
                    
                    var poObj = poObject.CRETPROCESOCREDITODETALLE.Where(x => !psLista.Contains(x.CodigoEstado) && x.IdCheckList.ToString() == detalle.Codigo && x.AgregadoRevision == null).FirstOrDefault();
                    if (poObj == null)
                    {
                        var poDet = new ProcesoCreditoDetalleRevision();

                        poDet.IdCheckList = int.Parse(detalle.Codigo);
                        poDet.IdProcesoCredito = tId;
                        
                        var poObj2 = poObject.CRETPROCESOCREDITODETALLE.Where(x => !psLista.Contains(x.CodigoEstado) && x.IdCheckList.ToString() == detalle.Codigo && x.AgregadoRevision == true).FirstOrDefault();
                        if (poObj2 != null)
                        {
                            poDet.Sel = true;
                            poDet.IdProcesoCreditoDetalle = poObj2.IdProcesoCreditoDetalle;
                        }
                        else
                        {
                            poDet.Sel = false;
                        }

                        poListaReturn.Add(poDet);
                    }
                }
            }
            return poListaReturn;
        }
        
        public List<ProcesoCredito> goListar(int tIdMenu, string tsUsuario)
        {
            loBaseDa.CreateContext();
            var psList = new List<string>();
            psList.Add(Diccionario.Inactivo);
            psList.Add(Diccionario.Eliminado);
            var poListaReturn = new List<ProcesoCredito>();
            var psUsuariosAsignados = loBaseDa.Find<SEGPUSUARIOASIGNADO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoUsuario == tsUsuario && x.IdMenu == tIdMenu).Select(x => x.CodigoUsuarioAsignado).ToList();
            var poLista = loBaseDa.Find<CRETPROCESOCREDITO>().Where(x => !psList.Contains(x.CodigoEstado) && psUsuariosAsignados.Contains(x.UsuarioIngreso)).ToList();
            var poUsuarios = loBaseDa.Find<SEGMUSUARIO>().Select(x=> new { x.CodigoUsuario, x.NombreCompleto }).ToList();

            foreach (var item in poLista)
            {
                var poCab = new ProcesoCredito();
                poCab.Fecha = item.Fecha;
                poCab.IdProcesoCredito = item.IdProcesoCredito;
                poCab.CodigoEstado = item.CodigoEstado;
                poCab.CodigoTipoSolicitud = item.CodigoTipoProceso;
                poCab.TipoSolicitud = item.TipoSolicitud;
                poCab.Observacion = item.Observacion;
                poCab.CodigoCliente = item.CodigoCliente;
                poCab.Cliente = item.Cliente;
                poCab.Completado = item.Completado ?? false;
                poCab.CodigoTipoPersona = item.CodigoTipoPersona;
                poCab.CodigoContado = item.CodigoContado;
                poCab.CodigoEstatusSeguro = item.CodigoEstatusSeguro;
                poCab.EnviarRevision = item.EnviarRevision ?? false;
                poCab.CupoSap = item.CupoSap ?? 0M;
                poCab.CupoSolicitado = item.CupoSolicitado ?? 0M;
                poCab.PlazoSolicitado = item.PlazoSolicitado ?? 0;
                poCab.PlazoSap = item.PlazoSap;
                poCab.RepresentanteLegal = item.RepresentanteLegal;
                poCab.NomUsuario = poUsuarios.Where(x => x.CodigoUsuario == item.UsuarioIngreso).Select(x => x.NombreCompleto).FirstOrDefault();
                poCab.ComentarioRevisor = item.ComentarioRevisor;
                poCab.CupoAprobado = item.CupoAprobado ?? 0;
                poCab.PlazoAprobado = item.PlazoAprobado ?? 0;
                poCab.ResolucionAfecor = item.ResolucionAfecor;
                poCab.ComentarioResolucion = item.ComentarioResolucion;

                poCab.ProcesoCreditoDetalle = new List<ProcesoCreditoDetalle>();
                var psLista = new List<string>();
                psLista.Add(Diccionario.Eliminado);
                psLista.Add(Diccionario.Inactivo);
                foreach (var detalle in item.CRETPROCESOCREDITODETALLE.Where(x => !psLista.Contains(x.CodigoEstado)))
                {
                    var poDet = new ProcesoCreditoDetalle();

                    poDet.CodigoEstado = detalle.CodigoEstado;
                    poDet.CheckList = detalle.CheckList;
                    poDet.Completado = detalle.Completado ?? false ? "SI" : "NO";
                    poDet.IdCheckList = detalle.IdCheckList;
                    poDet.IdProcesoCredito = detalle.IdProcesoCredito;
                    poDet.IdProcesoCreditoDetalle = detalle.IdProcesoCreditoDetalle;
                    poDet.ArchivoAdjunto = detalle.ArchivoAdjunto;
                    poDet.NombreOriginal = detalle.NombreOriginal;
                    poDet.FechaReferencial = detalle.FechaReferencial ?? DateTime.Now;
                    poDet.FechaFisicoRecibido = detalle.FechaFisicoRecibido;
                    poDet.FisicoRecibido = detalle.FisicoRecibido;
                    poDet.RutaDestino = ConfigurationManager.AppSettings["CarpetaProCre"].ToString();
                    poDet.Comentarios = detalle.Comentarios;
                    poCab.ProcesoCreditoDetalle.Add(poDet);
                }




                poListaReturn.Add(poCab);
            }
            return poListaReturn.OrderByDescending(x=>x.IdProcesoCredito).ToList();
        }

        public List<ProcesoCredito> goListarHistorialImportado()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<ProcesoCredito>();
            var poLista = loBaseDa.Find<CRETPROCESOCREDITO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdReferenciaForm != null).ToList();

            foreach (var item in poLista)
            {
                var poCab = new ProcesoCredito();
                poCab.Fecha = item.Fecha;
                poCab.IdProcesoCredito = item.IdProcesoCredito;
                poCab.CodigoEstado = item.CodigoEstado;
                poCab.CodigoTipoSolicitud = item.CodigoTipoProceso;
                poCab.TipoSolicitud = item.TipoSolicitud;
                poCab.Observacion = item.Observacion;
                poCab.CodigoCliente = item.CodigoCliente;
                poCab.Cliente = item.Cliente;
                poCab.Completado = item.Completado ?? false;
                poCab.CodigoTipoPersona = item.CodigoTipoPersona;
                poCab.CodigoContado = item.CodigoContado;
                poCab.CodigoEstatusSeguro = item.CodigoEstatusSeguro;
                poCab.EnviarRevision = item.EnviarRevision ?? false;
                poCab.CupoSap = item.CupoSap ?? 0M;
                poCab.CupoSolicitado = item.CupoSolicitado ?? 0M;
                poCab.PlazoSolicitado = item.PlazoSolicitado ?? 0;
                poCab.PlazoSap = item.PlazoSap;
                poCab.RepresentanteLegal = item.RepresentanteLegal;
                poCab.ComentarioRevisor = item.ComentarioRevisor;
                poCab.CupoAprobado = item.CupoAprobado ?? 0;
                poCab.PlazoAprobado = item.PlazoAprobado ?? 0;
                poCab.ResolucionAfecor = item.ResolucionAfecor;

                poCab.ProcesoCreditoDetalle = new List<ProcesoCreditoDetalle>();
                var psLista = new List<string>();
                psLista.Add(Diccionario.Eliminado);
                psLista.Add(Diccionario.Inactivo);
                foreach (var detalle in item.CRETPROCESOCREDITODETALLE.Where(x => !psLista.Contains(x.CodigoEstado)))
                {
                    var poDet = new ProcesoCreditoDetalle();

                    poDet.CodigoEstado = detalle.CodigoEstado;
                    poDet.CheckList = detalle.CheckList;
                    poDet.Completado = detalle.Completado ?? false ? "SI" : "NO";
                    poDet.IdCheckList = detalle.IdCheckList;
                    poDet.IdProcesoCredito = detalle.IdProcesoCredito;
                    poDet.IdProcesoCreditoDetalle = detalle.IdProcesoCreditoDetalle;
                    poDet.ArchivoAdjunto = detalle.ArchivoAdjunto;
                    poDet.NombreOriginal = detalle.NombreOriginal;
                    poDet.FechaReferencial = detalle.FechaReferencial??DateTime.Now;
                    poDet.RutaDestino = ConfigurationManager.AppSettings["CarpetaProCre"].ToString();
                    poDet.Comentarios = detalle.Comentarios;
                    poDet.FechaFisicoRecibido = detalle.FechaFisicoRecibido;
                    poDet.FisicoRecibido = detalle.FisicoRecibido;
                    poCab.ProcesoCreditoDetalle.Add(poDet);
                }



                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public string gsGuardar(List<ProcesoCredito> toLista, string tsUsuario, string tsTerminal, out int tId)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = lsEsValido(toLista);

            tId = 0;

            if (string.IsNullOrEmpty(psMsg))
            {
                if (toLista != null && toLista.Count > 0)
                {
                    var poListaItem = goSapConsultaItems();
                    var poRelacion = goConsultarComboTipoRelacionPersonal();
                    var poReferenciaPersona = goConsultarComboTipoReferenciaPersonal();

                    var LisId = toLista.Select(x => x.IdProcesoCredito).Distinct();
                    var poLista = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Include(x => x.CRETPROCESOCREDITOACCIONISTA).Include(x => x.CRETPROCESOCREDITOADMACTUAL)
                    .Include(x => x.CRETPROCESOCREDITOBURO).Include(x => x.CRETPROCESOCREDITOFUNCIONJUDICIAL).Where(x => LisId.Contains(x.IdProcesoCredito)).ToList();

                    
                    using (var poTran = new TransactionScope())
                    {
                        foreach (var poItem in toLista)
                        {
                            var dt = goConsultaDataTable("SELECT LicTradNum FROM SBO_AFECOR.dbo.OCRD (NOLOCK) WHERE CardCode = '" + poItem.CodigoCliente + "'");
                            string psNumIden = dt.Rows[0][0].ToString();

                            string psEstadoAnt = "";
                            var poObject = poLista.Where(x => x.IdProcesoCredito == poItem.IdProcesoCredito && x.IdProcesoCredito != 0).FirstOrDefault();
                            if (poObject != null)
                            {
                                poObject.UsuarioModificacion = tsUsuario;
                                poObject.FechaModificacion = DateTime.Now;
                                poObject.TerminalModificacion = tsTerminal;
                                psEstadoAnt = poObject.CodigoEstado;
                                poObject.CodigoEstado = poItem.CodigoEstado;

                            }
                            else
                            {
                                poObject = new CRETPROCESOCREDITO();
                                loBaseDa.CreateNewObject(out poObject);
                                poObject.UsuarioIngreso = tsUsuario;
                                poObject.FechaIngreso = DateTime.Now;
                                poObject.TerminalIngreso = tsTerminal;
                                poObject.CodigoEstado = Diccionario.Pendiente;
                            }

                            poObject.Identificacion = string.IsNullOrEmpty(poObject.Identificacion) ? psNumIden : poObject.Identificacion;
                            poObject.Fecha = poItem.Fecha;
                            poObject.CodigoTipoProceso = poItem.CodigoTipoSolicitud;
                            poObject.TipoSolicitud = poItem.TipoSolicitud;
                            poObject.CodigoTipoPersona = poItem.CodigoTipoPersona;
                            poObject.CodigoEstatusSeguro = poItem.CodigoEstatusSeguro;
                            poObject.CodigoContado = poItem.CodigoContado;
                            poObject.Observacion = poItem.Observacion;
                            poObject.CodigoCliente = poItem.CodigoCliente;
                            poObject.Cliente = poItem.Cliente;
                            poObject.IdReferenciaForm = poItem.IdReferenciaForm;
                            poObject.Completado = poItem.Completado;
                            poObject.EnviarRevision = poItem.EnviarRevision;
                            poObject.CupoSap = poItem.CupoSap;
                            poObject.CupoSolicitado = poItem.CupoSolicitado;
                            poObject.PlazoSolicitado = poItem.PlazoSolicitado;
                            poObject.PlazoSap = poItem.PlazoSap;
                            poObject.RepresentanteLegal = poItem.RepresentanteLegal;
                            poObject.ComentarioRevisor = poItem.ComentarioRevisor;
                            poObject.RTC = poItem.RTC;
                            poObject.IdRTC = poItem.IdRTC;
                            poObject.Zona = poItem.Zona;
                            poObject.ComentarioResolucion = poItem.ComentarioResolucion;
                            poObject.CerrarRequerimientoPorVigencia = poItem.CerrarRequerimientoPorVigencia;

                            gActualizaRequerimiento(poObject.IdProcesoCredito, poItem.RepresentanteLegal, poItem.PlazoSolicitado ?? 0, poItem.CupoSolicitado);
                            
                            if (poItem.ProcesoCreditoDetalle != null)
                            {
                                //Eliminar Detalle 
                                var piListaIdPresentacion = poItem.ProcesoCreditoDetalle.Where(x => x.IdProcesoCreditoDetalle != 0).Select(x => x.IdProcesoCreditoDetalle).ToList();
                                var psLista = new List<string>();
                                psLista.Add(Diccionario.Eliminado);
                                psLista.Add(Diccionario.Inactivo);
                                foreach (var poItemDel in poObject.CRETPROCESOCREDITODETALLE.Where(x => !psLista.Contains(x.CodigoEstado) && !piListaIdPresentacion.Contains(x.IdProcesoCreditoDetalle)))
                                {
                                    poItemDel.CodigoEstado = Diccionario.Inactivo;
                                    poItemDel.UsuarioModificacion = tsUsuario;
                                    poItemDel.FechaModificacion = DateTime.Now;
                                    poItemDel.TerminalModificacion = tsTerminal;
                                }

                                foreach (var item in poItem.ProcesoCreditoDetalle)
                                {
                                    int pId = item.IdProcesoCreditoDetalle;
                                    string psEstadoAntDet = "";
                                    var poObjectItem = poObject.CRETPROCESOCREDITODETALLE.Where(x => x.IdProcesoCreditoDetalle == pId && pId != 0).FirstOrDefault();
                                    if (poObjectItem != null)
                                    {
                                        poObjectItem.UsuarioModificacion = tsUsuario;
                                        poObjectItem.FechaModificacion = DateTime.Now;
                                        poObjectItem.TerminalModificacion = tsTerminal;
                                        psEstadoAntDet = poObjectItem.CodigoEstado;
                                        poObjectItem.CodigoEstado = item.CodigoEstado;
                                    }
                                    else
                                    {
                                        poObjectItem = poObject.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == item.IdCheckList).FirstOrDefault();
                                        if (poObjectItem != null)
                                        {
                                            poObjectItem.UsuarioModificacion = tsUsuario;
                                            poObjectItem.FechaModificacion = DateTime.Now;
                                            poObjectItem.TerminalModificacion = tsTerminal;
                                            psEstadoAntDet = poObjectItem.CodigoEstado;
                                            poObjectItem.CodigoEstado = Diccionario.Pendiente;
                                        }
                                        else
                                        {
                                            poObjectItem = new CRETPROCESOCREDITODETALLE();
                                            poObjectItem.UsuarioIngreso = tsUsuario;
                                            poObjectItem.FechaIngreso = DateTime.Now;
                                            poObjectItem.TerminalIngreso = tsTerminal;
                                            poObject.CRETPROCESOCREDITODETALLE.Add(poObjectItem);
                                            poObjectItem.CodigoEstado = Diccionario.Pendiente;
                                        }
                                    }

                                    poObjectItem.IdCheckList = item.IdCheckList;
                                    poObjectItem.CheckList = item.CheckList;
                                    poObjectItem.Completado = item.Completado == "SI" ? true : false;

                                    if (poObjectItem.ArchivoAdjunto != item.ArchivoAdjunto)
                                    {
                                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                        loBaseDa.CreateNewObject(out poTransaccion);
                                        poTransaccion.CodigoEstado = Diccionario.Activo;
                                        poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.CheckListAdjunto;
                                        poTransaccion.Ruta = item.RutaDestino;
                                        poTransaccion.ComentarioAprobador = string.IsNullOrEmpty(item.NombreOriginal) ? "Adjunto Eliminado": item.NombreOriginal;
                                        poTransaccion.IdTransaccionReferencial = poObjectItem.IdProcesoCreditoDetalle;
                                        poTransaccion.UsuarioAprobacion = tsUsuario;
                                        poTransaccion.UsuarioIngreso = tsUsuario;
                                        poTransaccion.FechaIngreso = DateTime.Now;
                                        poTransaccion.TerminalIngreso = tsTerminal;
                                        poTransaccion.EstadoAnterior = "";
                                        poTransaccion.EstadoPosterior = "";
                                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;
                                    }
                                    poObjectItem.ArchivoAdjunto = item.ArchivoAdjunto;
                                    poObjectItem.NombreOriginal = item.NombreOriginal;
                                    
                                    poObjectItem.FechaReferencial = item.FechaReferencial;

                                    if (poObjectItem.Comentarios != item.Comentarios)
                                    {
                                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                        loBaseDa.CreateNewObject(out poTransaccion);
                                        poTransaccion.CodigoEstado = Diccionario.Activo;
                                        poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.CheckListComentarios;
                                        poTransaccion.ComentarioAprobador = item.Comentarios;
                                        poTransaccion.IdTransaccionReferencial = poObjectItem.IdProcesoCreditoDetalle;
                                        poTransaccion.UsuarioAprobacion = tsUsuario;
                                        poTransaccion.UsuarioIngreso = tsUsuario;
                                        poTransaccion.FechaIngreso = DateTime.Now;
                                        poTransaccion.TerminalIngreso = tsTerminal;
                                        poTransaccion.EstadoAnterior = "";
                                        poTransaccion.EstadoPosterior = "";
                                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;
                                    }
                                    poObjectItem.Comentarios = item.Comentarios;

                                    poObjectItem.MontoReferencial = item.MontoReferencial;

                                    if (poObjectItem.FechaCompromiso != item.FechaCompromiso)
                                    {
                                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                        loBaseDa.CreateNewObject(out poTransaccion);
                                        poTransaccion.CodigoEstado = Diccionario.Activo;
                                        poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.CheckListFechaCompromiso;
                                        if (item.FechaCompromiso != null)
                                        {
                                            poTransaccion.ComentarioAprobador = string.Format("Fecha de compromiso: {0}", item.FechaCompromiso?.ToString("dd/MM/yyyy"));
                                        }
                                        else
                                        {
                                            poTransaccion.ComentarioAprobador = string.Format("Fecha de compromiso eliminada.");
                                        }
                                        poTransaccion.IdTransaccionReferencial = poObjectItem.IdProcesoCreditoDetalle;
                                        poTransaccion.UsuarioAprobacion = tsUsuario;
                                        poTransaccion.UsuarioIngreso = tsUsuario;
                                        poTransaccion.FechaIngreso = DateTime.Now;
                                        poTransaccion.TerminalIngreso = tsTerminal;
                                        poTransaccion.EstadoAnterior = "";
                                        poTransaccion.EstadoPosterior = "";
                                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;
                                    }
                                    poObjectItem.FechaCompromiso = item.FechaCompromiso;

                                    poObjectItem.Periodo = item.Periodo;
                                    if (poObjectItem.FisicoRecibido != item.FisicoRecibido)
                                    {
                                        poObjectItem.FechaFisicoRecibido = DateTime.Now;
                                    }
                                    poObjectItem.FisicoRecibido = item.FisicoRecibido;

                                    loBaseDa.SaveChanges();

                                    if (psEstadoAntDet != poObjectItem.CodigoEstado)
                                    {
                                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                        loBaseDa.CreateNewObject(out poTransaccion);
                                        poTransaccion.CodigoEstado = Diccionario.Activo;
                                        poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                                        poTransaccion.ComentarioAprobador = "";
                                        poTransaccion.IdTransaccionReferencial = poObjectItem.IdProcesoCreditoDetalle;
                                        poTransaccion.UsuarioAprobacion = tsUsuario;
                                        poTransaccion.UsuarioIngreso = tsUsuario;
                                        poTransaccion.FechaIngreso = DateTime.Now;
                                        poTransaccion.TerminalIngreso = tsTerminal;
                                        poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(psEstadoAnt);
                                        poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(poObjectItem.CodigoEstado);
                                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;
                                    }
                                }
                            }

                            List<string> psListaAdjuntoEliminar = new List<string>();

                            List<int> poListaIdPeAJ = poItem.ProcesoCreditoAdjunto.Select(x => x.IdProcesoCreditoAdjunto).ToList();
                            List<int> piArchivoAdjuntoEliminar = poObject.CRETPROCESOCREDITOADJUNTO.Where(x => !poListaIdPeAJ.Contains(x.IdProcesoCreditoAdjunto)).Select(x => x.IdProcesoCreditoAdjunto).ToList();
                            foreach (var del in poObject.CRETPROCESOCREDITOADJUNTO.Where(x => piArchivoAdjuntoEliminar.Contains(x.IdProcesoCreditoAdjunto)))
                            {
                                psListaAdjuntoEliminar.Add(del.ArchivoAdjunto);
                                del.CodigoEstado = Diccionario.Eliminado;
                                del.UsuarioModificacion = tsUsuario;
                                del.FechaModificacion = DateTime.Now;
                                del.TerminalModificacion = tsTerminal;
                            }

                            //Guardar Archivo Adjunto
                            if (poItem.ProcesoCreditoAdjunto != null)
                            {
                                foreach (var det in poItem.ProcesoCreditoAdjunto)
                                {
                                    int pIdDetalle = det.IdProcesoCreditoAdjunto;

                                    var poObjectItem = poObject.CRETPROCESOCREDITOADJUNTO.Where(x => x.IdProcesoCreditoAdjunto == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                                    if (poObjectItem != null)
                                    {
                                        poObjectItem.UsuarioModificacion = tsUsuario;
                                        poObjectItem.FechaModificacion = DateTime.Now;
                                        poObjectItem.TerminalModificacion = tsTerminal;

                                        if (poObjectItem.ArchivoAdjunto != det.ArchivoAdjunto)
                                        {
                                            psListaAdjuntoEliminar.Add(poObjectItem.ArchivoAdjunto);
                                        }

                                    }
                                    else
                                    {
                                        poObjectItem = new CRETPROCESOCREDITOADJUNTO();

                                        poObjectItem.UsuarioIngreso = tsUsuario;
                                        poObjectItem.FechaIngreso = DateTime.Now;
                                        poObjectItem.TerminalIngreso = tsTerminal;

                                        poObject.CRETPROCESOCREDITOADJUNTO.Add(poObjectItem);
                                    }

                                    poObjectItem.CodigoEstado = Diccionario.Activo;
                                    poObjectItem.ArchivoAdjunto = det.ArchivoAdjunto.Trim();
                                    poObjectItem.NombreOriginal = det.NombreOriginal.Trim();
                                    poObjectItem.Descripcion = det.Descripcion;

                                }
                            }

                            loBaseDa.SaveChanges();

                            if (psEstadoAnt != poObject.CodigoEstado)
                            {
                                REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                loBaseDa.CreateNewObject(out poTransaccion);
                                poTransaccion.CodigoEstado = Diccionario.Activo;
                                poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.RequerimientoCredito;
                                poTransaccion.ComentarioAprobador = "";
                                poTransaccion.IdTransaccionReferencial = poObject.IdProcesoCredito;
                                poTransaccion.UsuarioAprobacion = tsUsuario;
                                poTransaccion.UsuarioIngreso = tsUsuario;
                                poTransaccion.FechaIngreso = DateTime.Now;
                                poTransaccion.TerminalIngreso = tsTerminal;
                                poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(psEstadoAnt);
                                poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                                poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;
                            }
                            loBaseDa.SaveChanges();

                            foreach (var fila in poItem.ProcesoCreditoDetalle)
                            {
                                if (!string.IsNullOrEmpty(fila.ArchivoAdjunto) && !string.IsNullOrEmpty(fila.RutaDestino))
                                {
                                    if (fila.RutaOrigen != fila.RutaDestino)
                                    {
                                        if (fila.RutaOrigen != null)
                                        {
                                            if (File.Exists(fila.RutaDestino))
                                            {
                                                File.Delete(fila.RutaDestino);
                                            }
                                            File.Copy(fila.RutaOrigen, fila.RutaDestino);
                                        }

                                    }
                                }
                            }

                            //Guardar Documentos Adjuntos
                            foreach (var det in poItem.ProcesoCreditoAdjunto)
                            {
                                if (!string.IsNullOrEmpty(det.ArchivoAdjunto) && !string.IsNullOrEmpty(det.RutaDestino))
                                {
                                    if (det.RutaOrigen != det.RutaDestino)
                                    {
                                        if (det.RutaOrigen != null)
                                        {
                                            File.Copy(det.RutaOrigen, det.RutaDestino);
                                        }

                                    }
                                }
                            }

                            foreach (var psItem in psListaAdjuntoEliminar)
                            {
                                string eliminar = ConfigurationManager.AppSettings["CarpetaProCreAdj"].ToString() + psItem;
                                File.Delete(eliminar);
                            }

                            tId = poObject.IdProcesoCredito;
                        }
                        poTran.Complete();
                    }

                    gsActualzarRequerimientoCredito(tId, "", tsUsuario, tsTerminal);

                }
                return psMsg;
            }

            return psMsg;
        }

        public string gsGuardarPlanillaServicioBasico(ProcesoCredito toObject, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = lsEsValidoPlanillaServicioBasico(toObject);

            if (string.IsNullOrEmpty(psMsg))
            {

                var poObject = loBaseDa.Get<CRETPROCESOCREDITO>().Where(x => x.IdProcesoCredito == toObject.IdProcesoCredito && x.IdProcesoCredito != 0).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;

                    poObject.CodigoTipoServicioBasico = toObject.CodigoTipoServicioBasico;
                    poObject.ComentarioPlanillaServicioBasico = toObject.ComentarioPlanillaServicioBasico;
                    poObject.DireccionPlanillaServicioBasico = toObject.DireccionPlanillaServicioBasico;
                    poObject.FechaPlanillaServicioBasico = toObject.FechaPlanillaServicioBasico;
                    poObject.TipoServicioBasico = toObject.TipoServicioBasico;

                    foreach (var item in poObject.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == 9))
                    {
                        if (item.CodigoEstado != Diccionario.Cargado)
                        {
                            REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTransaccion);
                            poTransaccion.CodigoEstado = Diccionario.Activo;
                            poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                            poTransaccion.ComentarioAprobador = "";
                            poTransaccion.IdTransaccionReferencial = item.IdProcesoCreditoDetalle;
                            poTransaccion.UsuarioAprobacion = tsUsuario;
                            poTransaccion.UsuarioIngreso = tsUsuario;
                            poTransaccion.FechaIngreso = DateTime.Now;
                            poTransaccion.TerminalIngreso = tsTerminal;
                            poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(item.CodigoEstado);
                            poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Cargado);
                            poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                            item.Completado = true;
                            item.CodigoEstado = Diccionario.Cargado;
                            item.FechaReferencial = poObject.FechaIngreso;
                            item.Comentarios = "";
                        }
                    }
                }

                loBaseDa.SaveChanges();

                return psMsg;
            }

            return psMsg;
        }

        public string gsEnviarDocumentosAReq(string tsUsuario, string tsTerminal, List<ProcesoCreditoDetalleRevision> toListaDocRev, string tsComentario)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();
            
            if (string.IsNullOrEmpty(psMsg))
            {
                //Agregar Documentos 
                if (toListaDocRev.Count > 0)
                {
                    string psEstadoAnt = "";
                    var poCombo = goConsultarComboCheckList();
                    var pIdPro = toListaDocRev.FirstOrDefault().IdProcesoCredito;
                    var poObj = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == pIdPro).FirstOrDefault();

                    poObj.EnviarRevision = false;

                    var piListaIdPresentacion = toListaDocRev.Where(x => x.IdProcesoCreditoDetalle != 0 && x.Sel == false).Select(x => x.IdProcesoCreditoDetalle).ToList();

                    foreach (var poItemDel in poObj.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && piListaIdPresentacion.Contains(x.IdProcesoCreditoDetalle) && x.AgregadoRevision == true))
                    {
                        poItemDel.CodigoEstado = Diccionario.Inactivo;
                        poItemDel.UsuarioModificacion = tsUsuario;
                        poItemDel.FechaModificacion = DateTime.Now;
                        poItemDel.TerminalModificacion = tsTerminal;
                    }

                    foreach (var item in toListaDocRev.Where(x => x.Sel))
                    {
                        int pId = item.IdProcesoCreditoDetalle;
                        string psEstadoAntDet = "";
                        var poObjectItem = poObj.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == item.IdCheckList).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            if (poObjectItem.AgregadoRevision == true)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                                psEstadoAntDet = item.Sel ? poObjectItem.CodigoEstado : Diccionario.Inactivo;
                            }
                        }
                        else
                        {
                            poObjectItem = new CRETPROCESOCREDITODETALLE();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObj.CRETPROCESOCREDITODETALLE.Add(poObjectItem);
                            poObjectItem.CodigoEstado = Diccionario.Pendiente;
                            poObjectItem.AgregadoRevision = true;
                            poObjectItem.Completado = false;
                            poObjectItem.ArchivoAdjunto = "";
                            poObjectItem.NombreOriginal = "";
                            poObjectItem.FechaReferencial = DateTime.MinValue;
                            poObjectItem.Comentarios = "";
                        }

                        if (poObjectItem.AgregadoRevision == true)
                        {
                            poObjectItem.IdCheckList = item.IdCheckList;
                            poObjectItem.CheckList = poCombo.Where(x => x.Codigo == item.IdCheckList.ToString()).Select(x => x.Descripcion).FirstOrDefault();

                            if (psEstadoAntDet != poObjectItem.CodigoEstado)
                            {
                                REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                loBaseDa.CreateNewObject(out poTransaccion);
                                poTransaccion.CodigoEstado = Diccionario.Activo;
                                poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                                poTransaccion.ComentarioAprobador = "";
                                poTransaccion.IdTransaccionReferencial = poObjectItem.IdProcesoCreditoDetalle;
                                poTransaccion.UsuarioAprobacion = tsUsuario;
                                poTransaccion.UsuarioIngreso = tsUsuario;
                                poTransaccion.FechaIngreso = DateTime.Now;
                                poTransaccion.TerminalIngreso = tsTerminal;
                                poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(psEstadoAnt);
                                poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(poObjectItem.CodigoEstado);
                                poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;
                            }
                        }
                    }

                    if (poObj.CodigoEstado != Diccionario.Pendiente)
                    {
                        REHTTRANSACCIONAUTOIZACION poTransa = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTransa);
                        poTransa.CodigoEstado = Diccionario.Activo;
                        poTransa.CodigoTransaccion = Diccionario.Tablas.Transaccion.RequerimientoCredito;
                        poTransa.ComentarioAprobador = tsComentario;
                        poTransa.IdTransaccionReferencial = poObj.IdProcesoCredito;
                        poTransa.UsuarioAprobacion = tsUsuario;
                        poTransa.UsuarioIngreso = tsUsuario;
                        poTransa.FechaIngreso = DateTime.Now;
                        poTransa.TerminalIngreso = tsTerminal;
                        poTransa.EstadoAnterior = Diccionario.gsGetDescripcion(poObj.CodigoEstado);
                        poTransa.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Pendiente);
                        poTransa.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                        poObj.CodigoEstado = Diccionario.Pendiente;
                        poObj.ComentarioResolucion = tsComentario;
                    }
                }
                loBaseDa.SaveChanges();
                
                return psMsg;
            }

            return psMsg;
        }


        public string gsGuardarResolucionCredito(ProcesoCredito toObject, string tsUsuario, string tsTerminal, List<ProcesoCreditoDetalleRevision> toListaDocRev)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = lsEsValidoResolucion(toObject);

            List<string> psListaAdjuntoEliminar = new List<string>();

            if (string.IsNullOrEmpty(psMsg))
            {
                var poObject = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x=>x.CRETPROCESOCREDITORESOLUCIONADJUNTO).Where(x => x.IdProcesoCredito == toObject.IdProcesoCredito && x.IdProcesoCredito != 0).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;

                    poObject.CodigoTipoServicioBasico = toObject.CodigoTipoServicioBasico;
                    poObject.PlazoAprobado = toObject.PlazoAprobado;
                    poObject.CupoAprobado = toObject.CupoAprobado;
                    poObject.ResolucionAfecor = toObject.ResolucionAfecor;
                    poObject.FechaResolucion = DateTime.Now;
                    poObject.CodigoEstatusSeguro = toObject.CodigoEstatusSeguro;
                   

                    poObject.NombreOriginal = toObject.NombreOriginal;
                    poObject.ArchivoAdjunto = toObject.ArchivoAdjunto;

                    //Agregar Documentos 
                    if (toListaDocRev.Count > 0)
                    {
                        string psEstadoAnt = "";
                        var poCombo = goConsultarComboCheckList();
                        var pIdPro = toListaDocRev.FirstOrDefault().IdProcesoCredito;
                        var poObj = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == pIdPro).FirstOrDefault();

                        var piListaIdPresentacion = toListaDocRev.Where(x => x.IdProcesoCreditoDetalle != 0 && x.Sel == false).Select(x => x.IdProcesoCreditoDetalle).ToList();

                        foreach (var poItemDel in poObj.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && piListaIdPresentacion.Contains(x.IdProcesoCreditoDetalle) && x.AgregadoRevision == true))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in toListaDocRev.Where(x => x.Sel))
                        {
                            int pId = item.IdProcesoCreditoDetalle;
                            string psEstadoAntDet = "";
                            var poObjectItem = poObj.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == item.IdCheckList).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                if (poObjectItem.AgregadoRevision == true)
                                {
                                    poObjectItem.UsuarioModificacion = tsUsuario;
                                    poObjectItem.FechaModificacion = DateTime.Now;
                                    poObjectItem.TerminalModificacion = tsTerminal;
                                    psEstadoAntDet = item.Sel ? poObjectItem.CodigoEstado : Diccionario.Inactivo;
                                }
                            }
                            else
                            {
                                poObjectItem = new CRETPROCESOCREDITODETALLE();
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                                poObj.CRETPROCESOCREDITODETALLE.Add(poObjectItem);
                                poObjectItem.CodigoEstado = Diccionario.Pendiente;
                                poObjectItem.AgregadoRevision = true;
                                poObjectItem.Completado = false;
                                poObjectItem.ArchivoAdjunto = "";
                                poObjectItem.NombreOriginal = "";
                                poObjectItem.FechaReferencial = DateTime.MinValue;
                                poObjectItem.Comentarios = "";
                            }

                            if (poObjectItem.AgregadoRevision == true)
                            {
                                poObjectItem.IdCheckList = item.IdCheckList;
                                poObjectItem.CheckList = poCombo.Where(x => x.Codigo == item.IdCheckList.ToString()).Select(x => x.Descripcion).FirstOrDefault();

                                if (psEstadoAntDet != poObjectItem.CodigoEstado)
                                {
                                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                    loBaseDa.CreateNewObject(out poTransaccion);
                                    poTransaccion.CodigoEstado = Diccionario.Activo;
                                    poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                                    poTransaccion.ComentarioAprobador = "";
                                    poTransaccion.IdTransaccionReferencial = poObjectItem.IdProcesoCreditoDetalle;
                                    poTransaccion.UsuarioAprobacion = tsUsuario;
                                    poTransaccion.UsuarioIngreso = tsUsuario;
                                    poTransaccion.FechaIngreso = DateTime.Now;
                                    poTransaccion.TerminalIngreso = tsTerminal;
                                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(psEstadoAnt);
                                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(poObjectItem.CodigoEstado);
                                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;
                                }
                            }
                        }

                        if (poObj.CodigoEstado != Diccionario.Pendiente)
                        {
                            REHTTRANSACCIONAUTOIZACION poTransa = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTransa);
                            poTransa.CodigoEstado = Diccionario.Activo;
                            poTransa.CodigoTransaccion = Diccionario.Tablas.Transaccion.RequerimientoCredito;
                            poTransa.ComentarioAprobador = "";
                            poTransa.IdTransaccionReferencial = poObj.IdProcesoCredito;
                            poTransa.UsuarioAprobacion = tsUsuario;
                            poTransa.UsuarioIngreso = tsUsuario;
                            poTransa.FechaIngreso = DateTime.Now;
                            poTransa.TerminalIngreso = tsTerminal;
                            poTransa.EstadoAnterior = Diccionario.gsGetDescripcion(poObj.CodigoEstado);
                            poTransa.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Pendiente);
                            poTransa.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                            poObj.CodigoEstado = Diccionario.Pendiente;
                        }
                    }
                    else
                    {
                        if (poObject.CodigoEstado != toObject.CodigoEstado)
                        {
                            REHTTRANSACCIONAUTOIZACION poTransa = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTransa);
                            poTransa.CodigoEstado = Diccionario.Activo;
                            poTransa.CodigoTransaccion = Diccionario.Tablas.Transaccion.RequerimientoCredito;
                            poTransa.ComentarioAprobador = "";
                            poTransa.IdTransaccionReferencial = toObject.IdProcesoCredito;
                            poTransa.UsuarioAprobacion = tsUsuario;
                            poTransa.UsuarioIngreso = tsUsuario;
                            poTransa.FechaIngreso = DateTime.Now;
                            poTransa.TerminalIngreso = tsTerminal;
                            poTransa.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                            poTransa.EstadoPosterior = Diccionario.gsGetDescripcion(toObject.CodigoEstado);
                            poTransa.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                            poObject.CodigoEstado = toObject.CodigoEstado;
                        }
                    }

                }

                List<int> poListaIdPeAJ = toObject.ProcesoCreditoResolucionAdjunto.Select(x => x.IdProcesoCreditoResolucionAdjunto).ToList();
                List<int> piArchivoAdjuntoEliminar = poObject.CRETPROCESOCREDITORESOLUCIONADJUNTO.Where(x => !poListaIdPeAJ.Contains(x.IdProcesoCreditoResolucionAdjunto)).Select(x => x.IdProcesoCreditoResolucionAdjunto).ToList();
                foreach (var poItem in poObject.CRETPROCESOCREDITORESOLUCIONADJUNTO.Where(x => piArchivoAdjuntoEliminar.Contains(x.IdProcesoCreditoResolucionAdjunto)))
                {
                    psListaAdjuntoEliminar.Add(poItem.ArchivoAdjunto);
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                //Guardar Archivo Adjunto
                if (toObject.ProcesoCreditoResolucionAdjunto != null)
                {
                    foreach (var poItem in toObject.ProcesoCreditoResolucionAdjunto)
                    {
                        int pIdDetalle = poItem.IdProcesoCreditoResolucionAdjunto;

                        var poObjectItem = poObject.CRETPROCESOCREDITORESOLUCIONADJUNTO.Where(x => x.IdProcesoCreditoResolucionAdjunto == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            if (poObjectItem.ArchivoAdjunto != poItem.ArchivoAdjunto)
                            {
                                psListaAdjuntoEliminar.Add(poObjectItem.ArchivoAdjunto);
                            }

                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                            
                            // Insert Auditoría
                            loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                        }
                        else
                        {
                            poObjectItem = new CRETPROCESOCREDITORESOLUCIONADJUNTO();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            
                            // Insert Auditoría
                            loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                            poObject.CRETPROCESOCREDITORESOLUCIONADJUNTO.Add(poObjectItem);
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        if (string.IsNullOrEmpty(poItem.Descripcion))
                        {
                            poObjectItem.Descripcion = "";
                        }

                        poObjectItem.ArchivoAdjunto = poItem.ArchivoAdjunto.Trim();
                        poObjectItem.NombreOriginal = poItem.NombreOriginal.Trim();
                        poObjectItem.Descripcion = poItem.Descripcion;
                    }
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();

                    if (!string.IsNullOrEmpty(toObject.ArchivoAdjunto) && !string.IsNullOrEmpty(toObject.RutaDestino))
                    {
                        if (toObject.RutaOrigen != toObject.RutaDestino)
                        {
                            if (toObject.RutaOrigen != null)
                            {
                                if (File.Exists(toObject.RutaDestino))
                                {
                                    File.Delete(toObject.RutaDestino);
                                }
                                File.Copy(toObject.RutaOrigen, toObject.RutaDestino);
                            }

                        }
                    }

                    //Guardar Documentos Adjuntos
                    foreach (var poItem in toObject.ProcesoCreditoResolucionAdjunto)
                    {
                        if (!string.IsNullOrEmpty(poItem.ArchivoAdjunto) && !string.IsNullOrEmpty(poItem.RutaDestino))
                        {
                            if (poItem.RutaOrigen != poItem.RutaDestino)
                            {
                                if (poItem.RutaOrigen != null)
                                {
                                    if (File.Exists(poItem.RutaDestino))
                                    {
                                        File.Delete(poItem.RutaDestino);
                                    }
                                    File.Copy(poItem.RutaOrigen, poItem.RutaDestino);
                                }

                            }
                        }
                    }

                    foreach (var psItem in psListaAdjuntoEliminar)
                    {
                        string eliminar = ConfigurationManager.AppSettings["CarpetaProCreRes"].ToString() + psItem;
                        File.Delete(eliminar);
                    }

                    poTran.Complete();
                }

                return psMsg;
            }

            return psMsg;
        }

        public string gEliminar(int tId, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";

            var poObject = loBaseDa.Get<CRETPROCESOCREDITO>().Where(x => x.IdProcesoCredito == tId).FirstOrDefault();

            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.FechaModificacion = DateTime.Now;
                poObject.TerminalModificacion = tsTerminal;

                //foreach (var item in poObject.CRETEPROCESOCREDITOOTROSACTIVOS.Where(x => x.CodigoEstado == Diccionario.Activo))
                //{
                //    item.CodigoEstado = Diccionario.Eliminado;
                //    item.UsuarioModificacion = tsUsuario;
                //    item.FechaModificacion = DateTime.Now;
                //    item.TerminalModificacion = tsTerminal;
                //}

                if (string.IsNullOrEmpty(psMsg))
                {
                    loBaseDa.SaveChanges();
                }

            }

            return psMsg;
        }

        public string gsCerrar(int tId, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";

            var poObject = loBaseDa.Get<CRETPROCESOCREDITO>().Where(x => x.IdProcesoCredito == tId).FirstOrDefault();

            if (poObject != null)
            {
                List<string> psList = new List<string>();
                psList.Add(Diccionario.Pendiente);
                psList.Add(Diccionario.Corregir);
                psList.Add(Diccionario.EnEspera);
                psList.Add(Diccionario.Cargado);

                if (psList.Contains(poObject.CodigoEstado))
                {
                    REHTTRANSACCIONAUTOIZACION poTransa = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransa);
                    poTransa.CodigoEstado = Diccionario.Activo;
                    poTransa.CodigoTransaccion = Diccionario.Tablas.Transaccion.RequerimientoCredito;
                    poTransa.ComentarioAprobador = "";
                    poTransa.IdTransaccionReferencial = poObject.IdProcesoCredito;
                    poTransa.UsuarioAprobacion = tsUsuario;
                    poTransa.UsuarioIngreso = tsUsuario;
                    poTransa.FechaIngreso = DateTime.Now;
                    poTransa.TerminalIngreso = tsTerminal;
                    poTransa.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    poTransa.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Cerrado);
                    poTransa.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    poObject.CodigoEstado = Diccionario.Cerrado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;

                    loBaseDa.SaveChanges();
                }
                else
                {
                    psMsg = "No es posible cerrar requerimiento con el estado: " + Diccionario.gsGetDescripcion(poObject.CodigoContado);
                }
                
            }

            return psMsg;
        }


        private string lsEsValido(List<ProcesoCredito> toObject)
        {
            string psMsg = string.Empty;

            foreach (var item in toObject)
            {

                if (item.CodigoTipoSolicitud != "ACT")
                {
                    List<string> psEstados = new List<string>();
                    psEstados.Add(Diccionario.Eliminado);
                    psEstados.Add(Diccionario.Inactivo);
                    psEstados.Add(Diccionario.Cerrado);
                    psEstados.Add(Diccionario.Finalizado);

                    var Registro = loBaseDa.Find<CRETPROCESOCREDITO>().Where(x => x.CodigoCliente == item.CodigoCliente && x.CodigoTipoProceso == item.CodigoTipoSolicitud && !psEstados.Contains(x.CodigoEstado) && x.IdProcesoCredito != item.IdProcesoCredito).Select(x => new { x.IdProcesoCredito, x.Observacion, x.FechaIngreso }).FirstOrDefault();
                    if (Registro != null)
                    {
                        psMsg = psMsg + "No es posible Guardar, Ya existe un requerimiento en curso. \n";
                    }

                }


                if (item.CodigoTipoSolicitud == Diccionario.Seleccione)
                {
                    psMsg = psMsg + "Falta seleccionar Tipo de Proceso. \n";
                }

                if (item.CodigoCliente == Diccionario.Seleccione)
                {
                    psMsg = psMsg + "Falta seleccionar Cliente. \n";
                }
                
                if (item.CodigoTipoPersona == Diccionario.Seleccione)
                {
                    psMsg = psMsg + "Falta seleccionar Tipo de Ruc. \n";
                }

                if (item.CodigoContado == Diccionario.Seleccione)
                {
                    psMsg = psMsg + "Falta seleccionar Código Contado. \n";
                }

                if (item.CodigoTipoSolicitud == Diccionario.ListaCatalogo.TipoSolicitudCreditoClass.AumentoCupo)
                {
                    if (item.CupoSolicitado == 0)
                    {
                        psMsg = psMsg + "Ingrese el Cupo Solicitado. \n";
                    }
                }
                else if (item.CodigoTipoSolicitud == Diccionario.ListaCatalogo.TipoSolicitudCreditoClass.AumentoPlazo)
                {
                    if (item.PlazoSolicitado == 0)
                    {
                        psMsg = psMsg + "Ingrese el Plazo Solicitado. \n";
                    }
                }
                else if (item.CodigoTipoSolicitud == Diccionario.ListaCatalogo.TipoSolicitudCreditoClass.Actualizacion)
                {
                    item.CupoSolicitado = 0;
                    item.PlazoSolicitado = 0;
                }
                else
                {
                    if (item.CupoSolicitado == 0)
                    {
                        psMsg = psMsg + "Ingrese el Cupo Solicitado. \n";
                    }

                    if (item.PlazoSolicitado == 0)
                    {
                        psMsg = psMsg + "Ingrese el Plazo Solicitado. \n";
                    }
                }
                
                if (item.IdRTC == 0)
                {
                    psMsg = psMsg + "Falta seleccionar RTC. \n";
                }

                //var poListaMuestra = new List<int>();
                //int num = 0;
                //foreach (var fila in item.ProcesoCreditoOtrosActivos)
                //{
                //    num++;
                //    if (item.ProcesoCreditoDetalle.Where(x => x.Muestra == num).Count() == 0)
                //    {
                //        psMsg = psMsg + "# Muestra '" + num + "' no existe, corregir debe existir una secuencia \n";
                //    }
                //}
            }

            return psMsg;
        }

        private string lsEsValidoRevision(ProcesoCredito item)
        {
            string psMsg = string.Empty;

            //if (item.CodigoTipoSolicitud == Diccionario.Seleccione)
            //{
            //    psMsg = psMsg + "Falta seleccionar Tipo de Proceso. \n";
            //}

            //if (item.CodigoCliente == Diccionario.Seleccione)
            //{
            //    psMsg = psMsg + "Falta seleccionar Cliente. \n";
            //}

            //if (item.CodigoTipoPersona == Diccionario.Seleccione)
            //{
            //    psMsg = psMsg + "Falta seleccionar Tipo de Ruc. \n";
            //}

            //if (item.CodigoContado == Diccionario.Seleccione)
            //{
            //    psMsg = psMsg + "Falta seleccionar Código Contado. \n";
            //}

            //if (item.CupoSolicitado == 0)
            //{
            //    psMsg = psMsg + "Ingrese el Cupo Solicitado. \n";
            //}
            return psMsg;
        }

        private string lsEsValidoPlanillaServicioBasico(ProcesoCredito item)
        {
            string psMsg = string.Empty;

            if (item.CodigoTipoServicioBasico == Diccionario.Seleccione)
            {
                psMsg = psMsg + "Falta seleccionar Tipo de Servicio. \n";
            }

            if (item.FechaPlanillaServicioBasico == DateTime.MinValue)
            {
                psMsg = psMsg + "Ingrese la Fecha de Emisión. \n";
            }

            if (string.IsNullOrEmpty(item.DireccionPlanillaServicioBasico))
            {
                psMsg = psMsg + "Ingrese la dirección. \n";
            }

            return psMsg;
        }

        private string lsEsValidoResolucion(ProcesoCredito item)
        {
            string psMsg = string.Empty;

            if (item.CodigoEstatusSeguro == Diccionario.Seleccione)
            {
                psMsg = psMsg + "Falta seleccionar Estatus de Seguro. \n";
            }

            //if (item.CupoAprobado == 0)
            //{
            //    psMsg = psMsg + "Ingrese el Cupo Aprobado Afecor. \n";
            //}
            
            if (string.IsNullOrEmpty(item.ResolucionAfecor))
            {
                psMsg = psMsg + "Ingrese la Resolución Afecor. \n";
            }

            return psMsg;
        }

        public List<ProcesoCreditoDetalle> goCheckListObligatorio()
        {
            loBaseDa.CreateContext();
            List<int> piList = new List<int>();
            piList.Add(2);
            piList.Add(3);
            piList.Add(5);
            piList.Add(6);
            piList.Add(12);
            return (from a in loBaseDa.Find<CREMCHECKLIST>()
                    where a.CodigoEstado == Diccionario.Activo && piList.Contains(a.IdCheckList)
                    select new ProcesoCreditoDetalle()
                    {
                        IdCheckList = a.IdCheckList,
                        CheckList = a.Descripcion,
                        Completado = "NO"
                    }).ToList();

        }

        public List<ProcesoCreditoDetalle> goCheckList(string tsTipoSolicitud, string tsTipoPersona, string tsCodContado, string tsStatusSeguro, bool tbObligatorio)
        {
            loBaseDa.CreateContext();
            return (from a in loBaseDa.Find<CREPCHECKLIST>()
                    join b in loBaseDa.Find<CREMCHECKLIST>() on a.IdCheckList equals b.IdCheckList
                    where a.CodigoEstado == Diccionario.Activo && a.CodigoTipoProceso == tsTipoSolicitud && a.CodigoTipoPersona == tsTipoPersona
                    && a.CodigoContado == tsCodContado && a.CodigoEstatusSeguro == tsStatusSeguro && a.Necesario == "SI"
                    && b.Obligatorio == tbObligatorio
                    select new ProcesoCreditoDetalle()
                    {
                       IdCheckList = a.IdCheckList,
                       CheckList = a.CheckList,
                       Completado = "NO"
                    }).ToList();

        }

        public List<ProcesoCreditoDetalle> goCheckList(string tsTipoSolicitud, string tsTipoPersona, string tsCodContado, string tsStatusSeguro)
        {
            loBaseDa.CreateContext();
            return (from a in loBaseDa.Find<CREPCHECKLIST>()
                    where a.CodigoEstado == Diccionario.Activo && a.CodigoTipoProceso == tsTipoSolicitud && a.CodigoTipoPersona == tsTipoPersona
                    && a.CodigoContado == tsCodContado && a.CodigoEstatusSeguro == tsStatusSeguro && a.Necesario == "SI"
                    select new ProcesoCreditoDetalle()
                    {
                        IdCheckList = a.IdCheckList,
                        CheckList = a.CheckList,
                        Completado = "NO"
                    }).ToList();

        }

        public bool gbCheckListObligatorio(string tsTipoSolicitud, string tsTipoPersona, string tsCodContado, string tsStatusSeguro, int tIdCheckList, int tIdProceso)
        {
            loBaseDa.CreateContext();
            var poRegistro = (from a in loBaseDa.Find<CREPCHECKLIST>()
                              where a.CodigoEstado == Diccionario.Activo && a.CodigoTipoProceso == tsTipoSolicitud && a.CodigoTipoPersona == tsTipoPersona
                              && a.CodigoContado == tsCodContado && a.CodigoEstatusSeguro == tsStatusSeguro && a.IdCheckList == tIdCheckList select a).FirstOrDefault();
            if (poRegistro != null)
            {
                if (poRegistro.Necesario == "SI")
                {
                    return true;
                }
                else
                {
                    var psList = new List<string>();
                    psList.Add(Diccionario.Eliminado);
                    psList.Add(Diccionario.Inactivo);
                    var poObj = loBaseDa.Find<CRETPROCESOCREDITODETALLE>().Where(x => !psList.Contains(x.CodigoEstado) && x.IdCheckList == tIdCheckList && x.IdProcesoCredito == tIdProceso).FirstOrDefault();
                    if (poObj != null)
                    {
                        if (poObj.AgregadoRevision == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

        }
                    
        public string gsActualizaEstado(List<ActualizaEstatus> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";
            loBaseDa.CreateContext();

            if (string.IsNullOrEmpty(psMsg))
            {
                foreach (var item in toLista)
                {
                    var poObject = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x=>x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == item.IdProcesoCredito).FirstOrDefault();
                    if (poObject != null)
                    {
                        if (poObject.CodigoEstatusSeguro != item.EstatusSeguro)
                        {
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.CodigoEstatusSeguro = item.EstatusSeguro;

                            var poListaCheckList = goCheckList(poObject.CodigoTipoProceso, poObject.CodigoTipoPersona, poObject.CodigoContado, item.EstatusSeguro);
                            foreach (var ite in poListaCheckList)
                            {
                                var psLista = new List<string>();
                                psLista.Add(Diccionario.Eliminado);
                                psLista.Add(Diccionario.Inactivo);
                                if (poObject.CRETPROCESOCREDITODETALLE.Where(x => !psLista.Contains(x.CodigoEstado) && x.IdCheckList == ite.IdCheckList).Count() == 0)
                                {
                                    var poObjectItem = new CRETPROCESOCREDITODETALLE();
                                    poObjectItem.UsuarioIngreso = tsUsuario;
                                    poObjectItem.FechaIngreso = DateTime.Now;
                                    poObjectItem.TerminalIngreso = tsTerminal;
                                    poObjectItem.CodigoEstado = Diccionario.Activo;
                                    poObjectItem.IdCheckList = ite.IdCheckList;
                                    poObjectItem.CheckList = ite.CheckList;
                                    poObjectItem.Completado = false;
                                    poObjectItem.ArchivoAdjunto = "";
                                    poObjectItem.NombreOriginal = "";
                                    poObjectItem.FechaReferencial = DateTime.MinValue;

                                    poObject.CRETPROCESOCREDITODETALLE.Add(poObjectItem);
                                }
                            }

                            loBaseDa.SaveChanges();
                        }
                    }

                    
                }
            }

            return psMsg;
        }

        public List<ActualizaEstatus> goListarCambioEstado(string tsEstatus)
        {
            loBaseDa.CreateContext();
            return loBaseDa.ExecStoreProcedure<ActualizaEstatus>(string.Format("CRESPCONSULTAREQUERIMIENTO '{0}'", tsEstatus));
        }

        /// <summary>
        /// Buscar Codigo de la Entidad
        /// </summary>
        /// <param name="tsTipo">P: Primero, A: Anterior, S: Siguiente, U: Último</param>
        /// <param name="tsCodigo">Codigo de la entidad</param>
        /// <returns></returns>
        public string goBuscarCodigo(string tsTipo, string tsCodigo, int tIdMenu, string tsUsuario)
        {
            var psUsuariosAsignados = loBaseDa.Find<SEGPUSUARIOASIGNADO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoUsuario == tsUsuario && x.IdMenu == tIdMenu).Select(x => x.CodigoUsuarioAsignado).ToList();

            string psCodigo = string.Empty;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<CRETPROCESOCREDITO>().Where(x => x.CodigoEstado != Diccionario.Eliminado && psUsuariosAsignados.Contains(x.UsuarioIngreso)).Select(x => new { x.IdProcesoCredito }).OrderBy(x => x.IdProcesoCredito).FirstOrDefault()?.IdProcesoCredito.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<CRETPROCESOCREDITO>().Where(x => x.CodigoEstado != Diccionario.Eliminado && psUsuariosAsignados.Contains(x.UsuarioIngreso)).Select(x => new { x.IdProcesoCredito }).OrderByDescending(x => x.IdProcesoCredito).FirstOrDefault()?.IdProcesoCredito.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<CRETPROCESOCREDITO>().Where(x => x.CodigoEstado != Diccionario.Eliminado && psUsuariosAsignados.Contains(x.UsuarioIngreso)).Select(x => new { x.IdProcesoCredito }).ToList().Where(x => x.IdProcesoCredito < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdProcesoCredito).FirstOrDefault().IdProcesoCredito.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<CRETPROCESOCREDITO>().Where(x => x.CodigoEstado != Diccionario.Eliminado && psUsuariosAsignados.Contains(x.UsuarioIngreso)).Select(x => new { x.IdProcesoCredito }).ToList().Where(x => x.IdProcesoCredito > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdProcesoCredito).FirstOrDefault().IdProcesoCredito.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            return psCodigo;

        }

        public string gsActualzarRequerimeintoCorregir(int tiId, string tsComentario, string tsUsuario, string tsTerminal, bool tbRevision)
        {
            var psReturn = "";
            loBaseDa.CreateContext();
            var poReg = loBaseDa.Get<CRETPROCESOCREDITO>().Where(x => x.IdProcesoCredito == tiId).FirstOrDefault();
            if (poReg != null)
            {
                if (poReg.CodigoEstado != Diccionario.Finalizado)
                {
                    if (poReg.CodigoEstado == Diccionario.Cargado || poReg.CodigoEstado == Diccionario.EnRevision || poReg.CodigoEstado == Diccionario.Pendiente || poReg.CodigoEstado == Diccionario.EnEspera)
                    {
                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTransaccion);
                        poTransaccion.CodigoEstado = Diccionario.Activo;
                        poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.RequerimientoCredito;
                        poTransaccion.ComentarioAprobador = tsComentario;
                        poTransaccion.IdTransaccionReferencial = tiId;
                        poTransaccion.UsuarioAprobacion = tsUsuario;
                        poTransaccion.UsuarioIngreso = tsUsuario;
                        poTransaccion.FechaIngreso = DateTime.Now;
                        poTransaccion.TerminalIngreso = tsTerminal;
                        poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poReg.CodigoEstado);
                        poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Corregir);
                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                        poReg.CodigoEstado = Diccionario.Corregir;
                        poReg.FechaModificacion = DateTime.Now;
                        poReg.UsuarioModificacion = tsUsuario;
                        poReg.EnviarRevision = false;
                        //poReg.
                        //poReg.Comentarios = tsComentario;

                        if (!tbRevision)
                        {
                            poReg.ComentarioResolucion = tsComentario;
                        }
                        else
                        {
                            poReg.ComentarioRevisor = tsComentario;
                        }

                        loBaseDa.SaveChanges();
                    }
                    else
                    {
                        psReturn = "Solo se pueden enviar a corregir requerimientos cuyos estados sean: CARGADO, EN REVISIÓN O PENDIENTE";
                    }
                }
                else
                {
                    psReturn = "No es posible enviar a corregir con un estado FINALIZADO";
                }

            }

            return psReturn;

        }


        public string gsActualzarChecklistCorregir(int tiId, string tsComentario, string tsUsuario, string tsTerminal)
        {
            var psReturn = "";
            loBaseDa.CreateContext();
            var poReg = loBaseDa.Get<CRETPROCESOCREDITODETALLE>().Include(x=>x.CRETPROCESOCREDITO).Where(x => x.IdProcesoCreditoDetalle == tiId).FirstOrDefault();
            if (poReg != null)
            {
                if (poReg.CodigoEstado != Diccionario.Aprobado)
                {
                    if (poReg.CodigoEstado == Diccionario.Cargado || poReg.CodigoEstado == Diccionario.EnRevision || poReg.CodigoEstado == Diccionario.Excepcion || poReg.CodigoEstado == Diccionario.Pendiente || poReg.CodigoEstado == Diccionario.EnEspera)
                    {
                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTransaccion);
                        poTransaccion.CodigoEstado = Diccionario.Activo;
                        poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                        poTransaccion.ComentarioAprobador = tsComentario;
                        poTransaccion.IdTransaccionReferencial = tiId;
                        poTransaccion.UsuarioAprobacion = tsUsuario;
                        poTransaccion.UsuarioIngreso = tsUsuario;
                        poTransaccion.FechaIngreso = DateTime.Now;
                        poTransaccion.TerminalIngreso = tsTerminal;
                        poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poReg.CodigoEstado);
                        poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Corregir);
                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                        poReg.CodigoEstado = Diccionario.Corregir;
                        poReg.FechaModificacion = DateTime.Now;
                        poReg.UsuarioModificacion = tsUsuario;
                        //poReg.Comentarios = tsComentario;

                        poReg.CRETPROCESOCREDITO.EnviarRevision = false;

                        bool pbEnviarCorreoCorregir = loBaseDa.Find<CREPPARAMETRO>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => x.EnviarCorreoCorregirRTC).FirstOrDefault() ?? false;

                        using (var poTran = new TransactionScope())
                        {

                            loBaseDa.SaveChanges();
                            gsActualzarRequerimientoCredito(poReg.IdProcesoCredito, "", tsUsuario, tsTerminal);

                            //Enviar correo si es Informe de RTC
                            if (poReg.IdCheckList == 12 && pbEnviarCorreoCorregir)
                            {
                                var Parametro = loBaseDa.Find<CREPPARAMETRO>().Where(x => x.CodigoEstado == Diccionario.Activo).FirstOrDefault();
                                string Asunto = Parametro?.AsuntoCorregirInforme;
                                string Texto = Parametro?.TextoCorregirInforme;
                                Texto = string.Format("{0} {1}", Texto, tsComentario);
                                int idRTC = poReg.CRETPROCESOCREDITO.IdRTC ?? 0;
                                int IdPersona = loBaseDa.Find<VTAPVENDEDORGRUPO>().Where(x => x.IdVendedorGrupo == idRTC).Select(x => x.IdPersona).FirstOrDefault() ?? 0;
                                string Correo = loBaseDa.Find<REHPEMPLEADO>().Where(x => x.IdPersona == IdPersona).Select(x => x.Correo).FirstOrDefault();
                                try
                                {
                                    EnviarPorCorreo(Correo, Asunto, Texto, null, false, "", "", false, "", 0, tsUsuario);
                                }
                                catch (Exception ex)
                                {
                                    psReturn = string.Format("ERROR: Revisar Actualizar Contraseña. Mensaje de Error: {0}",ex.Message);
                                }
                                
                            }

                            poTran.Complete();

                        }
                        
                    }
                }
            }
            
            return psReturn;

        }

        public string gsActualzarChecklistExcepcion(int tiId, string tsComentario, string tsUsuario, string tsTerminal)
        {
            var psReturn = "";
            loBaseDa.CreateContext();
            var poReg = loBaseDa.Get<CRETPROCESOCREDITODETALLE>().Where(x => x.IdProcesoCreditoDetalle == tiId).FirstOrDefault();
            if (poReg != null)
            {
                if (poReg.CodigoEstado != Diccionario.Aprobado)
                {
                    if (poReg.CodigoEstado == Diccionario.Cargado || poReg.CodigoEstado == Diccionario.EnRevision || poReg.CodigoEstado == Diccionario.Pendiente || poReg.CodigoEstado == Diccionario.EnEspera)
                    {
                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTransaccion);
                        poTransaccion.CodigoEstado = Diccionario.Activo;
                        poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                        poTransaccion.ComentarioAprobador = tsComentario;
                        poTransaccion.IdTransaccionReferencial = tiId;
                        poTransaccion.UsuarioAprobacion = tsUsuario;
                        poTransaccion.UsuarioIngreso = tsUsuario;
                        poTransaccion.FechaIngreso = DateTime.Now;
                        poTransaccion.TerminalIngreso = tsTerminal;
                        poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poReg.CodigoEstado);
                        poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Excepcion);
                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                        poReg.CodigoEstado = Diccionario.Excepcion;
                        poReg.FechaModificacion = DateTime.Now;
                        poReg.UsuarioModificacion = tsUsuario;
                        //poReg.Comentarios = tsComentario;

                        loBaseDa.SaveChanges();
                        gsActualzarRequerimientoCredito(poReg.IdProcesoCredito, "", tsUsuario, tsTerminal);
                    }
                }

            }

            return psReturn;

        }

        public string gsActualzarChecklistEnEspera(int tiId, string tsComentario, string tsUsuario, string tsTerminal)
        {
            var psReturn = "";
            loBaseDa.CreateContext();
            var poReg = loBaseDa.Get<CRETPROCESOCREDITODETALLE>().Where(x => x.IdProcesoCreditoDetalle == tiId).FirstOrDefault();
            if (poReg != null)
            {
                if (poReg.CodigoEstado != Diccionario.Aprobado)
                {
                    if (poReg.CodigoEstado == Diccionario.Cargado || poReg.CodigoEstado == Diccionario.EnRevision || poReg.CodigoEstado == Diccionario.Pendiente | poReg.CodigoEstado == Diccionario.Excepcion)
                    {
                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTransaccion);
                        poTransaccion.CodigoEstado = Diccionario.Activo;
                        poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                        poTransaccion.ComentarioAprobador = tsComentario;
                        poTransaccion.IdTransaccionReferencial = tiId;
                        poTransaccion.UsuarioAprobacion = tsUsuario;
                        poTransaccion.UsuarioIngreso = tsUsuario;
                        poTransaccion.FechaIngreso = DateTime.Now;
                        poTransaccion.TerminalIngreso = tsTerminal;
                        poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poReg.CodigoEstado);
                        poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.EnEspera);
                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                        poReg.CodigoEstado = Diccionario.EnEspera;
                        poReg.FechaModificacion = DateTime.Now;
                        poReg.UsuarioModificacion = tsUsuario;
                        //poReg.Comentarios = tsComentario;

                        loBaseDa.SaveChanges();
                        gsActualzarRequerimientoCredito(poReg.IdProcesoCredito, "", tsUsuario, tsTerminal);
                    }
                }

            }

            return psReturn;

        }


        public string gsActualzarChecklistAprobar(int tiId, string tsComentario, string tsUsuario, string tsTerminal)
        {
            var psReturn = "";
            loBaseDa.CreateContext();
            var poReg = loBaseDa.Get<CRETPROCESOCREDITODETALLE>().Where(x => x.IdProcesoCreditoDetalle == tiId).FirstOrDefault();
            if (poReg != null)
            {
                if (poReg.CodigoEstado == Diccionario.Cargado || poReg.CodigoEstado == Diccionario.EnRevision || poReg.CodigoEstado == Diccionario.Pendiente || poReg.CodigoEstado == Diccionario.Excepcion || poReg.CodigoEstado == Diccionario.EnEspera)
                {
                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                    poTransaccion.ComentarioAprobador = tsComentario;
                    poTransaccion.IdTransaccionReferencial = tiId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poReg.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Aprobado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    poReg.CodigoEstado = Diccionario.Aprobado;
                    poReg.FechaModificacion = DateTime.Now;
                    poReg.UsuarioModificacion = tsUsuario;

                    List<string> psEstados = new List<string>();
                    psEstados.Add(Diccionario.Cargado);
                    psEstados.Add(Diccionario.Corregir);
                    psEstados.Add(Diccionario.Pendiente);
                    psEstados.Add(Diccionario.EnRevision);

                    //var poCab = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x=>x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == poReg.IdProcesoCredito).FirstOrDefault();
                    //if (poCab != null)
                    //{
                    //    if (poCab.CRETPROCESOCREDITODETALLE.Where(x=>psEstados.Contains(x.CodigoEstado)).Count() == 0)
                    //    {
                    //        if (poCab.CodigoEstado != Diccionario.EnResolucion)
                    //        {
                    //            REHTTRANSACCIONAUTOIZACION poTran = new REHTTRANSACCIONAUTOIZACION();
                    //            loBaseDa.CreateNewObject(out poTran);
                    //            poTran.CodigoEstado = Diccionario.Activo;
                    //            poTran.CodigoTransaccion = Diccionario.Tablas.Transaccion.RequerimientoCredito;
                    //            poTran.ComentarioAprobador = "";
                    //            poTran.IdTransaccionReferencial = poCab.IdProcesoCredito;
                    //            poTran.UsuarioAprobacion = tsUsuario;
                    //            poTran.UsuarioIngreso = tsUsuario;
                    //            poTran.FechaIngreso = DateTime.Now;
                    //            poTran.TerminalIngreso = tsTerminal;
                    //            poTran.EstadoAnterior = Diccionario.gsGetDescripcion(poCab.CodigoEstado);
                    //            poTran.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.EnResolucion);
                    //            poTran.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    //            poCab.CodigoEstado = Diccionario.EnResolucion;
                    //            poCab.FechaModificacion = DateTime.Now;
                    //            poCab.UsuarioModificacion = tsUsuario;
                    //        }
                    //    }
                    //}

                    loBaseDa.SaveChanges();
                    gsActualzarRequerimientoCredito(poReg.IdProcesoCredito, "", tsUsuario, tsTerminal);
                }
            }


            return psReturn;

        }

        public string gsActualzarRequerimientoCredito(int tiId, string tsComentario, string tsUsuario, string tsTerminal)
        {
            var psReturn = "";
            loBaseDa.CreateContext();
            List<string> psEstadosEli = new List<string>();
            psEstadosEli.Add(Diccionario.Eliminado);
            psEstadosEli.Add(Diccionario.Inactivo);

            // Para la validación no debe considedar Excepcion
            psEstadosEli.Add(Diccionario.Excepcion);


            var poReg = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x=>x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == tiId).FirstOrDefault();
            if (poReg != null)
            {
                if (poReg.CodigoEstado != Diccionario.Finalizado && poReg.CodigoEstado != Diccionario.EnResolucion && poReg.CodigoEstado != Diccionario.EnAnalisis)
                {
                    bool CortoPlazo = poReg.Completado??false;

                    string psEstado = "";
                    var contTodos = poReg.CRETPROCESOCREDITODETALLE.Where(x => !psEstadosEli.Contains(x.CodigoEstado)).Count();
                    var contObli = poReg.CRETPROCESOCREDITODETALLE.Where(x => !psEstadosEli.Contains(x.CodigoEstado) && x.AgregadoRevision == null).Count();
                    var conCorregir = poReg.CRETPROCESOCREDITODETALLE.Where(x => x.CodigoEstado == Diccionario.Corregir).Count();
                    var conCargado = poReg.CRETPROCESOCREDITODETALLE.Where(x => x.CodigoEstado == Diccionario.Cargado).Count();
                    var conExcepcion = poReg.CRETPROCESOCREDITODETALLE.Where(x => x.CodigoEstado == Diccionario.Excepcion).Count();
                    var conPendiente = poReg.CRETPROCESOCREDITODETALLE.Where(x => x.CodigoEstado == Diccionario.Pendiente).Count();
                    var conRevision = poReg.CRETPROCESOCREDITODETALLE.Where(x => x.CodigoEstado == Diccionario.EnRevision).Count();
                    var conEnEspera = poReg.CRETPROCESOCREDITODETALLE.Where(x => x.CodigoEstado == Diccionario.EnEspera).Count();
                    var conDifApro = poReg.CRETPROCESOCREDITODETALLE.Where(x => !psEstadosEli.Contains(x.CodigoEstado) && x.CodigoEstado != Diccionario.Aprobado).Count();

                    if (!CortoPlazo)
                    {
                        if (conCorregir > 0)
                        {
                            psEstado = Diccionario.Pendiente;
                        }
                        else if (conPendiente > 0)
                        {
                            psEstado = Diccionario.Pendiente;
                        }
                        else if (conRevision > 0)
                        {
                            psEstado = EnRevision;
                        }
                        else if (conEnEspera > 0)
                        {
                            psEstado = Diccionario.EnEspera;
                        }
                        else if (contTodos == contObli && conCargado == contObli)
                        {
                            psEstado = Diccionario.Cargado;
                        }
                        else if (contTodos == conCargado)
                        {
                            psEstado = Diccionario.Cargado;
                        }
                        else if (conDifApro == conCargado)
                        {
                            psEstado = Diccionario.Cargado;
                        }
                        else if (conCargado > 0 && conPendiente == 0 && conCorregir == 0)
                        {
                            psEstado = Diccionario.Cargado;
                        }
                        else if (conDifApro == conCorregir)
                        {
                            psEstado = Diccionario.Corregir;
                        }
                        else if (conDifApro == conRevision)
                        {
                            psEstado = Diccionario.EnRevision;
                        }
                        else if (conDifApro == conRevision)
                        {
                            psEstado = Diccionario.EnRevision;
                        }
                        else if (conDifApro == conPendiente)
                        {
                            psEstado = Diccionario.Pendiente;
                        }
                    }
                    else
                    {
                        if (conCorregir > 0)
                        {
                            psEstado = Diccionario.Pendiente;
                        }
                        else if (conRevision > 0)
                        {
                            psEstado = EnRevision;
                        }
                        else if (conEnEspera > 0)
                        {
                            psEstado = Diccionario.EnEspera;
                        }
                        else if (conPendiente > 0)
                        {
                            List<int> piDocExc = new List<int>();
                            piDocExc.Add(2); // Solicitud de Crédito
                            piDocExc.Add(12); // Informe RTC
                            if (poReg.CRETPROCESOCREDITODETALLE.Where(x => x.CodigoEstado == Diccionario.Pendiente && !piDocExc.Contains(x.IdCheckList)).Count() > 0)
                            {
                                psEstado = Diccionario.Pendiente;
                            }
                            else
                            {
                                psEstado = Diccionario.Cargado;
                            }
                            
                        }
                        
                        else if (contTodos == contObli && conCargado == contObli)
                        {
                            psEstado = Diccionario.Cargado;
                        }
                        else if (contTodos == conCargado)
                        {
                            psEstado = Diccionario.Cargado;
                        }
                        else if (conDifApro == conCargado)
                        {
                            psEstado = Diccionario.Cargado;
                        }
                        else if (conCargado > 0 && conPendiente == 0 && conCorregir == 0)
                        {
                            psEstado = Diccionario.Cargado;
                        }
                        else if (conDifApro == conCorregir)
                        {
                            psEstado = Diccionario.Corregir;
                        }
                        else if (conDifApro == conRevision)
                        {
                            psEstado = Diccionario.EnRevision;
                        }
                        else if (conDifApro == conRevision)
                        {
                            psEstado = Diccionario.EnRevision;
                        }
                        else if (conDifApro == conPendiente)
                        {
                            psEstado = Diccionario.Pendiente;
                        }
                    }

                    

                    if (!string.IsNullOrEmpty(psEstado) && poReg.CodigoEstado != psEstado)
                    {
                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTransaccion);
                        poTransaccion.CodigoEstado = Diccionario.Activo;
                        poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.RequerimientoCredito;
                        poTransaccion.ComentarioAprobador = tsComentario;
                        poTransaccion.IdTransaccionReferencial = tiId;
                        poTransaccion.UsuarioAprobacion = tsUsuario;
                        poTransaccion.UsuarioIngreso = tsUsuario;
                        poTransaccion.FechaIngreso = DateTime.Now;
                        poTransaccion.TerminalIngreso = tsTerminal;
                        poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poReg.CodigoEstado);
                        poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psEstado);
                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                        poReg.CodigoEstado = psEstado;
                        poReg.FechaModificacion = DateTime.Now;
                        poReg.UsuarioModificacion = tsUsuario;

                        loBaseDa.SaveChanges();
                    }
                }
            }


            return psReturn;

        }
        
        public string gsActualzarChecklistDesAprobar(int tiId, string tsComentario, string tsUsuario, string tsTerminal)
        {
            var psReturn = "";
            loBaseDa.CreateContext();
            var poReg = loBaseDa.Get<CRETPROCESOCREDITODETALLE>().Where(x => x.IdProcesoCreditoDetalle == tiId).FirstOrDefault();
            if (poReg != null)
            {
                if (poReg.CodigoEstado == Diccionario.Aprobado)
                {
                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                    poTransaccion.ComentarioAprobador = tsComentario;
                    poTransaccion.IdTransaccionReferencial = tiId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poReg.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Cargado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    poReg.CodigoEstado = Diccionario.Cargado;
                    poReg.FechaModificacion = DateTime.Now;
                    poReg.UsuarioModificacion = tsUsuario;

                    List<string> psEstados = new List<string>();
                    psEstados.Add(Diccionario.Cargado);
                    psEstados.Add(Diccionario.Corregir);
                    psEstados.Add(Diccionario.Pendiente);
                    psEstados.Add(Diccionario.EnRevision);

                    var poCab = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == poReg.IdProcesoCredito).FirstOrDefault();
                    if (poCab != null)
                    {
                        if (poCab.CodigoEstado == Diccionario.EnResolucion)
                        {
                            REHTTRANSACCIONAUTOIZACION poTran = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTran);
                            poTran.CodigoEstado = Diccionario.Activo;
                            poTran.CodigoTransaccion = Diccionario.Tablas.Transaccion.RequerimientoCredito;
                            poTran.ComentarioAprobador = "";
                            poTran.IdTransaccionReferencial = poCab.IdProcesoCredito;
                            poTran.UsuarioAprobacion = tsUsuario;
                            poTran.UsuarioIngreso = tsUsuario;
                            poTran.FechaIngreso = DateTime.Now;
                            poTran.TerminalIngreso = tsTerminal;
                            poTran.EstadoAnterior = Diccionario.gsGetDescripcion(poCab.CodigoEstado);
                            poTran.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Pendiente);
                            poTran.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                            poCab.CodigoEstado = Diccionario.Pendiente;
                            poCab.FechaModificacion = DateTime.Now;
                            poCab.UsuarioModificacion = tsUsuario;
                        }
                    }

                    loBaseDa.SaveChanges();
                    gsActualzarRequerimientoCredito(poReg.IdProcesoCredito, "", tsUsuario, tsTerminal);
                }
            }


            return psReturn;

        }
        
        public string gsActualzarChecklistEnRevision(int tiId, string tsComentario, string tsUsuario, string tsTerminal)
        {
            var psReturn = "";
            loBaseDa.CreateContext();
            var poReg = loBaseDa.Get<CRETPROCESOCREDITODETALLE>().Include(x=>x.CRETPROCESOCREDITO).Where(x => x.IdProcesoCreditoDetalle == tiId).FirstOrDefault();
            
            if (poReg != null)
            {
                if (poReg.CRETPROCESOCREDITO.CodigoEstado != Diccionario.Cerrado && poReg.CRETPROCESOCREDITO.CodigoEstado != Diccionario.Finalizado)
                {
                    if (poReg.CodigoEstado != Diccionario.Aprobado)
                    {
                        if (poReg.CodigoEstado == Diccionario.Cargado)
                        {
                            REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTransaccion);
                            poTransaccion.CodigoEstado = Diccionario.Activo;
                            poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                            poTransaccion.ComentarioAprobador = tsComentario;
                            poTransaccion.IdTransaccionReferencial = tiId;
                            poTransaccion.UsuarioAprobacion = tsUsuario;
                            poTransaccion.UsuarioIngreso = tsUsuario;
                            poTransaccion.FechaIngreso = DateTime.Now;
                            poTransaccion.TerminalIngreso = tsTerminal;
                            poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poReg.CodigoEstado);
                            poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.EnRevision);
                            poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                            poReg.CodigoEstado = Diccionario.EnRevision;
                            poReg.FechaModificacion = DateTime.Now;
                            poReg.UsuarioModificacion = tsUsuario;

                            if (poReg.CRETPROCESOCREDITO.CodigoEstado != Diccionario.EnRevision)
                            {
                                REHTTRANSACCIONAUTOIZACION poTransacciondetalle = new REHTTRANSACCIONAUTOIZACION();
                                loBaseDa.CreateNewObject(out poTransacciondetalle);
                                poTransacciondetalle.CodigoEstado = Diccionario.Activo;
                                poTransacciondetalle.CodigoTransaccion = Diccionario.Tablas.Transaccion.RequerimientoCredito;
                                poTransacciondetalle.ComentarioAprobador = tsComentario;
                                poTransacciondetalle.IdTransaccionReferencial = poReg.CRETPROCESOCREDITO.IdProcesoCredito;
                                poTransacciondetalle.UsuarioAprobacion = tsUsuario;
                                poTransacciondetalle.UsuarioIngreso = tsUsuario;
                                poTransacciondetalle.FechaIngreso = DateTime.Now;
                                poTransacciondetalle.TerminalIngreso = tsTerminal;
                                poTransacciondetalle.EstadoAnterior = Diccionario.gsGetDescripcion(poReg.CRETPROCESOCREDITO.CodigoEstado);
                                poTransacciondetalle.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.EnRevision);
                                poTransacciondetalle.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                                poReg.CRETPROCESOCREDITO.CodigoEstado = Diccionario.EnRevision;
                                poReg.CRETPROCESOCREDITO.FechaModificacion = DateTime.Now;
                                poReg.CRETPROCESOCREDITO.UsuarioModificacion = tsUsuario;
                            }

                            loBaseDa.SaveChanges();
                            gsActualzarRequerimientoCredito(poReg.IdProcesoCredito, "", tsUsuario, tsTerminal);
                        }
                    }
                }

            }


            return psReturn;

        }

        public string gsActualzarChecklistCargado(int tiId, string tsComentario, string tsUsuario, string tsTerminal)
        {
            var psReturn = "";
            loBaseDa.CreateContext();
            var poReg = loBaseDa.Get<CRETPROCESOCREDITODETALLE>().Where(x => x.IdProcesoCreditoDetalle == tiId).FirstOrDefault();
            if (poReg != null)
            {
                if (poReg.CodigoEstado == Diccionario.Corregir || poReg.CodigoEstado == Diccionario.Pendiente)
                {
                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                    poTransaccion.ComentarioAprobador = tsComentario;
                    poTransaccion.IdTransaccionReferencial = tiId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poReg.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Cargado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    poReg.CodigoEstado = Diccionario.Cargado;
                    poReg.FechaModificacion = DateTime.Now;
                    poReg.UsuarioModificacion = tsUsuario;

                    loBaseDa.SaveChanges();
                    gsActualzarRequerimientoCredito(poReg.IdProcesoCredito, "", tsUsuario, tsTerminal);
                }
            }


            return psReturn;

        }

        public string gsGuardarRevision(ProcesoCredito toObject, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = lsEsValidoRevision(toObject);
            
            if (string.IsNullOrEmpty(psMsg))
            {
                var poObject = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x=>x.CRETPROCESOCREDITOACCIONISTA).Include(x => x.CRETPROCESOCREDITOADMACTUAL)
                    .Include(x => x.CRETPROCESOCREDITOBURO).Include(x => x.CRETPROCESOCREDITOFUNCIONJUDICIAL)
                    .Where(x => x.IdProcesoCredito == toObject.IdProcesoCredito).FirstOrDefault();

                if (poObject != null)
                {
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                    poObject.UsuarioModificacion = tsUsuario;

                    poObject.ComentarioBuro = toObject.ComentarioBuro;
                    poObject.ComentarioFuncionJudicial = toObject.ComentarioFuncionJudicial;
                    poObject.ComentarioSri = toObject.ComentarioSri;
                    poObject.ComentarioSuperIntendenciaCia = toObject.ComentarioSuperIntendenciaCia;
                    poObject.FechaConsultaSuper = toObject.FechaConsultaSuper;
                    poObject.RucSuper = toObject.RucSuper;
                    poObject.TipoCompaniaSuper = toObject.TipoCompaniaSuper;
                    poObject.CapitalSuscritoSuper = toObject.CapitalSuscritoSuper;
                    poObject.CumplimientoObligacionesSuper = toObject.CumplimientoObligacionesSuper;
                    poObject.MotivoNoCumplimientoObligacionesSuper = toObject.MotivoNoCumplimientoObligacionesSuper;

                    if (toObject.ProcesoCreditoAccionista != null)
                    {
                        //Eliminar Detalle 
                        var piListaIdPresentacion = toObject.ProcesoCreditoAccionista.Where(x => x.IdProcesoCreditoAccionista != 0).Select(x => x.IdProcesoCreditoAccionista).ToList();
                        var psLista = new List<string>();
                        psLista.Add(Diccionario.Eliminado);
                        psLista.Add(Diccionario.Inactivo);
                        foreach (var poItemDel in poObject.CRETPROCESOCREDITOACCIONISTA.Where(x => !psLista.Contains(x.CodigoEstado) && !piListaIdPresentacion.Contains(x.IdProcesoCreditoAccionista)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in toObject.ProcesoCreditoAccionista)
                        {
                            int pId = item.IdProcesoCreditoAccionista;
                            string psEstadoAntDet = "";
                            var poObjectItem = poObject.CRETPROCESOCREDITOACCIONISTA.Where(x => x.IdProcesoCreditoAccionista == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new CRETPROCESOCREDITOACCIONISTA();
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                                poObject.CRETPROCESOCREDITOACCIONISTA.Add(poObjectItem);
                            }

                            poObjectItem.CodigoEstado = Diccionario.Activo;
                            poObjectItem.Accionista = item.Accionista;
                            poObjectItem.Capital = item.Capital;
                            poObjectItem.ParticipacionAcciones = item.PorcentajeParticipacionAcciones;

                        }
                    }

                    if (toObject.ProcesoCreditoBuro != null)
                    {
                        //Eliminar Detalle 
                        var piListaIdPresentacion = toObject.ProcesoCreditoBuro.Where(x => x.IdProcesoCreditoBuro != 0).Select(x => x.IdProcesoCreditoBuro).ToList();
                        var psLista = new List<string>();
                        psLista.Add(Diccionario.Eliminado);
                        psLista.Add(Diccionario.Inactivo);
                        foreach (var poItemDel in poObject.CRETPROCESOCREDITOBURO.Where(x => !psLista.Contains(x.CodigoEstado) && !piListaIdPresentacion.Contains(x.IdProcesoCreditoBuro)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in toObject.ProcesoCreditoBuro)
                        {
                            int pId = item.IdProcesoCreditoBuro;
                            var poObjectItem = poObject.CRETPROCESOCREDITOBURO.Where(x => x.IdProcesoCreditoBuro == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new CRETPROCESOCREDITOBURO();
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                                poObject.CRETPROCESOCREDITOBURO.Add(poObjectItem);
                            }

                            poObjectItem.CodigoEstado = Diccionario.Activo;
                            poObjectItem.FechaConsulta = item.FechaConsulta;
                            poObjectItem.HabilitadoCtaCte = item.HabilitadoCtaCte;
                            poObjectItem.FechaInhabilitado = item.FechaInhabilitado;
                            poObjectItem.TiempoInhabilitado = item.TiempoInhabilitado;
                            poObjectItem.Score = item.Score;
                            poObjectItem.PorcentajeProbMora = item.PorcentajeProbMora;
                            poObjectItem.RiesgoComercial = item.RiesgoComercial;
                            poObjectItem.RiesgoFinanciero = item.RiesgoFinanciero;
                            poObjectItem.TotalRiesgo = item.TotalRiesgo;
                            poObjectItem.TotalDeudaVencida = item.TotalDeudaVencida;
                            poObjectItem.CuotasMensualesCancelTiempo = item.CuotasMensualesCancelTiempo;
                            poObjectItem.CuotasMensualesPagadasMora = item.CuotasMensualesPagadasMora;
                            poObjectItem.Nombre = item.Nombre;


                        }
                    }

                    if (toObject.ProcesoCreditoFuncionJudicial != null)
                    {
                        //Eliminar Detalle 
                        var piListaIdPresentacion = toObject.ProcesoCreditoFuncionJudicial.Where(x => x.IdProcesoCreditoFuncionJudicial != 0).Select(x => x.IdProcesoCreditoFuncionJudicial).ToList();
                        var psLista = new List<string>();
                        psLista.Add(Diccionario.Eliminado);
                        psLista.Add(Diccionario.Inactivo);
                        foreach (var poItemDel in poObject.CRETPROCESOCREDITOFUNCIONJUDICIAL.Where(x => !psLista.Contains(x.CodigoEstado) && !piListaIdPresentacion.Contains(x.IdProcesoCreditoFuncionJudicial)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in toObject.ProcesoCreditoFuncionJudicial)
                        {
                            int pId = item.IdProcesoCreditoFuncionJudicial;
                            var poObjectItem = poObject.CRETPROCESOCREDITOFUNCIONJUDICIAL.Where(x => x.IdProcesoCreditoFuncionJudicial == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new CRETPROCESOCREDITOFUNCIONJUDICIAL();
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                                poObject.CRETPROCESOCREDITOFUNCIONJUDICIAL.Add(poObjectItem);
                            }

                            poObjectItem.CodigoEstado = Diccionario.Activo;
                            poObjectItem.FechaConsulta = item.FechaConsulta;
                            poObjectItem.DemandasVigentes = item.DemandasVigentes;
                            poObjectItem.FechaCaso = item.FechaCaso;
                            poObjectItem.Ofendido = item.Ofendido;
                            poObjectItem.DetalleAccion = item.DetalleAccion;
                            poObjectItem.MontoDemanda = item.MontoDemanda;
                        }
                    }

                    if (toObject.ProcesoCreditoAdmActual != null)
                    {
                        //Eliminar Detalle 
                        var piListaIdPresentacion = toObject.ProcesoCreditoAdmActual.Where(x => x.IdProcesoCreditoAdmActual != 0).Select(x => x.IdProcesoCreditoAdmActual).ToList();
                        var psLista = new List<string>();
                        psLista.Add(Diccionario.Eliminado);
                        psLista.Add(Diccionario.Inactivo);
                        foreach (var poItemDel in poObject.CRETPROCESOCREDITOADMACTUAL.Where(x => !psLista.Contains(x.CodigoEstado) && !piListaIdPresentacion.Contains(x.IdProcesoCreditoAdmActual)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in toObject.ProcesoCreditoAdmActual)
                        {
                            int pId = item.IdProcesoCreditoAdmActual;
                            string psEstadoAntDet = "";
                            var poObjectItem = poObject.CRETPROCESOCREDITOADMACTUAL.Where(x => x.IdProcesoCreditoAdmActual == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new CRETPROCESOCREDITOADMACTUAL();
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                                poObject.CRETPROCESOCREDITOADMACTUAL.Add(poObjectItem);
                            }

                            poObjectItem.CodigoEstado = Diccionario.Activo;
                            poObjectItem.Nombre = item.Nombre;
                            poObjectItem.Cargo = item.Cargo;
                            poObjectItem.FechaNombramiento = item.FechaNombramiento;
                            poObjectItem.Anios = item.Anios;
                            poObjectItem.Tipo = item.Tipo;
                            poObjectItem.Accionista = item.Accionista;
                            poObjectItem.PorcentajeParticipacionAcciones = item.PorcentajeParticipacionAcciones;
                            
                        }
                    }

                    if (toObject.ProcesoCreditoSri != null)
                    {
                        //Eliminar Detalle 
                        var piListaIdPresentacion = toObject.ProcesoCreditoSri.Where(x => x.IdProcesoCreditoSri != 0).Select(x => x.IdProcesoCreditoSri).ToList();
                        var psLista = new List<string>();
                        psLista.Add(Diccionario.Eliminado);
                        psLista.Add(Diccionario.Inactivo);
                        foreach (var poItemDel in poObject.CRETPROCESOCREDITOSRI.Where(x => !psLista.Contains(x.CodigoEstado) && !piListaIdPresentacion.Contains(x.IdProcesoCreditoSri)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in toObject.ProcesoCreditoSri)
                        {
                            int pId = item.IdProcesoCreditoSri;
                            var poObjectItem = poObject.CRETPROCESOCREDITOSRI.Where(x => x.IdProcesoCreditoSri == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new CRETPROCESOCREDITOSRI();
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                                poObject.CRETPROCESOCREDITOSRI.Add(poObjectItem);
                            }

                            poObjectItem.CodigoEstado = Diccionario.Activo;
                            poObjectItem.FechaConsulta = item.FechaConsulta;
                            poObjectItem.ObligacionesPdtes = item.ObligacionesPdtes;
                            poObjectItem.TipoObligacion = item.TipoObligacion;
                            poObjectItem.ValorTotalObligacion = item.ValorTotalObligacion;
                            poObjectItem.MotivoObligacionesPdtes = item.MotivoObligacionesPdtes;
                        }
                    }
                }

                loBaseDa.SaveChanges();

                return psMsg;
            }

            return psMsg;
        }

        #region Bandeja de Informe de RTC
        public List<BandejaInformeRTC> goConsultarBandejaRTC(DateTime tdFechaInicial, DateTime tdFechaFinal, string tsUsuario)
        {
            return loBaseDa.ExecStoreProcedure<BandejaInformeRTC>(string.Format("CRESPBANDEJAINFORMERTC '{0}','{1}','{2}'", tdFechaInicial.Date, tdFechaFinal.Date, tsUsuario));
        }
        #endregion

        public string gsGuardarRevisionCedula(ProcesoCreditoCedula toObject, string tsTipo, string tsUsuario, string tsTerminal, int tIdSolCredito)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = "";

            if (string.IsNullOrEmpty(psMsg))
            {

                var poObject = loBaseDa.Get<CRETPROCESOCREDITOCEDULA>().Where(x => x.IdProcesoCredito == toObject.IdProcesoCredito && x.IdProcesoCredito != 0).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.FechaModificacion = DateTime.Now;   
                }
                else
                {
                    poObject = new CRETPROCESOCREDITOCEDULA();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.FechaIngreso = DateTime.Now;
                }

                poObject.IdentificacionRepLegal = toObject.IdentificacionRepLegal;
                poObject.FechaCaducidadCedulaRepLegal = toObject.FechaCaducidadCedulaRepLegal;
                poObject.FechaNacimiento = toObject.FechaNacimiento;
                poObject.Edad = toObject.Edad;
                poObject.CodigoEstadoCivil = toObject.CodigoEstadoCivil;
                poObject.IdentificacionConyuge = toObject.IdentificacionConyuge;
                poObject.NombreConyuge = toObject.NombreConyuge;
                poObject.ComentarioRevisionCedula = toObject.ComentarioRevisionCedula;
                poObject.IdProcesoCredito = toObject.IdProcesoCredito;

                if (tIdSolCredito > 0)
                {
                    var poObjectSol = loBaseDa.Get<CRETSOLICITUDCREDITO>().Where(x => x.IdSolicitudCredito == tIdSolCredito).FirstOrDefault();
                    if (poObjectSol != null)
                    {
                        poObjectSol.UsuarioModificacion = tsUsuario;
                        poObjectSol.FechaModificacion = DateTime.Now;
                        poObjectSol.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObjectSol = new CRETSOLICITUDCREDITO();
                        loBaseDa.CreateNewObject(out poObjectSol);
                        poObjectSol.UsuarioIngreso = tsUsuario;
                        poObjectSol.FechaIngreso = DateTime.Now;
                        poObjectSol.TerminalIngreso = tsTerminal;
                    }


                    poObjectSol.IdentificacionRepLegal = toObject.IdentificacionRepLegal;
                    poObjectSol.FechaCaducidadCedulaRepLegal = toObject.FechaCaducidadCedulaRepLegal;
                    poObjectSol.FechaNacimiento = toObject.FechaNacimiento;
                    poObjectSol.Edad = toObject.Edad;
                    poObjectSol.CodigoEstadoCivil = toObject.CodigoEstadoCivil;
                    poObjectSol.IdentificacionConyuge = toObject.IdentificacionConyuge;
                    poObjectSol.NombreConyuge = toObject.NombreConyuge;
                    poObjectSol.ComentarioRevisionCedula = toObject.ComentarioRevisionCedula;
                }

                var poRefOb = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == toObject.IdProcesoCredito).FirstOrDefault();
                if (poRefOb != null)
                {
                    poRefOb.Identificacion = toObject.IdentificacionRepLegal;
                }

                if (tsTipo == "REQ" && poObject.IdProcesoCredito != 0)
                {
                    poObject.IdProcesoCredito = poObject.IdProcesoCredito;
                    var poRef = poRefOb.CRETPROCESOCREDITODETALLE.Where(x => x.IdProcesoCredito == poObject.IdProcesoCredito).ToList();
                    if (poRef != null)
                    {
                        foreach (var item in poRef.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == 3))
                        {
                            if (item.CodigoEstado != Diccionario.Cargado)
                            {
                                REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                loBaseDa.CreateNewObject(out poTransaccion);
                                poTransaccion.CodigoEstado = Diccionario.Activo;
                                poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                                poTransaccion.ComentarioAprobador = "";
                                poTransaccion.IdTransaccionReferencial = item.IdProcesoCreditoDetalle;
                                poTransaccion.UsuarioAprobacion = tsUsuario;
                                poTransaccion.UsuarioIngreso = tsUsuario;
                                poTransaccion.FechaIngreso = DateTime.Now;
                                poTransaccion.TerminalIngreso = tsTerminal;
                                poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(item.CodigoEstado);
                                poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Cargado);
                                poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                                item.Completado = true;
                                item.CodigoEstado = Diccionario.Cargado;
                                item.FechaReferencial = poObject.FechaIngreso;
                                item.Comentarios = "";
                            }
                        }
                    }
                }

                loBaseDa.SaveChanges();

                return psMsg;
            }

            return psMsg;
        }

        public string gsGuardarRevisionNombramiento(ProcesoCredito toObject, string tsTipo, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = "";

            if (string.IsNullOrEmpty(psMsg))
            {

                var poObject = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x=>x.CRETPROCESOCREDITONOMBRAMIENTO).Where(x => x.IdProcesoCredito == toObject.IdProcesoCredito && x.IdProcesoCredito != 0).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObject = new CRETPROCESOCREDITO();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                }


                poObject.ComentariosNombramiento = toObject.ComentariosNombramiento;
                poObject.Identificacion = toObject.Identificacion;

                gActualizaRequerimiento(toObject.IdProcesoCredito, toObject.RepresentanteLegal, 0,0, false);

                if (toObject.ProcesoCreditoNombramiento != null)
                {
                    //Eliminar Detalle 
                    var piListaIdPresentacion = toObject.ProcesoCreditoNombramiento.Where(x => x.IdProcesoCreditoNombramiento != 0).Select(x => x.IdProcesoCreditoNombramiento).ToList();

                    foreach (var poItemDel in poObject.CRETPROCESOCREDITONOMBRAMIENTO.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdProcesoCreditoNombramiento)))
                    {
                        poItemDel.CodigoEstado = Diccionario.Inactivo;
                        poItemDel.UsuarioModificacion = tsUsuario;
                        poItemDel.FechaModificacion = DateTime.Now;
                        poItemDel.TerminalModificacion = tsTerminal;
                    }

                    foreach (var item in toObject.ProcesoCreditoNombramiento)
                    {
                        int pId = item.IdProcesoCreditoNombramiento;
                        var poObjectItem = poObject.CRETPROCESOCREDITONOMBRAMIENTO.Where(x => x.IdProcesoCreditoNombramiento == pId && pId != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new CRETPROCESOCREDITONOMBRAMIENTO();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.CRETPROCESOCREDITONOMBRAMIENTO.Add(poObjectItem);
                        }

                        poObjectItem.IdProcesoCredito = toObject.IdProcesoCredito;
                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.Cargo = item.Cargo;
                        poObjectItem.CodigoPresentacion = item.CodigoPresentacion;
                        poObjectItem.FechaCaducidad = item.FechaCaducidad;
                        poObjectItem.FechaInscripcion = item.FechaInscripcion;
                        poObjectItem.Periodo = item.Periodo;
                    }
                }

                if (tsTipo == "REQ" && poObject.IdProcesoCredito != 0)
                {
                    poObject.IdProcesoCredito = poObject.IdProcesoCredito;
                    var poRef = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == poObject.IdProcesoCredito).FirstOrDefault();
                    if (poRef != null)
                    {
                        foreach (var item in poRef.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == 6))
                        {
                            if (item.CodigoEstado != Diccionario.Cargado)
                            {
                                REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                loBaseDa.CreateNewObject(out poTransaccion);
                                poTransaccion.CodigoEstado = Diccionario.Activo;
                                poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                                poTransaccion.ComentarioAprobador = "";
                                poTransaccion.IdTransaccionReferencial = item.IdProcesoCreditoDetalle;
                                poTransaccion.UsuarioAprobacion = tsUsuario;
                                poTransaccion.UsuarioIngreso = tsUsuario;
                                poTransaccion.FechaIngreso = DateTime.Now;
                                poTransaccion.TerminalIngreso = tsTerminal;
                                poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(item.CodigoEstado);
                                poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Cargado);
                                poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                                item.Completado = true;
                                item.CodigoEstado = Diccionario.Cargado;
                                item.FechaReferencial = poObject.FechaIngreso;
                                item.Comentarios = "";
                            }
                        }
                    }
                }

                loBaseDa.SaveChanges();

                return psMsg;
            }

            return psMsg;
        }

        public string gsGuardarRevisionRuc(ProcesoCreditoRuc toObject, string tsTipo, string tsUsuario, string tsTerminal, int tIdSolCredito)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = "";

            if (string.IsNullOrEmpty(psMsg))
            {

                var poObject = loBaseDa.Get<CRETPROCESOCREDITORUC>().Where(x => x.IdProcesoCredito == toObject.IdProcesoCredito && x.IdProcesoCredito != 0).FirstOrDefault();
                if (poObject == null)
                {
                    poObject = new CRETPROCESOCREDITORUC();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.FechaIngreso = DateTime.Now;
                }

                poObject.EstadoRuc = toObject.EstadoRuc;
                poObject.FechaInicioActividadRuc = toObject.FechaInicioActividadRuc;
                poObject.FechaReinicioActividadRuc = toObject.FechaReinicioActividadRuc;
                poObject.FechaUltActualizacionRuc = toObject.FechaUltActualizacionRuc;
                poObject.ObligadoLlevarCantidadRuc = toObject.ObligadoLlevarCantidadRuc;
                poObject.OtrasActividades = toObject.OtrasActividades;
                poObject.ActividadesSecundarias = toObject.ActividadesSecundarias;
                poObject.Comentarios = toObject.Comentarios;
                poObject.Identificacion = toObject.Identificacion;
                poObject.NombreComercial = toObject.NombreComercial;
                poObject.CodigoTipoNegocio = toObject.CodigoTipoNegocio;
                poObject.IdProcesoCredito = toObject.IdProcesoCredito;


                if (tIdSolCredito > 0)
                {
                    var poObjectSol = loBaseDa.Get<CRETSOLICITUDCREDITO>().Where(x => x.IdSolicitudCredito == tIdSolCredito && x.IdSolicitudCredito != 0).FirstOrDefault();
                    if (poObjectSol != null)
                    {
                        poObjectSol.UsuarioModificacion = tsUsuario;
                        poObjectSol.FechaModificacion = DateTime.Now;
                        poObjectSol.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObjectSol = new CRETSOLICITUDCREDITO();
                        loBaseDa.CreateNewObject(out poObjectSol);
                        poObjectSol.UsuarioIngreso = tsUsuario;
                        poObjectSol.FechaIngreso = DateTime.Now;
                        poObjectSol.TerminalIngreso = tsTerminal;
                    }

                    poObjectSol.EstadoRuc = toObject.EstadoRuc;
                    poObjectSol.FechaInicioActividadRuc = toObject.FechaInicioActividadRuc;
                    poObjectSol.FechaReinicioActividadRuc = toObject.FechaReinicioActividadRuc;
                    poObjectSol.FechaUltActualizacionRuc = toObject.FechaUltActualizacionRuc;
                    poObjectSol.ObligadoLlevarCantidadRuc = toObject.ObligadoLlevarCantidadRuc;
                    poObjectSol.OtrasActividades = toObject.OtrasActividades;
                    poObjectSol.ActividadesSecundarias = toObject.ActividadesSecundarias;
                    poObjectSol.Comentarios = toObject.Comentarios;
                    poObjectSol.Identificacion = toObject.Identificacion;
                    poObjectSol.NombreComercial = toObject.NombreComercial;
                    poObjectSol.CodigoTipoNegocio = toObject.CodigoTipoNegocio;
                    poObjectSol.IdProcesoCredito = toObject.IdProcesoCredito;
                }

                

                if (toObject.ProcesoCreditoRucDireccion != null)
                {
                    //Eliminar Detalle 
                    var piListaIdPresentacion = toObject.ProcesoCreditoRucDireccion.Where(x => x.IdProcesoCreditoRucDireccion != 0).Select(x => x.IdProcesoCreditoRucDireccion).ToList();

                    foreach (var poItemDel in poObject.CRETPROCESOCREDITORUCDIRECCION.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdProcesoCreditoRucDireccion)))
                    {
                        poItemDel.CodigoEstado = Diccionario.Inactivo;
                        poItemDel.UsuarioModificacion = tsUsuario;
                        poItemDel.FechaModificacion = DateTime.Now;
                        poItemDel.TerminalModificacion = tsTerminal;
                    }

                    foreach (var item in toObject.ProcesoCreditoRucDireccion)
                    {
                        int pId = item.IdProcesoCreditoRucDireccion;
                        var poObjectItem = poObject.CRETPROCESOCREDITORUCDIRECCION.Where(x => x.IdProcesoCreditoRucDireccion == pId && pId != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new CRETPROCESOCREDITORUCDIRECCION();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.CRETPROCESOCREDITORUCDIRECCION.Add(poObjectItem);
                        }
                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.Principal = item.Principal;
                        poObjectItem.Direccion = item.Direccion;
                    }
                }

                if (tsTipo == "REQ" && poObject.IdProcesoCredito != 0)
                {
                    poObject.IdProcesoCredito = poObject.IdProcesoCredito;
                    var poRef = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == poObject.IdProcesoCredito).FirstOrDefault();
                    if (poRef != null)
                    {
                        foreach (var item in poRef.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == 5))
                        {
                            if (item.CodigoEstado != Diccionario.Cargado)
                            {
                                REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                loBaseDa.CreateNewObject(out poTransaccion);
                                poTransaccion.CodigoEstado = Diccionario.Activo;
                                poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                                poTransaccion.ComentarioAprobador = "";
                                poTransaccion.IdTransaccionReferencial = item.IdProcesoCreditoDetalle;
                                poTransaccion.UsuarioAprobacion = tsUsuario;
                                poTransaccion.UsuarioIngreso = tsUsuario;
                                poTransaccion.FechaIngreso = DateTime.Now;
                                poTransaccion.TerminalIngreso = tsTerminal;
                                poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(item.CodigoEstado);
                                poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Cargado);
                                poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                                item.Completado = true;
                                item.CodigoEstado = Diccionario.Cargado;
                                item.FechaReferencial = poObject.FechaIngreso;
                                item.Comentarios = "";
                            }
                        }
                    }
                }

                loBaseDa.SaveChanges();

                return psMsg;
            }

            return psMsg;
        }

        public string gsGuardarRevisionRefBancaria(ProcesoCredito toObject, string tsTipo, string tsUsuario, string tsTerminal)

        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = "";

            if (string.IsNullOrEmpty(psMsg))
            {

                var poObject = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITOREFBANCARIA).Where(x => x.IdProcesoCredito == toObject.IdProcesoCredito && x.IdProcesoCredito != 0).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObject = new CRETPROCESOCREDITO();
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                }

                poObject.ComentariosCertBancarios = toObject.ComentariosCertBancarios;

                if (toObject.ProcesoCreditoRefBancaria != null)
                {
                    //Eliminar Detalle 
                    var piListaIdPresentacion = toObject.ProcesoCreditoRefBancaria.Where(x => x.IdProcesoCreditoRefBancaria != 0).Select(x => x.IdProcesoCreditoRefBancaria).ToList();

                    foreach (var poItemDel in poObject.CRETPROCESOCREDITOREFBANCARIA.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdProcesoCreditoRefBancaria)))
                    {
                        poItemDel.CodigoEstado = Diccionario.Inactivo;
                        poItemDel.UsuarioModificacion = tsUsuario;
                        poItemDel.FechaModificacion = DateTime.Now;
                        poItemDel.TerminalModificacion = tsTerminal;
                    }

                    foreach (var item in toObject.ProcesoCreditoRefBancaria)
                    {
                        int pId = item.IdProcesoCreditoRefBancaria;
                        var poObjectItem = poObject.CRETPROCESOCREDITOREFBANCARIA.Where(x => x.IdProcesoCreditoRefBancaria == pId && pId != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new CRETPROCESOCREDITOREFBANCARIA();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.CRETPROCESOCREDITOREFBANCARIA.Add(poObjectItem);
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.CodigoBanco = item.CodigoBanco;
                        poObjectItem.CodigoTipoCuentaBancaria = item.CodigoTipoCuentaBancaria;
                        poObjectItem.NumeroCuenta = item.NumeroCuenta;
                        poObjectItem.FechaApertura = item.FechaApertura;
                        poObjectItem.ArchivoAdjunto = item.ArchivoAdjunto;
                        poObjectItem.NombreOriginal = item.NombreOriginal;
                        poObjectItem.FechaEmision = item.FechaEmision;
                        poObjectItem.Titular = item.Titular;
                        poObjectItem.SaldosPromedios = item.SaldosPromedios;
                    }
                }

                if (tsTipo == "REQ" && poObject.IdProcesoCredito != 0)
                {
                    poObject.IdProcesoCredito = poObject.IdProcesoCredito;
                    var poRef = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == poObject.IdProcesoCredito).FirstOrDefault();
                    if (poRef != null)
                    {
                        foreach (var item in poRef.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == 11))
                        {
                            if (item.CodigoEstado != Diccionario.Cargado)
                            {
                                REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                loBaseDa.CreateNewObject(out poTransaccion);
                                poTransaccion.CodigoEstado = Diccionario.Activo;
                                poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                                poTransaccion.ComentarioAprobador = "";
                                poTransaccion.IdTransaccionReferencial = item.IdProcesoCreditoDetalle;
                                poTransaccion.UsuarioAprobacion = tsUsuario;
                                poTransaccion.UsuarioIngreso = tsUsuario;
                                poTransaccion.FechaIngreso = DateTime.Now;
                                poTransaccion.TerminalIngreso = tsTerminal;
                                poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(item.CodigoEstado);
                                poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Cargado);
                                poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                                item.Completado = true;
                                item.CodigoEstado = Diccionario.Cargado;
                                item.FechaReferencial = poObject.FechaIngreso;
                                item.Comentarios = "";
                            }
                        }
                    }
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();

                    foreach (var poItem in toObject.ProcesoCreditoRefBancaria)
                    {
                        if (!string.IsNullOrEmpty(poItem.ArchivoAdjunto) && !string.IsNullOrEmpty(poItem.RutaDestino))
                        {
                            if (poItem.RutaOrigen != poItem.RutaDestino)
                            {
                                if (poItem.RutaOrigen != null)
                                {
                                    File.Copy(poItem.RutaOrigen, poItem.RutaDestino);
                                }
                            }
                        }
                    }

                    poTran.Complete();
                }

                return psMsg;
            }

            return psMsg;
        }

        public string gsGuardarRevisionRefComercial(ProcesoCredito toObject, string tsTipo, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = "";

            if (string.IsNullOrEmpty(psMsg))
            {

                var poObject = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITOREFCOMERCIAL).Where(x => x.IdProcesoCredito == toObject.IdProcesoCredito && x.IdProcesoCredito != 0).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObject = new CRETPROCESOCREDITO();
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                }

                poObject.ComentariosRefComerciales = toObject.ComentariosRefComerciales;
                if (toObject.ProcesoCreditoRefComercial != null)
                {
                    //Eliminar Detalle 
                    var piListaIdPresentacion = toObject.ProcesoCreditoRefComercial.Where(x => x.IdProcesoCreditoRefComercial != 0).Select(x => x.IdProcesoCreditoRefComercial).ToList();

                    foreach (var poItemDel in poObject.CRETPROCESOCREDITOREFCOMERCIAL.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdProcesoCreditoRefComercial)))
                    {
                        poItemDel.CodigoEstado = Diccionario.Inactivo;
                        poItemDel.UsuarioModificacion = tsUsuario;
                        poItemDel.FechaModificacion = DateTime.Now;
                        poItemDel.TerminalModificacion = tsTerminal;
                    }

                    foreach (var item in toObject.ProcesoCreditoRefComercial)
                    {
                        int pId = item.IdProcesoCreditoRefComercial;
                        var poObjectItem = poObject.CRETPROCESOCREDITOREFCOMERCIAL.Where(x => x.IdProcesoCreditoRefComercial == pId && pId != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new CRETPROCESOCREDITOREFCOMERCIAL();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.CRETPROCESOCREDITOREFCOMERCIAL.Add(poObjectItem);
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.Nombre = item.Nombre;
                        poObjectItem.Telefono = item.Telefono;
                        poObjectItem.ArchivoAdjunto = item.ArchivoAdjunto;
                        poObjectItem.NombreOriginal = item.NombreOriginal;
                        poObjectItem.FechaReferenciaComercial = item.FechaReferenciaComercial;


                        poObjectItem.ClienteDesde = item.ClienteDesde;
                        poObjectItem.Cupo = item.Cupo;
                        poObjectItem.FechaConsulta = item.FechaConsulta;
                        poObjectItem.Garantia = item.Garantia;
                        poObjectItem.Plazo = item.Plazo;
                        poObjectItem.PromedioComprasAnual = item.PromedioComprasAnual;
                        poObjectItem.PromedioComprasMensual = item.PromedioComprasMensual;
                        poObjectItem.PromedioDiasPagos = item.PromedioDiasPagos;
                        poObjectItem.CodigoGarantia = item.CodigoGarantia;
                    }
                }

                if (tsTipo == "REQ" && poObject.IdProcesoCredito != 0)
                {
                    poObject.IdProcesoCredito = poObject.IdProcesoCredito;
                    var poRef = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == poObject.IdProcesoCredito).FirstOrDefault();
                    if (poRef != null)
                    {
                        foreach (var item in poRef.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == 14))
                        {
                            if (item.CodigoEstado != Diccionario.Cargado)
                            {
                                REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                                loBaseDa.CreateNewObject(out poTransaccion);
                                poTransaccion.CodigoEstado = Diccionario.Activo;
                                poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.Checklist;
                                poTransaccion.ComentarioAprobador = "";
                                poTransaccion.IdTransaccionReferencial = item.IdProcesoCreditoDetalle;
                                poTransaccion.UsuarioAprobacion = tsUsuario;
                                poTransaccion.UsuarioIngreso = tsUsuario;
                                poTransaccion.FechaIngreso = DateTime.Now;
                                poTransaccion.TerminalIngreso = tsTerminal;
                                poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(item.CodigoEstado);
                                poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Cargado);
                                poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                                item.Completado = true;
                                item.CodigoEstado = Diccionario.Cargado;
                                item.FechaReferencial = poObject.FechaIngreso;
                                item.Comentarios = "";
                            }
                        }
                    }
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();

                    foreach (var poItem in toObject.ProcesoCreditoRefComercial)
                    {
                        if (!string.IsNullOrEmpty(poItem.ArchivoAdjunto) && !string.IsNullOrEmpty(poItem.RutaDestino))
                        {
                            if (poItem.RutaOrigen != poItem.RutaDestino)
                            {
                                if (poItem.RutaOrigen != null)
                                {
                                    File.Copy(poItem.RutaOrigen, poItem.RutaDestino);
                                }
                            }
                        }
                    }

                    poTran.Complete();
                }

                return psMsg;
            }

            return psMsg;
        }

        public string gsGuardarRevisionBienes(ProcesoCredito toObject, string tsTipo, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = "";

            if (string.IsNullOrEmpty(psMsg))
            {
                var poObject = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITOOTROSACTIVOS).Include(x => x.CRETPROCESOCREDITOPROPIEDADES)
                    .Where(x => x.IdProcesoCredito == toObject.IdProcesoCredito && x.IdProcesoCredito != 0).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObject = new CRETPROCESOCREDITO();
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                }

                poObject.ComentariosBienes = toObject.ComentariosBienes;

                if (toObject.ProcesoCreditoOtrosActivos != null)
                {
                    //Eliminar Detalle 
                    var piListaIdPresentacion = toObject.ProcesoCreditoOtrosActivos.Where(x => x.IdProcesoCreditoOtrosActivos != 0).Select(x => x.IdProcesoCreditoOtrosActivos).ToList();

                    foreach (var poItemDel in poObject.CRETPROCESOCREDITOOTROSACTIVOS.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdProcesoCreditoOtrosActivos)))
                    {
                        poItemDel.CodigoEstado = Diccionario.Inactivo;
                        poItemDel.UsuarioModificacion = tsUsuario;
                        poItemDel.FechaModificacion = DateTime.Now;
                        poItemDel.TerminalModificacion = tsTerminal;
                    }

                    foreach (var item in toObject.ProcesoCreditoOtrosActivos)
                    {
                        int pId = item.IdProcesoCreditoOtrosActivos;
                        var poObjectItem = poObject.CRETPROCESOCREDITOOTROSACTIVOS.Where(x => x.IdProcesoCreditoOtrosActivos == pId && pId != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new CRETPROCESOCREDITOOTROSACTIVOS();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.CRETPROCESOCREDITOOTROSACTIVOS.Add(poObjectItem);
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.CodigoTipoOtrosActivos = item.CodigoTipoOtrosActivos;
                        poObjectItem.Marca = item.Marca;
                        poObjectItem.Modelo = item.Modelo;
                        poObjectItem.Anio = item.Anio;
                        poObjectItem.AvaluoComercial = item.AvaluoComercial;
                        poObjectItem.DescripcionDocumento = item.DescripcionDocumento;
                        poObjectItem.PropietarioBeneficiario = item.PropietarioBeneficiario;
                    }
                }

                if (toObject.ProcesoCreditoPropiedades != null)
                {
                    //Eliminar Detalle 
                    var piListaIdPresentacion = toObject.ProcesoCreditoPropiedades.Where(x => x.IdProcesoCreditoPropiedades != 0).Select(x => x.IdProcesoCreditoPropiedades).ToList();

                    foreach (var poItemDel in poObject.CRETPROCESOCREDITOPROPIEDADES.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdProcesoCreditoPropiedades)))
                    {
                        poItemDel.CodigoEstado = Diccionario.Inactivo;
                        poItemDel.UsuarioModificacion = tsUsuario;
                        poItemDel.FechaModificacion = DateTime.Now;
                        poItemDel.TerminalModificacion = tsTerminal;
                    }

                    foreach (var item in toObject.ProcesoCreditoPropiedades)
                    {
                        int pId = item.IdProcesoCreditoPropiedades;
                        var poObjectItem = poObject.CRETPROCESOCREDITOPROPIEDADES.Where(x => x.IdProcesoCreditoPropiedades == pId && pId != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new CRETPROCESOCREDITOPROPIEDADES();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.CRETPROCESOCREDITOPROPIEDADES.Add(poObjectItem);
                        }
                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.CodigoTipoBien = item.CodigoTipoBien;
                        poObjectItem.Direccion = item.Direccion;
                        poObjectItem.Hipoteca = item.Hipoteca;
                        poObjectItem.AvaluoComercial = item.AvaluoComercial;
                        poObjectItem.FechaPago = item.FechaPago;
                        poObjectItem.DescripcionDocumento = item.DescripcionDocumento;
                        poObjectItem.PropietarioBeneficiario = item.PropietarioBeneficiario;
                    }
                }

                loBaseDa.SaveChanges();

                return psMsg;
            }

            return psMsg;
        }

        #region Reporte de Requerimientos

        public List<spRequerimientos> goConsultarRequerimientos(DateTime tdFechaDesde, DateTime tdFechaHasta)
        {
            return loBaseDa.ExecStoreProcedure<spRequerimientos>("CRESPCONSULTAREQUERIMIENTOS @FechaDesde, @FechaHasta", new SqlParameter("@FechaDesde", tdFechaDesde), new SqlParameter("@FechaHasta", tdFechaHasta));
        }

        public List<RequerimientoDetalle> goConsultarRequerimientoDetalle(int tId)
        {
            List<string> psEstados = new List<string>();
            psEstados.Add(Diccionario.Activo);
            psEstados.Add(Diccionario.Pendiente);
            psEstados.Add(Diccionario.EnRevision);
            psEstados.Add(Diccionario.Corregir);
            psEstados.Add(Diccionario.Excepcion);

            return loBaseDa.Find<CRETPROCESOCREDITODETALLE>().Where(x => psEstados.Contains(x.CodigoEstado) && x.IdProcesoCredito == tId).ToList()
                .Select(x => new RequerimientoDetalle()
            {
                IdProcesoCredito = x.IdProcesoCredito,
                IdProcesoCreditoDetalle = x.IdProcesoCreditoDetalle,
                Documento = x.CheckList,
                Estado = Diccionario.gsGetDescripcion(x.CodigoEstado),
                FechaCompromiso = x.FechaCompromiso,
            }).ToList();
        }

        #endregion
    }
}
