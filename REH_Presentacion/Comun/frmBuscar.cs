using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace REH_Presentacion.Comun
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 17/01/2020
    /// Formulario Genérico para busqueda de datos
    /// </summary>
    public partial class frmBuscar : Form
    {
        
        #region Variables
        private DataTable loResultado;
        public DataGridViewRow FilaSeleccionada { get; private set; }
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="toResultado">Tabla de datos a mostrar</param>
        public frmBuscar(DataTable toResultado)
        {
            InitializeComponent();
            loResultado = toResultado;
            dgvResultados.DataSource = loResultado;
            tslTotalRegsitros.Text = String.Format("Total de registro(s) encontrado(s): {0} ", dgvResultados.RowCount);
        }

        /// <summary>
        /// Convierte las columnas en campos para filtrar
        /// </summary>
        private void CargaFiltros()
        {
            tscBuscar.Items.Clear();
            foreach (DataColumn column in loResultado.Columns)
                if (dgvResultados.Columns[column.ColumnName].Visible)
                    tscBuscar.Items.Add(column.ToString());
        }
        /// <summary>
        /// Selecciona una fila del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvResultados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                FilaSeleccionada = dgvResultados.Rows[e.RowIndex];
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        /// <summary>
        /// Busqueda mientras set dato en el campo de texto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tstBuscar_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tstBuscar.Text.Trim()))
            {
                List<DataRow> poResultado = loResultado.AsEnumerable().Where(x => x.Field<string>(tscBuscar.Text).Trim().ToUpper().Contains(tstBuscar.Text.Trim().ToUpper())).ToList();
                if (poResultado.Count > 0)
                    dgvResultados.DataSource = poResultado.CopyToDataTable();
                else
                {
                    DataTable poTempResultado = loResultado.Clone();
                    dgvResultados.DataSource = poTempResultado;
                }
            }
            else
                dgvResultados.DataSource = loResultado;

            tslTotalRegsitros.Text = "Total de registro(s) encontrado(s) " + dgvResultados.RowCount;
        }
        /// <summary>
        /// Inicia eventos del formulario 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCoBuscar_Load(object sender, EventArgs e)
        {
            CargaFiltros();
            tstBuscar.Focus();
            if (tscBuscar.Items.Count > 0) tscBuscar.SelectedIndex = 0;
        }
        #endregion
    }
}
