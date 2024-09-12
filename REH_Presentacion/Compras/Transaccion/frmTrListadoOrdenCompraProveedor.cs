using COM_Negocio;
using DevExpress.Data;
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
    public partial class frmTrListadoOrdenCompraProveedor : frmBaseTrxDev
    {
        RepositoryItemMemoEdit rpiMedDescripcion;
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnReport;
        clsNOrdenCompra loLogicaNegocio = new clsNOrdenCompra();

        BindingSource bsDatos = new BindingSource();
        public frmTrListadoOrdenCompraProveedor()
        {
            InitializeComponent();
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.MaxLength = 200;
            rpiMedDescripcion.WordWrap = true;
            rpiBtnShow = new RepositoryItemButtonEdit();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnReport = new RepositoryItemButtonEdit();
            rpiBtnReport.ButtonClick += rpiBtnReport_ButtonClick;

        }
        private void rpiBtnReport_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                var poLista = (List<ListadoProveedor>)bsDatos.DataSource;
                var piIndex = dgvBandejaOrdenCompraProveedor.GetFocusedDataSourceRowIndex();
                clsComun.gImprimirOrdenCompra(poLista[piIndex].idOrdenCompra,poLista[piIndex].IdentificacionProveedor);
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
        private void rpiBtnShow_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            lmostrarOrdenCompra();
            lListar();
        }

        private void frmTrListadoOrdenCompraProveedor_Load(object sender, EventArgs e)
        {
            bsDatos.DataSource = new List<ListadoProveedor>();
            gcBandejaOrdenCompraProveedor.DataSource = bsDatos;
            lListar();
            lCargarEventosBotones();
            lColumnas();
        }

        private void lListar()
        {
            var menu = Tag.ToString().Split(',');
            var poObject = loLogicaNegocio.goBuscarListarProveedores(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
            if (poObject.Count>0)
            {
                bsDatos.DataSource = new List<ListadoProveedor>();
                bsDatos.DataSource = poObject;
                gcBandejaOrdenCompraProveedor.DataSource = bsDatos.DataSource;
            }
          
        }
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            lListar();
        }
        private void lColumnas()
        {
            dgvBandejaOrdenCompraProveedor.OptionsView.RowAutoHeight = true;
            for (int i = 0; i < dgvBandejaOrdenCompraProveedor.Columns.Count; i++)
            {
                dgvBandejaOrdenCompraProveedor.Columns[i].OptionsColumn.AllowEdit = false;
                dgvBandejaOrdenCompraProveedor.Columns[i].OptionsColumn.FixedWidth = true;
            }
            dgvBandejaOrdenCompraProveedor.Columns["Ver"].OptionsColumn.AllowEdit = true;
            dgvBandejaOrdenCompraProveedor.Columns["Imprimir"].OptionsColumn.AllowEdit = true;

            dgvBandejaOrdenCompraProveedor.Columns["IdProveedor"].Caption = "No";
            dgvBandejaOrdenCompraProveedor.Columns["IdentificacionProveedor"].Visible = false;
            dgvBandejaOrdenCompraProveedor.Columns["idOrdenCompra"].Visible = false;

            dgvBandejaOrdenCompraProveedor.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenCompraProveedor.Columns["Nombre"].ColumnEdit = rpiMedDescripcion;

            lColocarTotal("Total", dgvBandejaOrdenCompraProveedor);
            lColocarTotal("ValorAnticipo", dgvBandejaOrdenCompraProveedor);
            lColocarTotal("Saldo", dgvBandejaOrdenCompraProveedor);
            lColocarbotonVisualizar(dgvBandejaOrdenCompraProveedor.Columns["Ver"]);
            lColocarbotonReporte(dgvBandejaOrdenCompraProveedor.Columns["Imprimir"]);

            dgvBandejaOrdenCompraProveedor.Columns["Nombre"].Width = 100;
            dgvBandejaOrdenCompraProveedor.Columns["IdProveedor"].Width = 35;
            dgvBandejaOrdenCompraProveedor.Columns["Estado"].Width = 45;
            dgvBandejaOrdenCompraProveedor.Columns["Observacion"].Width = 150;
        }


        public void lColocarTotal(string psNameColumn, DevExpress.XtraGrid.Views.Grid.GridView dgv)
        {
            //  var psNameColumn = "Total";
            //dgvCotizacionDetalle.Columns[psNameColumn].UnboundType = UnboundColumnType.Decimal;
            dgv.Columns[psNameColumn].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgv.Columns[psNameColumn].DisplayFormat.FormatString = "c2";
            dgv.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");


            //GridGroupSummaryItem item1 = new GridGroupSummaryItem();
            //item1.FieldName = psNameColumn;
            //item1.SummaryType = SummaryItemType.Sum;
            //item1.DisplayFormat = "{0:c2}";
            //item1.ShowInGroupColumnFooter = dgv.Columns[psNameColumn];
            //dgv.GroupSummary.Add(item1);

        }

        private void lColocarbotonVisualizar(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Visualizar";
            colXmlDown.ColumnEdit = rpiBtnShow;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnShow.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnShow.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/show_16x16.png");
            rpiBtnShow.TextEditStyle = TextEditStyles.HideTextEditor;
            colXmlDown.Width = 45;
        }

        private void lmostrarOrdenCompra()
        {
            int piIndex = dgvBandejaOrdenCompraProveedor.GetFocusedDataSourceRowIndex();
            var poLista = (List<ListadoProveedor>)bsDatos.DataSource;
            if (piIndex >= 0)
            {
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(Diccionario.Tablas.Menu.OrdenCompra, clsPrincipal.gIdPerfil);

                if (poForm != null)
                {
                    frmTrOrdenCompra poFrmMostrarFormulario = new frmTrOrdenCompra();
                    poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    poFrmMostrarFormulario.Text = poForm.Nombre;
                    poFrmMostrarFormulario.ShowInTaskbar = true;
                    poFrmMostrarFormulario.lIdordenCompra = poLista[piIndex].idOrdenCompra;
                    poFrmMostrarFormulario.MdiParent = this.ParentForm;
                    poFrmMostrarFormulario.Show();
                    lListar();
                }
                else
                {
                    XtraMessageBox.Show("No se encontró formulario de Orden de compra", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

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
