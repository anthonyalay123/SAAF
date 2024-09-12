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
   public class clsNTransaccion : clsNBase
    {

        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(PaTransaccion toObject)
        {
            loBaseDa.CreateContext();
            string psReturn = string.Empty;

            psReturn =  lValidar(toObject);
            string psCodigo = string.Empty;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
            List<string> psCodigoEstadoExlusion = new List<string>();
            psCodigoEstadoExlusion.Add(Diccionario.Eliminado);
            psCodigoEstadoExlusion.Add(Diccionario.Inactivo);
            var poObject = loBaseDa.Get<GENPTRANSACCION>().Where(x => x.CodigoTransaccion.ToString() == psCodigo).FirstOrDefault();
        
            if (poObject != null)
            {
                poObject.CodigoTransaccion=toObject.Codigo;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.CantidadAutorizaciones = toObject.CantidadAutorizacion;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;
        
                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
               
            }
            
            else
            {

                poObject = new GENPTRANSACCION();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.CantidadAutorizaciones = toObject.CantidadAutorizacion;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.TerminalIngreso = toObject.Terminal;
                poObject.CodigoTransaccion = toObject.Codigo;
                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
            }

            loBaseDa.SaveChanges();
            return psReturn;

        }


        private string lValidar(PaTransaccion toObject)
        {
            string psReturn = string.Empty;
            if (string.IsNullOrEmpty(toObject.Descripcion))
            {
                psReturn = "Falta la descripción";
            }
            if (toObject.CantidadAutorizacion<0)
            {
                psReturn = "Falta la Cantidad de Autorización";
            }


            return psReturn;
        }

        public List<PaTransaccion> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new PaTransaccion
                   {
                       Codigo = x.CodigoTransaccion.ToString(),
                       Descripcion = x.Descripcion,
                       CodigoEstado = x.CodigoEstado,
                   }).ToList();
        }


        public PaTransaccion goBuscarMaestro(string tsCodigo )
        {
            return loBaseDa.Find<GENPTRANSACCION>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.CodigoTransaccion == tsCodigo)
                   .Select(x => new PaTransaccion
                   {
                       Codigo = x.CodigoTransaccion.ToString(),
                       Descripcion = x.Descripcion,
                       CodigoEstado = x.CodigoEstado,
                       CantidadAutorizacion = x.CantidadAutorizaciones,
                       Usuario =x.UsuarioIngreso,
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
            var poObject = loBaseDa.Get<GENPTRANSACCION>().Where(x => x.CodigoTransaccion.ToString() == tsCodigo).FirstOrDefault();
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
