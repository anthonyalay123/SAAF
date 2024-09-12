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
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VTA_Negocio;

namespace REH_Presentacion.Compras.Transaccion
{

    public partial class frmTrBandejaFactPendPagoFinanciero : frmBaseTrxDev
    {

        #region Variables
        clsNOrdenPago loLogicaNegocio;
        RepositoryItemButtonEdit rpiBtnDel;
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();
        BindingSource bsDatos;
        BindingSource bsRebate;
        bool pbCargado = false;
        List<int> lIdEliminar = new List<int>();

        #endregion

        #region Eventos

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmTrBandejaFactPendPagoFinanciero()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNOrdenPago();
            rpiBtnDel = new RepositoryItemButtonEdit();
            //rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiMedDescripcion.WordWrap = true;

        }

        ///// <summary>
        ///// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        //{
        //    try
        //    {
        //        int piIndex = dgvRebate.GetFocusedDataSourceRowIndex();
        //        var poLista = (List<FacturaAprobadasDetalleGrid>)bsRebate.DataSource;
        //        if (piIndex >= 0)
        //        {
        //            var poListaCuentas = loLogicaNegocio.goConsultarCuentasProveedor(poLista[piIndex].Identificacion);
        //            var poListaObject = poListaCuentas;
        //            DataTable dt = new DataTable();

        //            dt.Columns.AddRange(new DataColumn[]
        //                                {
        //                                    new DataColumn("Id"),
        //                                    new DataColumn("Nombre"),
        //                                    new DataColumn("Banco"),
        //                                    new DataColumn("Tipo Cuenta"),
        //                                    new DataColumn("Cuenta")
        //                                });

        //            poListaObject.ForEach(a =>
        //            {
        //                DataRow row = dt.NewRow();
        //                row["Id"] = a.IdProveedorCuenta;
        //                row["Nombre"] = a.Nombre;
        //                row["Banco"] = a.DesBanco;
        //                row["Tipo Cuenta"] = a.DesTipoCuentaBancaria;
        //                row["Cuenta"] = a.NumeroCuenta;

        //                dt.Rows.Add(row);
        //            });

        //            frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Cuentas Bancarias del Proveedor" };
        //            if (pofrmBuscar.ShowDialog() == DialogResult.OK)
        //            {
        //                int pId = int.Parse(pofrmBuscar.lsCodigoSeleccionado);
        //                var poRegistro = poListaCuentas.Where(x => x.IdProveedorCuenta == pId).FirstOrDefault();
        //                if (poRegistro != null)
        //                {
        //                    poLista[piIndex].CodigoBanco = poRegistro.CodigoBanco;
        //                    poLista[piIndex].CodigoFormaPago = poRegistro.CodigoFormaPago;
        //                    poLista[piIndex].CodigoTipoCuentaBancaria = poRegistro.CodigoTipoCuentaBancaria;
        //                    poLista[piIndex].CodigoTipoIdentificacion = poRegistro.CodigoTipoIdentificacion;
        //                    poLista[piIndex].IdentificacionCuenta = poRegistro.Identificacion;
        //                    poLista[piIndex].Nombre = poRegistro.Nombre;
        //                    poLista[piIndex].NumeroCuenta = poRegistro.NumeroCuenta;

        //                    dgvRebate.RefreshData();

