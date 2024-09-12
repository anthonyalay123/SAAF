using DevExpress.XtraEditors;
using REH_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Login
{
    public partial class frmCambioContrasena : Form
    {

        public string lsCodigoUsuario;
        public string lsClave;
        public string Form = "SAAF";

        clsNUsuario loUsuarioBl;

        public frmCambioContrasena()
        {
            InitializeComponent();
            loUsuarioBl = new clsNUsuario();
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

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (clsPrincipal.gsUsuario==null)
                {
                    clsPrincipal.gsUsuario = lsCodigoUsuario;
                }

                var poObject = loUsuarioBl.goBuscarMaestroUsuario(clsPrincipal.gsUsuario);

                if (Form == "SAAF")
                {
                    if (string.IsNullOrEmpty(txtAnterior.Text.Trim()))
                    {
                        XtraMessageBox.Show("Ingrese las contraseñas!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (gsDesencriptar(poObject.Clave) != txtAnterior.Text.Trim())
                    {
                        XtraMessageBox.Show("Contraseña anterior incorrecta!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (txtClave.Text.Trim() == lsCodigoUsuario)
                    {
                        XtraMessageBox.Show("La clave no puede ser igual a su Usuario!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {

                }
                


                if (string.IsNullOrEmpty(txtClave.Text.Trim()))
                {
                    XtraMessageBox.Show("Ingrese las contraseñas!" , "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txtClave.Text.Trim() != txtRepetirClave.Text.Trim())
                {
                    XtraMessageBox.Show("No coinciden las contraseñas ingresadas!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                lsClave = txtClave.Text.Trim();

                if (lsCodigoUsuario==null)
                {
                    lsCodigoUsuario = clsPrincipal.gsUsuario;
                }
                var msg = loUsuarioBl.gActualizarContrasena(Form, lsCodigoUsuario, lsClave);

                if (string.IsNullOrEmpty(msg))
                {
                    XtraMessageBox.Show("Cambio de Clave Exitoso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

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

        private void frmCambioContrasena_Load(object sender, EventArgs e)
        {
            if (Form == "SAAF")
            {
                Text = "Cambiar contraseña de SAAF";
                txtClave.CharacterCasing = CharacterCasing.Upper;
                txtRepetirClave.CharacterCasing = CharacterCasing.Upper;
            }
            else
            {
                if (!clsPrincipal.gbEnviarDesdeCorreoCorporativo)
                {
                    XtraMessageBox.Show("No tiene habilitada esta opción, comunicarse con sistemas.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                Text = "Cambiar contraseña de Office 365";
                txtClave.CharacterCasing = CharacterCasing.Normal;
                txtRepetirClave.CharacterCasing = CharacterCasing.Normal;
                txtAnterior.Visible = false;
                label2.Visible = false;
            }
        }


        /// <summary>
        /// Función que devuelve una cadena desencriptada
        /// </summary>
        /// <param name="tsCadena">texto a desencriptar</param>
        /// <returns>valor desencriptado</returns>
        public static string gsDesencriptar(string tsCadena)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(tsCadena);
            result = Encoding.Unicode.GetString(decryted);
            return result;
        }

        private void txtAnterior_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtRepetirClave_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblClave_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtClave_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
