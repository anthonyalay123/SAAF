using DevExpress.XtraGrid.Views.Grid;
using REH_Negocio;
using REH_Presentacion.Comun;
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
using System.Net.Mail;
using DevExpress.XtraPrinting;
using System.Configuration;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.Utils;
using System.Drawing;

namespace REH_Presentacion.Ventas.Reportes
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 07/04/2020
    /// Formulario de reporte de Cumplimiento de Kilos Litros
    /// </summary>
    public partial class frmRpCumplimientoKilosLitrosGrupoGV : frmBaseTrxDev
    {

        #region Variables

        clsNGestorConsulta loLogicaNegocio;
        string lsQuery;
        string lsTituloReporte;
        string lsDataSet;
        bool lbLandSpace;
        Nullable<int> liFixedColumn;
        public int lIdGestorConsulta;

        decimal Presupuesto;
        decimal Ventas;
        #endregion

        #region Eventos
        public frmRpCumplimientoKilosLitrosGrupoGV()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNGestorConsulta();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                //cmbReporte.Focus();
                //clsComun.gLLenarCombo(ref cmbReporte, loLogicaNegocio.goConsultarComboReportes());
                //clsComun.gLLenarCombo(ref cmbZonas, loLogicaNegocio.goConsultarZonasSAAF(), false, true);
                var piGestor = loLogicaNegocio.giConsultaId(int.Parse(Tag.ToString().Split(',')[0]));
                if (piGestor != 0) lIdGestorConsulta = piGestor;
                lCargarEventosBotones();
                lLimpiar();
                //loLogicaNegocio.ProbarConexion();
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
                        clsComun.gSaveFile(gcDatos, Text + ".pdf", psFilter,"PDF");
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

         
            Cursor.Current = Cursors.Default;
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
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnExportarPdf"] != null) tstBotones.Items["btnExportarPdf"].Click += btnExportarPdf_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

            //txtPar1.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar2.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar3.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar4.KeyDown += new KeyEventHandler(EnterEqualTab);


        }

        private bool lbEsValido()
        {
            if (dtpFechaInicial.Value.Year != dtpFechaFinal.Value.Year)
            {
                XtraMessageBox.Show("Ingrese rango de fechas del mismo año", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (dtpFechaInicial.Value.Year > dtpFechaFinal.Value.Year)
            {
                XtraMessageBox.Show("La fecha inicial no puede ser mayor que la fecha final", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void lLimpiar(bool tbSoloDetalle = false)
        {
            if (!tbSoloDetalle)
            {
                gcDatos.DataSource = null;
                dgvDatos.Columns.Clear();
                dtpFechaInicial.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpFechaFinal.Value = DateTime.Now;
                //if ((cmbZonas.Properties.DataSource as IList).Count > 0) cmbZonas.ItemIndex = 0;
                lsQuery = string.Empty;

            }
            
            
        }

        private DataTable ldtValidaQuery()
        {
            return loLogicaNegocio.goConsultaDataTable(string.Format("EXEC [dbo].[VTA_SP_CUMPLIMIENTO_KILOS_LITROS_X_GRUPO_GV] '{0}', '{1}'", dtpFechaInicial.Value, dtpFechaFinal.Value));
        }

        private void lBuscar()
        {

            if (lbEsValido())
            {
                gcDatos.DataSource = null;
                dgvDatos.Columns.Clear();

                DateTime psFechaIni = dtpFechaInicial.Value;
                DateTime psFechaFinal = dtpFechaFinal.Value;
                var dt = ldtValidaQuery();

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
                    
                    gridBandIni.Columns.Add(dgvDatos.Columns[0]); // Grupo
                    acum = 1;

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
                        gridBandDin.Caption = string.Format("{0} - {1}", clsComun.gsGetMes(pdFechaCalculo), pdFechaCalculo.Year);

                        dgvDatos.Columns[acum].Caption = dgvDatos.Columns[acum].FieldName.Substring(0, dgvDatos.Columns[acum].FieldName.Length - 9);
                        dgvDatos.Columns[acum + 1].Caption = dgvDatos.Columns[acum + 1].FieldName.Substring(0, dgvDatos.Columns[acum + 1].FieldName.Length - 9);
                        dgvDatos.Columns[acum + 2].Caption = dgvDatos.Columns[acum + 2].FieldName.Substring(0, dgvDatos.Columns[acum + 2].FieldName.Length - 9);
                        
                        gridBandDin.Columns.Add(dgvDatos.Columns[acum]);
                        gridBandDin.Columns.Add(dgvDatos.Columns[acum + 1]);
                        gridBandDin.Columns.Add(dgvDatos.Columns[acum + 2]);
                        

                        acum = acum + 3;
                        pdFechaCalculo = pdFechaCalculo.AddMonths(1);

                        dgvDatos.Bands.Add(gridBandDin);
                    }

                    GridBand gridBandTot = new GridBand();
                    gridBandTot.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    gridBandTot.AppearanceHeader.Options.UseTextOptions = true;
                    gridBandTot.AppearanceHeader.Options.UseFont = true;
                    Font fontTot = new Font(gridBandTot.AppearanceHeader.Font, FontStyle.Bold);
                    gridBandTot.AppearanceHeader.Font = fontTot;

                    gridBandTot.Caption = "TOTAL ACUMULADO";

                    gridBandTot.Columns.Add(dgvDatos.Columns[acum]);
                    gridBandTot.Columns.Add(dgvDatos.Columns[acum + 1]);
                    gridBandTot.Columns.Add(dgvDatos.Columns[acum + 2]);
                    
                    dgvDatos.Bands.Add(gridBandTot);

                    clsComun.gFormatearColumnasGrid(dgvDatos, true);
                    dgvDatos.BestFitColumns();
                }
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

        private void frmRpCumplimientoKilosLitros_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lConsultarBoton();
            }
        }

        private void lEnvioAutomatio()
        {
            
            Cursor.Current = Cursors.WaitCursor;

           
            Cursor.Current = Cursors.Default;

           
        }

        private void dgvDatos_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;

                var item = (e.Item as GridSummaryItem);
                var columna = item.FieldName;
                var fecha = columna.Contains("20") ? columna.Substring(columna.Length - 9, 9) : "";

                // Initialization. 
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    Presupuesto = 0;
                    Ventas = 0;
                }

                // Calculation.
                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    Ventas += Convert.ToDecimal(view.GetRowCellValue(e.RowHandle, string.Format("#Ventas{0}", fecha)));
                    Presupuesto += Convert.ToDecimal(view.GetRowCellValue(e.RowHandle, string.Format("#Presupuesto{0}", fecha)));
                }

                // Finalization. 
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    e.TotalValue = Presupuesto > 0 ? Ventas / Presupuesto : 0;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
