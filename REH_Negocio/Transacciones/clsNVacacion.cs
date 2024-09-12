using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using GEN_Entidad;
using System.Data;
using System.Data.Entity;
using System.Transactions;
using GEN_Negocio;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 26/05/2021
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNVacacion : clsNBase
    {

        /// <summary>
        /// Consulta de Datos del las solicitudes de vacaciones 
        /// </summary>
        /// <returns></returns>
        public List<SolicitudVacacion> goListar(string tsUsuario, int tIdMenu)
        {
            return (from a in loBaseDa.Find<REHTSOLICITUDVACACION>()
                    join b in loBaseDa.Find<GENMPERSONA>() on a.IdPersona equals b.IdPersona
                    join c in loBaseDa.Find<GENMCATALOGO>()
                    on new { CodigoGrupo = Diccionario.ListaCatalogo.TipoVacacion, Codigo = a.CodigoTipoVacacion }
                    equals new { c.CodigoGrupo, c.Codigo }
                    join e in loBaseDa.Find<SEGPUSUARIOPERSONAASIGNADO>() on a.IdPersona equals e.IdPersona
                    where a.CodigoEstado != Diccionario.Eliminado
                    && e.CodigoEstado == Diccionario.Activo && e.IdMenu == tIdMenu && e.CodigoUsuario == tsUsuario
                    select new SolicitudVacacion
                    {
                        Id = a.IdSolicitudVacacion,
                        IdPersona = a.IdPersona,
                        CodigoEstado = a.CodigoEstado,
                        FechaInicio = a.FechaInicio,
                        FechaFin = a.FechaFin,
                        CodigoTipoVacacion = a.CodigoTipoVacacion,
                        DesTipoVacacion = c.Descripcion,
                        DesPersona = b.NombreCompleto,
                        NumeroIdentificacion = b.NumeroIdentificacion,
                        Fecha = a.FechaModificacion == null ? a.FechaIngreso : a.FechaModificacion ?? DateTime.Now,
                        Dias = a.Dias,
                        IdPersonaReemplazo = a.IdPersonaReemplazo,
                        PagarReemplazo = a.PagarReemplazo ?? false,
                    }).OrderBy(x => x.FechaInicio).ToList();
        }

        /// <summary>
        /// Guardar Objeto Solciitud Vacación
        /// </summary>
        /// <param name="toObject"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gbGuardar(SolicitudVacacion toObject, string tsUsuario, string tsTerminal, out int tId)
        {
            tId = 0;
            loBaseDa.CreateContext();
            var poObject = loBaseDa.Get<REHTSOLICITUDVACACION>().Where(x => x.IdSolicitudVacacion == toObject.Id).FirstOrDefault();
            var poObjectContrato = loBaseDa.Find<REHVTPERSONACONTRATO>().Where(x => x.IdPersona == toObject.IdPersona).Select(x => new { x.IdEmpleadoContrato, x.Sucursal, x.Departamento, x.CargoLaboral, x.FechaInicioContrato}).FirstOrDefault();

            var psListaEstado = new List<string>();
            psListaEstado.Add(Diccionario.PreAprobado);
            psListaEstado.Add(Diccionario.Aprobado);
            psListaEstado.Add(Diccionario.Pendiente);

            var poListaFechas = loBaseDa.Get<REHTSOLICITUDVACACION>().Where(x => x.CodigoTipoVacacion == "001" && x.IdPersona == toObject.IdPersona && psListaEstado.Contains(x.CodigoEstado)).ToList();
            DateTime pdUltimaFecha = new DateTime();
            if (poListaFechas.Count > 0)
            {
                pdUltimaFecha = poListaFechas.Max(x => x.FechaFin);
            }
            

            string psResult = string.Empty;

            //En caso de existir una solicitud con fecha superior
            if (pdUltimaFecha != DateTime.MinValue)
            {
                if (pdUltimaFecha >= toObject.FechaInicio || pdUltimaFecha >= toObject.FechaFin)
                {
                    psResult = string.Format("{0}No es posible guardar solicitud, existe una solicitud donde su fecha de vacaciones es hasta {1}, Por favor elimine dicha solicitud y vuelva ingresar en el orden correcto desde la más próxima hasta la más lejana. \n", psResult, pdUltimaFecha.ToString("dd/MM/yyyy"));
                }
            }

            // Si el valor es 0 no existe registro en base de datos
            var pIdPersona = loBaseDa.Find<REHPEMPLEADO>().Where(x => x.IdPersona == toObject.IdPersona).Select(x => x.IdPersona).FirstOrDefault();
            if (pIdPersona == 0) psResult = string.Format("{0}Código de Empleado no existe, Código: {1} \n", psResult, toObject.IdPersona);

            // Si el valor es vacio no existe registro en base de datos
            var psCodigoTipoVacacion = loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo ==  Diccionario.ListaCatalogo.TipoVacacion && x.Codigo == toObject.CodigoTipoVacacion).Select(x => x.Codigo).FirstOrDefault();
            if (string.IsNullOrEmpty(psCodigoTipoVacacion)) psResult = string.Format("{0}Código Tipo de vacación no existe, Código: {1} \n", psResult, toObject.CodigoTipoVacacion);

            //Validación de última nómina cerrada
            var pdtUltimaFechaNominaCerrada = gdtConsultaFechaUltimoPeriodoCerrado();
            if (pdtUltimaFechaNominaCerrada != null)
            {
                if (pdtUltimaFechaNominaCerrada.Value.Date >= toObject.FechaInicio.Date)
                {
                    psResult = string.Format("{0}Último Periodo de Nómina Cerrada tiene fecha: {1}, su periodo de vacaciones debe ser superior a esta fecha. Rango de fechas seleccionadas de: {2} a {3} \n", psResult, pdtUltimaFechaNominaCerrada.Value.ToString("dd/MM/yyyy"), toObject.FechaInicio.ToString("dd/MM/yyyy"), toObject.FechaFin.ToString("dd/MM/yyyy"));
                }
            }

            if (toObject.FechaInicio > toObject.FechaFin)
            {
                psResult = string.Format("{0}Rango de fechas inconsistentes, la fecha de inicio no puede ser mayor a la fecha fin., FechaInicio: {1} - FechaFin: {2} \n", psResult, toObject.FechaInicio.ToString("dd/MM/yyyy"), toObject.FechaFin.ToString("dd/MM/yyyy"));
            }

            bool pbSinValor = false;

            var piSaldoVacacion = giConsultaSaldoVacacion(pIdPersona);
            var piDiasTomar = toObject.FechaFin.Date.Subtract(toObject.FechaInicio.Date).Days + 1;

            if (piDiasTomar > piSaldoVacacion)
            {
                psResult = string.Format("{0}Los días a tomar: {1}, no puede ser mayor que la Cantidad Máxima por Tomar: {2}. Verifique si tiene Solicitudes Pendientes \n", psResult, piDiasTomar, piSaldoVacacion);
                /*
                var pdFechaInicio = new DateTime();
                var piSaldoVacacionSinValor = giConsultaSaldoVacacionSInValor(pIdPersona, out pdFechaInicio);
                var pdFechaIniPer = pdFechaInicio.AddYears(1).AddDays(-1);
                if (pdFechaInicio.Date != DateTime.MinValue.Date)
                {
                    if (piDiasTomar > piSaldoVacacionSinValor)
                    {
                        psResult = string.Format("{0}Los días a tomar: {1}, no puede ser mayor que la Cantidad Máxima por Tomar: {2}. Verifique si tiene Solicitudes Pendientes \n", psResult, piDiasTomar, piSaldoVacacionSinValor);
                    }

                    pbSinValor = true;
                }
                else
                {
                    psResult = string.Format("{0}Los días a tomar: {1}, no puede ser mayor que la Cantidad Máxima por Tomar: {2}. Verifique si tiene Solicitudes Pendientes \n", psResult, piDiasTomar, piSaldoVacacion);
                }
                */
            }

            string psCodigoTipoPersmiso = String.Empty;
            if (toObject.CodigoTipoVacacion == Diccionario.ListaCatalogo.TipoVacacionClass.Gozadas)
                psCodigoTipoPersmiso = Diccionario.Tablas.TipoPermiso.Vacaciones;
            else
                psCodigoTipoPersmiso = Diccionario.Tablas.TipoPermiso.VacacionesPagadas;

            // Crear lista detalle
            var poListaDetalle = new List<REHTSOLICITUDVACACIONDETALLE>();
            var poListaDetalleValor = new List<REHTSOLICITUDVACACIONVALOR>();
            var pdFecha = toObject.FechaInicio.Date;
            if (string.IsNullOrEmpty(psResult))
            {
                var poLista = goConsultarPreliminarVacaciones(toObject.FechaInicio, toObject.FechaFin, toObject.IdPersona, piDiasTomar);
                toObject.Dias = poLista.Sum(x=>x.Dias);
                int piContDias = 0;
                foreach (var item in poLista)
                {
                    var poDetalle = new REHTSOLICITUDVACACIONDETALLE();
                    poDetalle.CodigoEstado = Diccionario.Activo;
                    poDetalle.Dias = item.Dias;
                    poDetalle.FechaIngreso = DateTime.Now;
                    poDetalle.Periodo = item.Periodo;
                    poDetalle.TerminalIngreso = string.Empty;
                    poDetalle.UsuarioIngreso = tsUsuario;
                    poDetalle.Valor = pbSinValor ? 0 : item.Valor;

                   
                    decimal pdcAcumulador = 0M;
                    decimal pdcValorDia = Math.Round((item.Valor/ item.Dias),2);
                    bool pbFechaDesde = false;
                    DateTime pdtFechaDesde = new DateTime();
                    DateTime pdtFechaHasta = new DateTime();
                    for (int i = 0; i < item.Dias; i++)
                    {
                        if (!pbFechaDesde) pdtFechaDesde = pdFecha.AddDays(piContDias);
                        pbFechaDesde = true;
                        pdtFechaHasta = pdFecha.AddDays(piContDias);
                        if (!pbSinValor)
                        {
                            var poDetalleValor = new REHTSOLICITUDVACACIONVALOR();
                            int piMes = 0, piAnio = 0, piDia = 0;
                            decimal pdcValor = 0M;
                            if ((pdcAcumulador + pdcValorDia) <= item.Valor)
                            {
                                pdcValor = pdcValorDia;
                            }
                            else
                            {
                                pdcValor = item.Valor - pdcAcumulador;
                            }

                            //Si existe un valor de diferencia se lo pone en la última cuota

                            if (i == item.Dias - 1)
                            {
                                decimal pdcDif = item.Valor - (pdcAcumulador + pdcValorDia);
                                if (pdcDif > 0)
                                {
                                    pdcValor += pdcDif;
                                }
                            }


                            var pdFechaCalculo = pdFecha.AddDays(piContDias);
                            piDia = pdFechaCalculo.Day;
                            if (pdFechaCalculo.Day == 31)
                            {
                                piAnio = pdFechaCalculo.AddDays(1).Year;
                                piMes = pdFechaCalculo.AddDays(1).Month;
                                piDia = pdFechaCalculo.AddDays(1).Day;
                            }
                            else
                            {
                                piAnio = pdFechaCalculo.Year;
                                piMes = pdFechaCalculo.Month;
                            }

                            poDetalleValor.Fecha = pdFechaCalculo;
                            poDetalleValor.CodigoEstado = Diccionario.Activo;
                            poDetalleValor.Anio = piAnio;
                            poDetalleValor.FechaIngreso = DateTime.Now;
                            poDetalleValor.Mes = piMes;
                            poDetalleValor.Dia = piDia;
                            poDetalleValor.TerminalIngreso = string.Empty;
                            poDetalleValor.Periodo = item.Periodo;
                            poDetalleValor.UsuarioIngreso = tsUsuario;
                            poDetalleValor.Valor = pdcValor;
                            poListaDetalleValor.Add(poDetalleValor);

                            pdcAcumulador += pdcValor;
                        }
                        
                        piContDias++;
                    }
                    
                    poDetalle.DiasSaldo = item.DiasSaldo;
                    poDetalle.FechaDesde = pdtFechaDesde;
                    poDetalle.FechaHasta = pdtFechaHasta;
                    poListaDetalle.Add(poDetalle);
                }
            }

            if (string.IsNullOrEmpty(psResult))
            {
                using (var poTran = new TransactionScope())
                {
                    
                    if (poObject != null)
                    {
                        //poObject.CodigoEstado = toObject.CodigoEstado;
                        poObject.IdPersona = toObject.IdPersona;
                        poObject.CodigoTipoVacacion = toObject.CodigoTipoVacacion;
                        poObject.FechaInicio = toObject.FechaInicio;
                        poObject.FechaFin = toObject.FechaFin;
                        poObject.Observacion = toObject.Observacion;
                        poObject.Reemplazo = toObject.Reemplazo;
                        poObject.IdPersonaReemplazo = toObject.IdPersonaReemplazo;
                        poObject.PagarReemplazo = toObject.PagarReemplazo;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.TerminalModificacion = tsTerminal;
                        poObject.Sucursal = poObjectContrato.Sucursal;
                        poObject.Departamento = poObjectContrato.Departamento;
                        poObject.Cargo = poObjectContrato.CargoLaboral;
                        poObject.IdEmpleadoContrato = poObjectContrato.IdEmpleadoContrato;
                        poObject.DetalleValorPendiente = pbSinValor;
                        
                        // Insert Auditoría
                        loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                    }
                    else
                    {
                        poObject = new REHTSOLICITUDVACACION();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.IdPersona = toObject.IdPersona;
                        poObject.CodigoTipoVacacion = toObject.CodigoTipoVacacion;
                        poObject.FechaInicio = toObject.FechaInicio;
                        poObject.FechaFin = toObject.FechaFin;
                        poObject.Observacion = toObject.Observacion;
                        poObject.Reemplazo = toObject.Reemplazo;
                        poObject.IdPersonaReemplazo = toObject.IdPersonaReemplazo;
                        poObject.PagarReemplazo = toObject.PagarReemplazo;
                        poObject.Dias = toObject.Dias;
                        poObject.FechaIngreso = DateTime.Now;
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.TerminalIngreso = tsTerminal;
                        poObject.Sucursal = poObjectContrato.Sucursal;
                        poObject.Departamento = poObjectContrato.Departamento;
                        poObject.Cargo = poObjectContrato.CargoLaboral;
                        poObject.IdEmpleadoContrato = poObjectContrato.IdEmpleadoContrato;
                        poObject.CodigoEstado = Diccionario.Pendiente;
                        poObject.DetalleValorPendiente = pbSinValor;
                        
                        foreach (var item in poListaDetalle)
                        {
                            poObject.REHTSOLICITUDVACACIONDETALLE.Add(item);
                        }

                        foreach (var item in poListaDetalleValor)
                        {
                            poObject.REHTSOLICITUDVACACIONVALOR.Add(item);
                        }

                        foreach (DataRow item in gdtConsultaSaldoVacacion(toObject.IdPersona).Rows)
                        {
                            var poDet = new REHTSOLICITUDVACACIONSALDOS();

                            poDet.CodigoEstado = Diccionario.Activo;
                            poDet.Periodo = Convert.ToInt32(item["Periodo"].ToString());
                            poDet.Dias = Convert.ToInt32(item["Días"].ToString());
                            poDet.Valor = Convert.ToDecimal(item["Valor"].ToString());
                            poDet.FechaIngreso = DateTime.Now;
                            poDet.UsuarioIngreso = tsUsuario;
                            poDet.TerminalIngreso = tsTerminal;
                            poObject.REHTSOLICITUDVACACIONSALDOS.Add(poDet);
                        }

                        // Insert Auditoría
                        loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);

                        loBaseDa.SaveChanges();

                        //Insertar Permiso
                        REHPPERMISO poObjectPermiso = new REHPPERMISO();
                        loBaseDa.CreateNewObject(out poObjectPermiso);
                        poObjectPermiso.CodigoEstado = Diccionario.Activo;
                        poObjectPermiso.IdPersona = poObject.IdPersona;
                        poObjectPermiso.CodigoTipoPermiso = psCodigoTipoPersmiso;
                        poObjectPermiso.FechaInicio = poObject.FechaInicio;
                        poObjectPermiso.FechaFin = poObject.FechaFin;
                        poObjectPermiso.Observacion = poObject.Observacion;
                        poObjectPermiso.FechaIngreso = DateTime.Now;
                        poObjectPermiso.UsuarioIngreso = tsUsuario;
                        poObjectPermiso.TerminalIngreso = tsTerminal;
                        poObjectPermiso.IdPersonaCubre = null;
                        poObjectPermiso.IdSolicitudVacacion = poObject.IdSolicitudVacacion;
                    }
                    loBaseDa.SaveChanges();
                    poTran.Complete();
                    tId = poObject.IdSolicitudVacacion;
                }

            }
            
            return psResult;
        }


        public List<BandejaSolicitudVacaciones> goListarBandejaSolicitudVacaciones(string tsUsuario, int tiMenu)
        {
            return loBaseDa.ExecStoreProcedure<BandejaSolicitudVacaciones>(string.Format("COMSPCONSULTABANDEJASOLICITUDVACACIONES {0},'{1}'", tiMenu, tsUsuario));
        }

        public List<BandejaSolicitudVacaciones> goListarSolicitudVacaciones(string tsUsuario)
        {
            return loBaseDa.ExecStoreProcedure<BandejaSolicitudVacaciones>(string.Format("COMSPCONSULTASOLICITUDVACACIONES '{0}'", tsUsuario));
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

            var poObject = loBaseDa.Get<REHTSOLICITUDVACACION>().Where(x => x.IdSolicitudVacacion == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)

            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.SolicitudVacaciones;
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
                            }
                            else
                            {
                                psCodigoEstado = Diccionario.PreAprobado;
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
                        poObject.Comentario = "";

                        loBaseDa.SaveChanges();
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
            var poObject = loBaseDa.Get<REHTSOLICITUDVACACION>().Where(x => x.IdSolicitudVacacion == tId).FirstOrDefault();
            if (poObject != null)
            {
                if (poObject.CodigoEstado != Diccionario.Aprobado)
                {

                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.SolicitudVacaciones;
                    poTransaccion.ComentarioAprobador = Observacion;
                    poTransaccion.IdTransaccionReferencial = tId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(TipoEstadoSolicitud);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    poObject.Comentario = Observacion;
                    poObject.CodigoEstado = TipoEstadoSolicitud;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;


                    var poTran = loBaseDa.Get<REHTTRANSACCIONAUTOIZACION>().Where(x => x.CodigoTransaccion == Diccionario.Tablas.Transaccion.SolicitudVacaciones && x.CodigoEstado == Diccionario.Activo && x.IdTransaccionReferencial == tId && x.Tipo == Diccionario.TipoAprobacion.Aprobado).ToList();

                    foreach (var item in poTran)
                    {
                        item.CodigoEstado = Diccionario.Inactivo;
                        item.FechaModificacion = DateTime.Now;
                        item.UsuarioModificacion = tsUsuario;
                        item.TerminalModificacion = tsTerminal;
                    }

                    loBaseDa.SaveChanges();
                }
                else
                {
                    psMsg = string.Format("Solicitud No: {0} no puede cambiarse su estado ya que está aprobada.", tId);
                }
                

            }

           

            return psMsg;
        }


        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public SolicitudVacacion goBuscarMaestro(int tId)
        {
            var poObjectResult = new SolicitudVacacion();
            var poObject = loBaseDa.Find<REHTSOLICITUDVACACION>().Include(x => x.REHTSOLICITUDVACACIONDETALLE).Where(x => x.IdSolicitudVacacion == tId).FirstOrDefault();

            if (poObject != null)
            {
                poObjectResult.Id = poObject.IdSolicitudVacacion;
                poObjectResult.FechaInicio = poObject.FechaInicio;
                poObjectResult.FechaFin = poObject.FechaFin;
                poObjectResult.CodigoEstado = poObject.CodigoEstado;
                poObjectResult.CodigoTipoVacacion = poObject.CodigoTipoVacacion;
                poObjectResult.IdPersona = poObject.IdPersona;
                poObjectResult.Fecha = poObject.FechaIngreso;
                poObjectResult.Usuario = poObject.UsuarioIngreso;
                poObjectResult.Terminal = poObject.TerminalIngreso;
                poObjectResult.FechaMod = poObject.FechaModificacion;
                poObjectResult.UsuarioMod = poObject.UsuarioModificacion;
                poObjectResult.TerminalMod = poObject.TerminalModificacion;
                poObjectResult.Dias = poObject.Dias;
                poObjectResult.IdPersonaReemplazo = poObject.IdPersonaReemplazo;
                poObjectResult.PagarReemplazo = poObject.PagarReemplazo??false;


            }

            poObjectResult.SolicitudVacacionDetalle = new List<SolicitudVacacionDetalle>();
            foreach (var item in poObject.REHTSOLICITUDVACACIONDETALLE.Where(x => x.CodigoEstado != Diccionario.Eliminado))
            {
                var poObjectItem = new SolicitudVacacionDetalle();
                poObjectItem.CodigoEstado = item.CodigoEstado;
                poObjectItem.Dias = item.Dias;
                poObjectItem.FechaIngreso = item.FechaIngreso;
                poObjectItem.Periodo = item.Periodo;
                poObjectItem.TerminalIngreso = item.TerminalIngreso;
                poObjectItem.UsuarioIngreso = item.UsuarioIngreso;
                poObjectItem.Valor = item.Valor;
                poObjectResult.SolicitudVacacionDetalle.Add(poObjectItem);
                poObjectItem.DiasSaldo = item.DiasSaldo;
            }

            return poObjectResult;
        }

        /// <summary>
        /// Guarda valores de Días Gozados por Liquidar y Días Liquidados por Gozar en transacción de vacaciones
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public bool gbGuardarDiasObservaciones(List<PlantillaVacacion> toLista, string tsUsuario, string tsTerminal)
        {
            bool pbResult = true;

            loBaseDa.CreateContext();
            var poLista = loBaseDa.Find<REHTVACACION>().Where(x=>x.CodigoEstado == Diccionario.Activo).Select(x=> new { x.Periodo,x.IdPersona,x.DiasLiqPorGozar,x.DiasGozadosPorLiq,x.Observacion } ).ToList();

            foreach (var item in toLista)
            {
                var poRegistro = poLista.Where(x =>x.Periodo == item.Periodo && x.IdPersona == item.IdPersona && (x.DiasGozadosPorLiq != item.DiasGozadosPorLiquidar || x.DiasLiqPorGozar != item.DiasLiquidadosPorGozar || x.Observacion != item.Observaciones)).FirstOrDefault();
                if (poRegistro != null)
                {
                    var poRegistroSave = loBaseDa.Get<REHTVACACION>().Where(x => x.Periodo == item.Periodo && x.IdPersona == item.IdPersona && x.CodigoEstado == Diccionario.Activo).FirstOrDefault();
                    poRegistroSave.DiasGozadosPorLiq = item.DiasGozadosPorLiquidar;
                    poRegistroSave.DiasLiqPorGozar = item.DiasLiquidadosPorGozar;
                    poRegistroSave.Observacion = item.Observaciones;
                    poRegistroSave.UsuarioModificacion = tsUsuario;
                    poRegistroSave.FechaModificacion = DateTime.Now;

                }
            }
            loBaseDa.SaveChanges();
            return pbResult;
        }
        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tId">Id de la entidad</param>
        public string gsEliminarMaestro(int tId, string tsUsuario, string tsTerminal)
        {
            var psResult = string.Empty;
            var poObject = loBaseDa.Get<REHTSOLICITUDVACACION>().Include(x=>x.REHTSOLICITUDVACACIONVALOR).Include(x=>x.REHTSOLICITUDVACACIONDETALLE).Where(x => x.IdSolicitudVacacion == tId).FirstOrDefault();
            if (poObject != null)
            {
                var poSolPost = loBaseDa.Get<REHTSOLICITUDVACACION>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdEmpleadoContrato == poObject.IdEmpleadoContrato && x.IdSolicitudVacacion > poObject.IdSolicitudVacacion).FirstOrDefault();
                if (poSolPost != null)
                {
                    psResult = string.Format("{0}No es posible eliminar solicitud existe una solicitud posterior. Eliminar solicitud No. {1}.\n", psResult, poSolPost.IdSolicitudVacacion);
                }
                if (poObject.CodigoEstado == Diccionario.Devengado)
                {
                    psResult = string.Format("{0}No es posible eliminar una solicitud devengada, Código: {1}.\n", psResult, tId);
                }
                //if (poObject.CodigoEstado == Diccionario.Rechazado)
                //{
                //    psResult = string.Format("{0}No es posible eliminar una solicitud rechazada, Código: {1}.\n", psResult, tId);
                //}
                else
                {
                    if (poObject.REHTSOLICITUDVACACIONVALOR.Where(x=>x.CodigoEstado == Diccionario.Devengado).Count() > 0)
                    {
                        psResult = string.Format("{0}No es posible eliminar una solicitud parcialmente devengada, Código: {1}.\n", psResult, tId);
                        return psResult;
                    }

                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;

                    foreach (var item in poObject.REHTSOLICITUDVACACIONDETALLE)
                    {
                        item.CodigoEstado = Diccionario.Eliminado;
                        item.FechaIngreso = DateTime.Now;
                        item.UsuarioModificacion = tsUsuario;
                        item.TerminalModificacion = tsTerminal;
                    }

                    foreach (var item in poObject.REHTSOLICITUDVACACIONVALOR)
                    {
                        item.CodigoEstado = Diccionario.Eliminado;
                        item.FechaIngreso = DateTime.Now;
                        item.UsuarioModificacion = tsUsuario;
                        item.TerminalModificacion = tsTerminal;
                    }

                    var poObjectPermiso = loBaseDa.Get<REHPPERMISO>().Where(x => x.IdSolicitudVacacion == tId).FirstOrDefault();
                    if (poObjectPermiso != null)
                    {
                        poObjectPermiso.CodigoEstado = Diccionario.Eliminado;
                        poObjectPermiso.FechaIngreso = DateTime.Now;
                        poObjectPermiso.UsuarioModificacion = tsUsuario;
                        poObjectPermiso.TerminalModificacion = tsTerminal;
                    }

                    loBaseDa.SaveChanges();
                }
            }
            else
            {

                psResult = string.Format("{0}No existe registro para eliminar, Código: {1}.\n", psResult, tId);
            }
            return psResult;
        }

        /// <summary>
        /// Consulta de Permisos 
        /// </summary>
        /// <param name="tIdPersona"></param>
        /// <param name="tdFechaInicio"></param>
        /// <param name="tdFechaFin"></param>
        /// <returns></returns>
        public List<Permiso> goPermisosExistentes(int tIdPersona, DateTime tdFechaInicio, DateTime tdFechaFin)
        {
            return loBaseDa.Find<REHPPERMISO>().Where
                (x =>
                    x.IdPersona == tIdPersona
                    && 
                    (
                        (x.FechaInicio <= tdFechaInicio && x.FechaFin >= tdFechaInicio) 
                        || (x.FechaInicio <= tdFechaFin && x.FechaFin >= tdFechaFin)
                        || (tdFechaInicio <= x.FechaInicio && tdFechaInicio >= x.FechaFin )
                        || (tdFechaFin <= x.FechaInicio && tdFechaFin >=  x.FechaFin  )
                    )
                    && x.CodigoEstado == Diccionario.Activo
                )
                .Select(x => new Permiso
                {
                    Id = x.Id,
                    FechaInicio = x.FechaInicio,
                    FechaFin = x.FechaFin,
                    CodigoEstado = x.CodigoEstado,
                    CodigoTipoPermiso = x.CodigoTipoPermiso,
                    IdPersona = x.IdPersona,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    IdPersonaCubre = x.IdPersonaCubre
                }).ToList();
        }

        public DataTable gdtConsultaSaldoVacacion(int tIdPersona, string tsIdSolicitud = "")
        {
            if (string.IsNullOrEmpty(tsIdSolicitud))
            {
                //return loBaseDa.DataTable("SELECT Periodo,SaldoDias Días,SaldoValor Valor FROM REHTVACACION (NOLOCK) WHERE CodigoEstado = 'A' AND IdPersona = " + tIdPersona + " AND SaldoDias > 0 AND DATEDIFF(DAY,FechaInicial,FechaFinal) >= 364");
                return loBaseDa.DataTable("EXEC REHSPPRELIMINARSALDOVACACIONES " + tIdPersona);
            }
            else
            {
                return loBaseDa.DataTable("SELECT Periodo,Dias Días, Valor FROM [REHTSOLICITUDVACACIONSALDOS] (NOLOCK) WHERE CodigoEstado = 'A' AND IdSolicitudVacacion = " + tsIdSolicitud);
            }
            
        }

        public int giConsultaSaldoVacacion(int tIdPersona)
        {
            int piResult = 0;
            var dt = loBaseDa.DataTable("SELECT isnull(SUM(SaldoDias),0) Valor FROM REHTVACACION (NOLOCK) WHERE CodigoEstado = 'A' AND IdPersona = " + tIdPersona + " AND SaldoDias > 0 AND DATEDIFF(DAY,FechaInicial,FechaFinal) >= 364");
            
            if (dt != null)
            {
                piResult = int.Parse(dt.Rows[0][0].ToString());
            }

            piResult -= giConsultaDiasEnSolicitud(tIdPersona);

            return piResult;
        }

        public int giConsultaSaldoVacacionSInValor(int tIdPersona, out DateTime pdFechaInicial)
        {
            pdFechaInicial = new DateTime();
            int piResult = 0;
            var dt = loBaseDa.DataTable("SELECT isnull(SaldoDias,0) Valor, FechaInicial  FROM REHTVACACION (NOLOCK) WHERE CodigoEstado = 'A' AND IdPersona = " + tIdPersona + " AND SaldoDias > 0 AND DATEDIFF(DAY,FechaInicial,FechaFinal) < 364");

            if (dt != null)
            {
                piResult = int.Parse(dt.Rows[0][0].ToString());
                pdFechaInicial = Convert.ToDateTime(dt.Rows[0][1].ToString());
            }

            piResult -= giConsultaDiasEnSolicitud(tIdPersona);
            
            return piResult;
        }

        public int giConsultaDiasEnSolicitud(int tIdPersona)
        {
            int piResult = 0;
            var dt = loBaseDa.DataTable("SELECT isnull(SUM(Dias),0) Valor FROM REHTSOLICITUDVACACION (NOLOCK) WHERE CodigoEstado NOT IN ('E','H','D') AND IdPersona = " + tIdPersona);

            if (dt != null)
            {
                piResult = int.Parse(dt.Rows[0][0].ToString());
            }
            return piResult;
        }

        public Nullable<DateTime> gdtConsultaFechaUltimoPeriodoCerrado()
        {
            Nullable<DateTime> pdtResult = null;
            var dt = loBaseDa.DataTable("SELECT MAX(P.FechaFin) FROM REHPPERIODO P (NOLOCK) INNER JOIN REHMTIPOROL TR (NOLOCK) ON P.CodigoTipoRol = TR.CodigoTipoRol AND P.CodigoEstadoNomina = 'C' AND P.CodigoTipoRol = '002'");

            if (dt != null)
            {
                if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                {
                    pdtResult = DateTime.Parse(dt.Rows[0][0].ToString());
                }
                
            }
            return pdtResult;
        }

        public List<Vacacion> goConsultaVacacion(int tIdPersona)
        {
            var poLista =  loBaseDa.Find<REHTVACACION>().Where(x => x.IdPersona == tIdPersona && x.CodigoEstado == Diccionario.Activo && x.SaldoDias > 0).Select
                (x => new Vacacion()
                {
                    IdVacacion = x.IdVacacion,
                    CodigoEstado = x.CodigoEstado,
                    Periodo = x.Periodo,
                    IdEmpleadoContrato = x.IdEmpleadoContrato,
                    IdPersona = x.IdPersona,
                    FechaInicial = x.FechaInicial,
                    FechaFinal = x.FechaFinal,
                    DiasNormales = x.DiasNormales,
                    DiasAdicionales = x.DiasAdicionales,
                    TotalDias = x.TotalDias,
                    ValorDiasNormales = x.ValorDiasNormales,
                    ValorDiasAdicionales = x.ValorDiasAdicionales,
                    ValorTotalDias = x.ValorTotalDias,
                    DiasNormalesDevengados = x.DiasNormalesDevengados,
                    DiasAdicionalesDevengados = x.DiasAdicionalesDevengados,
                    TotalDiasDevengados = x.TotalDiasDevengados,
                    ValorDiasNormalesDevengados = x.ValorDiasNormalesDevengados,
                    ValorDiasAdicionalesDevengados = x.ValorDiasAdicionalesDevengados,
                    ValorTotalDiasDevengados = x.ValorTotalDiasDevengados,
                    SaldoDias = x.SaldoDias,
                    SaldoValor = x.SaldoValor,
                    DiasGozadosPorLiquidar = x.DiasGozadosPorLiq,
                    DiasLiquidadosPorGozar = x.DiasLiqPorGozar,
                    Observacion = x.Observacion,
                    IdVacacionReferencial = x.IdVacacionReferencial,
                    UsuarioIngreso = x.UsuarioIngreso,
                    FechaIngreso = x.FechaIngreso,
                    TerminalIngreso = x.TerminalIngreso,
                    UsuarioModificacion = x.UsuarioModificacion,
                    FechaModificacion = x.FechaModificacion,
                    TerminalModificacion = x.TerminalModificacion,
                }).ToList();

            var psList = new List<string>();
            psList.Add(Diccionario.Aprobado);
            psList.Add(Diccionario.Pendiente);
            psList.Add(Diccionario.PreAprobado);
            psList.Add(Diccionario.Devengado);

            var poListaSolApro = (from a in loBaseDa.Find<REHTSOLICITUDVACACION>()
                                  join b in loBaseDa.Find<REHTSOLICITUDVACACIONVALOR>()
                                  on a.IdSolicitudVacacion equals b.IdSolicitudVacacion
                                  where psList.Contains(a.CodigoEstado) && a.IdPersona == tIdPersona
                                  && b.CodigoEstado == Diccionario.Activo
                                  select b).ToList();

            foreach (var poItem in poLista)
            {
                var poRegistro = poListaSolApro.Where(x => x.Periodo == poItem.Periodo).FirstOrDefault();
                if (poRegistro != null)
                {
                    poItem.SaldoDias -= poListaSolApro.Where(x => x.Periodo == poItem.Periodo).Count();
                    poItem.SaldoValor -= poListaSolApro.Where(x => x.Periodo == poItem.Periodo).Sum(x => x.Valor);

                }

            }

            return poLista;
        }

        public List<PlantillaVacacion> goConsultaVacaciones()
        {
            return (from x in loBaseDa.Find<REHTVACACION>()
                    join b in loBaseDa.Find<GENMPERSONA>() on x.IdPersona equals b.IdPersona
                    join c in loBaseDa.Find<REHPEMPLEADOCONTRATO>() on x.IdEmpleadoContrato equals c.IdEmpleadoContrato
                    join d in loBaseDa.Find<GENMCATALOGO>() on 
                    new { Grupo = Diccionario.ListaCatalogo.Departamento, Codigo = c.CodigoDepartamento }
                    equals new { Grupo = d.CodigoGrupo , Codigo = d.Codigo }
                    join f in loBaseDa.Find<GENMCENTROCOSTO>() on c.CodigoCentroCosto equals f.CodigoCentroCosto
                    join e in loBaseDa.Find<GENMESTADO>() on c.CodigoEstado equals e.CodigoEstado
                    where x.CodigoEstado == Diccionario.Activo
                    select new PlantillaVacacion
                    {
                        Periodo = x.Periodo,
                        IdPersona = x.IdPersona,
                        Estado = e.Descripcion,
                        Cedula = b.NumeroIdentificacion,
                        Empleado = b.NombreCompleto,
                        Departamento = d.Descripcion,
                        FechaInicial = x.FechaInicial,
                        FechaFinal = x.FechaFinal,
                        DiasNormales = x.DiasNormales,
                        DiasAdicionales = x.DiasAdicionales,
                        TotalDias = x.TotalDias,
                        ValorDiasNormales = x.ValorDiasNormales,
                        ValorDiasAdicionales = x.ValorDiasAdicionales,
                        ValorTotalDias = x.ValorTotalDias,
                        DiasNormalesDevengados = x.DiasNormalesDevengados,
                        DiasAdicionalesDevengados = x.DiasAdicionalesDevengados,
                        TotalDiasDevengados = x.TotalDiasDevengados,
                        ValorDiasNormalesDevengados = x.ValorDiasNormalesDevengados,
                        ValorDiasAdicionalesDevengados = x.ValorDiasAdicionalesDevengados,
                        ValorTotalDiasDevengados = x.ValorTotalDiasDevengados,
                        SaldoDias = x.SaldoDiasDec??0M,
                        SaldoValor = x.SaldoValor,
                        DiasGozadosPorLiquidar = x.DiasGozadosPorLiq ?? 0,
                        DiasLiquidadosPorGozar = x.DiasLiqPorGozar ?? 0,
                        Observaciones = x.Observacion,
                        CentroCosto = f.Descripcion,
                        //DiasProporcionales = x.SaldoDiasDec ??0
                    }).ToList().OrderByDescending(x=>x.Periodo).ToList();
        }

        public List<SolicitudVacacionDetalleGridView> goConsultarPreliminarVacaciones(DateTime tdFechaInicio, DateTime tdFechaFin, int tIdPersona, int tiCantMaxDiasTomar)
        {
            loBaseDa.CreateContext();
            var poLista = new List<SolicitudVacacionDetalleGridView>();

            int piContador = 0;
            int piValorTomar = tdFechaFin.Date.Subtract(tdFechaInicio.Date).Days + 1;
            if (piValorTomar > 0)
            {
                foreach (var item in goConsultaVacacion(tIdPersona).OrderBy(x => x.Periodo))
                {
                    int piContadorFor = 0;
                    for (int i = 0; i < item.SaldoDias; i++)
                    {
                        piContador++;
                        piContadorFor++;
                        if (piContador == tiCantMaxDiasTomar) break;
                        if (piContador == piValorTomar) break;
                    }
                    if (piContadorFor > 0) poLista.Add(new SolicitudVacacionDetalleGridView() { Periodo = item.Periodo, Dias = piContadorFor, Valor = Math.Round((item.SaldoValor * piContadorFor) / item.SaldoDias, 2), DiasSaldo = item.SaldoDias - piContadorFor });
                    if (piContador == tiCantMaxDiasTomar) break;
                    if (piContador == piValorTomar) break;
                }
            }

            return poLista;
        }

        public string gsActualizaReempleazo(int tId, int tIdPersonaReemplazo, string tsUsuario, bool tbPagarReemplazo)
        {
            string psMsg = "";
            loBaseDa.CreateContext();
            var poObject = loBaseDa.Get<REHTSOLICITUDVACACION>().Where(x => x.IdSolicitudVacacion == tId).FirstOrDefault();
            if (poObject != null)
            {
                var poPersona = loBaseDa.Find<GENMPERSONA>().Where(x => x.IdPersona == tIdPersonaReemplazo).FirstOrDefault();
                if (poPersona != null)
                {
                    poObject.Reemplazo = poPersona.NombreCompleto;
                    poObject.IdPersonaReemplazo = poPersona.IdPersona;
                    poObject.PagarReemplazo = tbPagarReemplazo;
                }
                else 
                {
                    poObject.Reemplazo = "";
                    poObject.IdPersonaReemplazo = null;
                    poObject.PagarReemplazo = false;
                }

                loBaseDa.SaveChanges();
            }
            return psMsg;
        }
    }
}
