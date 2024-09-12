using GEN_Entidad;
using GEN_Entidad.Entidades.ActivoFijo;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static GEN_Entidad.Diccionario;

namespace AFI_Negocio
{
    public class clsNActivoFijo : clsNBase
    {
        /// <summary>
        /// Buscar Entidad
        /// </summary>
        /// <returns></returns>
        public ItemActivoFijo goBuscarItemActivoFijo(int tId)
        {
            return loBaseDa.Find<AFIPITEMACTIVOFIJO>()
                .Where(x => x.IdItemActivoFijo == tId).ToList()
                .Select(x => new ItemActivoFijo
                {
                    IdItemActivoFijo = x.IdItemActivoFijo,
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    CodigoTipoActivoFijo = x.CodigoTipoActivoFijo,
                    CodigoSucursal = x.CodigoSucursal,
                    CodigoCentroCosto = x.CodigoCentroCosto,
                    FechaCompra = x.FechaCompra,
                    FechaActivacion = x.FechaActivacion,
                    CostoCompra = x.CostoCompra,
                    ValorResidual = x.ValorResidual,
                    ValorDepreciable = x.ValorDepreciable,
                    DepreciacionAcumulada = x.DepreciacionAcumulada,
                    CostoActual = x.CostoActual,
                    Modelo = x.Modelo,
                    Marca = x.Marca,
                    Serie = x.Serie,
                    ProductId = x.ProductId,
                    FechaRegistro = x.FechaIngreso,
                    Codigo = x.Codigo,
                    IdPersona = x.IdPersona,
                    Persona = x.Persona,
                    NumFactura = x.NumFactura,
                    CodigoEstadoActivoFijo = x.CodigoEstadoActivoFijo,
                    CodigoAgrupacion = x.CodigoAgrupacion,
                    NombreOriginal = x.NombreOriginal,
                    ArchivoAdjunto = x.ArchivoAdjunto,
                    RutaDestino = ConfigurationManager.AppSettings["CarpetaAfiIaf"].ToString(),
                    IdProveedor = x.IdProveedor,
                    Proveedor = x.Proveedor
        }).FirstOrDefault();
        }

        /// <summary>
        /// Listar Entidades
        /// </summary>
        /// <returns></returns>
        public List<ItemActivoFijo> goListarItemActivoFijo()
        {
            //var poProveedores = loBaseDa.Find<COMMPROVEEDORES>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new { x.IdProveedor, x.Nombre }).ToList();
            return loBaseDa.Find<AFIPITEMACTIVOFIJO>()
                .Where(x => x.CodigoEstado == Diccionario.Activo).ToList()
                .Select(x => new ItemActivoFijo
                {
                    IdItemActivoFijo = x.IdItemActivoFijo,
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    CodigoTipoActivoFijo = x.CodigoTipoActivoFijo,
                    CodigoSucursal = x.CodigoSucursal,
                    CodigoCentroCosto = x.CodigoCentroCosto,
                    FechaCompra = x.FechaCompra,
                    FechaActivacion = x.FechaActivacion,
                    CostoCompra = x.CostoCompra,
                    ValorResidual = x.ValorResidual,
                    ValorDepreciable = x.ValorDepreciable,
                    DepreciacionAcumulada = x.DepreciacionAcumulada,
                    CostoActual = x.CostoActual,
                    Modelo = x.Modelo,
                    Marca = x.Marca,
                    Serie = x.Serie,
                    ProductId = x.ProductId,
                    Codigo = x.Codigo,
                    IdPersona = x.IdPersona,
                    Persona = x.Persona,
                    NumFactura = x.NumFactura,
                    CodigoEstadoActivoFijo = x.CodigoEstadoActivoFijo,
                    CodigoAgrupacion = x.CodigoAgrupacion,
                    NombreOriginal = x.NombreOriginal,
                    ArchivoAdjunto = x.ArchivoAdjunto,
                    RutaDestino = ConfigurationManager.AppSettings["CarpetaAfiIaf"].ToString(),
                    IdProveedor = x.IdProveedor,
                    Proveedor = !string.IsNullOrEmpty(x.Proveedor) && x.Proveedor.Length > 50 ? x.Proveedor.Substring(0, 50) : x.Proveedor,
                }).ToList();
        }

