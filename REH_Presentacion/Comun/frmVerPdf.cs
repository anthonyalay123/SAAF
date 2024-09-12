using DevExpress.XtraEditors;
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
    /// Formulario genérico para visualizar los pdf
    /// </summary>
    public partial class frmVerPdf : Form
    {
        #region Eventos
        public string lsRuta;
        #endregion

        #region Eventos
        /// <summary>
        /// Constrcutor de la clase
        /// </summary>
        public frmVerPdf()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Evento que se dispara al inciar el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmVerPdf_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lsRuta))
                {
                    axAcroPDF1.src = lsRuta;
                    this.WindowState = FormWindowState.Normal;
                }
                else
                {
                    XtraMessageBox.Show("No se encotró ruta para abrir archivo", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
            

        }
        #endregion
    }
}
