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
    public partial class frmListadoOrdenCompra : frmBaseTrxDev
    {
        BindingSource bsDatos = new BindingSource();
        clsNOrdenCompra loLogicaNegocio = new clsNOrdenCompra();
        RepositoryItemMemoEdit rpiMedDescripcion;
        RepositoryItemButtonEdit rpiBtnShow;

        public frmListadoOrdenCompra()
        {
            InitializeComponent();
        }

        private void frmListadoOrdeCompra_Load(object sender, EventArgs e)
        {
            rpiBtnShow = new RepositoryItemButtonEdit();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            bsDatos.DataSource = new List<BandejaOrdenCompra>();
            gcBandejaOrdenCompra.DataSource = bsDatos;
            Listar();
            lCargarEventosBotones();
            lColumnas();
        }

        private void rpiBtnShow_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            lmostrarOrdenCompra();
            Listar();
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            //if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            // if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            bsDatos.DataSource = new List<BandejaOrdenCompra>();
            gcBandejaOrdenCompra.DataSource = bsDatos;
            Listar();
        }

        private void Listar()
        {
            var menu = Tag.ToString().Split(',');
            var poObject = loLogicaNegocio.goListarBandejaUsuario(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
            if (poObject != null)
            {
                bsDatos.DataSource = new List<BandejaOrdenCompra>();
                bsDatos.DataSource = poObject;
                gcBandejaOrdenCompra.DataSource = bsDatos.DataSource;
            }
        }
        private void lColumnas()
        {
            for (int i = 0; i < dgvBandejaOrdenCompra.Columns.Count; i++)
            {
                dgvBandejaOrdenCompra.Columns[i].OptionsColumn.AllowEdit = false;
                dgvBandejaOrdenCompra.Columns[i].OptionsColumn.FixedWidth = true;
            }
            dgvBandejaOrdenCompra.Columns["Visualizar"].OptionsColumn.AllowEdit = true;
            dgvBandejaOrdenCompra.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenCompra.Columns["Usuario"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenCompra.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenCompra.Columns["IdOrdenCompra"].Width = 20;
            dgvBandejaOrdenCompra.Columns["Visualizar"].Width = 30;
            dgvBandejaOrdenCompra.Columns["Usuario"].Width = 45;
            dgvBandejaOrdenCompra.Columns["Fecha"].Width = 30;
            dgvBandejaOrdenCompra.Columns["Estado"].Width = 30;
            lColocarbotonVisualizar(dgvBandejaOrdenCompra.Columns["Visualizar"]);
            dgvBandejaOrdenCompra.Columns["IdOrdenCompra"].Caption = "No";
            dgvBandejaOrdenCompra.OptionsView.RowAutoHeight = true;
        }

        private void lmostrarOrdenCompra()
        {
            int piIndex = dgvBandejaOrdenCompra.GetFocusedDataSourceRowIndex();
            var poLista = (List<BandejaOrdenCompra>)bsDatos.DataSource;
            if (piIndex >= 0)
            {
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(Diccionario.Tablas.Menu.OrdenCompra, clsPrincipal.gIdPerfil);

                if (poForm != null)
                {
                    frmTrOrdenCompra poFrmMostrarFormulario = new frmTrOrdenCompra();
                    poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    poFrmMostrarFormulario.Text = poForm.Nombre;
                    poFrmMostrarFormulario.ShowInTaskbar = true;
                    poFrmMostrarFormulario.lIdordenCompra = poLista[piIndex].IdOrdenCompra;
                    poFrmMostrarFormulario.MdiParent = this.ParentForm;

                    poFrmMostrarFormulario.Show();
                    Listar();
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
