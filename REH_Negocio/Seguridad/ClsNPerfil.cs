using GEN_Entidad;
using GEN_Entidad.Entidades;
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
    /// Fecha: 05/07/2021
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNPerfil : clsNBase
    {
        #region Funciones
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(Perfil toObject)
        {
            string psReturn = string.Empty;
            loBaseDa.CreateContext();
            bool pbResult = false;
            string psCodigo = string.Empty;
            psReturn = psValidar(toObject);
            if (string.IsNullOrEmpty(psReturn))
            {
                if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
                var poObject = loBaseDa.Get<GENPPERFIL>().Where(x => x.IdPerfil.ToString() == psCodigo).FirstOrDefault();
                if (poObject != null)
                {
                    // poObject.IdPerfil = toObject.IdPerfil ;
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Nombre = toObject.Descripcion;
                    poObject.FechaModificacion = toObject.Fecha;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;
                    //poObject.CodigoFlujoCompras = toObject.CodigoFlujoCompras;

                    List<int> poListaIdModificarProveedor = toObject.Flujos.Select(x => x.IdPerfilFlujo).ToList();
                    List<int> piListaIdEliminarProveedor = poObject.GENPPERFILFLUJO.Where(x => x.CodigoEstado != Diccionario.Eliminado && !poListaIdModificarProveedor.Contains(x.IdPerfilFlujo)).Select(x => x.IdPerfilFlujo).ToList();

                    foreach (var poProveedores in poObject.GENPPERFILFLUJO.Where(x => x.CodigoEstado != Diccionario.Inactivo && piListaIdEliminarProveedor.Contains(x.IdPerfilFlujo)))
                    {
                        poProveedores.CodigoEstado = Diccionario.Inactivo;
                        poProveedores.UsuarioModificacion = toObject.Usuario;
                        poProveedores.FechaModificacion = DateTime.Now;
                        poProveedores.TerminalModificacion = toObject.Terminal;
                    }
                    if (toObject.Flujos.Count > 0)
                    {
                        foreach (var item in toObject.Flujos)
                        {
                            var poObjectFlujo = loBaseDa.Get<GENPPERFILFLUJO>().Where(x => x.IdPerfilFlujo == item.IdPerfilFlujo && x.CodigoEstado != Diccionario.Inactivo).FirstOrDefault();
                            if (poObjectFlujo != null)
                            {
                                //Actualizar
                                lCargarPerfilFlujo(ref poObjectFlujo, item, DateTime.Now, toObject.Usuario, toObject.Terminal, true);
                            }
                            else
                            {   //Crear
                                poObjectFlujo = new GENPPERFILFLUJO();
                                lCargarPerfilFlujo(ref poObjectFlujo, item, DateTime.Now, toObject.Usuario, toObject.Terminal, false);
                                poObject.GENPPERFILFLUJO.Add(poObjectFlujo);
                            }
                        }

                    }

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                    pbResult = true;
                }
                else
                {

                    poObject = new GENPPERFIL();

                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Nombre = toObject.Descripcion;
                    poObject.CodigoFlujoCompras = toObject.CodigoFlujoCompras;
                    poObject.FechaIngreso = toObject.Fecha;
                    poObject.UsuarioIngreso = toObject.Usuario;
                    poObject.TerminalIngreso = toObject.Terminal;


                    if (toObject.Flujos.Count > 0)
                    {
                        foreach (var item in toObject.Flujos)
                        {
                            var poObjectFlujo = loBaseDa.Get<GENPPERFILFLUJO>().Where(x => x.IdPerfilFlujo == item.IdPerfilFlujo && x.CodigoEstado != Diccionario.Inactivo).FirstOrDefault();
                            if (poObjectFlujo != null)
                            {
                                //Actualizar
                                lCargarPerfilFlujo(ref poObjectFlujo, item, DateTime.Now, toObject.Usuario, toObject.Terminal, true);
                            }
                            else
                            {   //Crear
                                poObjectFlujo = new GENPPERFILFLUJO();
                                lCargarPerfilFlujo(ref poObjectFlujo, item, DateTime.Now, toObject.Usuario, toObject.Terminal, false);
                                poObject.GENPPERFILFLUJO.Add(poObjectFlujo);
                            }
                        }

                    }

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                    pbResult = true;
                    loBaseDa.SaveChanges();

                }

                loBaseDa.SaveChanges();
            }
            

            return psReturn;
        }

        private string psValidar(Perfil toObject)
        {
            string psReturn = string.Empty;
            foreach (var flujo in toObject.Flujos)
            {
                if (flujo.CodigoFlujo == Diccionario.Seleccione)
                {
                    psReturn = "Falta seleccionar el flujo";
                }

            }
            return psReturn;
        }
        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsDescripcion"></param>
        /// <returns></returns>
        public List<Perfil> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<GENPPERFIL>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Perfil
                   {
                       Codigo = x.IdPerfil.ToString(),
                       IdPerfil = x.IdPerfil,
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Nombre,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,
                      CodigoFlujoCompras = x.CodigoFlujoCompras,
                   }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Perfil goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<GENPPERFIL>().Where(x => x.IdPerfil.ToString() == tsCodigo)
                .Select(x => new Perfil
                {
                    IdPerfil = x.IdPerfil,
                  Codigo = x.IdPerfil.ToString(),
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Nombre,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    CodigoFlujoCompras = x.CodigoFlujoCompras

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
                psCodigo = loBaseDa.Find<GENPPERFIL>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdPerfil }).OrderBy(x => x.IdPerfil).FirstOrDefault()?.IdPerfil.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<GENPPERFIL>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdPerfil }).OrderByDescending(x => x.IdPerfil).FirstOrDefault()?.IdPerfil.ToString();
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<GENPPERFIL>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdPerfil }).ToList().Where(x => x.IdPerfil < int.Parse(tsCodigo)).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.IdPerfil).FirstOrDefault().IdPerfil.ToString();
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<GENPPERFIL>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.IdPerfil }).ToList().Where(x => x.IdPerfil > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.IdPerfil).FirstOrDefault().IdPerfil.ToString();
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
            var poObject = loBaseDa.Get<GENPPERFIL>().Where(x => x.IdPerfil.ToString() == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.SaveChanges();
            }
        }

        public List<Combo> goConsultarComboMenuPadre()
        {
            loBaseDa.CreateContext();
            return loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoGrupo =="030")
                 .Select(x => new Combo
                 {
                     Codigo = x.Codigo.ToString(),
                     Descripcion = x.Descripcion.ToString(),
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        private void lCargarPerfilFlujo(ref GENPPERFILFLUJO toEntidadBd, PerfilFlujo toEntidadData, DateTime tdFecha, string tsUsuario, string tsTerminal, bool tbActualiza = false)
        {


            toEntidadBd.CodigoEstado = Diccionario.Activo;
            toEntidadBd.IdPerfil = toEntidadData.IdPerfil;
            toEntidadBd.CodigoFlujo = toEntidadData.CodigoFlujo;
       
            if (tbActualiza)
            {
                toEntidadBd.UsuarioModificacion = tsUsuario;
                toEntidadBd.FechaModificacion = tdFecha;
                toEntidadBd.TerminalModificacion = tsTerminal;
            }
            else
            {
                toEntidadBd.UsuarioIngreso = tsUsuario;
                toEntidadBd.FechaIngreso = tdFecha;
                toEntidadBd.TerminalIngreso = tsTerminal;
            }
        }

        public List<PerfilFlujo> goBuscarFlujo(int tiIdPerfil)
        {
            return loBaseDa.Find<GENPPERFILFLUJO>().Where(x => x.CodigoEstado.ToString() == Diccionario.Activo && x.IdPerfil == tiIdPerfil)
                .Select(x => new PerfilFlujo
                {
                    IdPerfil = x.IdPerfil,
                    IdPerfilFlujo = x.IdPerfilFlujo,
                    CodigoFlujo = x.CodigoFlujo,
                     
                }).ToList();
        }

        #endregion
    }
}
