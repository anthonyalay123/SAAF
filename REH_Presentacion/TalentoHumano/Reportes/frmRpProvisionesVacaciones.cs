using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using REH_Negocio;
using REH_Presentacion.Formularios;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
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
    public partial class frmRpProvisionesVacaciones : frmBaseTrxDev
    {
        clsNGestorConsulta loLogicaNegocio = new clsNGestorConsulta();
        BindingSource bsDatos = new BindingSource();

        public frmRpProvisionesVacaciones()
        {
            InitializeComponent();
        }

        private void frmRpProvisionesVacacionesCentroCosto_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                dtpFechaInicial.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpFechaFinal.DateTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {

                dgvDatos.PostEditor();
                
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
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Imprimir, Presenta por pantalla Reporte.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (lEsValido())
                {
                    System.Data.DataSet ds = new System.Data.DataSet();
                    string psFechaIni = dtpFechaInicial.DateTime.Date.ToString("dd/MM/yyyy");
                    string psFechaFinal = dtpFechaFinal.DateTime.Date.ToString("dd/MM/yyyy");
                    string psParametro = string.Format("LISTADO DE PROVISIONES DE VACACIONES DEL {0} AL {1}", psFechaIni, psFechaFinal);
                    var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPHISTORICOPROVISIONESCC '{0}','{1}'", psFechaIni, psFechaFinal));
                    dt.TableName = "ProvisionVacacionesCC";
                    ds.Merge(dt);

                    if (dt.Rows.Count > 0)
                    {
                        xrptProvisionesVacaciones xrpt = new xrptProvisionesVacaciones();

                        //Se establece el origen de datos del reporte (El dataset previamente leído)
                        xrpt.Parameters["parameter1"].Value = psParametro;
                        xrpt.DataSource = ds;
                        //Se invoca la ventana que muestra el reporte.
                        xrpt.RequestParameters = false;
                        xrpt.Parameters["parameter1"].Visible = false;

                        using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                        {
                            printTool.ShowRibbonPreviewDialog();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
        }

        /// <summary>
        /// Evento del botón Consultar, Consulta Query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        ///
        private void lBuscar()
        {
            if (lEsValido())
            {
                gcDatos.DataSource = null;
                dgvDatos.Columns.Clear();

                string psFechaIni = dtpFechaInicial.DateTime.Date.ToString("dd/MM/yyyy");
                string psFechaFinal = dtpFechaFinal.DateTime.Date.ToString("dd/MM/yyyy");
                var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPHISTORICOPROVISIONESCC '{0}','{1}'", psFechaIni, psFechaFinal));

                bsDatos.DataSource = dt;
                gcDatos.DataSource = bsDatos;
                dgvDatos.PopulateColumns();

                if (dt.Rows.Count > 0)
                {

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
                dgvDatos.OptionsCustomization.AllowColumnMoving = false;
                dgvDatos.OptionsView.ColumnAutoWidth = false;
                dgvDatos.OptionsView.ShowAutoFilterRow = true;
                dgvDatos.OptionsView.BestFitMode = GridBestFitMode.Full;
                dgvDatos.BestFitColumns();
            }
        }

        private void dtpFechaFinal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData.Equals(Keys.Enter))
                {
                    SendKeys.Send("{TAB}");
                    lBuscar();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool lEsValido()
        {
            if (dtpFechaInicial.DateTime > dtpFechaFinal.DateTime)
            {
                XtraMessageBox.Show("La fecha inicial no pude ser mayor a la fecha final", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            return true;
        }
    }
}
