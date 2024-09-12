using CRE_Negocio.Transacciones;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Credito;
using REH_Presentacion.Comun;
using REH_Presentacion.Credito.PopUp;
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
    public partial class frmTrPantillaSeguroRevision : frmBaseTrxDev
    {
        private frmTrProcesoCredito m_MainForm;
        public int lIdProceso = 0;
        public string lsCardCode = "";
        public string lsTipoProceso = "";
        public bool lbGuardado = false;
        public bool lbCompletado = false;
        public List<string> psDocumentosCargados = new List<string>();
        public bool lbCerrado = false;

        clsNPlantillaSeguro loLogicaNegocio = new clsNPlantillaSeguro();
        PlantillaSeguro poEntidadAdj = new PlantillaSeguro();

        public frmTrPantillaSeguroRevision(frmTrProcesoCredito form = null)
        {
            m_MainForm = form;
            InitializeComponent();
        }

        private void frmTrPantillaSeguro_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();

                clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstadoPlanilla(), true);

                if (lIdProceso != 0)
                {

                    if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Visible = false;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Visible = false;
                    if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Visible = false;
                    if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Visible = false;
                    if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Visible = false;
                    if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Visible = false;
                    dtpFechaRespuesta.DateTime = DateTime.Now;

                    var pIdSol = loLogicaNegocio.gIdPlantillaSeguro(lIdProceso);
                    if (pIdSol != 0)
                    {
                        loLogicaNegocio.gActualizaCheckListEnRevision(pIdSol, clsPrincipal.gsUsuario);
                        txtNo.Text = pIdSol.ToString();
                        lConsultar();
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
                        poObject.CupoSolicitado = decimal.Parse(txtCreditoSolicitado.EditValue.ToString());
                        poObject.CreditoAprobado = decimal.Parse(txtCreditoAprobado.EditValue.ToString());
                        poObject.PlazoCreditoSolicitado = int.Parse(txtPLazoSolicitado.EditValue.ToString());
                        poObject.PlazoAprobado = int.Parse(txtPlazoAprobado.EditValue.ToString());
                        poObject.Estado = cmbEstado.EditValue.ToString();
                        poObject.ObservacionesSeguro = txtObservacionesSeguro.Text;
                        poObject.Comentarios = txtComentarios.Text;
                        poObject.IdProcesoCredito = lIdProceso;
                        poObject.FechaRespuesta = dtpFechaRespuesta.DateTime;
                        poObject.ArchivoAdjunto = poEntidadAdj.ArchivoAdjunto;
                        poObject.NombreOriginal = poEntidadAdj.NombreOriginal;
                        poObject.RutaOrigen = poEntidadAdj.RutaOrigen;
                        poObject.RutaDestino = poEntidadAdj.RutaDestino;

                        var poLista = new List<PlantillaSeguro>();
                        var poListaAdd = new List<ProcesoCreditoDetalleRevision>();

                        poLista.Add(poObject);

                        if (cmbEstado.EditValue.ToString() != "NOMINADO")
                        {
                            DialogResult dialogResult2 = XtraMessageBox.Show("¿Desea enviar documentos obligatorios a Jefe de Alamacén?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                            if (dialogResult2 == DialogResult.Yes)
                            {
                                frmCheckList frm = new frmCheckList();
                                frm.tId = lIdProceso;
                                frm.ShowDialog();

                                if (frm.pbAcepto)
                                {
                                    poListaAdd = frm.ComisionDetalle;
                                }
                            }
                        }

                        string psMsg = loLogicaNegocio.gsGuardarRevision(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, poListaAdd);

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
            //if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            if (tstBotones.Items["btnPlantilla"] != null) tstBotones.Items["btnPlantilla"].Click += btnExportar_Click;

            //txtPlazoCredito.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtPlazoAprobado.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtPLazoSolicitado.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtCreditoAprobado.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtCreditoSolicitado.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            //txtObservacionesSeguro.KeyPress += new KeyPressEventHandler(SoloLetras);

        }


        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultar(Convert.ToInt32(txtNo.Text.Trim()));

                txtNumeroIdentificacion.Text = poObject.Ruc;
                txtNombre.Text = poObject.RazonSocial;
                txtDireccion.Text = poObject.Direccion;
                txtCiudad.Text = poObject.Ciudad;
                txtTelefono.Text = poObject.Telefono;
                txtPersonaContacto.Text = poObject.PersonaContacto;
                txtCorreoElectronico.Text = poObject.CorreoElectronico;

                txtNo.EditValue = poObject.IdPlantillaSeguro;
                txtCreditoSolicitado.Text = poObject.CupoSolicitado.ToString();
                txtCreditoAprobado.Text = poObject.CreditoAprobado.ToString();
                txtPLazoSolicitado.Text = poObject.PlazoCreditoSolicitado.ToString();
                txtPlazoAprobado.Text = poObject.PlazoAprobado.ToString();
                cmbEstado.EditValue = string.IsNullOrEmpty(poObject.Estado) ? Diccionario.Seleccione : poObject.Estado;
                txtObservacionesSeguro.Text = poObject.ObservacionesSeguro;
                txtComentarios.Text = poObject.Comentarios;
                lIdProceso = poObject.IdProcesoCredito ?? 0;
                lblFecha.Text = poObject.Fecha.ToString("dd/MM/yyyy");
                txtVentaProyectada.EditValue = poObject.ProyeccionVentasAnuales;

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

                poEntidadAdj.ArchivoAdjunto = poObject.ArchivoAdjunto;
                poEntidadAdj.NombreOriginal = poObject.NombreOriginal;
                poEntidadAdj.RutaDestino = poObject.RutaDestino;

                txtAdjunto.Text = poObject.NombreOriginal;
                txtAdjunto.Tag = poObject.ArchivoAdjunto;
            }
        }

        private void lLimpiar()
        {
            txtNo.EditValue = "";
            txtCreditoSolicitado.EditValue = 0;
            txtCreditoAprobado.EditValue = 0;
            txtPLazoSolicitado.EditValue = 0;
            txtPlazoAprobado.EditValue = 0;
            cmbEstado.EditValue = Diccionario.Seleccione;
            txtObservacionesSeguro.Text = "";
            txtComentarios.Text = "";
            lIdProceso = 0;
            lblFecha.Text = "";
            txtVentaProyectada.EditValue = 0;

            txtAdjunto.Text = "";
            txtAdjunto.Tag = "";

            poEntidadAdj.ArchivoAdjunto = "";
            poEntidadAdj.NombreOriginal = "";
            poEntidadAdj.RutaDestino = "";
            poEntidadAdj.RutaOrigen = "";

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

        private void btnAdjuntar_Click(object sender, EventArgs e)
        {
            try
            {
                // Presenta un dialogo para seleccionar las imagenes
                OpenFileDialog ofdArchivo = new OpenFileDialog();
                ofdArchivo.Title = "Seleccione Archivo pdf";
                ofdArchivo.Filter = "Image Files( *.*; ) | *.*";

                if (ofdArchivo.ShowDialog() == DialogResult.OK)
                {


                    if (!ofdArchivo.FileName.Equals(""))
                    {
                        FileInfo file = new FileInfo(ofdArchivo.FileName);
                        var piSize = file.Length;

                        if (piSize <= clsPrincipal.gdcTamanoMb * 1048576)
                        {
                            string Name = file.Name + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff") + Path.GetExtension(ofdArchivo.FileName);
                            poEntidadAdj = new PlantillaSeguro();

                            poEntidadAdj.ArchivoAdjunto = Name;
                            poEntidadAdj.RutaOrigen = ofdArchivo.FileName;
                            poEntidadAdj.NombreOriginal = file.Name;
                            poEntidadAdj.RutaDestino = ConfigurationManager.AppSettings["CarpetaProCreRevPla"].ToString() + Name;


                            txtAdjunto.Text = file.Name;
                            txtAdjunto.Tag = Name;

                        }

                        else
                        {
                            XtraMessageBox.Show("El tamano máximo permitido es de: " + clsPrincipal.gdcTamanoMb + "mb", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarAdjunto_Click(object sender, EventArgs e)
        {
            try
            {
                poEntidadAdj = new PlantillaSeguro();
                txtAdjunto.Text = "";
                txtAdjunto.Tag = "";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(poEntidadAdj.ArchivoAdjunto))
                {
                    frmVerPdf pofrmVerPdf = new frmVerPdf();
                    //Muestra archivo local
                    if (!string.IsNullOrEmpty(poEntidadAdj.RutaOrigen))
                    {
                        pofrmVerPdf.lsRuta = poEntidadAdj.RutaOrigen;
                        if (pofrmVerPdf.lsRuta.ToLower().Contains(".pdf"))
                        {
                            pofrmVerPdf.Show();
                            pofrmVerPdf.SetDesktopLocation(0, 0);
                            pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;
                        }
                        else
                        {
                            Process.Start(pofrmVerPdf.lsRuta);
                        }
                    }
                    //Muestra archivo ya subido
                    else
                    {
                        pofrmVerPdf.lsRuta = poEntidadAdj.RutaDestino + poEntidadAdj.ArchivoAdjunto;
                        if (pofrmVerPdf.lsRuta.ToLower().Contains(".pdf"))
                        {
                            pofrmVerPdf.Show();
                            pofrmVerPdf.SetDesktopLocation(0, 0);
                            pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;
                        }
                        else
                        {
                            Process.Start(pofrmVerPdf.lsRuta);
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            try
            {
                frmEnvioCorreo frm = new frmEnvioCorreo();
                frm.tId = 0;
                frm.Cuerpo = "";
                frm.Show();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
