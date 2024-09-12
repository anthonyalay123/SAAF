using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using GEN_Entidad.Entidades.Ventas;
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
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 06/02/2020
    /// Formulario Genérico para busqueda de datos
    /// </summary>
    public partial class frmBusquedaImport : Form
    {
        #region Variables

        public string lsSegundaColumna { get; private set; }
        public string lsTerceraColumna { get; private set; }
        public string lsCuartaColumna { get; private set; }
        public string lsCodigoSeleccionado { get; private set; }
        public int Index { get; private set; }

        public List<ListaGuiaRemisionFactura> loResultado = new List<ListaGuiaRemisionFactura>();

        RepositoryItemMemoEdit rpiMedDescripcion;
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();

        BindingSource bsDatos = new BindingSource();

        #endregion

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="toResultado">Tabla de datos a mostrar</param>
        public frmBusquedaImport(List<ListaGuiaRemisionFactura> toResultado, List<GridBusqueda> tsListaColumnaRepItem = null, List<string> tsListaColumnasOcultar = null, bool WordWrap = false)
        {
            InitializeComponent();

            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;

            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;

            bsBusqueda.DataSource = toResultado;
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
            else if(dgvBusqueda.Columns["Sel"] != null)
            {
                dgvBusqueda.OptionsBehavior.Editable = true;
                for (int i = 0; i < dgvBusqueda.Columns.Count; i++)
                {
                    if (dgvBusqueda.Columns[i].FieldName == "Sel")
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
                //if (dgvBusqueda.FocusedRowHandle >= 0)
                //{
                //    lsCodigoSeleccionado = dgvBusqueda.GetDataRow(dgvBusqueda.FocusedRowHandle).ItemArray[0].ToString();
                //    lsSegundaColumna = dgvBusqueda.GetDataRow(dgvBusqueda.FocusedRowHandle).ItemArray[1].ToString();
                //    try
                //    {
                //        lsTerceraColumna = dgvBusqueda.GetDataRow(dgvBusqueda.FocusedRowHandle).ItemArray[2].ToString();
                //    }
                //    catch (Exception){}
                //    try
                //    {
                //        lsCuartaColumna = dgvBusqueda.GetDataRow(dgvBusqueda.FocusedRowHandle).ItemArray[3].ToString();
                //    }
                //    catch (Exception) { }

                //    DialogResult = DialogResult.OK;
                //    Index= dgvBusqueda.FocusedRowHandle;
                //    Close();
                //}
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
                var poLista = (List<ListaGuiaRemisionFactura>)bsBusqueda.DataSource;
                if (poLista.Count > 0)
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

        private void btnPlantilla_Click(object sender, EventArgs e)
        {
            try
            {
                GridControl gc = new GridControl();
                BindingSource bs = new BindingSource();
                GridView dgv = new GridView();

                gc.DataSource = bs;
                gc.MainView = dgv;
                gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                dgv.GridControl = gc;
                this.Controls.Add(gc);
                var poLista = new List<ImportGuiaRemision>();
                poLista.Add(new ImportGuiaRemision() { Numero = "000-000-000000000", ValorPorBulto = 0.00M });
                bs.DataSource = poLista;
                dgv.BestFitColumns();
                dgv.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
                // Exportar Datos
                clsComun.gSaveFile(gc, "Plantilla_" + Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");

                gc.Visible = false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private string lsValidaDuplicados(List<ImportGuiaRemision> toListaGrid, List<ImportGuiaRemision> toListaInsertar)
        {
            string psMsg = string.Empty;

            foreach (var item in toListaInsertar)
            {
                var piRegistro = toListaGrid.Where(x => x.Numero == item.Numero).Count();
                if (piRegistro > 0)
                {
                    psMsg = string.Format("{0}Número de Guía: {1} Ya están parametrizados. \n", psMsg, item.Numero);
                }
            }

            return psMsg;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                this.loResultado = ((List<ListaGuiaRemisionFactura>)bsBusqueda.DataSource).Where(x=>x.Sel).ToList();

                string msg = "";
                foreach (var item in loResultado.Where(x=>string.IsNullOrEmpty(x.Proveedor)))
                {
                    msg = string.Format("{0}No es posible seleccionar guías sin relación del transportista con su proveedor. guía # {1}. Comunicarse con Contabilidad.\n",msg,item.Numero); 
                }

                if (string.IsNullOrEmpty(msg))
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    XtraMessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddManualmente_Click(object sender, EventArgs e)
        {
            try
            {
                List<ListaGuiaRemisionFactura> poLista = (List<ListaGuiaRemisionFactura>)bsBusqueda.DataSource;

                var sel = poLista.Select(x => x.Sel)?.FirstOrDefault()??false;
                foreach (var item in poLista)
                {
                    item.Sel = !sel;
                }


                bsBusqueda.DataSource = poLista;
                dgvBusqueda.RefreshData();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int cont = 0;
            try
            {

                DataTable dtBusqueda = new DataTable();

                dtBusqueda.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Cliente"),
                                    new DataColumn("Guia"),
                                    new DataColumn("Total")
                                    });

                OpenFileDialog ofdRuta = new OpenFileDialog();
                ofdRuta.Title = "Seleccione Archivo";
                //(*.jpg; *.jpeg; *.png)| *.jpg; *.jpeg; *.png; "
                ofdRuta.Filter = "Files(*.xls; *.xlsx;)|*.xls; *.xlsx;";

                if (ofdRuta.ShowDialog() == DialogResult.OK)
                {
                    if (!ofdRuta.FileName.Equals(""))
                    {
                        DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);

                        var poListaImportada = new List<ImportGuiaRemision>();

                        List<string> psListaMsg = new List<string>();

                        int fila = 9;
                        string psMsgLista = string.Empty;
                        foreach (DataRow item in dt.Rows)
                        {

                            if (!string.IsNullOrEmpty(item["F4"].ToString().Trim()) && item["F4"].ToString().Trim().Contains("-"))
                            {
                                string psMsgFila = "";
                                string psMsgOut = "";

                                //try
                                //{

                                ImportGuiaRemision poItem = new ImportGuiaRemision();
                                poItem.Cliente = clsComun.gdValidarRegistro("F3", "s", item["F3"].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.Numero = clsComun.gdValidarRegistro("F4", "s", item["F4"].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.ValorPorBulto = clsComun.gdValidarRegistro("F9", "d", item["F9"].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.Total = clsComun.gdValidarRegistro("F10", "d", item["F10"].ToString().Trim(), fila, true, ref psMsgOut);



                                fila++;

                                if (string.IsNullOrEmpty(psMsgOut))
                                {
                                    poListaImportada.Add(poItem);
                                }
                                else
                                {
                                    psMsgLista = psMsgLista + psMsgOut;
                                }

                                //}
                                //catch (Exception ex)
                                //{

                                //    throw;
                                //}
                            }
                        }
                        if (!string.IsNullOrEmpty(psMsgLista))
                        {
                            psListaMsg.Add(psMsgLista);
                        }


                        if (!string.IsNullOrEmpty(psMsgLista))
                        {
                            XtraMessageBox.Show(psMsgLista, "No es posible importar datos!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        string psMensajeDeImportar = "";
                        var poLista = (List<ListaGuiaRemisionFactura>)bsBusqueda.DataSource;
                        foreach (var item in poListaImportada)
                        {
                            var reg = poLista.Where(x => x.Numero == item.Numero).FirstOrDefault();
                            if (reg != null)
                            {
                                reg.Sel = true;
                                reg.ValorPorBulto = item.ValorPorBulto;
                                reg.Total = item.Total;
                            }
                            else
                            {
                                DataRow row = dtBusqueda.NewRow();
                                row["Cliente"] = item.Cliente;
                                row["Guia"] = item.Numero;
                                row["Total"] = item.Total.ToString("c2");

                                dtBusqueda.Rows.Add(row);
                            }
                        }

                        if (dtBusqueda.Rows.Count > 0)
                        {
                            XtraMessageBox.Show("No se importaron las siguientes guías", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            frmBusqueda pofrmBuscar = new frmBusqueda(dtBusqueda) { Text = "Listado de guías no importadas" };
                            pofrmBuscar.ShowDialog();
                        }

                        if (dtBusqueda.Rows.Count != poListaImportada.Count)
                        {
                            dgvBusqueda.RefreshData();
                            XtraMessageBox.Show("Importado Exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
        }
    }
}
