using GEN_Entidad;
using GEN_Entidad.Entidades.SHEQ;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static GEN_Entidad.Diccionario;

namespace SHE_Negocio.Transacciones
{
    public class clsNEntregaEPP : clsNBase
    {

        public string gsGuardar(EntregaEPP toObject, string tsUsuario, string tsTerminal, out int tId, bool LimpiarContexto = true)
        {
            tId = 0;
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            int pId = toObject.IdEntregaEPP;

            psMsg = lsValidarDatos(toObject);
            if (string.IsNullOrEmpty(psMsg))
            {
                //Consulta entidad Cabecera
                var poObject = loBaseDa.Get<SHETENTREGAEPP>().Include(x => x.SHETENTREGAEPPDETALLE).Where(x => x.IdEntregaEPP == pId && x.IdEntregaEPP != 0).FirstOrDefault();

                // Determinar que filas fueron eliminadas de la presentación
                var piListaIdPresentacion = toObject.EntregaEPPDetalle.Where(x => x.IdEntregaEPPDetalle != 0).Select(x => x.IdEntregaEPPDetalle).ToList();
                if (poObject != null)
                {
                    foreach (var poItem in poObject.SHETENTREGAEPPDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdEntregaEPPDetalle)))
                    {
                        poItem.CodigoEstado = Diccionario.Inactivo;
                        poItem.UsuarioModificacion = tsUsuario;
                        poItem.FechaModificacion = DateTime.Now;
                        poItem.TerminalModificacion = tsTerminal;
                    }
                }

                bool pbuevo = false;
                if (poObject != null)
                {
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObject = new SHETENTREGAEPP();
                    loBaseDa.CreateNewObject(out poObject);
                    pbuevo = true;
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                }

                poObject.CodigoEstado = Diccionario.Activo;
                poObject.IdBodega = toObject.IdBodega;
                poObject.IdPersona = toObject.IdEmpleado;
                var nombreCompleto = gsNombre(poObject.IdPersona);
                poObject.Observaciones = toObject.Observaciones;
                poObject.CentroCosto = toObject.CentroCosto;

                if (toObject.EntregaEPPDetalle != null)
                {

                    foreach (var item in toObject.EntregaEPPDetalle)
                    {
                        int pIdDetalle = item.IdEntregaEPPDetalle;
                        var poObjectItem = poObject.SHETENTREGAEPPDETALLE.Where(x => x.IdEntregaEPPDetalle == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new SHETENTREGAEPPDETALLE();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObject.SHETENTREGAEPPDETALLE.Add(poObjectItem);
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.IdBodega = toObject.IdBodega;
                        poObjectItem.IdItemEPP = item.IdItemEPP;
                        poObjectItem.Cantidad = item.Cantidad;
                        poObjectItem.FechaEntrega = item.FechaEntrega;
                    }
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();
                    tId = poObject.IdEntregaEPP;

                    MovimientoInventario poMovimiento = new MovimientoInventario();
                    poMovimiento.GrupoMotivo = Diccionario.ListaCatalogo.MotivoEgresoInventarioEPP;
                    poMovimiento.CodigoMotivo = "CON";
                    poMovimiento.Observaciones = poObject.Observaciones;
                    poMovimiento.MovimientoInventarioDetalle = toObject.EntregaEPPDetalle
                        .Select(d => new MovimientoInventarioDetalle
                        {
                            IdBodegaEPP = d.IdBodega,
                            IdItemEPP = d.IdItemEPP,
                            Cantidad = d.Cantidad
                        })
                        .ToList();
                    poMovimiento.Tipo = Diccionario.TipoMovimiento.Egreso;
                    poMovimiento.IdEntregaEPP= poObject.IdEntregaEPP;
                    poMovimiento.Fechamovimiento = poObject.FechaIngreso;
                    poMovimiento.IdBodegaEPP = poObject.IdBodega;
                    poMovimiento.CentroCosto = poObject.CentroCosto;

                    int pIdMovimientoEgreso;
                    var poLogicaNegocioMovimiento = new clsNMovimientoInventario();
                    psMsg = poLogicaNegocioMovimiento.gsGuardarMovimientoEntregaEPP(poMovimiento, tsUsuario, tsTerminal, out pIdMovimientoEgreso);

                    loBaseDa.SaveChanges();
                    poTran.Complete();
                }
            }

