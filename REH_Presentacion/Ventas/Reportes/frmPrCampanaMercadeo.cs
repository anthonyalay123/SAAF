using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
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
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VTA_Negocio;

namespace REH_Presentacion.Ventas.Reportes
{
    public partial class frmPrCampanaMercadeo : frmBaseTrxDev
    {
        clsNCampañaMercadeo loLogicaNegocio = new clsNCampañaMercadeo();
        BindingSource bsDatos = new BindingSource();
        BindingSource bsCuerpo = new BindingSource();
        BindingSource bsArchivoAdjunto = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDelCue = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnAdd = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDownload = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShowArchivo = new RepositoryItemButtonEdit();

        public frmPrCampanaMercadeo()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnDelCue.ButtonClick += rpiBtnDelCue_ButtonClick;
            rpiBtnAdd.ButtonClick += rpiBtnAdd_ButtonClick;
            rpiBtnShowArchivo.ButtonClick += rpiBtnShowArchivo_ButtonClick;
            rpiBtnDownload.ButtonClick += rpiBtnDownload_ButtonClick;

        }

        private void frmPrCampanaMercadeo_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();               
                clsComun.gLLenarComboGrid(ref dgvCuerpo, loLogicaNegocio.goComboTipoEtiqueta(), "CodigoTipo");

