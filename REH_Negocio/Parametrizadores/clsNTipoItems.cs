using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REH_Negocio.Parametrizadores
{
     public class clsNTipoItems : clsNBase
    {



        #region Funciones  
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(TipoItems toObject)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;
            psResult = lsEsValido(toObject);
            string psCodigo = string.Empty;
            if (string.IsNullOrEmpty(psResult))
            {
                if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
                var poObject = loBaseDa.Get<COMPTIPOITEMS>().Where(x => x.CodigoTipoItem.ToString() == psCodigo).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.CodigoTipoItem = toObject.Codigo;
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Descripcion = toObject.Descripcion;
                    poObject.DiasFechaEntrega = toObject.DiasFechaEntrega;
                    poObject.ItemSap = toObject.ItemSap;
                    poObject.FechaModificacion = toObject.Fecha;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;
                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                    
                }
                else
                {

                    poObject = new COMPTIPOITEMS();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoTipoItem = toObject.Codigo;
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Descripcion = toObject.Descripcion;
                    poObject.DiasFechaEntrega = toObject.DiasFechaEntrega;
                    poObject.ItemSap = toObject.ItemSap;

                    poObject.UsuarioIngreso = toObject.Usuario;
                    poObject.FechaIngreso = toObject.Fecha;
                    poObject.TerminalIngreso = toObject.Terminal;


                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                   
                }

                loBaseDa.SaveChanges();
                
            }
            return psResult;
        }


        private string lsEsValido(TipoItems toObject)
        {
            string psMsg = string.Empty;

            if (toObject.Codigo == "")
            {
                psMsg = psMsg + "El codigo no debe estar vacio. \n";
            }
            else if (toObject.Codigo.Length<3)
            {
                psMsg = psMsg + "El codigo debe tener tres caracteres. \n";
            }
            if (toObject.Descripcion=="")
            {
                psMsg = psMsg + "La descripcion no puede estar vacia. \n";
            }

            return psMsg;
        }


        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsDescripcion"></param>
        /// <returns></returns>
        public List<TipoItems> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<COMPTIPOITEMS>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new TipoItems
                   {
                       Codigo = x.CodigoTipoItem.ToString(),
                       DiasFechaEntrega = x.DiasFechaEntrega ?? 0,
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Descripcion,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,
                       ItemSap = x.ItemSap?? false,
                   }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public TipoItems goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<COMPTIPOITEMS>().Where(x => x.CodigoTipoItem.ToString() == tsCodigo)
                .Select(x => new TipoItems
                {
                    Codigo = x.CodigoTipoItem.ToString(),
                    DiasFechaEntrega = x.DiasFechaEntrega ?? 0,
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    ItemSap = x.ItemSap ?? false,
                }).FirstOrDefault();
        }

        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestro(string tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<COMPTIPOITEMS>().Where(x => x.CodigoTipoItem.ToString() == tsCodigo).FirstOrDefault();
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
        /// Consulta los datos de la tabla GENPMENU para porsteriormente llenar el Combobox 
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboMenuPadre()
        {
            return loBaseDa.Find<GENPMENU>().Where(x => x.CodigoEstado.ToString() == Diccionario.Activo)
                 .Select(x => new Combo
                 {
                     Codigo = x.IdMenu.ToString(),
                     Descripcion = x.Nombre.ToString(),
                 }).OrderBy(x => x.Codigo).ToList();
        }
        #endregion
    }
}

