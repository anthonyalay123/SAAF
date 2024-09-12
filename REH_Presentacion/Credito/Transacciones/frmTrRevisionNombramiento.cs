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
    public partial class frmTrRevisionNombramiento : frmBaseTrxDev
    {
        private frmTrProcesoCredito m_MainForm;

        public string lsTipo = "";
        public int lIdProceso = 0;
        public string lsTipoRuc = "";
        public string lsTipoProceso = "";
        public bool lbGuardado = false;
        public bool lbCompletado = false;
        public int lIdSol = 0;
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        clsNProcesoCredito loLogicaNegocio = new clsNProcesoCredito();
        public bool lbCerrado = false;

        public frmTrRevisionNombramiento(frmTrProcesoCredito form = null)
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


                if (lIdProceso != 0)
                {

                    if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Visible = false;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Visible = false;
                    if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Visible = false;
                    if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Visible = false;
                    if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Visible = false;
                    if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Visible = false;

                    clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboPresentacionNombramiento(), "CodigoPresentacion", true);
                    //clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboAños(), "Periodo");

                    //lIdSol = new clsNSolicitudCredito().gIdSolicitudCredito(lIdProceso);

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
                var poLista = (List<ProcesoCreditoNombramiento>)bsDatos.DataSource;

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
                var psMsgVal = lsEsValido();
                if (string.IsNullOrEmpty(psMsgVal))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ProcesoCredito poObject = new ProcesoCredito();
                        poObject.IdProcesoCredito = lIdProceso;
                        poObject.Identificacion = txtIdentificacion.EditValue.ToString();
                        poObject.RepresentanteLegal = txtNombreComercial.EditValue.ToString();
                        poObject.ComentariosNombramiento = txtComentarios.Text;
                        poObject.ProcesoCreditoNombramiento = (List<ProcesoCreditoNombramiento>)bsDatos.DataSource;

                        string psMsg = loLogicaNegocio.gsGuardarRevisionNombramiento(poObject, lsTipo, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

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

            bsDatos.DataSource = new List<ProcesoCreditoNombramiento>();
            gcDatos.DataSource = bsDatos;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);

            dgvDatos.Columns["IdProcesoCreditoNombramiento"].Visible = false;
            dgvDatos.Columns["IdProcesoCredito"].Visible = false;
            dgvDatos.Columns["FechaCaducidad"].OptionsColumn.ReadOnly = true;

            dgvDatos.Columns["FechaInscripcion"].Caption = "Fecha Inscripción";
            dgvDatos.Columns["Periodo"].Caption = "Periodo (Años)";
            dgvDatos.Columns["CodigoPresentacion"].Caption = "Representación";

            dgvDatos.Columns["Cargo"].Width = 200;
        }


        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultar(Convert.ToInt32(txtNo.Text.Trim()));

                txtNo.EditValue = poObject.IdProcesoCredito;
                txtIdentificacion.Text = poObject.Identificacion;
                txtNombreComercial.Text = poObject.RepresentanteLegal;
                lIdProceso = poObject.IdProcesoCredito;
                lblFecha.Text = poObject.Fecha.ToString("dd/MM/yyyy");
                txtComentarios.Text = poObject.ComentariosNombramiento;
                bsDatos.DataSource = poObject.ProcesoCreditoNombramiento;
                dgvDatos.RefreshData();

                if (loLogicaNegocio.gsGetEstadoReqCreCheckList(poObject.IdProcesoCredito, Diccionario.Tablas.CheckList.NombramientotRepLeg) == Diccionario.Aprobado)
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

        private string lsEsValido()
        {
            string psMsg = "";
            int cont = 0;
            var poLista = (List<ProcesoCreditoNombramiento>)bsDatos.DataSource;
            if (poLista.Count > 0)
            {
                foreach (var item in poLista)
                {
                    cont++;
                    if (item.FechaInscripcion.Date > DateTime.Now.Date)
                    {
                        psMsg = psMsg + string.Format("\"Datos del Nombramiento\" Fila # " + cont + ", Fecha de inscripción no debe ser mayor a la fecha actual, corregir.\n");
                    }
                    if (item.FechaCaducidad.Date < DateTime.Now.Date)
                    {
                        psMsg = psMsg + string.Format("\"Datos del Nombramiento\" Fila # " + cont + ", Fecha de caducidad debe ser mayor a la fecha actual, corregir.\n");
                    }
                    if (string.IsNullOrEmpty(item.Cargo))
                    {
                        psMsg = psMsg + string.Format("\"Datos del Nombramiento\" Fila # " + cont + ", Ingrese el cargo, corregir.\n");
                    }
                    //if (item.Periodo == 0)
                    //{
                    //    psMsg = psMsg + string.Format("\"Datos del Nombramiento\" Fila # " + cont + ", Ingrese los años, corregir.\n");
                    //}
                    if (item.CodigoPresentacion == Diccionario.Seleccione)
                    {
                        psMsg = psMsg + string.Format("\"Datos del Nombramiento\" Fila # " + cont + ", Seleccione Representación.\n");
                    }
                }
            }
            else
            {
                psMsg = psMsg + string.Format("\"Datos del Nombramiento\" Ingrese datos del Nombramiento.\n");
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
                var poLista = (List<ProcesoCreditoNombramiento>)bsDatos.DataSource;
                poLista.LastOrDefault().CodigoPresentacion = Diccionario.Seleccione;
                poLista.LastOrDefault().Periodo = 0;
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
