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
    public class clsNFeriado : clsNBase
    {

        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsDescripcion"></param>
        /// <returns></returns>
        public List<Feriado> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<GENMFERIADO>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Feriado
                   {
                       Codigo = x.IdFeriado.ToString(),
                     
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Descripcion,
                       FechaFeriado = x.Fecha,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,

                   }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Feriado goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<GENMFERIADO>().Where(x => x.IdFeriado.ToString() == tsCodigo)
                .Select(x => new Feriado
                {
                    Codigo = x.IdFeriado.ToString(),
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    FechaFeriado = x.Fecha,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,

                }).FirstOrDefault();
        }

        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestro(string tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<GENMFERIADO>().Where(x => x.IdFeriado.ToString() == tsCodigo).FirstOrDefault();
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
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(Feriado toObject)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;
           psResult =  lValidar(toObject);
            if (string.IsNullOrEmpty(psResult))
            {
                string psCodigo = string.Empty;
                if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
                List<string> psCodigoEstadoExlusion = new List<string>();
                psCodigoEstadoExlusion.Add(Diccionario.Eliminado);
                psCodigoEstadoExlusion.Add(Diccionario.Inactivo);
                var poObject = loBaseDa.Get<GENMFERIADO>().Where(x => x.IdFeriado.ToString() == psCodigo).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.IdFeriado = Convert.ToInt32(toObject.Codigo);
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Descripcion = toObject.Descripcion;
                    poObject.Fecha = toObject.FechaFeriado;
                    poObject.FechaModificacion = toObject.Fecha;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);

                }
                else
                {

                    poObject = new GENMFERIADO();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Descripcion = toObject.Descripcion;
                    poObject.Fecha = toObject.FechaFeriado;
                    poObject.UsuarioIngreso = toObject.Usuario;
                    poObject.FechaIngreso = toObject.Fecha;
                    poObject.TerminalIngreso = toObject.Terminal;

                }

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);



                loBaseDa.SaveChanges();
            }
           
            return psResult;
        }

        private string lValidar(Feriado toObject)
        {
            string psResult = string.Empty;
            var poPobject = loBaseDa.Get<GENMFERIADO>().Where(x => x.Fecha == toObject.FechaFeriado && x.IdFeriado.ToString() !=toObject.Codigo).FirstOrDefault();
            if (poPobject!=null)
            {
                psResult += "La fecha ya ha sido utilizada. \n";
            }
            var y = new DateTime();
            if (string.IsNullOrEmpty(toObject.FechaFeriado.ToString()) || toObject.FechaFeriado == new DateTime())
            {
                psResult += "Falta Agregar Fecha. \n";
            }
            if (string.IsNullOrEmpty(toObject.Descripcion))
            {
                psResult += "Falta Agregar Descripcion. \n";
            }
                
            return psResult;
        }

    }
}
