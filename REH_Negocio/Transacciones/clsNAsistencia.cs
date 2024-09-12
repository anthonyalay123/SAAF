using GEN_Entidad;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace REH_Negocio.Transacciones
{

    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 18/08/2021
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNAsistencia : clsNBase
    {

        public List<SpAsistenciaDetalle> goConsultarAsistenciaDetalle(DateTime tdFecha, string tsUsuario,out string tsFechaCreacion, out string tsUsuarioCreacion,out string tsFechaMod, out string tsUsuarioMod)
        {
            tsFechaCreacion = "";
            tsFechaMod = "";
            tsUsuarioCreacion = "";
            tsUsuarioMod = "";

            var poObject = loBaseDa.Find<REHTASISTENCIA>().Where(x=>x.CodigoEstado == Diccionario.Activo && DbFunctions.TruncateTime(x.Fecha) == DbFunctions.TruncateTime(tdFecha)).FirstOrDefault();
            if (poObject != null)
            {
                tsFechaCreacion = poObject.FechaIngreso.ToString("dd/MM/yyyy");
                tsUsuarioCreacion = loBaseDa.Find<SEGMUSUARIO>().Where(x=>x.CodigoUsuario == poObject.UsuarioIngreso).Select(x=>x.NombreCompleto).FirstOrDefault();

                if (poObject.FechaModificacion != null)
                {
                    tsFechaMod = poObject.FechaModificacion?.ToString("dd/MM/yyyy");
                    tsUsuarioMod = loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoUsuario == poObject.UsuarioModificacion).Select(x => x.NombreCompleto).FirstOrDefault();
                }
            }

            return loBaseDa.ExecStoreProcedure<SpAsistenciaDetalle>("REHSPCONSULTAASISTENCIA @Fecha, @Usuario", new SqlParameter("@Fecha", tdFecha), new SqlParameter("@Usuario", tsUsuario));
        }

        public List<SpPermisoPorHoras> goConsultarPermisosPorHoras(DateTime tdFechaDesde, DateTime tdFechaHasta)
        {
            return loBaseDa.ExecStoreProcedure<SpPermisoPorHoras>("REHSPCONSULTAPERMISOSPORHORAS @FechaDesde, @FechaHasta", new SqlParameter("@FechaDesde", tdFechaDesde), new SqlParameter("@FechaHasta", tdFechaHasta));
        }

        public List<SpAsistenciaDetalleBiometrico> goConsultarAsistenciaDetalleBiometrico(DateTime tdFechaDesde, DateTime tdFechaHasta, string tsUsuario)
        {
            return loBaseDa.ExecStoreProcedure<SpAsistenciaDetalleBiometrico>("REHSPCONSULTAASISTENCIABIOMETRICO @FechaDesde, @FechaHasta, @Usuario", new SqlParameter("@FechaDesde", tdFechaDesde), new SqlParameter("@FechaHasta", tdFechaHasta), new SqlParameter("@Usuario", tsUsuario));
        }

        public List<SpDetalleMarcaciones> goConsultarDetalleMarcaciones(DateTime tdFechaDesde, DateTime tdFechaHasta)
        {
            return loBaseDa.ExecStoreProcedure<SpDetalleMarcaciones>("REHSPCONSULTADETALLEMARCACIONES @FechaDesde, @FechaHasta", new SqlParameter("@FechaDesde", tdFechaDesde), new SqlParameter("@FechaHasta", tdFechaHasta));
        }

        /// <summary>
        /// Elimina Registros
        /// </summary>
        public string gsEliminarAsistencia(DateTime tdFecha, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";
            var poObject = loBaseDa.Get<REHTASISTENCIA>().Include(x=>x.REHTASISTENCIADETALLE).Where(x=> x.CodigoEstado == Diccionario.Activo && DbFunctions.TruncateTime(x.Fecha) == DbFunctions.TruncateTime(tdFecha)).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;

                foreach (var item in poObject.REHTASISTENCIADETALLE.Where(x=>x.CodigoEstado == Diccionario.Activo))
                {
                    item.CodigoEstado = Diccionario.Eliminado;
                    item.FechaIngreso = DateTime.Now;
                    item.UsuarioModificacion = tsUsuario;
                    item.TerminalModificacion = tsTerminal;
                }

                loBaseDa.SaveChanges();
            }
            else
            {
                psMsg = "No existen datos para Eliminar.";
            }

            return psMsg;

        }

        public string gsGuardar(DateTime tdFecha, List<SpAsistenciaDetalle> toLista, string tsUsuario, string tsTerminal, bool tbJefeDepartamentales = false )
        {
            loBaseDa.CreateContext();
            string psMsg = string.Empty;

            //Validaciones
            if (toLista.Count() == 0)
            {
                psMsg = string.Format("{0}No existe detalle para grabar! \n", psMsg);
            }

            if(string.IsNullOrEmpty(psMsg))
            {
                var poListaDep = goConsultarComboDepartamento();
                //Guardar Cabecera
                var poObject = loBaseDa.Get<REHTASISTENCIA>().Include(x => x.REHTASISTENCIADETALLE).Where(x => x.CodigoEstado == Diccionario.Activo && DbFunctions.TruncateTime(x.Fecha)  == DbFunctions.TruncateTime(tdFecha)).FirstOrDefault();
                if (poObject != null)
                {
                    if (!tbJefeDepartamentales)
                    {
                        lCargaAsistencia(ref poObject, tdFecha, tsUsuario, tsTerminal);
                    }
                    
                }
                else
                {
                    poObject = new REHTASISTENCIA();
                    loBaseDa.CreateNewObject(out poObject);
                    lCargaAsistencia(ref poObject, tdFecha, tsUsuario, tsTerminal);
                }

                //Guardar Detalle
                if (toLista != null)
                {
                    foreach (var poItem in toLista)
                    {
                        poItem.Departamento = poListaDep.Where(x => x.Codigo == poItem.CodigoDepartamento).Select(x => x.Descripcion).FirstOrDefault();
                        int pId = poItem.IdAsistenciaDetalle;
                        var poObjectItem = poObject.REHTASISTENCIADETALLE.Where(x => x.IdAsistenciaDetalle == pId && pId != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            lCargaAsistenciaDetalle(ref poObjectItem, poItem, tsUsuario, tsTerminal, tbJefeDepartamentales);
                        }
                        else
                        {
                            if (!tbJefeDepartamentales)
                            {
                                poObjectItem = new REHTASISTENCIADETALLE();
                                lCargaAsistenciaDetalle(ref poObjectItem, poItem, tsUsuario, tsTerminal);
                                poObject.REHTASISTENCIADETALLE.Add(poObjectItem);
                            }
                            else
                            {
                                throw new Exception("Ocurrió un error al momento de guardar, por favor cierre la ventana y vuelva a intentar.");
                            }
                        }
                    }
                }


                loBaseDa.SaveChanges();
            }
            return psMsg;
        }

        private void lCargaAsistencia(ref REHTASISTENCIA toEntidadBd, DateTime tdFecha, string tsUsuario, string tsTerminal)
        {
            toEntidadBd.CodigoEstado = Diccionario.Activo;
            toEntidadBd.Fecha = tdFecha;

            if (!string.IsNullOrEmpty(toEntidadBd.UsuarioIngreso))
            {
                toEntidadBd.UsuarioModificacion = tsUsuario;
                toEntidadBd.FechaModificacion = DateTime.Now;
                toEntidadBd.TerminalModificacion = tsTerminal;
            }
            else
            {
                toEntidadBd.UsuarioIngreso = tsUsuario;
                toEntidadBd.FechaIngreso = DateTime.Now;
                toEntidadBd.TerminalIngreso = tsTerminal;
            }
        }

        private void lCargaAsistenciaDetalle(ref REHTASISTENCIADETALLE toEntidadBd, SpAsistenciaDetalle toEntidadData, string tsUsuario, string tsTerminal, bool tbJefeDepartamentales = false)
        {
            if (toEntidadBd.HoraLlegada != toEntidadData.HoraLlegada || toEntidadBd.HoraSalida != toEntidadData.HoraSalida || toEntidadBd.Asistencia != toEntidadData.Asistencia ||
                toEntidadBd.CodigoTipoPermiso != toEntidadData.Permiso || toEntidadBd.TipoPermiso != toEntidadData.DesPermiso || 
                toEntidadBd.AplicaHE != toEntidadData.AplicaHE || toEntidadBd.AplicaHEAntesEntrada != toEntidadData.AplicaHEAntesEntrada ||
                toEntidadBd.AplicaTiempoGraciasPostSalida != toEntidadData.AplicaTiempoGraciaPostSalida || toEntidadBd.Observacion != toEntidadData.Observacion ||
                toEntidadBd.UsuarioJefatura != toEntidadData.UsuarioJefatura)
            {
                toEntidadBd.CodigoEstado = Diccionario.Activo;
                toEntidadBd.IdPersona = toEntidadData.IdPersona;
                toEntidadBd.NumeroIdentificacion = toEntidadData.NumeroIdentificacion;
                toEntidadBd.NombreCompleto = toEntidadData.NombreCompleto;
                toEntidadBd.Departamento = toEntidadData.Departamento;
                toEntidadBd.HoraLlegada = toEntidadData.HoraLlegada;
                toEntidadBd.HoraSalida = toEntidadData.HoraSalida;
                toEntidadBd.TiempoAtraso = toEntidadData.TiempoAtraso;
                toEntidadBd.MinutosAtraso = toEntidadData.MinutosAtraso;
                toEntidadBd.Asistencia = toEntidadData.Asistencia;

                if (tbJefeDepartamentales)
                {
                    toEntidadBd.UsuarioJefatura = tsUsuario;
                    toEntidadBd.FechaJefatura = DateTime.Now;
                }

                if (toEntidadData.Permiso == Diccionario.Seleccione || toEntidadData.Permiso == null)
                {
                    toEntidadBd.CodigoTipoPermiso = null;
                }
                else
                {
                    toEntidadBd.CodigoTipoPermiso = toEntidadData.Permiso;
                }
                
                toEntidadBd.TipoPermiso = toEntidadData.DesPermiso;
                toEntidadBd.AplicaHE = toEntidadData.AplicaHE;
                toEntidadBd.AplicaHEAntesEntrada = toEntidadData.AplicaHEAntesEntrada;
                toEntidadBd.MinutosExtras = toEntidadData.MinutosExtras;
                toEntidadBd.AplicaTiempoGraciasPostSalida = toEntidadData.AplicaTiempoGraciaPostSalida;
                toEntidadBd.CodigoDepartamento = toEntidadData.CodigoDepartamento;
                toEntidadBd.Observacion = toEntidadData.Observacion;

                if (!string.IsNullOrEmpty(toEntidadBd.UsuarioIngreso))
                {
                    toEntidadBd.UsuarioModificacion = tsUsuario;
                    toEntidadBd.FechaModificacion = DateTime.Now;
                    toEntidadBd.TerminalModificacion = tsTerminal;
                }
                else
                {
                    toEntidadBd.UsuarioIngreso = tsUsuario;
                    toEntidadBd.FechaIngreso = DateTime.Now;
                    toEntidadBd.TerminalIngreso = tsTerminal;
                }
            }
        }

        public DataTable gdtRptAsistencia(DateTime tdFecha)
        {
            DataTable dt = new DataTable();

            dt = loBaseDa.DataTable("EXEC REHSPRPTASISTENCIA @Fecha = @toFecha ",
            new SqlParameter("toFecha", SqlDbType.DateTime) { Value = tdFecha });

            return dt;
        }

        public string gsGuardaPermisoPorHoras(List<SpPermisoPorHoras> toLista, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            string psMsg = string.Empty;
            
            if (string.IsNullOrEmpty(psMsg))
            {
                foreach (var poItem in toLista)
                {
                    
                    var poObject = loBaseDa.Get<REHPPERMISOHORAS>().Where(x => x.Id == poItem.Id).FirstOrDefault();
                    
                    if (poObject != null)
                    {
                        if (poObject.HoraInicio != poItem.HoraInicio || poObject.HoraFin != poItem.HoraFin || poObject.CodigoTipoPermiso != poItem.CodigoTipoPermiso)
                        {
                            poObject.HoraInicio = poItem.HoraInicio;
                            poObject.HoraFin = poItem.HoraFin;
                            poObject.CodigoTipoPermiso = poItem.CodigoTipoPermiso;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.TerminalModificacion = tsTerminal;
                            // Insert Auditoría
                            loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);

                        }

                    } 
                }

                loBaseDa.SaveChanges();

            }
            return psMsg;
        }

        public bool gbFechaAsistenciaCerrado(DateTime tdFecha)
        {
            bool pbResult = false;

            var piCount = loBaseDa.Find<REHTHORASEXTRAS>().Where(x => x.CodigoEstado == Diccionario.Cerrado && DbFunctions.TruncateTime(x.Fecha) == DbFunctions.TruncateTime(tdFecha)).Count();

            if (piCount > 0)
            {
                pbResult = true;
            }
            return pbResult;
        }

        public List<int> giConsultarPersonasUsuarioDepAsig()
        {
            return (from a in loBaseDa.Find<SEGPUSUARIODEPARTAMENTOASIGNADO>()
             join b in loBaseDa.Find<SEGMUSUARIO>() on a.CodigoUsuario equals b.CodigoUsuario
             where a.CodigoEstado == Diccionario.Activo && b.IdPersona != null
             select b.IdPersona??0
            ).ToList();
        }

        public int giCantDiasAprobacionHE()
        {
            return loBaseDa.Find<GENPPARAMETRO>().Select(x => x.CantDiasAprobacionHE).FirstOrDefault()??0;
        }

        public DateTime gdtFechaMaximaModificacionJefatura(DateTime pdtFecha)
        {
            var poListaFeriados = loBaseDa.Find<GENMFERIADO>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => x.Fecha).ToList();
            int piDiaCorteHE = loBaseDa.Find<GENPPARAMETRO>().Select(x => x.DiaCorteHorasExtras).FirstOrDefault() ?? 0;
            int piCantDiasAprobacionHE = giCantDiasAprobacionHE();
            if (piDiaCorteHE == 0) piDiaCorteHE = 1;
            int cont = 0;

            DateTime tdFechaMaximaMod = new DateTime();

            if (pdtFecha.Day > piDiaCorteHE)
            {
                tdFechaMaximaMod = new DateTime(pdtFecha.AddMonths(1).Year, pdtFecha.AddMonths(1).Month, piDiaCorteHE);
            }
            else
            {
                tdFechaMaximaMod = new DateTime(pdtFecha.Year, pdtFecha.Month, piDiaCorteHE);
            }


            while (cont < piCantDiasAprobacionHE)
            {
                bool pbEntra = true;
                if (poListaFeriados.Where(x=>x.Date == tdFechaMaximaMod.Date).Count() > 0)
                {
                    pbEntra = false;
                }

                if (tdFechaMaximaMod.DayOfWeek == DayOfWeek.Saturday || tdFechaMaximaMod.DayOfWeek == DayOfWeek.Sunday)
                {
                    pbEntra = false;
                }

                if (pbEntra)
                {
                    cont++;
                }

                tdFechaMaximaMod = tdFechaMaximaMod.AddDays(1);
                
            }
            

            return tdFechaMaximaMod;
        }

    }
}
