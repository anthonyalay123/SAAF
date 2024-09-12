using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using DevExpress.Xpo;
using GEN_Entidad;
using GEN_Entidad.Entidades.SHEQ;
using GEN_Negocio;
using REH_Dato;
using static GEN_Entidad.Diccionario;

namespace SHE_Negocio.Transacciones
{

    public class clsNTransferenciaStock : clsNBase
    {
        public string gsGuardar(TransferenciaStock toObject, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();

            string psMsg = lsValida(toObject);

            int pId = toObject.IdTransferenciaStock;

            if (string.IsNullOrEmpty(psMsg))
            {
                var poObject = loBaseDa.Get<SHETTRANSFERENCIASTOCK>().Include(x => x.SHETTRANSFERENCIASTOCKDETALLE).Where(x => x.IdTransferenciaStock == pId && x.IdTransferenciaStock != 0).FirstOrDefault();

                var piListaIdPresentacion = toObject.TransferenciaStockDetalle.Where(x => x.IdTransferenciaStockDetalle != 0).Select(x => x.IdTransferenciaStockDetalle).ToList();

                if (poObject != null)
                {
                    //ELIMINAR
                    foreach (var poItem in poObject.SHETTRANSFERENCIASTOCKDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdTransferenciaStockDetalle)))
                    {
                        poItem.CodigoEstado = Diccionario.Inactivo;
                        poItem.UsuarioModificacion = tsUsuario;
                        poItem.FechaModificacion = DateTime.Now;
                        poItem.TerminalModificacion = tsTerminal;
                    }
                }

                //ACTUALIZAR Y ELIMINAR
                if (poObject != null)
                {
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObject = new SHETTRANSFERENCIASTOCK();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                }

                poObject.FechaTrasferencia = toObject.FechaTransferencia;
                poObject.CodigoEstado = Diccionario.Activo;
                poObject.Observaciones = toObject.Observaciones;
                poObject.CodigoMotivoMovInvEPP = toObject.CodigoMotivo;
                poObject.GrupoMotivoMovInvEPP = toObject.GrupoMotivo;
                poObject.IdBodegaEPPOrigen = toObject.IdBodegaEPPOrigen;
                poObject.IdBodegaEPPDestino = toObject.IdBodegaEPPDestino;

