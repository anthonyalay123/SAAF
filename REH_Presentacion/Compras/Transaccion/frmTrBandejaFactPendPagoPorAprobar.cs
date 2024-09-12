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
using System.Collections;
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
    public partial class frmTrBandejaFactPendPagoPorAprobar : frmBaseTrxDev
    {

        #region Variables
        clsNOrdenPago loLogicaNegocio;
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();
        BindingSource bsRebate;
        List<int> lIdEliminar = new List<int>();
        private decimal customSum = 0;
        private decimal customSumGroup = 0;
        private bool pbCargado = false;

        #endregion

        #region Eventos

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmTrBandejaFactPendPagoPorAprobar()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNOrdenPago();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnShowComentarios.ButtonClick += rpiBtnShowComentarios_ButtonClick;
            rpiMedDescripcion.WordWrap = true;
            rpiMedDescripcion.MaxLength = 100;

        }

        private void frmTrRebate_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                clsComun.gLLenarComboGrid(ref dgvRebate, loLogicaNegocio.goConsultarComboEstado(), "CodigoEstado");
                clsComun.gLLenarCombo(ref cmbGrupoPago, loLogicaNegocio.goConsultarComboFacturaGrupoPagoSinAprobados(),false, true);
                lBuscar();
                lCalcularTotal();
                pbCargado = true;
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
                int piIndex = dgvRebate.GetFocusedDataSourceRowIndex();
                var poLista = (List<FacturaPorAprobarDetalleGrid>)bsRebate.DataSource;
                if (piIndex >= 0)
                {

                    string psForma = Diccionario.Tablas.Menu.OrdenPago;
                    var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                    if (poForm != null)
                    {
                        if (poLista[piIndex].IdOrdenPago != 0)
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
                            XtraMessageBox.Show("No existe Orden de Pago para este documento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        
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
        /// Evento del boton de Visulizar en el Grid, comentarios de las aprobaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShowComentarios_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvRebate.GetFocusedDataSourceRowIndex();
                var poLista = (List<FacturaPorAprobarDetalleGrid>)bsRebate.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.FacturaPago, poLista[piIndex].IdFacturaPago);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Aprobadores con sus comentarios" };
                    if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                    {

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
        /// Evento del boton Grabar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvRebate.PostEditor();
                //dgvRebate.RefreshData();

                var poLista = (List<FacturaPorAprobarDetalleGrid>)bsRebate.DataSource;
                poLista.ForEach(x => x.Sel = false);

                for (int i = 0; i < dgvRebate.SelectedRowsCount; i++)
                {
                    int pIdSel = dgvRebate.GetSelectedRows()[i];
                    //int pId = int.Parse(dgvRebate.GetRowCellValue(i, "IdFacturaPago").ToString());
                    //var asd = poLista.Where(x => x.IdFacturaPago == pId).FirstOrDefault();
                    if (pIdSel >= 0)
                    {
                        poLista[pIdSel].Sel = true;
                    }
                    
                }
                
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
                        bool pbRechazaNoSel = false;
                        DialogResult dialogResult2 = XtraMessageBox.Show("¿Desea rechazar registros no seleccionados?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult2 == DialogResult.Yes)
                        {
                            pbRechazaNoSel = true;
                        }

                        string IdNoActualizadas = "";
                        for (int i = 0; i < poLista.Count; i++)
                        {
                            if (poLista[i].Sel == true)
                            {

                                var psMess = loLogicaNegocio.gsAprobarFacturaPago(poLista[i].IdFacturaPago, poLista[i].Comentario ,clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, int.Parse(Tag.ToString().Split(',')[0]));

                                if (psMess != "")
                                {
                                    IdNoActualizadas += "Factura No. " + poLista[i].NumDocumento + ": " + psMess + "\n";
                                }
                            }
                            else
                            {
                                if (pbRechazaNoSel)
                                {
                                    loLogicaNegocio.gsRechazarFacturaPago(poLista[i].IdFacturaPago, poLista[i].Comentario, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                                }
                            }
                            //lBuscar();
                        }
                        lBuscar();
                        if (IdNoActualizadas != "")
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

        /// <summary>
        /// Evento del boton aprobación definitiva
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAprobacionDefinitiva_Click(object sender, EventArgs e)
        {
            try
            {
                dgvRebate.PostEditor();
                //dgvRebate.RefreshData();

                var poLista = (List<FacturaPorAprobarDetalleGrid>)bsRebate.DataSource;
                poLista.ForEach(x => x.Sel = false);

                for (int i = 0; i < dgvRebate.SelectedRowsCount; i++)
                {
                    int pIdSel = dgvRebate.GetSelectedRows()[i];
                    if (pIdSel >= 0)
                    {
                        poLista[pIdSel].Sel = true;
                    }

                }

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
                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de guardar una aprobación definitiva?", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                       
                        string IdNoActualizadas = "";
                        for (int i = 0; i < poLista.Count; i++)
                        {
                            if (poLista[i].Sel == true)
                            {

                                var psMess = loLogicaNegocio.gsAprobacionDefinitiva(poLista[i].IdFacturaPago, poLista[i].Comentario, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                if (psMess != "")
                                {
                                    IdNoActualizadas += "Factura No. " + poLista[i].NumDocumento + ": " + psMess + "\n";
                                }
                            }
                            
                            //lBuscar();
                        }
                        lBuscar();
                        if (IdNoActualizadas != "")
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

        /// <summary>
        /// Evento del boton Desaprobar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDesAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvRebate.PostEditor();
                
                var poLista = (List<FacturaPorAprobarDetalleGrid>)bsRebate.DataSource;
                poLista.ForEach(x => x.Sel = false);

                for (int i = 0; i < dgvRebate.SelectedRowsCount; i++)
                {
                    int pIdSel = dgvRebate.GetSelectedRows()[i];
                    if (pIdSel >= 0)
                    {
                        poLista[pIdSel].Sel = true;
                    }
                }

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
                            if (poLista[i].Sel == true)
                            {

                                var psMess = loLogicaNegocio.gsDesaprobarFacturaPago(poLista[i].IdFacturaPago, poLista[i].Comentario, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                if (psMess != "")
                                {
                                    IdNoActualizadas += "Factura No. " + poLista[i].NumDocumento + ": " + psMess + "\n";
                                }
                            }
                            //lBuscar();
                        }
                        lBuscar();
                        if (IdNoActualizadas != "")
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


        /// <summary>
        /// Evento del botón Nuevo, Generalmente Limpia el formulario 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!string.IsNullOrEmpty(txtAnio.Text.Trim()) && cmbTrimestre.EditValue.ToString() != Diccionario.Seleccione)
                //{
                //    var dt = loLogicaNegocio.gdtConsultaRebateTrimestre(int.Parse(txtAnio.Text), int.Parse(cmbTrimestre.EditValue.ToString()));
                //    xrptGeneral xrptGen = new xrptGeneral();
                //    xrptGen.dt = dt;
                //    xrptGen.Landscape = false;
                //    xrptGen.Parameters["parameter1"].Value = string.Format("CLIENTES REBATE AÑO: {0}, TRIMESTRE {1}", txtAnio.Text, cmbTrimestre.EditValue);

                //    xrptGen.RequestParameters = false;
                //    xrptGen.Parameters["parameter1"].Visible = false;
                //    using (ReportPrintTool printTool = new ReportPrintTool(xrptGen))
                //    {
                //        printTool.ShowRibbonPreviewDialog();
                //    }
                //}
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

                GridControl gc = new GridControl();
                BindingSource bs = new BindingSource();
                GridView dgv = new GridView();
                List<FacturaPorAprobarDetalleGridExcel> listbd = new List<FacturaPorAprobarDetalleGridExcel>();

                var poLista = (List<FacturaPorAprobarDetalleGrid>)bsRebate.DataSource;
                if (poLista.Count > 0)
                {
                    foreach (var po in poLista)
                    {
                        FacturaPorAprobarDetalleGridExcel be = new FacturaPorAprobarDetalleGridExcel();
                        
                        be.UsuarioCreaOP = po.UsuarioCreaOP;
                        be.UsuarioApruebaOP = po.UsuarioApruebaOP;
                        be.Identificacion = po.Identificacion;
                        be.Proveedor = po.Proveedor;
                        be.NumDocumento = po.NumDocumento;
                        be.DocNum = po.DocNum;
                        be.FechaEmision = po.FechaEmision;
                        be.FechaVencimiento = po.FechaVencimiento;
                        be.ValorPago = po.ValorPago;
                        be.Observacion = po.Observacion;


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

        private void btnRechazar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvRebate.PostEditor();

                var poLista = (List<FacturaPorAprobarDetalleGrid>)bsRebate.DataSource;
                poLista.ForEach(x => x.Sel = false);

                for (int i = 0; i < dgvRebate.SelectedRowsCount; i++)
                {
                    int pIdSel = dgvRebate.GetSelectedRows()[i];
                    if (pIdSel >= 0)
                    {
                        poLista[pIdSel].Sel = true;
                    }
                    
                }

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
                        var result = XtraInputBox.Show("Ingrese comentario", "Rechazar", "");
                        if (string.IsNullOrEmpty(result))
                        {
                            XtraMessageBox.Show("Debe agregar comentario para poder rechazar", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string IdNoActualizadas = "";
                        for (int i = 0; i < poLista.Count; i++)
                        {
                            if (poLista[i].Sel == true)
                            {

                                var psMess = loLogicaNegocio.gsRechazarFacturaPago(poLista[i].IdFacturaPago, result, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                if (psMess != "")
                                {
                                    IdNoActualizadas += "Factura No. " + poLista[i].NumDocumento + ": " + psMess + "\n";
                                }
                            }
                            //lBuscar();
                        }
                        lBuscar();
                        if (IdNoActualizadas != "")
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
                dgvRebate.PostEditor();

                var poLista = (List<FacturaPorAprobarDetalleGrid>)bsRebate.DataSource;
                poLista.ForEach(x => x.Sel = false);

                for (int i = 0; i < dgvRebate.SelectedRowsCount; i++)
                {
                    int pIdSel = dgvRebate.GetSelectedRows()[i];
                    if (pIdSel >= 0)
                    {
                        poLista[pIdSel].Sel = true;
                    }
                }

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
                        var result = XtraInputBox.Show("Ingrese comentario", "Corregir", "");
                        if (string.IsNullOrEmpty(result))
                        {
                            XtraMessageBox.Show("Debe agregar comentario para poder corregir", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        string IdNoActualizadas = "";
                        for (int i = 0; i < poLista.Count; i++)
                        {
                            if (poLista[i].Sel == true)
                            {

                                var psMess = loLogicaNegocio.gsCorregirFacturaPago(poLista[i].IdFacturaPago, result, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                if (psMess != "")
                                {
                                    IdNoActualizadas += "Factura No. " + poLista[i].NumDocumento + ": " + psMess + "\n";
                                }
                            }
                            lBuscar();
                        }

                        if (IdNoActualizadas != "")
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

       

        //void Grid_CheckedChanged(object sender, EventArgs e)
        //{
        //    dgvRebate.PostEditor();
        //    dgvRebate.UpdateSummary();
        //}

        //void Grid_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        //{
        //    GridView view = sender as GridView;
        //    // Get the summary ID. 
        //    //decimal summaryID = Convert.ToDecimal((e.Item as GridSummaryItem).Tag);

        //    // Initialization. 
        //    if (e.SummaryProcess == CustomSummaryProcess.Start)
        //    {
        //        customSum = 0;
        //        customSumGroup = 0;
        //    }

        //    // Calculation.
        //    if (e.SummaryProcess == CustomSummaryProcess.Calculate)
        //    {
        //        if ((bool)dgvRebate.GetRowCellValue(e.RowHandle, "Sel"))
        //        {
        //            customSum += decimal.Parse(e.FieldValue.ToString());
        //        }
        //    }
        //    if (e.SummaryProcess == CustomSummaryProcess.Finalize)
        //    {
        //        e.TotalValue = customSum;
        //    }
        //}

        void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            dgvRebate.UpdateSummary();
        }

        void gridView1_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            GridView view = sender as GridView;

            //Get the summary ID. 
            int summaryID = Convert.ToInt32((e.Item as GridSummaryItem).Tag);

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                customSum = 0;
                customSumGroup = 0;
            }

            // Calculation.
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {
                if (dgvRebate.IsRowSelected(e.RowHandle))
                {
                    //customSum += decimal.Parse(e.FieldValue.ToString());

                    switch (summaryID)
                    {
                        case 1: // The total summary calculated against the 'UnitPrice' column. 
                                //int unitsInStock = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "UnitsInStock"));
                            customSum += decimal.Parse(e.FieldValue.ToString());
                            break;
                        case 2: // The group summary. 
                                //Boolean isDiscontinued = Convert.ToBoolean(e.FieldValue);
                            customSumGroup += decimal.Parse(e.FieldValue.ToString());
                            break;
                    }
                }

               
            }

            // Finalization. 
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                //e.TotalValue = customSum;

                switch (summaryID)
                {
                    case 1:
                        e.TotalValue = customSum;
                        break;
                    case 2:
                        e.TotalValue = customSumGroup;
                        break;
                }

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

        private void dgvRebate_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                DateTime pdFecha = Convert.ToDateTime(dgvRebate.GetRowCellValue(e.RowHandle, "FechaVencimiento"));
                int piDias = DateTime.Now.Subtract(pdFecha).Days;
                if (pdFecha != DateTime.MinValue)
                {
                    if (piDias > 15)
                    {
                        e.Appearance.BackColor = Color.FromArgb(252, 108, 108);
                    }
                    else if (piDias >= 8 && piDias <= 15)
                    {
                        e.Appearance.BackColor = Color.Yellow;
                    }
                    else if (piDias >= -99999 && piDias <= 7)
                    {
                        e.Appearance.BackColor = Color.LightGreen;
                    }
                }

                //Override any other formatting  
                e.HighPriority = true;
            }
            catch (Exception)
            {

                throw;
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
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Métodos
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Click += btnAprobar_Click;
            if (tstBotones.Items["btnDesAprobar"] != null) tstBotones.Items["btnDesAprobar"].Click += btnDesAprobar_Click;   
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Click += btnRechazar_Click;
            if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Click += btnCorregir_Click;
            if (tstBotones.Items["btnAprobacionDefinitiva"] != null) tstBotones.Items["btnAprobacionDefinitiva"].Click += btnAprobacionDefinitiva_Click;



            /*********************************************************************************************************************************************/

            /*********************************************************************************************************************************************/

            bsRebate = new BindingSource();
            bsRebate.DataSource = new List<FacturaPorAprobarDetalleGrid>();
            gcRebate.DataSource = bsRebate;

            dgvRebate.OptionsBehavior.Editable = true;
            dgvRebate.OptionsPrint.AutoWidth = false;

            dgvRebate.OptionsSelection.MultiSelect = true;
            dgvRebate.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            dgvRebate.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            dgvRebate.OptionsSelection.CheckBoxSelectorColumnWidth = 40;

            dgvRebate.OptionsCustomization.AllowSort = false;

            dgvRebate.OptionsView.ShowGroupedColumns = true;
            dgvRebate.Columns["Proveedor"].GroupIndex = 0;
            dgvRebate.ExpandAllGroups();

            dgvRebate.Appearance.GroupFooter.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);

            dgvRebate.Columns["IdOrdenPagoFactura"].Visible = false;
            dgvRebate.Columns["IdFacturaPago"].Visible = false;
            dgvRebate.Columns["Identificacion"].Visible = false;
            dgvRebate.Columns["Sel"].Visible = false;
            dgvRebate.Columns["Generado"].Visible = false;
            dgvRebate.Columns["Del"].Visible = false;
            dgvRebate.Columns["IdOrdenPago"].Visible = false;
            dgvRebate.Columns["Valor"].Visible = false;
            dgvRebate.Columns["Abono"].Visible = false;
            dgvRebate.Columns["Saldo"].Visible = false;
            dgvRebate.Columns["FechaEmision"].Visible = false;
            dgvRebate.Columns["DocNum"].Visible = false;

            //dgvRebate.Columns["Sel"].Visible = false;

            dgvRebate.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvRebate.Columns["Proveedor"].ColumnEdit = rpiMedDescripcion;
            dgvRebate.Columns["Aprobo"].ColumnEdit = rpiMedDescripcion;
            dgvRebate.Columns["UsuarioCreaOP"].ColumnEdit = rpiMedDescripcion;
            dgvRebate.Columns["UsuarioApruebaOP"].ColumnEdit = rpiMedDescripcion;
            dgvRebate.Columns["Comentario"].ColumnEdit = rpiMedDescripcion;

            

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
            dgvRebate.Columns["Aprobaciones"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Aprobo"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["UsuarioCreaOP"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["UsuarioApruebaOP"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Comentario"].OptionsColumn.AllowEdit = true;
            dgvRebate.Columns["VerComentarios"].OptionsColumn.AllowEdit = true;

            dgvRebate.Columns["UsuarioCreaOP"].Caption = "Creó Orden Pago";
            dgvRebate.Columns["UsuarioApruebaOP"].Caption = "Aprobó Orden Pago";
            dgvRebate.Columns["ValorPago"].Caption = "Valor a Pagar";
            dgvRebate.Columns["NumDocumento"].Caption = "# Factura";
            dgvRebate.Columns["CodigoEstado"].Caption = "Estado";
            dgvRebate.Columns["Aprobo"].Caption = "Aprobó";
            dgvRebate.Columns["Aprobaciones"].Caption = "#";


            //dgvRebate.FixedLineWidth = 3;
            //dgvRebate.Columns["Sel"].Fixed = FixedStyle.Left;
            //dgvRebate.Columns["Proveedor"].Fixed = FixedStyle.Left;
            //dgvRebate.Columns["NumDocumento"].Fixed = FixedStyle.Left;
            //dgvRebate.Columns["Ver"].Fixed = FixedStyle.Right;

            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvRebate.Columns["Ver"], "Orden Pago", Diccionario.ButtonGridImage.show_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvRebate.Columns["VerComentarios"], "Ver", Diccionario.ButtonGridImage.showhidecomment_16x16);

            dgvRebate.OptionsView.RowAutoHeight = true;
            dgvRebate.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;

            dgvRebate.BestFitColumns();
            dgvRebate.Columns["Comentario"].Width = 145;
            dgvRebate.Columns["UsuarioCreaOP"].Width = 80;
            dgvRebate.Columns["UsuarioApruebaOP"].Width = 80; 
            dgvRebate.Columns["Sel"].Width = 40;
            dgvRebate.Columns["Proveedor"].Width = 150;
            dgvRebate.Columns["NumDocumento"].Width = 65;
            dgvRebate.Columns["FechaVencimiento"].Width = 75;
            dgvRebate.Columns["DocNum"].Width = 65;
            dgvRebate.Columns["ValorPago"].Width = 90;
            dgvRebate.Columns["Observacion"].Width = 150;
            dgvRebate.Columns["CodigoEstado"].Width = 90;
            dgvRebate.Columns["Aprobo"].Width = 145;
            dgvRebate.Columns["Aprobaciones"].Width = 30;
            dgvRebate.Columns["VerComentarios"].Width = 40;
            dgvRebate.Columns["Ver"].Width = 40;
            
            //clsComun.gFormatearColumnasGrid(dgvRebate);

            dgvRebate.Columns["ValorPago"].UnboundType = UnboundColumnType.Decimal;
            dgvRebate.Columns["ValorPago"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvRebate.Columns["ValorPago"].DisplayFormat.FormatString = "c2";
            //dgvRebate.Columns["ValorPago"].Summary.Add(SummaryItemType.Sum, "ValorPago", "{0:c2}");



            //sumColumn.FieldName = "ValorPago";
            //sumColumn.SummaryType = SummaryItemType.Sum;
            //sumColumn.DisplayFormat = "{0:c2}";
            //sumColumn.ShowInGroupColumnFooter = dgvRebate.Columns["ValorPago"];
            //dgvRebate.GroupSummary.Add(sumColumn);

            
            dgvRebate.Columns["ValorPago"].SummaryItem.SummaryType = SummaryItemType.Custom;
            dgvRebate.Columns["ValorPago"].SummaryItem.DisplayFormat = "{0:c2}";
            dgvRebate.Columns["ValorPago"].SummaryItem.Tag = 1;
            
            //dgvRebate.CustomSummaryCalculate += Grid_CustomSummaryCalculate;
            //(dgvRebate.Columns["Sel"].RealColumnEdit as RepositoryItemCheckEdit).CheckedChanged += Grid_CheckedChanged;
            dgvRebate.GroupSummary.Clear();

            GridGroupSummaryItem sumColumn = new GridGroupSummaryItem();
            //sumColumn = dgvRebate.GroupSummary.Add(SummaryItemType.Sum, "ValorPago", dgvRebate.Columns["ValorPago"], "{0:c2}");
            sumColumn.ShowInGroupColumnFooter = dgvRebate.Columns["ValorPago"];
            sumColumn.FieldName = "ValorPago";
            sumColumn.SummaryType = SummaryItemType.Custom;
            sumColumn.Tag = 2;
            sumColumn.DisplayFormat = "{0:c2}";
            

            dgvRebate.GroupSummary.Add(sumColumn);

            //sumColumn.SummaryValue

            dgvRebate.CustomSummaryCalculate += gridView1_CustomSummaryCalculate;
            dgvRebate.SelectionChanged += gridView1_SelectionChanged;

            dgvRebate.UpdateSummary();

            dtpFechaInicio.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaFin.EditValue = DateTime.Now;
            

        }

        private void lLimpiar()
        {
            bsRebate.DataSource = new List<FacturaPorAprobarDetalleGrid>();
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

            bsRebate.DataSource = loLogicaNegocio.goConsultaFacturasPendientePagoPorAprobar(int.Parse(Tag.ToString().Split(',')[0]),clsPrincipal.gsUsuario, cmbGrupoPago.EditValue.ToString());
            gcRebate.DataSource = bsRebate;

            dgvRebate.ExpandAllGroups();
            lCalcularTotal();
            Cursor.Current = Cursors.Default;

        }

        private void lCalcularTotal()
        {
            var pdcTotal = ((List<FacturaPorAprobarDetalleGrid>)bsRebate.DataSource).Sum(x => x.ValorPago);
            lblTotal.Text = pdcTotal.ToString("c2");
        }

        #endregion

    }
}
