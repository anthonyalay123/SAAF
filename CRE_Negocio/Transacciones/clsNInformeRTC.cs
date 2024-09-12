using GEN_Entidad;
using GEN_Entidad.Entidades.Credito;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static GEN_Entidad.Diccionario;

namespace CRE_Negocio.Transacciones
{
    public class clsNInformeRTC : clsNBase
    {
        public InformeRtc goConsultar(int tId)
        {
            loBaseDa.CreateContext();
            var poLista = loBaseDa.Find<CRETINFORMERTC>().Include(x => x.CRETINFORMERTCCULTIVOS).Include(x=>x.CRETINFORMERTCCLIENTES).Include(x => x.CRETINFORMERTCCROQUIS)
                .Include(x => x.CRETINFORMERTCDOCUMENTOSPENDIENTES).Include(x => x.CRETINFORMERTCPRODUCTOS).Include(x => x.CRETINFORMERTCPROVEEDOR)
                .Where(x => x.IdInformeRTC == tId).ToList();
            return loLlenarDatos(poLista).FirstOrDefault();

        }

        private List<InformeRtc> loLlenarDatos(List<CRETINFORMERTC> poLista)
        {
            var poListaReturn = new List<InformeRtc>();

            foreach (var item in poLista)
            {
                var poCab = new InformeRtc();
                poCab.IdInformeRTC = item.IdInformeRTC;
                poCab.CodigoEstado = item.CodigoEstado;
                poCab.CodigoTipoProceso = item.CodigoTipoProceso;
                poCab.TipoProceso = item.TipoProceso;
                poCab.CodigoActividadCliente = item.CodigoActividadCliente;
                poCab.ActividadCliente = item.ActividadCliente;
                poCab.IdRTC = item.IdRTC;
                poCab.RTC = item.RTC;
                poCab.Zona = item.Zona;
                poCab.CodigoCliente = item.CodigoCliente;
                poCab.Cliente = item.Cliente;
                poCab.RepresentanteLegal = item.RepresentanteLegal;
                poCab.CodigoTipoPersona = item.CodigoTipoPersona;
                poCab.CodigoSector = item.CodigoSector;
                poCab.FechaInforme = item.FechaInforme;
                poCab.Ciudad = item.Ciudad;
                poCab.TiempoEnZonaActividad = item.TiempoEnZonaActividad;
                poCab.ZonasCubrenVentas = item.ZonasCubrenVentas;
                poCab.TipoCultivo = item.TipoCultivo;
                poCab.CantidadHectareasCubreVentas = item.CantidadHectareasCubreVentas;
                poCab.CultivosSiembra = item.CultivosSiembra;
                poCab.CantidadHectareasSembradasCultivo = item.CantidadHectareasSembradasCultivo;
                poCab.CostoHectarea = item.CostoHectarea;
                poCab.PotencialVenta = item.PotencialVenta;
                poCab.CantidadHectareasPropias = item.CantidadHectareasPropias;
                poCab.CantidadHectareasAlquiladas = item.CantidadHectareasAlquiladas;
                poCab.TieneCodigoSap = item.TieneCodigoSap;
                poCab.GrupoSap = item.GrupoSap;
                poCab.TotalHectareas = item.TotalHectareas;
                poCab.TieneOtrasActividadesComerciales = item.TieneOtrasActividadesComerciales;
                poCab.MontoAnualesVenta = item.MontoAnualesVenta;
                poCab.TieneCuencaCorriente = item.TieneCuencaCorriente;
                poCab.CodigoBanco = item.CodigoBanco;
                poCab.Banco = item.Banco;
                poCab.DetalleFormaPago = item.DetalleFormaPago;
                poCab.TrabajadoAntesCliente = item.TrabajadoAntesCliente;
                poCab.Donde = item.Donde;
                poCab.TiempoConocerlo = item.TiempoConocerlo;
                poCab.ComportamientoPago = item.ComportamientoPago;
                poCab.TieneHistorialAfecor = item.TieneHistorialAfecor;
                poCab.CupoSolicitado = item.CupoSolicitado;
                poCab.VentaEstimadaAnual = item.VentaEstimadaAnual;
                poCab.PlazoSolicitado = item.PlazoSolicitado;
                poCab.Mes1 = item.Mes1;
                poCab.Mes2 = item.Mes2;
                poCab.Mes3 = item.Mes3;
                poCab.Mes4 = item.Mes4;
                poCab.Mes5 = item.Mes5;
                poCab.Mes6 = item.Mes6;
                poCab.Mes7 = item.Mes7;
                poCab.Mes8 = item.Mes8;
                poCab.Mes9 = item.Mes9;
                poCab.Mes10 = item.Mes10;
                poCab.Mes11 = item.Mes11;
                poCab.Mes12 = item.Mes12;
                poCab.VentasSri = item.VentasSri;
                poCab.ComprasSri = item.ComprasSri;
                poCab.FacturacionAfecor = item.FacturacionAfecor;
                poCab.PorcAfecorSobreCompras = item.PorcAfecorSobreCompras;
                poCab.PorcAfecorSobreComprasProyectadas = item.PorcAfecorSobreComprasProyectadas;
                poCab.MontoCrecimientoAfecor = item.MontoCrecimientoAfecor;
                poCab.PorcCrecimientoAfecor = item.PorcCrecimientoAfecor;
                poCab.IdReferenciaForm = item.IdReferenciaForm;
                poCab.Completado = item.Completado;
                poCab.IdProcesoCredito = item.IdProcesoCredito;
                poCab.Direccion1Almacen = item.Direccion1Almacen;
                poCab.Direccion1Titular = item.Direccion1Titular;
                poCab.Direccion2Almacen = item.Direccion2Almacen;
                poCab.Direccion2Titular = item.Direccion2Titular;
                poCab.Argumentos = item.Argumentos;
                poCab.MontoHistorialCompras = item.MontoHistorialCompras??0M;
                poCab.TiempoConocerloMes = item.TiempoConocerloMes??0;
                poCab.TiempoEnZonaActividadMes = item.TiempoEnZonaActividadMes??0;
                poCab.Usuario = item.UsuarioIngreso;

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
                    poCab.FechaUltimaSolicitud = item.FechaUltimaSolicitud;
                }
                

                poCab.InformeRtcCultivos = new List<InformeRtcCultivos>();
                foreach (var detalle in item.CRETINFORMERTCCULTIVOS.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new InformeRtcCultivos();

                    poDet.IdInformeRTCUbiCultivos = detalle.IdInformeRTCUbiCultivos;
                    poDet.IdInformeRTC = detalle.IdInformeRTC;
                    poDet.Ubicacion = detalle.Ubicacion;
                    poDet.CodigoEstado = detalle.CodigoEstado;
                    
                    poCab.InformeRtcCultivos.Add(poDet);
                }

                poCab.InformeRtcClientes = new List<InformeRtcClientes>();
                foreach (var detalle in item.CRETINFORMERTCCLIENTES.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new InformeRtcClientes();

                    poDet.IdInformeRTCCliente = detalle.IdInformeRTCCliente;
                    poDet.IdInformeRTC = detalle.IdInformeRTC;
                    poDet.Cliente = detalle.Cliente;
                    poDet.CodigoEstado = detalle.CodigoEstado;

                    poCab.InformeRtcClientes.Add(poDet);
                }

                poCab.InformeRtcCroquis = new List<InformeRtcCroquis>();
                foreach (var detalle in item.CRETINFORMERTCCROQUIS.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new InformeRtcCroquis();

                    poDet.IdInformeRTCCroquis = detalle.IdInformeRTCCroquis;
                    poDet.IdInformeRTC = detalle.IdInformeRTC;
                    poDet.ArchivoAdjunto = detalle.ArchivoAdjunto;
                    poDet.Coordenadas = detalle.Coordenadas;
                    poDet.Lugar = detalle.Lugar;
                    poDet.NombreOriginal = detalle.NombreOriginal;
                    poDet.Referencia = detalle.Referencia;
                    poDet.CodigoEstado = detalle.CodigoEstado;
                    poDet.RutaDestino = ConfigurationManager.AppSettings["CarpetaInfRtc"].ToString();

                    poCab.InformeRtcCroquis.Add(poDet);
                }

                poCab.InformeRtcDocumentosPendientes = new List<InformeRtcDocumentosPendientes>();
                foreach (var detalle in item.CRETINFORMERTCDOCUMENTOSPENDIENTES.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new InformeRtcDocumentosPendientes();

                    poDet.IdInformeRTCDocumentosPendientes = detalle.IdInformeRTCDocumentosPendientes;
                    poDet.IdInformeRTC = detalle.IdInformeRTC;
                    poDet.CheckList = detalle.CheckList;
                    poDet.IdCheckList = detalle.IdCheckList;
                    poDet.Compromisos = detalle.Compromisos;
                    poDet.FechaEntrega = detalle.FechaEntrega;
                    poDet.CodigoEstado = detalle.CodigoEstado;

                    poCab.InformeRtcDocumentosPendientes.Add(poDet);
                }

                poCab.InformeRtcProductos = new List<InformeRtcProductos>();
                foreach (var detalle in item.CRETINFORMERTCPRODUCTOS.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new InformeRtcProductos();

                    poDet.IdInformeRTCProducto = detalle.IdInformeRTCProducto;
                    poDet.IdInformeRTC = detalle.IdInformeRTC;
                    poDet.ItemCode = detalle.ItemCode;
                    poDet.ItemName = detalle.ItemName;
                    poDet.ItmsGrpCod = detalle.ItmsGrpCod;
                    poDet.ItmsGrpNam = detalle.ItmsGrpNam;
                    poDet.CodigoEstado = detalle.CodigoEstado;

                    poCab.InformeRtcProductos.Add(poDet);
                }

                poCab.InformeRtcProveedor = new List<InformeRtcProveedor>();
                foreach (var detalle in item.CRETINFORMERTCPROVEEDOR.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new InformeRtcProveedor();

                    poDet.IdInformeRTCProveedor = detalle.IdInformeRTCProveedor;
                    poDet.IdInformeRTC = detalle.IdInformeRTC;
                    poDet.MontoEstimadoVenta = detalle.MontoEstimadoVenta;
                    poDet.Proveedor = detalle.Proveedor;
                    poDet.CodigoEstado = detalle.CodigoEstado;

                    poCab.InformeRtcProveedor.Add(poDet);
                }

                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public List<InformeRtc> goListar(string tsCodigoCliente = "")
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<SolicitudCredito>();
            var poLista = loBaseDa.Find<CRETINFORMERTC>().Include(x => x.CRETINFORMERTCCULTIVOS).Include(x => x.CRETINFORMERTCCLIENTES).Include(x => x.CRETINFORMERTCCROQUIS)
                .Include(x => x.CRETINFORMERTCDOCUMENTOSPENDIENTES).Include(x => x.CRETINFORMERTCPRODUCTOS).Include(x => x.CRETINFORMERTCPROVEEDOR)
                .Where(x => x.CodigoEstado == Diccionario.Activo && (tsCodigoCliente != "" && (tsCodigoCliente == "" || x.CodigoCliente == tsCodigoCliente))).ToList();

            return loLlenarDatos(poLista);
        }

