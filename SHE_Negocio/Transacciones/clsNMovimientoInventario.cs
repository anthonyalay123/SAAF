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
using static GEN_Entidad.Diccionario;

namespace SHE_Negocio.Transacciones
{
    public class clsNMovimientoInventario : clsNBase
    {
        #region Movimiento Inventario
        public string gsGuardarMovimiento(MovimientoInventario toObject, string tsUsuario, string tsTerminal, out int tId, bool LimpiarContexto = true)
        {
            tId = 0;
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            int pId = toObject.IdMovimientoInventario;

            psMsg = lsValidarDatosMovimientoInventario(toObject);

            if (string.IsNullOrEmpty(psMsg))
            {
                //Consulta entidad Cabecera
                var poObject = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>().Include(x => x.SHETMOVIMIENTOINVENTARIODETALLE).Where(x => x.IdMovimientoInventario == pId && x.IdMovimientoInventario != 0).FirstOrDefault();

                // Determinar que filas fueron eliminadas de la presentación
                var piListaIdPresentacion = toObject.MovimientoInventarioDetalle.Where(x => x.IdMovimientoInventarioDetalle != 0).Select(x => x.IdMovimientoInventarioDetalle).ToList();
                if (poObject != null)
                {
                    foreach (var poItem in poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdMovimientoInventarioDetalle)))
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
                    poObject = new SHETMOVIMIENTOINVENTARIO();
                    loBaseDa.CreateNewObject(out poObject);
                    pbuevo = true;
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                }

                poObject.CodigoEstado = Diccionario.Activo;
                poObject.Tipo = toObject.Tipo;
                poObject.Observaciones = toObject.Observaciones;
                poObject.CodigoMotivoMovInvEPP = toObject.CodigoMotivo;
                poObject.GrupoMotivoMovInvEPP = toObject.GrupoMotivo;
                poObject.IdTransferenciaStock = toObject.IdTransferenciaStock;
                poObject.FechaMovimiento = toObject.Fechamovimiento;
                poObject.CentroCosto = toObject.CentroCosto;
                poObject.IdBodegaEPP = toObject.IdBodegaEPP;

                if(toObject.Tipo == "I")
                {
                    poObject.NumeroFactura = toObject.NumeroFactura;
                    poObject.IdProveedor = toObject.IdProveedor;
                }
                
                //poObject.Aprobado = true;
                
                if (toObject.MovimientoInventarioDetalle != null)
                {

                    foreach (var item in toObject.MovimientoInventarioDetalle)
                    {
                        int pIdDetalle = item.IdMovimientoInventarioDetalle;
                        var poObjectItem = poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.IdMovimientoInventarioDetalle == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new SHETMOVIMIENTOINVENTARIODETALLE();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.SHETMOVIMIENTOINVENTARIODETALLE.Add(poObjectItem);
                        }

                        if (poObject.Tipo == "I")
                        {
                            poObjectItem.Costo = item.Costo;
                        }
                        else
                        {
                            var poListaCosto = (from a in loBaseDa.Find<SHETMOVIMIENTOINVENTARIO>()
                                                join b in loBaseDa.Find<SHETMOVIMIENTOINVENTARIODETALLE>() on a.IdMovimientoInventario equals b.IdMovimientoInventario
                                                where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo
                                                && a.Tipo == "I" && b.IdItemEPP == item.IdItemEPP && b.Costo > 0 && a.IdTransferenciaStock == null
                                                && a.IdActualizacionInventario == null && a.IdEntregaEpp == null
                                                select new { costo = b.Costo ?? 0 }
                                                   ).ToList();
                            if (poListaCosto.Count > 0)
                            {
                                poObjectItem.Costo = poListaCosto.Select(x => x.costo).Average();
                            }
                            else
                            {
                                poObjectItem.Costo = 0;
                            }
                            poObjectItem.PrecioSinImpuesto = 0;
                            poObjectItem.Impuesto = 0;
                            poObjectItem.GrabaIva = loBaseDa.Find<SHEMITEMEPP>().Where(x => x.IdItemEPP == item.IdItemEPP).Select(x => x.GrabaIva).FirstOrDefault();
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.IdBodegaEPP = toObject.IdBodegaEPP;
                        poObjectItem.IdItemEPP = item.IdItemEPP;
                        poObjectItem.Cantidad = item.Cantidad;
                        poObjectItem.GrabaIva = item.GrabaIva;
                        poObjectItem.Impuesto = item.Impuesto;
                        poObjectItem.Stock = item.Stock;
                        poObjectItem.PrecioSinImpuesto = item.PrecioSinImpuesto;
                    }
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();
                    tId = poObject.IdMovimientoInventario;
                    if (pbuevo)
                    {
                        string psTipoStock = poObject.Tipo == Diccionario.TipoMovimiento.Ingreso ? "+" : "-";
                        foreach (var item in poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                        {
                            var dtStock = goConsultaDataTable(string.Format("EXEC [dbo].[SHESPACTUALIZARSTOCK] '{0}','{1}','{2}','{3}'", item.IdItemEPP, item.IdBodegaEPP, item.Cantidad, psTipoStock));
                            if (dtStock == null || !string.IsNullOrEmpty(dtStock.Rows[0][0].ToString())) psMsg = string.Format(dtStock.Rows[0][0].ToString(), psMsg);

                            if (poObject.Tipo == "I")
                            {
                                var poItem = loBaseDa.Get<SHEMITEMEPP>().Where(x => x.IdItemEPP == item.IdItemEPP).FirstOrDefault();
                                if (poItem != null)
                                {
                                    var poListaCosto = (from a in loBaseDa.Find<SHETMOVIMIENTOINVENTARIO>()
                                                        join b in loBaseDa.Find<SHETMOVIMIENTOINVENTARIODETALLE>() on a.IdMovimientoInventario equals b.IdMovimientoInventario
                                                        where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo
                                                        && a.Tipo == "I" && b.IdItemEPP == item.IdItemEPP && b.Costo > 0 && a.IdTransferenciaStock == null
                                                        && a.IdActualizacionInventario == null && a.IdEntregaEpp == null
                                                        select new { costo = b.Costo ?? 0 }
                                                   ).ToList();

                                    if (poListaCosto.Count > 0)
                                    {
                                        poItem.Costo = poListaCosto.Select(x => x.costo).Average();
                                    }
                                    else
                                    {
                                        poItem.Costo = 0;
                                    }
                                }
                            }

                        }
                        if (!string.IsNullOrEmpty(psMsg)) throw new Exception(string.Format(psMsg));

                    }

                    loBaseDa.SaveChanges();
                    poTran.Complete();
                }
            }

