using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 15/10/2021
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNMovimientos : clsNBase
    {
        #region Movimientos
        /// <summary>
        /// Guardar Movimientos
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsGuardarMovimientos(string tsCodigoMovimiento, List<MovimientosGrid> toLista, string tsUsuario, string tsTerminal, List<int> tListaId, bool tbCrearContexto = true)
        {
            string psMsg = string.Empty;
            if (tbCrearContexto)
            {
                loBaseDa.CreateContext();
            }

            if (toLista != null && toLista.Count > 0)
            {

                var poListaEmpleado = loBaseDa.Find<REHVTEMPLEADOS>().ToList();
                var poListaEmpleadoActivo = loBaseDa.Find<REHVTPERSONACONTRATO>().ToList();

                var poLista = new List<REHTMOVIMIENTO>();
                if (tListaId.Count == 0)
                {
                    poLista = loBaseDa.Get<REHTMOVIMIENTO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoTipoMovimiento == tsCodigoMovimiento && x.IdNomina == null).ToList();
                }
                else
                {
                    poLista = loBaseDa.Get<REHTMOVIMIENTO>().Where(x => tListaId.Contains(x.IdMovimiento) && x.CodigoEstado == Diccionario.Activo).ToList();
                }
                //var poLista = loBaseDa.Get<REHTMOVIMIENTO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoTipoMovimiento == tsCodigoMovimiento && x.IdNomina == null).ToList();
                var poListaTipoMotivo = goConsultarCatalogo(Diccionario.ListaCatalogo.TipoMovimiento);

                var piListaId = toLista.Where(x => x.IdMovimiento != 0).Select(x => x.IdMovimiento).ToList();

                foreach (var poItem in poLista.Where(x => !piListaId.Contains(x.IdMovimiento)))
                {
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }
                int pi = 0;

                foreach (var poItem in toLista)
                {
                    if (poListaEmpleadoActivo.Where(x => x.NumeroIdentificacion == poItem.Cedula).FirstOrDefault() == null && poListaEmpleado.Where(x => x.NumeroIdentificacion == poItem.Cedula).FirstOrDefault() == null)
                    {
                        psMsg = psMsg + "Empleado con Cédula: "+ poItem.Cedula +" y nombre: "+ poItem.Empleado + " No existe en el sistema";
                    }
                    else
                    {
                        var poObject = poLista.Where(x => x.IdMovimiento == poItem.IdMovimiento && x.IdMovimiento != 0).FirstOrDefault();
                        if (poObject != null)
                        {
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObject = new REHTMOVIMIENTO();
                            loBaseDa.CreateNewObject(out poObject);
                            poObject.UsuarioIngreso = tsUsuario;
                            poObject.FechaIngreso = DateTime.Now;
                            poObject.TerminalIngreso = tsTerminal;
                            poObject.Fecha = DateTime.Now;
                        }

                        poObject.Anio = poItem.Anio == 0 ? poItem.FechaIngreso.Year : poItem.Anio;
                        poObject.CodigoEstado = Diccionario.Activo;
                        poObject.CodigoTipoMovimiento = poItem.CodigoTipoMovimiento;

                        poObject.IdEmpleadoContrato = poListaEmpleadoActivo.Where(x => x.NumeroIdentificacion == poItem.Cedula).FirstOrDefault() == null ? poListaEmpleado.Where(x => x.NumeroIdentificacion == poItem.Cedula).LastOrDefault().IdEmpleadoContrato : poListaEmpleadoActivo.Where(x => x.NumeroIdentificacion == poItem.Cedula).FirstOrDefault().IdEmpleadoContrato;
                        poObject.IdPersona = poListaEmpleado.Where(x => x.NumeroIdentificacion == poItem.Cedula).LastOrDefault().IdPersona;
                        poObject.Mes = poItem.Mes == 0 ? poItem.FechaIngreso.Month : poItem.Mes;
                        poObject.NombreCompleto = poListaEmpleado.Where(x => x.NumeroIdentificacion == poItem.Cedula).LastOrDefault().NombreCompleto;
                        poObject.NumeroIdentificacion = poItem.Cedula;
                        poObject.Observacion = poItem.Observacion;
                        poObject.TipoMovimiento = poListaTipoMotivo.Where(x => x.Codigo == poItem.CodigoTipoMovimiento).Select(x => x.Descripcion).FirstOrDefault();
                        //poObject.CodigoRubro = poItem.CodigoRubro;
                        //poObject.Rubro = poItem.Rubro;
                        //poObject.CodigoTipoPrestamo = poItem.CodigoTipoPrestamo;
                        //poObject.TipoPrestamo = poItem.TipoPrestamo;
                        if (poItem.IdReferencial != 0)
                        {
                            poObject.IdReferecial = poItem.IdReferencial;
                        }

                        poObject.Valor = poItem.Valor;

                    }
                }

                if (string.IsNullOrEmpty(psMsg))
                {
                    loBaseDa.SaveChanges();
                }
                
            }

            return psMsg;
        }

        /// <summary>
        /// Consultar Movimientos
        /// </summary>
        /// <returns></returns>
        public List<MovimientosGrid> goConsultarMovimientos(string tsCodigoTipoMovimiento)
        {
            // && (tiPeriodo != 0 ? X.Periodo == tiPeriodo : true)
            return loBaseDa.Find<REHTMOVIMIENTO>().Where(X => X.CodigoEstado == Diccionario.Activo && X.CodigoTipoMovimiento == tsCodigoTipoMovimiento)
                .Select(x => new MovimientosGrid()
                {
                    IdMovimiento = x.IdMovimiento,
                    //Anio = x.Anio,
                    Cedula = x.NumeroIdentificacion,
                    CodigoTipoMovimiento = x.CodigoTipoMovimiento,
                    Empleado = x.NombreCompleto,
                    //Mes = x.Mes,
                    Valor = x.Valor,
                    Observacion = x.Observacion,
                    FechaIngreso = x.Fecha,
                    IdNomina = x.IdNomina ?? 0

                }).ToList().OrderBy(x => x.Empleado).ToList();
        }

        public List<SpConsultaMovimientosResumen> goConsultarMovimientoResumen(string tsCodigoTipoMovimiento = "")
        {
            if (!string.IsNullOrEmpty(tsCodigoTipoMovimiento))
            {
                return loBaseDa.ExecStoreProcedure<SpConsultaMovimientosResumen>("REHSPCONSULTAMOVIMIENTOSRESUMEN @TipoMovimiento ", new SqlParameter("@TipoMovimiento", tsCodigoTipoMovimiento));
            }
            else
            {
                return loBaseDa.ExecStoreProcedure<SpConsultaMovimientosResumen>("REHSPCONSULTAMOVIMIENTOSRESUMEN ");
            }
        }

        public DataTable gdtConsultarMovimientoResumen(string tsCodigoTipoMovimiento = "")
        {

            if (!string.IsNullOrEmpty(tsCodigoTipoMovimiento))
            {
                return loBaseDa.DataTable(string.Format("EXEC REHSPCONSULTAMOVIMIENTOSRESUMEN '{0}'", tsCodigoTipoMovimiento));
            }
            else
            {
                return loBaseDa.DataTable(string.Format("EXEC REHSPCONSULTAMOVIMIENTOSRESUMEN"));
            }
        }

        public string tsTituloReporte()
        {
            string psMsg = "";
            var poNomina = loBaseDa.Find<REHTNOMINA>().Where(x => x.CodigoEstado == Diccionario.Pendiente).Select(x => new { x.IdPeriodo, x.Anio, x.Mes, x.CodigoTipoRol }).FirstOrDefault();
            if (poNomina != null)
            {
                var psTipoRol = loBaseDa.Find<REHMTIPOROL>().Where(x => x.CodigoTipoRol == poNomina.CodigoTipoRol).Select(x => x.Descripcion).FirstOrDefault();
                psMsg = string.Format("SALDO DE MOVIMIENTOS. CORTE A {0} DEL {1}-{2}", psTipoRol, poNomina.Mes, poNomina.Anio);
            }
            else
            {
                psMsg = string.Format("SALDO DE MOVIMIENTOS. CORTE A {0}", DateTime.Now.Date.ToString("dd/MM/yyyyy"));
            }

            return psMsg;
        }

        #endregion

        #region Préstamos Internos
        /// <summary>
        /// Consulta capacidad de endeudamiento
        /// </summary>
        /// <param name="tIdPersona"></param>
        /// <param name="tdFechaIngreso"></param>
        /// <returns></returns>
        public SpCapacidadEndeudamiento goCapacidadEndeudamiento(int tIdPersona, out DateTime tdFechaIngreso)
        {
            tdFechaIngreso = loBaseDa.Find<REHVTPERSONACONTRATO>().Where(x => x.IdPersona == tIdPersona).Select(x => x.FechaInicioContrato).FirstOrDefault();
            return loBaseDa.ExecStoreProcedure<SpCapacidadEndeudamiento>("REHSPCAPACIDADENDEUDAMIENTO @IdPersona ", new SqlParameter("@IdPersona", tIdPersona)).FirstOrDefault();
        }

        public string gsGuardarPrestamoInterno(PrestamoInterno toObject, string tsTipo, string tsUsuario, string tsTerminal, out int tId)
        {
            tId = 0;
            string psMsg = lsValidaPrevioSave(toObject, tsTipo);
            
            if (string.IsNullOrEmpty(psMsg))
            {
                loBaseDa.CreateContext();

                var poObjectCat = loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo == Diccionario.ListaCatalogo.TipoPrestamoAnticipoDescuento && x.Codigo == toObject.CodigoTipoPrestamo).Select(x => new { x.CodigoAlterno1, x.Descripcion }).FirstOrDefault();
                var psCodigoRubro = poObjectCat.CodigoAlterno1;
                var psTipoPrestamo = poObjectCat.Descripcion;

                if (string.IsNullOrEmpty(psCodigoRubro))
                {
                    throw new Exception("Tipo no tiene configurado Rubro para descuento");
                }

                var psRubro = loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoRubro ==psCodigoRubro).Select(x => x.Descripcion).FirstOrDefault();

                var poEmpleado = loBaseDa.Find<REHVTPERSONACONTRATO>().Where(x => x.IdPersona == toObject.IdPersona).Select(x => new { x.Sucursal, x.Departamento }).FirstOrDefault();
                int pId = toObject.IdPrestamoInterno;
                var poObject = loBaseDa.Get<REHTPRESTAMOINTERNO>().Include(x => x.REHTPRESTAMOINTERNODETALLE).Where(x => x.IdPrestamoInterno == pId).FirstOrDefault();

                if (poObject != null)
                {
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObject = new REHTPRESTAMOINTERNO();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.IdEmpleadoContrato = loBaseDa.Find<REHVTPERSONACONTRATO>().Where(x => x.IdPersona == toObject.IdPersona).Select(x => x.IdEmpleadoContrato).FirstOrDefault();
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = tsTerminal;
                    poObject.SueldoFijo = loBaseDa.Find<REHVTPERSONACONTRATO>().Where(x => x.IdPersona == toObject.IdPersona).Select(x => x.Sueldo).FirstOrDefault();
                }

                poObject.CodigoEstado = Diccionario.Activo;
                poObject.FechaSolicitud = toObject.FechaSolicitud;
                poObject.IdPersona = toObject.IdPersona;
                poObject.FechaInicioContrato = toObject.FechaInicioContrato;
                poObject.Ciudad = toObject.Ciudad;
                poObject.Ingresos = toObject.Ingresos;
                poObject.Egresos = toObject.Egresos;
                poObject.CapacidadEndeudamiento = toObject.CapacidadEndeudamiento;
                poObject.ValorPrestamo = toObject.ValorCredito;
                poObject.Plazo = toObject.Plazo;
                poObject.CodigoMotivoPrestamoInterno = toObject.CodigoMotivoPrestamoInterno;
                poObject.CodigoTipoPrestamo = toObject.CodigoTipoPrestamo;
                poObject.Observacion = toObject.Observacion;
                poObject.FechaInicioPago = toObject.FechaInicioPago;
                poObject.Sucursal = poEmpleado.Sucursal;
                poObject.Departamento = poEmpleado.Departamento;
                poObject.CodigoRubro = psCodigoRubro;
                //poObject.Rubro = psRubro;

                /////////////////////////////////////////////////////////////////////////////////////////////////
                List<MovimientosGrid> movimientos = new List<MovimientosGrid>();
                var poPersona = loBaseDa.Find<GENMPERSONA>().Where(x => x.IdPersona == poObject.IdPersona).Select(x => new { x.NumeroIdentificacion, x.NombreCompleto }).FirstOrDefault();
                ////////////////////////////////////////////////////////////////////////////////////////////////////

                if (toObject.PrestamoInternoDetalle != null)
                {
                    List<int> poListaIdPe = toObject.PrestamoInternoDetalle.Select(x => x.IdPrestamoInternoDetalle).ToList();
                    List<int> piListaEliminar = poObject.REHTPRESTAMOINTERNODETALLE.Where(x => x.CodigoEstado != Diccionario.Eliminado && !poListaIdPe.Contains(x.IdPrestamoInternoDetalle)).Select(x => x.IdPrestamoInternoDetalle).ToList();
                    foreach (var poItem in poObject.REHTPRESTAMOINTERNODETALLE.Where(x => piListaEliminar.Contains(x.IdPrestamoInternoDetalle)))
                    {
                        poItem.CodigoEstado = Diccionario.Eliminado;
                        poItem.UsuarioModificacion = tsUsuario;
                        poItem.FechaModificacion = DateTime.Now;
                        poItem.TerminalModificacion = tsTerminal;

                        var poListaMovimientos = loBaseDa.Get<REHTMOVIMIENTO>().Where(x => x.IdNomina == poItem.IdPrestamoInternoDetalle).FirstOrDefault();
                        if (poListaMovimientos != null)
                        {
                            poListaMovimientos.CodigoEstado = Diccionario.Eliminado;
                            poListaMovimientos.UsuarioModificacion = tsUsuario;
                            poListaMovimientos.FechaModificacion = DateTime.Now;
                            poListaMovimientos.TerminalModificacion = tsTerminal;
                        }

                    }

                    foreach (var item in toObject.PrestamoInternoDetalle)
                    {
                        int pIdDetalle = item.IdPrestamoInternoDetalle;
                        var poObjectDetalle = poObject.REHTPRESTAMOINTERNODETALLE.Where(y => y.IdPrestamoInternoDetalle == pIdDetalle && pIdDetalle != 0).FirstOrDefault();
                        if (poObjectDetalle != null)
                        {
                            //Actualizar
                            poObjectDetalle.UsuarioModificacion = tsUsuario;
                            poObjectDetalle.FechaModificacion = DateTime.Now;
                            poObjectDetalle.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            //Crear
                            poObjectDetalle = new REHTPRESTAMOINTERNODETALLE();
                            poObject.REHTPRESTAMOINTERNODETALLE.Add(poObjectDetalle);
                            poObjectDetalle.UsuarioIngreso = tsUsuario;
                            poObjectDetalle.FechaIngreso = DateTime.Now;
                            poObjectDetalle.TerminalIngreso = tsTerminal;
                        }

                        if (poObjectDetalle.CodigoEstado == Diccionario.Cerrado && (poObjectDetalle.Anio != item.Anio || poObjectDetalle.Mes != item.Mes || 
                            poObjectDetalle.CodigoTipoRol != item.CodigoTipoRol || poObjectDetalle.Valor != item.Valor))
                        {
                            psMsg = string.Format("La Cuota {0} no puede ser editada, ya está cerrada!", item.Cuota);
                            return psMsg;
                        }
                        poObjectDetalle.CodigoEstado = item.CodigoEstado;
                        poObjectDetalle.Cuota = item.Cuota;
                        poObjectDetalle.Anio = item.Anio;
                        poObjectDetalle.Mes = item.Mes;
                        poObjectDetalle.CodigoTipoRol = item.CodigoTipoRol;
                        poObjectDetalle.CodigoPeriodo = item.CodigoPeriodo;
                        poObjectDetalle.Valor = item.Valor;
                        poObjectDetalle.CodigoEstadoCuota = "";
                        poObjectDetalle.Observacion = item.Observacion;
                        //poObjectDetalle.CodigoRubro = psCodigoRubro;

                    }
                }
                
                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();
                    List<int> pListaId = new List<int>();
                    foreach (var item in poObject.REHTPRESTAMOINTERNODETALLE.Where(x=>x.CodigoEstado != Diccionario.Eliminado))
                    {
                        var mov = new MovimientosGrid();
                        mov.IdMovimiento = loBaseDa.Find<REHTMOVIMIENTO>().Where(x => x.IdReferecial == item.IdPrestamoInternoDetalle && x.CodigoEstado == Diccionario.Activo).Select(x => x.IdMovimiento).FirstOrDefault();
                        mov.Cedula = poPersona.NumeroIdentificacion;
                        mov.CodigoTipoMovimiento = tsTipo;
                        mov.Empleado = poPersona.NombreCompleto;
                        mov.Valor = item.Valor;
                        mov.Observacion = psTipoPrestamo;
                        mov.FechaIngreso = DateTime.Now;
                        mov.Anio = item.Anio;
                        mov.Mes = item.Mes;
                        mov.IdReferencial = item.IdPrestamoInternoDetalle;
                        mov.CodigoRubro = psCodigoRubro;
                        mov.Rubro = psRubro;
                        mov.CodigoTipoPrestamo = toObject.CodigoTipoPrestamo;
                        mov.TipoPrestamo = psTipoPrestamo;
                        movimientos.Add(mov);

                        pListaId.Add(mov.IdMovimiento);
                    }

                    gsGuardarMovimientos(tsTipo, movimientos, tsUsuario, tsTerminal, pListaId, false);
                    poTran.Complete();
                }

                    
                tId = poObject.IdPrestamoInterno;
            }
            
            return psMsg;
        }

        private string lsValidaPrevioSave(PrestamoInterno toObject, string tsTipo)
        {
            string psResult = string.Empty;

            if (toObject.CodigoTipoPrestamo == Diccionario.Seleccione)
            {
                psResult = psResult + "Seleccione el Tipo. \n";
            }

            if (toObject.IdPersona.ToString() == Diccionario.Seleccione)
            {
                psResult = psResult + "Seleccione el Empleado. \n";
            }

            if (toObject.ValorCredito <= 0M)
            {
                psResult = psResult + "El valor del Crédito debe ser mayor a 0. \n";
            }

            if (tsTipo == Diccionario.ListaCatalogo.TipoMovimientoClass.DescuentosProgramados)
            {
                toObject.CodigoMotivoPrestamoInterno = "";
            }
            else
            {
                if (toObject.CodigoMotivoPrestamoInterno == Diccionario.Seleccione)
                {
                    psResult = psResult + "Seleccione el Motivo. \n";
                }
            }

            if (toObject.Plazo <= 0)
            {
                psResult = psResult + "El Plazo debe ser mayor a 0. \n";
            }


            if (toObject.PrestamoInternoDetalle == null || toObject.PrestamoInternoDetalle.Count() == 0)
            {
                psResult = psResult + "Calcule o Ingrese la tabla de amortización. \n";
            }
            else
            {
                if (toObject.ValorCredito != toObject.PrestamoInternoDetalle.Sum(x=>x.Valor))
                {
                    psResult = string.Format("{0}La suma del valor de las cuotas no coincide con el monto del préstamo. \n", psResult);
                }

                var poPeriodos = loBaseDa.Find<REHPPERIODO>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

                var poListaAmu = new List<PrestamoInternoDetalle>();
                var poListaMsg = new List<PrestamoInternoDetalle>();

                int fila = 0;
                foreach (var detalle in toObject.PrestamoInternoDetalle)
                {
                    fila++;

                    if (poListaAmu.Where(x=>x.Anio == detalle.Anio && x.Mes == detalle.Mes && x.CodigoTipoRol == detalle.CodigoTipoRol).Count() > 0)
                    {
                        poListaMsg.Add(detalle);
                    }
                    poListaAmu.Add(detalle);

                    if (detalle.Anio <= 2021)
                    {
                        psResult = string.Format("{0}En la cuota {1} el Año debe ser mayor al 2021. \n", psResult, detalle.Cuota);
                    }

                    if (detalle.Mes > 12 || detalle.Mes < 0)
                    {
                        psResult = string.Format("{0}En la cuota {1} el Mes debe ser entre 1 al 12. \n", psResult, detalle.Cuota);
                    }

                    var psCodigoEstadoNomina = poPeriodos.Where(x => x.Anio == detalle.Anio && x.FechaFin.Month == detalle.Mes && x.CodigoTipoRol == detalle.CodigoTipoRol)
                        .Select(x => x.CodigoEstadoNomina).FirstOrDefault();

                    if (psCodigoEstadoNomina == Diccionario.Cerrado)
                    {
                        psResult = string.Format("{0}En la cuota {1} el Rol ya está cerrado, cámbielo por favor. \n", psResult, detalle.Cuota);
                    }
                    //else if (string.IsNullOrEmpty(psCodigoEstadoNomina))
                    //{
                    //    psResult = string.Format("{0}En la cuota {1} el Rol no existe en el Sistema, cámbielo por favor. \n", psResult, detalle.Cuota);
                    //}
                    
                    if (detalle.Valor <= 0)
                    {
                        psResult = string.Format("{0}En la cuota {1} el Valor debe mayor a $0.00. \n", psResult, detalle.Cuota);
                    }

                }

                foreach (var detalle in poListaMsg)
                {
                    psResult = string.Format("{0}La cuota {1} está duplicada. Corregir! \n", psResult, detalle.Cuota);
                }

            }


            return psResult;
        }

        public List<PrestamoInterno> goListarPrestamos(string tsTipo)
        {
            return (from a in loBaseDa.Find<REHTPRESTAMOINTERNO>()
                    join b in loBaseDa.Find<GENMPERSONA>() on a.IdPersona equals b.IdPersona
                    join c in loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo == Diccionario.ListaCatalogo.TipoPrestamoAnticipoDescuento && x.CodigoEstado == Diccionario.Activo) on a.CodigoTipoPrestamo equals c.Codigo
                    join d in loBaseDa.Find<REHPRUBRO>() on c.CodigoAlterno1 equals d.CodigoRubro
                    where a.CodigoEstado == Diccionario.Activo
                    && d.CodigoTipoMovimiento == tsTipo
                    select new PrestamoInterno()
                    {
                        IdPrestamoInterno = a.IdPrestamoInterno,
                        DesPersona = b.NombreCompleto,
                        FechaSolicitud = a.FechaSolicitud,
                        ValorCredito = a.ValorPrestamo,
                        Observacion = a.Observacion,
                        CodigoEstado = a.CodigoEstado,
                        
                    }).ToList();
        }

        public PrestamoInterno goBuscarEntidadPrestamo(int tId)
        {
            PrestamoInterno poObject = new PrestamoInterno();
            var poResult = loBaseDa.Find<REHTPRESTAMOINTERNO>()
                .Include(x => x.REHTPRESTAMOINTERNODETALLE)
            .Where(x => x.IdPrestamoInterno == tId).FirstOrDefault();

            if (poResult != null)
            {
                poObject.CapacidadEndeudamiento = poResult.CapacidadEndeudamiento;
                poObject.Ciudad = poResult.Ciudad;
                poObject.CodigoEstado = poResult.CodigoEstado;
                poObject.CodigoMotivoPrestamoInterno = poResult.CodigoMotivoPrestamoInterno;
                poObject.CodigoTipoPrestamo = poResult.CodigoTipoPrestamo;
                poObject.Egresos = poResult.Egresos;
                poObject.FechaAprobacion = poResult.FechaAprobacion;
                poObject.FechaIngreso = poResult.FechaIngreso;
                poObject.FechaInicioContrato = poResult.FechaInicioContrato;
                poObject.FechaInicioPago = poResult.FechaInicioPago;
                poObject.FechaModificacion = poResult.FechaModificacion;
                poObject.FechaSolicitud = poResult.FechaSolicitud;
                poObject.IdPersona = poResult.IdPersona;
                poObject.IdPrestamoInterno = poResult.IdPrestamoInterno;
                poObject.Ingresos = poResult.Ingresos;
                poObject.Observacion = poResult.Observacion;
                poObject.Plazo = poResult.Plazo;
                poObject.TerminalIngreso = poResult.TerminalIngreso;
                poObject.TerminalModificacion = poResult.TerminalModificacion;
                poObject.UsuarioAprobacion = poResult.UsuarioAprobacion;
                poObject.UsuarioIngreso = poResult.UsuarioIngreso;
                poObject.UsuarioModificacion = poResult.UsuarioModificacion;
                poObject.ValorCredito = poResult.ValorPrestamo;
                

                poObject.PrestamoInternoDetalle = new List<PrestamoInternoDetalle>();
                foreach (var poItem in poResult.REHTPRESTAMOINTERNODETALLE.Where(x => x.CodigoEstado !=  Diccionario.Eliminado))
                {
                    var poObjectItem = new PrestamoInternoDetalle();
                    poObjectItem.Anio = poItem.Anio;
                    poObjectItem.CodigoEstado = poItem.CodigoEstado;
                    poObjectItem.CodigoEstadoCuota = poItem.CodigoEstadoCuota;
                    poObjectItem.CodigoTipoRol = poItem.CodigoTipoRol;
                    poObjectItem.Cuota = poItem.Cuota;
                    poObjectItem.FechaIngreso = poItem.FechaIngreso;
                    poObjectItem.FechaModificacion = poItem.FechaModificacion;
                    poObjectItem.IdPrestamoInterno = poItem.IdPrestamoInterno;
                    poObjectItem.IdPrestamoInternoDetalle = poItem.IdPrestamoInternoDetalle;
                    poObjectItem.Mes = poItem.Mes;
                    poObjectItem.Observacion = poItem.Observacion;
                    poObjectItem.TerminalIngreso = poItem.TerminalIngreso;
                    poObjectItem.TerminalModificacion = poItem.TerminalModificacion;
                    poObjectItem.UsuarioIngreso = poItem.UsuarioIngreso;
                    poObjectItem.UsuarioModificacion = poItem.UsuarioModificacion;
                    poObjectItem.Valor = poItem.Valor;
                    poObjectItem.CodigoPeriodo = poItem.CodigoPeriodo;

                    poObject.PrestamoInternoDetalle.Add(poObjectItem);
                }
            }

            return poObject;
        }

        /// <summary>
        /// Elimina Registros de Prestamo Interno
        /// </summary>
        public string gEliminarMaestro(int tId, string tsUsuario, string tsTerminal)
        {
            string Msg = "";
            var poObject = loBaseDa.Get<REHTPRESTAMOINTERNO>().Include(x=>x.REHTPRESTAMOINTERNODETALLE).Where(x => x.IdPrestamoInterno == tId).FirstOrDefault();

            
            if (poObject != null)
            {
                if (poObject.REHTPRESTAMOINTERNODETALLE.Where(x => x.CodigoEstado == Diccionario.Cerrado).Count() == 0)
                {
                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                    

                    foreach (var item in poObject.REHTPRESTAMOINTERNODETALLE.Where(x=>x.CodigoEstado == Diccionario.Activo))
                    {
                        item.CodigoEstado = Diccionario.Eliminado;
                        item.FechaIngreso = DateTime.Now;
                        item.UsuarioModificacion = tsUsuario;
                        item.TerminalModificacion = tsTerminal;

                        var mov = loBaseDa.Get<REHTMOVIMIENTO>().Where(x => x.IdReferecial == item.IdPrestamoInternoDetalle).FirstOrDefault();
                        if (mov != null)
                        {
                            mov.CodigoEstado = Diccionario.Eliminado;
                            mov.FechaIngreso = DateTime.Now;
                            mov.UsuarioModificacion = tsUsuario;
                            mov.TerminalModificacion = tsTerminal;

                        }
                    }

                    loBaseDa.SaveChanges();
                }
                else
                {
                    Msg = "No es posible eliminar préstamo, Existe al menos una cuota cerrada!";
                }
            }
            return Msg;
        }

        public Empleado goBuscarEmpleado(int tId)
        {
            return loBaseDa.Find<REHPEMPLEADO>().Where(x => x.IdPersona == tId).Select(x => new Empleado
            {
                IdPersona = x.IdPersona,
                CodigoRegionIess = x.CodigoRegionIess
            }).FirstOrDefault();
        }
        #endregion


    }

}
