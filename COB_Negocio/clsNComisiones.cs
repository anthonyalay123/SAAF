using GEN_Entidad;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using GEN_Entidad.Entidades.Cobranza;
using System.Data.SqlClient;
using GEN_Entidad.Entidades.Ventas;

namespace COB_Negocio
{
    public class clsNComisiones : clsNBase
    {
        #region Proceso de comisiones
        public List<Comisiones> goConsultarComisiones()
        {
            var poLista = new List<Comisiones>();
            return poLista;
            
        }

        public List<Comisiones> goConsultar()
        {
            return (from a in loBaseDa.Find<COBTCOMISIONES>()
                    join b in loBaseDa.Find<REHPPERIODO>() on a.IdPeriodo equals b.IdPeriodo
                    join c in loBaseDa.Find<REHMTIPOROL>() on b.CodigoTipoRol equals c.CodigoTipoRol
                    join d in loBaseDa.Find<GENMESTADO>() on a.CodigoEstado equals d.CodigoEstado
                    where !Diccionario.EstadosNoIncluyentesSistema.Contains(a.CodigoEstado)
                    select new Comisiones()
                    {
                        Id = a.IdComisiones,
                        IdPeriodo = a.IdPeriodo,
                        CodigoTipoRol = a.CodigoTipoRol,
                        CodigoPeriodo = b.Codigo,
                        DesTipoRol = c.Descripcion,
                        FechaCreacion = a.FechaIngreso,
                        FechaInicio = b.FechaInicioComi??DateTime.Now,
                        FechaFin = b.FechaFinComi??DateTime.Now,
                        CodigoEstado = a.CodigoEstado,
                        DesEstado = d.Descripcion,
                        Total = a.Total,
                        TotalComisiones = a.TotalComisiones??0M
                    }).OrderBy(x => x.Id).ToList();
        }

        public List<BorradorComisiones> goListarCobranza(DateTime tdFechaInicial, DateTime tdFechaFinal)
        {
            return loBaseDa.ExecStoreProcedure<BorradorComisiones>(string.Format("COBSPCONSULTACOBRANZA '{0}','{1}'", tdFechaInicial.ToString("yyyyMMdd"), tdFechaFinal.ToString("yyyyMMdd")));
        }

        public List<BorradorComisiones> gGenerarBorradorComisiones(int tIdPeriodo, string tsUsuario,bool tbEliminarRegistrosManuales)
        {
            return loBaseDa.ExecStoreProcedure<BorradorComisiones>(string.Format("COBSPGENERARBORRADORCOMISIONES {0},'{1}','{2}'", tIdPeriodo, tsUsuario, tbEliminarRegistrosManuales));
        }

        public void gCalcularComisiones(int tIdPeriodo, string tsUsuario)
        {
            loBaseDa.ExecuteQuery(string.Format("EXEC COBSPCALCULARCOMISIONES {0},'{1}'", tIdPeriodo, tsUsuario));
        }


        public DataSet gdsComisionesZona(int tIdPeriodo)
        {
            return loBaseDa.DataSet(string.Format("EXEC COBSPRPTCOMISIONESZONA {0}", tIdPeriodo));
        }

        public DataSet gdsComisionesEmpleado(int tIdPeriodo)
        {
            return loBaseDa.DataSet(string.Format("EXEC COBSPRPTCOMISIONESEMPLEADO {0}", tIdPeriodo));
        }

        public DataSet gdsComisionesZonaEmpleado(int tIdPeriodo)
        {
            return loBaseDa.DataSet(string.Format("EXEC COBSPRPTCOMISIONESZONAEMPLEADO {0}", tIdPeriodo));
        }

        public string gsGuardarBorradorComisiones(int tIdPeriodo, List<BorradorComisiones> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if (toLista != null && toLista.Count > 0)
            {
                List<string> psListaEstado = new List<string>();
                psListaEstado.Add(Diccionario.Eliminado);
                psListaEstado.Add(Diccionario.Inactivo);

                var poObject = loBaseDa.Get<COBTCOMISIONES>().Include(x=>x.COBTCOMISIONESDETALLE).Where(x => !psListaEstado.Contains(x.CodigoEstado) && x.IdPeriodo == tIdPeriodo).FirstOrDefault();

                if (poObject != null)
                {
                    var poLista = poObject.COBTCOMISIONESDETALLE.Where(x => !psListaEstado.Contains(x.CodigoEstado)).ToList();

                    var piListaId = toLista.Where(x => x.IdComisionesDetalle != 0).Select(x => x.IdComisionesDetalle).ToList();

                    foreach (var poItem in poLista.Where(x => !piListaId.Contains(x.IdComisionesDetalle)))
                    {
                        poItem.CodigoEstado = Diccionario.Inactivo;
                        poItem.UsuarioModificacion = tsUsuario;
                        poItem.FechaModificacion = DateTime.Now;
                        poItem.TerminalModificacion = tsTerminal;
                    }

                    foreach (var poItem in toLista)
                    {
                        var poDetalle = poLista.Where(x => x.IdComisionesDetalle == poItem.IdComisionesDetalle && x.IdComisionesDetalle != 0).FirstOrDefault();
                        if (poDetalle != null)
                        {
                            poDetalle.UsuarioModificacion = tsUsuario;
                            poDetalle.FechaModificacion = DateTime.Now;
                            poDetalle.TerminalModificacion = tsTerminal;
                            poDetalle.Valor = poItem.Valor;
                            poDetalle.Considerado = poItem.Considerar;
                            poDetalle.ConsideradoCobranza = poItem.ConsiderarCobranza;
                        }
                        else
                        {
                            poDetalle = new COBTCOMISIONESDETALLE();
                            poDetalle.UsuarioIngreso = tsUsuario;
                            poDetalle.FechaIngreso = DateTime.Now;
                            poDetalle.TerminalIngreso = tsTerminal;

                            poDetalle.CodigoEstado = Diccionario.Pendiente;
                            poDetalle.CodVendedor = poItem.CodVendedor;
                            poDetalle.NomVendedor = poItem.NomVendedor;
                            poDetalle.CodCliente = poItem.CodCliente;
                            poDetalle.GrupoCliente = poItem.GrupoCliente;
                            poDetalle.NumFactura = poItem.NumFactura;
                            poDetalle.Titular = poItem.Titular;
                            poDetalle.CodicionPago = poItem.CondicionPago;
                            poDetalle.FechaEmision = poItem.FechaEmision;
                            poDetalle.FechaVecimineto = poItem.FechaVencimiento;
                            poDetalle.FechaContabilizacion = poItem.FechaContabilizacion;
                            poDetalle.FechaEfectiva = poItem.FechaEfectiva;
                            poDetalle.NumDocPago = poItem.NumDocPago;
                            poDetalle.Recaudador = poItem.Recaudador;
                            poDetalle.TipoDoc = poItem.TipoDoc;
                            poDetalle.NoCheque = poItem.NoCheque;
                            poDetalle.CodBanco = poItem.CodBanco;
                            poDetalle.NomBanco = poItem.NomBanco;
                            poDetalle.ValorTotal = poItem.ValorTotal;
                            poDetalle.ValorCuenta = poItem.ValorCuenta;
                            poDetalle.Valor = poItem.Valor;
                            poDetalle.NomEmpresa = poItem.NomEmpresa;
                            poDetalle.CodeZona = poItem.Code;
                            poDetalle.NameZona = poItem.Zona;
                            poDetalle.CodeZonaMod = poItem.CodeZonaMod;
                            poDetalle.NameZonaMod = poItem.NameZonaMod;
                            poDetalle.DiasPago = poItem.DiasPago;
                            poDetalle.Considerado = poItem.Considerar;
                            poDetalle.ConsideradoCobranza = poItem.ConsiderarCobranza;
                            poDetalle.Ingreso = poItem.Ingreso;

                            poObject.COBTCOMISIONESDETALLE.Add(poDetalle);

                        }

                        poObject.Total = toLista.Where(x => x.Considerar).Sum(x => x.Valor);
                    }

                    loBaseDa.SaveChanges();
                }

                

            }

            return psMsg;
        }