        /// <summary>
        /// Listar Entidades
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboActivoFijo()
        {
            List<string> psEstados = new List<string>();
            psEstados.Add(Diccionario.Eliminado);
            psEstados.Add(Diccionario.Inactivo);

            return loBaseDa.Find<AFIPITEMACTIVOFIJO>()
                .Where(x => !psEstados.Contains(x.CodigoEstado))
                .Select(x => new Combo
                {
                    Codigo = x.IdItemActivoFijo.ToString(),
                    Descripcion = x.Descripcion,
                }).ToList().OrderBy(x=>x.Descripcion).ToList();
        }

        public int giTieneDepreciaciones(int tId)
        {
            List<string> psEstado = new List<string>();
            psEstado.Add(Diccionario.Eliminado);
            psEstado.Add(Diccionario.Inactivo);
            return loBaseDa.Find<AFIPITEMACTIVOFIJOHISTORICO>().Where(x => !psEstado.Contains(x.CodigoEstado) && x.IdItemActivoFijo == tId).Count();
        }
        
        public string gsUsuarioIngreso(int tId)
        {
            return loBaseDa.Find<AFIPITEMACTIVOFIJO>().Where(x => x.IdItemActivoFijo == tId).Select(x => x.UsuarioIngreso).FirstOrDefault();
        }

        public string gEliminar(int tId, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";
            

            if (giTieneDepreciaciones(tId) > 0)
            {
                psMsg = string.Format("No es posible eliminar existen depreciaciones con este activo.");
            }

            if (string.IsNullOrEmpty(psMsg))
            {
                var poObject = loBaseDa.Get<AFIPITEMACTIVOFIJO>().Where(x => x.IdItemActivoFijo == tId).FirstOrDefault();

                if (poObject != null)
                {
                    if (poObject.UsuarioIngreso != "MIGRACION")
                    {
                        poObject.CodigoEstado = Diccionario.Eliminado;
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;

                        loBaseDa.SaveChanges();
                    }
                    else
                    {
                        psMsg = string.Format("No es posible eliminar registro migrado.");
                    }
                }
            }
            
            return psMsg;
        }