        public DateTime gdtUltimaFechaSolicitud(string tsCodigoCliente)
        {
            var poFecha = loBaseDa.Find<CRETINFORMERTC>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoCliente == tsCodigoCliente).Select(x => x.FechaInforme).FirstOrDefault();
            if (poFecha == null || poFecha == DateTime.MinValue.Date)
            {
                var dt = goConsultaDataTable(string.Format("SELECT U_FEC_INFORME_VENDEDOR FROM [SBO_AFECOR].dbo.[OCRD] T0 WHERE T0.CardCode = '{0}'", tsCodigoCliente));
                if (dt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                    {
                        poFecha = Convert.ToDateTime(dt.Rows[0][0].ToString());
                    }
                    
                }
            }
            return poFecha;
        }

        public List<InformeRtc> goListarHistorialImportado()
        {
            loBaseDa.CreateContext();
            var poLista = loBaseDa.Find<CRETINFORMERTC>().Include(x => x.CRETINFORMERTCCULTIVOS).Include(x => x.CRETINFORMERTCCLIENTES).Include(x => x.CRETINFORMERTCCROQUIS)
                .Include(x => x.CRETINFORMERTCDOCUMENTOSPENDIENTES).Include(x => x.CRETINFORMERTCPRODUCTOS).Include(x => x.CRETINFORMERTCPROVEEDOR)
                .Where(x => x.CodigoEstado == Diccionario.Activo && x.IdReferenciaForm != null).ToList();

            return loLlenarDatos(poLista);
        }

