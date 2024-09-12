using COM_Negocio;
using DevExpress.XtraEditors;
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
    public partial class frmTrBandejaOrdenCompraAnticipo : frmBaseTrxDev
    {
        BindingSource bsProveedor = new BindingSource();
        clsNOrdenCompra loLogicaNegocio = new clsNOrdenCompra();
        RepositoryItemMemoEdit rpiMedDescripcion;

        public frmTrBandejaOrdenCompraAnticipo()
        {
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;
          
            InitializeComponent();
        }

        private void frmBandejaOrdenCompraAnticipo_Load(object sender, EventArgs e)
        {
            bsProveedor.DataSource = new List<BandejaProveedoresCompraAnticipo>();
            gcProveedor.DataSource = bsProveedor;
            lListar();
            lColumnas();
            lCargarEventosBotones();
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            //if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Click += btnGenerar_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
            if (dialogResult == DialogResult.Yes)
            {
                dgvProveedor.PostEditor();
                var plProveedor = (List<BandejaProveedoresCompraAnticipo>)bsProveedor.DataSource;
                bool verficar = false;
                foreach (var proveedor in plProveedor)
                {
                    if (proveedor.Sel == true)
                    {
                        loLogicaNegocio.goBuscarProveedorOrdenCompra(proveedor.IdOrdenCompraProveedor, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        verficar = true;
                    }

                }
                if (!verficar)
                {

                    XtraMessageBox.Show("Debe seleecionar un proveedor.", Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("Guardado Exitosamente.", Diccionario.MsgRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                lListar();
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            bsProveedor.DataSource = new List<BandejaProveedoresCompraAnticipo>();
            gcProveedor.DataSource = bsProveedor;
            lListar();
        }

        private void lListar()
        {
            var menu = Tag.ToString().Split(',');
            var poObject = loLogicaNegocio.goListarBandejaProveedores(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
            if (poObject != null)
            {
                bsProveedor.DataSource = new List<BandejaProveedoresCompraAnticipo>();
                bsProveedor.DataSource = poObject;
                gcProveedor.DataSource = bsProveedor.DataSource;
            }
        }

        private void lColumnas()
        {
            dgvProveedor.OptionsView.RowAutoHeight = true;
            for (int i = 0; i < dgvProveedor.Columns.Count; i++)
            {
                dgvProveedor.Columns[i].OptionsColumn.AllowEdit = false;
            }
            dgvProveedor.Columns["Sel"].OptionsColumn.AllowEdit = true;
            dgvProveedor.Columns["Sel"].Width = 30;
            dgvProveedor.Columns["IdOrdenCompraProveedor"].Width = 50;
            dgvProveedor.Columns["Estado"].Width = 60;
            dgvProveedor.Columns["Observacion"].Width = 120;
            dgvProveedor.Columns["IdOrdenCompraProveedor"].Caption = "No.";
            dgvProveedor.Columns["Nombre"].Caption = "Proveedor";

            dgvProveedor.Columns["Nombre"].ColumnEdit = rpiMedDescripcion;
            dgvProveedor.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;

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
