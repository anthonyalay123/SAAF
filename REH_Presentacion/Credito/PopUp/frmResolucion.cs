using CRE_Negocio.Transacciones;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
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

namespace REH_Presentacion.Credito.PopUp
{
    public partial class frmResolucion : frmBaseTrxDev
    {
        clsNProcesoCredito loLogicaNegocio = new clsNProcesoCredito();
        clsNPlantillaSeguro loLogicaNegocioSeg = new clsNPlantillaSeguro();
        PlantillaSeguro poEntidadPla = new PlantillaSeguro();
        ProcesoCredito poEntidadAdj = new ProcesoCredito();
        BindingSource bsArchivoAdjunto = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnAdd = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDownload = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnReferencia = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShowArchivo = new RepositoryItemButtonEdit();

        public bool lbRevision = false;
        public bool lbJefeAlmacen = false;
        public int lid = 0;
        public string lsTipo;
        public bool lbCerrado = false;
        public bool lbHabilitarFinalizado = false;

        public frmResolucion()
        {
            InitializeComponent();
            tstBotones.Visible = false;
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnAdd.ButtonClick += rpiBtnAdd_ButtonClick;
            rpiBtnShowArchivo.ButtonClick += rpiBtnShowArchivo_ButtonClick;
            rpiBtnDownload.ButtonClick += rpiBtnDownload_ButtonClick;
        }

        private void frmResolucion_Load(object sender, EventArgs e)
        {
            try
            {

                //lCargarEventosBotones();

                bsArchivoAdjunto.DataSource = new List<ProcesoCreditoResolucionAdjunto>();
                gcArchivoAdjunto.DataSource = bsArchivoAdjunto;

                dgvArchivoAdjunto.Columns["IdProcesoCreditoResolucionAdjunto"].Visible = false;
                dgvArchivoAdjunto.Columns["IdProcesoCredito"].Visible = false;
                dgvArchivoAdjunto.Columns["RutaDestino"].Visible = false;
                dgvArchivoAdjunto.Columns["RutaOrigen"].Visible = false;
                dgvArchivoAdjunto.Columns["ArchivoAdjunto"].Visible = false;
                dgvArchivoAdjunto.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
                dgvArchivoAdjunto.Columns["NombreOriginal"].Caption = "Archivo Adjunto";

                clsComun.gDibujarBotonGrid(rpiBtnDel, dgvArchivoAdjunto.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);
                clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvArchivoAdjunto.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16);
                clsComun.gDibujarBotonGrid(rpiBtnDownload, dgvArchivoAdjunto.Columns["Descargar"], "Descargar", Diccionario.ButtonGridImage.download_16x16);
                clsComun.gDibujarBotonGrid(rpiBtnShowArchivo, dgvArchivoAdjunto.Columns["Visualizar"], "Visualizar", Diccionario.ButtonGridImage.show_16x16);


                clsComun.gLLenarCombo(ref cmbTipoSolicitud, loLogicaNegocio.goConsultarComboTipoProcesoCredito(), true);
                clsComun.gLLenarCombo(ref cmbCliente, loLogicaNegocio.goSapConsultaClientesTodos(), true);
                clsComun.gLLenarCombo(ref cmbEstatusSeguro, loLogicaNegocio.goConsultarComboEstatusSeguro(), true);
                clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstadoPlanilla(), true);
                clsComun.gLLenarCombo(ref cmbEstadoRequerimiento, loLogicaNegocio.goConsultarComboEstado(), true, false, "...");

                if (lid != 0)
                {
                    txtNo.Text = lid.ToString();
                    lConsultar();
                }

                if (lsTipo != "ACT")
                {
                    if (clsPrincipal.gIdPerfil == 37 || clsPrincipal.gIdPerfil == 13)
                    {
                        btnAprobar.Enabled = false;
                    }
                }
                else
                {
                    if (lbRevision)
                    {
                        lbHabilitarFinalizado = true;
                    }
                }


