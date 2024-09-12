using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Administracion;
using GEN_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
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

namespace REH_Presentacion.Administracion.Transacciones
{
    public partial class frmTrMovimientoInventario : frmBaseTrxDev
    {
        #region Variables
        clsNInventarioSuministros loLogicaNegocio = new clsNInventarioSuministros();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        List<Combo> loComboItem = new List<Combo>();
        List<Combo> loComboBodega = new List<Combo>();
        List<Item> poItems = new List<Item>();
        bool pbCargado = false;
        string lsTipoMovimiento;
        #endregion

        #region Eventos
        public frmTrMovimientoInventario()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        private void frmTrMovimientoInventario_Load(object sender, EventArgs e)
        {
            try
            {
                if (Text.ToString().ToUpper().Contains("ING"))
                {
                    lsTipoMovimiento = Diccionario.TipoMovimiento.Ingreso;
                    clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoIngresoInventario(),true);
                    cmbMotivo.EditValue = "ING";

                }
                else
                {
                    lsTipoMovimiento = Diccionario.TipoMovimiento.Egreso;
                    clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoEgresoInventario(), true);
                    cmbMotivo.EditValue = "EGR";
                }

                lCargarEventosBotones();

                loComboBodega = loLogicaNegocio.goConsultarComboBodega();
                loComboItem = loLogicaNegocio.goConsultarComboItem();

                clsComun.gLLenarCombo(ref cmbBodega, loComboBodega);
                clsComun.gLLenarCombo(ref cmbCentroCosto, loLogicaNegocio.goConsultarComboCentroCosto(),true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboBodega, "IdBodegaEPP", false);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboItem, "IdItemEPP", true);

                cmbCentroCosto.EditValue = "001";

                pbCargado = true;

                dtpFechaTransaccion.DateTime = DateTime.Now.Date;

                poItems = loLogicaNegocio.goListarMaestroItem();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    MovimientoInventarioAdm poObject = new MovimientoInventarioAdm();

                    if (lsTipoMovimiento == Diccionario.TipoMovimiento.Ingreso)
                    {
                        poObject.GrupoMotivo = Diccionario.ListaCatalogo.MotivoIngresoInventario;
                    }
                    else
                    {
                        poObject.GrupoMotivo = Diccionario.ListaCatalogo.MotivoEgresoInventario;
                    }

                    poObject.IdMovimientoInventario = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                    poObject.Observaciones = txtObservaciones.Text;
                    poObject.Tipo = lsTipoMovimiento;
                    poObject.CodigoMotivo = cmbMotivo.EditValue.ToString();
                    poObject.CodigoCentroCosto = cmbCentroCosto.EditValue.ToString();
                    poObject.CentroCosto = cmbCentroCosto.Text;
                    poObject.Fechamovimiento = dtpFechaTransaccion.DateTime;

                    poObject.MovimientoInventarioDetalleAdm = (List<MovimientoInventarioDetalleAdm>)bsDatos.DataSource;

