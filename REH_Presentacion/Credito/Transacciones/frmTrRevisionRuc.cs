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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Credito.Transacciones
{
    public partial class frmTrRevisionRuc : frmBaseTrxDev
    {
        private frmTrProcesoCredito m_MainForm;

        public string lsTipo = "";
        public int lIdProceso = 0;
        public string lsTipoRuc = "";
        public string lsIdentificacion = "";
        public string lsTipoProceso = "";
        public bool lbGuardado = false;
        public bool lbCompletado = false;
        public int lIdSol = 0;
        public bool lbCerrado = false;
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        clsNProcesoCredito loLogicaNegocio = new clsNProcesoCredito();

        public frmTrRevisionRuc(frmTrProcesoCredito form = null)
        {
            m_MainForm = form;
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        private void frmTrPantillaSeguro_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();

                dtpFechaReinicioAct.ValueChanged += new EventHandler(FechaValue_Changed);
                clsComun.gResetDtpCheck(ref dtpFechaReinicioAct);

                if (lIdProceso != 0)
                {
                    clsComun.gLLenarCombo(ref cmbObligadoLlevarContabilidad, loLogicaNegocio.goConsultarComboSINO(), true);
                    clsComun.gLLenarCombo(ref cmbTipoRuc, loLogicaNegocio.goConsultarComboTipoPersonaNatJur(), true);
                    clsComun.gLLenarCombo(ref cmbEstadoRuc, loLogicaNegocio.goConsultarComboEstadoRUC(), true);

                    if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Visible = false;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Visible = false;
                    if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Visible = false;
                    if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Visible = false;
                    if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Visible = false;
                    if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Visible = false;

                    dtpFechaInicioActividad.DateTime = DateTime.MinValue;
                    dtpFechaReinicioActividad.DateTime = DateTime.MinValue;
                    dtpFechaUltimaActualizacion.DateTime = DateTime.MinValue;

                    cmbTipoRuc.EditValue = lsTipoRuc;
                    txtIdentificacion.Text = lsIdentificacion;

                    lIdSol = new clsNSolicitudCredito().gIdSolicitudCredito(lIdProceso);

                    if (lIdProceso != 0)
                    {
                        txtNo.Text = lIdProceso.ToString();
                        lConsultar();
                    }

                    //var pIdSol = loLogicaNegocio.gIdSolicitudCredito(lIdProceso);
                    //if (pIdSol != 0)
                    //{
                    //    cmbTipoRuc.EditValue = lsTipoRuc;
                    //    txtNo.Text = pIdSol.ToString();
                    //    lConsultar();
                    //}
                    //else
                    //{
                    //    XtraMessageBox.Show("No Existe Solicitud de Crédito para revisión de RUC", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    Close();
                    //}

                    //XtraMessageBox.Show("Revisar la fecha de última actualización en la Web", "¡IMPORTANTE!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var poLista = (List<ProcesoCreditoRucDireccion>)bsDatos.DataSource;

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


        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();

                var psMsgPri = lbEsValido();

                if (string.IsNullOrEmpty(psMsgPri))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ProcesoCreditoRuc poObject = new ProcesoCreditoRuc();
                        poObject.IdProcesoCredito = lIdProceso;
                        poObject.Identificacion = txtIdentificacion.EditValue.ToString();
                        poObject.NombreComercial = txtNombreComercial.EditValue.ToString();
                        poObject.EstadoRuc = cmbEstadoRuc.EditValue.ToString();
                        poObject.ObligadoLlevarCantidadRuc = cmbObligadoLlevarContabilidad.EditValue.ToString();
                        poObject.Identificacion = txtIdentificacion.Text;
                        if (dtpFechaInicioActividad.DateTime != DateTime.MinValue)
                        {
                            poObject.FechaInicioActividadRuc = dtpFechaInicioActividad.DateTime;
                        }
                        if (dtpFechaUltimaActualizacion.DateTime != DateTime.MinValue)
                        {
                            poObject.FechaUltActualizacionRuc = dtpFechaUltimaActualizacion.DateTime;
                        }
                        if (dtpFechaReinicioAct.Checked)
                        {
                            poObject.FechaReinicioActividadRuc = dtpFechaReinicioAct.Value;
                        }
                        else
                        {
                            poObject.FechaReinicioActividadRuc = null;
                        }
                        poObject.OtrasActividades = txtOtrasActividades.Text;
                        poObject.ActividadesSecundarias = txtActividadesSegundarias.Text;
                        poObject.CodigoTipoNegocio = txtActividadPrincipal.Text;
                        poObject.Comentarios = txtComentarios.Text;
                        poObject.ProcesoCreditoRucDireccion = (List<ProcesoCreditoRucDireccion>)bsDatos.DataSource;


                        string psMsg = loLogicaNegocio.gsGuardarRevisionRuc(poObject, lsTipo, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, lIdSol);

                        if (string.IsNullOrEmpty(psMsg))
                        {
                            if (m_MainForm != null)
                            {
                                new clsNProcesoCredito().gsActualzarRequerimientoCredito(lIdProceso, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                else
                {
                    XtraMessageBox.Show(psMsgPri, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            //if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            //if (tstBotones.Items["btnPlantilla"] != null) tstBotones.Items["btnPlantilla"].Click += btnExportar_Click;

            //txtPlazoCredito.KeyPress += new KeyPressEventHandler(SoloNumeros);
            //txtPlazoAprobado.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtNombreComercial.KeyPress += new KeyPressEventHandler(SoloLetras);
            //txtEstadoRuc.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtIdentificacion.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            //txtObservacionesSeguro.KeyPress += new KeyPressEventHandler(SoloLetras);

            if (lsTipo == "REQ")
            {
                gbxDatosRevision.Enabled = false;
            }
            else
            {
                gbxDatosRevision.Enabled = true;
            }

            bsDatos.DataSource = new List<ProcesoCreditoRucDireccion>();
            gcDatos.DataSource = bsDatos;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);

            dgvDatos.Columns["IdProcesoCreditoRucDireccion"].Visible = false;
            dgvDatos.Columns["IdProcesoCredito"].Visible = false;
            dgvDatos.Columns["Principal"].Width = 15;


        }


        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultar(Convert.ToInt32(txtNo.Text.Trim()));

                txtNo.EditValue = poObject.IdProcesoCredito;
                txtIdentificacion.Text = poObject.ProcesoCreditoRuc.Identificacion;
                txtNombreComercial.Text = poObject.ProcesoCreditoRuc.NombreComercial;
                lIdProceso = poObject.IdProcesoCredito;
                if (poObject.ProcesoCreditoRuc.FechaIngreso != DateTime.MinValue)
                {
                    lblFecha.Text = poObject.ProcesoCreditoRuc.FechaIngreso.ToString("dd/MM/yyyy");
                }

                if (poObject.ProcesoCreditoRuc.EstadoRuc != null)
                {
                    cmbEstadoRuc.EditValue = poObject.ProcesoCreditoRuc.EstadoRuc;
                }

                if (poObject.ProcesoCreditoRuc.ObligadoLlevarCantidadRuc != null)
                {
                    cmbObligadoLlevarContabilidad.EditValue = poObject.ProcesoCreditoRuc.ObligadoLlevarCantidadRuc;
                }

                dtpFechaInicioActividad.EditValue = poObject.ProcesoCreditoRuc.FechaInicioActividadRuc;
                dtpFechaUltimaActualizacion.EditValue = poObject.ProcesoCreditoRuc.FechaUltActualizacionRuc;
                txtActividadPrincipal.Text = poObject.ProcesoCreditoRuc.CodigoTipoNegocio;
                txtActividadesSegundarias.Text = poObject.ProcesoCreditoRuc.ActividadesSecundarias;
                txtOtrasActividades.Text = poObject.ProcesoCreditoRuc.OtrasActividades;
                txtComentarios.Text = poObject.ProcesoCreditoRuc.Comentarios;
                if (poObject.ProcesoCreditoRuc.FechaReinicioActividadRuc != null)
                {
                    dtpFechaReinicioAct.Checked = true;
                    dtpFechaReinicioAct.Value = poObject.ProcesoCreditoRuc.FechaReinicioActividadRuc ?? DateTime.MinValue;
                }

                bsDatos.DataSource = poObject.ProcesoCreditoRuc.ProcesoCreditoRucDireccion;
                dgvDatos.RefreshData();

                if (loLogicaNegocio.gsGetEstadoReqCreCheckList(poObject.IdProcesoCredito, Diccionario.Tablas.CheckList.RucCertSriRuc) == Diccionario.Aprobado)
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

        private void lLimpiar()
        {
            txtNo.EditValue = "";
            txtIdentificacion.EditValue = 0;
            cmbObligadoLlevarContabilidad.EditValue = Diccionario.Seleccione;
            txtNombreComercial.EditValue = 0;
            lIdProceso = 0;
            lblFecha.Text = "";

        }

        private void frmTrPantillaSeguro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

        private string lbEsValido()
        {
            string psMsg = "";

            if (txtIdentificacion.Text.Length < 10)
            {
                psMsg = psMsg + string.Format("Número de Identificación invalido.\n");
            }
            else
            {
                if (!clsComun.gVerificaIdentificacion(txtIdentificacion.Text))
                {
                    psMsg = psMsg + string.Format("Número de Identificación invalido.\n");
                }
            }

            if (cmbEstadoRuc.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + "Seleccione el estado de RUC.\n";
            }

            if (string.IsNullOrEmpty(txtNombreComercial.Text))
            {
                psMsg = psMsg + string.Format("Ingrese el nombre comercial.\n");
            }

            if (cmbTipoRuc.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + "Seleccione el tipo de RUC.\n";
            }

            if (cmbObligadoLlevarContabilidad.EditValue != null)
            {
                if (cmbObligadoLlevarContabilidad.EditValue.ToString() == Diccionario.Seleccione)
                {
                    psMsg = psMsg + "Seleccione si es obligado a llevar contabilidad. \n";
                }
            }
            else
            {
                psMsg = psMsg + "Seleccione si es obligado a llevar contabilidad. \n";
            }


            if (dtpFechaInicioActividad.DateTime.Date == DateTime.MinValue.Date)
            {
                psMsg = psMsg + "Ingrese fecha de Inicio de Actividad.\n";
            }

            if (dtpFechaInicioActividad.DateTime.Date > DateTime.Now.Date)
            {
                psMsg = psMsg + "La fecha de Inicio de Actividad no debe ser mayor a la fecha actual.\n";
            }
            /*
            if (dtpFechaUltimaActualizacion.DateTime.Date == DateTime.MinValue.Date)
            {
                psMsg = psMsg + "Ingrese fecha de última actualización.\n";
            }
            */
            if (dtpFechaUltimaActualizacion.DateTime.Date > DateTime.Now.Date)
            {
                //psMsg = psMsg + "La fecha de última actualización no debe ser mayor a la fecha actual.\n";
                XtraMessageBox.Show("La fecha de última actualización no debe ser mayor a la fecha actual.\n", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (dtpFechaReinicioAct.Checked)
            {
                if (dtpFechaReinicioAct.Value.Date == DateTime.MinValue.Date)
                {
                    psMsg = psMsg + "Ingrese fecha de reinicio de actividad.\n";
                }
            }

            DateTime pdFechaActual = DateTime.Now;
            int DiasVigencia = 1825; // Ruc Vigencia 5 años . 
            if (dtpFechaUltimaActualizacion.DateTime != DateTime.MinValue)
            {
                if (pdFechaActual.Subtract(dtpFechaUltimaActualizacion.DateTime).Days > DiasVigencia)
                {
                    //psMsg = string.Format("{0}Fecha de actualización no está vigente, Fecha de Vigencia mínima valida {1}\n", psMsg, pdFechaActual.AddDays(DiasVigencia * -1).ToString("dd/MM/yyyy"));
                    XtraMessageBox.Show(string.Format("Fecha de actualización no está vigente, Fecha de Vigencia mínima valida {0}\n", pdFechaActual.AddDays(DiasVigencia * -1).ToString("dd/MM/yyyy")), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (pdFechaActual.Subtract(dtpFechaInicioActividad.DateTime).Days > DiasVigencia)
                {
                    //psMsg = string.Format("{0}Fecha de inicio de actividad no está vigente, Fecha de Vigencia mínima valida {1}\n", psMsg, pdFechaActual.AddDays(DiasVigencia * -1).ToString("dd/MM/yyyy"));
                    XtraMessageBox.Show(string.Format("Fecha de inicio de actividad no está vigente, Fecha de Vigencia mínima valida {0}\n", pdFechaActual.AddDays(DiasVigencia * -1).ToString("dd/MM/yyyy")), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            return psMsg;
        }

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                bsDatos.AddNew();
                dgvDatos.Focus();
                dgvDatos.ShowEditor();
                dgvDatos.UpdateCurrentRow();
                var poLista = (List<ProcesoCreditoRucDireccion>)bsDatos.DataSource;
                //poLista.LastOrDefault().CodigoTipoCuentaBancaria = Diccionario.Seleccione;
                dgvDatos.RefreshData();
                dgvDatos.FocusedColumn = dgvDatos.VisibleColumns[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
