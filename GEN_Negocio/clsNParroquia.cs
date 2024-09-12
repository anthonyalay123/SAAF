using GEN_Entidad;
using GEN_Entidad.Entidades.General;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Negocio
{
    public class clsNParroquia : clsNBase
    {
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(Parroquia toObject)
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
                var poObject = loBaseDa.Get<GENPPARROQUIA>().Where(x => x.IdParroquia.ToString() == toObject.Codigo && !psCodigoEstadoExlusion.Contains(x.CodigoEstado)).FirstOrDefault();

                if (poObject != null)
                {

                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.IdCanton = toObject.IdCanton;
                    poObject.Descripcion = toObject.Descripcion;
                    poObject.FechaModificacion = toObject.Fecha;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);

                }

                else
                {

                    poObject = new GENPPARROQUIA();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.IdCanton = toObject.IdCanton;
                    poObject.Descripcion = toObject.Descripcion;
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


        private string lValidar(Parroquia toObject)
        {
            string psReturn = string.Empty;
            if (string.IsNullOrEmpty(toObject.Descripcion))
            {
                psReturn += "Falta la descripcion \n";
            }
            if (toObject.IdCanton <= 0)
            {
                psReturn += "Falta el Cantón \n";
            }
            
            return psReturn;
        }

        public List<Parroquia> goListarMaestro(string tsDescripcion = "")
        {
            return (from SC in loBaseDa.Find<GENPPARROQUIA>()

                        // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado
                    // Selección de las columnas / Datos
                    select new Parroquia
                    {
                        Codigo = SC.IdParroquia.ToString(),
                        Descripcion = SC.Descripcion,

                        CodigoEstado = SC.CodigoEstado
                    }).ToList();
        }

        public Parroquia goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<GENPPARROQUIA>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdParroquia.ToString() == tsCodigo)
                   .Select(x => new Parroquia
                   {
                       Codigo = x.IdParroquia.ToString(),
                       IdCanton = x.IdCanton,
                       Descripcion = x.Descripcion,
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
        public void gEliminarMaestro(string tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<GENPPARROQUIA>().Where(x => x.IdParroquia.ToString() == tsCodigo).FirstOrDefault();
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
