using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using REH_Negocio.Transacciones;
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

namespace REH_Presentacion.TalentoHumano.Transacciones
{
    /// <summary>
    /// Formulario para actualizar permisos por horas
    /// Desarrollado por Victor Arevalo
    /// Fecha: 24/08/2021
    /// </summary>
    public partial class frmTrValidaPermisoPorHora : frmBaseTrxDev
    {
        #region Variables
        clsNAsistencia loLogicaNegocio;
        RepositoryItemLookUpEdit poCmbTipoPermiso;
        bool lbCargado;
        BindingSource bsDatos = new BindingSource();
        BindingSource bsDatosPermisos = new BindingSource();
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmTrValidaPermisoPorHora()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNAsistencia();
        }

        private void frmTrAsistencia_Load(object sender, EventArgs e)
        {
            try
            {
                lBuscar();
                lCargarEventosBotones();
                lAsignarBsGrid();
                lbCargado = true;
                clsComun.gLLenarComboGrid(ref dgvPermisosPorHoras, loLogicaNegocio.goConsultarComboTipoPermisoPorHoras(), "CodigoTipoPermiso", false);

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
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvPermisosPorHoras.PostEditor();
                var poLista = (List<SpPermisoPorHoras>)bsDatosPermisos.DataSource;
                if(poLista.Count() > 0)
                {
                    string psMsg = loLogicaNegocio.gsGuardaPermisoPorHoras(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    if(string.IsNullOrEmpty(psMsg))
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
                
                DataSet ds = new DataSet();
                var dt = loLogicaNegocio.gdtRptAsistencia(dtpFechaInicio.Value);
                dt.TableName = "Asistencia";
                ds.Merge(dt);
                if (dt.Rows.Count > 0)
                {
                    xrptAsistenciaDetalle xrpt = new xrptAsistenciaDetalle();
                    xrpt.DataSource = ds;
                    //Se invoca la ventana que muestra el reporte.

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
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {

                dgvDatos.PostEditor();
                List<SpPermisoPorHorasExport> poLista = new List<SpPermisoPorHorasExport>();

                poLista = ((List<SpPermisoPorHoras>)bsDatosPermisos.DataSource).Select(x => new SpPermisoPorHorasExport
                {
                    Empleado = x.Empleado,
                    Fecha = x.Fecha,
                    HoraFin = x.HoraFin,
                    HoraInicio = x.HoraInicio,
                    Novedad = x.Novedad
                }).ToList();

              

                if (poLista.Count > 0)
                {
                    GridControl gc = new GridControl();
                    BindingSource bs = new BindingSource();
                    GridView dgv = new GridView();
                    gc.Visible = false;

                    gc.DataSource = bs;
                    gc.MainView = dgv;
                    gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                    dgv.GridControl = gc;
                    this.Controls.Add(gc);
                    bs.DataSource = poLista;
                    dgv.BestFitColumns();
                    
                    clsComun.gSaveFile(gc, Text, "Files(*.xlsx;)|*.xlsx;");
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

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtpFecha_Leave(object sender, EventArgs e)
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

        private void dtpFecha_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode.Equals(Keys.Enter))
                    SendKeys.Send("{TAB}");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtFiltroEmpleado_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string psValor = txtFiltroEmpleado.Text.Trim();
                dgvPermisosPorHoras.ActiveFilterString = "Contains([Empleado], '" + psValor + "')";
                dgvDatos.ActiveFilterString = "Contains([Empleado], '" + psValor + "')";

                if (string.IsNullOrEmpty(psValor))
                {
                    dgvPermisosPorHoras.ActiveFilterEnabled = false;
                    dgvDatos.ActiveFilterEnabled = false;
                }
                else
                {
                    dgvPermisosPorHoras.ActiveFilterEnabled = true;
                    dgvDatos.ActiveFilterEnabled = true;
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvAsistencia_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                
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
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
        }

        private void lLimpiar()
        {

        }

        private void lAsignarBsGrid()
        {
            
            var polistaAsisBiom = new List<SpDetalleMarcaciones>();
            bsDatos.DataSource = polistaAsisBiom;
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            dgvDatos.OptionsBehavior.Editable = false;
            //dgvDatos.Columns[0].Visible = false; // IdPersona
            dgvDatos.Columns["Fecha"].Width = 30;
            dgvDatos.Columns["Marcacion"].Width = 23;

            var polistaAsisDef = new List<SpPermisoPorHoras>();

            bsDatosPermisos.DataSource = polistaAsisDef;
            gcPermisosPorHoras.DataSource = bsDatosPermisos;

            dgvPermisosPorHoras.OptionsBehavior.Editable = true;
            dgvPermisosPorHoras.OptionsCustomization.AllowColumnMoving = false;
            dgvPermisosPorHoras.OptionsView.ColumnAutoWidth = false;
            dgvPermisosPorHoras.OptionsView.ShowAutoFilterRow = true;

            dgvPermisosPorHoras.Columns["Id"].Visible = false;
            dgvPermisosPorHoras.Columns["IdPersona"].Visible = false;
            dgvPermisosPorHoras.Columns["Novedad"].Visible = false;
            
            dgvPermisosPorHoras.Columns["CodigoTipoPermiso"].Caption = "Novedad";

            dgvPermisosPorHoras.Columns["Empleado"].OptionsColumn.AllowEdit = false;
            dgvPermisosPorHoras.Columns["Fecha"].OptionsColumn.AllowEdit = false;
            dgvPermisosPorHoras.Columns["Novedad"].OptionsColumn.AllowEdit = false;

            dgvPermisosPorHoras.Columns["Empleado"].Width = 200; // Nombre
            dgvPermisosPorHoras.Columns["CodigoTipoPermiso"].Width = 200; // Permiso
            dgvPermisosPorHoras.FixedLineWidth = 1;
            dgvPermisosPorHoras.Columns["Empleado"].Fixed = FixedStyle.Left;

            dgvPermisosPorHoras.Columns["HoraInicio"].Width = 85;
            dgvPermisosPorHoras.Columns["HoraFin"].Width = 85;

            (dgvPermisosPorHoras.Columns["HoraInicio"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            (dgvPermisosPorHoras.Columns["HoraInicio"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.EditMask = "HH:mm:ss";
            (dgvPermisosPorHoras.Columns["HoraInicio"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.UseMaskAsDisplayFormat = true;

            (dgvPermisosPorHoras.Columns["HoraFin"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            (dgvPermisosPorHoras.Columns["HoraFin"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.EditMask = "HH:mm:ss";
            (dgvPermisosPorHoras.Columns["HoraFin"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.UseMaskAsDisplayFormat = true;

        }

        private void lBuscar()
        {

            if (lbCargado)
            {

                var pdFechaInicio = dtpFechaInicio.Value;
                var pdFechaFin = dtpFechaFin.Value;

                var bsDatos = new BindingSource();
                var polistaAsisBiom = loLogicaNegocio.goConsultarDetalleMarcaciones(pdFechaInicio, pdFechaFin);
                bsDatos.DataSource = polistaAsisBiom;
                gcDatos.DataSource = bsDatos;


                bsDatosPermisos.DataSource = loLogicaNegocio.goConsultarPermisosPorHoras(pdFechaInicio, pdFechaFin); 
                gcPermisosPorHoras.DataSource = bsDatosPermisos;

            }

        }

        #endregion


        

    }
}
