using DevExpress.XtraEditors.Filtering;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Entidad.Entidades.Administracion;
using GEN_Entidad.Entidades.Compras;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;
using static GEN_Entidad.Diccionario;

namespace COM_Negocio
{
    /// <summary>
    /// Clase de Negocio de Anticipos
    /// </summary>
    public class clsNAnticipo : clsNBase
    {
        #region Solicitud de Anticipo
        /// <summary>
        /// Listar Solicitudes de anticipos por Usuario
        /// </summary>
        /// <param name="tsUsuario"></param>
        /// <param name="tiMenu"></param>
        /// <returns></returns>
        public List<SolicitudAnticipo> goListarSolicitudAnticipo(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);

            return (from a in loBaseDa.Find<COMTSOLICITUDANTICIPO>()
                    join b in loBaseDa.Find<SEGMUSUARIO>() on a.UsuarioIngreso equals b.CodigoUsuario
                    where !Diccionario.EstadosNoIncluyentesSistema.Contains(a.CodigoEstado)
                    && psListaUsuario.Contains(a.UsuarioIngreso)
                    select new SolicitudAnticipo
                    {
                        IdSolicitudAnticipo = a.IdSolicitudAnticipo,
                        CodigoEstado = a.CodigoEstado,
                        CodigoTipoAnticipo = a.CodigoTipoAnticipo,
                        IdProveedor = a.IdProveedor,
                        Proveedor = a.Proveedor,
                        CardCode = a.CardCode,
                        FechaAnticipo = a.FechaAnticipo,
                        CodigoDepartamento = a.CodigoDepartamento,
                        Departamento = a.Departamento,
                        CodigoSucursal = a.CodigoSucursal,
                        Sucursal = a.Sucursal,
                        Observacion = a.Observacion,
                        Valor = a.Valor,
                        Usuario = b.NombreCompleto
                    }).OrderByDescending(x => x.FechaAnticipo).ToList();
        }

        /// <summary>
        /// Consultar Solicitud de Anticipo por Id
        /// </summary>
        /// <param name="Id">Identificador Único</param>
        /// <returns></returns>
        public SolicitudAnticipo goConsultarSolicitudAnticipo(int Id)
        {
            return (from a in loBaseDa.Find<COMTSOLICITUDANTICIPO>()
                    where a.IdSolicitudAnticipo == Id && !Diccionario.EstadosNoIncluyentesSistema.Contains(a.CodigoEstado)
                    select new SolicitudAnticipo
                    {
                        IdSolicitudAnticipo = a.IdSolicitudAnticipo,
                        CodigoEstado = a.CodigoEstado,
                        CodigoTipoAnticipo = a.CodigoTipoAnticipo,
                        IdProveedor = a.IdProveedor,
                        Proveedor = a.Proveedor,
                        CardCode = a.CardCode,
                        FechaAnticipo = a.FechaAnticipo,
                        CodigoDepartamento = a.CodigoDepartamento,
                        Departamento = a.Departamento,
                        CodigoSucursal = a.CodigoSucursal,
                        Sucursal = a.Sucursal,
                        Observacion = a.Observacion,
                        Valor = a.Valor,
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Guardar Solictud de Anticipo
        /// </summary>
        /// <param name="toObject">Objeto cargado de datos</param>
        /// <param name="tsUsuario">Usuario que ejecuta la acción</param>
        /// <param name="tsTerminal">Terminal que ejecuta la acción</param>
        /// <param name="tIdMenu">Id de Menú</param>
        /// <returns></returns>
        public string gsGuardarSolicitudAnticipo(SolicitudAnticipo toObject, string tsUsuario, string tsTerminal, int tIdMenu, string tsComentario = "")
        {
            loBaseDa.CreateContext();
            string psMsg = string.Empty;

            if (string.IsNullOrEmpty(psMsg))
            {
                string psEstadoAnterior = "";
                var poObject = loBaseDa.Get<COMTSOLICITUDANTICIPO>().Where(x => x.IdSolicitudAnticipo == toObject.IdSolicitudAnticipo).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObject = new COMTSOLICITUDANTICIPO();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.TerminalIngreso = tsTerminal;
                }

                psEstadoAnterior = poObject.CodigoEstado;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.CodigoTipoAnticipo = toObject.CodigoTipoAnticipo;
                poObject.IdProveedor = toObject.IdProveedor;
                poObject.Proveedor = toObject.Proveedor;
                poObject.CardCode = loBaseDa.Find<COMMPROVEEDORES>().Where(x => x.IdProveedor == toObject.IdProveedor).Select(x => x.CardCode).FirstOrDefault();
                poObject.FechaAnticipo = toObject.FechaAnticipo;
                poObject.CodigoDepartamento = toObject.CodigoDepartamento;
                poObject.Departamento = toObject.Departamento;
                poObject.CodigoSucursal = toObject.CodigoSucursal;
                poObject.Sucursal = toObject.Sucursal;
                poObject.Observacion = toObject.Observacion;
                poObject.Valor = toObject.Valor;



                using (var tran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();

                    /************************** Insertar Seguimiento de Transacción ***************************/
                    if (psEstadoAnterior != toObject.CodigoEstado)
                    {
                        REHTTRANSACCIONAUTOIZACION poTran = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTran);
                        poTran.CodigoEstado = Diccionario.Activo;
                        poTran.CodigoTransaccion = Diccionario.Tablas.Transaccion.SolicitudAnticipo;
                        poTran.ComentarioAprobador = tsComentario;
                        poTran.IdTransaccionReferencial = poObject.IdSolicitudAnticipo;
                        poTran.UsuarioAprobacion = tsUsuario;
                        poTran.UsuarioIngreso = tsUsuario;
                        poTran.FechaIngreso = DateTime.Now;
                        poTran.TerminalIngreso = tsTerminal;
                        poTran.EstadoAnterior = Diccionario.gsGetDescripcion(psEstadoAnterior);
                        poTran.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Pendiente);
                        poTran.Tipo = Diccionario.TipoAprobacion.Seguimiento;
                    }

                    loBaseDa.SaveChanges();


                    tran.Complete();
                }
            }
            return psMsg;
        }

