
using GEN_Entidad;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GEN_Negocio;
using GEN_Entidad.Entidades.Compras;

namespace COM_Negocio
{
    public class clsNCostoPorBulto : clsNBase
    {

        #region Cliente Cambio Zona
        public List<CostoPorBulto> goConsultar()
        {
            loBaseDa.CreateContext();

            return loBaseDa.Find<COMPCOSTOPORBULTO>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new CostoPorBulto()
            {
                IdCostoPorBulto = x.IdCostoPorBulto,
                CodigoTipoTransporte = x.CodigoTipoTransporte,
                CardCode = x.CardCode,
                CardName = x.CardName,
                WhsCode = x.WhsCode,
                WhsName = x.WhsName,
                CodeZona = x.CodeZona,
                NameZona = x.NameZona,
                CodigoEstado = x.CodigoEstado,
                Costo = x.CostoPorBulto,
            }).ToList();

        }

        public string gsGuardar(List<CostoPorBulto> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<COMPCOSTOPORBULTO>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdCostoPorBulto != 0).Select(x => x.IdCostoPorBulto).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdCostoPorBulto)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;
            }

            if (toLista != null && toLista.Count > 0)
            {
                var poListaClientes = goSapConsultaClientesTodos();
                var poListaZonasSap = goConsultarZonasSAP();
                var poListaBodega = goSapConsultaComboCatalogoBodegas();

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdCostoPorBulto == poItem.IdCostoPorBulto && x.IdCostoPorBulto != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new COMPCOSTOPORBULTO();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.CodigoTipoTransporte = poItem.CodigoTipoTransporte;
                    poObject.CardCode = poItem.CardCode;
                    poObject.CardName = poListaClientes.Where(x => x.Codigo == poItem.CardCode).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.CodeZona = poItem.CodeZona;
                    poObject.NameZona = poListaZonasSap.Where(x => x.Codigo == poItem.CodeZona).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.WhsCode = poItem.WhsCode;
                    poObject.WhsName = poListaBodega.Where(x => x.Codigo == poItem.WhsCode).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.CostoPorBulto = poItem.Costo;
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion

    }
}
