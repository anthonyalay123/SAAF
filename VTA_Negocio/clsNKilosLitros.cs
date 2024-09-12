using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTA_Negocio
{
    public class clsNKilosLitros : clsNBase
    {

        #region Kilos Litros Familia
        /// <summary>
        /// Consulta de parametriaciones de presupuesto Kilos litros por Familia
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public List<PresupuestoKilosLitrosFamiliaGrid> goConsultarParametrizacionesFamilia(int tiPeriodo = 0)
        {

            return loBaseDa.Find<VTAPPRESUPUESTOKILOSLITROSFAMILIA>().Where(X => X.CodigoEstado == Diccionario.Activo && (tiPeriodo != 0 ? X.Periodo == tiPeriodo : true))
                .Select(x => new PresupuestoKilosLitrosFamiliaGrid()
                {
                    IdPresupuestoKilosLitrosFamilia = x.IdPresupuestoKilosLitrosFamilia,
                    Periodo = x.Periodo,
                    CodeZona = x.CodeZona,
                    NameZona = x.NameZona,
                    Familia = x.Familia,
                    Ene = x.Ene,
                    Feb = x.Feb,
                    Mar = x.Mar,
                    Abr = x.Abr,
                    May = x.May,
                    Jun = x.Jun,
                    Jul = x.Jul,
                    Ago = x.Ago,
                    Sep = x.Sep,
                    Oct = x.Oct,
                    Nov = x.Nov,
                    Dic = x.Dic,
                    Observacion = x.Observacion,
                }).ToList();

        
        }
        
        /// <summary>
        /// Guardar pregupuesto de Kilos litros - Familia
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsGuardarFamilia(List<PresupuestoKilosLitrosFamiliaGrid> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if (toLista != null && toLista.Count> 0)
            {
                int piPeriodo = toLista.Select(x => x.Periodo).FirstOrDefault();

                var poLista = loBaseDa.Get<VTAPPRESUPUESTOKILOSLITROSFAMILIA>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Periodo == piPeriodo).ToList();
                
                var piListaIdPresentacion = toLista.Where(x => x.IdPresupuestoKilosLitrosFamilia != 0).Select(x => x.IdPresupuestoKilosLitrosFamilia).ToList();

                foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdPresupuestoKilosLitrosFamilia)))
                {
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdPresupuestoKilosLitrosFamilia == poItem.IdPresupuestoKilosLitrosFamilia && x.IdPresupuestoKilosLitrosFamilia != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new VTAPPRESUPUESTOKILOSLITROSFAMILIA();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.Periodo = poItem.Periodo;
                    poObject.CodeZona = poItem.CodeZona;
                    poObject.Familia = poItem.Familia;
                    poObject.NameZona = poItem.NameZona;
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Ene = poItem.Ene;
                    poObject.Feb = poItem.Feb;
                    poObject.Mar = poItem.Mar;
                    poObject.Abr = poItem.Abr;
                    poObject.May = poItem.May;
                    poObject.Jun = poItem.Jun;
                    poObject.Jul = poItem.Jul;
                    poObject.Ago = poItem.Ago;
                    poObject.Sep = poItem.Sep;
                    poObject.Oct = poItem.Oct;
                    poObject.Nov = poItem.Nov;
                    poObject.Dic = poItem.Dic;
                    poObject.Total = poItem.Total;
                    poObject.Observacion = poItem.Observacion;
                }

                loBaseDa.SaveChanges();

            }

            return psMsg;
        }
        #endregion

        #region Kilos Litros Grupo
        /// <summary>
        /// Consulta de parametriaciones de presupuesto Kilos litros por Grupo
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public List<PresupuestoKilosLitrosGrupoGrid> goConsultarParametrizacionesGrupo(int tiPeriodo = 0)
        {

            return loBaseDa.Find<VTAPPRESUPUESTOKILOSLITROSGRUPO>().Where(X => X.CodigoEstado == Diccionario.Activo && (tiPeriodo != 0 ? X.Periodo == tiPeriodo : true))
                .Select(x => new PresupuestoKilosLitrosGrupoGrid()
                {
                    IdPresupuestoKilosLitrosGrupo = x.IdPresupuestoKilosLitrosGrupo,
                    Periodo = x.Periodo,
                    CodeZona = x.CodeZona,
                    NameZona = x.NameZona,
                    GroupCode = x.ItmsGrpCod,
                    GroupName = x.ItmsGrpNam,
                    Ene = x.Ene,
                    Feb = x.Feb,
                    Mar = x.Mar,
                    Abr = x.Abr,
                    May = x.May,
                    Jun = x.Jun,
                    Jul = x.Jul,
                    Ago = x.Ago,
                    Sep = x.Sep,
                    Oct = x.Oct,
                    Nov = x.Nov,
                    Dic = x.Dic,
                    Observacion = x.Observacion,
                }).ToList();


        }

        /// <summary>
        /// Guardar pregupuesto de Kilos litros - Grupo
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsGuardarGrupo(List<PresupuestoKilosLitrosGrupoGrid> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if (toLista != null && toLista.Count > 0)
            {
                int piPeriodo = toLista.Select(x => x.Periodo).FirstOrDefault();

                var poLista = loBaseDa.Get<VTAPPRESUPUESTOKILOSLITROSGRUPO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Periodo == piPeriodo).ToList();

                var piListaIdPresentacion = toLista.Where(x => x.IdPresupuestoKilosLitrosGrupo != 0).Select(x => x.IdPresupuestoKilosLitrosGrupo).ToList();

                foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdPresupuestoKilosLitrosGrupo)))
                {
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdPresupuestoKilosLitrosGrupo == poItem.IdPresupuestoKilosLitrosGrupo && x.IdPresupuestoKilosLitrosGrupo != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new VTAPPRESUPUESTOKILOSLITROSGRUPO();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.Periodo = poItem.Periodo;
                    poObject.CodeZona = poItem.CodeZona;
                    poObject.ItmsGrpCod = poItem.GroupCode;
                    poObject.ItmsGrpNam = poItem.GroupName;
                    poObject.NameZona = poItem.NameZona;
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Ene = poItem.Ene;
                    poObject.Feb = poItem.Feb;
                    poObject.Mar = poItem.Mar;
                    poObject.Abr = poItem.Abr;
                    poObject.May = poItem.May;
                    poObject.Jun = poItem.Jun;
                    poObject.Jul = poItem.Jul;
                    poObject.Ago = poItem.Ago;
                    poObject.Sep = poItem.Sep;
                    poObject.Oct = poItem.Oct;
                    poObject.Nov = poItem.Nov;
                    poObject.Dic = poItem.Dic;
                    poObject.Total = poItem.Total;
                    poObject.Observacion = poItem.Observacion;
                }

                loBaseDa.SaveChanges();

            }

            return psMsg;
        }
        #endregion

        #region Kilos Litros Producto
        /// <summary>
        /// Consulta de parametriaciones de presupuesto Kilos litros por Producto
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public List<PresupuestoKilosLitrosProductoGrid> goConsultarParametrizacionesProducto(int tiPeriodo = 0)
        {

            return loBaseDa.Find<VTAPPRESUPUESTOKILOSLITROSPRODUCTO>().Where(X => X.CodigoEstado == Diccionario.Activo && (tiPeriodo != 0 ? X.Periodo == tiPeriodo : true))
                .Select(x => new PresupuestoKilosLitrosProductoGrid()
                {
                    IdPresupuestoKilosLitrosProducto = x.IdPresupuestoKilosLitrosProducto,
                    Periodo = x.Periodo,
                    CodeZona = x.CodeZona,
                    NameZona = x.NameZona,
                    ItemCode = x.ItemCode,
                    ItemName = x.ItemName,
                    Ene = x.Ene,
                    Feb = x.Feb,
                    Mar = x.Mar,
                    Abr = x.Abr,
                    May = x.May,
                    Jun = x.Jun,
                    Jul = x.Jul,
                    Ago = x.Ago,
                    Sep = x.Sep,
                    Oct = x.Oct,
                    Nov = x.Nov,
                    Dic = x.Dic,
                    Observacion = x.Observacion,
                }).ToList();


        }

        /// <summary>
        /// Guardar pregupuesto de Kilos litros - Producto
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsGuardarProducto(List<PresupuestoKilosLitrosProductoGrid> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if (toLista != null && toLista.Count > 0)
            {
                int piPeriodo = toLista.Select(x => x.Periodo).FirstOrDefault();

                var poLista = loBaseDa.Get<VTAPPRESUPUESTOKILOSLITROSPRODUCTO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Periodo == piPeriodo).ToList();

                var piListaIdPresentacion = toLista.Where(x => x.IdPresupuestoKilosLitrosProducto != 0).Select(x => x.IdPresupuestoKilosLitrosProducto).ToList();

                foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdPresupuestoKilosLitrosProducto)))
                {
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdPresupuestoKilosLitrosProducto == poItem.IdPresupuestoKilosLitrosProducto && x.IdPresupuestoKilosLitrosProducto != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new VTAPPRESUPUESTOKILOSLITROSPRODUCTO();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.Periodo = poItem.Periodo;
                    poObject.CodeZona = poItem.CodeZona;
                    poObject.ItemCode = poItem.ItemCode;
                    poObject.ItemName = poItem.ItemName;
                    poObject.NameZona = poItem.NameZona;
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Ene = poItem.Ene;
                    poObject.Feb = poItem.Feb;
                    poObject.Mar = poItem.Mar;
                    poObject.Abr = poItem.Abr;
                    poObject.May = poItem.May;
                    poObject.Jun = poItem.Jun;
                    poObject.Jul = poItem.Jul;
                    poObject.Ago = poItem.Ago;
                    poObject.Sep = poItem.Sep;
                    poObject.Oct = poItem.Oct;
                    poObject.Nov = poItem.Nov;
                    poObject.Dic = poItem.Dic;
                    poObject.Total = poItem.Total;
                    poObject.Observacion = poItem.Observacion;
                }

                loBaseDa.SaveChanges();

            }

            return psMsg;
        }
        #endregion

        #region Kilos Litros Producto
        /// <summary>
        /// Consulta de parametriaciones de presupuesto Cantidades por Producto
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public List<PresupuestoCantidadesProductoGrid> goConsultarParametrizacionesCantidadesProducto(int tiPeriodo = 0)
        {

            return loBaseDa.Find<VTAPPRESUPUESTOCANTIDADESPRODUCTO>().Where(X => X.CodigoEstado == Diccionario.Activo && (tiPeriodo != 0 ? X.Periodo == tiPeriodo : true))
                .Select(x => new PresupuestoCantidadesProductoGrid()
                {
                    IdPresupuestoCantidadesProducto = x.IdPresupuestoCantidadesProducto,
                    Periodo = x.Periodo,
                    CodeZona = x.CodeZona,
                    NameZona = x.NameZona,
                    ItemCode = x.ItemCode,
                    ItemName = x.ItemName,
                    Ene = x.Ene,
                    Feb = x.Feb,
                    Mar = x.Mar,
                    Abr = x.Abr,
                    May = x.May,
                    Jun = x.Jun,
                    Jul = x.Jul,
                    Ago = x.Ago,
                    Sep = x.Sep,
                    Oct = x.Oct,
                    Nov = x.Nov,
                    Dic = x.Dic,
                    Observacion = x.Observacion,
                }).ToList();


        }

        /// <summary>
        /// Guardar pregupuesto Cantidades - Producto
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsGuardarCantidadesProducto(List<PresupuestoCantidadesProductoGrid> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if (toLista != null && toLista.Count > 0)
            {
                int piPeriodo = toLista.Select(x => x.Periodo).FirstOrDefault();

                var poLista = loBaseDa.Get<VTAPPRESUPUESTOCANTIDADESPRODUCTO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Periodo == piPeriodo).ToList();

                var piListaIdPresentacion = toLista.Where(x => x.IdPresupuestoCantidadesProducto != 0).Select(x => x.IdPresupuestoCantidadesProducto).ToList();

                foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdPresupuestoCantidadesProducto)))
                {
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdPresupuestoCantidadesProducto == poItem.IdPresupuestoCantidadesProducto && x.IdPresupuestoCantidadesProducto != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new VTAPPRESUPUESTOCANTIDADESPRODUCTO();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.Periodo = poItem.Periodo;
                    poObject.CodeZona = poItem.CodeZona;
                    poObject.ItemCode = poItem.ItemCode;
                    poObject.ItemName = poItem.ItemName;
                    poObject.NameZona = poItem.NameZona;
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Ene = poItem.Ene;
                    poObject.Feb = poItem.Feb;
                    poObject.Mar = poItem.Mar;
                    poObject.Abr = poItem.Abr;
                    poObject.May = poItem.May;
                    poObject.Jun = poItem.Jun;
                    poObject.Jul = poItem.Jul;
                    poObject.Ago = poItem.Ago;
                    poObject.Sep = poItem.Sep;
                    poObject.Oct = poItem.Oct;
                    poObject.Nov = poItem.Nov;
                    poObject.Dic = poItem.Dic;
                    poObject.Total = poItem.Total;
                    poObject.Observacion = poItem.Observacion;
                }
                loBaseDa.SaveChanges();
            }

            return psMsg;
        }
        #endregion

    }
}