        public string gsGuardar(List<InformeRtc> toLista, string tsUsuario, string tsTerminal)
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
                        var poObject = loBaseDa.Get<CRETINFORMERTC>().Include(x => x.CRETINFORMERTCCULTIVOS).Where(x => x.IdInformeRTC == poItem.IdInformeRTC && x.IdInformeRTC != 0).FirstOrDefault();
                        if (poObject != null)
                        {
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObject = new CRETINFORMERTC();
                            loBaseDa.CreateNewObject(out poObject);
                            poObject.UsuarioIngreso = tsUsuario;
                            poObject.FechaIngreso = DateTime.Now;
                            poObject.TerminalIngreso = tsTerminal;
                        }

                        poObject.CodigoEstado = Diccionario.Activo;
                        poObject.CodigoTipoProceso = poItem.CodigoTipoProceso;
                        poObject.TipoProceso = poItem.TipoProceso;
                        poObject.CodigoActividadCliente = poItem.CodigoActividadCliente;
                        poObject.ActividadCliente = poItem.ActividadCliente;
                        poObject.IdRTC = poItem.IdRTC;
                        poObject.RTC = poItem.RTC;
                        poObject.Zona = poItem.Zona;
                        poObject.CodigoCliente = poItem.CodigoCliente;
                        poObject.Cliente = poItem.Cliente;
                        poObject.RepresentanteLegal = poItem.RepresentanteLegal;
                        poObject.CodigoTipoPersona = poItem.CodigoTipoPersona;
                        poObject.CodigoSector = poItem.CodigoSector;
                        poObject.FechaInforme = poItem.FechaInforme;
                        poObject.Ciudad = poItem.Ciudad;
                        poObject.TiempoEnZonaActividad = poItem.TiempoEnZonaActividad;
                        poObject.ZonasCubrenVentas = poItem.ZonasCubrenVentas;
                        poObject.TipoCultivo = poItem.TipoCultivo;
                        poObject.CantidadHectareasCubreVentas = poItem.CantidadHectareasCubreVentas;
                        poObject.CultivosSiembra = poItem.CultivosSiembra;
                        poObject.CantidadHectareasSembradasCultivo = poItem.CantidadHectareasSembradasCultivo;
                        poObject.CostoHectarea = poItem.CostoHectarea;
                        poObject.PotencialVenta = poItem.PotencialVenta;
                        poObject.CantidadHectareasPropias = poItem.CantidadHectareasPropias;
                        poObject.CantidadHectareasAlquiladas = poItem.CantidadHectareasAlquiladas;
                        poObject.TieneCodigoSap = poItem.TieneCodigoSap;
                        poObject.GrupoSap = poItem.GrupoSap;
                        poObject.TotalHectareas = poItem.TotalHectareas;
                        poObject.TieneOtrasActividadesComerciales = poItem.TieneOtrasActividadesComerciales;
                        poObject.MontoAnualesVenta = poItem.MontoAnualesVenta;
                        poObject.TieneCuencaCorriente = poItem.TieneCuencaCorriente;
                        poObject.CodigoBanco = poItem.CodigoBanco;
                        poObject.Banco = poItem.Banco;
                        poObject.DetalleFormaPago = poItem.DetalleFormaPago;
                        poObject.TrabajadoAntesCliente = poItem.TrabajadoAntesCliente;
                        poObject.Donde = poItem.Donde;
                        poObject.TiempoConocerlo = poItem.TiempoConocerlo;
                        poObject.ComportamientoPago = poItem.ComportamientoPago;
                        poObject.TieneHistorialAfecor = poItem.TieneHistorialAfecor;
                        poObject.CupoSolicitado = poItem.CupoSolicitado;
                        poObject.VentaEstimadaAnual = poItem.VentaEstimadaAnual;
                        poObject.PlazoSolicitado = poItem.PlazoSolicitado;
                        poObject.Mes1 = poItem.Mes1;
                        poObject.Mes2 = poItem.Mes2;
                        poObject.Mes3 = poItem.Mes3;
                        poObject.Mes4 = poItem.Mes4;
                        poObject.Mes5 = poItem.Mes5;
                        poObject.Mes6 = poItem.Mes6;
                        poObject.Mes7 = poItem.Mes7;
                        poObject.Mes8 = poItem.Mes8;
                        poObject.Mes9 = poItem.Mes9;
                        poObject.Mes10 = poItem.Mes10;
                        poObject.Mes11 = poItem.Mes11;
                        poObject.Mes12 = poItem.Mes12;
                        poObject.VentasSri = poItem.VentasSri;
                        poObject.ComprasSri = poItem.ComprasSri;
                        poObject.FacturacionAfecor = poItem.FacturacionAfecor;
                        poObject.PorcAfecorSobreCompras = poItem.PorcAfecorSobreCompras;
                        poObject.PorcAfecorSobreComprasProyectadas = poItem.PorcAfecorSobreComprasProyectadas;
                        poObject.MontoCrecimientoAfecor = poItem.MontoCrecimientoAfecor;
                        poObject.PorcCrecimientoAfecor = poItem.PorcCrecimientoAfecor;
                        poObject.IdReferenciaForm = poItem.IdReferenciaForm;
                        poObject.Completado = poItem.Completado;
                        poObject.IdProcesoCredito = poItem.IdProcesoCredito;
                        poObject.Direccion1Almacen = poItem.Direccion1Almacen;
                        poObject.Direccion1Titular = poItem.Direccion1Titular;
                        poObject.Direccion2Almacen = poItem.Direccion2Almacen;
                        poObject.Direccion2Titular = poItem.Direccion2Titular;
                        poObject.Argumentos = poItem.Argumentos;
                        poObject.MontoHistorialCompras = poItem.MontoHistorialCompras;
                        poObject.TiempoConocerloMes = poItem.TiempoConocerloMes;
                        poObject.TiempoEnZonaActividadMes = poItem.TiempoEnZonaActividadMes;
                        poObject.FechaUltimaSolicitud = poItem.FechaUltimaSolicitud;

