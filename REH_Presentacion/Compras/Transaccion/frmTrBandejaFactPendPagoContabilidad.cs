using COM_Negocio;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using GEN_Entidad.Entidades.Ventas;
using REH_Presentacion.Compras.Transaccion.Modal;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VTA_Negocio;

namespace REH_Presentacion.Compras.Transaccion
{
    /// <summary>
    /// Formulario que permite mostrar las facturas de las ordenes de pagos
    /// Autor: Víctor Arévalo
    /// Fecha: 16/05/2022
    /// </summary>
    public partial class frmTrBandejaFactPendPagoContabilidad : frmBaseTrxDev
    {

        #region Variables
        clsNOrdenPago loLogicaNegocio;
        BindingSource bsDatos;
        RepositoryItemButtonEdit rpiBtnShowArchivo = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDelete = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnFacturaSap = new RepositoryItemButtonEdit();
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();
        #endregion

        #region Eventos


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTrRebate_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmTrBandejaFactPendPagoContabilidad()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNOrdenPago();
            rpiBtnShowArchivo.ButtonClick += rpiBtnShowArchivo_ButtonClick;
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnShowComentarios.ButtonClick += rpiBtnShowComentarios_ButtonClick;
            rpiBtnDelete.ButtonClick += rpiBtnDelete_ButtonClick;
            rpiBtnFacturaSap.ButtonClick += rpiBtnFacturaSap_ButtonClick;
            rpiMedDescripcion.WordWrap = true;

        }

        /// <summary>
        /// Evento del botón Nuevo, Generalmente Limpia el formulario 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                lLimpiar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Listar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            try
            {
                lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del boton de Agregar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShowArchivo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<SpFacturasPendPagoContabilidad>)bsDatos.DataSource;
                if (!string.IsNullOrEmpty(poLista[piIndex].ArchivoAdjunto))
                {
                    string psRuta = ConfigurationManager.AppSettings["CarpetaOPCompras"].ToString() + poLista[piIndex].ArchivoAdjunto;
                    if (psRuta.ToLower().Contains(".pdf"))
                    {
                        frmVerPdf pofrmVerPdf = new frmVerPdf();
                        pofrmVerPdf.lsRuta = psRuta;
                        pofrmVerPdf.Show();
                        pofrmVerPdf.SetDesktopLocation(0, 0);
                        pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;
                    }
                    else
                    {
                        Process.Start(psRuta);
                    }



                }