                if (lbJefeAlmacen)
                {
                    if (!lbRevision)
                    {
                        btnAprobar.Visible = false;
                        btnAdjuntar.Visible = false;
                        btnComentarios.Visible = false;
                        btnEliminarAdjunto.Visible = false;
                        btnGuardar.Visible = false;
                        btnGuardarSinCambioDeEstado.Visible = false;
                        btnSalir.Visible = false;
                        btnEnviarDocumentos.Visible = false;
                        btnCorregir.Visible = false;
                        btnEnviarCorreo.Visible = false;
                        dgvArchivoAdjunto.Columns["Add"].Visible = false;
                        dgvArchivoAdjunto.Columns["Descripcion"].Visible = false;
                        dgvArchivoAdjunto.Columns["Del"].Visible = false;
                        btnAddFila.Visible = false;
                    }
                    //else
                    //{
                    //    btnGuardar.Enabled = false;
                    //}
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

                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvArchivoAdjunto.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<ProcesoCreditoResolucionAdjunto>)bsArchivoAdjunto.DataSource;


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
                                poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaProCreRes"].ToString() + Name;
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
        private void rpiBtnShowArchivo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvArchivoAdjunto.FocusedRowHandle;
                var poLista = (List<ProcesoCreditoResolucionAdjunto>)bsArchivoAdjunto.DataSource;

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

        /// <summary>
        /// Evento del boton de Descargar en el Grid, descarga el archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDownload_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {

