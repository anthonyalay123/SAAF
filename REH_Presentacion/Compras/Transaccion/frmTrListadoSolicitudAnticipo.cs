using COM_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static DevExpress.XtraBars.Docking2010.Views.BaseRegistrator;

namespace REH_Presentacion.Compras.Transaccion
{
    public partial class frmTrListadoSolicitudAnticipo : frmBaseTrxDev
    {

        #region Variables
        clsNAnticipo loLogicaNegocio = new clsNAnticipo();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnReport;
        RepositoryItemMemoEdit rpiMedDescripcion;
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();
        GridView DGVCopiarPortapapeles;
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmTrListadoSolicitudAnticipo()
        {
            InitializeComponent();
            rpiBtnShow = new RepositoryItemButtonEdit();
            rpiBtnReport = new RepositoryItemButtonEdit();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnReport.ButtonClick += rpiBtnReport_ButtonClick;
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;
            rpiBtnShowComentarios.ButtonClick += rpiBtnShowComentarios_ButtonClick;
        }
        /// <summary>
        /// Evento cuando carga el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTrListadoOrdenPago_Load(object sender, EventArgs e)
        {
            dtpFechaInicial.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaFinal.DateTime = DateTime.Now;
            bsDatos.DataSource = new List<SpListadoSolicitudAnticipo>();
            gcDatos.DataSource = bsDatos;
            lListar();
            lCargarEventosBotones();
        }
        /// <summary>
        /// Evento cuando una tacla es soltada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTrListadoOrdenPago_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }
        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<SpListadoSolicitudAnticipo>)bsDatos.DataSource;
                if (piIndex >= 0)
                {

                    string psForma = Diccionario.Tablas.Menu.SolicitudAnticipo;
                    var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                    if (poForm != null)
                    {
                        frmTrSolicitudAnticipo poFrmMostrarFormulario = new frmTrSolicitudAnticipo();
                        poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                        poFrmMostrarFormulario.Text = poForm.Nombre;
                        poFrmMostrarFormulario.ShowInTaskbar = true;
                        poFrmMostrarFormulario.MdiParent = this.ParentForm;
                        poFrmMostrarFormulario.lid = poLista[piIndex].Id;
                        poFrmMostrarFormulario.Show();
                    }
                    else
                    {
                        XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Evento del boton de Visulizar en el Grid, comentarios de las aprobaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShowComentarios_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<SpListadoSolicitudAnticipo>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.SolicitudAnticipo, poLista[piIndex].Id);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Trazabilidad" };
                    pofrmBuscar.ShowDialog();

                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnReport_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                var poLista = (List<SpListadoSolicitudAnticipo>)bsDatos.DataSource;
                var piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                //clsComun.gImprimirOrdenPago(poLista[piIndex].Id);
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Busca información de la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                lListar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            try
            {
                lListar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Eliminar registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<SpListadoSolicitudAnticipo>)bsDatos.DataSource;
                int pId = poLista[piIndex].Id;

                if (pId != 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var msg = loLogicaNegocio.gsEliminarSolicitudAnticipo(pId, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(msg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }
                        else
                        {
                            XtraMessageBox.Show(msg, "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Evento del botón Exportar, Exporta a Excel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                List<SpListadoSolicitudAnticipo> poLista = (List<SpListadoSolicitudAnticipo>)bsDatos.DataSource;
                if (poLista != null && poLista.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcDatos, Text + ".xlsx", psFilter);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Envía a corregir el registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCorregir_Click(object sender, EventArgs e)
        {
            try
            {

                var poLista = (List<SpListadoSolicitudAnticipo>)bsDatos.DataSource;
                var piIndex = dgvDatos.GetFocusedDataSourceRowIndex();

                var result = XtraInputBox.Show("Ingrese Observación", "Corregir", "");
                if (string.IsNullOrEmpty(result))
                {
                    XtraMessageBox.Show("Debe agregar Obsevación para poder Corregir", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!string.IsNullOrEmpty(result))
                {

                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {

                        string msg = loLogicaNegocio.gActualizarEstadoSolicitudAnticipo(poLista[piIndex].Id, Diccionario.Corregir, result, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                        if (string.IsNullOrEmpty(msg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }
                        else
                        {
                            XtraMessageBox.Show(msg, "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay registros seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            if (e.MenuType != DevExpress.XtraGrid.Views.Grid.GridMenuType.Row)
                return;
            var menuItemCopyCellValue = new DevExpress.Utils.Menu.DXMenuItem("Copiar", new EventHandler(OnCopyItemClick) /*, assign an icon, if necessary */);
            DGVCopiarPortapapeles = sender as GridView;
            e.Menu.Items.Add(menuItemCopyCellValue);
        }
        void OnCopyItemClick(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(DGVCopiarPortapapeles.GetFocusedValue()?.ToString());
            }
            catch (Exception)
            {

                Clipboard.SetText(" ");
            }

        }
        #endregion

        #region Métodos
        private void lListar()
        {
            Cursor.Current = Cursors.WaitCursor;

            var poObject = loLogicaNegocio.goListadoSolicitudAnticipo(int.Parse(Tag.ToString().Split(',')[0]), clsPrincipal.gsUsuario, dtpFechaInicial.DateTime, dtpFechaFinal.DateTime);
            if (poObject != null)
            {
                bsDatos.DataSource = poObject;
                gcDatos.DataSource = bsDatos.DataSource;
                dgvDatos.Columns[0].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            }

            Cursor.Current = Cursors.Default;
        }
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Click += btnCorregir_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;

            dgvDatos.OptionsView.RowAutoHeight = true;
            dgvDatos.OptionsView.ShowFooter = true;
            dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;

            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                dgvDatos.Columns[i].OptionsColumn.AllowEdit = false;
            }

            dgvDatos.Columns["Imprimir"].Visible = false;

            dgvDatos.Columns["Ver"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["Imprimir"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["VerComentarios"].OptionsColumn.AllowEdit = true;

            dgvDatos.Columns["Id"].Caption = "No.";
            dgvDatos.Columns["Valor"].Caption = "Valor Anticipo";

            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDatos.Columns["Ver"], "Ver", Diccionario.ButtonGridImage.show_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnReport, dgvDatos.Columns["Imprimir"], "Imprimir", Diccionario.ButtonGridImage.printer_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvDatos.Columns["VerComentarios"], "Trazabilidad", Diccionario.ButtonGridImage.showhidecomment_16x16);

            dgvDatos.Columns["Usuario"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Proveedor"].ColumnEdit = rpiMedDescripcion;

            dgvDatos.Columns["Estado"].Width = 80;
            dgvDatos.Columns["Id"].Width = 40;
            dgvDatos.Columns["Ver"].Width = 40;
            dgvDatos.Columns["VerComentarios"].Width = 40;
            dgvDatos.Columns["Imprimir"].Width = 40;
            dgvDatos.Columns["Valor"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Valor"].DisplayFormat.FormatString = "c2";

            clsComun.gFormatearColumnasGrid(dgvDatos);
        }        
        #endregion

    }
}