            return psMsg;
        }

        public string gsGuardarMovimientoIngreso(MovimientoInventario toObject, string tsUsuario, string tsTerminal, out int tId, bool LimpiarContexto = true)
        {
            tId = 0;
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            int pId = toObject.IdMovimientoInventario;

            psMsg = lsValidarDatosMovimientoInventario(toObject);

            if (string.IsNullOrEmpty(psMsg))
            {
                //Consulta entidad Cabecera
                var poObject = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>().Include(x => x.SHETMOVIMIENTOINVENTARIODETALLE).Where(x => x.IdMovimientoInventario == pId && x.IdMovimientoInventario != 0).FirstOrDefault();

                // Determinar que filas fueron eliminadas de la presentación
                var piListaIdPresentacion = toObject.MovimientoInventarioDetalle.Where(x => x.IdMovimientoInventarioDetalle != 0).Select(x => x.IdMovimientoInventarioDetalle).ToList();
                if (poObject != null)
                {
                    foreach (var poItem in poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdMovimientoInventarioDetalle)))
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
                    poObject = new SHETMOVIMIENTOINVENTARIO();
                    loBaseDa.CreateNewObject(out poObject);
                    pbuevo = true;
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                }

                poObject.CodigoEstado = Diccionario.Activo;
                poObject.Tipo = toObject.Tipo;
                poObject.Observaciones = toObject.Observaciones;
                poObject.CodigoMotivoMovInvEPP = toObject.CodigoMotivo;
                poObject.GrupoMotivoMovInvEPP = toObject.GrupoMotivo;
                poObject.IdTransferenciaStock = toObject.IdTransferenciaStock;
                poObject.FechaMovimiento = toObject.Fechamovimiento;
                poObject.CentroCosto = toObject.CentroCosto;
                //poObject.Aprobado = false;
                poObject.IdBodegaEPP = toObject.IdBodegaEPP;