                bsDatos.AddNew();
                dgvDatos.Focus();
                dgvDatos.ShowEditor();
                dgvDatos.UpdateCurrentRow();
                var poLista = (List<CampanaPublicitariaDestinatario>)bsDatos.DataSource;
                dgvDatos.RefreshData();
                
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
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    var poLista = loLogicaNegocio.goListar();
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[]
                                        {
                                    new DataColumn("Código"),
                                    new DataColumn("Descripción"),
                                    new DataColumn("Asunto")
                                        });

                    poLista.ForEach(a =>
                    {
                        DataRow row = dt.NewRow();
                        row["Código"] = a.IdCampanaPublicitaria;
                        row["Descripción"] = a.Descripcion;
                        row["Asunto"] = a.Asunto;

                        dt.Rows.Add(row);
                    });

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                    if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                    {

                        txtCodigo.Text = pofrmBuscar.lsCodigoSeleccionado;
                        lConsultar();
                    }

                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ConstruirHtml()
        {
            string html = "";
            html = string.Format("{0}<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">\n", html);
            html = string.Format("{0}<html>\n", html);
            html = string.Format("{0}<head>\n", html);
            html = string.Format("{0}<title></title>\n", html);
            html = string.Format("{0}</head>\n", html);
            html = string.Format("{0}<body>\n", html);
            html = string.Format("{0}<p>{1}</p>\n", html,txtCuerpoCorreo.Text.Replace("\n","<br/>"));

            foreach (var item in (List<CampanaPublicitariaAdjunto>)bsArchivoAdjunto.DataSource)
            {
                html = string.Format("{0}<div align=\"center\">\n", html);
                html = string.Format("{0}<img src=\"https://www.afecor.com/Ventas/{1}\"width =75% height =75%/>\n", html, item.ArchivoAdjunto);
                html = string.Format("{0}</div>\n", html);
            }
            
            html = string.Format("{0}</body>", html);
            html = string.Format("{0}</html>\n", html);
            return html;
        }

        /// <summary>
        /// Evento del botón Exportar, Exporta a Xml.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            try
            {
                btnGrabar_Click(null, null);
                dgvDatos.PostEditor();
                var poDestinatarios = (List<CampanaPublicitariaDestinatario>)bsDatos.DataSource;
                int piCantCorreos = 0;
                if (chbEnviarEnviado.Checked)
                {
                    piCantCorreos = poDestinatarios.Count();
                }
                else
                {
                    piCantCorreos = poDestinatarios.Where(x=>!x.Enviado).Count();
                }
                
                if (piCantCorreos > 0)
                {

                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de enviar correos? \n\n" + string.Format("Se enviará(n) {0} correo(s)", piCantCorreos), "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        
                        int cont = 0;
                        Cursor.Current = Cursors.WaitCursor;
                        var poFil = chbEnviarEnviado.Checked ? poDestinatarios.ToList() : poDestinatarios.Where(x => !x.Enviado).ToList();
                        foreach (var item in poFil)
                        {
                            cont++;

                            List<Attachment> listAdjuntosEmail = new List<Attachment>();
                            string psAsunto = txtAsunto.Text;
                            string psDestinatario = item.EmailDestinatario;
                            string psEmailCC = string.IsNullOrEmpty(item.EmailCC) ? "" : item.EmailCC;
                            //string psNombrePresentar = txtNombrePresentar.Text;
                            string psMail = ConstruirHtml();
                            string psRuta = "";


                            if (File.Exists(psRuta))
                                listAdjuntosEmail.Add(new Attachment(psRuta));

                            try
                            {
                                var msg = loLogicaNegocio.EnviarPorCorreo(psDestinatario, psAsunto, psMail, listAdjuntosEmail, false, psEmailCC, "", true);
                                item.Enviado = true;
                                loLogicaNegocio.gActualizaEnviado(item.IdCampanaPublicitariaDestinatario);
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                        Cursor.Current = Cursors.Default;
                        XtraMessageBox.Show("Mensaje(s) enviado(s)", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgvArchivoAdjunto.RefreshData();
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existen correos destinatarios a enviar o están ya enviados!", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                {
                    bsDatos.AddNew();
                    //cmbProveedor.ReadOnly = true;
                    dgvDatos.Focus();
                    dgvDatos.ShowEditor();
                    dgvDatos.UpdateCurrentRow();
                    var poLista = (List<CampanaPublicitariaDestinatario>)bsDatos.DataSource;
                    dgvDatos.RefreshData();
                    dgvDatos.FocusedColumn = dgvDatos.VisibleColumns[0];
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
                dgvArchivoAdjunto.PostEditor();

                CampanaPublicitaria PoObject = new CampanaPublicitaria();
                PoObject.Observacion = txtObservacion.Text;
                PoObject.IdCampanaPublicitaria = !string.IsNullOrEmpty(txtCodigo.Text) ? int.Parse(txtCodigo.Text) : 0;
                PoObject.Asunto = txtAsunto.Text;
                PoObject.CuerpoDelEmail = txtCuerpoCorreo.Text;
                PoObject.Descripcion = txtDescripcion.Text;
                PoObject.NombrePresentar = "";
                //PoObject.CuerpoDelEmail = txtCuerpoCorreo.Text;

                var sdv = txtTexto.Text;
                var sfe = txtCuerpoCorreo.Text;

                PoObject.CampanaPublicitariaAdjunto = (List<CampanaPublicitariaAdjunto>)bsArchivoAdjunto.DataSource;
                PoObject.CampanaPublicitariaCuerpo = (List<CampanaPublicitariaCuerpo>)bsCuerpo.DataSource;
                PoObject.CampanaPublicitariaDestinatario = (List<CampanaPublicitariaDestinatario>)bsDatos.DataSource;

                int pId = 0;

                if (sender != null)
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        
                        string psMsg = loLogicaNegocio.gsGuardar(PoObject, clsPrincipal.gsUsuario, out pId);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtCodigo.Text = pId.ToString();
                            lConsultar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    string psMsg = loLogicaNegocio.gsGuardar(PoObject, clsPrincipal.gsUsuario, out pId);
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        txtCodigo.Text = pId.ToString();
                        lConsultar();
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

        /// <summary>
        /// Evento del boton de eliminar en el Grid, elimina la fila seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<CampanaPublicitariaDestinatario>)bsDatos.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {

                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source

                        if (poLista.Count == 0)
                        {
                            //cmbProveedor.ReadOnly = false;
                        }

                        bsDatos.DataSource = poLista;
                        dgvDatos.RefreshData();

                    }
                    
                }
                if (xtraTabControl1.SelectedTabPageIndex == 1)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvArchivoAdjunto.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<CampanaPublicitariaAdjunto>)bsArchivoAdjunto.DataSource;

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
        private void rpiBtnDelCue_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvCuerpo.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<CampanaPublicitariaCuerpo>)bsCuerpo.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {

                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source

                    if (poLista.Count == 0)
                    {
                        //cmbProveedor.ReadOnly = false;
                    }

                    bsCuerpo.DataSource = poLista;
                    dgvCuerpo.RefreshData();

                    //lPreliminar();
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
                var poLista = (List<CampanaPublicitariaAdjunto>)bsArchivoAdjunto.DataSource;


                // Presenta un dialogo para seleccionar las imagenes
                OpenFileDialog ofdArchicoAdjunto = new OpenFileDialog();
                ofdArchicoAdjunto.Title = "Seleccione Archivo";
                ofdArchicoAdjunto.Filter = "Image Files( *.jpg;*.jpeg;*.png; ) | *.jpg;*.jpeg;*.png;";

                if (ofdArchicoAdjunto.ShowDialog() == DialogResult.OK)
                {
                    if (poLista.Count > 0 && piIndex >= 0)
                    {

                        if (!ofdArchicoAdjunto.FileName.Equals(""))
                        {
                            FileInfo file = new FileInfo(ofdArchicoAdjunto.FileName);
                            var piSize = file.Length;

                            if (piSize <= int.Parse(ConfigurationManager.AppSettings["TamanoKbFtp"].ToString()) * 1024)
                            {
                                string Name = file.Name + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff") + Path.GetExtension(ofdArchicoAdjunto.FileName);
                                var poEntidad = poLista[piIndex];

                                poLista[piIndex].ArchivoAdjunto = Name;
                                poLista[piIndex].RutaOrigen = ofdArchicoAdjunto.FileName;
                                poLista[piIndex].NombreOriginal = file.Name;
                                poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaVtaCamPu"].ToString() + Name;
                                // Asigno mi nueva lista al Binding Source
                                bsArchivoAdjunto.DataSource = poLista;
                                dgvArchivoAdjunto.RefreshData();
                            }

                            else
                            {
                                XtraMessageBox.Show("El tamano máximo permitido es de: " + ConfigurationManager.AppSettings["TamanoKbFtp"].ToString() + "kb", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEnviarCorreo"] != null) tstBotones.Items["btnEnviarCorreo"].Click += btnEnviarCorreo_Click;

            if (tstBotones.Items["btnEnviarCorreo"] != null) tstBotones.Items["btnEnviarCorreo"].Enabled = false;

            bsDatos.DataSource = new List<CampanaPublicitariaDestinatario>();
            gcDatos.DataSource = bsDatos;
            
            bsArchivoAdjunto.DataSource = new List<CampanaPublicitariaAdjunto>();
            gcArchivoAdjunto.DataSource = bsArchivoAdjunto;

            bsCuerpo.DataSource = new List<CampanaPublicitariaCuerpo>();
            gcCuerpo.DataSource = bsCuerpo;

            dgvDatos.Columns["IdCampanaPublicitariaDestinatario"].Visible = false;
            dgvDatos.Columns["IdCampanaPublicitaria"].Visible = false;
            dgvDatos.Columns["Enviado"].OptionsColumn.ReadOnly = true;

            dgvArchivoAdjunto.Columns["IdCampanaPublicitariaAdjunto"].Visible = false;
            dgvArchivoAdjunto.Columns["IdCampanaPublicitaria"].Visible = false;
            dgvArchivoAdjunto.Columns["RutaDestino"].Visible = false;
            dgvArchivoAdjunto.Columns["RutaOrigen"].Visible = false;
            dgvArchivoAdjunto.Columns["ArchivoAdjunto"].Visible = false;
            dgvArchivoAdjunto.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvArchivoAdjunto.Columns["NombreOriginal"].Caption = "Archivo Adjunto";
            dgvArchivoAdjunto.Columns["Ruta"].Visible = false;

            dgvCuerpo.Columns["IdCampanaPublicitariaCuerpo"].Visible = false;
            dgvCuerpo.Columns["IdCampanaPublicitaria"].Visible = false;
            dgvCuerpo.Columns["CodigoTipo"].Caption = "Tipo";
            dgvCuerpo.Columns["Tipo"].Visible = false;
            dgvCuerpo.Columns["CodigoTipo"].Width = 20;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 40);

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvArchivoAdjunto.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvArchivoAdjunto.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnDownload, dgvArchivoAdjunto.Columns["Descargar"], "Descargar", Diccionario.ButtonGridImage.download_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowArchivo, dgvArchivoAdjunto.Columns["Visualizar"], "Visualizar", Diccionario.ButtonGridImage.show_16x16);

            clsComun.gDibujarBotonGrid(rpiBtnDelCue, dgvCuerpo.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 10);

        }

        private void lLimpiar()
        {
            txtCodigo.Text = "";
            txtObservacion.Text = "";
            txtAsunto.EditValue = "";
            txtDescripcion.EditValue = "";
            //txtNombrePresentar.EditValue = "";
            txtTexto.EditValue = "";
            txtCuerpoCorreo.Text = "";

            bsCuerpo.DataSource = new List<CampanaPublicitariaCuerpo>();
            dgvCuerpo.RefreshData();

            bsDatos.DataSource = new List<CampanaPublicitariaDestinatario>();
            dgvDatos.RefreshData();

            bsArchivoAdjunto.DataSource = new List<CampanaPublicitariaAdjunto>();
            dgvArchivoAdjunto.RefreshData();

            if (tstBotones.Items["btnEnviarCorreo"] != null) tstBotones.Items["btnEnviarCorreo"].Enabled = false;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
            chbEnviarEnviado.Checked = false;
            chbEnviarEnviado.Visible = false;

            bsDatos.AddNew();
            dgvDatos.Focus();
            dgvDatos.ShowEditor();
            dgvDatos.UpdateCurrentRow();
            var poLista = (List<CampanaPublicitariaDestinatario>)bsDatos.DataSource;
            dgvDatos.RefreshData();

        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultar(Convert.ToInt32(txtCodigo.Text.Trim()));
                txtObservacion.Text = poObject.Observacion;
                txtAsunto.EditValue = poObject.Asunto;
                txtDescripcion.EditValue = poObject.Descripcion;
                //txtNombrePresentar.EditValue = poObject.NombrePresentar;
               txtCuerpoCorreo.Text = poObject.CuerpoDelEmail;
                
                bsDatos.DataSource = poObject.CampanaPublicitariaDestinatario;
                dgvDatos.RefreshData();

                bsArchivoAdjunto.DataSource = poObject.CampanaPublicitariaAdjunto;
                dgvArchivoAdjunto.RefreshData();

                bsCuerpo.DataSource = poObject.CampanaPublicitariaCuerpo;
                dgvCuerpo.RefreshData();

                if (tstBotones.Items["btnEnviarCorreo"] != null) tstBotones.Items["btnEnviarCorreo"].Enabled = true;

                /*
                if (tbEnvioCorreo)
                {
                    txtObservacion.ReadOnly = true;
                    txtAsunto.ReadOnly = true;
                    txtDescripcion.ReadOnly = true;
                    txtCuerpoCorreo.ReadOnly = true;
                    //txtNombrePresentar.ReadOnly = true;

                    dgvDatos.Columns["EmailDestinatario"].OptionsColumn.ReadOnly = true;
                    dgvDatos.Columns["EmailCC"].OptionsColumn.ReadOnly = true;
                    dgvDatos.Columns["Del"].OptionsColumn.ReadOnly = true;

                    dgvArchivoAdjunto.Columns["Ruta"].OptionsColumn.ReadOnly = true;
                    dgvArchivoAdjunto.Columns["Del"].OptionsColumn.ReadOnly = true;

                    dgvDatos.Columns["Del"].Visible = false;
                    dgvArchivoAdjunto.Columns["Del"].Visible = false;
                    dgvArchivoAdjunto.Columns["Add"].Visible = false;
                    
                    if (tstBotones.Items["btnEnviarCorreo"] != null) tstBotones.Items["btnEnviarCorreo"].Enabled = true;
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;

                    btnAddFila.Enabled = false;
                    btnPlantilla.Enabled = false;
                    btnImportar.Enabled = false;
                    chbEnviarEnviado.Checked = false;
                    chbEnviarEnviado.Visible = true;
                }
                else
                {
                    txtObservacion.ReadOnly = false;
                    txtAsunto.ReadOnly = false;
                    txtDescripcion.ReadOnly = false;
                    txtCuerpoCorreo.ReadOnly = false;
                    //txtNombrePresentar.ReadOnly = false;

                    dgvDatos.Columns["EmailDestinatario"].OptionsColumn.ReadOnly = false;
                    dgvDatos.Columns["EmailCC"].OptionsColumn.ReadOnly = false;
                    dgvDatos.Columns["Del"].OptionsColumn.ReadOnly = false;

                    dgvArchivoAdjunto.Columns["Ruta"].OptionsColumn.ReadOnly = false;
                    dgvArchivoAdjunto.Columns["Del"].OptionsColumn.ReadOnly = false;

                    dgvDatos.Columns["Del"].Visible = true;
                    dgvArchivoAdjunto.Columns["Del"].Visible = true;
                    dgvArchivoAdjunto.Columns["Add"].Visible = true;


                    if (tstBotones.Items["btnEnviarCorreo"] != null) tstBotones.Items["btnEnviarCorreo"].Enabled = false;
                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;

                    btnAddFila.Enabled = true;
                    btnPlantilla.Enabled = true;
                    btnImportar.Enabled = true;
                    chbEnviarEnviado.Visible = false;
                }
                */
            }
        }

        private void btnAddFilaCuerpo_Click(object sender, EventArgs e)
        {
            try
            {
                bsCuerpo.AddNew();
                //cmbProveedor.ReadOnly = true;
                dgvCuerpo.Focus();
                dgvCuerpo.ShowEditor();
                dgvCuerpo.UpdateCurrentRow();
                var poLista = (List<CampanaPublicitariaCuerpo>)bsCuerpo.DataSource;
                poLista.LastOrDefault().CodigoTipo = "NI";
                dgvCuerpo.RefreshData();
                dgvCuerpo.FocusedColumn = dgvCuerpo.VisibleColumns[0];

                //lPreliminar();
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
        private void rpiBtnShowArchivo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvArchivoAdjunto.FocusedRowHandle;
                var poLista = (List<CampanaPublicitariaAdjunto>)bsArchivoAdjunto.DataSource;

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
                var poLista = (List<CampanaPublicitariaAdjunto>)bsArchivoAdjunto.DataSource;

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

        private void btnPreliminar_Click(object sender, EventArgs e)
        {
            lPreliminar();
        }

        private void lPreliminar()
        {
            try
            {
                txtTexto.Text = "";
                var poLista = (List<CampanaPublicitariaCuerpo>)bsCuerpo.DataSource;
                foreach (var item in poLista)
                {
                    if (item.CodigoTipo == "NI")
                    {
                        txtTexto.Text = string.Format("{0}{1}{2}", txtTexto.Text, "", item.Descripcion);
                    }
                    else if (item.CodigoTipo == "SL")
                    {
                        txtTexto.Text = string.Format("{0}{1}{2}", txtTexto.Text, "\r\n", item.Descripcion);
                    }
                    else if (item.CodigoTipo == "NE")
                    {
                        txtTexto.Text = string.Format("{0}{1}{2}", txtTexto.Text, "", item.Descripcion);
                    }
                    else if (item.CodigoTipo == "ES")
                    {
                        txtTexto.Text = string.Format("{0}{1}{2}", txtTexto.Text, " ", item.Descripcion);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCuerpo_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            lPreliminar();
        }

        private void btnPlantilla_Click(object sender, EventArgs e)
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
                bs.DataSource = new List<CampanaPublicitariaDestinatarioPlanilla>();
                dgv.BestFitColumns();
                dgv.OptionsView.BestFitMode = GridBestFitMode.Full;
                // Exportar Datos
                clsComun.gSaveFile(gc, "Plantilla_Correos_Destinatarios.xlsx", "Files(*.xlsx;)|*.xlsx;");

                gc.Visible = false;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int cont = 0;
            try
            {
                OpenFileDialog ofdRuta = new OpenFileDialog();
                ofdRuta.Title = "Seleccione Archivo";
                //(*.jpg; *.jpeg; *.png)| *.jpg; *.jpeg; *.png; "
                ofdRuta.Filter = "Files(*.xls; *.xlsx;)|*.xls; *.xlsx;";

                //clsComun.gsMensajePrevioImportar();

                if (ofdRuta.ShowDialog() == DialogResult.OK)
                {
                    if (!ofdRuta.FileName.Equals(""))
                    {
                        DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);
                        var poLista = (List<CampanaPublicitariaDestinatario>)bsDatos.DataSource;
                        var poListaImportada = new List<CampanaPublicitariaDestinatario>();

                        List<string> psListaMsg = new List<string>();

                        int fila = 2;
                        string psMsgLista = string.Empty;
                        foreach (DataRow item in dt.Rows)
                        {
                            if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                            {
                                string psMsgFila = "";
                                string psMsgOut = "";

    
                                CampanaPublicitariaDestinatario poItem = new CampanaPublicitariaDestinatario();
                                poItem.EmailDestinatario = clsComun.gdValidarRegistro("Correo Destinatario", "s", item[0].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.EmailCC = clsComun.gdValidarRegistro("Con Copia", "s", item[1].ToString().Trim(), fila, false, ref psMsgOut);

                                fila++;

                                if (string.IsNullOrEmpty(psMsgOut))
                                {
                                    poListaImportada.Add(poItem);
                                }
                                else
                                {
                                    psMsgLista = psMsgLista + psMsgOut;
                                }

                            }
                        }
                        if (!string.IsNullOrEmpty(psMsgLista))
                        {
                            psListaMsg.Add(psMsgLista);
                        }

                        if (psListaMsg.Count > 0)
                        {
                            XtraMessageBox.Show("Se emitirá un archivo de errores", "No es posible importar datos!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clsComun.gsGuardaLogTxt(psListaMsg);
                            return;
                        }


                        string psMsg = lsValidaDuplicados(poLista, poListaImportada);
                        if (!string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(psMsg, "No es posible importar! existen datos duplicados!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        poLista.AddRange(poListaImportada);

                        bsDatos.DataSource = poLista;
                        dgvDatos.BestFitColumns();
                        XtraMessageBox.Show("Importado Exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
        }

        private string lsValidaDuplicados(List<CampanaPublicitariaDestinatario> toListaGrid, List<CampanaPublicitariaDestinatario> toListaInsertar)
        {
            string psMsg = string.Empty;

            foreach (var item in toListaInsertar)
            {
                var piRegistro = toListaGrid.Where(x => x.EmailDestinatario == item.EmailDestinatario).Count();
                if (piRegistro > 0)
                {
                    psMsg = string.Format("{0}Correo Destinatario: {1} ya está parametrizado. \n", psMsg, item.EmailDestinatario);
                }
            }

            return psMsg;
        }

    }
}
