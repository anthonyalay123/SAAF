using DevExpress.Data.ODataLinq.Helpers;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Entidad.Entidades.Administracion;
using GEN_Entidad.Entidades.Compras;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static GEN_Entidad.Diccionario;

namespace COM_Negocio
{
   public class clsNOrdenPago : clsNBase
    {
        public string gsGuardar(OrdenPago toObject,string tsUsuario, string tsTerminal, bool tbCorregido = false, string tsComentarioCorrecion = "")
        {
            //Actualiza el Card Code por si no existe en SAAF
            loBaseDa.ExecuteQuery("UPDATE T0 SET T0.CardCode = T1.CardCode FROM COMMPROVEEDORES T0 INNER JOIN SBO_AFECOR.dbo.OCRD T1 (NOLOCK) ON T0.Identificacion = T1.LicTradNum COLLATE DATABASE_DEFAULT WHERE T0.CodigoEstado = 'A' AND (T0.CardCode = '' OR T0.CardCode IS NULL)");

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            psResult = lsEsValido(toObject);        

            List<string> psListaAdjuntoEliminar = new List<string>();

            var poListaProv = loBaseDa.Find<COMMPROVEEDORES>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new { x.IdProveedor, x.Identificacion, x.CardCode }).ToList();

            //Valida si existe CardCode en SAAF - Caso contrario significa que no existe proveedor creado en SAAF
            if (string.IsNullOrEmpty(poListaProv.Where(x => x.IdProveedor == toObject.IdProveedor).Select(x => x.CardCode).FirstOrDefault()))
            {
                throw new Exception("La orden de pago no puede ser creada porque el proveedor no existe en SAP. Comuníquese con el Dpto. Contable.");
            }

