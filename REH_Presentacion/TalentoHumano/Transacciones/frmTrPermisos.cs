using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.TalentoHumano.Transacciones
{
    public partial class frmTrPermisos : frmBaseTrxDev
    {

        #region Variables
        clsNHorasExtras loLogicaNegocio;
        BindingSource bsDatos = new BindingSource();
        bool lbCargado = false;
        #endregion

        #region Eventos
        public frmTrPermisos()
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
                    item.Descripcion = item.Descripcion.Replace("SEGUNDA QUINCENA", clsComun.gsGetMes(int.Parse(item.Descripcion.Substring(5, 2))));
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
                    string psMsg = loLogicaNegocio.gsGenerarDescuentoHaberes(int.Parse(cmbPeriodo.EditValue.ToString()),dtpFechaInicio.Value, dtpFechaFin.Value, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                    var dt = loLogicaNegocio.gdtConsultaResumenPermisos(dtpFechaInicio.Value, dtpFechaFin.Value);
                    dt.TableName = "DstoHaberes";
                    ds.Merge(dt);
                    if (dt.Rows.Count > 0)
                    {
                        xrptResumenPermisos xrpt = new xrptResumenPermisos();
                        xrpt.DataSource = ds;

                        //Se establece el origen de datos del reporte (El dataset previamente leído)
                        xrpt.Parameters["tsTitulo"].Value = string.Format("LISTADO DE PERMISOS DESDE {0} A {1}.", dtpFechaInicio.Value.ToString("dd/MM/yyyy"), dtpFechaFin.Value.ToString("dd/MM/yyyy"));
                        xrpt.DataSource = ds;
                        //Se invoca la ventana que muestra el reporte.
                        xrpt.RequestParameters = false;
                        xrpt.Parameters["tsTitulo"].Visible = false;


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
                lChangeValue();
                lBuscar();
                
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
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnProcesar"] != null) tstBotones.Items["btnProcesar"].Click += btnCalcular_Click;
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
                DataTable dt = loLogicaNegocio.gdtConsultaDescuentoHaberes(dtpFechaInicio.Value, dtpFechaFin.Value);

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
                /*
                if (poListaFecha.Count > 0)
                {
                    dtpFechaInicio.Value = poListaFecha.Min(x=>x);
                    dtpFechaFin.Value = poListaFecha.Max(x => x);
                    dtpFechaInicio.Checked = false;
                    dtpFechaFin.Checked = false;
                }
                */
                dtpFechaInicio.Checked = false;
                dtpFechaFin.Checked = false;

            }

        }

        private void lChangeValue()
        {
            if (lbCargado)
            {
                if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                {
                    lblEstado.Text = loLogicaNegocio.gsGetDescripcionEstadoNomina(int.Parse(cmbPeriodo.EditValue.ToString()));
                    DateTime pdFechaInicial;
                    DateTime pdFechaFinal;

                    loLogicaNegocio.gbFechasGuardadasDescuentoHabares(int.Parse(cmbPeriodo.EditValue.ToString()), out pdFechaInicial, out pdFechaFinal);
                    dtpFechaInicio.Value = pdFechaInicial;
                    dtpFechaFin.Value = pdFechaFinal;
                    dtpFechaInicio.Checked = false;
                    dtpFechaFin.Checked = false;
                    //lblRangoFechas.Text = string.Format("{0} A {1}.", pdFechaInicial.ToString("dd/MM/yyyy"), pdFechaFinal.ToString("dd/MM/yyyy"));
                }
                else
                {
                    lblEstado.Text = string.Empty;
                    dtpFechaInicio.Value = DateTime.Now;
                    dtpFechaFin.Value = DateTime.Now;
                    dtpFechaInicio.Checked = false;
                    dtpFechaFin.Checked = false;
                }
                lBuscar();
            }
        }

        #endregion


    }
}
