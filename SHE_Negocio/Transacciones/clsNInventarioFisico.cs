using GEN_Entidad;
using GEN_Entidad.Entidades.SHEQ;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SHE_Negocio.Transacciones
{
    public class clsNInventarioFisico : clsNBase
    {

        public List<ActualizacionInventarioDetalle> goConsultarStock(int idBodega)
        {
            loBaseDa.CreateContext();

            var lista = (from a in loBaseDa.Get<SHEPSTOCKEPP>()
                         where a.IdBodegaEPP == idBodega
                         select new ActualizacionInventarioDetalle
                         {
                             IdBodegaEPP = a.IdBodegaEPP,
                             IdItemEPP = a.IdItemEPP,
                             Descripcion = a.IdItemEPP.ToString(),
                             StockAnterior = a.Stock,
                         }).ToList().OrderBy(c => c.IdItemEPP).ToList();
             
            return lista;
        }

        public List<ActualizacionInventario> listaMovimientos(int idBodega)
        {
            loBaseDa.CreateContext();

            var lista = (from a in loBaseDa.Get<SHETACTUALIZACIONINVENTARIO>()
                         where a.CodigoEstado == "A" && a.IdBodega == idBodega
                         select new ActualizacionInventario
                         {
                             IdActualizacionInventario = a.IdActualizacionInventario,
                             Fecha = a.FechaIngreso,
                             Observaciones = a.Observaciones,
                             IdBodegaEPP = a.IdBodega,
                         }).ToList().OrderBy(x => x.IdActualizacionInventario).ToList();

            return lista;
        }

        public ActualizacionInventario goConsultarMovimiento(int tId)
        {
            loBaseDa.CreateContext();
            var poCab = loBaseDa.Get<SHETACTUALIZACIONINVENTARIO>()
                .Include(x => x.SHETACTUALIZACIONINVENTARIODETALLE).Where(x => x.IdActualizacionInventario == tId /*&& x.IdTransferenciaStock == null && x.IdEntregaEpp == null*/).FirstOrDefault();
            return goConsultarMovimiento(poCab);
        }

        private ActualizacionInventario goConsultarMovimiento(SHETACTUALIZACIONINVENTARIO poCab)
        {
            ActualizacionInventario poObjectReturn = null;

            if (poCab != null)
            {
                poObjectReturn = new ActualizacionInventario();

                poObjectReturn.IdActualizacionInventario = poCab.IdActualizacionInventario;
                poObjectReturn.Observaciones = poCab.Observaciones;
                poObjectReturn.Fecha = poCab.FechaIngreso;
                poObjectReturn.IdBodegaEPP = poCab.IdBodega;

                poObjectReturn.ActualizacionInventarioDetalle = new List<ActualizacionInventarioDetalle>();
                foreach (var detalle in poCab.SHETACTUALIZACIONINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new ActualizacionInventarioDetalle();

                    poDet.IdActualizacionInventarioDetalle = detalle.IdActualizacionInventarioDetalle;
                    poDet.IdActualizacionInventario = detalle.IdActualizacionInventario;
                    poDet.IdItemEPP = detalle.IdItemEPP;
                    poDet.Descripcion = detalle.IdItemEPP.ToString();
                    poDet.IdBodegaEPP = detalle.IdBodegaEPP;
                    poDet.StockAnterior = detalle.StockAnterior;
                    poDet.StockNuevo = detalle.StockNuevo;

                    poObjectReturn.ActualizacionInventarioDetalle.Add(poDet);
                }
            }
            return poObjectReturn;
        }

        public string gsGuardar(ActualizacionInventario toObject, string tsUsuario, string tsTerminal, bool LimpiarContexto = true)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            int pId = toObject.IdActualizacionInventario;

            psMsg = lsValidarDatosMovimientoInventario(toObject);

            if (string.IsNullOrEmpty(psMsg))
            {
                //Consulta entidad Cabecera
                var poObject = loBaseDa.Get<SHETACTUALIZACIONINVENTARIO>().Include(x => x.SHETACTUALIZACIONINVENTARIODETALLE).Where(x => x.IdActualizacionInventario == pId && x.IdActualizacionInventario != 0).FirstOrDefault();

                // Determinar que filas fueron eliminadas de la presentación
                var piListaIdPresentacion = toObject.ActualizacionInventarioDetalle.Where(x => x.IdActualizacionInventarioDetalle != 0).Select(x => x.IdActualizacionInventarioDetalle).ToList();
                if (poObject != null)
                {
                    foreach (var poItem in poObject.SHETACTUALIZACIONINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdActualizacionInventarioDetalle)))
                    {
                        poItem.CodigoEstado = Diccionario.Inactivo;
                        poItem.UsuarioModificacion = tsUsuario;
                        poItem.FechaModificacion = DateTime.Now;
                        poItem.TerminalModificacion = tsTerminal;
                    }
                }

                bool pbuevo = false;
                if (poObject != null)
                {
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObject = new SHETACTUALIZACIONINVENTARIO();
                    loBaseDa.CreateNewObject(out poObject);
                    pbuevo = true;
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                }

                poObject.CodigoEstado = Diccionario.Activo;
                poObject.Observaciones = toObject.Observaciones;
                poObject.IdBodega = toObject.IdBodegaEPP;

                if (toObject.ActualizacionInventarioDetalle != null)
                {

                    foreach (var item in toObject.ActualizacionInventarioDetalle)
                    {
                        int pIdDetalle = item.IdActualizacionInventarioDetalle;
                        var poObjectItem = poObject.SHETACTUALIZACIONINVENTARIODETALLE.Where(x => x.IdActualizacionInventarioDetalle == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new SHETACTUALIZACIONINVENTARIODETALLE();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.SHETACTUALIZACIONINVENTARIODETALLE.Add(poObjectItem);
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.IdBodegaEPP = toObject.IdBodegaEPP;
                        poObjectItem.IdItemEPP = item.IdItemEPP;
                        poObjectItem.StockAnterior = item.StockAnterior;
                        poObjectItem.StockNuevo = item.StockNuevo;
                    }
                }

                List<int> itemParaAjustarIngreso = new List<int>();
                List<int> itemParaAjustarEgreso = new List<int>();

                foreach (var item in toObject.ActualizacionInventarioDetalle)
                {
                    var dtStock = goConsultaDataTable(string.Format("EXEC [dbo].[SHESPSTOCKACTUALINVENTARIO] '{0}','{1}'", toObject.IdBodegaEPP, item.IdItemEPP));
                    if (dtStock != null && dtStock.Rows.Count > 0)
                    {
                        int cantidadDisponible = Convert.ToInt32(dtStock.Rows[0]["Cantidad"]);
                        if (item.StockNuevo > cantidadDisponible)
                        {
                            item.StockNuevoAgregar = item.StockNuevo - cantidadDisponible;
                            itemParaAjustarIngreso.Add(item.IdItemEPP);
                        }
                        else if (item.StockNuevo < cantidadDisponible)
                        {
                            item.StockNuevoAgregar = cantidadDisponible - item.StockNuevo;
                            itemParaAjustarEgreso.Add(item.IdItemEPP);
                        }
                        else if (item.StockNuevo == cantidadDisponible)
                        {
                            var poStock = loBaseDa.Get<SHEPSTOCKEPP>().Where(x => x.IdBodegaEPP == toObject.IdBodegaEPP && x.IdItemEPP == item.IdItemEPP).FirstOrDefault();
                            var valorAgregarStock = cantidadDisponible - poStock.Stock;
                            item.StockNuevoAgregar = valorAgregarStock;

                            poStock.UsuarioModificacion = tsUsuario;
                            poStock.FechaModificacion = DateTime.Now;
                            poStock.TerminalModificacion = tsTerminal;
                            poStock.Stock = valorAgregarStock + poStock.Stock;
                        }
                    }
                }

                var inventarioFisicoDetalleIngreso = toObject.ActualizacionInventarioDetalle
                                                .Where(x => itemParaAjustarIngreso.Contains(x.IdItemEPP))
                                                .ToList();

                var inventarioFisicoDetalleEgreso = toObject.ActualizacionInventarioDetalle
                                                .Where(x => itemParaAjustarEgreso.Contains(x.IdItemEPP))
                                                .ToList();

                //string psMsg = lsValida(toObject);



                if (string.IsNullOrEmpty(psMsg))
                {
                    using (var poTran = new TransactionScope())
                    {
                        loBaseDa.SaveChanges();

                        if (inventarioFisicoDetalleIngreso.Count > 0)
                        {
                            psMsg = CrearMovimientoInventario(poObject, inventarioFisicoDetalleIngreso, Diccionario.TipoMovimiento.Ingreso, tsUsuario, tsTerminal);
                        }

                        if (string.IsNullOrEmpty(psMsg) && inventarioFisicoDetalleEgreso.Count > 0)
                        {
                            psMsg = CrearMovimientoInventario(poObject, inventarioFisicoDetalleEgreso, Diccionario.TipoMovimiento.Egreso, tsUsuario, tsTerminal);
                        }

                        loBaseDa.SaveChanges();
                        poTran.Complete();
                        //return psMsg;
                    }

                }

                return psMsg;
            }

            return psMsg;
        }

        private string lsValidarDatosMovimientoInventario(ActualizacionInventario toObject)
        {
            string psMsg = "";

            if (string.IsNullOrEmpty(toObject.Observaciones))
            {
                psMsg = string.Format("{0}Ingrese Observaciones. \n", psMsg);
            }

            if (toObject.IdBodegaEPP == 0)
            {
                psMsg = string.Format("{0}La bodega no puede estar vacia. \n", psMsg);
            }

            if (toObject.ActualizacionInventarioDetalle.Count == 0)
            {
                psMsg = string.Format("{0}Ingrese detalle de ítems. \n", psMsg);
            }


            var loCombo = goConsultarComboItemEPP();

            var poLista = toObject.ActualizacionInventarioDetalle;
            var poListaRecorrida = new List<ActualizacionInventarioDetalle>();
            int fila = 1;

            foreach (var item in poLista)
            {
                string psName = loCombo.Where(x => x.Codigo == item.IdItemEPP.ToString()).Select(x => x.Descripcion).FirstOrDefault();

                if (item.IdItemEPP.ToString() == Diccionario.Seleccione)
                {
                    psMsg = string.Format("{0}Seleccione ïtem en la Fila # {1}\n", psMsg, fila);
                }

                else if (poListaRecorrida.Where(x => x.IdItemEPP == item.IdItemEPP).Count() > 0)
                {
                    psMsg = string.Format("{0}Eliminar Fila # {1}, Ya existe ingresado el ítem: {2}\n", psMsg, fila, psName);
                }

                poListaRecorrida.Add(item);
                fila++;
            }
           
            return psMsg;
        }

        private string CrearMovimientoInventario(SHETACTUALIZACIONINVENTARIO toObject, List<ActualizacionInventarioDetalle> inventarioFisicoDetalle, string tipoMovimiento, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            var poObject = new SHETMOVIMIENTOINVENTARIO();
            loBaseDa.CreateNewObject(out poObject);
            poObject.UsuarioIngreso = tsUsuario;
            poObject.FechaIngreso = DateTime.Now;
            poObject.TerminalIngreso = tsTerminal;

            poObject.CodigoEstado = Diccionario.Activo;
            poObject.Tipo = tipoMovimiento;
            poObject.Observaciones = toObject.Observaciones;
            poObject.CodigoMotivoMovInvEPP = tipoMovimiento == Diccionario.TipoMovimiento.Ingreso
                ? "AJI"
                : "AJE";
            poObject.GrupoMotivoMovInvEPP = tipoMovimiento == Diccionario.TipoMovimiento.Ingreso
                ? Diccionario.ListaCatalogo.MotivoIngresoInventarioEPP
                : Diccionario.ListaCatalogo.MotivoEgresoInventarioEPP;
            poObject.FechaMovimiento = DateTime.Now;
            //poObject.Aprobado = true;
            poObject.IdBodegaEPP = toObject.IdBodega;
            poObject.IdActualizacionInventario = toObject.IdActualizacionInventario;

            if (inventarioFisicoDetalle != null)
            {
                foreach (var item in inventarioFisicoDetalle)
                {
                    var poObjectItem = new SHETMOVIMIENTOINVENTARIODETALLE();
                    poObjectItem.UsuarioIngreso = tsUsuario;
                    poObjectItem.FechaIngreso = DateTime.Now;
                    poObjectItem.TerminalIngreso = tsTerminal;
                    poObject.SHETMOVIMIENTOINVENTARIODETALLE.Add(poObjectItem);

                    poObjectItem.CodigoEstado = Diccionario.Activo;
                    poObjectItem.IdBodegaEPP = toObject.IdBodega;
                    poObjectItem.IdItemEPP = item.IdItemEPP;
                    poObjectItem.Cantidad = item.StockNuevoAgregar;

                    var poStock = loBaseDa.Get<SHEPSTOCKEPP>().Where(x => x.IdBodegaEPP == toObject.IdBodega && x.IdItemEPP == item.IdItemEPP).FirstOrDefault();
                    poStock.UsuarioModificacion = tsUsuario;
                    poStock.FechaModificacion = DateTime.Now;
                    poStock.TerminalModificacion = tsTerminal;
                    poStock.Stock = item.StockNuevo;
                }
            }

            loBaseDa.SaveChanges();
            return psMsg;

            //using (var poTran = new TransactionScope())
            //{
            //    loBaseDa.SaveChanges();

            //    string psTipoStock = tipoMovimiento == Diccionario.TipoMovimiento.Ingreso ? "+" : "-";
            //    string psMsg = string.Empty;
            //    foreach (var item in poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
            //    {
            //        var dtStock = goConsultaDataTable(string.Format("EXEC [dbo].[SHESPACTUALIZARSTOCK] '{0}','{1}','{2}','{3}'", item.IdItemEPP, item.IdBodegaEPP, item.Cantidad, psTipoStock));
            //        if (dtStock == null || !string.IsNullOrEmpty(dtStock.Rows[0][0].ToString()))
            //        {
            //            psMsg = string.Format(dtStock.Rows[0][0].ToString(), psMsg);
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(psMsg)) throw new Exception(string.Format(psMsg));

            //    loBaseDa.SaveChanges();
            //    poTran.Complete();
            //    return psMsg;
            //}
        }

        private string lsValida(InventarioFisico toObject)
        {
            string psMsg = "";

            if (string.IsNullOrEmpty(toObject.Observaciones))
            {
                psMsg = string.Format("{0}Ingrese Observaciones. \n", psMsg);
            }

            if (toObject.IdBodegaEPP == 0)
            {
                psMsg = string.Format("{0}La bodega no puede estar vacia. \n", psMsg);
            }

            if (toObject.InventarioFisicoDetalle == null || toObject.InventarioFisicoDetalle.Count == 0)
            {
                psMsg = string.Format("{0}Debe selecionar al menos un ítem en el detalle. \n", psMsg);
            }
            else
            {
                var loCombo = goConsultarComboItemEPP();
                var poLista = toObject.InventarioFisicoDetalle;
                var poListaRecorrida = new List<InventarioFisicoDetalle>();

                int fila = 1;
                foreach (var item in poLista)
                {
                    string psName = loCombo.Where(x => x.Codigo == item.IdItemEPP.ToString()).Select(x => x.Descripcion).FirstOrDefault();

                    if (item.IdItemEPP.ToString() == Diccionario.Seleccione)
                    {
                        psMsg = string.Format("{0}Seleccione ítem en la Fila # {1}\n", psMsg, fila);
                    }

                    if (item.Cantidad <= 0)
                    {
                        psMsg = string.Format("{0}La cantidad debe ser mayor a 0 en la Fila # {1}\n", psMsg, fila);
                    }

                    else if (poListaRecorrida.Where(x => x.IdItemEPP == item.IdItemEPP).Count() > 0)
                    {
                        psMsg = string.Format("{0}Eliminar Fila # {1}, ya existe ingresado el ítem: {2}\n", psMsg, fila, psName);
                    }

                    poListaRecorrida.Add(item);
                    fila++;
                }
            }

            return psMsg;
        }

    }
}
