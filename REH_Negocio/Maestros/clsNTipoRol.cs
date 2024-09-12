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
    /// Fecha: 24/01/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNTipoRol
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
        public bool gbGuardar(TipoRol toObject)
        {
            loBaseDa.CreateContext();
            bool pbResult = false;
            string psCodigo = string.Empty;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
            var poObject = loBaseDa.Get<REHMTIPOROL>().Where(x => x.CodigoTipoRol == psCodigo).FirstOrDefault();
            if (poObject != null )
            {
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.Codigo = toObject.CodigoPeriodo;
                poObject.Dias = toObject.Dias;
                poObject.OrdenPagoMensual = toObject.OrdenPagoMensual;
                poObject.DiaPago = toObject.DiaPago;
                poObject.DiaMaxReverso = toObject.DiaMaxReverso;
                poObject.AplicaDecimoTercero = toObject.DecimoTercero;
                poObject.AplicaImpuestoRenta = toObject.ImpuestoRenta;
           




                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;
                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }
            else
            {
                string psCodigoNew = loBaseDa.GeneraSecuencia("REHMTIPOROL");
                if (string.IsNullOrEmpty(psCodigoNew)) throw new Exception(string.Format("No existe configuración de secuencia."));
                poObject = new REHMTIPOROL();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoTipoRol = psCodigoNew;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.Codigo = toObject.CodigoPeriodo;
                poObject.Dias = toObject.Dias;
                poObject.OrdenPagoMensual = toObject.OrdenPagoMensual;
                poObject.DiaPago = toObject.DiaPago;
                poObject.DiaMaxReverso = toObject.DiaMaxReverso;
                poObject.AplicaDecimoTercero = toObject.DecimoTercero;
                poObject.AplicaImpuestoRenta = toObject.ImpuestoRenta;

                poObject.FechaIngreso = toObject.Fecha;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.TerminalIngreso = toObject.Terminal;

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
        public List<TipoRol> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<REHMTIPOROL>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new TipoRol
                   {
                       Codigo = x.CodigoTipoRol,
                       DiaPago = x.DiaPago ?? 0,
                       DiaMaxReverso = x.DiaMaxReverso ?? 0,
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Descripcion,
                       CodigoPeriodo = x.Codigo,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,
                       Dias = x.Dias ?? 0,
                       OrdenPagoMensual = x.OrdenPagoMensual ?? 0,
                       DecimoTercero = x.AplicaDecimoTercero?? false,
                       ImpuestoRenta = x.AplicaImpuestoRenta?? false

                   }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public TipoRol goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<REHMTIPOROL>().Where(x=>x.CodigoTipoRol == tsCodigo)
                .Select(x => new TipoRol
                {
                    Codigo = x.CodigoTipoRol,
                    DiaPago = x.DiaPago ?? 0,
                    DiaMaxReverso = x.DiaMaxReverso ?? 0,
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    CodigoPeriodo = x.Codigo,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                     Dias = x.Dias ?? 0,
                    OrdenPagoMensual = x.OrdenPagoMensual ?? 0,
                    DecimoTercero = x.AplicaDecimoTercero ?? false,
                    ImpuestoRenta = x.AplicaImpuestoRenta ?? false
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
                psCodigo = loBaseDa.Find<REHMTIPOROL>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoRol }).OrderBy(x => x.CodigoTipoRol).FirstOrDefault()?.CodigoTipoRol;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<REHMTIPOROL>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoRol }).OrderByDescending(x => x.CodigoTipoRol).FirstOrDefault()?.CodigoTipoRol;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<REHMTIPOROL>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoRol }).ToList().Where(x => int.Parse(x.CodigoTipoRol) < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.CodigoTipoRol).FirstOrDefault().CodigoTipoRol;
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<REHMTIPOROL>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoRol }).ToList().Where(x => int.Parse(x.CodigoTipoRol) > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.CodigoTipoRol).FirstOrDefault().CodigoTipoRol;
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
            var poObject = loBaseDa.Get<REHMTIPOROL>().Where(x => x.CodigoTipoRol == tsCodigo).FirstOrDefault();
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
