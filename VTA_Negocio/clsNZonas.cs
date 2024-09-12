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
    public class clsNZonas : clsNBase
    {
        #region Zona Grupo
        public List<ZonaGrupo> goConsultarZonaGrupo()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<ZonaGrupo>();
            var poLista = loBaseDa.Find<VTAPZONAGRUPO>().Include(x => x.VTAPZONAGRUPODETALLE).Include(x=>x.VTAPZONAGRUPOENVIOCORREO).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            foreach (var item in poLista)
            {
                var poCab = new ZonaGrupo();
                poCab.IdZonaGrupo = item.IdZonaGrupo;
                poCab.IdVendedorGrupo = item.IdVendedorGrupo;
                poCab.Nombre = item.Nombre;
                poCab.Vendedor = item.Vendedor;

                poCab.ZonaGrupoDetalle = new List<ZonaGrupoDetalle>();
                foreach (var detalle in item.VTAPZONAGRUPODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new ZonaGrupoDetalle();
                    poDet.IdZonaGrupo = detalle.IdZonaGrupo;
                    poDet.IdZonaGrupoDetalle = detalle.IdZonaGrupoDetalle;
                    poDet.Code = detalle.Code;
                    poDet.Name = detalle.Name;
                    poDet.CodAgrupacion = detalle.CodAgrupacion;
                    poDet.Agrupacion = detalle.Agrupacion;
                    poDet.CodResponsableCobranza = detalle.CodResponsableCobranza;
                    poDet.ResponsableCobranza = detalle.ResponsableCobranza;
                    poDet.DiasProAcepFin = detalle.DiasProAcepFin??0;
                    poDet.DiasProAcepIni = detalle.DiasProAcepIni??0;
                    poDet.DiasProGestFin = detalle.DiasProGestFin??0;
                    poDet.DiasProGestIni = detalle.DiasProGestIni??0;
                    poDet.DiasProNoAcepFin = detalle.DiasProNoAcepFin??0;
                    poDet.DiasProNoAcepIni = detalle.DiasProNoAcepIni??0;
                    poDet.CodVendedor = detalle.CodVendedor;
                    poDet.NomVendedor = detalle.NomVendedor;
                    poDet.CodTitular = detalle.CodTitular;
                    poDet.Titular = detalle.Titular;
                    poDet.CodRecaudador = detalle.CodRecaudador;
                    poDet.Recaudador = detalle.Recaudador;

                    poCab.ZonaGrupoDetalle.Add(poDet);
                }

                poCab.ZonaGrupoEnvioCorreo = new List<ZonaGrupoEnvioCorreo>();
                foreach (var detalle in item.VTAPZONAGRUPOENVIOCORREO.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new ZonaGrupoEnvioCorreo();
                    poDet.IdZonaGrupo = detalle.IdZonaGrupo;
                    poDet.IdZonaGrupoEnvioCorreo = detalle.IdZonaGrupoEnvioCorreo;
                    poDet.TipoDestinatario = detalle.TipoDestinatario;
                    poDet.IdPersona = detalle.IdPersona;
                    poDet.CorreoManual = detalle.CorreoManual;
                    
                    poCab.ZonaGrupoEnvioCorreo.Add(poDet);
                }
                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public string gsGuardarZonaGrupo(List<ZonaGrupo> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<VTAPZONAGRUPO>().Include(x => x.VTAPZONAGRUPODETALLE).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdZonaGrupo != 0).Select(x => x.IdZonaGrupo).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdZonaGrupo)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;

                foreach (var item in poItem.VTAPZONAGRUPODETALLE.Where(X => X.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Inactivo;
                    item.UsuarioModificacion = tsUsuario;
                    item.FechaModificacion = DateTime.Now;
                    item.TerminalModificacion = tsTerminal;
                }
            }

            if (toLista != null && toLista.Count > 0)
            {
                var poZonasSap = goConsultarZonasSAP();
                var poAgrupacion = goConsultarComboAgrupacionCobranza();
                var poResponsable = goConsultarComboResponsableCobranza();
                var poVendedor = goSapConsultVendedoresTodos();
                var poVendedorGrupo = goConsultarComboVendedorGrupoTodos();
                var poTitulares = goSapConsultTitularesTodos();
                var poPersonas = goConsultarComboIdPersona();

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdZonaGrupo == poItem.IdZonaGrupo && x.IdZonaGrupo != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new VTAPZONAGRUPO();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.IdVendedorGrupo = poItem.IdVendedorGrupo;
                    poObject.Nombre = poItem.Nombre;
                    poObject.IdVendedorGrupo = poItem.IdVendedorGrupo;
                    poObject.Vendedor = poVendedorGrupo.Where(x => x.Codigo == poItem.IdVendedorGrupo.ToString()).Select(x => x.Descripcion).FirstOrDefault();

                    if (poItem.ZonaGrupoDetalle != null)
                    {
                        //Eliminar Detalle 
                        piListaIdPresentacion = poItem.ZonaGrupoDetalle.Where(x => x.IdZonaGrupoDetalle != 0).Select(x => x.IdZonaGrupoDetalle).ToList();

                        foreach (var poItemDel in poObject.VTAPZONAGRUPODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdZonaGrupoDetalle)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in poItem.ZonaGrupoDetalle)
                        {
                            int pId = item.IdZonaGrupoDetalle;
                            var poObjectItem = poObject.VTAPZONAGRUPODETALLE.Where(x => x.IdZonaGrupoDetalle == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new VTAPZONAGRUPODETALLE();
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                                poObject.VTAPZONAGRUPODETALLE.Add(poObjectItem);
                            }

                            poObjectItem.CodigoEstado = Diccionario.Activo;
                            poObjectItem.Code = item.Code;
                            poObjectItem.Name = poZonasSap.Where(x => x.Codigo == item.Code).Select(x => x.Descripcion).FirstOrDefault();
                            poObjectItem.CodAgrupacion = item.CodAgrupacion;
                            poObjectItem.Agrupacion = poAgrupacion.Where(x => x.Codigo == item.CodAgrupacion).Select(x => x.Descripcion).FirstOrDefault();
                            poObjectItem.CodResponsableCobranza = item.CodResponsableCobranza;
                            poObjectItem.ResponsableCobranza = poResponsable.Where(x => x.Codigo == item.CodResponsableCobranza).Select(x => x.Descripcion).FirstOrDefault();
                            poObjectItem.DiasProAcepFin = item.DiasProAcepFin;
                            poObjectItem.DiasProAcepIni = item.DiasProAcepIni;
                            poObjectItem.DiasProGestFin = item.DiasProGestFin;
                            poObjectItem.DiasProGestIni = item.DiasProGestIni;
                            poObjectItem.DiasProNoAcepFin = item.DiasProNoAcepFin;
                            poObjectItem.DiasProNoAcepIni = item.DiasProNoAcepIni;
                            poObjectItem.CodVendedor = item.CodVendedor;
                            poObjectItem.NomVendedor = item.CodVendedor == null ? null : poVendedor.Where(x => x.Codigo == item.CodVendedor.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                            poObjectItem.CodTitular = item.CodTitular;
                            poObjectItem.Titular = item.CodTitular == null ? null : poTitulares.Where(x => x.Codigo == item.CodTitular.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                            poObjectItem.CodRecaudador = item.CodRecaudador;
                            poObjectItem.Recaudador = item.CodRecaudador == null ? null : poVendedor.Where(x => x.Codigo == item.CodRecaudador.ToString()).Select(x => x.Descripcion).FirstOrDefault();

                        }
                    }

                    if (poItem.ZonaGrupoEnvioCorreo != null)
                    {
                        //Eliminar Detalle 
                        piListaIdPresentacion = poItem.ZonaGrupoEnvioCorreo.Where(x => x.IdZonaGrupoEnvioCorreo != 0).Select(x => x.IdZonaGrupoEnvioCorreo).ToList();

                        foreach (var poItemDel in poObject.VTAPZONAGRUPOENVIOCORREO.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdZonaGrupoEnvioCorreo)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in poItem.ZonaGrupoEnvioCorreo)
                        {
                            int pId = item.IdZonaGrupoEnvioCorreo;
                            var poObjectItem = poObject.VTAPZONAGRUPOENVIOCORREO.Where(x => x.IdZonaGrupoEnvioCorreo == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new VTAPZONAGRUPOENVIOCORREO();
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                                poObject.VTAPZONAGRUPOENVIOCORREO.Add(poObjectItem);
                            }

                            poObjectItem.CodigoEstado = Diccionario.Activo;
                            poObjectItem.IdPersona = item.IdPersona;
                            poObjectItem.CorreoManual = item.CorreoManual;
                            poObjectItem.TipoDestinatario = item.TipoDestinatario;
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
