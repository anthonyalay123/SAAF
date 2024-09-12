using GEN_Entidad;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GEN_Entidad.Diccionario;

namespace REH_Negocio.Parametrizadores
{
    /// <summary>
    /// Autor: Guillermo Murillo
    /// Fecha: 1/07/2021
    /// Clase Unidad de trabajo o Logica de Negocio
    /// </summary>
    public class clsNTipoPermiso : clsNBase
    {
        #region Funciones
        /// <summary>
        /// Guardar objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public bool gbGuardar(TipoPermiso toObject)
        {
            loBaseDa.CreateContext();
            bool pbResult = false;
            string psCodigo = string.Empty;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
            var poObject = loBaseDa.Get<REHPTIPOPERMISO>().Where(x => x.CodigoTipoPermiso == psCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoTipoPermiso = toObject.Codigo;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;
                poObject.DiasMaximoCobertura = toObject.DiasMaximoCobertura;
                poObject.AfectaDiasLaborables = toObject.AfectaDiasLaborales;
                poObject.DiasCoberturaPorcentaje = toObject.DiasCoberturaPorcentaje;
                poObject.PorcentajeCobertura = toObject.PorcentajeCobertura;
                poObject.MinutosDescontar = toObject.MinutosDescontar;
                poObject.DescuentaHaberes = toObject.AplicaDescuentoHaberes;
                poObject.AplicaLicencias = toObject.AplicaLicencias;
                poObject.AplicaPermisosPorHoras = toObject.AplicaPermisosPorHoras;


                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }
            else
            {
                string psCodigoNew = loBaseDa.GeneraSecuencia("REHPTIPOPERMISO");
                if (string.IsNullOrEmpty(psCodigoNew)) throw new Exception(string.Format("No existe configuración de secuencia."));
                poObject = new REHPTIPOPERMISO();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoTipoPermiso = psCodigoNew;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.DiasCoberturaPorcentaje = toObject.DiasCoberturaPorcentaje;
                poObject.DiasMaximoCobertura = toObject.DiasMaximoCobertura;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.TerminalIngreso = toObject.Terminal;
                poObject.PorcentajeCobertura = toObject.PorcentajeCobertura;
                poObject.MinutosDescontar = toObject.MinutosDescontar;
                poObject.DescuentaHaberes = toObject.AplicaDescuentoHaberes;
                poObject.AplicaLicencias = toObject.AplicaLicencias;
                poObject.AplicaPermisosPorHoras = toObject.AplicaPermisosPorHoras;


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
        public List<TipoPermiso> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<REHPTIPOPERMISO>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new TipoPermiso
                   {
                       Codigo = x.CodigoTipoPermiso,
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Descripcion,
                       DiasMaximoCobertura = x.DiasMaximoCobertura??0,
                       AfectaDiasLaborales = x.AfectaDiasLaborables,
                       DiasCoberturaPorcentaje = x.DiasCoberturaPorcentaje??0,
                       PorcentajeCobertura = x.PorcentajeCobertura??0,
                       MinutosDescontar = x.MinutosDescontar ?? 0,
                       AplicaDescuentoHaberes = x.DescuentaHaberes ?? false,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,
                       AplicaLicencias = x.AplicaLicencias?? false,
                       AplicaPermisosPorHoras = x.AplicaPermisosPorHoras ?? false,
        }).ToList();
        }

        /// <summary>
        /// Buscar entidad Maestro
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public TipoPermiso goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<REHPTIPOPERMISO>().Where(x => x.CodigoTipoPermiso == tsCodigo)
                .Select(x => new TipoPermiso
                {
                    Codigo = x.CodigoTipoPermiso,

                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    DiasMaximoCobertura = x.DiasMaximoCobertura??0,
                    AfectaDiasLaborales = x.AfectaDiasLaborables,
                    DiasCoberturaPorcentaje = x.DiasCoberturaPorcentaje??0,
                    PorcentajeCobertura = x.PorcentajeCobertura??0,
                    MinutosDescontar = x.MinutosDescontar ?? 0,
                    AplicaDescuentoHaberes = x.DescuentaHaberes ?? false,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    AplicaLicencias = x.AplicaLicencias ?? false,
                    AplicaPermisosPorHoras = x.AplicaPermisosPorHoras ?? false,

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
                psCodigo = loBaseDa.Find<REHPTIPOPERMISO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoPermiso }).OrderBy(x => x.CodigoTipoPermiso).FirstOrDefault()?.CodigoTipoPermiso;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<REHPTIPOPERMISO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoPermiso }).OrderByDescending(x => x.CodigoTipoPermiso).FirstOrDefault()?.CodigoTipoPermiso;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<REHPTIPOPERMISO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoPermiso }).ToList().Where(x => int.Parse(x.CodigoTipoPermiso) < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.CodigoTipoPermiso).FirstOrDefault().CodigoTipoPermiso;
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<REHPTIPOPERMISO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoPermiso }).ToList().Where(x => int.Parse(x.CodigoTipoPermiso) > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.CodigoTipoPermiso).FirstOrDefault().CodigoTipoPermiso;
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
            var poObject = loBaseDa.Get<REHPTIPOPERMISO>().Where(x => x.CodigoTipoPermiso == tsCodigo).FirstOrDefault();
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
   
