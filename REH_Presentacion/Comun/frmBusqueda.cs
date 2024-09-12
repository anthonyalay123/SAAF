using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
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
    /// Autor: Victor Arévalo
    /// Fecha: 06/02/2020
    /// Formulario Genérico para busqueda de datos
    /// </summary>
    public partial class frmBusqueda : Form
    {
        #region Variables
        private DataTable loResultado;
        public string lsSegundaColumna { get; private set; }
        public string lsTerceraColumna { get; private set; }
        public string lsCuartaColumna { get; private set; }
        public string lsCodigoSeleccionado { get; private set; }
        public int Index { get; private set; }
        RepositoryItemMemoEdit rpiMedDescripcion;
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();

        #endregion

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="toResultado">Tabla de datos a mostrar</param>
        public frmBusqueda(DataTable toResultado, List<GridBusqueda> tsListaColumnaRepItem = null, List<string> tsListaColumnasOcultar = null, bool WordWrap = false)
        {
            InitializeComponent();

            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;

            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;

            loResultado = toResultado;
            bsBusqueda.DataSource = loResultado;
            gcBusqueda.DataSource = bsBusqueda;
            dgvBusqueda.PopulateColumns();
            dgvBusqueda.BestFitColumns();


            if (dgvBusqueda.Columns["VerAdjunto"] != null)
            {
                clsComun.gDibujarBotonGrid(rpiBtnShow, dgvBusqueda.Columns["VerAdjunto"], "Ver Adjunto", Diccionario.ButtonGridImage.show_16x16,15);
                dgvBusqueda.OptionsBehavior.Editable = true;
                for (int i = 0; i < dgvBusqueda.Columns.Count; i++)
                {
                    if (dgvBusqueda.Columns[i].FieldName == "VerAdjunto")
                    {
                        dgvBusqueda.Columns[i].OptionsColumn.ReadOnly = false; 
                    }
                    else
                    {
                        dgvBusqueda.Columns[i].OptionsColumn.ReadOnly = true;
                    }
                }
            }
            else
            {
                dgvBusqueda.OptionsBehavior.Editable = false;
            }

            if (tsListaColumnasOcultar != null)
            {
                foreach (var item in tsListaColumnasOcultar)
                {
                    try
                    { dgvBusqueda.Columns[item].Visible = false; }
                    catch (Exception) { }
                }
            }

            if (WordWrap)
            {
                for (int i = 0; i < dgvBusqueda.Columns.Count; i++)
                {
                    dgvBusqueda.Columns[i].ColumnEdit = rpiMedDescripcion;
                }
            }

            dgvBusqueda.OptionsCustomization.AllowColumnMoving = false;
            //dgvBusqueda.OptionsView.ColumnAutoWidth = false;
            dgvBusqueda.OptionsView.ShowAutoFilterRow = true;
            dgvBusqueda.OptionsView.RowAutoHeight = true;
            //dgvBusqueda.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            
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
            try
            {
                if (dgvBusqueda.FocusedRowHandle >= 0)
                {
                    lsCodigoSeleccionado = dgvBusqueda.GetDataRow(dgvBusqueda.FocusedRowHandle).ItemArray[0].ToString();
                    lsSegundaColumna = dgvBusqueda.GetDataRow(dgvBusqueda.FocusedRowHandle).ItemArray[1].ToString();
                    try
                    {
                        lsTerceraColumna = dgvBusqueda.GetDataRow(dgvBusqueda.FocusedRowHandle).ItemArray[2].ToString();
                    }
                    catch (Exception){}
                    try
                    {
                        lsCuartaColumna = dgvBusqueda.GetDataRow(dgvBusqueda.FocusedRowHandle).ItemArray[3].ToString();
                    }
                    catch (Exception) { }

                    DialogResult = DialogResult.OK;
                    Index= dgvBusqueda.FocusedRowHandle;
                    Close();
                }
            }
            catch (Exception ex )
            {
                XtraMessageBox.Show(ex.Message);
            }
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


        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvBusqueda.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (DataTable)bsBusqueda.DataSource;
                if (!string.IsNullOrEmpty(poLista.Rows[piIndex]["VerAdjunto"].ToString()))
                {
                    frmVerPdf pofrmVerPdf = new frmVerPdf();
                    pofrmVerPdf.lsRuta = poLista.Rows[piIndex]["VerAdjunto"].ToString();
                    pofrmVerPdf.Show();
                    pofrmVerPdf.SetDesktopLocation(0, 0);

                    pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;

                }

                else
                {
                    XtraMessageBox.Show("No hay archivo para mostrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void frmBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
