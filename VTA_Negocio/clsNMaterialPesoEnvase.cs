using GEN_Entidad;
using GEN_Entidad.Entidades.SHEQ;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTA_Negocio
{
    public class clsNMaterialPesoEnvase : clsNBase
    {
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(MaterialPesoEnvase toObject)
        {
            loBaseDa.CreateContext();
            string psReturn = string.Empty;

            psReturn = lValidar(toObject);
            if (string.IsNullOrEmpty(psReturn))
            {
                string psCodigo = string.Empty;
                if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
                List<string> psCodigoEstadoExlusion = new List<string>();
                psCodigoEstadoExlusion.Add(Diccionario.Eliminado);
                psCodigoEstadoExlusion.Add(Diccionario.Inactivo);
                var poObject = loBaseDa.Get<SHEPMATERIALPESOENVASE>().Where(x => x.IdMaterialPesoEnvase.ToString() == psCodigo || x.BaseQty == toObject.BaseQty).FirstOrDefault();

                if (poObject != null)
                {
                   
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.BaseQty = toObject.BaseQty;
                    poObject.Material = toObject.Material;
                    poObject.PesoEnvase = toObject.PesoEnvase;



                    poObject.FechaModificacion = toObject.Fecha;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);

                }

                else
                {

                    poObject = new SHEPMATERIALPESOENVASE();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.BaseQty = toObject.BaseQty;
                    poObject.Material = toObject.Material;
                    poObject.PesoEnvase = toObject.PesoEnvase;
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



        private string lValidar(MaterialPesoEnvase toObject)
        {
            string psReturn = string.Empty;
            if (toObject.BaseQty<0)
            {
                psReturn = "Falta la BaseQty";
            }
            if (toObject.PesoEnvase<0)
            {
                psReturn = "Falta el Peso del Envase";
            }
            if (string.IsNullOrEmpty(toObject.Material)|| toObject.Material == Diccionario.Seleccione)
            {
                psReturn = "Falta el Material";
            }
           


            return psReturn;
        }

        public List<MaterialPesoEnvase> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<SHEPMATERIALPESOENVASE>().Where(x => x.CodigoEstado == Diccionario.Activo)
                   .Select(x => new MaterialPesoEnvase
                   {
                       Codigo = x.IdMaterialPesoEnvase.ToString(),
                       Material = x.Material,
                       BaseQty = x.BaseQty,
                       PesoEnvase = x.PesoEnvase,
                       CodigoEstado = x.CodigoEstado,
                   }).ToList();
        }

        public MaterialPesoEnvase goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<SHEPMATERIALPESOENVASE>().Where(x => x.CodigoEstado == Diccionario.Activo && x.IdMaterialPesoEnvase.ToString() == tsCodigo)
                   .Select(x => new MaterialPesoEnvase
                   {
                       Codigo = x.IdMaterialPesoEnvase.ToString(),
                       Material = x.Material,
                       BaseQty = x.BaseQty,
                       PesoEnvase = x.PesoEnvase,
                       CodigoEstado = x.CodigoEstado,
                       Usuario = x.UsuarioIngreso,
                       UsuarioMod = x.UsuarioModificacion,
                       Terminal = x.TerminalIngreso,
                       TerminalMod = x.TerminalModificacion,
                       Fecha = x.FechaIngreso,
                       FechaMod = x.FechaModificacion,
                      
                   }).FirstOrDefault();
        }

        public List<Combo> goConsultarCatalogoMaterialEnvase ()
        {
            return loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo == Diccionario.ListaCatalogo.MaterialEnvase
                                                && x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.Codigo,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }


        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestro(string tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<SHEPMATERIALPESOENVASE>().Where(x => x.IdMaterialPesoEnvase.ToString() == tsCodigo).FirstOrDefault();
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
