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
using System.IO;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
using GEN_Entidad.Entidades;
using DevExpress.XtraReports.UI;
using REH_Presentacion.Ventas.Reportes.Rpt;
using COM_Negocio;
using AFI_Negocio;
using REH_Presentacion.ActivoFijo.Reportes.Rpt;

namespace REH_Presentacion.ActivoFijo.Reportes
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 09/11/2020
    /// Formulario de gestor de consultas
    /// </summary>
    public partial class frmRpDepreciacionMensual : frmBaseTrxDev
    {

        #region Variables

        clsNActivoFijo loLogicaNegocio;
        string lsQuery;
        string lsTituloReporte;
        string lsDataSet;
        bool lbLandSpace;
        Nullable<int> liFixedColumn;
        public int lIdGestorConsulta;
        #endregion

        #region Eventos
        public frmRpDepreciacionMensual()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNActivoFijo();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                lLimpiar();
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
        /// Evento del botón Nuevo, Generalmente Limpia el formulario 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerarDiario_Click(object sender, EventArgs e)
        {
            try
            {
                var piAnio = string.IsNullOrEmpty(txtAnio.Text) ? 0 : int.Parse(txtAnio.Text);
                var piMes = string.IsNullOrEmpty(txtMes.Text) ? 0 : int.Parse(txtMes.Text);

                GridControl gc = new GridControl();
                BindingSource bs = new BindingSource();
                GridView dgv = new GridView();

                gc.DataSource = bs;
                gc.MainView = dgv;
                gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                dgv.GridControl = gc;
                this.Controls.Add(gc);
                bs.DataSource = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPGCDIARIODEPRECIACION {0}, {1}", piAnio, piMes));
                clsComun.gOrdenarColumnasGridFullEditableNone(dgv);
                clsComun.gFormatearColumnasGrid(dgv);
                //dgv.BestFitColumns();

                //dgv.PopulateColumns();
                //dgv.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
                // Exportar Datos
                clsComun.gSaveFile(gc, "Diario_Depreciacion_"+ clsComun.gsGetMes(piMes) + "_" + piAnio + ".xlsx", "Files(*.xlsx;)|*.xlsx;");

                gc.Visible = false;
                
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
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                var piAnio = string.IsNullOrEmpty(txtAnio.Text) ? 0 : int.Parse(txtAnio.Text);
                var piMes = string.IsNullOrEmpty(txtMes.Text) ? 0 : int.Parse(txtMes.Text);
                var psMsg = loLogicaNegocio.gsValidaPrevioGeneracion(piAnio, piMes);
                if (string.IsNullOrEmpty(psMsg))
                {
                    loLogicaNegocio.goConsultaDataTable("EXEC AFISPGENERADEPRECIACIONMENSUAL " + piAnio + "," + piMes);
                    XtraMessageBox.Show("Depreciación generada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lConsultarBoton();
                }
                else
                {
                    XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                SendKeys.Send("{TAB}");
                lConsultarBoton();

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
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                SendKeys.Send("{TAB}");
                var piAnio = string.IsNullOrEmpty(txtAnio.Text) ? 0 : int.Parse(txtAnio.Text);
                var piMes = string.IsNullOrEmpty(txtMes.Text) ? 0 : int.Parse(txtMes.Text);
                var psMsg = loLogicaNegocio.gsCerrarDepreciacion(clsPrincipal.gsUsuario, piAnio, piMes);
                if (string.IsNullOrEmpty(psMsg))
                {
                    XtraMessageBox.Show("Periodo Cerrado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lBuscar();
                }
                else
                {
                    XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void btnReversar_Click(object sender, EventArgs e)
        {
            try
            {
                SendKeys.Send("{TAB}");
                var piAnio = string.IsNullOrEmpty(txtAnio.Text) ? 0 : int.Parse(txtAnio.Text);
                var piMes = string.IsNullOrEmpty(txtMes.Text) ? 0 : int.Parse(txtMes.Text);
                var psMsg = loLogicaNegocio.gsReversarDepreciacion(clsPrincipal.gsUsuario, piAnio, piMes);
                if (string.IsNullOrEmpty(psMsg))
                {
                    XtraMessageBox.Show("Periodo Reversado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lBuscar();
                }
                else
                {
                    XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

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
                        clsComun.gSaveFile(gcDatos, Text + ".pdf", psFilter, "PDF");
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

            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                var dt = loLogicaNegocio.goConsultaDataTable(string.Format("SELECT I.Tipo,I.Agrupacion,SUM(I.ValorOriginal) 'ValorOriginal',SUM(I.ValorResidual) 'ValorResidual',SUM(I.ValorDepreciable) 'ValorDepreciable',SUM(I.ValorDepreciado) 'ValorDepreciado',SUM(ISNULL(I.ValorPorDepreciar,0)) 'ValorPorDepreciar',SUM(ISNULL(I.DepreciacionMensual,0)) 'DepreciacionMensual',SUM(I.ValorActual) 'ValorActual' FROM [AFILITEMACTIVOFIJOHISTORICO] I WHERE I.CodigoEstado = 'A' AND ANIO = {0} AND MES = {1} GROUP BY I.Agrupacion,I.Tipo ORDER BY Agrupacion", txtAnio.Text, txtMes.Text));
                dt.TableName = "Depreciacion";
                if (ds.Tables["Depreciacion"] != null) ds.Tables.Remove("Depreciacion");
                ds.Merge(dt);
                if (dt.Rows.Count > 0)
                {
                    
                    xrptDepreciacion xrpt = new xrptDepreciacion();

                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrpt.Parameters["parameter1"].Value = string.Format("RESUMEN DE DEPRECIACÓN DE ACTIVOS FIJOS {0} - {1}", clsComun.gsGetMes(int.Parse(txtMes.Text)), txtAnio.Text);
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
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
        }

        private DataTable ldtValidaQuery()
        {
            var piAnio = string.IsNullOrEmpty(txtAnio.Text) ? 0 : int.Parse(txtAnio.Text);
            var piMes = string.IsNullOrEmpty(txtMes.Text) ? 0 : int.Parse(txtMes.Text);
            lblEstado.Text = Diccionario.gsGetDescripcion(loLogicaNegocio.gsEstadoDepreciacion(piAnio, piMes));
            lOcultarBotones();
            return loLogicaNegocio.goConsultaDataTable("EXEC AFISPCONSULTADEPRECIACION "+piAnio +","+piMes);
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
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnGenerarDiario"] != null) tstBotones.Items["btnGenerarDiario"].Click += btnGenerarDiario_Click;
            if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Click += btnGenerar_Click;
            if (tstBotones.Items["btnCerrar"] != null) tstBotones.Items["btnCerrar"].Click += btnCerrar_Click;
            if (tstBotones.Items["btnReversar"] != null) tstBotones.Items["btnReversar"].Click += btnReversar_Click;

            txtAnio.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtMes.KeyPress += new KeyPressEventHandler(SoloNumeros);

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
                lsQuery = string.Empty;
            }

            txtAnio.Text = "";
            txtMes.Text = "";
            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();

            lOcultarBotones();
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

                                //dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:0.00}");


                                //GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                                //item1.FieldName = psNameColumn;
                                //item1.SummaryType = SummaryItemType.Sum;
                                //item1.DisplayFormat = "{0:0.00}";
                                //item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                                //dgvDatos.GroupSummary.Add(item1);
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

            if (int.Parse(Tag.ToString().Split(',')[0]) == 246 || int.Parse(Tag.ToString().Split(',')[0]) == 247)
            {
                dgvDatos.Columns["Item"].Width = 400;
            }
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

        private void txtAnio_Leave(object sender, EventArgs e)
        {
            lConsultarBoton();
        }

        private void lOcultarBotones()
        {

            var piAnio = string.IsNullOrEmpty(txtAnio.Text) ? 0 : int.Parse(txtAnio.Text);
            var piMes = string.IsNullOrEmpty(txtMes.Text) ? 0 : int.Parse(txtMes.Text);
            var psEstadoDepreciacion = loLogicaNegocio.gsEstadoDepreciacion(piAnio, piMes);

            if (string.IsNullOrEmpty(psEstadoDepreciacion))
            {
                if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = false;
                if (tstBotones.Items["btnGenerarDiario"] != null) tstBotones.Items["btnGenerarDiario"].Enabled = false;
                if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Enabled = true;
                if (tstBotones.Items["btnCerrar"] != null) tstBotones.Items["btnCerrar"].Enabled = false;
                if (tstBotones.Items["btnReversar"] != null) tstBotones.Items["btnReversar"].Enabled = false;
            }
            else if (Diccionario.Pendiente == psEstadoDepreciacion)
            {
                if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = true;
                if (tstBotones.Items["btnGenerarDiario"] != null) tstBotones.Items["btnGenerarDiario"].Enabled = false;
                if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Enabled = true;
                if (tstBotones.Items["btnCerrar"] != null) tstBotones.Items["btnCerrar"].Enabled = true;
                if (tstBotones.Items["btnReversar"] != null) tstBotones.Items["btnReversar"].Enabled = false;
            }
            else if (Diccionario.Cerrado == psEstadoDepreciacion)
            {
                if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = true;
                if (tstBotones.Items["btnGenerarDiario"] != null) tstBotones.Items["btnGenerarDiario"].Enabled = true;
                if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Enabled = false;
                if (tstBotones.Items["btnCerrar"] != null) tstBotones.Items["btnCerrar"].Enabled = false;
                if (tstBotones.Items["btnReversar"] != null) tstBotones.Items["btnReversar"].Enabled = true;
            }

        }
    }


}
