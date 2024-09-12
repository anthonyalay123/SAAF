using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Entidad.Entidades.SHEQ;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static GEN_Entidad.Diccionario.Tablas;

namespace REH_Negocio.Seguridad
{
    public class clsNMenuAccionPerfil : clsNBase
    {

        public string gsGuardar(MenuAccionPerfilBandeja toObject, string usuario)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;
            //psResult = lsEsValido(toObject);
            string psCodigo = string.Empty;

            int idMenu = toObject.IdMenu;
            int idAccion = toObject.IdAccion;
            int idPerfil = toObject.IdPerfil;

            var poListaMenuAccionPerfil = loBaseDa.Get<GENPMENUACCIONPERFIL>().Where(x => x.IdMenu == idMenu  && x.IdPerfil == idPerfil).ToList();
            var poListaMenuPerfil = loBaseDa.Get<GENPMENUPERFIL>().Where(x => x.IdPerfil == idPerfil).ToList();
            var poListaMenu = loBaseDa.Find<GENPMENU>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

            if (string.IsNullOrEmpty(psResult))
            {
                
                var poObject = poListaMenuAccionPerfil.Where(x => x.IdMenu == idMenu && x.IdAccion == idAccion && x.IdPerfil == idPerfil).FirstOrDefault();
                
                if (poObject != null)
                {  
                   // Inactivo botón del formulario y perfil
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = usuario;
                    poObject.TerminalModificacion = "";

                    if (toObject.CodigoEstado == Diccionario.Inactivo)
                    {
                        // Consulto cualtas acciones activas tiene el menú
                        var piAccionesActivas = poListaMenuAccionPerfil.Where(x => x.CodigoEstado == Diccionario.Activo && x.IdMenu == idMenu && x.IdPerfil == idPerfil).Count();

                        // Consulto datos del Menú, para obtener el IdMenuPadre
                        var poMenu = poListaMenu.Where(x => x.IdMenu == poObject.IdMenu).FirstOrDefault();

                        // Si las acciones activas con igual a 0 debo inactivar el menú en el perfil
                        if (piAccionesActivas == 0)
                        {
                            // Consulto el Menú Perfil donde fueron inactivados las acciones
                            var poMenuPerfil = poListaMenuPerfil.Where(x => x.CodigoEstado == Diccionario.Activo && x.IdMenu == poMenu.IdMenu && x.IdPerfil == idPerfil).FirstOrDefault();

                            // Inactivo Menú Perfil asociado a los botones
                            poMenuPerfil.CodigoEstado = toObject.CodigoEstado;
                            poMenuPerfil.UsuarioModificacion = usuario;
                            poMenuPerfil.FechaModificacion = DateTime.Now;
                            poMenuPerfil.TerminalModificacion = "";

                            while (poMenu != null)
                            {

                                // Consultar Ids menús tienen el mismo IdMenuPadre del menú que Inactivamos
                                var piMenusConElMismoMenuPadre = poListaMenu.Where(x => x.IdMenuPadre == poMenu.IdMenuPadre).Select(x => x.IdMenu).ToList();

                                // Consultamos Cuandos Menús existen parametrizados como activos en la trabla Menu Perfil, que tuvieron el mismo IdMenuPadre del menú que Inactivamos
                                var piExistenMenusConElMismoPadre = poListaMenuPerfil.Where(x => x.CodigoEstado == Diccionario.Activo && piMenusConElMismoMenuPadre.Contains(x.IdMenu)).Count();

                                // Si Existen menus con el mismo Padre no se Inactiva el Menú Perfil, Caso Contrario se Inactiva el Menu Perfil
                                if (piExistenMenusConElMismoPadre == 0)
                                {
                                    // Consulta el Menú Perfil que se va a Inactivar
                                    var poMenuPerfilPadre = poListaMenuPerfil.Where(x => x.CodigoEstado == Diccionario.Activo && x.IdMenu == poMenu.IdMenuPadre && x.IdPerfil == idPerfil).FirstOrDefault();

                                    //Inactivamos el Menu Perfil
                                    poMenuPerfilPadre.CodigoEstado = toObject.CodigoEstado;
                                    poMenuPerfilPadre.UsuarioModificacion = usuario;
                                    poMenuPerfilPadre.FechaModificacion = DateTime.Now;
                                    poMenuPerfilPadre.TerminalModificacion = "";
                                }
                                else
                                {
                                    break;
                                }

                                poMenu = poListaMenu.Where(x => x.IdMenu == poMenu.IdMenuPadre).FirstOrDefault();

                            }
                        }
                    }
                    else
                    {
                        var poMenu = poListaMenu.Where(x => x.IdMenu == poObject.IdMenu).FirstOrDefault();
                        while (poMenu != null)
                        {

                            var poMenuPerfil = poListaMenuPerfil.Where(x => x.IdMenu == poMenu.IdMenu && x.IdPerfil == idPerfil).FirstOrDefault();
                            if (poMenuPerfil != null)
                            {
                                poMenuPerfil.CodigoEstado = toObject.CodigoEstado;
                                poMenuPerfil.UsuarioModificacion = usuario;
                                poMenuPerfil.FechaModificacion = DateTime.Now;
                                poMenuPerfil.TerminalModificacion = "";
                            }
                            else
                            {
                                poMenuPerfil = new GENPMENUPERFIL();
                                loBaseDa.CreateNewObject(out poMenuPerfil);
                                poMenuPerfil.IdMenu = poMenu.IdMenu;
                                poMenuPerfil.CodigoEstado = Diccionario.Activo;
                                poMenuPerfil.IdPerfil = idPerfil;
                                poMenuPerfil.UsuarioIngreso = usuario;
                                poMenuPerfil.FechaIngreso = DateTime.Now;
                                poMenuPerfil.TerminalIngreso = "";
                            }
                            poMenu = loBaseDa.Get<GENPMENU>().Where(x => x.IdMenu == poMenu.IdMenuPadre).FirstOrDefault();

                        }
                    }
                    
                    
                }
                else
                {
                   
                    poObject = new GENPMENUACCIONPERFIL();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.IdMenu = Int32.Parse(toObject.Menu);
                    poObject.IdPerfil = Int32.Parse(toObject.Perfil);
                    poObject.CodigoEstado = toObject.Estado;
                    poObject.IdAccion = Int32.Parse(toObject.NombreAccion);


                    poObject.UsuarioIngreso = usuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = "";
                    var poMenu = loBaseDa.Find<GENPMENU>().Where(x => x.IdMenu == poObject.IdMenu).FirstOrDefault();
                    while (poMenu != null)
                    {
                        
                        var poMenuPerfil = loBaseDa.Find<GENPMENUPERFIL>().Where(x => x.IdMenu == poMenu.IdMenu && x.IdPerfil == idPerfil).FirstOrDefault();
                        if (poMenuPerfil != null)
                        {
                            poMenuPerfil.CodigoEstado = Diccionario.Activo;
                            poMenuPerfil.UsuarioModificacion = usuario;
                            poMenuPerfil.FechaModificacion = DateTime.Now;
                            poMenuPerfil.TerminalModificacion = "";
                        }
                        else
                        {
                            poMenuPerfil = new GENPMENUPERFIL();
                            loBaseDa.CreateNewObject(out poMenuPerfil);
                            poMenuPerfil.IdMenu = poMenu.IdMenu;
                            poMenuPerfil.CodigoEstado = Diccionario.Activo;
                            poMenuPerfil.IdPerfil = idPerfil;
                            poMenuPerfil.UsuarioIngreso = usuario;
                            poMenuPerfil.FechaIngreso = DateTime.Now;
                            poMenuPerfil.TerminalIngreso = "";
                        }
                        poMenu = loBaseDa.Get<GENPMENU>().Where(x => x.IdMenu == poMenu.IdMenuPadre).FirstOrDefault();

                    }
                }

                loBaseDa.SaveChanges();

            }