        /// <summary>
        /// Elimina logicamente el registro del sistema
        /// </summary>
        /// <param name="Id">Identificador Único</param>
        /// <param name="tsUsuario">Usuario que ejecuta la acción</param>
        /// <param name="tsTerminal">Terminal que ejecuta la acción</param>
        /// <returns></returns>
        public string gsEliminarSolicitudAnticipo(int tId, string tsUsuario, string tsTerminal)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTSOLICITUDANTICIPO>().Where(x => x.IdSolicitudAnticipo == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado != Diccionario.Aprobado)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.SolicitudAnticipo;

                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                    poTransaccion.ComentarioAprobador = "";
                    poTransaccion.IdTransaccionReferencial = tId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Eliminado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;

                    loBaseDa.SaveChanges();
                }
                else
                {
                    psResult = "No es posible eliminar, registro aprobado.";
                }
            }
            else
            {
                psResult = "No existe registro";
            }
            return psResult;

        }

        /// <summary>
        /// Listado de Solicitudes de Anticipos por Aprobar
        /// </summary>
        /// <param name="tIdMenu">Id de Menú</param>
        /// <param name="tsUsuario">Usuario que ejecuta la acción</param>
        /// <returns></returns>
        public List<SpBandejaSolicitudAnticipo> goListarBandejaSolicitudAnticipo(int tIdMenu, string tsUsuario)
        {
            return loBaseDa.ExecStoreProcedure<SpBandejaSolicitudAnticipo>(string.Format("EXEC [COMSPBANDEJASOLICITUDANTICIPOSPORAPROBAR] {0}, '{1}'", tIdMenu, tsUsuario));
        }

        /// <summary>
        /// Aprueba la Solicitud de Anticipo
        /// </summary>
        /// <param name="Id">Identificador Único</param>
        /// <param name="tsComentario">Comentario por la aprobación</param>
        /// <param name="tsUsuario">Usuario que ejecuta la acción</param>
        /// <param name="tsTerminal">Terminal que ejecuta la acción</param>
        /// <returns></returns>
        public string gsAprobarSolicitudAnticipo(int tId, string tsComentario, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTSOLICITUDANTICIPO>().Where(x => x.IdSolicitudAnticipo == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir || poObject.CodigoEstado == Diccionario.Activo)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.SolicitudAnticipo;
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

                        string psCodigoEstado = string.Empty;
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

                        loBaseDa.SaveChanges();
                    }
                }
                else
                {
                    psResult = "Transacción ya aprobada!";
                }
            }
            else
            {
                psResult = "No existe transacción por aprobar";
            }
            return psResult;

        }

        /// <summary>
        /// Aprobación definitiva de la Solicitud de Anticipo
        /// </summary>
        /// <param name="Id">Identificador Único</param>
        /// <param name="tsComentario">Comentario por la aprobación</param>
        /// <param name="tsUsuario">Usuario que ejecuta la acción</param>
        /// <param name="tsTerminal">Terminal que ejecuta la acción</param>
        /// <returns></returns>
        public string gsAprobacionDefinitivaSolicitudAnticipo(int tId, string tsComentario, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTSOLICITUDANTICIPO>().Where(x => x.IdSolicitudAnticipo == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir || poObject.CodigoEstado == Diccionario.Activo)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.SolicitudAnticipo;
                    int pId = tId;

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
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Aprobado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Aprobado;


                    poObject.CodigoEstado = Diccionario.Aprobado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioAprobacion = tsUsuario;
                    poObject.FechaAprobacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;

                    loBaseDa.SaveChanges();
                }
                else
                {
                    psResult = "Transacción ya aprobada!";
                }
            }
            else
            {
                psResult = "No existe transacción por aprobar";
            }
            return psResult;

        }

        /// <summary>
        /// Desaprueba la Solicitud de Anticipo
        /// </summary>
        /// <param name="Id">Identificador Único</param>
        /// <param name="tsComentario">Comentario por la desaprobación</param>
        /// <param name="tsUsuario">Usuario que ejecuta la acción</param>
        /// <param name="tsTerminal">Terminal que ejecuta la acción</param>
        /// <returns></returns>
        public string gsDesaprobarSolicitudAnticipo(int tId, string tsComentario, string tsUsuario, string tsTerminal)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTSOLICITUDANTICIPO>().Where(x => x.IdSolicitudAnticipo == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                string psCodigoTransaccion = Diccionario.Tablas.Transaccion.SolicitudAnticipo;
                int pId = tId;

                List<string> psUsuario = new List<string>();

                var poAprobaciones = loBaseDa.Get<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                    x.CodigoEstado == Diccionario.Activo &&
                    x.CodigoTransaccion == psCodigoTransaccion &&
                    x.IdTransaccionReferencial == pId
                    && x.Tipo == Diccionario.TipoAprobacion.Aprobado).ToList();

                if (poAprobaciones.Count > 0)
                {
                    if (poAprobaciones.Where(x => x.UsuarioAprobacion == tsUsuario).Count() > 0)
                    {
                        var poUltimaAprobacion = poAprobaciones.OrderByDescending(x => x.FechaIngreso).FirstOrDefault();
                        if (tsUsuario == poUltimaAprobacion.UsuarioAprobacion)
                        {

                            string psCodigoEstado = string.Empty;
                            if (poObject.CodigoEstado == Diccionario.Pendiente)
                            {
                                psCodigoEstado = Diccionario.Pendiente;
                            }
                            else if (poObject.CodigoEstado == Diccionario.PreAprobado)
                            {
                                if (poAprobaciones.Count > 1)
                                {
                                    psCodigoEstado = Diccionario.PreAprobado;
                                }
                                else
                                {
                                    psCodigoEstado = Diccionario.Pendiente;
                                }
                            }
                            else if (poObject.CodigoEstado == Diccionario.Aprobado)
                            {
                                psCodigoEstado = Diccionario.PreAprobado;
                            }


                            poUltimaAprobacion.CodigoEstado = Diccionario.Eliminado;
                            poUltimaAprobacion.UsuarioModificacion = tsUsuario;
                            poUltimaAprobacion.FechaModificacion = DateTime.Now;
                            poUltimaAprobacion.TerminalModificacion = tsTerminal;


                            REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTransaccion);
                            poTransaccion.CodigoEstado = Diccionario.Activo;
                            poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                            poTransaccion.ComentarioAprobador = tsComentario;
                            poTransaccion.IdTransaccionReferencial = tId;
                            poTransaccion.UsuarioAprobacion = tsUsuario;
                            poTransaccion.UsuarioIngreso = tsUsuario;
                            poTransaccion.FechaIngreso = DateTime.Now;
                            poTransaccion.TerminalIngreso = tsTerminal;
                            poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                            poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psCodigoEstado);
                            poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                            poObject.CodigoEstado = psCodigoEstado;
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;

                            loBaseDa.SaveChanges();
                        }
                        else
                        {
                            psResult = "Existe una aprobación posterior a su aprobación, NO es posible desaprobar. \n";
                        }
                    }
                    else
                    {
                        psResult = "NO existe aprobación con el usuario: " + tsUsuario + " para desaprobar. \n";
                    }
                }
                else
                {
                    psResult = "NO existen aprobaciones para desaprobar. \n";
                }
            }
            else
            {
                psResult = "No existe Documento por aprobar";
            }
            return psResult;

        }

        /// <summary>
        /// Cambio de estadp de la Solicitud de Anticipo
        /// </summary>
        /// <param name="Id">Identificador Único</param>
        /// <param name="tsComentario">Comentario por el cambio de estado</param>
        /// <param name="tsUsuario">Usuario que ejecuta la acción</param>
        /// <param name="tsTerminal">Terminal que ejecuta la acción</param>
        /// <returns></returns>
        public string gActualizarEstadoSolicitudAnticipo(int tId, string tsEstado, string Observacion, string tsUsuario, string tsTerminal)
        {

            string msg = "";

            loBaseDa.CreateContext();

            var poObject = loBaseDa.Get<COMTSOLICITUDANTICIPO>().Where(x => x.IdSolicitudAnticipo == tId).FirstOrDefault();
            if (poObject != null)
            {
                if (tsEstado != poObject.CodigoEstado)
                {
                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.SolicitudAnticipo;
                    poTransaccion.ComentarioAprobador = Observacion;
                    poTransaccion.IdTransaccionReferencial = tId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(tsEstado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    poObject.CodigoEstado = tsEstado;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;

                    loBaseDa.SaveChanges();
                }
                else
                {
                    msg = string.Format("No es posible cambiarlo a estado: {0}. Ya tiene ese estado.", Diccionario.gsGetDescripcion(tsEstado));
                }
            }
            return msg;
        }
        /// <summary>
        /// Buscar Codigo de la Entidad
        /// </summary>
        /// <param name="tsTipo">P: Primero, A: Anterior, S: Siguiente, U: Último</param>
        /// <param name="tsCodigo">Codigo de la entidad</param>
        /// <returns></returns>
        public int goBuscarCodigoSolicitudAnticipo(string tsTipo, string tsCodigo = "")
        {
            int psCodigo = 0;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<COMTSOLICITUDANTICIPO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdSolicitudAnticipo }).OrderBy(x => x.IdSolicitudAnticipo).FirstOrDefault().IdSolicitudAnticipo;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<COMTSOLICITUDANTICIPO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdSolicitudAnticipo }).OrderByDescending(x => x.IdSolicitudAnticipo).FirstOrDefault().IdSolicitudAnticipo;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<COMTSOLICITUDANTICIPO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdSolicitudAnticipo }).ToList().Where(x => x.IdSolicitudAnticipo < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdSolicitudAnticipo).FirstOrDefault().IdSolicitudAnticipo;
                }
                else
                {
                    psCodigo = int.Parse(tsCodigo);
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<COMTSOLICITUDANTICIPO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdSolicitudAnticipo }).ToList().Where(x => x.IdSolicitudAnticipo > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdSolicitudAnticipo).FirstOrDefault().IdSolicitudAnticipo;
                }
                else
                {
                    psCodigo = int.Parse(tsCodigo);
                }
            }
            return psCodigo;

        }
        #endregion

        #region Listado de Solicitud de Anticipo
        public List<SpListadoSolicitudAnticipo> goListadoSolicitudAnticipo(int tIdMenu, string tsUsuario, DateTime tdFechaDesde, DateTime tdFechaHasta)
        {
            return loBaseDa.ExecStoreProcedure<SpListadoSolicitudAnticipo>(string.Format("EXEC COMSPLISTADOSOLICITUDANTICIPO {0},'{1}','{2}','{3}'",tIdMenu, tsUsuario, tdFechaDesde.ToString("yyyyMMdd"), tdFechaHasta.ToString("yyyyMMdd")));
        }
        #endregion

        #region Liquidación de Anticipo
        /// <summary>
        /// Listar Liquidaciones de anticipos por Usuario
        /// </summary>
        /// <param name="tsUsuario"></param>
        /// <param name="tiMenu"></param>
        /// <returns></returns>
        public List<LiquidacionAnticipo> goListarLiquidacionAnticipo(string tsUsuario, int tiMenu)
        {
            var psListaUsuario = goConsultarUsuarioAsignados(tsUsuario, tiMenu);

            return (from a in loBaseDa.Find<COMTLIQUIDACIONANTICIPO>()
                    join b in loBaseDa.Find<SEGMUSUARIO>() on a.UsuarioIngreso equals b.CodigoUsuario
                    where !Diccionario.EstadosNoIncluyentesSistema.Contains(a.CodigoEstado)
                    && psListaUsuario.Contains(a.UsuarioIngreso)
                    select new LiquidacionAnticipo
                    {
                        IdLiquidacionAnticipo = a.IdLiquidacionAnticipo,
                        CodigoEstado = a.CodigoEstado,
                        IdProveedor = a.IdProveedor,
                        Proveedor = a.Proveedor,
                        CardCode = a.CardCode,
                        FechaLiquidacion = a.FechaLiquidacion,
                        Observacion = a.Observacion,
                        TotalGastos = a.TotalGastos,
                        Usuario = b.NombreCompleto
                    }).OrderByDescending(x => x.FechaLiquidacion).ToList();
        }
        private List<LiquidacionAnticipo> loLlenarDatos(List<COMTLIQUIDACIONANTICIPO> toLista)
        {
            List<LiquidacionAnticipo> poLista = new List<LiquidacionAnticipo>();

            foreach (var item in toLista)
            {
                var poCab = new LiquidacionAnticipo();
                poCab.IdLiquidacionAnticipo = item.IdLiquidacionAnticipo;
                poCab.CodigoEstado = item.CodigoEstado;
                poCab.IdProveedor = item.IdProveedor;
                poCab.Proveedor = item.Proveedor;
                poCab.CardCode = item.CardCode;
                poCab.FechaLiquidacion = item.FechaLiquidacion;
                poCab.Observacion = item.Observacion;
                poCab.TotalGastos = item.TotalGastos;
                poCab.TotalAnticipo = item.TotalAnticipo;
                poCab.ValorDevolver = item.ValorDevolver;
                poCab.ValorReponer = item.ValorReponer;

                foreach (var det in item.COMTLIQUIDACIONANTICIPODETALLE.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado)))
                {
                    var poDet = new LiquidacionAnticipoDetalle();
                    poDet.IdLiquidacionAnticipoDetalle = det.IdLiquidacionAnticipoDetalle;
                    poDet.CodigoEstado = det.CodigoEstado;
                    poDet.IdLiquidacionAnticipo = det.IdLiquidacionAnticipo;
                    poDet.Fecha = det.Fecha;
                    poDet.NumeroDocumento = det.NumeroDocumento;
                    poDet.Proveedor = det.Proveedor;
                    poDet.Descripcion = det.Descripcion;
                    poDet.Valor = det.Valor;

                    poCab.LiquidacionAnticipoDetalle.Add(poDet);
                }

                foreach (var det in item.COMTLIQUIDACIONANTICIPOSOLICITUD.Where(x => !Diccionario.EstadosNoIncluyentesSistema.Contains(x.CodigoEstado)))
                {
                    var poDet = new LiquidacionAnticipoSolicitud();
                    poDet.IdLiquidacionAnticipoSolicitud = det.IdLiquidacionAnticipoSolicitud;
                    poDet.CodigoEstado = det.CodigoEstado;
                    poDet.IdLiquidacionAnticipo = det.IdLiquidacionAnticipo;
                    poDet.IdSolicitudAnticipo = det.IdSolicitudAnticipo;
                    poDet.Valor = det.COMTSOLICITUDANTICIPO.Valor;
                    poDet.Proveedor = det.COMTSOLICITUDANTICIPO?.Proveedor;
                    poDet.FechaAnticipo = det.COMTSOLICITUDANTICIPO.FechaAnticipo;

                    poCab.LiquidacionAnticipoSolicitud.Add(poDet);
                }

                poLista.Add(poCab);
            }

            return poLista;
        }
        /// <summary>
        /// Consultar Liquidación de Anticipo por Id
        /// </summary>
        /// <param name="Id">Identificador Único</param>
        /// <returns></returns>
        public LiquidacionAnticipo goConsultarLiquidacionAnticipo(int Id)
        {
            var poLista = loBaseDa.Find<COMTLIQUIDACIONANTICIPO>().Include(x=>x.COMTLIQUIDACIONANTICIPODETALLE)
                .Include(x=>x.COMTLIQUIDACIONANTICIPOSOLICITUD).Where(x => x.IdLiquidacionAnticipo == Id).ToList();
            return loLlenarDatos(poLista).FirstOrDefault();
        }
        /// <summary>
        /// Guardar Liquidacion de Anticipo
        /// </summary>
        /// <param name="toObject">Objeto cargado de datos</param>
        /// <param name="tsUsuario">Usuario que ejecuta la acción</param>
        /// <param name="tsTerminal">Terminal que ejecuta la acción</param>
        /// <param name="tIdMenu">Id de Menú</param>
        /// <returns></returns>
        public string gsGuardarLiquidacionAnticipo(LiquidacionAnticipo toObject, string tsUsuario, string tsTerminal, int tIdMenu, string tsComentario = "")
        {
            loBaseDa.CreateContext();
            string psMsg = lsEsValidoLiquidacionAnticipo(toObject);

            if (string.IsNullOrEmpty(psMsg))
            {
                string psEstadoAnterior = "";
                var poObject = loBaseDa.Get<COMTLIQUIDACIONANTICIPO>().Include(x => x.COMTLIQUIDACIONANTICIPODETALLE)
                .Include(x => x.COMTLIQUIDACIONANTICIPOSOLICITUD).Where(x => x.IdLiquidacionAnticipo == toObject.IdLiquidacionAnticipo).FirstOrDefault();

                if (poObject != null)
                {
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                }
                else
                {
                    poObject = new COMTLIQUIDACIONANTICIPO();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.TerminalIngreso = tsTerminal;
                }

                psEstadoAnterior = poObject.CodigoEstado;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.IdProveedor = toObject.IdProveedor;
                poObject.Proveedor = toObject.Proveedor;
                poObject.CardCode = loBaseDa.Find<COMMPROVEEDORES>().Where(x => x.IdProveedor == toObject.IdProveedor).Select(x => x.CardCode).FirstOrDefault();
                poObject.FechaLiquidacion = toObject.FechaLiquidacion;
                poObject.Observacion = toObject.Observacion;
                poObject.TotalGastos = toObject.TotalGastos;
                poObject.TotalAnticipo = toObject.TotalAnticipo;
                poObject.ValorDevolver = toObject.ValorDevolver;
                poObject.ValorReponer = toObject.ValorReponer;
                poObject.Observacion = toObject.Observacion;

                List<int> poListaIdPe = toObject.LiquidacionAnticipoDetalle.Select(x => x.IdLiquidacionAnticipoDetalle).ToList();
                List<int> piListaEliminar = poObject.COMTLIQUIDACIONANTICIPODETALLE.Where(x => !poListaIdPe.Contains(x.IdLiquidacionAnticipoDetalle)).Select(x => x.IdLiquidacionAnticipoDetalle).ToList();
                //Recorrer la base de dato modificando el codigo estado a eliminado
                foreach (var poItem in poObject.COMTLIQUIDACIONANTICIPODETALLE.Where(x => piListaEliminar.Contains(x.IdLiquidacionAnticipoDetalle)))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                if (toObject.LiquidacionAnticipoDetalle != null)
                {
                    foreach (var det in toObject.LiquidacionAnticipoDetalle)
                    {
                        var poObjectItem = poObject.COMTLIQUIDACIONANTICIPODETALLE.Where(x => x.IdLiquidacionAnticipoDetalle == det.IdLiquidacionAnticipoDetalle && det.IdLiquidacionAnticipoDetalle != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new COMTLIQUIDACIONANTICIPODETALLE();
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObjectItem.CodigoEstado = Diccionario.Activo;
                            poObject.COMTLIQUIDACIONANTICIPODETALLE.Add(poObjectItem);
                        }

                        
                        
                        poObjectItem.Fecha = det.Fecha;
                        poObjectItem.NumeroDocumento = det.NumeroDocumento;
                        poObjectItem.Proveedor = det.Proveedor;
                        poObjectItem.Descripcion = det.Descripcion;
                        poObjectItem.Valor = det.Valor;
                    }
                }

                poListaIdPe = toObject.LiquidacionAnticipoSolicitud.Select(x => x.IdLiquidacionAnticipoSolicitud).ToList();
                piListaEliminar = poObject.COMTLIQUIDACIONANTICIPOSOLICITUD.Where(x => !poListaIdPe.Contains(x.IdLiquidacionAnticipoSolicitud)).Select(x => x.IdLiquidacionAnticipoSolicitud).ToList();
                //Recorrer la base de dato modificando el codigo estado a eliminado
                foreach (var poItem in poObject.COMTLIQUIDACIONANTICIPOSOLICITUD.Where(x => piListaEliminar.Contains(x.IdLiquidacionAnticipoSolicitud)))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                if (toObject.LiquidacionAnticipoSolicitud != null)
                {
                    foreach (var det in toObject.LiquidacionAnticipoSolicitud)
                    {
                        var poObjectItem = poObject.COMTLIQUIDACIONANTICIPOSOLICITUD.Where(x => x.IdLiquidacionAnticipoSolicitud == det.IdLiquidacionAnticipoSolicitud && det.IdLiquidacionAnticipoSolicitud != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new COMTLIQUIDACIONANTICIPOSOLICITUD();
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.TerminalIngreso = tsTerminal;
                            poObjectItem.CodigoEstado = Diccionario.Activo;
                            poObject.COMTLIQUIDACIONANTICIPOSOLICITUD.Add(poObjectItem);
                        }
                        poObjectItem.IdLiquidacionAnticipo = det.IdLiquidacionAnticipo;
                        poObjectItem.IdSolicitudAnticipo = det.IdSolicitudAnticipo;
                    }
                }

                using (var tran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();

                    /************************** Insertar Seguimiento de Transacción ***************************/
                    if (psEstadoAnterior != toObject.CodigoEstado)
                    {
                        REHTTRANSACCIONAUTOIZACION poTran = new REHTTRANSACCIONAUTOIZACION();
                        loBaseDa.CreateNewObject(out poTran);
                        poTran.CodigoEstado = Diccionario.Activo;
                        poTran.CodigoTransaccion = Diccionario.Tablas.Transaccion.LiquidacionAnticipo;
                        poTran.ComentarioAprobador = tsComentario;
                        poTran.IdTransaccionReferencial = poObject.IdLiquidacionAnticipo;
                        poTran.UsuarioAprobacion = tsUsuario;
                        poTran.UsuarioIngreso = tsUsuario;
                        poTran.FechaIngreso = DateTime.Now;
                        poTran.TerminalIngreso = tsTerminal;
                        poTran.EstadoAnterior = Diccionario.gsGetDescripcion(psEstadoAnterior);
                        poTran.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Pendiente);
                        poTran.Tipo = Diccionario.TipoAprobacion.Seguimiento;
                    }

                    loBaseDa.SaveChanges();


                    tran.Complete();
                }
            }
            return psMsg;
        }
        /// <summary>
        /// Elimina logicamente el registro del sistema
        /// </summary>
        /// <param name="Id">Identificador Único</param>
        /// <param name="tsUsuario">Usuario que ejecuta la acción</param>
        /// <param name="tsTerminal">Terminal que ejecuta la acción</param>
        /// <returns></returns>
        public string gsEliminarLiquidacionAnticipo(int tId, string tsUsuario, string tsTerminal)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTLIQUIDACIONANTICIPO>().Include(x=>x.COMTLIQUIDACIONANTICIPOSOLICITUD).Include(x => x.COMTLIQUIDACIONANTICIPODETALLE).Where(x => x.IdLiquidacionAnticipo == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado != Diccionario.Aprobado)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.LiquidacionAnticipo;

                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                    poTransaccion.ComentarioAprobador = "";
                    poTransaccion.IdTransaccionReferencial = tId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Eliminado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    poObject.CodigoEstado = Diccionario.Eliminado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;


                    foreach (var item in poObject.COMTLIQUIDACIONANTICIPOSOLICITUD.Where(x=>x.CodigoEstado == Diccionario.Activo))
                    {
                        item.CodigoEstado = Diccionario.Eliminado;
                        item.UsuarioModificacion = tsUsuario;
                        item.FechaModificacion = DateTime.Now;
                        item.TerminalModificacion = tsTerminal;
                    }

                    foreach (var item in poObject.COMTLIQUIDACIONANTICIPODETALLE.Where(x => x.CodigoEstado == Diccionario.Activo))
                    {
                        item.CodigoEstado = Diccionario.Eliminado;
                        item.UsuarioModificacion = tsUsuario;
                        item.FechaModificacion = DateTime.Now;
                        item.TerminalModificacion = tsTerminal;
                    }

                    loBaseDa.SaveChanges();
                }
                else
                {
                    psResult = "No es posible eliminar, registro aprobado.";
                }
            }
            else
            {
                psResult = "No existe registro";
            }
            return psResult;

        }
        /// <summary>
        /// Listado de Liquidaciones de Anticipos por Aprobar
        /// </summary>
        /// <param name="tIdMenu">Id de Menú</param>
        /// <param name="tsUsuario">Usuario que ejecuta la acción</param>
        /// <returns></returns>
        public List<SpBandejaLiquidacionAnticipo> goListarBandejaLiquidacionAnticipo(int tIdMenu, string tsUsuario)
        {
            return loBaseDa.ExecStoreProcedure<SpBandejaLiquidacionAnticipo>(string.Format("EXEC [COMSPBANDEJALIQUIDACIONANTICIPOSPORAPROBAR] {0}, '{1}'", tIdMenu, tsUsuario));
        }
        /// <summary>
        /// Aprueba la Liquidación de Anticipo
        /// </summary>
        /// <param name="Id">Identificador Único</param>
        /// <param name="tsComentario">Comentario por la aprobación</param>
        /// <param name="tsUsuario">Usuario que ejecuta la acción</param>
        /// <param name="tsTerminal">Terminal que ejecuta la acción</param>
        /// <returns></returns>
        public string gsAprobarLiquidacionAnticipo(int tId, string tsComentario, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTLIQUIDACIONANTICIPO>().Where(x => x.IdLiquidacionAnticipo == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir || poObject.CodigoEstado == Diccionario.Activo)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.LiquidacionAnticipo;
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

                        string psCodigoEstado = string.Empty;
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

                        loBaseDa.SaveChanges();
                    }
                }
                else
                {
                    psResult = "Transacción ya aprobada!";
                }
            }
            else
            {
                psResult = "No existe transacción por aprobar";
            }
            return psResult;

        }
        /// <summary>
        /// Aprobación definitiva de la Liquidacion de Anticipo
        /// </summary>
        /// <param name="Id">Identificador Único</param>
        /// <param name="tsComentario">Comentario por la aprobación</param>
        /// <param name="tsUsuario">Usuario que ejecuta la acción</param>
        /// <param name="tsTerminal">Terminal que ejecuta la acción</param>
        /// <returns></returns>
        public string gsAprobacionDefinitivaLiquidacionAnticipo(int tId, string tsComentario, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTLIQUIDACIONANTICIPO>().Where(x => x.IdLiquidacionAnticipo == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.PreAprobado || poObject.CodigoEstado == Diccionario.Corregir || poObject.CodigoEstado == Diccionario.Activo)
                {
                    string psCodigoTransaccion = Diccionario.Tablas.Transaccion.LiquidacionAnticipo;
                    int pId = tId;

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
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(Diccionario.Aprobado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Aprobado;


                    poObject.CodigoEstado = Diccionario.Aprobado;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioAprobacion = tsUsuario;
                    poObject.FechaAprobacion = DateTime.Now;
                    poObject.TerminalModificacion = tsTerminal;

                    loBaseDa.SaveChanges();
                }
                else
                {
                    psResult = "Transacción ya aprobada!";
                }
            }
            else
            {
                psResult = "No existe transacción por aprobar";
            }
            return psResult;

        }
        /// <summary>
        /// Desaprueba la Liquidacion de Anticipo
        /// </summary>
        /// <param name="Id">Identificador Único</param>
        /// <param name="tsComentario">Comentario por la desaprobación</param>
        /// <param name="tsUsuario">Usuario que ejecuta la acción</param>
        /// <param name="tsTerminal">Terminal que ejecuta la acción</param>
        /// <returns></returns>
        public string gsDesaprobarLiquidacionAnticipo(int tId, string tsComentario, string tsUsuario, string tsTerminal)
        {

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<COMTLIQUIDACIONANTICIPO>().Where(x => x.IdLiquidacionAnticipo == tId && x.CodigoEstado != Diccionario.Eliminado).FirstOrDefault();

            if (poObject != null)
            {
                string psCodigoTransaccion = Diccionario.Tablas.Transaccion.LiquidacionAnticipo;
                int pId = tId;

                List<string> psUsuario = new List<string>();

                var poAprobaciones = loBaseDa.Get<REHTTRANSACCIONAUTOIZACION>().Where(x =>
                    x.CodigoEstado == Diccionario.Activo &&
                    x.CodigoTransaccion == psCodigoTransaccion &&
                    x.IdTransaccionReferencial == pId
                    && x.Tipo == Diccionario.TipoAprobacion.Aprobado).ToList();

                if (poAprobaciones.Count > 0)
                {
                    if (poAprobaciones.Where(x => x.UsuarioAprobacion == tsUsuario).Count() > 0)
                    {
                        var poUltimaAprobacion = poAprobaciones.OrderByDescending(x => x.FechaIngreso).FirstOrDefault();
                        if (tsUsuario == poUltimaAprobacion.UsuarioAprobacion)
                        {

                            string psCodigoEstado = string.Empty;
                            if (poObject.CodigoEstado == Diccionario.Pendiente)
                            {
                                psCodigoEstado = Diccionario.Pendiente;
                            }
                            else if (poObject.CodigoEstado == Diccionario.PreAprobado)
                            {
                                if (poAprobaciones.Count > 1)
                                {
                                    psCodigoEstado = Diccionario.PreAprobado;
                                }
                                else
                                {
                                    psCodigoEstado = Diccionario.Pendiente;
                                }
                            }
                            else if (poObject.CodigoEstado == Diccionario.Aprobado)
                            {
                                psCodigoEstado = Diccionario.PreAprobado;
                            }


                            poUltimaAprobacion.CodigoEstado = Diccionario.Eliminado;
                            poUltimaAprobacion.UsuarioModificacion = tsUsuario;
                            poUltimaAprobacion.FechaModificacion = DateTime.Now;
                            poUltimaAprobacion.TerminalModificacion = tsTerminal;


                            REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                            loBaseDa.CreateNewObject(out poTransaccion);
                            poTransaccion.CodigoEstado = Diccionario.Activo;
                            poTransaccion.CodigoTransaccion = psCodigoTransaccion;
                            poTransaccion.ComentarioAprobador = tsComentario;
                            poTransaccion.IdTransaccionReferencial = tId;
                            poTransaccion.UsuarioAprobacion = tsUsuario;
                            poTransaccion.UsuarioIngreso = tsUsuario;
                            poTransaccion.FechaIngreso = DateTime.Now;
                            poTransaccion.TerminalIngreso = tsTerminal;
                            poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                            poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(psCodigoEstado);
                            poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                            poObject.CodigoEstado = psCodigoEstado;
                            poObject.UsuarioModificacion = tsUsuario;
                            poObject.FechaModificacion = DateTime.Now;
                            poObject.TerminalModificacion = tsTerminal;

                            loBaseDa.SaveChanges();
                        }
                        else
                        {
                            psResult = "Existe una aprobación posterior a su aprobación, NO es posible desaprobar. \n";
                        }
                    }
                    else
                    {
                        psResult = "NO existe aprobación con el usuario: " + tsUsuario + " para desaprobar. \n";
                    }
                }
                else
                {
                    psResult = "NO existen aprobaciones para desaprobar. \n";
                }
            }
            else
            {
                psResult = "No existe Documento por aprobar";
            }
            return psResult;

        }
        /// <summary>
        /// Cambio de estadp de la Liquidacion de Anticipo
        /// </summary>
        /// <param name="Id">Identificador Único</param>
        /// <param name="tsComentario">Comentario por el cambio de estado</param>
        /// <param name="tsUsuario">Usuario que ejecuta la acción</param>
        /// <param name="tsTerminal">Terminal que ejecuta la acción</param>
        /// <returns></returns>
        public string gActualizarEstadoLiquidacionAnticipo(int tId, string tsEstado, string Observacion, string tsUsuario, string tsTerminal)
        {

            string msg = "";

            loBaseDa.CreateContext();

            var poObject = loBaseDa.Get<COMTLIQUIDACIONANTICIPO>().Where(x => x.IdLiquidacionAnticipo == tId).FirstOrDefault();
            if (poObject != null)
            {
                if (tsEstado != poObject.CodigoEstado)
                {
                    REHTTRANSACCIONAUTOIZACION poTransaccion = new REHTTRANSACCIONAUTOIZACION();
                    loBaseDa.CreateNewObject(out poTransaccion);
                    poTransaccion.CodigoEstado = Diccionario.Activo;
                    poTransaccion.CodigoTransaccion = Diccionario.Tablas.Transaccion.LiquidacionAnticipo;
                    poTransaccion.ComentarioAprobador = Observacion;
                    poTransaccion.IdTransaccionReferencial = tId;
                    poTransaccion.UsuarioAprobacion = tsUsuario;
                    poTransaccion.UsuarioIngreso = tsUsuario;
                    poTransaccion.FechaIngreso = DateTime.Now;
                    poTransaccion.TerminalIngreso = tsTerminal;
                    poTransaccion.EstadoAnterior = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    poTransaccion.EstadoPosterior = Diccionario.gsGetDescripcion(tsEstado);
                    poTransaccion.Tipo = Diccionario.TipoAprobacion.Seguimiento;

                    poObject.CodigoEstado = tsEstado;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;

                    loBaseDa.SaveChanges();
                }
                else
                {
                    msg = string.Format("No es posible cambiarlo a estado: {0}. Ya tiene ese estado.", Diccionario.gsGetDescripcion(tsEstado));
                }
            }
            return msg;
        }
        /// <summary>
        /// Buscar Codigo de la Entidad
        /// </summary>
        /// <param name="tsTipo">P: Primero, A: Anterior, S: Siguiente, U: Último</param>
        /// <param name="tsCodigo">Codigo de la entidad</param>
        /// <returns></returns>
        public int goBuscarCodigoLiquidacionAnticipo(string tsTipo, string tsCodigo = "")
        {
            int psCodigo = 0;
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<COMTLIQUIDACIONANTICIPO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdLiquidacionAnticipo }).OrderBy(x => x.IdLiquidacionAnticipo).FirstOrDefault().IdLiquidacionAnticipo;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<COMTLIQUIDACIONANTICIPO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdLiquidacionAnticipo }).OrderByDescending(x => x.IdLiquidacionAnticipo).FirstOrDefault().IdLiquidacionAnticipo;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<COMTLIQUIDACIONANTICIPO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdLiquidacionAnticipo }).ToList().Where(x => x.IdLiquidacionAnticipo < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdLiquidacionAnticipo).FirstOrDefault().IdLiquidacionAnticipo;
                }
                else
                {
                    psCodigo = int.Parse(tsCodigo);
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<COMTLIQUIDACIONANTICIPO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdLiquidacionAnticipo }).ToList().Where(x => x.IdLiquidacionAnticipo > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdLiquidacionAnticipo).FirstOrDefault().IdLiquidacionAnticipo;
                }
                else
                {
                    psCodigo = int.Parse(tsCodigo);
                }
            }
            return psCodigo;

        }
        public DataTable gdtSolicitudAnticiposPorProveedor(int tIdProveedor, string tsIdNoConsiderar)
        {
            if (string.IsNullOrEmpty(tsIdNoConsiderar))
            {
                return loBaseDa.DataTable(string.Format("SELECT SA.IdSolicitudAnticipo Id,SA.FechaAnticipo, SA.Proveedor,SA.Valor FROM COMTSOLICITUDANTICIPO SA LEFT JOIN COMTLIQUIDACIONANTICIPOSOLICITUD LSA ON SA.IdSolicitudAnticipo = LSA.IdSolicitudAnticipo AND LSA.CodigoEstado NOT IN ('E','I') WHERE SA.CodigoEstado = 'R' AND SA.IdProveedor = '{0}' AND SA.IdSolicitudAnticipo NOT IN (SELECT T0.IdSolicitudAnticipo FROM COMTLIQUIDACIONANTICIPOSOLICITUD T0 WHERE T0.CodigoEstado NOT IN ('E','I'))", tIdProveedor));
            }
            else
            {
                return loBaseDa.DataTable(string.Format("SELECT SA.IdSolicitudAnticipo Id,SA.FechaAnticipo, SA.Proveedor,SA.Valor FROM COMTSOLICITUDANTICIPO SA LEFT JOIN COMTLIQUIDACIONANTICIPOSOLICITUD LSA ON SA.IdSolicitudAnticipo = LSA.IdSolicitudAnticipo AND LSA.CodigoEstado NOT IN ('E','I') WHERE SA.CodigoEstado = 'R' AND SA.IdProveedor = '{0}' AND SA.IdSolicitudAnticipo NOT IN (SELECT T0.IdSolicitudAnticipo FROM COMTLIQUIDACIONANTICIPOSOLICITUD T0 WHERE T0.CodigoEstado NOT IN ('E','I')) AND SA.IdSolicitudAnticipo NOT IN ({1})", tIdProveedor,tsIdNoConsiderar));
            }
            

        }
        private string lsEsValidoLiquidacionAnticipo(LiquidacionAnticipo toObject)
        {
            string psMsg = string.Empty;
            if (string.IsNullOrEmpty(toObject.Observacion))
            {
                psMsg = psMsg + "Falta la observación  \n";
            }
            if (toObject.LiquidacionAnticipoSolicitud == null || toObject.LiquidacionAnticipoSolicitud.Count == 0)
            {
                psMsg = psMsg + "Falta agregar anticipos \n";
            }

            if (toObject.LiquidacionAnticipoDetalle == null || toObject.LiquidacionAnticipoDetalle.Count == 0)
            {
                psMsg = psMsg + "Falta agregar etalle de gastos \n";
            }
            else
            {
                int num = 1;
                foreach (var det in toObject.LiquidacionAnticipoDetalle)
                {
                    if (det.Fecha == DateTime.MinValue)
                    {
                        psMsg = psMsg + "Falta agregar Fecha en la fila:" + num + "\n";
                    }
                    if (string.IsNullOrEmpty(det.Proveedor))
                    {
                        psMsg = psMsg + "Falta agregar Proveedor en la fila:" + num + "\n";
                    }
                    if (string.IsNullOrEmpty(det.Descripcion))
                    {
                        psMsg = psMsg + "Falta agregar Descripcion en la fila: " + num + "\n";
                    }
                    if (det.Valor <= 0)
                    {
                        psMsg = psMsg + "Valor de gasto no es valido en la fila " + num + "\n";
                    }

                    num = num + 1;
                }

            }

            return psMsg;
        }

        #endregion

        #region Listado de Liquidacion de Anticipo
        public List<SpListadoLiquidacionAnticipo> goListadoLiquidacionAnticipo(int tIdMenu, string tsUsuario, DateTime tdFechaDesde, DateTime tdFechaHasta)
        {
            return loBaseDa.ExecStoreProcedure<SpListadoLiquidacionAnticipo>(string.Format("EXEC COMSPLISTADOLIQUIDACIONANTICIPO {0},'{1}','{2}','{3}'", tIdMenu, tsUsuario, tdFechaDesde.ToString("yyyyMMdd"), tdFechaHasta.ToString("yyyyMMdd")));
        }
        #endregion
    }
}