            if (psResult == string.Empty)
            {

                var poZona = goConsultarZonasSAAFSinNumero();

                using (var poTran = new TransactionScope())
                {

                    var poObject = loBaseDa.Get<COMTORDENPAGO>().Include(x => x.COMTORDENPAGOFACTURA).Include(x=>x.COMTORDENPAGOADJUNTO).Where(x => x.IdOrdenPago == toObject.IdOrdenPago).FirstOrDefault();
                    if (poObject!=null)
                    { 
                        poObject.Observacion = toObject.Observacion;
                        poObject.Valor = toObject.Valor;
                        //poObject.UsuarioAprobacion = tsUsuario;
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.TotalOrdenCompra = toObject.TotalOrdenCompra;
                        poObject.CodigoTipoCompra = toObject.CodigoTipoCompra;
                        poObject.CodigoTipoOrdenPago = toObject.CodigoTipoOrdenPago;
                        poObject.FormaPago = toObject.FormaPago;
                        poObject.ComentarioAprobador = toObject.ComentarioAprobador;

                    

                        string psCodigoEstado = "";
                        if (toObject.Valor == toObject.TotalOrdenCompra)
                        {
                            psCodigoEstado = Diccionario.Aprobado;
                            poObject.FechaAprobacion = DateTime.Now;
                            poObject.UsuarioAprobacion = tsUsuario;
                        }
                        else
                        {
                            psCodigoEstado = toObject.CodigoEstado;
                        }

                        if (tbCorregido)
                        {
                            REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTransaccion);
                            poTransaccion.CodigoEstado = Diccionario.Activo;
                            poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.OrdenPago;
                            poTransaccion.ComentarioAprobador = tsComentarioCorrecion;
                            poTransaccion.IdTransaccionReferencial = poObject.IdOrdenPago;
                            poTransaccion.UsuarioAprobacion = tsUsuario;
                            poTransaccion.UsuarioIngreso = tsUsuario;
                            poTransaccion.FechaIngreso = DateTime.Now;
                            poTransaccion.TerminalIngreso = tsTerminal;
                            poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                            poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psCodigoEstado);
                            poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;
                        }

                        poObject.CodigoEstado = psCodigoEstado;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;

                        poObject.IdentificacionProveedor = poListaProv.Where(x => x.IdProveedor == toObject.IdProveedor).Select(x => x.Identificacion).FirstOrDefault();
                        poObject.IdProveedor = toObject.IdProveedor;

                        //Valida si hay alguna factura que es tomada en otro proceso 

                        //foreach (var item in poObject.COMTORDENPAGOFACTURA.Where(x => x.CodigoEstado == Diccionario.Activo))
                        //{
                        //    if (loBaseDa.Find<COMTFACTURAPAGO>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdOrdenPagoFactura == item.IdOrdenPagoFactura).Count() > 0)
                        //    {
                        //        psResult = string.Format("{0}Factura: {1} Está siendo usada en otro proceso no es posible Modificar datos de la Orden de Pago \n", psResult, item.Factura);
                        //    }
                        //}

                        if (!string.IsNullOrEmpty(psResult))
                        {
                            return psResult;
                        }


                        List<int> poListaIdPe = toObject.Facturas.Select(x => x.IdFactura).ToList();
                        List<int> piListaEliminar = poObject.COMTORDENPAGOFACTURA.Where(x => !poListaIdPe.Contains(x.IdOrdenPagoFactura)).Select(x => x.IdOrdenPagoFactura).ToList();
                        //Recorrer la base de dato modificando el codigo estado a eliminado
                        foreach (var poItem in poObject.COMTORDENPAGOFACTURA.Where(x => piListaEliminar.Contains(x.IdOrdenPagoFactura)))
                        {
                            psListaAdjuntoEliminar.Add(poItem.ArchivoAdjunto);
                            poItem.CodigoEstado = Diccionario.Eliminado;
                            poItem.UsuarioModificacion = tsUsuario;
                            poItem.FechaModificacion = DateTime.Now;
                            poItem.TerminalModificacion = tsTerminal;

                            foreach (var det in poItem.COMTGUIAREMISIONFACTURA.Where(x=>x.CodigoEstado == Diccionario.Activo))
                            {
                                if (det.COMTGUIAREMISION != null)
                                {
                                    det.COMTGUIAREMISION.CodigoEstado = Diccionario.Eliminado;
                                    det.COMTGUIAREMISION.UsuarioModificacion = tsUsuario;
                                    det.COMTGUIAREMISION.FechaModificacion = DateTime.Now;
                                    det.COMTGUIAREMISION.TerminalModificacion = tsTerminal;

                                    //if (det.COMTGUIAREMISION.IdOrdenPagoFactura != null)
                                    //{
                                    //    det.COMTGUIAREMISION.IdOrdenPagoFactura = null;
                                    //}
                                }
                            }
                        }

                        if (toObject.Facturas != null)
                        {
                            foreach (var factura in toObject.Facturas)
                            {
                                factura.ZonaGrupo = poZona.Where(x => x.Codigo == factura.IdZonaGrupo.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                                var poObjectItem = poObject.COMTORDENPAGOFACTURA.Where(x => x.IdOrdenPagoFactura == factura.IdFactura && factura.IdFactura != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    psResult += lCargarFactura(ref poObjectItem, factura, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                                    // Insert Auditoría
                                    loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                                }
                                else
                                {
                                    poObjectItem = new COMTORDENPAGOFACTURA();
                                    poObject.COMTORDENPAGOFACTURA.Add(poObjectItem);
                                    psResult += lCargarFactura(ref poObjectItem, factura, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                                    // Insert Auditoría
                                    loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);   
                                }
                            }
                        }
                    
                        List<int> poListaIdPeAJ = toObject.OrdenPagoAdjunto.Select(x => x.IdOrdenPagoAdjunto).ToList();
                        List<int> piArchivoAdjuntoEliminar = poObject.COMTORDENPAGOADJUNTO.Where(x => !poListaIdPeAJ.Contains(x.IdOrdenPagoAdjunto)).Select(x => x.IdOrdenPagoAdjunto).ToList();
                        foreach (var poItem in poObject.COMTORDENPAGOADJUNTO.Where(x => piArchivoAdjuntoEliminar.Contains(x.IdOrdenPagoAdjunto)))
                        {
                            psListaAdjuntoEliminar.Add(poItem.ArchivoAdjunto);
                            poItem.CodigoEstado = Diccionario.Eliminado;
                            poItem.UsuarioModificacion = tsUsuario;
                            poItem.FechaModificacion = DateTime.Now;
                            poItem.TerminalModificacion = tsTerminal;
                        }

                        //Guardar Archivo Adjunto
                        if (toObject.OrdenPagoAdjunto != null)
                        {
                            foreach (var poItem in toObject.OrdenPagoAdjunto)
                            {
                                int pIdDetalle = poItem.IdOrdenPagoAdjunto;

                                var poObjectItem = poObject.COMTORDENPAGOADJUNTO.Where(x => x.IdOrdenPagoAdjunto == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    if (poObjectItem.ArchivoAdjunto != poItem.ArchivoAdjunto)
                                    {
                                        psListaAdjuntoEliminar.Add(poObjectItem.ArchivoAdjunto);
                                    }

                                    lCargarArchivoAdjunto(ref poObjectItem, poItem, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso, true);

                                    // Insert Auditoría
                                    loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                                }
                                else
                                {
                                    poObjectItem = new COMTORDENPAGOADJUNTO();
                                    lCargarArchivoAdjunto(ref poObjectItem, poItem, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                                    // Insert Auditoría
                                    loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);

                                    poObject.COMTORDENPAGOADJUNTO.Add(poObjectItem);
                                }
                            }
                        }

                    }
                    else
                    {
                        poObject = new COMTORDENPAGO();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.Observacion = toObject.Observacion;
                        poObject.Valor = toObject.Valor;
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                        poObject.TotalOrdenCompra = toObject.TotalOrdenCompra;
                        poObject.IdentificacionProveedor = toObject.ProveedorNombre;
                        poObject.CodigoTipoCompra = toObject.CodigoTipoCompra;
                        poObject.CodigoTipoOrdenPago = toObject.CodigoTipoOrdenPago;
                        poObject.FormaPago = toObject.FormaPago;

                        poObject.IdentificacionProveedor = poListaProv.Where(x => x.IdProveedor == toObject.IdProveedor).Select(x => x.Identificacion).FirstOrDefault();
                        poObject.IdProveedor = toObject.IdProveedor;

                        if (toObject.Valor == toObject.TotalOrdenCompra)
                        {
                            poObject.CodigoEstado = Diccionario.Aprobado;
                            poObject.FechaAprobacion = DateTime.Now;
                            poObject.UsuarioAprobacion = tsUsuario;
                        }
                        else
                        {
                            poObject.CodigoEstado = toObject.CodigoEstado;
                        }

                        if (toObject.Facturas!=null)
                        {
                            foreach (var factura in toObject.Facturas)
                            {
                                factura.ZonaGrupo = poZona.Where(x => x.Codigo == factura.IdZonaGrupo.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                                var poObjectItem = poObject.COMTORDENPAGOFACTURA.Where(x => x.IdOrdenPagoFactura == factura.IdFactura && factura.IdFactura != 0).FirstOrDefault();
                                if (poObjectItem != null)
                                {
                                    psResult += lCargarFactura(ref poObjectItem, factura, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                                    // Insert Auditoría
                                    loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                                }
                                else
                                {
                                    poObjectItem = new COMTORDENPAGOFACTURA();
                                    poObject.COMTORDENPAGOFACTURA.Add(poObjectItem);
                                    psResult += lCargarFactura(ref poObjectItem, factura, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                                    // Insert Auditoría
                                    loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);

                                    
                                }

                                loBaseDa.SaveChanges();
                            }
                        }

                        //Seguimiento para Orden de pago
                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTransaccion);
                        poTransaccion.CodigoEstado = Diccionario.Activo;
                        poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.OrdenPago;
                        poTransaccion.ComentarioAprobador = tsComentarioCorrecion;
                        poTransaccion.IdTransaccionReferencial = poObject.IdOrdenPago;
                        poTransaccion.UsuarioAprobacion = tsUsuario;
                        poTransaccion.UsuarioIngreso = tsUsuario;
                        poTransaccion.FechaIngreso = DateTime.Now;
                        poTransaccion.TerminalIngreso = tsTerminal;
                        poTransaccion.EstadoAnterior = "";
                        poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                        //Guardar Archivo Adjunto Nuevo
                        if (toObject.OrdenPagoAdjunto != null)
                        {
                            foreach (var poItem in toObject.OrdenPagoAdjunto)
                            {

                                var poObjectItem = new COMTORDENPAGOADJUNTO();
                                lCargarArchivoAdjunto(ref poObjectItem, poItem, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);
                                // Insert Auditoría
                                loBaseDa.Auditoria(poObjectItem, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, poObject.UsuarioIngreso, poObject.TerminalIngreso);

                                poObject.COMTORDENPAGOADJUNTO.Add(poObjectItem);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(psResult))
                    {
                        return psResult;
                    }

                    loBaseDa.SaveChanges();

                    //Guardar Facturas Adjuntas
                    foreach (var poItem in toObject.Facturas)
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

                    //Guardar Documentos Adjuntos
                    foreach (var poItem in toObject.OrdenPagoAdjunto)
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

                    foreach (var psItem in psListaAdjuntoEliminar)
                    {
                        string eliminar = ConfigurationManager.AppSettings["CarpetaOPCompras"].ToString() + psItem;
                        File.Delete(eliminar);
                    }

                    poTran.Complete();
                }
            }

            return psResult;
        }

        public void gGuardaArchivoPagoResumen(string tsCodigo , FacturaAprobadasDetalleGrid toObject, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();

            int tiCodigo = int.Parse(tsCodigo);
            var poObject = loBaseDa.Get<COMTSEMANAPAGOS>().Where(x=>x.IdSemanaPago == tiCodigo).FirstOrDefault();

            if(poObject != null)
            {
                if (!string.IsNullOrEmpty(poObject.ArchivoAdjunto))
                {
                    string eliminar = ConfigurationManager.AppSettings["CarpetaOPBcoCompras"].ToString() + poObject.ArchivoAdjunto;
                    File.Delete(eliminar);
                }

                poObject.ArchivoAdjunto = toObject.ArchivoAdjunto;
                poObject.NombreOriginal = toObject.NombreOriginal;
                poObject.FechaModificacion = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;

                if (!string.IsNullOrEmpty(toObject.ArchivoAdjunto) && !string.IsNullOrEmpty(toObject.RutaDestino))
                {
                    if (toObject.RutaOrigen != toObject.RutaDestino)
                    {

                        if (toObject.RutaOrigen != null)
                        {
                            File.Copy(toObject.RutaOrigen, toObject.RutaDestino);
                        }

                    }
                }

                loBaseDa.SaveChanges();
            }
        }

        public string gGuardaAdjuntosArchivoPago(string tsCodigo, FacturaAprobadasDetalleGrid toObject, List<FacturaAprobadasDetalleGrid> toLista, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            string psMsg = "";
            int tiCodigo = int.Parse(tsCodigo);
            var poObject = loBaseDa.Get<COMPGRUPOPAGOS>().Where(x => x.IdGrupoPagos == tiCodigo).FirstOrDefault();

            if (poObject != null)
            {
                if (!string.IsNullOrEmpty(poObject.ArchivoAdjunto))
                {
                    if (poObject.ArchivoAdjunto != toObject.ArchivoAdjunto)
                    {
                        string eliminar = ConfigurationManager.AppSettings["CarpetaOPBcoCompras"].ToString() + poObject.ArchivoAdjunto;
                        File.Delete(eliminar);
                    }
                    
                }

                if (poObject.ArchivoAdjunto != toObject.ArchivoAdjunto)
                {
                    poObject.ArchivoAdjunto = toObject.ArchivoAdjunto;
                    poObject.NombreOriginal = toObject.NombreOriginal;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;

                    if (!string.IsNullOrEmpty(toObject.ArchivoAdjunto) && !string.IsNullOrEmpty(toObject.RutaDestino))
                    {
                        if (toObject.RutaOrigen != toObject.RutaDestino)
                        {

                            if (toObject.RutaOrigen != null)
                            {
                                File.Copy(toObject.RutaOrigen, toObject.RutaDestino);
                            }

                        }
                    }
                }
                
                loBaseDa.SaveChanges();
            }

            var pIdLista = toLista.Select(x => x.IdFacturaPago).ToList();
            var poLista = loBaseDa.Get<COMTFACTURAPAGO>().Where(x => pIdLista.Contains(x.IdFacturaPago)).ToList();

            foreach (var item in toLista)
            {
                var poRegistro = poLista.Where(x => x.IdFacturaPago == item.IdFacturaPago).FirstOrDefault();
                if (poRegistro != null)
                {
                    if (poRegistro.ArchivoAdjunto != item.ArchivoAdjunto)
                    {
                        if (!string.IsNullOrEmpty(item.ArchivoAdjunto) && !string.IsNullOrEmpty(item.RutaDestino))
                        {
                            if (item.RutaOrigen != item.RutaDestino)
                            {
                                if (item.RutaOrigen != null)
                                {
                                    poRegistro.ArchivoAdjunto = item.ArchivoAdjunto;
                                    poRegistro.NombreOriginal = item.NombreOriginal;
                                    poRegistro.FechaModificacion = DateTime.Now;
                                    poRegistro.UsuarioModificacion = tsUsuario;
                                    poRegistro.TerminalModificacion = tsTerminal;

                                    if (!File.Exists(item.RutaDestino))
                                    {
                                        File.Copy(item.RutaOrigen, item.RutaDestino);
                                    }
                                }

                            }
                        }
                    }
                }

                loBaseDa.SaveChanges();
            }

            return psMsg;
        }

        public Factura gConsultaArchivoPagoResumen(string tsCodigo)
        {
            var poResult = new Factura();
            int tiCodigo = int.Parse(tsCodigo);
            var poObject = loBaseDa.Find<COMPGRUPOPAGOS>().Where(x => x.IdGrupoPagos == tiCodigo).FirstOrDefault();
            
            if (poObject != null)
            {
                poResult.NombreOriginal = poObject.NombreOriginal;
                poResult.ArchivoAdjunto = poObject.ArchivoAdjunto;
                poResult.RutaDestino = ConfigurationManager.AppSettings["CarpetaOPBcoCompras"].ToString();
            }

            return poResult;
        }

        public void gEliminarchivoPagoResumen(string tsCodigo, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();

            int tiCodigo = int.Parse(tsCodigo);
            var poObject = loBaseDa.Get<COMTSEMANAPAGOS>().Where(x => x.IdSemanaPago == tiCodigo).FirstOrDefault();

            if (poObject != null)
            {
                if (!string.IsNullOrEmpty(poObject.ArchivoAdjunto))
                {
                    string eliminar = ConfigurationManager.AppSettings["CarpetaOPBcoCompras"].ToString() + poObject.ArchivoAdjunto;
                    File.Delete(eliminar);
                }

                poObject.ArchivoAdjunto = "";
                poObject.NombreOriginal = "";
                poObject.FechaModificacion = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;


                loBaseDa.SaveChanges();
            }
        }

        private string lsEsValido(OrdenPago toObject)
        {
            string psMsg = string.Empty;
            bool pbHabilitaControlGuiasObligatorias = loBaseDa.Find<COMPPARAMETRO>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => x.HabilitaControlGuiasObligatorias).FirstOrDefault() ?? false;
            if (string.IsNullOrEmpty(toObject.Observacion))
            {
                psMsg = psMsg + "Falta la observación  \n";
            }
            if (toObject.Facturas==null)
            {
                psMsg = psMsg + "Falta agregar Factura \n";
            }
            if (toObject.CodigoTipoCompra == Diccionario.Seleccione)
            {
                psMsg = psMsg + "Falta seleccionar Tipo Compra \n";
            }
            if (toObject.Facturas.Count == 0 && toObject.IdOrdenPago == 0)
            {
                psMsg = psMsg + "Debe ingresar detalle. \n";
            }
            else
            {
                int num = 1;
                foreach (var factura in toObject.Facturas)
                {
                    if (string.IsNullOrEmpty(factura.NoFactura))
                    {
                        psMsg = psMsg + "Falta agregar No. Factura en la fila:" + num + "\n";
                    }
                    if (string.IsNullOrEmpty(factura.Descripcion))
                    {
                        psMsg = psMsg + "Falta agregar Descripcion en la fila: "+num+"\n";
                    }
                    if (factura.Valor <= 0)
                    {
                        psMsg = psMsg + "Valor de Factura no es valido en la fila " + num + "\n";
                    }

                    if (pbHabilitaControlGuiasObligatorias)
                    {
                        // Validación de Tipo de Transporte
                        if (factura.TipoTransporte != Diccionario.Seleccione && factura.GuiaRemision.Count == 0)
                        {
                            psMsg = psMsg + "Debe ingresar guías de remisión en la fila " + num + "\n";
                        }

                        if (factura.TipoTransporte == Diccionario.Seleccione && factura.GuiaRemision.Count > 0)
                        {
                            psMsg = psMsg + "Debe asignar el tipo de transporte en la fila " + num + "\n";
                        }
                    }

                    if (factura.GuiaRemision != null)
                    {
                        var contadorDeTipoDeTransporte = factura.GuiaRemision.Select(x => x.TipoTransporte).Distinct().Count();
                        
                        if (contadorDeTipoDeTransporte > 1)
                        {
                            psMsg = psMsg + "En la factura # " + factura.NoFactura + " Existen guías con diferentes tipos de transporte. CORREGIR! \n";
                        }
                    }
                    
                    num = num + 1;
                }

                num = 0;

                foreach (var a in toObject.OrdenPagoAdjunto)
                {
                    num = num + 1;
                    if (a.NombreOriginal == null)
                    {
                        psMsg = psMsg + "Ingrese el Archivo Adjunto en la fila: " + num + "\n";

                    }

                }
            }
           

            return psMsg;
        }

        private string lCargarFactura(ref COMTORDENPAGOFACTURA toEntidadBd, Factura toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            string psMsg = "";

            if (toEntidadBd.CodigoEstado != Diccionario.Aprobado)
            {
                toEntidadBd.CodigoEstado = Diccionario.Activo;
            }

            toEntidadBd.Factura = toEntidadData.NoFactura;
            toEntidadBd.Descripcion = toEntidadData.Descripcion;
            toEntidadBd.FechaFactura = toEntidadData.FechaFactura;
            toEntidadBd.FechaVencimiento = toEntidadData.FechaFactura.AddDays(toEntidadData.Dias);
            toEntidadBd.Valor = toEntidadData.Valor;
            toEntidadBd.Observacion = toEntidadData.Observacion;
            toEntidadBd.SaldoOrdenCompra = toEntidadData.SaldoOc;
            toEntidadBd.ArchivoAdjunto = toEntidadData.ArchivoAdjunto;
            toEntidadBd.NombreOriginal = toEntidadData.NombreOriginal;
            toEntidadBd.DiasPago = toEntidadData.Dias;
            toEntidadBd.CodigoZonaFactura = toEntidadData.TipoTransporte;
            toEntidadBd.IdZonaGrupo = toEntidadData.IdZonaGrupo;
            toEntidadBd.ZonaGrupo = toEntidadData.ZonaGrupo;

            if (tbActualiza)
            {
                toEntidadBd.UsuarioModificacion = tsUsuario;
                toEntidadBd.FechaModificacion = tdFecha;
                toEntidadBd.TerminalModificacion = tsTerminal;
            }
            else
            {
                toEntidadBd.UsuarioIngreso = tsUsuario;
                toEntidadBd.FechaIngreso = tdFecha;
                toEntidadBd.TerminalIngreso = tsTerminal;
            }

            loBaseDa.SaveChanges();

            if (toEntidadData.GuiaRemision != null)
            {

                int pIdOrdenPagoFactura = toEntidadBd.IdOrdenPagoFactura;
                List<int> poListaIdPeGuia = toEntidadData.GuiaRemision.Select(x => x.IdGuiaRemision).ToList();
                List<int> piListaEliminarGuia = toEntidadBd.COMTGUIAREMISIONFACTURA.Where(x => x.CodigoEstado == Diccionario.Activo && x.IdOrdenPagoFactura == pIdOrdenPagoFactura && !poListaIdPeGuia.Contains(x.IdGuiaRemision)).Select(x => x.IdGuiaRemision).ToList();

                var poListaGuias = loBaseDa.Get<COMTGUIAREMISION>().Include(x => x.COMTGUIAREMISIONDETALLE).Include(x=>x.COMTGUIAREMISIONFACTURA).Where(x => x.CodigoEstado == Diccionario.Activo && piListaEliminarGuia.Contains(x.IdGuiaRemision)).ToList();
                foreach (var poItem in poListaGuias)
                {
                    foreach (var item in poItem.COMTGUIAREMISIONFACTURA.Where(x => x.CodigoEstado == Diccionario.Activo && x.IdOrdenPagoFactura == pIdOrdenPagoFactura))
                    {
                        item.FechaModificacion = DateTime.Now;
                        item.UsuarioModificacion = tsUsuario;
                        item.TerminalModificacion = tsTerminal;
                        item.CodigoEstado = Diccionario.Eliminado;
                    }

                    if (poItem.IngresoManual == false)
                    {
                        poItem.FechaModificacion = DateTime.Now;
                        poItem.UsuarioModificacion = tsUsuario;
                        poItem.TerminalModificacion = tsTerminal;
                        poItem.CodigoEstado = Diccionario.Eliminado;

                        foreach (var Det in poItem.COMTGUIAREMISIONDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                        {
                            Det.CodigoEstado = Diccionario.Eliminado;
                            Det.UsuarioModificacion = tsUsuario;
                            Det.TerminalModificacion = tsTerminal;
                            Det.FechaModificacion = DateTime.Now;
                        }
                    }
                    
                }
                

                foreach (var item in toEntidadData.GuiaRemision)
                {
                    psMsg = psMsg + lsGuardarGuiaRemision(item, tsUsuario, tsTerminal, toEntidadBd);
                }
            }
            //if (toEntidadData.GuiaRemisionFactura != null)
            //{
            //    foreach (var item in toEntidadData.GuiaRemisionFactura)
            //    {
            //        //List<int> poListaIdPeGuia = item.GuiaRemision..Select(x => x.IdGuiaRemision).ToList();
            //        //List<int> piListaEliminarGuia = toEntidadBd.COMTGUIAREMISION.Where(x => x.CodigoEstado == Diccionario.Activo && !poListaIdPeGuia.Contains(x.IdGuiaRemision)).Select(x => x.IdGuiaRemision).ToList();

            //        //foreach (var poItem in toEntidadBd.COMTGUIAREMISION.Where(x => piListaEliminarGuia.Contains(x.IdGuiaRemision)))
            //        //{

            //        //    poItem.FechaModificacion = DateTime.Now;
            //        //    poItem.UsuarioModificacion = tsUsuario;
            //        //    poItem.TerminalModificacion = tsTerminal;
            //        //    if (poItem.IngresoManual == false)
            //        //    {
            //        //        poItem.CodigoEstado = Diccionario.Eliminado;

            //        //        foreach (var Det in poItem.COMTGUIAREMISIONDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
            //        //        {
            //        //            Det.CodigoEstado = Diccionario.Eliminado;
            //        //            Det.UsuarioModificacion = tsUsuario;
            //        //            Det.TerminalModificacion = tsTerminal;
            //        //            Det.FechaModificacion = DateTime.Now;
            //        //        }
            //        //    }
            //        //    else
            //        //    {
            //        //        poItem.IdOrdenPagoFactura = null;
            //        //    }

            //        //}
            //    }



            //    foreach (var item in toEntidadData.GuiaRemisionFactura)
            //    {
            //        psMsg = psMsg + lsGuardarGuiaRemision(item.GuiaRemision, tsUsuario, tsTerminal, toEntidadBd);
            //    }
            //}

            return psMsg;
        }

        #region Guia Remision

        public void gsGuardarDesdeLN(GuiaRemision toObject, string tsUsuario, string tsTerminal, COMTORDENPAGOFACTURA toOrdenPagoFactura = null)
        {
            lsGuardarGuiaRemision(toObject, tsUsuario, tsTerminal, toOrdenPagoFactura);
        }

        public string gsGuardarDesdeGuia(GuiaRemision toObject, string tsUsuario, string tsTerminal)
        {
            string psResult = string.Empty;

            loBaseDa.CreateContext();

            psResult = lsEsValidoGuia(toObject);

            if (psResult == string.Empty)
            {
                using (var poTran = new TransactionScope())
                {
                    lsGuardarGuiaRemision(toObject, tsUsuario, tsTerminal, null);
                    loBaseDa.SaveChanges();
                    poTran.Complete();
                }       
            }

            return psResult;
        }

        private string lsGuardarGuiaRemision(GuiaRemision toObject, string tsUsuario, string tsTerminal, COMTORDENPAGOFACTURA toOrdenPagoFactura = null)
        {
            string psMsg = "";
            
            bool pbValidaGuiasDuplicadas = loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoUsuario == tsUsuario).Select(x => x.ControlaDuplicidadGuias).FirstOrDefault() ?? false;
            
            if (pbValidaGuiasDuplicadas)
            {
                if (toOrdenPagoFactura != null)
                {
                    var poListaId = loBaseDa.Find<COMTGUIAREMISIONFACTURA>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdOrdenPagoFactura == toOrdenPagoFactura.IdOrdenPagoFactura).Select(x => x.IdGuiaRemision).ToList();

                    var poListaGuiaRemisionBase = loBaseDa.Find<COMTGUIAREMISION>().Where(x => x.CodigoEstado == Diccionario.Activo && !poListaId.Contains(x.IdGuiaRemision))
                        .Select(x => new { x.Tipo, x.Numero, x.IdGuiaRemision, x.IngresoManual }).ToList();

                    List<string> psListaString = new List<string>();
                    foreach (var item in poListaGuiaRemisionBase)
                    {
                        if (toObject.Tipo == item.Tipo && toObject.Numero == item.Numero)
                        {
                            var psFactura = loBaseDa.Find<COMTGUIAREMISIONFACTURA>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdGuiaRemision == item.IdGuiaRemision && x.Numero == item.Numero && x.Tipo == item.Tipo).Select(x => x.Factura).FirstOrDefault();
                            if (string.IsNullOrEmpty(psFactura) && item.IngresoManual == true)
                            {
                                psMsg = psMsg;
                            }
                            else
                            {
                                psMsg = string.Format("{0}En la factura: {1} NO es posible tomar la guía No: {2} de tipo: {3} porque ya ha sido tomada en la Factura: {4}.\n", psMsg, toOrdenPagoFactura.Factura, toObject.Numero, toObject.Tipo, psFactura);
                            }
                            
                        }
                    }
                }
                
                //var poListaGuiaRemisionBase = loBaseDa.Get<COMTGUIAREMISION>().Include(x=>x.COMTGUIAREMISIONFACTURA).Where(x => x.CodigoEstado == Diccionario.Activo && x.IdOrdenPagoFactura != null && x.IdOrdenPagoFactura != toOrdenPagoFactura.IdOrdenPagoFactura)
                //    .Select(x => new { x.Tipo, x.Numero, x.COMTORDENPAGOFACTURA.Factura }).ToList();

                //var poListaGuiaRemisionBase = loBaseDa.Get<COMTGUIAREMISION>().Include(x => x.COMTGUIAREMISIONFACTURA).Where(x => x.CodigoEstado == Diccionario.Activo && x.IdOrdenPagoFactura != null && x.IdOrdenPagoFactura != toOrdenPagoFactura.IdOrdenPagoFactura)
                //   .Select(x => new { x.Tipo, x.Numero, x.COMTORDENPAGOFACTURA.Factura }).ToList();

                //List<string> psListaString = new List<string>();
                //foreach (var item in poListaGuiaRemisionBase)
                //{
                //    if (toObject.Tipo == item.Tipo && toObject.Numero == item.Numero)
                //    {
                //        psMsg = string.Format("{0}En la factura: {1} NO es posible tomar la guía No: {2} de tipo: {3} porque ya ha sido tomada en la Factura: {4}.\n", psMsg, toOrdenPagoFactura.Factura, toObject.Numero, toObject.Tipo, item.Factura);
                //    }
                //}
            }

