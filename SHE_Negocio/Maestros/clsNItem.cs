using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using GEN_Entidad;
using static GEN_Entidad.Diccionario;
using GEN_Negocio;

namespace SHE_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 24/01/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNItem : clsNBase
    {

        #region Funciones
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public bool gbGuardar(Maestro toObject)
        {
            loBaseDa.CreateContext();
            bool pbResult = false;
            int psCodigo = 0;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = int.Parse(toObject.Codigo);
            var poObject = loBaseDa.Get<SHEMITEMEPP>().Where(x => x.IdItemEPP == psCodigo).FirstOrDefault();
            if (poObject != null )
            {
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;
                poObject.CodigoTipoEPP = toObject.Tipo;
                poObject.Codigo = toObject.Codigo;
                poObject.Costo = toObject.Costo;
                poObject.GrabaIva = toObject.GrabaIva;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }
            else
            {
                poObject = new SHEMITEMEPP();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.TerminalIngreso = toObject.Terminal;
                poObject.CodigoTipoEPP = toObject.Tipo;
                poObject.Codigo = toObject.Codigo;
                poObject.Costo = toObject.Costo;
                poObject.GrabaIva = toObject.GrabaIva;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject,Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }

            loBaseDa.SaveChanges();
            return pbResult;
        }

        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsDescripcion"></param>
        /// <returns></returns>
        public List<Maestro> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<SHEMITEMEPP>().Where(x =>  x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Maestro
                   {
                       Codigo = x.IdItemEPP.ToString(),
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Descripcion,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,
                       Tipo = x.CodigoTipoEPP,
                       Costo = x.Costo??0,
                       GrabaIva = x.GrabaIva ?? false
                   }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Maestro goBuscarMaestro(int tsCodigo)
        {
            return loBaseDa.Find<SHEMITEMEPP>().Where(x=>x.IdItemEPP == tsCodigo)
                .Select(x => new Maestro
                {
                    Codigo = x.IdItemEPP.ToString(),
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    Tipo = x.CodigoTipoEPP,
                    Costo = x.Costo??0,
                    GrabaIva = x.GrabaIva ?? false
                }).FirstOrDefault();
        }

        /// <summary>
        /// Buscar Codigo de la Entidad
        /// </summary>
        /// <param name="tsTipo">P: Primero, A: Anterior, S: Siguiente, U: Último</param>
        /// <param name="tsCodigo">Codigo de la entidad</param>
        /// <returns></returns>
        public string goBuscarCodigo(string tsTipo, string tsCodigo = "")
        {
            string psCodigo = string.Empty;
            if(tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<SHEMITEMEPP>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdItemEPP }).OrderBy(x => x.IdItemEPP).FirstOrDefault()?.IdItemEPP.ToString();
            }
            else if(tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<SHEMITEMEPP>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdItemEPP }).OrderByDescending(x => x.IdItemEPP).FirstOrDefault()?.IdItemEPP.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<SHEMITEMEPP>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdItemEPP }).ToList().Where(x=> x.IdItemEPP < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdItemEPP).FirstOrDefault().IdItemEPP.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<SHEMITEMEPP>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdItemEPP }).ToList().Where(x => x.IdItemEPP > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdItemEPP).FirstOrDefault().IdItemEPP.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            return psCodigo;

        }

        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public string gEliminarMaestro(int tsCodigo, string tsUsuario, string tsTerminal)
        {
            string psMsg = string.Empty;

            var poObjectMovimiento = loBaseDa.Get<SHETMOVIMIENTOINVENTARIODETALLE>().Where(x => x.IdItemEPP == tsCodigo).ToList();

            if (poObjectMovimiento != null)
            {
                return psMsg = "El item no puede ser eliminado porque ya esta en uso en las transaciones de EPP.";
            }

            var poObject = loBaseDa.Get<SHEMITEMEPP>().Where(x => x.IdItemEPP == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.SaveChanges();
            }
            return psMsg;
        }

        #endregion

    }
}