        public string gsReversarComisiones(int tiPeriodo, string tsUsuario)
        {
            return loBaseDa.ExecStoreProcedure<string>
               ("COBSPREVERSARCOMISIONES @IdPeriodo, @Usuario",
               new SqlParameter("@IdPeriodo", tiPeriodo),
               new SqlParameter("@Usuario", tsUsuario)
               ).FirstOrDefault();
        }

        public string gsCerrarBorradorComisiones(int tiPeriodo, string tsUsuario)
        {
            return loBaseDa.ExecStoreProcedure<string>
                ("COBSPCERRARCOMISIONES @IdPeriodo, @Usuario",
                new SqlParameter("@IdPeriodo", tiPeriodo),
                new SqlParameter("@Usuario", tsUsuario)
                ).FirstOrDefault();
        }

        /// <summary>
        /// Elimina Nómina, Pone en estado Eliminado la Nomina
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public bool gbEliminarComisiones(int tiPeriodo, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();

            var poObject = loBaseDa.Get<COBTCOMISIONES>().Include(x => x.COBTCOMISIONESDETALLE).Where(x => x.IdPeriodo == tiPeriodo && x.CodigoEstado == Diccionario.Pendiente).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                poObject.FechaModificacion = DateTime.Now;

                foreach (var poItem in poObject.COBTCOMISIONESDETALLE.Where(x => x.CodigoEstado == Diccionario.Pendiente))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.TerminalModificacion = tsTerminal;
                    poItem.FechaModificacion = DateTime.Now;
                }

                loBaseDa.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// Retorna estado de nNómina para validaciones posteriores
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public string gsGetEstadoComision(int tiPeriodo)
        {
            return loBaseDa.Find<COBTCOMISIONES>().Where(x => x.IdPeriodo == tiPeriodo && (x.CodigoEstado == Diccionario.Activo || x.CodigoEstado == Diccionario.Pendiente || x.CodigoEstado == Diccionario.Cerrado)).Select(x => x.CodigoEstado).FirstOrDefault();
        }

        public List<BorradorComisiones> goConsultarComisiones(int tIdPeriodo)
        {
            List<string> psListaEstado = new List<string>();
            psListaEstado.Add(Diccionario.Eliminado);
            psListaEstado.Add(Diccionario.Inactivo);

            return (from a in loBaseDa.Find<COBTCOMISIONES>()
                    join b in loBaseDa.Find<COBTCOMISIONESDETALLE>() on a.IdComisiones equals b.IdComisiones
                    where !psListaEstado.Contains(b.CodigoEstado) && a.IdPeriodo == tIdPeriodo
                    select new BorradorComisiones()
                    {
                        IdComisionesDetalle = b.IdComisionesDetalle,
                        CodBanco = b.CodBanco,
                        CodCliente = b.CodCliente,
                        CondicionPago = b.CodicionPago,
                        CodVendedor = b.CodVendedor,
                        Considerar = b.Considerado,
                        ConsiderarCobranza = b.ConsideradoCobranza??false,
                        DiasPago = b.DiasPago,
                        FechaContabilizacion = b.FechaContabilizacion,
                        FechaEfectiva = b.FechaEfectiva,
                        FechaEmision = b.FechaEmision,
                        FechaVencimiento = b.FechaVecimineto,
                        GrupoCliente = b.GrupoCliente,
                        NoCheque = b.NoCheque,
                        NomBanco = b.NomBanco,
                        NomEmpresa = b.NomEmpresa,
                        NomVendedor = b.NomVendedor,
                        NumDocPago = b.NumDocPago,
                        NumFactura = b.NumFactura,
                        Recaudador = b.Recaudador,
                        TipoDoc = b.TipoDoc,
                        Titular = b.Titular,
                        Valor = b.Valor,
                        ValorCuenta = b.ValorCuenta,
                        ValorTotal = b.ValorTotal,
                        Code = b.CodeZona,
                        Zona = b.NameZona,
                        CodeZonaMod = b.CodeZonaMod,
                        NameZonaMod = b.NameZonaMod,
                        Ingreso = b.Ingreso,
                        DiasDocumento = b.DiasDocumento??0
                    }).ToList();
        }

