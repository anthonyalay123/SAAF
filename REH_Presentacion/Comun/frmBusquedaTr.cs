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
    /// Autor: Victor Arévalo
    /// Fecha: 06/02/2020
    /// Formulario Genérico para busqueda de datos
    /// </summary>
    public partial class frmBusquedaTr : Form
    {
        #region Variables
        private DataTable loResultado;
        public string lsSegundaColumna { get; private set; }
        public string lsCodigoSeleccionado { get; private set; }
        #endregion
        
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="toResultado">Tabla de datos a mostrar</param>
        public frmBusquedaTr(DataTable toResultado)
        {
            InitializeComponent();
            loResultado = toResultado;
            bsBusqueda.DataSource = loResultado;
            //tslTotalRegsitros.Text = String.Format("Total de registro(s) encontrado(s): {0} ", dgvResultados.RowCount);
        }

        /// <summary>
        /// Selecciona una fila del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBusqueda_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (dgvBusqueda.FocusedRowHandle >= 0)
                {
                    lsCodigoSeleccionado = dgvBusqueda.GetDataRow(dgvBusqueda.FocusedRowHandle).ItemArray[0].ToString();
                    lsSegundaColumna = dgvBusqueda.GetDataRow(dgvBusqueda.FocusedRowHandle).ItemArray[1].ToString();
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex )
            {
                XtraMessageBox.Show(ex.Message);
            }
        }
    }
}