                else
                {
                    XtraMessageBox.Show("No hay archivo para mostrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                var poLista = (List<SpFacturasPendPagoContabilidad>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.OrdenPagoFactura, poLista[piIndex].IdOrdenPagoFactura);

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
        /// Evento del boton de Visulizar en el Grid, comentarios de las aprobaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnFacturaSap_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<SpFacturasPendPagoContabilidad>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    int pId = poLista[piIndex].DocEntry;
                    var dt = loLogicaNegocio.goConsultaDataTable(string.Format("SELECT CardName Proveedor, FolioNum Factura, DocNum, DocDate 'Fecha Emision', DocDueDate 'Fecha Vencimiento', DocTotal Total FROM SBO_AFECOR.DBO.OPCH T4 (NOLOCK) WHERE DocEntry = {0}",pId));

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Factura Relacionada" };
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
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<SpFacturasPendPagoContabilidad>)bsDatos.DataSource;
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
                        poFrmMostrarFormulario.lid = poLista[piIndex].IdOrdenPago;
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

        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<SpFacturasPendPagoContabilidad>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    if (poLista[piIndex].DocEntry > 0)
                    {
                        DialogResult dialogResult = XtraMessageBox.Show(string.Format("Esta seguro de desligar factura: {0} del proveedor {1}", poLista[piIndex].Factura, poLista[piIndex].Proveedor), Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {

                            loLogicaNegocio.gDesligarSaafSap(poLista[piIndex].IdOrdenPagoFactura, poLista[piIndex].DocEntry, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lBuscar();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show(string.Format("Factura: {0} del proveedor: {1} no está relacionada en SAP, no es posible continuar.", poLista[piIndex].Factura, poLista[piIndex].Proveedor), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Generar, Genera Novedad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                dgvDatos.PostEditor();
                //string psMsgValida = lsEsValido();

                List<SpFacturasPendPagoContabilidad> poLista = (List<SpFacturasPendPagoContabilidad>)bsDatos.DataSource;

                if (poLista.Count > 0 )
                {
                    //string psMsg = loLogicaNegocio.gsGuardarFacturaPago(poLista, lIdEliminar, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    //if (string.IsNullOrEmpty(psMsg))
                    //{
                    //    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    lBuscar();
                    //}
                    //else
                    //{
                    //    XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a guardar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }
        
        /// <summary>
        /// Evento del botón Exportar, Exporta a Excel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {

                
                dgvDatos.PostEditor();

                List<SpFacturasPendPagoContabilidad> poLista = (List<SpFacturasPendPagoContabilidad>)bsDatos.DataSource;
                if (poLista != null && poLista.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcDatos, Text + ".xlsx", psFilter);
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

                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<SpFacturasPendPagoContabilidad>)bsDatos.DataSource;
                int pId = poLista[piIndex].IdOrdenPago;
                string psTexto = string.Format("{0}- Factura:{1} ", poLista[piIndex].ComentarioAprobador, poLista[piIndex].Factura); 
                var result = XtraInputBox.Show("Ingrese comentario", "Corregir", psTexto);
                if (string.IsNullOrEmpty(result))
                {
                    XtraMessageBox.Show("Debe agregar comentario para poder enviarla a corregir", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string psMsg = "";

                if (pId != 0)
                {
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        DialogResult dialogResult = XtraMessageBox.Show(string.Format("Esta seguro de enviar a corregir la factura: {0} del proveedor {1}", poLista[piIndex].Factura, poLista[piIndex].Proveedor), Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {

                            loLogicaNegocio.gActualizarEstadoOrdenPago(pId, Diccionario.Corregir, result, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, poLista[piIndex].IdOrdenPagoFactura);
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lBuscar();
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void frmTrBandejaFactPendPago_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
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
            catch (Exception ex)
            {

                //Clipboard.SetText(" ");
            }

        }
        #endregion

        #region Métodos
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Click += btnCorregir_Click;

            /*********************************************************************************************************************************************/

            bsDatos = new BindingSource();
            bsDatos.DataSource = new List<SpFacturasPendPagoContabilidad>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsBehavior.Editable = true;
            dgvDatos.OptionsView.RowAutoHeight = true;
            //this.dgvDatos.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            //this.dgvDatos.OptionsCustomization.AllowColumnMoving = false;
            //this.dgvDatos.OptionsPrint.AutoWidth = false;
            //this.dgvDatos.OptionsView.ShowAutoFilterRow = true;
            //this.dgvDatos.OptionsView.ShowGroupPanel = false;

            //dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            //dgvDatos.OptionsView.ShowFooter = true;
            //dgvDatos.OptionsView.RowAutoHeight = true;
            //dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;

            dgvDatos.Columns["IdOrdenPago"].Visible = false;
            //dgvDatos.Columns["IdOrdenPagoFactura"].Visible = false;
            dgvDatos.Columns["ArchivoAdjunto"].Visible = false;
            dgvDatos.Columns["NombreOriginal"].Visible = false;
            dgvDatos.Columns["Aprobo"].Visible = false;
            //dgvDatos.Columns["IdProveedor"].Visible = false;
            dgvDatos.Columns["DocEntry"].Visible = false;

            dgvDatos.Columns["Proveedor"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["FacturaRelSap"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["DocNum"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["IdOrdenPagoFactura"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Estado"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Descripcion"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["CardCode"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Factura"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["FechaIngreso"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["FechaFactura"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["FechaVencimiento"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Valor"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["ArchivoAdjunto"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Aprobo"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Mapeada"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Usuario"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Departamento"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["IngresadaSap"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["Proveedor"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["ComentarioAprobador"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;


            dgvDatos.Columns["Factura"].Caption = "# Factura";
            dgvDatos.Columns["Valor"].Caption = "Total";
            dgvDatos.Columns["Mapeada"].Caption = "Relacionada en SAP";
            dgvDatos.Columns["IdOrdenPagoFactura"].Caption = "Sec";
            dgvDatos.Columns["FechaIngreso"].Caption = "Fecha Orden Pago";

            
            clsComun.gDibujarBotonGrid(rpiBtnDelete, dgvDatos.Columns["Desligar"], "Desligar", Diccionario.ButtonGridImage.deletelist2_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDatos.Columns["Ver"], "Orden Pago", Diccionario.ButtonGridImage.show_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowArchivo, dgvDatos.Columns["VerFactura"], "Ver Factura", Diccionario.ButtonGridImage.show_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvDatos.Columns["VerComentarios"], "Ver Comentarios", Diccionario.ButtonGridImage.showhidecomment_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnFacturaSap, dgvDatos.Columns["Relacion"], "Factura SAP", Diccionario.ButtonGridImage.inserttable_16x16);

            dgvDatos.FixedLineWidth = 7;
            dgvDatos.Columns["Mapeada"].Fixed = FixedStyle.Left;
            dgvDatos.Columns["IngresadaSap"].Fixed = FixedStyle.Left;
            dgvDatos.Columns["Estado"].Fixed = FixedStyle.Left;
            dgvDatos.Columns["FechaIngreso"].Fixed = FixedStyle.Left;
            dgvDatos.Columns["CardCode"].Fixed = FixedStyle.Left;
            dgvDatos.Columns["Proveedor"].Fixed = FixedStyle.Left;
            dgvDatos.Columns["Factura"].Fixed = FixedStyle.Left;

            //dgvDatos.Columns["ComentarioAprobador"].Width = 05;
            //dgvDatos.Columns["Proveedor"].Width = 25;

            clsComun.gFormatearColumnasGrid(dgvDatos);

            dtpFechaInicio.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaFin.EditValue = DateTime.Now;

        }

        private void lLimpiar()
        {
            bsDatos.DataSource = new List<FacturaDetalleGrid>();
            dtpFechaInicio.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaFin.EditValue = DateTime.Now;
        }

        private void lBuscar()
        {

            Cursor.Current = Cursors.WaitCursor;

            
            bsDatos.DataSource = loLogicaNegocio.goConsultaFacturasPendientePagoContabilidad(dtpFechaInicio.DateTime, dtpFechaFin.DateTime);
            gcDatos.DataSource = bsDatos;

            clsComun.gOrdenarColumnasGridFullEditable(dgvDatos);

            dgvDatos.Columns["Mapeada"].Width = 80;
            dgvDatos.Columns["IngresadaSap"].Width = 80;

            Cursor.Current = Cursors.Default;

        }

        #endregion

    }
}
