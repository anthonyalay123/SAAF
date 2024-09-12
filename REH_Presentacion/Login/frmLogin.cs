using REH_Negocio;
using System.Linq;
using System.Text;
using System;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraEditors;
using System.Timers;
using REH_Presentacion.Comun;

namespace REH_Presentacion.Login
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 16/01/2020
    /// Formulario Genérico para Maestros y parametrizadores
    /// </summary>
    public partial class frmLogin : Form
    {
        #region Variables
        public bool gbLoginExitoso = false;
        clsNUsuario loUsuarioBl;
        frmNotificaciones poFrmMostrarFormulario = new frmNotificaciones();
        #endregion 

        #region Eventos del Formulario
        public frmLogin()
        {
            InitializeComponent();
            loUsuarioBl = new clsNUsuario();
        }

        private void btnAceptar_Click(object sender, EventArgs e)


        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string psMensaje = lsValidaFormulario();
                if (string.IsNullOrEmpty(psMensaje))
                {
                    int pIdPerfil = 0;
                    var pbResult = loUsuarioBl.gbConsultaUsuario(txtUsuario.Text.Trim(), txtClave.Text.ToUpper().Trim(), out pIdPerfil);
                    if (pbResult)
                    {
                        var poParametro = new clsNParametro().goBuscarEntidad();
                        clsPrincipal.gsRucEmpresa = poParametro.Ruc;
                        clsPrincipal.gdcIva = poParametro.PorcentajeIva;

                        if (txtUsuario.Text.Trim() == txtClave.Text.ToUpper().Trim())
                        {
                            
                            var poUsuario = loUsuarioBl.goBuscarMaestroUsuario(txtUsuario.Text.Trim());
                            var poPerfil = loUsuarioBl.goConsultarPerfil(poUsuario.IdPerfil);
                            frmCambioContrasena pfrm = new frmCambioContrasena();
                            pfrm.lsCodigoUsuario = txtUsuario.Text.Trim();
                            pfrm.ShowDialog();
                            if (!string.IsNullOrEmpty(pfrm.lsClave))
                            {
                                clsPrincipal.gsUsuario = txtUsuario.Text.Trim();
                                clsPrincipal.gsTerminal = "";
                                clsPrincipal.gIdPerfil = pIdPerfil;
                                clsPrincipal.gdcTamanoMb = poUsuario.TamanoMB;
                                clsPrincipal.gsDesPerfil = poPerfil.Descripcion;
                                clsPrincipal.gsDesUsuario = poUsuario.Descripcion;
                                clsPrincipal.gsCorreo = poUsuario.Correo;
                                clsPrincipal.gsDesDepartamento = !string.IsNullOrEmpty(poUsuario.CodigoDepartamento) ? loUsuarioBl.goConsultarComboDepartamento().Where(x => x.Codigo == poUsuario.CodigoDepartamento).Select(x => x.Descripcion).FirstOrDefault():"";
                                clsPrincipal.gbEditaProveedorFormaPago = poUsuario.EditaProveedorFormaPago;
                                clsPrincipal.gbEditaTipoOrdenPago = poUsuario.EditaTipoOrdenPago;
                                clsPrincipal.gbSuperUsuario = poUsuario.SuperUsuario;
                                clsPrincipal.gbEnviarDesdeCorreoCorporativo = poUsuario.EnviarDesdeCorreoCorporativo;
                                clsPrincipal.gsCodigoUsuarioSap = poUsuario.CodigoUsuarioSap;
                                gbLoginExitoso = pbResult;

                                clsPrincipal.gsAccionPerfil = loUsuarioBl.goAccionPerfil(pIdPerfil);
                                Close();
                            }
                        }
                        else
                        {
                            var poUsuario = loUsuarioBl.goBuscarMaestroUsuario(txtUsuario.Text.Trim());
                            var poPerfil = loUsuarioBl.goConsultarPerfil(poUsuario.IdPerfil);
                            clsPrincipal.gsUsuario = txtUsuario.Text.Trim();
                            clsPrincipal.gsTerminal = "";
                            clsPrincipal.gIdPerfil = pIdPerfil;
                            clsPrincipal.gdcTamanoMb = poUsuario.TamanoMB;
                            clsPrincipal.gsDesPerfil = poPerfil.Descripcion;
                            clsPrincipal.gsDesUsuario = poUsuario.Descripcion;
                            clsPrincipal.HoraInicioNotificacion = poUsuario.HoraInicioNotificacion;
                            clsPrincipal.HoraFinNotificacion = poUsuario.HoraFinNotificacion;
                            clsPrincipal.MinFrecuenciaNotificacion = poUsuario.MinFrecuenciaNotificacion;
                            clsPrincipal.gsCorreo = poUsuario.Correo;
                            clsPrincipal.gsDesDepartamento = !string.IsNullOrEmpty(poUsuario.CodigoDepartamento) ? loUsuarioBl.goConsultarComboDepartamento().Where(x => x.Codigo == poUsuario.CodigoDepartamento).Select(x => x.Descripcion).FirstOrDefault() : "";
                            clsPrincipal.gbEditaProveedorFormaPago = poUsuario.EditaProveedorFormaPago;
                            clsPrincipal.gbEditaTipoOrdenPago = poUsuario.EditaTipoOrdenPago;
                            clsPrincipal.gbSuperUsuario = poUsuario.SuperUsuario;
                            clsPrincipal.gbEnviarDesdeCorreoCorporativo = poUsuario.EnviarDesdeCorreoCorporativo;
                            clsPrincipal.gsCodigoUsuarioSap = poUsuario.CodigoUsuarioSap;
                            gbLoginExitoso = pbResult;

                            clsPrincipal.gsAccionPerfil = loUsuarioBl.goAccionPerfil(pIdPerfil);
                            Close();
                        }
                        
                    }
                    else
                    {
                        XtraMessageBox.Show("Usuario/Clave incorrecta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show(psMensaje, "Aviso",MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (ex.Message.Contains("command"))
                {
                    XtraMessageBox.Show(ex.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Cursor.Current = Cursors.Default;

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnterEqualTab(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
                System.Windows.Forms.SendKeys.Send("{TAB}");
        }
        #endregion

        #region Validaciones
        /// <summary>
        /// Valida controles del formulario
        /// </summary>
        /// <returns>Retorna un string, vacio si todo está correcto caso contrario una descripción de la novedad</returns>
        private string lsValidaFormulario()
        {
            string psMensaje = string.Empty;
            if (string.IsNullOrEmpty(txtUsuario.Text.Trim()))
            {
                return "Ingrese usuario.";
            }
            if (string.IsNullOrEmpty(txtClave.Text.Trim()))
            {
                return "Ingrese clave";
            }
            return psMensaje;
        }
        #endregion

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void frmLogin_Load_1(object sender, EventArgs e)
        {

        }

        
        private void lmostrarNotificaciones()
        {
            poFrmMostrarFormulario.Show();
        }

        private void lblRestaurarContrasena_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (txtUsuario.Text!="")
                {
                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro que desea Restaurar Contraseña?", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var poObject = loUsuarioBl.goBuscarMaestroUsuario(txtUsuario.Text.ToString());
                        if (poObject != null)
                        {
                            if (!string.IsNullOrEmpty(poObject.Correo))
                            {
                                Random rdn = new Random();
                                string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890%$#@";
                                int longitud = caracteres.Length;
                                char letra;
                                int longitudContrasenia = 10;
                                string contraseniaAleatoria = string.Empty;
                                for (int i = 0; i < longitudContrasenia; i++)
                                {
                                    letra = caracteres[rdn.Next(longitud)];
                                    contraseniaAleatoria += letra.ToString();
                                }
                                string contraseniaEncriptada = gsEncriptar(contraseniaAleatoria);

                                //var mensaje = loUsuarioBl.RestaurarContrasena(poObject.Codigo, poObject.Descripcion, poObject.Correo, contraseniaEncriptada, contraseniaAleatoria);
                                var guardado = loUsuarioBl.goCambiarContrasena(txtUsuario.Text.ToString(), contraseniaEncriptada);
                                if (guardado)
                                {
                                    clsComun.EnviarPorCorreo(poObject.Correo, "Restaurar Contraseña", "Su constraseña ha sido reestablecida, Nueva Contraseña: " + contraseniaAleatoria, null);
                                    XtraMessageBox.Show("Nueva contraseña fue enviada al correo que tiene registrado en su usuario. Correo: "+ poObject.Correo, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                 
                            }
                            else
                            {
                                XtraMessageBox.Show("Correo no configurado, comunicarse con sistemas", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("No existe usuario", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("Ingrese el Usuario", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string gsEncriptar(string tsCadena)
        {
            string result = string.Empty;
            byte[] encryted = Encoding.Unicode.GetBytes(tsCadena);
            result = Convert.ToBase64String(encryted);
            return result;
        }
    }
}