        public List<CalcularComisiones> goConsultarComisionesCalculadas(int tIdPeriodo)
        {
            List<string> psListaEstado = new List<string>();
            psListaEstado.Add(Diccionario.Eliminado);
            psListaEstado.Add(Diccionario.Inactivo);

            return (from a in loBaseDa.Find<COBTCOMISIONES>()
                    join b in loBaseDa.Find<COBTCOMISIONESDETALLE>() on a.IdComisiones equals b.IdComisiones
                    where !psListaEstado.Contains(b.CodigoEstado) && a.IdPeriodo == tIdPeriodo
                    select new CalcularComisiones()
                    {
                        IdComisionesDetalle = b.IdComisionesDetalle,
                        CondicionPago = b.CodicionPago,
                        DiasPago = b.DiasPago,
                        NomEmpresa = b.NomEmpresa,
                        NomVendedor = b.NomVendedor,
                        NumFactura = b.NumFactura,
                        Recaudador = b.Recaudador,
                        Titular = b.Titular,
                        Valor = b.Valor,
                        Zona = b.NameZona,
                        Ingreso = b.Ingreso,
                        PorcentajeRTC = b.PorcentajeRTC ?? 0,
                        PorcentajeJBO = b.PorcentajeJBO ?? 0,
                        PorcentajeABO = b.PorcentajeABO ?? 0,
                        PorcentajePTC = b.PorcentajePTC ?? 0,
                        PorcentajeFMA = b.PorcentajeFMA ?? 0,
                        ComisionRTC = b.ComisionRTC ?? 0,
                        ComisionJBO = b.ComisionJBO ?? 0,
                        ComisionABO = b.ComisionABO ?? 0,
                        ComisionPTC = b.ComisionPTC ?? 0,
                        ComisionFMA = b.ComisionFMA ?? 0,
                    }).ToList();
        }
        #endregion

