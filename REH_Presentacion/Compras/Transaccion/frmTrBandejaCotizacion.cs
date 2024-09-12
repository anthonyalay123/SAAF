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
    public partial class frmTrBandejaCotizacion : frmBaseTrxDev
    {

        #region Variables
        BindingSource bsDatos = new BindingSource();
        clsNCotizacion loLogicaNegocio = new clsNCotizacion();
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnAprobar;
        RepositoryItemMemoEdit rpiMedDescripcion;
        #endregion

        public frmTrBandejaCotizacion()
        {
            rpiBtnShow = new RepositoryItemButtonEdit();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnAprobar = new RepositoryItemButtonEdit();
            rpiBtnAprobar.ButtonClick += rpiBtnAprobar_ButtonClick;
            rpiBtnShowComentarios.ButtonClick += rpiBtnShowComentarios_ButtonClick;
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;
            rpiMedDescripcion.MaxLength = 120;
            InitializeComponent();
        }

        private void frmTrBandejaCotizacion_Load(object sender, EventArgs e)
        {
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
                
                lmostrarCotizacion();
                lListar();
                
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
                int piIndex = dgvBandejaCotizacion.GetFocusedDataSourceRowIndex();
                var poLista = (List<BandejaCotizacion>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.CotizacionGanadora, poLista[piIndex].IdCotizacion);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Aprobadores con sus comentarios" };
                    pofrmBuscar.ShowDialog();

                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void rpiBtnAprobar_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                lmostrarAprobacion();
                lListar();
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
                //dgvBandejaCotizacion.Columns.Clear();
                gcBandejaCotizacion.DataSource = null;
                lListar();
                lColumnas();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void lmostrarCotizacion()
        {
            int piIndex = dgvBandejaCotizacion.GetFocusedDataSourceRowIndex();
            var poLista = (List<BandejaCotizacion>)bsDatos.DataSource;
            if (piIndex >= 0)
            {
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(Diccionario.Tablas.Menu.Cotizacion, clsPrincipal.gIdPerfil);

                if (poForm != null)
                {
                    frmTrCotizacion poFrmMostrarFormulario = new frmTrCotizacion();
                    poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    poFrmMostrarFormulario.Text = poForm.Nombre;
                    poFrmMostrarFormulario.ShowInTaskbar = true;
                    poFrmMostrarFormulario.lIdCotizacion = poLista[piIndex].IdCotizacion;
                    poFrmMostrarFormulario.MdiParent = this.ParentForm;
                   
                    poFrmMostrarFormulario.Show();
                    lListar();
                }
                else
                {
                    XtraMessageBox.Show("No se encontró formulario de Cotizacion", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void lListar()
        {
            var menu = Tag.ToString().Split(',');
           // var usuario = loLogicaNegocio.goBuscarAprobacionFinalCotizacion(clsPrincipal.gsUsuario);
            //Si es el ultimo usuario en aprobar
            //if (usuario)
            //{
                var poObject = loLogicaNegocio.goListarBandejaCotizacion(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
               
                if (poObject != null)
                {
                    foreach (var x in poObject)
                    {
                        x.Aprobaciones = loLogicaNegocio.goBuscarCantidadAprobacion(x.IdCotizacion);
                        x.UsuariosAprobacion = loLogicaNegocio.goBuscarUsuarioAprobacion(x.IdCotizacion);
                    }
                    bsDatos.DataSource = new List<BandejaCotizacion>();
                    bsDatos.DataSource = poObject;
                    gcBandejaCotizacion.DataSource = bsDatos.DataSource;
                dgvBandejaCotizacion.Columns[0].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            }
           





        }
        private void lColumnas()
        {
            //dgvBandejaCotizacion.Columns["UsuariosAprobacion"].Visible = false;

            dgvBandejaCotizacion.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaCotizacion.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaCotizacion.Columns["UsuariosAprobacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaCotizacion.Columns["Usuario"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaCotizacion.Columns["UsuariosAprobacion"].ColumnEdit = rpiMedDescripcion;
            for (int i = 0; i < dgvBandejaCotizacion.Columns.Count; i++)
            {
                dgvBandejaCotizacion.Columns[i].OptionsColumn.AllowEdit = false;
                dgvBandejaCotizacion.Columns[i].OptionsColumn.FixedWidth = true;
            }
            dgvBandejaCotizacion.Columns["Visualizar"].OptionsColumn.AllowEdit = true;
            dgvBandejaCotizacion.Columns["Aprobar"].OptionsColumn.AllowEdit = true;
            dgvBandejaCotizacion.Columns["VerComentarios"].OptionsColumn.AllowEdit = true;

            dgvBandejaCotizacion.Columns["IdCotizacion"].Caption = "No.";
            dgvBandejaCotizacion.Columns["UsuariosAprobacion"].Caption = "Aprobado por";
            dgvBandejaCotizacion.Columns["VerComentarios"].Caption = "Comentarios";

            //lColocarbotonVisualizar(dgvBandejaCotizacion.Columns["Visualizar"]);
            //lColocarbotonAprobar(dgvBandejaCotizacion.Columns["Aprobar"]);

            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvBandejaCotizacion.Columns["Visualizar"], "Cotización", Diccionario.ButtonGridImage.show_16x16, 60);
            clsComun.gDibujarBotonGrid(rpiBtnAprobar, dgvBandejaCotizacion.Columns["Aprobar"], "Aprobación", Diccionario.ButtonGridImage.show_16x16, 60);
            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvBandejaCotizacion.Columns["VerComentarios"], "Comentarios", Diccionario.ButtonGridImage.showhidecomment_16x16, 60);
            
            //dgvBandejaCotizacion.Columns["Descripcion"].Width = 220;
            dgvBandejaCotizacion.OptionsView.RowAutoHeight = true;
            dgvBandejaCotizacion.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            dgvBandejaCotizacion.BestFitColumns();
            dgvBandejaCotizacion.Columns["Fecha"].Width = 70;
            dgvBandejaCotizacion.Columns["Usuario"].Width = 80;
            dgvBandejaCotizacion.Columns["Estado"].Width = 90;
            dgvBandejaCotizacion.Columns["IdCotizacion"].Width = 35;
            dgvBandejaCotizacion.Columns["Descripcion"].Width = 200;
            dgvBandejaCotizacion.Columns["Observacion"].Width = 150;
            dgvBandejaCotizacion.Columns["UsuariosAprobacion"].Width = 150;

            //dgvBandejaCotizacion.OptionsView.ColumnAutoWidth = true;
        }
        private void lmostrarAprobacion()
        {
            int piIndex = dgvBandejaCotizacion.GetFocusedDataSourceRowIndex();
            var poLista = (List<BandejaCotizacion>)bsDatos.DataSource;
            if (piIndex >= 0)
            {
                string psForma = Diccionario.Tablas.Menu.AprobacionCotizacion;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);

                if (poForm != null)
                {
                    frmTrCotizacionAprobacion poFrmMostrarFormulario = new frmTrCotizacionAprobacion();

                    poFrmMostrarFormulario.lIdCotizacion = poLista[piIndex].IdCotizacion;
                    poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    poFrmMostrarFormulario.Text = poForm.Nombre;
                    poFrmMostrarFormulario.ShowInTaskbar = true;
                    poFrmMostrarFormulario.MdiParent = this.ParentForm;
                    
                    poFrmMostrarFormulario.Show();

                }
                else
                {
                    XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void lColocarbotonVisualizar(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Cotización";
            colXmlDown.ColumnEdit = rpiBtnShow;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnShow.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnShow.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/show_16x16.png");
            rpiBtnShow.TextEditStyle = TextEditStyles.HideTextEditor;
            colXmlDown.Width = 48;
        }
        private void lColocarbotonAprobar(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Aprobación";
            colXmlDown.ColumnEdit = rpiBtnAprobar;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnAprobar.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnAprobar.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/show_16x16.png");
            rpiBtnAprobar.TextEditStyle = TextEditStyles.HideTextEditor;
            colXmlDown.Width = 48;
        }
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
        }
        private void lListarCotizacionGanadora()
        {
            var menu = Tag.ToString().Split(',');
            var poObject = loLogicaNegocio.goListarBandeja(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
            if (poObject != null)
            {
                bsDatos.DataSource = new List<BandejaCotizacion>();
                bsDatos.DataSource = poObject;
                gcBandejaCotizacion.DataSource = bsDatos.DataSource;
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