                    int pId;
                    string psMsg = loLogicaNegocio.gsGuardarMovimiento(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal,out pId);

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
                        loLogicaNegocio.gsAnularMovimiento(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
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
            try
            {
                //if (!string.IsNullOrEmpty(txtNo.Text.ToString()))
                //{
                //    int lId = int.Parse(txtNo.Text);


                //    //DataSet ds = new DataSet();
                //    var ds = loLogicaNegocio.goConsultaDataSet(string.Format("EXEC SHESPMovimientoInventarioAdm '{0}','{1}'",);

                //    ds.Tables[0].TableName = "MovimientoInventarioAdm";

                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        xrptListadoMovimientoInventarioAdm xrpt = new xrptListadoMovimientoInventarioAdm();
                //        xrpt.DataSource = ds;
                //        xrpt.RequestParameters = false;

                //        using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                //        {
                //            printTool.ShowRibbonPreviewDialog();
                //        }
                //    }
                //}
                //else
                //{
                //    XtraMessageBox.Show("No existe detalles guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigoMovInv(Diccionario.BuscarCodigo.Tipo.Primero, txtNo.Text.Trim());
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigoMovInv(Diccionario.BuscarCodigo.Tipo.Anterior, txtNo.Text.Trim());
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigoMovInv(Diccionario.BuscarCodigo.Tipo.Siguiente, txtNo.Text.Trim());
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigoMovInv(Diccionario.BuscarCodigo.Tipo.Ultimo, txtNo.Text.Trim());
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
                var poLista = (List<MovimientoInventarioDetalleAdm>)bsDatos.DataSource;

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
                var poLista = (List<MovimientoInventarioDetalleAdm>)bsDatos.DataSource;
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

            bsDatos.DataSource = new List<MovimientoInventarioDetalleAdm>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["IdMovimientoInventarioDetalle"].Visible = false;
            dgvDatos.Columns["IdMovimientoInventario"].Visible = false;
            dgvDatos.Columns["GrabaIva"].Visible = false;

            if (lsTipoMovimiento == Diccionario.TipoMovimiento.Egreso)
            {
                dgvDatos.Columns["Costo"].Visible = false;
                dgvDatos.Columns["PrecioSinImpuesto"].Visible = false;
                dgvDatos.Columns["Impuesto"].Visible = false;
                dgvDatos.Columns["Total"].Visible = false;
            }

            dgvDatos.Columns["Costo"].Caption = "PVP";
            dgvDatos.Columns["IdBodegaEPP"].Caption = "Bodega";
            dgvDatos.Columns["IdItemEPP"].Caption = "Item";
            dgvDatos.Columns["PrecioSinImpuesto"].Caption = "Precio";

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


            //GridGroupSummaryItem item1 = new GridGroupSummaryItem();
            //item1.FieldName = psNameColumn;
            //item1.SummaryType = SummaryItemType.Sum;
            //item1.DisplayFormat = "{0:c2}";
            //item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
            //dgvDatos.GroupSummary.Add(item1);

            dgvDatos.Columns["Codigo"].Width = 50;
            dgvDatos.Columns["IdItemEPP"].Width = 200;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);

            dtpFecha.DateTime = DateTime.Now;

        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultarMovimiento(Convert.ToInt32(txtNo.Text.Trim()));
                lsTipoMovimiento = poObject.Tipo;

                if (lsTipoMovimiento == Diccionario.TipoMovimiento.Ingreso)
                {
                    clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoIngresoInventario(), true);
                }
                else
                {
                    clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoEgresoInventario(), true);
                }

                cmbMotivo.EditValue = poObject.CodigoMotivo;
                txtNo.EditValue = poObject.IdMovimientoInventario;
                txtObservaciones.Text = poObject.Observaciones;
                dtpFecha.DateTime = poObject.Fecha;
                cmbCentroCosto.EditValue = poObject.CodigoCentroCosto;
                dtpFechaTransaccion.DateTime = poObject.Fechamovimiento;


                bsDatos.DataSource = poObject.MovimientoInventarioDetalleAdm;
                dgvDatos.RefreshData();

                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                dgvDatos.OptionsBehavior.Editable = false;
                btnAddFila.Enabled = false;
            }
        }
        
        private void lLimpiar()
        {
            pbCargado = false;
            txtNo.EditValue = "";
            txtObservaciones.Text = "";
            bsDatos.DataSource = new List<MovimientoInventarioDetalleAdm>();
            dgvDatos.RefreshData();
            pbCargado = true;
            //if ((cmbMotivo.Properties.DataSource as IList).Count > 0) cmbMotivo.ItemIndex = 0;
            if ((cmbBodega.Properties.DataSource as IList).Count > 0) cmbBodega.ItemIndex = 0;
            if ((cmbCentroCosto.Properties.DataSource as IList).Count > 0) cmbCentroCosto.EditValue = "001";
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
            dgvDatos.OptionsBehavior.Editable = true;
            btnAddFila.Enabled = true;
            dtpFechaTransaccion.DateTime = DateTime.Now.Date;
            if (lsTipoMovimiento == "I")
            {
                cmbMotivo.EditValue = "ING";
            }
            else
            {
                cmbMotivo.EditValue = "EGR";
            }
        }
        #endregion

        private void dgvDatos_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Cantidad" || e.Column.FieldName == "PrecioSinImpuesto" || e.Column.FieldName == "IdItemEPP")
                {
                    int piIndex;
                    piIndex = e.RowHandle;
                    var poLista = (List<MovimientoInventarioDetalleAdm>)bsDatos.DataSource;
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
    }
}
