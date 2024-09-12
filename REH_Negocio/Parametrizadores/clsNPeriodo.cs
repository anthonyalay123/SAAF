using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using GEN_Entidad;
using GEN_Negocio;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 07/02/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNPeriodo : clsNBase
    {
        #region Funciones

        /// <summary>
        /// Consulta de Datos de Tipo Rol
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public string gsConsultaDatosTipoRol(string tsCodigo)
        {
            string psValor = string.Empty;
            psValor = loBaseDa.Find<REHMTIPOROL>().Where(x => x.CodigoTipoRol == tsCodigo).Select(x => x.Codigo).FirstOrDefault();
            return psValor;
        }

        /// <summary>
        /// Consulta de Datos del Periodo
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public List<Periodo> gsConsultaPeriodos(int tiAnio)
        {
            return  (from a in loBaseDa.Find<REHPPERIODO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.Anio == tiAnio
                    select new Periodo
                    {
                        IdPeriodo = a.IdPeriodo,
                        Anio = a.Anio,
                        CodigoTipoRol = a.CodigoTipoRol,
                        Codigo = a.Codigo,
                        FechaInicio = a.FechaInicio,
                        FechaFin = a.FechaFin,
                        TipoRol = b.Descripcion,
                        FechaFinComi = a.FechaFinComi,
                        FechaInicioComi = a.FechaInicioComi,
                        FechaFinHorasExtras = a.FechaFinHorasExtras,
                        FechaInicioHorasExtras = a.FechaInicioHorasExtras
                    }).OrderBy(x => x.FechaFin).ToList();

        }

        /// <summary>
        /// Guardar Objeto parámetro
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public bool gbGuardar(List<Periodo> toLista, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            bool pbResult = false;
            foreach(var poItem in toLista)
            {
                int pId = poItem.IdPeriodo;
                var poObject = loBaseDa.Get<REHPPERIODO>().Where(x => x.IdPeriodo == pId).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Anio = poItem.Anio;
                    poObject.CodigoTipoRol = poItem.CodigoTipoRol;
                    poObject.Codigo = poItem.Codigo;
                    poObject.FechaInicio = poItem.FechaInicio;
                    poObject.FechaFin = poItem.FechaFin;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                    poObject.FechaInicioComi = poItem.FechaInicioComi;
                    poObject.FechaFinComi = poItem.FechaFinComi;
                    poObject.FechaInicioHorasExtras = poItem.FechaInicioHorasExtras;
                    poObject.FechaFinHorasExtras = poItem.FechaFinHorasExtras;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                    pbResult = true;
                }
                else
                {
                    poObject = new REHPPERIODO();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.Anio = poItem.Anio;
                    poObject.CodigoTipoRol = poItem.CodigoTipoRol;
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Codigo = poItem.Codigo;
                    poObject.FechaInicio = poItem.FechaInicio;
                    poObject.FechaFin = poItem.FechaFin;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.TerminalIngreso = tsTerminal;
                    poObject.FechaInicioComi = poItem.FechaInicioComi;
                    poObject.FechaFinComi = poItem.FechaFinComi;
                    poObject.FechaInicioHorasExtras = poItem.FechaInicioHorasExtras;
                    poObject.FechaFinHorasExtras = poItem.FechaFinHorasExtras;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                    pbResult = true;
                }
            }
            loBaseDa.SaveChanges();
            return pbResult;
        }
        #endregion
    }
}
