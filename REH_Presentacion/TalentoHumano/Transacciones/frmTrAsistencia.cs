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
using reporte;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace REH_Presentacion.TalentoHumano.Transacciones
{
    /// <summary>
    /// Formulario para registrar la asistencia
    /// Desarrollado por Victor Arevalo
    /// Fecha: 06/08/2021
    /// </summary>
    public partial class frmTrAsistencia : frmBaseTrxDev
    {
        #region Variables
        clsNAsistencia loLogicaNegocio;
        RepositoryItemLookUpEdit poCmbTipoPermiso;
        bool lbCargado;
        BindingSource bsDatos = new BindingSource();
        BindingSource bsDatosAsistencia = new BindingSource();
        List<SpAsistenciaDetalle> loDatos = new List<SpAsistenciaDetalle>();
        bool pbAplicaUsuarioDepatamentoAsignado;
        DateTime ldtFecha = new DateTime();
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmTrAsistencia()
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
                pbAplicaUsuarioDepatamentoAsignado = loLogicaNegocio.gbAplicaUsuarioDepartamentoAsignado(clsPrincipal.gsUsuario, int.Parse(Tag.ToString().Split(',')[0]));
                lAsignarBsGrid();
                lbCargado = true;
                lblMensaje.Text = string.Empty;
                lblMensajeGuardado.Text = string.Empty;
                clsComun.gLLenarComboGrid(ref dgvAsistencia, loLogicaNegocio.goConsultarComboDepartamento(), "CodigoDepartamento", false);
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
        /// Evento del botón Grabar, Guarda datos del formulario a el objeto de BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvAsistencia.PostEditor();
                var poLista = (List<SpAsistenciaDetalle>)bsDatosAsistencia.DataSource;
                if(poLista.Count() > 0)
                {

                    string Msg = ""; 
                    foreach (var item in poLista)
                    {
                        if (item.HoraSalida != null && item.HoraLlegada != null)
                        {
                            if (item.HoraSalida < item.HoraLlegada)
                            {
                                Msg = string.Format("{0}Empleado: {1} tiene una hora de llegada: {2} y hora de salida: {3}\n", Msg, item.NombreCompleto, item.HoraLlegada, item.HoraSalida);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(Msg))
                    {
                        DialogResult dialogResult = XtraMessageBox.Show(Msg, "Existen Inconsistencias!!! ¿Desea Corregir?", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        if (dialogResult == DialogResult.Yes)
                        {
                            return;
                        }
                    }

                    string psMsg = loLogicaNegocio.gsGuardar(dtpFecha.Value, poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, pbAplicaUsuarioDepatamentoAsignado);
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
        /// Evento del botón Eliminar, Elimina la información de base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                
                DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    string psMsg = loLogicaNegocio.gsEliminarAsistencia(dtpFecha.Value, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
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

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {

                dgvDatos.PostEditor();
                List<SpAsistenciaDetalleExport> poLista = new List<SpAsistenciaDetalleExport>();

                poLista = ((List<SpAsistenciaDetalle>)bsDatosAsistencia.DataSource).Select(x => new SpAsistenciaDetalleExport
                {
                    AplicaHE = x.AplicaHE,
                    AplicaHEAntesEntrada = x.AplicaHE,
                    Asistencia = x.Asistencia,
                    Departamento = x.Departamento,
                    Novedad = x.DesPermiso,
                    HoraLlegada = x.HoraLlegada,
                    HoraSalida = x.HoraSalida,
                    NombreCompleto = x.NombreCompleto,
                    NumeroIdentificacion = x.NumeroIdentificacion,
                    TiempoAtraso = x.TiempoAtraso,
                    AplicaTiempoGraciaPostSalida = x.AplicaTiempoGraciaPostSalida,
            

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

        private void cmbTipoPermiso_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                dgvAsistencia.PostEditor();
                dgvAsistencia.SetFocusedRowCellValue("IdProvincia", null);
                dgvAsistencia.SetFocusedRowCellValue("IdCanton", 0);

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
                var dt = loLogicaNegocio.gdtRptAsistencia(dtpFecha.Value);
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
        
        private void btnBuscar_Click_1(object sender, EventArgs e)
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
                dgvAsistencia.ActiveFilterString = "Contains([NombreCompleto], '" + psValor + "')";
                dgvDatos.ActiveFilterString = "Contains([Empleado], '" + psValor + "')";

                if (string.IsNullOrEmpty(psValor))
                {
                    dgvAsistencia.ActiveFilterEnabled = false;
                    dgvDatos.ActiveFilterEnabled = false;
                }
                else
                {
                    dgvAsistencia.ActiveFilterEnabled = true;
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
                if (e.Column.FieldName == "HoraLlegada")
                {
                    int piIndex = dgvAsistencia.GetFocusedDataSourceRowIndex();
                    var poLista = (List<SpAsistenciaDetalle>)bsDatosAsistencia.DataSource;
                    if (poLista != null && poLista.Count() > 0)
                    {

                        TimeSpan poTiempoAtraso;
                        int piMinutosAtraso;
                        var poFila = poLista[piIndex];
                        if (poFila.HoraLlegada.Value.Days > 0)
                        {
                            poFila.HoraLlegada = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Month, poFila.HoraLlegada.Value.Hours, poFila.HoraLlegada.Value.Minutes, poFila.HoraLlegada.Value.Seconds, 0).TimeOfDay;
                        }

                        clsComun.gObtenerDatosAtraso(poFila.HoraLlegada.Value, out poTiempoAtraso, out piMinutosAtraso);
                        poFila.TiempoAtraso = poTiempoAtraso;
                        poFila.MinutosAtraso = piMinutosAtraso;
                    }
                }
                else if (e.Column.FieldName == "HoraSalida")
                {
                    int piIndex = dgvAsistencia.GetFocusedDataSourceRowIndex();
                    var poLista = (List<SpAsistenciaDetalle>)bsDatosAsistencia.DataSource;
                    if (poLista != null && poLista.Count() > 0)
                    {
                        var poFila = poLista[piIndex];
                        if (poFila.HoraSalida.Value.Days > 0)
                        {
                            poFila.HoraSalida = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Month, poFila.HoraSalida.Value.Hours, poFila.HoraSalida.Value.Minutes, poFila.HoraSalida.Value.Seconds, 0).TimeOfDay;
                        }
                    }
                }
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
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
        }

        private void lLimpiar()
        {
            bsDatos.DataSource = new List<SpAsistenciaDetalleBiometrico>();
            gcDatos.DataSource = bsDatos;
            bsDatosAsistencia.DataSource = new List<SpAsistenciaDetalle>();
            gcAsistencia.DataSource = bsDatosAsistencia;

            lblFechaCreacion.Text = "";
            lblUsuarioCreacion.Text = "";
            lblFechaMod.Text = "";
            lblUltimoUsuarioMod.Text = "";
            lblMensaje.Text = "";
            lblMensajeGuardado.Text = "";

            loDatos = new List<SpAsistenciaDetalle>();

        }

        private void lAsignarBsGrid()
        {
            
            var polistaAsisBiom = new List<SpAsistenciaDetalleBiometrico>();
            bsDatos.DataSource = polistaAsisBiom;
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            dgvDatos.OptionsBehavior.Editable = false;
            dgvDatos.Columns["Fecha"].Visible = false; 
            dgvDatos.Columns["HoraLlegada"].Width = 22; 
            dgvDatos.Columns["HoraSalida"].Width = 22; 


            
            var polistaAsisDef = new List<SpAsistenciaDetalle>();

            bsDatosAsistencia.DataSource = polistaAsisDef;
            gcAsistencia.DataSource = bsDatosAsistencia;

            clsComun.gLLenarComboGridOut(ref dgvAsistencia, loLogicaNegocio.goConsultarComboTipoPermiso(), "Permiso", out poCmbTipoPermiso, true);
            poCmbTipoPermiso.EditValueChanged += cmbTipoPermiso_EditValueChanged;

            dgvAsistencia.OptionsBehavior.Editable = true;
            dgvAsistencia.OptionsCustomization.AllowColumnMoving = false;
            dgvAsistencia.OptionsView.ColumnAutoWidth = false;
            dgvAsistencia.OptionsView.ShowAutoFilterRow = true;

            dgvAsistencia.Columns["IdAsistenciaDetalle"].Visible = false; // IdAsistenciaDetalle
            dgvAsistencia.Columns["IdAsistencia"].Visible = false; // IdAsistencia
            dgvAsistencia.Columns["IdPersona"].Visible = false; // IdPersona
            dgvAsistencia.Columns["NumeroIdentificacion"].Visible = false; // NumeroIdentificacion
            dgvAsistencia.Columns["MinutosAtraso"].Visible = false; // MinutosAtraso
            dgvAsistencia.Columns["DesPermiso"].Visible = false; // DesPermiso
            dgvAsistencia.Columns["MinutosExtras"].Visible = false; // MinutosExtras
            dgvAsistencia.Columns["Departamento"].Visible = false; // CodigoDepartamento

            dgvAsistencia.Columns["AplicaHEAntesEntrada"].Caption = "Aplica H/ E.Pre Jornada.";
            dgvAsistencia.Columns["CodigoDepartamento"].Caption = "Departamento";
            dgvAsistencia.Columns["Permiso"].Caption = "Observaciones";


            if (pbAplicaUsuarioDepatamentoAsignado)
            {
                dgvAsistencia.Columns["CodigoDepartamento"].Visible = false; // Departamento
                //dgvAsistencia.Columns["Asistencia"].Visible = false; // Asistencia
                dgvAsistencia.Columns["Permiso"].Visible = false; // Asistencia
                dgvAsistencia.Columns["AplicaHE"].OptionsColumn.AllowEdit = false;
            }

            dgvAsistencia.Columns["NombreCompleto"].OptionsColumn.AllowEdit = false;
            dgvAsistencia.Columns["Departamento"].OptionsColumn.AllowEdit = false;
            dgvAsistencia.Columns["MinutosExtras"].OptionsColumn.AllowEdit = false;
            dgvAsistencia.Columns["TiempoAtraso"].OptionsColumn.AllowEdit = false;
            dgvAsistencia.Columns["UsuarioJefatura"].OptionsColumn.AllowEdit = false;

            dgvAsistencia.Columns["NombreCompleto"].Width = 200; // Nombre
            dgvAsistencia.Columns["CodigoDepartamento"].Width = 120; // Departamento
            dgvAsistencia.Columns["Permiso"].Width = 200; // Permiso
            dgvAsistencia.FixedLineWidth = 2;
            dgvAsistencia.Columns["NombreCompleto"].Fixed = FixedStyle.Left;
            dgvAsistencia.Columns["CodigoDepartamento"].Fixed = FixedStyle.Left;

            dgvAsistencia.Columns["HoraLlegada"].Width = 85; // HoraLlegada
            dgvAsistencia.Columns["HoraSalida"].Width = 85; // HoraSalida

            (dgvAsistencia.Columns["HoraLlegada"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            (dgvAsistencia.Columns["HoraLlegada"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.EditMask = "HH:mm:ss";
            (dgvAsistencia.Columns["HoraLlegada"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.UseMaskAsDisplayFormat = true;

            (dgvAsistencia.Columns["HoraSalida"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            (dgvAsistencia.Columns["HoraSalida"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.EditMask = "HH:mm:ss";
            (dgvAsistencia.Columns["HoraSalida"].RealColumnEdit as RepositoryItemTimeSpanEdit).Mask.UseMaskAsDisplayFormat = true;

            
            //dgvAsistencia.Columns["HoraLlegada"].Summary.Add(SummaryItemType.Sum, psNameColumn, "{ 0:0.000000}");

        }

        private void lBuscar()
        {

            if (lbCargado)
            {

                Cursor.Current = Cursors.WaitCursor;

                if (lbConfirmarCambios())
                {

                    ldtFecha = dtpFecha.Value;
                    var bsDatos = new BindingSource();
                    var polistaAsisBiom = loLogicaNegocio.goConsultarAsistenciaDetalleBiometrico(ldtFecha, ldtFecha, clsPrincipal.gsUsuario);
                    bsDatos.DataSource = polistaAsisBiom;
                    gcDatos.DataSource = bsDatos;

                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;

                    if (loLogicaNegocio.gbFechaAsistenciaCerrado(ldtFecha))
                    {
                        if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                        if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;

                    }

                    bool MensajeGuardado = false;
                    if (ldtFecha.Date > DateTime.Now.Date)
                    {
                        if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                        if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;

                    }
                    else
                    {
                        MensajeGuardado = true;

                    }

                    string tsFechaCreacion = "";
                    string tsFechaMod = "";
                    string tsUsuarioCreacion = "";
                    string tsUsuarioMod = "";
                    var poLista = loLogicaNegocio.goConsultarAsistenciaDetalle(ldtFecha, clsPrincipal.gsUsuario, out tsFechaCreacion, out tsUsuarioCreacion, out tsFechaMod, out tsUsuarioMod);

                    loDatos = new List<SpAsistenciaDetalle>();
                    foreach (var item in poLista)
                    {
                        var reg = new SpAsistenciaDetalle();
                        reg = item.Clone();
                        loDatos.Add(reg);
                    }

                    lblMensaje.Text = string.Empty;
                    lblMensajeGuardado.Text = string.Empty;
                    if (pbAplicaUsuarioDepatamentoAsignado)
                    {
                        var piListaIdPersona = loLogicaNegocio.giConsultarPersonasUsuarioDepAsig();
                        var piListaIdPersonaNoHE = poLista.Where(x => piListaIdPersona.Contains(x.IdPersona) && !x.AplicaHE).Select(x => x.IdPersona).ToList();
                        poLista = poLista.Where(x => !piListaIdPersonaNoHE.Contains(x.IdPersona)).ToList();

                        if (poLista.Count == 0)
                        {
                            XtraMessageBox.Show("No existen datos guardados por el área de talento humano.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        var pdtFechaMaxMod = loLogicaNegocio.gdtFechaMaximaModificacionJefatura(dtpFecha.Value.Date);
                        lblMensaje.Text = "Fecha máxima para modificación de datos: " + pdtFechaMaxMod.ToString("dd/MM/yyyy");
                        if (DateTime.Now.Date > pdtFechaMaxMod)
                        {
                            if (dtpFecha.Value.Date <= pdtFechaMaxMod)
                            {
                                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                            }
                            else
                            {
                                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                            }
                        }
                        else
                        {
                            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                        }
                    }
                    //poLista.ForEach(x => x.AplicaTiempoGraciaPostSalida = true);
                    bsDatosAsistencia.DataSource = poLista;
                    gcAsistencia.DataSource = bsDatosAsistencia;

                    if (MensajeGuardado)
                    {
                        if (string.IsNullOrEmpty(tsFechaCreacion))
                        {
                            lblMensajeGuardado.Text = "ASISTENCIA PRELIMINAR - SE DEBE GUARDAR";
                            lblMensajeGuardado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
                        }
                        else
                        {
                            lblMensajeGuardado.Text = "ASISTENCIA GUARDADA EN EL SISTEMA!";
                            lblMensajeGuardado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
                        }
                    }

                    lblFechaCreacion.Text = tsFechaCreacion;
                    lblUsuarioCreacion.Text = tsUsuarioCreacion;
                    lblFechaMod.Text = tsFechaMod;
                    lblUltimoUsuarioMod.Text = tsUsuarioMod;
                }
                Cursor.Current = Cursors.Default;
            }

        }

        private bool lbCambiosEnGrid(List<SpAsistenciaDetalle> toObjBase, List<SpAsistenciaDetalle> toObjGrid)
        {
            bool pbResult = false;

            if (toObjBase.Count() != toObjGrid.Count() || toObjBase.Where(x=>x.IdAsistenciaDetalle != 0).Count() != toObjGrid.Where(x => x.IdAsistenciaDetalle != 0).Count())
            {
                return true;
            }

            foreach (var item in toObjBase.Where(x=>x.IdAsistenciaDetalle != 0))
            {
                var poObj = toObjGrid.Where(x => x.IdAsistenciaDetalle == item.IdAsistenciaDetalle && x.IdAsistenciaDetalle != 0).FirstOrDefault();
                if (poObj != null)
                {
                    if (item.HoraLlegada != poObj.HoraLlegada || item.HoraSalida != poObj.HoraSalida || item.CodigoDepartamento != poObj.CodigoDepartamento ||
                        item.Asistencia != poObj.Asistencia || item.Observacion != poObj.Observacion || item.AplicaHE != poObj.AplicaHE ||
                        item.AplicaHEAntesEntrada != poObj.AplicaHEAntesEntrada || item.AplicaTiempoGraciaPostSalida != poObj.AplicaTiempoGraciaPostSalida || 
                        item.Permiso != poObj.Permiso)
                    {
                        return true;
                    }
                }
            }
            return pbResult;
        }

        private bool lbConfirmarCambios()
        {
            bool pbResult = true;

            dgvAsistencia.PostEditor();
            var poLista = (List<SpAsistenciaDetalle>)bsDatosAsistencia.DataSource;

            if (lbCambiosEnGrid(loDatos, poLista))
            {
                DialogResult dialogResult = XtraMessageBox.Show("Se ha modificado la asistencia, ¿Está de seguro de continuar sin Guardar?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (dialogResult == DialogResult.Yes)
                {
                    pbResult = true;
                }
                else
                {
                    pbResult = false;
                    lbCargado = false;
                    dtpFecha.Value = ldtFecha;
                    lbCargado = true;
                }
            }

            return pbResult;
        }

        #endregion

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
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
    }
}
