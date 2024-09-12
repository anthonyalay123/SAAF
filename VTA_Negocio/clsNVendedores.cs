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
using System.Data.Entity;

namespace VTA_Negocio
{
    public class clsNVendedores : clsNBase
    {
        #region Vendedor Grupo

        public List<VendedorGrupo> goConsultarVendedorGrupo()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<VendedorGrupo>();
            var poLista = loBaseDa.Find<VTAPVENDEDORGRUPO>().Include(x => x.VTAPVENDEDORGRUPODETALLE).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            foreach (var item in poLista)
            {
                var poCab = new VendedorGrupo();
                poCab.IdPersona = item.IdPersona;
                poCab.IdVendedorGrupo = item.IdVendedorGrupo;
                poCab.Nombre = item.Nombre;
                poCab.IdZonaGrupo = loBaseDa.Find<VTAPZONAGRUPO>().Where(x => x.IdVendedorGrupo == item.IdVendedorGrupo).Select(x => x.IdZonaGrupo).FirstOrDefault();

                poCab.VendedorGrupoDetalle = new List<VendedorGrupoDetalle>();
                foreach (var detalle in item.VTAPVENDEDORGRUPODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new VendedorGrupoDetalle();
                    poDet.IdVendedorGrupo = detalle.IdVendedorGrupo;
                    poDet.IdVendedorGrupoDetalle = detalle.IdVendedorGrupoDetalle;
                    poDet.SlpCode = detalle.SlpCode;
                    poDet.SlpName = detalle.SlpName;
                    poCab.VendedorGrupoDetalle.Add(poDet);
                }
                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public string gsGuardarVendedorGrupo(List<VendedorGrupo> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<VTAPVENDEDORGRUPO>().Include(x => x.VTAPVENDEDORGRUPODETALLE).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdVendedorGrupo != 0).Select(x => x.IdVendedorGrupo).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdVendedorGrupo)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;

                foreach (var item in poItem.VTAPVENDEDORGRUPODETALLE.Where(X => X.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Inactivo;
                    item.UsuarioModificacion = tsUsuario;
                    item.FechaModificacion = DateTime.Now;
                    item.TerminalModificacion = tsTerminal;
                }
            }

            if (toLista != null && toLista.Count > 0)
            {
                var poListaVendedores = goSapConsultVendedoresTodos();

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdVendedorGrupo == poItem.IdVendedorGrupo && x.IdVendedorGrupo != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new VTAPVENDEDORGRUPO();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.IdPersona = poItem.IdPersona == 0 ? null : poItem.IdPersona;
                    poObject.Nombre = poItem.Nombre;

                    var poZona = loBaseDa.Get<VTAPZONAGRUPO>().Where(x => x.IdVendedorGrupo == poItem.IdVendedorGrupo).FirstOrDefault();
                    if (poZona != null)
                    {
                        poZona.Vendedor = poItem.Nombre;
                    }

                    if (poItem.VendedorGrupoDetalle != null)
                    {
                        //Eliminar Detalle 
                        piListaIdPresentacion = poItem.VendedorGrupoDetalle.Where(x => x.IdVendedorGrupoDetalle != 0).Select(x => x.IdVendedorGrupoDetalle).ToList();

                        foreach (var poItemDel in poObject.VTAPVENDEDORGRUPODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdVendedorGrupoDetalle)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in poItem.VendedorGrupoDetalle)
                        {
                            int pId = item.IdVendedorGrupoDetalle;
                            var poObjectItem = poObject.VTAPVENDEDORGRUPODETALLE.Where(x => x.IdVendedorGrupoDetalle == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new VTAPVENDEDORGRUPODETALLE();
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                                poObject.VTAPVENDEDORGRUPODETALLE.Add(poObjectItem);
                            }

                            poObjectItem.SlpCode = item.SlpCode;
                            poObjectItem.SlpName = poListaVendedores.Where(x => x.Codigo == item.SlpCode.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                            poObjectItem.CodigoEstado = Diccionario.Activo;
                        }
                    }
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        
        #endregion
    }
}
