using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using GEN_Entidad;
using static GEN_Entidad.Diccionario;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 17/01/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNCentroCosto
    {
        #region Variables
        private readonly clsDBase loBaseDa = new clsDBase();
        #endregion

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
            string psCodigo = string.Empty;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
            var poObject = loBaseDa.Get<GENMCENTROCOSTO>().Where(x => x.CodigoCentroCosto == psCodigo).FirstOrDefault();
            if (poObject != null )
            {
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;
                poObject.Alias1 = toObject.Alias1;
                poObject.Alias2 = toObject.Alias2;
                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }
            else
            {
                string psCodigoNew = loBaseDa.GeneraSecuencia("GENMCENTROCOSTO");
                if (string.IsNullOrEmpty(psCodigoNew)) throw new Exception(string.Format("No existe configuración de secuencia."));
                poObject = new GENMCENTROCOSTO();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoCentroCosto = psCodigoNew;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.TerminalIngreso = toObject.Terminal;
                poObject.Alias1 = toObject.Alias1;
                poObject.Alias2 = toObject.Alias2;

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
            return loBaseDa.Find<GENMCENTROCOSTO>().Where(x =>  x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Maestro
                   {
                       Codigo = x.CodigoCentroCosto,
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Descripcion,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,
                       Alias1 = x.Alias1,
                       Alias2 = x.Alias2,
                   }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Maestro goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<GENMCENTROCOSTO>().Where(x=>x.CodigoCentroCosto == tsCodigo)
                .Select(x => new Maestro
                {
                    Codigo = x.CodigoCentroCosto,
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    Alias1 = x.Alias1,
                    Alias2 = x.Alias2,
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
            if (tsTipo == BuscarCodigo.Tipo.Primero || (tsTipo == BuscarCodigo.Tipo.Anterior && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<GENMCENTROCOSTO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoCentroCosto }).OrderBy(x => x.CodigoCentroCosto).FirstOrDefault()?.CodigoCentroCosto;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<GENMCENTROCOSTO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoCentroCosto }).OrderByDescending(x => x.CodigoCentroCosto).FirstOrDefault()?.CodigoCentroCosto;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<GENMCENTROCOSTO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoCentroCosto }).ToList().Where(x => int.Parse(x.CodigoCentroCosto) < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.CodigoCentroCosto).FirstOrDefault().CodigoCentroCosto;
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<GENMCENTROCOSTO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoCentroCosto }).ToList().Where(x => int.Parse(x.CodigoCentroCosto) > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.CodigoCentroCosto).FirstOrDefault().CodigoCentroCosto;
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
        public void gEliminarMaestro(string tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<GENMCENTROCOSTO>().Where(x => x.CodigoCentroCosto == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.SaveChanges();
            }
        }
        #endregion

    }
}
