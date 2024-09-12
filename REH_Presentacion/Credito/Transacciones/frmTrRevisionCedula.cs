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
    public partial class frmTrRevisionCedula : frmBaseTrxDev
    {
        private frmTrProcesoCredito m_MainForm;

        public string lsTipo = "";
        public int lIdProceso = 0;
        public string lsTipoRuc = "";
        public string lsTipoProceso = "";
        public bool lbGuardado = false;
        public bool lbCompletado = false;
        public int lIdSol = 0;
        public bool lbCerrado = false;

        clsNProcesoCredito loLogicaNegocio = new clsNProcesoCredito();

        public frmTrRevisionCedula(frmTrProcesoCredito form = null)
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
                    clsComun.gLLenarCombo(ref cmbEstadiCivil, loLogicaNegocio.goConsultarComboEstadoCivil(), true);

                    if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Visible = false;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Visible = false;
                    if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Visible = false;
                    if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Visible = false;
                    if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Visible = false;
                    if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Visible = false;

                    dtpFechaCaducidad.DateTime = DateTime.MinValue;

                    lIdSol = new clsNSolicitudCredito().gIdSolicitudCredito(lIdProceso);

                    if (lIdProceso != 0)
                    {
                        txtNo.Text = lIdProceso.ToString();
                        lConsultar();
                    }

                    if (lbCerrado)
                    {
                        if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                        if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                    }
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

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                var psMsgPri = lbEsValido();
                if (string.IsNullOrEmpty(psMsgPri))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ProcesoCreditoCedula poObject = new ProcesoCreditoCedula();
                        poObject.IdProcesoCredito = lIdProceso;
                        poObject.IdentificacionRepLegal = txtIdentificacion.EditValue.ToString();
                        poObject.FechaCaducidadCedulaRepLegal = dtpFechaCaducidad.DateTime;
                        poObject.FechaNacimiento = dtpFechaNacimiento.DateTime;
                        poObject.CodigoEstadoCivil = cmbEstadiCivil.EditValue.ToString();

                        poObject.NombreConyuge = txtNombreConyuge.Text;
                        poObject.IdentificacionConyuge = txtIdentificacionConyuge.Text;
                        poObject.ComentarioRevisionCedula = txtObservaciones.Text;


                        string psMsg = loLogicaNegocio.gsGuardarRevisionCedula(poObject, lsTipo, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, lIdSol);

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
            txtEdad.KeyPress += new KeyPressEventHandler(SoloNumeros);
            //txtEstadoRuc.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtIdentificacion.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            //txtObservacionesSeguro.KeyPress += new KeyPressEventHandler(SoloLetras);

        }


        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultar(Convert.ToInt32(txtNo.Text.Trim()));

                txtNo.EditValue = poObject.IdProcesoCredito;
                txtIdentificacion.Text = poObject.ProcesoCreditoCedula?.IdentificacionRepLegal;
                //txtIdentificacion.Text = poObject.ProcesoCreditoCedula?.Identificacion;
                txtEdad.Text = poObject.ProcesoCreditoCedula?.Edad.ToString();
                lIdProceso = poObject.IdProcesoCredito;
                if (poObject.ProcesoCreditoCedula?.FechaIngreso != DateTime.MinValue)
                {
                    lblFecha.Text = poObject.ProcesoCreditoCedula?.FechaIngreso.ToString("dd/MM/yyyy");
                }
                txtIdentificacionConyuge.Text = poObject.ProcesoCreditoCedula?.IdentificacionConyuge;
                txtNombreConyuge.Text = poObject.ProcesoCreditoCedula?.NombreConyuge;
                dtpFechaNacimiento.EditValue = poObject.ProcesoCreditoCedula?.FechaNacimiento;
                cmbEstadiCivil.EditValue = string.IsNullOrEmpty(poObject.ProcesoCreditoCedula?.CodigoEstadoCivil) ? Diccionario.Seleccione : poObject.ProcesoCreditoCedula?.CodigoEstadoCivil;
                txtObservaciones.Text = poObject.ProcesoCreditoCedula?.ComentarioRevisionCedula;
                if (poObject.ProcesoCreditoCedula?.FechaCaducidadCedulaRepLegal != null)
                {
                    dtpFechaCaducidad.EditValue = poObject.ProcesoCreditoCedula?.FechaCaducidadCedulaRepLegal;
                }

                if (loLogicaNegocio.gsGetEstadoReqCreCheckList(poObject.IdProcesoCredito, Diccionario.Tablas.CheckList.CedulaTitularRepLegal) == Diccionario.Aprobado)
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
            cmbEstadiCivil.EditValue = Diccionario.Seleccione;
            txtEdad.EditValue = 0;
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
                psMsg = psMsg + string.Format("Número de Identificación del Representante Legal invalido.\n");
            }
            else
            {
                if (!clsComun.gVerificaIdentificacion(txtIdentificacion.Text))
                {
                    psMsg = psMsg + string.Format("Número de Identificación del Representante Legal invalido.\n");
                }
            }

            if (dtpFechaCaducidad.DateTime < DateTime.Now)
            {
                psMsg = psMsg + "La fecha de caducidad debe ser mayor a la fecha actual. \n";
            }
            else if (dtpFechaCaducidad.DateTime.Date == DateTime.MinValue.Date)
            {
                psMsg = psMsg + "Ingrese fecha de Caducidad. \n";
            }
            if (dtpFechaNacimiento.DateTime.Date == DateTime.MinValue)
            {
                psMsg = psMsg + string.Format("Ingrese la Fecha de Nacimiento.\n");
            }
            if ((DateTime.Now.Year - dtpFechaNacimiento.DateTime.Year) < 18)
            {
                psMsg = psMsg + string.Format("El cliente debe ser mayoria de edad.\n");
            }


            if (cmbEstadiCivil.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + string.Format("Seleccione Estado Civil.\n");
            }

            if (cmbEstadiCivil.EditValue.ToString() == "CAS")
            {
                if (txtIdentificacionConyuge.Text.Length < 10)
                {
                    psMsg = psMsg + string.Format("Número de Identificación del cónyuge invalido.\n");
                }
                else
                {
                    if (!clsComun.gVerificaIdentificacion(txtIdentificacionConyuge.Text))
                    {
                        psMsg = psMsg + string.Format("Número de Identificación del cónyuge invalido.\n");
                    }
                }

                if (string.IsNullOrEmpty(txtNombreConyuge.Text))
                {
                    psMsg = psMsg + string.Format("Ingrese el nombre de su cónyuge.\n");
                }
            }





            return psMsg;
        }

        private void dtpFechaNacimiento_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtEdad.EditValue = (DateTime.Now.Year - dtpFechaNacimiento.DateTime.Year);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
