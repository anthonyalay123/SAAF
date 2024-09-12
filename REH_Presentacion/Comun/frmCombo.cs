using DevExpress.XtraEditors;
using GEN_Entidad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Comun
{
    /// <summary>
    /// Formulario base que muestra un combo para seleccionar un dato
    /// </summary>
    public partial class frmCombo : Form
    {
        #region Variables
        public string lsNombre;
        public List<Combo> loCombo = new List<Combo>();
        public string lsSeleccionado;

        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmCombo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento de inicialización del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCombo_Load(object sender, EventArgs e)
        {
            try
            {
                lblNombre.Text = lsNombre;
                clsComun.gLLenarCombo(ref cmbNombre, loCombo);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Acepta los datos ingresados/seleccionados y cierra el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                lsSeleccionado = cmbNombre.EditValue.ToString();
                Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Cierra el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Enter igual que Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCombo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }
        #endregion
    }
}