        /// <summary>
        /// Buscar Codigo de la Entidad
        /// </summary>
        /// <param name="tsTipo">P: Primero, A: Anterior, S: Siguiente, U: Último</param>
        /// <param name="tsCodigo">Codigo de la entidad</param>
        /// <returns></returns>
        public string goBuscarCodigo(string tsTipo, string tsCodigo = "")
        {
            int psCodigo = 0;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<AFIPITEMACTIVOFIJO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdItemActivoFijo }).OrderBy(x => x.IdItemActivoFijo).FirstOrDefault().IdItemActivoFijo;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<AFIPITEMACTIVOFIJO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdItemActivoFijo }).OrderByDescending(x => x.IdItemActivoFijo).FirstOrDefault().IdItemActivoFijo;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<AFIPITEMACTIVOFIJO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdItemActivoFijo }).ToList().Where(x => x.IdItemActivoFijo < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdItemActivoFijo).FirstOrDefault().IdItemActivoFijo;
                }
                else
                {
                    psCodigo = int.Parse(tsCodigo);
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<AFIPITEMACTIVOFIJO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdItemActivoFijo }).ToList().Where(x => x.IdItemActivoFijo > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdItemActivoFijo).FirstOrDefault().IdItemActivoFijo;
                }
                else
                {
                    psCodigo = int.Parse(tsCodigo);
                }
            }
            return psCodigo.ToString();

        }

        /// <summary>
        /// Guardar Objeto 
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardarItemActivoFijo(ItemActivoFijo toObject, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            string psResult = "";
            int pId = toObject.IdItemActivoFijo;
            
            var poObject = loBaseDa.Get<AFIPITEMACTIVOFIJO>().Where(x => x.IdItemActivoFijo == pId).FirstOrDefault();
            if (poObject != null)
            {
                poObject.UsuarioModificacion = tsUsuario;
                poObject.FechaModificacion = DateTime.Now;
                poObject.TerminalModificacion = tsTerminal;
            }
            else
            {

                var poTipoActivo = loBaseDa.Get<AFIPTIPOACTIVOFIJO>().Where(x => x.CodigoTipoActivoFijo == toObject.CodigoTipoActivoFijo).FirstOrDefault();

                poObject = new AFIPITEMACTIVOFIJO();
                loBaseDa.CreateNewObject(out poObject);
                poObject.UsuarioIngreso = tsUsuario;
                poObject.FechaIngreso = DateTime.Now;
                poObject.TerminalIngreso = tsTerminal;
                poObject.Codigo = poTipoActivo.Alias + "-" + new String('0', (4 - poTipoActivo.ProximaSecuencia.ToString().Length)) + poTipoActivo.ProximaSecuencia.ToString();//string.Format("", poTipoActivo.ProximaSecuencia);

                poTipoActivo.ProximaSecuencia = poTipoActivo.ProximaSecuencia + 1;
            }

            poObject.CodigoEstado = Diccionario.Activo;
            poObject.Descripcion = toObject.Descripcion;
            poObject.CodigoTipoActivoFijo = toObject.CodigoTipoActivoFijo;
            poObject.CodigoSucursal = toObject.CodigoSucursal;
            poObject.CodigoCentroCosto = toObject.CodigoCentroCosto;
            poObject.FechaCompra = toObject.FechaCompra;
            poObject.FechaActivacion = toObject.FechaActivacion;
            poObject.CostoCompra = toObject.CostoCompra;
            poObject.ValorResidual = toObject.ValorResidual;
            poObject.ValorDepreciable = toObject.ValorDepreciable;
            poObject.DepreciacionAcumulada = toObject.DepreciacionAcumulada;
            poObject.CostoActual = toObject.CostoActual;
            poObject.Modelo = toObject.Modelo;
            poObject.Marca = toObject.Marca;
            poObject.Serie = toObject.Serie;
            poObject.ProductId = toObject.ProductId;
            poObject.IdPersona = toObject.IdPersona;
            poObject.Persona = toObject.Persona;
            poObject.NumFactura = toObject.NumFactura;
            poObject.CodigoEstadoActivoFijo = toObject.CodigoEstadoActivoFijo;
            poObject.CodigoAgrupacion = toObject.CodigoAgrupacion;
            poObject.Agrupacion = toObject.Agrupacion;
            poObject.NombreOriginal = toObject.NombreOriginal;
            poObject.ArchivoAdjunto = toObject.ArchivoAdjunto;
            poObject.IdProveedor = toObject.IdProveedor;
            poObject.Proveedor = toObject.Proveedor;

            using (var poTran = new TransactionScope())
            {
                loBaseDa.SaveChanges();

                if (!string.IsNullOrEmpty(toObject.ArchivoAdjunto) && !string.IsNullOrEmpty(toObject.RutaDestino))
                {
                    if (toObject.RutaOrigen != toObject.RutaDestino)
                    {
                        if (!string.IsNullOrEmpty(toObject.RutaOrigen))
                        {
                            File.Copy(toObject.RutaOrigen, toObject.RutaDestino);
                        }

                    }
                }

                poTran.Complete();
            }
            
            return psResult;
        }

        public TipoActivoFijo goBuscarTipoActivoFijo(string tsCodigo)
        {
            return loBaseDa.Find<AFIPTIPOACTIVOFIJO>().Where(x => x.CodigoTipoActivoFijo == tsCodigo).Select(x => new TipoActivoFijo()
            {
                Alias = x.Alias,
                CodigoTipoActivoFijo = x.CodigoTipoActivoFijo,
                CodigoEstado = x.CodigoEstado,
                Depreciable = x.Depreciable,
                Descripcion = x.Descripcion,
                PorcentajeDepreciacion = x.PorcentajeDepreciacion,
                PorcentajeResidual = x.PorcentajeResidual,
                VidaUtil = x.VidaUtil,
                ValorResidual = x.ValorResidual

            }).FirstOrDefault();
        }

        public string gsEstadoDepreciacion(int tiAnio, int tiMes)
        {
            var psList = new List<string>();

            psList.Add(Diccionario.Activo);
            psList.Add(Diccionario.Pendiente);
            psList.Add(Diccionario.Aprobado);
            psList.Add(Diccionario.Cerrado);

            return loBaseDa.Find<AFITACTIVOFIJO>().Where(x => psList.Contains(x.CodigoEstado) && x.Anio == tiAnio && x.Mes == tiMes).Select(x => x.CodigoEstado).FirstOrDefault();
        }

        public string gsCerrarDepreciacion(string tsUsuario,int tiAnio, int tiMes)
        {
            string psMsg = "";

            loBaseDa.CreateContext();

            var poObject = loBaseDa.Get<AFITACTIVOFIJO>().Where(x => x.CodigoEstado == Diccionario.Pendiente && x.Anio == tiAnio && x.Mes == tiMes).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Cerrado;
                poObject.FechaModificacion = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;

                int Id = poObject.IdActivoFijo;

                var poListaRec = loBaseDa.Find<AFIPITEMACTIVOFIJOHISTORICO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdActivoFijo == Id).ToList();
                var poListaUpd = loBaseDa.Get<AFIPITEMACTIVOFIJO>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

                foreach (var item in poListaRec)
                {
                    var poRegistro = poListaUpd.Where(x => x.IdItemActivoFijo == item.IdItemActivoFijo).FirstOrDefault();
                    if (poRegistro != null)
                    {
                        poRegistro.CostoActual = item.CostoActual;
                        poRegistro.DepreciacionAcumulada = item.DepreAcumuladaTotal;
                        poRegistro.FechaModificacion = DateTime.Now;
                        poRegistro.UsuarioModificacion = tsUsuario;

                        if (poRegistro.DepreciacionAcumulada >= poRegistro.ValorDepreciable)
                        {
                            poRegistro.Depreciado = true;
                            poRegistro.CodigoEstadoActivoFijo = "DEP";
                        }
                    }
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();
                    loBaseDa.ExecuteQuery(string.Format("EXEC AFISPGUARDAHISTORIAACTIVOFIJO '{0}','{1}'", tiAnio, tiMes));
                    poTran.Complete();
                }
                    
            }
            else
            {
                psMsg = "No existen datos para cerrar depreciación";
            }


            return psMsg;
        }

        public string gsReversarDepreciacion(string tsUsuario, int tiAnio, int tiMes)
        {
            string psMsg = "";

            loBaseDa.CreateContext();

            var poObject = loBaseDa.Get<AFITACTIVOFIJO>().Where(x => x.CodigoEstado == Diccionario.Cerrado && x.Anio == tiAnio && x.Mes == tiMes).FirstOrDefault();
            if (poObject != null)
            {

                int ultAnio = 0;
                int ultMes = 0;

                var poListUlt = loBaseDa.Get<AFITACTIVOFIJO>().Where(x => x.CodigoEstado == Diccionario.Cerrado).Select(x => new { x.Anio, x.Mes }).ToList();
                if (poListUlt.Count > 0)
                {
                    ultAnio = poListUlt.Select(x => x.Anio).Max();
                    ultMes = loBaseDa.Get<AFITACTIVOFIJO>().Where(x => x.CodigoEstado == Diccionario.Cerrado && x.Anio == ultAnio).Select(x => x.Mes).Max();
                }

                if (ultAnio != 0)
                {
                    var dt = new DateTime(ultAnio, ultMes, 1).AddMonths(1);
                    if (ultAnio == tiAnio && ultMes == tiMes)
                    {

                    }
                    else
                    {
                        psMsg = string.Format("No es posible Reversar!! Se dede reversar desde el último hasta el primero. Último Periodo: {0}-{1}", ultAnio, ultMes);
                    }

                }

                if (string.IsNullOrEmpty(psMsg))
                {
                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;

                    int Id = poObject.IdActivoFijo;

                    var poListaRec = loBaseDa.Get<AFIPITEMACTIVOFIJOHISTORICO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdActivoFijo == Id).ToList();
                    var poListaUpd = loBaseDa.Get<AFIPITEMACTIVOFIJO>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

                    foreach (var item in poListaRec)
                    {
                        item.CodigoEstado = Diccionario.Eliminado;
                        item.FechaModificacion = DateTime.Now;
                        item.UsuarioModificacion = tsUsuario;

                        var poRegistro = poListaUpd.Where(x => x.IdItemActivoFijo == item.IdItemActivoFijo).FirstOrDefault();
                        if (poRegistro != null)
                        {
                            poRegistro.CostoActual = item.CostoActual + item.DepreciacionMensual;
                            poRegistro.DepreciacionAcumulada = item.DepreciacionAcumulada??0;
                            poRegistro.FechaModificacion = DateTime.Now;
                            poRegistro.UsuarioModificacion = tsUsuario;

                            if (poRegistro.DepreciacionAcumulada < poRegistro.ValorDepreciable)
                            {
                                poRegistro.Depreciado = false;
                                poRegistro.CodigoEstadoActivoFijo = "ACT";
                            }
                        }
                    }


                    loBaseDa.SaveChanges();
                }
            }
            else
            {
                psMsg = "El Periodo debe estar en un estado cerrado para reversar";
            }


            return psMsg;
        }

        public string gsValidaPrevioGeneracion(int tiAnio, int tiMes)
        {

            loBaseDa.CreateContext();

            string psMsg = "";

            int ultAnio = 0;
            int ultMes = 0;

            var poListUlt = loBaseDa.Get<AFITACTIVOFIJO>().Where(x => x.CodigoEstado == Diccionario.Cerrado).Select(x => new { x.Anio, x.Mes }).ToList();
            if (poListUlt.Count > 0)
            {
                ultAnio  = poListUlt.Select(x => x.Anio).Max();
                ultMes = loBaseDa.Get<AFITACTIVOFIJO>().Where(x => x.CodigoEstado == Diccionario.Cerrado && x.Anio == ultAnio).Select(x => x.Mes).Max();
            }
            
            if (ultAnio != 0)
            {
                var dt = new DateTime(ultAnio, ultMes, 1).AddMonths(1);
                if (dt.Year == tiAnio && dt.Month == tiMes)
                {

                }
                else
                {
                    psMsg = string.Format("No es posible Generar!! Periodo que continua es {0}-{1}", dt.Year, dt.Month);
                }

            }

            ultAnio = 0;
            ultMes = 0;
            poListUlt = loBaseDa.Get<AFITACTIVOFIJO>().Where(x => x.CodigoEstado == Diccionario.Pendiente).Select(x => new { x.Anio, x.Mes }).ToList();
            if (poListUlt.Count > 0)
            {
                ultAnio = poListUlt.Select(x => x.Anio).FirstOrDefault();
                ultMes = poListUlt.Select(x => x.Mes).FirstOrDefault();

            }

            if (ultAnio != 0)
            {
                var dt = new DateTime(ultAnio, ultMes, 1).AddMonths(1);
                if (ultAnio == tiAnio && ultMes == tiMes)
                {

                }
                else
                {
                    psMsg = string.Format("No es posible Generar!! Existe un periodo pendiente: {0}-{1}, por favor cerrar ese periodo para poder generar el siguiente.", ultAnio, ultMes);
                }

            }


            return psMsg;
        }

        #region Movimiento
        public List<MovimientoActivoFijo> goListarMovimientoActivoFijo()
        {
            return (from a in loBaseDa.Find<AFITMOVIMIENTOACTIVOFIJO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                    join b in loBaseDa.Find<GENMCATALOGO>() 
                    on new { t0 = Diccionario.ListaCatalogo.TipoMovimientoActivo, t1 = a.CodigoTipoMovimiento } equals new { t0 = b.CodigoGrupo, t1 = b.Codigo }
                    join c in loBaseDa.Find<AFIPITEMACTIVOFIJO>() on a.IdItemActivoFijo equals c.IdItemActivoFijo
                     select new MovimientoActivoFijo()
                     {
                         CodigoEstado = a.CodigoEstado,
                         CodigoTipoMovimiento = a.CodigoTipoMovimiento,
                         TipoMovimiento = b.Descripcion,
                         IdItemActivoFijo = a.IdItemActivoFijo,
                         IdMovimientoActivoFijo = a.IdMovimientoActivoFijo,
                         ActivoFijo = a.ActivoFijo,
                         CostoCompra = a.CostoCompra,
                         ValorResidual = a.ValorResidual,
                         ValorDepreciable = a.ValorDepreciable,
                         DepreciacionAcumulada = a.DepreciacionAcumulada,
                         CostoActual = a.CostoActual,
                         Observacion = a.Observacion,
                         ValorVenta = a.ValorVenta,
                         FechaMovimiento = a.FechaMovimiento,
                         CodigoActivoFijo = c.Codigo,
                     }).ToList();
        }

        public MovimientoActivoFijo goConsultaMovimientoActivoFijo(int tIdMovimientoActivoFijo)
        {
            return loBaseDa.Find<AFITMOVIMIENTOACTIVOFIJO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdMovimientoActivoFijo == tIdMovimientoActivoFijo).Select(x => new MovimientoActivoFijo()
            {
                CodigoEstado = x.CodigoEstado,
                CodigoTipoMovimiento = x.CodigoTipoMovimiento,
                IdItemActivoFijo = x.IdItemActivoFijo,
                CostoCompra = x.CostoCompra,
                ValorResidual = x.ValorResidual,
                ValorDepreciable = x.ValorDepreciable,
                DepreciacionAcumulada = x.DepreciacionAcumulada,
                CostoActual = x.CostoActual,
                Observacion = x.Observacion,
                ValorVenta = x.ValorVenta,
                IdMovimientoActivoFijo = x.IdMovimientoActivoFijo,
                FechaMovimiento = x.FechaMovimiento,
                ActivoFijo = x.ActivoFijo,
            }).FirstOrDefault();
        }

        public MovimientoActivoFijo goConsultaMovimientoItemActivoFijo(int IdItemActivoFijo)
        {
            return loBaseDa.Find<AFITMOVIMIENTOACTIVOFIJO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdItemActivoFijo == IdItemActivoFijo).Select(x => new MovimientoActivoFijo()
            {
                CodigoEstado = x.CodigoEstado,
                CodigoTipoMovimiento = x.CodigoTipoMovimiento,
                IdMovimientoActivoFijo = x.IdMovimientoActivoFijo,
                IdItemActivoFijo = x.IdItemActivoFijo,
                ActivoFijo = x.ActivoFijo,
                CostoCompra = x.CostoCompra,
                ValorResidual = x.ValorResidual,
                ValorDepreciable = x.ValorDepreciable,
                DepreciacionAcumulada = x.DepreciacionAcumulada,
                CostoActual = x.CostoActual,
                Observacion = x.Observacion,
                ValorVenta = x.ValorVenta,

            }).FirstOrDefault();
        }

        private string psEsValidoGuardarMovimiento(MovimientoActivoFijo toObject)
        {
            string psMsg = "";
            if (toObject.FechaMovimiento == DateTime.MinValue)
            {
                psMsg = string.Format("{0}Ingrese la Fecha de Movimiento.\n", psMsg);
            }
            if (toObject.IdItemActivoFijo.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Activo.\n", psMsg);
            }
            if (toObject.CodigoTipoMovimiento == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Tipo de Movimiento.\n", psMsg);
            }
            
            if (toObject.CodigoTipoMovimiento == "VTA")
            {
                if (toObject.ValorVenta == null )
                {
                    psMsg = string.Format("{0}Ingrese el valor de la venta.\n", psMsg);
                }
                else
                {
                    if (toObject.ValorVenta <= 0)
                    {
                        psMsg = string.Format("{0}Ingrese el valor de la venta.\n", psMsg);
                    }
                }
            }
            
            return psMsg;
        }

        public string gsGuardarMovimientoActivoFijo(MovimientoActivoFijo toObject, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();

            string psMsg = psEsValidoGuardarMovimiento(toObject);

            if (string.IsNullOrEmpty(psMsg))
            {
                var poObject = loBaseDa.Get<AFITMOVIMIENTOACTIVOFIJO>().Where(x => x.IdMovimientoActivoFijo == toObject.IdMovimientoActivoFijo && toObject.IdMovimientoActivoFijo != 0).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObject = new AFITMOVIMIENTOACTIVOFIJO();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.TerminalIngreso = tsTerminal;
                }

                poObject.CodigoEstado = Diccionario.Activo;
                poObject.CodigoTipoMovimiento = toObject.CodigoTipoMovimiento;
                poObject.IdItemActivoFijo = toObject.IdItemActivoFijo;
                poObject.ActivoFijo = toObject.ActivoFijo;
                poObject.CostoCompra = toObject.CostoCompra;
                poObject.ValorResidual = toObject.ValorResidual;
                poObject.ValorDepreciable = toObject.ValorDepreciable;
                poObject.DepreciacionAcumulada = toObject.DepreciacionAcumulada;
                poObject.CostoActual = toObject.CostoActual;
                poObject.Observacion = toObject.Observacion;
                poObject.ValorVenta = toObject.ValorVenta;
                poObject.FechaMovimiento = toObject.FechaMovimiento;

                var poItem = loBaseDa.Get<AFIPITEMACTIVOFIJO>().Where(x => x.IdItemActivoFijo == toObject.IdItemActivoFijo).FirstOrDefault();
                poObject.CodigoEstadoActivoFijoAnt = poItem.CodigoEstadoActivoFijo;

                if (toObject.CodigoTipoMovimiento == "VTA" || toObject.CodigoTipoMovimiento == "BAJ")
                {
                    string psEstadoActivo = toObject.CodigoTipoMovimiento; // Los Códigos son iguales tanto de estado de activo como el tipo de movimiento.
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                    poItem.CodigoEstadoActivoFijo = psEstadoActivo;
                    
                }

                
                loBaseDa.SaveChanges();

            }

            return psMsg;
        }

        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public string gEliminarMovimientoActivoFijo(int tId, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";

            var poObject = loBaseDa.Get<AFITMOVIMIENTOACTIVOFIJO>().Where(x => x.IdMovimientoActivoFijo == tId).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;

                var poItem = loBaseDa.Get<AFIPITEMACTIVOFIJO>().Where(x => x.IdItemActivoFijo == poObject.IdItemActivoFijo).FirstOrDefault();
                if (poItem != null)
                {
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.TerminalModificacion = tsTerminal;
                    poItem.CodigoEstadoActivoFijo = poObject.CodigoEstadoActivoFijoAnt;
                }

                loBaseDa.SaveChanges();
            }

            return psMsg;
        }
        #endregion
    }
}
