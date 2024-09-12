using GEN_Entidad;
using GEN_Entidad.Entidades.REH;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REH_Negocio.Parametrizadores
{
    public class clsNGestorConsultaPerfil : clsNBase
    {


        /// <summary>
        /// Consulta Catálogo de Perfiles
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboGestorConsulta()
        {
            return loBaseDa.Find<REHMGESTORCONSULTA>()/*.Where(x => x.CodigoEstado == Diccionario.Activo)*/
                 .Select(x => new Combo
                 {
                     Codigo = x.Id.ToString(),
                     Descripcion = x.Nombre.ToString(),
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de Perfiles
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboPerfil()
        {
            return loBaseDa.Find<GENPPERFIL>()/*.Where(x => x.CodigoEstado == Diccionario.Activo)*/
                 .Select(x => new Combo
                 {
                     Codigo = x.IdPerfil.ToString(),
                     Descripcion = x.Nombre.ToString(),
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(GestorConsultaPerfil toObject)
        {
            loBaseDa.CreateContext();
            string psReturn = string.Empty;

            psReturn = lValidar(toObject);
            if (string.IsNullOrEmpty(psReturn))
            {
                //  string psCodigo = string.Empty;
                //if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.CodigoTipoComision;
                List<string> psCodigoEstadoExlusion = new List<string>();
                psCodigoEstadoExlusion.Add(Diccionario.Eliminado);
                psCodigoEstadoExlusion.Add(Diccionario.Inactivo);
                var poObject = loBaseDa.Get<GENPGESTORCONSULTAPERFIL>().Where(x => x.Id == toObject.IdGestorConsulta && x.IdPerfil == toObject.IdPerfil).FirstOrDefault();

                if (poObject != null)
                {

                    poObject.CodigoEstado = toObject.CodigoEstado;

                    poObject.FechaModificacion = toObject.Fecha;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);

                }

                else
                {

                    poObject = new GENPGESTORCONSULTAPERFIL();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Id = toObject.IdGestorConsulta;
                    poObject.IdPerfil = toObject.IdPerfil;
                    poObject.UsuarioIngreso = toObject.Usuario;
                    poObject.FechaIngreso = toObject.Fecha;
                    poObject.TerminalIngreso = toObject.Terminal;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                }

                loBaseDa.SaveChanges();
            }
            
            return psReturn;

        }

        private string lValidar(GestorConsultaPerfil toObject)
        {
            string psReturn = string.Empty;
            if (toObject.IdPerfil<=0)
            {
                psReturn += "Falta el Perfil \n";
            }
            if (toObject.IdGestorConsulta <= 0)
            {
                psReturn += "Falta el Gestor de Consulta \n";
            }



            return psReturn;

        }


        public List<GestorConsultaPerfil> goListarMaestro(string tsDescripcion = "")
        {
            return (from SC in loBaseDa.Find<GENPGESTORCONSULTAPERFIL>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<REHMGESTORCONSULTA>()
                    on new { SC.Id } equals new { E.Id }

                    join U in loBaseDa.Find<GENPPERFIL>()
                on new { SC.IdPerfil } equals new { U.IdPerfil }

                // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado
                    // Selección de las columnas / Datos
                    select new GestorConsultaPerfil
                    {
                        IdPerfil = U.IdPerfil,
                        IdGestorConsulta = E.Id,
                        DesGestorConsulta = E.Nombre,
                        DesPerfil = U.Nombre,
                        
                        CodigoEstado = SC.CodigoEstado
                    }).ToList();
        }

        public GestorConsultaPerfil goBuscarMaestro(string tsId, string tsIdPerfil)
        {
            return loBaseDa.Find<GENPGESTORCONSULTAPERFIL>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.Id.ToString() == tsId && x.IdPerfil.ToString() == tsIdPerfil)
                   .Select(x => new GestorConsultaPerfil
                   {
                       IdPerfil = x.IdPerfil,
                       IdGestorConsulta = x.Id,
                       CodigoEstado = x.CodigoEstado,

                       Usuario = x.UsuarioIngreso,
                       UsuarioMod = x.UsuarioModificacion,
                       Terminal = x.TerminalIngreso,
                       TerminalMod = x.TerminalModificacion,
                       Fecha = x.FechaIngreso,
                       FechaMod = x.FechaModificacion,


                   }).FirstOrDefault();
        }



        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestro(string tsId, string tsIdPerfil, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<GENPGESTORCONSULTAPERFIL>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.Id.ToString() == tsId && x.IdPerfil.ToString() == tsIdPerfil).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.SaveChanges();
            }
        }
    }
}
