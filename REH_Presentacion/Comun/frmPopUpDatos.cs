using DevExpress.XtraEditors;
using reporte;
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
    public partial class frmPopUpDatos : Form
    {
        public frmPopUpDatos(DataTable toResultado)
        {
            InitializeComponent();
            bsDatos = new BindingSource();
            bsDatos.DataSource = toResultado;
            gcDatos.DataSource = bsDatos;

            dgvDatos.PopulateColumns();
            dgvDatos.BestFitColumns();

        }

        private void btnCopiar_Click(object sender, EventArgs e)
        {
            dgvDatos.OptionsSelection.MultiSelect = true;
            dgvDatos.SelectAll();
            dgvDatos.CopyToClipboard();
            dgvDatos.OptionsSelection.MultiSelect = false;

            //XtraMessageBox.Show("Copiado!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lblTexto.Text = "COPIADO!";
        }
    }
}