                if (toObject.TransferenciaStockDetalle != null)
                {
                    foreach (var item in toObject.TransferenciaStockDetalle)
                    {
                        int pIdDetalle = item.IdTransferenciaStockDetalle;
                        var poObjectItem = poObject.SHETTRANSFERENCIASTOCKDETALLE.Where(x => x.IdTransferenciaStockDetalle == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new SHETTRANSFERENCIASTOCKDETALLE();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.SHETTRANSFERENCIASTOCKDETALLE.Add(poObjectItem);
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.IdBodegaEPPOrigen = toObject.IdBodegaEPPOrigen;
                        poObjectItem.IdBodegaEPPDestino = toObject.IdBodegaEPPDestino;
                        poObjectItem.IdItemEPP = item.IdItemEPP;
                        poObjectItem.Cantidad = item.Cantidad;
                        poObjectItem.Stock = item.Stock;
                    }
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();

                    MovimientoInventario poMovimientoEgreso = new MovimientoInventario();
                    poMovimientoEgreso.GrupoMotivo = Diccionario.ListaCatalogo.MotivoEgresoInventarioEPP;
                    poMovimientoEgreso.CodigoMotivo = "001";
                    poMovimientoEgreso.Observaciones = toObject.Observaciones;
                    poMovimientoEgreso.MovimientoInventarioDetalle = poObject.SHETTRANSFERENCIASTOCKDETALLE
                        .Select(d => new MovimientoInventarioDetalle
                        {
                            IdBodegaEPP = d.IdBodegaEPPOrigen,
                            IdItemEPP = d.IdItemEPP,
                            Cantidad = d.Cantidad
                        })
                        .ToList();
                    poMovimientoEgreso.Tipo = Diccionario.TipoMovimiento.Egreso;
                    poMovimientoEgreso.IdTransferenciaStock = poObject.IdTransferenciaStock;
                    poMovimientoEgreso.Fechamovimiento = poObject.FechaTrasferencia;
                    poMovimientoEgreso.Aprobado = true;
                    poMovimientoEgreso.IdBodegaEPP = poObject.IdBodegaEPPOrigen;

                    int pIdMovimientoEgreso;
                    var poLogicaNegocioMovimiento = new clsNMovimientoInventario();
                    var psMsgEgreso = poLogicaNegocioMovimiento.gsGuardarMovimientoTransferenciaStock(poMovimientoEgreso, tsUsuario, tsTerminal, out pIdMovimientoEgreso);


                    MovimientoInventario poMovimientoIngreso = new MovimientoInventario();
                    poMovimientoIngreso.GrupoMotivo = Diccionario.ListaCatalogo.MotivoIngresoInventarioEPP;
                    poMovimientoIngreso.CodigoMotivo = "001";
                    poMovimientoIngreso.Observaciones = toObject.Observaciones;
                    poMovimientoIngreso.MovimientoInventarioDetalle = poObject.SHETTRANSFERENCIASTOCKDETALLE
                       .Select(d => new MovimientoInventarioDetalle
                       {
                           IdBodegaEPP = d.IdBodegaEPPDestino,
                           IdItemEPP = d.IdItemEPP,
                           Cantidad = d.Cantidad
                       })
                       .ToList();
                    poMovimientoIngreso.Tipo = Diccionario.TipoMovimiento.Ingreso;
                    poMovimientoIngreso.IdTransferenciaStock = poObject.IdTransferenciaStock;
                    poMovimientoIngreso.Fechamovimiento = poObject.FechaTrasferencia;
                    poMovimientoIngreso.Aprobado = false;
                    poMovimientoIngreso.IdBodegaEPP = poObject.IdBodegaEPPDestino;

                    int pIdMovimientoIngreso;
                    var psMsgIngreso = poLogicaNegocioMovimiento.gsGuardarMovimientoTransferenciaStock(poMovimientoIngreso, tsUsuario, tsTerminal, out pIdMovimientoIngreso);

                    poTran.Complete();
                }
                    
            }

            return psMsg;
        }

        public List<TransferenciaStock> listaTodos()
        {
            loBaseDa.CreateContext();

            var lista = (from a in loBaseDa.Get<SHETTRANSFERENCIASTOCK>()
                         where a.CodigoEstado == "A" 
                         select new TransferenciaStock
                         {
                             IdTransferenciaStock = a.IdTransferenciaStock,
                             FechaTransferencia = a.FechaTrasferencia,
                             Observaciones = a.Observaciones,
                             CodigoMotivo = a.CodigoMotivoMovInvEPP,
                             GrupoMotivo = a.GrupoMotivoMovInvEPP,
                             IdBodegaEPPOrigen = a.IdBodegaEPPOrigen,
                             IdBodegaEPPDestino = a.IdBodegaEPPDestino
                         }).ToList().OrderBy(x => x.IdTransferenciaStock).ToList();

            return lista;
        }

        public List<TransferenciaStock> listaMovimientosEliminados()
        {
            loBaseDa.CreateContext();

            var lista = (from a in loBaseDa.Get<SHETTRANSFERENCIASTOCK>()
                         select new TransferenciaStock
                         {
                             IdTransferenciaStock = a.IdTransferenciaStock,
                             FechaTransferencia = a.FechaTrasferencia,
                             Observaciones = a.Observaciones,
                             CodigoMotivo = a.CodigoMotivoMovInvEPP,
                             GrupoMotivo = a.GrupoMotivoMovInvEPP,
                             IdBodegaEPPOrigen = a.IdBodegaEPPOrigen,
                             IdBodegaEPPDestino = a.IdBodegaEPPDestino,
                             CodigoEstado = a.CodigoEstado
                         }).ToList().OrderBy(x => x.IdTransferenciaStock).ToList();

            return lista;
        }

        public TransferenciaStock goConsultarMovimiento(int tId)
        {
            loBaseDa.CreateContext();
            var poCab = loBaseDa.Get<SHETTRANSFERENCIASTOCK>().Include(x => x.SHETTRANSFERENCIASTOCKDETALLE).Where(x => x.IdTransferenciaStock == tId).FirstOrDefault();
            return goConsultarMovimiento(poCab);
        }

