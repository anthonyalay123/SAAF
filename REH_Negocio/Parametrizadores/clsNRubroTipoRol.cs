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
    /// Fecha: 28/02/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNRubroTipoRol : clsNBase
    {

        /// <summary>
        /// Consulta de Datos del los Tipos de Roles con sus Rubros
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public List<RubroTipoRol> goConsultaRubroTipoRol(string tsCodigoTipoRol)
        {
            return (from a in loBaseDa.Find<REHPRUBROTIPOROL>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    join c in loBaseDa.Find<REHPRUBRO>() on a.CodigoRubro equals c.CodigoRubro
                    where a.CodigoTipoRol == tsCodigoTipoRol && a.CodigoEstado == Diccionario.Activo
                    select new RubroTipoRol
                    {
                        CodigoTipoRol = a.CodigoTipoRol,
                        DescripcionTipoRol = b.Descripcion,
                        CodigoRubro = a.CodigoRubro,
                        DescripcionRubro = c.Descripcion,
                        Orden = a.Orden ?? 0
                    }).OrderBy(x => x.DescripcionRubro).ToList();
        }

        /// <summary>
        /// Consulta de Datos de Rubros
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public List<RubroTipoRol> goConsultaRubros()
        {
            return (from a in loBaseDa.Find<REHPRUBRO>()
                    join b in loBaseDa.Find<REHMTIPORUBRO>() on a.CodigoTipoRubro equals b.CodigoTipoRubro
                    where a.CodigoEstado == Diccionario.Activo
                    select new RubroTipoRol
                    {
                        CodigoRubro = a.CodigoRubro,
                        DescripcionRubro = a.Descripcion,
                        CodigoTipoRubro = b.CodigoTipoRubro,
                        DescripcionTipoRubro = b.Descripcion,
                    }).OrderBy(x => x.DescripcionRubro).ToList();
        }

        /// <summary>
        /// Guardar Objeto parámetro
        /// </summary>
        /// <param name="toLista"></param>
        /// <param name="tsUsuario"></param>
        /// <param name="tsTerminal"></param>
        /// <returns></returns>
        public bool gbGuardar(List<RubroTipoRol> toLista, string tsCodigoTipoRol, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            List<string> psListaCodigoRubroDesactivo = toLista.Where(x=>x.Aplica == false).Select(x => x.CodigoRubro).Distinct().ToList();
            List<string> psListaCodigoRubroActivo = toLista.Where(x => x.Aplica == true).Select(x => x.CodigoRubro).Distinct().ToList();
            List<string> psListaCodigoRubroTabla = new List<string>();
            var poLista = loBaseDa.Get<REHPRUBROTIPOROL>().Where(x => tsCodigoTipoRol == x.CodigoTipoRol).ToList();
            //if (poLista.Count > 0)
            //{
            //    psListaCodigoRubroTabla = poLista.Where(x => !psListaCodigoRubroTabla.Contains(x.CodigoRubro) && x.CodigoEstado == Diccionario.Activo).Select(x => x.CodigoRubro).ToList();
            //}
            bool pbResult = false;
            foreach (var poItem in toLista)
            {
                string psCodigoRubro = poItem.CodigoRubro;
                var poObject = poLista.Where(x => x.CodigoRubro == psCodigoRubro && x.CodigoTipoRol == tsCodigoTipoRol).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.CodigoEstado = poItem.Aplica ? Diccionario.Activo : Diccionario.Inactivo;
                    poObject.CodigoRubro = poItem.CodigoRubro;
                    poObject.Orden = poItem.Orden;
                    poObject.CodigoTipoRol = tsCodigoTipoRol;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = tsUsuario;
                    poObject.TerminalModificacion = tsTerminal;
                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                    pbResult = true;
                }
                else
                {
                    poObject = new REHPRUBROTIPOROL();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = poItem.Aplica ? Diccionario.Activo : Diccionario.Inactivo;
                    poObject.CodigoRubro = poItem.CodigoRubro;
                    poObject.CodigoTipoRol = tsCodigoTipoRol;
                    poObject.Orden = poItem.Orden;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.TerminalIngreso = tsTerminal;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                    pbResult = true;
                }
            }

            //Inactivar Registros Eliminados
            //foreach(var poItem in poLista.Where(x=> !psListaCodigoRubro.Contains(x.CodigoRubro) && x.CodigoEstado == Diccionario.Activo).ToList())
            //{
            //    poItem.CodigoEstado = Diccionario.Inactivo;
            //    poItem.FechaModificacion = DateTime.Now;
            //    poItem.UsuarioModificacion = tsUsuario;
            //    poItem.TerminalModificacion = tsTerminal;
            //    // Insert Auditoría
            //    loBaseDa.Auditoria(poItem, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
            //}
            loBaseDa.SaveChanges();
            return pbResult;
        }

    }
}
