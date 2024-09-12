using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REH_Negocio.Seguridad
{
   public class clsNUsuarioAsignado: clsNBase
    {
        public List<Combo> goConsultarComboMenu()
        {
            return loBaseDa.Find<GENPMENU>().Where(x => x.CodigoEstado.ToString() == Diccionario.Activo && x.IdMenuPadre!= null)
                   .Select(x => new Combo
                   {
                       Codigo = x.IdMenu.ToString(),
                       Descripcion = x.Nombre.ToString(),
                   }).OrderBy(x => x.Descripcion).ToList();
        }
        /// <summary>
        /// Consulta Catálogo de Perfiles
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboUsuarioTodos()
        {
            return loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                 .Select(x => new Combo
                 {
                     Codigo = x.CodigoUsuario.ToString(),
                     Descripcion = x.NombreCompleto.ToString(),
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de Perfiles
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboUsuario()
        {
            return loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                 .Select(x => new Combo
                 {
                     Codigo = x.CodigoUsuario.ToString(),
                     Descripcion = x.NombreCompleto.ToString(),
                 }).OrderBy(x => x.Descripcion).ToList();
        }


        public List<UsuarioAsignado> goListarBandeja()
        {


            return (from SC in loBaseDa.Find<SEGPUSUARIOASIGNADO>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado }
                    equals new { E.CodigoEstado }


                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                    join CUSUARIO in loBaseDa.Find<SEGMUSUARIO>()
                    on new { cg = SC.CodigoUsuario }
                    equals new { cg = CUSUARIO.CodigoUsuario }

                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                    join CUSUARIOASI in loBaseDa.Find<SEGMUSUARIO>()
                    on new { cg = SC.CodigoUsuarioAsignado }
                    equals new { cg = CUSUARIOASI.CodigoUsuario }

                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                    join m in loBaseDa.Find<GENPMENU>()
                    on new { cg = SC.IdMenu }
                    equals new { cg = m.IdMenu }


                    // Condición de la consulta WHERE
                    where SC.CodigoEstado == Diccionario.Activo
                    // Selección de las columnas / Datos
                    select new UsuarioAsignado
                    {
                        codigoUsuarioPrincipal = SC.CodigoUsuario,
                        CodigoEstado = SC.CodigoEstado,
                        CodigoUsuarioAsignado = SC.CodigoUsuarioAsignado,
                        idMenu = SC.IdMenu,
                        UsuarioPrincipal = CUSUARIO.NombreCompleto,
                        UsuarioAsi = CUSUARIOASI.NombreCompleto,
                        Estado = E.Descripcion,
                        Menu = m.Nombre,

                    }).ToList();
        }

        public List<UsuarioAsignado> goListarBandejaUsuarioPersonaAsingado()
        {


            return (from SC in loBaseDa.Find<SEGPUSUARIOPERSONAASIGNADO>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado }
                    equals new { E.CodigoEstado }


                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                    join CUSUARIO in loBaseDa.Find<SEGMUSUARIO>()
                    on new { cg = SC.CodigoUsuario }
                    equals new { cg = CUSUARIO.CodigoUsuario }

                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                    join CUSUARIOASI in loBaseDa.Find<GENMPERSONA>()
                    on new { cg = SC.IdPersona }
                    equals new { cg = CUSUARIOASI.IdPersona }

                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                    join m in loBaseDa.Find<GENPMENU>()
                    on new { cg = SC.IdMenu }
                    equals new { cg = m.IdMenu }


                    // Condición de la consulta WHERE
                    where SC.CodigoEstado == Diccionario.Activo
                    // Selección de las columnas / Datos
                    select new UsuarioAsignado
                    {
                        codigoUsuarioPrincipal = SC.CodigoUsuario,
                        CodigoEstado = SC.CodigoEstado,
                        IdPersona = SC.IdPersona,
                        UsuarioAsi = CUSUARIOASI.NombreCompleto,
                        idMenu = SC.IdMenu,
                        UsuarioPrincipal = CUSUARIO.NombreCompleto,
                        Estado = E.Descripcion,
                        Menu = m.Nombre,

                    }).ToList();
        }


        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(UsuarioAsignado toObject, string usuario)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;
            psResult = lsEsValido(toObject);
            string psCodigo = string.Empty;
            if (string.IsNullOrEmpty(psResult))
            {
                int idMenu = Int32.Parse(toObject.Menu);
                var poObject = loBaseDa.Get<SEGPUSUARIOASIGNADO>().Where(x => x.IdMenu == idMenu && x.CodigoUsuarioAsignado == toObject.UsuarioAsi && x.CodigoUsuario == toObject.UsuarioPrincipal).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.CodigoEstado = toObject.Estado;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = usuario;
                    poObject.TerminalModificacion = "";
                }
                else
                {
                    poObject = new SEGPUSUARIOASIGNADO();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.IdMenu = Int32.Parse(toObject.Menu);
                    poObject.CodigoUsuario = toObject.UsuarioPrincipal;
                    poObject.CodigoEstado = toObject.Estado;
                    poObject.CodigoUsuarioAsignado = toObject.UsuarioAsi;


                    poObject.UsuarioIngreso = usuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso ="";
                }

                loBaseDa.SaveChanges();

            }
            return psResult;
        }

        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardarUsuarioPersonaAsingado(UsuarioAsignado toObject, string usuario)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;
            psResult = lsEsValidoUsuarioPersonaAsignado(toObject);
            string psCodigo = string.Empty;
            if (string.IsNullOrEmpty(psResult))
            {
                int idMenu = Int32.Parse(toObject.Menu);
                var poObject = loBaseDa.Get<SEGPUSUARIOPERSONAASIGNADO>().Where(x => x.IdMenu == idMenu && x.IdPersona == toObject.IdPersona && x.CodigoUsuario == toObject.UsuarioPrincipal).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.CodigoEstado = toObject.Estado;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = usuario;
                    poObject.TerminalModificacion = "";
                }
                else
                {
                    poObject = new SEGPUSUARIOPERSONAASIGNADO();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.IdMenu = Int32.Parse(toObject.Menu);
                    poObject.CodigoUsuario = toObject.UsuarioPrincipal;
                    poObject.CodigoEstado = toObject.Estado;
                    poObject.IdPersona = toObject.IdPersona;


                    poObject.UsuarioIngreso = usuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = "";
                }

                loBaseDa.SaveChanges();

            }
            return psResult;
        }

        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardarUsuarioAsignadoSap(UsuarioAsignado toObject, string usuario)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;
            psResult = lsEsValido(toObject);
            string psCodigo = string.Empty;
            if (string.IsNullOrEmpty(psResult))
            {
                int idMenu = Int32.Parse(toObject.Menu);
                var poObject = loBaseDa.Get<SEGPUSUARIOASIGNADOSAP>().Where(x => x.IdMenu == idMenu && x.USER_CODE == toObject.UsuarioAsi && x.CodigoUsuario == toObject.UsuarioPrincipal).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.CodigoEstado = toObject.Estado;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = usuario;
                    poObject.TerminalModificacion = "";
                }
                else
                {
                    poObject = new SEGPUSUARIOASIGNADOSAP();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.IdMenu = Int32.Parse(toObject.Menu);
                    poObject.CodigoUsuario = toObject.UsuarioPrincipal;
                    poObject.CodigoEstado = toObject.Estado;
                    poObject.USER_CODE = toObject.UsuarioAsi;


                    poObject.UsuarioIngreso = usuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = "";
                }

                loBaseDa.SaveChanges();

            }
            return psResult;
        }

        public List<UsuarioAsignado> goListarBandejaUsuarioAsignadoSap()
        {
            var poUsuarioASig = goSapConsultaComboUsuarios();

            var poLista = (from SC in loBaseDa.Find<SEGPUSUARIOASIGNADOSAP>()
                               // Inner Join con la tabla GENMESTADO
                           join E in loBaseDa.Find<GENMESTADO>()
                           on new { SC.CodigoEstado }
                           equals new { E.CodigoEstado }


                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                           join CUSUARIO in loBaseDa.Find<SEGMUSUARIO>()
                           on new { cg = SC.CodigoUsuario }
                           equals new { cg = CUSUARIO.CodigoUsuario }

                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                           join m in loBaseDa.Find<GENPMENU>()
                           on new { cg = SC.IdMenu }
                           equals new { cg = m.IdMenu }


                    // Condición de la consulta WHERE
                           where SC.CodigoEstado == Diccionario.Activo
                           // Selección de las columnas / Datos
                           select new UsuarioAsignado
                           {
                               codigoUsuarioPrincipal = SC.CodigoUsuario,
                               CodigoEstado = SC.CodigoEstado,
                               CodigoUsuarioAsignado = SC.USER_CODE,
                               idMenu = SC.IdMenu,
                               UsuarioPrincipal = CUSUARIO.NombreCompleto,
                               Estado = E.Descripcion,
                               Menu = m.Nombre,

                           }).ToList();

            foreach (var item in poLista)
            {
                item.UsuarioAsi = poUsuarioASig.Where(x => x.Codigo == item.CodigoUsuarioAsignado).Select(x => x.Descripcion).FirstOrDefault();
            }

            return poLista;
        }

        public string lsEsValido(UsuarioAsignado toObject)
        {
            string msg=null;
            if (toObject.Menu == Diccionario.Seleccione || toObject.Menu == null)
            {
                msg = msg + "Ingrese el Menu";
            }
            if (toObject.UsuarioAsi == Diccionario.Seleccione || toObject.UsuarioAsi == null)
            {
                msg = msg + "Ingrese el Usuario Asignado";
            }
            if (toObject.UsuarioPrincipal == Diccionario.Seleccione || toObject.UsuarioPrincipal==null)
            {
                msg = msg + "Ingrese el Usuario Principal";
            }
            return msg;
        }

        public string lsEsValidoUsuarioPersonaAsignado(UsuarioAsignado toObject)
        {
            string msg = null;
            if (toObject.Menu == Diccionario.Seleccione || toObject.Menu == null)
            {
                msg = msg + "Ingrese el Menu";
            }
            if (toObject.IdPersona.ToString() == Diccionario.Seleccione || toObject.IdPersona == null)
            {
                msg = msg + "Ingrese Persona";
            }
            if (toObject.UsuarioPrincipal == Diccionario.Seleccione || toObject.UsuarioPrincipal == null)
            {
                msg = msg + "Ingrese el Usuario";
            }
            return msg;
        }
    }
}
