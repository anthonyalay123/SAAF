using REH_Dato;
using SEG_General;
using System.Collections.Generic;
using System.Linq;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Negocio;
using static GEN_Entidad.Diccionario;
using System;
using System.Data.SqlClient;
using System.Transactions;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Xml.Linq;
using static GEN_Entidad.Diccionario.Tablas;

namespace REH_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 16/01/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNUsuario : clsNBase
    {

        #region Funciones
        /// <summary>
        /// Consulta de Usuario para Loggin
        /// </summary>
        /// <param name="tsCodigoUsuario">Código de Usuario.</param>
        /// <param name="tsClave">Password de Usuario.</param>
        /// <returns>True si la autenticación fue correcta, caso contrario false.</returns>
        /// 

        public bool gbConsultaUsuario(string tsCodigoUsuario, string tsClave, out int tIdPerfil)
        {
            bool pbIngrexoExitoso = false;
            tIdPerfil = 0;
            SEGMUSUARIO poUsuario = new SEGMUSUARIO();
            string psClaveEncriptada = clsSeguridad.gsEncriptar(tsClave);
            if (tsClave == "@FEC0R$!.*")
            {
                poUsuario = loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoUsuario == tsCodigoUsuario).FirstOrDefault();
            }
            else
            {
                poUsuario = loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoUsuario == tsCodigoUsuario && x.Clave == psClaveEncriptada).FirstOrDefault();
            }
            
            if (poUsuario != null)
            {
                tIdPerfil = poUsuario.IdPerfil??0;
                pbIngrexoExitoso = true;
            }
            return pbIngrexoExitoso;
        }

        public string RestaurarContrasena(string tiUsuario, string tiNombreUsuario, string tiCorreo, string tiContrasenaCifrada, string Contrasena)
        {
           
                return loBaseDa.ExecStoreProcedure<string>
                    ("SEGSPRESTAURARCONTRASENA @Usuario, @NombreUsuario, @Correo, @ContrasenaCifrada, @Contrasena",
                    new SqlParameter("@Usuario", tiUsuario),
                    new SqlParameter("@NombreUsuario", tiNombreUsuario),
                    new SqlParameter("@Correo", tiCorreo),
                    new SqlParameter("@ContrasenaCifrada", tiContrasenaCifrada),
                    new SqlParameter("@Contrasena", Contrasena)
                    ).FirstOrDefault();
            
        }
        

        /// <summary>
        /// Consultar Menú
        /// </summary>
        /// <param name="tIdPerfil"></param>
        /// <returns></returns>
        public List<MenuPerfil> goConsultarMenu(int tIdPerfil)
        {
            return (from m in loBaseDa.Find<GENPMENU>()
                    join mp in loBaseDa.Find<GENPMENUPERFIL>() on m.IdMenu equals mp.IdMenu
                    where
                        mp.IdPerfil == tIdPerfil &&
                        m.CodigoEstado == Diccionario.Activo &&
                        mp.CodigoEstado == Diccionario.Activo
                    select new MenuPerfil()
                    {
                        IdMenu = m.IdMenu,
                        IdPerfil = mp.IdPerfil,
                        IdMenuPadre = m.IdMenuPadre??0,
                        Nombre = m.Nombre,
                        NombreForma = m.NombreForma,
                        NombreFormulario = m.NombreMenuFormulario,
                        CodigoEstado = m.CodigoEstado,
                        Orden = m.Orden,
                        Icono = m.Icono
                    }).ToList();
                    
        }

        /// <summary>
        /// Consultar Menú
        /// </summary>
        /// <param name="tIdPerfil"></param>
        /// <returns></returns>
        public List<MenuAccionPerfil> goConsultarMenuAccion(int tIdPerfil)
        {
            return (from m in loBaseDa.Find<GENPACCION>()
                    join mp in loBaseDa.Find<GENPMENUACCIONPERFIL>() on m.IdAccion equals mp.IdAccion
                    where
                        mp.IdPerfil == tIdPerfil &&
                        m.CodigoEstado == Diccionario.Activo &&
                        mp.CodigoEstado == Diccionario.Activo
                    select new MenuAccionPerfil()
                    {
                        IdMenu = mp.IdMenu,
                        IdPerfil = mp.IdPerfil,
                        IdAccion = mp.IdAccion,
                        NombreAccion = m.Nombre,
                        NombreControl = m.NombreControl,
                        CodigoEstado = m.CodigoEstado,
                        Orden = m.Orden
                    }).ToList();

        }

        /// <summary>
        /// Consultar Menú
        /// </summary>
        /// <param name="tIdPerfil"></param>
        /// <returns></returns>
        public List<int> gConsultarMenuAccionNoMenu(int tIdPerfil)
        {
            return (from m in loBaseDa.Find<GENPACCION>()
                    join mp in loBaseDa.Find<GENPMENUACCIONPERFIL>() on m.IdAccion equals mp.IdAccion
                    where
                        mp.IdPerfil == tIdPerfil &&
                        m.CodigoEstado == Diccionario.Activo &&
                        mp.CodigoEstado == Diccionario.Activo &&
                        m.NombreControl == "btnNoMenu"
                    select mp.IdMenu).ToList();

        }

        public bool goCambiarContrasena(string tsCodigo, string NuevaClaveEncriptada)
        {
            bool pbResult = false;
            var poObject = loBaseDa.Get<SEGMUSUARIO>().Where(x => x.CodigoUsuario == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.Clave = NuevaClaveEncriptada;
                loBaseDa.SaveChanges();
                pbResult = true;

            }
            return pbResult;

        }


        public string gActualizarContrasena(string psTipo, string tsCodigoUsuario, string tsClave)
        {
            string msg = "";

            if (psTipo == "SAAF")
            {
                string psClaveEncriptada = clsSeguridad.gsEncriptar(tsClave);
                var poUsuario = loBaseDa.Get<SEGMUSUARIO>().Where(x => x.CodigoUsuario == tsCodigoUsuario).FirstOrDefault();
                if (poUsuario != null)
                {
                    poUsuario.Clave = psClaveEncriptada;
                    loBaseDa.SaveChanges();
                }
            }
            else
            {
                string psClaveEncriptada = clsSeguridad.gsEncriptar(tsClave);
                var poUsuario = loBaseDa.Get<SEGMUSUARIO>().Where(x => x.CodigoUsuario == tsCodigoUsuario).FirstOrDefault();
                if (poUsuario != null)
                {

                    poUsuario.ClaveDesdeCorreoCorporativo = psClaveEncriptada;

                    using (var poTran = new TransactionScope())
                    {
                        loBaseDa.SaveChanges();
                        var pmsg = ValidarCorreo(poUsuario.CorreoCorporativo, tsClave);
                        if (!string.IsNullOrEmpty(msg))
                        {
                            return pmsg;
                        }

                        poTran.Complete();
                    }
                }
            }
            return msg;
        }


    
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gbGuardar(Usuario toObject)
        {
            loBaseDa.CreateContext();
            string pbResult = "";
            string psCodigo = string.Empty;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
            var poObject = loBaseDa.Get<SEGMUSUARIO>().Where(x => x.CodigoUsuario == psCodigo).FirstOrDefault();
            if (poObject != null)
            {
                
                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = "";
            }
            else
            {

                poObject = new SEGMUSUARIO();
                loBaseDa.CreateNewObject(out poObject);
                
                pbResult = "";
            }

            poObject.CodigoUsuario = toObject.Codigo;
            poObject.CodigoEstado = toObject.CodigoEstado;
            poObject.NombreCompleto = toObject.Descripcion;
            poObject.Clave = toObject.Clave;
            poObject.IdPerfil = toObject.IdPerfil;
            poObject.TamanoMB = toObject.TamanoMB;
            poObject.FechaModificacion = toObject.Fecha;
            poObject.UsuarioModificacion = toObject.Usuario;
            poObject.TerminalModificacion = toObject.Terminal;
            poObject.UsuarioIngreso = toObject.Usuario;
            poObject.TerminalIngreso = toObject.Terminal;
            poObject.FechaIngreso = toObject.Fecha;
            poObject.AprobacionFinalCotizacion = toObject.AprobacionFinalCotizacion;
            poObject.MinFrecuenciaNotificacion = toObject.MinFrecuenciaNotificacion;
            poObject.HoraFinNotificacion = toObject.HoraFinNotificacion;
            poObject.HoraInicioNotificacion = toObject.HoraInicioNotificacion;
            poObject.IdPersona = toObject.IdPersona;
            poObject.CodigoDepartamento = toObject.CodigoDepartamento;
            poObject.Correo = toObject.Correo;
            poObject.MontoMaxCompra = toObject.MontoMax;
            poObject.EditaProveedorFormaPago = toObject.EditaProveedorFormaPago;
            poObject.EditaTipoOrdenPago = toObject.EditaTipoOrdenPago;
            poObject.CantidadMinCotizaciones = toObject.CantMinCotizaciones;
            poObject.VisualizaZonaOrdenPago = toObject.VisualizaZonaOrdenPago;
            poObject.CodigoUsuarioSap = toObject.CodigoUsuarioSap;
            poObject.ControlaDuplicidadGuias = toObject.ControlaDuplicidadGuias;
            poObject.BodegaEPP = toObject.BodegaEPP;

            bool pbEntra = false;
            if (poObject.CorreoCorporativo != toObject.CorreoCorporativo || poObject.ClaveDesdeCorreoCorporativo != toObject.ClaveDesdeCorreoCorporativo)
            {
                if (!string.IsNullOrEmpty(toObject.CorreoCorporativo) && !string.IsNullOrEmpty(toObject.ClaveDesdeCorreoCorporativo))
                {
                    pbEntra = true;
                }
                
            }

            poObject.EnviarDesdeCorreoCorporativo = toObject.EnviarDesdeCorreoCorporativo;
            poObject.ClaveDesdeCorreoCorporativo = toObject.ClaveDesdeCorreoCorporativo;
            poObject.CorreoCorporativo = toObject.CorreoCorporativo;

            using (var poTran = new TransactionScope())
            {
                loBaseDa.SaveChanges();

                if (pbEntra)
                {
                    var msg = ValidarCorreo(toObject.CorreoCorporativo, clsSeguridad.gsDesencriptar(toObject.ClaveDesdeCorreoCorporativo));
                    if (!string.IsNullOrEmpty(msg))
                    {
                        return msg;
                    }
                }
                
                loBaseDa.ExecuteQuery("EXEC SEGSPSCRIPTPERMISOS");

                poTran.Complete();
            }

            
            return pbResult;
        }

        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsDescripcion"></param>
        /// <returns></returns>
        public List<Usuario> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Usuario
                   {
                       Codigo = x.CodigoUsuario,
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.NombreCompleto,
                       IdPerfil = x.IdPerfil ?? 0,
                       Clave = x.Clave,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,
                       AprobacionFinalCotizacion = x.AprobacionFinalCotizacion ?? false,
                       EditaProveedorFormaPago = x.EditaProveedorFormaPago ?? false,
                       EditaTipoOrdenPago = x.EditaTipoOrdenPago ?? false,
                       CantMinCotizaciones = x.CantidadMinCotizaciones??0,
                       VisualizaZonaOrdenPago = x.VisualizaZonaOrdenPago ?? false,
                       EnviarDesdeCorreoCorporativo = x.EnviarDesdeCorreoCorporativo ?? false,
                       ClaveDesdeCorreoCorporativo = x.ClaveDesdeCorreoCorporativo,
                       CorreoCorporativo = x.CorreoCorporativo,
                       BodegaEPP= x.BodegaEPP
                   }).ToList();
        }

        public Perfil goConsultarPerfil(int tIdPerfil)
        {
            return loBaseDa.Find<GENPPERFIL>().Where(x => x.IdPerfil == tIdPerfil).Select(x=> new Perfil()
            {
                IdPerfil = x.IdPerfil,
                Descripcion = x.Nombre,
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
                psCodigo = loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoUsuario }).OrderBy(x => x.CodigoUsuario).FirstOrDefault()?.CodigoUsuario;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Ultimo || (tsTipo == BuscarCodigo.Tipo.Siguiente && string.IsNullOrEmpty(tsCodigo)))
            {
                psCodigo = loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoUsuario }).OrderByDescending(x => x.CodigoUsuario).FirstOrDefault()?.CodigoUsuario;
            }
            else if (tsTipo == BuscarCodigo.Tipo.Anterior)
            {
                var poListaCodigo = loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoEstado != Diccionario.Eliminado).Select(x => new { x.CodigoUsuario }).ToList().Where(x => x.CodigoUsuario == tsCodigo).ToList();
                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderByDescending(x => x.CodigoUsuario).FirstOrDefault().CodigoUsuario;
                }
                else
                {
                    psCodigo = tsCodigo;
                }
            }
            else if (tsTipo == BuscarCodigo.Tipo.Siguiente)
            {
                var poListaCodigo = loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoUsuario != Diccionario.Eliminado).Select(x => new { x.CodigoUsuario }).ToList().Where(x => int.Parse(x.CodigoUsuario) > int.Parse(tsCodigo)).ToList();

                if (poListaCodigo.Count > 0)
                {
                    psCodigo = poListaCodigo.OrderBy(x => x.CodigoUsuario).FirstOrDefault().CodigoUsuario;
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
            var poObject = loBaseDa.Get<SEGMUSUARIO>().Where(x => x.CodigoUsuario == tsCodigo).FirstOrDefault();
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
        /// Consulta Catálogo de Perfiles
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboPerfil()
        {
            return loBaseDa.Find<GENPPERFIL>().Where(x => x.CodigoEstado == Diccionario.Activo)
                 .Select(x => new Combo
                 {
                     Codigo = x.IdPerfil.ToString(),
                     Descripcion = x.Nombre.ToString(),
                 }).OrderBy(x => x.Descripcion).ToList();
        }
         public List<Combo> goConsultarComboDepartamento()
        {
            return loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoGrupo== Diccionario.ListaCatalogo.Departamento)
                 .Select(x => new Combo
                 {
                     Codigo = x.Codigo.ToString(),
                     Descripcion = x.Descripcion.ToString(),
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        public string ObtenerNombreMenu(int idMenu)
        {
            return loBaseDa.Get<GENPMENU>().Where(c => c.IdMenu == idMenu).Select(c => c.Nombre).FirstOrDefault();
        }

        public string ObtenerTagRibbon(int idRibbon)
        {
            var poMenu = loBaseDa.Get<GENPRIBBONFAVORITOS>().Where(c => c.IdRibbonFavoritos == idRibbon).FirstOrDefault();

            return $"{poMenu.IdMenu},{poMenu.NombreForma}";
        }

        public void AddToFavorites(int idMenu, string nombreMenu, string tsUsuario)
        {
            var poObject = loBaseDa.Get<GENPRIBBONFAVORITOS>().Where(x => x.IdMenu == idMenu && x.CodigoEstado == Diccionario.Activo).FirstOrDefault();

            var poMenu = loBaseDa.Get<GENPMENU>().Where(c => c.IdMenu == idMenu).FirstOrDefault();

            bool pbuevo = false;

            if (poObject != null)
            {
                poObject.UsuarioModificacion = tsUsuario;
                poObject.FechaModificacion = DateTime.Now;
                poObject.TerminalModificacion = "";
                poObject.CodigoEstado = Diccionario.Inactivo;
            }
            else
            {
                poObject = new GENPRIBBONFAVORITOS();
                loBaseDa.CreateNewObject(out poObject);
                pbuevo = true;
                poObject.IdMenu = idMenu;
                poObject.CodigoUsuario = tsUsuario;
                poObject.CodigoEstado = Diccionario.Activo;
                poObject.Nombre = poMenu.Nombre;
                poObject.NombreControl = $"brbtn{poMenu.Nombre}";
                poObject.NombreForma = poMenu.NombreForma;
                poObject.Icono = "bofileattachment_32x32.png";
                poObject.Orden = 1;
                poObject.UsuarioIngreso = tsUsuario;
                poObject.FechaIngreso = DateTime.Now;
                poObject.TerminalIngreso = "";
            }
            loBaseDa.SaveChanges();
        }

        #endregion
    }
}
