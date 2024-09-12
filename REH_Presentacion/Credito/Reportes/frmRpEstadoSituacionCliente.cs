using CRE_Negocio.Reportes;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using GEN_Entidad;
using REH_Presentacion.Credito.Reportes.Rpt;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Credito.Reportes
{
    public partial class frmRpEstadoSituacionCliente : frmBaseTrxDev
    {

        #region Variables
        private static PrinterSettings prnSettings;
        clsNEstadoSituacionCliente loLogicaNegocio;
        //private bool pbCargado = false;
        #endregion


        public frmRpEstadoSituacionCliente()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNEstadoSituacionCliente();
        }

        private void frmRpEstadoSituacionCliente_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbCliente, loLogicaNegocio.goSapConsultaClientes(), true);
                //clsComun.gLLenarGridCombo(ref gcmbCliente, loLogicaNegocio.goSapConsultaClientes(), true);
                lLimpiar();
                lCargarEventosBotones();
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
        /// Evento del botón Consultar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            lConsultar();
        }

        /// <summary>
        /// Evento del botón Imprimir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnConsultar_Click;

        }

        private void lLimpiar()
        {
            if ((cmbCliente.Properties.DataSource as IList).Count > 0) cmbCliente.ItemIndex = 0;
        }

        private void lConsultar()
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {

                if (cmbCliente.EditValue.ToString() != Diccionario.Seleccione)
                {
                    System.Data.DataSet ds = new System.Data.DataSet();

                    var dtSitCli = loLogicaNegocio.gdtSapConsultaSituacionCliente(cmbCliente.EditValue.ToString());
                    dtSitCli.TableName = "InformeSituacionCliente";
                    ds.Merge(dtSitCli);

                    var dtSitCliFacChePos = loLogicaNegocio.gdtSapConsultaSituacionClienteFacChePos(cmbCliente.EditValue.ToString());
                    dtSitCliFacChePos.TableName = "DetalleFacturaChequePosfechado";
                    ds.Merge(dtSitCliFacChePos);

                    var dtSitCliFacSinChe = loLogicaNegocio.gdtSapConsultaSituacionClienteFacSinChe(cmbCliente.EditValue.ToString());
                    dtSitCliFacSinChe.TableName = "DetalleFacturaSinRespaldoCheque";
                    ds.Merge(dtSitCliFacSinChe);

                    var dtSitCliFacSinResChe = loLogicaNegocio.gdtSapConsultaSituacionClienteRecComPag(cmbCliente.EditValue.ToString());
                    dtSitCliFacSinResChe.TableName = "RecordCompraPago";
                    ds.Merge(dtSitCliFacSinResChe);

                    var dtSitCliSalConChe = loLogicaNegocio.gdtSapConsultaSituacionClienteSalConChe(cmbCliente.EditValue.ToString());
                    dtSitCliSalConChe.TableName = "SaldoCheques";
                    ds.Merge(dtSitCliSalConChe);

                    var dtSitCliSaSinChe = loLogicaNegocio.gdtSapConsultaSituacionClienteSalSinChe(cmbCliente.EditValue.ToString());
                    dtSitCliSaSinChe.TableName = "SaldoSinCheques";
                    ds.Merge(dtSitCliSaSinChe);

                    if (dtSitCli.Rows.Count > 0)
                    {
                        xrptEstadoSituacionCliente xrpt = new xrptEstadoSituacionCliente();
                        //xrptEstadoSituacionClienteFacChePos xrptFacChePos = new xrptEstadoSituacionClienteFacChePos();

                        //Se establece el origen de datos del reporte (El dataset previamente leído)
                        decimal pdcPorDeudaTotalConChe = 0, pdcPorDeudaTotalSinChe = 0, pdcDeudaTotal = 0;
                        if (!string.IsNullOrEmpty(dtSitCli.Rows[0]["DeudaTotal"].ToString()))
                        {
                            decimal pdcSaldoVencidoConChe = 0, pdcSaldoVencidoSinChe = 0;
                            pdcDeudaTotal = Convert.ToDecimal(dtSitCli.Rows[0]["DeudaTotal"].ToString());
                            if (pdcDeudaTotal > 0)
                            {
                                if (!string.IsNullOrEmpty(dtSitCliSalConChe.Rows[0]["SaldoVencido"].ToString()))
                                {
                                    pdcSaldoVencidoConChe = Convert.ToDecimal(dtSitCliSalConChe.Rows[0]["SaldoVencido"].ToString());
                                }

                                if (!string.IsNullOrEmpty(dtSitCliSaSinChe.Rows[0]["SaldoVencido"].ToString()))
                                {
                                    pdcSaldoVencidoSinChe = Convert.ToDecimal(dtSitCliSaSinChe.Rows[0]["SaldoVencido"].ToString());
                                }

                                pdcPorDeudaTotalConChe = pdcSaldoVencidoConChe / pdcDeudaTotal;
                                pdcPorDeudaTotalSinChe = pdcSaldoVencidoSinChe / pdcDeudaTotal;
                            }

                        }

                        xrpt.Parameters["PorDeudaTotConCh"].Value = pdcPorDeudaTotalConChe;
                        xrpt.Parameters["PorDeudaTotSinCh"].Value = pdcPorDeudaTotalSinChe;
                        xrpt.DataSource = ds;

                        //Se invoca la ventana que muestra el reporte.
                        xrpt.RequestParameters = false;
                        xrpt.Parameters["PorDeudaTotConCh"].Visible = false;
                        xrpt.Parameters["PorDeudaTotSinCh"].Visible = false;

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

                        //using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                        //{
                        //    printTool.ShowRibbonPreviewDialog();
                        //}
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos para consultar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("Seleccione el Cliente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        private void cmbCliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.Equals(Keys.Enter)) lConsultar();
        }

        /// <summary>
        /// Eventos necesarios para la presentación de reportes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void PrintingSystem_StartPrint(object sender, PrintDocumentEventArgs e)
        {
            prnSettings = e.PrintDocument.PrinterSettings;
        }

        /// <summary>
        /// Eventos necesarios para la presentación de reportes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void reportsStartPrintEventHandler(object sender, PrintDocumentEventArgs e)
        {
            int pageCount = e.PrintDocument.PrinterSettings.ToPage;
            e.PrintDocument.PrinterSettings = prnSettings;

            // The following line is required if the number of pages for each report varies, 
            // and you consistently need to print all pages.
            e.PrintDocument.PrinterSettings.ToPage = pageCount;
        }

    }
}
