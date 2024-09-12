using GEN_Entidad.Entidades.Credito;
using GEN_Negocio;
using REH_Dato;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GEN_Entidad;
using static GEN_Entidad.Diccionario;
using System.Configuration;
using System.Transactions;
using System.IO;

namespace CRE_Negocio.Transacciones
{
    public class clsNSolicitudCredito : clsNBase
    {

        public SolicitudCredito goConsultar(int tId, int tIdProcesoCredito = 0)
        {
            loBaseDa.CreateContext();
            var poLista = loBaseDa.Find<CRETSOLICITUDCREDITO>().Include(x => x.CRETSOLICITUDCREDITOOTROSACTIVOS).Include(x => x.CRETSOLICITUDCREDITOREFBANCARIA)
                .Include(x => x.CRETSOLICITUDCREDITOREFCOMERCIAL).Include(x => x.CRETSOLICITUDCREDITOREFPERSONAL).Include(x => x.CRETSOLICITUDCREDITOPROPIEDADES)
                .Include(x=>x.CRETSOLICITUDCREDITODIRECCION).Where(x => x.IdSolicitudCredito == tId).ToList();
            return loLlenarDatos(poLista, tIdProcesoCredito).FirstOrDefault();

        }

        private List<SolicitudCredito> loLlenarDatos(List<CRETSOLICITUDCREDITO> poLista, int tIdProcesoCredito = 0)
        {
            var poListaReturn = new List<SolicitudCredito>();
            if (tIdProcesoCredito != 0)
            {
                var poCab = new SolicitudCredito();
                poCab.ProcesoCredito = new clsNProcesoCredito().goConsultar(tIdProcesoCredito);
                poListaReturn.Add(poCab);
            }
            else
            {
                foreach (var item in poLista)
                {
                    var poCab = new SolicitudCredito();
                    poCab.IdSolicitudCredito = item.IdSolicitudCredito;
                    poCab.IdProcesoCredito = item.IdProcesoCredito ?? 0;
                    poCab.CodigoEstado = item.CodigoEstado;
                    poCab.FechaSolicitud = item.FechaSolicitud;
                    poCab.CodigoTipoSolicitud = item.CodigoTipoSolicitud;
                    poCab.TipoSolicitud = item.TipoSolicitud;
                    poCab.CodigoActividadCliente = item.CodigoActividadCliente;
                    poCab.ActividadCliente = item.ActividadCliente;
                    poCab.IdRTC = item.IdRTC;
                    poCab.RTC = item.RTC;
                    poCab.Zona = item.Zona;
                    poCab.Cupo = item.Cupo;
                    poCab.CupoSap = item.CupoSap ?? 0m;
                    poCab.Observacion = item.Observacion;
                    poCab.CodigoCliente = item.CodigoCliente;
                    poCab.Cliente = item.Cliente;
                    poCab.Identificacion = item.Identificacion;
                    poCab.FechaNacimiento = item.FechaNacimiento;
                    poCab.CodigoEstadoCivil = item.CodigoEstadoCivil;
                    poCab.Ciudad = item.Ciudad;
                    poCab.DireccionDomicilio = item.DireccionDomicilio;
                    poCab.Telefono = item.Telefono;
                    poCab.Celular = item.Celular;
                    poCab.CodigoVivienda = item.CodigoVivienda;
                    poCab.Tiempo = item.Tiempo;
                    poCab.Email = item.Email;
                    poCab.NombreConyuge = item.NombreConyuge;
                    poCab.IdentificacionConyuge = item.IdentificacionConyuge;
                    poCab.NombreComercial = item.NombreComercial;
                    poCab.TiempoActividad = item.TiempoActividad;
                    poCab.Ruc = item.Ruc;
                    poCab.CodigoTipoNegocio = item.CodigoTipoNegocio;
                    poCab.CiudadNegocio = item.CiudadNegocio;
                    poCab.TelefonoNegocio = item.TelefonoNegocio;
                    poCab.CelularNegocio = item.CelularNegocio;
                    poCab.CodigoLocal = item.CodigoLocal;
                    poCab.TiempoNegocio = item.TiempoNegocio;
                    poCab.ActividadAnterior = item.ActividadAnterior;
                    poCab.TiempoActividadAnterior = item.TiempoActividadAnterior;
                    poCab.EmpresaActividadAnterior = item.EmpresaActividadAnterior;
                    poCab.CargoActividadAnterior = item.CargoActividadAnterior;
                    poCab.NombreContactoPago = item.NombreContactoPago;
                    poCab.CargoContactoPago = item.CargoContactoPago;
                    poCab.TelefonoContactoPago = item.TelefonoContactoPago;
                    poCab.CelularContactoPago = item.CelularContactoPago;
                    poCab.EmailContactoPago = item.EmailContactoPago;
                    poCab.EmailFacturaElectronica = item.EmailFacturaElectronica;
                    poCab.OtrosIngresos = item.OtrosIngresos;
                    poCab.OrigenOtrosIngresos = item.OrigenOtrosIngresos;
                    poCab.EmailRealizaCompras = item.EmailRealizaCompras;
                    poCab.EmailRecibeCompras = item.EmailRecibeCompras;
                    poCab.IdentificacionRealizaCompras = item.IdentificacionRealizaCompras;
                    poCab.IdentificacionEmailRecibeCompras = item.IdentificacionEmailRecibeCompras;
                    poCab.TiempoAgricultor = item.TiempoAgricultor;
                    poCab.ClaseCultivo = item.ClaseCultivo;
                    poCab.NumeroHectariasCultivadas = item.NumeroHectariasCultivadas;
                    poCab.CodigoTerreno = item.CodigoTerreno;
                    poCab.AntiguedadTerreno = item.AntiguedadTerreno;
                    poCab.MesesCosecha = item.MesesCosecha;
                    poCab.PromedioIngresos = item.PromedioIngresos;
                    poCab.UbicacionTerrenos = item.UbicacionTerrenos;
                    poCab.Completado = item.Completado ?? false;
                    poCab.CupoSap = item.CupoSap ?? 0m;

                    poCab.NombreContactoPagoAgricultor = item.NombreContactoPagoAgricultor;
                    poCab.CargoContactoPagoAgricultor = item.CargoContactoPagoAgricultor;
                    poCab.TelefonoContactoPagoAgricultor = item.TelefonoContactoPagoAgricultor;
                    poCab.CelularContactoPagoAgricultor = item.CelularContactoPagoAgricultor;
                    poCab.EmailContactoPagoAgricultor = item.EmailContactoPagoAgricultor;
                    poCab.DireccionNegocio = item.DireccionNegocio;
                    poCab.IdentificacionRepLegal = item.IdentificacionRepLegal;

                    poCab.EstadoRuc = item.EstadoRuc;
                    poCab.FechaInicioActividadRuc = item.FechaInicioActividadRuc;
                    poCab.FechaReinicioActividadRuc = item.FechaReinicioActividadRuc;
                    poCab.FechaUltActualizacionRuc = item.FechaUltActualizacionRuc;
                    poCab.ObligadoLlevarCantidadRuc = item.ObligadoLlevarCantidadRuc;

                    poCab.FechaIngreso = item.FechaIngreso;
                    poCab.FechaCaducidadCedulaRepLegal = item.FechaCaducidadCedulaRepLegal;

                    poCab.OtrasActividades = item.OtrasActividades;
                    poCab.ActividadesSecundarias = item.ActividadesSecundarias;
                    poCab.Comentarios = item.Comentarios;
                    poCab.ComentariosNombramiento = item.ComentariosNombramiento;

                    poCab.ComentariosRefComerciales = item.ComentariosRefComerciales;
                    poCab.ComentariosCertBancarios = item.ComentariosCertBancarios;
                    poCab.ComentariosBienes = item.ComentariosBienes;

                    poCab.AntiguedadTerrenoMes = item.AntiguedadTerrenoMes ?? 0;
                    poCab.TiempoActividadAnteriorMes = item.TiempoActividadAnteriorMes ?? 0;
                    poCab.TiempoActividadMes = item.TiempoActividadMes ?? 0;
                    poCab.TiempoMes = item.TiempoMes ?? 0;
                    poCab.TiempoNegocioMes = item.TiempoNegocioMes ?? 0;
                    poCab.TiempoAgricultorMes = item.TiempoAgricultorMes ?? 0;

                    poCab.ComentarioRevisionCedula = item.ComentarioRevisionCedula;
                    poCab.UsuarioIngreso = item.UsuarioIngreso;

                    if (tIdProcesoCredito == 0)
                    {
                        poCab.ProcesoCredito = new clsNProcesoCredito().goConsultar(item.IdProcesoCredito ?? 0);
                    }

                    if (item.FechaUltimaSolicitud == null)
                    {
                        var pdFechaUltSol = gdtUltimaFechaSolicitud(poCab.CodigoCliente);
                        if (pdFechaUltSol != null)
                        {
                            if (pdFechaUltSol != DateTime.MinValue)
                            {
                                poCab.FechaUltimaSolicitud = pdFechaUltSol;
                            }
                        }
                    }
                    else
                    {
                        poCab.FechaUltimaSolicitud = item.FechaUltimaSolicitud ?? DateTime.Now;
                    }

                    poCab.SolicitudCreditoRefPersonal = new List<SolicitudCreditoRefPersonal>();
                    foreach (var detalle in item.CRETSOLICITUDCREDITOREFPERSONAL.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {
                        var poDet = new SolicitudCreditoRefPersonal();
                        poDet.IdSolicitudCreditoRefPersonal = detalle.IdSolicitudCreditoRefPersonal;
                        poDet.IdSolicitudCredito = detalle.IdSolicitudCredito;
                        poDet.TipoReferenciaPersonal = detalle.TipoReferenciaPersonal;
                        poDet.ReferenciaPersonal = detalle.ReferenciaPersonal;
                        poDet.Nombre = detalle.Nombre;
                        poDet.CodigoRelacion = detalle.CodigoRelacion;
                        poDet.Relacion = detalle.Relacion;
                        poDet.Telefono = detalle.Telefono;
                        poDet.Direccion = detalle.Direccion;
                        poDet.Ciudad = detalle.Ciudad;

                        poCab.SolicitudCreditoRefPersonal.Add(poDet);
                    }

                    poListaReturn.Add(poCab);
                }
            }
            return poListaReturn;
        }

        public DateTime gdtUltimaFechaSolicitud(string tsCodigoCliente)
        {
            var poFecha  = loBaseDa.Find<CRETSOLICITUDCREDITO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoCliente == tsCodigoCliente).Select(x => x.FechaSolicitud).FirstOrDefault();
            if (poFecha == null || poFecha == DateTime.MinValue.Date)
            {
                var dt = goConsultaDataTable(string.Format("SELECT U_anio_sol_cre FROM [SBO_AFECOR].dbo.[OCRD] T0 WHERE T0.CardCode = '{0}'", tsCodigoCliente));
                if (dt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                    {
                        poFecha = new DateTime(int.Parse(dt.Rows[0][0].ToString()), 1, 1);
                    }
                }
            }
            return poFecha;
        }

