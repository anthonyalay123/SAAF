using COM_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
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
    public partial class frmTrBandejaSolicitudCompraUsuario : frmBaseTrxDev
    {
        #region Variables
        BindingSource bsDatos = new BindingSource();
        clsNSolicitudCompra loLogicaNegocio = new clsNSolicitudCompra();
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnReport;
        RepositoryItemMemoEdit rpiMedDescripcion;
        #endregion


        #region Eventos
        public frmTrBandejaSolicitudCompraUsuario()
        {
            BindingSource bsDatos = new BindingSource();
            InitializeComponent();

            rpiBtnShow = new RepositoryItemButtonEdit();
            rpiBtnReport = new RepositoryItemButtonEdit();
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;

            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnReport.ButtonClick += rpiBtnReport_ButtonClick;

        }

        private void frmTrBandejaSolicitudCompraUsuario_Load(object sender, EventArgs e)
        {
            try
            {
                bsDatos.DataSource = new List<BandejaSolicitudCompraUsuario>();
                gcBandejaSolicitud.DataSource = bsDatos;

                lListar();
                lColumnas();
                lCargarEventosBotones();

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
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                lmostrarSolicitudCompra();
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
                lImprimir();
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                lRecargar();
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
                lRecargar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                GridControl gc = new GridControl();
                BindingSource bs = new BindingSource();
                GridView dgv = new GridView();
                List<BandejaSolicitudCompraUsuarioExcel> listbd = new List<BandejaSolicitudCompraUsuarioExcel>();

                var poLista = (List<BandejaSolicitudCompraUsuario>)bsDatos.DataSource;

                if (poLista.Count>0)
                {
                    foreach (var po in poLista)
                    {
                        BandejaSolicitudCompraUsuarioExcel be = new BandejaSolicitudCompraUsuarioExcel();
                        be.Id = po.Id;
                        be.Estado = po.Estado;
                        be.Fecha = po.FechaCreacion;
                        be.FechaEntrega = po.FechaEntrega;
                        be.Observacion = po.Observacion;
                        listbd.Add(be);
                    }
                    bs.DataSource = listbd;
                    gc.DataSource = bs;
                    gc.MainView = dgv;
                    gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                    dgv.GridControl = gc;
                    this.Controls.Add(gc);

                    // Exportar Datos

                    try
                    {
                        clsComun.gSaveFile(gc, "" + Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");
                        gc.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gc.Visible = false;
                    }
                    gc.Visible = false;
                }
               
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        #endregion



        private void lColocarbotonVisualizar(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Ver";
            colXmlDown.ColumnEdit = rpiBtnShow;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnShow.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnShow.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/show_16x16.png");
            rpiBtnShow.TextEditStyle = TextEditStyles.HideTextEditor;
            colXmlDown.Width = 30;
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
            colXmlDown.Width = 50;


        }

        private void lListar()
        {
            var menu = Tag.ToString().Split(',');
            var poObject = loLogicaNegocio.goListarBandejaUsuario(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
            if (poObject != null)
            {
                bsDatos.DataSource = new List<BandejaSolicitudCompraUsuario>();
                bsDatos.DataSource = poObject;
                gcBandejaSolicitud.DataSource = bsDatos.DataSource;
                dgvBandejaSolicitud.Columns[0].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            }
        }

        private void lColumnas()
        {
            dgvBandejaSolicitud.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            //dgvBandejaSolicitud.OptionsView.ColumnAutoWidth = true;
            dgvBandejaSolicitud.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaSolicitud.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaSolicitud.Columns["Usuario"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaSolicitud.Columns["Aprueba"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaSolicitud.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaSolicitud.Columns["Usuario"].Visible = true;
            dgvBandejaSolicitud.Columns["Terminal"].Visible = false;
           
            dgvBandejaSolicitud.Columns["Id"].Caption = "No";

            for (int i = 0; i < dgvBandejaSolicitud.Columns.Count; i++)
            {
                dgvBandejaSolicitud.Columns[i].OptionsColumn.AllowEdit = false;
                dgvBandejaSolicitud.Columns[i].OptionsColumn.FixedWidth = true;
            }
            dgvBandejaSolicitud.Columns["Id"].Width = 28;
           
            dgvBandejaSolicitud.Columns["Descripcion"].Width = 250;
            dgvBandejaSolicitud.Columns["Usuario"].Width = 120;
            dgvBandejaSolicitud.Columns["Usuario"].Caption= "Solicita";
            lColocarbotonReporte(dgvBandejaSolicitud.Columns["Reporte"]);
            lColocarbotonVisualizar(dgvBandejaSolicitud.Columns["Visualizar"]);
            dgvBandejaSolicitud.Columns["Reporte"].OptionsColumn.AllowEdit = true;
            dgvBandejaSolicitud.Columns["Visualizar"].OptionsColumn.AllowEdit = true;
            dgvBandejaSolicitud.Columns["Descripcion"].Width = 200;

            dgvBandejaSolicitud.OptionsView.RowAutoHeight = true;
            //dgvBandejaSolicitud.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            //dgvBandejaSolicitud.BestFitColumns();
            


        }

        private void lImprimir()
        {
            int piIndex = dgvBandejaSolicitud.GetFocusedDataSourceRowIndex();
            var poLista = (List<BandejaSolicitudCompraUsuario>)bsDatos.DataSource;
            if (poLista[piIndex].Id.ToString() != "")
            {
                DataSet ds = new DataSet();
                int tIdSolicitud = poLista[piIndex].Id;
                var dt = loLogicaNegocio.gConsultarCabecera(tIdSolicitud);
                var dtDetalle = loLogicaNegocio.gConsultarDetalle(tIdSolicitud);
                dt.TableName = "SolicitudCompra";
                dtDetalle.TableName = "SolicitudCompraDetalle";
                ds.Merge(dt);
                ds.Merge(dtDetalle);
                if (dt.Rows.Count > 0)
                {
                    Reportes.xrptSolicitudCompra xrpt = new Reportes.xrptSolicitudCompra();
                    xrpt.DataSource = ds;
                    xrpt.RequestParameters = false;

                    using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                    {
                        printTool.ShowRibbonPreviewDialog();
                    }
                }
            }
            else
            {
                XtraMessageBox.Show("No existe detalles guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Click += btnCorregir_Click;
        }

        private void btnCorregir_Click(object sender, EventArgs e)
        {
            try
            {
                
                var poLista = (List<BandejaSolicitudCompraUsuario>)bsDatos.DataSource;
                var piIndex = dgvBandejaSolicitud.GetFocusedDataSourceRowIndex();

                if (poLista[piIndex].Estado == Diccionario.DesAprobado)
                {
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

                            loLogicaNegocio.gActualizarEstadoSolicitud(poLista[piIndex].Id, Diccionario.Corregir, result, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }

                    }
                    else
                    {
                        XtraMessageBox.Show("No hay registros seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("Solo se puede enviar a corregir solicitudes aprobadas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

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
                int piIndex = dgvBandejaSolicitud.GetFocusedDataSourceRowIndex();
                var poLista = (List<BandejaSolicitudCompraUsuario>)bsDatos.DataSource;
                int pId = poLista[piIndex].Id;

                if (poLista[piIndex].Estado == Diccionario.DesAprobado)
                {
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
                else
                {
                    XtraMessageBox.Show("Solo se puede eliminar solicitudes aprobadas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void lRecargar()
        {
           // dgvBandejaSolicitud.Columns.Clear();
            gcBandejaSolicitud.DataSource = null;
            lListar();
            lColumnas();
           
        }

        private void lmostrarSolicitudCompra()
        {
            int piIndex = dgvBandejaSolicitud.GetFocusedDataSourceRowIndex();
            var poLista = (List<BandejaSolicitudCompraUsuario>)bsDatos.DataSource;
            if (piIndex >= 0)
            {
                string psForma = Diccionario.Tablas.Menu.SolicitudCompra;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);

                if (poForm != null)
                {
                    frmTrSolicitudCompras poFrmMostrarFormulario = new frmTrSolicitudCompras();
                    poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    
                    poFrmMostrarFormulario.Text = poForm.Nombre;
                    poFrmMostrarFormulario.ShowInTaskbar = true;
                    poFrmMostrarFormulario.lId = poLista[piIndex].Id;
                    poFrmMostrarFormulario.MdiParent = this.ParentForm;
                    poFrmMostrarFormulario.Show();


                }
                else
                {
                    XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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


    }
}
