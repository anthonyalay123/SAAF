using COM_Negocio;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using REH_Presentacion.Comun;
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
    public partial class frmTrBandejaOrdenPago : frmBaseTrxDev
    {
        clsNOrdenPago loLogicaNegocio = new clsNOrdenPago();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnReport;
        RepositoryItemMemoEdit rpiMedDescripcion;
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();

        #region MyRegion

        public frmTrBandejaOrdenPago()
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
                bsDatos.DataSource = new List<BandejaOrdenPago>();
                gcBandejaOrdenPago.DataSource = bsDatos;
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
        /// Evento del boton de Visulizar en el Grid, comentarios de las aprobaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShowComentarios_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvBandejaOrdenPago.GetFocusedDataSourceRowIndex();
                var poLista = (List<BandejaOrdenPago>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.OrdenPago, poLista[piIndex].Id);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Comentarios" };
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
                var poLista = (List<BandejaOrdenPago>)bsDatos.DataSource;
                var piIndex = dgvBandejaOrdenPago.GetFocusedDataSourceRowIndex();
                clsComun.gImprimirOrdenPago(poLista[piIndex].Id);
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
                dgvBandejaOrdenPago.PostEditor();
                var poLista = (List<BandejaOrdenPago>)bsDatos.DataSource;
                bool poCambios = false;
                for (int i = 0; i < poLista.Count; i++)
                {
                    if (poLista[i].Sel != false)
                    {
                        poCambios = true;
                    }
                }
                if (poCambios == true)
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string IdNoActualizadas = "";
                        for (int i = 0; i < poLista.Count; i++)
                        {
                            if (poLista[i].Sel==true)
                            {
                                poLista[i].ReferenciasIDOrdenCompraProveedor = loLogicaNegocio.gsBuscarFacturaObservacion(poLista[i].Id);

                                List<int> id = new List<int>();
                                if (!string.IsNullOrEmpty(poLista[i].ReferenciasIDOrdenCompraProveedor))
                                {
                                    var separar = poLista[i].ReferenciasIDOrdenCompraProveedor.Split(new char[] { ',' });

                                    foreach (var num in separar)
                                    {
                                        if (num != " ")
                                        {
                                            id.Add(int.Parse(num));

                                        }
                                    }
                                }


                                string Aprobar = loLogicaNegocio.gsVerAprobaciones(Diccionario.Tablas.Transaccion.OrdenPago ,poLista[i].Id, clsPrincipal.gsUsuario);
                                if (Aprobar == "")
                                {
                                    var menu = Tag.ToString().Split(',');
                                    var psMess = loLogicaNegocio.gsAprobar(poLista[i].Id, poLista[i].Comentario, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, Int32.Parse(menu[0]));

                                    if (psMess!= "")
                                    {
                                        IdNoActualizadas += "Orden de Pago No. " + poLista[i].Id + ": " + psMess + "\n";
                                    }
                                    else
                                    {
                                        loLogicaNegocio.gActualizarEstadoOrdenCompraProveedor(clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, id);
                                    }

                                }
                                else
                                {
                                    IdNoActualizadas += "Orden de Pago No. "+ poLista[i].Id+ ": "+ Aprobar+ "\n";
                                }
                            }
                            lListar();
                        }

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

                dgvBandejaOrdenPago.PostEditor();
                var poLista = ((List<BandejaOrdenPago>)bsDatos.DataSource).Where(x=>x.Sel).ToList();

                //var result = XtraInputBox.Show("Ingrese Observación", "Rechazar", "");
                //if (string.IsNullOrEmpty(result))
                //{
                //    XtraMessageBox.Show("Debe agregar Obsevación para poder rechazar", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                string psMsg = "";
                foreach (var item in poLista.Where(x=>string.IsNullOrEmpty(x.Comentario)))
                {
                    psMsg = string.Format("{0}Orden de pago No. {1} debe tener un comentario para poder enviarla a corregir. \n",psMsg, item.Id);
                }

                if (poLista.Count > 0)
                {
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {

                            foreach (var item in poLista)
                            {
                                loLogicaNegocio.gActualizarEstadoOrdenPago(item.Id, Diccionario.Corregir, item.Comentario, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            }
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay registros seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                dgvBandejaOrdenPago.PostEditor();
                var poLista = ((List<BandejaOrdenPago>)bsDatos.DataSource).Where(x => x.Sel).ToList();

                string psMsg = "";
                foreach (var item in poLista.Where(x => string.IsNullOrEmpty(x.Comentario)))
                {
                    psMsg = string.Format("{0}Orden de pago No. {1} debe tener un comentario para poder enviarla a rechazar. \n", psMsg, item.Id);
                }

                if (poLista.Count > 0)
                {
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {

                            foreach (var item in poLista)
                            {
                                loLogicaNegocio.gActualizarEstadoOrdenPago(item.Id, Diccionario.Rechazado, item.Comentario, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            }
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lListar();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay registros seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var poObject = loLogicaNegocio.goListarBandeja(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
            //if (poObject != null)
            //{
            //    bsDatos.DataSource = new List<BandejaOrdenPago>();
            //    bsDatos.DataSource = poObject;
            //    gcBandejaOrdenPago.DataSource = bsDatos.DataSource;
            //}

            if (poObject != null)
            {
                foreach (var x in poObject)
                {
                    x.Aprobaciones = loLogicaNegocio.goBuscarCantidadAprobacion(x.Id);
                    x.UsuariosAprobacion = loLogicaNegocio.goBuscarUsuarioAprobacion(x.Id);
                }
                bsDatos.DataSource = new List<BandejaOrdenPago>();
                bsDatos.DataSource = poObject;
                gcBandejaOrdenPago.DataSource = bsDatos.DataSource;
            }
        }
        
        
        public void lMostrarOrdenPago()
        {
            int piIndex = dgvBandejaOrdenPago.GetFocusedDataSourceRowIndex();
            var poLista = (List<BandejaOrdenPago>)bsDatos.DataSource;
            if (piIndex >= 0)
            {

                string psForma = Diccionario.Tablas.Menu.OrdenPago;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                if (poForm != null)
                {
                    frmTrOrdenPago poFrmMostrarFormulario = new frmTrOrdenPago();
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
            dgvBandejaOrdenPago.OptionsView.RowAutoHeight = true;
            dgvBandejaOrdenPago.OptionsView.ShowFooter = true;

            for (int i = 0; i < dgvBandejaOrdenPago.Columns.Count; i++)
            {
                dgvBandejaOrdenPago.Columns[i].OptionsColumn.AllowEdit = false;
                // dgvBandejaSolicitud.Columns[i].OptionsColumn.FixedWidth = true;
            }
            dgvBandejaOrdenPago.Columns["Ver"].OptionsColumn.AllowEdit = true;
            dgvBandejaOrdenPago.Columns["Imprimir"].OptionsColumn.AllowEdit = true;
            dgvBandejaOrdenPago.Columns["Sel"].OptionsColumn.AllowEdit = true;
            dgvBandejaOrdenPago.Columns["Comentario"].OptionsColumn.AllowEdit = true;
            dgvBandejaOrdenPago.Columns["VerComentarios"].OptionsColumn.AllowEdit = true; 

            dgvBandejaOrdenPago.Columns["Id"].Caption = "No.";
            dgvBandejaOrdenPago.Columns["Valor"].Caption = "Total Orden de Pago";
            dgvBandejaOrdenPago.Columns["Observacion"].Caption = "Descripción";
            dgvBandejaOrdenPago.Columns["Ver"].Caption = "Orden Pago";
            
            clsComun.gDibujarBotonGrid(rpiBtnReport, dgvBandejaOrdenPago.Columns["Imprimir"], "Imprimir", Diccionario.ButtonGridImage.printer_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvBandejaOrdenPago.Columns["Ver"], "Orden Pago", Diccionario.ButtonGridImage.show_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvBandejaOrdenPago.Columns["VerComentarios"], "Ver", Diccionario.ButtonGridImage.showhidecomment_16x16);

            dgvBandejaOrdenPago.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenPago.Columns["Proveedor"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenPago.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenPago.Columns["Comentario"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenPago.Columns["Usuario"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenPago.Columns["UsuariosAprobacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenPago.Columns["Factura"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaOrdenPago.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;

            dgvBandejaOrdenPago.Columns["ReferenciasIDOrdenCompraProveedor"].Visible = false;
            dgvBandejaOrdenPago.Columns["Diferencia"].Visible = false;
            dgvBandejaOrdenPago.Columns["UsuariosAprobacion"].Visible = false;
            dgvBandejaOrdenPago.Columns["Aprobaciones"].Visible = false;

            dgvBandejaOrdenPago.Columns["Sel"].Width = 30;
            dgvBandejaOrdenPago.Columns["Id"].Width = 40;
            dgvBandejaOrdenPago.Columns["VerComentarios"].Width = 40;
            dgvBandejaOrdenPago.Columns["Observacion"].Width = 200;
            dgvBandejaOrdenPago.Columns["Proveedor"].Width = 150;
            dgvBandejaOrdenPago.Columns["Ver"].Width = 40;
            dgvBandejaOrdenPago.Columns["Estado"].Width = 80;
            dgvBandejaOrdenPago.Columns["Imprimir"].Width = 45;
            dgvBandejaOrdenPago.Columns["Valor"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvBandejaOrdenPago.Columns["Valor"].DisplayFormat.FormatString = "c2";
            dgvBandejaOrdenPago.Columns["TotalOrdenCompra"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvBandejaOrdenPago.Columns["TotalOrdenCompra"].DisplayFormat.FormatString = "c2";
            dgvBandejaOrdenPago.Columns["Diferencia"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvBandejaOrdenPago.Columns["Diferencia"].DisplayFormat.FormatString = "c2";

            clsComun.gFormatearColumnasGrid(dgvBandejaOrdenPago);
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
                List<BandejaOrdenPagoExcel> listbd = new List<BandejaOrdenPagoExcel>();

                var poLista = (List<BandejaOrdenPago>)bsDatos.DataSource;
                if (poLista.Count > 0)
                {
                    foreach (var po in poLista)
                    {
                        BandejaOrdenPagoExcel be = new BandejaOrdenPagoExcel();
                        be.No = po.Id;
                        be.FechaOrdenPago = po.FechaOrdenPago;
                        be.Proveedor = po.Proveedor;
                        be.Usuario = po.Usuario;
                        be.Descripcion = po.Observacion;
                        be.Total = po.Valor;
                        be.Aprobo = po.UsuariosAprobacion;
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
