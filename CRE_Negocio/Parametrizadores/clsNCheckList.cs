using GEN_Entidad;
using GEN_Entidad.Entidades.Credito;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRE_Negocio.Parametrizadores
{
    public class clsNCheckList : clsNBase
    {
        #region Parámetros de CheckList
        public List<CheckListParametro> goConsultar()
        {
            loBaseDa.CreateContext();

            return loBaseDa.Find<CREPCHECKLIST>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new CheckListParametro()
            {
                Id = x.Id,
                CheckList = x.CheckList,
                CodigoContado = x.CodigoContado,
                CodigoEstatusSeguro = x.CodigoEstatusSeguro,
                CodigoTipoPersona = x.CodigoTipoPersona,
                CodigoTipoSolicitud = x.CodigoTipoProceso,
                IdCheckList = x.IdCheckList,
                Necesario = x.Necesario,
            }).ToList();

        }

        public string gsGuardar(List<CheckListParametro> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<CREPCHECKLIST>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.Id != 0).Select(x => x.Id).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.Id)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;
            }

            if (toLista != null && toLista.Count > 0)
            {
                var poListaCheckList = goConsultarComboCheckList();

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.Id == poItem.Id && x.Id != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new CREPCHECKLIST();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.CodigoContado = poItem.CodigoContado;
                    poObject.CodigoEstatusSeguro = poItem.CodigoEstatusSeguro;
                    poObject.CodigoTipoPersona = poItem.CodigoTipoPersona;
                    poObject.CodigoTipoProceso = poItem.CodigoTipoSolicitud;
                    poObject.IdCheckList = poItem.IdCheckList;
                    poObject.CheckList = poListaCheckList.Where(x => x.Codigo == poItem.IdCheckList.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.Necesario = poItem.Necesario;
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion
    }
}
