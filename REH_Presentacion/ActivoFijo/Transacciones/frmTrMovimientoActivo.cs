using AFI_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.ActivoFijo;
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

namespace REH_Presentacion.ActivoFijo.Transacciones
{
    public partial class frmTrMovimientoActivo : frmBaseTrxDev
    {
        clsNActivoFijo loLogicaNegocio = new clsNActivoFijo();
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();
        BindingSource bsDatos = new BindingSource();

        bool pbCargado = false;

        public frmTrMovimientoActivo()
        {
            InitializeComponent();
            rpiMedDescripcion.WordWrap = true;
        }

        private void frmTrMovimientoActivo_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                clsComun.gLLenarCombo(ref cmbActivo, loLogicaNegocio.goConsultarComboActivoFijo(), true);
                clsComun.gLLenarCombo(ref cmbMovimiento, loLogicaNegocio.goConsultarComboTipoMovimientoActivo(), true);
                clsComun.gLLenarCombo(ref cmbEstadoActivo, loLogicaNegocio.goConsultarComboEstadoActivoFijo(), true);
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                if (tstBotones.Items["btnReversar"] != null) tstBotones.Items["btnReversar"].Enabled = false;
                if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Enabled = true;
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                if (tstBotones.Items["btnGenerarDiario"] != null) tstBotones.Items["btnGenerarDiario"].Enabled = false;
                pbCargado = true;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void glueActivo_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnReversar"] != null) tstBotones.Items["btnReversar"].Click += btnReversar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnReversar_Click;
            if (tstBotones.Items["btnGenerarDiario"] != null) tstBotones.Items["btnGenerarDiario"].Click += btnGenerarDiario_Click;
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

        private void lLimpiar()
        {
            pbCargado = false;
            if ((cmbActivo.Properties.DataSource as IList).Count > 0) cmbActivo.ItemIndex = 0;
            if ((cmbMovimiento.Properties.DataSource as IList).Count > 0) cmbMovimiento.ItemIndex = 0;
            cmbEstadoActivo.EditValue = null;
            dtpFechaMovimiento.DateTime = DateTime.MinValue;
            txtCostoCompra.EditValue = 0;
            txtCostoActual.EditValue = 0;
            txtDepreciacionAcumulada.EditValue = 0;
            txtValorDepreciable.EditValue = 0;
            txtNo.EditValue = "";
            txtObservacion.EditValue = "";
            txtValorResidual.EditValue = 0;
            txtValorVenta.EditValue = 0;
            txtObservacion.Text = "";
            lblEstado.Text = "";
            txtActivo.EditValue = "";
            txtIdActivo.EditValue = "";

            txtObservacion.ReadOnly = false;
            cmbMovimiento.ReadOnly = false;
            dtpFechaMovimiento.ReadOnly = false;
            cmbActivo.ReadOnly = false;
            btnBuscar.Enabled = true;

            if (tstBotones.Items["btnReversar"] != null) tstBotones.Items["btnReversar"].Enabled = false;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
            if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Enabled = true;
            if (tstBotones.Items["btnGenerarDiario"] != null) tstBotones.Items["btnGenerarDiario"].Enabled = false;

            pbCargado = true;
        }

        /// <summary>
        /// Evento del botón Grabar, Guarda datos del formulario a el objeto de BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {

                MovimientoActivoFijo poObject = new MovimientoActivoFijo();
                poObject.IdMovimientoActivoFijo = string.IsNullOrEmpty(txtNo.Text) ? 0 : Convert.ToInt32(txtNo.Text);
                poObject.FechaMovimiento = dtpFechaMovimiento.DateTime;
                poObject.Observacion = txtObservacion.Text;
                poObject.IdItemActivoFijo = string.IsNullOrEmpty(txtIdActivo.Text) ? 0 : int.Parse(txtIdActivo.Text);
                poObject.ActivoFijo = txtActivo.Text;
                poObject.CostoCompra = Convert.ToDecimal(txtCostoCompra.EditValue);
                poObject.ActivoFijo = txtActivo.Text;
                poObject.ValorResidual = Convert.ToDecimal(txtValorResidual.EditValue);
                poObject.ValorDepreciable = Convert.ToDecimal(txtValorDepreciable.EditValue);
                poObject.DepreciacionAcumulada = Convert.ToDecimal(txtDepreciacionAcumulada.EditValue);
                poObject.CostoActual = Convert.ToDecimal(txtCostoActual.EditValue);
                poObject.CodigoTipoMovimiento = cmbMovimiento.EditValue.ToString();
                if (txtValorVenta.Enabled)
                {
                    poObject.ValorVenta = string.IsNullOrEmpty(txtValorVenta.EditValue.ToString()) ? 0 : Convert.ToDecimal(txtValorVenta.EditValue);
                }
                else
                {
                    poObject.ValorVenta = null;
                }

                DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de guardar los datos?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    string psMsg = loLogicaNegocio.gsGuardarMovimientoActivoFijo(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void btnGenerarDiario_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text))
                {
                    GridControl gc = new GridControl();
                    BindingSource bs = new BindingSource();
                    GridView dgv = new GridView();

                    gc.DataSource = bs;
                    gc.MainView = dgv;
                    gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                    dgv.GridControl = gc;
                    this.Controls.Add(gc);
                    bs.DataSource = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC AFISPGCDIARIOMOVIMIENTO {0}", txtNo.Text));
                    clsComun.gOrdenarColumnasGridFullEditableNone(dgv);
                    clsComun.gFormatearColumnasGrid(dgv);
                    //dgv.BestFitColumns();

                    //dgv.PopulateColumns();
                    //dgv.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
                    // Exportar Datos
                    clsComun.gSaveFile(gc, "Diario_Movimiento_" + txtNo.Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");

                    gc.Visible = false;
                }
                else
                {
                    XtraMessageBox.Show("No existe movimiento para generar diario", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReversar_Click(object sender, EventArgs e)
        {
            try
            {
                SendKeys.Send("{TAB}");

                if (!string.IsNullOrEmpty(txtNo.Text))
                {
                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de reversar movimiento?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var psMsg = loLogicaNegocio.gEliminarMovimientoActivoFijo(int.Parse(txtNo.Text), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show("Movimiento Reversado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existe movimiento para reversar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lConsultarActivo()
        {
            try
            {
                if (pbCargado)
                {

                    var poObjectMov = loLogicaNegocio.goConsultaMovimientoItemActivoFijo(int.Parse(txtIdActivo.EditValue.ToString()));
                    if (poObjectMov != null)
                    {
                        txtNo.EditValue = poObjectMov.IdMovimientoActivoFijo;
                        lConsultar();
                    }
                    else
                    {
                        txtNo.EditValue = "";
                        txtValorVenta.EditValue = "";
                        txtObservacion.Text = "";
                        var poObjectAct = loLogicaNegocio.goBuscarItemActivoFijo(int.Parse(txtIdActivo.EditValue.ToString()));
                        if (poObjectAct != null)
                        {
                            txtCostoCompra.EditValue = poObjectAct.CostoCompra;
                            txtValorResidual.EditValue = poObjectAct.ValorResidual;
                            txtValorDepreciable.EditValue = poObjectAct.ValorDepreciable;
                            txtDepreciacionAcumulada.EditValue = poObjectAct.DepreciacionAcumulada;
                            txtCostoActual.EditValue = poObjectAct.CostoActual;
                            cmbEstadoActivo.EditValue = poObjectAct.CodigoEstadoActivoFijo;
                            txtActivo.Text = poObjectAct.Descripcion;
                        }
                        else
                        {
                            txtCostoCompra.EditValue = "";
                            txtValorResidual.EditValue = "";
                            txtValorDepreciable.EditValue = "";
                            txtDepreciacionAcumulada.EditValue = "";
                            txtCostoActual.EditValue = "";
                            dtpFechaMovimiento.DateTime = DateTime.Now;
                            cmbEstadoActivo.EditValue = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbActivo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    
                    var poObjectMov = loLogicaNegocio.goConsultaMovimientoItemActivoFijo(int.Parse(cmbActivo.EditValue.ToString()));
                    if (poObjectMov != null)
                    {
                        txtNo.EditValue = poObjectMov.IdMovimientoActivoFijo;
                        lConsultar();
                    }
                    else
                    {
                        txtNo.EditValue = "";
                        txtValorVenta.EditValue = "";
                        txtObservacion.Text = "";
                        var poObjectAct = loLogicaNegocio.goBuscarItemActivoFijo(int.Parse(cmbActivo.EditValue.ToString()));
                        if (poObjectAct != null)
                        {
                            txtCostoCompra.EditValue = poObjectAct.CostoCompra;
                            txtValorResidual.EditValue = poObjectAct.ValorResidual;
                            txtValorDepreciable.EditValue = poObjectAct.ValorDepreciable;
                            txtDepreciacionAcumulada.EditValue = poObjectAct.DepreciacionAcumulada;
                            txtCostoActual.EditValue = poObjectAct.CostoActual;
                            cmbEstadoActivo.EditValue = poObjectAct.CodigoEstadoActivoFijo;
                        }
                        else
                        {
                            txtCostoCompra.EditValue = "";
                            txtValorResidual.EditValue = "";
                            txtValorDepreciable.EditValue = "";
                            txtDepreciacionAcumulada.EditValue = "";
                            txtCostoActual.EditValue = "";
                            dtpFechaMovimiento.DateTime = DateTime.Now;
                            cmbEstadoActivo.EditValue = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var poListaObject = loLogicaNegocio.goListarMovimientoActivoFijo();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Id"),
                                    new DataColumn("Movimiento"),
                                    new DataColumn("Código"),
                                    new DataColumn("Activo"),
                                    new DataColumn("Estado"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Id"] = a.IdMovimientoActivoFijo;
                    row["Movimiento"] = a.TipoMovimiento;
                    row["Código"] = a.CodigoActivoFijo;
                    row["Activo"] = a.ActivoFijo;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);

                    dt.Rows.Add(row);
                });

                List<GridBusqueda> poListaGrid = new List<GridBusqueda>();
                poListaGrid.Add(new GridBusqueda() { Columna = "Activo", Ancho = 150 });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt, poListaGrid) { Text = "Listado de Ítems" };
                pofrmBuscar.Width = 1200;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar();
                }

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
                var poObject = loLogicaNegocio.goConsultaMovimientoActivoFijo(int.Parse(txtNo.Text));
                if (poObject != null)
                {
                    pbCargado = false;
                    txtIdActivo.EditValue = poObject.IdItemActivoFijo.ToString();
                    txtActivo.EditValue = poObject.ActivoFijo;
                    var poObjectAct = loLogicaNegocio.goBuscarItemActivoFijo(int.Parse(txtIdActivo.EditValue.ToString()));
                    if (poObjectAct != null)
                    {
                        cmbEstadoActivo.EditValue = poObjectAct.CodigoEstadoActivoFijo;
                    }
                    else
                    {
                        cmbEstadoActivo.EditValue = Diccionario.Seleccione;
                    }
                    cmbMovimiento.EditValue = poObject.CodigoTipoMovimiento;
                    txtCostoCompra.EditValue = poObject.CostoCompra;
                    txtValorResidual.EditValue = poObject.ValorResidual;
                    txtValorDepreciable.EditValue = poObject.ValorDepreciable;
                    txtDepreciacionAcumulada.EditValue = poObject.DepreciacionAcumulada;
                    txtCostoActual.EditValue = poObject.CostoActual;
                    txtValorVenta.EditValue = poObject.ValorVenta;
                    dtpFechaMovimiento.DateTime = poObject.FechaMovimiento;
                    txtObservacion.Text = poObject.Observacion;
                    lblEstado.Text = Diccionario.gsGetDescripcion(poObject.CodigoEstado);

                    txtObservacion.ReadOnly = true;
                    cmbMovimiento.ReadOnly = true;
                    dtpFechaMovimiento.ReadOnly = true;
                    cmbActivo.ReadOnly = true;
                    btnBuscar.Enabled = false;

                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
                    if (tstBotones.Items["btnReversar"] != null) tstBotones.Items["btnReversar"].Enabled = true;
                    if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Enabled = false;
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                    if (tstBotones.Items["btnGenerarDiario"] != null) tstBotones.Items["btnGenerarDiario"].Enabled = true;

                    pbCargado = true;
                }
            }
        }

        private void cmbMovimiento_EditValueChanged(object sender, EventArgs e)
        {
            if (pbCargado)
            {
                if (cmbMovimiento.EditValue.ToString() == "VTA")
                {
                    txtValorVenta.Enabled = true;
                }
                else
                {
                    txtValorVenta.Enabled = false;
                }
            }
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            try
            {
                var poLista = loLogicaNegocio.goListarItemActivoFijo();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Codigo"),
                                    new DataColumn("Descripcion"),
                                    //new DataColumn("Estado"),
                                    });

                poLista.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Codigo"] = a.IdItemActivoFijo.ToString();
                    row["Descripcion"] = a.Descripcion;
                    dt.Rows.Add(row);
                });

                List<GridBusqueda> poListaGrid = new List<GridBusqueda>();
                poListaGrid.Add(new GridBusqueda() { Columna = "Descripcion", Ancho = 350 });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt, poListaGrid) { Text = "Listado de Ítems" };
                pofrmBuscar.Width = 800;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    txtIdActivo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    txtActivo.Text = pofrmBuscar.lsSegundaColumna;
                    lConsultarActivo();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
