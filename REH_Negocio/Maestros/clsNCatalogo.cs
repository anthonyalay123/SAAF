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
    /// Fecha: 05/05/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNCatalogo : clsNBase
    {
        #region Funciones
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public bool gbGuardar(Catalogo toObject)
        {
            loBaseDa.CreateContext();
            bool pbResult = false;
            string psCodigo = string.Empty;
            string psCodigoGrupo = string.Empty;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
            if (!string.IsNullOrEmpty(toObject.CodigoGrupo)) psCodigoGrupo = toObject.CodigoGrupo;
            var poObject = loBaseDa.Get<GENMCATALOGO>().Where(x => x.Codigo == psCodigo && x.CodigoGrupo == psCodigoGrupo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.Codigo = toObject.Codigo;
                poObject.CodigoGrupo = toObject.CodigoGrupo;
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
                poObject = new GENMCATALOGO();
                loBaseDa.CreateNewObject(out poObject);
                poObject.Codigo = toObject.Codigo;
                poObject.CodigoGrupo = toObject.CodigoGrupo;
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Descripcion = toObject.Descripcion;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.TerminalIngreso = toObject.Terminal;
                

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }

            if (toObject.CodigoGrupo == Diccionario.ListaCatalogo.TipoPrestamoAnticipoDescuento)
            {
                poObject.CodigoAlterno1 = toObject.CodigoAlterno1;
            }

            loBaseDa.SaveChanges();
            return pbResult;
        }
        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsCodigoGrupo"></param>
        /// <returns></returns>
        public List<Catalogo> goListarMaestro(string tsCodigoGrupo = "")
        {
            return (from a in loBaseDa.Find<GENMCATALOGO>()
                    join b in loBaseDa.Find<GENMCATALOGO>() on new { a.Codigo, a.CodigoGrupo } equals new { Codigo = b.CodigoGrupo, CodigoGrupo = Diccionario.ListaCatalogo.GrupoCatalogo }
                    where b.CodigoEstado != Diccionario.Eliminado && (!string.IsNullOrEmpty(tsCodigoGrupo) ? tsCodigoGrupo == a.Codigo : "" == "")
                    select new Catalogo
                    {
                        CodigoGrupo = a.Codigo,
                        DescripcionGrupo = a.Descripcion,
                        Codigo = b.Codigo,
                        CodigoEstado = b.CodigoEstado,
                        Descripcion = b.Descripcion,
                        Fecha = b.FechaIngreso,
                        Usuario = b.UsuarioIngreso,
                        Terminal = b.TerminalIngreso,
                        FechaMod = b.FechaModificacion,
                        UsuarioMod = b.UsuarioModificacion,
                        TerminalMod = b.TerminalModificacion,
                        CodigoAlterno1 = b.CodigoAlterno1
                    }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Catalogo goBuscarMaestro(string tsCodigoGrupo, string tsCodigo)
        {
            return loBaseDa.Find<GENMCATALOGO>().Where(x => x.Codigo == tsCodigo && x.CodigoGrupo == tsCodigoGrupo)
                .Select(x => new Catalogo
                {
                    Codigo = x.Codigo,
                    CodigoGrupo = x.CodigoGrupo,
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    CodigoAlterno1 = x.CodigoAlterno1
                }).FirstOrDefault();
        }
        
        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestro(string tsCodigoGrupo, string tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<GENMCATALOGO>().Where(x => x.CodigoGrupo == tsCodigoGrupo && x.Codigo == tsCodigo).FirstOrDefault();
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
