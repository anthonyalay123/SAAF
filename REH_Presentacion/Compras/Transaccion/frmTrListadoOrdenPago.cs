using COM_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Compras.Transaccion
{
    public partial class frmTrListadoOrdenPago : frmBaseTrxDev
    {

        clsNOrdenPago loLogicaNegocio = new clsNOrdenPago();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnReport;
        RepositoryItemMemoEdit rpiMedDescripcion;
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();

        public frmTrListadoOrdenPago()
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

        private void frmTrListadoOrdenPago_Load(object sender, EventArgs e)
        {
            dtpFechaInicial.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaFinal.DateTime = DateTime.Now;
            bsDatos.DataSource = new List<BandejaOrdenPago>();
            gcBandejaOrdenPago.DataSource = bsDatos;
            lListar();
            lColumnas();
            lCargarEventosBotones();
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
                lMostrarOrdenPago();
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
                int piIndex = dgvBandejaOrdenPago.GetFocusedDataSourceRowIndex();
                var poLista = (List<BandejaOrdenPago>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.OrdenPago, poLista[piIndex].Id);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Aprobadores con sus comentarios" };
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
                var poLista = (List<BandejaOrdenPago>)bsDatos.DataSource;
                var piIndex = dgvBandejaOrdenPago.GetFocusedDataSourceRowIndex();
                clsComun.gImprimirOrdenPago(poLista[piIndex].Id);
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int piIndex = dgvBandejaOrdenPago.GetFocusedDataSourceRowIndex();
                var poLista = (List<BandejaOrdenPago>)bsDatos.DataSource;
                int pId = poLista[piIndex].Id;

                if (pId != 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loLogicaNegocio.gEliminarMaestro(pId, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lListar();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lListar()
        {
            Cursor.Current = Cursors.WaitCursor;

            var menu = Tag.ToString().Split(',');
            var poObject = loLogicaNegocio.goListarListado(clsPrincipal.gsUsuario, dtpFechaInicial.DateTime, dtpFechaFinal.DateTime);
            if (poObject != null)
            {
                bsDatos.DataSource = poObject;
                gcBandejaOrdenPago.DataSource = bsDatos.DataSource;
                dgvBandejaOrdenPago.Columns[0].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;

                //foreach (var x in poObject)
                //{
                //    x.Aprobaciones = loLogicaNegocio.goBuscarCantidadAprobacion(x.Id);
                //    x.UsuariosAprobacion = loLogicaNegocio.goBuscarUsuarioAprobacion(x.Id);
                //    x.Factura = loLogicaNegocio.gsFacturasRelacionadasOrdenPago(x.Id);  
                //}
                //bsDatos.DataSource = new List<BandejaSolicitudCompra>();
                //bsDatos.DataSource = poObject;
                //gcBandejaOrdenPago.DataSource = bsDatos.DataSource;
                //dgvBandejaOrdenPago.Columns[0].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            }

            Cursor.Current = Cursors.Default;
        }

        private void lColumnas()
        {
            dgvBandejaOrdenPago.OptionsView.RowAutoHeight = true;
            dgvBandejaOrdenPago.OptionsView.ShowFooter = true;

            for (int i = 0; i < dgvBandejaOrdenPago.Columns.Count; i++)
            {
                dgvBandejaOrdenPago.Columns[i].OptionsColumn.AllowEdit = false;
                // dgvBandejaSolicitud.Columns[i].OptionsColumn.FixedWidth = true;
            }
            dgvBandejaOrdenPago.Columns["Ver"].OptionsColumn.AllowEdit = true;
            dgvBandejaOrdenPago.Columns["Imprimir"].OptionsColumn.AllowEdit = true;
            dgvBandejaOrdenPago.Columns["Sel"].OptionsColumn.AllowEdit = true;
            dgvBandejaOrdenPago.Columns["VerComentarios"].OptionsColumn.AllowEdit = true;
            dgvBandejaOrdenPago.Columns["Id"].Caption = "No.";
            dgvBandejaOrdenPago.Columns["Valor"].Caption = "Total Orden de Pago";
            lColocarbotonVisualizar(dgvBandejaOrdenPago.Columns["Ver"]);
            lColocarbotonReporte(dgvBandejaOrdenPago.Columns["Imprimir"]);
            dgvBandejaOrdenPago.Columns["Usuario"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenPago.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenPago.Columns["Proveedor"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenPago.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenPago.Columns["UsuariosAprobacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenPago.Columns["Factura"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenPago.Columns["Sel"].Visible = false;
            dgvBandejaOrdenPago.Columns["Diferencia"].Visible = false;
            //dgvBandejaOrdenPago.Columns["TotalOrdenCompra"].ColumnEdit = rpiMedDescripcion;
            //dgvBandejaOrdenPago.Columns["Valor"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenPago.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;

            dgvBandejaOrdenPago.Columns["ReferenciasIDOrdenCompraProveedor"].Visible = false;
            dgvBandejaOrdenPago.Columns["UsuariosAprobacion"].Visible = false;
            dgvBandejaOrdenPago.Columns["Aprobaciones"].Visible = false;
            dgvBandejaOrdenPago.Columns["Estado"].Width = 80;
            dgvBandejaOrdenPago.Columns["Id"].Width = 40;
            dgvBandejaOrdenPago.Columns["Ver"].Width = 40;
            dgvBandejaOrdenPago.Columns["VerComentarios"].Width = 40;
            dgvBandejaOrdenPago.Columns["Imprimir"].Width = 40;
            dgvBandejaOrdenPago.Columns["Valor"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvBandejaOrdenPago.Columns["Valor"].DisplayFormat.FormatString = "c2";
            dgvBandejaOrdenPago.Columns["TotalOrdenCompra"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvBandejaOrdenPago.Columns["TotalOrdenCompra"].DisplayFormat.FormatString = "c2";
            dgvBandejaOrdenPago.Columns["Diferencia"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvBandejaOrdenPago.Columns["Diferencia"].DisplayFormat.FormatString = "c2";

            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvBandejaOrdenPago.Columns["VerComentarios"], "Ver Comentarios", Diccionario.ButtonGridImage.showhidecomment_16x16);

            clsComun.gFormatearColumnasGrid(dgvBandejaOrdenPago);
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Click += btnCorregir_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            
            
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

        private void lColocarbotonVisualizar(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Ver";
            colXmlDown.ColumnEdit = rpiBtnShow;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnShow.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnShow.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/show_16x16.png");
            rpiBtnShow.TextEditStyle = TextEditStyles.HideTextEditor;
            colXmlDown.Width = 35;
        }

        /// <summary>
        /// Agregar boton delete a una Columna de un grid
        /// </summary>
        /// <param name="colXmlDown"></param>
        private void lColocarbotonReporte(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Imprimir";
            colXmlDown.ColumnEdit = rpiBtnReport;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;


            rpiBtnReport.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnReport.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/print/printer_16x16.png");
            rpiBtnReport.TextEditStyle = TextEditStyles.HideTextEditor;
            colXmlDown.Width = 35;
        }
        public void lMostrarOrdenPago()
        {
            int piIndex = dgvBandejaOrdenPago.GetFocusedDataSourceRowIndex();
            var poLista = (List<BandejaOrdenPago>)bsDatos.DataSource;
            if (piIndex >= 0)
            {

                string psForma = Diccionario.Tablas.Menu.OrdenPago;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                if (poForm != null)
                {
                    frmTrOrdenPago poFrmMostrarFormulario = new frmTrOrdenPago();
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnCorregir_Click(object sender, EventArgs e)
        {
            try
            {

                //dgvBandejaOrdenPago.PostEditor();
                var poLista = (List<BandejaOrdenPago>)bsDatos.DataSource;
                var piIndex = dgvBandejaOrdenPago.GetFocusedDataSourceRowIndex();

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

                        loLogicaNegocio.gActualizarEstadoOrdenPago(poLista[piIndex].Id, Diccionario.Corregir, result, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lListar();
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


        GridView DGVCopiarPortapapeles;
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

        private void frmTrListadoOrdenPago_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }
    }
}