        #region Excepcion Cliente Comision
        public List<ClienteComision> goConsultarExcepcionClienteComision()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<ClienteComision>();
            var poLista = loBaseDa.Find<COBPCLIENTECOMISION>().Include(x => x.COBPCLIENTECOMISIONDETALLE).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            foreach (var item in poLista)
            {
                var poCab = new ClienteComision();
                poCab.IdClienteComision = item.IdClienteComision;
                poCab.CardCode = item.CardCode;
                poCab.CardName = item.CardName;

                poCab.ClienteComisionDetalle = new List<ClienteComisionDetalle>();
                foreach (var detalle in item.COBPCLIENTECOMISIONDETALLE.Where(x=>x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new ClienteComisionDetalle();
                    
                    poDet.CodeZona = detalle.CodeZona;
                    poDet.CodigoComision = detalle.CodigoComision;
                    poDet.CodigoToleraciaContado = detalle.CodigoToleraciaContado;
                    poDet.IdClienteComision = detalle.IdClienteComision;
                    poDet.IdClienteComisionDetalle = detalle.IdClienteComisionDetalle;
                    //poDet.NameZona = detalle.NameZona;
                    poDet.PorcentajeComision = detalle.PorcentajeComision;
                    poCab.ClienteComisionDetalle.Add(poDet);
                }
                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public string gsGuardarClienteComision(List<ClienteComision> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<COBPCLIENTECOMISION>().Include(X => X.COBPCLIENTECOMISIONDETALLE).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdClienteComision != 0).Select(x => x.IdClienteComision).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdClienteComision)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;

                foreach (var item in poItem.COBPCLIENTECOMISIONDETALLE.Where(X => X.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Inactivo;
                    item.UsuarioModificacion = tsUsuario;
                    item.FechaModificacion = DateTime.Now;
                    item.TerminalModificacion = tsTerminal;
                }
            }

            if (toLista != null && toLista.Count > 0)
            {
                var poListaClientes = goSapConsultaClientes();
                var poListaZonasSap = goConsultarZonasSAP();
                
                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdClienteComision == poItem.IdClienteComision && x.IdClienteComision != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new COBPCLIENTECOMISION();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.CardCode = poItem.CardCode;
                    poObject.CardName = poListaClientes.Where(x=>x.Codigo == poItem.CardCode).Select(x=>x.Descripcion).FirstOrDefault();

                    if (poItem.ClienteComisionDetalle != null)
                    {
                        //Eliminar Detalle 
                        piListaIdPresentacion = poItem.ClienteComisionDetalle.Where(x => x.IdClienteComisionDetalle != 0).Select(x => x.IdClienteComisionDetalle).ToList();

                        foreach (var poItemDel in poObject.COBPCLIENTECOMISIONDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdClienteComisionDetalle)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in poItem.ClienteComisionDetalle)
                        {
                            int pId = item.IdClienteComisionDetalle;
                            var poObjectItem = poObject.COBPCLIENTECOMISIONDETALLE.Where(x => x.IdClienteComisionDetalle == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new COBPCLIENTECOMISIONDETALLE();
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                                poObject.COBPCLIENTECOMISIONDETALLE.Add(poObjectItem);
                            }

                            poObjectItem.CodeZona = item.CodeZona;
                            poObjectItem.CodigoComision = item.CodigoComision;
                            poObjectItem.CodigoEstado = Diccionario.Activo;
                            poObjectItem.CodigoToleraciaContado = item.CodigoToleraciaContado;
                            poObjectItem.NameZona = poListaZonasSap.Where(x => x.Codigo == item.CodeZona).Select(x => x.Descripcion).FirstOrDefault();
                            poObjectItem.PorcentajeComision = item.PorcentajeComision;
                        }
                    }
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion

        #region Excepcion Factura Comision
        public List<FacturaComision> goConsultarExcepcionFacturaComision()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<FacturaComision>();
            var poLista = loBaseDa.Find<COBPFACTURACOMISION>().Include(x => x.COBPFACTURACOMISIONDETALLE).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            foreach (var item in poLista)
            {
                var poCab = new FacturaComision();
                poCab.IdFacturaComision = item.IdFacturaComision;
                poCab.NumFactura = item.NumFactura;
                poCab.CardCode = item.CardCode;
                poCab.CardName = item.CardName;

                poCab.FacturaComisionDetalle = new List<FacturaComisionDetalle>();
                foreach (var detalle in item.COBPFACTURACOMISIONDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new FacturaComisionDetalle();

                    poDet.CodeZona = detalle.CodeZona;
                    poDet.CodigoComision = detalle.CodigoComision;
                    poDet.CodigoToleraciaContado = detalle.CodigoToleraciaContado;
                    poDet.IdFacturaComision = detalle.IdFacturaComision;
                    poDet.IdFacturaComisionDetalle = detalle.IdFacturaComisionDetalle;
                    //poDet.NameZona = detalle.NameZona;
                    poDet.PorcentajeComision = detalle.PorcentajeComision;
                    poCab.FacturaComisionDetalle.Add(poDet);
                }
                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public string gsGuardarFacturaComision(List<FacturaComision> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<COBPFACTURACOMISION>().Include(X => X.COBPFACTURACOMISIONDETALLE).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdFacturaComision != 0).Select(x => x.IdFacturaComision).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdFacturaComision)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;

                foreach (var item in poItem.COBPFACTURACOMISIONDETALLE.Where(X => X.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Inactivo;
                    item.UsuarioModificacion = tsUsuario;
                    item.FechaModificacion = DateTime.Now;
                    item.TerminalModificacion = tsTerminal;

                }
            }

            if (toLista != null && toLista.Count > 0)
            {
                var poListaClientes = goSapConsultaClientes();
                var poListaZonasSap = goConsultarZonasSAP();

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdFacturaComision == poItem.IdFacturaComision && x.IdFacturaComision != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new COBPFACTURACOMISION();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.NumFactura = poItem.NumFactura;
                    poObject.CardCode = poItem.CardCode;
                    poObject.CardName = poListaClientes.Where(x => x.Codigo == poItem.CardCode).Select(x => x.Descripcion).FirstOrDefault();

                    if (poItem.FacturaComisionDetalle != null)
                    {
                        //Eliminar Detalle 
                        piListaIdPresentacion = poItem.FacturaComisionDetalle.Where(x => x.IdFacturaComisionDetalle != 0).Select(x => x.IdFacturaComisionDetalle).ToList();

                        foreach (var poItemDel in poObject.COBPFACTURACOMISIONDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdFacturaComisionDetalle)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in poItem.FacturaComisionDetalle)
                        {
                            int pId = item.IdFacturaComisionDetalle;
                            var poObjectItem = poObject.COBPFACTURACOMISIONDETALLE.Where(x => x.IdFacturaComisionDetalle == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new COBPFACTURACOMISIONDETALLE();
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                                poObject.COBPFACTURACOMISIONDETALLE.Add(poObjectItem);
                            }

                            poObjectItem.CodeZona = item.CodeZona;
                            poObjectItem.CodigoComision = item.CodigoComision;
                            poObjectItem.CodigoEstado = Diccionario.Activo;
                            poObjectItem.CodigoToleraciaContado = item.CodigoToleraciaContado;
                            poObjectItem.NameZona = poListaZonasSap.Where(x => x.Codigo == item.CodeZona).Select(x => x.Descripcion).FirstOrDefault();
                            poObjectItem.PorcentajeComision = item.PorcentajeComision;
                        }
                    }
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion

        #region Excepcion Zonas Días de Documento
        public List<ZonaDiasDocumento> goConsultarExcepcionZonaDiasDocumento()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<ZonaDiasDocumento>();
            var poLista = loBaseDa.Find<COBPZONADIASDOCUMENTO>().Include(x => x.COBPZONADIASDOCUMENTODETALLE).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            foreach (var item in poLista)
            {
                var poCab = new ZonaDiasDocumento();
                poCab.IdZonaDiasDocumento = item.IdZonaDiasDocumento;
                poCab.CodeZona = item.CodeZona;
                poCab.NameZona = item.NameZona;

                poCab.ZonaDiasDocumentoDetalle = new List<ZonaDiasDocumentoDetalle>();
                foreach (var detalle in item.COBPZONADIASDOCUMENTODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new ZonaDiasDocumentoDetalle();

                    poDet.DiasDocumento = detalle.DiasDocumento;
                    poDet.DiasDocumentoAdd = detalle.DiasDocumentoAdd;
                    poDet.IdZonaDiasDocumento = detalle.IdZonaDiasDocumento;
                    poDet.IdZonaDiasDocumentoDetalle = detalle.IdZonaDiasDocumentoDetalle;
                    poCab.ZonaDiasDocumentoDetalle.Add(poDet);
                }
                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public string gsGuardarZonaDiasDocumento(List<ZonaDiasDocumento> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<COBPZONADIASDOCUMENTO>().Include(X => X.COBPZONADIASDOCUMENTODETALLE).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdZonaDiasDocumento != 0).Select(x => x.IdZonaDiasDocumento).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdZonaDiasDocumento)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;

                foreach (var item in poItem.COBPZONADIASDOCUMENTODETALLE.Where(X => X.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Inactivo;
                    item.UsuarioModificacion = tsUsuario;
                    item.FechaModificacion = DateTime.Now;
                    item.TerminalModificacion = tsTerminal;
                }
            }

            if (toLista != null && toLista.Count > 0)
            {
                var poListaZonasSap = goConsultarZonasSAP();

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdZonaDiasDocumento == poItem.IdZonaDiasDocumento && x.IdZonaDiasDocumento != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new COBPZONADIASDOCUMENTO();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.CodeZona = poItem.CodeZona;
                    poObject.NameZona = poListaZonasSap.Where(x => x.Codigo == poItem.CodeZona).Select(x => x.Descripcion).FirstOrDefault();

                    if (poItem.ZonaDiasDocumentoDetalle != null)
                    {
                        //Eliminar Detalle 
                        piListaIdPresentacion = poItem.ZonaDiasDocumentoDetalle.Where(x => x.IdZonaDiasDocumentoDetalle != 0).Select(x => x.IdZonaDiasDocumentoDetalle).ToList();

                        foreach (var poItemDel in poObject.COBPZONADIASDOCUMENTODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdZonaDiasDocumentoDetalle)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in poItem.ZonaDiasDocumentoDetalle)
                        {
                            int pId = item.IdZonaDiasDocumentoDetalle;
                            var poObjectItem = poObject.COBPZONADIASDOCUMENTODETALLE.Where(x => x.IdZonaDiasDocumentoDetalle == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new COBPZONADIASDOCUMENTODETALLE();
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                                poObject.COBPZONADIASDOCUMENTODETALLE.Add(poObjectItem);
                            }

                            poObjectItem.DiasDocumento = item.DiasDocumento;
                            poObjectItem.DiasDocumentoAdd = item.DiasDocumentoAdd;
                            poObjectItem.CodigoEstado = Diccionario.Activo;
                        }
                    }
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion
        
        #region Cliente Pago Aseguradora
        public List<ClientePagoAseguradora> goConsultarClientePagoAseguradora()
        {
            loBaseDa.CreateContext();
            
            return loBaseDa.Find<COBPCLIENTEPAGOASEGURADORA>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x=> new ClientePagoAseguradora()
            {
                IdClientePagoAseguradora = x.IdClientePagoAseguradora,
                CardCode = x.CardCode,
                CardName = x.CardName,
                CodigoEstado = x.CodigoEstado,
            }).ToList();
            
        }

