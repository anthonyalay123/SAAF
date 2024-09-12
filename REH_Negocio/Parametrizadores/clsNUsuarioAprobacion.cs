using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REH_Negocio.Parametrizadores
{
    public class clsNUsuarioAprobacion : clsNBase
    {
        public List<UsuarioAprobacion> goListarMaestro(string tsDescripcion = "")
        {
            return (from SC in loBaseDa.Find<SEGPUSUARIOAPROBACION>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENPMENU>()
                    on new { SC.IdMenu } equals new { E.IdMenu }

                    join U in loBaseDa.Find<SEGMUSUARIO>()
                on new { SC.CodigoUsuario } equals new { U.CodigoUsuario }

                // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado
                    // Selección de las columnas / Datos
                    select new UsuarioAprobacion
                    {
                        Codigo = SC.IdAprobaciones.ToString(),
                        Descripcion = E.Nombre,
                        NombreUsuario = U.NombreCompleto,
                        CodigoEstado = E.CodigoEstado,
                        inicio = SC.CantAproInicio,
                        Fin = SC.CantAproFin
                    }).ToList();


       
        }

        public List<UsuarioAprobacion> goListarMaestroExcepcion(string tsDescripcion = "")
        {
            return (from SC in loBaseDa.Find<SEGPUSUARIOAPROBACIONEXCEPCION>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENPMENU>()
                    on new { SC.IdMenu } equals new { E.IdMenu }

                    join U in loBaseDa.Find<SEGMUSUARIO>()
                on new { SC.CodigoUsuario } equals new { U.CodigoUsuario }

                // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado
                    // Selección de las columnas / Datos
                    select new UsuarioAprobacion
                    {
                        Codigo = SC.IdUsuarioAprobacionExcepcion.ToString(),
                        Descripcion = E.Nombre,
                        NombreUsuario = U.NombreCompleto,
                        CodigoEstado = E.CodigoEstado,
                    }).ToList();
        }
        
        public UsuarioAprobacion goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<SEGPUSUARIOAPROBACION>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdAprobaciones.ToString() == tsCodigo)
                   .Select(x => new UsuarioAprobacion
                   {
                       Codigo = x.IdAprobaciones.ToString(),
                       idMenu = x.IdMenu,
                       CodigoEstado = x.CodigoEstado,
                       inicio = x.CantAproInicio,
                       Fin = x.CantAproFin,
                       NombreUsuario = x.CodigoUsuario,
                       Usuario = x.UsuarioIngreso,
                       UsuarioMod = x.UsuarioModificacion,
                       Terminal = x.TerminalIngreso,
                       TerminalMod = x.TerminalModificacion,
                       Fecha = x.FechaIngreso,
                       FechaMod = x.FechaModificacion,

                   }).FirstOrDefault();
        }

        public UsuarioAprobacion goBuscarMaestroExcepcion(string tsCodigo)
        {
            return loBaseDa.Find<SEGPUSUARIOAPROBACIONEXCEPCION>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdUsuarioAprobacionExcepcion.ToString() == tsCodigo)
                   .Select(x => new UsuarioAprobacion
                   {
                       Codigo = x.IdUsuarioAprobacionExcepcion.ToString(),
                       idMenu = x.IdMenu,
                       CodigoEstado = x.CodigoEstado,
                       NombreUsuario = x.CodigoUsuario,
                       Usuario = x.UsuarioIngreso,
                       UsuarioMod = x.UsuarioModificacion,
                       Terminal = x.TerminalIngreso,
                       TerminalMod = x.TerminalModificacion,
                       Fecha = x.FechaIngreso,
                       FechaMod = x.FechaModificacion,

                   }).FirstOrDefault();
        }

        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(UsuarioAprobacion toObject)
        {
            loBaseDa.CreateContext();
            string psReturn = string.Empty;

            psReturn = lValidar(toObject);
            string psCodigo = string.Empty;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
            List<string> psCodigoEstadoExlusion = new List<string>();
            psCodigoEstadoExlusion.Add(Diccionario.Eliminado);
            psCodigoEstadoExlusion.Add(Diccionario.Inactivo);
            var poObject = loBaseDa.Get<SEGPUSUARIOAPROBACION>().Where(x => x.IdAprobaciones.ToString() == psCodigo).FirstOrDefault();

            if (poObject != null)
            {
                poObject.IdMenu = toObject.idMenu;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.CantAproFin = toObject.Fin;
                poObject.CantAproInicio = toObject.inicio;
                poObject.CodigoUsuario = toObject.NombreUsuario;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);

            }

            else
            {

                poObject = new SEGPUSUARIOAPROBACION();
                loBaseDa.CreateNewObject(out poObject);
                poObject.IdMenu = toObject.idMenu;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.CantAproFin = toObject.Fin;
                poObject.CantAproInicio = toObject.inicio;
                poObject.CodigoUsuario = toObject.NombreUsuario;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.TerminalIngreso = toObject.Terminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
            }

            loBaseDa.SaveChanges();
            return psReturn;

        }

        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardarExcepcion(UsuarioAprobacion toObject)
        {
            loBaseDa.CreateContext();
            string psReturn = string.Empty;

            psReturn = lValidar(toObject);
            string psCodigo = string.Empty;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
            List<string> psCodigoEstadoExlusion = new List<string>();
            psCodigoEstadoExlusion.Add(Diccionario.Eliminado);
            psCodigoEstadoExlusion.Add(Diccionario.Inactivo);
            var poObject = loBaseDa.Get<SEGPUSUARIOAPROBACIONEXCEPCION>().Where(x => x.IdUsuarioAprobacionExcepcion.ToString() == psCodigo).FirstOrDefault();

            if (poObject != null)
            {
                poObject.IdMenu = toObject.idMenu;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.CodigoUsuario = toObject.NombreUsuario;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
            }
            else
            {
                poObject = new SEGPUSUARIOAPROBACIONEXCEPCION();
                loBaseDa.CreateNewObject(out poObject);
                poObject.IdMenu = toObject.idMenu;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.CodigoUsuario = toObject.NombreUsuario;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.TerminalIngreso = toObject.Terminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
            }

            loBaseDa.SaveChanges();
            return psReturn;

        }

        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestro(string tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<SEGPUSUARIOAPROBACION>().Where(x => x.IdAprobaciones.ToString() == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.SaveChanges();
            }
        }

        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestroExcepcion(string tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<SEGPUSUARIOAPROBACIONEXCEPCION>().Where(x => x.IdUsuarioAprobacionExcepcion.ToString() == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.SaveChanges();
            }
        }

        private string lValidar(UsuarioAprobacion toObject)
        {
            string psReturn = string.Empty;
            if (string.IsNullOrEmpty(toObject.NombreUsuario) || toObject.NombreUsuario== Diccionario.Seleccione)
            {
                psReturn += "Falta Usuario \n";
            }
            if (toObject.idMenu <= 0)
            {
                psReturn += "Falta la descripción \n";
            }
            if (toObject.inicio < 0)
            {
                psReturn += "Falta la Cantidad de inicio \n";
            }
            if (toObject.Fin < 0)
            {
                psReturn += "Falta la Cantidad de fin \n";
            }

            return psReturn;
        }



    }
}