                if (toObject.MovimientoInventarioDetalle != null)
                {

                    foreach (var item in toObject.MovimientoInventarioDetalle)
                    {
                        int pIdDetalle = item.IdMovimientoInventarioDetalle;
                        var poObjectItem = poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.IdMovimientoInventarioDetalle == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new SHETMOVIMIENTOINVENTARIODETALLE();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.SHETMOVIMIENTOINVENTARIODETALLE.Add(poObjectItem);
                        }

                        if (poObject.Tipo == "I")
                        {
                            poObjectItem.Costo = item.Costo;
                        }
                        else
                        {
                            var poListaCosto = (from a in loBaseDa.Find<SHETMOVIMIENTOINVENTARIO>()
                                                join b in loBaseDa.Find<SHETMOVIMIENTOINVENTARIODETALLE>() on a.IdMovimientoInventario equals b.IdMovimientoInventario
                                                where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo
                                                && a.Tipo == "I" && b.IdItemEPP == item.IdItemEPP && b.Costo > 0 && a.IdTransferenciaStock == null
                                                 && a.IdActualizacionInventario == null && a.IdEntregaEpp == null
                                                select new { costo = b.Costo ?? 0 }
                                                   ).ToList();
                            if (poListaCosto.Count > 0)
                            {
                                poObjectItem.Costo = poListaCosto.Select(x => x.costo).Average();
                            }
                            else
                            {
                                poObjectItem.Costo = 0;
                            }
                            poObjectItem.PrecioSinImpuesto = 0;
                            poObjectItem.Impuesto = 0;
                            poObjectItem.GrabaIva = loBaseDa.Find<SHEMITEMEPP>().Where(x => x.IdItemEPP == item.IdItemEPP).Select(x => x.GrabaIva).FirstOrDefault();
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.IdBodegaEPP = toObject.IdBodegaEPP;
                        poObjectItem.IdItemEPP = item.IdItemEPP;
                        poObjectItem.Cantidad = item.Cantidad;
                        poObjectItem.GrabaIva = item.GrabaIva;
                        poObjectItem.Impuesto = item.Impuesto;
                        poObjectItem.PrecioSinImpuesto = item.PrecioSinImpuesto;
                    }
                }

