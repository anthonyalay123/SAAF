using DevExpress.XtraGrid.Views.Grid;
using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraGrid.Columns;
using System.Configuration;
using System.IO;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
using GEN_Entidad.Entidades;

namespace REH_Presentacion.TalentoHumano.Reportes
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 09/11/2020
    /// Formulario de gestor de consultas
    /// </summary>
    public partial class frmReporteAceptaRol : frmBaseTrxDev
    {

        #region Variables

        clsNGestorConsulta loLogicaNegocio;
        string lsQuery;
        string lsTituloReporte;
        string lsDataSet;
        public int lIdGestorConsulta;
        #endregion

        #region Eventos
        public frmReporteAceptaRol()
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
                //var piGestor = loLogicaNegocio.giConsultaId(int.Parse(Tag.ToString().Split(',')[0]));
                //if (piGestor != 0) lIdGestorConsulta = piGestor;
                lCargarEventosBotones();
                lCargarControles();
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
                    var poLista = (List<LogAceptaRolEmpleado>)bsDatos.DataSource;
                    if (poLista.Count > 0)
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

        private void txtPar1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData.Equals(Keys.Enter)) lConsultarBoton();
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
            

            lblPar3.Visible = false;
            txtPar3.Visible = false;
        }

        private DataTable ldtValidaQuery()
        {
            DataTable dt = new DataTable();

            string psQuery = lsQuery;
            bool pbValido = true;

            if (psQuery.Contains("#PARAMETRO1"))
            {
                if (!string.IsNullOrEmpty(txtPar1.Text))
                {
                    psQuery = psQuery.Replace("#PARAMETRO1", txtPar1.Text);
                }
                else
                {
                    XtraMessageBox.Show("Es necesario ingresar dato:" + lblPar1.Text, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pbValido = false;
                }
            }

            if (psQuery.Contains("#PARAMETRO2"))
            {
                if (!string.IsNullOrEmpty(txtPar2.Text))
                {
                    psQuery = psQuery.Replace("#PARAMETRO2", txtPar2.Text);
                }
                else
                {
                    XtraMessageBox.Show("Es necesario ingresar dato:" + lblPar2.Text, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pbValido = false;
                }
            }

            if (psQuery.Contains("#PARAMETRO3"))
            {
                if (!string.IsNullOrEmpty(txtPar3.Text))
                {
                    psQuery = psQuery.Replace("#PARAMETRO3", txtPar3.Text);
                }
                else
                {
                    XtraMessageBox.Show("Es necesario ingresar dato:" + lblPar3.Text, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pbValido = false;
                }
            }

            if (pbValido)
            {
               dt = loLogicaNegocio.goConsultaDataTable(psQuery);
            }

            return dt;
        }

        private void lBuscar()
        {
            
            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();

            var pdFechaIni = Convert.ToDateTime(txtPar1.Text);
            var pdFechaFin = Convert.ToDateTime(txtPar2.Text);

            var dt = loLogicaNegocio.goConsultarAceptaRol(pdFechaIni, pdFechaFin);
            
            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;
            dgvDatos.PopulateColumns();

            //for (int i = 0; i < dgvDatos.Columns.Count; i++)
            //{
               
            //    if (i > 1)
            //    {
            //        if (dt.Columns[i].DataType == typeof(decimal))
            //        {
            //            var psNameColumn = dgvDatos.Columns[i].FieldName;
            //            if (psNameColumn.ToUpper().Contains("PORCENTAJE") || psNameColumn.ToUpper().Contains("PESO"))
            //            {
            //                dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
            //                dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //                dgvDatos.Columns[i].DisplayFormat.FormatString = "d6";
            //                dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:0.000000}");


            //                GridGroupSummaryItem item1 = new GridGroupSummaryItem();
            //                item1.FieldName = psNameColumn;
            //                item1.SummaryType = SummaryItemType.Sum;
            //                item1.DisplayFormat = "{0:0.000000}";
            //                item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
            //                dgvDatos.GroupSummary.Add(item1);
            //            }
            //            else
            //            {
            //                dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
            //                dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //                dgvDatos.Columns[i].DisplayFormat.FormatString = "c2";
            //                dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");


            //                GridGroupSummaryItem item1 = new GridGroupSummaryItem();
            //                item1.FieldName = psNameColumn;
            //                item1.SummaryType = SummaryItemType.Sum;
            //                item1.DisplayFormat = "{0:c2}";
            //                item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
            //                dgvDatos.GroupSummary.Add(item1);
            //            }
            //        }
                   
            //    }

            //}

            dgvDatos.OptionsBehavior.Editable = false;
            dgvDatos.OptionsCustomization.AllowColumnMoving = false;
            dgvDatos.OptionsView.ColumnAutoWidth = false;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            dgvDatos.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            dgvDatos.BestFitColumns();
        }

        private void lCargarControles()
        {
            lLimpiar();
            lFormatControl(lblPar1, txtPar1, new GestorConsultaDetalle() { Nombre = "Fecha Inicio", Longitud = 10, TipoDato = "DATE" });
            lFormatControl(lblPar2, txtPar2, new GestorConsultaDetalle() { Nombre = "Fecha Fin", Longitud = 10, TipoDato = "DATE" });
            
        }

        private void lFormatControl(Label lbl, TextEdit txt, GestorConsultaDetalle item)    
        {
            lbl.Visible = true;
            lbl.Text = item.Nombre;
            txt.Visible = true;
            txt.Properties.MaxLength = item.Longitud;
            if (item.TipoDato.ToUpper() == "DATE" || item.TipoDato.ToUpper() == "DATETIME")
            {
                txt.Properties.Mask.EditMask = "d";
                txt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                txt.EditValue = DateTime.Now.Date;

            }
            else if (item.TipoDato.ToUpper() == "DECIMAL")
            {
                txt.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            }
            else if (item.TipoDato.ToUpper() == "INT")
            {
                txt.KeyPress += new KeyPressEventHandler(SoloNumeros);
            }
        }


        private void lConsultarBoton()
        {
            try
            {
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
