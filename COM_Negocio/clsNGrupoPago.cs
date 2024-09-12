using GEN_Entidad;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GEN_Entidad.Diccionario;

namespace COM_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 06/10/2022
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNGrupoPago : clsNBase
    {
        #region Variables
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
            int piCodigo = 0;
            if (!string.IsNullOrEmpty(toObject.Codigo)) piCodigo = int.Parse(toObject.Codigo);
            var poObject = loBaseDa.Get<COMPGRUPOPAGOS>().Where(x => x.IdGrupoPagos == piCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;
                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }
            else
            {

                poObject = new COMPGRUPOPAGOS();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.TerminalIngreso = toObject.Terminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
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
            return loBaseDa.Find<COMPGRUPOPAGOS>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Maestro
                   {
                       Codigo = x.IdGrupoPagos.ToString(),
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Descripcion,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion
                   }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Maestro goBuscarMaestro(int tiCodigo)
        {
            return loBaseDa.Find<COMPGRUPOPAGOS>().Where(x => x.IdGrupoPagos == tiCodigo)
                .Select(x => new Maestro
                {
                    Codigo = x.IdGrupoPagos.ToString(),
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion
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
                psCodigo = loBaseDa.Find<COMPGRUPOPAGOS>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdGrupoPagos }).OrderBy(x => x.IdGrupoPagos).FirstOrDefault()?.IdGrupoPagos.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<COMPGRUPOPAGOS>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdGrupoPagos }).OrderByDescending(x => x.IdGrupoPagos).FirstOrDefault()?.IdGrupoPagos.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<COMPGRUPOPAGOS>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdGrupoPagos }).ToList().Where(x => x.IdGrupoPagos < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdGrupoPagos).FirstOrDefault().IdGrupoPagos.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<COMPGRUPOPAGOS>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdGrupoPagos }).ToList().Where(x => x.IdGrupoPagos > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdGrupoPagos).FirstOrDefault().IdGrupoPagos.ToString();
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
        public void gEliminarMaestro(int tiCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<COMPGRUPOPAGOS>().Where(x => x.IdGrupoPagos == tiCodigo).FirstOrDefault();
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