                loBaseDa.SaveChanges();

            }

            return psMsg;
        }

        public string gsAnularMovimientoStock(int tId, string tsUsuario, string tsTerminal, bool LimpiarContexto = true)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poObject = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>().Include(x => x.SHETMOVIMIENTOINVENTARIODETALLE).Where(x => x.IdMovimientoInventario == tId && x.IdMovimientoInventario != 0).FirstOrDefault();

            if (string.IsNullOrEmpty(psMsg))
            {
                //Consulta entidad Cabecera
                poObject = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>().Include(x => x.SHETMOVIMIENTOINVENTARIODETALLE).Where(x => x.IdMovimientoInventario == tId && x.IdMovimientoInventario != 0).FirstOrDefault();

                // Determinar que filas fueron eliminadas de la presentación
                if (poObject != null)
                {
                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;

                    foreach (var poItem in poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
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

                    string psMsgStock = "";
                    string psTipoStock = poObject.Tipo == Diccionario.TipoMovimiento.Ingreso ? "-" : "+";
                    foreach (var item in poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Eliminado))
                    {
                        var dtStock = goConsultaDataTable(string.Format("EXEC [dbo].[SHESPACTUALIZARSTOCK] '{0}','{1}','{2}','{3}'", item.IdItemEPP, item.IdBodegaEPP, item.Cantidad, psTipoStock));
                        if (dtStock == null || !string.IsNullOrEmpty(dtStock.Rows[0][0].ToString())) psMsg = string.Format(dtStock.Rows[0][0].ToString(), psMsg);

                    }
                    if (!string.IsNullOrEmpty(psMsg)) throw new Exception(string.Format(psMsg));

                    loBaseDa.SaveChanges();
                    poTran.Complete();
                }
            }

            return psMsg;

        }

        public string gsAnularMovimiento(int tId, string tsUsuario, string tsTerminal, bool LimpiarContexto = true)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poObject = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>().Include(x => x.SHETMOVIMIENTOINVENTARIODETALLE).Where(x => x.IdMovimientoInventario == tId && x.IdMovimientoInventario != 0).FirstOrDefault();


            if (poObject.IdTransferenciaStock == null && poObject.IdEntregaEpp == null && poObject.IdActualizacionInventario == null)
            {
                
                if (string.IsNullOrEmpty(psMsg))
                {
                    //Consulta entidad Cabecera
                    poObject = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>().Include(x => x.SHETMOVIMIENTOINVENTARIODETALLE).Where(x => x.IdMovimientoInventario == tId && x.IdMovimientoInventario != 0).FirstOrDefault();

                    // Determinar que filas fueron eliminadas de la presentación
                    if (poObject != null)
                    {
                        poObject.CodigoEstado = Diccionario.Eliminado;
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;

                        foreach (var poItem in poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
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

                        string psMsgStock = "";
                        string psTipoStock = poObject.Tipo == Diccionario.TipoMovimiento.Ingreso ? "-" : "+";
                        foreach (var item in poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Eliminado))
                        {
                            var dtStock = goConsultaDataTable(string.Format("EXEC [dbo].[SHESPACTUALIZARSTOCK] '{0}','{1}','{2}','{3}'", item.IdItemEPP, item.IdBodegaEPP, item.Cantidad, psTipoStock));
                            if (dtStock == null || !string.IsNullOrEmpty(dtStock.Rows[0][0].ToString())) psMsg = string.Format(dtStock.Rows[0][0].ToString(), psMsg);

                            if (poObject.Tipo == "I")
                            {
                                var poItem = loBaseDa.Get<SHEMITEMEPP>().Where(x => x.IdItemEPP == item.IdItemEPP).FirstOrDefault();
                                if (poItem != null)
                                {
                                    var poListaCosto = (from a in loBaseDa.Find<SHETMOVIMIENTOINVENTARIO>()
                                                        join b in loBaseDa.Find<SHETMOVIMIENTOINVENTARIODETALLE>() on a.IdMovimientoInventario equals b.IdMovimientoInventario
                                                        where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo
                                                        && a.Tipo == "I" && b.IdItemEPP == item.IdItemEPP && b.Costo > 0
                                                        select new { costo = b.Costo ?? 0 }
                                                        ).ToList();

                                    if (poListaCosto.Count > 0)
                                    {
                                        poItem.Costo = poListaCosto.Select(x => x.costo).Average();
                                    }
                                    else
                                    {
                                        poItem.Costo = 0;
                                    }
                                }
                            }

                        }
                        if (!string.IsNullOrEmpty(psMsg)) throw new Exception(string.Format(psMsg));

                        loBaseDa.SaveChanges();
                        poTran.Complete();
                    }
                }

                return psMsg;
            }
            psMsg = "No se puede eliminar un egreso realizado por otra transferencia.";

            return psMsg;
        }

        public bool? ObtenerIva(int idItemEPP)
        {
            var tieneIVA = loBaseDa.Get<SHEMITEMEPP>().Where(c => c.IdItemEPP == idItemEPP && c.CodigoEstado == "A")
                .Select(c => c.GrabaIva).FirstOrDefault();

            return tieneIVA;
        }

        public string gsGuardarMovimientoTransferenciaStock(MovimientoInventario toObject, string tsUsuario, string tsTerminal, out int tId, bool LimpiarContexto = true)
        {
            tId = 0;
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            int pId = toObject.IdMovimientoInventario;

            if (string.IsNullOrEmpty(psMsg))
            {
                //Consulta entidad Cabecera
                var poObject = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>().Include(x => x.SHETMOVIMIENTOINVENTARIODETALLE).Where(x => x.IdMovimientoInventario == pId && x.IdMovimientoInventario != 0).FirstOrDefault();

                // Determinar que filas fueron eliminadas de la presentación
                var piListaIdPresentacion = toObject.MovimientoInventarioDetalle.Where(x => x.IdMovimientoInventarioDetalle != 0).Select(x => x.IdMovimientoInventarioDetalle).ToList();
                if (poObject != null)
                {
                    foreach (var poItem in poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdMovimientoInventarioDetalle)))
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
                    poObject = new SHETMOVIMIENTOINVENTARIO();
                    loBaseDa.CreateNewObject(out poObject);
                    pbuevo = true;
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                }

                poObject.CodigoEstado = Diccionario.Activo;
                poObject.Tipo = toObject.Tipo;
                poObject.Observaciones = toObject.Observaciones;
                poObject.CodigoMotivoMovInvEPP = toObject.CodigoMotivo;
                poObject.GrupoMotivoMovInvEPP = toObject.GrupoMotivo;
                poObject.IdTransferenciaStock = toObject.IdTransferenciaStock;
                poObject.FechaMovimiento = toObject.Fechamovimiento;
                poObject.CentroCosto = toObject.CentroCosto;
                poObject.IdBodegaEPP = toObject.IdBodegaEPP;

                //if (toObject.Tipo == "I")
                //{
                //    poObject.Aprobado = false;
                //} else
                //{
                //    poObject.Aprobado = true;
                //}

                if (toObject.MovimientoInventarioDetalle != null)
                {

                    foreach (var item in toObject.MovimientoInventarioDetalle)
                    {
                        int pIdDetalle = item.IdMovimientoInventarioDetalle;
                        var poObjectItem = poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.IdMovimientoInventarioDetalle == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new SHETMOVIMIENTOINVENTARIODETALLE();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.SHETMOVIMIENTOINVENTARIODETALLE.Add(poObjectItem);
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.IdBodegaEPP = item.IdBodegaEPP;
                        poObjectItem.IdItemEPP = item.IdItemEPP;
                        poObjectItem.Cantidad = item.Cantidad;
                        poObjectItem.GrabaIva = item.GrabaIva;
                        poObjectItem.Impuesto = item.Impuesto;
                        poObjectItem.PrecioSinImpuesto = item.PrecioSinImpuesto;
                    }
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();
                    tId = poObject.IdMovimientoInventario;
                    if (pbuevo)
                    {
                        string psTipoStock = poObject.Tipo == Diccionario.TipoMovimiento.Ingreso ? "+" : "-";
                        foreach (var item in poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                        {
                            var dtStock = goConsultaDataTable(string.Format("EXEC [dbo].[SHESPACTUALIZARSTOCK] '{0}','{1}','{2}','{3}'", item.IdItemEPP, item.IdBodegaEPP, item.Cantidad, psTipoStock));
                            if (dtStock == null || !string.IsNullOrEmpty(dtStock.Rows[0][0].ToString())) psMsg = string.Format(dtStock.Rows[0][0].ToString(), psMsg);

                        }
                        if (!string.IsNullOrEmpty(psMsg)) throw new Exception(string.Format(psMsg));

                    }

                    loBaseDa.SaveChanges();
                    poTran.Complete();
                }
            }

            return psMsg;
        }

        public string gsGuardarMovimientoEntregaEPP(MovimientoInventario toObject, string tsUsuario, string tsTerminal, out int tId, bool LimpiarContexto = true)
        {
            tId = 0;
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            int pId = toObject.IdMovimientoInventario;

            if (string.IsNullOrEmpty(psMsg))
            {
                //Consulta entidad Cabecera
                var poObject = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>().Include(x => x.SHETMOVIMIENTOINVENTARIODETALLE).Where(x => x.IdMovimientoInventario == pId && x.IdMovimientoInventario != 0).FirstOrDefault();

                // Determinar que filas fueron eliminadas de la presentación
                var piListaIdPresentacion = toObject.MovimientoInventarioDetalle.Where(x => x.IdMovimientoInventarioDetalle != 0).Select(x => x.IdMovimientoInventarioDetalle).ToList();
                if (poObject != null)
                {
                    foreach (var poItem in poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdMovimientoInventarioDetalle)))
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
                    poObject = new SHETMOVIMIENTOINVENTARIO();
                    loBaseDa.CreateNewObject(out poObject);
                    pbuevo = true;
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                }

                poObject.CodigoEstado = Diccionario.Activo;
                poObject.Tipo = toObject.Tipo;
                poObject.Observaciones = toObject.Observaciones;
                poObject.CodigoMotivoMovInvEPP = toObject.CodigoMotivo;
                poObject.GrupoMotivoMovInvEPP = toObject.GrupoMotivo;
                poObject.IdEntregaEpp = toObject.IdEntregaEPP;
                poObject.FechaMovimiento = toObject.Fechamovimiento;
                poObject.CentroCosto = toObject.CentroCosto;
                //poObject.Aprobado = true;
                poObject.IdBodegaEPP = toObject.IdBodegaEPP;

                if (toObject.MovimientoInventarioDetalle != null)
                {

                    foreach (var item in toObject.MovimientoInventarioDetalle)
                    {
                        int pIdDetalle = item.IdMovimientoInventarioDetalle;
                        var poObjectItem = poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.IdMovimientoInventarioDetalle == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new SHETMOVIMIENTOINVENTARIODETALLE();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.SHETMOVIMIENTOINVENTARIODETALLE.Add(poObjectItem);
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.IdBodegaEPP = item.IdBodegaEPP;
                        poObjectItem.IdItemEPP = item.IdItemEPP;
                        poObjectItem.Cantidad = item.Cantidad;
                        poObjectItem.GrabaIva = item.GrabaIva;
                        poObjectItem.Impuesto = item.Impuesto;
                        poObjectItem.PrecioSinImpuesto = item.PrecioSinImpuesto;
                    }
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();
                    tId = poObject.IdMovimientoInventario;
                    if (pbuevo)
                    {
                        string psTipoStock = poObject.Tipo == Diccionario.TipoMovimiento.Ingreso ? "+" : "-";
                        foreach (var item in poObject.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                        {
                            var dtStock = goConsultaDataTable(string.Format("EXEC [dbo].[SHESPACTUALIZARSTOCK] '{0}','{1}','{2}','{3}'", item.IdItemEPP, item.IdBodegaEPP, item.Cantidad, psTipoStock));
                            if (dtStock == null || !string.IsNullOrEmpty(dtStock.Rows[0][0].ToString())) psMsg = string.Format(dtStock.Rows[0][0].ToString(), psMsg);

                        }
                        if (!string.IsNullOrEmpty(psMsg)) throw new Exception(string.Format(psMsg));

                    }

                    loBaseDa.SaveChanges();
                    poTran.Complete();
                }
            }

            return psMsg;
        }

        public MovimientoInventario goConsultarMovimiento(int tId, string TipoMovimiento)
        {
            loBaseDa.CreateContext();
            var poCab = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>()
                .Include(x => x.SHETMOVIMIENTOINVENTARIODETALLE).Where(x => x.IdMovimientoInventario == tId && x.Tipo == TipoMovimiento /*&& x.IdTransferenciaStock == null && x.IdEntregaEpp == null*/).FirstOrDefault();
            return goConsultarMovimiento(poCab);
        }

        public MovimientoInventario goConsultarMovimiento(int tId)
        {
            loBaseDa.CreateContext();
            var poCab = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>()
                .Include(x => x.SHETMOVIMIENTOINVENTARIODETALLE).Where(x => x.IdMovimientoInventario == tId /*&& x.IdTransferenciaStock == null && x.IdEntregaEpp == null*/).FirstOrDefault();
            return goConsultarMovimiento(poCab);
        }

        public MovimientoInventario goConsultarMovimientoCopiar(int tId, string TipoMovimiento)
        {
            loBaseDa.CreateContext();
            var poCab = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>()
                .Include(x => x.SHETMOVIMIENTOINVENTARIODETALLE).Where(x => x.IdMovimientoInventario == tId && x.Tipo == TipoMovimiento /*&& x.IdTransferenciaStock == null && x.IdEntregaEpp == null*/).FirstOrDefault();
            return goConsultarMovimientoCopiar(poCab);
        }

        public List<MovimientoInventario> listaMovimientos(string TipoMovimiento)
        {
            loBaseDa.CreateContext();

            var lista = (from a in loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>()
                         where a.CodigoEstado == "A" && a.Tipo == TipoMovimiento
                         && a.IdTransferenciaStock == null && a.IdEntregaEpp == null && a.IdActualizacionInventario == null
                         select new MovimientoInventario
                         {
                             IdMovimientoInventario = a.IdMovimientoInventario,
                             Tipo = a.Tipo,
                             Fecha = a.FechaIngreso,
                             Observaciones = a.Observaciones,
                             CodigoMotivo = a.CodigoMotivoMovInvEPP,
                             GrupoMotivo = a.GrupoMotivoMovInvEPP,
                             CentroCosto = a.CentroCosto,
                             NumeroFactura = a.NumeroFactura ?? " ",
                             IdProveedor = a.IdProveedor ?? 0,

                         }).ToList().OrderBy(x => x.IdMovimientoInventario).ToList();

            return lista;
        }

        public List<MovimientoInventario> listaMovimientosEliminados(string TipoMovimiento)
        {
            loBaseDa.CreateContext();

            var lista = (from a in loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>()
                         where a.Tipo == TipoMovimiento
                         && a.IdTransferenciaStock == null && a.IdEntregaEpp == null && a.IdActualizacionInventario == null 
                         select new MovimientoInventario
                         {
                             IdMovimientoInventario = a.IdMovimientoInventario,
                             Tipo = a.Tipo,
                             Fecha = a.FechaIngreso,
                             Observaciones = a.Observaciones,
                             CodigoMotivo = a.CodigoMotivoMovInvEPP,
                             GrupoMotivo = a.GrupoMotivoMovInvEPP,
                             CentroCosto = a.CentroCosto,
                             CodigoEstado = a.CodigoEstado,
                             NumeroFactura = a.NumeroFactura ?? " ",
                             IdProveedor = a.IdProveedor ?? 0,
                         }).ToList().OrderBy(x => x.IdMovimientoInventario).ToList();

            return lista;
        }

        private MovimientoInventario goConsultarMovimiento(SHETMOVIMIENTOINVENTARIO poCab)
        {
            MovimientoInventario poObjectReturn = null;

            if (poCab != null)
            {
                poObjectReturn = new MovimientoInventario();

                poObjectReturn.IdMovimientoInventario = poCab.IdMovimientoInventario;
                poObjectReturn.Tipo = poCab.Tipo;
                poObjectReturn.Observaciones = poCab.Observaciones;
                poObjectReturn.CodigoMotivo = poCab.CodigoMotivoMovInvEPP;
                poObjectReturn.GrupoMotivo = poCab.GrupoMotivoMovInvEPP;
                poObjectReturn.Fecha = poCab.FechaIngreso;
                poObjectReturn.CentroCosto = poCab.CentroCosto;
                poObjectReturn.NumeroFactura = poCab.NumeroFactura ?? " ";
                poObjectReturn.IdProveedor = poCab.IdProveedor ?? 0;

                poObjectReturn.MovimientoInventarioDetalle = new List<MovimientoInventarioDetalle>();
                foreach (var detalle in poCab.SHETMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new MovimientoInventarioDetalle();

                    poDet.IdMovimientoInventarioDetalle = detalle.IdMovimientoInventarioDetalle;
                    poDet.IdMovimientoInventario = detalle.IdMovimientoInventario;
                    poDet.IdItemEPP = detalle.IdItemEPP;
                    poDet.IdBodegaEPP = detalle.IdBodegaEPP;
                    poDet.Cantidad = detalle.Cantidad;
                    poDet.GrabaIva = detalle.GrabaIva ?? false;
                    poDet.PrecioSinImpuesto = detalle.PrecioSinImpuesto ?? 0M;
                    poDet.Costo = detalle.GrabaIva.HasValue && detalle.GrabaIva.Value ? detalle.PrecioSinImpuesto.GetValueOrDefault() * 1.15M : detalle.PrecioSinImpuesto.GetValueOrDefault(0M);
                    poDet.Total = poDet.Costo * poDet.Cantidad;
                    poDet.Impuesto = detalle.Impuesto ?? 0M;
                    poDet.Stock = detalle.Stock ?? 0;

                    poObjectReturn.MovimientoInventarioDetalle.Add(poDet);
                }
            }
            return poObjectReturn;
        }

        private MovimientoInventario goConsultarMovimientoCopiar(SHETMOVIMIENTOINVENTARIO poCab)
        {
            MovimientoInventario poObjectReturn = null;

            if (poCab != null)
            {
                poObjectReturn = new MovimientoInventario();

                //poObjectReturn.IdMovimientoInventario = poCab.IdMovimientoInventario;
                poObjectReturn.Tipo = poCab.Tipo;
                poObjectReturn.Observaciones = poCab.Observaciones;
                poObjectReturn.CodigoMotivo = poCab.CodigoMotivoMovInvEPP;
                poObjectReturn.GrupoMotivo = poCab.GrupoMotivoMovInvEPP;
                poObjectReturn.Fecha = poCab.FechaIngreso;
                poObjectReturn.CentroCosto = poCab.CentroCosto;
                poObjectReturn.NumeroFactura = poCab.NumeroFactura ?? " ";
                poObjectReturn.IdProveedor = poCab.IdProveedor ?? 0;

                poObjectReturn.MovimientoInventarioDetalle = new List<MovimientoInventarioDetalle>();
                foreach (var detalle in poCab.SHETMOVIMIENTOINVENTARIODETALLE)
                {
                    var poDet = new MovimientoInventarioDetalle();

                    //poDet.IdMovimientoInventarioDetalle = detalle.IdMovimientoInventarioDetalle;
                    //poDet.IdMovimientoInventario = detalle.IdMovimientoInventario;
                    poDet.IdItemEPP = detalle.IdItemEPP;
                    poDet.IdBodegaEPP = detalle.IdBodegaEPP;
                    poDet.Cantidad = detalle.Cantidad;
                    poDet.GrabaIva = detalle.GrabaIva ?? false;
                    poDet.PrecioSinImpuesto = detalle.PrecioSinImpuesto ?? 0M;
                    poDet.Costo = detalle.GrabaIva.HasValue && detalle.GrabaIva.Value ? detalle.PrecioSinImpuesto.GetValueOrDefault() * 1.15M : detalle.PrecioSinImpuesto.GetValueOrDefault(0M);
                    poDet.Total = poDet.Costo * poDet.Cantidad;
                    poDet.Impuesto = detalle.Impuesto ?? 0M;

                    poObjectReturn.MovimientoInventarioDetalle.Add(poDet);
                }
            }
            return poObjectReturn;
        }

        private string lsValidarDatosMovimientoInventario(MovimientoInventario toObject)
        {
            string psMsg = "";

            if (string.IsNullOrEmpty(toObject.Observaciones))
            {
                psMsg = string.Format("{0}Ingrese Observaciones. \n", psMsg);
            }

            if (toObject.CentroCosto == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Selecione Centro de Costo. \n", psMsg);
            }

            if(toObject.CodigoMotivo == "COM"){
                if (toObject.IdProveedor == 0)
                {
                    psMsg = string.Format("{0}Selecione Proveedor. \n", psMsg);
                }

                if (string.IsNullOrEmpty(toObject.NumeroFactura))
                {
                    psMsg = string.Format("{0}Ingrese Numero de Factura. \n", psMsg);
                }
            }
           
            if (toObject.CodigoMotivo == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Motivo. \n", psMsg);
            }

            if (toObject.MovimientoInventarioDetalle.Count == 0)
            {
                psMsg = string.Format("{0}Ingrese detalle de ítems. \n", psMsg);
            }


            var loCombo = goConsultarComboItemEPP();



            var poLista = toObject.MovimientoInventarioDetalle;
            var poListaRecorrida = new List<MovimientoInventarioDetalle>();
            int fila = 1;

            if (toObject.Tipo == "I")
            {
                foreach (var item in poLista)
                {
                    string psName = loCombo.Where(x => x.Codigo == item.IdItemEPP.ToString()).Select(x => x.Descripcion).FirstOrDefault();

                    if (item.IdItemEPP.ToString() == Diccionario.Seleccione)
                    {
                        psMsg = string.Format("{0}Seleccione ïtem en la Fila # {1}\n", psMsg, fila);
                    }

                    if (item.Cantidad <= 0)
                    {
                        psMsg = string.Format("{0}Cantidad debe ser mayor a 0 en la Fila # {1}\n", psMsg, fila);
                    }

                    if (item.Costo <= 0)
                    {
                        psMsg = string.Format("{0}El precio debe ser mayor a 0 en la Fila # {1}\n", psMsg, fila);
                    }

                    else if (poListaRecorrida.Where(x => x.IdItemEPP == item.IdItemEPP).Count() > 0)
                    {
                        psMsg = string.Format("{0}Eliminar Fila # {1}, Ya existe ingresado el ítem: {2}\n", psMsg, fila, psName);
                    }

                    poListaRecorrida.Add(item);
                    fila++;
                }
            }
            else
            {
                foreach (var item in poLista)
                {
                    string psName = loCombo.Where(x => x.Codigo == item.IdItemEPP.ToString()).Select(x => x.Descripcion).FirstOrDefault();

                    if (item.IdItemEPP.ToString() == Diccionario.Seleccione)
                    {
                        psMsg = string.Format("{0}Seleccione ïtem en la Fila # {1}\n", psMsg, fila);
                    }

                    if (item.Cantidad <= 0)
                    {
                        psMsg = string.Format("{0}Cantidad debe ser mayor a 0 en la Fila # {1}\n", psMsg, fila);
                    }

                    poListaRecorrida.Add(item);
                    fila++;
                }
            }


            return psMsg;
        }

        public string goBuscarCodigo(string tsTipo, string TipoMovimiento, string tsCodigo = "")
        {
            string psCodigo = string.Empty;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<SHETMOVIMIENTOINVENTARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.Tipo == TipoMovimiento && x.IdTransferenciaStock == null && x.IdEntregaEpp == null && x.IdActualizacionInventario == null).Select(x => new { x.IdMovimientoInventario }).OrderBy(x => x.IdMovimientoInventario).FirstOrDefault()?.IdMovimientoInventario.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<SHETMOVIMIENTOINVENTARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.Tipo == TipoMovimiento && x.IdTransferenciaStock == null && x.IdEntregaEpp == null && x.IdActualizacionInventario == null).Select(x => new { x.IdMovimientoInventario }).OrderByDescending(x => x.IdMovimientoInventario).FirstOrDefault()?.IdMovimientoInventario.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<SHETMOVIMIENTOINVENTARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.Tipo == TipoMovimiento && x.IdTransferenciaStock == null && x.IdEntregaEpp == null && x.IdActualizacionInventario == null).Select(x => new { x.IdMovimientoInventario }).ToList().Where(x => x.IdMovimientoInventario < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdMovimientoInventario).FirstOrDefault().IdMovimientoInventario.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<SHETMOVIMIENTOINVENTARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.Tipo == TipoMovimiento && x.IdTransferenciaStock == null && x.IdEntregaEpp == null && x.IdActualizacionInventario == null).Select(x => new { x.IdMovimientoInventario }).ToList().Where(x => x.IdMovimientoInventario > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdMovimientoInventario).FirstOrDefault().IdMovimientoInventario.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            return psCodigo;

        }

        public string obtenerCentroCostoUsuario(string gsUsuario)
        {
            var Usuario = loBaseDa.Get<SEGMUSUARIO>().Where(c => c.CodigoUsuario == gsUsuario).FirstOrDefault();

            var gsCentroCosto = loBaseDa.Get<REHVTPERSONACONTRATO>()
                                .Where(c => c.IdPersona == Usuario.IdPersona)
                                .Select(c => c.CentroCosto)
                                .FirstOrDefault();

            var idCentroCosto = loBaseDa.Get<GENMCENTROCOSTO>()
                                .Where(c => c.Descripcion == gsCentroCosto)
                                .Select(c => c.CodigoCentroCosto).FirstOrDefault();

            if (idCentroCosto == null)
            {
                idCentroCosto = "0";
            }

            return idCentroCosto;
        }
        #endregion
    }
}
