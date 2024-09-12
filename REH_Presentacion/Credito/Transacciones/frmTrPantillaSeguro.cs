using CRE_Negocio.Transacciones;
using DevExpress.XtraEditors;
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
    public partial class frmTrPantillaSeguro : frmBaseTrxDev
    {

        private frmTrProcesoCredito m_MainForm;

        public int lIdProceso = 0;
        public string lsCardCode = "";
        public string lsTipoProceso = "";
        public bool lbGuardado = false;
        public bool lbCompletado = false;
        public decimal ldcCupoSolicitado = 0;
        public int liPlazoSolicitado = 0;
        public bool lbCerrado = false;

        clsNPlantillaSeguro loLogicaNegocio = new clsNPlantillaSeguro();

        public frmTrPantillaSeguro(frmTrProcesoCredito form = null)
        {
            m_MainForm = form;
            InitializeComponent();
        }

        private void frmTrPantillaSeguro_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();

                if (lIdProceso != 0)
                {

                    if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Visible = false;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Visible = false;
                    if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Visible = false;
                    if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Visible = false;
                    if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Visible = false;
                    if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Visible = false;

                    var dt = loLogicaNegocio.goConsultaDataTable("SELECT CardCode,CardName,LicTradNum,Phone1,Cellular,E_Mail,CreditLine FROM SBO_AFECOR.dbo.OCRD (NOLOCK) WHERE CardCode = '" + lsCardCode + "'");

                    if (dt.Rows.Count > 0)
                    {
                        txtNombre.Text = dt.Rows[0]["CardName"].ToString();
                        txtNumeroIdentificacion.Text = dt.Rows[0]["LicTradNum"].ToString();
                        txtTelefono.Text = dt.Rows[0]["Phone1"].ToString();
                    }
                    else
                    {
                        txtNombre.Text = "";
                        txtNumeroIdentificacion.Text = "";
                        txtTelefono.Text = "";
                    }

                    txtCupoSolicitado.EditValue = ldcCupoSolicitado;
                    if (ldcCupoSolicitado > 0)
                    {
                        txtCupoSolicitado.ReadOnly = true;
                    }
                    //txtPlazoCredito.ReadOnly = true;
                    txtPlazoCredito.EditValue = liPlazoSolicitado;
                    if (liPlazoSolicitado > 0)
                    {
                        txtPlazoCredito.ReadOnly = true;
                    }
                    var pIdSol = loLogicaNegocio.gIdPlantillaSeguro(lIdProceso);
                    if (pIdSol != 0)
                    {
                        txtNo.Text = pIdSol.ToString();
                        lConsultar();
                    }
                    else
                    {
                        frmBusqueda frm = new frmBusqueda(loLogicaNegocio.goConsultaDataTable("SELECT Address,Street,City FROM SBO_AFECOR.dbo.CRD1 WHERE CardCode = '" + lsCardCode + "'")) { Text = "Seleccione la Dirección" };
                        frm.Width = 1200;
                        if (frm.ShowDialog() == DialogResult.OK)
                        {

                            string psAddress = frm.lsCodigoSeleccionado;
                            var dt2 = loLogicaNegocio.goConsultaDataTable("SELECT Street,City FROM SBO_AFECOR.dbo.CRD1 WHERE CardCode = '" + lsCardCode + "' AND Address = '" + psAddress + "'");
                            if (dt.Rows.Count > 0)
                            {
                                txtDireccion.Text = dt2.Rows[0]["Street"].ToString();
                                txtCiudad.Text = dt2.Rows[0]["City"].ToString();
                            }
                        }
                    }

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
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Razón Social"),
                                    new DataColumn("Ruc"),
                                    new DataColumn("Cupo")

                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdPlantillaSeguro;
                    row["Fecha"] = a.Fecha;
                    row["Razón Social"] = a.RazonSocial;
                    row["Ruc"] = a.Ruc;
                    row["Cupo"] = a.CupoSolicitado;
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
                if (lbEsValido())
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {


                        PlantillaSeguro poObject = new PlantillaSeguro();
                        poObject.IdPlantillaSeguro = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                        poObject.Pais = "ECUADOR";
                        poObject.Ruc = txtNumeroIdentificacion.Text;
                        poObject.RazonSocial = txtNombre.Text;
                        poObject.Direccion = txtDireccion.Text;
                        poObject.Ciudad = txtCiudad.Text;
                        poObject.Telefono = txtTelefono.Text;
                        poObject.PersonaContacto = txtPersonaContacto.Text;
                        poObject.CorreoElectronico = txtCorreoElectronico.Text;
                        poObject.CupoSolicitado = string.IsNullOrEmpty(txtCupoSolicitado.Text) ? 0 : decimal.Parse(txtCupoSolicitado.EditValue.ToString());
                        poObject.ProyeccionVentasAnuales = string.IsNullOrEmpty(txtEstimadoVentasAnual.Text) ? 0 : decimal.Parse(txtEstimadoVentasAnual.EditValue.ToString());
                        poObject.PlazoCreditoSolicitado = string.IsNullOrEmpty(txtPlazoCredito.Text) ? 0 : int.Parse(txtPlazoCredito.EditValue.ToString());
                        poObject.IdProcesoCredito = lIdProceso;

                        var poLista = new List<PlantillaSeguro>();

                        poLista.Add(poObject);

                        string psMsg = loLogicaNegocio.gsGuardar(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lbGuardado = true;
                            lbCompletado = true;
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
        /// Evento del botón Plantilla, Exporta Plantilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                GridControl gc = new GridControl();
                BindingSource bs = new BindingSource();
                GridView dgv = new GridView();

                gc.DataSource = bs;
                gc.MainView = dgv;
                gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                dgv.GridControl = gc;
                this.Controls.Add(gc);

                bs.DataSource = loLogicaNegocio.goPlantillaSeguro(!string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0);

                dgv.PostEditor();
                clsComun.gOrdenarColumnasGridFull(dgv);
                //dgv.BestFitColumns();
                //dgv.OptionsView.BestFitMode = GridBestFitMode.Full;
                // Exportar Datos
                clsComun.gSaveFile(gc, "PlantillaSeguro_" + txtNombre.Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");

                gc.Visible = false;

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
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            if (tstBotones.Items["btnPlantilla"] != null) tstBotones.Items["btnPlantilla"].Click += btnExportar_Click;

            txtPlazoCredito.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtCiudad.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtDireccion.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtNombre.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtNumeroIdentificacion.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtPersonaContacto.KeyPress += new KeyPressEventHandler(SoloLetras);

        }


        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultar(Convert.ToInt32(txtNo.Text.Trim()));

                txtNo.EditValue = poObject.IdPlantillaSeguro;
                txtNumeroIdentificacion.Text = poObject.Ruc;
                txtNombre.Text = poObject.RazonSocial;
                txtDireccion.Text = poObject.Direccion;
                txtCiudad.Text = poObject.Ciudad;
                txtTelefono.Text = poObject.Telefono;
                txtPersonaContacto.Text = poObject.PersonaContacto;
                txtCorreoElectronico.Text = poObject.CorreoElectronico;
                txtCupoSolicitado.EditValue = ldcCupoSolicitado == 0 ? poObject.CupoSolicitado : ldcCupoSolicitado;
                txtEstimadoVentasAnual.EditValue = poObject.ProyeccionVentasAnuales;
                txtPlazoCredito.EditValue = liPlazoSolicitado == 0 ? poObject.PlazoCreditoSolicitado : liPlazoSolicitado;
                lIdProceso = poObject.IdProcesoCredito ?? 0;
                lblFecha.Text = poObject.Fecha.ToString("dd/MM/yyyy");

                if (loLogicaNegocio.gsGetEstadoReqCreCheckList(poObject.IdProcesoCredito ?? 0, Diccionario.Tablas.CheckList.PlantillaCalifSeguro) == Diccionario.Aprobado)
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
            txtNumeroIdentificacion.Text = "";
            txtNombre.Text = "";
            txtDireccion.Text = "";
            txtCiudad.Text = "";
            txtTelefono.Text = "";
            txtPersonaContacto.Text = "";
            txtCorreoElectronico.Text = "";
            txtCupoSolicitado.EditValue = 0;
            txtEstimadoVentasAnual.EditValue = 0;
            txtPlazoCredito.EditValue = "";
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

        private bool lbEsValido()
        {

            if (string.IsNullOrEmpty(txtNumeroIdentificacion.Text.Trim()))
            {
                XtraMessageBox.Show("Número de Identificación no puede estar vacío.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                //if (!clsComun.gVerificaIdentificacion(txtNumeroIdentificacion.Text.Trim()))
                if (txtNumeroIdentificacion.Text.Trim().Length != 13)
                {
                    XtraMessageBox.Show("Ruc Ingresado no válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception)
            {
                XtraMessageBox.Show("Número de Identificación inválido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!clsComun.gValidaFormatoCorreo(txtCorreoElectronico.Text))
            {
                XtraMessageBox.Show("Correo no tiene el formato correcto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            return true;
        }
    }
}
