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
    /// <summary>
    /// Lógica de Negocio de Presupuesto de Ventas
    /// </summary>
    public class clsNPresupuestoVentas : clsNBase
    {
        #region Presupuesto Ventas
        
        #region Presupuesto de Ventas - Familia - Grupo - Producto
        /// <summary>
        /// Consulta de parametriaciones de presupuesto de Ventas
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public List<PresupuestoVentasGrid> goConsultarParametrizaciones(int tiPeriodo = 0)
        {
            return loBaseDa.Find<VTAPPRESUPUESTOVENTAS>().Where(X => X.CodigoEstado == Diccionario.Activo && (tiPeriodo != 0 ? X.Periodo == tiPeriodo : true))
                .Select(x => new PresupuestoVentasGrid()
                {
                    IdPresupuestoVentas = x.IdPresupuestoVentas,
                    Periodo = x.Periodo,
                    //CodigoEstado = x.CodigoEstado,
                    Familia = x.Familia,
                    IdZona = x.IdZona,
                    ItmsGrpCod = x.ItmsGrpCod,
                    Grupo = x.ItmsGrpNam,
                    MedidaConversion = x.MedidaConversion,
                    Mes = x.Mes,
                    PrecioReferencial = x.PrecioReferencial,
                    TipoProducto = x.TipoProducto,
                    //Total = x.Total,
                    Unidades = x.Unidades,
                    //Valor = x.Valor,
                    Zona = x.Zona,
                    ItemCode = x.ItemCode,
                    ItemName = x.ItemName,
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
        public string gsGuardarProducto(List<PresupuestoVentasGrid> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if (toLista != null && toLista.Count > 0)
            {
                int piPeriodo = toLista.Select(x => x.Periodo).FirstOrDefault();

                var poLista = loBaseDa.Get<VTAPPRESUPUESTOVENTAS>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Periodo == piPeriodo).ToList();

                var piListaIdPresentacion = toLista.Where(x => x.IdPresupuestoVentas != 0).Select(x => x.IdPresupuestoVentas).ToList();

                foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdPresupuestoVentas)))
                {
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdPresupuestoVentas == poItem.IdPresupuestoVentas && x.IdPresupuestoVentas != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new VTAPPRESUPUESTOVENTAS();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Periodo = poItem.Periodo;
                    poObject.Familia = poItem.Familia;
                    poObject.ItemCode = poItem.ItemCode;
                    poObject.ItemName = poItem.ItemName;
                    poObject.IdZona = poItem.IdZona;
                    poObject.Zona = poItem.Zona;
                    poObject.ItmsGrpCod = poItem.ItmsGrpCod;
                    poObject.ItmsGrpNam = poItem.Grupo;
                    poObject.MedidaConversion = poItem.MedidaConversion;
                    poObject.Mes = poItem.Mes;
                    poObject.Observacion = poItem.Observacion;
                    poObject.TipoProducto = poItem.TipoProducto;
                    poObject.Unidades = poItem.Unidades;
                    poObject.Valor = poItem.Valor;
                    poObject.Total = poItem.Total;
                    poObject.PrecioReferencial = poItem.PrecioReferencial;

                }

                loBaseDa.SaveChanges();

            }

            return psMsg;
        }
        #endregion

        #region Presupuesto de Ventas Interno - Familia - Grupo - Producto
        /// <summary>
        /// Consulta de parametriaciones de presupuesto de Ventas
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public List<PresupuestoVentasGrid> goConsultarParametrizacionesInterno(int tiPeriodo = 0)
        {
            return loBaseDa.Find<VTAPPRESUPUESTOVENTASINTERNO>().Where(X => X.CodigoEstado == Diccionario.Activo && (tiPeriodo != 0 ? X.Periodo == tiPeriodo : true))
                .Select(x => new PresupuestoVentasGrid()
                {
                    IdPresupuestoVentas = x.IdPresupuestoVentasInterno,
                    Periodo = x.Periodo,
                    //CodigoEstado = x.CodigoEstado,
                    Familia = x.Familia,
                    IdZona = x.IdZona,
                    ItmsGrpCod = x.ItmsGrpCod,
                    Grupo = x.ItmsGrpNam,
                    MedidaConversion = x.MedidaConversion,
                    Mes = x.Mes,
                    PrecioReferencial = x.PrecioReferencial,
                    TipoProducto = x.TipoProducto,
                    //Total = x.Total,
                    Unidades = x.Unidades,
                    //Valor = x.Valor,
                    Zona = x.Zona,
                    ItemCode = x.ItemCode,
                    ItemName = x.ItemName,
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
        public string gsGuardarProductoInterno(List<PresupuestoVentasGrid> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if (toLista != null && toLista.Count > 0)
            {
                int piPeriodo = toLista.Select(x => x.Periodo).FirstOrDefault();

                var poLista = loBaseDa.Get<VTAPPRESUPUESTOVENTASINTERNO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Periodo == piPeriodo).ToList();

                var piListaIdPresentacion = toLista.Where(x => x.IdPresupuestoVentas != 0).Select(x => x.IdPresupuestoVentas).ToList();

                foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdPresupuestoVentasInterno)))
                {
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdPresupuestoVentasInterno == poItem.IdPresupuestoVentas && x.IdPresupuestoVentasInterno != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new VTAPPRESUPUESTOVENTASINTERNO();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Periodo = poItem.Periodo;
                    poObject.Familia = poItem.Familia;
                    poObject.ItemCode = poItem.ItemCode;
                    poObject.ItemName = poItem.ItemName;
                    poObject.IdZona = poItem.IdZona;
                    poObject.Zona = poItem.Zona;
                    poObject.ItmsGrpCod = poItem.ItmsGrpCod;
                    poObject.ItmsGrpNam = poItem.Grupo;
                    poObject.MedidaConversion = poItem.MedidaConversion;
                    poObject.Mes = poItem.Mes;
                    poObject.Observacion = poItem.Observacion;
                    poObject.TipoProducto = poItem.TipoProducto;
                    poObject.Unidades = poItem.Unidades;
                    poObject.Valor = poItem.Valor;
                    poObject.Total = poItem.Total;
                    poObject.PrecioReferencial = poItem.PrecioReferencial;

                }

                loBaseDa.SaveChanges();

            }

            return psMsg;
        }
        #endregion

        #region Presupuesto de Kilos Litros
        /// <summary>
        /// Consulta de parametriaciones de presupuesto Kilos Litros
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public List<PresupuestoKilosLitrosGrid> goConsultarParametrizacionesKilosLitros(int tiPeriodo = 0)
        {
            return loBaseDa.Find<VTAPPRESUPUESTOKILOSLITROS>().Where(X => X.CodigoEstado == Diccionario.Activo && (tiPeriodo != 0 ? X.Periodo == tiPeriodo : true))
                .Select(x => new PresupuestoKilosLitrosGrid()
                {
                    IdPresupuestoKilosLitros = x.IdPresupuestoKilosLitros,
                    Periodo = x.Periodo,
                    Familia = x.Familia,
                    IdZona = x.IdZona,
                    ItmsGrpCod = x.ItmsGrpCod,
                    Grupo = x.ItmsGrpNam,
                    MedidaConversion = x.MedidaConversion,
                    Mes = x.Mes,
                    Unidades = x.Unidades,
                    Zona = x.Zona,
                    ItemCode = x.ItemCode,
                    ItemName = x.ItemName,
                    Observacion = x.Observacion,
                }).ToList();
        }

        /// <summary>
        /// Guardar pregupuesto de Kilos litros
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsGuardarProductoKilosLitros(List<PresupuestoKilosLitrosGrid> toLista,int tiPeriodo, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            int piPeriodo = tiPeriodo;

            var poLista = loBaseDa.Get<VTAPPRESUPUESTOKILOSLITROS>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Periodo == piPeriodo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdPresupuestoKilosLitros != 0).Select(x => x.IdPresupuestoKilosLitros).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdPresupuestoKilosLitros)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;
            }

            if (toLista != null && toLista.Count > 0)
            {
             
                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdPresupuestoKilosLitros == poItem.IdPresupuestoKilosLitros && x.IdPresupuestoKilosLitros != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new VTAPPRESUPUESTOKILOSLITROS();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Periodo = poItem.Periodo;
                    poObject.Familia = poItem.Familia;
                    poObject.ItemCode = poItem.ItemCode;
                    poObject.ItemName = poItem.ItemName;
                    poObject.IdZona = poItem.IdZona;
                    poObject.Zona = poItem.Zona;
                    poObject.ItmsGrpCod = poItem.ItmsGrpCod;
                    poObject.ItmsGrpNam = poItem.Grupo;
                    poObject.MedidaConversion = poItem.MedidaConversion;
                    poObject.Mes = poItem.Mes;
                    poObject.Observacion = poItem.Observacion;
                    poObject.Unidades = poItem.Unidades;
                    poObject.Total = poItem.Total;
                    poObject.TipoProducto = "";

                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion


        #endregion


    }
}
