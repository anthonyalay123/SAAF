using DevExpress.XtraEditors;
using GEN_Entidad.Entidades.SHEQ;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using SHE_Negocio.Transacciones;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GEN_Entidad;
using REH_Presentacion.Comun;
using DevExpress.XtraEditors.Repository;
using REH_Presentacion.SHEQ.Reportes.Rpt;
using DevExpress.XtraBars;

namespace REH_Presentacion.SHEQ.Transacciones
{
    public partial class frmTrEntregaEPP : frmBaseTrxDev
    {
        BindingSource bsDatos = new BindingSource();
        clsNEntregaEPP loLogicaNegocio = new clsNEntregaEPP();
        List<Combo> loComboItem = new List<Combo>();
        List<Combo> loComboEmpleado = new List<Combo>();
        List<Combo> loComboBodega = new List<Combo>();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        public int lIdTransferencia = 0;

        public frmTrEntregaEPP()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            cmbBodega.EditValueChanged += cmbBodega_EditValueChanged;
            cmbEmpleado.EditValueChanged += cmbEmpleado_EditValueChanged;
        }

        private void frmTrEntregaEPP_Load(object sender, EventArgs e)
        {
            lCargarEventosBotones();

            loComboItem = loLogicaNegocio.goConsultarComboItemEPP();
            loComboBodega = loLogicaNegocio.goConsultarComboBodegaEPP();
            loComboEmpleado = loLogicaNegocio.goConsultarComboPersonaContrato();

            clsComun.gLLenarCombo(ref cmbBodega, loComboBodega, true);
            clsComun.gLLenarCombo(ref cmbEmpleado, loComboEmpleado, true);
            clsComun.gLLenarCombo(ref cmbCentroCosto, loLogicaNegocio.goConsultarComboCentroCosto(), true);
            clsComun.gLLenarComboGrid(ref dgvDatos, loComboBodega, "IdBodega", false);
            clsComun.gLLenarComboGrid(ref dgvDatos, loComboItem, "IdItemEPP", true);

            cmbBodega.Properties.ReadOnly = true;
            cmbCentroCosto.Properties.ReadOnly = true;

            dgvDatos.RefreshData();

            dtpFecha.DateTime = DateTime.Now;

            lConsultarEntrega();
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();

            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnCopiar"] != null) tstBotones.Items["btnCopiar"].Click += btnCopiar_Click;

            //BarItemLink btnNuevo = bar2.ItemLinks.FirstOrDefault(c => c.Caption == "Nuevo");
            //if (btnNuevo != null) btnNuevo.Item.ItemClick += btnNuevo_Click;
            //BarItemLink btnBuscar = bar2.ItemLinks.FirstOrDefault(c => c.Caption == "Buscar");
            //if (btnBuscar != null) btnBuscar.Item.ItemClick += btnBuscar_Click;
            //BarItemLink btnGrabar = bar2.ItemLinks.FirstOrDefault(c => c.Caption == "Guardar");
            //if (btnGrabar != null) btnGrabar.Item.ItemClick += btnGrabar_Click;
            //BarItemLink btnPrimero = bar2.ItemLinks.FirstOrDefault(c => c.Caption == "");
            //if (btnPrimero != null) btnPrimero.Item.ItemClick += btnPrimero_Click;

            //foreach (BarItemLink link in bar2.ItemLinks)
            //{
            //    if (link.Item.Name == "btnNuevo") link.Item.ItemClick += btnNuevo_Click;
            //    else if (link.Item.Name == "btnBuscar") link.Item.ItemClick += btnBuscar_Click;
            //    else if (link.Item.Name == "btnGrabar") link.Item.ItemClick += btnGrabar_Click;
            //    else if (link.Item.Name == "btnPrimero") link.Item.ItemClick += btnPrimero_Click;
            //    else if (link.Item.Name == "btnAnterior") link.Item.ItemClick += btnAnterior_Click;
            //    else if (link.Item.Name == "btnSiguiente") link.Item.ItemClick += btnSiguiente_Click;
            //    else if (link.Item.Name == "btnUltimo") link.Item.ItemClick += btnUltimo_Click;
            //    else if (link.Item.Name == "btnEliminar") link.Item.ItemClick += btnEliminar_Click;
            //    else if (link.Item.Name == "btnImprimir") link.Item.ItemClick += btnImprimir_Click;
            //    else if (link.Item.Name == "btnCopiar") link.Item.ItemClick += btnCopiar_Click;
            //}

            bsDatos.DataSource = new List<EntregaEPPDetalle>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["IdEntregaEPPDetalle"].Visible = false;
            dgvDatos.Columns["Codigo"].Caption = "No";
            dgvDatos.Columns["Codigo"].Width = 20;
            dgvDatos.Columns["IdEntregaEPP"].Visible = false;
            dgvDatos.Columns["IdBodega"].Visible = false;

            dgvDatos.Columns["IdItemEPP"].Caption = "Item";
            dgvDatos.Columns["IdItemEPP"].Width = 220;
            dgvDatos.Columns["FechaEntrega"].Caption = "Fecha Entrega";
            dgvDatos.Columns["Cantidad"].Caption = "Cantidad";

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 30);

        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                DataSet ds = new DataSet();
                var id = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                var dtCabVac = loLogicaNegocio.goConsultaDataSet(string.Format("EXEC SHESPENTREGAEPP '{0}'", id));

