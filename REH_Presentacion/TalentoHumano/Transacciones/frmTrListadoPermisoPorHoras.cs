using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.REH;
using REH_Negocio;
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

namespace REH_Presentacion.TalentoHumano.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 11/03/2022
    /// Formulario de bandeja de aprobación de permisos por horas
    /// </summary>
    public partial class frmTrListadoPermisoPorHoras : frmBaseTrxDev
    {
        //clsNPermiso
        clsNPermiso loLogicaNegocio = new clsNPermiso();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnReport;
        RepositoryItemMemoEdit rpiMedDescripcion;

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmTrListadoPermisoPorHoras()
        {
            rpiBtnShow = new RepositoryItemButtonEdit();
            rpiBtnReport = new RepositoryItemButtonEdit();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnReport.ButtonClick += rpiBtnReport_ButtonClick;
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;
           
            InitializeComponent();
        }

        /// <summary>
        /// Evento que inicializa el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTrBandejaOrdenPago_Load(object sender, EventArgs e)
        {
            try
            {
                bsDatos.DataSource = new List<BandejaPermisoHoras>();
                gcDatos.DataSource = bsDatos;
                lListar();
                lColumnas();
                lCargarEventosBotones();
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) ;
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
                lMostrarOrdenPago();
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
                var poLista = (List<BandejaPermisoHoras>)bsDatos.DataSource;
                var piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                clsComun.gImprimirOrdenPago(poLista[piIndex].Id);
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

   
        #endregion

        private void lListar()
        {
            var menu = Tag.ToString().Split(',');
            var poObject = loLogicaNegocio.goListarPermisosPorHoras(clsPrincipal.gsUsuario);

            //if (poObject != null)
            //{
            //    bsDatos.DataSource = new List<BandejaOrdenPago>();
            //    bsDatos.DataSource = poObject;
            //    gcBandejaOrdenPago.DataSource = bsDatos.DataSource;
            //}

            if (poObject != null)
            {
                //foreach (var x in poObject)
                //{
                //    x.Aprobaciones = loLogicaNegocio.goBuscarCantidadAprobacion(x.Id);
                //    x.UsuariosAprobacion = loLogicaNegocio.goBuscarUsuarioAprobacion(x.Id);
                //}
                bsDatos.DataSource = new List<BandejaPermisoHoras>();
                bsDatos.DataSource = poObject;
                gcDatos.DataSource = bsDatos.DataSource;
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
            colXmlDown.Width = 48;
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
            colXmlDown.Width = 48;
        }

        public void lMostrarOrdenPago()
        {
            int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
            var poLista = (List<BandejaPermisoHoras>)bsDatos.DataSource;
            if (piIndex >= 0)
            {

                string psForma = Diccionario.Tablas.Menu.OrdenPago;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                if (poForm != null)
                {
                    //frmTrOrdenPago poFrmMostrarFormulario = new frmTrOrdenPago();
                    //poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    //poFrmMostrarFormulario.Text = poForm.Nombre;
                    //poFrmMostrarFormulario.ShowInTaskbar = true;
                    //poFrmMostrarFormulario.MdiParent = this.ParentForm;
                    //poFrmMostrarFormulario.lid = poLista[piIndex].Id;
                    //poFrmMostrarFormulario.Show();
                }
                else
                {
                    XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lColumnas()
        {
            dgvDatos.OptionsView.RowAutoHeight = true;
            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                dgvDatos.Columns[i].OptionsColumn.AllowEdit = false;
                // dgvBandejaSolicitud.Columns[i].OptionsColumn.FixedWidth = true;
            }

            dgvDatos.Columns["Sel"].Visible = false;

            dgvDatos.Columns["Ver"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["Imprimir"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["Sel"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["Id"].Caption = "No.";
            //dgvDatos.Columns["Valor"].Caption = "Total Orden de Pago";
            lColocarbotonVisualizar(dgvDatos.Columns["Ver"]);
            lColocarbotonReporte(dgvDatos.Columns["Imprimir"]);
            //dgvDatos.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            //dgvDatos.Columns["Proveedor"].ColumnEdit = rpiMedDescripcion;
            //dgvDatos.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            //dgvBandejaOrdenPago.Columns["Aprobaciones"].ColumnEdit = rpiMedDescripcion;
            //dgvDatos.Columns["UsuariosAprobacion"].ColumnEdit = rpiMedDescripcion;
            //dgvBandejaOrdenPago.Columns["TotalOrdenCompra"].ColumnEdit = rpiMedDescripcion;
            //dgvBandejaOrdenPago.Columns["Valor"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;

            dgvDatos.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["TipoPermiso"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Empleado"].ColumnEdit = rpiMedDescripcion;

            dgvDatos.Columns["Id"].Visible = false;
            dgvDatos.Columns["Ver"].Visible = false;
            dgvDatos.Columns["Imprimir"].Visible = false;

            dgvDatos.Columns["Sel"].Width = 30;
            dgvDatos.Columns["Id"].Width = 40;
            dgvDatos.Columns["Ver"].Width = 40;
            dgvDatos.Columns["Fecha"].Width = 45;
            dgvDatos.Columns["HoraInicio"].Width = 30;
            dgvDatos.Columns["HoraFin"].Width = 30;
            dgvDatos.Columns["Estado"].Width = 50;
            dgvDatos.Columns["Aprobaciones"].Width = 45;
            dgvDatos.Columns["Imprimir"].Width = 45;
            //dgvDatos.Columns["Valor"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //dgvDatos.Columns["Valor"].DisplayFormat.FormatString = "c2";
            //dgvDatos.Columns["TotalOrdenCompra"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //dgvDatos.Columns["TotalOrdenCompra"].DisplayFormat.FormatString = "c2";
            //dgvDatos.Columns["Diferencia"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //dgvDatos.Columns["Diferencia"].DisplayFormat.FormatString = "c2";
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            //if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            //if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnRefrescar_Click;
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

        public void lColocarTotal(string psNameColumn, DevExpress.XtraGrid.Views.Grid.GridView dgv)
        {
            //  var psNameColumn = "Total";
            //dgvCotizacionDetalle.Columns[psNameColumn].UnboundType = UnboundColumnType.Decimal;
            dgv.Columns[psNameColumn].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgv.Columns[psNameColumn].DisplayFormat.FormatString = "c2";
            dgv.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");


            GridGroupSummaryItem item1 = new GridGroupSummaryItem();
            item1.FieldName = psNameColumn;
            item1.SummaryType = SummaryItemType.Sum;
            item1.DisplayFormat = "{0:c2}";
            item1.ShowInGroupColumnFooter = dgv.Columns[psNameColumn];
            dgv.GroupSummary.Add(item1);

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