        public string gsGuardarClientePagoAseguradora(List<ClientePagoAseguradora> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<COBPCLIENTEPAGOASEGURADORA>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdClientePagoAseguradora != 0).Select(x => x.IdClientePagoAseguradora).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdClientePagoAseguradora)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;
            }

            if (toLista != null && toLista.Count > 0)
            {
                var poListaClientes = goSapConsultaClientes();
                //var poListaZonasSap = goConsultarZonasSAP();

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdClientePagoAseguradora == poItem.IdClientePagoAseguradora && x.IdClientePagoAseguradora != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new COBPCLIENTEPAGOASEGURADORA();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.CardCode = poItem.CardCode;
                    poObject.CardName = poListaClientes.Where(x => x.Codigo == poItem.CardCode).Select(x => x.Descripcion).FirstOrDefault();
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion

        #region Cliente Cambio Zona
        public List<ClienteCambioZona> goConsultarClienteCambioZona()
        {
            loBaseDa.CreateContext();

            return loBaseDa.Find<COBPCLIENTECAMBIOZONA>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new ClienteCambioZona()
            {
                IdClienteCambioZona = x.IdClienteCambioZona,
                CardCode = x.CardCode,
                CardName = x.CardName,
                CodeZona = x.CodeZona,
                NameZona = x.NameZona,
                FechaInicio = x.FechaInicio??DateTime.Now,
                FechaFin = x.FechaFin??DateTime.Now,
                CodigoEstado = x.CodigoEstado,
            }).ToList();

        }

        public string gsGuardarClienteCambioZona(List<ClienteCambioZona> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<COBPCLIENTECAMBIOZONA>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdClienteCambioZona != 0).Select(x => x.IdClienteCambioZona).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdClienteCambioZona)))
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

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdClienteCambioZona == poItem.IdClienteCambioZona && x.IdClienteCambioZona != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new COBPCLIENTECAMBIOZONA();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.CardCode = poItem.CardCode;
                    poObject.CardName = poListaClientes.Where(x => x.Codigo == poItem.CardCode).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.CodeZona = poItem.CodeZona;
                    poObject.NameZona = poListaZonasSap.Where(x => x.Codigo == poItem.CodeZona).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.FechaInicio = poItem.FechaInicio;
                    poObject.FechaFin = poItem.FechaFin;
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion

        #region Empleado Comisión
        public List<EmpleadoComision> goConsultarEmpleadoComision()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<EmpleadoComision>();
            var poListaZona = loBaseDa.Find<COBPEMPLEADOZONASAP>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();
            var poListaComision = loBaseDa.Find<COBPEMPLEADOCODIGOCOMISION>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();
            var poLista = new List<int>();

            poLista.AddRange(poListaZona.Select(x => x.IdPersona).Distinct());
            poLista.AddRange(poListaZona.Where(x=> !poLista.Contains(x.IdPersona)).Select(x => x.IdPersona).Distinct());

            foreach (var item in poLista)
            {
                var poCab = new EmpleadoComision();
                poCab.IdPersona = item;
                
                poCab.EmpleadoComisionZona = new List<EmpleadoComisionZona>();
                foreach (var detalle in poListaZona.Where(x=>x.IdPersona == item))
                {
                    var poDet = new EmpleadoComisionZona();
                    poDet.IdPersona = detalle.IdPersona;
                    poDet.CodeZona = detalle.CodeZona;
                    poDet.IdEmpleadoZonaSap = detalle.IdEmpleadoZonaSap;
                    poCab.EmpleadoComisionZona.Add(poDet);
                }
                poCab.EmpleadoComisionCodigo = new List<EmpleadoComisionCodigo>();
                foreach (var detalle in poListaComision.Where(x => x.IdPersona == item))
                {
                    var poDet = new EmpleadoComisionCodigo();
                    poDet.IdCodigoComision = detalle.IdCodigoComision;
                    poDet.IdPersona = detalle.IdPersona;
                    poDet.CodigoComision = detalle.CodigoComision;
                    poCab.EmpleadoComisionCodigo.Add(poDet);
                }

                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public string gsGuardarEmpleadoComision(List<EmpleadoComision> toLista, string tsUsuario, string tsTerminal)
        {

            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poListaZona = loBaseDa.Get<COBPEMPLEADOZONASAP>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();
            var poListaComision = loBaseDa.Get<COBPEMPLEADOCODIGOCOMISION>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdPersona != 0).Select(x => x.IdPersona).ToList();

            foreach (var poItem in poListaZona.Where(x => !piListaIdPresentacion.Contains(x.IdPersona)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;
            }

            foreach (var poItem in poListaComision.Where(x => !piListaIdPresentacion.Contains(x.IdPersona)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;
            }

            if (toLista != null && toLista.Count > 0)
            {
                var poListaPersona = goConsultarComboIdPersona();
                var poListaZonasSap = goConsultarZonasSAP();

                foreach (var poItem in toLista)
                {
                    if (poItem.EmpleadoComisionZona != null)
                    {
                        //Eliminar Detalle 
                        piListaIdPresentacion = poItem.EmpleadoComisionZona.Where(x => x.IdEmpleadoZonaSap != 0).Select(x => x.IdEmpleadoZonaSap).ToList();

                        foreach (var poItemDel in poListaZona.Where(x => x.IdPersona == poItem.IdPersona && !piListaIdPresentacion.Contains(x.IdEmpleadoZonaSap)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in poItem.EmpleadoComisionZona)
                        {
                            int pId = item.IdEmpleadoZonaSap;
                            var poObjectItem = poListaZona.Where(x => x.IdEmpleadoZonaSap == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new COBPEMPLEADOZONASAP();
                                loBaseDa.CreateNewObject(out poObjectItem);
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                            }

                            poObjectItem.CodeZona = item.CodeZona;
                            poObjectItem.CodigoEstado = Diccionario.Activo;
                            poObjectItem.NameZona = poListaZonasSap.Where(x => x.Codigo == item.CodeZona).Select(x => x.Descripcion).FirstOrDefault();
                            poObjectItem.NombreCompleto = poListaPersona.Where(x => x.Codigo == poItem.IdPersona.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                            poObjectItem.IdPersona = poItem.IdPersona;
                        }
                    }

                    if (poItem.EmpleadoComisionCodigo != null)
                    {
                        //Eliminar Detalle 
                        piListaIdPresentacion = poItem.EmpleadoComisionCodigo.Where(x => x.IdCodigoComision != 0).Select(x => x.IdCodigoComision).ToList();

                        foreach (var poItemDel in poListaComision.Where(x => x.IdPersona == poItem.IdPersona && !piListaIdPresentacion.Contains(x.IdCodigoComision)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in poItem.EmpleadoComisionCodigo)
                        {
                            int pId = item.IdCodigoComision;
                            var poObjectItem = poListaComision.Where(x => x.IdCodigoComision == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new COBPEMPLEADOCODIGOCOMISION();
                                loBaseDa.CreateNewObject(out poObjectItem);
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                            }

                            poObjectItem.CodigoComision = item.CodigoComision;
                            poObjectItem.CodigoEstado = Diccionario.Activo;
                            poObjectItem.NombreCompleto = poListaPersona.Where(x => x.Codigo == poItem.IdPersona.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                            poObjectItem.IdPersona = poItem.IdPersona;
                        }
                    }
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion

        #region Vendedor
        public List<Vendedor> goConsultarVendedor()
        {
            loBaseDa.CreateContext();

            return loBaseDa.Find<COBPVENDEDOR>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new Vendedor()
            {
                Id = x.Id,
                SlpCode = x.SlpCode,
                SlpName = x.SlpName,
                SlpCodeCurrent = x.SlpCodeCurrent,
                SlpNameCurrent = x.SlpNameCurrent,
                CodigoGrupoFactura = x.CodigoGrupoFactura,
            }).ToList();

        }

        public string gsGuardarVendedor(List<Vendedor> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<COBPVENDEDOR>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.Id  != 0).Select(x => x.Id).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.Id)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;
            }

            if (toLista != null && toLista.Count > 0)
            {
                
                var poListaVendedor = goSapConsultVendedoresTodos();
                var poListaGrupoFactura = goConsultarComboGrupoFactura();

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
                        poObject = new COBPVENDEDOR();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.SlpCode = poItem.SlpCode;
                    poObject.SlpName = poListaVendedor.Where(x => x.Codigo == poItem.SlpCode.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.SlpCodeCurrent = poItem.SlpCodeCurrent;
                    poObject.SlpNameCurrent = poListaVendedor.Where(x => x.Codigo == poItem.SlpCodeCurrent.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.CodigoGrupoFactura = poItem.CodigoGrupoFactura;
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion

        #region Porcentajes de Comisiones
        public List<PrComisiones> goConsultarPrComisiones()
        {
            loBaseDa.CreateContext();

            return loBaseDa.Find<COBPCOMISIONES>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new PrComisiones()
            {
                Id = x.Id,
                CodigoComision = x.CodigoComision,
                CodeZona = x.CodeZona,
                CodigoToleranciaContado = x.CodigoToleraciaContado,
                NameZona = x.NameZona,
                PorcentajeComision = x.PorcentajeComision,
                CodigoGrupoFactura = x.CodigoGrupoFactura,
            }).ToList();

        }

        public string gsGuardarPrComisiones(List<PrComisiones> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<COBPCOMISIONES>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

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
                var poListaZonasSap = goConsultarZonasSAP();

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
                        poObject = new COBPCOMISIONES();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.CodigoComision = poItem.CodigoComision;
                    poObject.CodeZona = poItem.CodeZona;
                    poObject.CodigoGrupoFactura = poItem.CodigoGrupoFactura;
                    poObject.CodigoToleraciaContado = poObject.CodigoToleraciaContado;
                    poObject.PorcentajeComision = poItem.PorcentajeComision;
                    poObject.NameZona = poListaZonasSap.Where(x => x.Codigo == poItem.CodeZona).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.CodigoGrupoFactura = poItem.CodigoGrupoFactura;
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion

        #region Tolerancia de Contado
        public List<ToleranciaContado> goConsultarPrToleranciaContado()
        {
            loBaseDa.CreateContext();

            return loBaseDa.Find<COBPTOLERANCIACONTADO>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new ToleranciaContado()
            {
                Hasta = x.Hasta,
                Desde = x.Desde,
                CodigoToleranciaContado = x.CodigoToleraciaContado,
                Descripcion = x.Descripcion,
            }).ToList();

        }

        public string gsGuardarToleranciaContado(List<ToleranciaContado> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<COBPTOLERANCIACONTADO>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => !string.IsNullOrEmpty(x.CodigoToleranciaContado)).Select(x => x.CodigoToleranciaContado).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.CodigoToleraciaContado)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;
            }

            if (toLista != null && toLista.Count > 0)
            {
                var poListaZonasSap = goConsultarZonasSAP();

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.CodigoToleraciaContado == poItem.CodigoToleranciaContado && !string.IsNullOrEmpty(x.CodigoToleraciaContado)).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new COBPTOLERANCIACONTADO();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.CodigoToleraciaContado = poItem.CodigoToleranciaContado;
                    poObject.Descripcion = poItem.Descripcion;
                    poObject.Desde  = poItem.Desde;
                    poObject.Hasta = poItem.Hasta;
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion

        #region Zona Detalle - Parámetros de Cobranza
        public List<ZonaGrupoDetalle> goConsultarPrCobGrupoZonaDetalle()
        {
            loBaseDa.CreateContext();

            return loBaseDa.Find<VTAPZONAGRUPODETALLE>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new ZonaGrupoDetalle()
            {
                IdZonaGrupoDetalle = x.IdZonaGrupoDetalle,
                IdZonaGrupo = x.IdZonaGrupo,
                Code = x.Code,
                Name = x.Name,
                Agrupacion = x.Agrupacion,
                ResponsableCobranza = x.ResponsableCobranza,
                CodAgrupacion = x.CodAgrupacion,
                CodResponsableCobranza = x.CodResponsableCobranza,
                DiasProAcepIni = x.DiasProAcepIni ?? 0,
                DiasProAcepFin = x.DiasProAcepFin ?? 0,
                DiasProGestIni = x.DiasProGestIni ?? 0,
                DiasProGestFin = x.DiasProGestFin ?? 0,
                DiasProNoAcepIni = x.DiasProNoAcepIni ?? 0,
                DiasProNoAcepFin = x.DiasProNoAcepFin ?? 0,
                CodVendedor = x.CodVendedor,
                NomVendedor = x.NomVendedor,
                Titular = x.Titular,
                Recaudador = x.Recaudador,
                CodTitular = x.CodTitular,
                CodRecaudador = x.CodRecaudador,
            }).ToList();

        }

        public string gsGuardarPrCobGrupoZonaDetalle(List<ZonaGrupoDetalle> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<VTAPZONAGRUPODETALLE>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdZonaGrupoDetalle != 0).Select(x => x.IdZonaGrupoDetalle).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdZonaGrupoDetalle)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;
            }

            if (toLista != null && toLista.Count > 0)
            {

                var poListaVendedor = goSapConsultVendedoresTodos();
                var poListaZona = goConsultarZonasSAP();
                var poListaTitular = goSapConsultTitularesTodos();
                var poListaAgrupacion = goConsultarComboAgrupacionCobranza();
                var poListaResponsable = goConsultarComboResponsableCobranza();

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdZonaGrupoDetalle == poItem.IdZonaGrupoDetalle && x.IdZonaGrupoDetalle != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new VTAPZONAGRUPODETALLE();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.IdZonaGrupoDetalle = poItem.IdZonaGrupoDetalle;
                    poObject.IdZonaGrupo = poItem.IdZonaGrupo;
                    poObject.Code = poItem.Code;
                    poObject.Name = poListaZona.Where(x => x.Codigo == poItem.Code).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.CodAgrupacion = poItem.CodAgrupacion;
                    poObject.Agrupacion = poListaAgrupacion.Where(x => x.Codigo == poItem.CodAgrupacion).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.CodResponsableCobranza = poItem.CodResponsableCobranza;
                    poObject.ResponsableCobranza = poListaResponsable.Where(x => x.Codigo == poItem.CodResponsableCobranza).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.DiasProAcepIni = poItem.DiasProAcepIni;
                    poObject.DiasProAcepFin = poItem.DiasProAcepFin;
                    poObject.DiasProGestIni = poItem.DiasProGestIni;
                    poObject.DiasProGestFin = poItem.DiasProGestFin;
                    poObject.DiasProNoAcepIni = poItem.DiasProNoAcepIni;
                    poObject.DiasProNoAcepFin = poItem.DiasProNoAcepFin;
                    poObject.CodVendedor = poItem.CodVendedor;
                    poObject.NomVendedor = poItem.CodVendedor == null ? null : poListaVendedor.Where(x => x.Codigo == poItem.CodVendedor.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.CodTitular = poItem.CodTitular;
                    poObject.Titular = poItem.CodTitular == null ? null : poListaTitular.Where(x => x.Codigo == poItem.CodTitular.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                    poObject.CodRecaudador = poItem.CodRecaudador;
                    poObject.Recaudador = poItem.CodRecaudador == null ? null : poListaVendedor.Where(x => x.Codigo == poItem.CodRecaudador.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion

        #region No Aplica Factura Comision
        public List<FacturaComision> goConsultarNoAplicaFacturaComision()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<FacturaComision>();
            var poLista = loBaseDa.Find<COBPNOAPLICAFACTURACOMISION>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            foreach (var item in poLista)
            {
                var poCab = new FacturaComision();
                poCab.IdFacturaComision = item.IdNoAplicaFacturaComision;
                poCab.NumFactura = item.NumFactura;
                poCab.CardCode = item.CardCode;
                poCab.CardName = item.CardName;
                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public string gsGuardarNoAplicaFacturaComision(List<FacturaComision> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<COBPNOAPLICAFACTURACOMISION>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdFacturaComision != 0).Select(x => x.IdFacturaComision).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdNoAplicaFacturaComision)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;
            }

            if (toLista != null && toLista.Count > 0)
            {
                var poListaClientes = goSapConsultaClientes();
                var poListaZonasSap = goConsultarZonasSAP();

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdNoAplicaFacturaComision == poItem.IdFacturaComision && x.IdNoAplicaFacturaComision != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new COBPNOAPLICAFACTURACOMISION();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.NumFactura = poItem.NumFactura;
                    poObject.CardCode = poItem.CardCode;
                    poObject.CardName = poListaClientes.Where(x => x.Codigo == poItem.CardCode).Select(x => x.Descripcion).FirstOrDefault();

                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion

        #region No Num Doc Pago Comision
        public List<NoAplicaNumDocPagoComision> goConsultarNoAplicaNumDocPagoComision()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<NoAplicaNumDocPagoComision>();
            var poLista = loBaseDa.Find<COBPNOAPLICANUMDOCPAGOCOMISION>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            foreach (var item in poLista)
            {
                var poCab = new NoAplicaNumDocPagoComision();
                poCab.IdNoAplicaNumDocPagoComision = item.IdNoAplicaNumDocPagoComision;
                poCab.NumDocPago = item.NumDocPago;
                poCab.Considerar = item.Considerar;
                poCab.ConsiderarCobranza = item.ConsiderarCobranza;
                poCab.CardCode = item.CardCode;
                poCab.CardName = item.CardName;
                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public string gsGuardarNoAplicaNumDocPagoComision(List<NoAplicaNumDocPagoComision> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<COBPNOAPLICANUMDOCPAGOCOMISION>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdNoAplicaNumDocPagoComision != 0).Select(x => x.IdNoAplicaNumDocPagoComision).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdNoAplicaNumDocPagoComision)))
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
                    var poObject = poLista.Where(x => x.IdNoAplicaNumDocPagoComision == poItem.IdNoAplicaNumDocPagoComision && x.IdNoAplicaNumDocPagoComision != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new COBPNOAPLICANUMDOCPAGOCOMISION();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.NumDocPago = poItem.NumDocPago;
                    poObject.Considerar = poItem.Considerar;
                    poObject.ConsiderarCobranza = poItem.ConsiderarCobranza;
                    poObject.CardCode = poItem.CardCode;
                    poObject.CardName = poListaClientes.Where(x => x.Codigo == poItem.CardCode).Select(x => x.Descripcion).FirstOrDefault();

                }
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }
        #endregion


        #region Dia no laborable
        public List<DiaNoLaborable> goConsultarDiaNoLaborable()
        {
            loBaseDa.CreateContext();
            var poListaReturn = new List<DiaNoLaborable>();
            var poLista = loBaseDa.Find<COBPDIANOLABORABLE>().Include(x => x.COBPDIANOLABORABLEZONA).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            foreach (var item in poLista)
            {
                var poCab = new DiaNoLaborable();
                poCab.IdDiaNoLaborable = item.IdDiaNoLaborable;
                poCab.CodigoEstado = item.CodigoEstado;
                poCab.Fecha = item.Fecha;
                poCab.Descripcion = item.Descripcion;

                poCab.DiaNoLaborableZona = new List<DiaNoLaborableZona>();
                foreach (var detalle in item.COBPDIANOLABORABLEZONA.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    var poDet = new DiaNoLaborableZona();

                    poDet.CodeZona = detalle.CodeZona;
                    poDet.NameZona = detalle.NameZona;
                    poDet.IdDiaNoLaborable = detalle.IdDiaNoLaborable;
                    poDet.IdDiaNoLaborableZona = detalle.IdDiaNoLaborableZona;
                    poCab.DiaNoLaborableZona.Add(poDet);
                }
                poListaReturn.Add(poCab);
            }
            return poListaReturn;
        }

        public string gsGuardarDiaNoLaborable(List<DiaNoLaborable> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poLista = loBaseDa.Get<COBPDIANOLABORABLE>().Include(X => X.COBPDIANOLABORABLEZONA).Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            var piListaIdPresentacion = toLista.Where(x => x.IdDiaNoLaborable != 0).Select(x => x.IdDiaNoLaborable).ToList();

            foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdDiaNoLaborable)))
            {
                poItem.CodigoEstado = Diccionario.Inactivo;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.FechaModificacion = DateTime.Now;
                poItem.TerminalModificacion = tsTerminal;

                foreach (var item in poItem.COBPDIANOLABORABLEZONA.Where(X => X.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Inactivo;
                    item.UsuarioModificacion = tsUsuario;
                    item.FechaModificacion = DateTime.Now;
                    item.TerminalModificacion = tsTerminal;

                }
            }

            if (toLista != null && toLista.Count > 0)
            {
                var poListaZonasSap = goConsultarZonasSAP();

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdDiaNoLaborable == poItem.IdDiaNoLaborable && x.IdDiaNoLaborable != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new COBPDIANOLABORABLE();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Fecha = poItem.Fecha;
                    poObject.Descripcion = poItem.Descripcion;

                    if (poItem.DiaNoLaborableZona != null)
                    {
                        //Eliminar Detalle 
                        piListaIdPresentacion = poItem.DiaNoLaborableZona.Where(x => x.IdDiaNoLaborableZona != 0).Select(x => x.IdDiaNoLaborableZona).ToList();

                        foreach (var poItemDel in poObject.COBPDIANOLABORABLEZONA.Where(x => x.CodigoEstado == Diccionario.Activo && !piListaIdPresentacion.Contains(x.IdDiaNoLaborableZona)))
                        {
                            poItemDel.CodigoEstado = Diccionario.Inactivo;
                            poItemDel.UsuarioModificacion = tsUsuario;
                            poItemDel.FechaModificacion = DateTime.Now;
                            poItemDel.TerminalModificacion = tsTerminal;
                        }

                        foreach (var item in poItem.DiaNoLaborableZona)
                        {
                            int pId = item.IdDiaNoLaborableZona;
                            var poObjectItem = poObject.COBPDIANOLABORABLEZONA.Where(x => x.IdDiaNoLaborableZona == pId && pId != 0).FirstOrDefault();
                            if (poObjectItem != null)
                            {
                                poObjectItem.UsuarioModificacion = tsUsuario;
                                poObjectItem.FechaModificacion = DateTime.Now;
                                poObjectItem.TerminalModificacion = tsTerminal;
                            }
                            else
                            {
                                poObjectItem = new COBPDIANOLABORABLEZONA();
                                poObjectItem.UsuarioIngreso = tsUsuario;
                                poObjectItem.FechaIngreso = DateTime.Now;
                                poObjectItem.TerminalIngreso = tsTerminal;
                                poObject.COBPDIANOLABORABLEZONA.Add(poObjectItem);
                            }

                            poObjectItem.CodeZona = item.CodeZona;
                            poObjectItem.CodigoEstado = Diccionario.Activo;
                            poObjectItem.NameZona = poListaZonasSap.Where(x => x.Codigo == item.CodeZona).Select(x => x.Descripcion).FirstOrDefault();
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