        //                    clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        /// <summary>
        /// Evento del botón Exportar, Exporta a TXT.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerarPagos_Click(object sender, EventArgs e)
        {
            try
            {
                dgvRebate.PostEditor();
                
                var poLista = ((List<FacturaAprobadasDetalleGrid>)bsRebate.DataSource).Where(x=>x.Sel).ToList();
                if (poLista.Count > 0)
                {
                    var psMsg = loLogicaNegocio.goGeneraPagoFinanciero(poLista,clsPrincipal.gsUsuario);

                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show("Guardado Exitosamente", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lBuscar();
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<FacturaAprobadasDetalleGrid> poLista = (List<FacturaAprobadasDetalleGrid>)bsDatos.DataSource;
                if (poLista != null && poLista.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcRebate, Text + ".xlsx", psFilter);
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
                lBuscar();
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
                lCargarEventosBotones();
                clsComun.gLLenarCombo(ref cmbGrupoPago, loLogicaNegocio.goConsultarComboFacturaGrupoPagoAprobadosFinanciero(), false, true);

                clsComun.gLLenarComboGrid(ref dgvRebate, loLogicaNegocio.goConsultarComboEstado(), "CodigoEstado");
                clsComun.gLLenarComboGrid(ref dgvRebate, loLogicaNegocio.goConsultarComboBanco(), "CodigoBanco", true);
                clsComun.gLLenarComboGrid(ref dgvRebate, loLogicaNegocio.goConsultarComboFormaPago(), "CodigoFormaPago", true);
                clsComun.gLLenarComboGrid(ref dgvRebate, loLogicaNegocio.goConsultarComboTipoIdentificación(), "CodigoTipoIdentificacion", true);
                clsComun.gLLenarComboGrid(ref dgvRebate, loLogicaNegocio.goConsultarComboTipoCuentaBancaria(), "CodigoTipoCuentaBancaria", true);
                pbCargado = true;
                lBuscar();
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
            
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnGenerarPagos"] != null) tstBotones.Items["btnGenerarPagos"].Click += btnGenerarPagos_Click;


            /*********************************************************************************************************************************************/

            /*********************************************************************************************************************************************/

            bsRebate = new BindingSource();
            bsRebate.DataSource = new List<FacturaAprobadasDetalleGrid>();
            gcRebate.DataSource = bsRebate;

            dgvRebate.OptionsBehavior.Editable = true;
            //dgvRebate.OptionsPrint.AutoWidth = false;
            //dgvRebate.OptionsView.RowAutoHeight = true;

            dgvRebate.Columns["VerOP"].Visible = false;
            dgvRebate.Columns["IdGrupoPago"].Visible = false;
            dgvRebate.Columns["Det"].Visible = false;
            dgvRebate.Columns["IdOrdenPagoFactura"].Visible = false;

            dgvRebate.Columns["IdOrdenPagoFactura"].Visible = false;
            dgvRebate.Columns["IdFacturaPago"].Visible = false;
            dgvRebate.Columns["Identificacion"].Visible = false;
            //dgvRebate.Columns["Sel"].Visible = false;
            dgvRebate.Columns["Generado"].Visible = false;
            dgvRebate.Columns["Del"].Visible = false;
            dgvRebate.Columns["IdOrdenPago"].Visible = false;
            dgvRebate.Columns["Valor"].Visible = false;
            dgvRebate.Columns["Abono"].Visible = false;
            dgvRebate.Columns["Saldo"].Visible = false;
            dgvRebate.Columns["FechaEmision"].Visible = false;
            dgvRebate.Columns["Ver"].Visible = false;
            dgvRebate.Columns["Aprobaciones"].Visible = false;
            dgvRebate.Columns["Aprobo"].Visible = false;
            dgvRebate.Columns["DocNum"].Visible = false;
            dgvRebate.Columns["CodigoTipoIdentificacion"].Visible = false;
            dgvRebate.Columns["IdentificacionCuenta"].Visible = false;
            dgvRebate.Columns["Nombre"].Visible = false;
            dgvRebate.Columns["CodigoBanco"].Visible = false;
            dgvRebate.Columns["CodigoFormaPago"].Visible = false;
            dgvRebate.Columns["CodigoTipoCuentaBancaria"].Visible = false;
            dgvRebate.Columns["NumeroCuenta"].Visible = false;
            dgvRebate.Columns["IdProveedor"].Visible = false;

            dgvRebate.Columns["FechaPago"].Visible = false;
            dgvRebate.Columns["IdSemanaPago"].Visible = false;
            dgvRebate.Columns["Add"].Visible = false;
            dgvRebate.Columns["ArchivoAdjunto"].Visible = false;
            dgvRebate.Columns["RutaOrigen"].Visible = false;
            dgvRebate.Columns["RutaDestino"].Visible = false;
            dgvRebate.Columns["NombreOriginal"].Visible = false;
            dgvRebate.Columns["Descargar"].Visible = false;
            dgvRebate.Columns["Visualizar"].Visible = false;

            dgvRebate.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvRebate.Columns["Proveedor"].ColumnEdit = rpiMedDescripcion;
            dgvRebate.Columns["Aprobo"].ColumnEdit = rpiMedDescripcion;

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
            dgvRebate.Columns["ValorPago"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["ValorPagoOriginal"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["ComentarioAprobador"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Aprobaciones"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Aprobo"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CodigoTipoIdentificacion"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["IdentificacionCuenta"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Nombre"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CodigoBanco"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CodigoFormaPago"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CodigoTipoCuentaBancaria"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["NumeroCuenta"].OptionsColumn.AllowEdit = false;

            dgvRebate.Columns["ValorPago"].Caption = "Valor a Pagar";
            dgvRebate.Columns["Ver"].Caption = "Seleccionar Cuenta";
            dgvRebate.Columns["NumDocumento"].Caption = "# Factura";
            dgvRebate.Columns["CodigoEstado"].Caption = "Estado";
            dgvRebate.Columns["Aprobo"].Caption = "Aprobó";
            dgvRebate.Columns["CodigoTipoIdentificacion"].Caption = "Tipo Identifiación";
            dgvRebate.Columns["IdentificacionCuenta"].Caption = "Identificación";
            dgvRebate.Columns["Nombre"].Caption = "Nombre";
            dgvRebate.Columns["CodigoBanco"].Caption = "Banco";
            dgvRebate.Columns["CodigoFormaPago"].Caption = "Forma Pago";
            dgvRebate.Columns["CodigoTipoCuentaBancaria"].Caption = "Tipo Cuenta";
            dgvRebate.Columns["NumeroCuenta"].Caption = "Cuenta";

            dgvRebate.FixedLineWidth = 3;
            dgvRebate.Columns["Sel"].Fixed = FixedStyle.Left;
            dgvRebate.Columns["Proveedor"].Fixed = FixedStyle.Left;
            dgvRebate.Columns["NumDocumento"].Fixed = FixedStyle.Left;
            //dgvRebate.Columns["Ver"].Fixed = FixedStyle.Right;

            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvRebate.Columns["Ver"], "Ver", Diccionario.ButtonGridImage.show_16x16);

            clsComun.gFormatearColumnasGrid(dgvRebate);
            clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);

            dtpFechaInicio.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaFin.EditValue = DateTime.Now;


        }

        private void lLimpiar()
        {

            bsDatos.DataSource = new List<FacturaAprobadasDetalleGrid>();
            bsRebate.DataSource = new List<FacturaAprobadasDetalleGrid>();
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

            bsRebate.DataSource = loLogicaNegocio.goConsultaFacturasPendientePagoFinanciero(cmbGrupoPago.EditValue.ToString());
            gcRebate.DataSource = bsRebate;

            clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);

            lIdEliminar = new List<int>();

            Cursor.Current = Cursors.Default;

        }

        #endregion

      
        private void lCalcularTotal()
        {
            var pdcTotal = ((List<FacturaAprobadasDetalleGrid>)bsRebate.DataSource).Where(x => x.Sel).Sum(x => x.ValorPago);
            lblTotal.Text = pdcTotal.ToString("c2");
        }

        private void dgvRebate_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Sel")
                {
                    dgvRebate.PostEditor();
                    lCalcularTotal();
                }
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
                List<FacturaAprobadasDetalleGrid> poLista = (List<FacturaAprobadasDetalleGrid>)bsRebate.DataSource;

                var sel = poLista.Select(x => x.Sel).FirstOrDefault();
                foreach (var item in poLista)
                {
                    item.Sel = !sel;
                }


                bsRebate.DataSource = poLista;
                dgvRebate.RefreshData();
                lCalcularTotal();
                //clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbGrupoPago_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lBuscar();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
}
