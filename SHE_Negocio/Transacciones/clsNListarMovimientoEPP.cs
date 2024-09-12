using GEN_Entidad;
using GEN_Entidad.Entidades.SHEQ;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHE_Negocio.Transacciones
{
    public class clsNListarMovimientoEPP : clsNBase
    {
        public List<MovimientoInventario> listaMovimientosInventario(string tipoMovimiento, string gsUsuario)
        {
            loBaseDa.CreateContext();

            var poUsuario = loBaseDa.Get<SEGMUSUARIO>().Where(c => c.CodigoUsuario == gsUsuario).FirstOrDefault();

            if(poUsuario.BodegaEPP == 3)
            {
                var lista = (from a in loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>()
                             where a.CodigoEstado == Diccionario.Activo
                                 && a.Tipo == tipoMovimiento
                                 && a.IdBodegaEPP == 3
                                 && a.IdTransferenciaStock == null && a.IdEntregaEpp == null 
                             select new MovimientoInventario
                             {
                                 IdMovimientoInventario = a.IdMovimientoInventario,
                                 IdBodegaEPP = a.IdBodegaEPP ?? 0,
                                 CodigoMotivo = a.CodigoMotivoMovInvEPP,
                                 CentroCosto = a.CentroCosto ?? " ",
                                 Fechamovimiento = a.FechaIngreso,
                                 Observaciones = a.Observaciones,
                             }).ToList().OrderBy(x => x.IdMovimientoInventario).ToList();

                return lista;
            } else if (poUsuario.BodegaEPP == 2)
            {
                var lista = (from a in loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>()
                             where a.CodigoEstado == Diccionario.Activo
                                 && a.Tipo == tipoMovimiento
                                 && a.IdBodegaEPP == 2
                                 && a.IdTransferenciaStock == null && a.IdEntregaEpp == null
                             select new MovimientoInventario
                             {
                                 IdMovimientoInventario = a.IdMovimientoInventario,
                                 IdBodegaEPP = a.IdBodegaEPP ?? 0,
                                 CodigoMotivo = a.CodigoMotivoMovInvEPP,
                                 CentroCosto = a.CentroCosto ?? " ",
                                 Fechamovimiento = a.FechaIngreso,
                                 Observaciones = a.Observaciones,
                             }).ToList().OrderBy(x => x.IdMovimientoInventario).ToList();

                return lista;
            }

            var plista = (from a in loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>()
                         where a.CodigoEstado == Diccionario.Activo
                             && a.Tipo == tipoMovimiento
                             && a.IdTransferenciaStock == null && a.IdEntregaEpp == null 
                         select new MovimientoInventario
                         {
                             IdMovimientoInventario = a.IdMovimientoInventario,
                             IdBodegaEPP = a.IdBodegaEPP ?? 0,
                             CodigoMotivo = a.CodigoMotivoMovInvEPP,
                             CentroCosto = a.CentroCosto ?? " ",
                             Fechamovimiento = a.FechaIngreso,
                             Observaciones = a.Observaciones,
                         }).ToList().OrderBy(x => x.IdMovimientoInventario).ToList();

            return plista;

        }

        public List<EntregaEPP> listaEntregaEPP(string gsUsuario)
        {
            loBaseDa.CreateContext();

            var poUsuario = loBaseDa.Get<SEGMUSUARIO>().Where(c => c.CodigoUsuario == gsUsuario).FirstOrDefault();

            if (poUsuario.BodegaEPP == 3)
            {
                var lista = (from a in loBaseDa.Get<SHETENTREGAEPP>()
                             where a.CodigoEstado == Diccionario.Activo
                                 && a.IdBodega == 3
                             select new EntregaEPP
                             {
                                 IdEntregaEPP = a.IdEntregaEPP,
                                 IdBodega = a.IdBodega,
                                 IdEmpleado = a.IdPersona,
                                 FechaIngreso = a.FechaIngreso,
                                 CentroCosto = a.CentroCosto ?? " ",
                                 Observaciones = a.Observaciones,
                             }).ToList().OrderBy(x => x.IdEntregaEPP).ToList();

                return lista;
            }
            else if (poUsuario.BodegaEPP == 2)
            {
                var lista = (from a in loBaseDa.Get<SHETENTREGAEPP>()
                             where a.CodigoEstado == Diccionario.Activo
                                 && a.IdBodega == 2
                             select new EntregaEPP
                             {
                                 IdEntregaEPP = a.IdEntregaEPP,
                                 IdBodega = a.IdBodega,
                                 IdEmpleado = a.IdPersona,
                                 FechaIngreso = a.FechaIngreso,
                                 CentroCosto = a.CentroCosto ?? " ",
                                 Observaciones = a.Observaciones,
                             }).ToList().OrderBy(x => x.IdEntregaEPP).ToList();

                return lista;
            }

            var plista = (from a in loBaseDa.Get<SHETENTREGAEPP>()
                          where a.CodigoEstado == Diccionario.Activo
                          select new EntregaEPP
                          {
                              IdEntregaEPP = a.IdEntregaEPP,
                              IdBodega = a.IdBodega,
                              IdEmpleado = a.IdPersona,
                              FechaIngreso = a.FechaIngreso,
                              CentroCosto = a.CentroCosto ?? " ",
                              Observaciones = a.Observaciones,
                          }).ToList().OrderBy(x => x.IdEntregaEPP).ToList();

            return plista;

        }

        public List<TransferenciaStock> listaTransferenciaStock(string gsUsuario)
        {
            loBaseDa.CreateContext();

            var poUsuario = loBaseDa.Get<SEGMUSUARIO>().Where(c => c.CodigoUsuario == gsUsuario).FirstOrDefault();

            if (poUsuario.BodegaEPP == 3)
            {
                var lista = (from a in loBaseDa.Get<SHETTRANSFERENCIASTOCK>()
                             where a.CodigoEstado == Diccionario.Activo
                                 && a.IdBodegaEPPDestino == 3
                             select new TransferenciaStock
                             {
                                 IdTransferenciaStock = a.IdTransferenciaStock,
                                 FechaTransferencia = a.FechaTrasferencia,
                                 Observaciones = a.Observaciones,
                                 IdBodegaEPPDestino = a.IdBodegaEPPDestino,
                                 IdBodegaEPPOrigen = a.IdBodegaEPPOrigen,
                                 FechaIngreso = a.FechaIngreso,
                                 CodigoEstado = a.CodigoEstado,
                                 Aprobado = a.Aprobado.HasValue && a.Aprobado.Value ? "APROBADO" : "PENDIENTE"
                             }).ToList().OrderBy(x => x.IdTransferenciaStock).ToList();

                return lista;
            }
            else if (poUsuario.BodegaEPP == 2)
            {
                var lista = (from a in loBaseDa.Get<SHETTRANSFERENCIASTOCK>()
                             where a.CodigoEstado == Diccionario.Activo
                                 && a.IdBodegaEPPDestino == 2                
                             select new TransferenciaStock
                             {
                                 IdTransferenciaStock = a.IdTransferenciaStock,
                                 FechaTransferencia = a.FechaTrasferencia,
                                 Observaciones = a.Observaciones,
                                 IdBodegaEPPDestino = a.IdBodegaEPPDestino,
                                 IdBodegaEPPOrigen = a.IdBodegaEPPOrigen,
                                 FechaIngreso = a.FechaIngreso,
                                 CodigoEstado = a.CodigoEstado,
                                 Aprobado = a.Aprobado.HasValue && a.Aprobado.Value ? "APROBADO" : "PENDIENTE"
                             }).ToList().OrderBy(x => x.IdTransferenciaStock).ToList();

                return lista;
            }

            var plista = (from a in loBaseDa.Get<SHETTRANSFERENCIASTOCK>()
                         where a.CodigoEstado == Diccionario.Activo
                         select new TransferenciaStock
                         {
                             IdTransferenciaStock = a.IdTransferenciaStock,
                             FechaTransferencia = a.FechaTrasferencia,
                             Observaciones = a.Observaciones,
                             IdBodegaEPPDestino = a.IdBodegaEPPDestino,
                             IdBodegaEPPOrigen = a.IdBodegaEPPOrigen,
                             FechaIngreso = a.FechaIngreso,
                             CodigoEstado = a.CodigoEstado,
                             Aprobado = a.Aprobado.HasValue && a.Aprobado.Value ? "APROBADO" : "PENDIENTE",
                             Motivo = a.IdBodegaEPPOrigen == poUsuario.BodegaEPP ? "ENTREGADO" : "RECIBIDO",
                         }).ToList().OrderBy(x => x.IdTransferenciaStock).ToList();

            return plista;

        }
    
    }
}
