using GEN_Entidad;
using GEN_Entidad.Entidades.ActivoFijo;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AFI_Negocio
{

    public class clsNAgrupacion : clsNBase
    {
        /// <summary>
        /// Listar Entidades
        /// </summary>
        /// <returns></returns>
        public List<Agrupacion> goListar()
        {
            //var poProveedores = loBaseDa.Find<COMMPROVEEDORES>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new { x.IdProveedor, x.Nombre }).ToList();
            return loBaseDa.Find<AFIPAGRUPACION>()
                .Where(x => x.CodigoEstado == Diccionario.Activo).ToList()
                .Select(x => new Agrupacion
                {
                    CuentaVentaInferiorDebito = x.CuentaVentaInferiorDebito,
                    CuentaBajaCredito = x.CuentaBajaCredito,
                    Descripcion = x.Descripcion,
                    CuentaBajaDebito = x.CuentaBajaDebito,
                    CuentaDepreciacionCredito = x.CuentaDepreciacionCredito,
                    CuentaDepreciacionDebito = x.CuentaDepreciacionDebito,
                    CuentaVentaInferiorCredito = x.CuentaVentaInferiorCredito,
                    CuentaVentaSuperiorCredito = x.CuentaVentaSuperiorCredito,
                    CuentaVentaSuperiorDebito = x.CuentaVentaSuperiorDebito,
                    CodigoAgrupacion = x.CodigoAgrupacion
                }).ToList();
        }

        public string gsGuardar(List<Agrupacion> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";
            if (string.IsNullOrEmpty(psMsg))
            {
                var poLista = loBaseDa.Get<AFIPAGRUPACION>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

                List<string> poListaId = toLista.Select(x => x.CodigoAgrupacion).ToList();
                List<string> piListaEliminar = poLista.Where(x => !poListaId.Contains(x.CodigoAgrupacion)).Select(x => x.CodigoAgrupacion).ToList();

                foreach (var poItem in poLista.Where(x => piListaEliminar.Contains(x.CodigoAgrupacion)))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                if (toLista != null)
                {
                    foreach (var item in toLista)
                    {
                        var poObjectItem = poLista.Where(x => x.CodigoAgrupacion == item.CodigoAgrupacion).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new AFIPAGRUPACION();
                            loBaseDa.CreateNewObject(out poObjectItem);
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.CodigoAgrupacion = item.CodigoAgrupacion;
                        poObjectItem.CuentaBajaCredito = item.CuentaBajaCredito;
                        poObjectItem.CuentaBajaDebito = item.CuentaBajaDebito;
                        poObjectItem.CuentaDepreciacionCredito = item.CuentaDepreciacionCredito;
                        poObjectItem.CuentaDepreciacionDebito = item.CuentaDepreciacionDebito;
                        poObjectItem.CuentaVentaInferiorCredito = item.CuentaVentaInferiorCredito;
                        poObjectItem.CuentaVentaInferiorDebito = item.CuentaVentaInferiorDebito;
                        poObjectItem.CuentaVentaSuperiorCredito = item.CuentaVentaSuperiorCredito;
                        poObjectItem.CuentaVentaSuperiorDebito = item.CuentaVentaSuperiorDebito;
                        poObjectItem.Descripcion = item.Descripcion;
                    }
                }

                loBaseDa.SaveChanges();
            }

            return psMsg;
        }
    }
}