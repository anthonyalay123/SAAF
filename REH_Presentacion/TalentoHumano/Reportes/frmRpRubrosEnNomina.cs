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
    public partial class frmRpRubrosEnNomina : frmBaseTrxDev
    {
        clsNGestorConsulta loLogicaNegocio = new clsNGestorConsulta();
        BindingSource bsDatos = new BindingSource();

        public frmRpRubrosEnNomina()
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
                clsComun.gLLenarCombo(ref cmbEmpleado,loLogicaNegocio.goConsultarComboEmpleado(), false, false, "", true);
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
                //if (lEsValido())
                //{

                //    System.Data.DataSet ds = new System.Data.DataSet();
                //    var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC [REHSPGCCONSULTANOMINAEMPLEADOXFECHAS] '{0}','{1}','{2}'", dtpFechaInicial.DateTime, dtpFechaFinal.DateTime, cmbEmpleado.EditValue.ToString()));
                //    dt.TableName = "Provision";
                //    ds.Merge(dt);

                //    if (dt.Rows.Count > 0)
                //    {
                //        if (cmbEmpleado.EditValue.ToString() == "E")
                //        {
                //            xrptProvisionesFondoReserva xrpt = new xrptProvisionesFondoReserva();

                //            //Se establece el origen de datos del reporte (El dataset previamente leído)
                //            xrpt.Parameters["parameter1"].Value = string.Format("PROVISIÓN FONDO DE RESERVA\n{0} / {1}", clsComun.gsGetMes(psFechaFinal.Month), psFechaFinal.Year); ;
                //            xrpt.Parameters["cab1"].Value = string.Format("Prov. Pagada {0}/{1}", clsComun.gsGetMes(psFechaFinal.AddMonths(-1).Month).ToLower(), psFechaFinal.AddMonths(-1).Year);
                //            //xrpt.Parameters["cab2"].Value = string.Format("Pagos Mens. Iess {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                //            xrpt.Parameters["cab2"].Value = string.Format("Pagos Mens. Iess");
                //            xrpt.Parameters["cab3"].Value = string.Format("Saldo Acum. {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                //            xrpt.DataSource = ds;
                //            //Se invoca la ventana que muestra el reporte.
                //            xrpt.RequestParameters = false;
                //            xrpt.Parameters["parameter1"].Visible = false;
                //            xrpt.Parameters["cab1"].Visible = false;
                //            xrpt.Parameters["cab2"].Visible = false;
                //            xrpt.Parameters["cab3"].Visible = false;

                //            using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                //            {
                //                printTool.ShowRibbonPreviewDialog();
                //            }
                //        }
                //        else
                //        {
                //            xrptProvisionesFondoReservaCC xrpt = new xrptProvisionesFondoReservaCC();

                //            //Se establece el origen de datos del reporte (El dataset previamente leído)
                //            xrpt.Parameters["parameter1"].Value = string.Format("PROVISIÓN FONDO DE RESERVA\n{0} / {1}", clsComun.gsGetMes(psFechaFinal.Month), psFechaFinal.Year); ;
                //            xrpt.Parameters["cab1"].Value = string.Format("Prov. Pagada {0}/{1}", clsComun.gsGetMes(psFechaFinal.AddMonths(-1).Month).ToLower(), psFechaFinal.AddMonths(-1).Year);
                //            //xrpt.Parameters["cab2"].Value = string.Format("Pagos Mens. Iess {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                //            xrpt.Parameters["cab2"].Value = string.Format("Pagos Mens. Iess");
                //            xrpt.Parameters["cab3"].Value = string.Format("Saldo Acum. {0}", clsComun.gsGetMes(psFechaFinal.Month).ToLower());
                //            xrpt.DataSource = ds;
                //            //Se invoca la ventana que muestra el reporte.
                //            xrpt.RequestParameters = false;
                //            xrpt.Parameters["parameter1"].Visible = false;
                //            xrpt.Parameters["cab1"].Visible = false;
                //            xrpt.Parameters["cab2"].Visible = false;
                //            xrpt.Parameters["cab3"].Visible = false;

                //            using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                //            {
                //                printTool.ShowRibbonPreviewDialog();
                //            }
                //        }
                //    }
                //    else
                //    {
                //        XtraMessageBox.Show("No existen datos guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //}
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

                DataTable dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC [REHSPGCCONSULTANOMINAEMPLEADOXFECHAS] '{0}','{1}','{2}'", dtpFechaInicial.DateTime, dtpFechaFinal.DateTime, cmbEmpleado.EditValue.ToString()));

                bsDatos.DataSource = dt;
                gcDatos.DataSource = bsDatos;
                clsComun.gFormatearColumnasGrid(dgvDatos);
                clsComun.gOrdenarColumnasGridFull(dgvDatos);

                /*
                gcDatos.DataSource = null;
                dgvDatos.Columns.Clear();

                dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC [REHSPGCCONSULTANOMINAEMPLEADOXFECHAS] '{0}','{1}','{2}'", dtpFechaInicial.DateTime, dtpFechaFinal.DateTime, cmbEmpleado.EditValue.ToString()));

                dgvDatos.Columns.Clear();
                dgvDatos.Bands.Clear();
                if (dt.Rows.Count > 0)
                {
                    bsDatos.DataSource = dt;
                    gcDatos.DataSource = bsDatos;
                    clsComun.gFormatearColumnasGrid(dgvDatos);
                    dgvDatos.BestFitColumns();
                }
                */
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
                XtraMessageBox.Show("La fecha desde no debe ser mayor que la fecha hasta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            return true;
        }
    }
}
