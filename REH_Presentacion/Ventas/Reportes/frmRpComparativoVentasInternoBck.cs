using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using GEN_Entidad;
using REH_Negocio;
using REH_Presentacion.Formularios;
using REH_Presentacion.Ventas.Reportes.Rpt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Ventas.Reportes
{
    public partial class frmRpComparativoVentasInternoBck : frmBaseTrxDev
    {
        clsNGestorConsulta loLogicaNegocio;
        BindingSource bsDatos = new BindingSource();
        Nullable<int> liFixedColumn;
        string lsTituloReporte;

        public frmRpComparativoVentasInternoBck()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNGestorConsulta();
        }

        private void frmRpComparativoVentasInterno_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbTipo, loLogicaNegocio.goConsultarComboTipoCierreVentasBck());

                lCargarEventosBotones();
                lLimpiar();

                dtpFechaInicial.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpFechaFinal.DateTime = DateTime.Now;

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
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

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

        private void lLimpiar()
        {

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            //SendKeys.Send("{TAB}");
            lConsultarBoton();
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

        private bool lbEsValido()
        {
            //if (cmbReporte.EditValue.ToString() == Diccionario.Seleccione)
            //{
            //    XtraMessageBox.Show("Seleccione Empleado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}

            return true;
        }

        private DataTable ldtValidaQuery()
        {
            string psQuery = string.Format("EXEC VTASPCOMPARATIVOVENTASINTERNOBCK '{0}', '{1}', '{2}'", dtpFechaInicial.DateTime, dtpFechaFinal.DateTime, cmbTipo.EditValue.ToString());

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


            var pIdMenu = int.Parse(Tag.ToString().Split(',')[0]);
            if (pIdMenu == 161 || pIdMenu == 162 || pIdMenu == 169 || pIdMenu == 170 || pIdMenu == 307)
            {
                dgvDatos.Columns["Ord"].Visible = false;
                dgvDatos.Columns["Orden"].Visible = false;
            }

            dgvDatos.OptionsBehavior.Editable = true;
            dgvDatos.OptionsCustomization.AllowColumnMoving = true;
            dgvDatos.OptionsView.ColumnAutoWidth = false;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            dgvDatos.OptionsView.BestFitMode = GridBestFitMode.Full;
            dgvDatos.BestFitColumns();

            if (pIdMenu == 208 || pIdMenu == 214)
            {
                DateTime pdFechaFin = dtpFechaFinal.DateTime;

                DateTime pdFechaIni = dtpFechaInicial.DateTime;

                var pdFechaFinAño = new DateTime(pdFechaFin.Year, 12, 31);

                dgvDatos.Columns["Total1"].Caption = string.Format("Venta {0}", pdFechaFin.Year - 2);
                dgvDatos.Columns["Total2"].Caption = string.Format("Venta {0}", pdFechaFin.Year - 1);
                dgvDatos.Columns["Total3"].Caption = string.Format("Venta {0}", pdFechaFin.Year);
                dgvDatos.Columns["PorcCre1"].Caption = string.Format("% Crec {0} vs {1}", pdFechaFin.Year - 1, pdFechaFin.Year - 2);
                dgvDatos.Columns["PorcCre2"].Caption = string.Format("% Crec {0} vs {1}", pdFechaFin.Year, pdFechaFin.Year - 1);
                dgvDatos.Columns["PresupuestoCurso"].Caption = string.Format("PPTO {0}-{1} {2}", clsComun.gsGetMes(pdFechaIni).Substring(0, 3), clsComun.gsGetMes(pdFechaFin).Substring(0, 3), pdFechaFin.Year);
                dgvDatos.Columns["PorcCumpCurso"].Caption = string.Format("% Cump PPTO");
                dgvDatos.Columns["PresupuestoProyec"].Caption = string.Format("PPTO {0}-{1} {2}", clsComun.gsGetMes(pdFechaFin.AddMonths(1)).Substring(0, 3), clsComun.gsGetMes(pdFechaFinAño).Substring(0, 3), pdFechaFin.Year);
                dgvDatos.Columns["PorcCumpAnual"].Caption = string.Format("% Cump PPTO Anual");
                dgvDatos.Columns["RentabilidadMesAct"].Caption = string.Format("Rentabilidad {0} {1}", clsComun.gsGetMes(pdFechaFin).Substring(0, 3), pdFechaFin.Year);
                dgvDatos.Columns["VentasMesAct"].Caption = string.Format("Ventas {0} {1}", clsComun.gsGetMes(pdFechaFin).Substring(0, 3), pdFechaFin.Year);
                dgvDatos.Columns["PorcMesAct"].Caption = string.Format("Margen {0} {1}", clsComun.gsGetMes(pdFechaFin).Substring(0, 3), pdFechaFin.Year);
                dgvDatos.Columns["RentabilidadMesAnt"].Caption = string.Format("Rentabilidad {0} {1}", clsComun.gsGetMes(pdFechaFin.AddMonths(-1)).Substring(0, 3), pdFechaFin.Year);
                dgvDatos.Columns["VentasMesAnt"].Caption = string.Format("Ventas {0} {1}", clsComun.gsGetMes(pdFechaFin.AddMonths(-1)).Substring(0, 3), pdFechaFin.Year);
                dgvDatos.Columns["PorcMesAnt"].Caption = string.Format("Margen {0} {1}", clsComun.gsGetMes(pdFechaFin.AddMonths(-1)).Substring(0, 3), pdFechaFin.Year);
                dgvDatos.Columns["RentabilidadAcum"].Caption = string.Format("Rentabilidad Acum");
                dgvDatos.Columns["PorcRentaAcum"].Caption = string.Format("Margen Acum");

            }
        }

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
                var dsVal = loLogicaNegocio.goConsultaDataSet(string.Format("EXEC VTASPCOMPARATIVOVENTASINTERNOBCK '{0}', '{1}', '{2}'", dtpFechaInicial.DateTime, dtpFechaFinal.DateTime, cmbTipo.EditValue.ToString()));
                var msgVal = "";

                if (!string.IsNullOrEmpty(msgVal))
                {
                    XtraMessageBox.Show(msgVal, "IMPORTANTE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                /****************************************************************************************************************/
                DateTime pdFechaFin = dtpFechaFinal.DateTime;

                DateTime pdFechaIni = dtpFechaInicial.DateTime;

                var pdFechaFinAño = new DateTime(pdFechaFin.Year, 12, 31);

                xrptComparativoVentas xrpt = new xrptComparativoVentas();
                string fechaInicial = dtpFechaInicial.DateTime.ToString("dd/MM/yyyy");
                string fechaFinal = dtpFechaFinal.DateTime.ToString("dd/MM/yyyy");

                var psParametro1 = string.Format("DEL {0} AL {1}", fechaInicial, fechaFinal);

                System.Data.DataSet ds1 = new System.Data.DataSet();
                var dt1 = ldtValidaQuery();
                dt1.TableName = "ComparativoVentas";
                ds1.Merge(dt1);

                var subtitulo = " ";

                if (cmbTipo.EditValue.ToString() == "T")
                {
                    subtitulo = " ";
                }
                else
                {
                    subtitulo = string.Format(" - {0}", cmbTipo.Text);
                }

                //Se establece el origen de datos del reporte (El dataset previamente leído)
                xrpt.Parameters["Titulo"].Value = psParametro1;
                xrpt.Parameters["Subtitulo"].Value = subtitulo;
                xrpt.Parameters["Total1"].Value = string.Format("Venta {0}", pdFechaFin.Year - 2);
                xrpt.Parameters["Total2"].Value = string.Format("Venta {0}", pdFechaFin.Year - 1);
                xrpt.Parameters["Total3"].Value = string.Format("Venta {0}", pdFechaFin.Year);
                xrpt.Parameters["PorcCre1"].Value = string.Format("% Crec {0} vs {1}", pdFechaFin.Year - 1, pdFechaFin.Year - 2);
                xrpt.Parameters["PorcCre2"].Value = string.Format("% Crec {0} vs {1}", pdFechaFin.Year, pdFechaFin.Year - 1);
                xrpt.Parameters["PresupuestoCurso"].Value = string.Format("PPTO {0}-{1} {2}", clsComun.gsGetMes(pdFechaIni).Substring(0, 3), clsComun.gsGetMes(pdFechaFin).Substring(0, 3), pdFechaFin.Year);
                xrpt.Parameters["PorcCumpCurso"].Value = string.Format("% Cump PPTO");
                xrpt.Parameters["PresupuestoProyec"].Value = string.Format("PPTO {0}-{1} {2}", clsComun.gsGetMes(pdFechaFin.AddMonths(1)).Substring(0, 3), clsComun.gsGetMes(pdFechaFinAño).Substring(0, 3), pdFechaFin.Year);
                xrpt.Parameters["PorcCumpAnual"].Value = string.Format("% Cump PPTO Anual");
                xrpt.Parameters["PorcMesAct"].Value = string.Format("Margen {0} {1}", clsComun.gsGetMes(pdFechaFin).Substring(0, 3), pdFechaFin.Year);
                xrpt.Parameters["PorcMesAnt"].Value = string.Format("Margen {0} {1}", clsComun.gsGetMes(pdFechaFin.AddMonths(-1)).Substring(0, 3), pdFechaFin.AddMonths(-1).Month == 12 ? pdFechaFin.Year - 1 : pdFechaFin.Year);
                xrpt.Parameters["PorcRentaAcum"].Value = string.Format("Margen Acum");

                xrpt.DataSource = ds1;
                //Se invoca la ventana que muestra el reporte.
                xrpt.RequestParameters = false;
                xrpt.Parameters["Titulo"].Visible = false;
                xrpt.Parameters["Subtitulo"].Visible = false;
                xrpt.Parameters["Total1"].Visible = false;
                xrpt.Parameters["Total2"].Visible = false;
                xrpt.Parameters["Total3"].Visible = false;
                xrpt.Parameters["PorcCre1"].Visible = false;
                xrpt.Parameters["PorcCre2"].Visible = false;
                xrpt.Parameters["PresupuestoCurso"].Visible = false;
                xrpt.Parameters["PorcCumpCurso"].Visible = false;
                xrpt.Parameters["PresupuestoProyec"].Visible = false;
                xrpt.Parameters["PorcCumpAnual"].Visible = false;
                xrpt.Parameters["PorcMesAct"].Visible = false;
                xrpt.Parameters["PorcMesAnt"].Visible = false;
                xrpt.Parameters["PorcRentaAcum"].Visible = false;

                using (ReportPrintTool printTool = new ReportPrintTool(xrpt))
                {
                    printTool.ShowRibbonPreviewDialog();
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
    }
}