                        // Actualiza Datos 
                        gActualizaRequerimiento(poItem.IdProcesoCredito??0, poItem.RepresentanteLegal, poItem.PlazoSolicitado, poItem.CupoSolicitado);
                        
                        if (poItem.IdProcesoCredito != 0 && poItem.Completado == true)
                        {
                            poObject.IdProcesoCredito = poItem.IdProcesoCredito;
                            var poRef = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == poItem.IdProcesoCredito).FirstOrDefault();
                            if (poRef != null)
                            {
                                foreach (var item in poRef.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == 12))
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
                            var poRef = loBaseDa.Get<CRETPROCESOCREDITO>().Include(x => x.CRETPROCESOCREDITODETALLE).Where(x => x.IdProcesoCredito == poItem.IdProcesoCredito).FirstOrDefault();
                            if (poRef != null)
                            {
                                foreach (var item in poRef.CRETPROCESOCREDITODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado) && x.IdCheckList == 12))
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



                        if (poItem.InformeRtcCultivos != null)
                        {
                            //Eliminar Detalle 
                            var piListaIdPresentacion = poItem.InformeRtcCultivos.Where(x => x.IdInformeRTCUbiCultivos != 0).Select(x => x.IdInformeRTCUbiCultivos).ToList();

                            foreach (var poItemDel in poObject.CRETINFORMERTCCULTIVOS.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdInformeRTCUbiCultivos)))
                            {
                                poItemDel.CodigoEstado = Diccionario.Inactivo;
                                poItemDel.UsuarioModificacion = tsUsuario;
                                poItemDel.FechaModificacion = DateTime.Now;
                                poItemDel.TerminalModificacion = tsTerminal;
                            }

                            foreach (var item in poItem.InformeRtcCultivos)
                            {
                                int pId = item.IdInformeRTCUbiCultivos;
                                var poObjectItem = poObject.CRETINFORMERTCCULTIVOS.Where(x => x.IdInformeRTCUbiCultivos == pId && pId != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    poObjectItem.UsuarioModificacion = tsUsuario;
                                    poObjectItem.FechaModificacion = DateTime.Now;
                                    poObjectItem.TerminalModificacion = tsTerminal;
                                }
                                else
                                {
                                    poObjectItem = new CRETINFORMERTCCULTIVOS();
                                    poObjectItem.UsuarioIngreso = tsUsuario;
                                    poObjectItem.FechaIngreso = DateTime.Now;
                                    poObjectItem.TerminalIngreso = tsTerminal;
                                    poObject.CRETINFORMERTCCULTIVOS.Add(poObjectItem);
                                }

                                poObjectItem.CodigoEstado = Diccionario.Activo;
                                poObjectItem.Ubicacion = item.Ubicacion;
                            }
                        }

                        if (poItem.InformeRtcClientes != null)
                        {
                            //Eliminar Detalle 
                            var piListaIdPresentacion = poItem.InformeRtcClientes.Where(x => x.IdInformeRTCCliente != 0).Select(x => x.IdInformeRTCCliente).ToList();

                            foreach (var poItemDel in poObject.CRETINFORMERTCCLIENTES.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdInformeRTCCliente)))
                            {
                                poItemDel.CodigoEstado = Diccionario.Inactivo;
                                poItemDel.UsuarioModificacion = tsUsuario;
                                poItemDel.FechaModificacion = DateTime.Now;
                                poItemDel.TerminalModificacion = tsTerminal;
                            }

                            foreach (var item in poItem.InformeRtcClientes)
                            {
                                int pId = item.IdInformeRTCCliente;
                                var poObjectItem = poObject.CRETINFORMERTCCLIENTES.Where(x => x.IdInformeRTCCliente == pId && pId != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    poObjectItem.UsuarioModificacion = tsUsuario;
                                    poObjectItem.FechaModificacion = DateTime.Now;
                                    poObjectItem.TerminalModificacion = tsTerminal;
                                }
                                else
                                {
                                    poObjectItem = new CRETINFORMERTCCLIENTES();
                                    poObjectItem.UsuarioIngreso = tsUsuario;
                                    poObjectItem.FechaIngreso = DateTime.Now;
                                    poObjectItem.TerminalIngreso = tsTerminal;
                                    poObject.CRETINFORMERTCCLIENTES.Add(poObjectItem);
                                }

                                poObjectItem.CodigoEstado = Diccionario.Activo;
                                poObjectItem.Cliente = item.Cliente;
                            }
                        }

                        if (poItem.InformeRtcCroquis != null)
                        {
                            //Eliminar Detalle 
                            var piListaIdPresentacion = poItem.InformeRtcCroquis.Where(x => x.IdInformeRTCCroquis != 0).Select(x => x.IdInformeRTCCroquis).ToList();

                            foreach (var poItemDel in poObject.CRETINFORMERTCCROQUIS.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdInformeRTCCroquis)))
                            {
                                poItemDel.CodigoEstado = Diccionario.Inactivo;
                                poItemDel.UsuarioModificacion = tsUsuario;
                                poItemDel.FechaModificacion = DateTime.Now;
                                poItemDel.TerminalModificacion = tsTerminal;
                            }

                            foreach (var item in poItem.InformeRtcCroquis)
                            {
                                int pId = item.IdInformeRTCCroquis;
                                var poObjectItem = poObject.CRETINFORMERTCCROQUIS.Where(x => x.IdInformeRTCCroquis == pId && pId != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    poObjectItem.UsuarioModificacion = tsUsuario;
                                    poObjectItem.FechaModificacion = DateTime.Now;
                                    poObjectItem.TerminalModificacion = tsTerminal;
                                }
                                else
                                {
                                    poObjectItem = new CRETINFORMERTCCROQUIS();
                                    poObjectItem.UsuarioIngreso = tsUsuario;
                                    poObjectItem.FechaIngreso = DateTime.Now;
                                    poObjectItem.TerminalIngreso = tsTerminal;
                                    poObject.CRETINFORMERTCCROQUIS.Add(poObjectItem);
                                }

                                poObjectItem.CodigoEstado = Diccionario.Activo;
                                poObjectItem.ArchivoAdjunto = item.ArchivoAdjunto;
                                poObjectItem.Coordenadas = item.Coordenadas;
                                poObjectItem.Lugar = item.Lugar;
                                poObjectItem.NombreOriginal = item.NombreOriginal;
                                poObjectItem.Referencia = item.Referencia;

                            }
                        }

                        if (poItem.InformeRtcDocumentosPendientes != null)
                        {
                            var poCombo = goConsultarComboCheckList();
                            //Eliminar Detalle 
                            var piListaIdPresentacion = poItem.InformeRtcDocumentosPendientes.Where(x => x.IdInformeRTCDocumentosPendientes != 0).Select(x => x.IdInformeRTCDocumentosPendientes).ToList();

                            foreach (var poItemDel in poObject.CRETINFORMERTCDOCUMENTOSPENDIENTES.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdInformeRTCDocumentosPendientes)))
                            {
                                poItemDel.CodigoEstado = Diccionario.Inactivo;
                                poItemDel.UsuarioModificacion = tsUsuario;
                                poItemDel.FechaModificacion = DateTime.Now;
                                poItemDel.TerminalModificacion = tsTerminal;
                            }

                            foreach (var item in poItem.InformeRtcDocumentosPendientes)
                            {
                                int pId = item.IdInformeRTCDocumentosPendientes;
                                var poObjectItem = poObject.CRETINFORMERTCDOCUMENTOSPENDIENTES.Where(x => x.IdInformeRTCDocumentosPendientes == pId && pId != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    poObjectItem.UsuarioModificacion = tsUsuario;
                                    poObjectItem.FechaModificacion = DateTime.Now;
                                    poObjectItem.TerminalModificacion = tsTerminal;
                                }
                                else
                                {
                                    poObjectItem = new CRETINFORMERTCDOCUMENTOSPENDIENTES();
                                    poObjectItem.UsuarioIngreso = tsUsuario;
                                    poObjectItem.FechaIngreso = DateTime.Now;
                                    poObjectItem.TerminalIngreso = tsTerminal;
                                    poObject.CRETINFORMERTCDOCUMENTOSPENDIENTES.Add(poObjectItem);
                                }

                                poObjectItem.CodigoEstado = Diccionario.Activo;
                                poObjectItem.Compromisos = item.Compromisos;
                                poObjectItem.FechaEntrega = item.FechaEntrega;
                                poObjectItem.IdCheckList = item.IdCheckList;
                                poObjectItem.CheckList = poCombo.Where(x => x.Codigo == item.IdCheckList.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                            }
                        }
                        
                        if (poItem.InformeRtcProductos != null)
                        {
                            var poCombo = goSapConsultaItems();
                            var poGrupo = goSapConsultaGrupos();

                            //Eliminar Detalle 
                            var piListaIdPresentacion = poItem.InformeRtcProductos.Where(x => x.IdInformeRTCProducto != 0).Select(x => x.IdInformeRTCProducto).ToList();

                            foreach (var poItemDel in poObject.CRETINFORMERTCPRODUCTOS.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdInformeRTCProducto)))
                            {
                                poItemDel.CodigoEstado = Diccionario.Inactivo;
                                poItemDel.UsuarioModificacion = tsUsuario;
                                poItemDel.FechaModificacion = DateTime.Now;
                                poItemDel.TerminalModificacion = tsTerminal;
                            }

                            foreach (var item in poItem.InformeRtcProductos)
                            {
                                int pId = item.IdInformeRTCProducto;
                                var poObjectItem = poObject.CRETINFORMERTCPRODUCTOS.Where(x => x.IdInformeRTCProducto == pId && pId != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    poObjectItem.UsuarioModificacion = tsUsuario;
                                    poObjectItem.FechaModificacion = DateTime.Now;
                                    poObjectItem.TerminalModificacion = tsTerminal;
                                }
                                else
                                {
                                    poObjectItem = new CRETINFORMERTCPRODUCTOS();
                                    poObjectItem.UsuarioIngreso = tsUsuario;
                                    poObjectItem.FechaIngreso = DateTime.Now;
                                    poObjectItem.TerminalIngreso = tsTerminal;
                                    poObject.CRETINFORMERTCPRODUCTOS.Add(poObjectItem);
                                }

                                poObjectItem.CodigoEstado = Diccionario.Activo;
                                poObjectItem.ItemCode = item.ItemCode;
                                poObjectItem.ItemName = poCombo.Where(x => x.Codigo == item.ItemCode).Select(x => x.Descripcion).FirstOrDefault();
                                poObjectItem.ItmsGrpCod = item.ItmsGrpCod;
                                poObjectItem.ItmsGrpNam = poGrupo.Where(x => x.Codigo == item.ItmsGrpCod.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                            }
                        }

                        if (poItem.InformeRtcProveedor != null)
                        {
                            //Eliminar Detalle 
                            var piListaIdPresentacion = poItem.InformeRtcProveedor.Where(x => x.IdInformeRTCProveedor != 0).Select(x => x.IdInformeRTCProveedor).ToList();

                            foreach (var poItemDel in poObject.CRETINFORMERTCPROVEEDOR.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdInformeRTCProveedor)))
                            {
                                poItemDel.CodigoEstado = Diccionario.Inactivo;
                                poItemDel.UsuarioModificacion = tsUsuario;
                                poItemDel.FechaModificacion = DateTime.Now;
                                poItemDel.TerminalModificacion = tsTerminal;
                            }

                            foreach (var item in poItem.InformeRtcProveedor)
                            {
                                int pId = item.IdInformeRTCProveedor;
                                var poObjectItem = poObject.CRETINFORMERTCPROVEEDOR.Where(x => x.IdInformeRTCProveedor == pId && pId != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    poObjectItem.UsuarioModificacion = tsUsuario;
                                    poObjectItem.FechaModificacion = DateTime.Now;
                                    poObjectItem.TerminalModificacion = tsTerminal;
                                }
                                else
                                {
                                    poObjectItem = new CRETINFORMERTCPROVEEDOR();
                                    poObjectItem.UsuarioIngreso = tsUsuario;
                                    poObjectItem.FechaIngreso = DateTime.Now;
                                    poObjectItem.TerminalIngreso = tsTerminal;
                                    poObject.CRETINFORMERTCPROVEEDOR.Add(poObjectItem);
                                }

                                poObjectItem.CodigoEstado = Diccionario.Activo;
                                poObjectItem.Proveedor = item.Proveedor;
                                poObjectItem.MontoEstimadoVenta = item.MontoEstimadoVenta;
                            }
                        }
                    }


                    using (var poTran = new TransactionScope())
                    {
                        loBaseDa.SaveChanges();

                        foreach (var item in toLista)
                        {
                            foreach (var poItem in item.InformeRtcCroquis)
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

            var poObject = loBaseDa.Get<CRETINFORMERTC>().Include(x => x.CRETINFORMERTCCULTIVOS).Where(x => x.IdInformeRTC == tId).FirstOrDefault();

            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.FechaModificacion = DateTime.Now;
                poObject.TerminalModificacion = tsTerminal;

                foreach (var item in poObject.CRETINFORMERTCCULTIVOS.Where(x => x.CodigoEstado == Diccionario.Activo))
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

        private string lsEsValido(List<InformeRtc> toObject)
        {
            string psMsg = string.Empty;

            foreach (var item in toObject)
            {
                if (item.CodigoActividadCliente == Diccionario.Seleccione)
                {
                    psMsg = psMsg + "Falta seleccionar Actividad Cliente. \n";
                }

                var poListaMuestra = new List<int>();
                int num = 0;
                foreach (var fila in item.InformeRtcDocumentosPendientes)
                {
                    num++;
                    if (fila.FechaEntrega.Date < DateTime.Now.Date)
                    {
                        psMsg = psMsg + "Fecha de entrega en documentos pendientes debe ser mayor a la fecha actual. \n";
                    }
                }
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
                psCodigo = loBaseDa.Find<CRETINFORMERTC>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdInformeRTC }).OrderBy(x => x.IdInformeRTC).FirstOrDefault()?.IdInformeRTC.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<CRETINFORMERTC>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdInformeRTC }).OrderByDescending(x => x.IdInformeRTC).FirstOrDefault()?.IdInformeRTC.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<CRETINFORMERTC>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdInformeRTC }).ToList().Where(x => x.IdInformeRTC < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdInformeRTC).FirstOrDefault().IdInformeRTC.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<CRETINFORMERTC>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdInformeRTC }).ToList().Where(x => x.IdInformeRTC > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdInformeRTC).FirstOrDefault().IdInformeRTC.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            return psCodigo;

        }

        public int gIdInformeCredito(int tIdProcesoCredito)
        {
            return loBaseDa.Find<CRETINFORMERTC>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdProcesoCredito == tIdProcesoCredito).Select(x => x.IdInformeRTC).FirstOrDefault();
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
                piCont = loBaseDa.Find<CRETINFORMERTC>().Where(x => !psListaEstado.Contains(x.CodigoEstado) && x.CodigoCliente == tsCodeCliente && x.IdInformeRTC != piNum).Count();
            }
            else
            {
                piCont = loBaseDa.Find<CRETINFORMERTC>().Where(x => !psListaEstado.Contains(x.CodigoEstado) && x.CodigoCliente == tsCodeCliente).Count();
            }

            if (piCont == 0)
            {
                pbresult = true;
            }

            return pbresult;
        }
    }
}
