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
    public partial class frmTrSolicitudesCompraAprobadas : frmBaseTrxDev
    {


        #region Variables
        BindingSource bsDatos = new BindingSource();
        clsNSolicitudCompra loLogicaNegocio = new clsNSolicitudCompra();
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnReport;
        RepositoryItemMemoEdit rpiMedDescripcion;
        public int lId;
        public int lIdCotizacion = 0;

        #endregion

        #region Eventos

        public frmTrSolicitudesCompraAprobadas()
        {
            rpiBtnShow = new RepositoryItemButtonEdit();

            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;

            rpiBtnReport = new RepositoryItemButtonEdit();
            rpiBtnReport.ButtonClick += rpiBtnReport_ButtonClick;
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;

            InitializeComponent();
        }

        private void frmTrSolicitudesCompraAprobadas_Load(object sender, EventArgs e)
        {
            try
            {
                bsDatos.DataSource = new SolicitudesCompraAprobadas();
                gcBandejaSolicitud.DataSource = bsDatos;

                lListar();
               
                lCargarEventosBotones();
                if (lId != 0)
                {
                    var poSolicitud = loLogicaNegocio.goBuscarSolicitudCompra(lId);
                    var poLista = (SolicitudesCompraAprobadas)bsDatos.DataSource;
                    poLista.Id = poSolicitud.IdSolicitudCompra;
                    poLista.Estado = poSolicitud.CodigoEstado;
                    poLista.Descripcion = poSolicitud.Descripcion;
                    poLista.Observacion = poSolicitud.Observacion;
                    poLista.Fecha = poSolicitud.FechaIngreso;
                    poLista.Solicita = poSolicitud.Usuario;

                }
                lColumnas();
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        private void lImprimir()
        {
            int piIndex = dgvBandejaSolicitud.GetFocusedDataSourceRowIndex();
            var poLista = (List<SolicitudesCompraAprobadas>)bsDatos.DataSource;
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

        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                 lMostrarCotizacion();
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
                List<SolicitudesCompraAprobadasExcel> listbd = new List<SolicitudesCompraAprobadasExcel>();

                var poLista = (List<SolicitudesCompraAprobadas>)bsDatos.DataSource;
                if (poLista.Count>0)
                {
                    foreach (var po in poLista)
                    {
                        SolicitudesCompraAprobadasExcel be = new SolicitudesCompraAprobadasExcel();
                        be.Id = po.Id;
                        be.Estado = po.Estado;
                        be.Fecha = po.Fecha;
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
                else
                {
                    XtraMessageBox.Show("No existen registros para exportar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void btnCotizar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvBandejaSolicitud.PostEditor();
                var poLista = (List<SolicitudesCompraAprobadas>)bsDatos.DataSource;
                
                
              
                var validar = poLista.Where(x => x.Cotizar == true).FirstOrDefault();
                if (validar!=null)
                {
                    var validar1 = poLista.Where(x => x.Cotizar == true).ToList();
                    string MsgCotizar = "";
                    foreach (var item in validar1)
                    {
                        if (!string.IsNullOrEmpty(loLogicaNegocio.sVerificarCotizacion(item.Id)))
                        {
                            MsgCotizar = MsgCotizar + "\n" + loLogicaNegocio.sVerificarCotizacion(item.Id);
                        }
                      
                       
                    }
                    if (MsgCotizar == "")
                    {
                        lMostrarCotizacion();
                    }
                    else
                    {
                        XtraMessageBox.Show("Las Solicitudes: " + MsgCotizar+ "\nya fueron cotizadas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
                else
                {
                    XtraMessageBox.Show("Debe seleccionar un registro primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


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

        //private void btnEliminar_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int piIndex = dgvBandejaOrdenPago.GetFocusedDataSourceRowIndex();
        //        var poLista = (List<BandejaOrdenPago>)bsDatos.DataSource;
        //        int pId = poLista[piIndex].Id;

        //        if (pId != 0)
        //        {
        //            DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
        //            if (dialogResult == DialogResult.Yes)
        //            {
        //                loLogicaNegocio.gEliminarMaestro(pId, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
        //                XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                lListar();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}


        #endregion

        #region Metodos
        private void lMostrarCotizacion()
        {
            int piIndex = dgvBandejaSolicitud.GetFocusedDataSourceRowIndex();
            var poLista = (List<SolicitudesCompraAprobadas>)bsDatos.DataSource;
            if (piIndex >= 0)
            {
                string psForma = Diccionario.Tablas.Menu.Cotizacion;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);

                if (poForm != null)
                {
                    frmTrCotizacion poFrmMostrarFormulario = new frmTrCotizacion();
                    //if (poLista[piIndex].CodigoEstado == Diccionario.Cotizado)
                    //{
                    //    poFrmMostrarFormulario.lIdCotizacion = loLogicaNegocio.goBuscarCotizacion(poLista[piIndex].Id).IdCotizacion;
                    //}
                    poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    poFrmMostrarFormulario.Text = poForm.Nombre;
                    poFrmMostrarFormulario.ShowInTaskbar = true;
                    poFrmMostrarFormulario.MdiParent = this.ParentForm;
                    poFrmMostrarFormulario.lIdSolicitudCompra = poLista.Where(x=> x.Cotizar==true).Select(x=> x.Id).ToList();
                    poFrmMostrarFormulario.Show();

                 //   lRecargar();

                }
                else
                {
                    XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnCotizar"] != null) tstBotones.Items["btnCotizar"].Click += btnCotizar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnCotizar"].Click += btnCotizar_Click;
        }

        private void lListar()
        {
            var menu = Tag.ToString().Split(',');
            var poObject = loLogicaNegocio.goSolicitudesAprobadas(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
            if (poObject != null)
            {
                bsDatos.DataSource = new List<SolicitudesCompraAprobadas>();
                bsDatos.DataSource = poObject;
                gcBandejaSolicitud.DataSource = bsDatos.DataSource;
            }
        }

        private void lColumnas()
        {
            dgvBandejaSolicitud.OptionsBehavior.Editable = true;
            dgvBandejaSolicitud.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            dgvBandejaSolicitud.OptionsView.RowAutoHeight = true;
            dgvBandejaSolicitud.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaSolicitud.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaSolicitud.Columns["Solicita"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaSolicitud.Columns["Aprueba"].ColumnEdit = rpiMedDescripcion;

            dgvBandejaSolicitud.Columns["Terminal"].Visible = false;
            dgvBandejaSolicitud.Columns["CodigoEstado"].Visible = false;
            
            dgvBandejaSolicitud.Columns["Id"].Caption = "No";

            for (int i = 0; i < dgvBandejaSolicitud.Columns.Count; i++)
            {
                dgvBandejaSolicitud.Columns[i].OptionsColumn.AllowEdit = false;
                dgvBandejaSolicitud.Columns[i].OptionsColumn.FixedWidth = true;
            }
            dgvBandejaSolicitud.Columns["Id"].Width = 28;
            //dgvBandejaSolicitud.Columns["Descripcion"].Width = 120;
            dgvBandejaSolicitud.Columns["Cotizar"].Width = 30;
            dgvBandejaSolicitud.Columns["Cotizar"].OptionsColumn.AllowEdit = true;
            dgvBandejaSolicitud.Columns["Cotizar"].Caption = "Sel";

            dgvBandejaSolicitud.Columns["Imprimir"].OptionsColumn.AllowEdit = true;
            // lColocarbotonCotizar(dgvBandejaSolicitud.Columns["Cotizar"]);
            dgvBandejaSolicitud.Columns["Estado"].Width = 65;
            dgvBandejaSolicitud.Columns["Fecha"].Width = 60;
            dgvBandejaSolicitud.Columns["FechaEntrega"].Width = 80;
            dgvBandejaSolicitud.Columns["Descripcion"].Width = 200;
            dgvBandejaSolicitud.Columns["Observacion"].Width = 150;

            clsComun.gDibujarBotonGrid(rpiBtnReport, dgvBandejaSolicitud.Columns["Imprimir"], "Imprimir", Diccionario.ButtonGridImage.printer_16x16, 40);

            //lColocarbotonReporte(dgvBandejaSolicitud.Columns["Imprimir"]);
            dgvBandejaSolicitud.OptionsView.RowAutoHeight = true;

            ////dgvBandejaSolicitud.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            ////dgvBandejaSolicitud.BestFitColumns();
        }


        private void lColocarbotonCotizar(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {

            colXmlDown.ColumnEdit = rpiBtnShow;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowEdit = false;

            rpiBtnShow.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnShow.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/show_16x16.png");
            rpiBtnShow.TextEditStyle = TextEditStyles.HideTextEditor;
            //colXmlDown.Width = 50;
        }

        private void lRecargar()
        {
            // dgvBandejaSolicitud.Columns.Clear();
            //gcBandejaSolicitud.DataSource = null;
            lListar();
            //lColumnas();


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


        #endregion


    }
}