            if (string.IsNullOrEmpty(psMsg))
            {
                var poObject = loBaseDa.Get<COMTGUIAREMISION>().Include(x => x.COMTGUIAREMISIONDETALLE).Where(x => x.IdGuiaRemision == toObject.IdGuiaRemision).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                    poObject.FechaModificacion = DateTime.Now;
                }
                else
                {
                    poObject = new COMTGUIAREMISION();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.TerminalIngreso = tsTerminal;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.CodigoEstado = Diccionario.Activo;
                }

                List<int> poListaIdPe = toObject.GuiaRemisionDetalle.Select(x => x.IdGuiaRemisionDetalle).ToList();
                List<int> piListaEliminar = poObject.COMTGUIAREMISIONDETALLE.Where(x => !poListaIdPe.Contains(x.IdGuiaRemisionDetalle)).Select(x => x.IdGuiaRemisionDetalle).ToList();
                //Recorrer la base de dato modificando el codigo estado a eliminado
                foreach (var poItem in poObject.COMTGUIAREMISIONDETALLE.Where(x => piListaEliminar.Contains(x.IdGuiaRemisionDetalle)))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                poObject.Tipo = toObject.Tipo;
                poObject.Local = toObject.Local;
                poObject.PuntoEmision = toObject.PuntoEmision;
                poObject.Secuencia = toObject.Secuencia;
                poObject.Numero = toObject.Numero;
                poObject.FechaEmision = toObject.FechaEmision;
                poObject.FechaInicioTraslado = toObject.FechaInicioTraslado;
                poObject.FechaTerminoTraslado = toObject.FechaTerminoTraslado;
                poObject.CodigoMotivoTraslado = toObject.CodigoMotivoTraslado;
                poObject.MotivoTraslado = toObject.MotivoTraslado;
                poObject.CodigoTransportista = toObject.CodigoTransportista;
                poObject.Transportista = toObject.Transportista;
                poObject.IdentificacionTransportista = toObject.IdentificacionTransportista;
                poObject.CodigoTransporte = toObject.CodigoTransporte;
                poObject.Transporte = toObject.Transporte;
                poObject.PuntoPartida = toObject.PuntoPartida;
                poObject.CodCliente = toObject.CodCliente;
                poObject.Cliente = toObject.Cliente;
                poObject.CodProveedor = toObject.CodProveedor;
                poObject.Proveedor = toObject.Proveedor;
                poObject.IdentificacionCliente = toObject.IdentificacionCliente;
                poObject.PuntoLlegada = toObject.PuntoLlegada;
                poObject.ValorBulto = toObject.ValorBulto;
                poObject.NumBultos = toObject.NumBultos;
                poObject.DocEntry = toObject.DocEntry;
                poObject.DocNum = toObject.DocNum;
                poObject.CodVendedor = toObject.CodVendedor;
                poObject.Vendedor = toObject.Vendedor;
                poObject.CodZonaVendedor = toObject.CodZonaVendedor;
                poObject.ZonaVendedor = toObject.ZonaVendedor;
                poObject.CodZonaTransporte = toObject.CodZonaTransporte;
                poObject.ZonaTransporte = toObject.ZonaTransporte;
                //poObject.IdOrdenPagoFactura = toObject.IdOrdenPagoFactura;
                poObject.UsuarioSap = toObject.UsuarioSap;
                poObject.CodAlmacen = toObject.CodAlmacen;
                poObject.CodAlmacenDestino = toObject.CodAlmacenDestino;
                poObject.FechaContabilizacion = toObject.FechaContabilizacion;
                poObject.FechaDocumento = toObject.FechaDocumento;
                poObject.FechaEntrega = toObject.FechaEntrega;
                poObject.Total = toObject.Total;
                poObject.Comentario = toObject.Comentario;
                poObject.FolioNum = toObject.FolioNum;
                poObject.Prefijo = toObject.Prefijo;
                poObject.NumeroGuia = toObject.NumeroGuia;
                poObject.IngresoManual = toObject.IngresoManual;
                poObject.TipoItem = toObject.TipoItem;
                poObject.CodigoMotivoGuia = toObject.CodigoMotivoGuia;
                poObject.GR = toObject.GR;
                poObject.Externa = toObject.Externa;
                poObject.TipoTransporte = toObject.TipoTransporte;
                


                if (toObject.GuiaRemisionDetalle != null)
                {
                    var poListaBodega = goSapConsultaComboCatalogoBodegas();
                    var poItems = goSapConsultaItems();
                    int piTotal = toObject.GuiaRemisionDetalle.Select(x => x.Cantidad).Sum();
                    foreach (var item in toObject.GuiaRemisionDetalle)
                    {
                        item.ItemName = poItems.Where(x => x.Codigo == item.ItemCode).Select(x => x.Descripcion).FirstOrDefault();

                        var poObjectItem = poObject.COMTGUIAREMISIONDETALLE.Where(x => x.IdGuiaRemisionDetalle == item.IdGuiaRemisionDetalle && item.IdGuiaRemisionDetalle != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.TerminalModificacion = tsTerminal;
                            poObjectItem.FechaModificacion = DateTime.Now;
                        }
                        else
                        {
                            poObjectItem = new COMTGUIAREMISIONDETALLE();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObject.COMTGUIAREMISIONDETALLE.Add(poObjectItem);
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.ItemCode = item.ItemCode;
                        poObjectItem.ItemName = string.IsNullOrEmpty(item.ItemName) ? "" : item.ItemName;
                        poObjectItem.Cantidad = item.Cantidad;
                        poObjectItem.AlmacenOrigen = item.AlmacenOrigen;
                        poObjectItem.AlmacenDestino = item.AlmacenDestino;
                        poObjectItem.CantidadOriginal = item.CantidadOriginal;
                        poObjectItem.NombreAlmacenOrigen = poListaBodega.Where(x=>x.Codigo == item.AlmacenOrigen).Select(x=>x.Descripcion).FirstOrDefault();
                        poObjectItem.NombreAlmacenDestino = poListaBodega.Where(x => x.Codigo == item.AlmacenDestino).Select(x => x.Descripcion).FirstOrDefault();
                        poObjectItem.CantidadTomada = item.CantidadTomada;
                        poObjectItem.Saldo = item.Saldo;
                        poObjectItem.LineNum = item.LineNum;
                        poObjectItem.TipoItem = toObject.TipoItem;
                        poObjectItem.Prorrateo = Convert.ToDecimal(item.Cantidad) / Convert.ToDecimal(piTotal);
                        poObjectItem.DescripcionServicio = item.DescripcionServicio;
                    }
                }

                List<string> psListaAdjuntoEliminar = new List<string>();

                List<int> poListaIdPeAJ = toObject.GuiaRemisionAdjunto.Select(x => x.IdGuiaRemisionAdjunto).ToList();
                List<int> piArchivoAdjuntoEliminar = poObject.COMTGUIAREMISIONADJUNTO.Where(x => !poListaIdPeAJ.Contains(x.IdGuiaRemisionAdjunto)).Select(x => x.IdGuiaRemisionAdjunto).ToList();
                foreach (var poItem in poObject.COMTGUIAREMISIONADJUNTO.Where(x => piArchivoAdjuntoEliminar.Contains(x.IdGuiaRemisionAdjunto)))
                {
                    psListaAdjuntoEliminar.Add(poItem.ArchivoAdjunto);
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                //Guardar Archivo Adjunto
                if (toObject.GuiaRemisionAdjunto != null)
                {
                    foreach (var poItem in toObject.GuiaRemisionAdjunto)
                    {
                        int pIdDetalle = poItem.IdGuiaRemisionAdjunto;

                        var poObjectItem = poObject.COMTGUIAREMISIONADJUNTO.Where(x => x.IdGuiaRemisionAdjunto == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;

                            if (poObjectItem.ArchivoAdjunto != poItem.ArchivoAdjunto)
                            {
                                psListaAdjuntoEliminar.Add(poObjectItem.ArchivoAdjunto);
                            }

                        }
                        else
                        {
                            poObjectItem = new COMTGUIAREMISIONADJUNTO();

                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;

                            poObject.COMTGUIAREMISIONADJUNTO.Add(poObjectItem);
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.ArchivoAdjunto = poItem.ArchivoAdjunto.Trim();
                        poObjectItem.NombreOriginal = poItem.NombreOriginal.Trim();
                        poObjectItem.Descripcion = poItem.Descripcion;

                    }
                }

                loBaseDa.SaveChanges();

                if (toOrdenPagoFactura != null)
                {
                    var poGuiaFactura = toOrdenPagoFactura.COMTGUIAREMISIONFACTURA.Where(x => x.CodigoEstado == Diccionario.Activo && x.IdOrdenPagoFactura == toOrdenPagoFactura.IdOrdenPagoFactura && x.IdGuiaRemision == poObject.IdGuiaRemision).FirstOrDefault();
                    if (poGuiaFactura != null)
                    {
                        poGuiaFactura.UsuarioModificacion = tsUsuario;
                        poGuiaFactura.TerminalModificacion = tsTerminal;
                        poGuiaFactura.FechaModificacion = DateTime.Now;
                    }
                    else
                    {
                        poGuiaFactura = new COMTGUIAREMISIONFACTURA();
                        loBaseDa.CreateNewObject(out poGuiaFactura);
                        poGuiaFactura.UsuarioIngreso = tsUsuario;
                        poGuiaFactura.TerminalIngreso = tsTerminal;
                        poGuiaFactura.FechaIngreso = DateTime.Now;
                        poGuiaFactura.CodigoEstado = Diccionario.Activo;
                    }

                    poGuiaFactura.IdOrdenPagoFactura = toOrdenPagoFactura.IdOrdenPagoFactura;
                    poGuiaFactura.Factura = toOrdenPagoFactura.Factura;
                    poGuiaFactura.IdGuiaRemision = poObject.IdGuiaRemision;
                    poGuiaFactura.Tipo = poObject.Tipo;
                    poGuiaFactura.Numero = poObject.Numero;
                }

                loBaseDa.SaveChanges();

                //Guardar Documentos Adjuntos
                foreach (var poItem in toObject.GuiaRemisionAdjunto)
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

                foreach (var psItem in psListaAdjuntoEliminar)
                {
                    string eliminar = ConfigurationManager.AppSettings["CarpetaGuiaRemision"].ToString() + psItem;
                    File.Delete(eliminar);
                }
            }

            return psMsg;
        }

        private string lsEsValidoGuia(GuiaRemision toObject)
        {
            string psMsg = "";

            if (toObject.Total == 0)
            {
                psMsg = psMsg + "Ingrese el Total de Flete  \n";
            }

            if (toObject.NumBultos == 0)
            {
                psMsg = psMsg + "Ingrese el número de bultos  \n";
            }

            var picont = loBaseDa.Find<COMTGUIAREMISION>().Where(x => x.CodigoEstado == Diccionario.Activo
            && x.IdGuiaRemision != toObject.IdGuiaRemision && x.CodProveedor == toObject.CodProveedor
            && x.Numero == toObject.Numero ).Count(); 

            if (picont > 0)
            {
                psMsg = psMsg + "Guía con #: " + toObject.Numero + " Ya está ingresada. \n";
            }

            int num = 1;
            if (toObject.TipoItem == "P") // Clase Producto
            {
                foreach (var item in toObject.GuiaRemisionDetalle)
                {
                    if (item.ItemCode == Diccionario.Seleccione)
                    {
                        psMsg = psMsg + "Falta seleccionar ítem en la fila:" + num + "\n";
                    }
                    if (item.Cantidad <= 0)
                    {
                        psMsg = psMsg + "Falta ingresar la cantidad en la fila: " + num + "\n";
                    }
                    if (item.Cantidad > item.Saldo)
                    {
                        psMsg = psMsg + "La cantidad ingresada no puede ser mayor que " + item.Saldo + " en la fila: " + num + "\n";
                    }
                    //if (item.Cantidad == item.CantidadOriginal)
                    //{
                    //    psMsg = psMsg + "La cantidad ingresada debe ser parcial y no total, corregir en la fila: " + num + "\n";
                    //}
                    num = num + 1;
                }

                // Comentado por un caso de Luis Reyna -- 20240822 VAR
                //int TotalOri = toObject.GuiaRemisionDetalle.Select(x => x.CantidadOriginal).Sum();
                //int TotalCant = toObject.GuiaRemisionDetalle.Select(x => x.Cantidad).Sum();
                //if (TotalOri == TotalCant)
                //{
                //    psMsg = psMsg + "La cantidad ingresada debe ser parcial y no total. \n";
                //}
            }
            else
            {
                int TotalOri = toObject.GuiaRemisionDetalle.Select(x => x.CantidadOriginal).Sum();
                int TotalCant = toObject.GuiaRemisionDetalle.Select(x => x.Cantidad).Sum();
                foreach (var item in toObject.GuiaRemisionDetalle)
                {
                    if (string.IsNullOrEmpty(item.DescripcionServicio))
                    {
                        psMsg = psMsg + "Debe ingresar la descripción en la fila:" + num + "\n";
                    }
                    if (item.Cantidad <= 0)
                    {
                        psMsg = psMsg + "Falta ingresar la cantidad en la fila: " + num + "\n";
                    }
                    num = num + 1;
                }
            }
            

            return psMsg;
        }

        public List<GuiaRemision> goListarGuiaRemision(string tsUsuario, int tIdMenu)
        {

            var poLista = new List<GuiaRemision>();

            List<string> psListaEstadoExclusion = new List<string>();
            psListaEstadoExclusion.Add(Diccionario.Inactivo);
            psListaEstadoExclusion.Add(Diccionario.Eliminado);

            var psListaUsuarioAsignado = loBaseDa.Find<SEGPUSUARIOASIGNADO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoUsuario == tsUsuario).Select(x => x.CodigoUsuarioAsignado).ToList();

            var poObject = loBaseDa.Find<COMTGUIAREMISION>().Include(x => x.COMTGUIAREMISIONDETALLE).Where(x => !psListaEstadoExclusion.Contains(x.CodigoEstado) && psListaUsuarioAsignado.Contains(x.UsuarioIngreso)).ToList();

            return LlenarDatosGuiaRemision(poObject);
        }