        public TransferenciaStock goConsultarMovimientoCopiar(int tId)
        {
            loBaseDa.CreateContext();
            var poCab = loBaseDa.Get<SHETTRANSFERENCIASTOCK>().Include(x => x.SHETTRANSFERENCIASTOCKDETALLE).Where(x => x.IdTransferenciaStock == tId).FirstOrDefault();
            return goConsultarMovimientoCopiar(poCab);
        }

        private TransferenciaStock goConsultarMovimiento(SHETTRANSFERENCIASTOCK poCab)
        {
            TransferenciaStock poObjectReturn = null;

            if (poCab != null)
            {
                poObjectReturn = new TransferenciaStock();

                poObjectReturn.IdTransferenciaStock = poCab.IdTransferenciaStock;
                poObjectReturn.Observaciones = poCab.Observaciones;
                poObjectReturn.CodigoMotivo = poCab.CodigoMotivoMovInvEPP;
                poObjectReturn.GrupoMotivo = poCab.GrupoMotivoMovInvEPP;
                poObjectReturn.IdBodegaEPPOrigen = poCab.IdBodegaEPPOrigen;
                poObjectReturn.IdBodegaEPPDestino = poCab.IdBodegaEPPDestino;
                poObjectReturn.FechaTransferencia = poCab.FechaTrasferencia;

                poObjectReturn.TransferenciaStockDetalle = new List<TransferenciaStockDetalle>();
                foreach (var detalle in poCab.SHETTRANSFERENCIASTOCKDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new TransferenciaStockDetalle();

                    poDet.IdTransferenciaStockDetalle = detalle.IdTransferenciaStockDetalle;
                    poDet.IdTransferenciaStock = detalle.IdTransferenciaStock;
                    poDet.IdItemEPP = detalle.IdItemEPP;
                    poDet.IdBodegaEPPOrigen = detalle.IdBodegaEPPOrigen;
                    poDet.IdBodegaEPPDestino = detalle.IdBodegaEPPDestino;
                    poDet.Cantidad = detalle.Cantidad;
                    poDet.Stock = detalle.Stock;

                    poObjectReturn.TransferenciaStockDetalle.Add(poDet);
                }
            }
            return poObjectReturn;
        }

        private TransferenciaStock goConsultarMovimientoCopiar(SHETTRANSFERENCIASTOCK poCab)
        {
            TransferenciaStock poObjectReturn = null;

            if (poCab != null)
            {
                poObjectReturn = new TransferenciaStock();

                //poObjectReturn.IdTransferenciaStock = poCab.IdTransferenciaStock;
                poObjectReturn.Observaciones = poCab.Observaciones;
                poObjectReturn.CodigoMotivo = poCab.CodigoMotivoMovInvEPP;
                poObjectReturn.GrupoMotivo = poCab.GrupoMotivoMovInvEPP;
                poObjectReturn.IdBodegaEPPOrigen = poCab.IdBodegaEPPOrigen;
                poObjectReturn.IdBodegaEPPDestino = poCab.IdBodegaEPPDestino;
                poObjectReturn.FechaTransferencia = poCab.FechaTrasferencia;

                poObjectReturn.TransferenciaStockDetalle = new List<TransferenciaStockDetalle>();
                foreach (var detalle in poCab.SHETTRANSFERENCIASTOCKDETALLE)
                {
                    var poDet = new TransferenciaStockDetalle();

                    //poDet.IdTransferenciaStockDetalle = detalle.IdTransferenciaStockDetalle;
                   // poDet.IdTransferenciaStock = detalle.IdTransferenciaStock;
                    poDet.IdItemEPP = detalle.IdItemEPP;
                    //poDet.IdBodegaEPPOrigen = detalle.IdBodegaEPPOrigen;
                    //poDet.IdBodegaEPPDestino = detalle.IdBodegaEPPDestino;
                    poDet.Cantidad = detalle.Cantidad;
                    poDet.Stock = detalle.Stock;

                    poObjectReturn.TransferenciaStockDetalle.Add(poDet);
                }
            }
            return poObjectReturn;
        }

