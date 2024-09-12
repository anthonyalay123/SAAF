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
   public class clsNEstado : clsNBase
    {

        public string gsGuardar(Estado toObject)
        {
            loBaseDa.CreateContext();
            string psReturn = string.Empty;

            psReturn = lValidar(toObject);
            string psCodigo = string.Empty;
            if (!string.IsNullOrEmpty(toObject.CodigoEstado)) psCodigo = toObject.CodigoEstado;
            List<string> psCodigoEstadoExlusion = new List<string>();
            psCodigoEstadoExlusion.Add(Diccionario.Eliminado);
            psCodigoEstadoExlusion.Add(Diccionario.Inactivo);
            var poObject = loBaseDa.Get<GENMESTADO>().Where(x => x.CodigoEstado.ToString() == psCodigo).FirstOrDefault();

            if (poObject != null)
            {
             
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);

            }

            else
            {

                poObject = new GENMESTADO();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.TerminalIngreso = toObject.Terminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
            }

            loBaseDa.SaveChanges();
            return psReturn;

        }

        private string lValidar(Estado toObject)
        {
            string psReturn = string.Empty;
            if (string.IsNullOrEmpty(toObject.CodigoEstado))
            {
                psReturn = "Falta el Codigo";
            }
            if (string.IsNullOrEmpty(toObject.Descripcion))
            {
                psReturn = "Falta la Descripcion";
            }
          

            return psReturn;
        }

        public List<Estado> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<GENMESTADO>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Estado
                   {
                       Codigo = x.CodigoEstado.ToString(),
                      Descripcion = x.Descripcion,
                       
                   }).ToList();
        }

        public Estado goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<GENMESTADO>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.CodigoEstado == tsCodigo)
                   .Select(x => new Estado
                   {
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


        ///// <summary>
        ///// Elimina Registro Maestro
        ///// </summary>
        ///// <param name="tsCodigo">código de la entidad</param>
        //public void gEliminarMaestro(string tsCodigo, string tsUsuario, string tsTerminal)
        //{
        //    var poObject = loBaseDa.Get<GENMESTADO>().Where(x => x.CodigoEstado.ToString() == tsCodigo).FirstOrDefault();
        //    if (poObject != null)
        //    {
        //        poObject.CodigoEstado = Diccionario.Eliminado;
        //        poObject.FechaIngreso = DateTime.Now;
        //        poObject.UsuarioModificacion = tsUsuario;
        //        poObject.TerminalModificacion = tsTerminal;
        //        loBaseDa.SaveChanges();
        //    }
        //}



    }
}
