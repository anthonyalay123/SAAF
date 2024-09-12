using DevExpress.XtraGrid.Views.Grid;
using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraGrid.Columns;
using System.IO;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
using GEN_Entidad.Entidades;
using DevExpress.XtraReports.UI;
using REH_Presentacion.Ventas.Reportes.Rpt;
using System.Net.Mail;
using DevExpress.XtraPrinting;
using System.Configuration;
using System.Drawing.Printing;
using System.Data.SqlClient;

namespace REH_Presentacion.Ventas.Reportes
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 07/04/2020
    /// Formulario de reporte de Cumplimiento de Kilos Litros
    /// </summary>
    public partial class frmRpCumplimientoKilosLitros : frmBaseTrxDev
    {

        #region Variables

        clsNGestorConsulta loLogicaNegocio;
        string lsQuery;
        string lsTituloReporte;
        string lsDataSet;
        bool lbLandSpace;
        Nullable<int> liFixedColumn;
        public int lIdGestorConsulta;

        private PrinterSettings prnSettings;

        decimal Presupuesto;
        decimal Ventas;
        #endregion

        #region Eventos
        public frmRpCumplimientoKilosLitros()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNGestorConsulta();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                //cmbReporte.Focus();
                //clsComun.gLLenarCombo(ref cmbReporte, loLogicaNegocio.goConsultarComboReportes());
                clsComun.gLLenarCombo(ref cmbZonas, loLogicaNegocio.goConsultarZonasSAAF(), false, false);
                var piGestor = loLogicaNegocio.giConsultaId(int.Parse(Tag.ToString().Split(',')[0]));
                if (piGestor != 0) lIdGestorConsulta = piGestor;
                lCargarEventosBotones();
                lLimpiar();
                //loLogicaNegocio.ProbarConexion();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Nuevo, Generalmente Limpia el formulario 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
               lLimpiar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                SendKeys.Send("{TAB}");
                lConsultarBoton();
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Consultar, Consulta Query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            //SendKeys.Send("{TAB}");
            lConsultarBoton();
        }

        /// <summary>
        /// Evento del botón Exportar, Exporta a Excel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {

                dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    DataTable poLista = (DataTable)bsDatos.DataSource;
                    if (poLista != null && poLista.Rows.Count > 0)
                    {
                        string psFilter = "Files(*.xlsx;)|*.xlsx;";
                        clsComun.gSaveFile(gcDatos, Text + ".xlsx", psFilter);     
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Exportar, Exporta a Pdf.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            try
            {

                dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    DataTable poLista = (DataTable)bsDatos.DataSource;
                    if (poLista != null && poLista.Rows.Count > 0)
                    {
                        string psFilter = "Files(*.PDF;)|*.PDF;";
                        clsComun.gSaveFile(gcDatos, Text + ".pdf", psFilter,"PDF");
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      

        /// <summary>
        /// Evento del botón Imprimir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            //lEnvioAutomatio();
            try
            {
                System.Data.DataSet ds1 = new System.Data.DataSet();
                string psParametro1 = "";

                var dt1 = ldtValidaQuery();
                dt1.TableName = "KilosLitros";
                ds1.Merge(dt1);
                if (dt1.Rows.Count > 0)
                {
                    psParametro1 = string.Format("VENTAS DEL {0} AL {1} (AF)",dtpFechaInicial.Value.ToString("dd/MM/yyyy"), dtpFechaFinal.Value.ToString("dd/MM/yyyy"));

                    xrptKilosLitrosZona xrpt = new xrptKilosLitrosZona();

                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrpt.Parameters["Titulo"].Value = psParametro1;
                    xrpt.DataSource = ds1;
                    //Se invoca la ventana que muestra el reporte.
                    xrpt.RequestParameters = false;
                    xrpt.Parameters["Titulo"].Visible = false;

                    //xrptKilosLitrosFamilia xrpt2 = new xrptKilosLitrosFamilia();

                    ////Se establece el origen de datos del reporte (El dataset previamente leído)
                    //xrpt2.Parameters["Titulo"].Value = string.Format("VENTAS DEL {0} AL {1}. ZONA: {2}. (AF)", dtpFechaInicial.Value.ToString("dd/MM/yyyy"), dtpFechaFinal.Value.ToString("dd/MM/yyyy"),cmbZonas.Text);
                    //xrpt2.DataSource = ds1;
                    ////Se invoca la ventana que muestra el reporte.
                    //xrpt2.RequestParameters = false;
                    //xrpt2.Parameters["Titulo"].Visible = false;



                    XtraReport[] reports;

                    reports = new XtraReport[] { xrpt };

                    ReportPrintTool pt1 = new ReportPrintTool(xrpt);
                    pt1.PrintingSystem.StartPrint += new PrintDocumentEventHandler(PrintingSystem_StartPrint);

                    foreach (XtraReport report in reports)
                    {
                        ReportPrintTool pts = new ReportPrintTool(report);
                        pts.PrintingSystem.StartPrint +=
                            new PrintDocumentEventHandler(reportsStartPrintEventHandler);
                    }

                    foreach (XtraReport report in reports)
                    {
                        ReportPrintTool pts = new ReportPrintTool(report);
                        pts.ShowRibbonPreview();
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existen datos para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Eventos necesarios para la presentación de reportes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintingSystem_StartPrint(object sender, PrintDocumentEventArgs e)
        {
            prnSettings = e.PrintDocument.PrinterSettings;
        }

        /// <summary>
        /// Eventos necesarios para la presentación de reportes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reportsStartPrintEventHandler(object sender, PrintDocumentEventArgs e)
        {
            int pageCount = e.PrintDocument.PrinterSettings.ToPage;
            e.PrintDocument.PrinterSettings = prnSettings;

            // The following line is required if the number of pages for each report varies, 
            // and you consistently need to print all pages.
            e.PrintDocument.PrinterSettings.ToPage = pageCount;
        }

        private void txtPar1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData.Equals(Keys.Enter))
                {
                    SendKeys.Send("{TAB}");
                    lConsultarBoton();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnExportarPdf"] != null) tstBotones.Items["btnExportarPdf"].Click += btnExportarPdf_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

            //txtPar1.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar2.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar3.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar4.KeyDown += new KeyEventHandler(EnterEqualTab);


        }

        private bool lbEsValido()
        {
            //if (cmbReporte.EditValue.ToString() == Diccionario.Seleccione)
            //{
            //    XtraMessageBox.Show("Seleccione Empleado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}

            return true;
        }

        private void lLimpiar(bool tbSoloDetalle = false)
        {
            if (!tbSoloDetalle)
            {
                gcDatos.DataSource = null;
                dgvDatos.Columns.Clear();
                dtpFechaInicial.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpFechaFinal.Value = DateTime.Now;
                //if ((cmbZonas.Properties.DataSource as IList).Count > 0) cmbZonas.ItemIndex = 0;
                lsQuery = string.Empty;

            }
            
            
        }

        //private DataTable ldtValidaQuery()
        //{
        //    return loLogicaNegocio.goConsultaDataTable(string.Format("EXEC [dbo].[VTA_SP_CUMPLIMIENTO_KILOS_LITROS_X_ZONA_AF] '{0}', '{1}', {2} ", dtpFechaInicial.Value, dtpFechaFinal.Value, cmbZonas.EditValue.ToString()));
        //}

        private DataTable ldtValidaQuery()
        {
            string psZonas = cmbZonas.EditValue.ToString().Replace(" ", "");
            string query = string.Format("EXEC [dbo].[VTA_SP_CUMPLIMIENTO_KILOS_LITROS_X_ZONA_AF] '{0}', '{1}', '{2}'", dtpFechaInicial.Value, dtpFechaFinal.Value, psZonas);
            return loLogicaNegocio.goConsultaDataTable(query);
        }


        private void lBuscar()
        {
            


            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();

            DataTable dt = ldtValidaQuery();

            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;
            dgvDatos.PopulateColumns();

            if (dt.Rows.Count > 0)
            {
                if (liFixedColumn != null)
                {
                    int piCant = liFixedColumn ?? 0;
                    dgvDatos.FixedLineWidth = piCant;

                    for (int x = 0; x <= piCant; x++)
                    {
                        dgvDatos.Columns[x].Fixed = FixedStyle.Left;
                    }
                }


                for (int i = 0; i < dgvDatos.Columns.Count; i++)
                {
                    dgvDatos.Columns[i].OptionsColumn.ReadOnly = true;
                    // Quita numeral del nombre de la columna, sirve para convertir a número
                    if (dgvDatos.Columns[i].CustomizationSearchCaption.Contains("#")) dgvDatos.Columns[i].Caption = dgvDatos.Columns[i].CustomizationSearchCaption.Replace("#", "").Trim();

                    if (i > 1)
                    {
                        if (dt.Columns[i].DataType == typeof(decimal))
                        {
                            var psNameColumn = dgvDatos.Columns[i].FieldName;
                            if (psNameColumn.ToUpper().Contains("%") || psNameColumn.ToUpper().Contains("PORC"))
                            {
                                dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                dgvDatos.Columns[i].DisplayFormat.FormatString = "p2";

                                dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Custom, psNameColumn, "{0:p2}");

                                GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                                item1.FieldName = psNameColumn;
                                item1.SummaryType = SummaryItemType.Custom;
                                item1.DisplayFormat = "{0:p2}";
                                item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                                dgvDatos.GroupSummary.Add(item1);
                            }
                            else if (psNameColumn.ToUpper().Contains("PESO"))
                            {
                                dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                dgvDatos.Columns[i].DisplayFormat.FormatString = "{0:##,##0.0000}";

                            }
                            else if (psNameColumn.ToUpper().Contains("#") || psNameColumn.ToUpper().Contains("CANT") || psNameColumn.ToUpper().Contains("UNI"))
                            {
                                dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                dgvDatos.Columns[i].DisplayFormat.FormatString = "n2";
                                dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:n2}");


                                GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                                item1.FieldName = psNameColumn;
                                item1.SummaryType = SummaryItemType.Sum;
                                item1.DisplayFormat = "{0:n2}";
                                item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                                dgvDatos.GroupSummary.Add(item1);
                            }
                            else
                            {
                                dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                dgvDatos.Columns[i].DisplayFormat.FormatString = "c2";
                                dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");


                                GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                                item1.FieldName = psNameColumn;
                                item1.SummaryType = SummaryItemType.Sum;
                                item1.DisplayFormat = "{0:c2}";
                                item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                                dgvDatos.GroupSummary.Add(item1);
                            }
                        }
                        else if (dt.Columns[i].DataType == typeof(int))
                        {
                            var psNameColumn = dgvDatos.Columns[i].FieldName;

                            if (psNameColumn.ToUpper().Contains("#") || psNameColumn.ToUpper().Contains("CANT") || psNameColumn.ToUpper().Contains("UNI"))
                            {
                                dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                dgvDatos.Columns[i].DisplayFormat.FormatString = "n";
                                dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:#,#}");


                                GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                                item1.FieldName = psNameColumn;
                                item1.SummaryType = SummaryItemType.Sum;
                                item1.DisplayFormat = "{0:#,#}";
                                item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                                dgvDatos.GroupSummary.Add(item1);
                            }
                        }

                    }

                }

            }
            
            dgvDatos.OptionsBehavior.Editable = true;
            dgvDatos.OptionsCustomization.AllowColumnMoving = true;
            dgvDatos.OptionsView.ColumnAutoWidth = false;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            dgvDatos.OptionsView.BestFitMode = GridBestFitMode.Full;
            dgvDatos.BestFitColumns();
        }

       
        


        private void lConsultarBoton()
        {
            try
            {
                //SendKeys.Send("{TAB}");
                Cursor.Current = Cursors.WaitCursor;
                
                if (lbEsValido())
                {
                    lBuscar();
                }
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void frmRpCumplimientoKilosLitros_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lConsultarBoton();
            }
        }

        private void lEnvioAutomatio()
        {
            
            Cursor.Current = Cursors.WaitCursor;

           
            Cursor.Current = Cursors.Default;

           
        }

        private void dgvDatos_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                var item = (e.Item as GridSummaryItem);
                var columna = item.FieldName;
                var fecha = columna.Contains("20") ? columna.Substring(columna.Length - 9, 9) : "";

                // Initialization. 
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    Presupuesto = 0;
                    Ventas = 0;
                }

                // Calculation.
                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    Ventas += Convert.ToDecimal(view.GetRowCellValue(e.RowHandle, string.Format("#Ventas Lts-Kgs{0}", fecha)));
                    Presupuesto += Convert.ToDecimal(view.GetRowCellValue(e.RowHandle, string.Format("#Presupuesto{0}", fecha)));
                }

                // Finalization. 
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    e.TotalValue = Presupuesto > 0 ? Ventas / Presupuesto : 0;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