        private string lsValida(TransferenciaStock toObject)
        {
            string psMsg = "";

            if (toObject.FechaTransferencia > DateTime.Now)
            {
                psMsg = string.Format("{0}La fecha de transferencia no puede ser mayor a la fecha actual. \n", psMsg);
            }

            if (string.IsNullOrEmpty(toObject.Observaciones))
            {
                psMsg = string.Format("{0}Ingrese Observaciones. \n", psMsg);
            }

            if (string.IsNullOrEmpty(toObject.CodigoMotivo))
            {
                psMsg = string.Format("{0}Seleccione Motivo. \n", psMsg);
            }

            if (toObject.IdBodegaEPPOrigen == toObject.IdBodegaEPPDestino)
            {
                psMsg = string.Format("{0}La bodega de origen y destino no pueden ser iguales. \n", psMsg);
            }

            if (toObject.TransferenciaStockDetalle == null || toObject.TransferenciaStockDetalle.Count == 0)
            {
                psMsg = string.Format("{0}Debe selecionar al menos un ítem en el detalle. \n", psMsg);
            }
            else
            {
                var loCombo = goConsultarComboItemEPP();
                var poLista = toObject.TransferenciaStockDetalle;
                var poListaRecorrida = new List<TransferenciaStockDetalle>();

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

        public string gsEliminarMovimiento(int tId, string tsUsuario, string tsTerminal, bool LimpiarContexto = true)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if (string.IsNullOrEmpty(psMsg))
            {
                var poObject = loBaseDa.Get<SHETTRANSFERENCIASTOCK>().Include(x => x.SHETTRANSFERENCIASTOCKDETALLE).Where(x => x.IdTransferenciaStock == tId && x.IdTransferenciaStock != 0).FirstOrDefault();

                if (poObject != null)
                {
                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;

                    foreach (var poItem in poObject.SHETTRANSFERENCIASTOCKDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {
                        poItem.CodigoEstado = Diccionario.Eliminado;
                        poItem.UsuarioModificacion = tsUsuario;
                        poItem.FechaModificacion = DateTime.Now;
                        poItem.TerminalModificacion = tsTerminal;
                    }
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();
                    var idEgreso = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>().Where(c => c.IdTransferenciaStock == tId && c.CodigoEstado == "A" && c.Tipo == "E")
                        .Select(c => c.IdMovimientoInventario).FirstOrDefault();
                    var poLogicaNegocioMovimiento = new clsNMovimientoInventario();

                    var psMsgEgreso = poLogicaNegocioMovimiento.gsAnularMovimientoStock(idEgreso, tsUsuario, tsTerminal);

                    var idIngreso = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>().Where(c => c.IdTransferenciaStock == tId && c.CodigoEstado == "A" && c.Tipo == "I")
                        .Select(c => c.IdMovimientoInventario).FirstOrDefault();
                    var psMsgIngreso = poLogicaNegocioMovimiento.gsAnularMovimientoStock(idIngreso, tsUsuario, tsTerminal);

                    poTran.Complete();
                }
            }

            return psMsg;
        }

        public string goBuscarCodigo(string tsTipo, string tsCodigo = "")
        {
            string psCodigo = string.Empty;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<SHETTRANSFERENCIASTOCK>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdTransferenciaStock }).OrderBy(x => x.IdTransferenciaStock).FirstOrDefault()?.IdTransferenciaStock.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<SHETTRANSFERENCIASTOCK>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdTransferenciaStock }).OrderByDescending(x => x.IdTransferenciaStock).FirstOrDefault()?.IdTransferenciaStock.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<SHETTRANSFERENCIASTOCK>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdTransferenciaStock }).ToList().Where(x => x.IdTransferenciaStock < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdTransferenciaStock).FirstOrDefault().IdTransferenciaStock.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<SHETTRANSFERENCIASTOCK>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdTransferenciaStock }).ToList().Where(x => x.IdTransferenciaStock > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdTransferenciaStock).FirstOrDefault().IdTransferenciaStock.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            return psCodigo;

        }

       

    }

}