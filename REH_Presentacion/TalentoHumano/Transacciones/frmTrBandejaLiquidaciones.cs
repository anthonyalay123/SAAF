using COM_Negocio;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Entidad.Entidades.Compras;
using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
using REH_Presentacion.Transacciones;
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
    public partial class frmTrBandejaLiquidaciones : frmBaseTrxDev
    {
        clsNNomina loLogicaNegocio = new clsNNomina();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnReport;
        RepositoryItemMemoEdit rpiMedDescripcion;
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();
        

        #region MyRegion

        public frmTrBandejaLiquidaciones()
        {
            rpiBtnShow = new RepositoryItemButtonEdit();
            rpiBtnReport = new RepositoryItemButtonEdit();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnReport.ButtonClick += rpiBtnReport_ButtonClick;
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;
            rpiBtnShowComentarios.ButtonClick += rpiBtnShowComentarios_ButtonClick;

            InitializeComponent();
        }

        private void frmTrBandejaOrdenPago_Load(object sender, EventArgs e)
        {
            try
            {
                bsDatos.DataSource = new List<BandejaLiquidacion>();
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
                lMostrarLiquidacion();
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
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<BandejaLiquidacion>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.Liquidacion, poLista[piIndex].Id);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Aprobadores con sus comentarios" };
                    pofrmBuscar.ShowDialog();
                    
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
                var poLista = (List<BandejaLiquidacion>)bsDatos.DataSource;
                var piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                lImprimir(poLista[piIndex].Id);
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
        private void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                var poLista = ((List<BandejaLiquidacion>)bsDatos.DataSource).Where(x => x.Sel).ToList();

                if (poLista.Count() > 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string IdNoActualizadas = "";

                        foreach (var item in poLista)
                        {
                            string Aprobar = loLogicaNegocio.gsVerAprobaciones(Diccionario.Tablas.Transaccion.Liquidacion, item.Id, clsPrincipal.gsUsuario);
                            if (Aprobar == "")
                            {
                                var psMess = loLogicaNegocio.gsAprobar(item.Id, item.Comentario, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, int.Parse(Tag.ToString().Split(',')[0]));
                            }
                            else
                            {
                                IdNoActualizadas += "Liquidación No. " + item.Id + ": " + Aprobar + "\n";
                            }
                        }
                        lListar();
                        
                        if (IdNoActualizadas!="")
                        {
                            XtraMessageBox.Show(IdNoActualizadas, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                       
                    }
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCorregir_Click(object sender, EventArgs e)
        {
            try
            {

                dgvDatos.PostEditor();
                var poLista = ((List<BandejaLiquidacion>)bsDatos.DataSource).Where(x => x.Sel).ToList();

                if (poLista.Count() > 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string psMsg = "";
                        foreach (var item in poLista)
                        {
                            if (string.IsNullOrEmpty(item.Comentario))
                            {
                                psMsg = psMsg + "Liquidación No. " + item.Id + " debe tener comentario para envíar a corregir. \n";
                            }
                        }
                        
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            string IdNoActualizadas = "";
                            foreach (var item in poLista)
                            {

                                var psMess = loLogicaNegocio.gsActualizarEstadoLiquidacion(item.Id, Diccionario.Corregir, item.Comentario, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                if (psMess != "")
                                {
                                    IdNoActualizadas += "Liquidación No. " + item.Id + ": " + psMess + "\n";
                                }
                            }
                            lListar();

                            if (IdNoActualizadas != "")
                            {
                                XtraMessageBox.Show(IdNoActualizadas, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        

                       

                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRechazar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                var poLista = ((List<BandejaLiquidacion>)bsDatos.DataSource).Where(x => x.Sel).ToList();

                if (poLista.Count() > 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string psMsg = "";
                        foreach (var item in poLista)
                        {
                            if (string.IsNullOrEmpty(item.Comentario))
                            {
                                psMsg = psMsg + "Liquidación No. " + item.Id + " debe tener comentario para envíar a Rechazar. \n";
                            }
                        }

                        if (string.IsNullOrEmpty(psMsg))
                        {
                            string IdNoActualizadas = "";
                            foreach (var item in poLista)
                            {

                                var psMess = loLogicaNegocio.gsActualizarEstadoLiquidacion(item.Id, Diccionario.Rechazado, item.Comentario, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                if (psMess != "")
                                {
                                    IdNoActualizadas += "Liquidación No. " + item.Id + ": " + psMess + "\n";
                                }
                            }
                            lListar();

                            if (IdNoActualizadas != "")
                            {
                                XtraMessageBox.Show(IdNoActualizadas, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }



                    }
                }
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
            var poObject = loLogicaNegocio.goListarBandejaLiquidacion(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));

            if (poObject != null)
            {
                bsDatos.DataSource = poObject;
                gcDatos.DataSource = bsDatos.DataSource;
            }
        }

        private void lImprimir(int tId)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {

                if (tId >0)
                {
                    DataSet ds = new DataSet();

                    var dsFull = loLogicaNegocio.gdtRptLiquidacion(tId);
                    DataTable dtCab = dsFull.Tables[0];
                    dtCab.TableName = "Cab";
                    ds.Merge(dtCab);

                    DataTable dtDetRol = dsFull.Tables[2];
                    dtDetRol.TableName = "DetRol";
                    ds.Merge(dtDetRol);

                    DataTable dtDetBSDT = dsFull.Tables[3];
                    dtDetBSDT.TableName = "DetBSDT";
                    ds.Merge(dtDetBSDT);

                    DataTable dtDetBSDC = dsFull.Tables[4];
                    dtDetBSDC.TableName = "DetBSDC";
                    ds.Merge(dtDetBSDC);


                    DataTable dtDetRes = dsFull.Tables[1];
                    dtDetRes.TableName = "DetRes";
                    ds.Merge(dtDetRes);

                    DataTable dtDetVac = dsFull.Tables[5];
                    dtDetVac.TableName = "DetVac";
                    ds.Merge(dtDetVac);

                    if (dtCab.Rows.Count > 0)
                    {
                        xrptLiquidacionResumen xrpt = new xrptLiquidacionResumen();

                        xrpt.DataSource = ds;
                        xrpt.RequestParameters = false;

                        using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                        {
                            printTool.ShowRibbonPreviewDialog();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos para consultar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("Seleccione el Cliente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }


        public void lMostrarLiquidacion()
        {
            int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
            var poLista = (List<BandejaLiquidacion>)bsDatos.DataSource;
            if (piIndex >= 0)
            {

                string psForma = Diccionario.Tablas.Menu.frmTrLiquidacion;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                if (poForm != null)
                {
                    frmTrLiquidacion poFrmMostrarFormulario = new frmTrLiquidacion();
                    poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    poFrmMostrarFormulario.Text = poForm.Nombre;
                    poFrmMostrarFormulario.ShowInTaskbar = true;
                    poFrmMostrarFormulario.MdiParent = this.ParentForm;
                    poFrmMostrarFormulario.lid = poLista[piIndex].Id;
                    poFrmMostrarFormulario.Show();
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
            dgvDatos.OptionsView.ShowFooter = true;

            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                dgvDatos.Columns[i].OptionsColumn.AllowEdit = false;
                //dgvBandejaSolicitud.Columns[i].OptionsColumn.FixedWidth = true;
            }
            dgvDatos.Columns["Ver"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["Imprimir"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["Sel"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["Comentario"].OptionsColumn.AllowEdit = true;
            dgvDatos.Columns["VerComentarios"].OptionsColumn.AllowEdit = true; 

            dgvDatos.Columns["Id"].Caption = "No.";
            dgvDatos.Columns["Total"].Caption = "Total";
            dgvDatos.Columns["Observacion"].Caption = "Observación";
            
            clsComun.gDibujarBotonGrid(rpiBtnReport, dgvDatos.Columns["Imprimir"], "Imprimir", Diccionario.ButtonGridImage.printer_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDatos.Columns["Ver"], "Liquidación", Diccionario.ButtonGridImage.show_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvDatos.Columns["VerComentarios"], "Ver", Diccionario.ButtonGridImage.showhidecomment_16x16);

            dgvDatos.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Empleado"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Comentario"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;

            
            dgvDatos.Columns["Sel"].Width = 30;
            dgvDatos.Columns["Id"].Width = 40;
            dgvDatos.Columns["VerComentarios"].Width = 40;
            dgvDatos.Columns["Ver"].Width = 40;
            dgvDatos.Columns["Estado"].Width = 80;
            dgvDatos.Columns["Imprimir"].Width = 45;
            dgvDatos.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Total"].DisplayFormat.FormatString = "c2";

            clsComun.gFormatearColumnasGrid(dgvDatos);
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Click += btnAprobar_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Click += btnCorregir_Click;
            if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Click += btnRechazar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                GridControl gc = new GridControl();
                BindingSource bs = new BindingSource();
                GridView dgv = new GridView();
                List<BandejaLiquidacionExcel> listbd = new List<BandejaLiquidacionExcel>();

                var poLista = (List<BandejaLiquidacion>)bsDatos.DataSource;
                if (poLista.Count > 0)
                {
                    foreach (var po in poLista)
                    {
                        BandejaLiquidacionExcel be = new BandejaLiquidacionExcel();
                        be.No = po.Id;
                        be.Empleado = po.Empleado;
                        be.Estado = po.Estado;
                        be.FechaCreacion = po.FechaCreacion;
                        be.Observacion = po.Observacion;
                        be.Total = po.Total;
                        be.UsuarioCreacion = po.UsuarioCreacion;
                        be.Estado = po.Estado;


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
