using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using GEN_Entidad;
using static GEN_Entidad.Diccionario;
using GEN_Negocio;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 26/02/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNRubro : clsNBase
    {
        #region Funciones
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public bool gbGuardar(Rubro toObject)
        {
            loBaseDa.CreateContext();
            bool pbResult = false;
            string psCodigo = string.Empty;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
            var poObject = loBaseDa.Get<REHPRUBRO>().Where(x => x.CodigoRubro == psCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoTipoRubro = toObject.CodigoTipoRubro;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;
                poObject.CodigoCategoriaRubro = toObject.CodigoCategoriaRubro;
                poObject.NovedadEditable = toObject.NovedadEditable;
                poObject.EsEntero = toObject.EsEntero;
                poObject.CodigoTipoContabilizacion = toObject.CodigoTipoContabilizacion;
                poObject.CodigoTipoMovimiento = toObject.CodigoTipoMovimiento;
                poObject.Iess = toObject.Aportable;
                poObject.Formula = toObject.Formula;
                poObject.Orden = toObject.Orden;
                poObject.ImpuestoRenta = toObject.ImpuestoRenta;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }
            else
            {
                string psCodigoNew = loBaseDa.GeneraSecuencia("REHPRUBRO");
                if (string.IsNullOrEmpty(psCodigoNew)) throw new Exception(string.Format("No existe configuración de secuencia."));
                poObject = new REHPRUBRO();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoTipoRubro = toObject.CodigoTipoRubro;
                poObject.CodigoRubro = psCodigoNew;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.CodigoCategoriaRubro = toObject.CodigoCategoriaRubro;
                poObject.Iess = toObject.Aportable;
                poObject.NovedadEditable = toObject.NovedadEditable;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.TerminalIngreso = toObject.Terminal;
                poObject.Formula = toObject.Formula;
                poObject.Orden = toObject.Orden;
                poObject.EsEntero = toObject.EsEntero;
                poObject.CodigoTipoContabilizacion = toObject.CodigoTipoContabilizacion;
                poObject.ImpuestoRenta = toObject.ImpuestoRenta;
                poObject.CodigoTipoMovimiento = toObject.CodigoTipoMovimiento;

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
        public List<Rubro> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Rubro
                   {
                       Codigo = x.CodigoRubro,
                       CodigoTipoRubro = x.CodigoTipoRubro,
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Descripcion,
                       Aportable = x.Iess ?? false,
                       Orden = x.Orden ?? 0,
                       Formula = x.Formula,
                       NovedadEditable = x.NovedadEditable ?? false,
                       CodigoCategoriaRubro = x.CodigoCategoriaRubro,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,
                       ImpuestoRenta = x.ImpuestoRenta ?? false,
                       CodigoTipoMovimiento = x.CodigoTipoMovimiento,
                       CodigoTipoContabilizacion = x.CodigoTipoContabilizacion
                   }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Rubro goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoRubro == tsCodigo)
                .Select(x => new Rubro
                {
                    Codigo = x.CodigoRubro,
                    CodigoTipoRubro = x.CodigoTipoRubro,
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    Aportable = x.Iess??false,
                    NovedadEditable = x.NovedadEditable ?? false,
                    CodigoCategoriaRubro = x.CodigoCategoriaRubro,
                    Orden = x.Orden ?? 0,
                    Formula = x.Formula,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    EsEntero = x.EsEntero??false,
                    ImpuestoRenta = x.ImpuestoRenta ?? false,
                    CodigoTipoContabilizacion = x.CodigoTipoContabilizacion,
                    CodigoTipoMovimiento = x.CodigoTipoMovimiento
                    
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
                psCodigo = loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoRubro }).OrderBy(x => x.CodigoRubro).FirstOrDefault()?.CodigoRubro;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoRubro }).OrderByDescending(x => x.CodigoRubro).FirstOrDefault()?.CodigoRubro;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoRubro }).ToList().Where(x => int.Parse(x.CodigoRubro) < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.CodigoRubro).FirstOrDefault().CodigoRubro;
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoRubro }).ToList().Where(x => int.Parse(x.CodigoRubro) > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.CodigoRubro).FirstOrDefault().CodigoRubro;
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
            var poObject = loBaseDa.Get<REHPRUBRO>().Where(x => x.CodigoRubro == tsCodigo).FirstOrDefault();
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
