using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using GEN_Entidad;
using static GEN_Entidad.Diccionario;
using System.Transactions;
using GEN_Negocio;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 24/01/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>

    public class clsNTipoComision : clsNBase
    {
        #region Funciones
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public bool gbGuardar(TipoComision toObject)
        {
            loBaseDa.CreateContext();
            bool pbResult = false;
            string psCodigo = string.Empty;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
            var poObject = loBaseDa.Get<REHMTIPOCOMISION>().Where(x => x.CodigoTipoComision == psCodigo).FirstOrDefault();
            if (poObject != null )
            {
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;
                poObject.AplicaPorcentajeGrupal = toObject.AplicaPorcentajeGrupal;
                poObject.ValidaDiasTrabajados = toObject.ValidaDiasTrabajados;
                poObject.Porcentaje = toObject.Porcentaje;
                poObject.EditableCobranza = toObject.EditableCobranza;
                poObject.PorcentajeTotalMaximo = toObject.PorcentajeTotalMaximo;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }
            else
            {
                string psCodigoNew = loBaseDa.GeneraSecuencia("REHMTIPOCOMISION");
                if (string.IsNullOrEmpty(psCodigoNew)) throw new Exception(string.Format("No existe configuración de secuencia."));
                poObject = new REHMTIPOCOMISION();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoTipoComision = psCodigoNew;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.TerminalIngreso = toObject.Terminal;
                poObject.AplicaPorcentajeGrupal = toObject.AplicaPorcentajeGrupal;
                poObject.ValidaDiasTrabajados = toObject.ValidaDiasTrabajados;
                poObject.Porcentaje = toObject.Porcentaje;
                poObject.EditableCobranza = toObject.EditableCobranza;
                poObject.PorcentajeTotalMaximo = toObject.PorcentajeTotalMaximo;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject,Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }

            using (var poTran = new TransactionScope())
            {

                loBaseDa.SaveChanges();
                goActualizarPorcentajeComision(poObject.CodigoTipoComision);
                loBaseDa.SaveChanges();
                poTran.Complete();
            }
            return pbResult;
        }

        public int giCantContratosTipoComision(string tsCodigo)
        {
            int piReturn = 0;
            var poLista = loBaseDa.Get<REHPEMPLEADOCONTRATO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoTipoComision == tsCodigo).ToList();
            piReturn = poLista.Count;   
            return piReturn;
        }


        public int giCantRegistrosActualizar(string tsCodigo)
        {
            int piReturn = 0;
            decimal pdcComision = loBaseDa.Find<REHMTIPOCOMISION>().Where(x => x.CodigoTipoComision == tsCodigo && x.AplicaPorcentajeGrupal == true).Select(x => x.Porcentaje).FirstOrDefault() ?? 0M;
            if (pdcComision > 0)
            {
                var poLista = loBaseDa.Get<REHPEMPLEADOCONTRATO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoTipoComision == tsCodigo).ToList();

                piReturn = poLista.Count;
            }
            return piReturn;
        }

        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsDescripcion"></param>
        /// <returns></returns>
        public List<TipoComision> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<REHMTIPOCOMISION>().Where(x =>  x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new TipoComision
                   {
                       Codigo = x.CodigoTipoComision,
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Descripcion,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,
                       Porcentaje = x.Porcentaje??0M,
                       AplicaPorcentajeGrupal = x.AplicaPorcentajeGrupal??false,
                       ValidaDiasTrabajados = x.ValidaDiasTrabajados??false,
                       EditableCobranza = x.EditableCobranza ?? false,
                       PorcentajeTotalMaximo = x.PorcentajeTotalMaximo
                       
                   }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public TipoComision goBuscarMaestro(string tsCodigo)
        {
            return goConsultarTipoComision(tsCodigo);
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
                psCodigo = loBaseDa.Find<REHMTIPOCOMISION>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoComision }).OrderBy(x => x.CodigoTipoComision).FirstOrDefault()?.CodigoTipoComision;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<REHMTIPOCOMISION>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoComision }).OrderByDescending(x => x.CodigoTipoComision).FirstOrDefault()?.CodigoTipoComision;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<REHMTIPOCOMISION>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoComision }).ToList().Where(x => int.Parse(x.CodigoTipoComision) < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.CodigoTipoComision).FirstOrDefault().CodigoTipoComision;
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<REHMTIPOCOMISION>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoTipoComision }).ToList().Where(x => int.Parse(x.CodigoTipoComision) > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.CodigoTipoComision).FirstOrDefault().CodigoTipoComision;
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
            var poObject = loBaseDa.Get<REHMTIPOCOMISION>().Where(x => x.CodigoTipoComision == tsCodigo).FirstOrDefault();
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
