using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTA_Negocio
{
    public class clsNGrupoTipoProducto : clsNBase
    {
        #region Grupo Tipo Producto
        public List<GrupoTipoProducto> goConsultarGrupoTipoProducto()
        {
            loBaseDa.CreateContext();

            return loBaseDa.Find<VTAPGRUPOITEMTIPOPRODUCTOSAAF>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new GrupoTipoProducto()
            {
                IdGrupoItemTipoProductoSaaf = x.IdGrupoItemTipoProductoSaaf,
                IdZona = x.IdZona,
                ItmsGrpCod = x.ItmsGrpCod,
                CodigoTipoProducto = x.CodigoTipoProducto,
                General = x.CodigoGeneral,
            }).ToList();

        }

        public string gsGuardarTipoProducto(List<GrupoTipoProducto> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<VTAPGRUPOITEMTIPOPRODUCTOSAAF>()
                        .Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista
                        .Where(x => x.IdGrupoItemTipoProductoSaaf != 0)
                        .Select(x => x.IdGrupoItemTipoProductoSaaf).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdGrupoItemTipoProductoSaaf)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;
            }

            if (toLista != null && toLista.Count > 0)
            {
                var poListaTipoProducto = goConsultarComboTipoProducto();
                var poListaZonasSAAF = goConsultarZonasSAAF();
                var poListaZonasSAP = goSapConsultaGrupos();

                foreach (var poItem in toLista)
                {
                    var poObject = poLista
                        .FirstOrDefault(x => x.IdGrupoItemTipoProductoSaaf == poItem.IdGrupoItemTipoProductoSaaf && x.IdGrupoItemTipoProductoSaaf != 0);

                    if (poObject != null)
                    {
                        if (poObject.IdZona != poItem.IdZona || poObject.ItmsGrpCod != poItem.ItmsGrpCod ||poObject.CodigoTipoProducto != poItem.CodigoTipoProducto || poObject.CodigoGeneral != poItem.General)
                        {
                            poObject.IdZona = poItem.IdZona;
                            poObject.ItmsGrpCod = poItem.ItmsGrpCod;
                            poObject.ItmsGrpNam = poListaZonasSAP.Where(x => x.Codigo == poItem.ItmsGrpCod.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                            poObject.CodigoTipoProducto = poItem.CodigoTipoProducto;
                            poObject.CodigoGeneral = poItem.General;
                            poObject.TipoProducto = poListaTipoProducto.Where(x => x.Codigo == poItem.CodigoTipoProducto).Select(x => x.Descripcion).FirstOrDefault();

                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;
                        }
                    }
                    else
                    {
                        poObject = new VTAPGRUPOITEMTIPOPRODUCTOSAAF();
                        loBaseDa.CreateNewObject(out poObject);

                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;

                        poObject.CodigoEstado = Diccionario.Activo;
                        poObject.IdZona = poItem.IdZona;
                        poObject.ItmsGrpCod = poItem.ItmsGrpCod;
                        poObject.ItmsGrpNam = poListaZonasSAP.Where(x => x.Codigo == poItem.ItmsGrpCod.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                        poObject.CodigoTipoProducto = poItem.CodigoTipoProducto;
                        poObject.CodigoGeneral = poItem.General;
                        poObject.TipoProducto = poListaTipoProducto.Where(x => x.Codigo == poItem.CodigoTipoProducto).Select(x => x.Descripcion).FirstOrDefault();
                    }
                }
            }

            loBaseDa.SaveChanges();
            return psMsg;
        }


        //public string gsGuardarTipoProducto(List<GrupoTipoProducto> toLista, string tsUsuario, string tsTerminal)
        //{
        //    string psMsg = string.Empty;
        //    loBaseDa.CreateContext();

        //    var poLista = loBaseDa.Get<VTAPGRUPOITEMTIPOPRODUCTOSAAF>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

        //    var piListaIdPresentacion = toLista.Where(x => x.IdGrupoItemTipoProductoSaaf != 0).Select(x => x.IdGrupoItemTipoProductoSaaf).ToList();

        //    foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdGrupoItemTipoProductoSaaf)))
        //    {
        //        poItem.CodigoEstado = Diccionario.Inactivo;
        //        poItem.UsuarioModificacion = tsUsuario;
        //        poItem.FechaModificacion = DateTime.Now;
        //        poItem.TerminalModificacion = tsTerminal;
        //    }

        //    if (toLista != null && toLista.Count > 0)
        //    {
        //        var poListaTipoProducto = goConsultarComboTipoProducto();
        //        var poListaZonasSAAF = goConsultarZonasSAAF();
        //        var poListaZonasSAP = goSapConsultaGrupos();

        //        foreach (var poItem in toLista)
        //        {
        //            var poObject = poLista.Where(x => x.IdGrupoItemTipoProductoSaaf == poItem.IdGrupoItemTipoProductoSaaf && x.IdGrupoItemTipoProductoSaaf != 0).FirstOrDefault();
        //            if (poObject != null)
        //            {
        //                poObject.UsuarioModificacion = tsUsuario;
        //                poObject.FechaModificacion = DateTime.Now;
        //                poObject.TerminalModificacion = tsTerminal;
        //            }
        //            else
        //            {
        //                poObject = new VTAPGRUPOITEMTIPOPRODUCTOSAAF();
        //                loBaseDa.CreateNewObject(out poObject);
        //                poObject.UsuarioIngreso = tsUsuario;
        //                poObject.FechaIngreso = DateTime.Now;
        //                poObject.TerminalIngreso = tsTerminal;
        //            }

        //            poObject.CodigoEstado = Diccionario.Activo;
        //            poObject.IdZona = poItem.IdZona;
        //            poObject.ItmsGrpCod = poItem.ItmsGrpCod;
        //            poObject.ItmsGrpNam = poListaZonasSAP.Where(x => x.Codigo == poItem.ItmsGrpCod.ToString()).Select(x => x.Descripcion).FirstOrDefault();
        //            poObject.CodigoTipoProducto = poItem.CodigoTipoProducto;
        //            poObject.CodigoGeneral = poItem.General;
        //            poObject.TipoProducto = poListaTipoProducto.Where(x => x.Codigo == poItem.CodigoTipoProducto).Select(x => x.Descripcion).FirstOrDefault();
        //        }
        //    }

        //    loBaseDa.SaveChanges();

        //    return psMsg;
        //}
        #endregion
    }
}