            return psMsg;
        }

        public string gsNombre( int idPersona)
        {
            var nombre = loBaseDa.Get<REHVTPERSONACONTRATO>().Where(x => x.IdPersona == idPersona)
                .Select(c => c.NombreCompleto).FirstOrDefault();
            return nombre;
        }

        public string gsAnular(int tId, string tsUsuario, string tsTerminal, bool LimpiarContexto = true)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poObject = loBaseDa.Get<SHETENTREGAEPP>().Include(x => x.SHETENTREGAEPPDETALLE).Where(x => x.IdEntregaEPP == tId && x.IdEntregaEPP != 0).FirstOrDefault();

            if (string.IsNullOrEmpty(psMsg))
            {
                //Consulta entidad Cabecera
                poObject = loBaseDa.Get<SHETENTREGAEPP>().Include(x => x.SHETENTREGAEPPDETALLE).Where(x => x.IdEntregaEPP == tId && x.IdEntregaEPP != 0).FirstOrDefault();

                // Determinar que filas fueron eliminadas de la presentación
                if (poObject != null)
                {
                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;

                    foreach (var poItem in poObject.SHETENTREGAEPPDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {
                        poItem.CodigoEstado = Diccionario.Eliminado;
                        poItem.UsuarioModificacion = tsUsuario;
                        poItem.FechaModificacion = DateTime.Now;
                        poItem.TerminalModificacion = tsTerminal;
                    }
                }

                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();

                    var idMovimiento = loBaseDa.Get<SHETMOVIMIENTOINVENTARIO>().Where(c => c.IdEntregaEpp == tId && c.CodigoEstado == "A" && c.Tipo == "E")
                       .Select(c => c.IdMovimientoInventario).FirstOrDefault();

                    var poLogicaNegocioMovimiento = new clsNMovimientoInventario();

                    psMsg = poLogicaNegocioMovimiento.gsAnularMovimientoStock(idMovimiento, tsUsuario, tsTerminal);


                    loBaseDa.SaveChanges();
                    poTran.Complete();
                }

            }

