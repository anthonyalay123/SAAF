using GEN_Entidad;
using GEN_Entidad.Entidades.Administracion;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static GEN_Entidad.Diccionario;

namespace GEN_Negocio
{
    public class clsNInventarioSuministros : clsNBase
    {
        #region Funciones - Bodega
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public bool gbGuardarBodega(Maestro toObject)
        {
            loBaseDa.CreateContext();
            bool pbResult = false;
            int psCodigo = 0;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = int.Parse(toObject.Codigo);
            var poObject = loBaseDa.Get<ADMMBODEGA>().Where(x => x.IdBodega == psCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;
                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }
            else
            {
                poObject = new ADMMBODEGA();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.TerminalIngreso = toObject.Terminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }

            loBaseDa.SaveChanges();
            return pbResult;
        }

        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsDescripcion"></param>
        /// <returns></returns>
        public List<Maestro> goListarMaestroBodega(string tsDescripcion = "")
        {
            return loBaseDa.Find<ADMMBODEGA>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Maestro
                   {
                       Codigo = x.IdBodega.ToString(),
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Descripcion,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,
                   }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Maestro goBuscarMaestroBodega(int tsCodigo)
        {
            return loBaseDa.Find<ADMMBODEGA>().Where(x => x.IdBodega == tsCodigo)
                .Select(x => new Maestro
                {
                    Codigo = x.IdBodega.ToString(),
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion
                }).FirstOrDefault();
        }

        /// <summary>
        /// Buscar Codigo de la Entidad
        /// </summary>
        /// <param name="tsTipo">P: Primero, A: Anterior, S: Siguiente, U: Último</param>
        /// <param name="tsCodigo">Codigo de la entidad</param>
        /// <returns></returns>
        public string goBuscarCodigoBodega(string tsTipo, string tsCodigo = "")
        {
            string psCodigo = string.Empty;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<ADMMBODEGA>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdBodega }).OrderBy(x => x.IdBodega).FirstOrDefault()?.IdBodega.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<ADMMBODEGA>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdBodega }).OrderByDescending(x => x.IdBodega).FirstOrDefault()?.IdBodega.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<ADMMBODEGA>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdBodega }).ToList().Where(x => x.IdBodega < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdBodega).FirstOrDefault().IdBodega.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<ADMMBODEGA>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdBodega }).ToList().Where(x => x.IdBodega > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdBodega).FirstOrDefault().IdBodega.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            return psCodigo;

        }

        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestroBodega(int tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<ADMMBODEGA>().Where(x => x.IdBodega == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.SaveChanges();
            }
        }

        #endregion

        #region Funciones - Item
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public bool gbGuardarItem(Item toObject)
        {
            loBaseDa.CreateContext();
            bool pbResult = false;
            int psCodigo = 0;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = int.Parse(toObject.Codigo);
            var poObject = loBaseDa.Get<ADMMITEM>().Where(x => x.IdItem == psCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;
                poObject.CodigoTipo = toObject.Tipo;
                poObject.Costo = toObject.Costo;
                poObject.GrabaIva = toObject.GrabaIva;
                poObject.CodigoSubtipo = toObject.Subtipo;
                poObject.IdPresentacion = toObject.IdPresentacion;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }
            else
            {
                poObject = new ADMMITEM();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.TerminalIngreso = toObject.Terminal;
                poObject.CodigoTipo = toObject.Tipo;
                poObject.Costo = toObject.Costo;
                poObject.GrabaIva = toObject.GrabaIva;
                poObject.CodigoSubtipo = toObject.Subtipo;
                poObject.IdPresentacion = toObject.IdPresentacion;
                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }

            loBaseDa.SaveChanges();
            return pbResult;
        }

        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsDescripcion"></param>
        /// <returns></returns>
        public List<Item> goListarMaestroItem(string tsDescripcion = "")
        {
            return loBaseDa.Find<ADMMITEM>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Item
                   {
                       Codigo = x.IdItem.ToString(),
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Descripcion,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,
                       Tipo = x.CodigoTipo,
                       Costo = x.Costo??0,
                       GrabaIva = x.GrabaIva??false,
                       Subtipo = x.CodigoSubtipo,
                       IdPresentacion = x.IdPresentacion
                       
                   }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Item goBuscarMaestroItem(int tsCodigo)
        {
            return loBaseDa.Find<ADMMITEM>().Where(x => x.IdItem == tsCodigo)
                .Select(x => new Item
                {
                    Codigo = x.IdItem.ToString(),
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    Tipo = x.CodigoTipo,
                    Costo = x.Costo??0,
                    GrabaIva = x.GrabaIva ?? false,
                    Subtipo = x.CodigoSubtipo,
                    IdPresentacion = x.IdPresentacion,
                }).FirstOrDefault();
        }

        /// <summary>
        /// Buscar Codigo de la Entidad
        /// </summary>
        /// <param name="tsTipo">P: Primero, A: Anterior, S: Siguiente, U: Último</param>
        /// <param name="tsCodigo">Codigo de la entidad</param>
        /// <returns></returns>
        public string goBuscarCodigoItem(string tsTipo, string tsCodigo = "")
        {
            string psCodigo = string.Empty;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<ADMMITEM>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdItem }).OrderBy(x => x.IdItem).FirstOrDefault()?.IdItem.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<ADMMITEM>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdItem }).OrderByDescending(x => x.IdItem).FirstOrDefault()?.IdItem.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<ADMMITEM>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdItem }).ToList().Where(x => x.IdItem < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdItem).FirstOrDefault().IdItem.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<ADMMITEM>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdItem }).ToList().Where(x => x.IdItem > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdItem).FirstOrDefault().IdItem.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            return psCodigo;

        }

        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestroItem(int tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<ADMMITEM>().Where(x => x.IdItem == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.SaveChanges();
            }
        }

        #endregion

        #region Movimiento Inventario
        public string gsGuardarMovimiento(MovimientoInventarioAdm toObject, string tsUsuario, string tsTerminal, out int tId, bool LimpiarContexto = true)
        {
            tId = 0;
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            int pId = toObject.IdMovimientoInventario;

            psMsg = lsValidarDatosMovimientoInventario(toObject);
            if (string.IsNullOrEmpty(psMsg))
            {
                //Consulta entidad Cabecera
                var poObject = loBaseDa.Get<ADMTMOVIMIENTOINVENTARIO>().Include(x => x.ADMTMOVIMIENTOINVENTARIODETALLE).Where(x => x.IdMovimientoInventario == pId && x.IdMovimientoInventario != 0).FirstOrDefault();

                // Determinar que filas fueron eliminadas de la presentación
                var piListaIdPresentacion = toObject.MovimientoInventarioDetalleAdm.Where(x => x.IdMovimientoInventarioDetalle != 0).Select(x => x.IdMovimientoInventarioDetalle).ToList();
                if (poObject != null)
                {
                    foreach (var poItem in poObject.ADMTMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdMovimientoInventarioDetalle)))
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
                    poObject = new ADMTMOVIMIENTOINVENTARIO();
                    loBaseDa.CreateNewObject(out poObject);
                    pbuevo = true;
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                }

                
                poObject.CodigoEstado = Diccionario.Activo;
                poObject.Tipo = toObject.Tipo;
                poObject.Observaciones = toObject.Observaciones;
                poObject.CodigoMotivoMovInvAdm = toObject.CodigoMotivo;
                poObject.GrupoMotivoMovInvAdm = toObject.GrupoMotivo;
                poObject.CodigoCentroCosto = toObject.CodigoCentroCosto;
                poObject.CentroCosto = toObject.CentroCosto;
                poObject.Fecha = toObject.Fechamovimiento;

                if (toObject.MovimientoInventarioDetalleAdm != null)
                {

                    foreach (var item in toObject.MovimientoInventarioDetalleAdm)
                    {
                        int pIdDetalle = item.IdMovimientoInventarioDetalle;
                        var poObjectItem = poObject.ADMTMOVIMIENTOINVENTARIODETALLE.Where(x => x.IdMovimientoInventarioDetalle == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new ADMTMOVIMIENTOINVENTARIODETALLE();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.ADMTMOVIMIENTOINVENTARIODETALLE.Add(poObjectItem);
                        }
                        
                        if (poObject.Tipo == "I")
                        {
                            poObjectItem.Costo = item.Costo;
                        }
                        else
                        {
                            var poListaCosto = (from a in loBaseDa.Find<ADMTMOVIMIENTOINVENTARIO>()
                                                join b in loBaseDa.Find<ADMTMOVIMIENTOINVENTARIODETALLE>() on a.IdMovimientoInventario equals b.IdMovimientoInventario
                                                where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo
                                                && a.Tipo == "I" && b.IdItemAdm == item.IdItemEPP && b.Costo > 0
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
                            poObjectItem.GrabaIva = loBaseDa.Find<ADMMITEM>().Where(x => x.IdItem == item.IdItemEPP).Select(x => x.GrabaIva).FirstOrDefault();
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.IdBodegaAdm = item.IdBodegaEPP;
                        poObjectItem.IdItemAdm = item.IdItemEPP;
                        poObjectItem.Cantidad = item.Cantidad;
                        poObjectItem.PrecioSinImpuesto = item.PrecioSinImpuesto;
                        poObjectItem.Impuesto = item.Impuesto;
                        poObjectItem.GrabaIva = item.GrabaIva;


                    }
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();
                    tId = poObject.IdMovimientoInventario;
                    if (pbuevo)
                    {
                        string psMsgStock = "";
                        string psTipoStock = poObject.Tipo == Diccionario.TipoMovimiento.Ingreso ? "+" : "-";
                        foreach (var item in poObject.ADMTMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                        {
                            var dtStock = goConsultaDataTable(string.Format("EXEC [dbo].[ADMSPACTUALIZARSTOCK] '{0}','{1}','{2}','{3}'", item.IdItemAdm, item.IdBodegaAdm, item.Cantidad, psTipoStock));
                            if (dtStock == null || !string.IsNullOrEmpty(dtStock.Rows[0][0].ToString())) psMsg = string.Format(dtStock.Rows[0][0].ToString(), psMsg);

                            if (poObject.Tipo == "I")
                            {
                                var poItem = loBaseDa.Get<ADMMITEM>().Where(x => x.IdItem == item.IdItemAdm).FirstOrDefault();
                                if (poItem !=  null)
                                {
                                    var poListaCosto = (from a in loBaseDa.Find<ADMTMOVIMIENTOINVENTARIO>()
                                                        join b in loBaseDa.Find<ADMTMOVIMIENTOINVENTARIODETALLE>() on a.IdMovimientoInventario equals b.IdMovimientoInventario
                                                        where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo
                                                        && a.Tipo == "I" && b.IdItemAdm == item.IdItemAdm && b.Costo > 0
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

        public string gsAnularMovimiento(int tId, string tsUsuario, string tsTerminal, bool LimpiarContexto = true)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if (string.IsNullOrEmpty(psMsg))
            {
                //Consulta entidad Cabecera
                var poObject = loBaseDa.Get<ADMTMOVIMIENTOINVENTARIO>().Include(x => x.ADMTMOVIMIENTOINVENTARIODETALLE).Where(x => x.IdMovimientoInventario == tId && x.IdMovimientoInventario != 0).FirstOrDefault();

                // Determinar que filas fueron eliminadas de la presentación
                if (poObject != null)
                {
                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;

                    foreach (var poItem in poObject.ADMTMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {
                        poItem.CodigoEstado = Diccionario.Eliminado;
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();

                    string psMsgStock = "";
                    string psTipoStock = poObject.Tipo == Diccionario.TipoMovimiento.Ingreso ? "-" : "+";
                    foreach (var item in poObject.ADMTMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Eliminado))
                    {
                        var dtStock = goConsultaDataTable(string.Format("EXEC [dbo].[ADMSPACTUALIZARSTOCK] '{0}','{1}','{2}','{3}'", item.IdItemAdm, item.IdBodegaAdm, item.Cantidad, psTipoStock));
                        if (dtStock == null || !string.IsNullOrEmpty(dtStock.Rows[0][0].ToString())) psMsg = string.Format(dtStock.Rows[0][0].ToString(), psMsg);
                        if (poObject.Tipo == "I")
                        {
                            var poItem = loBaseDa.Get<ADMMITEM>().Where(x => x.IdItem == item.IdItemAdm).FirstOrDefault();
                            if (poItem != null)
                            {
                                var poListaCosto = (from a in loBaseDa.Find<ADMTMOVIMIENTOINVENTARIO>()
                                                    join b in loBaseDa.Find<ADMTMOVIMIENTOINVENTARIODETALLE>() on a.IdMovimientoInventario equals b.IdMovimientoInventario
                                                    where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo
                                                    && a.Tipo == "I" && b.IdItemAdm == item.IdItemAdm && b.Costo > 0
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


                    poTran.Complete();
                }
            }

            return psMsg;
        }

        public MovimientoInventarioAdm goConsultarMovimiento(int tId)
        {
            loBaseDa.CreateContext();
            var poCab = loBaseDa.Get<ADMTMOVIMIENTOINVENTARIO>().Include(x => x.ADMTMOVIMIENTOINVENTARIODETALLE).Where(x => x.IdMovimientoInventario == tId).FirstOrDefault();
            return goConsultarMovimiento(poCab);
        }

        public List<MovimientoInventarioAdm> listaMovimientos(string TipoMovimiento)
        {
            loBaseDa.CreateContext();

            var lista = (from a in loBaseDa.Get<ADMTMOVIMIENTOINVENTARIO>()
                         where a.CodigoEstado == "A" && a.Tipo == TipoMovimiento
                         select new MovimientoInventarioAdm
                         {
                             IdMovimientoInventario = a.IdMovimientoInventario,
                             Tipo = a.Tipo,
                             Fecha = a.FechaIngreso,
                             Observaciones = a.Observaciones,
                             CodigoMotivo = a.CodigoMotivoMovInvAdm,
                             GrupoMotivo = a.GrupoMotivoMovInvAdm,
                             CodigoCentroCosto = a.CodigoCentroCosto,
                             Fechamovimiento = a.Fecha??DateTime.Now,
                             
                             
                         }).ToList().OrderBy(x => x.IdMovimientoInventario).ToList();

            return lista;
        }

        private MovimientoInventarioAdm goConsultarMovimiento(ADMTMOVIMIENTOINVENTARIO poCab)
        {
            MovimientoInventarioAdm poObjectReturn = null;

            if (poCab != null)
            {
                poObjectReturn = new MovimientoInventarioAdm();

                poObjectReturn.IdMovimientoInventario = poCab.IdMovimientoInventario;
                poObjectReturn.Tipo = poCab.Tipo;
                poObjectReturn.Observaciones = poCab.Observaciones;
                poObjectReturn.CodigoMotivo = poCab.CodigoMotivoMovInvAdm;
                poObjectReturn.GrupoMotivo = poCab.GrupoMotivoMovInvAdm;
                poObjectReturn.CodigoCentroCosto = poCab.CodigoCentroCosto;
                poObjectReturn.Fechamovimiento = poCab.Fecha ?? DateTime.Now;
                poObjectReturn.Fecha = poCab.FechaIngreso;

                poObjectReturn.MovimientoInventarioDetalleAdm = new List<MovimientoInventarioDetalleAdm>();
                foreach (var detalle in poCab.ADMTMOVIMIENTOINVENTARIODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new MovimientoInventarioDetalleAdm();

                    poDet.IdMovimientoInventarioDetalle = detalle.IdMovimientoInventarioDetalle;
                    poDet.IdMovimientoInventario = detalle.IdMovimientoInventario;
                    poDet.IdItemEPP = detalle.IdItemAdm;
                    poDet.IdBodegaEPP = detalle.IdBodegaAdm;
                    poDet.Cantidad = detalle.Cantidad;
                    poDet.GrabaIva = detalle.GrabaIva??false;
                    poDet.PrecioSinImpuesto = detalle.PrecioSinImpuesto??0M;

                    poObjectReturn.MovimientoInventarioDetalleAdm.Add(poDet);
                }
            }
            return poObjectReturn;
        }

        private string lsValidarDatosMovimientoInventario(MovimientoInventarioAdm toObject)
        {
            string psMsg = "";

            if (string.IsNullOrEmpty(toObject.Observaciones))
            {
                psMsg = string.Format("{0}Ingrese Observaciones. \n", psMsg);
            }

            if (toObject.CodigoMotivo == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Motivo. \n", psMsg);
            }

            if (toObject.CentroCosto == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Centro de Costo. \n", psMsg);
            }

            if (toObject.MovimientoInventarioDetalleAdm.Count == 0)
            {
                psMsg = string.Format("{0}Ingrese detalle de ítems. \n", psMsg);
            }


            var loCombo = goConsultarComboItemEPP();



            var poLista = toObject.MovimientoInventarioDetalleAdm;
            var poListaRecorrida = new List<MovimientoInventarioDetalleAdm>();
            int fila = 1;
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

                else if (poListaRecorrida.Where(x => x.IdItemEPP == item.IdItemEPP).Count() > 0)
                {
                    psMsg = string.Format("{0}Eliminar Fila # {1}, Ya existe ingresado el ítem: {2}\n", psMsg, fila, psName);
                }

                if (toObject.Tipo == "I")
                {
                    if (item.Costo == 0M)
                    {
                        psMsg = string.Format("{0}Ingrese el costo del ítem: {1}. en la Fila # {2}\n", psMsg, psName, fila);
                    }
                }

                poListaRecorrida.Add(item);
                fila++;
            }


            return psMsg;
        }

        /// <summary>
        /// Buscar Codigo de la Entidad
        /// </summary>
        /// <param name="tsTipo">P: Primero, A: Anterior, S: Siguiente, U: Último</param>
        /// <param name="tsCodigo">Codigo de la entidad</param>
        /// <returns></returns>
        public string goBuscarCodigoMovInv(string tsTipo, string tsCodigo = "")
        {
            string psCodigo = string.Empty;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<ADMTMOVIMIENTOINVENTARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdMovimientoInventario }).OrderBy(x => x.IdMovimientoInventario).FirstOrDefault()?.IdMovimientoInventario.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<ADMTMOVIMIENTOINVENTARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdMovimientoInventario }).OrderByDescending(x => x.IdMovimientoInventario).FirstOrDefault()?.IdMovimientoInventario.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<ADMTMOVIMIENTOINVENTARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdMovimientoInventario }).ToList().Where(x => x.IdMovimientoInventario < int.Parse(tsCodigo)).ToList();
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
                var poListaCodigo = loBaseDa.Find<ADMTMOVIMIENTOINVENTARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdMovimientoInventario }).ToList().Where(x => x.IdMovimientoInventario > int.Parse(tsCodigo)).ToList();

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

        #endregion

    }
}