        public List<SolicitudCredito> goListar(string tsCodigoCliente = "")
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<SolicitudCredito>();
            var poLista = loBaseDa.Find<CRETSOLICITUDCREDITO>().Include(x => x.CRETSOLICITUDCREDITOOTROSACTIVOS).Include(x => x.CRETSOLICITUDCREDITOREFBANCARIA)
                .Include(x => x.CRETSOLICITUDCREDITOREFCOMERCIAL).Include(x => x.CRETSOLICITUDCREDITOREFPERSONAL).Include(x => x.CRETSOLICITUDCREDITOPROPIEDADES)
                .Include(x => x.CRETSOLICITUDCREDITODIRECCION).Where(x => x.CodigoEstado == Diccionario.Activo && (tsCodigoCliente != "" && (tsCodigoCliente == "" || x.CodigoCliente == tsCodigoCliente))).ToList();

            return loLlenarDatos(poLista);
        }

        public List<SolicitudCredito> goListarHistorialImportado()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<SolicitudCredito>();
            var poLista = loBaseDa.Find<CRETSOLICITUDCREDITO>().Include(x => x.CRETSOLICITUDCREDITOOTROSACTIVOS).Include(x => x.CRETSOLICITUDCREDITOREFBANCARIA)
                .Include(x => x.CRETSOLICITUDCREDITOREFCOMERCIAL).Include(x => x.CRETSOLICITUDCREDITOREFPERSONAL).Include(x => x.CRETSOLICITUDCREDITOPROPIEDADES)
                .Include(x => x.CRETSOLICITUDCREDITODIRECCION).Where(x => x.CodigoEstado == Diccionario.Activo && x.IdReferenciaForm != null).ToList();