                dtCabVac.Tables[0].TableName = "SHETENTREGAEPP";

                dtCabVac.Tables[1].TableName = "SHETENTREGAEPPDETALLE";

                ds.Merge(dtCabVac);

                if (dtCabVac.Tables[0].Rows.Count > 0)
                {

                    xrptListadoEntregaEPP xrpt = new xrptListadoEntregaEPP();

                    xrpt.DataSource = ds;
                    //Se invoca la ventana que muestra el reporte.
                    xrpt.Parameters["Titulo"].Value = $"REPORTE DE ENTREGA EPP - # {txtNo.Text.Trim()}";
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

        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<EntregaEPPDetalle>)bsDatos.DataSource;

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
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Empleado"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdEntregaEPP;
                    row["Fecha"] = a.FechaIngreso;
                    row["Empleado"] = loLogicaNegocio.obtenerNombreCompleto(a.IdEmpleado);
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

        private void btnCopiar_Click(object sender, EventArgs e)
        {
            try
            {

                var menu = Tag.ToString().Split(',');
                var poListaObject = loLogicaNegocio.listaTodosCopia();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Empleado"),
                                    new DataColumn("Estado"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdEntregaEPP;
                    row["Fecha"] = a.FechaIngreso;
                    row["Empleado"] = loLogicaNegocio.obtenerNombreCompleto(a.IdEmpleado);
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

                cmbBodega.EditValue = poObject.IdBodega.ToString();
                cmbEmpleado.EditValue = poObject.IdEmpleado.ToString();
                cmbCentroCosto.EditValue = poObject.CentroCosto.ToString();
                //txtNo.EditValue = poObject.IdEntregaEPP;
                txtNo.EditValue = "";
                txtObservaciones.EditValue = poObject.Observaciones;
                dtpFecha.DateTime = DateTime.Now;

                bsDatos.DataSource = poObject.EntregaEPPDetalle;
                dgvDatos.RefreshData();
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

        private void lLimpiar()
        {
            txtNo.EditValue = "";
            txtObservaciones.EditValue = "";
            bsDatos.DataSource = new List<EntregaEPPDetalle>();
            dgvDatos.RefreshData();
            if ((cmbBodega.Properties.DataSource as IList).Count > 0) cmbBodega.ItemIndex = 0;
            if ((cmbEmpleado.Properties.DataSource as IList).Count > 0) cmbEmpleado.ItemIndex = 0;
            if ((cmbCentroCosto.Properties.DataSource as IList).Count > 0) cmbCentroCosto.ItemIndex = 0;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;

            //BarItemLink btnGrabar = bar2.ItemLinks.FirstOrDefault(c => c.Caption == "Guardar");
            //if (btnGrabar != null) btnGrabar.Item.Enabled = true;

            dgvDatos.OptionsBehavior.Editable = true;
            btnAddFila.Enabled = true;

        }

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                bsDatos.AddNew();
                dgvDatos.Focus();
                dgvDatos.ShowEditor();
                dgvDatos.UpdateCurrentRow();
                var poLista = (List<EntregaEPPDetalle>)bsDatos.DataSource;
                poLista.LastOrDefault().IdBodega = int.Parse(cmbBodega.EditValue.ToString());
                poLista.LastOrDefault().IdItemEPP = 0;
                poLista.LastOrDefault().FechaEntrega = DateTime.Now;
                //poLista.LastOrDefault().FechaEntrega
                dgvDatos.RefreshData();
                dgvDatos.FocusedColumn = dgvDatos.VisibleColumns[0];
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

                    EntregaEPP poObject = new EntregaEPP();

                    poObject.IdEntregaEPP = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                    poObject.IdBodega = int.Parse(cmbBodega.EditValue.ToString());
                    poObject.IdEmpleado = int.Parse(cmbEmpleado.EditValue.ToString());
                    poObject.Observaciones = txtObservaciones.Text;
                    poObject.FechaIngreso = dtpFecha.DateTime;
                    poObject.CentroCosto = cmbCentroCosto.EditValue.ToString();

                    poObject.EntregaEPPDetalle = (List<EntregaEPPDetalle>)bsDatos.DataSource;

                    int pId;

                    string psMsg = loLogicaNegocio.gsGuardar(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, out pId);

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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var psMsg = loLogicaNegocio.gsAnular(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

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

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultarMovimiento(Convert.ToInt32(txtNo.Text.Trim()));
                
                cmbBodega.EditValue = poObject.IdBodega.ToString();
                cmbEmpleado.EditValue = poObject.IdEmpleado.ToString();
                txtNo.EditValue = poObject.IdEntregaEPP;
                dtpFecha.DateTime = poObject.FechaIngreso;
                txtObservaciones.EditValue = poObject.Observaciones.ToString();
                cmbCentroCosto.EditValue = poObject.CentroCosto.ToString();

                bsDatos.DataSource = poObject.EntregaEPPDetalle;
                dgvDatos.RefreshData();

                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;

                //BarItemLink btnGrabar = bar2.ItemLinks.FirstOrDefault(c => c.Caption == "Guardar");
                //if (btnGrabar != null) btnGrabar.Item.Enabled = false;

                dgvDatos.OptionsBehavior.Editable = false;
                btnAddFila.Enabled = false;
            }
        }

        private void lConsultarEntrega()
        {
            if (lIdTransferencia != 0)
            {
                var poObject = loLogicaNegocio.goConsultarMovimiento(lIdTransferencia);

                cmbBodega.EditValue = poObject.IdBodega.ToString();
                cmbEmpleado.EditValue = poObject.IdEmpleado.ToString();
                txtNo.EditValue = poObject.IdEntregaEPP;
                dtpFecha.DateTime = poObject.FechaIngreso;
                txtObservaciones.EditValue = poObject.Observaciones.ToString();
                cmbCentroCosto.EditValue = poObject.CentroCosto.ToString();

                bsDatos.DataSource = poObject.EntregaEPPDetalle;
                dgvDatos.RefreshData();

                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Enabled = false;
                if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Enabled = false;
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Enabled = false;
                if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Enabled = false;
                if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Enabled = false;
                if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Enabled = false;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = false;
                if (tstBotones.Items["btnCopiar"] != null) tstBotones.Items["btnCopiar"].Enabled = false;
                dgvDatos.OptionsBehavior.Editable = false;
                btnAddFila.Enabled = false;
            }
        }

        private void cmbEmpleado_EditValueChanged(object sender, EventArgs e)
        {
            int idEmpleado = Convert.ToInt32(cmbEmpleado.EditValue);

            int idBodega = loLogicaNegocio.obtenerBodegaUsuario(idEmpleado);

            var idCentroCosto = loLogicaNegocio.obtenerCentroCostoUsuario(idEmpleado);

            cmbBodega.EditValue = idBodega.ToString();

            cmbCentroCosto.EditValue = idCentroCosto.ToString();

        }

        private void cmbBodega_EditValueChanged(object sender, EventArgs e)
       {
            ActualizarColumnasBodega("IdBodega", cmbBodega);
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
    }
}
