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
    public partial class frmTrRecibirProductos : frmBaseTrxDev
    {
        BindingSource bsDatos = new BindingSource();
        BindingSource bsDatosItems = new BindingSource();
        clsNOrdenCompra loLogicaNegocio = new clsNOrdenCompra();
        List<BandejaRecibirProductosItems> BandejaItems = new List<BandejaRecibirProductosItems>();
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnReport;
        RepositoryItemMemoEdit rpiMedDescripcion;

        public frmTrRecibirProductos()
        {
            InitializeComponent();

            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.MaxLength = 120;
        }

        private void frmTrRecibirProductos_Load(object sender, EventArgs e)
        {
            lListar();
            lColumnas();
            MostrarDetalle(0);
            lCargarEventosBotones();
        }

        private void lListar()
        {
            bsDatos.DataSource = new List<BandejaRecibirProductos>();
            bsDatosItems.DataSource = new List< BandejaRecibirProductosItems > ();
            gcBandejaOrdenCompra.DataSource = bsDatos;
            gcItems.DataSource = bsDatosItems;


            var menu = Tag.ToString().Split(',');

            var plID = loLogicaNegocio.goListarIdProveedores(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
            var poObject = loLogicaNegocio.goListarBandejaOrdenCompraRecibirProductos(clsPrincipal.gsUsuario, Int32.Parse(menu[0]), plID);

            if (poObject != null)
            {

                bsDatos.DataSource = new List<BandejaRecibirProductos>();
                bsDatos.DataSource = poObject;
                gcBandejaOrdenCompra.DataSource = bsDatos.DataSource;
            }
                BandejaItems = loLogicaNegocio.goListarBandejaOrdenCompraRecibirProductoItems(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));

        }

       


        private void dgvBandejaOrdenCompra_Click(object sender, EventArgs e)
        {
          
            dgvBandejaOrdenCompra.PostEditor();
            var piIndex = dgvBandejaOrdenCompra.GetFocusedDataSourceRowIndex();
            //   var poLista = (List<BandejaRecibirProductos>)bsDatos.DataSource;
            if (piIndex>=0)
            {
                MostrarDetalle(piIndex);
            }
            
        }


        private void MostrarDetalle(int piIndex)
        {
            dgvBandejaOrdenCompra.PostEditor();
            var poProveedor = (List<BandejaRecibirProductos>)bsDatos.DataSource;
            if (poProveedor.Count>0)
            {
                foreach (var item in poProveedor)
                {
                    item.Sel = false;
                }
                poProveedor[piIndex].Sel = true;

                var poItemsProveedor = BandejaItems.Where(i => i.NombreProveedor == poProveedor[piIndex].Nombre && i.IdOrdenCompraProveedor == poProveedor[piIndex].No).ToList();
                bsDatosItems.DataSource = poItemsProveedor;
                dgvItems.RefreshData();
                dgvBandejaOrdenCompra.RefreshData();
            }
            

        }

        private void lColumnas()
        {
            for (int i = 0; i < dgvBandejaOrdenCompra.Columns.Count; i++)
            {
                dgvBandejaOrdenCompra.Columns[i].OptionsColumn.AllowEdit = false;
            }

            dgvBandejaOrdenCompra.Columns["Sel"].Width = 2 ;
            dgvBandejaOrdenCompra.Columns["No"].Width = 5;
            dgvBandejaOrdenCompra.Columns["Identificacion"].Width = 30;
            dgvBandejaOrdenCompra.Columns["Fecha"].Width = 40;
            dgvBandejaOrdenCompra.Columns["Nombre"].Width = 200;
            dgvBandejaOrdenCompra.Columns["Nombre"].ColumnEdit = rpiMedDescripcion;
            for (int i = 0; i < dgvItems.Columns.Count; i++)
            {
                dgvItems.Columns[i].OptionsColumn.AllowEdit = false;
            }

            //  dgvItems.Columns["RecibiConforme"].ColumnEdit = rpiMedDescripcion;
            dgvItems.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
            dgvItems.Columns["RecibiConformeObservacion"].ColumnEdit = rpiMedDescripcion;
            dgvItems.Columns["NombreProveedor"].ColumnEdit = rpiMedDescripcion;
            dgvItems.OptionsView.RowAutoHeight = true;
            //    (dgvItems.Columns["RecibiConforme"].RealColumnEdit as RepositoryItemTextEdit).MaxLength = 3;
            dgvItems.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            dgvItems.Columns["IdOrdenCompra"].Visible = false;
            dgvItems.Columns["IdOrdenCompraProveedorItem"].Visible = false;
            dgvItems.Columns["IdOrdenCompraProveedor"].Visible = false;
            dgvItems.Columns["NombreProveedor"].Visible = false;
            dgvItems.Columns["RecibiConformeObservacion"].Caption = "Observacion";

            dgvItems.Columns["RecibiConforme"].Width = 30;
            dgvItems.Columns["Cantidad"].Width = 30;
          //  dgvItems.Columns["Fecha"].Width = 30;


            dgvItems.Columns["RecibiConforme"].OptionsColumn.AllowEdit = true;
            dgvItems.Columns["RecibiConformeObservacion"].OptionsColumn.AllowEdit = true;

        }


        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            //   if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            //if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            //if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvItems.PostEditor();
                //var poItems = (List<BandejaRecibirProductosItems>)bsDatosItems.DataSource;
                string psResult = "";
                foreach (var item in BandejaItems)
                {
                    if (item.RecibiConforme == true)
                    {
                        psResult = loLogicaNegocio.ActualizarRecibiConforme(item, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    }
                }
                if (string.IsNullOrEmpty(psResult))
                {
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lListar();
                }
            }
            catch (Exception ex)
            {


                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           

        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            lListar();
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
