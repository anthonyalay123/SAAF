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
    public class clsNTipoContrato
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
            var poObject = loBaseDa.Get<REHMTIPOCONTRATO>().Where(x => x.CodigoTipoContrato == psCodigo).FirstOrDefault();
            if (poObject != null )
            {
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;
                poObject.AsumeIessEmpresa = toObject.AsumeIessEmpresa;
                poObject.PagoUtilidades = toObject.PagoUtilidades;
                poObject.PagoVacacionesMensual = toObject.PagoVacacionesMensual;
                poObject.ProvisionVac = toObject.ProvicionVacaciones;
                poObject.ProvisionD3 = toObject.ProvisionD3;
                poObject.ProvisionD4 = toObject.ProvisionD4;
                poObject.PorcAportePatronal = toObject.PorcAportePatronal;
                poObject.PorcAportePersonal = toObject.PorcAportePersonal;
                poObject.PorcAporteSecap = toObject.PorcAporteSecap;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }
            else
            {
                string psCodigoNew = loBaseDa.GeneraSecuencia("REHMTIPOCONTRATO");
                if (string.IsNullOrEmpty(psCodigoNew)) throw new Exception(string.Format("No existe configuración de secuencia."));
                poObject = new REHMTIPOCONTRATO();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoTipoContrato = psCodigoNew;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.TerminalIngreso = toObject.Terminal;
                poObject.AsumeIessEmpresa = toObject.AsumeIessEmpresa;
                poObject.PagoUtilidades = toObject.PagoUtilidades;
                poObject.PagoVacacionesMensual = toObject.PagoVacacionesMensual;
                poObject.ProvisionVac = toObject.ProvicionVacaciones;
                poObject.ProvisionD3 = toObject.ProvisionD3;
                poObject.ProvisionD4 = toObject.ProvisionD4;
                poObject.PorcAportePatronal = toObject.PorcAportePatronal;
                poObject.PorcAportePersonal = toObject.PorcAportePersonal;
                poObject.PorcAporteSecap = toObject.PorcAporteSecap;

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
            return loBaseDa.Find<REHMTIPOCONTRATO>().Where(x => x.Descripcion.Contains(tsDescripcion))
                   .Select(x => new Maestro
                   {
                       Codigo = x.CodigoTipoContrato,
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Descripcion,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,
                       AsumeIessEmpresa = x.AsumeIessEmpresa ?? false,
                       PagoUtilidades = x.PagoUtilidades ?? false,
                       PagoVacacionesMensual = x.PagoVacacionesMensual ?? false,
                       ProvicionVacaciones = x.ProvisionVac ?? false,
                       ProvisionD3 = x.ProvisionD3??false,
                       ProvisionD4 = x.ProvisionD4??false,
                       PorcAportePersonal = x.PorcAportePersonal??0,
                       PorcAportePatronal = x.PorcAportePatronal??0,
                       PorcAporteSecap = x.PorcAporteSecap??0,

                       
                   }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Maestro goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<REHMTIPOCONTRATO>().Where(x=>x.CodigoTipoContrato == tsCodigo && x.CodigoEstado != Diccionario.Eliminado)
                .Select(x => new Maestro
                {
                    Codigo = x.CodigoTipoContrato,
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    AsumeIessEmpresa = x.AsumeIessEmpresa ?? false,
                    PagoUtilidades = x.PagoUtilidades ?? false,
                    PagoVacacionesMensual = x.PagoVacacionesMensual ?? false,
                    ProvicionVacaciones = x.ProvisionVac ?? false,
                    ProvisionD3 = x.ProvisionD3 ?? false,
                    ProvisionD4 = x.ProvisionD4 ?? false,
                    PorcAportePersonal = x.PorcAportePersonal ?? 0,
                    PorcAportePatronal = x.PorcAportePatronal ?? 0,
                    PorcAporteSecap = x.PorcAporteSecap ?? 0,
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
                psCodigo = loBaseDa.Find<REHMTIPOCONTRATO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoContrato }).OrderBy(x => x.CodigoTipoContrato).FirstOrDefault()?.CodigoTipoContrato;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<REHMTIPOCONTRATO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoContrato }).OrderByDescending(x => x.CodigoTipoContrato).FirstOrDefault()?.CodigoTipoContrato;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<REHMTIPOCONTRATO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoContrato }).ToList().Where(x => int.Parse(x.CodigoTipoContrato) < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.CodigoTipoContrato).FirstOrDefault().CodigoTipoContrato;
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<REHMTIPOCONTRATO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoContrato }).ToList().Where(x => int.Parse(x.CodigoTipoContrato) > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.CodigoTipoContrato).FirstOrDefault().CodigoTipoContrato;
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
            var poObject = loBaseDa.Get<REHMTIPOCONTRATO>().Where(x => x.CodigoTipoContrato == tsCodigo).FirstOrDefault();
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