                int piIndex = dgvArchivoAdjunto.FocusedRowHandle;
                var poLista = (List<ProcesoCreditoResolucionAdjunto>)bsArchivoAdjunto.DataSource;

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
                piIndex = dgvArchivoAdjunto.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<ProcesoCreditoResolucionAdjunto>)bsArchivoAdjunto.DataSource;

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
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #region Métodos
        /*
        private void lCargarEventosBotones()
        {
           
            bsCheckLisk.DataSource = new List<ProcesoCreditoDetalle>();
            gcCheckList.DataSource = bsCheckLisk;

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

            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvCheckList.Columns["Transaccion"], "Transacción", Diccionario.ButtonGridImage.show_16x16, 40);
            clsComun.gDibujarBotonGrid(rpiBtnRevision, dgvCheckList.Columns["Revision"], "Revisión", Diccionario.ButtonGridImage.show_16x16, 40);

            dgvCheckList.Columns["Completado"].Width = 40;
            dgvCheckList.Columns["FechaReferencial"].Width = 40;
            dgvCheckList.Columns["Estado"].Width = 40;

            dgvCheckList.Columns["Del"].Visible = false;
            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvCheckList.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);

            clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvCheckList.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16, 40);
            clsComun.gDibujarBotonGrid(rpiBtnView, dgvCheckList.Columns["Ver"], "Visualizar", Diccionario.ButtonGridImage.show_16x16, 40);
            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvCheckList.Columns["VerComentarios"], "Trazabilidad", Diccionario.ButtonGridImage.showhidecomment_16x16, 40);
            clsComun.gDibujarBotonGrid(rpiBtnCorregir, dgvCheckList.Columns["Corregir"], "Corregir", Diccionario.ButtonGridImage.cancel_16x16, 40);
            clsComun.gDibujarBotonGrid(rpiBtnAprobar, dgvCheckList.Columns["Aprobar"], "Aprobar", Diccionario.ButtonGridImage.apply_16x16, 40);
            clsComun.gDibujarBotonGrid(rpiBtnDesAprobar, dgvCheckList.Columns["DesAprobar"], "Desaprobar", Diccionario.ButtonGridImage.deletelist2_16x16, 40);

            dgvCheckList.Columns["RutaDestino"].Visible = false;
            dgvCheckList.Columns["RutaOrigen"].Visible = false;
            dgvCheckList.Columns["ArchivoAdjunto"].Visible = false;
            dgvCheckList.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvCheckList.Columns["NombreOriginal"].Caption = "Adjunto";

            dgvCheckList.Columns["Comentarios"].ColumnEdit = rpiMedDescripcion;
            dgvCheckList.Columns["Comentarios"].Width = 100;

        }
        */

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {

                var poObject = loLogicaNegocio.goConsultar(Convert.ToInt32(txtNo.Text.Trim()));

                txtNo.EditValue = poObject.IdProcesoCredito;
                lblFecha.Text = poObject.Fecha.ToString("dd/MM/yyyy");
                cmbTipoSolicitud.EditValue = poObject.CodigoTipoSolicitud;
                cmbEstatusSeguro.EditValue = poObject.CodigoEstatusSeguro;
                cmbCliente.EditValue = poObject.CodigoCliente;
                txtCupoSolicitado.EditValue = poObject.CupoSolicitado;
                txtPlazoSolicitado.EditValue = poObject.PlazoSolicitado;
                cmbEstadoRequerimiento.EditValue = poObject.CodigoEstado;

                if (poObject.CodigoEstado == Diccionario.Cerrado)
                {
                    btnGuardar.Enabled = false;
                    btnGuardarSinCambioDeEstado.Enabled = false;
                    btnAprobar.Enabled = false;
                    btnEliminarAdjunto.Enabled = false;
                    btnEnviarCorreo.Enabled = false;
                    btnEnviarDocumentos.Enabled = false;
                    btnCorregir.Enabled = false;
                    btnCorregir.Enabled = false;
                }
                else if (poObject.CodigoEstado == Diccionario.EnResolucion)
                {
                    //btnGuardar.Enabled = false;
                    //btnAprobar.Enabled = false;
                    //btnEliminarAdjunto.Enabled = false;
                    //btnEnviarCorreo.Enabled = false;
                    //btnEnviarDocumentos.Enabled = false;
                    //btnCorregir.Enabled = false;
                }
                else if (poObject.CodigoEstado == Diccionario.Finalizado)
                {
                    //btnGuardar.Enabled = false;
                    btnAprobar.Enabled = false;
                    btnEliminarAdjunto.Enabled = false;
                    //btnEnviarCorreo.Enabled = false;
                    btnEnviarDocumentos.Enabled = false;
                    btnCorregir.Enabled = false;
                }
                else
                {
                    //if (lbHabilitarFinalizado)
                    //{
                    //    btnEliminarAdjunto.Enabled = false;
                    //    btnEnviarDocumentos.Enabled = false;
                    //    btnCorregir.Enabled = false;
                    //}
                }

                poEntidadAdj.ArchivoAdjunto = poObject.ArchivoAdjunto;
                poEntidadAdj.NombreOriginal = poObject.NombreOriginal;
                poEntidadAdj.RutaDestino = poObject.RutaDestino;
                txtAdjunto.Text = poObject.NombreOriginal;
                txtAdjunto.Tag = poObject.ArchivoAdjunto;

                txtResolucion.EditValue = poObject.ResolucionAfecor;

                txtPlazoAprobado.EditValue = poObject.PlazoAprobado;
                txtCupoAprobado.EditValue = poObject.CupoAprobado;

                var pIdSol = loLogicaNegocioSeg.gIdPlantillaSeguro(Convert.ToInt32(txtNo.Text.Trim()));
                var poObjectSeg = loLogicaNegocioSeg.goConsultar(pIdSol);
                if (poObjectSeg != null)
                {
                    txtCupoSeguro.EditValue = poObjectSeg.CreditoAprobado;
                    txtObservacionesSeguro.Text = poObjectSeg.ObservacionesSeguro;
                    //txtPlazoSolicitado.EditValue = poObjectSeg.PlazoCreditoSolicitado;
                    cmbEstado.EditValue = poObjectSeg.Estado;

                    poEntidadPla.ArchivoAdjunto = poObjectSeg.ArchivoAdjunto;
                    poEntidadPla.NombreOriginal = poObjectSeg.NombreOriginal;
                    poEntidadPla.RutaDestino = poObjectSeg.RutaDestino;
                    txtAdjuntoSeguro.Text = poObjectSeg.NombreOriginal;
                    txtAdjuntoSeguro.Tag = poObjectSeg.ArchivoAdjunto;

                    txtPlazoAprobado.EditValue = poObject.PlazoAprobado == 0 ? poObjectSeg.PlazoAprobado : poObject.PlazoAprobado;
                    txtCupoAprobado.EditValue = poObject.CupoAprobado == 0 ? poObjectSeg.CreditoAprobado : poObject.CupoAprobado;
                }

                bsArchivoAdjunto.DataSource = poObject.ProcesoCreditoResolucionAdjunto;
                dgvArchivoAdjunto.RefreshData();

            }
            else
            {
                lLimpiar();
            }
        }

        private void lLimpiar()
        {

            txtNo.EditValue = "";
            lblFecha.Text = "";
            cmbTipoSolicitud.EditValue = Diccionario.Seleccione;
            cmbEstatusSeguro.EditValue = "SIN";
            cmbCliente.EditValue = Diccionario.Seleccione;
            txtCupoSolicitado.Text = "";

            cmbCliente.ReadOnly = false;
            cmbEstatusSeguro.ReadOnly = true;
            cmbTipoSolicitud.ReadOnly = false;

            poEntidadAdj.ArchivoAdjunto = "";
            poEntidadAdj.NombreOriginal = "";
            poEntidadAdj.RutaDestino = "";
            poEntidadAdj.RutaOrigen = "";

            poEntidadPla.ArchivoAdjunto = "";
            poEntidadPla.NombreOriginal = "";
            poEntidadPla.RutaDestino = "";
            poEntidadPla.RutaOrigen = "";

            txtAdjunto.Text = "";
            txtAdjunto.Tag = "";

            txtAdjuntoSeguro.Text = "";
            txtAdjuntoSeguro.Tag = "";

            bsArchivoAdjunto.DataSource = new List<ProcesoCreditoDetalleRevision>();
            gcArchivoAdjunto.DataSource = bsArchivoAdjunto;

        }
        #endregion

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            lGuardar(Diccionario.EnResolucion);
        }

        private void lGuardar(string tsEstado, bool tbMostrarMensaje = true)
        {
            try
            {

                ProcesoCredito poObject = new ProcesoCredito();
                poObject.IdProcesoCredito = !string.IsNullOrEmpty(txtNo.Text) ? int.Parse(txtNo.Text) : 0;
                if (!string.IsNullOrEmpty(txtCupoAprobado.Text))
                {
                    poObject.CupoAprobado = Convert.ToDecimal(txtCupoAprobado.EditValue.ToString());
                }
                else
                {
                    poObject.CupoAprobado = null;
                }

                if (!string.IsNullOrEmpty(txtPlazoAprobado.Text))
                {
                    poObject.PlazoAprobado = Convert.ToInt32(txtPlazoAprobado.EditValue.ToString());
                }
                else
                {
                    poObject.PlazoAprobado = null;
                }
                poObject.CodigoEstatusSeguro = cmbEstatusSeguro.EditValue.ToString();
                poObject.ResolucionAfecor = txtResolucion.Text;
                poObject.CodigoEstado = tsEstado;

                poObject.ArchivoAdjunto = poEntidadAdj.ArchivoAdjunto;
                poObject.NombreOriginal = poEntidadAdj.NombreOriginal;
                poObject.RutaOrigen = poEntidadAdj.RutaOrigen;
                poObject.RutaDestino = poEntidadAdj.RutaDestino;

                poObject.ProcesoCreditoResolucionAdjunto = (List<ProcesoCreditoResolucionAdjunto>)bsArchivoAdjunto.DataSource;

                var poListaAdd = new List<ProcesoCreditoDetalleRevision>();

                if (tbMostrarMensaje)
                {
                    if ((cmbEstatusSeguro.EditValue.ToString() == "INN" || cmbEstatusSeguro.EditValue.ToString() == "RAF") && tsEstado != Diccionario.EnResolucion)
                    {
                        DialogResult dialogResult2 = XtraMessageBox.Show("¿Desea enviar documentos obligatorios a Jefe de Alamacén?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult2 == DialogResult.Yes)
                        {
                            frmCheckList frm = new frmCheckList();
                            frm.tId = int.Parse(txtNo.Text);
                            frm.ShowDialog();

                            if (frm.pbAcepto)
                            {
                                poListaAdd = frm.ComisionDetalle;
                            }
                        }
                    }
                }

                DialogResult dialogResult3 = XtraMessageBox.Show("¿Está seguro de guardar cambios?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult3 == DialogResult.Yes)
                {
                    string psMsg = loLogicaNegocio.gsGuardarResolucionCredito(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, poListaAdd);

                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
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
            }
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
                            poEntidadAdj = new ProcesoCredito();

                            poEntidadAdj.ArchivoAdjunto = Name;
                            poEntidadAdj.RutaOrigen = ofdArchivo.FileName;
                            poEntidadAdj.NombreOriginal = file.Name;
                            poEntidadAdj.RutaDestino = ConfigurationManager.AppSettings["CarpetaProCre"].ToString() + Name;

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
                poEntidadAdj = new ProcesoCredito();
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

        private void btnVerAdjuntoSeguro_Click(object sender, EventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(poEntidadPla.ArchivoAdjunto))
                {
                    frmVerPdf pofrmVerPdf = new frmVerPdf();
                    //Muestra archivo local
                    if (!string.IsNullOrEmpty(poEntidadPla.RutaOrigen))
                    {
                        pofrmVerPdf.lsRuta = poEntidadPla.RutaOrigen;

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
                        pofrmVerPdf.lsRuta = poEntidadPla.RutaDestino + poEntidadPla.ArchivoAdjunto;
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

        private void btnAprobar_Click(object sender, EventArgs e)
        {
            lGuardar(Diccionario.Finalizado);
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

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                bsArchivoAdjunto.AddNew();
                dgvArchivoAdjunto.Focus();
                dgvArchivoAdjunto.ShowEditor();
                dgvArchivoAdjunto.UpdateCurrentRow();
                dgvArchivoAdjunto.RefreshData();
                dgvArchivoAdjunto.FocusedColumn = dgvArchivoAdjunto.VisibleColumns[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (!lbJefeAlmacen || lbRevision)
                {
                    if (xtraTabControl1.SelectedTabPageIndex == 1)
                    {
                        btnAddFila.Visible = true;
                    }
                    else
                    {
                        btnAddFila.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEnviarDocumentos_Click(object sender, EventArgs e)
        {
            try
            {
                var poListaAdd = new List<ProcesoCreditoDetalleRevision>();

                DialogResult dialogResult2 = XtraMessageBox.Show("¿Desea enviar documentos obligatorios a Jefe de Alamacén?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult2 == DialogResult.Yes)
                {
                    frmCheckList frm = new frmCheckList();
                    frm.tId = int.Parse(txtNo.Text);
                    frm.ShowDialog();

                    if (frm.pbAcepto)
                    {
                        var result = XtraInputBox.Show("Ingrese comentario de resolución", "Documentos a enviar", "");
                        if (string.IsNullOrEmpty(result))
                        {
                            XtraMessageBox.Show("Debe agregar comentario para continuar", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        poListaAdd = frm.ComisionDetalle;

                        string psMsg = loLogicaNegocio.gsEnviarDocumentosAReq(clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, poListaAdd, result);

                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show("Documentos Enviador al Requerimiento!", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Close();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        Close();
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
                frm.tId = int.Parse(txtNo.Text);
                frm.Cuerpo = txtResolucion.Text;
                frm.Show();
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

                        var psMess = loLogicaNegocio.gsActualzarRequerimeintoCorregir(int.Parse(txtNo.Text), result, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, false);

                        if (psMess != "")
                        {
                            XtraMessageBox.Show(psMess, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Close();
                            //lConsultar();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnGuardarSinCambioDeEstado_Click(object sender, EventArgs e)
        {
            lGuardar(cmbEstadoRequerimiento.EditValue.ToString(), false);
        }
    }
}
