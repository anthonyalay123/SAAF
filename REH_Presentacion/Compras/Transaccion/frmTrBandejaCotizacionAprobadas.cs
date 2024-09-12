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
    public partial class frmTrBandejaCotizacionAprobadas : frmBaseTrxDev
    {
        #region Variables
        BindingSource bsDatos = new BindingSource();
        clsNCotizacion loLogicaNegocio = new clsNCotizacion();
        List <CotizacionAprobacion> BandejaItems = new List<CotizacionAprobacion>();
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnReport;
        RepositoryItemMemoEdit rpiMedDescripcion;
        #endregion

        #region Eventos

        public frmTrBandejaCotizacionAprobadas()
        {
            InitializeComponent();
            rpiBtnShow = new RepositoryItemButtonEdit();
            rpiBtnReport = new RepositoryItemButtonEdit();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnReport.ButtonClick += rpiBtnReport_ButtonClick;
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;


        }

        private void frmTrBandejaCotizacionAprobadas_Load(object sender, EventArgs e)
        {
            try
            {
                lListar();
                lColumnas();
               // gcItems.DataSource = BandejaItems;
                lCargarEventosBotones();

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
                lRefrescar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lRefrescar()
        {
            dgvItems.Columns.Clear();

            BandejaItems = new List<CotizacionAprobacion>();
            lListar();
            lColumnas();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                
                dgvBandejaCotizacion.PostEditor();
                
                var piIndex = dgvBandejaCotizacion.GetFocusedDataSourceRowIndex();
                var poLista = (List<BandejaCotizacionAprobacion>)bsDatos.DataSource;
                BandejaItems = new List<CotizacionAprobacion>();
                var validar = poLista.Where(x => x.Sel == true).ToList();
                if (validar.Count !=0)
                {
                    string z = string.Empty;
                    foreach (var item in validar)
                    {
                        
                        if (loLogicaNegocio.VerificarOrdenCompra(poLista[piIndex].IdCotizacion)!=null)
                        {
                            z = z + loLogicaNegocio.VerificarOrdenCompra(poLista[piIndex].IdCotizacion).IdCotizacion.ToString() + ", " ;
                        } 

                    }

                    if (z== "")
                    {
                        frmTrOrdenCompra poFrmMostrarFormulario = new frmTrOrdenCompra();
                        foreach (var x in poLista)
                        {

                            var id = x.IdCotizacion;
                            if (x.Sel == true)
                            {
                                string psForma = Diccionario.Tablas.Menu.OrdenCompra;
                                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);

                                poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                                poFrmMostrarFormulario.Text = poForm.Nombre;
                                poFrmMostrarFormulario.ShowInTaskbar = true;
                                poFrmMostrarFormulario.MdiParent = this.ParentForm;
                                if (poForm != null)
                                {
                                    poFrmMostrarFormulario.plCotizaciones.Add(x.IdCotizacion);
                                }

                                else
                                {
                                    XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }

                        }
                        poFrmMostrarFormulario.Show();
                        lRefrescar();

                    }
                    else
                    {
                        XtraMessageBox.Show(string.Format("La Cotizacion No: '{0}' ya han sido generadas", z), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
                else
                {
                    XtraMessageBox.Show("Debe elegir Cotizaciones ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                int piIndex = dgvBandejaCotizacion.GetFocusedDataSourceRowIndex();

                var poLista = (List<BandejaCotizacionAprobacion>)bsDatos.DataSource;
                if (piIndex >= 0) { }
                {
                    clsComun.gImprimirCotizacionGanadora(poLista[piIndex].IdCotizacion);

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
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvBandejaCotizacion.GetFocusedDataSourceRowIndex();

                var poLista = (List<BandejaCotizacionAprobacion>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    string psForma = Diccionario.Tablas.Menu.AprobacionCotizacion;
                    var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);

                    if (poForm != null)
                    {
                        frmTrCotizacionAprobacion poFrmMostrarFormulario = new frmTrCotizacionAprobacion();
                        poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;

                        poFrmMostrarFormulario.Text = poForm.Nombre;
                        poFrmMostrarFormulario.ShowInTaskbar = true;
                        poFrmMostrarFormulario.lIdCotizacion = poLista[piIndex].IdCotizacion;
                        poFrmMostrarFormulario.MdiParent = this.ParentForm;
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

        private void dgvBandejaCotizacion_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            dgvBandejaCotizacion.PostEditor();
            var piIndex = dgvBandejaCotizacion.GetFocusedDataSourceRowIndex();
            var poLista = (List<BandejaCotizacionAprobacion>)bsDatos.DataSource;
            BandejaItems = new List<CotizacionAprobacion>();
            foreach (var x in poLista)
            {
                var id = x.IdCotizacion;
                if (x.Sel == true)
                {
                    foreach (var y in loLogicaNegocio.goListarBandejaCotizacionGanadora(x.IdCotizacion))
                    {
                        BandejaItems.Add(y);
                    }

                }

            }

            BandejaItems = (
                    from p in BandejaItems
                    group p by new { p.Valor, p.Descripcion,p.IdProveedor } into g
                    select new CotizacionAprobacion()
                    {
                        Descripcion = g.Key.Descripcion,
                        //IdOrdenCompraItem = g.Select(c => c.IdOrdenCompraItem).FirstOrDefault(),
                        Proveedor = g.Select(c => c.Proveedor).FirstOrDefault(),
                        IdentificacionProveedor = g.Select(c => c.IdentificacionProveedor).FirstOrDefault(),
                        Cantidad = g.Sum(t => t.Cantidad),
                        Valor = g.Select(c => c.Valor).FirstOrDefault(),
                        SubTotal = g.Select(c => c.SubTotal).FirstOrDefault(),
                        ValorIva = g.Select(c => c.ValorIva).FirstOrDefault(),
                        Total = g.Select(c => c.Total).FirstOrDefault(),
                        Observacion = g.Select(c => c.Observacion).FirstOrDefault(),
                        IdProveedor = g.Key.IdProveedor
                    }
                    ).ToList();
            gcItems.DataSource = BandejaItems;
            dgvItems.Columns["Proveedor"].GroupIndex = 2;
            dgvItems.ExpandAllGroups();
            gcItems.RefreshDataSource();
            

        }

        #endregion



        #region Metodos

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
         //   if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Click += btnGenerar_Click;
            //if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            //if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

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
            colXmlDown.Width = 30;
        }

        private void lListar()
        {
            bsDatos.DataSource = new List<BandejaCotizacionAprobacion>();
            var menu = Tag.ToString().Split(',');

            var poObject = loLogicaNegocio.goListarBandejaCotizacionAprobadas(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
          
            if (poObject != null)
            {

                bsDatos.DataSource = new List<BandejaCotizacionAprobacion>();
                bsDatos.DataSource = poObject;
                gcBandejaCotizacion.DataSource = bsDatos.DataSource;
            }



            gcItems.DataSource = BandejaItems;


        }

        private void lColocarbotonReporte(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Imprimir";
            colXmlDown.ColumnEdit = rpiBtnReport;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;


            rpiBtnReport.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnReport.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/print/printer_16x16.png");
            rpiBtnReport.TextEditStyle = TextEditStyles.HideTextEditor;
             colXmlDown.Width = 38;


        }


        private void lColumnas()
        {


            dgvBandejaCotizacion.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaCotizacion.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaCotizacion.Columns["Usuario"].ColumnEdit = rpiMedDescripcion;
            dgvItems.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
            dgvItems.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            for (int i = 0; i < dgvBandejaCotizacion.Columns.Count; i++)
            {
                dgvBandejaCotizacion.Columns[i].OptionsColumn.AllowEdit = false;
                dgvBandejaCotizacion.Columns[i].OptionsColumn.FixedWidth = true;
            }
            //dgvBandejaSolicitud.Columns["Id"].Width = 28;
            //dgvBandejaSolicitud.Columns["Descripcion"].Width = 120;
            dgvBandejaCotizacion.Columns["Sel"].OptionsColumn.AllowEdit = true;
            dgvBandejaCotizacion.Columns["Visualizar"].OptionsColumn.AllowEdit = true;
            dgvBandejaCotizacion.Columns["Imprimir"].OptionsColumn.AllowEdit = true;
            dgvBandejaCotizacion.Columns["IdCotizacion"].Caption = "No.";
            dgvBandejaCotizacion.Columns["Usuario"].Caption = "Solicita";
            //dgvBandejaCotizacion.Columns["Imprimir"].OptionsColumn.AllowEdit = true;
            // lColocarbotonCotizar(dgvBandejaSolicitud.Columns["Cotizar"]);
            //lColocarbotonReporte(dgvBandejaSolicitud.Columns["Imprimir"]);

            lColocarbotonVisualizar(dgvBandejaCotizacion.Columns["Visualizar"]);

            lColocarbotonReporte(dgvBandejaCotizacion.Columns["Imprimir"]);

            dgvBandejaCotizacion.Columns["Descripcion"].Width = 220;
            dgvBandejaCotizacion.OptionsView.RowAutoHeight = true;
            dgvItems.OptionsView.RowAutoHeight = true;
            dgvBandejaCotizacion.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            dgvBandejaCotizacion.BestFitColumns();
            dgvBandejaCotizacion.Columns["Descripcion"].Width = 220;
            dgvBandejaCotizacion.Columns["Observacion"].Width = 175;

            for (int i = 0; i < dgvItems.Columns.Count; i++)
            {
                dgvItems.Columns[i].OptionsColumn.AllowEdit = false;
                //dgvBandejaSolicitud.Columns[i].OptionsColumn.FixedWidth = true;
            }

            dgvItems.Columns["idCotizacion"].Visible = false;
            dgvItems.Columns["IdProveedor"].Visible = false;
            dgvItems.Columns["IdCotizacionGanadora"].Visible = false;
            dgvItems.Columns["IdentificacionProveedor"].Visible = false;
            //dgvItems.Columns["IdProveedor"].Visible = false;

            dgvItems.Columns["Valor"].UnboundType = UnboundColumnType.Decimal;
            dgvItems.Columns["Valor"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvItems.Columns["Valor"].DisplayFormat.FormatString = "c2";
       

            lColocarTotal("Total", dgvItems);
            lColocarTotal("SubTotal", dgvItems);
            lColocarTotal("ValorIva", dgvItems);

        }
        #endregion

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
