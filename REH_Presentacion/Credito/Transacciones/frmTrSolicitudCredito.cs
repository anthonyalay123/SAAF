using CRE_Negocio.Transacciones;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Credito;
using REH_Presentacion.Comun;
using REH_Presentacion.Credito.Reportes.Rpt;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
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
    public partial class frmTrSolicitudCredito : frmBaseTrxDev
    {

        private frmTrProcesoCredito m_MainForm;

        public int lIdProceso = 0;
        public string lsCardCode = "";
        public string lsTipoProceso = "";
        public string lsCliente = "";
        public decimal ldcCupoSAP = 0M;
        public decimal ldcCupoSolicitado = 0M;
        public int liPlazoSolicitado = 0;
        public bool lbGuardado = false;
        public bool lbCompletado = false;
        public string lsRtc = "";
        public string lsZona = "";
        public bool lbCerrado = false;

        clsNSolicitudCredito loLogicaNegocio = new clsNSolicitudCredito();
        BindingSource bsOtrosActivos = new BindingSource();
        BindingSource bsRefBancaria = new BindingSource();
        BindingSource bsRefComercial = new BindingSource();
        BindingSource bsRefPersonal = new BindingSource();
        BindingSource bsPropiedades = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnAdd = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        bool pbCargado = false;

        public frmTrSolicitudCredito(frmTrProcesoCredito form = null)
        {
            m_MainForm = form;
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnAdd.ButtonClick += rpiBtnAdd_ButtonClick;
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
        }

        private void frmTrSolicitudCredito_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                clsComun.gLLenarCombo(ref cmbTipoSolicitud, loLogicaNegocio.goConsultarComboTipoSolicitudCredito(), true);
                clsComun.gLLenarCombo(ref cmbActividadCliente, loLogicaNegocio.goConsultarComboActividadCliente(), true);
                clsComun.gLLenarCombo(ref cmbRTC, loLogicaNegocio.goConsultarComboVendedorGrupo(), true);
                clsComun.gLLenarCombo(ref cmbVivienda, loLogicaNegocio.goConsultarComboVivienda(), true);
                clsComun.gLLenarCombo(ref cmbLocal, loLogicaNegocio.goConsultarComboLocal(), true);
                clsComun.gLLenarCombo(ref cmbTerreno, loLogicaNegocio.goConsultarComboLocal(), true);
                clsComun.gLLenarCombo(ref cmbEstadoCivil, loLogicaNegocio.goConsultarComboEstadoCivil(), true);
                clsComun.gLLenarCombo(ref cmbCliente, loLogicaNegocio.goSapConsultaClientesTodos(), true);

                var poMeses = loLogicaNegocio.goConsultarComboMesesDelAñoCero();
                clsComun.gLLenarCombo(ref cmbTiempoMes, poMeses);
                clsComun.gLLenarCombo(ref cmbTiempoActividadAnteriorMes, poMeses);
                clsComun.gLLenarCombo(ref cmbTiempoActividadMes, poMeses);
                clsComun.gLLenarCombo(ref cmbTiempoNegocioMes, poMeses);
                clsComun.gLLenarCombo(ref cmbTiempoAgricultorMes, poMeses);
                clsComun.gLLenarCombo(ref cmbAntiguedadTerrenoMes, poMeses);

                clsComun.gLLenarComboGrid(ref dgvOtrosActivos, loLogicaNegocio.goConsultarComboTipoOtrosActivos(), "CodigoTipoOtrosActivos", true);
                clsComun.gLLenarComboGrid(ref dgvReferenciasBancarias, loLogicaNegocio.goConsultarComboBanco(), "CodigoBanco", true);
                clsComun.gLLenarComboGrid(ref dgvReferenciasBancarias, loLogicaNegocio.goConsultarComboTipoCuentaBancaria().Where(x => x.Codigo != "AMI").ToList(), "CodigoTipoCuentaBancaria", true);
                clsComun.gLLenarComboGrid(ref dgvReferenciasPersonales, loLogicaNegocio.goConsultarComboTipoRelacionPersonal(), "CodigoRelacion", true);
                clsComun.gLLenarComboGrid(ref dgvReferenciasPersonales, loLogicaNegocio.goConsultarComboTipoReferenciaPersonal(), "TipoReferenciaPersonal", true);
                clsComun.gLLenarComboGrid(ref dgvPropiedades, loLogicaNegocio.goConsultarComboTipoBien(), "CodigoTipoBien", true);
                clsComun.gLLenarComboGrid(ref dgvPropiedades, loLogicaNegocio.goConsultarComboSINONE(), "Hipoteca", true);

                TabCab.TabPages[2].PageVisible = false;

                pbCargado = true;

                dtpFechaNacimiento.DateTime = DateTime.Now;

                if (lIdProceso != 0)
                {
                    cmbTipoSolicitud.EditValue = lsTipoProceso;
                    cmbCliente.EditValue = lsCardCode;
                    txtCupoSAP.EditValue = ldcCupoSAP;
                    txtCupo.EditValue = ldcCupoSolicitado;
                    //txt.EditValue = ldcCupoSolicitado;
                    txtCliente.Text = lsCliente;
                    if (!string.IsNullOrEmpty(lsRtc))
                    {
                        cmbRTC.EditValue = lsRtc;
                    }
                    txtZona.Text = lsZona;

                    cmbCliente.ReadOnly = true;
                    txtCliente.ReadOnly = true;
                    txtCupo.ReadOnly = true;
                    txtCupoSAP.ReadOnly = true;
                    cmbRTC.ReadOnly = true;
                    txtZona.ReadOnly = true;

                    lMensajeFechaUltSol();



                    if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Visible = false;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Visible = false;
                    if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Visible = false;
                    if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Visible = false;
                    if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Visible = false;
                    if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Visible = false;

                    var pIdSol = loLogicaNegocio.gIdSolicitudCredito(lIdProceso);
                    if (pIdSol != 0)
                    {
                        btnCopiar.Enabled = false;
                        txtNo.Text = pIdSol.ToString();
                    }

                    lConsultar();
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                var menu = Tag.ToString().Split(',');
                var poListaObject = loLogicaNegocio.goListar();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Fecha Solicitud", typeof(DateTime)),
                                    new DataColumn("Tipo Solicitud"),
                                    new DataColumn("Cliente"),
                                    new DataColumn("RTC"),
                                    new DataColumn("Cupo")

                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdSolicitudCredito;
                    row["Fecha Solicitud"] = a.FechaSolicitud;
                    row["Tipo Solicitud"] = a.TipoSolicitud;
                    row["Cliente"] = a.Cliente;
                    row["RTC"] = a.RTC;
                    row["Cupo"] = a.Cupo;
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                pofrmBuscar.Width = 1200;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    lLimpiar();
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar(true);

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
                dgvOtrosActivos.PostEditor();
                dgvReferenciasBancarias.PostEditor();
                dgvReferenciasComerciales.PostEditor();
                dgvReferenciasPersonales.PostEditor();
                dgvPropiedades.PostEditor();

                var psMsgVal = lbEsValido();
                if (string.IsNullOrEmpty(psMsgVal))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {

                        SolicitudCredito poObject = new SolicitudCredito();
                        poObject.IdSolicitudCredito = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                        poObject.FechaSolicitud = DateTime.Now;
                        poObject.CodigoTipoSolicitud = cmbTipoSolicitud.EditValue.ToString();
                        poObject.TipoSolicitud = cmbTipoSolicitud.Text;
                        poObject.CodigoActividadCliente = cmbActividadCliente.EditValue.ToString();
                        poObject.ActividadCliente = cmbActividadCliente.Text;
                        poObject.IdRTC = int.Parse(cmbRTC.EditValue.ToString());
                        poObject.RTC = cmbRTC.Text;
                        poObject.Zona = txtZona.Text;
                        poObject.Cupo = decimal.Parse(txtCupo.EditValue.ToString());
                        poObject.Observacion = txtObservacion.Text;
                        poObject.CodigoCliente = cmbCliente.EditValue.ToString();
                        poObject.Cliente = txtCliente.Text;
                        poObject.Identificacion = txtIdentificacion.Text;
                        poObject.FechaNacimiento = dtpFechaNacimiento.DateTime;
                        poObject.CodigoEstadoCivil = cmbEstadoCivil.EditValue.ToString();
                        poObject.Ciudad = txtCiudad.Text;
                        poObject.DireccionDomicilio = txtDireccionDomicilio.Text;
                        poObject.Telefono = txtTelefono.Text;
                        poObject.Celular = txtCelular.Text;
                        poObject.CodigoVivienda = cmbVivienda.EditValue.ToString();
                        poObject.Tiempo = string.IsNullOrEmpty(txtTiempo.Text) ? 0 : int.Parse(txtTiempo.Text);
                        poObject.Email = txtEmail.Text;
                        poObject.NombreConyuge = txtNombreConyuge.Text;
                        poObject.IdentificacionConyuge = txtIdentificacionConyuge.Text;
                        poObject.NombreComercial = txtNombreComercial.Text;
                        poObject.TiempoActividad = string.IsNullOrEmpty(txtTiempoActividad.Text) ? 0 : int.Parse(txtTiempoActividad.Text);
                        poObject.Ruc = txtRucNegocio.Text;
                        poObject.CodigoTipoNegocio = txtTipoNegocio.Text;
                        poObject.CiudadNegocio = txtCiudadNegocio.Text;
                        poObject.TelefonoNegocio = txtTelefonoNegocio.Text;
                        poObject.CelularNegocio = txtCelularNegocio.Text;
                        poObject.CodigoLocal = cmbLocal.EditValue.ToString();
                        poObject.TiempoNegocio = string.IsNullOrEmpty(txtTiempoNegocio.Text) ? 0 : int.Parse(txtTiempoNegocio.Text);
                        poObject.ActividadAnterior = txtActividadAnterior.Text;
                        poObject.TiempoActividadAnterior = string.IsNullOrEmpty(txtTiempoActividadAnterior.Text) ? 0 : int.Parse(txtTiempoActividadAnterior.Text);
                        poObject.EmpresaActividadAnterior = txtEmpresaActividadAnterior.Text;
                        poObject.CargoActividadAnterior = txtCargoActividadAnterior.Text;
                        poObject.NombreContactoPago = txtNombreContactoPago.Text;
                        poObject.CargoContactoPago = txtCargoContacto.Text;
                        poObject.TelefonoContactoPago = txtTelefonoContactoPago.Text;
                        poObject.CelularContactoPago = txtCelularPago.Text;
                        poObject.EmailContactoPago = txtEmailContactoPago.Text;
                        poObject.EmailFacturaElectronica = txtEmailEnvioFacturaElectronica.Text;
                        poObject.OtrosIngresos = string.IsNullOrEmpty(txtOrigenOtrosIngresos.Text) ? 0 : decimal.Parse(txtOtrosIngresos.EditValue.ToString());
                        poObject.OrigenOtrosIngresos = txtOrigenOtrosIngresos.Text;
                        poObject.EmailRealizaCompras = txtPersonaAutorizadaRealizarCompras.Text;//txtIdentificacionPersonaRealizaCompras.Text;
                        poObject.EmailRecibeCompras = txtPersonaAutorizadaRecibirCompras.Text;
                        poObject.IdentificacionRealizaCompras = txtIdentificacionPersonaRealizaCompras.Text;
                        poObject.IdentificacionEmailRecibeCompras = txtIdentificacionPersonaRecibeCompras.Text;
                        poObject.TiempoAgricultor = string.IsNullOrEmpty(txtTiempoAgricultor.Text) ? 0 : int.Parse(txtTiempoAgricultor.Text);
                        poObject.ClaseCultivo = txtClaseCultivo.Text;
                        poObject.NumeroHectariasCultivadas = string.IsNullOrEmpty(txtNoHectarias.Text) ? 0 : int.Parse(txtNoHectarias.Text);
                        poObject.CodigoTerreno = cmbTerreno.EditValue.ToString();
                        poObject.AntiguedadTerreno = string.IsNullOrEmpty(txtAntiguedadTerreno.Text) ? 0 : int.Parse(txtAntiguedadTerreno.Text);
                        poObject.MesesCosecha = string.IsNullOrEmpty(txtMesesCosecha.Text) ? 0 : int.Parse(txtMesesCosecha.Text);
                        poObject.PromedioIngresos = string.IsNullOrEmpty(txtPromedioIngresos.Text) ? 0 : decimal.Parse(txtPromedioIngresos.EditValue.ToString());
                        poObject.UbicacionTerrenos = txtUbicacionTerrenos.Text;
                        poObject.Completado = chbCompletado.Checked;
                        poObject.IdProcesoCredito = lIdProceso;
                        poObject.IdentificacionRepLegal = txtIdentificacionRepLegal.Text;
                        poObject.TiempoMes = int.Parse(cmbTiempoMes.EditValue.ToString());
                        poObject.TiempoActividadMes = int.Parse(cmbTiempoActividadMes.EditValue.ToString());
                        poObject.TiempoActividadAnteriorMes = int.Parse(cmbTiempoActividadAnteriorMes.EditValue.ToString());
                        poObject.TiempoAgricultorMes = int.Parse(cmbTiempoAgricultorMes.EditValue.ToString());
                        poObject.TiempoNegocioMes = int.Parse(cmbTiempoNegocioMes.EditValue.ToString());
                        poObject.AntiguedadTerrenoMes = int.Parse(cmbAntiguedadTerrenoMes.EditValue.ToString());

                        if (!string.IsNullOrEmpty(dtpFechaUltSol.Text))
                        {
                            poObject.FechaUltimaSolicitud = dtpFechaUltSol.DateTime;
                        }

                        //poObject.NombreContactoPagoAgricultor = txtContactoPagoAgricultor.Text;
                        //poObject.CargoContactoPagoAgricultor = txtCargoContactoPagoAgricultor.Text;
                        //poObject.TelefonoContactoPagoAgricultor = txtTelefonoContactoPagoAgricultor.Text;
                        //poObject.CelularContactoPagoAgricultor = txtCelularContactoAgricultor.Text;
                        //poObject.EmailContactoPagoAgricultor = txtEmailContactoPagoAgricultor.Text;
                        poObject.DireccionNegocio = txtDireccionNegocio.Text;

                        if (!string.IsNullOrEmpty(txtNoForms.Text))
                        {
                            poObject.IdReferenciaForm = int.Parse(txtNoForms.Text);
                        }

                        poObject.ProcesoCredito.ProcesoCreditoOtrosActivos = (List<ProcesoCreditoOtrosActivos>)bsOtrosActivos.DataSource;
                        poObject.ProcesoCredito.ProcesoCreditoRefBancaria = (List<ProcesoCreditoRefBancaria>)bsRefBancaria.DataSource;
                        poObject.ProcesoCredito.ProcesoCreditoRefComercial = (List<ProcesoCreditoRefComercial>)bsRefComercial.DataSource;
                        poObject.SolicitudCreditoRefPersonal = (List<SolicitudCreditoRefPersonal>)bsRefPersonal.DataSource;
                        poObject.ProcesoCredito.ProcesoCreditoPropiedades = (List<ProcesoCreditoPropiedades>)bsPropiedades.DataSource;

                        var poLista = new List<SolicitudCredito>();

                        poLista.Add(poObject);

                        string psMsg = loLogicaNegocio.gsGuardar(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lbGuardado = true;
                            lbCompletado = chbCompletado.Checked;
                            if (lIdProceso != 0)
                            {
                                if (m_MainForm != null)
                                {
                                    new clsNProcesoCredito().gsActualzarRequerimientoCredito(lIdProceso, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                                    m_MainForm.lConsultar();
                                }
                                Close();
                            }
                            lLimpiar();

                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show(psMsgVal, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.ToString()))
                {
                    int lId = int.Parse(txtNo.Text);


                    //DataSet ds = new DataSet();
                    var ds = loLogicaNegocio.goConsultaDataSet("EXEC CRESPRPTINFORMECREDITO " + "'" + lId + "'");

                    ds.Tables[0].TableName = "Cab";
                    ds.Tables[1].TableName = "Propiedades";
                    ds.Tables[2].TableName = "OtrosActivos";
                    ds.Tables[3].TableName = "RefBancarias";
                    ds.Tables[4].TableName = "RefComerciales";
                    ds.Tables[5].TableName = "RefPersonales";

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        xrptSolicitudCredito xrpt = new xrptSolicitudCredito();
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Primero, txtNo.Text.Trim());
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Anterior, txtNo.Text.Trim());
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Siguiente, txtNo.Text.Trim());
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Ultimo, txtNo.Text.Trim());
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
                if (tabDetDatosGenerales.SelectedTabPageIndex == 4)
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
                if (tabDetDatosGenerales.SelectedTabPageIndex == 0)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvReferenciasBancarias.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<ProcesoCreditoRefBancaria>)bsRefBancaria.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsRefBancaria.DataSource = poLista;
                        dgvReferenciasBancarias.RefreshData();
                    }
                }
                if (tabDetDatosGenerales.SelectedTabPageIndex == 1)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvReferenciasComerciales.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<ProcesoCreditoRefComercial>)bsRefComercial.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsRefComercial.DataSource = poLista;
                        dgvReferenciasComerciales.RefreshData();
                    }
                }
                if (tabDetDatosGenerales.SelectedTabPageIndex == 2)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvReferenciasPersonales.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<SolicitudCreditoRefPersonal>)bsRefPersonal.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsRefPersonal.DataSource = poLista;
                        dgvReferenciasPersonales.RefreshData();
                    }
                }
                if (tabDetDatosGenerales.SelectedTabPageIndex == 3)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvPropiedades.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<ProcesoCreditoPropiedades>)bsPropiedades.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsPropiedades.DataSource = poLista;
                        dgvPropiedades.RefreshData();
                    }
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
        private void rpiBtnAdd_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {

                if (tabDetDatosGenerales.SelectedTabPageIndex == 0)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvReferenciasBancarias.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<ProcesoCreditoRefBancaria>)bsRefBancaria.DataSource;

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
                                    poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaSolCreRefBan"].ToString() + Name;
                                    // Asigno mi nueva lista al Binding Source
                                    bsRefBancaria.DataSource = poLista;
                                    dgvReferenciasBancarias.RefreshData();
                                }

                                else
                                {
                                    XtraMessageBox.Show("El tamano máximo permitido es de: " + clsPrincipal.gdcTamanoMb + "mb", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
                else if (tabDetDatosGenerales.SelectedTabPageIndex == 1)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvReferenciasComerciales.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<ProcesoCreditoRefComercial>)bsRefComercial.DataSource;


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
                                    poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaSolCreRefCom"].ToString() + Name;
                                    // Asigno mi nueva lista al Binding Source
                                    bsRefComercial.DataSource = poLista;
                                    dgvReferenciasComerciales.RefreshData();
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
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (tabDetDatosGenerales.SelectedTabPageIndex == 0)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvReferenciasBancarias.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<ProcesoCreditoRefBancaria>)bsRefBancaria.DataSource;
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
                else if (tabDetDatosGenerales.SelectedTabPageIndex == 1)
                {

                    int piIndex = dgvReferenciasComerciales.FocusedRowHandle;
                    var poLista = (List<ProcesoCreditoRefComercial>)bsRefComercial.DataSource;

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


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

            bsOtrosActivos.DataSource = new List<ProcesoCreditoOtrosActivos>();
            gcOtrosActivos.DataSource = bsOtrosActivos;

            bsRefBancaria.DataSource = new List<ProcesoCreditoRefBancaria>();
            gcReferenciasBancarias.DataSource = bsRefBancaria;
            dgvReferenciasBancarias.Columns["FechaEmision"].Visible = false;
            dgvReferenciasBancarias.Columns["Titular"].Visible = false;
            dgvReferenciasBancarias.Columns["SaldosPromedios"].Visible = false;

            bsRefComercial.DataSource = new List<ProcesoCreditoRefComercial>();
            gcReferenciasComerciales.DataSource = bsRefComercial;
            dgvReferenciasComerciales.Columns["FechaConsulta"].Visible = false;
            dgvReferenciasComerciales.Columns["ClienteDesde"].Visible = false;
            dgvReferenciasComerciales.Columns["Cupo"].Visible = false;
            dgvReferenciasComerciales.Columns["Plazo"].Visible = false;
            dgvReferenciasComerciales.Columns["Garantia"].Visible = false;
            dgvReferenciasComerciales.Columns["PromedioComprasMensual"].Visible = false;
            dgvReferenciasComerciales.Columns["PromedioComprasAnual"].Visible = false;
            dgvReferenciasComerciales.Columns["PromedioDiasPagos"].Visible = false;
            dgvReferenciasComerciales.Columns["FechaReferenciaComercial"].Caption = "Fecha Referencia";
            dgvReferenciasComerciales.Columns["CodigoGarantia"].Visible = false;

            bsRefPersonal.DataSource = new List<SolicitudCreditoRefPersonal>();
            gcReferenciasPersonales.DataSource = bsRefPersonal;

            bsPropiedades.DataSource = new List<ProcesoCreditoPropiedades>();
            gcPropiedades.DataSource = bsPropiedades;

            dgvOtrosActivos.Columns["IdProcesoCredito"].Visible = false;
            dgvOtrosActivos.Columns["IdProcesoCreditoOtrosActivos"].Visible = false;
            dgvOtrosActivos.Columns["FechaPago"].Visible = false;
            dgvOtrosActivos.Columns["DescripcionDocumento"].Visible = false;
            dgvOtrosActivos.Columns["PropietarioBeneficiario"].Visible = false;
            dgvOtrosActivos.Columns["Anio"].Caption = "Año";

            dgvReferenciasBancarias.Columns["IdProcesoCredito"].Visible = false;
            dgvReferenciasBancarias.Columns["IdProcesoCreditoRefBancaria"].Visible = false;

            dgvReferenciasComerciales.Columns["IdProcesoCredito"].Visible = false;
            dgvReferenciasComerciales.Columns["IdProcesoCreditoRefComercial"].Visible = false;

            dgvReferenciasPersonales.Columns["IdSolicitudCredito"].Visible = false;
            dgvReferenciasPersonales.Columns["IdSolicitudCreditoRefPersonal"].Visible = false;

            dgvPropiedades.Columns["IdProcesoCredito"].Visible = false;
            dgvPropiedades.Columns["IdProcesoCreditoPropiedades"].Visible = false;
            dgvPropiedades.Columns["FechaPago"].Visible = false;
            dgvPropiedades.Columns["DescripcionDocumento"].Visible = false;
            dgvPropiedades.Columns["PropietarioBeneficiario"].Visible = false;

            dgvOtrosActivos.Columns["CodigoTipoOtrosActivos"].Caption = "Tipo";

            dgvReferenciasBancarias.Columns["CodigoBanco"].Caption = "Banco";
            dgvReferenciasBancarias.Columns["CodigoTipoCuentaBancaria"].Caption = "Tipo Cuenta";

            dgvReferenciasPersonales.Columns["CodigoRelacion"].Visible = false;
            dgvReferenciasPersonales.Columns["TipoReferenciaPersonal"].Caption = "Parentezco";
            dgvReferenciasPersonales.Columns["CodigoRelacion"].Caption = "Tipo Ruc";
            dgvReferenciasPersonales.Columns["ReferenciaPersonal"].Visible = false;
            dgvReferenciasPersonales.Columns["Relacion"].Visible = false;

            dgvPropiedades.Columns["CodigoTipoBien"].Caption = "Tipo Bien";

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvOtrosActivos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvReferenciasBancarias.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvReferenciasComerciales.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvReferenciasPersonales.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);
            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvPropiedades.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);

            clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvReferenciasBancarias.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvReferenciasBancarias.Columns["Ver"], "Visualizar", Diccionario.ButtonGridImage.show_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvReferenciasComerciales.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvReferenciasComerciales.Columns["Ver"], "Visualizar", Diccionario.ButtonGridImage.show_16x16);

            dgvReferenciasBancarias.Columns["RutaDestino"].Visible = false;
            dgvReferenciasBancarias.Columns["RutaOrigen"].Visible = false;
            dgvReferenciasBancarias.Columns["ArchivoAdjunto"].Visible = false;
            dgvReferenciasBancarias.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvReferenciasBancarias.Columns["NombreOriginal"].Caption = "Archivo Adjunto";

            dgvReferenciasComerciales.Columns["RutaDestino"].Visible = false;
            dgvReferenciasComerciales.Columns["RutaOrigen"].Visible = false;
            dgvReferenciasComerciales.Columns["ArchivoAdjunto"].Visible = false;
            dgvReferenciasComerciales.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvReferenciasComerciales.Columns["NombreOriginal"].Caption = "Archivo Adjunto";

            txtIdentificacion.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtIdentificacionRepLegal.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtCiudad.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtTelefono.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtCelular.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtTiempo.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtNombreConyuge.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtIdentificacionConyuge.KeyPress += new KeyPressEventHandler(SoloNumeros);

            txtTiempoActividad.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtRucNegocio.KeyPress += new KeyPressEventHandler(SoloNumeros);
            //txtTipoNegocio.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtCiudadNegocio.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtTelefonoNegocio.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtCelularNegocio.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtTiempoNegocio.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtActividadAnterior.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtTiempoActividadAnterior.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtCargoActividadAnterior.KeyPress += new KeyPressEventHandler(SoloLetras);

            txtTiempoAgricultor.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtClaseCultivo.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtNoHectarias.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtAntiguedadTerreno.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtMesesCosecha.KeyPress += new KeyPressEventHandler(SoloNumeros);

            txtNombreContactoPago.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtTelefonoContactoPago.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtCelularPago.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtCargoContacto.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtOrigenOtrosIngresos.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtIdentificacionPersonaRealizaCompras.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtIdentificacionPersonaRecibeCompras.KeyPress += new KeyPressEventHandler(SoloNumeros);

            clsComun.gFormatearColumnasGrid(dgvReferenciasBancarias);
            clsComun.gFormatearColumnasGrid(dgvReferenciasComerciales);
            clsComun.gFormatearColumnasGrid(dgvOtrosActivos);
            clsComun.gFormatearColumnasGrid(dgvPropiedades);
        }

        private void lConsultar(bool tbLimpiar = false, bool tbCopiarDator = false)
        {

            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                pbCargado = false;

                var poObject = loLogicaNegocio.goConsultar(Convert.ToInt32(txtNo.Text.Trim()));


                txtNo.EditValue = poObject.IdSolicitudCredito;
                lblFecha.Text = poObject.FechaSolicitud.ToString("dd/MM/yyyy");
                if (poObject.FechaUltimaSolicitud > DateTime.MinValue)
                {
                    dtpFechaUltSol.DateTime = poObject.FechaUltimaSolicitud ?? DateTime.Now;
                    if (DateTime.Now.Subtract(poObject.FechaUltimaSolicitud ?? DateTime.Now).Days > 1825) // Si la última solicitud es mayor a 5 años, aparece el mensaje
                    {
                        XtraMessageBox.Show("Fecha de solicitud en custodio es mayor a 5 años, debe anexar una solicitud actualizada o el proceso se regresa.", "¡IMPORTANTE!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    dtpFechaUltSol.Text = "";
                }

                cmbTipoSolicitud.EditValue = poObject.CodigoTipoSolicitud;
                cmbRTC.EditValue = poObject.IdRTC.ToString();
                txtZona.Text = poObject.Zona;
                txtCupo.EditValue = poObject.Cupo;
                txtObservacion.Text = poObject.Observacion;
                cmbCliente.EditValue = poObject.CodigoCliente;
                txtCliente.Text = poObject.Cliente;
                txtIdentificacion.Text = poObject.Identificacion;
                pbCargado = true;
                cmbActividadCliente.EditValue = poObject.CodigoActividadCliente;
                dtpFechaNacimiento.DateTime = poObject.FechaNacimiento;
                cmbEstadoCivil.EditValue = poObject.CodigoEstadoCivil;
                txtCiudad.Text = poObject.Ciudad;
                txtDireccionDomicilio.Text = poObject.DireccionDomicilio;
                txtTelefono.Text = poObject.Telefono;
                txtCelular.Text = poObject.Celular;
                cmbVivienda.EditValue = poObject.CodigoVivienda;
                txtTiempo.EditValue = poObject.Tiempo;
                txtEmail.Text = poObject.Email;
                txtNombreConyuge.Text = poObject.NombreConyuge;
                txtIdentificacionConyuge.Text = poObject.IdentificacionConyuge;
                txtNombreComercial.Text = poObject.NombreComercial;
                txtTiempoActividad.EditValue = poObject.TiempoActividad;
                txtRucNegocio.Text = poObject.Ruc;
                txtTipoNegocio.EditValue = poObject.CodigoTipoNegocio;
                txtCiudadNegocio.Text = poObject.CiudadNegocio;
                txtDireccionNegocio.Text = poObject.DireccionNegocio;
                txtTelefonoNegocio.Text = poObject.TelefonoNegocio;
                txtCelularNegocio.Text = poObject.CelularNegocio;
                cmbLocal.EditValue = poObject.CodigoLocal;
                txtTiempoNegocio.EditValue = poObject.TiempoNegocio;
                txtActividadAnterior.Text = poObject.ActividadAnterior;
                txtTiempoActividadAnterior.EditValue = poObject.TiempoActividadAnterior;
                txtEmpresaActividadAnterior.Text = poObject.EmpresaActividadAnterior;
                txtCargoActividadAnterior.Text = poObject.CargoActividadAnterior;
                txtNombreContactoPago.Text = poObject.NombreContactoPago;
                txtCargoContacto.Text = poObject.CargoContactoPago;
                txtTelefonoContactoPago.Text = poObject.TelefonoContactoPago;
                txtCelularPago.Text = poObject.CelularContactoPago;
                txtEmailContactoPago.Text = poObject.EmailContactoPago;
                txtIdentificacionRepLegal.Text = poObject.IdentificacionRepLegal;
                //txtContactoPagoAgricultor.Text = poObject.NombreContactoPagoAgricultor;
                //txtCargoContactoPagoAgricultor.Text = poObject.CargoContactoPagoAgricultor;
                //txtTelefonoContactoPagoAgricultor.Text = poObject.TelefonoContactoPagoAgricultor;
                //txtCelularContactoAgricultor.Text = poObject.CelularContactoPagoAgricultor;
                //txtEmailContactoPagoAgricultor.Text = poObject.EmailContactoPagoAgricultor;
                txtEmailEnvioFacturaElectronica.Text = poObject.EmailFacturaElectronica;
                txtOtrosIngresos.EditValue = poObject.OtrosIngresos;
                txtOrigenOtrosIngresos.Text = poObject.OrigenOtrosIngresos;
                txtPersonaAutorizadaRealizarCompras.Text = poObject.EmailRealizaCompras;
                txtPersonaAutorizadaRecibirCompras.Text = poObject.EmailRecibeCompras;
                txtIdentificacionPersonaRealizaCompras.Text = poObject.IdentificacionRealizaCompras;
                txtIdentificacionPersonaRecibeCompras.Text = poObject.IdentificacionEmailRecibeCompras;
                txtTiempoAgricultor.EditValue = poObject.TiempoAgricultor;
                txtClaseCultivo.Text = poObject.ClaseCultivo;
                txtNoHectarias.EditValue = poObject.NumeroHectariasCultivadas;
                cmbTerreno.EditValue = poObject.CodigoTerreno;
                txtAntiguedadTerreno.EditValue = poObject.AntiguedadTerreno;
                txtMesesCosecha.EditValue = poObject.MesesCosecha;
                txtPromedioIngresos.EditValue = poObject.PromedioIngresos;
                txtUbicacionTerrenos.Text = poObject.UbicacionTerrenos;
                txtNoForms.EditValue = poObject.IdReferenciaForm;
                chbCompletado.Checked = poObject.Completado;

                cmbTiempoMes.EditValue = poObject.TiempoMes.ToString();
                cmbTiempoActividadMes.EditValue = poObject.TiempoActividadMes.ToString();
                cmbTiempoActividadAnteriorMes.EditValue = poObject.TiempoActividadAnteriorMes.ToString();
                cmbTiempoAgricultorMes.EditValue = poObject.TiempoAgricultorMes.ToString();
                cmbTiempoNegocioMes.EditValue = poObject.TiempoNegocioMes.ToString();
                cmbAntiguedadTerrenoMes.EditValue = poObject.AntiguedadTerrenoMes.ToString();

                bsOtrosActivos.DataSource = poObject.ProcesoCredito.ProcesoCreditoOtrosActivos;
                dgvOtrosActivos.RefreshData();
                bsRefBancaria.DataSource = poObject.ProcesoCredito.ProcesoCreditoRefBancaria;
                dgvReferenciasBancarias.RefreshData();
                bsRefComercial.DataSource = poObject.ProcesoCredito.ProcesoCreditoRefComercial;
                dgvReferenciasComerciales.RefreshData();
                bsRefPersonal.DataSource = poObject.SolicitudCreditoRefPersonal;
                dgvReferenciasPersonales.RefreshData();
                bsPropiedades.DataSource = poObject.ProcesoCredito.ProcesoCreditoPropiedades;
                dgvPropiedades.RefreshData();

                if (loLogicaNegocio.gsGetEstadoReqCreCheckList(poObject.IdProcesoCredito, Diccionario.Tablas.CheckList.SolicitudCredito) == Diccionario.Aprobado)
                {
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                }
                else
                {
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                }

                if (tbCopiarDator)
                {
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                }

                if (tbLimpiar)
                {
                    txtNo.EditValue = "";
                    lblFecha.Text = "";
                    dtpFechaUltSol.Text = "";
                    ((List<ProcesoCreditoOtrosActivos>)bsOtrosActivos.DataSource).ForEach(x => x.IdProcesoCreditoOtrosActivos = 0);
                    dgvOtrosActivos.RefreshData();
                    ((List<ProcesoCreditoRefBancaria>)bsRefBancaria.DataSource).ForEach(x => x.IdProcesoCreditoRefBancaria = 0);
                    dgvReferenciasBancarias.RefreshData();
                    ((List<ProcesoCreditoRefComercial>)bsRefComercial.DataSource).ForEach(x => x.IdProcesoCreditoRefComercial = 0);
                    dgvReferenciasComerciales.RefreshData();
                    ((List<SolicitudCreditoRefPersonal>)bsRefPersonal.DataSource).ForEach(x => x.IdSolicitudCreditoRefPersonal = 0);
                    dgvReferenciasPersonales.RefreshData();
                    ((List<ProcesoCreditoPropiedades>)bsPropiedades.DataSource).ForEach(x => x.IdProcesoCreditoPropiedades = 0);
                    dgvPropiedades.RefreshData();
                }
            }
            else
            {

                var poObject = loLogicaNegocio.goConsultar(0, lIdProceso);

                bsOtrosActivos.DataSource = poObject.ProcesoCredito.ProcesoCreditoOtrosActivos;
                dgvOtrosActivos.RefreshData();
                bsRefBancaria.DataSource = poObject.ProcesoCredito.ProcesoCreditoRefBancaria;
                dgvReferenciasBancarias.RefreshData();
                bsRefComercial.DataSource = poObject.ProcesoCredito.ProcesoCreditoRefComercial;
                dgvReferenciasComerciales.RefreshData();
                bsPropiedades.DataSource = poObject.ProcesoCredito.ProcesoCreditoPropiedades;
                dgvPropiedades.RefreshData();

            }



            lCambiarFechaUltimaSolicitud();
        }

        private void lMensajeFechaUltSol()
        {
            var pdFecha = loLogicaNegocio.gdtUltimaFechaSolicitud(lsCardCode);
            if (pdFecha > DateTime.MinValue)
            {
                dtpFechaUltSol.DateTime = pdFecha;
                if (DateTime.Now.Subtract(pdFecha).Days > 1825) // Si la última solicitud es mayor a 5 años, aparece el mensaje
                {
                    XtraMessageBox.Show("Fecha de solicitud en custodio es mayor a 5 años, debe anexar una solicitud actualizada o el proceso se regresa.", "¡IMPORTANTE!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                dtpFechaUltSol.Text = "";
            }
        }

        private void lLimpiar()
        {
            pbCargado = false;
            txtNo.EditValue = "";
            lblFecha.Text = "";
            dtpFechaUltSol.Text = "";
            cmbTipoSolicitud.EditValue = Diccionario.Seleccione;
            cmbActividadCliente.EditValue = Diccionario.Seleccione;
            cmbRTC.EditValue = Diccionario.Seleccione;
            txtZona.Text = "";
            txtCupo.EditValue = "";
            txtObservacion.Text = "";
            cmbCliente.EditValue = Diccionario.Seleccione;
            txtCliente.Text = "";
            txtIdentificacion.Text = "";
            dtpFechaNacimiento.DateTime = DateTime.Now;
            cmbEstadoCivil.EditValue = Diccionario.Activo;
            txtCiudad.Text = "";
            txtDireccionDomicilio.Text = "";
            txtTelefono.Text = "";
            txtCelular.Text = "";
            cmbVivienda.EditValue = Diccionario.Seleccione;
            txtTiempo.EditValue = "";
            txtEmail.Text = "";
            txtNombreConyuge.Text = "";
            txtIdentificacionConyuge.Text = "";
            txtNombreComercial.Text = "";
            txtTiempoActividad.EditValue = "";
            txtRucNegocio.Text = "";
            txtTipoNegocio.EditValue = "";
            txtCiudadNegocio.Text = "";
            txtTelefonoNegocio.Text = "";
            txtCelularNegocio.Text = "";
            cmbLocal.EditValue = Diccionario.Seleccione;
            txtTiempoNegocio.EditValue = "";
            txtActividadAnterior.Text = "";
            txtTiempoActividadAnterior.EditValue = "";
            txtEmpresaActividadAnterior.Text = "";
            txtCargoActividadAnterior.Text = "";
            txtNombreContactoPago.Text = "";
            txtCargoContacto.Text = "";
            txtTelefonoContactoPago.Text = "";
            txtCelularPago.Text = "";
            txtEmailContactoPago.Text = "";
            txtEmailEnvioFacturaElectronica.Text = "";
            txtOtrosIngresos.EditValue = "";
            txtOrigenOtrosIngresos.Text = "";
            txtIdentificacionPersonaRealizaCompras.Text = "";
            txtPersonaAutorizadaRecibirCompras.Text = "";
            txtTiempoAgricultor.EditValue = "";
            txtClaseCultivo.Text = "";
            txtNoHectarias.EditValue = "";
            cmbTerreno.EditValue = Diccionario.Seleccione;
            txtAntiguedadTerreno.EditValue = "";
            txtMesesCosecha.EditValue = "";
            txtPromedioIngresos.EditValue = "";
            txtUbicacionTerrenos.Text = "";
            chbCompletado.Checked = false;
            //txtContactoPagoAgricultor.Text = "";
            //txtCargoContactoPagoAgricultor.Text = "";
            //txtTelefonoContactoPagoAgricultor.Text = "";
            //txtCelularContactoAgricultor.Text = "";
            //txtEmailContactoPagoAgricultor.Text = "";
            txtDireccionNegocio.Text = "";
            txtIdentificacionRepLegal.Text = "";

            txtNoForms.EditValue = "";

            bsOtrosActivos.DataSource = new List<ProcesoCreditoOtrosActivos>();
            dgvOtrosActivos.RefreshData();
            bsRefBancaria.DataSource = new List<ProcesoCreditoRefBancaria>();
            dgvReferenciasBancarias.RefreshData();
            bsRefComercial.DataSource = new List<ProcesoCreditoRefComercial>();
            dgvReferenciasComerciales.RefreshData();
            bsRefPersonal.DataSource = new List<SolicitudCreditoRefPersonal>();
            dgvReferenciasPersonales.RefreshData();
            bsPropiedades.DataSource = new List<ProcesoCreditoPropiedades>();
            dgvPropiedades.RefreshData();

            cmbAntiguedadTerrenoMes.EditValue = Diccionario.Seleccione;
            cmbTiempoActividadAnteriorMes.EditValue = Diccionario.Seleccione;
            cmbTiempoActividadMes.EditValue = Diccionario.Seleccione;
            cmbTiempoAgricultorMes.EditValue = Diccionario.Seleccione;
            cmbTiempoMes.EditValue = Diccionario.Seleccione;
            cmbTiempoNegocioMes.EditValue = Diccionario.Seleccione;

            pbCargado = true;

        }
        #endregion

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabDetDatosGenerales.SelectedTabPageIndex == 4)
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
                if (tabDetDatosGenerales.SelectedTabPageIndex == 0)
                {
                    bsRefBancaria.AddNew();
                    dgvReferenciasBancarias.Focus();
                    dgvReferenciasBancarias.ShowEditor();
                    dgvReferenciasBancarias.UpdateCurrentRow();
                    var poLista = (List<ProcesoCreditoRefBancaria>)bsRefBancaria.DataSource;
                    poLista.LastOrDefault().CodigoBanco = Diccionario.Seleccione;
                    poLista.LastOrDefault().CodigoTipoCuentaBancaria = Diccionario.Seleccione;
                    dgvReferenciasBancarias.RefreshData();
                    dgvReferenciasBancarias.FocusedColumn = dgvReferenciasBancarias.VisibleColumns[0];
                }
                if (tabDetDatosGenerales.SelectedTabPageIndex == 1)
                {
                    bsRefComercial.AddNew();
                    dgvReferenciasComerciales.Focus();
                    dgvReferenciasComerciales.ShowEditor();
                    dgvReferenciasComerciales.UpdateCurrentRow();
                    var poLista = (List<ProcesoCreditoRefComercial>)bsRefComercial.DataSource;
                    dgvReferenciasComerciales.RefreshData();
                    dgvReferenciasComerciales.FocusedColumn = dgvReferenciasComerciales.VisibleColumns[0];
                }
                if (tabDetDatosGenerales.SelectedTabPageIndex == 2)
                {
                    bsRefPersonal.AddNew();
                    dgvReferenciasPersonales.Focus();
                    dgvReferenciasPersonales.ShowEditor();
                    dgvReferenciasPersonales.UpdateCurrentRow();
                    var poLista = (List<SolicitudCreditoRefPersonal>)bsRefPersonal.DataSource;
                    poLista.LastOrDefault().CodigoRelacion = Diccionario.Seleccione;
                    poLista.LastOrDefault().TipoReferenciaPersonal = Diccionario.Seleccione;
                    dgvReferenciasPersonales.RefreshData();
                    dgvReferenciasPersonales.FocusedColumn = dgvReferenciasPersonales.VisibleColumns[0];

                }
                if (tabDetDatosGenerales.SelectedTabPageIndex == 3)
                {
                    bsPropiedades.AddNew();
                    dgvPropiedades.Focus();
                    dgvPropiedades.ShowEditor();
                    dgvPropiedades.UpdateCurrentRow();
                    var poLista = (List<ProcesoCreditoPropiedades>)bsPropiedades.DataSource;
                    poLista.LastOrDefault().CodigoTipoBien = Diccionario.Seleccione;
                    poLista.LastOrDefault().Hipoteca = Diccionario.Seleccione;
                    dgvPropiedades.RefreshData();
                    dgvPropiedades.FocusedColumn = dgvPropiedades.VisibleColumns[0];
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbTipoSolicitud_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    if (cmbTipoSolicitud.EditValue.ToString() == "NUE")
                    {
                        if ((cmbCliente.Properties.DataSource as IList).Count > 0) cmbCliente.ItemIndex = 0;
                        cmbCliente.Enabled = false;
                    }
                    else
                    {
                        if ((cmbCliente.Properties.DataSource as IList).Count > 0) cmbCliente.ItemIndex = 0;
                        cmbCliente.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbActividadCliente_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    if (cmbActividadCliente.EditValue.ToString() == "AGR" || cmbActividadCliente.EditValue.ToString() == "EMA")
                    {
                        TabCab.TabPages[2].PageVisible = true;
                    }
                    else
                    {
                        TabCab.TabPages[2].PageVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbEstadoCivil_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    if (cmbEstadoCivil.EditValue.ToString() == "CAS" || cmbEstadoCivil.EditValue.ToString() == "U03")
                    {
                        gboxConyuge.Visible = true;
                    }
                    else
                    {
                        gboxConyuge.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbRTC_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    string psZona = loLogicaNegocio.gsZonaGrupo(int.Parse(cmbRTC.EditValue.ToString()));

                    if (psZona.Contains("-"))
                    {
                        txtZona.Text = psZona.Split('-')[1].Trim();
                    }
                    else
                    {
                        txtZona.Text = psZona.Trim();
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbCliente_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    var dt = loLogicaNegocio.goConsultaDataTable("SELECT CardCode,CardName,LicTradNum,Phone1,Cellular,E_Mail,CreditLine FROM SBO_AFECOR.dbo.OCRD (NOLOCK) WHERE CardCode = '" + cmbCliente.EditValue.ToString() + "'");

                    if (dt.Rows.Count > 0)
                    {
                        txtCliente.Text = dt.Rows[0]["CardName"].ToString();
                        txtCupo.Text = dt.Rows[0]["CreditLine"].ToString();
                        txtTelefono.Text = dt.Rows[0]["Phone1"].ToString();
                        txtCelular.Text = dt.Rows[0]["Cellular"].ToString();
                        txtRucNegocio.Text = dt.Rows[0]["LicTradNum"].ToString();
                        txtIdentificacion.Text = dt.Rows[0]["LicTradNum"].ToString();
                    }
                    else
                    {
                        txtCliente.Text = "";
                        txtCupo.Text = "";
                        txtTelefono.Text = "";
                        txtCelular.Text = "";
                        txtRucNegocio.Text = "";
                        txtIdentificacion.Text = "";
                    }
                }

                lCambiarFechaUltimaSolicitud();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmTrSolicitudCredito_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

        private void chbCompletado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbCompletado.Checked)
                {
                    chbCompletado.Properties.Appearance.BackColor = System.Drawing.Color.PaleGreen;
                }
                else
                {
                    chbCompletado.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtTiempoActividad_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    if (string.IsNullOrEmpty(txtTiempoActividad.Text))
                    {
                        gboxMenos2Anios.Visible = false;
                    }
                    else
                    {
                        if (int.Parse(txtTiempoActividad.EditValue.ToString()) < 2)
                        {
                            gboxMenos2Anios.Visible = true;
                        }
                        else
                        {
                            gboxMenos2Anios.Visible = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbLocal_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbLocal.EditValue.ToString() == "ALQ")
                {
                    txtTiempoNegocio.Properties.ReadOnly = false;
                }
                else
                {
                    txtTiempoNegocio.Properties.ReadOnly = true;
                    txtTiempoNegocio.Text = "";
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string lbEsValido()
        {
            string psMsg = "";
            if (cmbTipoSolicitud.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + string.Format("Seleccione Tipo de Solicitud.\n");
            }
            if (cmbActividadCliente.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + string.Format("Seleccione Actividad del Cliente.\n");
            }
            if (cmbRTC.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + string.Format("Seleccione RTC.\n");
            }
            if (txtIdentificacion.Text.Length > 13)
            {
                psMsg = psMsg + string.Format("Número de Identificación invalido.\n");
            }

            #region Datos Personales 
            if (txtIdentificacionRepLegal.Text.Length < 10)
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Número de Identificación del Representante Legal invalido.\n");
            }
            else
            {
                if (!clsComun.gVerificaIdentificacion(txtIdentificacionRepLegal.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Número de Identificación del Representante Legal invalido.\n");
                }
            }
            if (dtpFechaNacimiento.DateTime.Date == DateTime.MinValue)
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Ingrese la Fecha de Nacimiento.\n");
            }
            if ((DateTime.Now.Year - dtpFechaNacimiento.DateTime.Year) < 18)
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" El cliente debe ser mayoria de edad.\n");
            }
            else if (cmbTipoSolicitud.EditValue.ToString() == "NUE" && (DateTime.Now.Year - dtpFechaNacimiento.DateTime.Year) > 65)
            {
                //psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" La edad del cliente supera los 65 años permitidos.\n");
                XtraMessageBox.Show(string.Format("\"Pestaña: Datos Personales\" La edad del cliente supera los 65 años permitidos.\n"), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (cmbTipoSolicitud.EditValue.ToString() != "NUE" && (DateTime.Now.Year - dtpFechaNacimiento.DateTime.Year) > 65)
            {
                XtraMessageBox.Show("\"Pestaña: Datos Personales\" La edad del cliente supera los 65 años permitidos y para continuar debe ingresar un garante.,", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (cmbEstadoCivil.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Seleccione Estado Civil.\n");
            }
            if (string.IsNullOrEmpty(txtCiudad.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Ingrese la Ciudad.\n");
            }
            if (string.IsNullOrEmpty(txtDireccionDomicilio.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Ingrese la Dirección de Domicilio.\n");
            }
            if (string.IsNullOrEmpty(txtTelefono.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Ingrese Teléfono.\n");
            }
            if (string.IsNullOrEmpty(txtCelular.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Ingrese Celular.\n");
            }
            if (cmbVivienda.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Seleccione Vivienda.\n");
            }
            if (!txtTiempo.Properties.ReadOnly)
            {
                if (string.IsNullOrEmpty(txtTiempo.Text) || txtTiempo.Text == "0")
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Ingrese el Tiempo / Años.\n");
                }
            }
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Ingrese el Email.\n");
            }
            else
            {
                if (!clsComun.gValidaFormatoCorreo(txtEmail.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Formato de Email invalido, corregir.\n");
                }
            }
            if (gboxConyuge.Visible)
            {
                if (string.IsNullOrEmpty(txtNombreConyuge.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Ingrese el Nombre de Conyuge.\n");
                }

                if (txtIdentificacionConyuge.Text.Length < 10)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Número de Identificación de Cónyuge invalido.\n");
                }
                else
                {
                    if (!clsComun.gVerificaIdentificacion(txtIdentificacionConyuge.Text))
                    {
                        psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Número de Identificación de Cónyuge invalido.\n");
                    }
                }
            }
            #endregion 

            #region Datos de Negocios
            if (string.IsNullOrEmpty(txtNombreComercial.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos de Negocio\" Ingrese el Nombre Comercial.\n");
            }
            if (!txtTiempoNegocio.ReadOnly)
            {
                if (string.IsNullOrEmpty(txtTiempoNegocio.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos de Negocio\" Ingrese el Tiempo / Años del Negocio.\n");
                }
            }

            if (string.IsNullOrEmpty(txtRucNegocio.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos de Negocio\" Ingrese el Ruc del Negocio.\n");
            }
            else
            {
                if (txtRucNegocio.Text.Length < 13)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos de Negocio\" Número de RUC invalido.\n");
                }
            }

            if (string.IsNullOrEmpty(txtTipoNegocio.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos de Negocio\" Ingrese el Tipo de Negocio.\n");
            }
            if (string.IsNullOrEmpty(txtCiudadNegocio.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos de Negocio\" Ingrese la Ciudad de Negocio.\n");
            }
            if (string.IsNullOrEmpty(txtDireccionNegocio.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos de Negocio\" Ingrese la Direccion de Negocio.\n");
            }
            if (string.IsNullOrEmpty(txtTelefonoNegocio.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos de Negocio\" Ingrese el Telefono de Negocio.\n");
            }
            if (string.IsNullOrEmpty(txtCelularNegocio.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos de Negocio\" Ingrese el Celular de Negocio.\n");
            }
            if (cmbLocal.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos de Negocio\" Seleccione Tipo de Local del Negocio.\n");
            }
            if (gboxMenos2Anios.Visible)
            {
                if (string.IsNullOrEmpty(txtActividadAnterior.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos de Negocio\" Ingrese la Actividad Anterior.\n");
                }
                if (string.IsNullOrEmpty(txtTiempoActividadAnterior.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos de Negocio\" Ingrese el Tiempo / Años de la Actividad Anteior.\n");
                }
                if (string.IsNullOrEmpty(txtEmpresaActividadAnterior.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos de Negocio\" Ingrese la Empresa de Actividad Anterior.\n");
                }
                if (string.IsNullOrEmpty(txtCargoActividadAnterior.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos de Negocio\" Ingrese el Cargo de la Actividad Anterior.\n");
                }
            }
            #endregion 

            #region Datos de Agricultor

            if (TabCab.TabPages[2].PageVisible)
            {
                if (string.IsNullOrEmpty(txtTiempoAgricultor.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos de Agricultor\" Ingrese el Tiempo / Años de Agricultor.\n");
                }
                if (string.IsNullOrEmpty(txtClaseCultivo.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos de Agricultor\" Ingrese la Clase de Cultivo.\n");
                }
                if (string.IsNullOrEmpty(txtNoHectarias.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos de Agricultor\" Ingrese el Número de Hectarias Cultivadas. del Agricultor.\n");
                }
                if (cmbTerreno.EditValue.ToString() == Diccionario.Seleccione)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos de Agricultor\" Seleccione Tipo de Terreno del Agricultor.\n");
                }
                if (!txtAntiguedadTerreno.Properties.ReadOnly)
                {
                    if (string.IsNullOrEmpty(txtAntiguedadTerreno.Text) || txtAntiguedadTerreno.Text == "0")
                    {
                        psMsg = psMsg + string.Format("\"Pestaña: Datos de Agricultor\" Ingrese la antiguedad / Años.\n");
                    }
                }
                if (string.IsNullOrEmpty(txtMesesCosecha.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos de Agricultor\" Ingrese la Cantidad de meses de Cosecha.\n");
                }
                if (string.IsNullOrEmpty(txtPromedioIngresos.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos de Agricultor\" Ingrese el Promedio de Ingresos.\n");
                }
                if (string.IsNullOrEmpty(txtUbicacionTerrenos.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos de Agricultor\" Ingrese la Ubicación de Terrenos.\n");
                }
            }
            #endregion

            #region Condición de Pago
            if (string.IsNullOrEmpty(txtNombreContactoPago.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Condición de Pago\" Ingrese el nombre del contacto de pago.\n");
            }
            if (string.IsNullOrEmpty(txtTelefonoContactoPago.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Condición de Pago\" Ingrese el teléfono del contacto de pago.\n");
            }
            if (string.IsNullOrEmpty(txtCelularPago.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Condición de Pago\" Ingrese el celular del contacto de pago.\n");
            }
            if (string.IsNullOrEmpty(txtCargoContacto.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Condición de Pago\" Ingrese el cargo del contacto de pago.\n");
            }
            if (string.IsNullOrEmpty(txtEmailContactoPago.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Condición de Pago\" Ingrese el email del contacto de pago.\n");
            }
            else
            {
                if (!clsComun.gValidaFormatoCorreo(txtEmailContactoPago.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Condición de Pago\" Formato de Email invalido del contacto de pago.\n");
                }
            }
            if (string.IsNullOrEmpty(txtEmailEnvioFacturaElectronica.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Condición de Pago\" Ingrese el email para envío de factura electrónica.\n");
            }
            else
            {
                if (!clsComun.gValidaFormatoCorreo(txtEmailEnvioFacturaElectronica.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Condición de Pago\" Formato de Email invalido para envío de factura electrónica.\n");
                }
            }
            if (string.IsNullOrEmpty(txtPersonaAutorizadaRealizarCompras.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Condición de Pago\" Ingrese el nombre de la persona autorizada a realizar compras.\n");
            }
            if (txtIdentificacionPersonaRealizaCompras.Text.Length < 10)
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Número de Identificación invalido de la persona autorizada a realizar compras.\n");
            }
            else
            {
                if (!clsComun.gVerificaIdentificacion(txtIdentificacionPersonaRealizaCompras.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Número de Identificación invalido de la persona autorizada a realizar compras.\n");
                }
            }
            if (string.IsNullOrEmpty(txtPersonaAutorizadaRecibirCompras.Text))
            {
                psMsg = psMsg + string.Format("\"Pestaña: Condición de Pago\" Ingrese el nombre de la persona autorizada a recibir compras.\n");
            }
            if (txtIdentificacionPersonaRecibeCompras.Text.Length < 10)
            {
                psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Número de Identificación invalido de la persona autorizada a recibir compras.\n");
            }
            else
            {
                if (!clsComun.gVerificaIdentificacion(txtIdentificacionPersonaRecibeCompras.Text))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Personales\" Número de Identificación invalido de la persona autorizada a recibir compras.\n");
                }
            }
            #endregion 

            #region Datos Generales
            int cont = 0;
            foreach (var item in (List<ProcesoCreditoRefBancaria>)bsRefBancaria.DataSource)
            {
                cont++;
                if (item.CodigoBanco == Diccionario.Seleccione)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Referencias Bancarias\" Fila # " + cont + ", Seleccione Banco.\n");
                }
                if (item.CodigoTipoCuentaBancaria == Diccionario.Seleccione)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Referencias Bancarias\" Fila # " + cont + ", Seleccione Tipo de cuenta.\n");
                }
                if (string.IsNullOrEmpty(item.NumeroCuenta))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Referencias Bancarias\" Fila # " + cont + ", Ingrese número de cuenta.\n");
                }
                if (item.FechaApertura != null)
                {
                    if (item.FechaApertura.Value > DateTime.Now.Date)
                    {
                        psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Referencias Bancarias\" Fila # " + cont + ", Fecha de apertura no debe ser mayor a la fecha actual, corregir.\n");
                    }
                }
            }

            cont = 0;
            var poParametro = loLogicaNegocio.goConsultarParametroCredito();
            foreach (var item in (List<ProcesoCreditoRefComercial>)bsRefComercial.DataSource)
            {
                cont++;
                if (string.IsNullOrEmpty(item.Nombre))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Referencias Comerciales\" Fila # " + cont + ", Ingrese nombre.\n");
                }
                if (string.IsNullOrEmpty(item.Telefono))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Referencias Comerciales\" Fila # " + cont + ", Ingrese teléfono.\n");
                }

                var poTimeSpan = DateTime.Now.Subtract(item.FechaReferenciaComercial ?? DateTime.MinValue);
                if (item.FechaReferenciaComercial == null || item.FechaReferenciaComercial == DateTime.MinValue)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Referencias Comerciales\" Fila # " + cont + ", Ingrese Fecha de Referencia.\n");
                }
                else if (poTimeSpan.Days > poParametro.DiasVigenciaFechaReferenciaComercial)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Referencias Comerciales\" Fila # " + cont + ", Fecha de Referencia NO Vigente, Tiene: {0} días, el límite es: {1} días.\n", poTimeSpan.Days, poParametro.DiasVigenciaFechaReferenciaComercial);
                }
            }

            cont = 0;
            foreach (var item in (List<SolicitudCreditoRefPersonal>)bsRefPersonal.DataSource)
            {
                cont++;
                if (item.TipoReferenciaPersonal == Diccionario.Seleccione)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Referencias Personales\" Fila # " + cont + ", Seleccione parentezco.\n");
                }
                if (string.IsNullOrEmpty(item.Nombre))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Referencias Personales\" Fila # " + cont + ", Ingrese nombre.\n");
                }
                if (string.IsNullOrEmpty(item.Telefono))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Referencias Personales\" Fila # " + cont + ", Ingrese teléfono.\n");
                }
                if (string.IsNullOrEmpty(item.Direccion))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Referencias Personales\" Fila # " + cont + ", Ingrese dirección.\n");
                }
                if (string.IsNullOrEmpty(item.Ciudad))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Referencias Personales\" Fila # " + cont + ", Ingrese ciudad.\n");
                }
            }

            cont = 0;
            foreach (var item in (List<ProcesoCreditoPropiedades>)bsPropiedades.DataSource)
            {
                cont++;
                if (item.CodigoTipoBien == Diccionario.Seleccione)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Propiedades\" Fila # " + cont + ", Seleccione tipo de bien.\n");
                }
                if (string.IsNullOrEmpty(item.Direccion))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Propiedades\" Fila # " + cont + ", Ingrese dirección.\n");
                }
                if (item.Hipoteca == Diccionario.Seleccione)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Propiedades\" Fila # " + cont + ", Seleccione hipoteca.\n");
                }
                if (item.AvaluoComercial == 0)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Propiedades\" Fila # " + cont + ", Ingrese avaluo comercial.\n");
                }
            }

            cont = 0;
            foreach (var item in (List<ProcesoCreditoOtrosActivos>)bsOtrosActivos.DataSource)
            {
                cont++;
                if (item.CodigoTipoOtrosActivos == Diccionario.Seleccione)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Otros Activos\" Fila # " + cont + ", Seleccione tipo.\n");
                }
                if (string.IsNullOrEmpty(item.Marca))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Otros Activos\" Fila # " + cont + ", Ingrese marca.\n");
                }
                if (string.IsNullOrEmpty(item.Modelo))
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Otros Activos\" Fila # " + cont + ", Ingrese modelo.\n");
                }
                if (item.Anio > DateTime.Now.Year + 1)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Otros Activos\" El año no puede se mayor que el " + DateTime.Now.Year + 1 + ", corregir.\n");
                }
                if (item.AvaluoComercial == 0)
                {
                    psMsg = psMsg + string.Format("\"Pestaña: Datos Generales - Otros Activos\" Fila # " + cont + ", Ingrese avaluo comercial.\n");
                }
            }

            #endregion


            return psMsg;
        }

        private void cmbVivienda_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    if (cmbVivienda.EditValue.ToString() == "ALQ")
                    {
                        txtTiempo.Properties.ReadOnly = false;
                        cmbTiempoMes.Properties.ReadOnly = false;
                    }
                    else
                    {
                        txtTiempo.Properties.ReadOnly = true;
                        cmbTiempoMes.Properties.ReadOnly = true;
                        cmbTiempoMes.EditValue = "0";
                        txtTiempo.Text = "";
                    }
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
                var poListaObject = loLogicaNegocio.goListar(cmbCliente.EditValue.ToString());
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Fecha Solicitud", typeof(DateTime)),
                                    new DataColumn("Usuario"),
                                    new DataColumn("Tipo Solicitud"),
                                    new DataColumn("RTC"),
                                    new DataColumn("Cupo")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdSolicitudCredito;
                    row["Fecha Solicitud"] = a.FechaSolicitud;
                    row["Usuario"] = a.UsuarioIngreso;
                    row["Tipo Solicitud"] = a.TipoSolicitud;
                    row["RTC"] = a.RTC;
                    row["Cupo"] = a.Cupo;
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Solicitudes a Copiar" };
                pofrmBuscar.Width = 1200;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    lLimpiar();
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar(true, true);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lCambiarFechaUltimaSolicitud()
        {
            if (loLogicaNegocio.gbHabilitarUltimaFechaSolicitud(txtNo.Text, cmbCliente.EditValue.ToString()))
            {
                dtpFechaUltSol.Enabled = true;
            }
            else
            {
                dtpFechaUltSol.Enabled = false;
            }
        }

        private void dtpFechaUltSol_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lMensajeFechaUltSol();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSincTiempoAgricultor_Click(object sender, EventArgs e)
        {
            try
            {
                txtTiempoAgricultor.EditValue = txtTiempoActividad.EditValue;
                cmbTiempoAgricultorMes.EditValue = cmbTiempoActividadMes.EditValue;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbTerreno_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    if (cmbTerreno.EditValue.ToString() == "ALQ")
                    {
                        txtAntiguedadTerreno.Properties.ReadOnly = false;
                        cmbAntiguedadTerrenoMes.Properties.ReadOnly = false;
                    }
                    else
                    {
                        txtAntiguedadTerreno.Properties.ReadOnly = true;
                        cmbAntiguedadTerrenoMes.Properties.ReadOnly = true;
                        cmbAntiguedadTerrenoMes.EditValue = "0";
                        txtAntiguedadTerreno.Text = "";
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
