using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using GEN_Entidad;
using GEN_Negocio;
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

namespace REH_Presentacion.Administración.Reportes
{
    public partial class frmRptCostoSumunistro : frmBaseTrxDev
    {
        BindingSource bsDatos = new BindingSource();
        clsNCostoSumunistros loLogicaNegocio = new clsNCostoSumunistros();
        public int lIdGestorConsulta;
        Nullable<int> liFixedColumn;
        string lsDataSet;

        public frmRptCostoSumunistro()
        {
            InitializeComponent();
            cmbTipo.EditValueChanged += cmbTipo_EditValueChanged;
        }

        private void frmRptCostoSumunistro_Load(object sender, EventArgs e)
        {
            clsComun.gLLenarCombo(ref cmbTipo, loLogicaNegocio.goConsultarComboTipoVisualizacionCosto());

            var piGestor = loLogicaNegocio.giConsultaId(int.Parse(Tag.ToString().Split(',')[0]));
            if (piGestor != 0) lIdGestorConsulta = piGestor;
            lCargarEventosBotones();
            if (!pnc1.Visible)
            {
                lBuscar();
            }

            dtpFechaInicial.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaFinal.DateTime = DateTime.Now;

            dgvDatos.OptionsView.ShowGroupPanel = true;
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            //if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string fechaInicial = dtpFechaInicial.DateTime.ToString("ddMMyyyy");
                string fechaFinal = dtpFechaFinal.DateTime.ToString("ddMMyyyy");
                dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    DataTable poLista = (DataTable)bsDatos.DataSource;
                    if (poLista != null && poLista.Rows.Count > 0)
                    {
                        string psFilter = "Files(*.xlsx;)|*.xlsx;";
                        clsComun.gSaveFile(gcDatos, "COSTO DE SUMUNISTRO_"+ cmbTipo.Text + "_" + fechaInicial + "_" + fechaFinal + ".xlsx", psFilter);
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
            return true;
        }

        private void lBuscar()
        {
            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();
            DateTime psFechaIni = dtpFechaInicial.DateTime;
            DateTime psFechaFinal = dtpFechaFinal.DateTime;

            DataTable dt = ldtValidaQuery();

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

                if (cmbTipo.EditValue.ToString() == "CEI")
                {
                    gridBandIni.Columns.Add(dgvDatos.Columns[0]); // Empleado
                    gridBandIni.Columns.Add(dgvDatos.Columns[1]); // Centro Costo
                    acum = 2;
                }
                else if (cmbTipo.EditValue.ToString() == "CEC")
                {
                    gridBandIni.Columns.Add(dgvDatos.Columns[0]); // Centro Costo
                    acum = 1;
                } else
                {
                    gridBandIni.Columns.Add(dgvDatos.Columns[0]); // Empleado
                    gridBandIni.Columns.Add(dgvDatos.Columns[1]); // Centro Costo
                    gridBandIni.Columns.Add(dgvDatos.Columns[2]); // Centro Costo
                    gridBandIni.Columns.Add(dgvDatos.Columns[3]); // Centro Costo
                    acum = 4;
                }


                if (cmbTipo.EditValue.ToString() != "DET") {

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

                        gridBandDin.Columns.Add(dgvDatos.Columns[acum]);
                        gridBandDin.Columns.Add(dgvDatos.Columns[acum + 1]);

                        acum = acum + 2;
                        pdFechaCalculo = pdFechaCalculo.AddMonths(1);

                        dgvDatos.Bands.Add(gridBandDin);
                    }

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
                //gridBandTot.Columns.Add(dgvDatos.Columns[acum + 4]);

                dgvDatos.Bands.Add(gridBandTot);

                clsComun.gFormatearColumnasGrid(dgvDatos);
                dgvDatos.BestFitColumns();
            }

            //bsDatos.DataSource = dt;
            //gcDatos.DataSource = bsDatos;
            //dgvDatos.PopulateColumns();

            //clsComun.gFormatearColumnasGrid(dgvDatos);
            //clsComun.gOrdenarColumnasGrid(dgvDatos);

            //dgvDatos.OptionsView.ShowGroupPanel = true;
            //dgvDatos.OptionsCustomization.AllowColumnMoving = true;

        }

        private DataTable ldtValidaQuery()
        {
            string fechaInicial = dtpFechaInicial.DateTime.ToString("yyyyMMdd");
            string fechaFinal = dtpFechaFinal.DateTime.ToString("yyyyMMdd");

            string psQuery = string.Format("EXEC ADMSPCOSTOSUMUNISTRO '{0}','{1}','{2}'", fechaInicial, fechaFinal, cmbTipo.EditValue.ToString());
            return loLogicaNegocio.goConsultaDataTable(psQuery);
        }

        private void frmRptCostoSumunistro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

        private void cmbTipo_EditValueChanged(object sender, EventArgs e)
        {
            lConsultarBoton();
        }
    }
}