            return psResult;
        }

        public List<Combo> goConsultarComboMenu()
        {
            return loBaseDa.Find<GENPMENU>().Where(x => x.CodigoEstado.ToString() == Diccionario.Activo && x.NombreForma != null)
                   .Select(x => new Combo
                   {
                       Codigo = x.IdMenu.ToString(),
                       Descripcion = x.Nombre.ToString(),
                   }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboAccion()
        {
            return loBaseDa.Find<GENPACCION>().Where(x => x.CodigoEstado.ToString() == Diccionario.Activo )
                   .Select(x => new Combo
                   {
                       Codigo = x.IdAccion.ToString(),
                       Descripcion = x.Nombre.ToString(),
                   }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<Combo> goConsultarComboPerfil()
        {
            return loBaseDa.Find<GENPPERFIL>().Where(x => x.CodigoEstado.ToString() == Diccionario.Activo)
                   .Select(x => new Combo
                   {
                       Codigo = x.IdPerfil.ToString(),
                       Descripcion = x.Nombre.ToString(),
                   }).OrderBy(x => x.Descripcion).ToList();
        }

        public List<MenuAccionPerfilBandeja> goListarBandeja()
        {

            return (from SC in loBaseDa.Find<GENPMENUACCIONPERFIL>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado }
                    equals new { E.CodigoEstado }


                    // Inner Join con la tabla GENPMENU - Nombre Completo
                    join CMENU in loBaseDa.Find<GENPMENU>()
                    on new { cg = SC.IdMenu }
                    equals new { cg = CMENU.IdMenu }

                    // Inner Join con la tabla GENPPERFIL - Nombre Completo
                    join CPERFIL in loBaseDa.Find<GENPPERFIL>()
                    on new { cg = SC.IdPerfil }
                    equals new { cg = CPERFIL.IdPerfil }

                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                    join CACCION in loBaseDa.Find<GENPACCION>()
                    on new { cg = SC.IdAccion }
                    equals new { cg = CACCION.IdAccion }

                    // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Inactivo
                    // Selección de las columnas / Datos
                    select new MenuAccionPerfilBandeja
                    {
                        IdMenu = CMENU.IdMenu,
                        Menu = CMENU.Nombre,
                        IdAccion = CACCION.IdAccion,
                        NombreAccion = CACCION.Nombre,
                        IdPerfil = CPERFIL.IdPerfil,
                        Perfil = CPERFIL.Nombre,
                        CodigoEstado = E.CodigoEstado,
                        Estado = E.Descripcion,
                    }).ToList();
        }

        public string gsGuardarMenusAccionParaPerfil(int idPerfil, List<int> idMenus, string tsUsuario)
        {
            loBaseDa.CreateContext();

            string psMsg = string.Empty;

            psMsg = lsValidarDatos(idPerfil, idMenus);

            if (string.IsNullOrEmpty(psMsg))
            {
                //todos los menus que vienen de pantalla
                var menuItems = new List<MenuTodo>();
                foreach (var idMenu in idMenus)
                {
                    var menuItem = goConsultarMenuAccion().FirstOrDefault(m => m.IdMenu == idMenu);
                    if (menuItem != null)
                    {
                        menuItems.Add(menuItem);
                    }
                }

                //todos los menus que tiene ese perfil
                List<MenuTodo> listaMenuActuales = goConsultarMenuPorPerfilTodo(idPerfil);

                //ver todos los menus que no tiene el perfil pero vienen de pantalla
                var menusParaAgregar = menuItems.Where(c => !listaMenuActuales.Any(x => x.IdMenu == c.IdMenu)).ToList();

                //ver todos los menus que tiene mi perfil y no vienen por pantalla
                var menusParaEliminar = listaMenuActuales.Where(c => !menuItems.Any(x => x.IdMenu == c.IdMenu)).ToList();

                //separar los menus con las acciones para agregar
                var accionesAgregar = menusParaAgregar.Where(m => m.IdAccion != null).ToList();
                var menusAgregar = menusParaAgregar.Where(m => m.IdAccion == null).ToList();

                //separar los menus con las acciones para eliminar
                var accionesEliminar = menusParaEliminar.Where(m => m.IdAccion != null).ToList();
                var menusEliminar = menusParaEliminar.Where(m => m.IdAccion == null).ToList();

                if (menusEliminar.Count != 0)
                {
                    foreach (var poItem in menusEliminar)
                    {
                        var poListaMenuPerfil = loBaseDa.Get<GENPMENUPERFIL>().Where(x => x.IdPerfil == idPerfil && x.IdMenu == poItem.IdMenu).FirstOrDefault();

                        poListaMenuPerfil.CodigoEstado = Diccionario.Inactivo;
                        poListaMenuPerfil.UsuarioModificacion = tsUsuario;
                        poListaMenuPerfil.FechaModificacion = DateTime.Now;
                        poListaMenuPerfil.TerminalModificacion = "";
                    }
                
                }

                if (accionesEliminar.Count != 0)
                {
                    foreach (var poItem in accionesEliminar)
                    {
                        var poListaMenuAccionPerfil = loBaseDa.Get<GENPMENUACCIONPERFIL>().Where(x => x.IdMenu == poItem.IdMenuPadre && x.IdPerfil == idPerfil && x.IdAccion == poItem.IdAccion).FirstOrDefault();

                        poListaMenuAccionPerfil.CodigoEstado = Diccionario.Inactivo;
                        poListaMenuAccionPerfil.UsuarioModificacion = tsUsuario;
                        poListaMenuAccionPerfil.FechaModificacion = DateTime.Now;
                        poListaMenuAccionPerfil.TerminalModificacion = "";
                    }

                }

                if (menusAgregar.Count != 0)
                {
                    foreach (var poItem in menusAgregar)
                    {
                        var poListaMenuPerfil = loBaseDa.Get<GENPMENUPERFIL>().Where(x => x.IdPerfil == idPerfil && x.IdMenu == poItem.IdMenu).FirstOrDefault();

                        if(poListaMenuPerfil != null)
                        {
                            poListaMenuPerfil.CodigoEstado = Diccionario.Activo;
                            poListaMenuPerfil.UsuarioModificacion = tsUsuario;
                            poListaMenuPerfil.FechaModificacion = DateTime.Now;
                            poListaMenuPerfil.TerminalModificacion = "";
                        } else
                        {
                            var poMenuPerfil = new GENPMENUPERFIL();
                            loBaseDa.CreateNewObject(out poMenuPerfil);
                            poMenuPerfil.IdMenu = poItem.IdMenu;
                            poMenuPerfil.IdPerfil = idPerfil;
                            poMenuPerfil.CodigoEstado = Diccionario.Activo;
                            poMenuPerfil.UsuarioIngreso = tsUsuario;
                            poMenuPerfil.FechaIngreso = DateTime.Now;
                            poMenuPerfil.TerminalIngreso = "";
                        }

                    }

                }

                if (accionesAgregar.Count != 0)
                {
                    foreach (var poItem in accionesAgregar)
                    {
                        var poListaMenuAccionPerfil = loBaseDa.Get<GENPMENUACCIONPERFIL>().Where(x => x.IdMenu == poItem.IdMenuPadre && x.IdPerfil == idPerfil && x.IdAccion == poItem.IdAccion).FirstOrDefault();
                        if (poListaMenuAccionPerfil != null)
                        {
                            poListaMenuAccionPerfil.CodigoEstado = Diccionario.Activo;
                            poListaMenuAccionPerfil.UsuarioModificacion = tsUsuario;
                            poListaMenuAccionPerfil.FechaModificacion = DateTime.Now;
                            poListaMenuAccionPerfil.TerminalModificacion = "";
                        }
                        else
                        {
                            var poMenuAccionPerfil = new GENPMENUACCIONPERFIL();
                            loBaseDa.CreateNewObject(out poMenuAccionPerfil);
                            poMenuAccionPerfil.IdMenu = poItem.IdMenuPadre ?? 0;
                            poMenuAccionPerfil.IdAccion = poItem.IdAccion ?? 0;
                            poMenuAccionPerfil.IdPerfil = idPerfil;
                            poMenuAccionPerfil.CodigoEstado = Diccionario.Activo;
                            poMenuAccionPerfil.UsuarioIngreso = tsUsuario;
                            poMenuAccionPerfil.FechaIngreso = DateTime.Now;
                            poMenuAccionPerfil.TerminalIngreso = "";
                        }
                    }

                }

                loBaseDa.SaveChanges();
            }

            return psMsg;
        }

        private string lsValidarDatos(int idPerfil, List<int> idMenus)
        {
            string psMsg = "";

            if (idPerfil <= 0)
            {
                psMsg = string.Format("{0}Seleccione un Perfil. \n", psMsg);
            }

            if (idMenus.Count == 0)
            {
                psMsg = string.Format("{0}Seleccione una Bodega. \n", psMsg);
            }

            return psMsg;
        }
        public List<MenuTodo> goConsultarMenuAccion()
        {
            loBaseDa.CreateContext();

            var dtCabVac = goConsultaDataSet("EXEC GENSPMENUSYACCIONES");

            var lista = new List<MenuTodo>();

            foreach (DataRow row in dtCabVac.Tables[0].Rows)
            {
                var menuTodo = new MenuTodo
                {
                    IdMenu = row.IsNull("IdMenu") ? 0 : row.Field<int>("IdMenu"), // O usar un valor predeterminado
                    IdAccion = row.IsNull("IdAccion") ? (int?)null : row.Field<int?>("IdAccion"),
                    IdMenuPadre = row.IsNull("IdMenuPadre") ? (int?)null : row.Field<int?>("IdMenuPadre"),
                    Nombre = row.IsNull("Nombre") ? string.Empty : row.Field<string>("Nombre") // O usar un valor predeterminado
                };

                lista.Add(menuTodo);
            }

            lista = lista.OrderBy(x => x.IdMenu).ToList();

            return lista;
        }

        public List<MenuTodo> goConsultarMenuPorPerfilTodo(int idPerfil)
        {
            loBaseDa.CreateContext();

            var dtCabVac = goConsultaDataSet(string.Format("EXEC GENSPOBTENERMENUSUSUARIOS '{0}'", idPerfil));

            List<MenuTodo> lista = new List<MenuTodo>();

            foreach (DataRow row in dtCabVac.Tables[0].Rows)
            {
                if (row["IdMenu"] != DBNull.Value)
                {
                    lista.Add(new MenuTodo
                    {
                        IdMenu = row.IsNull("IdMenu") ? 0 : row.Field<int>("IdMenu"), 
                        IdAccion = row.IsNull("IdAccion") ? (int?)null : row.Field<int?>("IdAccion"),
                        IdMenuPadre = row.IsNull("IdMenuPadre") ? (int?)null : row.Field<int?>("IdMenuPadre"),
                        Nombre = row.IsNull("Nombre") ? string.Empty : row.Field<string>("Nombre")
                    });
                }
            }

            return lista;
        }

        public List<int> goConsultarMenuPorPerfil(int idPerfil)
        {
            loBaseDa.CreateContext();

            //var dtCabVac = goConsultaDataSet("EXEC GENSPOBTENERMENUSUSUARIOS '{0}'", idPerfil);
            var dtCabVac = goConsultaDataSet(string.Format("EXEC GENSPOBTENERMENUSUSUARIOS '{0}'", idPerfil));

            List<int> lista = new List<int>();

            foreach (DataRow row in dtCabVac.Tables[0].Rows)
            {
                if (row["IdMenu"] != DBNull.Value)
                {
                    lista.Add(Convert.ToInt32(row["IdMenu"]));
                }
            }

            return lista;
        }

    }
}
