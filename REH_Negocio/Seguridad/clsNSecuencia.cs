using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REH_Negocio.Seguridad
{
    public class clsNSecuencia : clsNBase
    {
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(Secuencia toObject)
        {
            loBaseDa.CreateContext();
            string psReturn = string.Empty;

            psReturn = lValidar(toObject);
            string psCodigo = string.Empty;
            if (!string.IsNullOrEmpty(toObject.NombreTabla)) psCodigo = toObject.NombreTabla;
            List<string> psCodigoEstadoExlusion = new List<string>();
            psCodigoEstadoExlusion.Add(Diccionario.Eliminado);
            psCodigoEstadoExlusion.Add(Diccionario.Inactivo);
            var poObject = loBaseDa.Get<GENPSECUENCIA>().Where(x => x.NombreTabla.ToString() == psCodigo).FirstOrDefault();

            if (poObject != null)
            {
                poObject.NombreTabla = toObject.NombreTabla;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.ProximaSecuencia = toObject.ProximaSecuencia;
                poObject.Formato = toObject.Formato;
                poObject.Prefijo = toObject.Prefijo;
                poObject.Longitud = toObject.Logitud;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);

            }

            else
            {

                poObject = new GENPSECUENCIA();
                loBaseDa.CreateNewObject(out poObject);
                poObject.NombreTabla = toObject.NombreTabla;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.ProximaSecuencia = toObject.ProximaSecuencia;
                poObject.Formato = toObject.Formato;
                poObject.Prefijo = toObject.Prefijo;
                poObject.Longitud = toObject.Logitud;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.TerminalIngreso = toObject.Terminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
            }

            loBaseDa.SaveChanges();
            return psReturn;

        }


        private string lValidar(Secuencia toObject)
        {
           string psReturn = string.Empty;
            if (string.IsNullOrEmpty(toObject.Prefijo))
            {
                psReturn = "Falta la descripción";
            }
            if (toObject.ProximaSecuencia<0)
            {
                psReturn = "Falta la proxima secuencia";
            }
            if (toObject.Logitud < 0)
            {
                psReturn = "Falta la proxima secuencia";
            }


            return psReturn;
        }

        public List<Secuencia> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<GENPSECUENCIA>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Secuencia
                   {
                       Codigo = x.NombreTabla.ToString(),
                     //  Descripcion = x.Descripcion,
                       CodigoEstado = x.CodigoEstado,
                   }).ToList();
        }


        public Secuencia goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<GENPSECUENCIA>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.NombreTabla == tsCodigo)
                   .Select(x => new Secuencia
                   {
                       NombreTabla = x.NombreTabla,
                       ProximaSecuencia = x.ProximaSecuencia,
                       CodigoEstado = x.CodigoEstado,
                       Formato = x.Formato,
                       Prefijo = x.Prefijo,
                       Logitud = x.Longitud,  
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
            var poObject = loBaseDa.Get<GENPSECUENCIA>().Where(x => x.NombreTabla.ToString() == tsCodigo).FirstOrDefault();
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
