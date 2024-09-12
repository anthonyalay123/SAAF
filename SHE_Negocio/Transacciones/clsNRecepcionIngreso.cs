using GEN_Entidad;
using GEN_Entidad.Entidades.SHEQ;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SHE_Negocio.Transacciones
{
    public class clsNRecepcionIngreso : clsNBase
    {

        public List<TransferenciaStock> listaMovimientosAprobar(int idBodegaOrigen, int idBodegaDestino)
        {
            loBaseDa.CreateContext();


            var lista = (from a in loBaseDa.Get<SHETTRANSFERENCIASTOCK>()
                         where a.CodigoEstado == "A" 
                             && a.IdBodegaEPPOrigen == idBodegaOrigen
                             && a.IdBodegaEPPDestino == idBodegaDestino
                             && (a.Aprobado == null || a.Aprobado == false)
                         select new TransferenciaStock
                         {
                             IdTransferenciaStock = a.IdTransferenciaStock,
                             FechaTransferencia = a.FechaTrasferencia,
                             Observaciones = a.Observaciones,
                             IdBodegaEPPDestino = a.IdBodegaEPPDestino,
                             IdBodegaEPPOrigen = a.IdBodegaEPPOrigen
                         }).ToList().OrderBy(x => x.IdTransferenciaStock).ToList();

            return lista;
        }


        public string gsAprobarMovimiento(int IdMovimiento, string tsUsuario, string tsTerminal, bool LimpiarContexto = true)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poObject = loBaseDa.Get<SHETTRANSFERENCIASTOCK>().Include(x => x.SHETTRANSFERENCIASTOCKDETALLE).Where(x => x.IdTransferenciaStock == IdMovimiento && x.IdTransferenciaStock != 0).FirstOrDefault();

   
            if (poObject != null)
            {
                poObject.UsuarioModificacion = tsUsuario;
                poObject.FechaModificacion = DateTime.Now;
                poObject.TerminalModificacion = tsTerminal;
                poObject.Aprobado = true;
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }

        public string gsEliminarMovimientoStock(int tId, string tsUsuario, string tsTerminal, bool LimpiarContexto = true)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poObjectStock = loBaseDa.Get<SHETTRANSFERENCIASTOCK>().Include(x => x.SHETTRANSFERENCIASTOCKDETALLE).Where(x => x.IdTransferenciaStock == tId && x.IdTransferenciaStock != 0).FirstOrDefault();


            if (poObjectStock != null)
            {
                poObjectStock.UsuarioModificacion = tsUsuario;
                poObjectStock.FechaModificacion = DateTime.Now;
                poObjectStock.TerminalModificacion = tsTerminal;
                poObjectStock.CodigoEstado = Diccionario.Eliminado;
                poObjectStock.Aprobado = false;
                foreach (var poItem in poObjectStock.SHETTRANSFERENCIASTOCKDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();
                    var idEgreso = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>().Where(c => c.IdTransferenciaStock == poObjectStock.IdTransferenciaStock && c.CodigoEstado == "A" && c.Tipo == "E")
                        .Select(c => c.IdMovimientoInventario).FirstOrDefault();
                    var poLogicaNegocioMovimiento = new clsNMovimientoInventario();

                    psMsg = poLogicaNegocioMovimiento.gsAnularMovimientoStock(idEgreso, tsUsuario, tsTerminal);

                    var idIngreso = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>().Where(c => c.IdTransferenciaStock == poObjectStock.IdTransferenciaStock && c.CodigoEstado == "A" && c.Tipo == "I")
                        .Select(c => c.IdMovimientoInventario).FirstOrDefault();
                    psMsg = poLogicaNegocioMovimiento.gsAnularMovimientoStock(idIngreso, tsUsuario, tsTerminal);

                    poTran.Complete();
                }
            }

          
            return psMsg;
        }


    }
}
