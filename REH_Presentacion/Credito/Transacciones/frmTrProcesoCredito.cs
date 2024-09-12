using CRE_Negocio.Transacciones;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraReports.UI;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using GEN_Entidad.Entidades.Credito;
using REH_Presentacion.Comun;
using REH_Presentacion.Credito.PopUp;
using REH_Presentacion.Credito.Reportes.Rpt;
using REH_Presentacion.Formularios;
using reporte;
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
    public partial class frmTrProcesoCredito : frmBaseTrxDev
    {
        clsNProcesoCredito loLogicaNegocio = new clsNProcesoCredito();
        BindingSource bsCheckLisk = new BindingSource();
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnRevision = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDelAdj = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnAdd = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnView = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnCorregir = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnAprobar = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDesAprobar = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnExcepcion = new RepositoryItemButtonEdit();
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();
        RepositoryItemButtonEdit rpiBtnEnEspera = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnTrazabilidadAdjunto = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnTrazabilidadFechaCompromiso = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnTrazabilidadComentarios = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDownload = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShowArchivo = new RepositoryItemButtonEdit();
        BindingSource bsArchivoAdjunto = new BindingSource();

        bool pbCargado = false;
        public bool lbRevision = false;
        public bool lbResolucion = false;
        string lsActEntry = "";

        public int lid = 0;
        public bool lbCerrado = false;

        public frmTrProcesoCredito()
        {
            InitializeComponent();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnDelAdj.ButtonClick += rpiBtnDelAdj_ButtonClick;
            rpiBtnAdd.ButtonClick += rpiBtnAdd_ButtonClick;
            rpiBtnView.ButtonClick += rpiBtnView_ButtonClick;
            rpiBtnRevision.ButtonClick += rpiBtnRevision_ButtonClick;
            rpiBtnShowComentarios.ButtonClick += rpiBtnShowComentarios_ButtonClick;
            rpiBtnCorregir.ButtonClick += rpiBtnCorregir_ButtonClick;
            rpiBtnAprobar.ButtonClick += rpiBtnAprobar_ButtonClick;
            rpiBtnDesAprobar.ButtonClick += rpiBtnDesAprobar_ButtonClick;
            rpiBtnExcepcion.ButtonClick += rpiBtnExcepcion_ButtonClick;
            rpiBtnEnEspera.ButtonClick += rpiBtnEnEspera_ButtonClick;
            rpiBtnTrazabilidadAdjunto.ButtonClick += rpiBtnTrazabilidadAdjunto_ButtonClick;
            rpiBtnTrazabilidadFechaCompromiso.ButtonClick += rpiBtnTrazabilidadFechaCompromiso_ButtonClick;
            rpiBtnTrazabilidadComentarios.ButtonClick += rpiBtnTrazabilidadComentarios_ButtonClick;
            rpiBtnShowArchivo.ButtonClick += rpiBtnShowArchivo_ButtonClick;
            rpiBtnDownload.ButtonClick += rpiBtnDownload_ButtonClick;
            rpiMedDescripcion.WordWrap = true;
        }

        private void frmTrProcesoCredito_Load(object sender, EventArgs e)
        {
            try
            {

                lCargarEventosBotones();
                clsComun.gLLenarCombo(ref cmbTipoSolicitud, loLogicaNegocio.goConsultarComboTipoProcesoCredito(), true);
                clsComun.gLLenarCombo(ref cmbCliente, loLogicaNegocio.goSapConsultaClientesTodos(), true);
                clsComun.gLLenarCombo(ref cmbCodContado, loLogicaNegocio.goConsultarComboSINO(), true);
                clsComun.gLLenarCombo(ref cmbTipoPersona, loLogicaNegocio.goConsultarComboTipoPersonaNatJur(), true);
                clsComun.gLLenarCombo(ref cmbEstatusSeguro, loLogicaNegocio.goConsultarComboEstatusSeguro(), true);
                clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstado(), true, false, "...");
                clsComun.gLLenarCombo(ref cmbRTC, loLogicaNegocio.goConsultarComboVendedorGrupo(), true);

                cmbEstatusSeguro.EditValue = "SIN";
                cmbEstatusSeguro.ReadOnly = true;

                //clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstado(), true);

                clsComun.gLLenarComboGrid(ref dgvCheckList, loLogicaNegocio.goConsultarComboSINO(), "Completado", false);

                if (int.Parse(Tag.ToString().Split(',')[0]) == 291)
                {
                    lbRevision = true;
                }

                if (lbRevision)
                {
                    //btnCorregir.Visible = true;
                    if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Visible = true;
                    btnCopiarDocumentos.Visible = false;
                    cmbCliente.ReadOnly = true;
                    cmbRTC.ReadOnly = true;
                    //txtZona.ReadOnly = true;
                    cmbCodContado.ReadOnly = true;
                    cmbEstatusSeguro.ReadOnly = true;
                    cmbTipoPersona.ReadOnly = true;
                    cmbTipoSolicitud.ReadOnly = true;
                    txtCliente.ReadOnly = true;
                    chPlazoCorto.ReadOnly = true;
                    chbRevisionCompletada.Visible = true;
                    chbDocumentosCompletos.Visible = true;
                    chbEnviarRevision.Visible = false;
                    txtObservacion.ReadOnly = true;
                    btnAddFila.Visible = false;
                    dgvCheckList.Columns["Transaccion"].Visible = false;
                    dgvCheckList.Columns["Revision"].Visible = true;
                    dgvCheckList.Columns["Del"].Visible = false;
                    dgvCheckList.Columns["Add"].Visible = false;
                    dgvCheckList.Columns["DelAdj"].Visible = false;
                    dgvCheckList.Columns["FechaReferencial"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["FechaCompromiso"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["Comentarios"].OptionsColumn.ReadOnly = false;
                    dgvCheckList.Columns["Comentarios"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["Corregir"].Visible = true;
                    dgvCheckList.Columns["Aprobar"].Visible = true;
                    dgvCheckList.Columns["Excepcion"].Visible = true;
                    dgvCheckList.Columns["EnEspera"].Visible = true;
                    dgvCheckList.Columns["DesAprobar"].Visible = true;
                    dgvCheckList.Columns["Comentarios"].Visible = true;
                    dgvCheckList.Columns["FisicoRecibido"].OptionsColumn.ReadOnly = false;
                    dgvCheckList.Columns["FechaFisicoRecibido"].Visible = false;
                    txtComentarioRevisor.ReadOnly = false;
                    txtComentarioResolucion.ReadOnly = true;
                    btnCertBienes.Visible = true;
                    btnInfRevisora.Visible = true;
                    btnRefComerciales.Visible = true;
                    btnRefBancarias.Visible = true;
                    if (lbResolucion)
                    {
                        dgvCheckList.Columns["Corregir"].Visible = false;
                        dgvCheckList.Columns["Aprobar"].Visible = false;
                        dgvCheckList.Columns["Excepcion"].Visible = false;
                        dgvCheckList.Columns["EnEspera"].Visible = false;
                        dgvCheckList.Columns["DesAprobar"].Visible = false;
                    }
                }
                else
                {
                    //btnCorregir.Visible = false;
                    if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Visible = false;
                    btnCopiarDocumentos.Visible = true;
                    cmbRTC.ReadOnly = false;
                    //txtZona.ReadOnly = false;
                    cmbCliente.ReadOnly = false;
                    cmbCodContado.ReadOnly = false;
                    cmbEstatusSeguro.ReadOnly = true;
                    cmbTipoPersona.ReadOnly = false;
                    cmbTipoSolicitud.ReadOnly = false;
                    txtCliente.ReadOnly = false;
                    chPlazoCorto.ReadOnly = false;
                    chbRevisionCompletada.Visible = false;
                    chbDocumentosCompletos.Visible = false;
                    chbEnviarRevision.Visible = true;
                    txtObservacion.ReadOnly = false;
                    btnAddFila.Visible = true;
                    dgvCheckList.Columns["Transaccion"].Visible = true;
                    dgvCheckList.Columns["Revision"].Visible = false;
                    dgvCheckList.Columns["Del"].Visible = true;
                    dgvCheckList.Columns["Add"].Visible = true;
                    dgvCheckList.Columns["DelAdj"].Visible = true;
                    dgvCheckList.Columns["FechaReferencial"].OptionsColumn.ReadOnly = false;
                    dgvCheckList.Columns["FechaCompromiso"].OptionsColumn.ReadOnly = false;
                    dgvCheckList.Columns["Corregir"].Visible = false;
                    dgvCheckList.Columns["Aprobar"].Visible = false;
                    dgvCheckList.Columns["Excepcion"].Visible = false;
                    dgvCheckList.Columns["EnEspera"].Visible = false;
                    dgvCheckList.Columns["DesAprobar"].Visible = false;
                    dgvCheckList.Columns["Comentarios"].Visible = true;
                    dgvCheckList.Columns["FisicoRecibido"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["FechaFisicoRecibido"].Visible = false;
                    txtComentarioRevisor.ReadOnly = true;
                    txtComentarioResolucion.ReadOnly = true;
                    if (lbResolucion)
                    {
                        btnCertBienes.Visible = true;
                        btnInfRevisora.Visible = true;
                        btnRefComerciales.Visible = true;
                        btnRefBancarias.Visible = true;
                    }
                    else
                    {
                        btnCertBienes.Visible = false;
                        btnInfRevisora.Visible = false;
                        btnRefComerciales.Visible = false;
                        btnRefBancarias.Visible = false;
                    }

                }

                pbCargado = true;

                if (lid != 0)
                {
                    txtNo.Text = lid.ToString();
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                var menu = Tag.ToString().Split(',');
                var poListaObject = loLogicaNegocio.goListar(int.Parse(Tag.ToString().Split(',')[0]), clsPrincipal.gsUsuario);
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Usuario"),
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Tipo Solicitud"),
                                    new DataColumn("Cliente"),
                                    new DataColumn("Representante Legal"),
                                    new DataColumn("Estado"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdProcesoCredito;
                    row["Usuario"] = a.NomUsuario;
                    row["Fecha"] = a.Fecha;
                    row["Tipo Solicitud"] = a.TipoSolicitud;
                    row["Cliente"] = a.Cliente;
                    row["Representante Legal"] = a.RepresentanteLegal;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
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
                lblFecha.Focus();
                bool bContinua = false;
                if (sender == null)
                {
                    bContinua = true;
                }
                else
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        bContinua = true;
                    }
                }


                if (bContinua)
                {
                    dgvCheckList.PostEditor();
                    dgvArchivoAdjunto.PostEditor();

                    ProcesoCredito poObject = new ProcesoCredito();
                    poObject.IdProcesoCredito = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                    poObject.Fecha = DateTime.Now;
                    poObject.IdRTC = int.Parse(cmbRTC.EditValue.ToString());
                    poObject.RTC = cmbRTC.Text;
                    poObject.Zona = txtZona.Text;
                    poObject.CodigoTipoSolicitud = cmbTipoSolicitud.EditValue.ToString();
                    poObject.TipoSolicitud = cmbTipoSolicitud.Text;
                    poObject.CodigoContado = cmbCodContado.EditValue.ToString();
                    poObject.CodigoEstatusSeguro = cmbEstatusSeguro.EditValue.ToString();
                    poObject.CodigoTipoPersona = cmbTipoPersona.EditValue.ToString();
                    poObject.CodigoTipoSolicitud = cmbTipoSolicitud.EditValue.ToString();
                    poObject.Observacion = txtObservacion.Text;
                    poObject.CodigoCliente = cmbCliente.EditValue.ToString();
                    poObject.Cliente = cmbCliente.Text;
                    poObject.RepresentanteLegal = txtCliente.Text;
                    poObject.Completado = chPlazoCorto.Checked;
                    poObject.CupoSap = !string.IsNullOrEmpty(txtCupoSAP.EditValue.ToString()) ? decimal.Parse(txtCupoSAP.EditValue.ToString()) : 0;
                    poObject.CupoSolicitado = !string.IsNullOrEmpty(txtCupoSolicitado.EditValue.ToString()) ? decimal.Parse(txtCupoSolicitado.EditValue.ToString()) : 0;
                    poObject.PlazoSap = txtPlazoSap.Text;
                    poObject.PlazoSolicitado = !string.IsNullOrEmpty(txtPlazoSolicitado.EditValue.ToString()) ? int.Parse(txtPlazoSolicitado.EditValue.ToString()) : 0;
                    poObject.Observacion = txtObservacion.Text;
                    poObject.ComentarioRevisor = txtComentarioRevisor.Text;
                    poObject.ComentarioResolucion = txtComentarioResolucion.Text;
                    poObject.EnviarRevision = chbEnviarRevision.Checked;
                    poObject.CerrarRequerimientoPorVigencia = !chbDocumentosCompletos.Checked;

                    if (cmbEstado.EditValue == null)
                    {
                        poObject.CodigoEstado = Diccionario.Pendiente;
                    }
                    else
                    {
                        poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                    }
                    //if (!string.IsNullOrEmpty(txtNoForms.Text))
                    //{
                    //    poObject.IdReferenciaForm = int.Parse(txtNoForms.Text);
                    //}

                    poObject.ProcesoCreditoDetalle = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                    poObject.ProcesoCreditoAdjunto = (List<ProcesoCreditoAdjunto>)bsArchivoAdjunto.DataSource;

                    var poLista = new List<ProcesoCredito>();

                    poLista.Add(poObject);
                    int pId = 0;
                    string psMsg = loLogicaNegocio.gsGuardar(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, out pId);

                    if (string.IsNullOrEmpty(psMsg))
                    {
                        if (sender != null)
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                            txtNo.Text = pId.ToString();
                            lConsultar();
                        }
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
                        loLogicaNegocio.gEliminar(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show("¿Esta seguro de Cerrar Requerimiento?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var psMsg = loLogicaNegocio.gsCerrar(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show("Requerimiento Cerrado", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.ToString()))
                {
                    int lId = int.Parse(txtNo.Text);


                    //DataSet ds = new DataSet();
                    var ds = loLogicaNegocio.goConsultaDataSet("EXEC CRESPRPTINFORMEREVISION " + "'" + lId + "'");

                    ds.Tables[0].TableName = "Cab";
                    ds.Tables[1].TableName = "Dir";
                    ds.Tables[2].TableName = "Nom";
                    ds.Tables[3].TableName = "Cer";
                    ds.Tables[4].TableName = "Ban";
                    ds.Tables[5].TableName = "Com";
                    ds.Tables[6].TableName = "Bur";
                    ds.Tables[7].TableName = "Jud";
                    ds.Tables[8].TableName = "Sri";
                    ds.Tables[9].TableName = "Acc";
                    ds.Tables[10].TableName = "Adm";

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        xrptInfomeRevision xrpt = new xrptInfomeRevision();
                        xrpt.DataSource = ds;
                        xrpt.RequestParameters = false;

                        using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                        {
                            printTool.ShowRibbonPreviewDialog();
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existe detalles guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

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

        /// <summary>
        /// Evento del boton de Visulizar en el Grid, comentarios de las aprobaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShowComentarios_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.Checklist, poLista[piIndex].IdProcesoCreditoDetalle);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Trazabilidad del documento: " + poLista[piIndex].CheckList };
                    pofrmBuscar.ShowDialog();

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
        private void rpiBtnTrazabilidadAdjunto_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarTrazabilidadAdjunto(Diccionario.Tablas.Transaccion.CheckListAdjunto, poLista[piIndex].IdProcesoCreditoDetalle);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Trazabilidad del Adjuntos" };
                    pofrmBuscar.ShowDialog();

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
        private void rpiBtnTrazabilidadFechaCompromiso_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarTrazabilidadCampo(Diccionario.Tablas.Transaccion.CheckListFechaCompromiso, poLista[piIndex].IdProcesoCreditoDetalle);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt, null, null, true) { Text = "Trazabilidad de Fechas de Compromiso" };
                    pofrmBuscar.ShowDialog();

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
        private void rpiBtnTrazabilidadComentarios_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarTrazabilidadCampo(Diccionario.Tablas.Transaccion.CheckListComentarios, poLista[piIndex].IdProcesoCreditoDetalle);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt, null, null, true) { Text = "Trazabilidad de Comentarios"};
                    pofrmBuscar.ShowDialog();

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
        private void rpiBtnCorregir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                if (piIndex >= 0)
                {
                    if (!lbCerrado)
                    {
                        if (poLista[piIndex].CodigoEstado != Diccionario.Aprobado && poLista[piIndex].CodigoEstado != Diccionario.Cerrado)
                        {
                            if (poLista[piIndex].CodigoEstado != Diccionario.Corregir)
                            {
                                DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de enviar a corregir Checklist: " + poLista[piIndex].CheckList + ".?", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    var result = XtraInputBox.Show("Ingrese comentario", "Corregir", "");
                                    if (string.IsNullOrEmpty(result))
                                    {
                                        XtraMessageBox.Show("Debe agregar comentario para poder corregir", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    var psMess = loLogicaNegocio.gsActualzarChecklistCorregir(poLista[piIndex].IdProcesoCreditoDetalle, result, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                    if (psMess != "")
                                    {
                                        XtraMessageBox.Show(psMess, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        lConsultar();
                                    }
                                }
                            }
                            else
                            {
                                XtraMessageBox.Show("No es posible Mandar a Corregir, CheckList ya fue enviado a corregir.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("No es posible Mandar a Corregir, CheckList ya aprobado.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Evento del boton de Visulizar en el Grid, comentarios de las aprobaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnAprobar_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                if (piIndex >= 0)
                {
                    if (!lbCerrado)
                    {
                        if (poLista[piIndex].CodigoEstado != Diccionario.Aprobado && poLista[piIndex].CodigoEstado != Diccionario.Cerrado) 
                        {
                            DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de aprobar Checklist: " + poLista[piIndex].CheckList + ".?", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                            if (dialogResult == DialogResult.Yes)
                            {

                                var psMess = loLogicaNegocio.gsActualzarChecklistAprobar(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                if (psMess != "")
                                {
                                    XtraMessageBox.Show(psMess, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lConsultar();
                                }
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("No es posible APROBAR, CheckList ya aprobado.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Evento del boton de Visulizar en el Grid, comentarios de las aprobaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDesAprobar_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                if (piIndex >= 0)
                {
                    if (!lbCerrado)
                    {
                        if (poLista[piIndex].CodigoEstado == Diccionario.Aprobado)
                        {
                            DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de aprobar Checklist: " + poLista[piIndex].CheckList + ".?", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                            if (dialogResult == DialogResult.Yes)
                            {

                                var psMess = loLogicaNegocio.gsActualzarChecklistDesAprobar(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                if (psMess != "")
                                {
                                    XtraMessageBox.Show(psMess, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lConsultar();
                                }
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("Solo se puede DESAPROBAR cuando el estado está APROBADO.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Evento del boton de Visulizar en el Grid, comentarios de las aprobaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnExcepcion_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                if (piIndex >= 0)
                {
                    if (!lbCerrado)
                    {
                        if (poLista[piIndex].CodigoEstado != Diccionario.Aprobado)
                        {
                            DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de Exceptuar Checklist: " + poLista[piIndex].CheckList + ".?", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                            if (dialogResult == DialogResult.Yes)
                            {

                                var result = XtraInputBox.Show("Ingrese comentario", "Excepción", "");
                                if (string.IsNullOrEmpty(result))
                                {
                                    XtraMessageBox.Show("Debe agregar comentario para poder Exceptuar", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                var psMess = loLogicaNegocio.gsActualzarChecklistExcepcion(poLista[piIndex].IdProcesoCreditoDetalle, result, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                if (psMess != "")
                                {
                                    XtraMessageBox.Show(psMess, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lConsultar();
                                }
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("Solo se puede EXCEPTUAR cuando el estado está APROBADO.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Evento del boton de Visulizar en el Grid, comentarios de las aprobaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnEnEspera_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                if (piIndex >= 0)
                {
                    if (!lbCerrado)
                    {
                        if (poLista[piIndex].CodigoEstado != Diccionario.Aprobado && poLista[piIndex].CodigoEstado != Diccionario.Cerrado)
                        {
                            DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de poner en espera el documento: " + poLista[piIndex].CheckList + ".?", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                            if (dialogResult == DialogResult.Yes)
                            {

                                var result = XtraInputBox.Show("Ingrese algún comentario", "Opcional", "");

                                var psMess = loLogicaNegocio.gsActualzarChecklistEnEspera(poLista[piIndex].IdProcesoCreditoDetalle, result, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                                if (psMess != "")
                                {
                                    XtraMessageBox.Show(psMess, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lConsultar();
                                }
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("Solo se puede Poner en Espera cuando el estado está APROBADO.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Evento del boton de eliminar en el Grid, elimina la fila seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDelAdj_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (!lbCerrado)
                {
                    List<int> piListId = new List<int>();
                    piListId.Add(1);
                    piListId.Add(2);
                    piListId.Add(5);
                    piListId.Add(6);
                    piListId.Add(3);
                    piListId.Add(9);
                    piListId.Add(12);
                    piListId.Add(11);
                    piListId.Add(14);
                    piListId.Add(7);
                    piListId.Add(10);
                    piListId.Add(13);

                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;

                    if (poLista[piIndex].CodigoEstado != Diccionario.Aprobado && poLista[piIndex].CodigoEstado != Diccionario.Cerrado)
                    {
                        if (poLista.Count > 0 && piIndex >= 0 && !string.IsNullOrEmpty(poLista[piIndex].NombreOriginal))
                        {
                            DialogResult dialogResult = XtraMessageBox.Show("¿Desea eliminar documento adjunto?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                            if (dialogResult == DialogResult.Yes)
                            {
                                poLista[piIndex].ArchivoAdjunto = "";
                                poLista[piIndex].RutaOrigen = "";
                                poLista[piIndex].NombreOriginal = "";
                                poLista[piIndex].RutaDestino = "";
                                poLista[piIndex].Completado = "NO";
                                if (!piListId.Contains(poLista[piIndex].IdCheckList))
                                {
                                    if (poLista[piIndex].CodigoEstado == Diccionario.Cargado)
                                    {
                                        poLista[piIndex].CodigoEstado = Diccionario.Pendiente;
                                    }
                                }
                                // Asigno mi nueva lista al Binding Source
                                bsCheckLisk.DataSource = poLista;
                                dgvCheckList.RefreshData();

                                btnGrabar_Click(null, null);
                            }
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
        /// Evento del boton de eliminar en el Grid, elimina la fila seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {

                if (!lbCerrado)
                {

                    if (xtraTabControl1.SelectedTabPageIndex == 0)
                    {
                        int piIndex;
                        // Tomamos la fila seleccionada
                        piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                        // Tomamos la lista del Grid
                        var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;

                        if (poLista.Count > 0 && piIndex >= 0)
                        {
                            if (!loLogicaNegocio.gbCheckListObligatorio(cmbTipoSolicitud.EditValue.ToString(), cmbTipoPersona.EditValue.ToString(), cmbCodContado.EditValue.ToString(), cmbEstatusSeguro.EditValue.ToString(), poLista[piIndex].IdCheckList, string.IsNullOrEmpty(txtNo.Text) ? 0 : int.Parse(txtNo.Text)))
                            {
                                if (poLista[piIndex].Del == "NO" && poLista[piIndex].IdCheckList == 4) // Cedula si se puede eliminar
                                {
                                    // Tomamos la entidad de la fila seleccionada
                                    var poEntidad = poLista[piIndex];

                                    // Eliminar Fila seleccionada de mi lista
                                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                                    poLista.RemoveAt(piIndex);

                                    // Asigno mi nueva lista al Binding Source
                                    bsCheckLisk.DataSource = poLista;
                                    dgvCheckList.RefreshData();
                                }
                                else if (poLista[piIndex].Del != "NO")
                                {
                                    // Tomamos la entidad de la fila seleccionada
                                    var poEntidad = poLista[piIndex];

                                    // Eliminar Fila seleccionada de mi lista
                                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                                    poLista.RemoveAt(piIndex);

                                    // Asigno mi nueva lista al Binding Source
                                    bsCheckLisk.DataSource = poLista;
                                    dgvCheckList.RefreshData();
                                }
                                else
                                {
                                    XtraMessageBox.Show("No es posible eliminar, CheckList Obligatorio", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                XtraMessageBox.Show("No es posible eliminar, CheckList Obligatorio", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    if (xtraTabControl1.SelectedTabPageIndex == 1)
                    {
                        int piIndex;
                        // Tomamos la fila seleccionada
                        piIndex = dgvArchivoAdjunto.GetFocusedDataSourceRowIndex();
                        // Tomamos la lista del Grid
                        var poLista = (List<ProcesoCreditoAdjunto>)bsArchivoAdjunto.DataSource;

                        if (poLista.Count > 0 && piIndex >= 0)
                        {
                            // Tomamos la entidad de la fila seleccionada
                            var poEntidad = poLista[piIndex];

                            // Eliminar Fila seleccionada de mi lista
                            //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                            poLista.RemoveAt(piIndex);

                            // Asigno mi nueva lista al Binding Source
                            bsArchivoAdjunto.DataSource = poLista;
                            dgvArchivoAdjunto.RefreshData();
                        }
                    }

                    
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lSetCargado(ProcesoCreditoDetalle toObject)
        {
            List<int> piListId = new List<int>();
            piListId.Add(1);
            piListId.Add(2);
            piListId.Add(5);
            piListId.Add(6);
            piListId.Add(3);
            piListId.Add(9);

            if (!piListId.Contains(toObject.IdCheckList))
            {
                loLogicaNegocio.gsActualzarChecklistCorregir(toObject.IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
            }
        }

        /// <summary>
        /// Evento del boton de Agregar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnAdd_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (!lbCerrado)
                {

                    if (xtraTabControl1.SelectedTabPageIndex == 0)
                    {
                        List<int> piListId = new List<int>();
                        //piListId.Add(1);
                        //piListId.Add(2);
                        //piListId.Add(5);
                        //piListId.Add(6);
                        //piListId.Add(3);
                        //piListId.Add(9);
                        //piListId.Add(12);
                        //piListId.Add(11);
                        //piListId.Add(14);
                        //piListId.Add(7);
                        //piListId.Add(10);
                        //piListId.Add(13);
                        bool pbEntro = false;
                        int piIndex;
                        // Tomamos la fila seleccionada
                        piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                        // Tomamos la lista del Grid
                        var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                        if (!string.IsNullOrEmpty(poLista[piIndex].CodigoEstado))
                        {
                            if (poLista[piIndex].CodigoEstado != Diccionario.Aprobado && poLista[piIndex].CodigoEstado != Diccionario.Cerrado)
                            {
                                var dt = loLogicaNegocio.goConsultaDataTable(string.Format("SELECT AtcEntry FROM SBO_AFECOR.dbo.OCRD (NOLOCK) WHERE CardCode = '{0}'", cmbCliente.EditValue.ToString()));
                                if (dt.Rows.Count > 0)
                                {
                                    lsActEntry = dt.Rows[0][0].ToString();

                                    //var dt2 = loLogicaNegocio.goConsultaDataTable(string.Format("SELECT trgtPath Ruta,FileName Archivo,FileExt Extension,CONCAT(trgtPath,'\\',FileName,'.',FileExt) AS 'VerAdjunto' FROM SBO_AFECOR.dbo.ATC1 (NOLOCK) WHERE AbsEntry = {0}", string.IsNullOrEmpty(lsActEntry) ? "0" : lsActEntry));
                                    var dt2 = loLogicaNegocio.goConsultaDataTable(string.Format("SELECT trgtPath Ruta,Date Fecha,FileName Archivo,FileExt Extension,CONCAT(trgtPath,'\\',FileName,'.',FileExt) AS 'VerAdjunto' FROM SBO_AFECOR.dbo.ATC1 (NOLOCK) WHERE AbsEntry = {0}", string.IsNullOrEmpty(lsActEntry) ? "0" : lsActEntry));

                                    if (dt2.Rows.Count > 0)
                                    {
                                        DialogResult dialogResult = XtraMessageBox.Show("¿Desea visualizar los Anexos de SAP?", "Buscar adjuntos en SAP", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                                        if (dialogResult == DialogResult.Yes)
                                        {
                                            frmBusqueda frm = new frmBusqueda(dt2) { Text = "Anexos en SAP" };
                                            frm.Width = 1200;
                                            if (frm.ShowDialog() == DialogResult.OK)
                                            {
                                                string Name = frm.lsTerceraColumna + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff") + "." + frm.lsCuartaColumna;

                                                poLista[piIndex].ArchivoAdjunto = Name;
                                                poLista[piIndex].RutaOrigen = frm.lsCodigoSeleccionado + "\\" + frm.lsTerceraColumna + "." + frm.lsCuartaColumna;
                                                poLista[piIndex].NombreOriginal = frm.lsTerceraColumna + "." + frm.lsCuartaColumna;
                                                poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaProCre"].ToString() + Name;
                                                poLista[piIndex].Completado = "SI";
                                                if (!piListId.Contains(poLista[piIndex].IdCheckList))
                                                {
                                                    poLista[piIndex].CodigoEstado = Diccionario.Cargado;
                                                }
                                                // Asigno mi nueva lista al Binding Source
                                                bsCheckLisk.DataSource = poLista;
                                                dgvCheckList.RefreshData();

                                                pbEntro = true;

                                                btnGrabar_Click(null, null);


                                            }
                                        }
                                    }
                                }


                                if (!pbEntro)
                                {
                                    // Presenta un dialogo para seleccionar las imagenes
                                    OpenFileDialog openFileDialog = new OpenFileDialog();
                                    openFileDialog.Title = "Seleccione Archivo";
                                    openFileDialog.Filter = "Image Files( *.pdf; )|*.pdf;";

                                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                                    {
                                        if (poLista.Count > 0 && piIndex >= 0)
                                        {

                                            if (!openFileDialog.FileName.Equals(""))
                                            {
                                                FileInfo file = new FileInfo(openFileDialog.FileName);
                                                var piSize = file.Length;

                                                if (piSize <= clsPrincipal.gdcTamanoMb * 1048576)
                                                {
                                                    string Name = file.Name + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff") + Path.GetExtension(openFileDialog.FileName);
                                                    var poEntidad = poLista[piIndex];

                                                    poLista[piIndex].ArchivoAdjunto = Name;
                                                    poLista[piIndex].RutaOrigen = openFileDialog.FileName;
                                                    poLista[piIndex].NombreOriginal = file.Name;
                                                    poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaProCre"].ToString() + Name;
                                                    poLista[piIndex].Completado = "SI";
                                                    if (!piListId.Contains(poLista[piIndex].IdCheckList))
                                                    {
                                                        poLista[piIndex].CodigoEstado = Diccionario.Cargado;
                                                    }
                                                    // Asigno mi nueva lista al Binding Source
                                                    bsCheckLisk.DataSource = poLista;
                                                    dgvCheckList.RefreshData();

                                                    btnGrabar_Click(null, null);
                                                }
                                                else
                                                {
                                                    XtraMessageBox.Show("El tamano máximo permitido es de: " + clsPrincipal.gdcTamanoMb + "mb", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                XtraMessageBox.Show("Check List aprobado no es posible modificar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("No es posible continuar sin generar el No. de Proceso, Clic en el botón Guardar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        int piIndex;
                        // Tomamos la fila seleccionada
                        piIndex = dgvArchivoAdjunto.GetFocusedDataSourceRowIndex();
                        // Tomamos la lista del Grid
                        var poLista = (List<ProcesoCreditoAdjunto>)bsArchivoAdjunto.DataSource;

                        OpenFileDialog ofdArchicoAdjunto = new OpenFileDialog();

                        // Presenta un dialogo para seleccionar las imagenes
                        ofdArchicoAdjunto.Title = "Seleccione Archivo";
                        ofdArchicoAdjunto.Filter = "Image Files( *.*; ) | *.*";

                        if (ofdArchicoAdjunto.ShowDialog() == DialogResult.OK)
                        {
                            if (poLista.Count > 0 && piIndex >= 0)
                            {

                                if (!ofdArchicoAdjunto.FileName.Equals(""))
                                {
                                    FileInfo file = new FileInfo(ofdArchicoAdjunto.FileName);
                                    var piSize = file.Length;

                                    if (piSize <= clsPrincipal.gdcTamanoMb * 1048576)
                                    {
                                        string Name = file.Name + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff") + Path.GetExtension(ofdArchicoAdjunto.FileName);
                                        var poEntidad = poLista[piIndex];

                                        poLista[piIndex].ArchivoAdjunto = Name;
                                        poLista[piIndex].RutaOrigen = ofdArchicoAdjunto.FileName;
                                        poLista[piIndex].NombreOriginal = file.Name;
                                        poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaProCreAdj"].ToString() + Name;
                                        // Asigno mi nueva lista al Binding Source
                                        bsArchivoAdjunto.DataSource = poLista;
                                        dgvArchivoAdjunto.RefreshData();
                                    }

                                    else
                                    {
                                        XtraMessageBox.Show("El tamano máximo permitido es de: " + clsPrincipal.gdcTamanoMb + "mb", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                        }
                    }

                    
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Evento del boton de Agregar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnView_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                if (!string.IsNullOrEmpty(poLista[piIndex].ArchivoAdjunto))
                {
                    string psRuta = "";
                    //Muestra archivo local
                    if (!string.IsNullOrEmpty(poLista[piIndex].RutaOrigen))
                    {
                        psRuta = poLista[piIndex].RutaOrigen;
                    }
                    //Muestra archivo ya subido
                    else
                    {
                        psRuta = poLista[piIndex].RutaDestino + poLista[piIndex].ArchivoAdjunto;
                    }

                    if (psRuta.ToLower().Contains(".pdf"))
                    {
                        frmVerPdf pofrmVerPdf = new frmVerPdf();
                        pofrmVerPdf.lsRuta = psRuta;
                        pofrmVerPdf.Show();
                        pofrmVerPdf.SetDesktopLocation(0, 0);
                        pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;
                    }
                    else
                    {
                        Process.Start(psRuta);
                    }

                    if (lbRevision && !lbResolucion)
                    {
                        List<int> piCheck = new List<int>();
                        piCheck.Add(4); piCheck.Add(8); piCheck.Add(15); piCheck.Add(9); piCheck.Add(7);
                        if (piCheck.Contains(poLista[piIndex].IdCheckList))
                        {
                            loLogicaNegocio.gsActualzarChecklistEnRevision(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            lConsultar();
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No hay archivo para mostrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rpiBtnShowArchivo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvArchivoAdjunto.FocusedRowHandle;
                var poLista = (List<ProcesoCreditoAdjunto>)bsArchivoAdjunto.DataSource;

                if (!string.IsNullOrEmpty(poLista[piIndex].ArchivoAdjunto))
                {
                    string psRuta = "";
                    //Muestra archivo local
                    if (!string.IsNullOrEmpty(poLista[piIndex].RutaOrigen))
                    {
                        psRuta = poLista[piIndex].RutaOrigen;
                    }
                    //Muestra archivo ya subido
                    else
                    {
                        psRuta = poLista[piIndex].RutaDestino + poLista[piIndex].ArchivoAdjunto;
                    }

                    if (psRuta.ToLower().Contains(".pdf"))
                    {
                        frmVerPdf pofrmVerPdf = new frmVerPdf();
                        pofrmVerPdf.lsRuta = psRuta;
                        pofrmVerPdf.Show();
                        pofrmVerPdf.SetDesktopLocation(0, 0);
                        pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;
                    }
                    else
                    {
                        Process.Start(psRuta);
                    }
                }

                else
                {
                    XtraMessageBox.Show("No hay archivo para mostrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rpiBtnDownload_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {

                int piIndex = dgvArchivoAdjunto.FocusedRowHandle;
                var poLista = (List<ProcesoCreditoAdjunto>)bsArchivoAdjunto.DataSource;

                string psRuta = poLista[piIndex].RutaDestino + poLista[piIndex].ArchivoAdjunto;
                string psNombreArchivo = poLista[piIndex].NombreOriginal;

                if (!string.IsNullOrEmpty(psRuta))
                {
                    if (File.Exists(psRuta))
                    {
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "files (*.pdf)|*.pdf|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 2;
                        saveFileDialog1.RestoreDirectory = true;
                        saveFileDialog1.FileName = psNombreArchivo.Split('.')[0];
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            File.Copy(psRuta, saveFileDialog1.FileName + Path.GetExtension(psRuta));
                            XtraMessageBox.Show("Archivo descargado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Debe guardar cambios para descargar, Archivo no existe en Base de Datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("Debe guardar cambios para descargar, Archivo no existe en Base de Datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;
            if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Click += btnCorregir_Click;
            if (tstBotones.Items["btnCerrar"] != null) tstBotones.Items["btnCerrar"].Click += btnCerrar_Click;

            bsCheckLisk.DataSource = new List<ProcesoCreditoDetalle>();
            gcCheckList.DataSource = bsCheckLisk;

            bsArchivoAdjunto.DataSource = new List<ProcesoCreditoAdjunto>();
            gcArchivoAdjunto.DataSource = bsArchivoAdjunto;

            //bsRefBancaria.DataSource = new List<SolicitudCreditoRefBancaria>();
            //gcReferenciasBancarias.DataSource = bsRefBancaria;

            //bsRefComercial.DataSource = new List<SolicitudCreditoRefComercial>();
            //gcReferenciasComerciales.DataSource = bsRefComercial;

            //bsRefPersonal.DataSource = new List<SolicitudCreditoRefPersonal>();
            //gcReferenciasPersonales.DataSource = bsRefPersonal;

            //bsPropiedades.DataSource = new List<SolicitudCreditoPropiedades>();
            //gcPropiedades.DataSource = bsPropiedades;

            dgvCheckList.OptionsView.RowAutoHeight = true;

            dgvCheckList.Columns["IdProcesoCreditoDetalle"].Visible = false;
            dgvCheckList.Columns["IdProcesoCredito"].Visible = false;
            dgvCheckList.Columns["CodigoEstado"].Visible = false;
            dgvCheckList.Columns["IdCheckList"].Visible = false;
            dgvCheckList.Columns["Completado"].Visible = false;

            dgvCheckList.Columns["CheckList"].OptionsColumn.ReadOnly = true;
            dgvCheckList.Columns["Estado"].OptionsColumn.ReadOnly = true;

            dgvCheckList.Columns["CheckList"].Caption = "Requisito";
            dgvCheckList.Columns["FechaReferencial"].Caption = "Fecha Vigencia";

            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvCheckList.Columns["Transaccion"], "Transacción", Diccionario.ButtonGridImage.show_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnRevision, dgvCheckList.Columns["Revision"], "Revisión", Diccionario.ButtonGridImage.show_16x16, 30);

            dgvCheckList.Columns["Completado"].Width = 30;
            dgvCheckList.Columns["FechaReferencial"].Width = 40;
            dgvCheckList.Columns["FechaCompromiso"].Width = 40;
            dgvCheckList.Columns["Estado"].Width = 40;
            dgvCheckList.Columns["FisicoRecibido"].Width = 30;

            dgvCheckList.Columns["Del"].Visible = false;
            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvCheckList.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnDelAdj, dgvCheckList.Columns["DelAdj"], "Eliminar Adjunto", Diccionario.ButtonGridImage.trash_16x16, 30);

            clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvCheckList.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnView, dgvCheckList.Columns["Ver"], "Visualizar", Diccionario.ButtonGridImage.show_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvCheckList.Columns["VerComentarios"], "Trazabilidad", Diccionario.ButtonGridImage.showhidecomment_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnTrazabilidadAdjunto, dgvCheckList.Columns["TrazAdj"], "Trazabilidad", Diccionario.ButtonGridImage.showhidecomment_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnTrazabilidadFechaCompromiso, dgvCheckList.Columns["TrazFec"], "Trazabilidad", Diccionario.ButtonGridImage.showhidecomment_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnTrazabilidadComentarios, dgvCheckList.Columns["TrazCom"], "Trazabilidad", Diccionario.ButtonGridImage.showhidecomment_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnCorregir, dgvCheckList.Columns["Corregir"], "Corregir", Diccionario.ButtonGridImage.cancel_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnAprobar, dgvCheckList.Columns["Aprobar"], "Aprobar", Diccionario.ButtonGridImage.apply_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnExcepcion, dgvCheckList.Columns["Excepcion"], "Excepcion", Diccionario.ButtonGridImage.removeitem_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnEnEspera, dgvCheckList.Columns["EnEspera"], "Espera", Diccionario.ButtonGridImage.showhidecomment_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnDesAprobar, dgvCheckList.Columns["DesAprobar"], "Desaprobar", Diccionario.ButtonGridImage.deletelist2_16x16, 30);

            dgvCheckList.Columns["RutaDestino"].Visible = false;
            dgvCheckList.Columns["RutaOrigen"].Visible = false;
            dgvCheckList.Columns["FechaReferencial"].Visible = false;
            dgvCheckList.Columns["MontoReferencial"].Visible = false;
            dgvCheckList.Columns["Periodo"].Visible = false;
            dgvCheckList.Columns["ArchivoAdjunto"].Visible = false;
            dgvCheckList.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvCheckList.Columns["NombreOriginal"].Caption = "Adjunto";


            dgvArchivoAdjunto.Columns["IdProcesoCreditoAdjunto"].Visible = false;
            dgvArchivoAdjunto.Columns["IdProcesoCredito"].Visible = false;
            //dgvArchivoAdjunto.Columns["NombreOriginal"].Visible = false;
            dgvArchivoAdjunto.Columns["RutaDestino"].Visible = false;
            dgvArchivoAdjunto.Columns["RutaOrigen"].Visible = false;
            dgvArchivoAdjunto.Columns["ArchivoAdjunto"].Visible = false;
            dgvArchivoAdjunto.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvArchivoAdjunto.Columns["NombreOriginal"].Caption = "Archivo Adjunto";

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvArchivoAdjunto.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);
            clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvArchivoAdjunto.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnDownload, dgvArchivoAdjunto.Columns["Descargar"], "Descargar", Diccionario.ButtonGridImage.download_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowArchivo, dgvArchivoAdjunto.Columns["Visualizar"], "Visualizar", Diccionario.ButtonGridImage.show_16x16);

            xtraTabControl1.SelectedTabPageIndex = 0;

            if (lbResolucion)
            {
                dgvCheckList.Columns["Corregir"].Visible = false;
                dgvCheckList.Columns["Aprobar"].Visible = false;
                dgvCheckList.Columns["DesAprobar"].Visible = false;
            }

            dgvCheckList.Columns["Comentarios"].ColumnEdit = rpiMedDescripcion;
            dgvCheckList.Columns["Comentarios"].Width = 100;

            if (lbResolucion)
            {

            }

            txtPlazoSolicitado.KeyPress += new KeyPressEventHandler(SoloNumeros);
        }

        public void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                pbCargado = false;

                var poObject = loLogicaNegocio.goConsultar(Convert.ToInt32(txtNo.Text.Trim()));

                cmbEstado.EditValue = poObject.CodigoEstado;

                //if (poObject.CodigoEstado == Diccionario.Finalizado)
                //{
                //    btnIrResolucion.Visible = true;
                //}
                //else
                //{
                //    btnIrResolucion.Visible = false;
                //}

                if (lbRevision)
                {
                    lbCerrado = cmbEstado.EditValue.ToString() == Diccionario.Cerrado || cmbEstado.EditValue.ToString() == Diccionario.Finalizado ? true : false;
                }
                else
                {
                    lbCerrado = cmbEstado.EditValue.ToString() == Diccionario.Cerrado || cmbEstado.EditValue.ToString() == Diccionario.EnResolucion || cmbEstado.EditValue.ToString() == Diccionario.Finalizado ? true : false;
                }

                txtNo.EditValue = poObject.IdProcesoCredito;
                int piDias = DateTime.Now.Subtract(poObject.Fecha).Days;
                lblFecha.Text = string.Format("{0} - Req. Tiene {1} Día(s)", poObject.Fecha.ToString("dd/MM/yyyy"), piDias);
                if (poObject.ContadorCerrado != 0)
                {
                    lblContadorCerrado.Text = string.Format("Cliente tiene {0} trámite(s) cerrado(s).", poObject.ContadorCerrado);
                }
                else
                {
                    lblContadorCerrado.Text = "";
                }
                
                lblFechaUltSol.Text = poObject.FechaUltimaSolicitud > DateTime.MinValue ? poObject.FechaUltimaSolicitud.ToString("dd/MM/yyyy") : "";
                lblFechaResolucion.Text = poObject.FechaResolucion == null ? "" : poObject.FechaResolucion?.ToString("dd/MM/yyyy");
                txtCupoAprobado.Text = poObject.CupoAprobado == null ? "" : poObject.CupoAprobado.ToString();
                txtPlazoAprobado.Text = poObject.PlazoAprobado == null ? "" : poObject.PlazoAprobado.ToString();
                cmbTipoSolicitud.EditValue = poObject.CodigoTipoSolicitud;
                cmbCodContado.EditValue = poObject.CodigoContado;
                cmbEstatusSeguro.EditValue = poObject.CodigoEstatusSeguro;
                cmbTipoPersona.EditValue = poObject.CodigoTipoPersona;
                txtObservacion.Text = poObject.Observacion;
                cmbCliente.EditValue = poObject.CodigoCliente;
                txtCliente.Text = poObject.RepresentanteLegal;
                chPlazoCorto.Checked = poObject.Completado;
                cmbRTC.EditValue = poObject.IdRTC.ToString();
                txtZona.Text = poObject.Zona;
                lblUsuarioCreaReq.Text = poObject.NomUsuario;
                chbEnviarRevision.Checked = poObject.EnviarRevision;
                if (poObject.CodigoEstado == Diccionario.Pendiente || poObject.CodigoEstado == Diccionario.EnRevision || poObject.CodigoEstado == Diccionario.Cargado || poObject.CodigoEstado == Diccionario.EnEspera)
                {
                    chbRevisionCompletada.Checked = false;
                }
                else
                {
                    chbRevisionCompletada.Checked = true;
                }
                chbDocumentosCompletos.Checked = !poObject.CerrarRequerimientoPorVigencia;
                txtCupoSAP.EditValue = poObject.CupoSap;
                txtCupoSolicitado.EditValue = poObject.CupoSolicitado;
                txtPlazoSolicitado.EditValue = poObject.PlazoSolicitado;
                txtPlazoSap.EditValue = poObject.PlazoSap;
                txtComentarioRevisor.Text = poObject.ComentarioRevisor;
                txtComentarioResolucion.Text = poObject.ComentarioResolucion;

                chbEnviarRevision.ReadOnly = true;
                var psListaCheckList = new List<int>();
                if (poObject.ProcesoCreditoDetalle.Where(x => x.IdCheckList == 1).Count() > 0) psListaCheckList.Add(1); // Plantilla Calif. Seguro
                if (poObject.ProcesoCreditoDetalle.Where(x => x.IdCheckList == 2).Count() > 0) psListaCheckList.Add(2); // Solicitud de Credito
                if (poObject.ProcesoCreditoDetalle.Where(x => x.IdCheckList == 3).Count() > 0) psListaCheckList.Add(3); // Cedula titular/Rep. Legal
                if (poObject.ProcesoCreditoDetalle.Where(x => x.IdCheckList == 5).Count() > 0) psListaCheckList.Add(5); // RUC/CERT.SRI.RUC
                if (poObject.ProcesoCreditoDetalle.Where(x => x.IdCheckList == 12).Count() > 0) psListaCheckList.Add(12); // Informe RTC
                if (poObject.ProcesoCreditoDetalle.Where(x => psListaCheckList.Contains(x.IdCheckList) && x.CodigoEstado == Diccionario.Pendiente).Count() == 0 || chPlazoCorto.Checked)
                {
                    chbEnviarRevision.ReadOnly = false;
                }

                bsCheckLisk.DataSource = poObject.ProcesoCreditoDetalle;
                dgvCheckList.RefreshData();

                bsArchivoAdjunto.DataSource = poObject.ProcesoCreditoAdjunto;
                dgvArchivoAdjunto.RefreshData();



                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
                if (tstBotones.Items["btnCerrar"] != null) tstBotones.Items["btnCerrar"].Enabled = true;
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = true;

                if (poObject.CodigoEstado == Diccionario.Finalizado)
                {
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                    if (tstBotones.Items["btnCerrar"] != null) tstBotones.Items["btnCerrar"].Enabled = false;
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                    if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = false;
                    chbRevisionCompletada.Enabled = false;
                }
                else
                {
                    chbRevisionCompletada.Enabled = true;
                    if (poObject.CodigoEstado != Diccionario.Pendiente && poObject.CodigoEstado != Diccionario.EnRevision && poObject.CodigoEstado != Diccionario.Cargado && poObject.CodigoEstado != Diccionario.Corregir && poObject.CodigoEstado != Diccionario.EnEspera)
                    {
                        if (!lbRevision)
                        {
                            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                            if (tstBotones.Items["btnCerrar"] != null) tstBotones.Items["btnCerrar"].Enabled = false;
                            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                            if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = false;
                        }
                        else
                        {

                        }
                    }

                    if (lbResolucion)
                    {
                        if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                        if (tstBotones.Items["btnCerrar"] != null) tstBotones.Items["btnCerrar"].Enabled = false;
                        if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                        if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = false;
                    }
                }

                if (lbCerrado)
                {
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                    if (tstBotones.Items["btnCerrar"] != null) tstBotones.Items["btnCerrar"].Enabled = false;
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                    if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = false;
                    dgvCheckList.Columns["Add"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["DelAdj"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["FechaReferencial"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["FechaCompromiso"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["Comentarios"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["FisicoRecibido"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["FechaFisicoRecibido"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["Aprobar"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["Corregir"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["EnEspera"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["Excepcion"].OptionsColumn.ReadOnly = true;
                    dgvCheckList.Columns["DesAprobar"].OptionsColumn.ReadOnly = true;
                    btnAddFila.Enabled = false;
                    btnCopiarDocumentos.Enabled = false;
                }
                else
                {
                    dgvCheckList.Columns["Add"].OptionsColumn.ReadOnly = false;
                    dgvCheckList.Columns["DelAdj"].OptionsColumn.ReadOnly = false;
                    dgvCheckList.Columns["FisicoRecibido"].OptionsColumn.ReadOnly = false;
                    dgvCheckList.Columns["Aprobar"].OptionsColumn.ReadOnly = false;
                    dgvCheckList.Columns["Corregir"].OptionsColumn.ReadOnly = false;
                    dgvCheckList.Columns["EnEspera"].OptionsColumn.ReadOnly = false;
                    dgvCheckList.Columns["Excepcion"].OptionsColumn.ReadOnly = false;
                    dgvCheckList.Columns["DesAprobar"].OptionsColumn.ReadOnly = false;
                    btnAddFila.Enabled = true;
                    btnCopiarDocumentos.Enabled = true;
                }

                cmbRTC.ReadOnly = true;
                //txtZona.ReadOnly = true;
                cmbCliente.ReadOnly = true;
                cmbCodContado.ReadOnly = true;
                cmbEstatusSeguro.ReadOnly = true;
                cmbTipoPersona.ReadOnly = true;
                cmbTipoSolicitud.ReadOnly = true;
                //txtCliente.ReadOnly = true;

                pbCargado = true;
            }
            else
            {
                lLimpiar();
            }
        }

        private void lLimpiar()
        {
            pbCargado = false;
            txtNo.EditValue = "";
            lblFecha.Text = "";
            lblFechaUltSol.Text = "";
            lblFechaResolucion.Text = "";
            lblContadorCerrado.Text = "";
            lblContadorCerrado.Text = "";
            lblUsuarioCreaReq.Text = "";
            txtCupoAprobado.Text = "";
            txtPlazoAprobado.Text = "";
            cmbTipoSolicitud.EditValue = Diccionario.Seleccione;
            cmbCodContado.EditValue = Diccionario.Seleccione;
            cmbRTC.EditValue = Diccionario.Seleccione;
            txtZona.Text = "";
            cmbEstatusSeguro.EditValue = "SIN";
            cmbTipoPersona.EditValue = Diccionario.Seleccione;
            txtObservacion.Text = "";
            cmbCliente.EditValue = Diccionario.Seleccione;
            cmbEstado.EditValue = Diccionario.Seleccione;
            txtCliente.Text = "";
            txtCupoSAP.Text = "";
            txtCupoSolicitado.Text = "";
            txtPlazoSap.Text = "";
            txtPlazoSolicitado.Text = "";
            txtComentarioRevisor.EditValue = "";
            txtComentarioResolucion.EditValue = "";

            cmbCliente.ReadOnly = false;
            cmbRTC.ReadOnly = true;
            //txtZona.ReadOnly = false;
            cmbCodContado.ReadOnly = false;
            cmbEstatusSeguro.ReadOnly = true;
            cmbTipoPersona.ReadOnly = false;
            cmbTipoSolicitud.ReadOnly = false;
            //txtCliente.ReadOnly = false;
            chbRevisionCompletada.Checked = false;
            chbDocumentosCompletos.Checked = false;
            chbEnviarRevision.Checked = false;
            chbEnviarRevision.ReadOnly = false;

            bsCheckLisk.DataSource = new List<ProcesoCreditoDetalle>();
            dgvCheckList.RefreshData();

            bsArchivoAdjunto.DataSource = new List<ProcesoCreditoAdjunto>();
            gcArchivoAdjunto.DataSource = bsArchivoAdjunto;

            btnAddFila.Enabled = true;
            btnCopiarDocumentos.Enabled = true;

            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
            if (tstBotones.Items["btnCerrar"] != null) tstBotones.Items["btnCerrar"].Enabled = true;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
            if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = true;
            lbCerrado = false;
            pbCargado = true;

            xtraTabControl1.SelectedTabPageIndex = 0;

        }
        #endregion

        private void frmTrProcesoCredito_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

        private void Verificar_CheckList(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    if (cmbTipoSolicitud.EditValue.ToString() == Diccionario.ListaCatalogo.TipoSolicitudCreditoClass.AumentoCupo)
                    {
                        txtCupoSolicitado.Enabled = true;
                        txtPlazoSolicitado.Enabled = true;
                    }
                    else if (cmbTipoSolicitud.EditValue.ToString() == Diccionario.ListaCatalogo.TipoSolicitudCreditoClass.AumentoPlazo)
                    {
                        txtCupoSolicitado.Enabled = false;
                        txtPlazoSolicitado.Enabled = true;
                    }
                    else if (cmbTipoSolicitud.EditValue.ToString() == Diccionario.ListaCatalogo.TipoSolicitudCreditoClass.Actualizacion)
                    {
                        txtCupoSolicitado.Enabled = false;
                        txtPlazoSolicitado.Enabled = false;
                    }
                    else
                    {
                        txtCupoSolicitado.Enabled = true;
                        txtPlazoSolicitado.Enabled = true;
                    }

                    //int CountReg = loLogicaNegocio.giCuantaRequerimientos(cmbTipoSolicitud.EditValue.ToString(), cmbCliente.EditValue.ToString());
                    //if (CountReg > 0)
                    //{
                    //    XtraMessageBox.Show("Ya existe un requerimiento de crédito con tipo de proceso: " + cmbTipoSolicitud.Text, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                }

                lCheckList();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del boton de Agregar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
                {
                    int piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                    if (!string.IsNullOrEmpty(poLista[piIndex].CodigoEstado))
                    {
                        if (poLista[piIndex].IdCheckList == 1)
                        {
                            string psForma = Diccionario.Tablas.Menu.frmTrPlantillaSeguro;
                            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                            if (poForm != null)
                            {
                                frmTrPantillaSeguro frm = new frmTrPantillaSeguro(this);
                                frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                                frm.Text = poForm.Nombre;
                                frm.lIdProceso = int.Parse(txtNo.Text);
                                frm.lsCardCode = cmbCliente.EditValue.ToString();
                                frm.lsTipoProceso = cmbTipoSolicitud.EditValue.ToString();
                                frm.ldcCupoSolicitado = Convert.ToDecimal(txtCupoSolicitado.EditValue);
                                frm.liPlazoSolicitado = Convert.ToInt32(txtPlazoSolicitado.EditValue);
                                frm.lbCerrado = lbCerrado;
                                frm.Show();
                                //if (frm.lbGuardado)
                                //{
                                //    //loLogicaNegocio.gsActualzarRequerimientoCredito(int.Parse(txtNo.Text), "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                                //    //lConsultar();
                                //}
                                //else
                                //{
                                //    bsCheckLisk.DataSource = poLista;
                                //    dgvCheckList.RefreshData();
                                //}
                            }
                            else
                            {
                                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (poLista[piIndex].IdCheckList == 2)
                        {
                            string psForma = Diccionario.Tablas.Menu.frmTrSolicitudCredito;
                            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                            if (poForm != null)
                            {
                                frmTrSolicitudCredito frm = new frmTrSolicitudCredito(this);
                                frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                                frm.Text = poForm.Nombre;
                                frm.lIdProceso = int.Parse(txtNo.Text);
                                frm.lsCardCode = cmbCliente.EditValue.ToString();
                                frm.lsTipoProceso = cmbTipoSolicitud.EditValue.ToString();
                                frm.lsCliente = txtCliente.Text;
                                frm.ldcCupoSAP = decimal.Parse(txtCupoSAP.EditValue.ToString());
                                frm.ldcCupoSolicitado = decimal.Parse(txtCupoSolicitado.EditValue.ToString());
                                frm.liPlazoSolicitado = int.Parse(txtPlazoSolicitado.EditValue.ToString());
                                frm.lsRtc = cmbRTC.EditValue.ToString();
                                frm.lsZona = txtZona.Text;
                                frm.lbCerrado = lbCerrado;
                                frm.Show();
                                //loLogicaNegocio.gsActualzarRequerimientoCredito(int.Parse(txtNo.Text), "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                                //lConsultar();
                            }
                            else
                            {
                                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (poLista[piIndex].IdCheckList == 5)
                        {
                            string psForma = Diccionario.Tablas.Menu.frmTrRevisionRuc;
                            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                            if (poForm != null)
                            {
                                frmTrRevisionRuc frm = new frmTrRevisionRuc(this);
                                frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                                frm.Text = poForm.Nombre;
                                frm.lsTipoRuc = cmbTipoPersona.EditValue.ToString();
                                frm.lIdProceso = int.Parse(txtNo.Text);
                                frm.lsTipoProceso = cmbTipoSolicitud.EditValue.ToString();
                                frm.lsTipo = "REQ";
                                frm.Height = 350;
                                frm.lbCerrado = lbCerrado;
                                frm.Show();
                                //loLogicaNegocio.gsActualzarRequerimientoCredito(int.Parse(txtNo.Text), "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                                //lConsultar();
                                //bsCheckLisk.DataSource = poLista;
                                //dgvCheckList.RefreshData();
                            }
                            else
                            {
                                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (poLista[piIndex].IdCheckList == 6)
                        {
                            string psForma = Diccionario.Tablas.Menu.frmTrRevisionNombramiento;
                            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                            if (poForm != null)
                            {
                                frmTrRevisionNombramiento frm = new frmTrRevisionNombramiento(this);
                                frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                                frm.Text = poForm.Nombre;
                                frm.lsTipoRuc = cmbTipoPersona.EditValue.ToString();
                                frm.lIdProceso = int.Parse(txtNo.Text);
                                frm.lsTipoProceso = cmbTipoSolicitud.EditValue.ToString();
                                frm.lsTipo = "REQ";
                                frm.lbCerrado = lbCerrado;
                                frm.Show();
                                //loLogicaNegocio.gsActualzarRequerimientoCredito(int.Parse(txtNo.Text), "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                                //lConsultar();
                                //bsCheckLisk.DataSource = poLista;
                                //dgvCheckList.RefreshData();
                            }
                            else
                            {
                                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (poLista[piIndex].IdCheckList == 3)
                        {
                            string psForma = Diccionario.Tablas.Menu.frmTrRevisionCedula;
                            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                            if (poForm != null)
                            {
                                frmTrRevisionCedula frm = new frmTrRevisionCedula(this);
                                frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                                frm.Text = poForm.Nombre;
                                frm.lsTipoRuc = cmbTipoPersona.EditValue.ToString();
                                frm.lIdProceso = int.Parse(txtNo.Text);
                                frm.lsTipoProceso = cmbTipoSolicitud.EditValue.ToString();
                                frm.lsTipo = "REQ";
                                frm.lbCerrado = lbCerrado;
                                frm.Show();
                                //loLogicaNegocio.gsActualzarRequerimientoCredito(int.Parse(txtNo.Text), "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                                //lConsultar();
                                //bsCheckLisk.DataSource = poLista;
                                //dgvCheckList.RefreshData();
                            }
                            else
                            {
                                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (poLista[piIndex].IdCheckList == 12)
                        {
                            string psForma = Diccionario.Tablas.Menu.frmTrInformeRTC;
                            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                            if (poForm != null)
                            {
                                frmTrInformeRTC frm = new frmTrInformeRTC(this);
                                frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                                frm.Text = poForm.Nombre;
                                frm.lIdProceso = int.Parse(txtNo.Text);
                                frm.lsCardCode = cmbCliente.EditValue.ToString();
                                frm.lsTipoProceso = cmbTipoSolicitud.EditValue.ToString();
                                frm.lsCliente = txtCliente.Text;
                                frm.ldcCupoSAP = decimal.Parse(txtCupoSAP.EditValue.ToString());
                                frm.ldcCupoSolicitado = decimal.Parse(txtCupoSolicitado.EditValue.ToString());
                                frm.liPlazoSolicitado = int.Parse(txtPlazoSolicitado.EditValue.ToString());
                                frm.lsTipoPersona = cmbTipoPersona.EditValue.ToString();
                                frm.lsRtc = cmbRTC.EditValue.ToString();
                                frm.lsZona = txtZona.Text;
                                frm.lbCerrado = lbCerrado;
                                frm.Show();
                                //loLogicaNegocio.gsActualzarRequerimientoCredito(int.Parse(txtNo.Text), "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                                //lConsultar();
                            }
                            else
                            {
                                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (poLista[piIndex].IdCheckList == 9)
                        {
                            string psForma = Diccionario.Tablas.Menu.frmTrRevisionPlanillaServicioBasico;
                            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                            if (poForm != null)
                            {
                                frmTrRevisionPlanillaServicioBasico frm = new frmTrRevisionPlanillaServicioBasico(this);
                                frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                                frm.Text = poForm.Nombre;
                                //frm.lsTipo = "REQ";
                                frm.lIdProceso = int.Parse(txtNo.Text);
                                frm.lbCerrado = lbCerrado;
                                frm.Show();
                                //loLogicaNegocio.gsActualzarRequerimientoCredito(int.Parse(txtNo.Text), "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                                //lConsultar();
                            }
                            else
                            {
                                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (poLista[piIndex].IdCheckList == 11)
                        {
                            string psForma = Diccionario.Tablas.Menu.frmTrRevisionRefBancaria;
                            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                            if (poForm != null)
                            {
                                frmTrRevisionRefBancaria frm = new frmTrRevisionRefBancaria(this);
                                frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                                frm.Text = poForm.Nombre;
                                frm.lsTipo = "REQ";
                                frm.lIdProceso = int.Parse(txtNo.Text);
                                frm.lbRevision = lbResolucion;
                                frm.lbCerrado = lbCerrado;
                                frm.Show();
                                //lConsultar();
                            }
                            else
                            {
                                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }


                        }

                        else if (poLista[piIndex].IdCheckList == 14)
                        {

                            string psForma = Diccionario.Tablas.Menu.frmTrRevisionRefComercial;
                            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                            if (poForm != null)
                            {
                                frmTrRevisionRefComercial frm = new frmTrRevisionRefComercial(this);
                                frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                                frm.Text = poForm.Nombre;
                                frm.lIdProceso = int.Parse(txtNo.Text);
                                frm.lbRevision = lbResolucion;
                                frm.lsTipo = "REQ";
                                frm.lbCerrado = lbCerrado;
                                lConsultar();
                                frm.Show();
                                //lConsultar();
                            }
                            else
                            {
                                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }


                        }
                        else if (poLista[piIndex].IdCheckList == 10)
                        {
                            string psForma = Diccionario.Tablas.Menu.frmTrRevisionBienes;
                            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                            if (poForm != null)
                            {
                                frmTrRevisionBienes frm = new frmTrRevisionBienes(this);
                                frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                                frm.Text = poForm.Nombre;
                                frm.lsTipo = "REQ";
                                frm.lbRevision = lbResolucion;
                                frm.lIdProceso = int.Parse(txtNo.Text);
                                frm.lbRevision = lbResolucion;
                                frm.lbCerrado = lbCerrado;
                                frm.Show();

                            }
                            else
                            {
                                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (poLista[piIndex].IdCheckList == 7)
                        {

                            frmPeriodo frm = new frmPeriodo();
                            frm.Text = (poLista[piIndex].IdCheckList == 10 ? "Año de Tributación " : "Periodo Fiscal - ") + poLista[piIndex].CheckList;
                            frm.tIdCheckList = poLista[piIndex].IdCheckList;
                            frm.lsNombre = poLista[piIndex].IdCheckList == 10 ? "Año de Tributación " : "";
                            frm.poDetalle = poLista[piIndex];
                            frm.lsTipoProceso = cmbTipoSolicitud.EditValue.ToString();
                            frm.lbCerrado = lbCerrado;
                            frm.ShowDialog();
                            if (frm.pbAcepto)
                            {
                                poLista[piIndex].CodigoEstado = Diccionario.Cargado;
                                btnGrabar_Click(null, null);
                            }
                            loLogicaNegocio.gsActualzarRequerimientoCredito(int.Parse(txtNo.Text), "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            lConsultar();
                        }
                        else if (poLista[piIndex].IdCheckList == 13)
                        {

                            frmMontoPagare frm = new frmMontoPagare();
                            frm.Text = "Monto - " + poLista[piIndex].CheckList;
                            frm.tIdCheckList = poLista[piIndex].IdCheckList;
                            frm.ldcCupoSolicitado = Convert.ToDecimal(txtCupoSolicitado.EditValue);
                            frm.poDetalle = poLista[piIndex];
                            frm.lbCerrado = lbCerrado;
                            frm.ShowDialog();
                            if (frm.pbAcepto)
                            {
                                poLista[piIndex].CodigoEstado = Diccionario.Cargado;
                                btnGrabar_Click(null, null);
                            }
                            loLogicaNegocio.gsActualzarRequerimientoCredito(int.Parse(txtNo.Text), "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            lConsultar();
                        }
                        else
                        {
                            XtraMessageBox.Show(string.Format("No existe transacción para el check List: {0}, Adjunte documento .", poLista[piIndex].CheckList), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No es posible continuar, Clic en el botón Guardar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    XtraMessageBox.Show("No es posible continuar sin generar el No. de Proceso, Clic en el botón Guardar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del boton de Agregar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnRevision_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
                {
                    int piIndex = dgvCheckList.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;

                    if (poLista[piIndex].IdCheckList == 1)
                    {
                        string psForma = Diccionario.Tablas.Menu.frmTrPlantillaSeguroRevision;
                        var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                        if (poForm != null)
                        {
                            frmTrPantillaSeguroRevision frm = new frmTrPantillaSeguroRevision(this);
                            frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            frm.Text = poForm.Nombre;
                            frm.lIdProceso = int.Parse(txtNo.Text);
                            frm.lsCardCode = cmbCliente.EditValue.ToString();
                            frm.lsTipoProceso = cmbTipoSolicitud.EditValue.ToString();
                            frm.lbCerrado = lbCerrado;

                            loLogicaNegocio.gsActualzarChecklistEnRevision(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            lConsultar();
                            frm.Show();

                            //lConsultar();
                            //bsCheckLisk.DataSource = poLista;
                            //dgvCheckList.RefreshData();
                        }
                        else
                        {
                            XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (poLista[piIndex].IdCheckList == 2)
                    {
                        string psForma = Diccionario.Tablas.Menu.frmTrSolicitudCredito;
                        var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                        if (poForm != null)
                        {
                            frmTrSolicitudCredito frm = new frmTrSolicitudCredito(this);
                            frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            frm.Text = poForm.Nombre;
                            frm.lIdProceso = int.Parse(txtNo.Text);
                            frm.lsCardCode = cmbCliente.EditValue.ToString();
                            frm.lsTipoProceso = cmbTipoSolicitud.EditValue.ToString();
                            frm.lsCliente = txtCliente.Text;
                            frm.ldcCupoSAP = decimal.Parse(txtCupoSAP.EditValue.ToString());
                            frm.ldcCupoSolicitado = decimal.Parse(txtCupoSolicitado.EditValue.ToString());
                            frm.liPlazoSolicitado = int.Parse(txtPlazoSolicitado.EditValue.ToString());
                            frm.lbCerrado = lbCerrado;
                            loLogicaNegocio.gsActualzarChecklistEnRevision(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            lConsultar();
                            frm.Show();
                            //lConsultar();
                        }
                        else
                        {
                            XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (poLista[piIndex].IdCheckList == 5)
                    {
                        string psForma = Diccionario.Tablas.Menu.frmTrRevisionRuc;
                        var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                        if (poForm != null)
                        {
                            frmTrRevisionRuc frm = new frmTrRevisionRuc(this);
                            frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            frm.Text = poForm.Nombre;
                            frm.lsTipoRuc = cmbTipoPersona.EditValue.ToString();
                            frm.lIdProceso = int.Parse(txtNo.Text);
                            frm.lsTipoProceso = cmbTipoSolicitud.EditValue.ToString();
                            frm.lbCerrado = lbCerrado;
                            loLogicaNegocio.gsActualzarChecklistEnRevision(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            lConsultar();
                            frm.Show();
                            //lConsultar();
                            //bsCheckLisk.DataSource = poLista;
                            //dgvCheckList.RefreshData();
                        }
                        else
                        {
                            XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (poLista[piIndex].IdCheckList == 6)
                    {
                        string psForma = Diccionario.Tablas.Menu.frmTrRevisionNombramiento;
                        var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                        if (poForm != null)
                        {
                            frmTrRevisionNombramiento frm = new frmTrRevisionNombramiento(this);
                            frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            frm.Text = poForm.Nombre;
                            frm.lsTipoRuc = cmbTipoPersona.EditValue.ToString();
                            frm.lIdProceso = int.Parse(txtNo.Text);
                            frm.lsTipoProceso = cmbTipoSolicitud.EditValue.ToString();
                            frm.lbCerrado = lbCerrado;
                            loLogicaNegocio.gsActualzarChecklistEnRevision(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            lConsultar();
                            frm.Show();
                            //lConsultar();
                            //bsCheckLisk.DataSource = poLista;
                            //dgvCheckList.RefreshData();
                        }
                        else
                        {
                            XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (poLista[piIndex].IdCheckList == 3)
                    {
                        string psForma = Diccionario.Tablas.Menu.frmTrRevisionCedula;
                        var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                        if (poForm != null)
                        {
                            frmTrRevisionCedula frm = new frmTrRevisionCedula(this);
                            frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            frm.Text = poForm.Nombre;
                            frm.lsTipoRuc = cmbTipoPersona.EditValue.ToString();
                            frm.lIdProceso = int.Parse(txtNo.Text);
                            frm.lsTipoProceso = cmbTipoSolicitud.EditValue.ToString();
                            frm.lsTipo = "REV";
                            frm.lbCerrado = lbCerrado;
                            loLogicaNegocio.gsActualzarChecklistEnRevision(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            lConsultar();
                            frm.Show();
                            //lConsultar();
                            //bsCheckLisk.DataSource = poLista;
                            //dgvCheckList.RefreshData();
                        }
                        else
                        {
                            XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    else if (poLista[piIndex].IdCheckList == 12)
                    {
                        string psForma = Diccionario.Tablas.Menu.frmTrInformeRTC;
                        var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                        if (poForm != null)
                        {
                            frmTrInformeRTC frm = new frmTrInformeRTC(this);
                            frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            frm.Text = poForm.Nombre;
                            frm.lIdProceso = int.Parse(txtNo.Text);
                            frm.lsCardCode = cmbCliente.EditValue.ToString();
                            frm.lsTipoProceso = cmbTipoSolicitud.EditValue.ToString();
                            frm.lsCliente = txtCliente.Text;
                            frm.ldcCupoSAP = decimal.Parse(txtCupoSAP.EditValue.ToString());
                            frm.ldcCupoSolicitado = decimal.Parse(txtCupoSolicitado.EditValue.ToString());
                            frm.liPlazoSolicitado = int.Parse(txtPlazoSolicitado.EditValue.ToString());
                            frm.lsTipoPersona = cmbTipoPersona.EditValue.ToString();
                            frm.lsRtc = cmbRTC.EditValue.ToString();
                            frm.lsZona = txtZona.Text;
                            frm.lbCerrado = lbCerrado;
                            loLogicaNegocio.gsActualzarChecklistEnRevision(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            lConsultar();
                            frm.Show();
                            //lConsultar();
                        }
                        else
                        {
                            XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (poLista[piIndex].IdCheckList == 9)
                    {
                        string psForma = Diccionario.Tablas.Menu.frmTrRevisionPlanillaServicioBasico;
                        var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                        if (poForm != null)
                        {
                            frmTrRevisionPlanillaServicioBasico frm = new frmTrRevisionPlanillaServicioBasico(this);
                            frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            frm.Text = poForm.Nombre;
                            frm.lIdProceso = int.Parse(txtNo.Text);
                            frm.lbCerrado = lbCerrado;
                            loLogicaNegocio.gsActualzarChecklistEnRevision(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            lConsultar();
                            frm.Show();
                            //lConsultar();
                        }
                        else
                        {
                            XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (poLista[piIndex].IdCheckList == 11)
                    {
                        string psForma = Diccionario.Tablas.Menu.frmTrRevisionRefBancaria;
                        var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                        if (poForm != null)
                        {
                            frmTrRevisionRefBancaria frm = new frmTrRevisionRefBancaria(this);
                            frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            frm.Text = poForm.Nombre;
                            frm.lIdProceso = int.Parse(txtNo.Text);
                            frm.lbRevision = lbResolucion;
                            frm.lbCerrado = lbCerrado;
                            loLogicaNegocio.gsActualzarChecklistEnRevision(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            lConsultar();
                            frm.Show();
                            //lConsultar();
                        }
                        else
                        {
                            XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (poLista[piIndex].IdCheckList == 14)
                    {
                        string psForma = Diccionario.Tablas.Menu.frmTrRevisionRefComercial;
                        var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                        if (poForm != null)
                        {
                            frmTrRevisionRefComercial frm = new frmTrRevisionRefComercial(this);
                            frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            frm.Text = poForm.Nombre;
                            frm.lIdProceso = int.Parse(txtNo.Text);
                            frm.lbRevision = lbResolucion;
                            frm.lbCerrado = lbCerrado;
                            loLogicaNegocio.gsActualzarChecklistEnRevision(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            lConsultar();
                            frm.Show();
                            //lConsultar();
                        }
                        else
                        {
                            XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (poLista[piIndex].IdCheckList == 10)
                    {
                        string psForma = Diccionario.Tablas.Menu.frmTrRevisionBienes;
                        var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                        if (poForm != null)
                        {
                            frmTrRevisionBienes frm = new frmTrRevisionBienes(this);
                            frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            frm.Text = poForm.Nombre;
                            frm.lbRevision = lbResolucion;
                            frm.lIdProceso = int.Parse(txtNo.Text);
                            frm.lbRevision = lbResolucion;
                            frm.lbCerrado = lbCerrado;
                            loLogicaNegocio.gsActualzarChecklistEnRevision(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            lConsultar();
                            frm.Show();
                            //lConsultar();
                        }
                        else
                        {
                            XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (poLista[piIndex].IdCheckList == 11 || poLista[piIndex].IdCheckList == 14)
                    {

                        frmFechaVigencia frm = new frmFechaVigencia();
                        frm.Text = "Fecha de Vigencia - " + poLista[piIndex].CheckList;
                        frm.tIdCheckList = poLista[piIndex].IdCheckList;
                        frm.lsNombre = poLista[piIndex].IdCheckList == 11 ? "Fecha de Emisión" : "";
                        frm.poDetalle = poLista[piIndex];
                        frm.lbRevision = lbResolucion;
                        frm.lbCerrado = lbCerrado;
                        loLogicaNegocio.gsActualzarChecklistEnRevision(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                        frm.ShowDialog();
                        lConsultar();
                    }
                    else if (poLista[piIndex].IdCheckList == 7 || poLista[piIndex].IdCheckList == 10)
                    {

                        frmPeriodo frm = new frmPeriodo();
                        frm.Text = (poLista[piIndex].IdCheckList == 10 ? "Año de Tributación " : "Periodo Fiscal - ") + poLista[piIndex].CheckList;
                        frm.tIdCheckList = poLista[piIndex].IdCheckList;
                        frm.lsNombre = poLista[piIndex].IdCheckList == 10 ? "Año de Tributación " : "";
                        frm.poDetalle = poLista[piIndex];
                        frm.lsTipoProceso = cmbTipoSolicitud.EditValue.ToString();
                        frm.lbRevision = lbResolucion;
                        frm.lbCerrado = lbCerrado;
                        loLogicaNegocio.gsActualzarChecklistEnRevision(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                        frm.ShowDialog();
                        lConsultar();
                    }
                    else if (poLista[piIndex].IdCheckList == 13)
                    {

                        frmMontoPagare frm = new frmMontoPagare();
                        frm.Text = "Monto - " + poLista[piIndex].CheckList;
                        frm.tIdCheckList = poLista[piIndex].IdCheckList;
                        frm.ldcCupoSolicitado = Convert.ToDecimal(txtCupoSolicitado.EditValue);
                        frm.poDetalle = poLista[piIndex];
                        frm.lbRevision = lbResolucion;
                        frm.lbCerrado = lbCerrado;
                        loLogicaNegocio.gsActualzarChecklistEnRevision(poLista[piIndex].IdProcesoCreditoDetalle, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                        frm.ShowDialog();
                        lConsultar();
                    }
                    else
                    {
                        XtraMessageBox.Show(string.Format("No existe revisión para este check List: {0}, visualice el documento adjunto.", poLista[piIndex].CheckList), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No es posible continuar sin generar el No. de Proceso, Clic en el botón Guardar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void lCheckList()
        {
            if (pbCargado)
            {
                bsCheckLisk.DataSource = loLogicaNegocio.goCheckList(cmbTipoSolicitud.EditValue.ToString(), cmbTipoPersona.EditValue.ToString(), cmbCodContado.EditValue.ToString(), cmbEstatusSeguro.EditValue.ToString()); //loLogicaNegocio.goCheckListObligatorio(); //
                dgvCheckList.RefreshData();
            }
        }

        private void cmbCliente_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    var dt = loLogicaNegocio.goConsultaDataTable("SELECT CardCode,CardName,U_ESTATUS_SEGURO,GroupCode,U_Exx_TProv,U_TIPO_RUC,U_TIPO_CONTR,CreditLine,PymntGroup,SlpCode FROM SBO_AFECOR.dbo.OCRD T0 (NOLOCK) INNER JOIN SBO_AFECOR.dbo.OCTG T1 (NOLOCK) ON T0.GroupNum = T1.GroupNum WHERE CardCode = '" + cmbCliente.EditValue.ToString() + "'");

                    if (dt.Rows.Count > 0)
                    {
                        txtCliente.Text = dt.Rows[0]["CardName"].ToString();
                        txtPlazoSap.Text = dt.Rows[0]["PymntGroup"].ToString();

                        if (dt.Rows[0]["U_TIPO_RUC"].ToString() == "J")
                        {
                            cmbTipoPersona.EditValue = "JUR";
                            cmbTipoPersona.ReadOnly = true;
                        }
                        else if (dt.Rows[0]["U_TIPO_RUC"].ToString() == "N" || dt.Rows[0]["U_TIPO_RUC"].ToString() == "NR")
                        {
                            cmbTipoPersona.EditValue = "NAT";
                            cmbTipoPersona.ReadOnly = true;
                        }
                        else
                        {
                            cmbTipoPersona.EditValue = Diccionario.Seleccione;
                            cmbTipoPersona.ReadOnly = false;
                        }

                        if (dt.Rows[0]["GroupCode"].ToString() == "115")
                        {
                            cmbCodContado.EditValue = "SI";
                        }
                        else
                        {
                            cmbCodContado.EditValue = "NO";
                        }

                        cmbEstatusSeguro.EditValue = "SIN";

                        if (!string.IsNullOrEmpty(dt.Rows[0]["SlpCode"].ToString()))
                        {
                            int pIdVendedorGrupo = loLogicaNegocio.giGetIdGrupoVendedorDesdeCodigoDeSap(int.Parse(dt.Rows[0]["SlpCode"].ToString()));
                            if (pIdVendedorGrupo != 0)
                            {
                                cmbRTC.ReadOnly = true;
                                cmbRTC.EditValue = pIdVendedorGrupo;
                            }
                            else
                            {
                                cmbRTC.EditValue = Diccionario.Seleccione;
                                txtZona.Text = "";
                                cmbRTC.ReadOnly = false;
                            }
                        }
                        else
                        {
                            cmbRTC.EditValue = Diccionario.Seleccione;
                                txtZona.Text = "";
                            cmbRTC.ReadOnly = false;
                        }

                        /*Comentado por dispición de JE 20231003*/
                        /*
                        if (dt.Rows[0]["U_ESTATUS_SEGURO"].ToString() == "N")
                        {
                            cmbEstatusSeguro.EditValue = "NOM";
                        }
                        else if (dt.Rows[0]["U_ESTATUS_SEGURO"].ToString() == "I")
                        {
                            cmbEstatusSeguro.EditValue = "INN";
                        }
                        else if (dt.Rows[0]["U_ESTATUS_SEGURO"].ToString() == "R")
                        {
                            cmbEstatusSeguro.EditValue = "RAF";
                        }
                        else
                        {
                            cmbEstatusSeguro.EditValue = "SIN";
                        }
                        */

                        txtCupoSAP.Text = dt.Rows[0]["CreditLine"].ToString();
                        txtCupoSolicitado.Text = "";
                        txtPlazoSolicitado.Text = "";

                        //txtTelefono.Text = dt.Rows[0]["Phone1"].ToString();
                        //txtCelular.Text = dt.Rows[0]["Cellular"].ToString();
                        //txtRucNegocio.Text = dt.Rows[0]["LicTradNum"].ToString();
                        //txtIdentificacion.Text = dt.Rows[0]["LicTradNum"].ToString();
                    }
                    else
                    {
                        txtCupoSAP.Text = "";
                        txtPlazoSap.Text = "";
                        txtCupoSolicitado.Text = "";
                        txtPlazoSolicitado.Text = "";
                        txtCliente.Text = "";
                        //cmbTipoSolicitud.EditValue = Diccionario.Seleccione;
                        cmbEstatusSeguro.EditValue = "SIN";
                        cmbTipoPersona.EditValue = Diccionario.Seleccione;
                        cmbCodContado.EditValue = Diccionario.Seleccione;
                        cmbRTC.EditValue = Diccionario.Seleccione;
                    }
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
                if (!lbCerrado)
                {


                    if (xtraTabControl1.SelectedTabPageIndex == 0)
                    {
                        var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                        var piList = poLista.Select(x => x.IdCheckList).Distinct();

                        var poCheckList = loLogicaNegocio.goConsultarComboCheckList();

                        var poCombo = (from a in poCheckList.Where(x => !piList.Contains(int.Parse(x.Codigo)))
                                       select new Combo()
                                       {
                                           Codigo = a.Codigo,
                                           Descripcion = a.Descripcion
                                       }).ToList();

                        frmCombo frmTipoComision = new frmCombo();
                        frmTipoComision.lsNombre = "Documento";
                        frmTipoComision.loCombo = poCombo;
                        frmTipoComision.ShowDialog();

                        string psCode = frmTipoComision.lsSeleccionado;
                        if (!string.IsNullOrEmpty(frmTipoComision.lsSeleccionado))
                        {
                            bsCheckLisk.AddNew();
                            dgvCheckList.Focus();
                            dgvCheckList.ShowEditor();
                            dgvCheckList.UpdateCurrentRow();
                            poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                            poLista.LastOrDefault().IdCheckList = int.Parse(frmTipoComision.lsSeleccionado);
                            poLista.LastOrDefault().CheckList = poCheckList.Where(x => x.Codigo == frmTipoComision.lsSeleccionado).Select(x => x.Descripcion).FirstOrDefault();
                            poLista.LastOrDefault().Completado = "NO";
                            dgvCheckList.RefreshData();
                            dgvCheckList.FocusedColumn = dgvCheckList.VisibleColumns[0];
                        }
                    }
                    if (xtraTabControl1.SelectedTabPageIndex == 1)
                    {
                        bsArchivoAdjunto.AddNew();
                        dgvArchivoAdjunto.Focus();
                        dgvArchivoAdjunto.ShowEditor();
                        dgvArchivoAdjunto.UpdateCurrentRow();
                        dgvArchivoAdjunto.RefreshData();
                        dgvArchivoAdjunto.FocusedColumn = dgvArchivoAdjunto.VisibleColumns[0];
                    }

                    
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnComentarios_Click(object sender, EventArgs e)
        {

            try
            {
                int tId = string.IsNullOrEmpty(txtNo.Text) ? 0 : int.Parse(txtNo.Text);
                var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.RequerimientoCredito, tId);

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Comentarios" };
                pofrmBuscar.ShowDialog();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefBancarias_Click(object sender, EventArgs e)
        {
            try
            {
                string psForma = Diccionario.Tablas.Menu.frmTrRevisionRefBancaria;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                if (poForm != null)
                {
                    frmTrRevisionRefBancaria frm = new frmTrRevisionRefBancaria();
                    frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    frm.Text = poForm.Nombre;
                    frm.lIdProceso = int.Parse(txtNo.Text);
                    frm.lbRevision = lbResolucion;
                    frm.lbotonRevisor = true;
                    frm.lbCerrado = lbCerrado;
                    var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                    var pIdCheckList = poLista.Where(x => x.IdCheckList == 11).Select(x => x.IdProcesoCreditoDetalle).FirstOrDefault();
                    if (pIdCheckList != 0)
                    {
                        loLogicaNegocio.gsActualzarChecklistEnRevision(pIdCheckList, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    }

                    frm.ShowDialog();
                    lConsultar();
                }
                else
                {
                    XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefComerciales_Click(object sender, EventArgs e)
        {
            try
            {
                string psForma = Diccionario.Tablas.Menu.frmTrRevisionRefComercial;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                if (poForm != null)
                {
                    frmTrRevisionRefComercial frm = new frmTrRevisionRefComercial();
                    frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    frm.Text = poForm.Nombre;
                    frm.lIdProceso = int.Parse(txtNo.Text);
                    frm.lbRevision = lbResolucion;
                    frm.lbotonRevisor = true;
                    frm.lbCerrado = lbCerrado;
                    var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                    var pIdCheckList = poLista.Where(x => x.IdCheckList == 14).Select(x => x.IdProcesoCreditoDetalle).FirstOrDefault();
                    if (pIdCheckList != 0)
                    {
                        loLogicaNegocio.gsActualzarChecklistEnRevision(pIdCheckList, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    }

                    frm.ShowDialog();
                    lConsultar();
                }
                else
                {
                    XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCertBienes_Click(object sender, EventArgs e)
        {
            try
            {
                string psForma = Diccionario.Tablas.Menu.frmTrRevisionBienes;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                if (poForm != null)
                {
                    frmTrRevisionBienes frm = new frmTrRevisionBienes();
                    frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    frm.Text = poForm.Nombre;
                    frm.lIdProceso = int.Parse(txtNo.Text);
                    frm.lbRevision = lbResolucion;
                    frm.lbCerrado = lbCerrado;
                    var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                    var pIdCheckList = poLista.Where(x => x.IdCheckList == 10).Select(x => x.IdProcesoCreditoDetalle).FirstOrDefault();
                    if (pIdCheckList != 0)
                    {
                        loLogicaNegocio.gsActualzarChecklistEnRevision(pIdCheckList, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    }

                    frm.ShowDialog();
                    lConsultar();
                }
                else
                {
                    XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInfRevisora_Click(object sender, EventArgs e)
        {
            frmRevisionWeb frm = new frmRevisionWeb();
            frm.lbAprobado = cmbEstado.EditValue.ToString() != Diccionario.Finalizado ? false : true;
            frm.lid = int.Parse(txtNo.Text);
            frm.lbCerrado = lbCerrado;
            frm.Show();
        }

        private void chbRevisionCompletada_CheckedChanged_1(object sender, EventArgs e)
        {
            if (pbCargado)
            {
                if (chbRevisionCompletada.Checked)
                {
                    cmbEstado.EditValue = Diccionario.EnAnalisis;
                }
                else
                {
                    cmbEstado.EditValue = Diccionario.EnRevision;
                }
            }
        }

        private void cmbRTC_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    string psZona = loLogicaNegocio.gsZonaGrupo(int.Parse(cmbRTC.EditValue.ToString()));

                    if (!string.IsNullOrEmpty(psZona))
                    {
                        if (psZona.Contains("-"))
                        {
                            txtZona.Text = psZona.Split('-')[1].Trim();
                        }
                        else
                        {
                            txtZona.Text = psZona.Trim();
                        }
                    }
                    else
                    {
                        txtZona.Text = "";
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCupoSolicitado_Leave(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    pbCargado = false;
                    var poParametro = loLogicaNegocio.goConsultarParametroCredito();
                    if (!string.IsNullOrEmpty(txtCupoSolicitado.Text) && !string.IsNullOrEmpty(txtCupoSolicitado.EditValue.ToString()))
                    {
                        if (Convert.ToDecimal(txtCupoSolicitado.EditValue.ToString()) < poParametro.CupoMinimoEnvioSeguro)
                        {
                            var poLista = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                            var poEntidad = poLista.Where(x => x.IdCheckList == 1).FirstOrDefault();

                            if (poEntidad != null)
                            {
                                poLista.Remove(poEntidad);
                            }

                            bsCheckLisk.DataSource = poLista;
                            dgvCheckList.RefreshData();

                            DialogResult dialogResult = XtraMessageBox.Show("El Cupo Solicitado es menor a: " + poParametro.CupoMinimoEnvioSeguro.ToString("c2") + ", por lo tanto debe agregar todos los documentos para continuar.", "¿Desea Continuar?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                            if (dialogResult == DialogResult.Yes)
                            {
                                var poLista2 = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                                var piList = poLista2.Select(x => x.IdCheckList).Distinct();

                                var poCheckList = loLogicaNegocio.goConsultarComboCheckList();


                                foreach (var item in poCheckList.Where(x => x.Codigo != "1" && !piList.Contains(int.Parse(x.Codigo))))
                                {
                                    bsCheckLisk.AddNew();
                                    dgvCheckList.Focus();
                                    dgvCheckList.ShowEditor();
                                    dgvCheckList.UpdateCurrentRow();
                                    poLista2 = (List<ProcesoCreditoDetalle>)bsCheckLisk.DataSource;
                                    poLista2.LastOrDefault().IdCheckList = int.Parse(item.Codigo);
                                    poLista2.LastOrDefault().CheckList = item.Descripcion;
                                    poLista2.LastOrDefault().Completado = "NO";
                                    poLista2.LastOrDefault().Del = "NO";
                                    dgvCheckList.RefreshData();
                                    dgvCheckList.FocusedColumn = dgvCheckList.VisibleColumns[0];
                                }
                            }
                        }
                    }
                    pbCargado = true;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIrResolucion_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text))
                {
                    frmResolucion frm = new frmResolucion();
                    frm.lbJefeAlmacen = true;
                    frm.lbRevision = lbRevision;
                    frm.lsTipo = cmbTipoSolicitud.EditValue.ToString();
                    frm.lid = int.Parse(txtNo.Text);
                    frm.Show();
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
                if (!string.IsNullOrEmpty(txtNo.Text))
                {
                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de enviar a corregir el requerimiento?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var result = XtraInputBox.Show("Ingrese comentario", "Corregir", "");
                        if (string.IsNullOrEmpty(result))
                        {
                            XtraMessageBox.Show("Debe agregar comentario para enviar a corregir", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var psMess = loLogicaNegocio.gsActualzarRequerimeintoCorregir(int.Parse(txtNo.Text), result, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, true);

                        if (psMess != "")
                        {
                            XtraMessageBox.Show(psMess, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lConsultar();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCopiarDocumentos_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text))
                {

                    var dt = loLogicaNegocio.goConsultaDataTable(string.Format("SELECT IdProcesoCredito Id,Fecha FechaSolicitud,U.NombreCompleto Usuario, TipoSolicitud,CupoAprobado,PlazoAprobado,(SELECT COUNT(*) FROM CRETPROCESOCREDITODETALLE PD (NOLOCK) WHERE PD.CodigoEstado NOT IN ('E','I') AND PD.IdProcesoCredito = P.IdProcesoCredito) Documentos FROM CRETPROCESOCREDITO P (NOLOCK) INNER JOIN SEGMUSUARIO U (NOLOCK) ON P.UsuarioIngreso = U.CodigoUsuario WHERE P.CodigoEstado NOT IN ('E','I') AND IdProcesoCredito NOT IN ({0}) AND CodigoCliente = '{1}'", txtNo.Text, cmbCliente.EditValue));
                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                    pofrmBuscar.Width = 1200;
                    if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                    {

                        DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de copiar datos?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {
                            var piReqCopy = pofrmBuscar.lsCodigoSeleccionado;
                            var dt1 = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC CRESPCOPIARREQUERIMIENTOS {0},{1},{2}", piReqCopy, txtNo.Text, clsPrincipal.gsUsuario));

                            XtraMessageBox.Show(dt1.Rows[0][0].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lConsultar();

                        }

                    }
                }
                else
                {
                    XtraMessageBox.Show("Debe crear el requerimiento para poder copiar documentos, click en Guardar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chPlazoCorto_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (chPlazoCorto.Checked)
                //{
                //    chbEnviarRevision.ReadOnly = false;
                //    if (Diccionario.Pendiente == cmbEstado.EditValue.ToString())
                //    {
                //        cmbEstado.EditValue = Diccionario.Cargado;
                //    }
                //}
                //else
                //{
                //    chbEnviarRevision.ReadOnly = true;
                //}
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
