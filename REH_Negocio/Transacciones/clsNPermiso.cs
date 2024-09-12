using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using GEN_Entidad;
using GEN_Negocio;
using GEN_Entidad.Entidades.REH;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 19/09/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNPermiso : clsNBase
    {

        /// <summary>
        /// Consulta de Datos del los periodos 
        /// </summary>
        /// <returns></returns>
        public List<Permiso> goListar()
        {
            return (from a in loBaseDa.Find<REHPPERMISO>()
                    join b in loBaseDa.Find<GENMPERSONA>() on a.IdPersona equals b.IdPersona
                    join c in loBaseDa.Find<REHPTIPOPERMISO>() on a.CodigoTipoPermiso equals c.CodigoTipoPermiso
                    where a.CodigoEstado == Diccionario.Activo
                    select new Permiso
                    {
                        Id = a.Id,
                        IdPersona = a.IdPersona,
                        CodigoEstado = a.CodigoEstado,
                        FechaInicio = a.FechaInicio,
                        FechaFin = a.FechaFin,
                        CodigoTipoPermiso = a.CodigoTipoPermiso,
                        DesTipoPermiso = c.Descripcion,
                        DesPersona = b.NombreCompleto,
                        NumeroIdentificacion = b.NumeroIdentificacion,
                        Fecha = a.FechaModificacion == null ? a.FechaIngreso : a.FechaModificacion??DateTime.Now
                    }).OrderBy(x => x.FechaInicio).ToList();
        }

        /// <summary>
        /// Consulta de datos de Permisos por horas
        /// </summary>
        /// <returns></returns>
        public List<PermisoHoras> goListarPorHoras(string tsUsuario, int tIdMenu)
        {
            List<string> psCodigo = new List<string>();
            psCodigo.Add(Diccionario.Activo);
            psCodigo.Add(Diccionario.Aprobado);
            psCodigo.Add(Diccionario.Pendiente);
            psCodigo.Add(Diccionario.PreAprobado);

            return (from a in loBaseDa.Find<REHPPERMISOHORAS>()
                    join b in loBaseDa.Find<GENMPERSONA>() on a.IdPersona equals b.IdPersona
                    join d in loBaseDa.Find<REHPEMPLEADOCONTRATO>() on b.IdPersona equals d.IdPersona
                    join e in loBaseDa.Find<SEGPUSUARIOPERSONAASIGNADO>() on a.IdPersona equals e.IdPersona
                    join c in loBaseDa.Find<REHPTIPOPERMISO>() on a.CodigoTipoPermiso equals c.CodigoTipoPermiso
                    where psCodigo.Contains(a.CodigoEstado) && d.CodigoEstado == Diccionario.Activo && e.CodigoEstado == Diccionario.Activo
                    && e.IdMenu == tIdMenu && e.CodigoUsuario == tsUsuario
                    select new PermisoHoras
                    {
                        Id = a.Id,
                        IdPersona = a.IdPersona,
                        CodigoEstado = a.CodigoEstado,
                        Fecha = a.Fecha,
                        FechaInicio = a.FechaIngreso,
                        HoraInicio = a.HoraInicio,
                        HoraFin = a.HoraFin,
                        CodigoTipoPermiso = a.CodigoTipoPermiso,
                        DesTipoPermiso = c.Descripcion,
                        DesPersona = b.NombreCompleto,
                        NumeroIdentificacion = b.NumeroIdentificacion,
                    }).OrderBy(x => x.Fecha).ToList();
        }

        /// <summary>
        /// Guardar Objeto Permiso
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsGuardar(Permiso toObject, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            var poObject = loBaseDa.Get<REHPPERMISO>().Where(x => x.Id == toObject.Id).FirstOrDefault();

            string psMsg = string.Empty;
            
            if (poObject != null)
            {
                if (poObject.IdSolicitudVacacion == null)
                {
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.IdPersona = toObject.IdPersona;
                    poObject.CodigoTipoPermiso = toObject.CodigoTipoPermiso;
                    poObject.FechaInicio = toObject.FechaInicio;
                    poObject.FechaFin = toObject.FechaFin;
                    poObject.Observacion = toObject.Observacion;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                    poObject.IdPersonaCubre = toObject.IdPersonaCubre;
                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                }
                else
                {
                    psMsg += "No es posible modificar registro, Pertenece a una solicitud de Vacación";
                }
                        
                
            }
            else
            {
                poObject = new REHPPERMISO();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.IdPersona = toObject.IdPersona;
                poObject.CodigoTipoPermiso = toObject.CodigoTipoPermiso;
                poObject.FechaInicio = toObject.FechaInicio;
                poObject.FechaFin = toObject.FechaFin;
                poObject.Observacion = toObject.Observacion;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioIngreso = tsUsuario;
                poObject.TerminalIngreso = tsTerminal;
                poObject.IdPersonaCubre = toObject.IdPersonaCubre;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
            }
            
            loBaseDa.SaveChanges();
            return psMsg;
        }

        /// <summary>
        /// Guardar Objeto Permiso por Horas
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public string gsGuardarPorHoras(PermisoHoras toObject, string tsUsuario, string tsTerminal, int tIdPerfil)
        {
            loBaseDa.CreateContext();
            var poObject = loBaseDa.Get<REHPPERMISOHORAS>().Where(x => x.Id == toObject.Id).FirstOrDefault();

            string psMsg = string.Empty;

            if (toObject.CodigoTipoPermiso == Diccionario.Seleccione)
            {
                psMsg += string.Format("{0}Seleccione tipo de permiso.\n",psMsg);
            }

            if (toObject.CodigoEstado != Diccionario.Pendiente)
            {
                if (tIdPerfil == 2 || tIdPerfil == 12)
                {

                }
                else
                {
                    psMsg += string.Format("{0}Permiso debe tener estado pendiente, no es posible modificarlo.\n", psMsg);
                }
            }

            if (string.IsNullOrEmpty(psMsg))
            {
                if (poObject != null)
                {
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.IdPersona = toObject.IdPersona;
                    poObject.CodigoTipoPermiso = toObject.CodigoTipoPermiso;
                    poObject.Fecha = toObject.Fecha;
                    poObject.HoraInicio = toObject.HoraInicio;
                    poObject.HoraFin = toObject.HoraFin;
                    poObject.Observacion = toObject.Observacion;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                    poObject.TodoDia = toObject.TodoDia;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);

                }
                else
                {
                    poObject = new REHPPERMISOHORAS();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.IdPersona = toObject.IdPersona;
                    poObject.CodigoTipoPermiso = toObject.CodigoTipoPermiso;
                    poObject.Fecha = toObject.Fecha;
                    poObject.HoraInicio = toObject.HoraInicio;
                    poObject.HoraFin = toObject.HoraFin;
                    poObject.Observacion = toObject.Observacion;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.TerminalIngreso = tsTerminal;
                    poObject.TodoDia = toObject.TodoDia;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                }

                loBaseDa.SaveChanges();

            }
            return psMsg;
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Permiso goBuscarMaestro(int tId)
        {
            return loBaseDa.Find<REHPPERMISO>().Where(x => x.Id == tId)
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
                }).FirstOrDefault();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public PermisoHoras goBuscarMaestroPorHoras(int tId)
        {
            return loBaseDa.Find<REHPPERMISOHORAS>().Where(x => x.Id == tId)
                .Select(x => new PermisoHoras
                {
                    Id = x.Id,
                    Fecha = x.Fecha,
                    HoraInicio = x.HoraInicio,
                    HoraFin = x.HoraFin,
                    CodigoEstado = x.CodigoEstado,
                    CodigoTipoPermiso = x.CodigoTipoPermiso,
                    IdPersona = x.IdPersona,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    Observacion = x.Observacion,
                    UsuarioAprobacion = x.UsuarioAprobacion,
                    FechaAprobacion = x.FechaAprobacion,
                    TodoDia = x.TodoDia??false,
                }).FirstOrDefault();
        }

        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tId">Id de la entidad</param>
        public string gsEliminarMaestro(int tId, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;
            var poObject = loBaseDa.Get<REHPPERMISO>().Where(x => x.Id == tId).FirstOrDefault();
            if (poObject != null)
            {
                if (poObject.IdSolicitudVacacion == null)
                {
                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                    loBaseDa.SaveChanges();
                }
                else
                {
                    psMsg += "No es posible eliminar registro, Pertenece a una solicitud de Vacación."; 
                }
                
            }
            else
            {
                psMsg += "No existe registro para eliminar.";
            }

            return psMsg;
        }

        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tId">Id de la entidad</param>
        public string gsEliminarMaestroPorHoras(int tId, string tsUsuario, string tsTerminal,int tIdMenu)
        {
            string psMsg = string.Empty;

            var poObject = loBaseDa.Get<REHPPERMISOHORAS>().Where(x => x.Id == tId).FirstOrDefault();

            if (poObject.CodigoEstado == Diccionario.Aprobado)
            {
                if (loBaseDa.Find<SEGPUSUARIOELIMINAAPROBADAS>().Where(x=>x.CodigoEstado == Diccionario.Activo && x.IdMenu == tIdMenu && x.CodigoUsuario == tsUsuario).Count() == 0)
                {
                    if (DateTime.Now.Date > poObject.Fecha)
                    {
                        psMsg += string.Format("{0}Permiso Aprobado, no es posible modificarlo.\n", psMsg);
                    }
                    else if(DateTime.Now.Date == poObject.Fecha)
                    {
                        if (DateTime.Now.Date.TimeOfDay >= poObject.HoraInicio)
                        {
                            psMsg += string.Format("{0}Permiso Aprobado, no es posible modificarlo.\n", psMsg);
                        }
                    }
                }

            }
            if (string.IsNullOrEmpty(psMsg))
            {

                if (poObject != null)
                {

                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                    loBaseDa.SaveChanges();
                }
                else
                {
                    psMsg += "No existe registro para eliminar.";
                }
            }

            return psMsg;
        }

        public bool gbPermisosExistentes(int tIdPersona, DateTime tdFechaInicio, DateTime tdFechaFin, int tId)
        {
            bool pbResult = false;

            var poLista = loBaseDa.Find<REHPPERMISO>().Where
                (x =>
                    x.IdPersona == tIdPersona && x.CodigoEstado == Diccionario.Activo
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

            if (poLista.Where(x=> x.FechaInicio.Date <= tdFechaInicio.Date && x.FechaFin.Date >= tdFechaInicio.Date && x.Id != tId).Count() > 0)
            {
                pbResult = true;
            }
            else if (poLista.Where(x => x.FechaInicio.Date <= tdFechaFin.Date && x.FechaFin.Date >= tdFechaFin.Date && x.Id != tId).Count() > 0)
            {
                pbResult = true;
            }
            else if (poLista.Where(x => tdFechaInicio.Date <= x.FechaInicio.Date && tdFechaFin.Date >= x.FechaFin.Date && x.Id != tId).Count() > 0)
            {
                pbResult = true;
            }

            return pbResult;
        }

        public List<BandejaPermisoHoras> goListarBandeja(string tsUsuario, int tiMenu)
        {
            return loBaseDa.ExecStoreProcedure<BandejaPermisoHoras>(string.Format("REHSPCONSULTABANDEJAPERMISOHORAS {0},'{1}'", tiMenu, tsUsuario));
        }


        public List<BandejaPermisoHoras> goListarPermisosPorHoras(string tsUsuario)
        {
            return loBaseDa.ExecStoreProcedure<BandejaPermisoHoras>(string.Format("REHSPCONSULTAPERMISOHORAS '{0}'", tsUsuario));
        }

        /// <summary>
        /// Aprobar Permisos por Horas
        /// </summary>
        /// <param name="tId">Id de Registro</param>
        /// <param name="tsUsuario">Código Usuario</param>
        /// <param name="tsDesUsuario">Nombre de Usuario</param>
        /// <param name="tsTerminal">Terminal</param>
        /// <param name="tIdMenu">Id de Menú</param>
        /// <returns></returns>.
        public string gsAprobar(int tId, string tsUsuario, string tsDesUsuario, string tsTerminal, int tIdMenu)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;
            
            var poObject = loBaseDa.Get<REHPPERMISOHORAS>().Where(x => x.Id == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();
            
            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.PermisoPorHoras;
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

                        if (psUsuario.Contains(tsDesUsuario))
                        {
                            psResult = "Ya existe una aprobación con el usuario: " + tsDesUsuario + ". \n";
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
                        poObject.FechaAprobacion = DateTime.Now;
                        poObject.UsuarioAprobacion = tsUsuario;
                        poObject.FechaModificacion = DateTime.Now;
                        poObject.TerminalModificacion = tsTerminal;

                        // Insert Auditoría
                        // loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);

                        loBaseDa.SaveChanges();
                    }
                }
                else
                {
                    psResult = "Registro ya aprobado!";
                }
            }
            else
            {
                psResult = "No existe Registro por aprobar";
            }

            return psResult;

        }


    }
}
