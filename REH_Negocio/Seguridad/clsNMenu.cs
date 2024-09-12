using GEN_Entidad;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REH_Negocio
{
    public class clsNMenu : clsNBase
    {
        #region Funciones  
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public bool gbGuardar(Menu toObject)
        {
            loBaseDa.CreateContext();
            bool pbResult = false;
            string psCodigo = string.Empty;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
            List<string> psCodigoEstadoExlusion = new List<string>();
            psCodigoEstadoExlusion.Add(Diccionario.Eliminado);
            psCodigoEstadoExlusion.Add(Diccionario.Inactivo);
            var poObject = loBaseDa.Get<GENPMENU>().Where(x => x.IdMenu.ToString() == psCodigo).FirstOrDefault();
            var poObjectMenuPerfilPrincipal = loBaseDa.Get<GENPMENUPERFIL>().Where(x => !psCodigoEstadoExlusion.Contains(x.CodigoEstado)).ToList();
            if (poObject != null)
            {
                poObject.IdMenu = Convert.ToInt32(toObject.Codigo);
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Nombre = toObject.Descripcion;
                poObject.IdMenuPadre = toObject.IdMenuPadre;
                poObject.NombreForma = toObject.NombreForma;
                poObject.NombreMenuFormulario = toObject.NombreFormulario;
                poObject.Orden = toObject.Orden;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;
                poObject.IdGestorConsulta = toObject.IdGestorConsulta;

              //MenuPErfil que se va a duplicar
                var poObjectMenuPerfil1 = poObjectMenuPerfilPrincipal.Where(x => x.IdMenu.ToString() == toObject.Codigo).ToList();
                if (poObjectMenuPerfil1.Count > 0)
                {

                    var IdMenuPrincipal = poObject.IdMenuPadre??0;
                    while (IdMenuPrincipal != 0)
                    {
                        
                        foreach (var MP1 in poObjectMenuPerfil1)
                        {
                            //Buscar si en el menu Padre ya existe el menuPerfil 
                            var poObjectMenuPerfil2 = poObjectMenuPerfilPrincipal.Where(x => x.IdMenu == IdMenuPrincipal && x.IdPerfil == MP1.IdPerfil).FirstOrDefault();

                            if (poObjectMenuPerfil2!= null)
                            {
                                //ModMenuPerfil(ref poObjectMenuPerfil2, MP1, toObject.Usuario, toObject.Terminal, IdMenuPrincipal, true);
                            }
                            else
                            {
                                poObjectMenuPerfil2 = new GENPMENUPERFIL();
                                loBaseDa.CreateNewObject(out poObjectMenuPerfil2);
                                ModMenuPerfil(ref poObjectMenuPerfil2, MP1, toObject.Usuario, toObject.Terminal, IdMenuPrincipal, false);
                                poObjectMenuPerfilPrincipal.Add(poObjectMenuPerfil2);
                            }   
                        }

                        var poObjectval = loBaseDa.Find<GENPMENU>().Where(x => x.IdMenu == IdMenuPrincipal).Select(x=>x.IdMenuPadre).FirstOrDefault();
                        IdMenuPrincipal = poObjectval ?? 0;
                       
                    }
                }

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }
            else
            {
                
                poObject = new GENPMENU();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Nombre = toObject.Descripcion;
                poObject.IdMenuPadre = toObject.IdMenuPadre;
                poObject.NombreForma = toObject.NombreForma;
                poObject.NombreMenuFormulario = toObject.NombreFormulario;
                poObject.Orden = toObject.Orden;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.TerminalIngreso = toObject.Terminal;
                poObject.IdGestorConsulta = toObject.IdGestorConsulta;


                //MenuPErfil que se va a duplicar
                var poObjectMenuPerfil1 = poObjectMenuPerfilPrincipal.Where(x => x.IdMenu.ToString() == toObject.Codigo).ToList();
                if (poObjectMenuPerfil1.Count > 0)
                {

                    var IdMenuPrincipal = poObject.IdMenuPadre ?? 0;
                    while (IdMenuPrincipal != 0)
                    {

                        foreach (var MP1 in poObjectMenuPerfil1)
                        {
                            //Buscar si en el menu Padre ya existe el menuPerfil 
                            var poObjectMenuPerfil2 = poObjectMenuPerfilPrincipal.Where(x => x.IdMenu == IdMenuPrincipal && x.IdPerfil == MP1.IdPerfil).FirstOrDefault();

                            if (poObjectMenuPerfil2 != null)
                            {
                                //ModMenuPerfil(ref poObjectMenuPerfil2, MP1, toObject.Usuario, toObject.Terminal, IdMenuPrincipal, true);
                            }
                            else
                            {
                                poObjectMenuPerfil2 = new GENPMENUPERFIL();
                                loBaseDa.CreateNewObject(out poObjectMenuPerfil2);
                                ModMenuPerfil(ref poObjectMenuPerfil2, MP1, toObject.Usuario, toObject.Terminal, IdMenuPrincipal, false);
                                poObjectMenuPerfilPrincipal.Add(poObjectMenuPerfil2);
                            }

                        }

                        var poObjectval = loBaseDa.Find<GENPMENU>().Where(x => x.IdMenu == IdMenuPrincipal).Select(x => x.IdMenuPadre).FirstOrDefault();
                        IdMenuPrincipal = poObjectval ?? 0;

                    }
                }

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }

            loBaseDa.SaveChanges();
            return pbResult;
        }


        public void ModMenuPerfil(ref GENPMENUPERFIL MenuPerfil1, GENPMENUPERFIL MenuPerfil2, string tsUsuario, string tsTerminal , int tiMenu, bool actualiza = false)
        {

            MenuPerfil1.IdMenu = tiMenu;
            MenuPerfil1.IdPerfil = MenuPerfil2.IdPerfil;
            MenuPerfil1.CodigoEstado = Diccionario.Activo;

            if (actualiza)
            {
                MenuPerfil1.UsuarioModificacion = tsUsuario;
                MenuPerfil1.FechaModificacion = DateTime.Now;
                MenuPerfil1.TerminalModificacion = tsTerminal;
            }
            else
            {
                MenuPerfil1.UsuarioIngreso = tsUsuario;
                MenuPerfil1.FechaIngreso = DateTime.Now;
                MenuPerfil1.TerminalIngreso = tsTerminal;
            }
           



        




        }
        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsDescripcion"></param>
        /// <returns></returns>
        public List<Menu> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<GENPMENU>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Menu
                   {
                       Codigo = x.IdMenu.ToString(),
                       NombreForma = x.NombreForma,
                       NombreFormulario = x.NombreMenuFormulario,
                       IdMenuPadre = x.IdMenuPadre ?? 0,
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Nombre,
                       Orden = x.Orden,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,

                   }).ToList();
        }

        /// <summary>
        /// Buscar Entidad Maestra
        /// </summary>
        /// <param name="tsCodigo"></param>
        /// <returns></returns>
        public Menu goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<GENPMENU>().Where(x => x.IdMenu.ToString() == tsCodigo)
                .Select(x => new Menu
                {
                    Codigo = x.IdMenu.ToString(),
                    NombreForma = x.NombreForma,
                    NombreFormulario = x.NombreMenuFormulario,
                    IdMenuPadre = x.IdMenuPadre??0,
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Nombre,
                    Orden = x.Orden,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    IdGestorConsulta = x.IdGestorConsulta??0,

                }).FirstOrDefault();
        }

        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestro(string tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<GENPMENU>().Where(x => x.IdMenu.ToString() == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.SaveChanges();
            }
        }


        /// <summary>
        /// Consulta los datos de la tabla GENPMENU para porsteriormente llenar el Combobox 
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboMenuPadre()
        {
            return loBaseDa.Find<GENPMENU>().Where(x => x.CodigoEstado.ToString() == Diccionario.Activo)
                 .Select(x => new Combo
                 {
                     Codigo = x.IdMenu.ToString(),
                     Descripcion = x.Nombre.ToString(),
                 }).OrderBy(x => x.Codigo).ToList();
        }

        public List<Combo> goConsultarComboGestorConsulta()
        {
            return loBaseDa.Find<REHMGESTORCONSULTA>().Where(x => x.CodigoEstado.ToString() == Diccionario.Activo)
                 .Select(x => new Combo
                 {
                     Codigo = x.Id.ToString(),
                     Descripcion = x.Nombre.ToString(),
                 }).OrderBy(x => x.Codigo).ToList();
        }
        #endregion
    }
}
