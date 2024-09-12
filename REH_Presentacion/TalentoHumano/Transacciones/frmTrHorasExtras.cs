using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using GEN_Entidad;
using REH_Negocio.Transacciones;
using REH_Presentacion.Formularios;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
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

namespace REH_Presentacion.TalentoHumano.Transacciones
{
    public partial class frmTrHorasExtras : frmBaseTrxDev
    {

        #region Variables
        private PrinterSettings prnSettings;
        clsNHorasExtras loLogicaNegocio;
        BindingSource bsDatos = new BindingSource();
        bool lbCargado = false;
        #endregion

        #region Eventos
        public frmTrHorasExtras()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNHorasExtras();
        }

        private void frmTrHorasExtras_Load(object sender, EventArgs e)
        {
            try
            {
                var poLista = loLogicaNegocio.goConsultarComboPeriodo(Diccionario.Tablas.TipoRol.FinMes);

                foreach (var item in poLista)
                {
                    item.Descripcion = item.Descripcion.Replace("SEGUNDA QUINCENA", clsComun.gsGetMes(int.Parse(item.Descripcion.Substring(5,2))));
                }

                clsComun.gLLenarCombo(ref cmbPeriodo, poLista, true);

                lBuscar();
                lCargarEventosBotones();
                lAsignarBsGrid();
                lbCargado = true;

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
        /// Evento del botón Grabar, Guarda datos del formulario a el objeto de BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                {
                    string psMsg = loLogicaNegocio.gsGenerarHorasExtras(dtpFechaInicio.Value, dtpFechaFin.Value, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lBuscar();
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                

                
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
                lBuscar();

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
                if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                {
                    DataSet ds = new DataSet();
                    var dt = loLogicaNegocio.gdtConsultaResumenHorasExtras(int.Parse(cmbPeriodo.EditValue.ToString()));
                    dt.TableName = "HorasExtras";
                    ds.Merge(dt);
                    string psTitulo = "";
                    if (dt.Rows.Count > 0)
                    {
                        xrptResumenHorasExtras xrpt = new xrptResumenHorasExtras();
                        xrpt.DataSource = ds;

                        DateTime pdFechaInicial;
                        DateTime pdFechaFinal;

                        loLogicaNegocio.gConsultaCorteHorasExtras(int.Parse(cmbPeriodo.EditValue.ToString()), out pdFechaInicial, out pdFechaFinal);
                        psTitulo = string.Format("LISTADO DE HORAS EXTRAS DESDE {0} A {1}.", pdFechaInicial.ToString("dd/MM/yyyy"), pdFechaFinal.ToString("dd/MM/yyyy"));
                        //Se establece el origen de datos del reporte (El dataset previamente leído)
                        xrpt.Parameters["tsTitulo"].Value = psTitulo;
                        xrpt.DataSource = ds;
                        //Se invoca la ventana que muestra el reporte.
                        xrpt.RequestParameters = false;
                        xrpt.Parameters["tsTitulo"].Visible = false;


                        //using (ReportPrintTool printTool = new ReportPrintTool(xrpt))
                        //{
                        //    printTool.ShowRibbonPreviewDialog();
                        //}

                        /****************************************************************************************************************************************************************************/
                        /****************************************************************************************************************************************************************************/

                        ds = new DataSet();
                        dt = loLogicaNegocio.gdtConsultaDetalleHorasExtras(int.Parse(cmbPeriodo.EditValue.ToString()));
                        if (dt.Rows.Count > 0)
                        {
                            dt.TableName = "DetalleHorasExtras";
                            ds.Merge(dt);

                            xrptDetalleHorasExtras xrptDetalle = new xrptDetalleHorasExtras();
                            xrptDetalle.DataSource = ds;


                            //Se establece el origen de datos del reporte (El dataset previamente leído)
                            xrptDetalle.Parameters["tsTitulo"].Value = psTitulo;
                            xrptDetalle.DataSource = ds;
                            //Se invoca la ventana que muestra el reporte.
                            xrptDetalle.RequestParameters = false;
                            xrptDetalle.Parameters["tsTitulo"].Visible = false;


                            /****************************************************************************************************************************************************************************/
                            /****************************************************************************************************************************************************************************/

                            XtraReport[] reports;

                            reports = new XtraReport[] { xrpt, xrptDetalle };

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
                            using (ReportPrintTool printTool = new ReportPrintTool(xrpt))
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

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (bsDatos != null)
                {
                    var poLista = ((DataTable)bsDatos.DataSource);

                    if (poLista != null && poLista.Rows.Count > 0)
                    {
                        clsComun.gSaveFile(gcDatos, Text, "Files(*.xlsx;)|*.xlsx;");
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

        private void cmbPeriodo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                lBuscar();
                lChangeValue();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Métodos

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Click += btnCalcular_Click;
            if (tstBotones.Items["btnProcesar"] != null) tstBotones.Items["btnProcesar"].Click += btnCalcular_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
        }

        private void lLimpiar()
        {
            if ((cmbPeriodo.Properties.DataSource as IList).Count > 0) cmbPeriodo.ItemIndex = 0;
            lblEstado.Text = string.Empty;
            dtpFechaInicio.Checked = false;
            dtpFechaInicio.Value = DateTime.Now;
            dtpFechaFin.Value = DateTime.Now;
            dtpFechaFin.Checked = false;
            bsDatos = null;
        }

        private void lAsignarBsGrid()
        {
            
            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            dgvDatos.OptionsBehavior.Editable = false;
            dgvDatos.OptionsCustomization.AllowColumnMoving = false;
            dgvDatos.OptionsCustomization.AllowGroup = false;
            dgvDatos.OptionsView.ColumnAutoWidth = false;
            dgvDatos.OptionsView.ShowFooter = true;

        }

        private void lBuscar()
        {

            if(cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
            {
                gcDatos.DataSource = null;
                dgvDatos.Columns.Clear();
                DataTable dt = loLogicaNegocio.gdtConsultaHorasExtras(int.Parse(cmbPeriodo.EditValue.ToString()));

                bsDatos.DataSource = dt;
                gcDatos.DataSource = bsDatos;


                dgvDatos.FixedLineWidth = 1;
                dgvDatos.Columns["Empleado"].Fixed = FixedStyle.Left;

                var poListaFecha = new List<DateTime>();

                foreach (DataRow item in dt.Rows)
                {
                    poListaFecha.Add(Convert.ToDateTime(item["Fecha"].ToString()));
                }

                //Agregar Sumatoria
                for (int i = 0; i < dgvDatos.Columns.Count; i++)
                {

                    if (i > 1)
                    {
                        if (dt.Columns[i].DataType == typeof(decimal))
                        {
                            var psNameColumn = dgvDatos.Columns[i].FieldName;

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

                }

                //dgvDatos.PopulateColumns();
                //dgvDatos.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
                dgvDatos.BestFitColumns();

                if (poListaFecha.Count > 0)
                {
                    DateTime pdFechaInicial;
                    DateTime pdFechaFinal;

                    loLogicaNegocio.gConsultaCorteHorasExtras(int.Parse(cmbPeriodo.EditValue.ToString()), out pdFechaInicial, out pdFechaFinal);

                    dtpFechaInicio.Value = pdFechaInicial;
                    dtpFechaFin.Value = pdFechaFinal;

                    //dtpFechaInicio.Value = poListaFecha.Min(x=>x);
                    //dtpFechaFin.Value = poListaFecha.Max(x => x);

                    dtpFechaInicio.Checked = false;
                    dtpFechaFin.Checked = false;
                }
            }

        }

        private void lChangeValue()
        {
            dtpFechaInicio.Enabled = true;
            dtpFechaFin.Enabled = true;
            if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Enabled = true;

            if (lbCargado)
            {   
                if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                {
                    lblEstado.Text = loLogicaNegocio.gsGetDescripcionEstadoNomina(int.Parse(cmbPeriodo.EditValue.ToString()));
                    DateTime pdFechaInicial;
                    DateTime pdFechaFinal;

                    loLogicaNegocio.gConsultaCorteHorasExtras(int.Parse(cmbPeriodo.EditValue.ToString()), out pdFechaInicial, out pdFechaFinal);
                    dtpFechaInicio.Value = pdFechaInicial;
                    dtpFechaFin.Value = pdFechaFinal;
                    dtpFechaInicio.Checked = false;
                    dtpFechaFin.Checked = false;
                    //lblRangoFechas.Text = string.Format("{0} A {1}.", pdFechaInicial.ToString("dd/MM/yyyy"), pdFechaFinal.ToString("dd/MM/yyyy"));


                    var poRegistro = loLogicaNegocio.goGetEstadoHorasExtras(int.Parse(cmbPeriodo.EditValue.ToString()));

                    if (poRegistro != null)
                    {
                        int piMes = int.Parse(cmbPeriodo.Text.Trim().Substring(5, 2));
                        string psAnio = cmbPeriodo.Text.Trim().Substring(1, 4);
                        lblHorasExtras.Text = "CORRESPONDIENTE A " + clsComun.gsGetMes(piMes) + " DEL " + psAnio;
                        lblEstadoHorasExtras.Text = poRegistro.Descripcion;

                        if (poRegistro.Codigo == Diccionario.Aprobado || poRegistro.Codigo == Diccionario.Cerrado)
                        {
                            dtpFechaInicio.Enabled = false;
                            dtpFechaFin.Enabled = false;
                            if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Enabled = false;
                        }
                    }
                    else
                    {
                        lblHorasExtras.Text = string.Empty;
                        lblEstadoHorasExtras.Text = string.Empty;
                    }

                    var poUsuarios = loLogicaNegocio.gsGetAprobacionUsuarios(int.Parse(cmbPeriodo.EditValue.ToString()));
                    lblAprobacionUsuarios.Text = string.Empty;
                    if (poUsuarios != null && poUsuarios.Count > 0)
                    {
                        int piCount = poUsuarios.Count;
                        int piCont = 0;
                        foreach (var x in poUsuarios)
                        {
                            piCont++;
                            lblAprobacionUsuarios.Text += x;
                            if (piCont < piCount)
                                lblAprobacionUsuarios.Text += "\n";
                        }
                    }
                }
                else
                {
                    lblEstado.Text = string.Empty;
                    dtpFechaInicio.Value = DateTime.Now;
                    dtpFechaFin.Value = DateTime.Now;
                    dtpFechaInicio.Checked = false;
                    dtpFechaFin.Checked = false;
                    lblHorasExtras.Text = string.Empty;
                    lblEstadoHorasExtras.Text = string.Empty;
                    lblAprobacionUsuarios.Text = string.Empty;
                }
                lBuscar();
            }
        }

        #endregion
    }
}
