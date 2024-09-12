using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using GEN_Entidad;
using REH_Negocio;
using REH_Presentacion.Formularios;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.TalentoHumano.Reportes
{
    public partial class frmRpProvisionesDecimoCuarto : frmBaseTrxDev
    {
        clsNGestorConsulta loLogicaNegocio = new clsNGestorConsulta();
        PrinterSettings prnSettings;
        BindingSource bsCosta = new BindingSource();
        BindingSource bsSierra = new BindingSource();
        BindingSource bsConsolidado = new BindingSource();
        BindingSource bsAcumulado = new BindingSource();

        public frmRpProvisionesDecimoCuarto()
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
                //lista.Add(new Combo() { Codigo = "C", Descripcion = "POR CENTRO DE COSTO" });
                clsComun.gLLenarCombo(ref cmbTipoReporte, lista);

                if (clsPrincipal.gIdPerfil != 1)
                {
                    xtraTabControl1.TabPages[0].PageVisible = false;
                }

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
                dgvAcumulado.PostEditor();
                DataTable poLista = (DataTable)bsAcumulado.DataSource;
                if (poLista != null && poLista.Rows.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcAcumulado, "DecimoCuartoAcumulado.xlsx", psFilter);
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (clsPrincipal.gIdPerfil == 1)
                {
                    dgvConsolidado.PostEditor();
                    poLista = (DataTable)bsConsolidado.DataSource;
                    if (poLista != null && poLista.Rows.Count > 0)
                    {
                        string psFilter = "Files(*.xlsx;)|*.xlsx;";
                        clsComun.gSaveFile(gcConsolidado, "DecimoCuartoConsolidado.xlsx", psFilter);
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                dgvCosta.PostEditor();
                poLista = (DataTable)bsCosta.DataSource;
                if (poLista != null && poLista.Rows.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcCosta, "DecimoCosta.xlsx", psFilter);
                }

                dgvSierra.PostEditor();
                poLista = (DataTable)bsSierra.DataSource;
                if (poLista != null && poLista.Rows.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcSierra, "DecimoSierra.xlsx", psFilter);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintingSystem_StartPrint(object sender, PrintDocumentEventArgs e)
        {
            prnSettings = e.PrintDocument.PrinterSettings;
        }

        private void reportsStartPrintEventHandler(object sender, PrintDocumentEventArgs e)
        {
            int pageCount = e.PrintDocument.PrinterSettings.ToPage;
            e.PrintDocument.PrinterSettings = prnSettings;

            // The following line is required if the number of pages for each report varies, 
            // and you consistently need to print all pages.
            e.PrintDocument.PrinterSettings.ToPage = pageCount;
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
                    DateTime psFechaIni = new DateTime(int.Parse(txtAnio.Text), 1, 1); //dtpFechaInicial.DateTime.Date.ToString("dd/MM/yyyy");
                    DateTime psFechaFinal = new DateTime(int.Parse(txtAnio.Text), int.Parse(txtMes.Text), 1).AddMonths(1).AddDays(-1); //dtpFechaFinal.DateTime.Date.ToString("dd/MM/yyyy");
                    var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPHISTORICOPROVISIONESDC '{0}','{1}',NULL,'{2}','1'", psFechaIni.ToString("dd/MM/yyyy"), psFechaFinal.ToString("dd/MM/yyyy"), cmbTipoReporte.EditValue.ToString()));

                    if (dt.Rows.Count > 0)
                    {
                        var poPeriodoCosta = loLogicaNegocio.goConsultarPeriodoD4toCosta(psFechaFinal);
                        var poPeriodoSierra = loLogicaNegocio.goConsultarPeriodoD4toSierra(psFechaFinal);

                        if (cmbTipoReporte.EditValue.ToString() == "E")
                        {
                            /********************************************************************************************************************************************/
                            dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPHISTORICOPROVISIONESDC '{0}','{1}','COSTA','{2}','1'", poPeriodoCosta.FechaInicio.ToString("dd/MM/yyyy"), psFechaFinal.ToString("dd/MM/yyyy"), cmbTipoReporte.EditValue.ToString()));
                            System.Data.DataSet ds = new System.Data.DataSet();
                            dt.TableName = "Provision";
                            ds.Merge(dt);

                            xrptProvisionesDecimoCuarto xrpt = new xrptProvisionesDecimoCuarto();
                            xrptProvisionesDecimo4to xrpt4to = new xrptProvisionesDecimo4to();

                            xrpt.Parameters["parameter1"].Value = string.Format("PROVISIÓN DÉCIMO CUARTO COSTA\n{0} / {1}", clsComun.gsGetMes(psFechaFinal.Month), psFechaFinal.Year); ;
                            xrpt.Parameters["cab1"].Value = string.Format("Prov. Acum. {0}/{1}", clsComun.gsGetMes(psFechaFinal.AddMonths(-1).Month).ToLower(), psFechaFinal.AddMonths(-1).Year);
                            xrpt.Parameters["cab2"].Value = string.Format("Provisión {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt.Parameters["cab3"].Value = string.Format("Saldo Acum. {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt.DataSource = ds;
                            //Se invoca la ventana que muestra el reporte.
                            xrpt.RequestParameters = false;
                            xrpt.Parameters["parameter1"].Visible = false;
                            xrpt.Parameters["cab1"].Visible = false;
                            xrpt.Parameters["cab2"].Visible = false;
                            xrpt.Parameters["cab3"].Visible = false;

                            xrpt4to.Parameters["parameter1"].Value = string.Format("PROVISIÓN DÉCIMO CUARTO COSTA\n{0} / {1}", clsComun.gsGetMes(psFechaFinal.Month), psFechaFinal.Year); ;
                            xrpt4to.Parameters["cab1"].Value = string.Format("Prov. Acum. {0}/{1}", clsComun.gsGetMes(psFechaFinal.AddMonths(-1).Month).ToLower(), psFechaFinal.AddMonths(-1).Year);
                            xrpt4to.Parameters["cab2"].Value = string.Format("Provisión {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt4to.Parameters["cab3"].Value = string.Format("Saldo Acum. {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt4to.DataSource = ds;
                            //Se invoca la ventana que muestra el reporte.
                            xrpt4to.RequestParameters = false;
                            xrpt4to.Parameters["parameter1"].Visible = false;
                            xrpt4to.Parameters["cab1"].Visible = false;
                            xrpt4to.Parameters["cab2"].Visible = false;
                            xrpt4to.Parameters["cab3"].Visible = false;

                            /********************************************************************************************************************************************/
                            dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPHISTORICOPROVISIONESDC '{0}','{1}','SIERRA','{2}','1'", poPeriodoSierra.FechaInicio.ToString("dd/MM/yyyy"), psFechaFinal.ToString("dd/MM/yyyy"), cmbTipoReporte.EditValue.ToString()));
                            ds = new System.Data.DataSet();
                            dt.TableName = "Provision";
                            ds.Merge(dt);

                            xrptProvisionesDecimoCuarto xrptSierra = new xrptProvisionesDecimoCuarto();
                            xrptProvisionesDecimo4to xrpt4toSierra = new xrptProvisionesDecimo4to();

                            xrptSierra.Parameters["parameter1"].Value = string.Format("PROVISIÓN DÉCIMO CUARTO SIERRA\n{0} / {1}", clsComun.gsGetMes(psFechaFinal.Month), psFechaFinal.Year); ;
                            xrptSierra.Parameters["cab1"].Value = string.Format("Prov. Acum. {0}/{1}", clsComun.gsGetMes(psFechaFinal.AddMonths(-1).Month).ToLower(), psFechaFinal.AddMonths(-1).Year);
                            xrptSierra.Parameters["cab2"].Value = string.Format("Provisión {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrptSierra.Parameters["cab3"].Value = string.Format("Saldo Acum. {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrptSierra.DataSource = ds;
                            //Se invoca la ventana que muestra el reporte.
                            xrptSierra.RequestParameters = false;
                            xrptSierra.Parameters["parameter1"].Visible = false;
                            xrptSierra.Parameters["cab1"].Visible = false;
                            xrptSierra.Parameters["cab2"].Visible = false;
                            xrptSierra.Parameters["cab3"].Visible = false;

                            xrpt4toSierra.Parameters["parameter1"].Value = string.Format("PROVISIÓN DÉCIMO CUARTO SIERRA\n{0} / {1}", clsComun.gsGetMes(psFechaFinal.Month), psFechaFinal.Year); ;
                            xrpt4toSierra.Parameters["cab1"].Value = string.Format("Prov. Acum. {0}/{1}", clsComun.gsGetMes(psFechaFinal.AddMonths(-1).Month).ToLower(), psFechaFinal.AddMonths(-1).Year);
                            xrpt4toSierra.Parameters["cab2"].Value = string.Format("Provisión {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt4toSierra.Parameters["cab3"].Value = string.Format("Saldo Acum. {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt4toSierra.DataSource = ds;
                            //Se invoca la ventana que muestra el reporte.
                            xrpt4toSierra.RequestParameters = false;
                            xrpt4toSierra.Parameters["parameter1"].Visible = false;
                            xrpt4toSierra.Parameters["cab1"].Visible = false;
                            xrpt4toSierra.Parameters["cab2"].Visible = false;
                            xrpt4toSierra.Parameters["cab3"].Visible = false;


                            XtraReport[] reports;

                            if (poPeriodoCosta.FechaFin.Month == psFechaFinal.Month)
                            {
                                reports = new XtraReport[] { xrpt4to, xrptSierra };
                            }
                            else if (poPeriodoSierra.FechaFin.Month == psFechaFinal.Month)
                            {
                                reports = new XtraReport[] { xrpt, xrpt4toSierra };
                            }
                            else
                            {
                                reports = new XtraReport[] { xrpt, xrptSierra };
                            }

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
                            /********************************************************************************************************************************************/
                            dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPHISTORICOPROVISIONESDC '{0}','{1}','COSTA','{2}','1'", poPeriodoCosta.FechaInicio.ToString("dd/MM/yyyy"), psFechaFinal.ToString("dd/MM/yyyy"), cmbTipoReporte.EditValue.ToString()));
                            System.Data.DataSet ds = new System.Data.DataSet();
                            dt.TableName = "Provision";
                            ds.Merge(dt);

                            xrptProvisionesDecimoCuartoCC xrpt = new xrptProvisionesDecimoCuartoCC();
                            xrptProvisionesDecimo4toCC xrpt4to = new xrptProvisionesDecimo4toCC();

                            xrpt.Parameters["parameter1"].Value = string.Format("PROVISIÓN DÉCIMO CUARTO COSTA\n{0} / {1}", clsComun.gsGetMes(psFechaFinal.Month), psFechaFinal.Year); ;
                            xrpt.Parameters["cab1"].Value = string.Format("Prov. Acum. {0}/{1}", clsComun.gsGetMes(psFechaFinal.AddMonths(-1).Month).ToLower(), psFechaFinal.AddMonths(-1).Year);
                            xrpt.Parameters["cab2"].Value = string.Format("Provisión {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt.Parameters["cab3"].Value = string.Format("Saldo Acum. {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt.DataSource = ds;
                            //Se invoca la ventana que muestra el reporte.
                            xrpt.RequestParameters = false;
                            xrpt.Parameters["parameter1"].Visible = false;
                            xrpt.Parameters["cab1"].Visible = false;
                            xrpt.Parameters["cab2"].Visible = false;
                            xrpt.Parameters["cab3"].Visible = false;

                            xrpt4to.Parameters["parameter1"].Value = string.Format("PROVISIÓN DÉCIMO CUARTO COSTA\n{0} / {1}", clsComun.gsGetMes(psFechaFinal.Month), psFechaFinal.Year); ;
                            xrpt4to.Parameters["cab1"].Value = string.Format("Prov. Acum. {0}/{1}", clsComun.gsGetMes(psFechaFinal.AddMonths(-1).Month).ToLower(), psFechaFinal.AddMonths(-1).Year);
                            xrpt4to.Parameters["cab2"].Value = string.Format("Provisión {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt4to.Parameters["cab3"].Value = string.Format("Saldo Acum. {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt4to.DataSource = ds;
                            //Se invoca la ventana que muestra el reporte.
                            xrpt4to.RequestParameters = false;
                            xrpt4to.Parameters["parameter1"].Visible = false;
                            xrpt4to.Parameters["cab1"].Visible = false;
                            xrpt4to.Parameters["cab2"].Visible = false;
                            xrpt4to.Parameters["cab3"].Visible = false;

                            /********************************************************************************************************************************************/
                            dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPHISTORICOPROVISIONESDC '{0}','{1}','SIERRA','{2}','1'", poPeriodoSierra.FechaInicio.ToString("dd/MM/yyyy"), psFechaFinal.ToString("dd/MM/yyyy"), cmbTipoReporte.EditValue.ToString()));
                            ds = new System.Data.DataSet();
                            dt.TableName = "Provision";
                            ds.Merge(dt);

                            xrptProvisionesDecimoCuartoCC xrptSierra = new xrptProvisionesDecimoCuartoCC();
                            xrptProvisionesDecimo4toCC xrpt4toSierra = new xrptProvisionesDecimo4toCC();

                            xrptSierra.Parameters["parameter1"].Value = string.Format("PROVISIÓN DÉCIMO CUARTO SIERRA\n{0} / {1}", clsComun.gsGetMes(psFechaFinal.Month), psFechaFinal.Year); ;
                            xrptSierra.Parameters["cab1"].Value = string.Format("Prov. Acum. {0}/{1}", clsComun.gsGetMes(psFechaFinal.AddMonths(-1).Month).ToLower(), psFechaFinal.AddMonths(-1).Year);
                            xrptSierra.Parameters["cab2"].Value = string.Format("Provisión {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrptSierra.Parameters["cab3"].Value = string.Format("Saldo Acum. {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrptSierra.DataSource = ds;
                            //Se invoca la ventana que muestra el reporte.
                            xrptSierra.RequestParameters = false;
                            xrptSierra.Parameters["parameter1"].Visible = false;
                            xrptSierra.Parameters["cab1"].Visible = false;
                            xrptSierra.Parameters["cab2"].Visible = false;
                            xrptSierra.Parameters["cab3"].Visible = false;

                            xrpt4toSierra.Parameters["parameter1"].Value = string.Format("PROVISIÓN DÉCIMO CUARTO SIERRA\n{0} / {1}", clsComun.gsGetMes(psFechaFinal.Month), psFechaFinal.Year); ;
                            xrpt4toSierra.Parameters["cab1"].Value = string.Format("Prov. Acum. {0}/{1}", clsComun.gsGetMes(psFechaFinal.AddMonths(-1).Month).ToLower(), psFechaFinal.AddMonths(-1).Year);
                            xrpt4toSierra.Parameters["cab2"].Value = string.Format("Provisión {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt4toSierra.Parameters["cab3"].Value = string.Format("Saldo Acum. {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                            xrpt4toSierra.DataSource = ds;
                            //Se invoca la ventana que muestra el reporte.
                            xrpt4toSierra.RequestParameters = false;
                            xrpt4toSierra.Parameters["parameter1"].Visible = false;
                            xrpt4toSierra.Parameters["cab1"].Visible = false;
                            xrpt4toSierra.Parameters["cab2"].Visible = false;
                            xrpt4toSierra.Parameters["cab3"].Visible = false;

                            XtraReport[] reports;

                            if (poPeriodoCosta.FechaFin.Month == psFechaFinal.Month)
                            {
                                reports = new XtraReport[] { xrpt4to, xrptSierra };
                            }
                            else if (poPeriodoSierra.FechaFin.Month == psFechaFinal.Month)
                            {
                                reports = new XtraReport[] { xrpt, xrpt4toSierra };
                            }
                            else
                            {
                                reports = new XtraReport[] { xrpt, xrptSierra };
                            }

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

            txtAnio.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtMes.KeyPress += new KeyPressEventHandler(SoloNumeros);
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
                DateTime psFechaIni = new DateTime(int.Parse(txtAnio.Text), 1, 1); //dtpFechaInicial.DateTime.Date.ToString("dd/MM/yyyy");
                DateTime psFechaFinal = new DateTime(int.Parse(txtAnio.Text), int.Parse(txtMes.Text), 1).AddMonths(1).AddDays(-1); //dtpFechaFinal.DateTime.Date.ToString("dd/MM/yyyy");

                var poPeriodoCosta = loLogicaNegocio.goConsultarPeriodoD4toCosta(psFechaFinal);
                var poPeriodoSierra = loLogicaNegocio.goConsultarPeriodoD4toSierra(psFechaFinal);

                if (poPeriodoCosta == null)
                {
                    XtraMessageBox.Show("No existe creado el periodo décimo cuarto región costa, crearlo para poder consultar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (poPeriodoSierra == null)
                {
                    XtraMessageBox.Show("No existe creado el periodo décimo cuarto región sierra, crearlo para poder consultar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPHISTORICOPROVISIONESDC '{0}','{1}',NULL,'{2}','0','1'", psFechaIni.ToString("dd/MM/yyyy"), psFechaFinal.ToString("dd/MM/yyyy"), cmbTipoReporte.EditValue.ToString()));
                /*********************************************************************************************************************************/
                dgvAcumulado.Columns.Clear();
                dgvAcumulado.Bands.Clear();
                if (dt.Rows.Count > 0)
                {
                    GridBand gridBandIni = new GridBand();
                    dgvAcumulado.Bands.Add(gridBandIni);

                    bsAcumulado.DataSource = dt;
                    gcAcumulado.DataSource = bsAcumulado;
                    DateTime pdFechaCalculo = poPeriodoCosta.FechaInicio < poPeriodoSierra.FechaInicio ? poPeriodoCosta.FechaInicio : poPeriodoSierra.FechaInicio;

                    
                    gridBandIni.Columns.Add(dgvAcumulado.Columns[0]); // IdEmpleadoContrato
                    dgvAcumulado.Columns[0].Visible = false;
                    gridBandIni.Columns.Add(dgvAcumulado.Columns[1]); // Tipo
                    
                    int acum = 4;

                    if (cmbTipoReporte.EditValue.ToString() == "E")
                    {
                        gridBandIni.Columns.Add(dgvAcumulado.Columns[2]); // Empleado
                        gridBandIni.Columns.Add(dgvAcumulado.Columns[3]); // Region
                        gridBandIni.Columns.Add(dgvAcumulado.Columns[4]); // Centro Costo
                        gridBandIni.Columns.Add(dgvAcumulado.Columns[5]); // Provision Anterior
                        acum = 6;
                    }
                    else
                    {
                        gridBandIni.Columns.Add(dgvAcumulado.Columns[2]); // Region
                        gridBandIni.Columns.Add(dgvAcumulado.Columns[3]); // Centro Costo
                        gridBandIni.Columns.Add(dgvAcumulado.Columns[4]); // Provision Anterior
                        acum = 5;
                    }

                    dgvAcumulado.FixedLineWidth = 2;
                    dgvAcumulado.Bands[0].Fixed = FixedStyle.Left;


                    while (psFechaFinal > pdFechaCalculo)
                    {
                        GridBand gridBandDin = new GridBand();
                        gridBandDin.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                        gridBandDin.AppearanceHeader.Options.UseFont = true;
                        Font fontDin = new Font(gridBandDin.AppearanceHeader.Font, FontStyle.Bold);
                        gridBandDin.AppearanceHeader.Font = fontDin;

                        gridBandDin.AppearanceHeader.Options.UseTextOptions = true;
                        gridBandDin.Caption = clsComun.gsGetMes(pdFechaCalculo);

                        dgvAcumulado.Columns[acum].Caption = dgvAcumulado.Columns[acum].FieldName.Substring(0, dgvAcumulado.Columns[acum].FieldName.Length - 9);
                        dgvAcumulado.Columns[acum + 1].Caption = dgvAcumulado.Columns[acum + 1].FieldName.Substring(0, dgvAcumulado.Columns[acum + 1].FieldName.Length - 9);
                        dgvAcumulado.Columns[acum + 2].Caption = dgvAcumulado.Columns[acum + 2].FieldName.Substring(0, dgvAcumulado.Columns[acum + 2].FieldName.Length - 9);
                        dgvAcumulado.Columns[acum + 3].Caption = dgvAcumulado.Columns[acum + 3].FieldName.Substring(0, dgvAcumulado.Columns[acum + 3].FieldName.Length - 9);

                        gridBandDin.Columns.Add(dgvAcumulado.Columns[acum]);
                        gridBandDin.Columns.Add(dgvAcumulado.Columns[acum + 1]);
                        gridBandDin.Columns.Add(dgvAcumulado.Columns[acum + 2]);
                        gridBandDin.Columns.Add(dgvAcumulado.Columns[acum + 3]);

                        acum = acum + 4;
                        pdFechaCalculo = pdFechaCalculo.AddMonths(1);

                        dgvAcumulado.Bands.Add(gridBandDin);
                    }

                    GridBand gridBandTot = new GridBand();
                    gridBandTot.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    gridBandTot.AppearanceHeader.Options.UseTextOptions = true;
                    gridBandTot.AppearanceHeader.Options.UseFont = true;
                    Font fontTot = new Font(gridBandTot.AppearanceHeader.Font, FontStyle.Bold);
                    gridBandTot.AppearanceHeader.Font = fontTot;

                    gridBandTot.Caption = "TOTAL";

                    gridBandTot.Columns.Add(dgvAcumulado.Columns[acum]);
                    gridBandTot.Columns.Add(dgvAcumulado.Columns[acum + 1]);
                    gridBandTot.Columns.Add(dgvAcumulado.Columns[acum + 2]);
                    gridBandTot.Columns.Add(dgvAcumulado.Columns[acum + 3]);
                    //gridBandTot.Columns.Add(dgvAcumulado.Columns[acum + 4]);

                    dgvAcumulado.Bands.Add(gridBandTot);

                    clsComun.gFormatearColumnasGrid(dgvAcumulado);
                    dgvAcumulado.BestFitColumns();

                }

                /*********************************************************************************************************************************/
                dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPHISTORICOPROVISIONESDC '{0}','{1}',NULL,'{2}','0'", psFechaIni.ToString("dd/MM/yyyy"), psFechaFinal.ToString("dd/MM/yyyy"), cmbTipoReporte.EditValue.ToString()));
                /*********************************************************************************************************************************/
                dgvConsolidado.Columns.Clear();
                dgvConsolidado.Bands.Clear();
                if (dt.Rows.Count > 0)
                {
                    GridBand gridBandIni = new GridBand();
                    dgvConsolidado.Bands.Add(gridBandIni);

                    bsConsolidado.DataSource = dt;
                    gcConsolidado.DataSource = bsConsolidado;
                    DateTime pdFechaCalculo = psFechaIni;

                    gridBandIni.Columns.Add(dgvConsolidado.Columns[0]); // Id Empleado Contrato                    
                    dgvConsolidado.Columns[0].Visible = false;
                    gridBandIni.Columns.Add(dgvConsolidado.Columns[1]); // Tipo                    
                    int acum = 0;

                    if (cmbTipoReporte.EditValue.ToString() == "E")
                    {
                        gridBandIni.Columns.Add(dgvConsolidado.Columns[2]); // Empleado
                        gridBandIni.Columns.Add(dgvConsolidado.Columns[3]); // Region
                        gridBandIni.Columns.Add(dgvConsolidado.Columns[4]); // Centro Costo
                        gridBandIni.Columns.Add(dgvConsolidado.Columns[5]); // Provision Anterior
                        acum = 6;
                    }
                    else
                    {
                        gridBandIni.Columns.Add(dgvConsolidado.Columns[2]); // Region
                        gridBandIni.Columns.Add(dgvConsolidado.Columns[3]); // Centro Costo
                        gridBandIni.Columns.Add(dgvConsolidado.Columns[4]); // Provision Anterior
                        acum = 5;
                    }

                    dgvConsolidado.FixedLineWidth = 2;
                    dgvConsolidado.Bands[0].Fixed = FixedStyle.Left;


                    while (psFechaFinal > pdFechaCalculo)
                    {
                        GridBand gridBandDin = new GridBand();
                        gridBandDin.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                        gridBandDin.AppearanceHeader.Options.UseFont = true;
                        Font fontDin = new Font(gridBandDin.AppearanceHeader.Font, FontStyle.Bold);
                        gridBandDin.AppearanceHeader.Font = fontDin;

                        gridBandDin.AppearanceHeader.Options.UseTextOptions = true;
                        gridBandDin.Caption = clsComun.gsGetMes(pdFechaCalculo);

                        dgvConsolidado.Columns[acum].Caption = dgvConsolidado.Columns[acum].FieldName.Substring(0, dgvConsolidado.Columns[acum].FieldName.Length - 9);
                        dgvConsolidado.Columns[acum + 1].Caption = dgvConsolidado.Columns[acum + 1].FieldName.Substring(0, dgvConsolidado.Columns[acum + 1].FieldName.Length - 9);
                        dgvConsolidado.Columns[acum + 2].Caption = dgvConsolidado.Columns[acum + 2].FieldName.Substring(0, dgvConsolidado.Columns[acum + 2].FieldName.Length - 9);
                        dgvConsolidado.Columns[acum + 3].Caption = dgvConsolidado.Columns[acum + 3].FieldName.Substring(0, dgvConsolidado.Columns[acum + 3].FieldName.Length - 9);

                        gridBandDin.Columns.Add(dgvConsolidado.Columns[acum]);
                        gridBandDin.Columns.Add(dgvConsolidado.Columns[acum + 1]);
                        gridBandDin.Columns.Add(dgvConsolidado.Columns[acum + 2]);
                        gridBandDin.Columns.Add(dgvConsolidado.Columns[acum + 3]);

                        acum = acum + 4;
                        pdFechaCalculo = pdFechaCalculo.AddMonths(1);

                        dgvConsolidado.Bands.Add(gridBandDin);
                    }

                    GridBand gridBandTot = new GridBand();
                    gridBandTot.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    gridBandTot.AppearanceHeader.Options.UseTextOptions = true;
                    gridBandTot.AppearanceHeader.Options.UseFont = true;
                    Font fontTot = new Font(gridBandTot.AppearanceHeader.Font, FontStyle.Bold);
                    gridBandTot.AppearanceHeader.Font = fontTot;

                    gridBandTot.Caption = "TOTAL";

                    gridBandTot.Columns.Add(dgvConsolidado.Columns[acum]);
                    gridBandTot.Columns.Add(dgvConsolidado.Columns[acum + 1]);
                    gridBandTot.Columns.Add(dgvConsolidado.Columns[acum + 2]);
                    gridBandTot.Columns.Add(dgvConsolidado.Columns[acum + 3]);
                    //gridBandTot.Columns.Add(dgvConsolidado.Columns[acum + 4]);

                    dgvConsolidado.Bands.Add(gridBandTot);

                    clsComun.gFormatearColumnasGrid(dgvConsolidado);
                    dgvConsolidado.BestFitColumns();

                }
                /*********************************************************************************************************************************/

                if (poPeriodoCosta != null)
                {
                    dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPHISTORICOPROVISIONESDC '{0}','{1}','COSTA','{2}','0'", poPeriodoCosta.FechaInicio.ToString("dd/MM/yyyy"), psFechaFinal.ToString("dd/MM/yyyy"), cmbTipoReporte.EditValue.ToString()));
                    dgvCosta.Columns.Clear();
                    dgvCosta.Bands.Clear();
                    if (dt.Rows.Count > 0)
                    {
                        GridBand gridBandIni = new GridBand();
                        dgvCosta.Bands.Add(gridBandIni);

                        bsCosta.DataSource = dt;
                        gcCosta.DataSource = bsCosta;
                        DateTime pdFechaCalculo = poPeriodoCosta.FechaInicio;

                        dgvCosta.Columns[0].Visible = false;

                        gridBandIni.Columns.Add(dgvCosta.Columns[0]); // IdEmpleadoContrato
                        gridBandIni.Columns.Add(dgvCosta.Columns[1]); // Tipo
                        int acum = 0;

                        if (cmbTipoReporte.EditValue.ToString() == "E")
                        {
                            gridBandIni.Columns.Add(dgvCosta.Columns[2]); // Empleado
                            gridBandIni.Columns.Add(dgvCosta.Columns[3]); // Region
                            dgvCosta.Columns[3].Visible = false;
                            gridBandIni.Columns.Add(dgvCosta.Columns[4]); // Centro Costo
                            gridBandIni.Columns.Add(dgvCosta.Columns[5]); // Provision Anterior
                            acum = 6;
                        }
                        else
                        {
                            gridBandIni.Columns.Add(dgvCosta.Columns[2]); // Region
                            dgvCosta.Columns[2].Visible = false;
                            gridBandIni.Columns.Add(dgvCosta.Columns[3]); // Centro Costo
                            gridBandIni.Columns.Add(dgvCosta.Columns[4]);  //Provision Anterior
                            acum = 5;
                        }

                        dgvCosta.FixedLineWidth = 2;
                        dgvCosta.Bands[0].Fixed = FixedStyle.Left;


                        while (psFechaFinal > pdFechaCalculo)
                        {
                            GridBand gridBandDin = new GridBand();
                            gridBandDin.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                            gridBandDin.AppearanceHeader.Options.UseTextOptions = true;
                            gridBandDin.AppearanceHeader.Options.UseFont = true;
                            Font fontDin = new Font(gridBandDin.AppearanceHeader.Font, FontStyle.Bold);
                            gridBandDin.AppearanceHeader.Font = fontDin;
                            gridBandDin.Caption = clsComun.gsGetMes(pdFechaCalculo);

                            dgvCosta.Columns[acum].Caption = dgvCosta.Columns[acum].FieldName.Substring(0, dgvCosta.Columns[acum].FieldName.Length - 9);
                            dgvCosta.Columns[acum + 1].Caption = dgvCosta.Columns[acum + 1].FieldName.Substring(0, dgvCosta.Columns[acum + 1].FieldName.Length - 9);
                            dgvCosta.Columns[acum + 2].Caption = dgvCosta.Columns[acum + 2].FieldName.Substring(0, dgvCosta.Columns[acum + 2].FieldName.Length - 9);
                            dgvCosta.Columns[acum + 3].Caption = dgvCosta.Columns[acum + 3].FieldName.Substring(0, dgvCosta.Columns[acum + 3].FieldName.Length - 9);

                            gridBandDin.Columns.Add(dgvCosta.Columns[acum]);
                            gridBandDin.Columns.Add(dgvCosta.Columns[acum + 1]);
                            gridBandDin.Columns.Add(dgvCosta.Columns[acum + 2]);
                            gridBandDin.Columns.Add(dgvCosta.Columns[acum + 3]);

                            acum = acum + 4;
                            pdFechaCalculo = pdFechaCalculo.AddMonths(1);

                            dgvCosta.Bands.Add(gridBandDin);
                        }

                        GridBand gridBandTot = new GridBand();
                        gridBandTot.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                        gridBandTot.AppearanceHeader.Options.UseTextOptions = true;
                        gridBandTot.AppearanceHeader.Options.UseFont = true;
                        Font fontCos = new Font(gridBandTot.AppearanceHeader.Font, FontStyle.Bold);
                        gridBandTot.AppearanceHeader.Font = fontCos;
                        gridBandTot.Caption = "TOTAL";

                        gridBandTot.Columns.Add(dgvCosta.Columns[acum]);
                        gridBandTot.Columns.Add(dgvCosta.Columns[acum + 1]);
                        gridBandTot.Columns.Add(dgvCosta.Columns[acum + 2]);
                        gridBandTot.Columns.Add(dgvCosta.Columns[acum + 3]);
                        gridBandTot.Columns.Add(dgvCosta.Columns[acum + 4]);

                        dgvCosta.Bands.Add(gridBandTot);

                        clsComun.gFormatearColumnasGrid(dgvCosta);
                        dgvCosta.BestFitColumns();

                    }
                }
                /*********************************************************************************************************************************/

                if (poPeriodoSierra != null)
                {
                    dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPHISTORICOPROVISIONESDC '{0}','{1}','SIERRA','{2}','0'", poPeriodoSierra.FechaInicio.ToString("dd/MM/yyyy"), psFechaFinal.ToString("dd/MM/yyyy"), cmbTipoReporte.EditValue.ToString()));
                    dgvSierra.Columns.Clear();
                    dgvSierra.Bands.Clear();
                    if (dt.Rows.Count > 0)
                    {
                        GridBand gridBandIni = new GridBand();
                        dgvSierra.Bands.Add(gridBandIni);

                        bsSierra.DataSource = dt;
                        gcSierra.DataSource = bsSierra;
                        DateTime pdFechaCalculo = poPeriodoSierra.FechaInicio;


                        dgvSierra.Columns[0].Visible = false;

                        gridBandIni.Columns.Add(dgvSierra.Columns[0]); // IdEmpleadoContrato
                        gridBandIni.Columns.Add(dgvSierra.Columns[1]); // Tipo
                        int acum = 0;

                        if (cmbTipoReporte.EditValue.ToString() == "E")
                        {
                            gridBandIni.Columns.Add(dgvSierra.Columns[2]); // Empleado
                            gridBandIni.Columns.Add(dgvSierra.Columns[3]); // Region
                            dgvSierra.Columns[3].Visible = false;
                            gridBandIni.Columns.Add(dgvSierra.Columns[4]); // Centro Costo
                            gridBandIni.Columns.Add(dgvSierra.Columns[5]); // Provision Anterior
                            acum = 6;
                        }
                        else
                        {
                            gridBandIni.Columns.Add(dgvSierra.Columns[2]); // Region
                            dgvSierra.Columns[2].Visible = false;
                            gridBandIni.Columns.Add(dgvSierra.Columns[3]); // Centro Costo
                            gridBandIni.Columns.Add(dgvSierra.Columns[4]);  //Provision Anterior
                            acum = 5;
                        }

                        dgvSierra.FixedLineWidth = 2;
                        dgvSierra.Bands[0].Fixed = FixedStyle.Left;


                        while (psFechaFinal > pdFechaCalculo)
                        {
                            GridBand gridBandDin = new GridBand();
                            gridBandDin.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                            gridBandDin.AppearanceHeader.Options.UseTextOptions = true;
                            gridBandDin.AppearanceHeader.Options.UseFont = true;
                            Font fontDin = new Font(gridBandDin.AppearanceHeader.Font, FontStyle.Bold);
                            gridBandDin.AppearanceHeader.Font = fontDin;
                            gridBandDin.Caption = clsComun.gsGetMes(pdFechaCalculo);

                            dgvSierra.Columns[acum].Caption = dgvSierra.Columns[acum].FieldName.Substring(0, dgvSierra.Columns[acum].FieldName.Length - 9);
                            dgvSierra.Columns[acum + 1].Caption = dgvSierra.Columns[acum + 1].FieldName.Substring(0, dgvSierra.Columns[acum + 1].FieldName.Length - 9);
                            dgvSierra.Columns[acum + 2].Caption = dgvSierra.Columns[acum + 2].FieldName.Substring(0, dgvSierra.Columns[acum + 2].FieldName.Length - 9);
                            dgvSierra.Columns[acum + 3].Caption = dgvSierra.Columns[acum + 3].FieldName.Substring(0, dgvSierra.Columns[acum + 3].FieldName.Length - 9);

                            gridBandDin.Columns.Add(dgvSierra.Columns[acum]);
                            gridBandDin.Columns.Add(dgvSierra.Columns[acum + 1]);
                            gridBandDin.Columns.Add(dgvSierra.Columns[acum + 2]);
                            gridBandDin.Columns.Add(dgvSierra.Columns[acum + 3]);

                            acum = acum + 4;
                            pdFechaCalculo = pdFechaCalculo.AddMonths(1);

                            dgvSierra.Bands.Add(gridBandDin);
                        }

                        GridBand gridBandTot = new GridBand();
                        gridBandTot.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                        gridBandTot.AppearanceHeader.Options.UseTextOptions = true;
                        gridBandTot.AppearanceHeader.Options.UseFont = true;
                        Font fontSie = new Font(gridBandTot.AppearanceHeader.Font, FontStyle.Bold);
                        gridBandTot.AppearanceHeader.Font = fontSie;
                        gridBandTot.Caption = "TOTAL";

                        gridBandTot.Columns.Add(dgvSierra.Columns[acum]);
                        gridBandTot.Columns.Add(dgvSierra.Columns[acum + 1]);
                        gridBandTot.Columns.Add(dgvSierra.Columns[acum + 2]);
                        gridBandTot.Columns.Add(dgvSierra.Columns[acum + 3]);
                        gridBandTot.Columns.Add(dgvSierra.Columns[acum + 4]);

                        dgvSierra.Bands.Add(gridBandTot);

                        clsComun.gFormatearColumnasGrid(dgvSierra);
                        dgvSierra.BestFitColumns();

                    }
                }
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
    }
}
