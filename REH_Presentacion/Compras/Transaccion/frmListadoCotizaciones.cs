using COM_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
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
    public partial class frmListadoCotizaciones : frmBaseTrxDev
    {
        BindingSource bsDatos = new BindingSource();
        clsNCotizacion loLogicaNegocio = new clsNCotizacion();
        RepositoryItemMemoEdit rpiMedDescripcion;
        RepositoryItemButtonEdit rpiBtnShow;
        public frmListadoCotizaciones()
        {
            rpiBtnShow = new RepositoryItemButtonEdit();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;

            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;
            InitializeComponent();
        }

        private void rpiBtnShow_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            lmostrarCotizacion();
            lListar();
        }

        private void frmListadoCotizaciones_Load(object sender, EventArgs e)
        {
            try
            {
                bsDatos.DataSource = new List<ListadoCotizacion>();
                gcBandejaCotizaciones.DataSource = bsDatos;
                lListar();
                lCargarEventosBotones();
                lColumnas();
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            //if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            //if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            bsDatos.DataSource = new List<ListadoCotizacion>();
            gcBandejaCotizaciones.DataSource = bsDatos;
            lListar();
        }

      

        private void lListar()
        {
            var menu = Tag.ToString().Split(',');
            var poObject = loLogicaNegocio.goListarBandejaUsuario(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
            if (poObject != null)
            {
                foreach (var x in poObject)
                {
                    x.Aprobaciones = loLogicaNegocio.goBuscarCantidadAprobacion(x.IdCotizacion);
                    x.UsuariosAprobacion = loLogicaNegocio.goBuscarUsuarioAprobacion(x.IdCotizacion);
                }
                bsDatos.DataSource = new List<ListadoCotizacion>();
                bsDatos.DataSource = poObject;
                gcBandejaCotizaciones.DataSource = bsDatos.DataSource;
                dgvBandejaCotizaciones.Columns[0].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            }
        }

        private void lColumnas()
        {
            for (int i = 0; i < dgvBandejaCotizaciones.Columns.Count; i++)
            {
                dgvBandejaCotizaciones.Columns[i].OptionsColumn.AllowEdit = false;
                dgvBandejaCotizaciones.Columns[i].OptionsColumn.FixedWidth = true;
            }
            dgvBandejaCotizaciones.Columns["Visualizar"].OptionsColumn.AllowEdit = true;

            dgvBandejaCotizaciones.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaCotizaciones.Columns["Usuario"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaCotizaciones.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaCotizaciones.Columns["UsuariosAprobacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaCotizaciones.Columns["IdCotizacion"].Width = 20;
            dgvBandejaCotizaciones.Columns["Visualizar"].Width = 30;
            dgvBandejaCotizaciones.Columns["UsuariosAprobacion"].Width = 60;
            dgvBandejaCotizaciones.Columns["Aprobaciones"].Width = 30;
            dgvBandejaCotizaciones.Columns["Usuario"].Width = 45;
            dgvBandejaCotizaciones.Columns["Fecha"].Width = 30;
            dgvBandejaCotizaciones.Columns["Estado"].Width = 40;

            lColocarbotonVisualizar(dgvBandejaCotizaciones.Columns["Visualizar"]);
            dgvBandejaCotizaciones.Columns["IdCotizacion"].Caption = "No";

          dgvBandejaCotizaciones.OptionsView.RowAutoHeight = true;
        }

        private void lmostrarCotizacion()
        {
            int piIndex = dgvBandejaCotizaciones.GetFocusedDataSourceRowIndex();
            var poLista = (List<ListadoCotizacion>)bsDatos.DataSource;
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
        private void lColocarbotonVisualizar(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Ver";
            colXmlDown.ColumnEdit = rpiBtnShow;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnShow.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnShow.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/show_16x16.png");
            rpiBtnShow.TextEditStyle = TextEditStyles.HideTextEditor;
            colXmlDown.Width = 20;
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
