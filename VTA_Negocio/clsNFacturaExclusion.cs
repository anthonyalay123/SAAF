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
    public class clsNFacturaExclusion : clsNBase
    {
        #region Excepcion Factura Comision
        public List<FacturaExclusion> goConsultarFacturaExclusion()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<FacturaExclusion>();
            var poLista = loBaseDa.Find<VTAPFACTURAEXCLUSION>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            foreach (var item in poLista)
            {
                var poCab = new FacturaExclusion();
                poCab.IdFacturaExclusion = item.IdFacturaExclusion;
                poCab.NumFactura = item.NumFactura;
                poCab.CardCode = item.CardCode;
                poCab.CardName = item.CardName;
                poCab.TipoDocumento = item.TipoDocumento;
                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public string gsGuardarFacturaExclusion(List<FacturaExclusion> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<VTAPFACTURAEXCLUSION>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdFacturaExclusion != 0).Select(x => x.IdFacturaExclusion).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdFacturaExclusion)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;

            }

            if (toLista != null && toLista.Count > 0)
            {
                var poListaClientes = goSapConsultaClientes();

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdFacturaExclusion == poItem.IdFacturaExclusion && x.IdFacturaExclusion != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new VTAPFACTURAEXCLUSION();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.NumFactura = poItem.NumFactura;
                    poObject.CardCode = poItem.CardCode;
                    poObject.TipoDocumento = poItem.TipoDocumento;
                    poObject.CardName = poListaClientes.Where(x => x.Codigo == poItem.CardCode).Select(x => x.Descripcion).FirstOrDefault();
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion
    }
}
