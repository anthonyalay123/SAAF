using CRE_Negocio.Transacciones;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Credito;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Credito.Transacciones
{
    public partial class frmTrRevisionBienes : frmBaseTrxDev
    {
        private frmTrProcesoCredito m_MainForm;
        public string lsTipo = "";
        public int lIdProceso = 0;
        public string lsTipoRuc = "";
        public string lsTipoProceso = "";
        public bool lbGuardado = false;
        public bool lbCompletado = false;
        public bool lbRevision = false;
        BindingSource bsDatos = new BindingSource();
        BindingSource bsOtrosActivos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDelOt = new RepositoryItemButtonEdit();
        clsNProcesoCredito loLogicaNegocio = new clsNProcesoCredito();
        public bool lbCerrado = false;

        public frmTrRevisionBienes(frmTrProcesoCredito form = null)
        {
            m_MainForm = form;
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnDelOt.ButtonClick += rpiBtnDelOt_ButtonClick;
        }

        private void frmTrPantillaSeguro_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                clsComun.gLLenarComboGrid(ref dgvOtrosActivos, loLogicaNegocio.goConsultarComboTipoOtrosActivos(), "CodigoTipoOtrosActivos", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboTipoBien(), "CodigoTipoBien", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboSINONE(), "Hipoteca", true);

                if (lIdProceso != 0)
                {

                    if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Visible = false;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Visible = false;
                    if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Visible = false;
                    if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Visible = false;
                    if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Visible = false;
                    if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Visible = false;

                    if (lIdProceso != 0)
                    {
                        txtNo.Text = lIdProceso.ToString();
                        lConsultar();
                    }

                    //var pIdSol = loLogicaNegocio.gIdSolicitudCredito(lIdProceso);
                    //if (pIdSol != 0)
                    //{
                    //    txtNo.Text = pIdSol.ToString();
                    //    lConsultar();
                    //}
                    //else
                    //{
                    //    XtraMessageBox.Show("No Existe Solicitud de Crédito para revisión de RUC", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    Close();
                    //}
                }

                if (lbCerrado)
                {
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
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

        //private void btnBuscar_Click(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        var menu = Tag.ToString().Split(',');
        //        var poListaObject = loLogicaNegocio.goListar();
        //        DataTable dt = new DataTable();

        //        dt.Columns.AddRange(new DataColumn[]
        //                            {
        //                            new DataColumn("No"),
        //                            new DataColumn("Fecha", typeof(DateTime)),
        //                            new DataColumn("Razón Social"),
        //                            new DataColumn("Ruc"),
        //                            new DataColumn("Cupo")

        //                            });

        //        poListaObject.ForEach(a =>
        //        {
        //            DataRow row = dt.NewRow();
        //            row["No"] = a.IdPlantillaSeguro;
        //            row["Fecha"] = a.Fecha;
        //            row["Razón Social"] = a.RazonSocial;
        //            row["Ruc"] = a.Ruc;
        //            row["Cupo"] = a.CupoSolicitado;
        //            dt.Rows.Add(row);
        //        });

        //        frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
        //        pofrmBuscar.Width = 1200;
        //        if (pofrmBuscar.ShowDialog() == DialogResult.OK)
        //        {
        //            lLimpiar();
        //            txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
        //            lConsultar();

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

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
                var poLista = (List<ProcesoCreditoPropiedades>)bsDatos.DataSource;

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

        /// <summary>
        /// Evento del boton de eliminar en el Grid, elimina la fila seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDelOt_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvOtrosActivos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<ProcesoCreditoOtrosActivos>)bsOtrosActivos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsOtrosActivos.DataSource = poLista;
                    dgvOtrosActivos.RefreshData();
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

                dgvDatos.PostEditor();
                dgvOtrosActivos.PostEditor();
                if (lbEsValido())
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ProcesoCredito poObject = new ProcesoCredito();
                        poObject.IdProcesoCredito = lIdProceso;
                        //poObject.IdProcesoCredito = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                        poObject.ComentariosBienes = txtComentarios.Text;
                        poObject.ProcesoCreditoPropiedades = (List<ProcesoCreditoPropiedades>)bsDatos.DataSource;
                        poObject.ProcesoCreditoOtrosActivos = (List<ProcesoCreditoOtrosActivos>)bsOtrosActivos.DataSource;

                        string psMsg = loLogicaNegocio.gsGuardarRevisionBienes(poObject, lsTipo, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                        if (string.IsNullOrEmpty(psMsg))
                        {
                            if (m_MainForm != null)
                            {
                                m_MainForm.lConsultar();
                            }
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Close();

                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
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


        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!string.IsNullOrEmpty(txtNo.Text.ToString()))
                //{
                //    int lId = int.Parse(txtNo.Text);


                //    //DataSet ds = new DataSet();
                //    var ds = loLogicaNegocio.goConsultaDataSet("EXEC SHESPRPTCONTROLCALIDAD " + "'" + lId + "'");

                //    ds.Tables[0].TableName = "ControlCalidadCab";
                //    ds.Tables[1].TableName = "ControlCalidadDet";

                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        xrptControlCalidad xrpt = new xrptControlCalidad();
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Primero, txtNo.Text.Trim(), int.Parse(Tag.ToString().Split(',')[0]), clsPrincipal.gsUsuario);
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Anterior, txtNo.Text.Trim(), int.Parse(Tag.ToString().Split(',')[0]), clsPrincipal.gsUsuario);
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Siguiente, txtNo.Text.Trim(), int.Parse(Tag.ToString().Split(',')[0]), clsPrincipal.gsUsuario);
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Ultimo, txtNo.Text.Trim(), int.Parse(Tag.ToString().Split(',')[0]), clsPrincipal.gsUsuario);
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            //if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            //if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;


            bsOtrosActivos.DataSource = new List<ProcesoCreditoOtrosActivos>();
            gcOtrosActivos.DataSource = bsOtrosActivos;

            bsDatos.DataSource = new List<ProcesoCreditoPropiedades>();
            gcDatos.DataSource = bsDatos;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
            clsComun.gDibujarBotonGrid(rpiBtnDelOt, dgvOtrosActivos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
            //clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvDatos.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16);
            //clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDatos.Columns["Ver"], "Visualizar", Diccionario.ButtonGridImage.show_16x16);

            dgvDatos.Columns["IdProcesoCredito"].Visible = false;
            dgvDatos.Columns["IdProcesoCreditoPropiedades"].Visible = false;
            //dgvDatos.Columns["Del"].Visible = false;
            dgvDatos.Columns["CodigoTipoBien"].Caption = "Tipo Bien";

            dgvOtrosActivos.Columns["IdProcesoCredito"].Visible = false;
            dgvOtrosActivos.Columns["IdProcesoCreditoOtrosActivos"].Visible = false;
            //dgvOtrosActivos.Columns["Del"].Visible = false;
            dgvOtrosActivos.Columns["CodigoTipoOtrosActivos"].Caption = "Tipo";

            clsComun.gFormatearColumnasGrid(dgvDatos);
            clsComun.gFormatearColumnasGrid(dgvOtrosActivos);
        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultar(Convert.ToInt32(txtNo.Text.Trim()));

                txtNo.EditValue = poObject.IdProcesoCredito;
                lIdProceso = poObject.IdProcesoCredito;
                lblFecha.Text = poObject.Fecha.ToString("dd/MM/yyyy");
                txtComentarios.Text = poObject.ComentariosBienes;
                bsDatos.DataSource = poObject.ProcesoCreditoPropiedades;
                dgvDatos.RefreshData();
                bsOtrosActivos.DataSource = poObject.ProcesoCreditoOtrosActivos;
                dgvOtrosActivos.RefreshData();

                if (loLogicaNegocio.gsGetEstadoReqCreCheckList(poObject.IdProcesoCredito, Diccionario.Tablas.CheckList.PrediosCertfBienes) == Diccionario.Aprobado)
                {
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                }
                else
                {
                    if (lbRevision)
                    {
                        if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                        if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                    }
                    else
                    {
                        if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
                        if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                    }
                }
            }
        }

        private void lLimpiar()
        {
            txtNo.EditValue = "";
            lIdProceso = 0;
            lblFecha.Text = "";
            txtComentarios.Text = "";

        }

        private void frmTrPantillaSeguro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

        private bool lbEsValido()
        {
            return true;
        }

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                bsDatos.AddNew();
                dgvDatos.Focus();
                dgvDatos.ShowEditor();
                dgvDatos.UpdateCurrentRow();
                var poLista = (List<ProcesoCreditoPropiedades>)bsDatos.DataSource;
                poLista.LastOrDefault().CodigoTipoBien = Diccionario.Seleccione;
                poLista.LastOrDefault().Hipoteca = Diccionario.Seleccione;
                dgvDatos.RefreshData();
                dgvDatos.FocusedColumn = dgvDatos.VisibleColumns[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddOA_Click(object sender, EventArgs e)
        {
            try
            {
                bsOtrosActivos.AddNew();
                dgvOtrosActivos.Focus();
                dgvOtrosActivos.ShowEditor();
                dgvOtrosActivos.UpdateCurrentRow();
                var poLista = (List<ProcesoCreditoOtrosActivos>)bsOtrosActivos.DataSource;
                poLista.LastOrDefault().CodigoTipoOtrosActivos = Diccionario.Seleccione;
                dgvOtrosActivos.RefreshData();
                dgvOtrosActivos.FocusedColumn = dgvOtrosActivos.VisibleColumns[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