            return loLlenarDatos(poLista);
        }

        public string gsGuardar(List<SolicitudCredito> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = lsEsValido(toLista);

            if (string.IsNullOrEmpty(psMsg))
            {
                if (toLista != null && toLista.Count > 0)
                {
                    var poListaItem = goSapConsultaItems();
                    var poRelacion = goConsultarComboTipoRelacionPersonal();
                    var poReferenciaPersona = goConsultarComboTipoReferenciaPersonal();

                    foreach (var poItem in toLista)
                    {
                        var poObject = loBaseDa.Get<CRETSOLICITUDCREDITO>().Include(x => x.CRETSOLICITUDCREDITOOTROSACTIVOS).Include(x => x.CRETSOLICITUDCREDITOREFBANCARIA)
                            .Include(x => x.CRETSOLICITUDCREDITOREFCOMERCIAL).Include(x => x.CRETSOLICITUDCREDITOREFPERSONAL).Include(x => x.CRETSOLICITUDCREDITOPROPIEDADES).Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdSolicitudCredito == poItem.IdSolicitudCredito && x.IdSolicitudCredito != 0).FirstOrDefault();
                        if (poObject != null)
                        {
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObject = new CRETSOLICITUDCREDITO();
                            loBaseDa.CreateNewObject(out poObject);
                            poObject.UsuarioIngreso = tsUsuario;
                            poObject.FechaIngreso = DateTime.Now;
                            poObject.TerminalIngreso = tsTerminal;
                        }

                        poObject.CodigoEstado = Diccionario.Activo;
                        poObject.FechaSolicitud = poItem.FechaSolicitud;
                        poObject.CodigoTipoSolicitud = poItem.CodigoTipoSolicitud;
                        poObject.TipoSolicitud = poItem.TipoSolicitud;
                        poObject.CodigoActividadCliente = poItem.CodigoActividadCliente;
                        poObject.ActividadCliente = poItem.ActividadCliente;
                        poObject.IdRTC = poItem.IdRTC;
                        poObject.RTC = poItem.RTC;
                        poObject.Zona = poItem.Zona;
                        poObject.Cupo = poItem.Cupo;
                        poObject.CupoSap = poItem.CupoSap;
                        poObject.Observacion = poItem.Observacion;
                        poObject.CodigoCliente = poItem.CodigoCliente;
                        poObject.Cliente = poItem.Cliente;
                        poObject.Identificacion = poItem.Identificacion;
                        poObject.FechaNacimiento = poItem.FechaNacimiento;
                        poObject.CodigoEstadoCivil = poItem.CodigoEstadoCivil;
                        poObject.Ciudad = poItem.Ciudad;
                        poObject.DireccionDomicilio = poItem.DireccionDomicilio;
                        poObject.Telefono = poItem.Telefono;
                        poObject.Celular = poItem.Celular;
                        poObject.CodigoVivienda = poItem.CodigoVivienda;
                        poObject.Tiempo = poItem.Tiempo;
                        poObject.Email = poItem.Email;
                        poObject.NombreConyuge = poItem.NombreConyuge;
                        poObject.IdentificacionConyuge = poItem.IdentificacionConyuge;
                        poObject.NombreComercial = poItem.NombreComercial;
                        poObject.TiempoActividad = poItem.TiempoActividad;
                        poObject.Ruc = poItem.Ruc;
                        poObject.CodigoTipoNegocio = poItem.CodigoTipoNegocio;
                        poObject.CiudadNegocio = poItem.CiudadNegocio;
                        poObject.TelefonoNegocio = poItem.TelefonoNegocio;
                        poObject.CelularNegocio = poItem.CelularNegocio;
                        poObject.CodigoLocal = poItem.CodigoLocal;
                        poObject.TiempoNegocio = poItem.TiempoNegocio;
                        poObject.ActividadAnterior = poItem.ActividadAnterior;
                        poObject.TiempoActividadAnterior = poItem.TiempoActividadAnterior;
                        poObject.EmpresaActividadAnterior = poItem.EmpresaActividadAnterior;
                        poObject.CargoActividadAnterior = poItem.CargoActividadAnterior;
                        poObject.NombreContactoPago = poItem.NombreContactoPago;
                        poObject.CargoContactoPago = poItem.CargoContactoPago;
                        poObject.TelefonoContactoPago = poItem.TelefonoContactoPago;
                        poObject.CelularContactoPago = poItem.CelularContactoPago;
                        poObject.EmailContactoPago = poItem.EmailContactoPago;
                        poObject.EmailFacturaElectronica = poItem.EmailFacturaElectronica;
                        poObject.OtrosIngresos = poItem.OtrosIngresos;
                        poObject.OrigenOtrosIngresos = poItem.OrigenOtrosIngresos;
                        poObject.EmailRealizaCompras = poItem.EmailRealizaCompras;
                        poObject.EmailRecibeCompras = poItem.EmailRecibeCompras;
                        poObject.TiempoAgricultor = poItem.TiempoAgricultor;
                        poObject.ClaseCultivo = poItem.ClaseCultivo;
                        poObject.NumeroHectariasCultivadas = poItem.NumeroHectariasCultivadas;
                        poObject.CodigoTerreno = poItem.CodigoTerreno;
                        poObject.AntiguedadTerreno = poItem.AntiguedadTerreno;
                        poObject.MesesCosecha = poItem.MesesCosecha;
                        poObject.PromedioIngresos = poItem.PromedioIngresos;
                        poObject.UbicacionTerrenos = poItem.UbicacionTerrenos;
                        poObject.IdReferenciaForm = poItem.IdReferenciaForm;
                        poObject.Completado = poItem.Completado;
                        poObject.IdentificacionRealizaCompras = poItem.IdentificacionRealizaCompras;
                        poObject.IdentificacionEmailRecibeCompras = poItem.IdentificacionEmailRecibeCompras;

                        poObject.NombreContactoPagoAgricultor = poItem.NombreContactoPagoAgricultor;
                        poObject.CargoContactoPagoAgricultor = poItem.CargoContactoPagoAgricultor;
                        poObject.TelefonoContactoPagoAgricultor = poItem.TelefonoContactoPagoAgricultor;
                        poObject.CelularContactoPagoAgricultor = poItem.CelularContactoPagoAgricultor;
                        poObject.EmailContactoPagoAgricultor = poItem.EmailContactoPagoAgricultor;
                        poObject.DireccionNegocio = poItem.DireccionNegocio;
                        poObject.IdentificacionRepLegal = poItem.IdentificacionRepLegal;

                        poObject.AntiguedadTerrenoMes = poItem.AntiguedadTerrenoMes;
                        poObject.TiempoActividadAnteriorMes = poItem.TiempoActividadAnteriorMes;
                        poObject.TiempoActividadMes = poItem.TiempoActividadMes;
                        poObject.TiempoMes = poItem.TiempoMes;
                        poObject.TiempoNegocioMes = poItem.TiempoNegocioMes;
                        poObject.TiempoAgricultorMes = poItem.TiempoAgricultorMes;

                        poObject.FechaUltimaSolicitud = poItem.FechaUltimaSolicitud;

                        /**********************************************************************************************************************************************/
                        /**********************************************************************************************************************************************/

                        var poObjectProCred = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITOOTROSACTIVOS).Include(x => x.CRETPROCESOCREDITOPROPIEDADES).Include(x => x.CRETPROCESOCREDITOCEDULA)
                            .Include(x => x.CRETPROCESOCREDITONOMBRAMIENTO).Include(x => x.CRETPROCESOCREDITORUC.CRETPROCESOCREDITORUCDIRECCION).Include(x => x.CRETPROCESOCREDITOREFBANCARIA)
                            .Include(x => x.CRETPROCESOCREDITOREFCOMERCIAL).Where(x => x.IdProcesoCredito == poItem.IdProcesoCredito).FirstOrDefault();

                        if (poObjectProCred != null)
                        {
                            if (poItem.ProcesoCredito.ProcesoCreditoOtrosActivos != null)
                            {
                                //Eliminar Detalle 
                                var piListaIdPresentacion = poItem.ProcesoCredito.ProcesoCreditoOtrosActivos.Where(x => x.IdProcesoCreditoOtrosActivos != 0).Select(x => x.IdProcesoCreditoOtrosActivos).ToList();

                                foreach (var poItemDel in poObjectProCred.CRETPROCESOCREDITOOTROSACTIVOS.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdProcesoCreditoOtrosActivos)))
                                {
                                    poItemDel.CodigoEstado = Diccionario.Inactivo;
                                    poItemDel.UsuarioModificacion = tsUsuario;
                                    poItemDel.FechaModificacion = DateTime.Now;
                                    poItemDel.TerminalModificacion = tsTerminal;
                                }

                                foreach (var item in poItem.ProcesoCredito.ProcesoCreditoOtrosActivos)
                                {
                                    int pId = item.IdProcesoCreditoOtrosActivos;
                                    var poObjectItem = poObjectProCred.CRETPROCESOCREDITOOTROSACTIVOS.Where(x => x.IdProcesoCreditoOtrosActivos == pId && pId != 0).FirstOrDefault();
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
                                        poObjectProCred.CRETPROCESOCREDITOOTROSACTIVOS.Add(poObjectItem);
                                    }

                                    poObjectItem.CodigoEstado = Diccionario.Activo;
                                    poObjectItem.CodigoTipoOtrosActivos = item.CodigoTipoOtrosActivos;
                                    poObjectItem.Marca = item.Marca;
                                    poObjectItem.Modelo = item.Modelo;
                                    poObjectItem.Anio = item.Anio;
                                    poObjectItem.AvaluoComercial = item.AvaluoComercial;
                                }
                            }

                            if (poItem.ProcesoCredito.ProcesoCreditoRefBancaria != null)
                            {
                                //Eliminar Detalle 
                                var piListaIdPresentacion = poItem.ProcesoCredito.ProcesoCreditoRefBancaria.Where(x => x.IdProcesoCreditoRefBancaria != 0).Select(x => x.IdProcesoCreditoRefBancaria).ToList();

                                foreach (var poItemDel in poObjectProCred.CRETPROCESOCREDITOREFBANCARIA.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdProcesoCreditoRefBancaria)))
                                {
                                    poItemDel.CodigoEstado = Diccionario.Inactivo;
                                    poItemDel.UsuarioModificacion = tsUsuario;
                                    poItemDel.FechaModificacion = DateTime.Now;
                                    poItemDel.TerminalModificacion = tsTerminal;
                                }

                                foreach (var item in poItem.ProcesoCredito.ProcesoCreditoRefBancaria)
                                {
                                    int pId = item.IdProcesoCreditoRefBancaria;
                                    var poObjectItem = poObjectProCred.CRETPROCESOCREDITOREFBANCARIA.Where(x => x.IdProcesoCreditoRefBancaria == pId && pId != 0).FirstOrDefault();
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
                                        poObjectProCred.CRETPROCESOCREDITOREFBANCARIA.Add(poObjectItem);
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

                            if (poItem.ProcesoCredito.ProcesoCreditoRefComercial != null)
                            {
                                //Eliminar Detalle 
                                var piListaIdPresentacion = poItem.ProcesoCredito.ProcesoCreditoRefComercial.Where(x => x.IdProcesoCreditoRefComercial != 0).Select(x => x.IdProcesoCreditoRefComercial).ToList();

                                foreach (var poItemDel in poObjectProCred.CRETPROCESOCREDITOREFCOMERCIAL.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdProcesoCreditoRefComercial)))
                                {
                                    poItemDel.CodigoEstado = Diccionario.Inactivo;
                                    poItemDel.UsuarioModificacion = tsUsuario;
                                    poItemDel.FechaModificacion = DateTime.Now;
                                    poItemDel.TerminalModificacion = tsTerminal;
                                }

                                foreach (var item in poItem.ProcesoCredito.ProcesoCreditoRefComercial)
                                {
                                    int pId = item.IdProcesoCreditoRefComercial;
                                    var poObjectItem = poObjectProCred.CRETPROCESOCREDITOREFCOMERCIAL.Where(x => x.IdProcesoCreditoRefComercial == pId && pId != 0).FirstOrDefault();
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
                                        poObjectProCred.CRETPROCESOCREDITOREFCOMERCIAL.Add(poObjectItem);
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
                                    poObjectItem.CodigoGarantia = item.CodigoGarantia ?? "0";
                                }
                            }

                            if (poItem.ProcesoCredito.ProcesoCreditoPropiedades != null)
                            {
                                //Eliminar Detalle 
                                var piListaIdPresentacion = poItem.ProcesoCredito.ProcesoCreditoPropiedades.Where(x => x.IdProcesoCreditoPropiedades != 0).Select(x => x.IdProcesoCreditoPropiedades).ToList();

                                foreach (var poItemDel in poObjectProCred.CRETPROCESOCREDITOPROPIEDADES.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdProcesoCreditoPropiedades)))
                                {
                                    poItemDel.CodigoEstado = Diccionario.Inactivo;
                                    poItemDel.UsuarioModificacion = tsUsuario;
                                    poItemDel.FechaModificacion = DateTime.Now;
                                    poItemDel.TerminalModificacion = tsTerminal;
                                }

                                foreach (var item in poItem.ProcesoCredito.ProcesoCreditoPropiedades)
                                {
                                    int pId = item.IdProcesoCreditoPropiedades;
                                    var poObjectItem = poObjectProCred.CRETPROCESOCREDITOPROPIEDADES.Where(x => x.IdProcesoCreditoPropiedades == pId && pId != 0).FirstOrDefault();
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
                                        poObjectProCred.CRETPROCESOCREDITOPROPIEDADES.Add(poObjectItem);
                                    }

                                    poObjectItem.CodigoEstado = Diccionario.Activo;
                                    poObjectItem.CodigoTipoBien = item.CodigoTipoBien;
                                    poObjectItem.Direccion = item.Direccion;
                                    poObjectItem.Hipoteca = item.Hipoteca;
                                    poObjectItem.AvaluoComercial = item.AvaluoComercial;
                                }
                            }

                            /**********************************************************/

                            var poObjectCed = poObjectProCred.CRETPROCESOCREDITOCEDULA;
                            if (poObjectCed != null)
                            {
                                poObjectCed.FechaModificacion = DateTime.Now;
                            }
                            else
                            {
                                poObjectCed = new CRETPROCESOCREDITOCEDULA();
                                loBaseDa.CreateNewObject(out poObjectCed);
                                poObjectCed.FechaIngreso = DateTime.Now;
                            }

                            poObjectCed.FechaModificacion = DateTime.Now;
                            poObjectCed.IdentificacionRepLegal = poItem.IdentificacionRepLegal;
                            poObjectCed.FechaNacimiento = poItem.FechaNacimiento;
                            poObjectCed.Edad = poItem.Edad;
                            poObjectCed.CodigoEstadoCivil = poItem.CodigoEstadoCivil;
                            poObjectCed.IdentificacionConyuge = poItem.IdentificacionConyuge;
                            poObjectCed.NombreConyuge = poItem.NombreConyuge;
                            poObjectCed.IdProcesoCredito = poItem.IdProcesoCredito;



                            /**********************************************************/

                            var poObjectRuc = poObjectProCred.CRETPROCESOCREDITORUC;
                            if (poObjectRuc == null)
                            {
                                poObjectRuc = new CRETPROCESOCREDITORUC();
                                loBaseDa.CreateNewObject(out poObjectRuc);
                                poObjectRuc.FechaIngreso = DateTime.Now;
                            }

                            poObjectRuc.Identificacion = poItem.Identificacion;
                            poObjectRuc.NombreComercial = poItem.NombreComercial;
                            poObjectRuc.CodigoTipoNegocio = poItem.CodigoTipoNegocio;
                            poObjectRuc.IdProcesoCredito = poItem.IdProcesoCredito;

                            /**********************************************************************************************************************************************/
                            /**********************************************************************************************************************************************/

                        }


                        if (poItem.IdProcesoCredito != 0 && poItem.Completado == true)
                        {
                            poObject.IdProcesoCredito = poItem.IdProcesoCredito;
                            var poRef = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdProcesoCredito == poItem.IdProcesoCredito).FirstOrDefault();
                            if (poRef != null)
                            {
                                foreach (var item in poRef.CRETPROCESOCREDITODETALLE.Where(x=> !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == 2))
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
                                    item.FechaReferencial = poObject.FechaIngreso;
                                    item.CodigoEstado = Diccionario.Cargado;
                                    item.Comentarios = "";
                                }
                            }

                        }
                        else if (poItem.IdProcesoCredito != 0 && poItem.Completado == false)
                        {
                            poObject.IdProcesoCredito = poItem.IdProcesoCredito;
                            var poRef = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdProcesoCredito == poItem.IdProcesoCredito).FirstOrDefault();
                            if (poRef != null)
                            {
                                foreach (var item in poRef.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == 2))
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
                                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Pendiente);
                                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                                    item.Completado = false;
                                    item.CodigoEstado = Diccionario.Pendiente;
                                    item.FechaReferencial = DateTime.Now;
                                    item.Comentarios = "";
                                }
                            }
                        }
                        
                        if (poItem.SolicitudCreditoRefPersonal != null)
                        {
                            //Eliminar Detalle 
                            var piListaIdPresentacion = poItem.SolicitudCreditoRefPersonal.Where(x => x.IdSolicitudCreditoRefPersonal != 0).Select(x => x.IdSolicitudCreditoRefPersonal).ToList();

                            foreach (var poItemDel in poObject.CRETSOLICITUDCREDITOREFPERSONAL.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdSolicitudCreditoRefPersonal)))
                            {
                                poItemDel.CodigoEstado = Diccionario.Inactivo;
                                poItemDel.UsuarioModificacion = tsUsuario;
                                poItemDel.FechaModificacion = DateTime.Now;
                                poItemDel.TerminalModificacion = tsTerminal;
                            }

                            foreach (var item in poItem.SolicitudCreditoRefPersonal)
                            {
                                int pId = item.IdSolicitudCreditoRefPersonal;
                                var poObjectItem = poObject.CRETSOLICITUDCREDITOREFPERSONAL.Where(x => x.IdSolicitudCreditoRefPersonal == pId && pId != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    poObjectItem.UsuarioModificacion = tsUsuario;
                                    poObjectItem.FechaModificacion = DateTime.Now;
                                    poObjectItem.TerminalModificacion = tsTerminal;
                                }
                                else
                                {
                                    poObjectItem = new CRETSOLICITUDCREDITOREFPERSONAL();
                                    poObjectItem.UsuarioIngreso = tsUsuario;
                                    poObjectItem.FechaIngreso = DateTime.Now;
                                    poObjectItem.TerminalIngreso = tsTerminal;
                                    poObject.CRETSOLICITUDCREDITOREFPERSONAL.Add(poObjectItem);
                                }

                                poObjectItem.CodigoEstado = Diccionario.Activo;
                                poObjectItem.TipoReferenciaPersonal = item.TipoReferenciaPersonal;
                                poObjectItem.ReferenciaPersonal = poReferenciaPersona.Where(x => x.Codigo == item.TipoReferenciaPersonal).Select(x => x.Descripcion).FirstOrDefault();
                                poObjectItem.Nombre = item.Nombre;
                                poObjectItem.CodigoRelacion = "";// item.CodigoRelacion;
                                poObjectItem.Relacion = "";// poRelacion.Where(x=>x.Codigo == item.CodigoRelacion).Select(x=>x.Descripcion).FirstOrDefault();
                                poObjectItem.Telefono = item.Telefono;
                                poObjectItem.Direccion = item.Direccion;
                                poObjectItem.Ciudad = item.Ciudad;

                            }
                        }

                        
                        /*
                        if (poItem.SolicitudCreditoDireccion != null)
                        {
                            //Eliminar Detalle 
                            var piListaIdPresentacion = poItem.SolicitudCreditoDireccion.Where(x => x.IdSolicitudCreditoDireccion != 0).Select(x => x.IdSolicitudCreditoDireccion).ToList();

                            foreach (var poItemDel in poObject.CRETSOLICITUDCREDITODIRECCION.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdSolicitudCreditoDireccion)))
                            {
                                poItemDel.CodigoEstado = Diccionario.Inactivo;
                                poItemDel.UsuarioModificacion = tsUsuario;
                                poItemDel.FechaModificacion = DateTime.Now;
                                poItemDel.TerminalModificacion = tsTerminal;
                            }

                            foreach (var item in poItem.SolicitudCreditoDireccion)
                            {
                                int pId = item.IdSolicitudCreditoDireccion;
                                var poObjectItem = poObject.CRETSOLICITUDCREDITODIRECCION.Where(x => x.IdSolicitudCreditoDireccion == pId && pId != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    poObjectItem.UsuarioModificacion = tsUsuario;
                                    poObjectItem.FechaModificacion = DateTime.Now;
                                    poObjectItem.TerminalModificacion = tsTerminal;
                                }
                                else
                                {
                                    poObjectItem = new CRETSOLICITUDCREDITODIRECCION();
                                    poObjectItem.UsuarioIngreso = tsUsuario;
                                    poObjectItem.FechaIngreso = DateTime.Now;
                                    poObjectItem.TerminalIngreso = tsTerminal;
                                    poObject.CRETSOLICITUDCREDITODIRECCION.Add(poObjectItem);
                                }
                                poObjectItem.CodigoEstado = Diccionario.Activo;
                                poObjectItem.Principal = item.Principal;
                                poObjectItem.Direccion = item.Direccion;
                            }
                        }
                        */
                    }


                    using (var poTran = new TransactionScope())
                    {
                        loBaseDa.SaveChanges();

                        foreach (var item in toLista)
                        {
                            foreach (var poItem in item.ProcesoCredito.ProcesoCreditoRefBancaria)
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

                            foreach (var poItem in item.ProcesoCredito.ProcesoCreditoRefComercial)
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
                        }

                        poTran.Complete();
                    }
                }
                return psMsg;
            }

            return psMsg;
        }

        public string gEliminar(int tId, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";

            var poObject = loBaseDa.Get<CRETSOLICITUDCREDITO>().Include(x => x.CRETSOLICITUDCREDITOOTROSACTIVOS).Include(x => x.CRETSOLICITUDCREDITOREFBANCARIA)
                            .Include(x => x.CRETSOLICITUDCREDITOREFCOMERCIAL).Include(x => x.CRETSOLICITUDCREDITOREFPERSONAL).Include(x => x.CRETSOLICITUDCREDITOPROPIEDADES).Where(x => x.IdSolicitudCredito == tId).FirstOrDefault();

            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.FechaModificacion = DateTime.Now;
                poObject.TerminalModificacion = tsTerminal;

                foreach (var item in poObject.CRETSOLICITUDCREDITOOTROSACTIVOS.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Eliminado;
                    item.UsuarioModificacion = tsUsuario;
                    item.FechaModificacion = DateTime.Now;
                    item.TerminalModificacion = tsTerminal;
                }

                foreach (var item in poObject.CRETSOLICITUDCREDITOREFBANCARIA.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Eliminado;
                    item.UsuarioModificacion = tsUsuario;
                    item.FechaModificacion = DateTime.Now;
                    item.TerminalModificacion = tsTerminal;
                }

                foreach (var item in poObject.CRETSOLICITUDCREDITOREFCOMERCIAL.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Eliminado;
                    item.UsuarioModificacion = tsUsuario;
                    item.FechaModificacion = DateTime.Now;
                    item.TerminalModificacion = tsTerminal;
                }

                foreach (var item in poObject.CRETSOLICITUDCREDITOREFPERSONAL.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Eliminado;
                    item.UsuarioModificacion = tsUsuario;
                    item.FechaModificacion = DateTime.Now;
                    item.TerminalModificacion = tsTerminal;
                }

                foreach (var item in poObject.CRETSOLICITUDCREDITOPROPIEDADES.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Eliminado;
                    item.UsuarioModificacion = tsUsuario;
                    item.FechaModificacion = DateTime.Now;
                    item.TerminalModificacion = tsTerminal;
                }

                if (string.IsNullOrEmpty(psMsg))
                {
                    loBaseDa.SaveChanges();
                }

            }

            return psMsg;
        }

        private string lsEsValido(List<SolicitudCredito> toObject)
        {
            string psMsg = string.Empty;

            foreach (var item in toObject)
            {
                if (item.CodigoTipoSolicitud == Diccionario.Seleccione)
                {
                    psMsg = psMsg + "Falta seleccionar Tipo Solicitud. \n";
                }

                if (item.CodigoActividadCliente == Diccionario.Seleccione)
                {
                    psMsg = psMsg + "Falta seleccionar Actividad de Cliente. \n";
                }

                if (item.IdRTC == 0)
                {
                    psMsg = psMsg + "Falta seleccionar RTC. \n";
                }

                //var poListaMuestra = new List<int>();
                //int num = 0;
                //foreach (var fila in item.SolicitudCreditoOtrosActivos)
                //{
                //    num++;
                //    if (item.SolicitudCreditoDetalle.Where(x => x.Muestra == num).Count() == 0)
                //    {
                //        psMsg = psMsg + "# Muestra '" + num + "' no existe, corregir debe existir una secuencia \n";
                //    }
                //}
            }

            return psMsg;
        }

        //public string gsGetPresentacion(string tsCodeItem)
        //{
        //    return loBaseDa.DataTable("SELECT ISNULL(SalUnitMsr,'') FROM SBO_AFECOR.DBO.OITM (NOLOCK) WHERE ItemCode = '" + tsCodeItem + "'").Rows[0][0].ToString();
        //}

        /// <summary>
        /// Buscar Codigo de la Entidad
        /// </summary>
        /// <param name="tsTipo">P: Primero, A: Anterior, S: Siguiente, U: Último</param>
        /// <param name="tsCodigo">Codigo de la entidad</param>
        /// <returns></returns>
        public string goBuscarCodigo(string tsTipo, string tsCodigo = "")
        {
            string psCodigo = string.Empty;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<CRETSOLICITUDCREDITO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdSolicitudCredito }).OrderBy(x => x.IdSolicitudCredito).FirstOrDefault()?.IdSolicitudCredito.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<CRETSOLICITUDCREDITO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdSolicitudCredito }).OrderByDescending(x => x.IdSolicitudCredito).FirstOrDefault()?.IdSolicitudCredito.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<CRETSOLICITUDCREDITO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdSolicitudCredito }).ToList().Where(x => x.IdSolicitudCredito < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdSolicitudCredito).FirstOrDefault().IdSolicitudCredito.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<CRETSOLICITUDCREDITO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdSolicitudCredito }).ToList().Where(x => x.IdSolicitudCredito > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdSolicitudCredito).FirstOrDefault().IdSolicitudCredito.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            return psCodigo;

        }

        public int gIdSolicitudCredito(int tIdProcesoCredito)
        {
            return loBaseDa.Find<CRETSOLICITUDCREDITO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdProcesoCredito == tIdProcesoCredito).Select(x => x.IdSolicitudCredito).FirstOrDefault();
        }

        public string gsGuardarRevisionCedula(SolicitudCredito toObject, string tsTipo, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            psMsg = "";

            if (string.IsNullOrEmpty(psMsg))
            {

                var poObject = loBaseDa.Get<CRETSOLICITUDCREDITO>().Where(x => x.IdSolicitudCredito == toObject.IdSolicitudCredito && x.IdSolicitudCredito != 0).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObject = new CRETSOLICITUDCREDITO();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                }

                poObject.FechaCaducidadCedulaRepLegal = toObject.FechaCaducidadCedulaRepLegal;
                poObject.Edad = toObject.Edad;
                poObject.ComentarioRevisionCedula = toObject.ComentarioRevisionCedula;

                if (tsTipo == "REQ" && poObject.IdProcesoCredito != 0)
                {
                    poObject.IdProcesoCredito = poObject.IdProcesoCredito;
                    var poRef = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == poObject.IdProcesoCredito).FirstOrDefault();
                    if (poRef != null)
                    {
                        foreach (var item in poRef.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == 3))
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

        public bool gbHabilitarUltimaFechaSolicitud(string tsNumero, string tsCodeCliente)
        {
            var pbresult = false;

            var psListaEstado = new List<string>();
            psListaEstado.Add(Diccionario.Inactivo);
            psListaEstado.Add(Diccionario.Eliminado);

            var piCont = 0; 
            
            if (!string.IsNullOrEmpty(tsNumero))
            {
                int piNum = int.Parse(tsNumero);
                piCont = loBaseDa.Find<CRETSOLICITUDCREDITO>().Where(x => !psListaEstado.Contains(x.CodigoEstado) && x.CodigoCliente == tsCodeCliente && x.IdSolicitudCredito != piNum).Count();
            }
            else
            {
                piCont = loBaseDa.Find<CRETSOLICITUDCREDITO>().Where(x => !psListaEstado.Contains(x.CodigoEstado) && x.CodigoCliente == tsCodeCliente).Count();
            }

            if (piCont == 0)
            {
                pbresult = true;
            }

            return pbresult;
        }

    }
}
