using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
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
    public partial class frmRpProvisionesAportePatronal : frmBaseTrxDev
    {
        clsNGestorConsulta loLogicaNegocio = new clsNGestorConsulta();
        BindingSource bsDatos = new BindingSource();

        public frmRpProvisionesAportePatronal()
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

                List<Combo> lista = new List<Combo>();
                lista.Add(new Combo() { Codigo = "E", Descripcion = "POR EMPLEADO" });
                lista.Add(new Combo() { Codigo = "C", Descripcion = "POR CENTRO DE COSTO" });
                clsComun.gLLenarCombo(ref cmbTipoReporte, lista);
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
                    DateTime psFechaIni = new DateTime(int.Parse(txtAnio.Text), 1, 1); //dtpFechaInicial.DateTime.Date.ToString("dd/MM/yyyy");
                    DateTime psFechaFinal = new DateTime(int.Parse(txtAnio.Text), int.Parse(txtMes.Text), 1).AddMonths(1).AddDays(-1); //dtpFechaFinal.DateTime.Date.ToString("dd/MM/yyyy");
                    var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPHISTORICOPROVISIONESAP '{0}','{1}','{2}','1'", psFechaIni.ToString("dd/MM/yyyy"), psFechaFinal.ToString("dd/MM/yyyy"), cmbTipoReporte.EditValue.ToString()));
                    dt.TableName = "Provision";
                    ds.Merge(dt);

                    if (dt.Rows.Count > 0)
                    {
                        if (cmbTipoReporte.EditValue.ToString() == "E")
                        {
                            xrptProvisionesAportePatronal xrpt = new xrptProvisionesAportePatronal();

                            //Se establece el origen de datos del reporte (El dataset previamente leído)
                            xrpt.Parameters["parameter1"].Value = string.Format("PROVISIÓN APORTE PATRONAL\n{0} / {1}", clsComun.gsGetMes(psFechaFinal.Month), psFechaFinal.Year); ;
                            xrpt.Parameters["cab1"].Value = string.Format("Prov. Pagada {0}/{1}", clsComun.gsGetMes(psFechaFinal.AddMonths(-1).Month).ToLower(), psFechaFinal.AddMonths(-1).Year);
                            xrpt.Parameters["cab2"].Value = string.Format("Provisión {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt.Parameters["cab3"].Value = string.Format("Saldo Acum. {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt.DataSource = ds;
                            //Se invoca la ventana que muestra el reporte.
                            xrpt.RequestParameters = false;
                            xrpt.Parameters["parameter1"].Visible = false;
                            xrpt.Parameters["cab1"].Visible = false;
                            xrpt.Parameters["cab2"].Visible = false;
                            xrpt.Parameters["cab3"].Visible = false;

                            using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                            {
                                printTool.ShowRibbonPreviewDialog();
                            }
                        }
                        else
                        {
                            xrptProvisionesAportePatronalCC xrpt = new xrptProvisionesAportePatronalCC();

                            //Se establece el origen de datos del reporte (El dataset previamente leído)
                            xrpt.Parameters["parameter1"].Value = string.Format("PROVISIÓN APORTE PATRONAL\n{0} / {1}", clsComun.gsGetMes(psFechaFinal.Month), psFechaFinal.Year); ;
                            xrpt.Parameters["cab1"].Value = string.Format("Prov. Pagada {0}/{1}", clsComun.gsGetMes(psFechaFinal.AddMonths(-1).Month).ToLower(), psFechaFinal.AddMonths(-1).Year);
                            xrpt.Parameters["cab2"].Value = string.Format("Provisión {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt.Parameters["cab3"].Value = string.Format("Saldo Acum. {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt.DataSource = ds;
                            //Se invoca la ventana que muestra el reporte.
                            xrpt.RequestParameters = false;
                            xrpt.Parameters["parameter1"].Visible = false;
                            xrpt.Parameters["cab1"].Visible = false;
                            xrpt.Parameters["cab2"].Visible = false;
                            xrpt.Parameters["cab3"].Visible = false;

                            using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                            {
                                printTool.ShowRibbonPreviewDialog();
                            }
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

                DateTime psFechaIni = new DateTime(int.Parse(txtAnio.Text), 1, 1); //dtpFechaInicial.DateTime.Date.ToString("dd/MM/yyyy");
                DateTime psFechaFinal = new DateTime(int.Parse(txtAnio.Text), int.Parse(txtMes.Text), 1).AddMonths(1).AddDays(-1); //dtpFechaFinal.DateTime.Date.ToString("dd/MM/yyyy");
                var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPHISTORICOPROVISIONESAP '{0}','{1}','{2}','0'", psFechaIni.ToString("dd/MM/yyyy"), psFechaFinal.ToString("dd/MM/yyyy"), cmbTipoReporte.EditValue.ToString()));

                dgvDatos.Columns.Clear();
                dgvDatos.Bands.Clear();
                if (dt.Rows.Count > 0)
                {
                    GridBand gridBandIni = new GridBand();
                    dgvDatos.Bands.Add(gridBandIni);

                    bsDatos.DataSource = dt;
                    gcDatos.DataSource = bsDatos;
                    DateTime pdFechaCalculo = psFechaIni;

                    int acum = 0;
                    //dgvDatos.Columns[1].Visible = false;
                    //gridBandIni.Columns.Add(dgvDatos.Columns[0]); // Tipo

                    if (cmbTipoReporte.EditValue.ToString() == "E")
                    {
                        gridBandIni.Columns.Add(dgvDatos.Columns[0]); // Empleado
                        gridBandIni.Columns.Add(dgvDatos.Columns[1]); // Centro Costo
                        acum = 2;
                    }
                    else
                    {
                        gridBandIni.Columns.Add(dgvDatos.Columns[0]); // Centro Costo
                        acum = 1;
                    }

                    dgvDatos.FixedLineWidth = 2;
                    dgvDatos.Bands[0].Fixed = FixedStyle.Left;

                    while (psFechaFinal > pdFechaCalculo)
                    {
                        GridBand gridBandDin = new GridBand();
                        gridBandDin.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                        gridBandDin.AppearanceHeader.Options.UseFont = true;
                        Font fontDin = new Font(gridBandDin.AppearanceHeader.Font, FontStyle.Bold);
                        gridBandDin.AppearanceHeader.Font = fontDin;

                        gridBandDin.AppearanceHeader.Options.UseTextOptions = true;
                        gridBandDin.Caption = clsComun.gsGetMes(pdFechaCalculo);

                        dgvDatos.Columns[acum].Caption = dgvDatos.Columns[acum].FieldName.Substring(0, dgvDatos.Columns[acum].FieldName.Length - 9);
                        dgvDatos.Columns[acum + 1].Caption = dgvDatos.Columns[acum + 1].FieldName.Substring(0, dgvDatos.Columns[acum + 1].FieldName.Length - 9);
                        //dgvDatos.Columns[acum + 2].Caption = dgvDatos.Columns[acum + 2].FieldName.Substring(0, dgvDatos.Columns[acum + 2].FieldName.Length - 9);
                        //dgvDatos.Columns[acum + 3].Caption = dgvDatos.Columns[acum + 3].FieldName.Substring(0, dgvDatos.Columns[acum + 3].FieldName.Length - 9);

                        gridBandDin.Columns.Add(dgvDatos.Columns[acum]);
                        gridBandDin.Columns.Add(dgvDatos.Columns[acum + 1]);
                        //gridBandDin.Columns.Add(dgvDatos.Columns[acum + 2]);
                        //gridBandDin.Columns.Add(dgvDatos.Columns[acum + 3]);

                        acum = acum + 2;
                        pdFechaCalculo = pdFechaCalculo.AddMonths(1);

                        dgvDatos.Bands.Add(gridBandDin);
                    }

                    GridBand gridBandTot = new GridBand();
                    gridBandTot.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    gridBandTot.AppearanceHeader.Options.UseTextOptions = true;
                    gridBandTot.AppearanceHeader.Options.UseFont = true;
                    Font fontTot = new Font(gridBandTot.AppearanceHeader.Font, FontStyle.Bold);
                    gridBandTot.AppearanceHeader.Font = fontTot;

                    gridBandTot.Caption = "TOTAL";

                    gridBandTot.Columns.Add(dgvDatos.Columns[acum]);
                    gridBandTot.Columns.Add(dgvDatos.Columns[acum + 1]);
                    //gridBandTot.Columns.Add(dgvDatos.Columns[acum + 2]);
                    //gridBandTot.Columns.Add(dgvDatos.Columns[acum + 3]);
                    //gridBandTot.Columns.Add(dgvDatos.Columns[acum + 4]);

                    dgvDatos.Bands.Add(gridBandTot);

                    clsComun.gFormatearColumnasGrid(dgvDatos);
                    dgvDatos.BestFitColumns();
                }
            }
        }

        private bool lEsValido()
        {
            if (string.IsNullOrEmpty(txtAnio.Text))
            {
                XtraMessageBox.Show("Ingrese el año", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtMes.Text))
            {
                XtraMessageBox.Show("Ingrese el mes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
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
    }
}
