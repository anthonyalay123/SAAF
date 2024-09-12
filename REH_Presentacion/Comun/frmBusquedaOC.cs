using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
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
    public partial class frmBusquedaOC : Form
    {
        #region Variables
        private DataTable loResultado;
        public string lsSegundaColumna { get; private set; }
        public decimal ldSaldo { get; private set; }
        public string lsCodigoSeleccionado { get; private set; }
        public List<int> idUsado { get; private set; }
        RepositoryItemMemoEdit rpiMedDescripcion;
        #endregion

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="toResultado">Tabla de datos a mostrar</param>
        public frmBusquedaOC(List<Proveedor> toResultado, List<int> idUsados, List<GridBusqueda> tsListaColumnaRepItem = null)
        {
            InitializeComponent();

            idUsado = idUsados;
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;

            //loResultado = toResultado;
            bsBusqueda.DataSource = toResultado;

            dgvBusqueda.OptionsBehavior.Editable = false;
            dgvBusqueda.OptionsCustomization.AllowColumnMoving = false;
            //dgvBusqueda.OptionsView.ColumnAutoWidth = false;
            dgvBusqueda.OptionsView.ShowAutoFilterRow = true;
            dgvBusqueda.OptionsView.RowAutoHeight = true;
            //dgvBusqueda.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            dgvBusqueda.BestFitColumns();
            dgvBusqueda.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle;

            
            if (tsListaColumnaRepItem != null)
            {
                foreach (var item in tsListaColumnaRepItem)
                {
                    dgvBusqueda.Columns[item.Columna].ColumnEdit = rpiMedDescripcion;
                    dgvBusqueda.Columns[item.Columna].Width = item.Ancho;
                }
            }
            
        }

        /// <summary>
        /// Selecciona una fila del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBusqueda_DoubleClick(object sender, EventArgs e)
        {
            //try
            //{
            //    if (dgvBusqueda.FocusedRowHandle >= 0)
            //    {
            //        lsCodigoSeleccionado = dgvBusqueda.GetDataRow(dgvBusqueda.FocusedRowHandle).ItemArray[0].ToString();
            //        lsSegundaColumna = dgvBusqueda.GetDataRow(dgvBusqueda.FocusedRowHandle).ItemArray[1].ToString();
            //        DialogResult = DialogResult.OK;
            //        Close();
            //    }
            //}
            //catch (Exception ex )
            //{
            //    XtraMessageBox.Show(ex.Message);
            //}
        }

        /// <summary>
        /// Exportar datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvBusqueda.PostEditor();
                DataTable poLista = (DataTable)bsBusqueda.DataSource;
                if (poLista.Rows.Count > 0)
                {
                    clsComun.gSaveFile(gcBusqueda, Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private void gcBusqueda_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.KeyCode.Equals(Keys.Enter))
                {

                    dgvBusqueda_DoubleClick(sender,e);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }

            
        }

        private void frmBusqueda_Load(object sender, EventArgs e)
        {

        }

        private void tsbAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                var poLista = (List<Proveedor>)bsBusqueda.DataSource;
                 dgvBusqueda.PostEditor();

                foreach (var item in poLista.Where(x => x.Sel))
                {
                    lsCodigoSeleccionado += item.IdProveedor + ", ";
                    //if (idUsado.Contains(item.IdProveedor))
                    //{
                    //    item.Saldo = 0;
                    //}
                    ldSaldo += item.Saldo;

                    
                }

                DialogResult = DialogResult.OK;
                Close();
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private void tsbCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
