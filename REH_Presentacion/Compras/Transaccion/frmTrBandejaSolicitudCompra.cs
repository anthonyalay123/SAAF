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
    public partial class frmTrBandejaSolicitudCompra : frmBaseTrxDev
    {

        #region Variables
        BindingSource bsDatos =  new BindingSource();
        clsNSolicitudCompra loLogicaNegocio = new clsNSolicitudCompra();
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnReport;
        RepositoryItemMemoEdit rpiMedDescripcion;
        bool pbCargado = false;
        string anterior = null;
        #endregion
        public frmTrBandejaSolicitudCompra()
        {
            InitializeComponent();
            
        }

        private void frmTrBandejaSolicitudCompra_Load(object sender, EventArgs e)
        {
            try
            {
               
                rpiBtnShow = new RepositoryItemButtonEdit();
                rpiBtnReport = new RepositoryItemButtonEdit();
                rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
                rpiBtnReport.ButtonClick += rpiBtnReport_ButtonClick;
                bsDatos.DataSource = new List<BandejaSolicitudCompra>();
                rpiMedDescripcion = new RepositoryItemMemoEdit();
                rpiMedDescripcion.WordWrap = true;
                rpiMedDescripcion.MaxLength = 120;

                gcBandejaSolicitud.DataSource = bsDatos;
                lListar();
                lColumnas();
                lCargarEventosBotones();
                dgvBandejaSolicitud.OptionsView.ShowAutoFilterRow = true;
                lRecargar();

                pbCargado = true;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Evento del boton Grabar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvBandejaSolicitud.PostEditor();
                var poLista = (List<BandejaSolicitudCompra>)bsDatos.DataSource;
                bool poCambios = false;
                for (int i = 0; i < poLista.Count; i++)
                {
                    if (poLista[i].CodigoEstado != null)
                    {
                        poCambios = true;
                    } }
                if (poCambios == true)
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        for (int i = 0; i < poLista.Count; i++)
                        {
                            if (poLista[i].CodigoEstado != null)
                            {

                                loLogicaNegocio.gActualizarEstadoSolicitud(poLista[i].Id, poLista[i].CodigoEstado, poLista[i].Observacion, poLista[i].Usuario, poLista[i].Terminal);
                               

                            }
                            lListar();
                        }
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                
                
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
                List<BandejaSolicitudCompraExcel> listbd = new List<BandejaSolicitudCompraExcel>();

                var poLista = (List<BandejaSolicitudCompra>)bsDatos.DataSource;
                if (poLista.Count > 0)
                {
                    foreach (var po in poLista)
                    {
                        BandejaSolicitudCompraExcel be = new BandejaSolicitudCompraExcel();
                        be.Id = po.Id;
                        be.Departamento = po.Departamento;
                        be.Solicita = po.Persona;
                        be.Descripcion = po.Descripcion;
                        be.Observacion = po.Observacion;

                        be.FechaCreacion = po.Fecha;
                        be.FechaEntrega = po.FechaEntrega;


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
                    catch (Exception)
                    {
                        XtraMessageBox.Show("Fallo al guardar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gc.Visible = false;
                    }
                    gc.Visible = false;
                }
                else
                {
                    XtraMessageBox.Show("No hay registros para exportar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvBandejaSolicitud_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int piIndex = dgvBandejaSolicitud.GetFocusedDataSourceRowIndex();
                    var poLista = (List<BandejaSolicitudCompra>)bsDatos.DataSource;
                    if (piIndex!= -2147483648)
                {
                    
                    if (e.Value.ToString() != "False")
                    {

                        //Aprobar
                        if (e.Column.Name == "colAprobar")
                        {
                            LValidarEstado(Diccionario.Aprobado, poLista[piIndex]);
                            rdbAprobarTodos.Checked = false;
                            rdbCorregirTodos.Checked = false;
                            rdbRechazarTodos.Checked = false;

                        }
                        //Corregir
                        else if (e.Column.Name == "colCorregir")
                        {
                            LValidarEstado(Diccionario.Corregir, poLista[piIndex]);
                           
                            rdbAprobarTodos.Checked = false;
                            rdbCorregirTodos.Checked = false;
                            rdbRechazarTodos.Checked = false;
                        }
                        //Rechazar
                        else if (e.Column.Name == "colRechazar")
                        {
                            LValidarEstado(Diccionario.Negado, poLista[piIndex]);
                            
                            rdbAprobarTodos.Checked = false;
                            rdbCorregirTodos.Checked = false;
                            rdbRechazarTodos.Checked = false;
                        }
                        anterior = e.Column.Name;
                    }
                    else
                    {
                        poLista[piIndex].Aprobar = false;
                        poLista[piIndex].Corregir = false;
                        poLista[piIndex].Rechazar = false;
                        poLista[piIndex].CodigoEstado = null;
                        poLista[piIndex].Terminal = null;
                        poLista[piIndex].CodigoEstado = null;
                        anterior = null;
                        rdbAprobarTodos.Checked = false;
                        rdbCorregirTodos.Checked = false;
                        rdbRechazarTodos.Checked = false;
                    }
                  

                    dgvBandejaSolicitud.RefreshData();
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
        private void lColocarbotonReporte (DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
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
        private void lmostrarSolicitudCompra()
        {
            int piIndex = dgvBandejaSolicitud.GetFocusedDataSourceRowIndex();
            var poLista = (List<BandejaSolicitudCompra>)bsDatos.DataSource;
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
                    poFrmMostrarFormulario.MdiParent = this.ParentForm;
                    poFrmMostrarFormulario.lId = poLista[piIndex].Id;
                    poFrmMostrarFormulario.Show();


                    
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
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
        }
        private void lListar()
        {
            var menu = Tag.ToString().Split(','); 
            var poObject = loLogicaNegocio.goListarBandeja(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
            if (poObject != null)
            {
                bsDatos.DataSource = new List<BandejaSolicitudCompra>();
                bsDatos.DataSource = poObject;
                gcBandejaSolicitud.DataSource = bsDatos.DataSource;
            }
        }
        private void LValidarEstado(string Estado, BandejaSolicitudCompra pLista)
        {
            if (Estado == Diccionario.Aprobado)
            {
                pLista.Aprobar = true;
                pLista.Corregir = false;
                pLista.Rechazar = false;
                pLista.Usuario = clsPrincipal.gsUsuario;
                pLista.Terminal = clsPrincipal.gsTerminal;
                pLista.CodigoEstado = Diccionario.Aprobado;
            }
            if (Estado == Diccionario.Corregir)
            {
                pLista.Aprobar = false;
                pLista.Corregir = true;
                pLista.Rechazar = false;
                pLista.Usuario = clsPrincipal.gsUsuario;
                pLista.Terminal = clsPrincipal.gsTerminal;
                pLista.CodigoEstado = Diccionario.Corregir;
            }
            if (Estado == Diccionario.Negado)
            {
                pLista.Aprobar = false;
                pLista.Corregir = false;
                pLista.Rechazar = true;
                pLista.Usuario = clsPrincipal.gsUsuario;
                pLista.Terminal = clsPrincipal.gsTerminal;
                pLista.CodigoEstado = Diccionario.Negado;
            }
        }
        private void lColumnas()
        {
            dgvBandejaSolicitud.OptionsView.RowAutoHeight = true;


            dgvBandejaSolicitud.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaSolicitud.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaSolicitud.Columns["Persona"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaSolicitud.Columns["Persona"].Caption = "Solicita";
            dgvBandejaSolicitud.Columns["Fecha"].Caption = "Fecha Creacion";
            dgvBandejaSolicitud.Columns["Id"].Caption = "No";
          
            dgvBandejaSolicitud.Columns["Estado"].Visible = false;
            dgvBandejaSolicitud.Columns["Usuario"].Visible = false;
            dgvBandejaSolicitud.Columns["Terminal"].Visible = false;
            dgvBandejaSolicitud.Columns["CodigoEstado"].Visible = false;

            for (int i = 0; i < dgvBandejaSolicitud.Columns.Count; i++)
            {
                dgvBandejaSolicitud.Columns[i].OptionsColumn.AllowEdit = false;
               // dgvBandejaSolicitud.Columns[i].OptionsColumn.FixedWidth = true;
            }
            dgvBandejaSolicitud.Columns["Id"].Width = 28;
            dgvBandejaSolicitud.Columns["Descripcion"].Width = 140;
            dgvBandejaSolicitud.Columns["Persona"].Width = 120;
            dgvBandejaSolicitud.Columns["Aprobar"].Width = 50;
            dgvBandejaSolicitud.Columns["Corregir"].Width = 50;
            dgvBandejaSolicitud.Columns["Rechazar"].Width = 50;
            var Aprobar = dgvBandejaSolicitud.Columns["Aprobar"];
            var Corregir = dgvBandejaSolicitud.Columns["Corregir"];
            var Rechazar = dgvBandejaSolicitud.Columns["Rechazar"];
            dgvBandejaSolicitud.Columns["Observacion"].OptionsColumn.AllowEdit = true;
            dgvBandejaSolicitud.Columns["Reporte"].OptionsColumn.AllowEdit = true;
            dgvBandejaSolicitud.Columns["Visualizar"].OptionsColumn.AllowEdit = true;
            Aprobar.OptionsColumn.AllowEdit = true;
            Corregir.OptionsColumn.AllowEdit = true;
            Rechazar.OptionsColumn.AllowEdit = true;

            lColocarbotonReporte(dgvBandejaSolicitud.Columns["Reporte"]);
            lColocarbotonVisualizar(dgvBandejaSolicitud.Columns["Visualizar"]);

            
            
            //dgvBandejaSolicitud.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            //dgvBandejaSolicitud.BestFitColumns();

            dgvBandejaSolicitud.Columns["Descripcion"].Width = 150;
            dgvBandejaSolicitud.Columns["Observacion"].Width = 150;
        }
        private void lImprimir()
        {
            int piIndex = dgvBandejaSolicitud.GetFocusedDataSourceRowIndex();
            var poLista = (List<BandejaSolicitudCompra>)bsDatos.DataSource;
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

       

        private void rdbAprobarTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (pbCargado)
            {
                if (rdbAprobarTodos.Checked == true)
                {
                    var poLista = (List<BandejaSolicitudCompra>)bsDatos.DataSource;
                    for (int i = 0; i < poLista.Count; i++)
                    {
                        LValidarEstado(Diccionario.Aprobado, poLista[i]);
                    }
                    dgvBandejaSolicitud.RefreshData();

                }
            }
            

        }

        private void rdbRechazarTodos_CheckedChanged(object sender, EventArgs e)
        {
            {
                if (rdbRechazarTodos.Checked == true)
                {
                    var poLista = (List<BandejaSolicitudCompra>)bsDatos.DataSource;

                    for (int i = 0; i < poLista.Count; i++)
                    {
                        LValidarEstado(Diccionario.Negado, poLista[i]);
                    }

                }
                dgvBandejaSolicitud.RefreshData();
            }

        }

        private void rdbCorregirTodos_CheckedChanged(object sender, EventArgs e)
        {

            if (rdbCorregirTodos.Checked == true)
            {
                var poLista = (List<BandejaSolicitudCompra>)bsDatos.DataSource;

                for (int i = 0; i < poLista.Count; i++)
                {
                    LValidarEstado(Diccionario.Corregir, poLista[i]);
                }
                dgvBandejaSolicitud.RefreshData();
            }
        }

        private void lRecargar()
        {
            gcBandejaSolicitud.DataSource = null;
            dgvBandejaSolicitud.Columns.Clear();
            
            gcBandejaSolicitud.DataSource = bsDatos;
            lListar();
            lColumnas(); 
            rdbAprobarTodos.Checked = false;
            rdbCorregirTodos.Checked = false;
            rdbRechazarTodos.Checked = false;

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
