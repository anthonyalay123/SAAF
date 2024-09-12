using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.SHEQ;
using REH_Presentacion.Comun;
using REH_Presentacion.Credito.Transacciones;
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
    public partial class frmTrTransferenciaStock : frmBaseTrxDev
    {
        BindingSource bsDatos = new BindingSource();
        clsNTransferenciaStock loLogicaNegocio = new clsNTransferenciaStock();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        List<Combo> loComboBodega = new List<Combo>();
        List<Combo> loComboItem = new List<Combo>();
        List<Combo> loComboMotivo = new List<Combo>();
        bool pbCargado = false;
        public int lIdTransferencia = 0;

        public frmTrTransferenciaStock()
        {
            //m_MainForm = form;
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            cmbBodegaOrigen.EditValueChanged += cmbBodegaOrigen_EditValueChanged;
            cmbBodegaDestino.EditValueChanged += cmbBodegaDestino_EditValueChanged;
            
        }

        private void frmTrTransferenciaStock_Load(object sender, EventArgs e)
        {
            lCargarEventosBotones();

            loComboBodega = loLogicaNegocio.goConsultarComboBodegaEPP();

            clsComun.gLLenarCombo(ref cmbBodegaOrigen, loComboBodega);
            clsComun.gLLenarCombo(ref cmbBodegaDestino, loComboBodega);

            loComboMotivo = loLogicaNegocio.goConsultarMotivoTransferencia();
            loComboItem = loLogicaNegocio.goConsultarComboItemEPP();

            clsComun.gLLenarCombo(ref cmbMotivo, loComboMotivo);
            clsComun.gLLenarComboGrid(ref dgvDatos, loComboBodega, "IdBodegaEPPOrigen", false);
            clsComun.gLLenarComboGrid(ref dgvDatos, loComboBodega, "IdBodegaEPPDestino", false);
            clsComun.gLLenarComboGrid(ref dgvDatos, loComboItem, "IdItemEPP", true);

            int idBodega = loLogicaNegocio.obtenerBodegaUsuario(clsPrincipal.gsUsuario);

            if (idBodega == 1)
            {
                cmbBodegaOrigen.EditValue = idBodega.ToString();
                cmbBodegaDestino.EditValue = "2";
            } else if (idBodega == 2)
            {
                cmbBodegaOrigen.EditValue = idBodega.ToString();
                cmbBodegaDestino.EditValue = "1";
            } else
            {
                cmbBodegaOrigen.EditValue = idBodega.ToString();
                cmbBodegaDestino.EditValue = "1";
            }

            cmbBodegaOrigen.Properties.ReadOnly = true;

            lConsultarAprobacion();


            dtpFecha.DateTime = DateTime.Now;
            dgvDatos.CellValueChanged += dgvDatos_CellValueChanged;
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();

            //if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnCopiar"] != null) tstBotones.Items["btnCopiar"].Click += btnCopiar_Click;

            bsDatos.DataSource = new List<TransferenciaStockDetalle>();
            gcDatos.DataSource = bsDatos;
            dgvDatos.Columns["IdTransferenciaStockDetalle"].Visible = false;
            dgvDatos.Columns["IdTransferenciaStock"].Visible = false;
            dgvDatos.Columns["IdBodegaEPPOrigen"].Visible = false;
            dgvDatos.Columns["IdBodegaEPPDestino"].Visible = false;

            dgvDatos.Columns["IdBodegaEPPOrigen"].Caption = "Bodega Origgen";
            dgvDatos.Columns["IdBodegaEPPDestino"].Caption = "Bodega Destino";

            dgvDatos.Columns["IdItemEPP"].Caption = "Item";
            dgvDatos.Columns["IdItemEPP"].Width = 220;
            dgvDatos.Columns["Stock"].Width = 100;
            dgvDatos.Columns["Stock"].OptionsColumn.AllowEdit = false;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 30);

        }

        private void dgvDatos_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "IdItemEPP")
            {
                var idBodegaEPPOrigen = !string.IsNullOrEmpty(cmbBodegaOrigen.EditValue.ToString()) ? int.Parse(cmbBodegaOrigen.EditValue.ToString()) : 0;
                var idItemEPP = Convert.ToInt32(dgvDatos.GetRowCellValue(e.RowHandle, "IdItemEPP"));

                var stock = loLogicaNegocio.ObtenerStockDisponible(idBodegaEPPOrigen, idItemEPP);
                dgvDatos.SetRowCellValue(e.RowHandle, "Stock", stock);
            }
        }

        private void cmbBodegaOrigen_EditValueChanged(object sender, EventArgs e)
        {
            ActualizarColumnasBodega("IdBodegaEPPOrigen", cmbBodegaOrigen);
            ActualizarStock();
        }

        private void cmbBodegaDestino_EditValueChanged(object sender, EventArgs e)
        {
            ActualizarColumnasBodega("IdBodegaEPPDestino", cmbBodegaDestino);
        }

        private void ActualizarStock()
        {
            if (bsDatos.DataSource is List<TransferenciaStockDetalle> poLista)
            {
                int idBodegaEPPOrigen = !string.IsNullOrEmpty(cmbBodegaOrigen.EditValue.ToString()) ? int.Parse(cmbBodegaOrigen.EditValue.ToString()) : 0;
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

        private void ActualizarColumnasBodega(string bodega, LookUpEdit comboBox)
        {
            if (bsDatos.DataSource is List<TransferenciaStockDetalle> poLista)
            {
                int idBodegaEP = !string.IsNullOrEmpty(comboBox.EditValue.ToString()) ? int.Parse(comboBox.EditValue.ToString()) : 0;
                foreach (var item in poLista)
                {
                    item.GetType().GetProperty(bodega)?.SetValue(item, idBodegaEP);
                }
                dgvDatos.RefreshData();
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
                        loLogicaNegocio.gsEliminarMovimiento(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                DataSet ds = new DataSet();
                var id = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                var dtCabVac = loLogicaNegocio.goConsultaDataSet(string.Format("EXEC SHESPTRANSFERENCIASTOCK '{0}'", id));

                dtCabVac.Tables[0].TableName = "SHETTRANSFERENCIASTOCK";

                dtCabVac.Tables[1].TableName = "SHETRANSFERENCIASTOCKDETALLE";

                ds.Merge(dtCabVac);

                if (dtCabVac.Tables[0].Rows.Count > 0)
                {
                    xrptListadoItemTransferencia xrpt = new xrptListadoItemTransferencia();

                    xrpt.DataSource = ds;
                    //Se invoca la ventana que muestra el reporte.
                    xrpt.Parameters["Titulo"].Value = $"REPORTE DE TRANSFERENCIA DE STOCK - # {txtNo.Text.Trim()}";
                    xrpt.RequestParameters = false;

                    using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                    {
                        printTool.ShowRibbonPreviewDialog();
                    }
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

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();

                TransferenciaStock poObject = new TransferenciaStock();

                poObject.IdTransferenciaStock = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                poObject.Observaciones = txtObservaciones.Text;
                poObject.CodigoMotivo = cmbMotivo.EditValue.ToString();
                poObject.GrupoMotivo = Diccionario.ListaCatalogo.MotivoTransferencia;
                poObject.FechaTransferencia = dtpFecha.DateTime;
                poObject.IdBodegaEPPOrigen = !string.IsNullOrEmpty(cmbBodegaOrigen.EditValue.ToString()) ? int.Parse(cmbBodegaOrigen.EditValue.ToString()) : 0;
                poObject.IdBodegaEPPDestino = !string.IsNullOrEmpty(cmbBodegaDestino.EditValue.ToString()) ? int.Parse(cmbBodegaDestino.EditValue.ToString()) : 0;
                poObject.TransferenciaStockDetalle = (List<TransferenciaStockDetalle>)bsDatos.DataSource;

                DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    string psMsg = loLogicaNegocio.gsGuardar(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

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
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                var menu = Tag.ToString().Split(',');
                var poListaObject = loLogicaNegocio.listaTodos();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("FechaTransferencia", typeof(DateTime)),
                                    new DataColumn("Observaciones"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdTransferenciaStock;
                    row["FechaTransferencia"] = a.FechaTransferencia;
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

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultarMovimiento(Convert.ToInt32(txtNo.Text.Trim()));

                cmbMotivo.EditValue = poObject.CodigoMotivo;
                txtNo.EditValue = poObject.IdTransferenciaStock;
                txtObservaciones.Text = poObject.Observaciones;
                dtpFecha.DateTime = poObject.FechaTransferencia;
                cmbBodegaOrigen.EditValue = poObject.IdBodegaEPPOrigen.ToString();
                cmbBodegaDestino.EditValue = poObject.IdBodegaEPPDestino.ToString();

                bsDatos.DataSource = poObject.TransferenciaStockDetalle;
                dgvDatos.RefreshData();

                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                dgvDatos.OptionsBehavior.Editable = false;
                btnAddFila.Enabled = false;
                cmbBodegaOrigen.Enabled = false;
                cmbBodegaDestino.Enabled = false;
                dtpFecha.Enabled = false;
                cmbMotivo.Enabled = false;
                txtObservaciones.Enabled = false;
            }
        }

        public void lConsultarAprobacion()
        {
            if (lIdTransferencia != 0)
            {
                var poObject = loLogicaNegocio.goConsultarMovimiento(lIdTransferencia);

                cmbMotivo.EditValue = poObject.CodigoMotivo;
                txtNo.EditValue = poObject.IdTransferenciaStock;
                txtObservaciones.Text = poObject.Observaciones;
                dtpFecha.DateTime = poObject.FechaTransferencia;
                cmbBodegaOrigen.EditValue = poObject.IdBodegaEPPOrigen.ToString();
                cmbBodegaDestino.EditValue = poObject.IdBodegaEPPDestino.ToString();

                bsDatos.DataSource = poObject.TransferenciaStockDetalle;
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
                cmbBodegaOrigen.Enabled = false;
                cmbBodegaDestino.Enabled = false;
                dtpFecha.Enabled = false;
                cmbMotivo.Enabled  = false;
                txtObservaciones.Enabled = false;
            }
           
            
        }

        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<TransferenciaStockDetalle>)bsDatos.DataSource;

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

        private void lLimpiar()
        {
            pbCargado = false;
            txtNo.EditValue = "";
            txtObservaciones.Text = "";
            bsDatos.DataSource = new List<TransferenciaStockDetalle>();
            dgvDatos.RefreshData();
            pbCargado = true;
            if ((cmbMotivo.Properties.DataSource as IList).Count > 0) cmbMotivo.ItemIndex = 0;
            if ((cmbBodegaOrigen.Properties.DataSource as IList).Count > 0) cmbBodegaOrigen.ItemIndex = 0;
            if ((cmbBodegaDestino.Properties.DataSource as IList).Count > 0) cmbBodegaDestino.ItemIndex = 0;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
            dgvDatos.OptionsBehavior.Editable = true;
            btnAddFila.Enabled = true;
            cmbBodegaDestino.Enabled = true;
            dtpFecha.Enabled = true;
            cmbMotivo.Enabled = true;
            txtObservaciones.Enabled = true;

        }

        private void btnAddFila_Click_1(object sender, EventArgs e)
        {
            try
            {
                bsDatos.AddNew();
                dgvDatos.Focus();
                dgvDatos.ShowEditor();
                dgvDatos.UpdateCurrentRow();
                var poLista = (List<TransferenciaStockDetalle>)bsDatos.DataSource;
                poLista.LastOrDefault().IdBodegaEPPOrigen = int.Parse(cmbBodegaOrigen.EditValue.ToString());
                poLista.LastOrDefault().IdBodegaEPPDestino = int.Parse(cmbBodegaDestino.EditValue.ToString());
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
                var poListaObject = loLogicaNegocio.listaMovimientosEliminados();
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
                    row["No"] = a.IdTransferenciaStock;
                    row["Fecha"] = a.FechaTransferencia;
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
                var poObject = loLogicaNegocio.goConsultarMovimientoCopiar(Convert.ToInt32(txtNo.Text.Trim()));

                cmbMotivo.EditValue = poObject.CodigoMotivo;
                //txtNo.EditValue = poObject.IdTransferenciaStock;
                txtNo.EditValue = "";
                txtObservaciones.Text = poObject.Observaciones;
                dtpFecha.DateTime = poObject.FechaTransferencia;
                cmbBodegaOrigen.EditValue = poObject.IdBodegaEPPOrigen.ToString();
                cmbBodegaDestino.EditValue = poObject.IdBodegaEPPDestino.ToString();

                bsDatos.DataSource = poObject.TransferenciaStockDetalle;
                dgvDatos.RefreshData();

                //if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                //dgvDatos.OptionsBehavior.Editable = false;
                //btnAddFila.Enabled = false;
            }
        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Primero, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Ultimo, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Siguiente, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Anterior, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
