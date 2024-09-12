using GEN_Entidad;
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
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COM_Negocio
{
   public class clsNGuiaRemision : clsNBase
    {
        public string gsGuardar(GuiaRemision toObject,string tsUsuario, string tsTerminal)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            psResult = lsEsValido(toObject);
            

            if (psResult == string.Empty)
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
                poObject.FechaContabilizacion = toObject.FechaContabilizacion;
                poObject.FechaDocumento = toObject.FechaDocumento;
                poObject.FechaEntrega = toObject.FechaEntrega;
                poObject.Total = toObject.Total;
                poObject.Comentario = toObject.Comentario;
                poObject.FolioNum = toObject.FolioNum;
                poObject.Prefijo = toObject.Prefijo;


                if (toObject.GuiaRemisionDetalle != null)
                {
                    var poItems = goSapConsultaItems();
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
                        poObjectItem.ItemName = item.ItemName;
                        poObjectItem.Cantidad = item.Cantidad;
                        poObjectItem.CantidadOriginal = item.CantidadOriginal;
                        poObjectItem.CantidadTomada = item.CantidadTomada;
                        poObjectItem.Saldo = item.Saldo;
                        poObjectItem.LineNum = item.LineNum;



                    }
                }

                loBaseDa.SaveChanges();
            }


            //using (var poTran = new TransactionScope())
            //{
            //    loBaseDa.SaveChanges();


            //    //Guardar Facturas Adjuntas
            //    foreach (var poItem in toObject.Facturas)
            //    {
            //        if (!string.IsNullOrEmpty(poItem.ArchivoAdjunto) && !string.IsNullOrEmpty(poItem.RutaDestino))
            //        {
            //            if (poItem.RutaOrigen != poItem.RutaDestino)
            //            {
            //                if (poItem.RutaOrigen != null)
            //                {
            //                    File.Copy(poItem.RutaOrigen, poItem.RutaDestino);
            //                }

            //            }
            //        }
            //    }

            //    //Guardar Documentos Adjuntos
            //    foreach (var poItem in toObject.OrdenPagoAdjunto)
            //    {
            //        if (!string.IsNullOrEmpty(poItem.ArchivoAdjunto) && !string.IsNullOrEmpty(poItem.RutaDestino))
            //        {
            //            if (poItem.RutaOrigen != poItem.RutaDestino)
            //            {
            //                if (poItem.RutaOrigen != null)
            //                {
            //                    File.Copy(poItem.RutaOrigen, poItem.RutaDestino);
            //                }

            //            }
            //        }
            //    }

            //    foreach (var psItem in psListaAdjuntoEliminar)
            //    {
            //        string eliminar = ConfigurationManager.AppSettings["CarpetaOPCompras"].ToString() + psItem;
            //        File.Delete(eliminar);
            //    }

            //    poTran.Complete();
            //}

            return psResult;
        }

        private string lsEsValido(GuiaRemision toObject)
        {
            string psMsg = "";

            if (toObject.NumBultos == 0)
            {
                psMsg = psMsg + "Ingrese el número de bultos  \n";
            }

            int num = 1;
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
                    psMsg = psMsg + "La cantidad ingresada no puede ser mayor que "+ item.Saldo + " en la fila: " + num + "\n";
                }
                if (item.Cantidad == item.CantidadOriginal)
                {
                    psMsg = psMsg + "La cantidad ingresada debe ser parcial y no total, corregir en la fila: " + num + "\n";
                }
                num = num + 1;
            }

            return psMsg;
        }

        public List<GuiaRemision> goListar(string tsUsuario, int tIdMenu)
        {
            var poLista = new List<GuiaRemision>();

            List<string> psListaEstadoExclusion = new List<string>();
            psListaEstadoExclusion.Add(Diccionario.Inactivo);
            psListaEstadoExclusion.Add(Diccionario.Eliminado);
            
            var poObject = loBaseDa.Find<COMTGUIAREMISION>().Include(x => x.COMTGUIAREMISIONDETALLE).Where(x => !psListaEstadoExclusion.Contains(x.CodigoEstado)).ToList();
            
            return LlenarDatos(poObject);
        }

        public GuiaRemision goBuscarGuiaRemision(int tId)
        {
            var poLista = new List<GuiaRemision>();

            List<string> psListaEstadoExclusion = new List<string>();
            psListaEstadoExclusion.Add(Diccionario.Inactivo);
            psListaEstadoExclusion.Add(Diccionario.Eliminado);

            var poObject = loBaseDa.Find<COMTGUIAREMISION>().Include(x => x.COMTGUIAREMISIONDETALLE)
                .Where(x => x.IdGuiaRemision == tId && !psListaEstadoExclusion.Contains(x.CodigoEstado)).ToList();

            return LlenarDatos(poObject).FirstOrDefault();
        }

        private List<GuiaRemision> LlenarDatos(List<COMTGUIAREMISION> poObject)
        {
            var poLista = new List<GuiaRemision>();

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
                cab.UsuarioSap = item.UsuarioSap;
                cab.FechaContabilizacion = item.FechaContabilizacion;
                cab.FechaDocumento = item.FechaDocumento;
                cab.FechaEntrega = item.FechaEntrega;
                cab.Total = item.Total;
                cab.Comentario = item.Comentario;
                cab.FolioNum = item.FolioNum;
                cab.Prefijo = item.Prefijo;

                foreach (var detalle in item.COMTGUIAREMISIONDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var det = new GuiaRemisionDetalle();
                    det.IdGuiaRemisionDetalle = detalle.IdGuiaRemisionDetalle;
                    det.IdGuiaRemision = detalle.IdGuiaRemision;
                    det.ItemCode = detalle.ItemCode;
                    det.ItemName = detalle.ItemName;
                    det.CantidadOriginal = detalle.CantidadOriginal;
                    det.CantidadTomada = detalle.CantidadTomada;
                    det.Cantidad = detalle.Cantidad;
                    det.LineNum = detalle.LineNum;

                    cab.GuiaRemisionDetalle.Add(det);
                }

                poLista.Add(cab);
            }
            return poLista;

        }


        public string gEliminarMaestro(int tiCodigo, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";

            //var poObject = loBaseDa.Get<COMTORDENPAGO>().Include(x => x.COMTORDENPAGOFACTURA).Include(x => x.COMTORDENPAGOADJUNTO).Where(x => x.IdOrdenPago == tsCodigo).FirstOrDefault();
            //if (poObject != null)
            //{
            //    poObject.CodigoEstado = Diccionario.Eliminado;
            //    poObject.UsuarioModificacion = tsUsuario;
            //    poObject.TerminalModificacion = tsTerminal;

            //    List<string> psListaEstado = new List<string>();
            //    psListaEstado.Add(Diccionario.Eliminado);
            //    psListaEstado.Add(Diccionario.Rechazado);

            //    foreach (var item in poObject.COMTORDENPAGOFACTURA.Where(x => x.CodigoEstado == Diccionario.Activo))
            //    {
            //        item.CodigoEstado = Diccionario.Eliminado;
            //        item.UsuarioModificacion = tsUsuario;
            //        item.TerminalModificacion = tsTerminal;

            //        if (loBaseDa.Find<COMTFACTURAPAGO>().Where(x => psListaEstado.Contains(x.CodigoEstado) && x.IdOrdenPagoFactura == item.IdOrdenPagoFactura).Count() > 0)
            //        {
            //            string.Format("{0}Factura: {1} Está siendo usada en otro proceso no es posible eliminar Orden de Pago \n", psMsg, item.Factura);
            //        }

            //        if (item.DocEntry != null)
            //        {
            //            string.Format("{0}Factura: {1} ha sido ligada con SAP, pedir que se deslique a contabilidad para poder eliminar Orden de Pago \n", psMsg, item.Factura);
            //        }
            //    }

            //    foreach (var item in poObject.COMTORDENPAGOADJUNTO.Where(x => x.CodigoEstado == Diccionario.Activo))
            //    {
            //        item.CodigoEstado = Diccionario.Eliminado;
            //        item.UsuarioModificacion = tsUsuario;
            //        item.TerminalModificacion = tsTerminal;
            //    }

            //    if (string.IsNullOrEmpty(psMsg))
            //    {
            //        loBaseDa.SaveChanges();
            //    }

            //}

            return psMsg;
        }

        public string gsGetIdentificacionTransportista(string tsCode)
        {
            var dt = gdtDatosTransportista(tsCode);
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

        
    }
}
