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
    public class clsNTipoComisionPermiso : clsNBase
    {

        /// <summary>
        /// Consulta Catálogo de Perfiles
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoPermiso()
        {
            return loBaseDa.Find<REHPTIPOPERMISO>()/*.Where(x => x.CodigoEstado == Diccionario.Activo)*/
                 .Select(x => new Combo
                 {
                     Codigo = x.CodigoTipoPermiso.ToString(),
                     Descripcion = x.Descripcion.ToString(),
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de Perfiles
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoComision()
        {
            return loBaseDa.Find<REHMTIPOCOMISION>()/*.Where(x => x.CodigoEstado == Diccionario.Activo)*/
                 .Select(x => new Combo
                 {
                     Codigo = x.CodigoTipoComision.ToString(),
                     Descripcion = x.Descripcion.ToString(),
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(TipoComisionPermiso toObject)
        {
            loBaseDa.CreateContext();
            string psReturn = string.Empty;

            psReturn = lValidar(toObject);
            if (!string.IsNullOrEmpty(psReturn))
            {
                //  string psCodigo = string.Empty;
                //if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.CodigoTipoComision;
                List<string> psCodigoEstadoExlusion = new List<string>();
                psCodigoEstadoExlusion.Add(Diccionario.Eliminado);
                psCodigoEstadoExlusion.Add(Diccionario.Inactivo);
                var poObject = loBaseDa.Get<REHPTIPOCOMISIONPERMISO>().Where(x => x.CodigoTipoComision.ToString() == toObject.CodigoTipoComision && x.CodigoTipoPermiso == toObject.CodigoTipoPermiso).FirstOrDefault();

                if (poObject != null)
                {

                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.DescuentaValorComision = toObject.DescuentoValorComision;
                    poObject.FechaModificacion = toObject.Fecha;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);

                }

                else
                {

                    poObject = new REHPTIPOCOMISIONPERMISO();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.CodigoTipoPermiso = toObject.CodigoTipoPermiso;
                    poObject.CodigoTipoComision = toObject.CodigoTipoComision;
                    poObject.DescuentaValorComision = toObject.DescuentoValorComision;
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

        private string lValidar(TipoComisionPermiso toObject)
        {
            if (true)
            {

            }
          
            string psReturn = string.Empty;
            if (string.IsNullOrEmpty(toObject.CodigoTipoComision) || toObject.CodigoTipoComision== Diccionario.Seleccione)
            {
                psReturn += "Falta el Tipo Comision \n";
            }
            if (string.IsNullOrEmpty(toObject.CodigoTipoPermiso) || toObject.CodigoTipoPermiso == Diccionario.Seleccione)
            {
                psReturn += "Falta el Tipo Permiso \n";
            }



            return psReturn;

        }


        public List<TipoComisionPermiso> goListarMaestro(string tsDescripcion = "")
        {
            return (from SC in loBaseDa.Find<REHPTIPOCOMISIONPERMISO>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<REHPTIPOPERMISO>()
                    on new { SC.CodigoTipoPermiso } equals new { E.CodigoTipoPermiso }

                    join U in loBaseDa.Find<REHMTIPOCOMISION>()
                on new { SC.CodigoTipoComision } equals new { U.CodigoTipoComision }

                // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado
                    // Selección de las columnas / Datos
                    select new TipoComisionPermiso
                    {
                        CodigoTipoComision = U.Descripcion.ToString(),
                        CodigoTipoPermiso = E.Descripcion,
                        CodCodigoTipoPermiso= SC.CodigoTipoPermiso,
                        CodCodigoTipoComision = SC.CodigoTipoComision,

                        CodigoEstado = SC.CodigoEstado
                    }).ToList();
        }

        public TipoComisionPermiso goBuscarMaestro(string tsCodigoTipoPermiso, string tsCodigoComision)
        {
            return loBaseDa.Find<REHPTIPOCOMISIONPERMISO>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.CodigoTipoComision.ToString() == tsCodigoComision && x.CodigoTipoPermiso.ToString() == tsCodigoTipoPermiso)
                   .Select(x => new TipoComisionPermiso
                   {
                       CodigoTipoComision = x.CodigoTipoComision.ToString(),
                       CodigoTipoPermiso = x.CodigoTipoComision,
                       DescuentoValorComision = x.DescuentaValorComision,
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
        public void gEliminarMaestro(string tsCodigoTipoPermiso, string tsCodigoComision, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<REHPTIPOCOMISIONPERMISO>().Where(x => x.CodigoTipoComision.ToString() == tsCodigoComision && x.CodigoTipoPermiso.ToString() == tsCodigoTipoPermiso).FirstOrDefault();
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