            return psMsg;

        }

        public string goBuscarCodigo(string tsTipo, string tsCodigo = "")
        {
            string psCodigo = string.Empty;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<SHETENTREGAEPP>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdEntregaEPP }).OrderBy(x => x.IdEntregaEPP).FirstOrDefault()?.IdEntregaEPP.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<SHETENTREGAEPP>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdEntregaEPP }).OrderByDescending(x => x.IdEntregaEPP).FirstOrDefault()?.IdEntregaEPP.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<SHETENTREGAEPP>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdEntregaEPP }).ToList().Where(x => x.IdEntregaEPP < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdEntregaEPP).FirstOrDefault().IdEntregaEPP.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<SHETENTREGAEPP>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdEntregaEPP }).ToList().Where(x => x.IdEntregaEPP > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdEntregaEPP).FirstOrDefault().IdEntregaEPP.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            return psCodigo;

        }

        private string lsValidarDatos(EntregaEPP toObject)
        {
            string psMsg = "";

            if (toObject.IdEmpleado <= 0)
            {
                psMsg = string.Format("{0}Seleccione un Empleado. \n", psMsg);
            }

            if (toObject.IdBodega <= 0)
            {
                psMsg = string.Format("{0}Seleccione una Bodega. \n", psMsg);
            }

            if (string.IsNullOrEmpty(toObject.Observaciones))
            {
                psMsg = string.Format("{0}Ingrese Observaciones. \n", psMsg);
            }

            if (toObject.EntregaEPPDetalle.Count == 0)
            {
                psMsg = string.Format("{0}Ingrese detalle de ítems. \n", psMsg);
            }

            var loCombo = goConsultarComboItemEPP();

            var poLista = toObject.EntregaEPPDetalle;
            var poListaRecorrida = new List<EntregaEPPDetalle>();
            int fila = 1;
            foreach (var item in poLista)
            {
                string psName = loCombo.Where(x => x.Codigo == item.IdItemEPP.ToString()).Select(x => x.Descripcion).FirstOrDefault();

                if (item.FechaEntrega > DateTime.Now)
                {
                    psMsg = string.Format("{0}La fecha de entrega no puede ser mayor a la fecha actual en la Fila # {1}\n", psMsg, fila);
                }

                if (item.IdItemEPP.ToString() == Diccionario.Seleccione)
                {
                    psMsg = string.Format("{0}Seleccione ïtem en la Fila # {1}\n", psMsg, fila);
                }

                if (item.Cantidad <= 0)
                {
                    psMsg = string.Format("{0}Cantidad debe ser mayor a 0 en la Fila # {1}\n", psMsg, fila);
                }

                else if (poListaRecorrida.Where(x => x.IdItemEPP == item.IdItemEPP).Count() > 0)
                {
                    psMsg = string.Format("{0}Eliminar Fila # {1}, Ya existe ingresado el ítem: {2}\n", psMsg, fila, psName);
                }

                poListaRecorrida.Add(item);
                fila++;
            }


            return psMsg;
        }

        public EntregaEPP goConsultarMovimiento(int tId)
        {
            loBaseDa.CreateContext();
            var poCab = loBaseDa.Get<SHETENTREGAEPP>().Include(x => x.SHETENTREGAEPPDETALLE).Where(x => x.IdEntregaEPP == tId).FirstOrDefault();
            return goConsultarMovimiento(poCab);
        }

        private EntregaEPP goConsultarMovimiento(SHETENTREGAEPP poCab)
        {
            EntregaEPP poObjectReturn = null;

            if (poCab != null)
            {
                poObjectReturn = new EntregaEPP();

                poObjectReturn.IdEntregaEPP = poCab.IdEntregaEPP;
                poObjectReturn.IdBodega = poCab.IdBodega;
                poObjectReturn.IdEmpleado = poCab.IdPersona;
                poObjectReturn.FechaIngreso = poCab.FechaIngreso;
                poObjectReturn.Observaciones = poCab.Observaciones ?? " ";
                poObjectReturn.CentroCosto = poCab.CentroCosto ?? "0";

                poObjectReturn.EntregaEPPDetalle = new List<EntregaEPPDetalle>();
                foreach (var detalle in poCab.SHETENTREGAEPPDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new EntregaEPPDetalle();

                    poDet.IdEntregaEPPDetalle = detalle.IdEntregaEPPDetalle;
                    poDet.IdEntregaEPP = detalle.IdEntregaEPP;
                    poDet.IdItemEPP = detalle.IdItemEPP;
                    poDet.IdBodega = detalle.IdBodega;
                    poDet.Cantidad = detalle.Cantidad;
                    poDet.FechaEntrega = detalle.FechaEntrega;

                    poObjectReturn.EntregaEPPDetalle.Add(poDet);
                }
            }
            return poObjectReturn;
        }

        public EntregaEPP goConsultarMovimientoCopiar(int tId)
        {
            loBaseDa.CreateContext();
            var poCab = loBaseDa.Get<SHETENTREGAEPP>().Include(x => x.SHETENTREGAEPPDETALLE).Where(x => x.IdEntregaEPP == tId).FirstOrDefault();
            return goConsultarMovimientoCopiar(poCab);
        }

        private EntregaEPP goConsultarMovimientoCopiar(SHETENTREGAEPP poCab)
        {
            EntregaEPP poObjectReturn = null;

            if (poCab != null)
            {
                poObjectReturn = new EntregaEPP();

                //poObjectReturn.IdEntregaEPP = poCab.IdEntregaEPP;
                poObjectReturn.IdBodega = poCab.IdBodega;
                poObjectReturn.IdEmpleado = poCab.IdPersona;
                poObjectReturn.FechaIngreso = poCab.FechaIngreso;
                poObjectReturn.Observaciones = poCab.Observaciones;
                poObjectReturn.CentroCosto = poCab.CentroCosto;

                poObjectReturn.EntregaEPPDetalle = new List<EntregaEPPDetalle>();
                foreach (var detalle in poCab.SHETENTREGAEPPDETALLE)
                {
                    var poDet = new EntregaEPPDetalle();

                   //poDet.IdEntregaEPPDetalle = detalle.IdEntregaEPPDetalle;
                    //poDet.IdEntregaEPP = detalle.IdEntregaEPP;
                    poDet.IdItemEPP = detalle.IdItemEPP;
                    poDet.IdBodega = detalle.IdBodega;
                    poDet.Cantidad = detalle.Cantidad;
                    poDet.FechaEntrega = detalle.FechaEntrega;

                    poObjectReturn.EntregaEPPDetalle.Add(poDet);
                }
            }
            return poObjectReturn;
        }

        public List<EntregaEPP> listaTodos()
        {
            loBaseDa.CreateContext();

            var lista = (from a in loBaseDa.Get<SHETENTREGAEPP>()
                         where a.CodigoEstado == "A"
                         select new EntregaEPP
                         {
                             IdEntregaEPP = a.IdEntregaEPP,
                             FechaIngreso = a.FechaIngreso,
                             IdBodega = a.IdBodega,
                             IdEmpleado = a.IdPersona
                         }).ToList().OrderBy(x => x.IdEntregaEPP).ToList();

            return lista;
        }

        public List<EntregaEPP> listaTodosCopia()
        {
            loBaseDa.CreateContext();

            var lista = (from a in loBaseDa.Get<SHETENTREGAEPP>()
                          select new EntregaEPP
                         {
                             IdEntregaEPP = a.IdEntregaEPP,
                             FechaIngreso = a.FechaIngreso,
                             IdBodega = a.IdBodega,
                             IdEmpleado = a.IdPersona,
                             CodigoEstado = a.CodigoEstado
                         }).ToList().OrderBy(x => x.IdEntregaEPP).ToList();

            return lista;
        }

        public string obtenerNombreCompleto(int idPersona)
        {
            return loBaseDa.Find<REHVTPERSONACONTRATO>()
                 .Where(c => c.IdPersona == idPersona)
                 .Select(c => c.NombreCompleto).FirstOrDefault();
        }

        public int obtenerBodegaUsuario(int idPersona)
        {
            var idBodega = loBaseDa.Get<SEGMUSUARIO>()
                                .Where(c => c.IdPersona == idPersona && c.CodigoEstado == "A")
                                .Select(c => c.BodegaEPP ?? 0) 
                                .FirstOrDefault();
            return idBodega;
        }

        public string obtenerCentroCostoUsuario(int idPersona)
        {
            var gsCentroCosto = loBaseDa.Get<REHVTPERSONACONTRATO>()
                                .Where(c => c.IdPersona == idPersona)
                                .Select(c => c.CentroCosto)
                                .FirstOrDefault();

            var idCentroCosto = loBaseDa.Get<GENMCENTROCOSTO>()
                                .Where(c => c.Descripcion == gsCentroCosto)
                                .Select(c => c.CodigoCentroCosto).FirstOrDefault();

            if(idCentroCosto == null)
            {
                idCentroCosto = "0";
            }

            return idCentroCosto;
        }

    }
}
