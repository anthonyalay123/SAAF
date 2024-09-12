using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Entidad.Entidades.SHEQ;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using REH_Presentacion.SHEQ.Reportes.Rpt;
using SHE_Negocio.Transacciones;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.SHEQ.Transacciones
{
    public partial class frmTrMovimientoInventario : frmBaseTrxDev
    {
        #region Variables
        clsNMovimientoInventario loLogicaNegocio = new clsNMovimientoInventario();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        List<Combo> loComboItem = new List<Combo>();
        List<Maestro> poItems = new List<Maestro>();
        List<Combo> loComboBodega = new List<Combo>();
        bool pbCargado = false;
        public string lsTipoMovimiento;
        public int lIdTransferencia = 0;
        #endregion

        #region Eventos
        public frmTrMovimientoInventario()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            cmbBodega.EditValueChanged += cmbBodega_EditValueChanged;
            cmbMotivo.EditValueChanged += cmbMotivo_EditValueChanged;
            dgvDatos.CellValueChanged += dgvDatos_CellValueChanged;
            dgvDatos.CellValueChanged += dgvDatos_CellValueChangedTwo;
        }

        private void frmTrMovimientoInventario_Load(object sender, EventArgs e)
        {
            try
            {
                if (Text.ToString().ToUpper().Contains("ING"))
                {
                    lsTipoMovimiento = Diccionario.TipoMovimiento.Ingreso;
                    clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoIngresoInventarioEPPNuevo(), true);
                    txtNoFactura.Visible = false;
                    lblNoFactura.Visible = false;
                    lblProveedor.Visible = false;
                    cmbProveedor.Visible = false;
                }
                else
                {
                    lsTipoMovimiento = Diccionario.TipoMovimiento.Egreso;
                    clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoEgresoInventarioEPPNuevo(), true);
                    cmbCentroCosto.Properties.ReadOnly = true;
                    txtNoFactura.Visible = false;
                    lblNoFactura.Visible = false;
                    lblProveedor.Visible = false;
                    cmbProveedor.Visible = false;
                }

                lCargarEventosBotones();

                loComboBodega = loLogicaNegocio.goConsultarComboBodegaEPP();
                loComboItem = loLogicaNegocio.goConsultarComboItemEPP();

                clsComun.gLLenarCombo(ref cmbBodega, loComboBodega);
                clsComun.gLLenarCombo(ref cmbCentroCosto, loLogicaNegocio.goConsultarComboCentroCosto(), true);
                clsComun.gLLenarCombo(ref cmbProveedor, loLogicaNegocio.goConsultarComboProveedorId(), true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboBodega, "IdBodegaEPP", false);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboItem, "IdItemEPP", true);

                int idBodega = loLogicaNegocio.obtenerBodegaUsuario(clsPrincipal.gsUsuario);
                cmbBodega.EditValue = idBodega.ToString();
                cmbBodega.Properties.ReadOnly = true;

                cmbCentroCosto.Properties.ReadOnly = true;
                cmbCentroCosto.EditValue = loLogicaNegocio.obtenerCentroCostoUsuario(clsPrincipal.gsUsuario);

                pbCargado = true;

                dgvDatos.CellValueChanged += dgvDatos_CellValueChanged;

                lConsultarAprobacion();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDatos_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "IdItemEPP" || e.Column.FieldName == "Cantidad" || e.Column.FieldName == "PrecioSinImpuesto")
            {
                var idItemEPP = Convert.ToInt32(dgvDatos.GetRowCellValue(e.RowHandle, "IdItemEPP"));
                var tieneIVA = loLogicaNegocio.ObtenerIva(idItemEPP);

                var cantidad = Convert.ToDecimal(dgvDatos.GetRowCellValue(e.RowHandle, "Cantidad"));
                var precioSinImpuesto = Convert.ToDecimal(dgvDatos.GetRowCellValue(e.RowHandle, "PrecioSinImpuesto"));

                decimal impuesto = 0;
                decimal precioConImpuesto = precioSinImpuesto;
                decimal total;

                if (tieneIVA == true)
                {
                    impuesto = precioSinImpuesto * 0.15M;
                    precioConImpuesto = precioSinImpuesto * 1.15M;
                    dgvDatos.SetRowCellValue(e.RowHandle, "GrabaIva", 1);
                }

                total = cantidad * precioConImpuesto;

                // Actualizar las celdas relevantes en la fila actual
                dgvDatos.SetRowCellValue(e.RowHandle, "Impuesto", impuesto);
                dgvDatos.SetRowCellValue(e.RowHandle, "Costo", precioConImpuesto);
                dgvDatos.SetRowCellValue(e.RowHandle, "Total", total);
            }
        }

        private void dgvDatos_CellValueChangedTwo(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "IdItemEPP")
            {
                var idBodegaEPPOrigen = !string.IsNullOrEmpty(cmbBodega.EditValue.ToString()) ? int.Parse(cmbBodega.EditValue.ToString()) : 0;
                var idItemEPP = Convert.ToInt32(dgvDatos.GetRowCellValue(e.RowHandle, "IdItemEPP"));

                var stock = loLogicaNegocio.ObtenerStockDisponible(idBodegaEPPOrigen, idItemEPP);
                dgvDatos.SetRowCellValue(e.RowHandle, "Stock", stock);
            }
        }

        private void frmTrMovimientoInventario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                var menu = Tag.ToString().Split(',');
                var poListaObject = loLogicaNegocio.listaMovimientos(lsTipoMovimiento);
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Observaciones"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdMovimientoInventario;
                    row["Fecha"] = a.Fecha;
                    row["Observaciones"] = a.Observaciones;
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                pofrmBuscar.Width = 1200;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    lLimpiar();
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar();

                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {

            try
            {

                DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    dgvDatos.PostEditor();

                    MovimientoInventario poObject = new MovimientoInventario();

                    if (lsTipoMovimiento == Diccionario.TipoMovimiento.Ingreso)
                    {
                        poObject.GrupoMotivo = Diccionario.ListaCatalogo.MotivoIngresoInventarioEPP;
                    }
                    else
                    {
                        poObject.GrupoMotivo = Diccionario.ListaCatalogo.MotivoEgresoInventarioEPP;
                    }

                    poObject.IdMovimientoInventario = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                    poObject.Observaciones = txtObservaciones.Text;
                    poObject.Tipo = lsTipoMovimiento;
                    poObject.CodigoMotivo = cmbMotivo.EditValue.ToString();
                    poObject.Fechamovimiento = dtpFecha.DateTime;
                    poObject.CentroCosto = cmbCentroCosto.EditValue.ToString();
                    poObject.IdBodegaEPP = int.Parse(cmbBodega.EditValue.ToString());
                    poObject.NumeroFactura = txtNoFactura.Text;
                    poObject.IdProveedor = int.Parse(cmbProveedor.EditValue.ToString());

                    poObject.MovimientoInventarioDetalle = (List<MovimientoInventarioDetalle>)bsDatos.DataSource;

                    int pId;
                    string psMsg;

                    psMsg = loLogicaNegocio.gsGuardarMovimiento(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, out pId);

                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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


        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var psMsg = loLogicaNegocio.gsAnularMovimiento(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                        if (psMsg == "")
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                DataSet ds = new DataSet();
                var id = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                var dtCabVac = loLogicaNegocio.goConsultaDataSet(string.Format("EXEC SHESPREPORTEMOVIMIENTOINVENTARIO '{0}'", id));

                dtCabVac.Tables[0].TableName = "SHETMOVIMIENTOINVENTARIO";

                dtCabVac.Tables[1].TableName = "SHETMOVIMIENTOINVENTARIODETALLE";

                ds.Merge(dtCabVac);

                if (dtCabVac.Tables[0].Rows.Count > 0)
                {
                    if (Text.ToString().ToUpper().Contains("ING"))
                    {
                        xrptListadoMovimientoInventarioIngreso xrpt = new xrptListadoMovimientoInventarioIngreso();

                        //xrpt.DataSource = ds;
                        //Se invoca la ventana que muestra el reporte.
                        //xrpt.RequestParameters = false;

                        xrpt.Parameters["Titulo"].Value = $"INGRESO DE INVENTARIO - # {txtNo.Text.Trim()}";

                        xrpt.DataSource = ds;
                        xrpt.RequestParameters = false;

                        using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                        {
                            printTool.ShowRibbonPreviewDialog();
                        }
                    }
                    else
                    {
                        xrptListadoMovimientoInventarioEgreso xrpt = new xrptListadoMovimientoInventarioEgreso();

                        xrpt.DataSource = ds;
                        //Se invoca la ventana que muestra el reporte.
                        xrpt.Parameters["Titulo"].Value = $"EGRESO DE INVENTARIO - # {txtNo.Text.Trim()}";
                        xrpt.RequestParameters = false;

                        using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                        {
                            printTool.ShowRibbonPreviewDialog();
                        }
                    }

                    //xrptListadoMovimientoInventarioIngreso xrpt = new xrptListadoMovimientoInventarioIngreso();

                }
                else
                {
                    XtraMessageBox.Show("No existen datos guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Evento del botón Primero, Consulta el primer registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrimero_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Primero, lsTipoMovimiento, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Anterior, Consulta el anterior registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Anterior, lsTipoMovimiento, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón siguiente, Consulta el siguiente registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Siguiente, lsTipoMovimiento, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Último, Consulta el Último registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUltimo_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Ultimo, lsTipoMovimiento, txtNo.Text.Trim());
                lConsultar();
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
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<MovimientoInventarioDetalle>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsDatos.DataSource = poLista;
                    dgvDatos.RefreshData();
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
                bsDatos.AddNew();
                dgvDatos.Focus();
                dgvDatos.ShowEditor();
                dgvDatos.UpdateCurrentRow();
                var poLista = (List<MovimientoInventarioDetalle>)bsDatos.DataSource;
                poLista.LastOrDefault().IdBodegaEPP = int.Parse(cmbBodega.EditValue.ToString());
                poLista.LastOrDefault().IdItemEPP = 0;
                dgvDatos.RefreshData();
                dgvDatos.FocusedColumn = dgvDatos.VisibleColumns[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCopiar_Click(object sender, EventArgs e)
        {
            try
            {

                var menu = Tag.ToString().Split(',');
                var poListaObject = loLogicaNegocio.listaMovimientosEliminados(lsTipoMovimiento);
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Observaciones"),
                                    new DataColumn("Estado"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdMovimientoInventario;
                    row["Fecha"] = a.Fecha;
                    row["Observaciones"] = a.Observaciones;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                pofrmBuscar.Width = 1200;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    lLimpiar();
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultarCopiar();

                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lConsultarCopiar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultarMovimientoCopiar(Convert.ToInt32(txtNo.Text.Trim()), lsTipoMovimiento);
                lsTipoMovimiento = poObject.Tipo;

                if (lsTipoMovimiento == Diccionario.TipoMovimiento.Ingreso)
                {
                    clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoIngresoInventarioEPPNuevo(), true);
                }
                else
                {
                    clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoEgresoInventarioEPPNuevo(), true);
                }

                cmbMotivo.EditValue = poObject.CodigoMotivo;
                //txtNo.EditValue = poObject.IdMovimientoInventario;
                txtNo.EditValue = "";
                txtObservaciones.Text = poObject.Observaciones;
                dtpFecha.DateTime = poObject.Fecha;
                cmbCentroCosto.EditValue = poObject.CentroCosto;
                cmbProveedor.EditValue = poObject.IdProveedor.ToString();
                txtNoFactura.EditValue = poObject.NumeroFactura;

                var Bodega = poObject.MovimientoInventarioDetalle.First().IdBodegaEPP;
                cmbBodega.EditValue = Bodega.ToString();

                bsDatos.DataSource = poObject.MovimientoInventarioDetalle;
                dgvDatos.RefreshData();

                //if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                //dgvDatos.OptionsBehavior.Editable = false;
                //btnAddFila.Enabled = false;
            }
        }

        #endregion

        #region Métodos
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
           
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;
            if (tstBotones.Items["btnCopiar"] != null) tstBotones.Items["btnCopiar"].Click += btnCopiar_Click;

            bsDatos.DataSource = new List<MovimientoInventarioDetalle>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["IdMovimientoInventarioDetalle"].Visible = false;
            dgvDatos.Columns["IdMovimientoInventario"].Visible = false;
            dgvDatos.Columns["GrabaIva"].Visible = false;
            dgvDatos.Columns["IdBodegaEPP"].Visible = false;

            if (lsTipoMovimiento == Diccionario.TipoMovimiento.Egreso)
            {
                dgvDatos.Columns["Costo"].Visible = false;
                dgvDatos.Columns["PrecioSinImpuesto"].Visible = false;
                dgvDatos.Columns["Impuesto"].Visible = false;
                dgvDatos.Columns["Total"].Visible = false;
                dgvDatos.Columns["Stock"].Visible = false;
            }

            dgvDatos.Columns["Stock"].Width = 100;
            dgvDatos.Columns["Stock"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["IdBodegaEPP"].Caption = "Bodega";
            dgvDatos.Columns["IdBodegaEPP"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["IdItemEPP"].Caption = "Item";
            dgvDatos.Columns["IdItemEPP"].Width = 220;

            dgvDatos.Columns["Costo"].Caption = "Valor con IVA";
            dgvDatos.Columns["Costo"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["PrecioSinImpuesto"].Caption = "Costo";
            dgvDatos.Columns["Impuesto"].Caption = "IVA";
            dgvDatos.Columns["Impuesto"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Total"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["Costo"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Costo"].DisplayFormat.FormatString = "c2";
            dgvDatos.Columns["Impuesto"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Impuesto"].DisplayFormat.FormatString = "c2";
            dgvDatos.Columns["PrecioSinImpuesto"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["PrecioSinImpuesto"].DisplayFormat.FormatString = "c2";


            dgvDatos.Columns["Total"].UnboundType = UnboundColumnType.Decimal;
            dgvDatos.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Total"].DisplayFormat.FormatString = "c2";
            dgvDatos.Columns["Total"].Summary.Add(SummaryItemType.Sum, "Total", "{0:c2}");

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);

            dtpFecha.DateTime = DateTime.Now;

            dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;

        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultarMovimiento(Convert.ToInt32(txtNo.Text.Trim()), lsTipoMovimiento);
                lsTipoMovimiento = poObject.Tipo;

                if (lsTipoMovimiento == Diccionario.TipoMovimiento.Ingreso)
                {
                    clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoIngresoInventarioEPPNuevo(), true);
                }
                else
                {
                    clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoEgresoInventarioEPPNuevo(), true);
                }

                cmbMotivo.EditValue = poObject.CodigoMotivo;
                txtNo.EditValue = poObject.IdMovimientoInventario;
                txtObservaciones.Text = poObject.Observaciones;
                dtpFecha.DateTime = poObject.Fecha;
                cmbProveedor.EditValue = poObject.IdProveedor.ToString();
                txtNoFactura.EditValue = poObject.NumeroFactura;

                cmbCentroCosto.EditValue = poObject.CentroCosto;
                if (poObject.CentroCosto == null)
                {
                    clsComun.gLLenarCombo(ref cmbCentroCosto, loLogicaNegocio.goConsultarComboCentroCosto(), true);
                } 


                var Bodega = poObject.MovimientoInventarioDetalle.First().IdBodegaEPP;
                cmbBodega.EditValue = Bodega.ToString();

                bsDatos.DataSource = poObject.MovimientoInventarioDetalle;
                dgvDatos.RefreshData();

                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                dgvDatos.OptionsBehavior.Editable = false;
                btnAddFila.Enabled = false;
            }
        }

        private void cmbBodega_EditValueChanged(object sender, EventArgs e)
        {
            ActualizarColumnasBodega("IdBodegaEPP", cmbBodega);
            ActualizarStock();
        }

        private void cmbMotivo_EditValueChanged(object sender, EventArgs e)
        {
            var vsMotivo = cmbMotivo.EditValue.ToString();
            if (vsMotivo == "CON")
            {
                cmbCentroCosto.EditValue = "001";
            } else if (vsMotivo == "AJE")
            {
                cmbCentroCosto.EditValue = "028";
            } else if (vsMotivo == "COM")
            {
                txtNoFactura.Visible = true;
                lblNoFactura.Visible = true;
                lblProveedor.Visible = true;
                cmbProveedor.Visible = true;
            } else if (vsMotivo == "AJI")
            {
                txtNoFactura.Visible = false;
                lblNoFactura.Visible = false;
                lblProveedor.Visible = false;
                cmbProveedor.Visible = false;
            }
        }
           
        private void ActualizarStock()
        {
            if (bsDatos.DataSource is List<MovimientoInventarioDetalle> poLista)
            {
                int idBodegaEPPOrigen = !string.IsNullOrEmpty(cmbBodega.EditValue.ToString()) ? int.Parse(cmbBodega.EditValue.ToString()) : 0;
                foreach (var item in poLista)
                {
                    if (item.IdItemEPP != 0)
                    {
                        var stock = loLogicaNegocio.ObtenerStockDisponible(idBodegaEPPOrigen, item.IdItemEPP);
                        item.Stock = stock;
                    }
                }
                dgvDatos.RefreshData();
            }
        }

        private void ActualizarColumnasBodega(string columnName, LookUpEdit comboBox)
        {
            if (bsDatos.DataSource is List<TransferenciaStockDetalle> poLista)
            {
                int newValue = !string.IsNullOrEmpty(comboBox.EditValue.ToString()) ? int.Parse(comboBox.EditValue.ToString()) : 0;
                foreach (var item in poLista)
                {
                    item.GetType().GetProperty(columnName)?.SetValue(item, newValue);
                }
                dgvDatos.RefreshData();
            }
        }

        private void dgvDatos_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Cantidad" || e.Column.FieldName == "PrecioSinImpuesto" || e.Column.FieldName == "IdItemEPP")
                {
                    int piIndex;
                    piIndex = e.RowHandle;
                    var poLista = (List<MovimientoInventarioDetalle>)bsDatos.DataSource;
                    var poItem = poItems.Where(x => x.Codigo == poLista[piIndex].Codigo.ToString()).FirstOrDefault();
                    if (poItem != null)
                    {
                        poLista[piIndex].GrabaIva = poItem.GrabaIva;
                    }
                    else
                    {
                        poLista[piIndex].GrabaIva = false;
                    }

                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lLimpiar()
        {
            pbCargado = false;
            txtNo.EditValue = "";
            txtObservaciones.Text = "";
            txtNoFactura.Text = "";
            bsDatos.DataSource = new List<MovimientoInventarioDetalle>();
            dgvDatos.RefreshData();
            pbCargado = true;
            if ((cmbMotivo.Properties.DataSource as IList).Count > 0) cmbMotivo.ItemIndex = 0;
            if ((cmbCentroCosto.Properties.DataSource as IList).Count > 0) cmbCentroCosto.ItemIndex = 0;
            if ((cmbProveedor.Properties.DataSource as IList).Count > 0) cmbProveedor.ItemIndex = 0;
            if ((cmbBodega.Properties.DataSource as IList).Count > 0) cmbBodega.ItemIndex = 0;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
            dgvDatos.OptionsBehavior.Editable = true;
            btnAddFila.Enabled = true;

        }

        public void lConsultarAprobacion()
        {
            if (lIdTransferencia != 0)
            {
                var poObject = loLogicaNegocio.goConsultarMovimiento(lIdTransferencia);
                lsTipoMovimiento = poObject.Tipo;

                if (lsTipoMovimiento == Diccionario.TipoMovimiento.Ingreso)
                {
                    clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoIngresoInventarioEPPNuevo(), true);
                }
                else
                {
                    clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoEgresoInventarioEPPNuevo(), true);
                }

                cmbMotivo.EditValue = poObject.CodigoMotivo;
                txtNo.EditValue = poObject.IdMovimientoInventario;
                txtObservaciones.Text = poObject.Observaciones;
                dtpFecha.DateTime = poObject.Fecha;
                cmbProveedor.EditValue = poObject.IdProveedor.ToString();
                txtNoFactura.EditValue = poObject.NumeroFactura;

                cmbCentroCosto.EditValue = poObject.CentroCosto;
                if (poObject.CentroCosto == null)
                {
                    clsComun.gLLenarCombo(ref cmbCentroCosto, loLogicaNegocio.goConsultarComboCentroCosto(), true);
                }


                var Bodega = poObject.MovimientoInventarioDetalle.First().IdBodegaEPP;
                cmbBodega.EditValue = Bodega.ToString();

                bsDatos.DataSource = poObject.MovimientoInventarioDetalle;
                dgvDatos.RefreshData();

                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Enabled = false;
                if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Enabled = false;
                if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Enabled = false;
                if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Enabled = false;
                if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Enabled = false;
                if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Enabled = false;
                if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = false;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                if (tstBotones.Items["btnCopiar"] != null) tstBotones.Items["btnCopiar"].Enabled = false;

                dgvDatos.OptionsBehavior.Editable = false;
                btnAddFila.Enabled = false;
            }

        }

        #endregion
    }
}