        public GuiaRemision goBuscarGuiaRemision(int tId)
        {
            var poLista = new List<GuiaRemision>();

            List<string> psListaEstadoExclusion = new List<string>();
            psListaEstadoExclusion.Add(Diccionario.Inactivo);
            psListaEstadoExclusion.Add(Diccionario.Eliminado);

            var poObject = loBaseDa.Find<COMTGUIAREMISION>().Include(x => x.COMTGUIAREMISIONDETALLE).Include(x=>x.COMTGUIAREMISIONADJUNTO)
                .Where(x => x.IdGuiaRemision == tId && !psListaEstadoExclusion.Contains(x.CodigoEstado)).ToList();

            return LlenarDatosGuiaRemision(poObject).FirstOrDefault();
        }

        private List<GuiaRemision> LlenarDatosGuiaRemision(List<COMTGUIAREMISION> poObject)
        {
            var poLista = new List<GuiaRemision>();

            var poUsuarios = loBaseDa.Find<SEGMUSUARIO>().ToList();

            foreach (var item in poObject)
            {
                var cab = new GuiaRemision();
                cab.IdGuiaRemision = item.IdGuiaRemision;
                cab.CodigoEstado = item.CodigoEstado;
                cab.Tipo = item.Tipo;
                cab.Local = item.Local;
                cab.PuntoEmision = item.PuntoEmision;
                cab.Secuencia = item.Secuencia;
                cab.Numero = item.Numero;
                cab.FechaEmision = item.FechaEmision ?? DateTime.MinValue;
                cab.FechaInicioTraslado = item.FechaInicioTraslado ?? DateTime.MinValue;
                cab.FechaTerminoTraslado = item.FechaTerminoTraslado ?? DateTime.MinValue;
                cab.CodigoMotivoTraslado = item.CodigoMotivoTraslado;
                cab.MotivoTraslado = item.MotivoTraslado;
                cab.CodigoTransportista = item.CodigoTransportista;
                cab.Transportista = item.Transportista;
                cab.IdentificacionTransportista = item.IdentificacionTransportista;
                cab.CodigoTransporte = item.CodigoTransporte;
                cab.Transporte = item.Transporte;
                cab.PuntoPartida = item.PuntoPartida;
                cab.CodCliente = item.CodCliente;
                cab.Cliente = item.Cliente;
                cab.CodProveedor = item.CodProveedor;
                cab.Proveedor = item.Proveedor;
                cab.IdentificacionCliente = item.IdentificacionCliente;
                cab.PuntoLlegada = item.PuntoLlegada;
                cab.ValorBulto = item.ValorBulto ?? 0;
                cab.NumBultos = item.NumBultos ?? 0;
                cab.DocEntry = item.DocEntry;
                cab.DocNum = item.DocNum;
                cab.CodVendedor = item.CodVendedor ?? 0;
                cab.Vendedor = item.Vendedor;
                cab.CodZonaVendedor = item.CodZonaVendedor;
                cab.ZonaVendedor = item.ZonaVendedor;
                cab.CodZonaTransporte = item.CodZonaTransporte;
                cab.ZonaTransporte = item.ZonaTransporte;
                //cab.IdOrdenPagoFactura = item.IdOrdenPagoFactura;
                cab.CodAlmacen = item.CodAlmacen;
                cab.CodAlmacenDestino = item.CodAlmacenDestino;
                cab.UsuarioSap = item.UsuarioSap;
                cab.FechaContabilizacion = item.FechaContabilizacion;
                cab.FechaDocumento = item.FechaDocumento;
                cab.FechaEntrega = item.FechaEntrega;
                cab.Total = item.Total;
                cab.Comentario = item.Comentario;
                cab.FolioNum = item.FolioNum;
                cab.Prefijo = item.Prefijo;
                cab.NumeroGuia = item.NumeroGuia;
                cab.IngresoManual = item.IngresoManual??false;
                cab.CodigoMotivoGuia = item.CodigoMotivoGuia;
                cab.TipoItem = item.TipoItem;
                cab.GR = item.GR;
                cab.Externa = item.Externa;
                cab.TipoTransporte = item.TipoTransporte;

                cab.Usuario = poUsuarios.Where(x => x.CodigoUsuario == item.UsuarioIngreso).Select(x => x.NombreCompleto).FirstOrDefault();

                foreach (var detalle in item.COMTGUIAREMISIONDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var det = new GuiaRemisionDetalle();
                    det.IdGuiaRemisionDetalle = detalle.IdGuiaRemisionDetalle;
                    det.IdGuiaRemision = detalle.IdGuiaRemision;
                    det.ItemCode = detalle.ItemCode;
                    det.ItemName = detalle.ItemName;
                    det.CantidadOriginal = detalle.CantidadOriginal;
                    det.AlmacenOrigen = detalle.AlmacenOrigen;
                    det.AlmacenDestino = detalle.AlmacenDestino;
                    det.CantidadTomada = detalle.CantidadTomada;
                    det.Cantidad = detalle.Cantidad;
                    det.LineNum = detalle.LineNum;
                    det.DescripcionServicio = detalle.DescripcionServicio;
                    det.TipoItem = detalle.TipoItem;
                    det.Prorrateo = detalle.Prorrateo??0m;
                    det.NombreAlmacenOrigen = detalle.NombreAlmacenOrigen;
                    det.NombreAlmacenDestino = detalle.NombreAlmacenDestino;
                    det.DescripcionServicio = detalle.DescripcionServicio;

                    cab.GuiaRemisionDetalle.Add(det);
                }


                foreach (var detalle in item.COMTGUIAREMISIONADJUNTO.Where(x=>x.CodigoEstado == Diccionario.Activo))
                {
                    var det = new GuiaRemisionAdjunto();

                    det.IdGuiaRemisionAdjunto = detalle.IdGuiaRemisionAdjunto;
                    det.IdGuiaRemision = detalle.IdGuiaRemision;
                    det.Descripcion = detalle.Descripcion;
                    det.ArchivoAdjunto = detalle.ArchivoAdjunto;
                    det.NombreOriginal = detalle.NombreOriginal;
                    det.RutaDestino = ConfigurationManager.AppSettings["CarpetaGuiaRemision"].ToString();
                    cab.GuiaRemisionAdjunto.Add(det);
                }

                poLista.Add(cab);
            }
            return poLista;

        }

        public string gEliminarMaestroGuiaRemision(int tiCodigo, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";

            var poListaGuiaFactura = loBaseDa.Find<COMTGUIAREMISIONFACTURA>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdGuiaRemision == tiCodigo).ToList();
            foreach (var item in poListaGuiaFactura)
            {
                psMsg = string.Format("No es posible eliminar guía remisión, tiene relacionada la factura #{0}.\n", item.Factura);
            }


            if (string.IsNullOrEmpty(psMsg))
            {

                var poObject = loBaseDa.Get<COMTGUIAREMISION>().Include(x => x.COMTGUIAREMISIONDETALLE).Where(x => x.IdGuiaRemision == tiCodigo).FirstOrDefault();
                if (poObject != null)
                {
                    if (poObject.IngresoManual == true)
                    {
                        poObject.CodigoEstado = Diccionario.Eliminado;
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.TerminalModificacion = tsTerminal;

                        foreach (var item in poObject.COMTGUIAREMISIONDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                        {
                            item.CodigoEstado = Diccionario.Eliminado;
                            item.UsuarioModificacion = tsUsuario;
                            item.TerminalModificacion = tsTerminal;
                        }

                        loBaseDa.SaveChanges();
                    }
                    else
                    {
                        psMsg = string.Format("No es posible eliminar guía remisión, viene de una factura en la opción orden de pago.\n");
                    }
                    
                }
            }


            return psMsg;
        }

        public string gsGetIdentificacionTransportista(string tsCode)
        {
            var dt = gdtDatosTransportista(tsCode);
            return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";
        }

        public string gsGetCodZonaDelVendedor(string tsCodigo)
        {
            var dt = loBaseDa.DataTable(string.Format("SELECT U_zona FROM [SBO_AFECOR].DBO.[@AFE_REL_VEN_ZONA] T0 WHERE T0.U_cod_vendedor = {0}", tsCodigo));
            return dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";
        }

        private DataTable gdtDatosTransportista(string tsCode)
        {
            return loBaseDa.DataTable(string.Format("SELECT U_ID_TRANSP IDENTIFICACION FROM [SBO_AFECOR].[DBO].[@EXX_TRANSPORTISTA] (NOLOCK) WHERE Code = '{0}'", tsCode));
        }

        public GuiaRemision goDatosClienteGuiaRemision(string CodCliente)
        {
            var poObjejt = new GuiaRemision();

            var dt = loBaseDa.DataTable(string.Format("SELECT LicTradNum FROM [SBO_AFECOR].[DBO].[OCRD] (NOLOCK) WHERE CARDCODE = '{0}'", CodCliente));
            poObjejt.IdentificacionCliente = dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";
            dt = loBaseDa.DataTable(string.Format("SELECT Street FROM [SBO_AFECOR].[DBO].[CRD1] (NOLOCK) WHERE CardCode = '{0}' AND Address = 'DESPACHO' ", CodCliente));
            poObjejt.PuntoLlegada = dt.Rows.Count > 0 ? dt.Rows[0][0].ToString() : "";
            return poObjejt;
        }

        public DataTable gdtConsultaGuiaRemisionSap(string tsUsuarioSap)
        {
            return loBaseDa.DataTable(string.Format("EXEC COMSPCONSULTAGUIASSAP '{0}'", tsUsuarioSap));
        }

        #endregion

        public List<Cotizacion> goListarMaestro(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);

            var poLista = (from SC in loBaseDa.Find<COMTORDENPAGO>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado } equals new { E.CodigoEstado }

                    join U in loBaseDa.Find<SEGMUSUARIO>()
                    on new { SC.UsuarioIngreso } equals new { UsuarioIngreso = U.CodigoUsuario }

                    join P in loBaseDa.Find<COMMPROVEEDORES>()
                    on new { Iden = SC.IdProveedor??0, Codigo = Diccionario.Activo } equals new { Iden = P.IdProveedor, Codigo = P.CodigoEstado }

                    // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado && psListaUsuario.Contains(SC.UsuarioIngreso)
                    // Selección de las columnas / Datos
                    select new Cotizacion
                    {
                        IdCotizacion = SC.IdOrdenPago,
                        DesUsuario = U.NombreCompleto,
                        FechaIngreso = SC.FechaIngreso,
                        CodigoEstado = SC.CodigoEstado,
                        DesEstado = E.Descripcion,
                        Usuario = SC.UsuarioIngreso,
                        Descripcion = SC.Observacion,
                        Terminal = SC.TerminalIngreso,
                        ComentarioAprobador = SC.ComentarioAprobador,
                        Proveedor = P.Nombre

                    }).ToList().OrderByDescending(x=>x.FechaIngreso).ToList();

            List<int> pListaId = poLista.Select(x=>x.IdCotizacion).ToList();
            var poListaFacturas = loBaseDa.Find<COMTORDENPAGOFACTURA>().Where(x => x.CodigoEstado != Diccionario.Eliminado && pListaId.Contains(x.IdOrdenPago)).Select(x=> new {x.IdOrdenPago, x.IdOrdenPagoFactura, x.Factura }).ToList();

            foreach (var factura in poLista)
            {
                bool entro = false;
                foreach (var item in poListaFacturas.Where(x=>x.IdOrdenPago == factura.IdCotizacion))
                {
                    entro = true;
                    factura.Comentario += item.Factura + ",";
                }

                if (entro)
                {
                    factura.Comentario = factura.Comentario.Substring(0, factura.Comentario.Length - 1);
                }
            }

