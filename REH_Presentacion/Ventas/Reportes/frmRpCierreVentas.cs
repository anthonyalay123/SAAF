using DevExpress.XtraGrid.Views.Grid;
using REH_Negocio;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraReports.UI;
using REH_Presentacion.Ventas.Reportes.Rpt;
using COM_Negocio;
using DevExpress.Data;

namespace REH_Presentacion.Ventas.Reportes
{
    /// <summary>
    /// Autor: 
    /// Fecha: 
    /// Formulario
    /// </summary>
    public partial class frmRpCierreVentas : frmBaseTrxDev
    {

        #region Variables

        clsNGestorConsulta loLogicaNegocio;

        #endregion

        #region Eventos
        public frmRpCierreVentas()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNGestorConsulta();
            cmbTipoReporte.EditValueChanged += cmbTipoReporte_EditValueChanged;
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbTipo, loLogicaNegocio.goConsultarComboTipoCierreVentas());
                clsComun.gLLenarCombo(ref cmbTipoReporte, loLogicaNegocio.goConsultarComboTipoReporteCierreVentas());

                lCargarEventosBotones();
                lLimpiar();

                lblFechaInicial.Visible = true;
                dtpFechaInicial.Visible = true;
                lblFechaInicial.Text = "Fecha Corte:";
                lblFechaFinal.Visible = false;
                dtpFechaFinal.Visible = false;

