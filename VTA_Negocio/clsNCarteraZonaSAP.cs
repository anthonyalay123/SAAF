using GEN_Entidad.Entidades.Ventas;
using GEN_Entidad;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTA_Negocio
{
    public class clsNCarteraZonaSAP : clsNBase
    {
        public List<CarteraZonaSAP> goConsultarZonaSAPGrupo()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<CarteraZonaSAP>();
            var poLista = loBaseDa.Find<VTAPZONAGRUPODETALLE>().Include(x => x.VTAPCARTERAZONASAPENVIOCORREO).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            foreach (var item in poLista)
            {
                var poCab = new CarteraZonaSAP();
                poCab.IdZonaGrupoDetalle = item.IdZonaGrupoDetalle;
                poCab.Codigo = item.Code;
                poCab.Nombre = item.Name;

                poCab.CarteraZonaSAPEnvioCorreo = new List<CarteraZonaSAPEnvioCorreo>();
                foreach (var detalle in item.VTAPCARTERAZONASAPENVIOCORREO.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new CarteraZonaSAPEnvioCorreo();
                    poDet.IdCarteraZonaSAPEnvioCorreo = detalle.IdCarteraZonaSAPEnvioCorreo;
                    poDet.IdZonaSAPGrupo = detalle.IdZonaGrupoDetalle;
                    poDet.Codigo = detalle.Code;
                    poDet.TipoDestinatario = detalle.TipoDestinatario;
                    poDet.IdPersona = detalle.IdPersona;
                    poDet.CorreoManual = detalle.CorreoManual;

                    poCab.CarteraZonaSAPEnvioCorreo.Add(poDet);
                }
                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public string gsGuardarCarteraZonaSAP(List<CarteraZonaSAP> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<VTAPZONAGRUPODETALLE>()
                                  .Include(x => x.VTAPCARTERAZONASAPENVIOCORREO)
                                  .Where(x => x.CodigoEstado == Diccionario.Activo)
                                  .ToList();

            if (toLista != null && toLista.Count > 0)
            {
                foreach (var poItem in toLista)
                {
                    var poObject = poLista.FirstOrDefault(x => x.IdZonaGrupoDetalle == poItem.IdZonaGrupoDetalle);

                    if (poObject != null && poItem.CarteraZonaSAPEnvioCorreo != null)
                    {

                        var vistaIds = poItem.CarteraZonaSAPEnvioCorreo.Select(x => x.IdCarteraZonaSAPEnvioCorreo).ToList();
                        var dbIds = poObject.VTAPCARTERAZONASAPENVIOCORREO.Where(x => x.CodigoEstado == Diccionario.Activo)
                                                                         .Select(x => x.IdCarteraZonaSAPEnvioCorreo)
                                                                         .ToList();

                        foreach (var poItemDel in poObject.VTAPCARTERAZONASAPENVIOCORREO
                                                         .Where(x => x.CodigoEstado == Diccionario.Activo && !vistaIds.Contains(x.IdCarteraZonaSAPEnvioCorreo)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }


                        foreach (var item in poItem.CarteraZonaSAPEnvioCorreo)
                        {
                            var poObjectItem = poObject.VTAPCARTERAZONASAPENVIOCORREO.FirstOrDefault(x => x.IdCarteraZonaSAPEnvioCorreo == item.IdCarteraZonaSAPEnvioCorreo);

                            if (poObjectItem != null) 
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else 
                            {
                                poObjectItem = new VTAPCARTERAZONASAPENVIOCORREO
                                {
                                    UsuarioIngreso = tsUsuario,
                                    FechaIngreso = DateTime.Now,
                                    TerminalIngreso = tsTerminal
                                };
                                poObject.VTAPCARTERAZONASAPENVIOCORREO.Add(poObjectItem);
                            }

                            poObjectItem.Code = poItem.Codigo;
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

    }
}