            return poLista;

        }

        public OrdenPago goBuscarOrdenPago(int IdOrdenPago)
        {
            return (from OP in loBaseDa.Find<COMTORDENPAGO>()
                    join E in loBaseDa.Find<COMMPROVEEDORES>()
                    on new {x= OP.IdProveedor ?? 0} equals new { x= E.IdProveedor  }
                    where OP.CodigoEstado != Diccionario.Eliminado && OP.IdOrdenPago == IdOrdenPago
                    select new OrdenPago
                    {
                        IdOrdenPago = OP.IdOrdenPago,
                        Observacion = OP.Observacion,
                        CodigoEstado = OP.CodigoEstado,
                        ProveedorNombre = E.Identificacion,
                        CodigoTipoCompra = OP.CodigoTipoCompra,
                        CodigoTipoOrdenPago = OP.CodigoTipoOrdenPago,
                        FormaPago = OP.FormaPago,
                        Valor = OP.Valor,
                        Fecha = OP.FechaIngreso,
                        IdProveedor = OP.IdProveedor??0,
                        //DocEntry = OP.DocEntry??0
                    }).FirstOrDefault();

        }

        public List<Factura> goBuscarOrdenPagoFactura(int IdOrdenPago)
        {
            var poLista = loBaseDa.Find<COMTORDENPAGOFACTURA>().Where(x => x.IdOrdenPago == IdOrdenPago && x.CodigoEstado != Diccionario.Eliminado)
                 .Select(x => new Factura
                 {
                     CodigoEstado = x.CodigoEstado,
                     IdOrdenPago = x.IdOrdenPago,
                     IdFactura = x.IdOrdenPagoFactura,
                     NoFactura = x.Factura,
                     Descripcion = x.Descripcion,
                     FechaFactura = x.FechaFactura,
                     FechaVencimiento = x.FechaVencimiento,
                     Valor = x.Valor,
                     Observacion = x.Observacion,
                     SaldoOc = x.SaldoOrdenCompra ?? 0,
                     NombreOriginal = x.NombreOriginal,
                     ArchivoAdjunto = x.ArchivoAdjunto,
                     TipoTransporte = x.CodigoZonaFactura,
                     IdZonaGrupo = x.IdZonaGrupo ?? 0,
                     ZonaGrupo = x.ZonaGrupo,
                     Dias = x.DiasPago ?? 0,
                 }).ToList();

            foreach (var item in poLista)
            {
                var lista = new List<COMTGUIAREMISION>();
                var listaFac = loBaseDa.Find<COMTGUIAREMISIONFACTURA>().AsNoTracking().Where(x => x.IdOrdenPagoFactura == item.IdFactura && x.CodigoEstado == Diccionario.Activo).ToList() ;
                foreach (var fac in listaFac)
                {
                    lista.Add(fac.COMTGUIAREMISION);
                }
                
                if (lista != null && lista.Count > 0)
                {
                    var datos = LlenarDatosGuiaRemision(lista);
                    item.GuiaRemision = datos;
                }
            }

            return poLista;
        }

        public List<OrdenPagoAdjunto> goBuscarOrdenPagoArchivoAdjunto(int tId)
        {

            return loBaseDa.Find<COMTORDENPAGOADJUNTO>().Where(x => x.IdOrdenPago == tId && x.CodigoEstado != Diccionario.Eliminado)
               .ToList().Select(x => new OrdenPagoAdjunto
               {
                   IdOrdenPagoAdjunto = x.IdOrdenPagoAdjunto,
                   IdOrdenPago = x.IdOrdenPago,
                   Descripcion = x.Descripcion,
                   ArchivoAdjunto = x.ArchivoAdjunto,
                   NombreOriginal = x.NombreOriginal,
                   RutaDestino = ConfigurationManager.AppSettings["CarpetaOPCompras"].ToString()
               }).ToList();


        }

        public List<Proveedor> goBuscarListarProveedores(int tIdProveedor, List<int> id)
        {
            var psresult = loBaseDa.Find<COMTORDENCOMPRAPROVEEDOR>().Where(x => (x.CodigoEstado != Diccionario.Eliminado && x.CodigoEstado!= Diccionario.Cerrado) && x.IdProveedor == tIdProveedor)
                 .Select(x => new Proveedor
                 {IdProveedor = x.IdOrdenCompraProveedor,
                 Nombre =x.NombreProveedor,
                 Observacion = x.Observacion,
                 Total = x.TotalProveedor??0,
                  ValorAnticipo = x.ValorAnticipo ?? 0,
                  
                 }).ToList();

            foreach (var item in psresult)
            {
                //item.Saldo = item.Total - item.ValorAnticipo;
                if (id.Contains(item.IdProveedor))
                {
                    item.Sel = true;
                }
               
            }

            return psresult;

        }

        public List<Proveedor> goBuscarListarProveedores(string tsIdentificacion)
        {
            var psresult = loBaseDa.Find<COMTORDENCOMPRAPROVEEDOR>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdentificacionProveedor == tsIdentificacion)
                 .Select(x => new Proveedor
                 {
                     IdProveedor = x.IdOrdenCompraProveedor,
                     Nombre = x.NombreProveedor,
                     Observacion = x.Observacion,
                     Total = x.TotalProveedor ?? 0,
                     ValorAnticipo = x.ValorAnticipo ?? 0,

                 }).ToList();
            

            return psresult;

        }

        public string gEliminarMaestro(int tsCodigo, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";
            
            var poObject = loBaseDa.Get<COMTORDENPAGO>().Include(x => x.COMTORDENPAGOFACTURA).Include(x => x.COMTORDENPAGOADJUNTO).Where(x => x.IdOrdenPago == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                poObject.FechaModificacion = DateTime.Now;

                List<string> psListaEstado = new List<string>();
                psListaEstado.Add(Diccionario.Eliminado);
                psListaEstado.Add(Diccionario.Rechazado);

                foreach (var item in poObject.COMTORDENPAGOFACTURA.Where(x=>x.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Eliminado;
                    item.UsuarioModificacion = tsUsuario;
                    item.TerminalModificacion = tsTerminal;
                    item.FechaModificacion = DateTime.Now;

                    if (loBaseDa.Find<COMTFACTURAPAGO>().Where(x=> psListaEstado.Contains(x.CodigoEstado) && x.IdOrdenPagoFactura == item.IdOrdenPagoFactura).Count() > 0)
                    {
                        string.Format("{0}Factura: {1} Está siendo usada en otro proceso no es posible eliminar Orden de Pago \n", psMsg, item.Factura);
                    }

                    if (item.DocEntry != null)
                    {
                        string.Format("{0}Factura: {1} ha sido ligada con SAP, pedir que se deslique a contabilidad para poder eliminar Orden de Pago \n", psMsg, item.Factura);
                    }

                    foreach (var fac in item.COMTGUIAREMISIONFACTURA.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {

                        fac.COMTGUIAREMISION.UsuarioModificacion = tsUsuario;
                        fac.COMTGUIAREMISION.TerminalModificacion = tsTerminal;
                        fac.COMTGUIAREMISION.FechaModificacion = DateTime.Now;
                        if (fac.COMTGUIAREMISION.IngresoManual == false)
                        {
                            fac.COMTGUIAREMISION.CodigoEstado = Diccionario.Eliminado;

                            foreach (var Det in fac.COMTGUIAREMISION.COMTGUIAREMISIONDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                            {
                                Det.CodigoEstado = Diccionario.Eliminado;
                                Det.UsuarioModificacion = tsUsuario;
                                Det.TerminalModificacion = tsTerminal;
                                Det.FechaModificacion = DateTime.Now;
                            }
                        }

                        fac.CodigoEstado = Diccionario.Eliminado;
                        fac.UsuarioModificacion = tsUsuario;
                        fac.TerminalModificacion = tsTerminal;
                        fac.FechaModificacion = DateTime.Now;
                    }
                }

                foreach (var item in poObject.COMTORDENPAGOADJUNTO.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Eliminado;
                    item.UsuarioModificacion = tsUsuario;
                    item.TerminalModificacion = tsTerminal;   
                }

                if (string.IsNullOrEmpty(psMsg))
                {
                    loBaseDa.SaveChanges();
                }
                
            }

            return psMsg;
        }

        public List<BandejaOrdenPago> goListarBandeja(string tsUsuario, int tiMenu)
        {

            string psCodigoTransaccion = Diccionario.Tablas.Transaccion.OrdenPago;
            
            return loBaseDa.ExecStoreProcedure<BandejaOrdenPago>(string.Format("COMSPCONSULTABANDEJAORDENPAGO {0},'{1}','{2}'",tiMenu,tsUsuario,psCodigoTransaccion));

        }

        public string gsFacturasRelacionadasOrdenPago(int tId)
        {
            string psResult = "";

            var psListaEstado = new List<string>();
            psListaEstado.Add(Diccionario.Activo);
            psListaEstado.Add(Diccionario.Aprobado);

            var poLista = loBaseDa.Find<COMTORDENPAGOFACTURA>().Where(x => psListaEstado.Contains(x.CodigoEstado) && x.IdOrdenPago == tId).Select(x => x.Factura).ToList();
            foreach (var item in poLista)
            {
                psResult = string.Format("{0}, {1}",item,psResult);
            }

            if (!string.IsNullOrEmpty(psResult))
            {
                psResult = psResult.Substring(0, psResult.Length - 2);
            }

            return psResult;
        }

        public List<BandejaOrdenPago> goListarListado(string tsUsuario, DateTime tdFechaDesde, DateTime tdFechaHasta)
        {
            return loBaseDa.ExecStoreProcedure<BandejaOrdenPago>(string.Format("EXEC COMSPLISTADOORDENPAGO '{0}','{1}','{2}'", tsUsuario,tdFechaDesde.ToString("yyyyMMdd"), tdFechaHasta.ToString("yyyyMMdd")));
        }

        public List<FacturaDetalleGrid> goConsultaPreliminarFacturasPendientePago(DateTime tdFechaDesde, DateTime tdFechaHasta)
        {
            //return new List<FacturaDetalleGrid>();
            return loBaseDa.ExecStoreProcedure<FacturaDetalleGrid>(string.Format("EXEC [COMSPFACTURASPENDPAGOPRELIMINAR] '{0}', '{1}'", tdFechaDesde.Date, tdFechaHasta.Date));
        }

        public List<FacturaDetalleGrid> goConsultaFacturasPendientePago(DateTime tdFechaDesde, DateTime tdFechaHasta)
        {
            return loBaseDa.ExecStoreProcedure<FacturaDetalleGrid>(string.Format("EXEC [COMSPFACTURASPENDPAGO] '{0}', '{1}'", tdFechaDesde.Date, tdFechaHasta.Date));
        }

        public List<FacturaPorAprobarDetalleGrid> goConsultaFacturasPendientePagoPorAprobar(int tIdMenu, string tsUsuario, string tsGrupoPago)
        {
            return loBaseDa.ExecStoreProcedure<FacturaPorAprobarDetalleGrid>(string.Format("EXEC [COMSPFACTURASPENDPAGOXAPROBAR] {0}, '{1}', {2} ", tIdMenu, tsUsuario, tsGrupoPago));
        }

        public List<SpBandejaOrdenPagoContabilizar> goConsultaBandejaOrdenPagoContabilizar(DateTime tdDesde, DateTime tdHasta, bool tbTodos)
        {
            return loBaseDa.ExecStoreProcedure<SpBandejaOrdenPagoContabilizar>(string.Format("EXEC [COMSPBANDEJAORDENPAGOCONTABILIZAR] '{0}', '{1}', '{2}'", tdDesde.ToString("yyyy/MM/dd"), tdHasta.ToString("yyyy/MM/dd"), tbTodos ? "1" : "0"));
        }

        public List<FacturaAprobadasDetalleGrid> goConsultaFacturasPendientePagoTesoreria(string tsGrupoPago)
        {
            return loBaseDa.ExecStoreProcedure<FacturaAprobadasDetalleGrid>(string.Format("EXEC COMSPFACTURASPENDPAGOTESORERIA {0}",tsGrupoPago));
        }

        public List<FacturaAprobadasDetalleGrid> goConsultaHistorialFacturasPendientePagoTesoreria(DateTime tdFechaDesde, DateTime tdFechaHasta)
        {
            //DbFunctions.TruncateTime(x.FechaInicio) >= DbFunctions.TruncateTime(DateTime.Now)
            return loBaseDa.Find<COMTFACTURAPAGO>().Where(x =>
            x.CodigoEstado == Diccionario.Aprobado &&
            DbFunctions.TruncateTime(x.FechaPagoTesoreria) >= DbFunctions.TruncateTime(tdFechaDesde) &&
            DbFunctions.TruncateTime(x.FechaPagoTesoreria) <= DbFunctions.TruncateTime(tdFechaHasta)
            ).ToList().Select(x => new FacturaAprobadasDetalleGrid()
            {
                ArchivoAdjunto = x.ArchivoAdjunto,
                NombreOriginal = x.NombreOriginal,
                Proveedor = x.Proveedor,
                IdFacturaPago = x.IdFacturaPago,
                Identificacion = x.Identificacion,
                Nombre = x.Proveedor,
                NumDocumento = x.NumDocumento,
                DocNum = x.DocNum,
                FechaEmision = x.FechaEmision,
                FechaVencimiento = x.FechaVencimiento,
                Valor = x.Valor,
                Abono = x.Abono,
                Saldo = x.Saldo,
                ValorPago = x.ValorPago,
                Observacion = x.Observacion,
                IdOrdenPagoFactura = x.IdOrdenPagoFactura ?? 0,
                CodigoEstado = x.CodigoEstado,
                FechaPago = x.FechaPagoTesoreria ?? DateTime.Now,
                IdSemanaPago = x.IdSemanaPago.ToString(),
                RutaDestino = ConfigurationManager.AppSettings["CarpetaOPBcoCompras"].ToString(),
                ComentarioAprobador = x.ComentarioAprobador,
                ValorPagoOriginal = x.ValorPagoOriginal??0,
                IdGrupoPago = x.IdGrupoPagos??0,
                GrupoPago = x.GrupoPagos,
                IdOrdenPago = x.IdOrdenPago??0,
                IdProveedor = x.IdProveedor,
            }).ToList();

            
        }

        public List<FacturaAprobadasDetalleGrid> goConsultaFacturasPendientePagoFinanciero(string tsGrupoPago)
        {
            return loBaseDa.ExecStoreProcedure<FacturaAprobadasDetalleGrid>(string.Format("EXEC COMSPFACTURASPENDPAGOFINANCIERO {0}", tsGrupoPago));
        }

        public List<ProveedorCuentaBancaria> goConsultarCuentasProveedor(int tIdProveedor)
        {
            return (from a in loBaseDa.Find<COMMPROVEEDORES>()
                    join b in loBaseDa.Find<COMMPROVEEDORESCUENTABANC>() on a.IdProveedor equals b.IdProveedor
                    join c in loBaseDa.Find<GENMCATALOGO>() on new { CodigoGrupo = Diccionario.ListaCatalogo.TipoIdentificacion, Codigo = b.CodigoTipoIdentificacion } equals new { c.CodigoGrupo, c.Codigo }
                    join d in loBaseDa.Find<GENMCATALOGO>() on new { CodigoGrupo = Diccionario.ListaCatalogo.Banco, Codigo = b.CodigoBanco } equals new { d.CodigoGrupo, d.Codigo }
                    join e in loBaseDa.Find<GENMCATALOGO>() on new { CodigoGrupo = Diccionario.ListaCatalogo.TipoCuentaBancaria, Codigo = b.CodigoTipoCuentaBancaria } equals new { e.CodigoGrupo, e.Codigo }
                    join f in loBaseDa.Find<GENMCATALOGO>() on new { CodigoGrupo = Diccionario.ListaCatalogo.FormaPago, Codigo = b.CodigoFormaPago } equals new { f.CodigoGrupo, f.Codigo }
                    where a.CodigoEstado == Diccionario.Activo && a.IdProveedor == tIdProveedor && b.CodigoEstado == Diccionario.Activo
                    select new ProveedorCuentaBancaria()
                    {
                        CodigoBanco = b.CodigoBanco,
                        DesBanco = d.Descripcion,
                        CodigoFormaPago = b.CodigoFormaPago,
                        DesFormaPago = f.Descripcion,
                        CodigoTipoCuentaBancaria = b.CodigoTipoCuentaBancaria,
                        DesTipoCuentaBancaria = e.Descripcion,
                        CodigoTipoIdentificacion = b.CodigoTipoIdentificacion,
                        DesTipoIdentificacion = c.Descripcion,
                        Identificacion = b.Identificacion,
                        IdProveedor = b.IdProveedor,
                        IdProveedorCuenta = b.IdProveedorCuenta,
                        Nombre = b.Nombre,
                        Principal = b.Principal,
                        NumeroCuenta = b.NumeroCuenta,
                    }
                    ).ToList();
        }
        
        public string gsGuardarFacturaPago(List<FacturaDetalleGrid> toLista,List<int> ListIdEliminar, string tsUsuario, string tsTerminal, string tsSemana)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if ((toLista != null && toLista.Count > 0) || ListIdEliminar.Count > 0)
            {
                List<string> psListaEstado = new List<string>();
                psListaEstado.Add(Diccionario.Pendiente);
                psListaEstado.Add(Diccionario.Rechazado);
                psListaEstado.Add(Diccionario.Corregir);

                var poLista = loBaseDa.Get<COMTFACTURAPAGO>().Where(x => psListaEstado.Contains(x.CodigoEstado) && ListIdEliminar.Contains(x.IdFacturaPago)).ToList();

                using (var poTran = new TransactionScope())
                {
                    foreach (var item in poLista)
                    {
                        item.CodigoEstado = Diccionario.Eliminado;
                        item.UsuarioModificacion = tsUsuario;
                        item.FechaModificacion = DateTime.Now;
                        item.TerminalModificacion = tsTerminal;
                    }

                    foreach (var item in toLista)
                    {
                        bool pbGuardaSeg = false;
                        var poObject = loBaseDa.Get<COMTFACTURAPAGO>().Where(x => x.IdFacturaPago == item.IdFacturaPago && item.IdFacturaPago != 0).FirstOrDefault();
                        if (poObject != null)
                        {
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObject = new COMTFACTURAPAGO();
                            loBaseDa.CreateNewObject(out poObject);
                            poObject.UsuarioIngreso = tsUsuario;
                            poObject.FechaIngreso = DateTime.Now;
                            poObject.TerminalIngreso = tsTerminal;
                            pbGuardaSeg = true;
                        }

                        string psCodigoEstado = "";
                        string psCodigoEstadoAnt = poObject.CodigoEstado;
                        if (item.CodigoEstado == Diccionario.Rechazado || item.CodigoEstado == Diccionario.Corregir || item.CodigoEstado == Diccionario.Pendiente)
                        {
                            psCodigoEstado = Diccionario.Pendiente;
                            pbGuardaSeg = true;
                        }
                        else
                        {
                            psCodigoEstado = item.CodigoEstado;
                        }

                        poObject.CodigoEstado = psCodigoEstado;

                        if (item.CodigoEstado == Diccionario.Pendiente)
                        {
                            item.Comentario = "";
                        }

                        poObject.ComentarioTesoreria = item.ComentarioTesoreria.Length > 200 ? item.ComentarioTesoreria.Substring(0, 200) : item.ComentarioTesoreria;
                        poObject.Identificacion = item.Identificacion;
                        poObject.IdProveedor = item.IdProveedor;
                        poObject.Proveedor = item.Proveedor;
                        poObject.NumDocumento = item.NumDocumento;
                        poObject.DocNum = item.DocNum;
                        poObject.FechaEmision = item.FechaEmision;
                        poObject.FechaVencimiento = item.FechaVencimiento;
                        poObject.Valor = item.Valor;
                        poObject.Abono = item.Abono;
                        poObject.Saldo = item.Saldo;
                        poObject.ValorPago = item.ValorPago;
                        poObject.ValorPagoOriginal = item.ValorPago;
                        poObject.Observacion = item.Observacion.Length > 200 ? item.Observacion.Substring(0, 200) : item.Observacion;
                        poObject.ComentarioAprobadorOP = item.ComentarioAprobadorOP;
                        poObject.UsuarioAprobadorOP = item.UsuarioAprobadorOP;
                        poObject.IdOrdenPagoFactura = item.IdOrdenPagoFactura;
                        poObject.IdOrdenPago = item.IdOrdenPago;
                        poObject.IdGrupoPagos = item.IdGrupoPago;
                        poObject.GrupoPagos = item.GrupoPago;


                        loBaseDa.SaveChanges();

                        if (pbGuardaSeg)
                        {
                            REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTransaccion);
                            poTransaccion.CodigoEstado = Diccionario.Activo;
                            poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.FacturaPago;
                            poTransaccion.ComentarioAprobador = tsSemana;
                            poTransaccion.IdTransaccionReferencial = poObject.IdFacturaPago;
                            poTransaccion.UsuarioAprobacion = tsUsuario;
                            poTransaccion.UsuarioIngreso = tsUsuario;
                            poTransaccion.FechaIngreso = DateTime.Now;
                            poTransaccion.TerminalIngreso = tsTerminal;
                            poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(psCodigoEstadoAnt);
                            poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psCodigoEstado);
                            poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;
                        }


                    }

                    loBaseDa.SaveChanges();
                    poTran.Complete();
                }
                

            }

            return psMsg;
        }

        public List<ArchivoPagoMultiCashTxt> goArchivoPagosMultiCash(List<FacturaAprobadasDetalleGrid> toLista, string tsUsuario, out string tsMsg, out string tsNameFile)
        {
            var poLista = new List<ArchivoPagoMultiCashTxt>();

            tsMsg = lValidaLista(toLista);
            tsNameFile = "";

            if (string.IsNullOrEmpty(tsMsg))
            {
                loBaseDa.CreateContext();

                int tIdSemana = int.Parse(goConsultarComboSemanaPagos().FirstOrDefault().Codigo);

                var poListaFormaPago = loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoGrupo == Diccionario.ListaCatalogo.FormaPago).Select(x => new { x.Codigo, x.CodigoAlterno1 }).ToList();
                var poListaTipoCuenta = loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoGrupo == Diccionario.ListaCatalogo.TipoCuentaBancaria).Select(x => new { x.Codigo, x.CodigoAlterno2 }).ToList();
                var poListaBanco = loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoGrupo == Diccionario.ListaCatalogo.Banco).Select(x => new { x.Codigo, x.NumeroAlterno1 }).ToList();
                var poListaProveedor = loBaseDa.Find<COMMPROVEEDORES>().Where(x => x.CodigoEstado == Diccionario.Activo ).Select(x => new { x.Identificacion, x.Correo }).ToList();

                var poParametro = loBaseDa.Find<COMPPARAMETRO>().Where(x => x.CodigoEstado == Diccionario.Activo).FirstOrDefault();

                int piSecuenciaInicial = 1;
                DateTime pdFecha = DateTime.Now;
                REHPSECUENCIAPAGO poSecuencia = loBaseDa.Get<REHPSECUENCIAPAGO>().Where(x => x.CodigoTipoSecPago == Diccionario.Tablas.TipoSecuencia.MultiCash && x.Fecha == pdFecha.Date).FirstOrDefault();
                if (poSecuencia != null)
                {
                    poSecuencia.SecuenciaSiguiente += 1;
                    poSecuencia.UsuarioModificacion = tsUsuario;
                    poSecuencia.FechaModificacion = pdFecha;

                }
                else
                {
                    poSecuencia = new REHPSECUENCIAPAGO();
                    loBaseDa.CreateNewObject(out poSecuencia);
                    poSecuencia.Fecha = pdFecha.Date;
                    poSecuencia.CodigoTipoSecPago = Diccionario.Tablas.TipoSecuencia.MultiCash;
                    poSecuencia.SecuenciaSiguiente = piSecuenciaInicial;
                    poSecuencia.UsuarioCreacion = tsUsuario;
                    poSecuencia.FechaIngreso = pdFecha;
                    poSecuencia.TerminalIngreso = "";
                }
                

                tsNameFile = string.Format("PAGOS_MULTICASH_{0}_{1}.TXT", pdFecha.ToString("yyyyMMdd"), poSecuencia.SecuenciaSiguiente.ToString("00"));

                List<int> piListasId = toLista.Select(x => x.IdFacturaPago).ToList();
                var poListaFacturas = loBaseDa.Get<COMTFACTURAPAGO>().Where(x => piListasId.Contains(x.IdFacturaPago)).ToList();

                foreach (var item in poListaFacturas)
                {
                    item.PagadoTesoreria = true;
                    item.UsuarioPagoTesoreria = tsUsuario;
                    item.FechaPagoTesoreria = pdFecha;
                    item.IdSemanaPago = tIdSemana;

                    REHTTRANSACCIONAUTOIZACION poTran = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTran);
                    poTran.CodigoEstado = Diccionario.Activo;
                    poTran.CodigoTransaccion = Diccionario.Tablas.Transaccion.FacturaPago;
                    poTran.ComentarioAprobador = "Generación de Archivo de Pago";
                    poTran.IdTransaccionReferencial = item.IdFacturaPago;
                    poTran.UsuarioAprobacion = tsUsuario;
                    poTran.UsuarioIngreso = tsUsuario;
                    poTran.FechaIngreso = DateTime.Now;
                    poTran.TerminalIngreso = "";
                    poTran.EstadoAnterior = "";
                    poTran.EstadoPosterior = "GENERADO";
                    poTran.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    //Actualizar el valor de pago
                    item.ValorPago = toLista.Where(x => x.IdFacturaPago == item.IdFacturaPago).Select(x => x.ValorPago).FirstOrDefault();
                }

                int piSecuencia = 1;
                foreach (var item in toLista)
                {
                    var poReg = poLista.Where(x => x.Identificacion == item.IdentificacionCuenta).FirstOrDefault();
                    if (poReg != null)
                    {
                        string psFacturas = string.Format("{0}, {1}", poReg.Referencia, item.NumDocumento);
                        poReg.Referencia = psFacturas.Length > 200 ? psFacturas.Substring(0, 200) : psFacturas;
                        poReg.ValorDec += item.ValorPago;
                    }
                    else
                    {
                        var poRegistro = new ArchivoPagoMultiCashTxt();
                        poRegistro.Pa = poParametro.CodigoEmpresaMultiCash;
                        poRegistro.CuentaEmpresa = poParametro.CuentaBancariaEmpresa;
                        poRegistro.Secuencia = piSecuencia;
                        poRegistro.NumCompOP = "";
                        poRegistro.CuentaBenef = item.NumeroCuenta;
                        poRegistro.Moneda = "USD";
                        poRegistro.ValorDec = item.ValorPago;
                        string psValor = Convert.ToInt32(item.ValorPago * 100).ToString();
                        poRegistro.Valor = new String('0', 13 - psValor.Length) + psValor;
                        poRegistro.FormaPago = poListaFormaPago.Where(x => x.Codigo == item.CodigoFormaPago).Select(x => x.CodigoAlterno1).FirstOrDefault();
                        int piCodigoSpi = poListaBanco.Where(x => x.Codigo == item.CodigoBanco).Select(x => x.NumeroAlterno1).FirstOrDefault() ?? 0;
                        poRegistro.CodigoIntFin = piCodigoSpi.ToString("0000");
                        poRegistro.TipoCuenta = poListaTipoCuenta.Where(x => x.Codigo == item.CodigoTipoCuentaBancaria).Select(x => x.CodigoAlterno2).FirstOrDefault();
                        poRegistro.CuentaAcreditar = item.NumeroCuenta;
                        poRegistro.TipoIdentificacion = item.CodigoTipoIdentificacion.Substring(0, 1);
                        poRegistro.Identificacion = item.IdentificacionCuenta;
                        string psBeneficiario = gRemoverCaracteresEspeciales(item.Nombre);
                        poRegistro.Beneficiario = psBeneficiario.Length > 40 ? psBeneficiario.Substring(0, 40) : psBeneficiario;
                        poRegistro.DireccionOP = "";
                        poRegistro.CiudadOP = "";
                        poRegistro.TelefonoOP = "";
                        poRegistro.LocalidadOP = "";
                        string psReferencia = gRemoverCaracteresEspeciales(item.Observacion);
                        //poRegistro.Referencia = psReferencia.Length > 200 ? psReferencia.Substring(0,200): psReferencia;
                        
                        poRegistro.Referencia = item.NumDocumento;
                        string psCorreo = poListaProveedor.Where(x => x.Identificacion == item.Identificacion).Select(x => x.Correo).FirstOrDefault();
                        int LenCorreo = string.IsNullOrEmpty(psCorreo) ? 0 : psCorreo.Length;
                        string psNotificaciones = psReferencia.Length > (100 - LenCorreo - 1) ? psReferencia.Substring(0, (100 - LenCorreo - 1)) : psReferencia;
                        poRegistro.Notificaciones = psNotificaciones + "|" + psCorreo;

                        piSecuencia++;

                        poLista.Add(poRegistro);
                    }
                }

                foreach (var item in poLista)
                {
                    string psValor = Convert.ToInt32(item.ValorDec * 100).ToString();
                    item.Valor = new String('0', 13 - psValor.Length) + psValor;
                }
                loBaseDa.SaveChanges();
            }

            return poLista;
        }

        public string goGeneraPagoFinanciero(List<FacturaAprobadasDetalleGrid> toLista, string tsUsuario)
        {
            
            string psMsg = "";
            
            if (string.IsNullOrEmpty(psMsg))
            {
                loBaseDa.CreateContext();

                
                List<int> piListasId = toLista.Select(x => x.IdFacturaPago).ToList();
                var poListaFacturas = loBaseDa.Get<COMTFACTURAPAGO>().Where(x => piListasId.Contains(x.IdFacturaPago)).ToList();

                foreach (var item in poListaFacturas)
                {
                    item.PagadoFinanciero = true;
                    item.UsuarioPagoFinanciero = tsUsuario;
                    item.FechaPagoFinanciero = DateTime.Now;

                    REHTTRANSACCIONAUTOIZACION poTran = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTran);
                    poTran.CodigoEstado = Diccionario.Activo;
                    poTran.CodigoTransaccion = Diccionario.Tablas.Transaccion.FacturaPago;
                    poTran.ComentarioAprobador = "Aprobación de Archivo de Pago";
                    poTran.IdTransaccionReferencial = item.IdFacturaPago;
                    poTran.UsuarioAprobacion = tsUsuario;
                    poTran.UsuarioIngreso = tsUsuario;
                    poTran.FechaIngreso = DateTime.Now;
                    poTran.TerminalIngreso = "";
                    poTran.EstadoAnterior = "";
                    poTran.EstadoPosterior = "APROBADO";
                    poTran.Tipo = Diccionario.TipoAprobacion.Seguimiento;
                }

                loBaseDa.SaveChanges();
            }

            return psMsg;
        }

        public List<SpFacturasPendPagoContabilidad> goConsultaFacturasPendientePagoContabilidad(DateTime tdFechaDesde, DateTime tdFechaHasta)
        {
            return loBaseDa.ExecStoreProcedure<SpFacturasPendPagoContabilidad>(string.Format("EXEC [COMSPFACTURASPENDPAGOCONTABILIDAD] '{0}', '{1}'", tdFechaDesde.Date, tdFechaHasta.Date));
        }

        private string lValidaLista(List<FacturaAprobadasDetalleGrid> toLista)
        {
            string psMsg = "";

            if (toLista.Where(x=>string.IsNullOrEmpty(x.NumeroCuenta)).Count() > 0)
            {
                psMsg = string.Format("{0}Existen registros seleccionados sin cuenta bancaria\n", psMsg);
            }

            return psMsg;

        }

        public void gActualizarEstadoOrdenCompraProveedor( string tsUsuario, string tsTerminal, List<int> IdOrdecompraProveedor)
        {
            loBaseDa.CreateContext();
            foreach (var proveedor in IdOrdecompraProveedor)
                {
                    var poObjectOC = loBaseDa.Get<COMTORDENCOMPRAPROVEEDOR>().Where(x => x.IdOrdenCompraProveedor == proveedor && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();


                    // var poObjectOC = loBaseDa.Get<COMTORDENCOMPRA>().Include(x => x.COMTORDENCOMPRAPROVEEDOR.Where(y => y.IdOrdenCompraProveedor == proveedor && y.CodigoEstado != Diccionario.Eliminado).FirstOrDefault()).Where(x => x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();
                    if (poObjectOC != null)
                    {
                        poObjectOC.CodigoEstado = Diccionario.Cerrado;
                        poObjectOC.FechaModificacion = DateTime.Now;
                        poObjectOC.UsuarioModificacion = tsUsuario;
                        poObjectOC.TerminalModificacion = tsTerminal;
                    }

                }
                loBaseDa.SaveChanges();

                loBaseDa.CreateContext();
                foreach (var item in IdOrdecompraProveedor)
                {
                    var poObjectOCP = loBaseDa.Get<COMTORDENCOMPRAPROVEEDOR>().Where(x => x.IdOrdenCompraProveedor == item && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();
                    var poObjectOC = loBaseDa.Get<COMTORDENCOMPRA>().Where(x => x.IdOrdenCompra == poObjectOCP.IdOrdenCompra && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();
                    var poLista = poObjectOC.COMTORDENCOMPRAPROVEEDOR.Where(x => x.CodigoEstado == Diccionario.Cerrado);
                    if (poLista.Count() == poObjectOC.COMTORDENCOMPRAPROVEEDOR.Count())
                    {
                        poObjectOC.CodigoEstado = Diccionario.Cerrado;
                        poObjectOC.FechaModificacion = DateTime.Now;
                        poObjectOC.UsuarioModificacion = tsUsuario;
                        poObjectOC.TerminalModificacion = tsTerminal;
                    }

                }
                loBaseDa.SaveChanges();
        }

        public string goBuscarCantidadAprobacion(int tiId)
        {

            string psCodigoTransaccion = Diccionario.Tablas.Transaccion.OrdenPago;
            string lCantidad;
            var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == psCodigoTransaccion).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
            var y = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                 x.CodigoEstado == Diccionario.Activo &&
                 x.CodigoTransaccion == psCodigoTransaccion &&
                 x.IdTransaccionReferencial == tiId
                 && x.Tipo == Diccionario.TipoAprobacion.Aprobado
                 ).Count();
            lCantidad = y + "/" + piCantidadAutorizacion;
            return lCantidad;
        }

        public string goBuscarUsuarioAprobacion(int tiId)
        {

            string psCodigoTransaccion = Diccionario.Tablas.Transaccion.OrdenPago;
            string sNombre = "";
            var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == psCodigoTransaccion).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
            var y = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                 x.CodigoEstado == Diccionario.Activo &&
                 x.CodigoTransaccion == psCodigoTransaccion &&
                 x.IdTransaccionReferencial == tiId
                 && x.Tipo == Diccionario.TipoAprobacion.Aprobado
                 ).ToList();
            foreach (var usuario in y)
            {
                sNombre = sNombre + usuario.UsuarioAprobacion + ", ";
            }

            return sNombre;
        }

       
        /// <summary>
        /// Aprobar Comisiones
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsAprobar(int tId, string tsComentario, string tsUsuario, string tsTerminal, int tIdMenu)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;
            
            var poObject = loBaseDa.Get<COMTORDENPAGO>().Include(x=>x.COMTORDENPAGOFACTURA).Where(x => x.IdOrdenPago == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();
            
            if (poObject != null)

            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.OrdenPago;
                    int pId = tId;
                    var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == psCodigoTransaccion).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
                    List<string> psUsuario = new List<string>();
                    if (piCantidadAutorizacion > 0)
                    {
                        psUsuario = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                        x.CodigoEstado == Diccionario.Activo &&
                        x.CodigoTransaccion == psCodigoTransaccion &&
                        x.IdTransaccionReferencial == pId
                        && x.Tipo == Diccionario.TipoAprobacion.Aprobado
                        ).Select(x => x.UsuarioAprobacion).Distinct().ToList();

                        if (psUsuario.Contains(tsUsuario))
                        {
                            psResult = "Ya existe una aprobación con el usuario: " + tsUsuario + ". \n";
                        }

                        if (psUsuario.Count >= piCantidadAutorizacion)
                        {
                            psResult += "Transacción ya cuenta con la cantidad de aprobaciones necesarias. \n";
                        }
                    }


                    if (string.IsNullOrEmpty(psResult))
                    {

                        string psCodigoEstado = string.Empty;

                        // Usuario parametrizado para que su aprobación sea la definitiva
                        bool pbAprobacionFinal = false;
                        var psCodigoUsuario = loBaseDa.Find<SEGPUSUARIOAPROBACIONEXCEPCION>()
                            .Where(x => x.CodigoEstado == Diccionario.Activo && x.IdMenu == tIdMenu && x.CodigoUsuario == tsUsuario).Select(x => x.CodigoUsuario)
                            .FirstOrDefault();

                        if (!string.IsNullOrEmpty(psCodigoUsuario)) pbAprobacionFinal = true;

                        
                        if (pbAprobacionFinal)
                        {
                            psCodigoEstado = Diccionario.Aprobado;
                        }
                        else
                        {
                            // Se agrega una autorización más por la que se va a guardar en este proceso
                            if ((psUsuario.Count + 1) == piCantidadAutorizacion)
                            {
                                psCodigoEstado = Diccionario.Aprobado;
                            }
                            else
                            {
                                psCodigoEstado = Diccionario.PreAprobado;
                            }
                        }

                        if (psCodigoEstado == Diccionario.Aprobado)
                        {

                            //psCodigoEstado = Diccionario.Aprobado;
                            poObject.ComentarioAprobador = tsComentario;
                            poObject.UsuarioAprobacion = tsUsuario;
                            poObject.FechaAprobacion = DateTime.Now;

                            foreach (var item in poObject.COMTORDENPAGOFACTURA.Where(x => x.CodigoEstado == Diccionario.Activo))
                            {
                                item.CodigoEstado = Diccionario.Aprobado;
                                item.UsuarioModificacion = tsUsuario;
                                item.FechaModificacion = DateTime.Now;

                                var poFactPago = loBaseDa.Get<COMTFACTURAPAGO>().Where(x => x.IdOrdenPagoFactura == item.IdOrdenPagoFactura).FirstOrDefault();
                                if (poFactPago != null)
                                {

                                    REHTTRANSACCIONAUTOIZACION poTran = new REHTTRANSACCIONAUTOIZACION();
                                    loBaseDa.CreateNewObject(out poTran);
                                    poTran.CodigoEstado = Diccionario.Activo;
                                    poTran.CodigoTransaccion = Diccionario.Tablas.Transaccion.FacturaPago;
                                    poTran.ComentarioAprobador = "";
                                    poTran.IdTransaccionReferencial = poFactPago.IdFacturaPago;
                                    poTran.UsuarioAprobacion = tsUsuario;
                                    poTran.UsuarioIngreso = tsUsuario;
                                    poTran.FechaIngreso = DateTime.Now;
                                    poTran.TerminalIngreso = tsTerminal;
                                    poTran.EstadoAnterior = Diccionario.gsGetDescripcion(poFactPago.CodigoEstado);
                                    poTran.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Pendiente);
                                    poTran.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                                    poFactPago.CodigoEstado = Diccionario.Pendiente;
                                    poFactPago.UsuarioModificacion = tsUsuario;
                                    poFactPago.FechaModificacion = DateTime.Now;

                                }
                            }
                        }
                        

                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTransaccion);
                        poTransaccion.CodigoEstado = Diccionario.Activo;
                        poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                        poTransaccion.ComentarioAprobador = tsComentario;
                        poTransaccion.IdTransaccionReferencial = pId;
                        poTransaccion.UsuarioAprobacion = tsUsuario;
                        poTransaccion.UsuarioIngreso = tsUsuario;
                        poTransaccion.FechaIngreso = DateTime.Now;
                        poTransaccion.TerminalIngreso = tsTerminal;
                        poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                        poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psCodigoEstado);
                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Aprobado;


                        poObject.CodigoEstado = psCodigoEstado;
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;

                        // Insert Auditoría
                        // loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);

                       // poObject.COMTCOTIZACIONGANADORA.Where(x => x.CodigoEstado == Diccionario.Pendiente || x.CodigoEstado == Diccionario.PreAprobado).ToList().ForEach(x => x.CodigoEstado = psCodigoEstado);
                       // poObject.COMTCOTIZACIONSOLICITUDCOMPRA.Where(x => x.CodigoEstado == Diccionario.Pendiente || x.CodigoEstado == Diccionario.PreAprobado).ToList().ForEach(x => x.CodigoEstado = psCodigoEstado);
                        loBaseDa.SaveChanges();
                    }


                }
                else
                {
                    psResult = "Orden de pago ya aprobada!";
                }
            }
            else
            {
                psResult = "No existe orden de pago por aprobar";
            }
            return psResult;

        }
        
        /// <summary>
        /// Aprobar Factura de Pago
        /// </summary>
        /// <param name="tId"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsAprobarFacturaPago(int tId, string tsComentario, string tsUsuario, string tsTerminal, int tIdMenu)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTFACTURAPAGO>().Where(x => x.IdFacturaPago == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();
            
            if (poObject != null)

            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.FacturaPago;
                    int pId = tId;
                    var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == psCodigoTransaccion).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
                    List<string> psUsuario = new List<string>();
                    if (piCantidadAutorizacion > 0)
                    {
                        psUsuario = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                        x.CodigoEstado == Diccionario.Activo &&
                        x.CodigoTransaccion == psCodigoTransaccion &&
                        x.IdTransaccionReferencial == pId
                        && x.Tipo == Diccionario.TipoAprobacion.Aprobado
                        ).Select(x => x.UsuarioAprobacion).Distinct().ToList();

                        if (psUsuario.Contains(tsUsuario))
                        {
                            psResult = "Ya existe una aprobación con el usuario: " + tsUsuario + ". \n";
                        }

                        if (psUsuario.Count >= piCantidadAutorizacion)
                        {
                            psResult += "Transacción ya cuenta con la cantidad de aprobaciones necesarias. \n";
                        }
                    }


                    if (string.IsNullOrEmpty(psResult))
                    {
                        // Usuario parametrizado para que su aprobación sea la definitiva
                        bool pbAprobacionFinal = false;
                        var psCodigoUsuario = loBaseDa.Find<SEGPUSUARIOAPROBACIONEXCEPCION>()
                            .Where(x => x.CodigoEstado == Diccionario.Activo && x.IdMenu == tIdMenu && x.CodigoUsuario == tsUsuario).Select(x => x.CodigoUsuario)
                            .FirstOrDefault();

                        if (!string.IsNullOrEmpty(psCodigoUsuario)) pbAprobacionFinal = true;

                        
                        string psCodigoEstado = string.Empty;
                        if (pbAprobacionFinal)
                        {
                            psCodigoEstado = Diccionario.Aprobado;
                        }
                        else
                        {
                            // Se agrega una autorización más por la que se va a guardar en este proceso
                            if ((psUsuario.Count + 1) == piCantidadAutorizacion)
                            {

                                psCodigoEstado = Diccionario.Aprobado;
                                poObject.FechaAprobacion = DateTime.Now;
                                poObject.UsuarioAprobacion = tsUsuario;
                            }
                            else
                            {
                                psCodigoEstado = Diccionario.PreAprobado;
                            }
                        }
                        

                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTransaccion);
                        poTransaccion.CodigoEstado = Diccionario.Activo;
                        poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                        poTransaccion.ComentarioAprobador = tsComentario;
                        poTransaccion.IdTransaccionReferencial = pId;
                        poTransaccion.UsuarioAprobacion = tsUsuario;
                        poTransaccion.UsuarioIngreso = tsUsuario;
                        poTransaccion.FechaIngreso = DateTime.Now;
                        poTransaccion.TerminalIngreso = tsTerminal;
                        poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                        poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psCodigoEstado);
                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Aprobado;

                        poObject.CodigoEstado = psCodigoEstado;
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                        poObject.ComentarioAprobador = tsComentario;
                        loBaseDa.SaveChanges();
                    }


                }
                else
                {
                    psResult = "Documento ya aprobado!";
                }
            }
            else
            {
                psResult = "No existe Documento por aprobar";
            }
            return psResult;

        }

        /// <summary>
        /// Desaprobar Factura de Pago
        /// </summary>
        /// <param name="tId"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsDesaprobarFacturaPago(int tId, string tsComentario, string tsUsuario, string tsTerminal)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTFACTURAPAGO>().Where(x => x.IdFacturaPago == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                string psCodigoTransaccion = Diccionario.Tablas.Transaccion.FacturaPago;
                int pId = tId;

                List<string> psUsuario = new List<string>();

                var poAprobaciones = loBaseDa.Get<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                    x.CodigoEstado == Diccionario.Activo &&
                    x.CodigoTransaccion == psCodigoTransaccion &&
                    x.IdTransaccionReferencial == pId
                    && x.Tipo == Diccionario.TipoAprobacion.Aprobado).ToList();
                
                if (poAprobaciones.Count > 0)
                {
                    if (poAprobaciones.Where(x => x.UsuarioAprobacion == tsUsuario).Count() > 0)
                    {
                        var poUltimaAprobacion = poAprobaciones.OrderByDescending(x => x.FechaIngreso).FirstOrDefault();
                        if (tsUsuario == poUltimaAprobacion.UsuarioAprobacion)
                        {

                            string psCodigoEstado = string.Empty;
                            if (poObject.CodigoEstado == Diccionario.Pendiente)
                            {
                                psCodigoEstado = Diccionario.Pendiente;
                            }
                            else if (poObject.CodigoEstado == Diccionario.PreAprobado)
                            {
                                if (poAprobaciones.Count >1)
                                {
                                    psCodigoEstado = Diccionario.PreAprobado;
                                }
                                else
                                {
                                    psCodigoEstado = Diccionario.Pendiente;
                                }
                            }
                            else if (poObject.CodigoEstado == Diccionario.Aprobado)
                            {
                                psCodigoEstado = Diccionario.PreAprobado;
                            }


                            poUltimaAprobacion.CodigoEstado = Diccionario.Eliminado;
                            poUltimaAprobacion.UsuarioModificacion = tsUsuario;
                            poUltimaAprobacion.FechaModificacion = DateTime.Now;
                            poUltimaAprobacion.TerminalModificacion = tsTerminal;
                            

                            REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTransaccion);
                            poTransaccion.CodigoEstado = Diccionario.Activo;
                            poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                            poTransaccion.ComentarioAprobador = tsComentario;
                            poTransaccion.IdTransaccionReferencial = tId;
                            poTransaccion.UsuarioAprobacion = tsUsuario;
                            poTransaccion.UsuarioIngreso = tsUsuario;
                            poTransaccion.FechaIngreso = DateTime.Now;
                            poTransaccion.TerminalIngreso = tsTerminal;
                            poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                            poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psCodigoEstado);
                            poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                            poObject.CodigoEstado = psCodigoEstado;
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;
                            poObject.Comentario = tsComentario;

                            loBaseDa.SaveChanges();
                        }
                        else
                        {
                            psResult = "Existe una aprobación posterior a su aprobación, NO es posible desaprobar. \n";
                        }
                    }
                    else
                    {
                        psResult = "NO existe aprobación con el usuario: " + tsUsuario + " para desaprobar. \n";
                    }
                }
                else
                {
                    psResult = "NO existen aprobaciones para desaprobar. \n";
                }
            }
            else
            {
                psResult = "No existe Documento por aprobar";
            }
            return psResult;

        }

        /// <summary>
        /// Aprobar Factura de Pago
        /// </summary>
        /// <param name="tId"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsAprobacionDefinitiva(int tId, string tsComentario, string tsUsuario, string tsTerminal)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTFACTURAPAGO>().Where(x => x.IdFacturaPago == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)

            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.FacturaPago;
                    int pId = tId;

                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                    poTransaccion.ComentarioAprobador = tsComentario;
                    poTransaccion.IdTransaccionReferencial = pId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Aprobado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Aprobado;

                    poObject.CodigoEstado = Diccionario.Aprobado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                    poObject.ComentarioAprobador = tsComentario;
                    loBaseDa.SaveChanges();


                }
                else
                {
                    psResult = "Documento ya aprobado!";
                }
            }
            else
            {
                psResult = "No existe Documento por aprobar";
            }
            return psResult;

        }

        /// <summary>
        /// Rechazar Factura de Pago
        /// </summary>
        /// <param name="tId"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsRechazarFacturaPago(int tId, string tsComentario, string tsUsuario, string tsTerminal)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTFACTURAPAGO>().Where(x => x.IdFacturaPago == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.PagadoFinanciero == true)
                {
                    psResult = "No es posible eliminar, documento ya se pagó";
                }
                else
                {
                    if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Aprobado)
                    {
                        string psCodigoTransaccion = Diccionario.Tablas.Transaccion.FacturaPago;
                        int pId = tId;

                        var poLista = loBaseDa.Get<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                            x.CodigoEstado == Diccionario.Activo &&
                            x.CodigoTransaccion == psCodigoTransaccion &&
                            x.IdTransaccionReferencial == pId
                            && x.Tipo == Diccionario.TipoAprobacion.Aprobado
                            ).ToList();

                        foreach (var item in poLista)
                        {
                            item.CodigoEstado = Diccionario.Eliminado;
                            item.UsuarioModificacion = tsUsuario;
                            item.FechaModificacion = DateTime.Now;
                            item.TerminalModificacion = tsTerminal;
                        }

                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTransaccion);
                        poTransaccion.CodigoEstado = Diccionario.Activo;
                        poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                        poTransaccion.ComentarioAprobador = tsComentario;
                        poTransaccion.IdTransaccionReferencial = tId;
                        poTransaccion.UsuarioAprobacion = tsUsuario;
                        poTransaccion.UsuarioIngreso = tsUsuario;
                        poTransaccion.FechaIngreso = DateTime.Now;
                        poTransaccion.TerminalIngreso = tsTerminal;
                        poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                        poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Rechazado);
                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                        poObject.CodigoEstado = Diccionario.Rechazado;
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                        poObject.Comentario = tsComentario;

                        loBaseDa.SaveChanges();

                    }
                    else
                    {
                        psResult = "No es posible cambiar de estado.";
                    }
                }
                
            }
            else
            {
                psResult = "No existe Documento por aprobar";
            }
            return psResult;

        }

        /// <summary>
        /// Corregir Factura de Pago
        /// </summary>
        /// <param name="tId"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsCorregirFacturaPago(int tId, string tsComentario, string tsUsuario, string tsTerminal)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTFACTURAPAGO>().Where(x => x.IdFacturaPago == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Aprobado)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.FacturaPago;
                    int pId = tId;

                    var poLista = loBaseDa.Get<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                        x.CodigoEstado == Diccionario.Activo &&
                        x.CodigoTransaccion == psCodigoTransaccion &&
                        x.IdTransaccionReferencial == pId
                        && x.Tipo == Diccionario.TipoAprobacion.Aprobado
                        ).ToList();

                    foreach (var item in poLista)
                    {
                        item.CodigoEstado = Diccionario.Eliminado;
                        item.UsuarioModificacion = tsUsuario;
                        item.FechaModificacion = DateTime.Now;
                        item.TerminalModificacion = tsTerminal;
                    }

                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                    poTransaccion.ComentarioAprobador = tsComentario;
                    poTransaccion.IdTransaccionReferencial = tId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Corregir);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    poObject.CodigoEstado = Diccionario.Corregir;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                    poObject.Comentario = tsComentario;


                    using (var poTran = new TransactionScope())
                    {
                        loBaseDa.SaveChanges();
                        gActualizarEstadoOrdenPago(poObject.IdOrdenPago ?? 0, Diccionario.Corregir, tsComentario, tsUsuario, tsTerminal, poObject.IdOrdenPagoFactura ?? 0, false);
                        poTran.Complete();
                    }
                }
                else
                {
                    psResult = "No es posible cambiar de estado.";
                }
            }
            else
            {
                psResult = "No existe Documento por aprobar";
            }
            return psResult;

        }

        /// <summary>
        /// Eliminar Factura de Pago
        /// </summary>
        /// <param name="tId"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsEliminarFacturaPago(int tId, string tsComentario, string tsUsuario, string tsTerminal)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTFACTURAPAGO>().Where(x => x.IdFacturaPago == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Aprobado)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.FacturaPago;
                    int pId = tId;


                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                    poTransaccion.ComentarioAprobador = tsComentario;
                    poTransaccion.IdTransaccionReferencial = tId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Eliminado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                    poObject.Comentario = tsComentario;

                    loBaseDa.SaveChanges();

                }
                else
                {
                    psResult = "No es posible cambiar de estado.";
                }
            }
            else
            {
                psResult = "No existe Documento";
            }
            return psResult;

        }

        public string gsBuscarFacturaObservacion(int tsCodigo)
        {
            string psReturn = string.Empty;
            var poObject = loBaseDa.Find<COMTORDENPAGOFACTURA>().Where(x => x.IdOrdenPago == tsCodigo).Select(x=> x.Observacion).ToList();
            if (poObject != null)
            {

                foreach (var item in poObject)
                {
                    psReturn += item;
                }

            }
            return psReturn;
        }

        private void lCargarArchivoAdjunto(ref COMTORDENPAGOADJUNTO toEntidadBd, OrdenPagoAdjunto toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {
            toEntidadBd.CodigoEstado = Diccionario.Activo;
            if (string.IsNullOrEmpty(toEntidadData.Descripcion))
            {
                toEntidadBd.Descripcion = "";
            }

            toEntidadBd.ArchivoAdjunto = toEntidadData.ArchivoAdjunto.Trim();
            toEntidadBd.NombreOriginal = toEntidadData.NombreOriginal.Trim();
            toEntidadBd.Descripcion = toEntidadData.Descripcion;

            if (tbActualiza)
            {
                toEntidadBd.UsuarioModificacion = tsUsuario;
                toEntidadBd.FechaModificacion = tdFecha;
                toEntidadBd.TerminalModificacion = tsTerminal;
            }
            else
            {
                toEntidadBd.UsuarioIngreso = tsUsuario;
                toEntidadBd.FechaIngreso = tdFecha;
                toEntidadBd.TerminalIngreso = tsTerminal;
            }
        }

        public bool lbValidaControlTipoOrdenPago(string tsCodigoUsuario)
        {
            
            int piMenu = loBaseDa.Find<GENPMENU>().Where(x => x.CodigoEstado == Diccionario.Activo && x.NombreForma == Diccionario.Tablas.Menu.frmTrBandejaOrdenPago).Select(x => x.IdMenu).FirstOrDefault();

            List<string> psUsuarioPrincipal = loBaseDa.Find<SEGPUSUARIOASIGNADO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdMenu == piMenu && x.CodigoUsuarioAsignado == tsCodigoUsuario).Select(x => x.CodigoUsuario).ToList();

            int piCont = loBaseDa.Find<SEGPUSUARIOTIPOORDENPAGO>().Where(x => x.CodigoEstado == Diccionario.Activo && psUsuarioPrincipal.Contains(x.CodigoUsuario)).Count();

            return piCont > 0 ? true : false;
             

        }

        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gActualizarEstadoOrdenPago(int tId, string TipoEstadoSolicitud, string Observacion, string tsUsuario, string tsTerminal, int tIdDetalle = 0, bool tbLimpiarContexto = true)
        {
            if (tbLimpiarContexto)
            {
                loBaseDa.CreateContext();
            }
            
            var poObject = loBaseDa.Get<COMTORDENPAGO>().Where(x => x.IdOrdenPago == tId).FirstOrDefault();
            if (poObject != null)
            {
                REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                loBaseDa.CreateNewObject(out poTransaccion);
                poTransaccion.CodigoEstado = Diccionario.Activo;
                poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.OrdenPago;
                poTransaccion.ComentarioAprobador = Observacion;
                poTransaccion.IdTransaccionReferencial = tId;
                poTransaccion.UsuarioAprobacion = tsUsuario;
                poTransaccion.UsuarioIngreso = tsUsuario;
                poTransaccion.FechaIngreso = DateTime.Now;
                poTransaccion.TerminalIngreso = tsTerminal;
                poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(TipoEstadoSolicitud);
                poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                poObject.ComentarioAprobador = Observacion;
                poObject.CodigoEstado = TipoEstadoSolicitud;
                poObject.FechaModificacion = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
            }

            var poDetalle = loBaseDa.Get<COMTORDENPAGOFACTURA>().Where(x => x.IdOrdenPagoFactura == tIdDetalle).FirstOrDefault();
            if (poDetalle != null)
            {
                poDetalle.CodigoEstado = Diccionario.Activo;
                poDetalle.FechaModificacion = DateTime.Now;
                poDetalle.UsuarioModificacion = tsUsuario;
                poDetalle.TerminalModificacion = tsTerminal;
            }

            var poTran = loBaseDa.Get<REHTTRANSACCIONAUTOIZACION>().Where(x => x.CodigoTransaccion == Diccionario.Tablas.Transaccion.OrdenPago && x.CodigoEstado == Diccionario.Activo && x.IdTransaccionReferencial == tId && x.Tipo == Diccionario.TipoAprobacion.Aprobado).ToList();

            foreach (var item in poTran)
            {
                item.CodigoEstado = Diccionario.Inactivo;
                item.FechaModificacion = DateTime.Now;
                item.UsuarioModificacion = tsUsuario;
                item.TerminalModificacion = tsTerminal;
            }

            
            loBaseDa.SaveChanges();

        }

        public void gDesligarSaafSap(int tId, int tDocEntry, string tsUsuario, string tsTerminal)
        {
            using (var poTran = new TransactionScope())
            {
                loBaseDa.ExecuteQuery(String.Format("UPDATE SBO_AFECOR.DBO.OPCH SET U_ORDEN_PAGO_SAAF = NULL WHERE DocEntry = {0}", tDocEntry));

                loBaseDa.ExecuteQuery(String.Format("UPDATE COMTORDENPAGOFACTURA SET DocEntry = NULL WHERE IdOrdenPagoFactura = {0}", tId));

                REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                loBaseDa.CreateNewObject(out poTransaccion);
                poTransaccion.CodigoEstado = Diccionario.Activo;
                poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.OrdenPagoFactura;
                poTransaccion.ComentarioAprobador = "Desligada de SAP";
                poTransaccion.IdTransaccionReferencial = tId;
                poTransaccion.UsuarioAprobacion = tsUsuario;
                poTransaccion.UsuarioIngreso = tsUsuario;
                poTransaccion.FechaIngreso = DateTime.Now;
                poTransaccion.TerminalIngreso = tsTerminal;
                poTransaccion.EstadoAnterior = "INGRESADO";
                poTransaccion.EstadoPosterior = "DESLIGADO";
                poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                poTran.Complete();
            }
        }

        public DataTable gdtConsultaGuiaRemisionSapDesdeOrdenPago(string tsUsuarioSap, string tsCodCliente)
        {
            return loBaseDa.DataTable(string.Format("EXEC COMSPCONSULTAGUIASSAPORDENPAGO '{0}','{1}'", tsUsuarioSap, tsCodCliente));
        }

        public string GenerarDiarioOrdenPago(List<SpBandejaOrdenPagoContabilizar> poListaEnviada, string tsUsuario, string tsTerminal, out DataTable dt)
        {
            dt = new DataTable();
            string psMsg = "";

            List<int> piListaId = poListaEnviada.Select(x => x.IdOrdenPagoFactura).ToList();

            loBaseDa.CreateContext();
            var poListaObject = loBaseDa.Get<COMTORDENPAGOFACTURA>().Where(x => piListaId.Contains(x.IdOrdenPagoFactura)).ToList();

            string psIdEnviar = "";
            string psComentario = "FA ";
            string psCardCodeProveedor = "";
            bool Entro = false;
            foreach (var item in poListaObject)
            {
                Entro = true;
                psIdEnviar = psIdEnviar + item.IdOrdenPagoFactura.ToString() + ",";
                psComentario = psComentario + item.Factura + ",";
                if (item.Contabilizado != true)
                {
                    item.Contabilizado = true;
                    item.FechaContabilizacion = DateTime.Now;
                    item.UsuarioContabilizacion = tsUsuario;
                    item.TerminalModificacion = tsTerminal;
                }
            }

            /**********************************************************************************************/
            if (!string.IsNullOrEmpty(psIdEnviar))
            {
                psIdEnviar = psIdEnviar.Substring(0, psIdEnviar.Length - 1);
                psComentario = psComentario.Substring(0, psComentario.Length - 1);
            }

            /**********************************************************************************************/
            Entro = false;
            foreach (var item in poListaEnviada.Select(x=>x.CodProveedor).Distinct())
            {
                Entro = true;
                psComentario = psComentario + " " + item + ",";
                psCardCodeProveedor = psCardCodeProveedor + item + ",";
            }

            if (Entro)
            {
                psComentario = psComentario.Substring(0, psComentario.Length - 1);
                psCardCodeProveedor = psCardCodeProveedor.Substring(0, psCardCodeProveedor.Length - 1);
            }
            /**********************************************************************************************/
            /**********************************************************************************************/
            Entro = false;
            foreach (var item in poListaEnviada.Select(x => x.FechaFactura.Date.Month).Distinct())
            {
                Entro = true;
                psComentario = psComentario + " " + gsGetMes(item) + ",";
            }

            if (Entro)
            {
                psComentario = psComentario.Substring(0, psComentario.Length - 1);
            }
            /**********************************************************************************************/
            /**********************************************************************************************/
            Entro = false;
            foreach (var item in poListaEnviada.Select(x => x.FechaFactura.Date.Year).Distinct())
            {
                Entro = true;
                psComentario = psComentario + " " + item + ",";
            }

            if (Entro)
            {
                psComentario = psComentario.Substring(0, psComentario.Length - 1);
            }
            /**********************************************************************************************/
            using (var poTran = new TransactionScope())
            {
                loBaseDa.SaveChanges();

                dt = goConsultaDataTable(String.Format("EXEC COMSPDIARIOORDENPAGO '{0}','{1}','{2}'",psIdEnviar, psComentario, psCardCodeProveedor));

                poTran.Complete();
            }

            return psMsg;
        }

        public List<Combo> gsGetComboTransportistaDesdeProveedor(string tsCardCode)
        {
            //var dt = goConsultaDataTable(string.Format("SELECT U_ID_TRANSPORTISTA,U_NOMBRE_TRANSPORTISTA FROM [SBO_AFECOR].DBO.[@AFE_EMPTRANSPORTE] T8 WHERE T8.U_RUC_EMPTRANSPORTE = '{0}'", tsCardCode));
            var dt = goConsultaDataTable(string.Format("SELECT U_ID_TRANSPORTISTA,U_NOMBRE_TRANSPORTISTA FROM [AFECORPRUEBA].DBO.[@AFE_EMPTRANSPORTE] T8 WHERE T8.U_RUC_EMPTRANSPORTE = '{0}'", tsCardCode));
            return lsLLegaCombotoDataTable(dt);
        }

        /// <summary>
        /// Buscar Codigo de la Entidad
        /// </summary>
        /// <param name="tsTipo">P: Primero, A: Anterior, S: Siguiente, U: Último</param>
        /// <param name="tsCodigo">Codigo de la entidad</param>
        /// <returns></returns>
        public int goBuscarCodigo(string tsTipo, string tsCodigo = "")
        {
            int psCodigo = 0;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<COMTORDENPAGO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdOrdenPago }).OrderBy(x => x.IdOrdenPago).FirstOrDefault().IdOrdenPago;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<COMTORDENPAGO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdOrdenPago }).OrderByDescending(x => x.IdOrdenPago).FirstOrDefault().IdOrdenPago;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<COMTORDENPAGO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdOrdenPago }).ToList().Where(x => x.IdOrdenPago < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdOrdenPago).FirstOrDefault().IdOrdenPago;
                }
                else
                {
                    psCodigo = int.Parse(tsCodigo);
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<COMTORDENPAGO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdOrdenPago }).ToList().Where(x => x.IdOrdenPago > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdOrdenPago).FirstOrDefault().IdOrdenPago;
                }
                else
                {
                    psCodigo = int.Parse(tsCodigo);
                }
            }
            return psCodigo;

        }

    }
}