                dtpFechaInicial.DateTime = DateTime.Now;
                dtpFechaFinal.DateTime = dtpFechaInicial.DateTime;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbTipoReporte_EditValueChanged(object sender, EventArgs e)
        {
            if (cmbTipoReporte.EditValue != null && cmbTipoReporte.EditValue.ToString() == "D")
            {
                lblFechaInicial.Visible = true;
                dtpFechaInicial.Visible = true;
                lblFechaInicial.Text = "Fecha Corte:";
                lblFechaFinal.Visible = false;
                dtpFechaFinal.Visible = false;
                dtpFechaInicial.DateTime = DateTime.Now;
                dtpFechaFinal.DateTime = dtpFechaInicial.DateTime;
            }
            else
            {
                lblFechaInicial.Visible = true;
                dtpFechaInicial.Visible = true;
                lblFechaInicial.Text = "Fecha Inicial:";
                lblFechaFinal.Visible = true;
                dtpFechaFinal.Visible = true;
                dtpFechaInicial.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpFechaFinal.DateTime = DateTime.Now;
            }
        }

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

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {


                /******** VALIDACIÓN DE TIPOS DE PRODUCTOS VACIOS ***************************************************************/
                var dsVal = loLogicaNegocio.goConsultaDataSet(string.Format("EXEC [dbo].[VTASPPRESUPUESTOVENTAORGANICOCLIENTE] '{0}','{1}','0','0','1'", dtpFechaInicial.DateTime, dtpFechaFinal.DateTime));
                var msgVal = "";
                if (dsVal.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in dsVal.Tables[0].Rows)
                    {
                        msgVal = string.Format("{0}Código: {1} - Vendedor: {2} de SAP NO tiene relacionado Vendedor en SAAF.\n", msgVal, item[0].ToString(), item[1].ToString());
                    }

                }
                if (dsVal.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow item in dsVal.Tables[1].Rows)
                    {
                        msgVal = string.Format("{0}Código: {1} - Grupo de Producto: {2} de SAP NO tiene relacionado Tipo de Producto en SAAF.\n", msgVal, item[0].ToString(), item[1].ToString());
                    }

                }
                if (!string.IsNullOrEmpty(msgVal))
                {
                    XtraMessageBox.Show(msgVal, "IMPORTANTE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                /****************************************************************************************************************/

                DataTable dt1 = ldtValidaQuery();
                System.Data.DataSet ds1 = new System.Data.DataSet();
                dt1.TableName = "CierreVentas";
                ds1.Merge(dt1);
                DataTable dt2 = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC [dbo].[VTASPRESUMENVENTASIVAKL] '{0}', '{1}' ", dtpFechaInicial.DateTime, dtpFechaFinal.DateTime));

                //var psParametro1 = string.Format("CIERRE DE VENTAS DESDE {0} A {1}", dtpFechaInicial.DateTime, dtpFechaFinal.DateTime);

                string psLeyenda = "";
                DateTime pdFechaLeyenda = new DateTime(2022, 9, 5);
                if (pdFechaLeyenda.Date >= Convert.ToDateTime(dtpFechaInicial.DateTime).Date && pdFechaLeyenda.Date <= Convert.ToDateTime(dtpFechaFinal.DateTime).Date)
                {
                    psLeyenda = "Nota: Este reporte no está considerando la factura #10929 por un valor de $221,078.00 que fue emitida el 05/09/2022 por devolución de productos del proveedor FMC. Solicitado por la Gerencia Financiera.";
                }

                var subtitulo = " ";

                if (cmbTipo.EditValue.ToString() == "E")
                {
                    subtitulo = string.Format(" - CICLO CORTO, SIERRA Y FLORES");
                }
                else
                {
                    subtitulo = string.Format(" - {0}", cmbTipo.Text);
                }

                if (cmbTipoReporte.EditValue.ToString() == "D")
                {

                    xrptCierreVentasDiario xrpt = new xrptCierreVentasDiario();
                    var pdFechaFin = Convert.ToDateTime(dtpFechaInicial.Text.Substring(0, 10));
                    DateTime pdFecha = pdFechaFin;
                    string lsTituloReporte = "{0}, {1} de {2} del {3}";
                    var psParametro1 = string.Format(lsTituloReporte, clsComun.gsGetDia(pdFecha), pdFecha.Day, clsComun.gsGetMes(pdFecha.Month).ToLower(), pdFecha.Year);

                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrpt.Parameters["Titulo"].Value = psParametro1;
                    xrpt.Parameters["Subtitulo"].Value = subtitulo;
                    xrpt.Parameters["KiloLitros"].Value = dt2.Rows[0][1].ToString();
                    xrpt.Parameters["Iva"].Value = dt2.Rows[0][0].ToString();
                    xrpt.DataSource = ds1;
                    //Se invoca la ventana que muestra el reporte.
                    xrpt.RequestParameters = false;
                    xrpt.Parameters["Titulo"].Visible = false;
                    xrpt.Parameters["Subtitulo"].Visible = false;
                    xrpt.Parameters["KiloLitros"].Visible = false;
                    xrpt.Parameters["Iva"].Visible = false;

                    using (ReportPrintTool printTool = new ReportPrintTool(xrpt))
                    {
                        printTool.ShowRibbonPreviewDialog();
                    }
                }
                else
                {

                    string fechaInicial = dtpFechaInicial.DateTime.ToString("dd/MM/yyyy");
                    string fechaFinal = dtpFechaFinal.DateTime.ToString("dd/MM/yyyy");

                    var psParametro1 = string.Format("DESDE {0} A {1}", fechaInicial, fechaFinal);

                    xrptCierreVentasMensual xrpt = new xrptCierreVentasMensual(); 

                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrpt.Parameters["Titulo"].Value = psParametro1;
                    xrpt.Parameters["Subtitulo"].Value = subtitulo;
                    xrpt.Parameters["KiloLitros"].Value = dt2.Rows[0][1].ToString();
                    xrpt.Parameters["Iva"].Value = dt2.Rows[0][0].ToString();
                    xrpt.Parameters["Leyenda"].Value = psLeyenda;
                    xrpt.DataSource = ds1;
                    //Se invoca la ventana que muestra el reporte.
                    xrpt.RequestParameters = false;
                    xrpt.Parameters["Titulo"].Visible = false;
                    xrpt.Parameters["Subtitulo"].Visible = false;
                    xrpt.Parameters["KiloLitros"].Visible = false;
                    xrpt.Parameters["Iva"].Visible = false;
                    xrpt.Parameters["Leyenda"].Visible = false;

                    using (ReportPrintTool printTool = new ReportPrintTool(xrpt))
                    {
                        printTool.ShowRibbonPreviewDialog();
                    }
                }

                List<int> piListaimprimir = new List<int>();
                piListaimprimir.Add(161); // Reportes de Cierre de Ventas Mensual
                piListaimprimir.Add(162); // Reportes de Cierre de Ventas Diario
                //if (piListaimprimir.Contains(int.Parse(Tag.ToString().Split(',')[0])))
                var pIdMenu = int.Parse(Tag.ToString().Split(',')[0]);

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
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
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

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

        private void lLimpiar()
        {
           
        }

        private DataTable ldtValidaQuery()
        {
            string psQuery;
            if (cmbTipoReporte.EditValue.ToString() == "D")
            {
                var pdDesde = new DateTime(dtpFechaInicial.DateTime.Year, dtpFechaInicial.DateTime.Month, 1);
                psQuery = string.Format("EXEC VTASPCIERREVENTAS '{0}', '{1}','1', '0', '0', '{2}'", pdDesde, dtpFechaInicial.DateTime, cmbTipo.EditValue.ToString());
            } else
            {
                psQuery = string.Format("EXEC VTASPCIERREVENTAS '{0}', '{1}','0', '0', '0', '{2}'", dtpFechaInicial.DateTime, dtpFechaFinal.DateTime, cmbTipo.EditValue.ToString());
            }
           
            return loLogicaNegocio.goConsultaDataTable(psQuery);
;
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


            dgvDatos.Columns["Ord"].Visible = false;
            dgvDatos.Columns["Orden"].Visible = false;

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

    }

}
