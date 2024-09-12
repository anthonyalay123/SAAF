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
using REH_Presentacion.Compras.Maestros;
using REH_Presentacion.Compras.Transaccion.Modal;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VTA_Negocio;

namespace REH_Presentacion.Compras.Transaccion
{
    /// <summary>
    /// Formulario que permite guardar los clientes Rebate
    /// Autor: Víctor Arévalo
    /// Fecha: 21/12/2021
    /// </summary>
    public partial class frmTrBandejaFactPendPago : frmBaseTrxDev
    {

        #region Variables
        clsNOrdenPago loLogicaNegocio;
        RepositoryItemButtonEdit rpiBtnDel;
        BindingSource bsDatos;
        BindingSource bsRebate;
        List<int> lIdEliminar = new List<int>();
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShow0 = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnUpdate = new RepositoryItemButtonEdit();
        private decimal customSum = 0;
        #endregion

        #region Eventos

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmTrBandejaFactPendPago()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNOrdenPago();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnShow0.ButtonClick += rpiBtnShow0_ButtonClick;
            rpiBtnShowComentarios.ButtonClick += rpiBtnShowComentarios_ButtonClick;
            rpiBtnUpdate.ButtonClick += rpiBtnUpdate_ButtonClick;
        }

        private void frmTrRebate_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                clsComun.gLLenarComboGrid(ref dgvRebate, loLogicaNegocio.goConsultarComboEstado(), "CodigoEstado");
                clsComun.gLLenarCombo(ref cmbGrupoPago, loLogicaNegocio.goConsultarComboGrupoPagos(), true);

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del boton de eliminar en el Grid, elimina la fila seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvRebate.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<FacturaDetalleGrid>)bsDatos.DataSource;
                var poListaRebate = (List<FacturaDetalleGrid>)bsRebate.DataSource;

                if (poListaRebate.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poListaRebate[piIndex];

                    if (poEntidad.CodigoEstado == Diccionario.Pendiente || poEntidad.CodigoEstado == Diccionario.Corregir
                        || poEntidad.CodigoEstado == Diccionario.Rechazado)
                    {
                        if (poEntidad.DocNum != 0)
                        {
                            poLista.Add(poEntidad);
                        }

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poListaRebate.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsRebate.DataSource = poListaRebate;
                        dgvRebate.RefreshData();

                        bsDatos.DataSource = poLista.ToList();
                        dgvDatos.RefreshData();

                        lIdEliminar.Add(poEntidad.IdFacturaPago);
                    }
                    else
                    {
                        XtraMessageBox.Show("No es posible eliminar registro, debe tener un estado pendiente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        /// Evento del botón Generar, Genera Novedad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                dgvRebate.PostEditor();
                //string psMsgValida = lsEsValido();

                List<FacturaDetalleGrid> poLista = (List<FacturaDetalleGrid>)bsRebate.DataSource;

                if (poLista.Count > 0 || lIdEliminar.Count > 0)
                {
                    //string psSemana = loLogicaNegocio.gsConsultarSemana();
                    //var result = XtraInputBox.Show("Ingrese o Actualice la Semana", "Semana en curso", psSemana);
                    //if (string.IsNullOrEmpty(result))
                    //{
                    //    XtraMessageBox.Show("Debe ingresar la semana para poder continuar", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    return;
                    //}
                    //else
                    //{
                    //    loLogicaNegocio.gActualizarSemana(result);
                    //}

                    //string psMsg = loLogicaNegocio.gsGuardarFacturaPago(poLista, lIdEliminar, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, result);
                    string psMsg = loLogicaNegocio.gsGuardarFacturaPago(poLista, lIdEliminar, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, "");
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lBuscar();
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a guardar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (ex.Message.Contains("EntityValidation"))
                {
                    string psMensajeEntityValidation = string.Empty;
                    var ErrorEntityValidation = ((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors;
                    if (ErrorEntityValidation != null)
                    {
                        foreach (var poItem in ErrorEntityValidation)
                        {
                            string psEntidad = "Entidad: " + poItem.Entry.Entity.ToString() + ", ";
                            foreach (var poSubItem in poItem.ValidationErrors)
                            {
                                psMensajeEntityValidation = psMensajeEntityValidation + psEntidad + poSubItem.ErrorMessage + "\n";
                            }
                        }
                        XtraMessageBox.Show(psMensajeEntityValidation, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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
                dgvRebate.PostEditor();

                List<FacturaDetalleGrid> poLista = (List<FacturaDetalleGrid>)bsDatos.DataSource;
                if (poLista != null && poLista.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcDatos, Text + " Preliminar" + ".xlsx", psFilter);
                }

                List<FacturaDetalleGrid> poListaProvision = (List<FacturaDetalleGrid>)bsRebate.DataSource;
                if (poListaProvision != null && poListaProvision.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcRebate, Text + " Seleccionados" + ".xlsx", psFilter);
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
        private void rpiBtnShowComentarios_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvRebate.GetFocusedDataSourceRowIndex();
                var poLista = (List<FacturaDetalleGrid>)bsRebate.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.FacturaPago, poLista[piIndex].IdFacturaPago);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Comentarios" };
                    pofrmBuscar.ShowDialog();

                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void Grid_CheckedChanged(object sender, EventArgs e)
        {
            dgvDatos.PostEditor();
            dgvDatos.UpdateSummary();
        }

        void Grid_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == CustomSummaryProcess.Start)
            {
                customSum = 0;
            }
            if (e.SummaryProcess == CustomSummaryProcess.Calculate)
            {
                if ((bool)dgvDatos.GetRowCellValue(e.RowHandle, "Sel"))
                {
                    customSum += decimal.Parse(e.FieldValue.ToString());
                }
            }
            if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                e.TotalValue = customSum;
            }
        }

        #endregion

      
        #region Métodos
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;

            /*********************************************************************************************************************************************/

            bsDatos = new BindingSource();
            bsDatos.DataSource = new List<FacturaDetalleGrid>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["IdOrdenPago"].Visible = false;
            dgvDatos.Columns["IdOrdenPagoFactura"].Visible = false;
            dgvDatos.Columns["Identificacion"].Visible = false;
            dgvDatos.Columns["Del"].Visible = false;
            dgvDatos.Columns["Generado"].Visible = false;
            //dgvDatos.Columns["ValorPago"].Visible = false;
            dgvDatos.Columns["CodigoEstado"].Visible = false;
            dgvDatos.Columns["IdFacturaPago"].Visible = false;
            dgvDatos.Columns["Comentario"].Visible = false;
            dgvDatos.Columns["ComentarioTesoreria"].Visible = false;
            //dgvDatos.Columns["Ver"].Visible = false;
            dgvDatos.Columns["IdProveedor"].Visible = false;
            dgvDatos.Columns["VerComentarios"].Visible = false;
            dgvDatos.Columns["IdGrupoPago"].Visible = false;
            dgvDatos.Columns["GrupoPago"].Visible = false;
            dgvDatos.Columns["ActualizarGrupoPago"].Visible = false;
            

            dgvDatos.OptionsBehavior.Editable = true;
            //dgvRebate.Columns["GrupoPago"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["DocNum"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Proveedor"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["FechaEmision"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["FechaVencimiento"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NumDocumento"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Valor"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Abono"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Saldo"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Observacion"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Comentario"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["ComentarioAprobadorOP"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["UsuarioAprobadorOP"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["ValorPago"].Caption = "Valor a Pagar";
            dgvDatos.Columns["NumDocumento"].Caption = "# Factura";
            dgvDatos.Columns["Observacion"].Caption = "Comentarios";
            dgvDatos.Columns["ComentarioAprobadorOP"].Caption = "Observación del aprobador Orden de Pago";
            dgvDatos.Columns["UsuarioAprobadorOP"].Caption = "Usuario que Aprobó Orden de Pago";

            dgvDatos.FixedLineWidth = 3;
            dgvDatos.Columns["Sel"].Fixed = FixedStyle.Left;
            dgvDatos.Columns["Proveedor"].Fixed = FixedStyle.Left;
            dgvDatos.Columns["NumDocumento"].Fixed = FixedStyle.Left;

            clsComun.gDibujarBotonGrid(rpiBtnShow0, dgvDatos.Columns["Ver"], "Orden Pago", Diccionario.ButtonGridImage.show_16x16);

            //clsComun.gFormatearColumnasGrid(dgvDatos);

            dgvDatos.Columns["Valor"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Valor"].DisplayFormat.FormatString = "c2";
            dgvDatos.Columns["Abono"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Abono"].DisplayFormat.FormatString = "c2";
            dgvDatos.Columns["Saldo"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Saldo"].DisplayFormat.FormatString = "c2";

            dgvDatos.Columns["ValorPago"].SummaryItem.SummaryType = SummaryItemType.Custom;

            dgvDatos.CustomSummaryCalculate += Grid_CustomSummaryCalculate;
            (dgvDatos.Columns["Sel"].RealColumnEdit as RepositoryItemCheckEdit).CheckedChanged += Grid_CheckedChanged;
            dgvDatos.GroupSummary.Clear();

            GridGroupSummaryItem sumColumn = new GridGroupSummaryItem();
            sumColumn = dgvDatos.GroupSummary.Add(SummaryItemType.Sum, "ValorPago", dgvDatos.Columns["ValorPago"], "{0:c2}");
            sumColumn.SummaryType = SummaryItemType.Sum;
            sumColumn.DisplayFormat = "{0:c2}";

            //GridGroupSummaryItem sumColumn1 = new GridGroupSummaryItem();
            //sumColumn1 = dgvDatos.GroupSummary.Add(SummaryItemType.Sum, "ValorPago", dgvDatos.Columns["ValorPago"], "{0:c2}");
            //sumColumn1.SummaryType = SummaryItemType.Custom;
            //sumColumn1.DisplayFormat = "{0:c2}";

            //sumColumn.ShowInGroupColumnFooter = dgvRebate.Columns["ValorPago"];
            //sumColumn.FieldName = "ValorPago";
            //sumColumn.SummaryType = SummaryItemType.Custom;
            //sumColumn.Tag = 2;
            //sumColumn.DisplayFormat = "{0:c2}";


            dgvDatos.UpdateSummary();


            /*********************************************************************************************************************************************/

            bsRebate = new BindingSource();
            bsRebate.DataSource = new List<FacturaDetalleGrid>();
            gcRebate.DataSource = bsRebate;

            dgvRebate.OptionsBehavior.Editable = true;

            dgvRebate.Columns["IdOrdenPago"].Visible = false;
            dgvRebate.Columns["IdOrdenPagoFactura"].Visible = false;
            dgvRebate.Columns["IdFacturaPago"].Visible = false;
            dgvRebate.Columns["Identificacion"].Visible = false;
            dgvRebate.Columns["Sel"].Visible = false;
            dgvRebate.Columns["Generado"].Visible = false;
            dgvRebate.Columns["IdProveedor"].Visible = false;
            dgvRebate.Columns["Comentario"].Visible = false;
            dgvRebate.Columns["ComentarioAprobadorOP"].Visible = false;
            dgvRebate.Columns["UsuarioAprobadorOP"].Visible = false;
            dgvRebate.Columns["IdGrupoPago"].Visible = false;
            //dgvRebate.Columns["Comentario"].Visible = false;

            dgvRebate.Columns["GrupoPago"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["DocNum"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Proveedor"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["FechaEmision"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["FechaVencimiento"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["NumDocumento"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Valor"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Abono"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Saldo"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Observacion"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CodigoEstado"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Comentario"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["ComentarioAprobadorOP"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["UsuarioAprobadorOP"].OptionsColumn.AllowEdit = false;

            dgvRebate.Columns["ValorPago"].Caption = "Valor a Pagar";
            dgvRebate.Columns["NumDocumento"].Caption = "# Factura";
            dgvRebate.Columns["CodigoEstado"].Caption = "Estado";
            dgvRebate.Columns["Comentario"].Caption = "Observación Aprobadores";
            dgvRebate.Columns["Observacion"].Caption = "Comentarios";
            dgvRebate.Columns["ComentarioAprobadorOP"].Caption = "Observación del aprobador Orden de Pago";
            dgvRebate.Columns["UsuarioAprobadorOP"].Caption = "Usuario que Aprobó Orden de Pago";

            dgvRebate.FixedLineWidth = 2;
            dgvRebate.Columns["Proveedor"].Fixed = FixedStyle.Left;
            dgvRebate.Columns["NumDocumento"].Fixed = FixedStyle.Left;

            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvRebate.Columns["Ver"], "Orden Pago", Diccionario.ButtonGridImage.show_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvRebate.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvRebate.Columns["VerComentarios"], "Ver Comentarios", Diccionario.ButtonGridImage.showhidecomment_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnUpdate, dgvRebate.Columns["ActualizarGrupoPago"], " ", Diccionario.ButtonGridImage.refresh_16x16);

            dgvRebate.Columns["ActualizarGrupoPago"].Width = 30;

            clsComun.gFormatearColumnasGrid(dgvRebate);

            dtpFechaInicio.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaFin.EditValue = DateTime.Now;


        }

        private void lLimpiar()
        {
            bsDatos.DataSource = new List<FacturaDetalleGrid>();
            bsRebate.DataSource = new List<FacturaDetalleGrid>();
            gcRebate.DataSource = bsRebate;
            dtpFechaInicio.Enabled = true;
            dtpFechaFin.Enabled = true;
            dtpFechaInicio.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaFin.EditValue = DateTime.Now;
            lIdEliminar = new List<int>();
        }

        private void lBuscar()
        {

            Cursor.Current = Cursors.WaitCursor;
            
            bsDatos.DataSource = loLogicaNegocio.goConsultaPreliminarFacturasPendientePago(dtpFechaInicio.DateTime, dtpFechaFin.DateTime);
            gcDatos.DataSource = bsDatos;

            clsComun.gOrdenarColumnasGridFullEditable(dgvDatos);

            bsRebate.DataSource = loLogicaNegocio.goConsultaFacturasPendientePago(dtpFechaInicio.DateTime, dtpFechaFin.DateTime);
            gcRebate.DataSource = bsRebate;

            clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);

            lIdEliminar = new List<int>();

            lCalcularTotal();

            Cursor.Current = Cursors.Default;
        }

        #endregion

        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvRebate.GetFocusedDataSourceRowIndex();
                var poLista = (List<FacturaDetalleGrid>)bsRebate.DataSource;
                if (piIndex >= 0)
                {
                    if (poLista[piIndex].IdOrdenPago != 0)
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
                    else
                    {
                        XtraMessageBox.Show(string.Format("Factura: '{0}', No viene de una orden de pago.", poLista[piIndex].NumDocumento), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void rpiBtnUpdate_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvRebate.GetFocusedDataSourceRowIndex();
                var poLista = (List<FacturaDetalleGrid>)bsRebate.DataSource;
                if (piIndex >= 0)
                {

                    var poListaGrupo = loLogicaNegocio.goConsultarComboGrupoPagos();
                    frmCombo frmZona = new frmCombo();
                    frmZona.lsNombre = "Grupo de Pago";
                    frmZona.loCombo = poListaGrupo;
                    frmZona.ShowDialog();

                    string psCode = frmZona.lsSeleccionado;
                    if (!string.IsNullOrEmpty(psCode))
                    {
                        string psName = poListaGrupo.Where(x => x.Codigo == frmZona.lsSeleccionado).Select(x => x.Descripcion).FirstOrDefault();

                        poLista[piIndex].IdGrupoPago = int.Parse(psCode);
                        poLista[piIndex].GrupoPago = psName;

                        dgvRebate.RefreshData();
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
        private void rpiBtnShow0_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<FacturaDetalleGrid>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    if (poLista[piIndex].IdOrdenPago != 0)
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
                    else
                    {
                        XtraMessageBox.Show(string.Format("Factura: '{0}', No viene de una orden de pago.", poLista[piIndex].NumDocumento), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {

                dgvRebate.PostEditor();
                dgvDatos.PostEditor();

                List<FacturaDetalleGrid> poListaPreliminar = (List<FacturaDetalleGrid>)bsDatos.DataSource;
                List<FacturaDetalleGrid> poListaRebate = (List<FacturaDetalleGrid>)bsRebate.DataSource;


                List<FacturaDetalleGrid> poListaAgregar = new List<FacturaDetalleGrid>();

                if (cmbGrupoPago.EditValue.ToString() == Diccionario.Seleccione)
                {
                    XtraMessageBox.Show("Selecione el Grupo de Pago", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (poListaPreliminar.Where(x => x.Sel).Count() == 0)
                {
                    XtraMessageBox.Show("No ha seleccionado ningun registro", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string psMsg = "";
                foreach (var item in poListaPreliminar.Where(x=>x.Sel).ToList())
                {
                    if (item.IdOrdenPagoFactura != 0)
                    {
                        if (poListaRebate.Where(x => x.DocNum == item.DocNum).Count() == 0)
                        {
                            var poRegistro = new FacturaDetalleGrid();
                            poRegistro.IdOrdenPago = item.IdOrdenPago;
                            poRegistro.IdOrdenPagoFactura = item.IdOrdenPagoFactura;
                            poRegistro.DocNum = item.DocNum;
                            poRegistro.Identificacion = item.Identificacion;
                            poRegistro.Proveedor = item.Proveedor;
                            poRegistro.FechaEmision = item.FechaEmision;
                            poRegistro.FechaVencimiento = item.FechaVencimiento;
                            poRegistro.NumDocumento = item.NumDocumento;
                            poRegistro.Valor = item.Valor;
                            poRegistro.Abono = item.Abono;
                            poRegistro.Saldo = item.Saldo;
                            poRegistro.ValorPago = item.ValorPago;
                            poRegistro.Observacion = item.Observacion;
                            poRegistro.CodigoEstado = item.CodigoEstado;
                            poRegistro.Comentario = item.Comentario;
                            poRegistro.ComentarioTesoreria = item.Observacion;
                            poRegistro.UsuarioAprobadorOP = item.UsuarioAprobadorOP;
                            poRegistro.ComentarioAprobadorOP = item.ComentarioAprobadorOP;
                            poRegistro.IdOrdenPago = item.IdOrdenPago;
                            poRegistro.IdProveedor = item.IdProveedor;
                            poRegistro.IdGrupoPago = int.Parse(cmbGrupoPago.EditValue.ToString());
                            poRegistro.GrupoPago = cmbGrupoPago.Text;



                            poListaRebate.Add(poRegistro);

                            poListaPreliminar.Remove(item);

                        }
                    }
                    else
                    {
                        psMsg = string.Format("{0}Factura: {1} del Proveedor: {2} no está creada o relacionada.\n", psMsg, item.NumDocumento, item.Proveedor);
                    }
                }

                if (!string.IsNullOrEmpty(psMsg))
                {
                    XtraMessageBox.Show(psMsg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                tbcDocumentosPagar.SelectedTabPageIndex = 1;

                bsDatos.DataSource = poListaPreliminar;
                dgvDatos.RefreshData();

                bsRebate.DataSource = poListaRebate;
                dgvRebate.RefreshData();

                clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);


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
                dtpFechaInicio.Enabled = false;
                dtpFechaFin.Enabled = false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddManualmente_Click(object sender, EventArgs e)
        {
            try
            {
                
                frmAddFacturaPago frm = new frmAddFacturaPago();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    List<FacturaDetalleGrid> poListaRebate = (List<FacturaDetalleGrid>)bsRebate.DataSource;

                    var poRegistro = new FacturaDetalleGrid();
                    poRegistro.IdOrdenPagoFactura =0;
                    poRegistro.DocNum = 0;
                    poRegistro.Identificacion = frm.lsIdentificacion;
                    poRegistro.Proveedor = frm.lsProveedor;
                    poRegistro.FechaEmision = frm.ldFechaEmision;
                    poRegistro.FechaVencimiento = frm.ldFechaVencimiento;
                    poRegistro.NumDocumento = frm.lsNumDocumento;
                    poRegistro.Valor = frm.ldcValor;
                    poRegistro.Abono = 0M;
                    poRegistro.Saldo = frm.ldcValor;
                    poRegistro.ValorPago = frm.ldcValor;
                    poRegistro.Observacion = frm.lsObservacion;
                    poRegistro.ComentarioTesoreria = frm.lsObservacion;
                    poRegistro.CodigoEstado = Diccionario.Pendiente;
                    poRegistro.IdProveedor = frm.lIdProveedor;
                    poRegistro.IdGrupoPago = frm.lIdGrupoPago;
                    poRegistro.GrupoPago = frm.lsGrupoPago;

                    poListaRebate.Add(poRegistro);

                    bsRebate.DataSource = poListaRebate;
                    dgvRebate.RefreshData();

                    clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);

                    
                }
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

        private void btnSeleccionarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                List<FacturaDetalleGrid> poLista = (List<FacturaDetalleGrid>)bsDatos.DataSource;

                var sel = poLista.Select(x => x.Sel).FirstOrDefault();
                foreach (var item in poLista)
                {
                    item.Sel = !sel;
                }


                bsDatos.DataSource = poLista;
                dgvDatos.RefreshData();
                //lCalcularTotal();
                //clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lCalcularTotal()
        {
            var pdcTotal = ((List<FacturaDetalleGrid>)bsDatos.DataSource).Sum(x => x.Saldo);
            lblTotal.Text = pdcTotal.ToString("c2");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string psForma = Diccionario.Tablas.Menu.frmMaGrupoPago;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);

                if (poForm != null)
                {
                    frmMaGrupoPago poFrmMostrarFormulario = new frmMaGrupoPago();

                    poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    poFrmMostrarFormulario.Text = poForm.Nombre;
                    poFrmMostrarFormulario.ShowInTaskbar = true;
                    //poFrmMostrarFormulario.MdiParent = this.ParentForm;

                    poFrmMostrarFormulario.ShowDialog();
                    clsComun.gLLenarCombo(ref cmbGrupoPago, loLogicaNegocio.goConsultarComboGrupoPagos(), true);

                }
                else
                {
                    XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizarGrupoPago_Click(object sender, EventArgs e)
        {
            try
            {
                var poLista = loLogicaNegocio.goConsultarComboGrupoPagos();
                frmCombo frmZona = new frmCombo();
                frmZona.lsNombre = "Grupo de Pago";
                frmZona.loCombo = poLista;
                frmZona.ShowDialog();

                string psCode = frmZona.lsSeleccionado;

                if (!string.IsNullOrEmpty(psCode))
                {
                    string psName = poLista.Where(x => x.Codigo == frmZona.lsSeleccionado).Select(x => x.Descripcion).FirstOrDefault();

                    List<FacturaDetalleGrid> poListaRebate = (List<FacturaDetalleGrid>)bsRebate.DataSource;
                    foreach (var item in poListaRebate)
                    {
                        item.IdGrupoPago = int.Parse(psCode);
                        item.GrupoPago = psName;
                    }
                    dgvRebate.RefreshData();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
