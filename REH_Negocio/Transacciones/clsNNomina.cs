using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Negocio;
using System.Transactions;
using static GEN_Entidad.Diccionario;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 11/06/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNNomina : clsNBase
    {
        /// <summary>
        /// LLamada al Sp para generar Nómina
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <returns>Retorna un string: de estar vacio se ejecutó correctamente, caso contrario muestra mensaje de error</returns>
        public string gsGenerarNomina(int tiPeriodo, string tsUsuario)
        {
            string psMsg = "";

            using (var poTran = new TransactionScope())
            {

                loBaseDa.ExecuteQuery(String.Format("EXEC REHSPSIMULANOMINA '{0}', '{1}' ", tiPeriodo, tsUsuario));

                psMsg = loBaseDa.ExecStoreProcedure<string>
                ("REHSPGENERANOMINA @IdPeriodo, @Usuario",
                new SqlParameter("@IdPeriodo", tiPeriodo),
                new SqlParameter("@Usuario", tsUsuario)
                ).FirstOrDefault();
                poTran.Complete();
            }



            return psMsg;
        }


        public string gsCerrarPeriodoParaContabilidadEnLiquidados(int tiPeriodo, string tsUsuario)
        {
            string psMsg = "";

            loBaseDa.CreateContext();

            var poRegistro = loBaseDa.Get<REHPPERIODO>().Where(x => x.IdPeriodo == tiPeriodo).FirstOrDefault();
            if (poRegistro != null)
            {
                poRegistro.CerradoDiarioProvision = true;
                loBaseDa.SaveChanges();
            }

            return psMsg;
        }
        /// <summary>
        /// LLamada al Sp para generar Nómina
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <returns>Retorna un string: de estar vacio se ejecutó correctamente, caso contrario muestra mensaje de error</returns>
        public DataSet RptNomina(int tiPeriodo, string tsUsuario)
        {
            DataSet ds = new DataSet();

            ds = loBaseDa.DataSet("EXEC REHSPGENERAIR @ID = @paramID ",
            new SqlParameter("paramID", SqlDbType.Int) { Value = tiPeriodo }
            );

            return ds;
        }

        /// <summary>
        /// Envía roles de Pago
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tIdPersona"></param>
        private void lEnvioCorreoRol(int tiPeriodo, int? tIdPersona = null)
        {
            if (tIdPersona == null)
            {
                loBaseDa.ExecuteQuery(String.Format("EXEC REHSPENVIOCORREOROL {0} ", tiPeriodo));
            }
            else
            {
                loBaseDa.ExecuteQuery(String.Format("EXEC REHSPENVIOCORREOROL {0}, {1}", tiPeriodo, tIdPersona));
            }
        }

        private DataTable ldtEnvioCorreoRol(int tiPeriodo, int? tIdPersona = null)
        {
            if (tIdPersona == null)
            {
                return loBaseDa.DataTable(String.Format("EXEC REHSPENVIOCORREOROL {0}, NULL, '0', '1' ", tiPeriodo));
            }
            else
            {
                return loBaseDa.DataTable(String.Format("EXEC REHSPENVIOCORREOROL {0}, {1}, '0', '1'", tiPeriodo, tIdPersona));
            }
        }

        public DataSet gdtRptLiquidacion(int tId)
        {
            return loBaseDa.DataSet(string.Format("EXEC REHSPCONSULTALIQUIDACION {0}", tId));
        }

        public DataTable gCalcular(int tIdPersona, string tsMotivo, DateTime tdFecha,string tsUsuario, string tsObservacion, bool tbGrabar, decimal tdValorComisiones, out DataSet ds)
        {
            ds = new DataSet();
            DataTable dt = new DataTable();

            int pIdperiodo = 0;

            var poPeriodo = loBaseDa.Find<REHPPERIODO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoTipoRol == Diccionario.Tablas.TipoRol.FinMes
            && x.FechaInicio <= tdFecha && x.FechaFin >= DbFunctions.TruncateTime(tdFecha)).FirstOrDefault();

            loBaseDa.ExecuteQuery(string.Format("DELETE FROM TMPNOMDETFIN WHERE IdPersona = '{0}' AND CodigoTipoRol = '{1}'", tIdPersona, Diccionario.Tablas.TipoRol.FinMes));
            loBaseDa.ExecuteQuery(string.Format("EXEC REHSPGENERANOMINALIQ {0},'{1}','1', {2}, '0', '{3}'", poPeriodo.IdPeriodo, tsUsuario, tIdPersona, tdFecha.ToString("yyyyMMdd")));

            int pIdContEmpleado = loBaseDa.Find<REHVTPERSONACONTRATO>().Where(x => x.IdPersona == tIdPersona).Select(x => x.IdEmpleadoContrato).FirstOrDefault();

            //loBaseDa.ExecuteQuery(string.Format("DELETE FROM TMPNOMDETFIN WHERE IdPersona = '{0}' AND CodigoTipoRol = '{1}'", tIdPersona, Diccionario.Tablas.TipoRol.Comisiones));
            var RolComi = ListarRolManual(pIdContEmpleado);
            foreach (var item in RolComi.Where(x=>x.Valor > 0))
            {
                loBaseDa.ExecuteQuery(string.Format("DELETE FROM TMPNOMDETFIN WHERE IdPersona = '{0}' AND IdPeriodo = '{1}'", tIdPersona, item.IdPeriodo));
                loBaseDa.ExecuteQuery(string.Format("EXEC REHSPGENERANOMINALIQ {0},'{1}','1', {2}, '0', '{3}',{4}", item.IdPeriodo, tsUsuario, tIdPersona, tdFecha.ToString("yyyyMMdd"), item.Valor));
            }
            
            ds = loBaseDa.DataSet(string.Format("EXEC REHSPCALCULOLIQUIDACION {0},'{1}','{2}','{3}','{4}','{5}',{6}", tIdPersona, tdFecha.ToString("yyyyMMdd"), tsMotivo, tsUsuario, tsObservacion, tbGrabar, 0));

            dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Rubro"),
                                    new DataColumn("Descripción"),
                                    new DataColumn("Valor", typeof(decimal))
                                    });

            foreach (DataRow item in ds.Tables[1].Rows)
            {
                DataRow row = dt.NewRow();
                row["Rubro"] = item[3].ToString();
                row["Descripción"] = item[4].ToString();
                row["Valor"] = item[5].ToString();
                dt.Rows.Add(row);
            }

            return dt;

        }


        public DataTable ldtLiquidacion(int tId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            ds = gdtRptLiquidacion(tId);

            dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Rubro"),
                                    new DataColumn("Descripción"),
                                    new DataColumn("Valor", typeof(decimal))
                                    });

            foreach (DataRow item in ds.Tables[1].Rows)
            {
                DataRow row = dt.NewRow();
                row["Rubro"] = item[3].ToString();
                row["Descripción"] = item[4].ToString();
                row["Valor"] = item[5].ToString();
                dt.Rows.Add(row);
            }

            return dt;

        }

        public List<RubroManual> ListarRubroManual(int tIdEmpleadoContrato)
        {
            return loBaseDa.Find<REHTLIQUIDACIONRUBROMANUAL>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdEmpledoContrato == tIdEmpleadoContrato).Select
                (x => new RubroManual()
                {
                    Id = x.IdLiquidacionRubroManual,
                    Rubro = x.CodigoRubro,
                    Valor = x.Valor,
                    Descripcion = x.Descripcion
                }).ToList();
        }

        public string gsGuardarRubrosManuales(int tIdEmpleadoContrato, int tIdPersona, List<RubroManual> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";
            loBaseDa.CreateContext();

            if (tIdPersona == 0)
            {
                psMsg = string.Format("{0}Seleccione empleado. \n", psMsg);
            }

            if (tIdEmpleadoContrato == 0)
            {
                psMsg = string.Format("{0}No existe contrato vigente. \n", psMsg);
            }

            if (string.IsNullOrEmpty(psMsg))
            {
                var poRubros = loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();
                var poLista = loBaseDa.Get<REHTLIQUIDACIONRUBROMANUAL>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdEmpledoContrato == tIdEmpleadoContrato).ToList();

                //Saber cual elementos hay que modificar
                List<int> poListaId = toLista.Select(x => x.Id).ToList();
                List<int> piListaEliminar = poLista.Where(x => !poListaId.Contains(x.IdLiquidacionRubroManual)).Select(x => x.IdLiquidacionRubroManual).ToList();
                //Recorrer la base de dato modificando el codigo estado a eliminado
                foreach (var poItem in poLista.Where(x => piListaEliminar.Contains(x.IdLiquidacionRubroManual)))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toLista)
                {
                    int pId = poItem.Id;

                    var poObjectItem = poLista.Where(x => x.IdLiquidacionRubroManual == pId && pId != 0).FirstOrDefault();
                    if (poObjectItem != null)
                    {
                        poObjectItem.UsuarioModificacion = tsUsuario;
                        poObjectItem.TerminalModificacion = tsTerminal;
                        poObjectItem.FechaModificacion = DateTime.Now;
                    }
                    else
                    {
                        poObjectItem = new REHTLIQUIDACIONRUBROMANUAL();
                        loBaseDa.CreateNewObject(out poObjectItem);
                        poObjectItem.UsuarioIngreso = tsUsuario;
                        poObjectItem.TerminalIngreso = tsTerminal;
                        poObjectItem.FechaIngreso = DateTime.Now;
                    }

                    var poRubro = poRubros.Where(x => x.CodigoRubro == poItem.Rubro).FirstOrDefault();

                    poObjectItem.CodigoEstado = Diccionario.Activo;
                    poObjectItem.CodigoRubro = poRubro.CodigoRubro;
                    poObjectItem.Grupo = poRubro.CodigoTipoRubro == Diccionario.Tablas.TipoRubro.Egresos ? "DESCUENTOS" :"INGRESOS";
                    poObjectItem.TipoRubro = poRubro.CodigoTipoRubro == Diccionario.Tablas.TipoRubro.Egresos ? "" : "REMUNERACIONES PENDIENTES";
                    poObjectItem.Valor = poItem.Valor;
                    poObjectItem.Rubro = poRubro.Descripcion;
                    poObjectItem.IdEmpledoContrato = tIdEmpleadoContrato;
                    poObjectItem.IdPersona = tIdPersona;
                    poObjectItem.Descripcion = poItem.Descripcion;

                }

                loBaseDa.SaveChanges();
            }
            return psMsg;
        }

        public List<RolManual> ListarRolManual(int tIdEmpleadoContrato)
        {
            return loBaseDa.Find<REHTLIQUIDACIONROLCOMISIONES>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdEmpledoContrato == tIdEmpleadoContrato).Select
                (x => new RolManual()
                {
                    Id = x.IdLiquidacionRolComisiones,
                    IdPeriodo = x.IdPeriodo,
                    Valor = x.Valor,
                    Observacion = x.Observacion,
                    Mes = x.Mes
                }).ToList();
        }

        public string gsGuardarRolManuales(int tIdEmpleadoContrato, int tIdPersona, List<RolManual> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";
            loBaseDa.CreateContext();

            if (tIdPersona == 0)
            {
                psMsg = string.Format("{0}Seleccione empleado. \n", psMsg);
            }

            if (tIdEmpleadoContrato == 0)
            {
                psMsg = string.Format("{0}No existe contrato vigente. \n", psMsg);
            }

            if (string.IsNullOrEmpty(psMsg))
            {
                var poPeriodos = loBaseDa.Find<REHPPERIODO>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();
                var poLista = loBaseDa.Get<REHTLIQUIDACIONROLCOMISIONES>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdEmpledoContrato == tIdEmpleadoContrato).ToList();

                //Saber cual elementos hay que modificar
                List<int> poListaId = toLista.Select(x => x.Id).ToList();
                List<int> piListaEliminar = poLista.Where(x => !poListaId.Contains(x.IdLiquidacionRolComisiones)).Select(x => x.IdLiquidacionRolComisiones).ToList();
                //Recorrer la base de dato modificando el codigo estado a eliminado
                foreach (var poItem in poLista.Where(x => piListaEliminar.Contains(x.IdLiquidacionRolComisiones)))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toLista)
                {
                    int pId = poItem.Id;

                    var poObjectItem = poLista.Where(x => x.IdLiquidacionRolComisiones == pId && pId != 0).FirstOrDefault();
                    if (poObjectItem != null)
                    {
                        poObjectItem.UsuarioModificacion = tsUsuario;
                        poObjectItem.TerminalModificacion = tsTerminal;
                        poObjectItem.FechaModificacion = DateTime.Now;
                    }
                    else
                    {
                        poObjectItem = new REHTLIQUIDACIONROLCOMISIONES();
                        loBaseDa.CreateNewObject(out poObjectItem);
                        poObjectItem.UsuarioIngreso = tsUsuario;
                        poObjectItem.TerminalIngreso = tsTerminal;
                        poObjectItem.FechaIngreso = DateTime.Now;
                    }

                    var poPeriodo = poPeriodos.Where(x => x.IdPeriodo == poItem.IdPeriodo).FirstOrDefault();

                    poObjectItem.CodigoEstado = Diccionario.Activo;
                    poObjectItem.IdPeriodo = poItem.IdPeriodo;
                    poObjectItem.Codigo = poPeriodo.Codigo;
                    poObjectItem.Anio = poPeriodo.Anio;
                    poObjectItem.Mes = poPeriodo.FechaInicio.Month;
                    poObjectItem.Observacion = poItem.Observacion;
                    poObjectItem.Valor = poItem.Valor;
                    poObjectItem.IdEmpledoContrato = tIdEmpleadoContrato;
                    poObjectItem.IdPersona = tIdPersona;

                }

                loBaseDa.SaveChanges();
            }
            return psMsg;
        }

        public List<Liquidacion> goListarLiquidaciones()
        {
            return loBaseDa.Find<REHTLIQUIDACION>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.CodigoEstado != Diccionario.Inactivo)
                .Select(x => new Liquidacion()
                {
                    IdLiquidacion = x.IdLiquidacion,
                    IdEmpleadoContrato = x.IdEmpleadoContrato,
                    FechaCalculo = x.FechaCalculo,
                    Nombre = x.Nombre,
                    FechaInicioContrato = x.FechaInicioContrato,
                    FechaFinContrato = x.FechaFinContrato,
                    DesMotivo = x.Motivo,
                    Motivo = x.CodigoMotivo,
                    CodigoEstado = x.CodigoEstado,
                    Total = x.Total,
                }).ToList();
        }

        public Liquidacion goConsularLiquidacionXIdLiquidacion(int tId)
        {
            return loBaseDa.Find<REHTLIQUIDACION>().Where(x => x.IdLiquidacion == tId && x.CodigoEstado != Diccionario.Eliminado && x.CodigoEstado != Diccionario.Rechazado)
                .Select(x => new Liquidacion()
                {
                    IdLiquidacion = x.IdLiquidacion,
                    IdEmpleadoContrato = x.IdEmpleadoContrato,
                    FechaCalculo = x.FechaCalculo,
                    Nombre = x.Nombre,
                    FechaInicioContrato = x.FechaInicioContrato,
                    FechaFinContrato = x.FechaFinContrato,
                    Motivo = x.CodigoMotivo,
                    CodigoEstado = x.CodigoEstado,
                    IdPersona = x.IdPersona,
                    Observacion = x.Observacion,
                    ValorRolComisiones = x.ValorRolComisiones??0M
                }).FirstOrDefault();
        }

        /// <summary>
        /// Aprobar Comisiones
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsAprobar(int tId, string tsComentario, string tsUsuario, string tsTerminal, int tIdMenu)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<REHTLIQUIDACION>().Where(x => x.IdLiquidacion == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.Liquidacion;
                    int pId = tId;
                    var piCantidadAutorizacion = loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoTransaccion == psCodigoTransaccion).Select(x => x.CantidadAutorizaciones)?.FirstOrDefault();
                    List<string> psUsuario = new List<string>();
                    if (piCantidadAutorizacion > 0)
                    {
                        psUsuario = loBaseDa.Find<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                        x.CodigoEstado == Diccionario.Activo &&
                        x.CodigoTransaccion == psCodigoTransaccion &&
                        x.IdTransaccionReferencial == pId
                        && x.Tipo == Diccionario.TipoAprobacion.Aprobado
                        ).Select(x => x.UsuarioAprobacion).Distinct().ToList();

                        if (psUsuario.Contains(tsUsuario))
                        {
                            psResult = "Ya existe una aprobación con el usuario: " + tsUsuario + ". \n";
                        }

                        if (psUsuario.Count >= piCantidadAutorizacion)
                        {
                            psResult += "Transacción ya cuenta con la cantidad de aprobaciones necesarias. \n";
                        }
                    }


                    if (string.IsNullOrEmpty(psResult))
                    {

                        // Usuario parametrizado para que su aprobación sea la definitiva
                        bool pbAprobacionFinal = false;
                        var psCodigoUsuario = loBaseDa.Find<SEGPUSUARIOAPROBACIONEXCEPCION>()
                            .Where(x => x.CodigoEstado == Diccionario.Activo && x.IdMenu == tIdMenu && x.CodigoUsuario == tsUsuario).Select(x => x.CodigoUsuario)
                            .FirstOrDefault();

                        if (!string.IsNullOrEmpty(psCodigoUsuario)) pbAprobacionFinal = true;

                        string psCodigoEstado = string.Empty;
                        if (pbAprobacionFinal)
                        {
                            psCodigoEstado = Diccionario.Aprobado;
                        }
                        else
                        {
                            // Se agrega una autorización más por la que se va a guardar en este proceso
                            if ((psUsuario.Count + 1) == piCantidadAutorizacion)
                            {
                                psCodigoEstado = Diccionario.Aprobado;
                                poObject.UsuarioAprobacion = tsUsuario;
                                poObject.FechaAprobacion = DateTime.Now;
                            }
                            else
                            {
                                psCodigoEstado = Diccionario.PreAprobado;
                            }
                        }

                        if (psCodigoEstado == Diccionario.Aprobado)
                        {
                            var piAnio = DateTime.Now.Date.Year;
                            var piMes = DateTime.Now.Date.Month;
                            int piContNominaCerrada = loBaseDa.Find<REHTNOMINA>().Where(x => x.CodigoEstado == Diccionario.Cerrado && x.CodigoTipoRol == Diccionario.Tablas.TipoRol.FinMes && x.Anio == piAnio && x.Mes == piMes).Count();

                            if (piContNominaCerrada > 0)
                            {
                                return "No es posible aprobar deibo a que el periodo está cerrado.";
                            }
                        }

                        REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTransaccion);
                        poTransaccion.CodigoEstado = Diccionario.Activo;
                        poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                        poTransaccion.ComentarioAprobador = tsComentario;
                        poTransaccion.IdTransaccionReferencial = pId;
                        poTransaccion.UsuarioAprobacion = tsUsuario;
                        poTransaccion.UsuarioIngreso = tsUsuario;
                        poTransaccion.FechaIngreso = DateTime.Now;
                        poTransaccion.TerminalIngreso = tsTerminal;
                        poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                        poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psCodigoEstado);
                        poTransaccion.Tipo = Diccionario.TipoAprobacion.Aprobado;

                        poObject.CodigoEstado = psCodigoEstado;
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;


                        using (var poTran = new TransactionScope())
                        {
                            loBaseDa.SaveChanges();
                            if (psCodigoEstado == Diccionario.Aprobado)
                            {
                                loBaseDa.ExecuteQuery(string.Format("EXEC REHSPCERRARLIQUIDACION {0},'{1}'", poObject.IdLiquidacion, tsUsuario));
                            }

                            poTran.Complete();
                        }
                    }


                }
                else
                {
                    psResult = "Liquidación ya aprobada!";
                }
            }
            else
            {
                psResult = "No existe liquidación por aprobar";
            }
            return psResult;

        }

        /// <summary>
        /// Actualiza estado de Registro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public string gsActualizarEstadoLiquidacion(int tId, string TipoEstadoSolicitud, string Observacion, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";

            loBaseDa.CreateContext();
            var poObject = loBaseDa.Get<REHTLIQUIDACION>().Where(x => x.IdLiquidacion == tId).FirstOrDefault();
            if (poObject != null)
            {
                poObject.Observacion = Observacion;
                poObject.CodigoEstado = TipoEstadoSolicitud;
                poObject.FechaModificacion = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
            }

            var poTran = loBaseDa.Get<REHTTRANSACCIONAUTOIZACION>().Where(x => x.CodigoTransaccion == Diccionario.Tablas.Transaccion.Liquidacion && x.CodigoEstado == Diccionario.Activo && x.IdTransaccionReferencial == tId && x.Tipo == Diccionario.TipoAprobacion.Aprobado).ToList();

            foreach (var item in poTran)
            {
                item.CodigoEstado = Diccionario.Inactivo;
                item.FechaModificacion = DateTime.Now;
                item.UsuarioModificacion = tsUsuario;
                item.TerminalModificacion = tsTerminal;
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }

        public List<BandejaLiquidacion> goListarBandejaLiquidacion(string tsUsuario, int tiMenu)
        {
            return loBaseDa.ExecStoreProcedure<BandejaLiquidacion>(string.Format("COMSPCONSULTABANDEJALIQUIDACIONES {0},'{1}'", tiMenu, tsUsuario));
        }

        public string gsEliminarLiquidacion(int tIdLiquidacion, string tsUsuario, string tsTerminal)
        {

            string psMsg = "";

            var poObject = loBaseDa.Get<REHTLIQUIDACION>().Include(x => x.REHTLIQUIDACIONDETALLEBS).Include(x => x.REHTLIQUIDACIONDETALLEROL)
                .Include(x => x.REHTLIQUIDACIONDETALLEVAC).Include(x => x.REHTLIQUIDACIONRESUMEN)
                .Where(x => x.IdLiquidacion == tIdLiquidacion).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.Corregir || poObject.CodigoEstado == Diccionario.Rechazado)
                {
                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;


                    foreach (var item in poObject.REHTLIQUIDACIONDETALLEBS.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {
                        item.CodigoEstado = Diccionario.Eliminado;
                        item.FechaModificacion = DateTime.Now;
                        item.UsuarioModificacion = tsUsuario;
                        item.TerminalModificacion = tsTerminal;
                    }

                    foreach (var item in poObject.REHTLIQUIDACIONDETALLEROL.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {
                        item.CodigoEstado = Diccionario.Eliminado;
                        item.FechaModificacion = DateTime.Now;
                        item.UsuarioModificacion = tsUsuario;
                        item.TerminalModificacion = tsTerminal;
                    }

                    foreach (var item in poObject.REHTLIQUIDACIONDETALLEVAC.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {
                        item.CodigoEstado = Diccionario.Eliminado;
                        item.FechaModificacion = DateTime.Now;
                        item.UsuarioModificacion = tsUsuario;
                        item.TerminalModificacion = tsTerminal;
                    }

                    foreach (var item in poObject.REHTLIQUIDACIONRESUMEN.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {
                        item.CodigoEstado = Diccionario.Eliminado;
                        item.FechaModificacion = DateTime.Now;
                        item.UsuarioModificacion = tsUsuario;
                        item.TerminalModificacion = tsTerminal;
                    }
                }
                else
                {
                    psMsg = "No es posible eliminar, tiene un estado :" + Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                }
                
            }
            else
            {
                psMsg = "No existe registro para eliminar.";
            }

            loBaseDa.SaveChanges();

            return psMsg;
        }

        public Liquidacion goConsularLiquidacionXIdEmpleado(int tId)
        {
            return loBaseDa.Find<REHTLIQUIDACION>().Where(x => (x.CodigoEstado != Diccionario.Eliminado && x.CodigoEstado != Diccionario.Inactivo) && x.IdEmpleadoContrato == tId)
                .Select(x => new Liquidacion()
                {
                    IdLiquidacion = x.IdLiquidacion,
                    IdEmpleadoContrato = x.IdEmpleadoContrato,
                    FechaCalculo = x.FechaCalculo,
                    Nombre = x.Nombre,
                    FechaInicioContrato = x.FechaInicioContrato,
                    FechaFinContrato = x.FechaFinContrato,
                    Motivo = x.CodigoMotivo,
                    CodigoEstado = x.CodigoEstado,
                    IdPersona = x.IdPersona,
                    Observacion = x.Observacion,
                    ValorRolComisiones = x.ValorRolComisiones ?? 0M
                }).FirstOrDefault();
        }

        /// <summary>
        /// Método que envía correos a los empleados con su rol
        /// </summary>
        /// <param name="tiPeriodo"> Id del Periodo</param>
        /// <param name="tIdPersona">Id de Persona Opcional</param>
        public void gEnvioCorreoRol(int tiPeriodo, int? tIdPersona = null)
        {
            lEnvioCorreoRol(tiPeriodo, tIdPersona);
        }

        /// <summary>
        /// Método que envía correos a los empleados con su rol
        /// </summary>
        /// <param name="tiPeriodo"> Id del Periodo</param>
        /// <param name="tIdPersona">Id de Persona Opcional</param>
        public DataTable gdtEnvioCorreoRol(int tiPeriodo, int? tIdPersona = null)
        {
            return ldtEnvioCorreoRol(tiPeriodo, tIdPersona);
        }

        public List<Persona> goConsultarNominaempleado(int tIdPeriodo)
        {
            return (from a in loBaseDa.Find<REHTNOMINA>()
                    join b in loBaseDa.Find<REHTNOMINAEMPLEADO>() on a.IdNomina equals b.IdNomina
                    where a.CodigoEstado == Diccionario.Cerrado && a.IdPeriodo == tIdPeriodo
                    select new Persona()
                    {
                        NumeroIdentificacion = b.Identificacion,
                        IdPersona = b.IdPersona,
                        IdBiometrico = b.IdNominaEmpleado
                    }).ToList();
        }

        /// <summary>
        /// Método que envía correos a los empleados con su rol
        /// </summary>
        /// <param name="tiPeriodo"> Id del Periodo</param>
        /// <param name="tIdPersona">Id de Persona Opcional</param>
        public void gEnvioCorreoRol(int tiPeriodo, string tsNumeroIdentificacion)
        {

            int pIdPersona = loBaseDa.Find<GENMPERSONA>().Where(x => x.NumeroIdentificacion == tsNumeroIdentificacion).Select(x => x.IdPersona).FirstOrDefault();
            lEnvioCorreoRol(tiPeriodo, pIdPersona);
        }

        /// <summary>
        /// Método que envía correos a los empleados con su rol
        /// </summary>
        /// <param name="tiPeriodo"> Id del Periodo</param>
        /// <param name="tIdPersona">Id de Persona Opcional</param>
        public DataTable gdtEnvioCorreoRol(int tiPeriodo, string tsNumeroIdentificacion)
        {

            int pIdPersona = loBaseDa.Find<GENMPERSONA>().Where(x => x.NumeroIdentificacion == tsNumeroIdentificacion).Select(x => x.IdPersona).FirstOrDefault();
            return ldtEnvioCorreoRol(tiPeriodo, pIdPersona);
        }


        /// <summary>
        /// Aprobar Nómina
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public bool gbAprobarNomina(int tiPeriodo, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            lCambiaEstadoNomina(tiPeriodo, tsUsuario, tsTerminal, true);
            return true;
        }

        /// <summary>
        /// Reversar Nómina
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public bool gbReversarNomina(int tiPeriodo, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            lCambiaEstadoNomina(tiPeriodo, tsUsuario, tsTerminal, false);
            return true;
        }

        /// <summary>
        /// Reversar Nómina
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsReversarNomina(int tiPeriodo, string tsUsuario)
        {
            return loBaseDa.ExecStoreProcedure<string>
               ("REHSPREVERSARNOMINA @IdPeriodo, @Usuario",
               new SqlParameter("@IdPeriodo", tiPeriodo),
               new SqlParameter("@Usuario", tsUsuario)
               ).FirstOrDefault();
        }

        public string gsCerrarNomina(int tiPeriodo, string tsUsuario)
        {
            return loBaseDa.ExecStoreProcedure<string>
                ("REHSPCERRARNOMINA @IdPeriodo, @Usuario",
                new SqlParameter("@IdPeriodo", tiPeriodo),
                new SqlParameter("@Usuario", tsUsuario)
                ).FirstOrDefault();
        }

        private void lCambiaEstadoNomina(int tiPeriodo, string tsUsuario, string tsTerminal, bool tbAprobar)
        {
            string psCodigoEstadoConsulta = tbAprobar ? Diccionario.Pendiente : Diccionario.Cerrado;
            string psCodigoEstadoUpdate = tbAprobar ? Diccionario.Cerrado : Diccionario.Pendiente;
            string psCodigoEstadoConsultaBS = tbAprobar ? Diccionario.Pendiente : Diccionario.Activo;
            string psCodigoEstadoUpdateBS = tbAprobar ? Diccionario.Activo : Diccionario.Pendiente;
            var poObject = loBaseDa.Get<REHTNOMINA>().Include(x => x.REHTNOMINADETALLE).Where(x => x.IdPeriodo == tiPeriodo && x.CodigoEstado == psCodigoEstadoConsulta).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = psCodigoEstadoUpdate;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                poObject.FechaModificacion = DateTime.Now;

                foreach (var poItem in poObject.REHTNOMINADETALLE.Where(x => x.CodigoEstado == psCodigoEstadoConsulta))
                {
                    poItem.CodigoEstado = psCodigoEstadoUpdate;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.TerminalModificacion = tsTerminal;
                    poItem.FechaModificacion = DateTime.Now;
                }
            }

            var poObjectBS = loBaseDa.Get<REHTBENEFICIOSOCIALDETALLE>().Where(x => x.IdPeriodo == tiPeriodo && x.CodigoEstado == psCodigoEstadoConsultaBS).ToList();

            foreach (var poItem in poObjectBS)
            {
                poItem.CodigoEstado = psCodigoEstadoUpdateBS;
                poItem.UsuarioModificacion = tsUsuario;
                poItem.TerminalModificacion = tsTerminal;
                poItem.FechaModificacion = DateTime.Now;
            }

            loBaseDa.SaveChanges();
        }

        public string gsValidaNetoRecibirNegativos(int tiPeriodo)
        {
            string psRespuesta = string.Empty;
            var poLista = (from a in loBaseDa.Find<REHTNOMINA>()
                           join b in loBaseDa.Find<REHTNOMINADETALLE>() on a.IdNomina equals b.IdNomina
                           join c in loBaseDa.Find<REHPRUBRO>() on b.CodigoRubro equals c.CodigoRubro
                           join d in loBaseDa.Find<GENMPERSONA>() on b.IdPersona equals d.IdPersona
                           where a.IdPeriodo == tiPeriodo
                           && (a.CodigoEstado == Diccionario.Cerrado || a.CodigoEstado == Diccionario.Pendiente)
                           && (b.CodigoEstado == Diccionario.Cerrado || b.CodigoEstado == Diccionario.Pendiente)
                           && c.CodigoCategoriaRubro == Diccionario.ListaCatalogo.TipoCategoriaRubroClass.NetoRecibir && b.Valor < 0
                           select new NominaDetalleDt()
                           {
                               IdPeriodo = a.IdPeriodo,
                               NumeroIdentificacion = d.NumeroIdentificacion,
                               NombreCompleto = d.NombreCompleto,
                               CodigoRubro = b.CodigoRubro,
                               Rubro = c.Descripcion,
                               Valor = b.Valor,
                               CodigoEstado = a.CodigoEstado,
                           }).ToList();
            foreach (var item in poLista)
            {
                psRespuesta += "Colaborador: " + item.NombreCompleto + " tiene un neto a recibir en negativo, valor: " + item.Valor + "\n";
            }
            return psRespuesta;
        }

        public DataTable gdtValidaNomina(int tiPeriodo)
        {
           return loBaseDa.DataTable(string.Format("EXEC REHSPVALIDANOMINA {0}", tiPeriodo));
        }

        public string gsValidaCerrarRol(int tiPeriodo)
        {
            string psRespuesta = string.Empty;

            var poObject = (from a in loBaseDa.Find<REHPPERIODO>()
                            join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                            where a.IdPeriodo == tiPeriodo
                            select new
                            {
                                a.FechaInicio,
                                a.FechaFin,
                                DiaPago = b.DiaPago ?? 0,
                                DiaMaxReverso = b.DiaMaxReverso ?? 0
                            }).FirstOrDefault();

            if (poObject != null)
            {
                var pdFecha = DateTime.Now.Date;
                var pdFechaHasta = poObject.FechaFin.AddDays(poObject.DiaMaxReverso);

                if (pdFecha > pdFechaHasta)
                {
                    psRespuesta = "No es posible reversar!, Fecha máximo para reversar Rol: " + pdFechaHasta.ToString("dd/MM/yyyy");
                }
            }
            return psRespuesta;
        }

        /// <summary>
        /// Elimina Cálculo, Pone en estado Pendiente la Cab de Nómina y el detalle en estado Eliminado
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public bool gbEliminarCalculo(int tiPeriodo, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            var poObject = loBaseDa.Get<REHTNOMINA>().Include(x => x.REHTNOMINADETALLE).Where(x => x.IdPeriodo == tiPeriodo && x.CodigoEstado == Diccionario.Pendiente).FirstOrDefault();
            if (poObject != null)
            {
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                poObject.FechaModificacion = DateTime.Now;
                poObject.Total = 0M;

                foreach (var poItem in poObject.REHTNOMINADETALLE.Where(x => x.CodigoEstado == Diccionario.Pendiente))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.TerminalModificacion = tsTerminal;
                    poItem.FechaModificacion = DateTime.Now;
                }

                var poListVacDet = loBaseDa.Get<REHTVACACIONDETALLE>().Where(x => x.CodigoEstado == Diccionario.Pendiente && x.Anio == poObject.Anio && x.Mes == poObject.Mes).ToList();

                foreach (var item in poListVacDet)
                {
                    item.CodigoEstado = Diccionario.Eliminado;
                }

                loBaseDa.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// Elimina Nómina, Pone en estado Eliminado la Nomina
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public bool gbEliminarNomina(int tiPeriodo, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();

            var poObject = loBaseDa.Get<REHTNOMINA>().Include(x => x.REHTNOMINADETALLE).Where(x => x.IdPeriodo == tiPeriodo && x.CodigoEstado == Diccionario.Pendiente).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                poObject.FechaModificacion = DateTime.Now;

                foreach (var poItem in poObject.REHTNOMINADETALLE.Where(x => x.CodigoEstado == Diccionario.Pendiente))
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
        public string gsGetEstadoNomina(int tiPeriodo)
        {
            return loBaseDa.Find<REHTNOMINA>().Where(x => x.IdPeriodo == tiPeriodo && (x.CodigoEstado == Diccionario.Activo || x.CodigoEstado == Diccionario.Pendiente || x.CodigoEstado == Diccionario.Cerrado)).Select(x => x.CodigoEstado).FirstOrDefault();
        }

        public decimal gdcTotalNomina(int tIdPeriodo, out int tIdNomina, out string tsMensaje)
        {
            var poResp = loBaseDa.Find<REHTNOMINA>().Where(x => x.IdPeriodo == tIdPeriodo && x.CodigoEstado != Diccionario.Inactivo).Select(x => new { x.IdNomina, x.Total, x.CodigoEstado }).FirstOrDefault();
            tIdNomina = poResp.IdNomina;
            tsMensaje = Diccionario.gsGetDescripcion(poResp.CodigoEstado);
            return poResp.Total;
        }

        public List<Nomina> goConsultarNomina()
        {
            var poParametro = loBaseDa.Find<GENPPARAMETRO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                .Select(x => new { x.AnioInicioNomina, x.MesInicioNomina }).FirstOrDefault();

            DateTime pdFecha = new DateTime(poParametro.AnioInicioNomina ?? 0, poParametro.MesInicioNomina ?? 0, 1);


            return (from a in loBaseDa.Find<REHTNOMINA>().Include(x=>x.REHTNOMINAEMPLEADO)
                    join b in loBaseDa.Find<REHPPERIODO>() on a.IdPeriodo equals b.IdPeriodo
                    join c in loBaseDa.Find<REHMTIPOROL>() on b.CodigoTipoRol equals c.CodigoTipoRol
                    join d in loBaseDa.Find<GENMESTADO>() on a.CodigoEstado equals d.CodigoEstado
                    where !Diccionario.EstadosNoIncluyentesSistema.Contains(a.CodigoEstado)
                    && b.FechaFin >= pdFecha.Date
                    select new Nomina()
                    {
                        IdNomina = a.IdNomina,
                        IdPeriodo = a.IdPeriodo,
                        CodigoTipoRol = a.CodigoTipoRol,
                        CodigoPeriodo = b.Codigo,
                        DescripcionTipoRol = c.Descripcion,
                        FechaIngreso = a.FechaIngreso,
                        FechaInicio = b.FechaInicio,
                        FechaFin = b.FechaFin,
                        CodigoEstado = a.CodigoEstado,
                        Estado = d.Descripcion,
                        Total = a.Total,
                        Empleados = a.REHTNOMINAEMPLEADO.Count()
                    }).OrderByDescending(x => x.IdNomina).ToList();
        }

        /// <summary>
        /// Obtener Detalle de Nómina
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <returns></returns>
        public DataTable gdtGetNomina(int tiPeriodo, string tsCodigoTipoRol = "", bool tbConsultarJubilados = false)
        {
            DataTable dt = new DataTable();

            var poLista = (from a in loBaseDa.Find<REHTNOMINA>()
                           join b in loBaseDa.Find<REHTNOMINADETALLE>() on a.IdNomina equals b.IdNomina
                           join c in loBaseDa.Find<REHPRUBRO>() on b.CodigoRubro equals c.CodigoRubro
                           join d in loBaseDa.Find<GENMPERSONA>() on b.IdPersona equals d.IdPersona
                           join e in loBaseDa.Find<REHTNOMINAEMPLEADO>() on new { b.IdNomina, b.IdPersona  } equals  new { e.IdNomina, e.IdPersona }
                           where a.IdPeriodo == tiPeriodo
                           && (a.CodigoEstado == Diccionario.Cerrado || a.CodigoEstado == Diccionario.Pendiente)
                           && (b.CodigoEstado == Diccionario.Cerrado || b.CodigoEstado == Diccionario.Pendiente)
                           select new NominaDetalleDt()
                           {
                               IdPeriodo = a.IdPeriodo,
                               NumeroIdentificacion = d.NumeroIdentificacion,
                               NombreCompleto = d.NombreCompleto,
                               Departamento = e.Departamento,
                               CodigoRubro = b.CodigoRubro,
                               Rubro = c.Descripcion,
                               Valor = b.Valor,
                               CodigoEstado = a.CodigoEstado,
                           }).ToList();


            List<string> psRubros = new List<string>();
            List<string> psCodigoRubros = new List<string>();

            if (tsCodigoTipoRol == "E" && !tbConsultarJubilados)
            {
                poLista = poLista.Where(X => X.Departamento != "JUBILADOS").ToList();
            }
            if (tsCodigoTipoRol == "E" && tbConsultarJubilados)
            {
                poLista = poLista.Where(X => X.Departamento == "JUBILADOS").ToList();
            }

            var poRubros = poLista.Select(x => new { x.CodigoRubro, x.Rubro }).Distinct().ToList();
            psCodigoRubros.AddRange(poRubros.Select(x => x.CodigoRubro).ToList());
            psRubros.AddRange(poRubros.Select(x => x.Rubro).ToList());
            // Ordenar Rubros para su presentación
            var poListaRubrosOrdenados = loBaseDa.Find<REHPRUBRO>().Where(x => psCodigoRubros.Contains(x.CodigoRubro)).Select(x => new { x.Descripcion, x.Orden, EsEntero = x.EsEntero ?? false }).OrderBy(x => x.Orden).ToList();

            dt.Columns.AddRange(new DataColumn[]
                                {
                                    new DataColumn("IDENTIFICACIÓN"),
                                    new DataColumn("NOMBRE"),
                                    new DataColumn("DEPARTAMENTO"),
                                });

            //psRubros.ForEach(x => dt.Columns.Add(x));
            poListaRubrosOrdenados.ForEach(x => dt.Columns.Add(x.Descripcion, x.EsEntero ? typeof(Int32) : typeof(decimal)));


            List<lListNovedad> poListaEmpleados = new List<lListNovedad>();
            int piCont = 0;
            foreach (var psItem in poLista.Select(x => x.NumeroIdentificacion).Distinct().OrderBy(x => x).ToList())
            {
                poListaEmpleados.Add(new lListNovedad() { Index = piCont, Identificacion = psItem });
                piCont++;
            }

            string psDescripcionNetoARecibir = loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoCategoriaRubro == Diccionario.ListaCatalogo.TipoCategoriaRubroClass.NetoRecibir).Select(x => x.Descripcion).FirstOrDefault();

            List<string> psIdentIngresadas = new List<string>();
            foreach (var poItem in poLista.OrderBy(x => x.NumeroIdentificacion))
            {

                if (psIdentIngresadas.Where(x => x == poItem.NumeroIdentificacion).Count() == 0)
                {
                    DataRow row = dt.NewRow();
                    row["IDENTIFICACIÓN"] = poItem.NumeroIdentificacion;
                    row["NOMBRE"] = poItem.NombreCompleto;
                    row["DEPARTAMENTO"] = poItem.Departamento;
                    if (poItem.Rubro != psDescripcionNetoARecibir)
                    {
                        row[poItem.Rubro] = Math.Abs(poItem.Valor);
                    }
                    else
                    {
                        row[poItem.Rubro] = poItem.Valor;
                    }

                    dt.Rows.Add(row);
                    psIdentIngresadas.Add(poItem.NumeroIdentificacion);
                }
                else
                {
                    int pIndex = poListaEmpleados.Where(x => x.Identificacion == poItem.NumeroIdentificacion).Select(x => x.Index).FirstOrDefault();
                    DataRow row = dt.Rows[pIndex];
                    if (poItem.Rubro != psDescripcionNetoARecibir)
                    {
                        row[poItem.Rubro] = Math.Abs(poItem.Valor);
                    }
                    else
                    {
                        row[poItem.Rubro] = poItem.Valor;
                    }

                }
            }

            return dt;
        }

        public DataTable gdtImpuestoRenta(int tiAnio, int tiMes)
        {
            DataTable dt = new DataTable();

            dt = loBaseDa.DataTable("EXEC REHSPGENERAIR @Anio = @paramAnio, @Mes = @paramMes ",
            new SqlParameter("paramAnio", SqlDbType.Int) { Value = tiAnio },
            new SqlParameter("paramMes", SqlDbType.Int) { Value = tiMes });

            return dt;
        }

        public DataSet gdsRol(int tIdPeriodo, string tsNumeroIdentificacion)
        {
            int pIdPersona = loBaseDa.Find<GENMPERSONA>().Where(x => x.NumeroIdentificacion == tsNumeroIdentificacion).Select(x => x.IdPersona).FirstOrDefault();
            return ldsRol(tIdPeriodo, pIdPersona);

        }

        public DataSet gdsRol(int tIdPeriodo, int tIdPersona)
        {
            return ldsRol(tIdPeriodo, tIdPersona);

        }

        public BeneficioSocialDetalleAjuste goConsultarProvisionEmpleado(int tIdPersona, int tiAnio, int tiMes)
        {
            BeneficioSocialDetalleAjuste poObject = new BeneficioSocialDetalleAjuste();

            var poBenefSocial = loBaseDa.Find<REHTBENEFICIOSOCIALDETALLE>()
                .Where(x => x.CodigoEstado == Diccionario.Activo && x.IdPersona == tIdPersona
                && x.Anio == tiAnio && x.Mes == tiMes).Select(x => new
                {
                    x.CodigoTipoBeneficioSocial,
                    x.CodigoRubro,
                    x.Dias,
                    x.Valor,
                    x.TotalIngresos
                }).ToList();

            poObject.IdPersona = tIdPersona;
            poObject.Anio = tiAnio;
            poObject.Mes = tiMes;
            poObject.TotalIngresos = poBenefSocial.Where(x => x.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionVacaciones).Sum(x => x.TotalIngresos) ?? 0;
            poObject.ProvisionVacaciones = poBenefSocial.Where(x => x.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionVacaciones).Sum(x => x.Valor);
            poObject.ProvisionFondoReserva = poBenefSocial.Where(x => x.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionFondoReserva).Sum(x => x.Valor);
            poObject.ProvisionDecimoTercero = poBenefSocial.Where(x => x.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero).Sum(x => x.Valor);

            return poObject;
        }

        public BeneficioSocialDetalleAjuste goCalcularProvisionEmpleado(int tIdPersona, int tiAnio, int tiMes, decimal tdcTotalIngresos, bool tbSoloLiq = false, bool tbAjutarLiquidados = false)
        {
            BeneficioSocialDetalleAjuste poObject = new BeneficioSocialDetalleAjuste();
            var poVacacionDetalle = loBaseDa.Find<REHTVACACIONDETALLE>()
                .Where(x => x.CodigoEstado == Diccionario.Activo && x.IdPersona == tIdPersona && x.Anio == tiAnio && x.Mes == tiMes)
                .GroupBy(x => new { x.Periodo, x.Anio, x.Mes, x.DiasTrabajados, x.DiasNormales, x.DiasAdicionales })
                .Select(x => new { x.Key.Periodo, x.Key.Anio, x.Key.Mes, x.Key.DiasTrabajados, x.Key.DiasNormales, x.Key.DiasAdicionales })
                .ToList();

            var poVacacion = loBaseDa.Find<REHTVACACION>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdPersona == tIdPersona && x.TotalDiasDevengados != 0).ToList();

            foreach (var item in poVacacion)
            {
                item.FechaInicial = item.FechaInicial.AddDays(-item.FechaInicial.Day + 1);
                item.FechaFinal = item.FechaFinal.AddDays(-item.FechaFinal.Day + 1).AddMonths(1).AddDays(-1);
            }

            var pdFechaFiltro = new DateTime(tiAnio, tiMes, 1).AddMonths(1).AddDays(-1);

            poObject.ProvisionVacLiq = false;
            if (poVacacion.Where(x => pdFechaFiltro >= x.FechaInicial && pdFechaFiltro <= x.FechaFinal).Count() > 0)
            {
                poObject.ProvisionVacLiq = true;
                poObject.AjustadoVac = false;
            }
            else
            {
                poObject.AjustadoVac = true;
            }


            var poPeriodo = loBaseDa.Find<REHPPERIODO>().Where(x => x.CodigoEstadoNomina == Diccionario.Cerrado && x.CodigoTipoRol == Diccionario.Tablas.TipoRol.DecimoTercero)
                .Select(x => new
                {
                    x.FechaInicio,
                    x.FechaFin,
                }).ToList();

            if (poPeriodo.Where(x => pdFechaFiltro >= x.FechaInicio && pdFechaFiltro <= x.FechaFin).Count() > 0)
            {
                poObject.ProvisionDecLiq = true;
            }
            else if (pdFechaFiltro < poPeriodo.Min(x => x.FechaInicio))
            {
                poObject.ProvisionDecLiq = true;
            }

            if (loBaseDa.Find<REHVTPERSONACONTRATO>().Where(x => x.IdPersona == tIdPersona).Count() == 0)
            {
                poObject.ProvisionDecLiq = true;
                poObject.ProvisionVacLiq = true;
                poObject.ProvisionFonLiq = true;
                poObject.AjustadoVac = false;
            }

            poObject.IdPersona = tIdPersona;
            poObject.Anio = tiAnio;
            poObject.Mes = tiMes;
            poObject.TotalIngresos = tdcTotalIngresos;

            //if (!tbSoloLiq) poObject.ProvisionVacaciones = gdcProvisionPreliminar(tIdPersona, tiAnio, tiMes, tdcTotalIngresos);
            poObject.ProvisionVacaciones = gdcProvisionPreliminar(tIdPersona, tiAnio, tiMes, tdcTotalIngresos);

            var poLista = loBaseDa.Find<REHTBENEFICIOSOCIALDETALLE>()
                .Where(x => x.CodigoEstado == Diccionario.Activo && x.IdPersona == tIdPersona
                && x.Anio == tiAnio && x.Mes == tiMes).Select(x => new { x.CodigoTipoBeneficioSocial, x.Valor, x.EnNomina }).ToList();

            if (poLista.Where(x => x.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionFondoReserva && x.Valor > 0).Count() > 0)
            {
                if (poLista.Where(x => x.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionFondoReserva).Select(x => x.EnNomina).FirstOrDefault())
                {
                    poObject.ProvisionFonLiq = true;
                    poObject.AjustadoFon = false;

                    if (tbAjutarLiquidados) poObject.AjustadoFon = true;


                }
                else
                {
                    poObject.AjustadoFon = !poObject.ProvisionFonLiq;
                }

                poObject.ProvisionFondoReserva = Math.Round(tdcTotalIngresos * 0.0833M, 2);
            }
            else
            {
                if (!tbSoloLiq)
                {
                    var pdFechaContrato = loBaseDa.Find<REHVTPERSONACONTRATO>().Where(x => x.IdPersona == tIdPersona).Select(x => x.FechaInicioContrato).FirstOrDefault();
                    var pdFechaContratoFdm = pdFechaContrato.AddDays(-pdFechaContrato.Day + 1).AddYears(1).AddMonths(1).AddDays(-1);

                    if (pdFechaFiltro > pdFechaContratoFdm)
                    {
                        poObject.AjustadoFon = !poObject.ProvisionFonLiq;
                        poObject.ProvisionFondoReserva = Math.Round(tdcTotalIngresos * 0.0833M, 2);
                    }
                }
            }

            if (poLista.Where(x => x.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero && x.Valor >= 0).Count() > 0)
            {
                var pbAjuste = !poObject.ProvisionDecLiq;
                if (poLista.Where(x => x.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero).Select(x => x.EnNomina).FirstOrDefault())
                {
                    poObject.ProvisionDecLiq = true;
                    var pdcValorProvisionDecNue = Math.Round(tdcTotalIngresos / 12M, 2);
                    var pdcValorProvisionDec = poLista.Where(x => x.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero).Sum(x => x.Valor);

                    if (pdcValorProvisionDec < pdcValorProvisionDecNue)
                    {
                        if (tbAjutarLiquidados)
                        {
                            pbAjuste = true;
                        }
                        else
                        {
                            pbAjuste = false;
                        }
                    }
                    else
                    {
                        pbAjuste = false;
                    }
                }

                poObject.AjustadoDec = pbAjuste;
                poObject.ProvisionDecimoTercero = Math.Round(tdcTotalIngresos / 12M, 2);
            }

            return poObject;
        }

        public decimal gdcProvisionPreliminar(int tIdPersona, int tiAnio, int tiMes, decimal tdcTotalIngreso)
        {
            decimal pdcValor = 0M;

            var dt2 = loBaseDa.DataTable(string.Format("EXEC REHSPREPROCESOPROVISIONVACACIONES {0},{1},{2},{3},'1'", tIdPersona, tiAnio, tiMes, tdcTotalIngreso));

            if (dt2.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt2.Rows[0][0].ToString()))
                {
                    pdcValor = Convert.ToDecimal(dt2.Rows[0][0].ToString());
                }
                else
                    pdcValor = 0;

            }

            return pdcValor;
        }

        public string gsActualizarProvisionBS(List<ConsolidadoComparativo> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";

            var poPersonas = loBaseDa.Find<GENMPERSONA>().Select(x => new { x.IdPersona, x.NumeroIdentificacion, x.NombreCompleto }).ToList();

            //using (var poTran = new TransactionScope())
            //{
            foreach (var item in toLista)
            {
                int pIdPersona = poPersonas.Where(x => x.NumeroIdentificacion == item.Cedula).Select(x => x.IdPersona).FirstOrDefault();
                BeneficioSocialDetalleAjuste poObject = new BeneficioSocialDetalleAjuste();
                poObject.IdPersona = pIdPersona;
                poObject.Anio = item.Año;
                poObject.Mes = item.Mes;
                poObject.TotalIngresos = item.TotalIngresosPreliminar;
                poObject.ProvisionDecimoTercero = item.ProvisionDecimoTerceroPreliminar;
                poObject.ProvisionFondoReserva = item.ProvisionFondoReservaPreliminar;
                poObject.ProvisionVacaciones = item.ProvisionVacacionesPreliminar;

                lActualizaProvision(poObject, tsUsuario, tsTerminal);

            }

            //    poTran.Complete();
            //}

            return psMsg;
        }


        public string gsActualizarProvisionBS(BeneficioSocialDetalleAjuste toObject, string tsUsuario, string tsTerminal, bool tbAjustaLiquidado = false)
        {
            string psMsg = lbValidaActualizaProvisionBS(toObject);

            if (string.IsNullOrEmpty(psMsg))
            {
                lActualizaProvision(toObject, tsUsuario, tsTerminal, tbAjustaLiquidado);
            }

            return psMsg;
        }

        private void lActualizaProvision(BeneficioSocialDetalleAjuste toObject, string tsUsuario, string tsTerminal, bool tbAjustaLiquidado = false)
        {
            decimal ValorAJusteVac = 0;

            decimal pdcValorPermitidoPos = loBaseDa.Find<GENPPARAMETRO>().Select(x => x.IntervaloAjusteProvision).FirstOrDefault() ?? 0;
            decimal pdcValorPermitidoNeg = pdcValorPermitidoPos * -1;
            List<string> psCodigosInactivar = new List<string>();
            List<BsDetalleAjuste> poListaAjustar = new List<BsDetalleAjuste>();
            var poCalculoPreliminar = goCalcularProvisionEmpleado(toObject.IdPersona, toObject.Anio, toObject.Mes, toObject.TotalIngresos, true, tbAjustaLiquidado);

            string psCedula, psNombre;
            var poPersona = loBaseDa.Find<GENMPERSONA>().Where(x => x.IdPersona == toObject.IdPersona).Select(x => new { x.NumeroIdentificacion, x.NombreCompleto }).FirstOrDefault();
            psCedula = poPersona.NumeroIdentificacion;
            psNombre = poPersona.NombreCompleto;


            if (poCalculoPreliminar.AjustadoDec)
            {
                psCodigosInactivar.Add(Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero);
            }
            if (poCalculoPreliminar.AjustadoFon)
            {
                psCodigosInactivar.Add(Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionFondoReserva);
            }
            if (poCalculoPreliminar.AjustadoVac)
            {
                psCodigosInactivar.Add(Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionVacaciones);
            }

            loBaseDa.CreateContext();

            var poListaObject = loBaseDa.Get<REHTBENEFICIOSOCIALDETALLE>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdPersona == toObject.IdPersona && x.Anio == toObject.Anio && x.Mes == toObject.Mes && psCodigosInactivar.Contains(x.CodigoTipoBeneficioSocial)).ToList();
            int piDias = 0, pIdPeriodo = 0;
            var psCodigosIngresados = new List<string>();
            foreach (var poItem in poListaObject)
            {
                var poRegistroAjuste = poListaAjustar.Where(x => x.CodigoTipoBeneficioSocial == poItem.CodigoTipoBeneficioSocial).FirstOrDefault();
                if (poRegistroAjuste == null)
                {
                    poRegistroAjuste = new BsDetalleAjuste();
                    poListaAjustar.Add(poRegistroAjuste);
                }

                decimal pdcValorProvision = poListaObject.Where(x => x.CodigoTipoBeneficioSocial == poItem.CodigoTipoBeneficioSocial).Sum(x => x.Valor);

                poRegistroAjuste.IdPersona = poItem.IdPersona;
                poRegistroAjuste.Cedula = psCedula;
                poRegistroAjuste.Nombre = psNombre;
                poRegistroAjuste.Año = poItem.Anio;
                poRegistroAjuste.Mes = poItem.Mes;
                poRegistroAjuste.CodigoTipoBeneficioSocial = poItem.CodigoTipoBeneficioSocial;
                poRegistroAjuste.TotalIngresos += poItem.TotalIngresos ?? 0M;
                poRegistroAjuste.Valor += poItem.Valor;
                poRegistroAjuste.TotalIngresosPreliminar = toObject.TotalIngresos;


                bool pbAJustaDetablleBs = true;


                if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionFondoReserva)
                {
                    poRegistroAjuste.ValorPreliminar = toObject.ProvisionFondoReserva;
                    poRegistroAjuste.ProvisionLiq = poCalculoPreliminar.ProvisionFonLiq;
                    pbAJustaDetablleBs = poCalculoPreliminar.AjustadoFon;
                    decimal pdcDiferencia = pdcValorProvision - toObject.ProvisionFondoReserva;
                    if (pdcDiferencia >= pdcValorPermitidoNeg && pdcDiferencia <= pdcValorPermitidoPos)
                    {
                        pbAJustaDetablleBs = false;
                    }

                }
                else if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero)
                {
                    poRegistroAjuste.ValorPreliminar = toObject.ProvisionDecimoTercero;
                    poRegistroAjuste.ProvisionLiq = poCalculoPreliminar.ProvisionDecLiq;
                    pbAJustaDetablleBs = poCalculoPreliminar.AjustadoDec;
                    decimal pdcDiferencia = pdcValorProvision - toObject.ProvisionDecimoTercero;
                    if (pdcDiferencia >= pdcValorPermitidoNeg && pdcDiferencia <= pdcValorPermitidoPos)
                    {
                        pbAJustaDetablleBs = false;
                    }
                }
                else if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionVacaciones)
                {
                    poRegistroAjuste.ValorPreliminar = toObject.ProvisionVacaciones;
                    poRegistroAjuste.ProvisionLiq = poCalculoPreliminar.ProvisionVacLiq;
                    pbAJustaDetablleBs = poCalculoPreliminar.AjustadoVac;
                    decimal pdcDiferencia = pdcValorProvision - toObject.ProvisionVacaciones;
                    if (pdcDiferencia >= pdcValorPermitidoNeg && pdcDiferencia <= pdcValorPermitidoPos)
                    {
                        pbAJustaDetablleBs = false;
                    }
                }

                //if (poItem.EnNomina) pbAJustaDetablleBs = false;



                poRegistroAjuste.Ajustado = pbAJustaDetablleBs;

                if (pbAJustaDetablleBs)
                {
                    piDias = poItem.Dias ?? 0;
                    pIdPeriodo = poItem.IdPeriodo;
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.UsuarioModificacion = tsUsuario;

                    if (!psCodigosIngresados.Contains(poItem.CodigoTipoBeneficioSocial))
                    {

                        var poRegistro = new REHTBENEFICIOSOCIALDETALLE();
                        loBaseDa.CreateNewObject(out poRegistro);
                        poRegistro.CodigoEstado = Diccionario.Activo;
                        poRegistro.IdPersona = poItem.IdPersona;
                        poRegistro.IdPeriodo = poItem.IdPeriodo;
                        poRegistro.Anio = poItem.Anio;
                        poRegistro.Mes = poItem.Mes;
                        poRegistroAjuste.Ajustado = true;

                        var poRegAjuste = new REHTBENEFICIOSOCIALAJUSTE();
                        loBaseDa.CreateNewObject(out poRegAjuste);
                        poRegAjuste.CodigoEstado = Diccionario.Activo;
                        poRegAjuste.IdPersona = poItem.IdPersona;
                        poRegAjuste.Anio = poItem.Anio;
                        poRegAjuste.Mes = poItem.Mes;
                        poRegAjuste.CodigoTipoBeneficioSocial = poItem.CodigoTipoBeneficioSocial;
                        poRegAjuste.UsuarioIngreso = tsUsuario;
                        poRegAjuste.FechaIngreso = DateTime.Now;
                        poRegAjuste.TerminalIngreso = tsTerminal;
                        poRegAjuste.IdEmpleadoContrato = poItem.IdEmpleadoContrato ?? 0;

                        if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionFondoReserva)
                        {
                            poRegistro.Valor = toObject.ProvisionFondoReserva;
                            poRegAjuste.Valor = poRegistro.Valor - pdcValorProvision;
                        }
                        else if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero)
                        {
                            poRegistro.Valor = toObject.ProvisionDecimoTercero;
                            poRegAjuste.Valor = poRegistro.Valor - pdcValorProvision;

                        }
                        else if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionVacaciones)
                        {
                            poRegistro.Valor = toObject.ProvisionVacaciones;
                            poRegAjuste.Valor = poRegistro.Valor - pdcValorProvision;
                            ValorAJusteVac = poRegAjuste.Valor;
                        }



                        poRegistro.CodigoTipoBeneficioSocial = poItem.CodigoTipoBeneficioSocial;
                        poRegistro.CodigoRubro = poItem.CodigoRubro;
                        poRegistro.EnNomina = poItem.EnNomina;
                        poRegistro.UsuarioIngreso = tsUsuario;
                        poRegistro.FechaIngreso = DateTime.Now;
                        poRegistro.TerminalIngreso = tsTerminal;
                        poRegistro.TotalIngresos = toObject.TotalIngresos;
                        poRegistro.Dias = poItem.Dias;
                        poRegistro.IdEmpleadoContrato = poItem.IdEmpleadoContrato;

                        psCodigosIngresados.Add(poItem.CodigoTipoBeneficioSocial);
                    }
                }
            }

            foreach (var poItem in poListaAjustar)
            {
                var poRegistroAjuste = new REHTAJUSTEPROVISION();
                loBaseDa.CreateNewObject(out poRegistroAjuste);
                poRegistroAjuste.IdPersona = poItem.IdPersona;
                poRegistroAjuste.Cedula = poItem.Cedula;
                poRegistroAjuste.Nombre = poItem.Nombre;
                poRegistroAjuste.Anio = poItem.Año;
                poRegistroAjuste.Mes = poItem.Mes;
                poRegistroAjuste.CodigoTipoBS = poItem.CodigoTipoBeneficioSocial;
                poRegistroAjuste.TotalIngresos = poItem.TotalIngresos;
                poRegistroAjuste.Provision += poItem.Valor;
                poRegistroAjuste.TotalIngresosAjuste = poItem.TotalIngresosPreliminar;
                poRegistroAjuste.ProvisionAjuste = poItem.ValorPreliminar;
                poRegistroAjuste.ProvisionLiq = poItem.ProvisionLiq;
                poRegistroAjuste.Ajustado = poItem.Ajustado;
            }

            using (var poTran = new TransactionScope())
            {
                if (!poCalculoPreliminar.ProvisionVacLiq)
                {
                    loBaseDa.ExecuteQuery(string.Format("EXEC REHSPREPROCESOPROVISIONVACACIONES {0},{1},{2},{3},'0',{4}", toObject.IdPersona, toObject.Anio, toObject.Mes, toObject.TotalIngresos, ValorAJusteVac));
                }

                loBaseDa.SaveChanges();
                poTran.Complete();
            }

        }


        private void lActualizaProvisionModificadoNew(BeneficioSocialDetalleAjuste toObject, string tsUsuario, string tsTerminal, bool tbAjustaLiquidado = false)
        {

            decimal ValorAJusteVac = 0;

            decimal pdcValorPermitidoPos = loBaseDa.Find<GENPPARAMETRO>().Select(x => x.IntervaloAjusteProvision).FirstOrDefault() ?? 0;
            decimal pdcValorPermitidoNeg = pdcValorPermitidoPos * -1;
            List<string> psCodigosInactivar = new List<string>();
            List<string> psCodigosGuardarLiquidados = new List<string>();
            List<BsDetalleAjuste> poListaAjustar = new List<BsDetalleAjuste>();
            var poCalculoPreliminar = goCalcularProvisionEmpleado(toObject.IdPersona, toObject.Anio, toObject.Mes, toObject.TotalIngresos, true, tbAjustaLiquidado);

            string psCedula, psNombre;
            var poPersona = loBaseDa.Find<GENMPERSONA>().Where(x => x.IdPersona == toObject.IdPersona).Select(x => new { x.NumeroIdentificacion, x.NombreCompleto }).FirstOrDefault();
            psCedula = poPersona.NumeroIdentificacion;
            psNombre = poPersona.NombreCompleto;


            if (poCalculoPreliminar.AjustadoDec)
            {
                psCodigosInactivar.Add(Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero);
            }
            else
            {
                psCodigosGuardarLiquidados.Add(Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero);
            }

            if (poCalculoPreliminar.AjustadoFon)
            {
                psCodigosInactivar.Add(Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionFondoReserva);
            }
            else
            {
                psCodigosGuardarLiquidados.Add(Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionFondoReserva);
            }

            if (poCalculoPreliminar.AjustadoVac)
            {
                psCodigosInactivar.Add(Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionVacaciones);
            }
            else
            {
                psCodigosGuardarLiquidados.Add(Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionVacaciones);
            }

            loBaseDa.CreateContext();

            var poListaObject = loBaseDa.Get<REHTBENEFICIOSOCIALDETALLE>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdPersona == toObject.IdPersona && x.Anio == toObject.Anio && x.Mes == toObject.Mes && (psCodigosInactivar.Contains(x.CodigoTipoBeneficioSocial) || psCodigosGuardarLiquidados.Contains(x.CodigoTipoBeneficioSocial))).ToList();
            int piDias = 0, pIdPeriodo = 0;
            var psCodigosIngresados = new List<string>();
            var psCodigosIngresadosLiquidados = new List<string>();
            foreach (var poItem in poListaObject.OrderBy(x => x.CodigoTipoBeneficioSocial))
            {
                var poRegistroAjuste = poListaAjustar.Where(x => x.CodigoTipoBeneficioSocial == poItem.CodigoTipoBeneficioSocial).FirstOrDefault();
                if (poRegistroAjuste == null)
                {
                    poRegistroAjuste = new BsDetalleAjuste();
                    poListaAjustar.Add(poRegistroAjuste);
                }

                decimal pdcValorProvision = poListaObject.Where(x => x.CodigoTipoBeneficioSocial == poItem.CodigoTipoBeneficioSocial).Sum(x => x.Valor);

                poRegistroAjuste.IdPersona = poItem.IdPersona;
                poRegistroAjuste.Cedula = psCedula;
                poRegistroAjuste.Nombre = psNombre;
                poRegistroAjuste.Año = poItem.Anio;
                poRegistroAjuste.Mes = poItem.Mes;
                poRegistroAjuste.CodigoTipoBeneficioSocial = poItem.CodigoTipoBeneficioSocial;
                poRegistroAjuste.TotalIngresos += poItem.TotalIngresos ?? 0M;
                poRegistroAjuste.Valor += poItem.Valor;
                poRegistroAjuste.TotalIngresosPreliminar = toObject.TotalIngresos;


                bool pbAJustaDetablleBs = true;


                if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionFondoReserva)
                {
                    poRegistroAjuste.ValorPreliminar = toObject.ProvisionFondoReserva;
                    poRegistroAjuste.ProvisionLiq = poCalculoPreliminar.ProvisionFonLiq;
                    pbAJustaDetablleBs = poCalculoPreliminar.AjustadoFon;
                    decimal pdcDiferencia = pdcValorProvision - toObject.ProvisionFondoReserva;
                    if (pdcDiferencia >= pdcValorPermitidoNeg && pdcDiferencia <= pdcValorPermitidoPos)
                    {
                        pbAJustaDetablleBs = false;
                    }

                }
                else if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero)
                {
                    poRegistroAjuste.ValorPreliminar = toObject.ProvisionDecimoTercero;
                    poRegistroAjuste.ProvisionLiq = poCalculoPreliminar.ProvisionDecLiq;
                    pbAJustaDetablleBs = poCalculoPreliminar.AjustadoDec;
                    decimal pdcDiferencia = pdcValorProvision - toObject.ProvisionDecimoTercero;
                    if (pdcDiferencia >= pdcValorPermitidoNeg && pdcDiferencia <= pdcValorPermitidoPos)
                    {
                        pbAJustaDetablleBs = false;
                    }
                }
                else if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionVacaciones)
                {
                    poRegistroAjuste.ValorPreliminar = toObject.ProvisionVacaciones;
                    poRegistroAjuste.ProvisionLiq = poCalculoPreliminar.ProvisionVacLiq;
                    pbAJustaDetablleBs = poCalculoPreliminar.AjustadoVac;
                    decimal pdcDiferencia = pdcValorProvision - toObject.ProvisionVacaciones;
                    if (pdcDiferencia >= pdcValorPermitidoNeg && pdcDiferencia <= pdcValorPermitidoPos)
                    {
                        pbAJustaDetablleBs = false;
                    }
                }

                //if (poItem.EnNomina) pbAJustaDetablleBs = false;

                poRegistroAjuste.Ajustado = pbAJustaDetablleBs;

                if (psCodigosInactivar.Contains(poItem.CodigoTipoBeneficioSocial))
                {
                    if (pbAJustaDetablleBs)
                    {
                        piDias = poItem.Dias ?? 0;
                        pIdPeriodo = poItem.IdPeriodo;
                        poItem.CodigoEstado = Diccionario.Inactivo;
                        poItem.FechaModificacion = DateTime.Now;
                        poItem.UsuarioModificacion = tsUsuario;

                        if (!psCodigosIngresados.Contains(poItem.CodigoTipoBeneficioSocial))
                        {

                            var poRegistro = new REHTBENEFICIOSOCIALDETALLE();
                            loBaseDa.CreateNewObject(out poRegistro);
                            poRegistro.CodigoEstado = Diccionario.Activo;
                            poRegistro.IdPersona = poItem.IdPersona;
                            poRegistro.IdPeriodo = poItem.IdPeriodo;
                            poRegistro.Anio = poItem.Anio;
                            poRegistro.Mes = poItem.Mes;

                            poRegistroAjuste.Ajustado = true;

                            var poRegAjuste = new REHTBENEFICIOSOCIALAJUSTE();
                            loBaseDa.CreateNewObject(out poRegAjuste);
                            poRegAjuste.CodigoEstado = Diccionario.Activo;
                            poRegAjuste.IdPersona = poItem.IdPersona;
                            poRegAjuste.Anio = poItem.Anio;
                            poRegAjuste.Mes = poItem.Mes;
                            poRegAjuste.CodigoTipoBeneficioSocial = poItem.CodigoTipoBeneficioSocial;
                            poRegAjuste.UsuarioIngreso = tsUsuario;
                            poRegAjuste.FechaIngreso = DateTime.Now;
                            poRegAjuste.TerminalIngreso = tsTerminal;
                            poRegAjuste.IdEmpleadoContrato = poItem.IdEmpleadoContrato ?? 0;

                            if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionFondoReserva)
                            {
                                poRegistro.Valor = toObject.ProvisionFondoReserva;
                                poRegAjuste.Valor = poRegistro.Valor - pdcValorProvision;
                            }
                            else if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero)
                            {
                                poRegistro.Valor = toObject.ProvisionDecimoTercero;
                                poRegAjuste.Valor = poRegistro.Valor - pdcValorProvision;

                            }
                            else if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionVacaciones)
                            {
                                poRegistro.Valor = toObject.ProvisionVacaciones;
                                poRegAjuste.Valor = poRegistro.Valor - pdcValorProvision;
                                ValorAJusteVac = poRegAjuste.Valor;
                            }

                            poRegistro.CodigoTipoBeneficioSocial = poItem.CodigoTipoBeneficioSocial;
                            poRegistro.CodigoRubro = poItem.CodigoRubro;
                            poRegistro.EnNomina = poItem.EnNomina;
                            poRegistro.UsuarioIngreso = tsUsuario;
                            poRegistro.FechaIngreso = DateTime.Now;
                            poRegistro.TerminalIngreso = tsTerminal;
                            poRegistro.TotalIngresos = toObject.TotalIngresos;
                            poRegistro.Dias = poItem.Dias;
                            poRegistro.IdEmpleadoContrato = poItem.IdEmpleadoContrato;

                            psCodigosIngresados.Add(poItem.CodigoTipoBeneficioSocial);
                        }
                    }
                }
                else
                {
                    if (!psCodigosIngresadosLiquidados.Contains(poItem.CodigoTipoBeneficioSocial))
                    {
                        var poObject1 = loBaseDa.Get<REHTAJUSTEPROVISION>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Anio == poItem.Anio && x.Mes == poItem.Mes && x.IdPersona == poItem.IdPersona && x.CodigoTipoBS == poItem.CodigoTipoBeneficioSocial && !x.Ajustado).FirstOrDefault();
                        if (poObject1 != null)
                        {
                            poObject1.CodigoEstado = Diccionario.Inactivo;
                        }

                        var poObject = loBaseDa.Get<REHTBENEFICIOSOCIALAJUSTE>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Anio == poItem.Anio && x.Mes == poItem.Mes && x.IdPersona == poItem.IdPersona && x.CodigoTipoBeneficioSocial == poItem.CodigoTipoBeneficioSocial).FirstOrDefault();
                        if (poObject != null)
                        {
                            poObject.CodigoEstado = Diccionario.Inactivo;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.UsuarioModificacion = tsUsuario;
                        }

                        var poRegAjuste = new REHTBENEFICIOSOCIALAJUSTE();
                        loBaseDa.CreateNewObject(out poRegAjuste);
                        poRegAjuste.CodigoEstado = Diccionario.Activo;
                        poRegAjuste.IdPersona = poItem.IdPersona;
                        poRegAjuste.Anio = poItem.Anio;
                        poRegAjuste.Mes = poItem.Mes;
                        poRegAjuste.CodigoTipoBeneficioSocial = poItem.CodigoTipoBeneficioSocial;
                        poRegAjuste.UsuarioIngreso = tsUsuario;
                        poRegAjuste.FechaIngreso = DateTime.Now;
                        poRegAjuste.TerminalIngreso = tsTerminal;
                        poRegAjuste.IdEmpleadoContrato = poItem.IdEmpleadoContrato ?? 0;

                        if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionFondoReserva)
                        {
                            poRegAjuste.Valor = toObject.ProvisionFondoReserva - pdcValorProvision;
                        }
                        else if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero)
                        {
                            poRegAjuste.Valor = toObject.ProvisionDecimoTercero - pdcValorProvision;
                        }
                        else if (poItem.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionVacaciones)
                        {
                            poRegAjuste.Valor = toObject.ProvisionVacaciones - pdcValorProvision;
                            ValorAJusteVac = poRegAjuste.Valor;
                        }

                        psCodigosIngresadosLiquidados.Add(poItem.CodigoTipoBeneficioSocial);
                    }
                }
            }

            foreach (var poItem in poListaAjustar)
            {
                var poRegistroAjuste = new REHTAJUSTEPROVISION();
                loBaseDa.CreateNewObject(out poRegistroAjuste);
                poRegistroAjuste.IdPersona = poItem.IdPersona;
                poRegistroAjuste.Cedula = poItem.Cedula;
                poRegistroAjuste.Nombre = poItem.Nombre;
                poRegistroAjuste.Anio = poItem.Año;
                poRegistroAjuste.Mes = poItem.Mes;
                poRegistroAjuste.CodigoTipoBS = poItem.CodigoTipoBeneficioSocial;
                poRegistroAjuste.TotalIngresos = poItem.TotalIngresos;
                poRegistroAjuste.Provision += poItem.Valor;
                poRegistroAjuste.TotalIngresosAjuste = poItem.TotalIngresosPreliminar;
                poRegistroAjuste.ProvisionAjuste = poItem.ValorPreliminar;
                poRegistroAjuste.ProvisionLiq = poItem.ProvisionLiq;
                poRegistroAjuste.Ajustado = poItem.Ajustado;
                poRegistroAjuste.CodigoEstado = Diccionario.Activo;

            }

            using (var poTran = new TransactionScope())
            {
                if (!poCalculoPreliminar.ProvisionVacLiq)
                {
                    loBaseDa.ExecuteQuery(string.Format("EXEC REHSPREPROCESOPROVISIONVACACIONES {0},{1},{2},{3},'0',{4}", toObject.IdPersona, toObject.Anio, toObject.Mes, toObject.TotalIngresos, ValorAJusteVac));
                }

                loBaseDa.SaveChanges();
                poTran.Complete();
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        private string lbValidaActualizaProvisionBS(BeneficioSocialDetalleAjuste toObject)
        {
            string psMsg = string.Empty;

            if (toObject.IdPersona.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Selecione Persona.\n", psMsg);
            }

            if (toObject.Anio == 0)
            {
                psMsg = string.Format("{0}Ingrese el año.\n", psMsg);
            }

            if (toObject.Mes == 0)
            {
                psMsg = string.Format("{0}Ingrese el mes.\n", psMsg);
            }

            if (toObject.TotalIngresos == 0)
            {
                psMsg = string.Format("{0}No es posible guardar un Total Ingresos en 0.\n", psMsg);
            }

            return psMsg;
        }

        private DataSet ldsRol(int tIdPeriodo, int tIdPersona)
        {

            DataSet ds = new DataSet();
            ds = loBaseDa.DataSet("EXEC REHSPENVIOCORREOROL @IdPeriodo = @paramIdPeriodo, @ParaIdPersona = @paramParaIdPersona, @SoloConsulta = @paramSoloConsulta ",
            new SqlParameter("paramIdPeriodo", SqlDbType.Int) { Value = tIdPeriodo },
            new SqlParameter("paramParaIdPersona", SqlDbType.Int) { Value = tIdPersona },
            new SqlParameter("paramSoloConsulta", SqlDbType.Bit) { Value = true });

            return ds;
        }

       
        #region Fondo de Reserva

        public List<FondoReserva> gConsultarFondoReserva(int tIdPeriodo)
        {
            return (from a in loBaseDa.Find<REHTFONDORESERVA>()
                    join b in loBaseDa.Find<REHTFONDORESERVADETALLE>() on a.IdFondoReserva equals b.IdFondoReserva
                    join c in loBaseDa.Find<GENMESTADO>() on b.CodigoEstado equals c.CodigoEstado
                    where a.idPeriodo == tIdPeriodo && a.CodigoEstado == Diccionario.Activo
                    select new FondoReserva()
                    {
                        IdFondoReservaDetalle = b.IdFondoReservaDetalle,
                        IdPeriodo = a.idPeriodo,
                        IdFondoReserva = a.IdFondoReserva,
                        Nombre = b.Nombre,
                        Cedula = b.Cedula,
                        CodigoEstado = b.CodigoEstado,
                        DescEstado = c.Descripcion,
                        TieneDerecho = b.TieneDerecho,
                        TieneSolicitud = b.TieneSolicitud,

                    }).ToList().OrderBy(x => x.Nombre).ToList();
        }

        public bool gGuardaFondoReserva(int tiPeriodo, string tsUsuario, string tsTerminal, List<FondoReserva> toLista)
        {
            loBaseDa.CreateContext();

            var poListaEmpleadoContratoActivo = (from a in loBaseDa.Find<GENMPERSONA>()
                                                 join b in loBaseDa.Find<REHPEMPLEADOCONTRATO>() on a.IdPersona equals b.IdPersona
                                                 where b.CodigoEstado == Diccionario.Activo
                                                 select new
                                                 {
                                                     a.IdPersona,
                                                     a.NumeroIdentificacion,
                                                     b.IdEmpleadoContrato
                                                 }).ToList();

            var poObject = loBaseDa.Get<REHTFONDORESERVA>().Where(x => x.idPeriodo == tiPeriodo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Inactivo;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                poObject.FechaModificacion = DateTime.Now;
            }

            REHTFONDORESERVA poRegistro = new REHTFONDORESERVA();
            loBaseDa.CreateNewObject(out poRegistro);
            poRegistro.CodigoEstado = Diccionario.Activo;
            poRegistro.idPeriodo = tiPeriodo;
            poRegistro.Total = toLista.Count();
            poRegistro.UsuarioIngreso = tsUsuario;
            poRegistro.TerminalIngreso = tsTerminal;
            poRegistro.FechaIngreso = DateTime.Now;


            foreach (var item in toLista)
            {
                REHTFONDORESERVADETALLE poDetalle = new REHTFONDORESERVADETALLE();
                loBaseDa.CreateNewObject(out poDetalle);
                poDetalle.Cedula = item.Cedula;
                string vr = "";
                if (item.Cedula.Contains("0904475605")) vr = "";
                // Se deja en estado pendiente si no existe empleado con contrato activo
                int pIdContrato = poListaEmpleadoContratoActivo.Where(x => x.NumeroIdentificacion.Contains(item.Cedula)).Select(x => x.IdEmpleadoContrato).FirstOrDefault();

                var poContrato = loBaseDa.Get<REHPEMPLEADOCONTRATO>().Where(x => x.IdEmpleadoContrato == pIdContrato).FirstOrDefault();
                if (poContrato != null)
                {
                    bool pbSolicitudFondoReserva = item.TieneSolicitud == "SI" ? true : false;
                    bool pbDerechoFondoReserva = item.TieneDerecho == "SI" ? true : false;

                    if (poContrato.SolicitudFondoReserva != pbSolicitudFondoReserva)
                        poContrato.SolicitudFondoReserva = pbSolicitudFondoReserva;

                    if (poContrato.DerechoFondoReserva != pbDerechoFondoReserva)
                        poContrato.DerechoFondoReserva = pbDerechoFondoReserva;

                    poContrato.UsuarioIngreso = tsUsuario;
                    poContrato.TerminalIngreso = tsTerminal;
                    poContrato.FechaIngreso = DateTime.Now;

                    poDetalle.CodigoEstado = Diccionario.Activo;
                }
                else
                {
                    poDetalle.CodigoEstado = Diccionario.Pendiente;
                }


                poDetalle.Nombre = item.Nombre;
                poDetalle.TieneDerecho = item.TieneDerecho;
                poDetalle.TieneSolicitud = item.TieneSolicitud;
                poDetalle.UsuarioIngreso = tsUsuario;
                poDetalle.TerminalIngreso = tsTerminal;
                poDetalle.FechaIngreso = DateTime.Now;

                poRegistro.REHTFONDORESERVADETALLE.Add(poDetalle);

            }

            loBaseDa.SaveChanges();
            return true;
        }
        #endregion 

        public List<ConsolidadoComparativo> goAjustarSueldos(List<ConsolidadoGrid> toLista, string tsUsuario, string tsTerminal)
        {
            List<ConsolidadoComparativo> poLista = new List<ConsolidadoComparativo>();

            var psLIstaCedula = toLista.Select(x => x.Cedula).ToList();
            var poListaPersonas = loBaseDa.Find<GENMPERSONA>().Where(x => psLIstaCedula.Contains(x.NumeroIdentificacion)).Select(x => new { x.IdPersona, x.NumeroIdentificacion }).ToList();
            var piListaIdPersona = poListaPersonas.Select(x => x.IdPersona).ToList();
            var poBenefSocial = loBaseDa.Find<REHTBENEFICIOSOCIALDETALLE>()
                    .Where(x => x.CodigoEstado == Diccionario.Activo && piListaIdPersona.Contains(x.IdPersona)
                    ).Select(x => new
                    {
                        x.IdPersona,
                        x.Anio,
                        x.Mes,
                        x.CodigoTipoBeneficioSocial,
                        x.CodigoRubro,
                        x.Dias,
                        x.Valor,
                        x.TotalIngresos
                    }).ToList();

            foreach (var item in toLista)
            {

                var poComparativo = new ConsolidadoComparativo();

                poComparativo.IdPersona = poListaPersonas.Where(x => x.NumeroIdentificacion == item.Cedula).Select(x => x.IdPersona).FirstOrDefault();
                poComparativo.Año = item.Año;
                poComparativo.Mes = item.Mes;
                poComparativo.Cedula = item.Cedula;
                poComparativo.Nombre = item.Nombre;

                poComparativo.TotalIngresos = poBenefSocial.Where(x => x.IdPersona == poComparativo.IdPersona && x.Anio == item.Año && x.Mes == item.Mes && x.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero).Sum(x => x.TotalIngresos) ?? 0;
                poComparativo.ProvisionVacaciones = poBenefSocial.Where(x => x.IdPersona == poComparativo.IdPersona && x.Anio == item.Año && x.Mes == item.Mes && x.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionVacaciones).Sum(x => x.Valor);
                poComparativo.ProvisionFondoReserva = poBenefSocial.Where(x => x.IdPersona == poComparativo.IdPersona && x.Anio == item.Año && x.Mes == item.Mes && x.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionFondoReserva).Sum(x => x.Valor);
                poComparativo.ProvisionDecimoTercero = poBenefSocial.Where(x => x.IdPersona == poComparativo.IdPersona && x.Anio == item.Año && x.Mes == item.Mes && x.CodigoTipoBeneficioSocial == Diccionario.ListaCatalogo.TipoBeneficioSocialClass.ProvisionDecimoTercero).Sum(x => x.Valor);

                var poCalculoPreliminar = new BeneficioSocialDetalleAjuste();

                poCalculoPreliminar = goCalcularProvisionEmpleado(poComparativo.IdPersona, item.Año, item.Mes, item.Sueldo);

                poComparativo.TotalIngresosPreliminar = item.Sueldo;
                poComparativo.ProvisionVacacionesPreliminar = poCalculoPreliminar.ProvisionVacaciones;
                poComparativo.ProvisionDecimoTerceroPreliminar = poCalculoPreliminar.ProvisionDecimoTercero;
                poComparativo.ProvisionFondoReservaPreliminar = poCalculoPreliminar.ProvisionFondoReserva;

                poComparativo.AjustadoDec = poCalculoPreliminar.AjustadoDec;
                poComparativo.AjustadoFon = poCalculoPreliminar.AjustadoFon;
                poComparativo.AjustadoVac = poCalculoPreliminar.AjustadoVac;

                poComparativo.ProvisionDecLiq = poCalculoPreliminar.ProvisionDecLiq;
                poComparativo.ProvisionFonLiq = poCalculoPreliminar.ProvisionFonLiq;
                poComparativo.ProvisionVacLiq = poCalculoPreliminar.ProvisionVacLiq;


                poLista.Add(poComparativo);
            }




            return poLista;
        }

        public DataTable gdtConsultaPersonaCuentaBancaria()
        {
            string psQuery =
                "SELECT "
                + "	EC.IdEmpleadoContrato Id,"
                + "	P.NumeroIdentificacion  Identificación,"
                + "	NombreCompleto Empleado,"
                + "	CBA.Descripcion Banco,"
                + "	CFP.Descripcion FormaPago,"
                + "	CTC.Descripcion TipoCuenta,"
                + "	EC.NumeroCuenta"
                + " FROM "
                + "	GENMPERSONA P WITH (NOLOCK) "
                + "	INNER JOIN REHPEMPLEADOCONTRATO EC WITH (NOLOCK) ON P.IdPersona = EC.IdPersona"
                + "	INNER JOIN GENMCATALOGO AS CBA WITH (NOLOCK) ON CBA.CodigoGrupo = '016' AND EC.CodigoBanco = CBA.Codigo"
                + "	INNER JOIN GENMCATALOGO AS CFP WITH (NOLOCK) ON CFP.CodigoGrupo = '017' AND EC.CodigoFormaPago = CFP.Codigo"
                + "	INNER JOIN GENMCATALOGO AS CTC WITH (NOLOCK) ON CTC.CodigoGrupo = '018' AND EC.CodigoTipoCuentaBancaria = CTC.Codigo"
                + " WHERE "
                + "	P.CodigoEstado = 'A'"
                + " ORDER BY"
                + "	P.NombreCompleto";

            return loBaseDa.DataTable(psQuery);
        }

        #region Alimentación
        /// <summary>
        /// LLamada al Sp para generar Alimentación
        /// </summary>
        /// <param name="tiPeriodo"></param>
        /// <param name="tsUsuario"></param>
        /// <returns>Retorna un string: de estar vacio se ejecutó correctamente, caso contrario muestra mensaje de error</returns>
        public string gsGeneraAlimentacion(int tiAnio, int tiMes, string tsCodigoTipoRol, string tsUsuario)
        {
            string psMsg = "";
            psMsg = loBaseDa.ExecStoreProcedure<string>
               ("REHSPGENERAALIMENTACION @Anio, @Mes, @CodigoTipoRol, @Usuario",
               new SqlParameter("@Anio", tiAnio),
               new SqlParameter("@Mes", tiMes),
               new SqlParameter("@CodigoTipoRol", tsCodigoTipoRol),
               new SqlParameter("@Usuario", tsUsuario)
               ).FirstOrDefault();
            return psMsg;
        }

        public string gsGuardarAlimentacion(List<AlimentacionDetalle> toLista, int tiAnio, int tiMes, string tsCodigoTipoRol, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            var poListaEmpleado = loBaseDa.Find<REHVTPERSONACONTRATO>();

            if (toLista != null && toLista.Count > 0)
            {
                var poObjectAlimentacion = loBaseDa.Get<REHTALIMENTACION>().Where(x => x.CodigoEstado == Diccionario.Activo && x.Anio == tiAnio &&
                x.Mes == tiMes && x.CodigoTipoRol == tsCodigoTipoRol).FirstOrDefault();
                if (poObjectAlimentacion != null)
                {
                    poObjectAlimentacion.UsuarioModificacion = tsUsuario;
                    poObjectAlimentacion.FechaModificacion = DateTime.Now;
                    poObjectAlimentacion.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObjectAlimentacion = new REHTALIMENTACION();
                    loBaseDa.CreateNewObject(out poObjectAlimentacion);
                    poObjectAlimentacion.UsuarioIngreso = tsUsuario;
                    poObjectAlimentacion.FechaIngreso = DateTime.Now;
                    poObjectAlimentacion.TerminalIngreso = tsTerminal;
                    poObjectAlimentacion.Anio = tiAnio;
                    poObjectAlimentacion.Mes = tiMes;
                    poObjectAlimentacion.CodigoTipoRol = tsCodigoTipoRol;
                }

                poObjectAlimentacion.DiasLaborables = toLista.Max(x => x.Dias);
                poObjectAlimentacion.CodigoEstado = Diccionario.Activo;
                poObjectAlimentacion.Total = toLista.Sum(x => x.Total);
                
                int pIdAlimentacion = poObjectAlimentacion.IdAlimentacion;

                var poLista = loBaseDa.Get<REHTALIMENTACIONDETALLE>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdAlimentacion == pIdAlimentacion).ToList();

                var piListaIdPresentacion = toLista.Where(x => x.IdAlimentacionDetalle != 0).Select(x => x.IdAlimentacionDetalle).ToList();

                foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdAlimentacionDetalle)))
                {
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdAlimentacionDetalle == poItem.IdAlimentacionDetalle && x.IdAlimentacionDetalle != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new REHTALIMENTACIONDETALLE();
                        //loBaseDa.CreateNewObject(out poObject);
                        poObjectAlimentacion.REHTALIMENTACIONDETALLE.Add(poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Dias = poItem.Dias;
                    poObject.Identificacion = poItem.Identificacion;
                    poObject.IdPersona = poItem.IdPersona;
                    poObject.Nombre = poItem.Nombre;
                    poObject.Observacion = poItem.Observacion;
                    poObject.Total = poItem.Total;
                    poObject.ValorAlimentacion = poItem.ValorAlimentacion;
                    poObject.Observacion = poItem.Observacion;
                    poObject.Cargo = poListaEmpleado.Where(x => x.IdPersona == poItem.IdPersona).Select(x => x.CargoLaboral).FirstOrDefault();
                    poObject.Sucursal = poListaEmpleado.Where(x => x.IdPersona == poItem.IdPersona).Select(x => x.Sucursal).FirstOrDefault();
                }

                loBaseDa.SaveChanges();

            }

            return psMsg;
        }

        /// <summary>
        /// Consulta alimentación guardada en el sistema
        /// </summary>
        /// <param name="tiAnio"></param>
        /// <param name="tiMes"></param>
        /// <param name="tsCodigoTipoRol"></param>
        /// <returns></returns>
        public Alimentacion goConsultarAlimentacion(int tiAnio, int tiMes, string tsCodigoTipoRol)
        {
            return (from a in loBaseDa.Find<REHTALIMENTACION>()
                    where a.CodigoEstado == Diccionario.Activo
                    && a.Anio == tiAnio && a.Mes == tiMes && a.CodigoTipoRol == tsCodigoTipoRol
                    select new Alimentacion()
                    {
                        IdAlimentacion = a.IdAlimentacion,
                        Anio = a.Anio,
                        CodigoTipoRol = a.CodigoTipoRol,
                        Mes = a.Mes,
                        Total = a.Total,
                        Observacion = a.Observacion,
                        Dias = a.DiasLaborables??0
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Consulta alimentación guardada en el sistema
        /// </summary>
        /// <param name="tiAnio"></param>
        /// <param name="tiMes"></param>
        /// <param name="tsCodigoTipoRol"></param>
        /// <returns></returns>
        public Alimentacion goConsultarAlimentacion(int tIdALimentacion)
        {
            return (from a in loBaseDa.Find<REHTALIMENTACION>()
                    where a.CodigoEstado == Diccionario.Activo
                    && a.IdAlimentacion == tIdALimentacion
                    select new Alimentacion()
                    {
                        IdAlimentacion = a.IdAlimentacion,
                        Anio = a.Anio,
                        CodigoTipoRol = a.CodigoTipoRol,
                        Mes = a.Mes,
                        Total = a.Total,
                        Observacion = a.Observacion,
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Consulta alimentación guardada en el sistema
        /// </summary>
        /// <param name="tiAnio"></param>
        /// <param name="tiMes"></param>
        /// <param name="tsCodigoTipoRol"></param>
        /// <returns></returns>
        public List<Alimentacion> goConsultarListaAlimentacion()
        {
            return (from a in loBaseDa.Find<REHTALIMENTACION>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.CodigoEstado == Diccionario.Activo
                    select new Alimentacion()
                    {
                        IdAlimentacion = a.IdAlimentacion,
                        Anio = a.Anio,
                        CodigoTipoRol = a.CodigoTipoRol,
                        Mes = a.Mes,
                        Total = a.Total,
                        Observacion = a.Observacion,
                        FechaIngreso = a.FechaIngreso,
                        DesTipoRol = b.Descripcion
                    }).ToList();
        }

        /// <summary>
        /// Consulta detalle de alimentaciñón guardada en el sistema
        /// </summary>
        /// <param name="tiAnio"></param>
        /// <param name="tiMes"></param>
        /// <param name="tsCodigoTipoRol"></param>
        /// <returns></returns>
        public List<AlimentacionDetalle> goConsultarAlimentacionDetalle(int tiAnio, int tiMes, string tsCodigoTipoRol)
        {
            return (from a in loBaseDa.Find<REHTALIMENTACION>()
                    join b in loBaseDa.Find<REHTALIMENTACIONDETALLE>()
                    on a.IdAlimentacion equals b.IdAlimentacion
                    where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo
                    && a.Anio == tiAnio && a.Mes == tiMes && a.CodigoTipoRol == tsCodigoTipoRol
                    select new AlimentacionDetalle()
                    {
                        IdAlimentacionDetalle = b.IdAlimentacionDetalle,
                        Dias = b.Dias,
                        IdAlimentacion = b.IdAlimentacion,
                        Identificacion = b.Identificacion,
                        IdPersona = b.IdPersona,
                        Nombre = b.Nombre,
                        Observacion = b.Observacion,
                        ValorAlimentacion = b.ValorAlimentacion
                    }).ToList();
        }

        public bool gbEliminarAlimentacion(int tiAnio, int tiMes, string tsCodigoTipoRol, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            var poObject = loBaseDa.Get<REHTALIMENTACION>().Include(x => x.REHTALIMENTACIONDETALLE).Where(a => a.CodigoEstado == Diccionario.Activo
             && a.Anio == tiAnio && a.Mes == tiMes && a.CodigoTipoRol == tsCodigoTipoRol).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                poObject.FechaModificacion = DateTime.Now;

                foreach (var poItem in poObject.REHTALIMENTACIONDETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
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

        #endregion

        #region Rubro Fijo
        public string gsGuardarRubroFijo(List<RubroFijo> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            loBaseDa.CreateContext();

            if (toLista != null && toLista.Count > 0)
            {

                var poLista = loBaseDa.Get<REHPRUBROFIJO>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

                var poListaPersona = loBaseDa.Find<GENMPERSONA>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new { x.IdPersona, x.NumeroIdentificacion, x.NombreCompleto }).ToList();

                var piListaIdPresentacion = toLista.Where(x => x.IdRubroFijo != 0).Select(x => x.IdRubroFijo).ToList();

                foreach (var poItem in poLista.Where(x => !piListaIdPresentacion.Contains(x.IdRubroFijo)))
                {
                    poItem.CodigoEstado = Diccionario.Inactivo;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                foreach (var poItem in toLista)
                {
                    var poObject = poLista.Where(x => x.IdRubroFijo == poItem.IdRubroFijo && x.IdRubroFijo != 0).FirstOrDefault();
                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;
                    }
                    else
                    {
                        poObject = new REHPRUBROFIJO();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.TerminalIngreso = tsTerminal;
                    }
                    
                    poItem.IdPersona = int.Parse(poItem.IdPersonaString);
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.IdPersona = poItem.IdPersona;
                    poObject.CodigoRubro = poItem.CodigoRubro;
                    poObject.CodigoTipoRol = poItem.CodigoTipoRol;
                    poObject.Identificacion = poListaPersona.Where(x=>x.IdPersona == poItem.IdPersona).Select(x=>x.NumeroIdentificacion).FirstOrDefault();
                    poObject.Nombre = poListaPersona.Where(x => x.IdPersona == poItem.IdPersona).Select(x => x.NombreCompleto).FirstOrDefault();
                    poObject.Observacion = poItem.Observacion;
                    poObject.Valor = poItem.Valor;

                }

                loBaseDa.SaveChanges();

            }

            return psMsg;
        }

        
        /// <summary>
        /// Consulta Rubro Fijo guardada en el sistema
        /// </summary>
        /// <param name="tiAnio"></param>
        /// <param name="tiMes"></param>
        /// <param name="tsCodigoTipoRol"></param>
        /// <returns></returns>
        public RubroFijo goConsultarRubroFijo(int tId)
        {
            return (from a in loBaseDa.Find<REHPRUBROFIJO>()
                    where a.CodigoEstado == Diccionario.Activo
                    && a.IdRubroFijo == tId
                    select new RubroFijo()
                    {
                        CodigoRubro = a.CodigoRubro,
                        Identificacion = a.Identificacion,
                        CodigoTipoRol = a.CodigoTipoRol,
                        IdPersona = a.IdPersona,
                        IdPersonaString = a.IdPersona.ToString(),
                        Valor = a.Valor,
                        Nombre = a.Nombre,
                        IdRubroFijo = a.IdRubroFijo,
                        Observacion = a.Observacion,

                    }).FirstOrDefault();
        }

        /// <summary>
        /// Consulta Rubro Fijo guardada en el sistema
        /// </summary>
        /// <param name="tiAnio"></param>
        /// <param name="tiMes"></param>
        /// <param name="tsCodigoTipoRol"></param>
        /// <returns></returns>
        public List<RubroFijo> goConsultarListaRubroFijo()
        {
            return (from a in loBaseDa.Find<REHPRUBROFIJO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.CodigoEstado == Diccionario.Activo
                    select new RubroFijo()
                    {
                        CodigoRubro = a.CodigoRubro,
                        Identificacion = a.Identificacion,
                        CodigoTipoRol = a.CodigoTipoRol,
                        IdPersona = a.IdPersona,
                        IdPersonaString = a.IdPersona.ToString(),
                        Valor = a.Valor,
                        Nombre = a.Nombre,
                        IdRubroFijo = a.IdRubroFijo,
                        Observacion = a.Observacion
                    }).ToList();
        }

        #endregion

        /// <summary>
        /// Buscar Codigo de la Entidad
        /// </summary>
        /// <param name="tsTipo">P: Primero, A: Anterior, S: Siguiente, U: Último</param>
        /// <param name="tsCodigo">Codigo de la entidad</param>
        /// <returns></returns>
        public int goBuscarCodigo(string tsTipo, string tsCodigo = "")
        {
            int psCodigo = 0;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<REHTLIQUIDACION>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdLiquidacion }).OrderBy(x => x.IdLiquidacion).FirstOrDefault().IdLiquidacion;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<REHTLIQUIDACION>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdLiquidacion }).OrderByDescending(x => x.IdLiquidacion).FirstOrDefault().IdLiquidacion;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<REHTLIQUIDACION>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdLiquidacion }).ToList().Where(x => x.IdLiquidacion < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdLiquidacion).FirstOrDefault().IdLiquidacion;
                }
                else
                {
                    psCodigo = int.Parse(tsCodigo);
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<REHTLIQUIDACION>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdLiquidacion }).ToList().Where(x => x.IdLiquidacion > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdLiquidacion).FirstOrDefault().IdLiquidacion;
                }
                else
                {
                    psCodigo = int.Parse(tsCodigo);
                }
            }
            return psCodigo;

        }

    }


    public class lListNovedad
    {
        public int Index { get; set; }
        public string Identificacion { get; set; }
    }
}

