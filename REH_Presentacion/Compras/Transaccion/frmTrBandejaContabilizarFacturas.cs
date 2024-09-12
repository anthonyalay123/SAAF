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
    /// Formulario que permite generar diario contable de ordenes de pago
    /// Autor: Víctor Arévalo
    /// Fecha: 16/07/2024
    /// </summary>
    public partial class frmTrBandejaContabilizarFacturas : frmBaseTrxDev
    {

        #region Variables
        clsNOrdenPago loLogicaNegocio;
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();
        BindingSource bsDatos;
        List<int> lIdEliminar = new List<int>();
        private decimal customSum = 0;
        private decimal customSumGroup = 0;
        private bool pbCargado = false;

        #endregion

        #region Eventos

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmTrBandejaContabilizarFacturas()
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
                //clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboEstado(), "CodigoEstado");
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
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<SpBandejaOrdenPagoContabilizar>)bsDatos.DataSource;
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
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<SpBandejaOrdenPagoContabilizar>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    //var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.FacturaPago, poLista[piIndex].IdFacturaPago);

                    //frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Aprobadores con sus comentarios" };
                    //if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                    //{

                    //}

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
                dgvDatos.PostEditor();
                //dgvRebate.RefreshData();

                var poLista = (List<SpBandejaOrdenPagoContabilizar>)bsDatos.DataSource;
                poLista.ForEach(x => x.Sel = false);

                for (int i = 0; i < dgvDatos.SelectedRowsCount; i++)
                {
                    int pIdSel = dgvDatos.GetSelectedRows()[i];
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
                            //if (poLista[i].Sel == true)
                            //{

                            //    var psMess = loLogicaNegocio.gsAprobarFacturaPago(poLista[i].IdFacturaPago, poLista[i].Comentario ,clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, int.Parse(Tag.ToString().Split(',')[0]));

                            //    if (psMess != "")
                            //    {
                            //        IdNoActualizadas += "Factura No. " + poLista[i].NumDocumento + ": " + psMess + "\n";
                            //    }
                            //}
                            //else
                            //{
                            //    if (pbRechazaNoSel)
                            //    {
                            //        loLogicaNegocio.gsRechazarFacturaPago(poLista[i].IdFacturaPago, poLista[i].Comentario, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            //    }
                            //}
                            
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
                dgvDatos.PostEditor();
                //dgvRebate.RefreshData();

                var poLista = (List<SpBandejaOrdenPagoContabilizar>)bsDatos.DataSource;
                poLista.ForEach(x => x.Sel = false);

                for (int i = 0; i < dgvDatos.SelectedRowsCount; i++)
                {
                    int pIdSel = dgvDatos.GetSelectedRows()[i];
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

                                //var psMess = loLogicaNegocio.gsAprobacionDefinitiva(poLista[i].IdFacturaPago, poLista[i].Comentario, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                //if (psMess != "")
                                //{
                                //    IdNoActualizadas += "Factura No. " + poLista[i].NumDocumento + ": " + psMess + "\n";
                                //}
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
                dgvDatos.PostEditor();
                
                var poLista = (List<SpBandejaOrdenPagoContabilizar>)bsDatos.DataSource;
                poLista.ForEach(x => x.Sel = false);

                for (int i = 0; i < dgvDatos.SelectedRowsCount; i++)
                {
                    int pIdSel = dgvDatos.GetSelectedRows()[i];
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

                                //var psMess = loLogicaNegocio.gsDesaprobarFacturaPago(poLista[i].IdFacturaPago, poLista[i].Comentario, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                //if (psMess != "")
                                //{
                                //    IdNoActualizadas += "Factura No. " + poLista[i].NumDocumento + ": " + psMess + "\n";
                                //}
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
                List<SpBandejaOrdenPagoContabilizar> listbd = new List<SpBandejaOrdenPagoContabilizar>();

                //var poLista = (List<SpBandejaOrdenPagoContabilizar>)bsDatos.DataSource;
                //if (poLista.Count > 0)
                //{
                //    foreach (var po in poLista)
                //    {
                //        SpBandejaOrdenPagoContabilizar be = new SpBandejaOrdenPagoContabilizar();
                        
                //        be.UsuarioCreaOP = po.UsuarioCreaOP;
                //        be.UsuarioApruebaOP = po.UsuarioApruebaOP;
                //        be.Identificacion = po.Identificacion;
                //        be.Proveedor = po.Proveedor;
                //        be.NumDocumento = po.NumDocumento;
                //        be.DocNum = po.DocNum;
                //        be.FechaEmision = po.FechaEmision;
                //        be.FechaVencimiento = po.FechaVencimiento;
                //        be.ValorPago = po.ValorPago;
                //        be.Observacion = po.Observacion;


                //        listbd.Add(be);
                //    }

                //    bs.DataSource = listbd;
                //    gc.DataSource = bs;
                //    gc.MainView = dgv;
                //    gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                //    dgv.GridControl = gc;
                //    this.Controls.Add(gc);

                //    // Exportar Datos
                //    try
                //    {
                //        clsComun.gSaveFile(gc, "" + Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");
                //        gc.Visible = false;
                //    }
                //    catch (Exception)
                //    {
                //        XtraMessageBox.Show("Fallo al guardar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        gc.Visible = false;
                //    }
                //    gc.Visible = false;
                //}
                //else
                //{
                //    XtraMessageBox.Show("No hay registros para exportar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
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
                dgvDatos.PostEditor();

                var poLista = (List<SpBandejaOrdenPagoContabilizar>)bsDatos.DataSource;
                poLista.ForEach(x => x.Sel = false);

                for (int i = 0; i < dgvDatos.SelectedRowsCount; i++)
                {
                    int pIdSel = dgvDatos.GetSelectedRows()[i];
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

                                //var psMess = loLogicaNegocio.gsRechazarFacturaPago(poLista[i].IdFacturaPago, result, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                //if (psMess != "")
                                //{
                                //    IdNoActualizadas += "Factura No. " + poLista[i].NumDocumento + ": " + psMess + "\n";
                                //}
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
                dgvDatos.PostEditor();

                var poLista = (List<SpBandejaOrdenPagoContabilizar>)bsDatos.DataSource;
                poLista.ForEach(x => x.Sel = false);

                for (int i = 0; i < dgvDatos.SelectedRowsCount; i++)
                {
                    int pIdSel = dgvDatos.GetSelectedRows()[i];
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

                                //var psMess = loLogicaNegocio.gsCorregirFacturaPago(poLista[i].IdFacturaPago, result, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                //if (psMess != "")
                                //{
                                //    IdNoActualizadas += "Factura No. " + poLista[i].NumDocumento + ": " + psMess + "\n";
                                //}
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
            dgvDatos.UpdateSummary();
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
                if (dgvDatos.IsRowSelected(e.RowHandle))
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
                dtpFechaInicio.Enabled = true;
                dtpFechaFin.Enabled = true;
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
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Click += btnAprobar_Click;
            if (tstBotones.Items["btnDesAprobar"] != null) tstBotones.Items["btnDesAprobar"].Click += btnDesAprobar_Click;   
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Click += btnRechazar_Click;
            if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Click += btnCorregir_Click;
            if (tstBotones.Items["btnGenerarDiario"] != null) tstBotones.Items["btnGenerarDiario"].Click += btnGenerarDiario_Click;
            
            if (tstBotones.Items["btnAprobacionDefinitiva"] != null) tstBotones.Items["btnAprobacionDefinitiva"].Click += btnAprobacionDefinitiva_Click;



            /*********************************************************************************************************************************************/
            /*********************************************************************************************************************************************/

            bsDatos = new BindingSource();
            bsDatos.DataSource = new List<SpBandejaOrdenPagoContabilizar>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsBehavior.Editable = true;
            dgvDatos.OptionsPrint.AutoWidth = false;

            //dgvDatos.OptionsSelection.MultiSelect = true;
            //dgvDatos.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            //dgvDatos.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            //dgvDatos.OptionsSelection.CheckBoxSelectorColumnWidth = 40;

            dgvDatos.OptionsCustomization.AllowSort = true;

            //dgvDatos.OptionsView.ShowGroupedColumns = true;
            //dgvDatos.Columns["Proveedor"].GroupIndex = 0;
            //dgvDatos.ExpandAllGroups();

            dgvDatos.Appearance.GroupFooter.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);

            dgvDatos.Columns["IdOrdenPagoFactura"].Visible = false;
            dgvDatos.Columns["IdOrdenPago"].Visible = false;
            dgvDatos.Columns["CodProveedor"].Visible = false;
            dgvDatos.Columns["FechaFactura"].Visible = false;
            //dgvDatos.Columns["Sel"].Visible = false;

            dgvDatos.Columns["Proveedor"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Usuario"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;

            dgvDatos.Columns["Fecha"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Usuario"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Proveedor"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Observacion"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Factura"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Valor"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["Sel"].Width = 20;

            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDatos.Columns["Ver"], "Orden Pago", Diccionario.ButtonGridImage.show_16x16,40);
            //clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvDatos.Columns["VerComentarios"], "Ver", Diccionario.ButtonGridImage.showhidecomment_16x16);

            dgvDatos.OptionsView.RowAutoHeight = true;
            dgvDatos.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;

            dgvDatos.BestFitColumns();
            
            
            dgvDatos.Columns["Valor"].UnboundType = UnboundColumnType.Decimal;
            dgvDatos.Columns["Valor"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Valor"].DisplayFormat.FormatString = "c2";

            /*
            dgvDatos.Columns["Valor"].SummaryItem.SummaryType = SummaryItemType.Custom;
            dgvDatos.Columns["Valor"].SummaryItem.DisplayFormat = "{0:c2}";
            dgvDatos.Columns["Valor"].SummaryItem.Tag = 1;
            
            
            dgvDatos.GroupSummary.Clear();
            GridGroupSummaryItem sumColumn = new GridGroupSummaryItem();
            
            sumColumn.ShowInGroupColumnFooter = dgvDatos.Columns["Valor"];
            sumColumn.FieldName = "Valor";
            sumColumn.SummaryType = SummaryItemType.Custom;
            sumColumn.Tag = 2;
            sumColumn.DisplayFormat = "{0:c2}";
            

            dgvDatos.GroupSummary.Add(sumColumn);

            dgvDatos.CustomSummaryCalculate += gridView1_CustomSummaryCalculate;
            dgvDatos.SelectionChanged += gridView1_SelectionChanged;

            dgvDatos.UpdateSummary();
            */

            dtpFechaInicio.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaFin.EditValue = DateTime.Now;
            

        }

        private void lLimpiar()
        {
            bsDatos.DataSource = new List<SpBandejaOrdenPagoContabilizar>();
            gcDatos.DataSource = bsDatos;
            dtpFechaInicio.Enabled = true;
            dtpFechaFin.Enabled = true;
            dtpFechaInicio.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaFin.EditValue = DateTime.Now;
            lIdEliminar = new List<int>();
        }

        private void lBuscar()
        {

            Cursor.Current = Cursors.WaitCursor;

            bsDatos.DataSource = loLogicaNegocio.goConsultaBandejaOrdenPagoContabilizar(dtpFechaInicio.DateTime, dtpFechaFin.DateTime, chbConsultarTodos.Checked);
            gcDatos.DataSource = bsDatos;

            dgvDatos.ExpandAllGroups();
            lCalcularTotal();
            Cursor.Current = Cursors.Default;

        }

        private void lCalcularTotal()
        {
            var pdcTotal = ((List<SpBandejaOrdenPagoContabilizar>)bsDatos.DataSource).Sum(x => x.Valor);
            //lblTotal.Text = pdcTotal.ToString("c2");
        }

        private void btnGenerarDiario_Click(object sender, EventArgs e)
        {

            try
            {
                dgvDatos.PostEditor();
                //dgvRebate.RefreshData();

                var poLista = (List<SpBandejaOrdenPagoContabilizar>)bsDatos.DataSource;


                if (poLista.Where(x=>x.Sel).Count() > 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de generar diario?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var poListaEnviada = poLista.Where(x => x.Sel).ToList();
                        int Cont = 0;
                        int Tot = poListaEnviada.Count();
                        foreach (var item in poListaEnviada)
                        {
                            Cont++;
                            var poListaIndividual = new List<SpBandejaOrdenPagoContabilizar>();
                            poListaIndividual.Add(item);

                            DataTable dt = new DataTable();
                            string psMsg = loLogicaNegocio.GenerarDiarioOrdenPago(poListaIndividual, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, out dt);

                            if (!string.IsNullOrEmpty(psMsg))
                            {
                                XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                frmPopUpDatos frm = new frmPopUpDatos(dt);
                                frm.Text = string.Format("Diario de Factura. {0} de {1} ", Cont, Tot);
                                frm.ShowDialog();
                            }
                        }

                        //DataTable dt = new DataTable();
                        //string psMsg = loLogicaNegocio.GenerarDiarioOrdenPago(poListaEnviada,clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, out dt);
                        
                        //if (!string.IsNullOrEmpty(psMsg))
                        //{
                        //    XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //}
                        //else
                        //{
                        //    frmPopUpDatos frm = new frmPopUpDatos(dt);
                        //    frm.ShowDialog();

                        //    GridControl gc = new GridControl();
                        //    BindingSource bs = new BindingSource();
                        //    GridView dgv = new GridView();

                        //    gc.DataSource = bs;
                        //    gc.MainView = dgv;
                        //    gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                        //    dgv.GridControl = gc;
                        //    this.Controls.Add(gc);
                        //    bs.DataSource = dt;
                        //    dgv.BestFitColumns();
                        //    dgv.PopulateColumns();
                        //    dgv.PostEditor();
                        //    dgv.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
                        //    // Exportar Datos
                        //    clsComun.gSaveFile(gc, "Diario_Orden_Pagos_Facturas_" + Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");

                        //    gc.Visible = false;

                        //    lBuscar();
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void btnSeleccionarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                List<SpBandejaOrdenPagoContabilizar> poLista = (List<SpBandejaOrdenPagoContabilizar>)bsDatos.DataSource;
                int piContTrue = poLista.Where(x => x.Sel).Count();
                int piContFalse = poLista.Where(x => !x.Sel).Count();

                if (piContTrue == 0 && piContFalse > 0)
                {
                    poLista.ForEach(x => x.Sel = true);
                }
                else
                {
                    poLista.ForEach(x => x.Sel = false);
                }

                bsDatos.DataSource = poLista.ToList();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
