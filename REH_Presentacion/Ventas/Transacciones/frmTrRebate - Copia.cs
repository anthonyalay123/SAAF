using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
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

namespace REH_Presentacion.Ventas.Transacciones
{
    /// <summary>
    /// Formulario que permite guardar los clientes Rebate
    /// Autor: Víctor Arévalo
    /// Fecha: 21/12/2021
    /// </summary>
    public partial class frmTrRebate : frmBaseTrxDev
    {

        #region Variables
        clsNRebate loLogicaNegocio;
        RepositoryItemButtonEdit rpiBtnDel;
        BindingSource bsDatos;
        BindingSource bsRebate;
        decimal ldcPorcRebate;

        #endregion

        #region Eventos

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmTrRebate()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNRebate();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;

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
                var poLista = (List<RebateDetalleGrid>)bsDatos.DataSource;
                var poListaRebate = (List<RebateDetalleGrid>)bsRebate.DataSource;

                if (poListaRebate.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poListaRebate[piIndex];

                    poLista.Add(poEntidad);

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poListaRebate.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsRebate.DataSource = poListaRebate;
                    dgvRebate.RefreshData();

                    bsDatos.DataSource = poLista.ToList();
                    dgvDatos.RefreshData();
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

                List<RebateDetalleGrid> poLista = (List<RebateDetalleGrid>)bsRebate.DataSource;

                if (poLista.Count > 0)
                {
                    string psMsg = loLogicaNegocio.gsGuardarRebateCliente(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Evento del botón Nuevo, Generalmente Limpia el formulario 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtAnio.Text.Trim()) && cmbTrimestre.EditValue.ToString() != Diccionario.Seleccione)
                {
                    var dt = loLogicaNegocio.gdtConsultaRebateTrimestre(int.Parse(txtAnio.Text), int.Parse(cmbTrimestre.EditValue.ToString()));
                    xrptGeneral xrptGen = new xrptGeneral();
                    xrptGen.dt = dt;
                    xrptGen.Landscape = false;
                    xrptGen.Parameters["parameter1"].Value = string.Format("CLIENTES REBATE AÑO: {0}, TRIMESTRE {1}", txtAnio.Text, cmbTrimestre.EditValue);

                    xrptGen.RequestParameters = false;
                    xrptGen.Parameters["parameter1"].Visible = false;
                    using (ReportPrintTool printTool = new ReportPrintTool(xrptGen))
                    {
                        printTool.ShowRibbonPreviewDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                if (!string.IsNullOrEmpty(txtAnio.Text) && cmbTrimestre.EditValue.ToString() != Diccionario.Seleccione)
                {
                    dgvDatos.PostEditor();
                    dgvRebate.PostEditor();

                    List<RebateDetalleGrid> poLista = (List<RebateDetalleGrid>)bsDatos.DataSource;
                    if (poLista != null && poLista.Count > 0)
                    {
                        string psFilter = "Files(*.xlsx;)|*.xlsx;";
                        clsComun.gSaveFile(gcDatos, "Rebate_Preliminar_" + txtAnio.Text + "_" + cmbTrimestre.Text + ".xlsx", psFilter);
                    }

                    List<RebateDetalleGrid> poListaProvision = (List<RebateDetalleGrid>)bsRebate.DataSource;
                    if (poListaProvision != null && poListaProvision.Count > 0)
                    {
                        string psFilter = "Files(*.xlsx;)|*.xlsx;";
                        clsComun.gSaveFile(gcRebate, "Rebate_Seleccionados_" + txtAnio.Text + "_" + cmbTrimestre.Text + ".xlsx", psFilter);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void frmTrRebate_Load(object sender, EventArgs e)
        {
            try
            {

                clsComun.gLLenarCombo(ref cmbTrimestre, loLogicaNegocio.goConsultarComboTrimestres(), true);
                var poParametro = loLogicaNegocio.goConsultarParametroVta();
                ldcPorcRebate = poParametro != null ? poParametro.PorcBonificacionCumplimientoRebate: 0M;

                lCargarEventosBotones();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Métodos
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;

            /*********************************************************************************************************************************************/

            bsDatos = new BindingSource();
            bsDatos.DataSource = new List<RebateDetalleGrid>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["IdRebateClienteDetalle"].Visible = false;
            dgvDatos.Columns["Periodo"].Visible = false;
            dgvDatos.Columns["Trimestre"].Visible = false;
            dgvDatos.Columns["CodeZona"].Visible = false;
            dgvDatos.Columns["CodeCliente"].Visible = false;
            dgvDatos.Columns["Del"].Visible = false;
            dgvDatos.Columns["Generado"].Visible = false;
            //dgvDatos.Columns["Observacion"].Visible = false;
            dgvDatos.Columns["ValorRebate"].Visible = false;

            dgvDatos.OptionsBehavior.Editable = true;
            dgvDatos.Columns["NameZona"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NameCliente"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[4].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[5].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[6].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[7].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[8].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[9].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[10].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[11].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["CantFacturas"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["CantFacturasPagadas"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["DiasMora"].OptionsColumn.AllowEdit = false;

            clsComun.gFormatearColumnasGrid(dgvDatos);

            dgvDatos.FixedLineWidth = 3;
            dgvDatos.Columns[0].Fixed = FixedStyle.Left;
            dgvDatos.Columns[5].Fixed = FixedStyle.Left;
            dgvDatos.Columns[7].Fixed = FixedStyle.Left;

            /*********************************************************************************************************************************************/

            bsRebate = new BindingSource();
            bsRebate.DataSource = new List<RebateDetalleGrid>();
            gcRebate.DataSource = bsRebate;

            dgvRebate.OptionsBehavior.Editable = true;

            dgvRebate.Columns["IdRebateClienteDetalle"].Visible = false;
            dgvRebate.Columns["Periodo"].Visible = false;
            dgvRebate.Columns["Trimestre"].Visible = false;
            dgvRebate.Columns["CodeZona"].Visible = false;
            dgvRebate.Columns["CodeCliente"].Visible = false;
            //dgvRebate.Columns["Observacion"].Visible = false;
            dgvRebate.Columns["Sel"].Visible = false;
            dgvRebate.Columns["CantFacturas"].Visible = false;
            dgvRebate.Columns["CantFacturasPagadas"].Visible = false;

            dgvRebate.Columns["NameZona"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["VentaNeta"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CantFacturas"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CantFacturasPagadas"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Presupuesto"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["NameCliente"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["PorcCumplimientoMeta"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["PorcMargenRentabilidad"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["DiasMora"].OptionsColumn.AllowEdit = false;
            
            clsComun.gFormatearColumnasGrid(dgvRebate);

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvRebate.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);

            dgvRebate.FixedLineWidth = 2;
            dgvRebate.Columns[5].Fixed = FixedStyle.Left;
            dgvRebate.Columns[7].Fixed = FixedStyle.Left;

            cmbTrimestre.KeyDown += new KeyEventHandler(EnterEqualTab);
            
            txtAnio.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAnio.KeyPress += new KeyPressEventHandler(SoloNumeros);
            
            
        }

        private void lLimpiar()
        {
            //if ((cmbProducto.Properties.DataSource as IList).Count > 0) cmbProducto.ItemIndex = 0;
            //if ((cmbZona.Properties.DataSource as IList).Count > 0) cmbZona.ItemIndex = 0;
            //txtUnidades.Text = "0";
            //txtPrecio.Text = "0";
            //txtValor.Text = "0";
            //txtMedidaConversion.Text = "0";
            //txtTotal.Text = "0";
            //txtTipoProducto.Text = string.Empty;
            //txtAnio.Text = string.Empty;
            //bsDatos.DataSource = new List<PresupuestoVentasGrid>();
        }

        private void lBuscar()
        {

            Cursor.Current = Cursors.WaitCursor;

            if (!string.IsNullOrEmpty(txtAnio.Text) && cmbTrimestre.EditValue.ToString() != Diccionario.Seleccione)
            {
                
                bsDatos.DataSource = loLogicaNegocio.goConsultaPreliminarClientesRebate(int.Parse(txtAnio.Text), int.Parse(cmbTrimestre.EditValue.ToString()));
                gcDatos.DataSource = bsDatos;

                clsComun.gOrdenarColumnasGridFullEditable(dgvDatos);

                bsRebate.DataSource = loLogicaNegocio.goConsultaClientesRebate(int.Parse(txtAnio.Text), int.Parse(cmbTrimestre.EditValue.ToString()));
                gcRebate.DataSource = bsRebate;

                clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);


            }

            Cursor.Current = Cursors.Default;

        }

        #endregion

       
        private void cmbTrimestre_EditValueChanged(object sender, EventArgs e)
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

        private void txtAnio_Leave(object sender, EventArgs e)
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

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {

                List<RebateDetalleGrid> poListaPreliminar = (List<RebateDetalleGrid>)bsDatos.DataSource;
                List<RebateDetalleGrid> poListaRebate = (List<RebateDetalleGrid>)bsRebate.DataSource;

                List<RebateDetalleGrid> poListaAgregar = new List<RebateDetalleGrid>();

                foreach (var item in poListaPreliminar.Where(x=>x.Sel).ToList())
                {
                    if (poListaRebate.Where(x=> x.Periodo == item.Periodo && x.Trimestre == item.Trimestre && x.CodeZona == item.CodeZona && x.CodeCliente == item.CodeCliente).Count() == 0)
                    {
                        var poRegistro = new RebateDetalleGrid();
                        poRegistro.Periodo = int.Parse(txtAnio.Text);
                        poRegistro.Trimestre = int.Parse(cmbTrimestre.EditValue.ToString());
                        poRegistro.CodeZona = item.CodeZona;
                        poRegistro.NameZona = item.NameZona;
                        poRegistro.CodeCliente = item.CodeCliente;
                        poRegistro.NameCliente = item.NameCliente;
                        poRegistro.Presupuesto = item.Presupuesto;
                        poRegistro.VentaNeta = item.VentaNeta;
                        poRegistro.PorcCumplimientoMeta = item.PorcCumplimientoMeta;
                        poRegistro.PorcMargenRentabilidad = item.PorcMargenRentabilidad;
                        poRegistro.DiasMora = item.DiasMora;
                        poRegistro.ValorRebate = Math.Round(item.VentaNeta * ldcPorcRebate, 2);

                        poListaRebate.Add(poRegistro);

                        poListaPreliminar.Remove(item);

                        bsDatos.DataSource = poListaPreliminar;
                        dgvDatos.RefreshData();

                        bsRebate.DataSource = poListaRebate;
                        dgvRebate.RefreshData();

                        clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);
                    }

                }

                //if (poLista.Count > 0 && piIndex >= 0)
                //{
                //    // Tomamos la entidad de la fila seleccionada
                //    var poEntidad = poLista[piIndex];

                //    // Eliminar Fila seleccionada de mi lista
                //    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                //    poLista.RemoveAt(piIndex);

                //    // Asigno mi nueva lista al Binding Source
                //    bsDatos.DataSource = poLista;
                //    dgvParametrizaciones.RefreshData();
                //}


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
