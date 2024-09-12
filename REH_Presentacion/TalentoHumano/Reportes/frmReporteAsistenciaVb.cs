using DevExpress.XtraEditors;
using REH_Presentacion.Formularios;
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

namespace REH_Presentacion.TalentoHumano.Reportes
{
    public partial class frmReporteAsistenciaVb : Form1
    {
        public frmReporteAsistenciaVb()
        {
            InitializeComponent();
        }

        private void frmReporteAsistenciaVb_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
