using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using REH_Negocio;
using REH_Presentacion.Comun;
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

namespace REH_Presentacion.TalentoHumano.Parametrizadores
{
    public partial class frmPrFichaMedica : frmBaseTrxDev
    {
        clsNEmpleado loLogicaNegocio = new clsNEmpleado();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDownload = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShowArchivo = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnAdd = new RepositoryItemButtonEdit();
        BindingSource bsArchivoAdjunto = new BindingSource();
        public int lIdPersona = 0;
        bool pbCargado = false;

        public frmPrFichaMedica()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnShowArchivo.ButtonClick += rpiBtnShowArchivo_ButtonClick;
            rpiBtnAdd.ButtonClick += rpiBtnAdd_ButtonClick;
            rpiBtnDownload.ButtonClick += rpiBtnDownload_ButtonClick;
        }

        private void frmPrFichaEmpleado_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                clsComun.gLLenarCombo(ref cmbTipoSangre, loLogicaNegocio.goConsultarComboTipoSangre(), true);
                clsComun.gLLenarCombo(ref cmbTipo, loLogicaNegocio.goConsultarComboTipoFichaMedica(), true);
                clsComun.gLLenarCombo(ref cmbPersona, loLogicaNegocio.goConsultarComboIdPersona(), true);
                clsComun.gLLenarCombo(ref cmbEstadoIMC, loLogicaNegocio.goConsultarComboEstadoIMC(), true);
                dtpPeriodo.DateTime = DateTime.Now;
                pbCargado = true;
                if (lIdPersona != 0)
                {
                    cmbPersona.EditValue = lIdPersona.ToString();
                    cmbPersona.ReadOnly = true;
                }
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
        /// Evento del botón Grabar, Guarda datos del formulario a el objeto de BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvArchivoAdjunto.PostEditor();

                PersonaFichaMedica poObject = new PersonaFichaMedica();
                poObject.IdPersonaFichaMedica = string.IsNullOrEmpty(txtNo.Text) ? 0 : int.Parse(txtNo.Text);
                poObject.IdPersona = int.Parse(cmbPersona.EditValue.ToString());
                poObject.CodigoTipo = cmbTipo.EditValue.ToString();
                poObject.CodigoEstadoIMC = cmbEstadoIMC.EditValue.ToString();
                poObject.Peso = decimal.Parse(txtPeso.EditValue.ToString());
                poObject.CodigoTipoSangre = cmbTipoSangre.EditValue.ToString();
                poObject.Estatura = decimal.Parse(txtEstatura.EditValue.ToString());
                poObject.IMC = decimal.Parse(txtIMC.EditValue.ToString());
                poObject.ParametroAbdominal = decimal.Parse(txtParametroAbdominal.EditValue.ToString());
                poObject.PresionArterial = txtPresionArterial.Text;
                poObject.FrecuenciaCardiaca = decimal.Parse(txtFrecuenciaCardiaca.EditValue.ToString());
                poObject.Temperatura = decimal.Parse(txtTemperatura.EditValue.ToString());
                poObject.Saturacion = decimal.Parse(txtSaturacion.EditValue.ToString());
                poObject.AtencionPrioritaria = chbAtencionPrioritaria.Checked;
                poObject.Diagnostico = txtDiagnostico.Text;
                poObject.AntecedentesFamiliares = txtAntecedentesFamiliares.Text;
                poObject.Alergias = txtAlergias.Text;
                poObject.MedicacionContinua = txtMedicacionContinua.Text;
                poObject.Periodo = dtpPeriodo.DateTime;
                poObject.Observaciones = txtObservaciones.Text;
                poObject.CIE10 = txtCIE10.Text;
                poObject.AntecedentesQuirurgicos = txtAntecedentesQuirurgicos.Text;
                poObject.EnfermedadesPreexistentes = "";
                poObject.Piel = "";
                poObject.Ojos = "";

                poObject.PersonaFichaMedicaAdjunto = (List<PersonaFichaMedicaAdjunto>)bsArchivoAdjunto.DataSource;

                string psMsg = loLogicaNegocio.gsGuardarFichaMedica(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                if (string.IsNullOrEmpty(psMsg))
                {
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lLimpiar();
                }
                else
                {
                    XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               
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
                lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        var psMsg = loLogicaNegocio.gsEliminarFichaEmpleado(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var poLista = (List<PersonaFichaMedicaAdjunto>)bsArchivoAdjunto.DataSource;


                // Presenta un dialogo para seleccionar las imagenes
                OpenFileDialog ofdArchicoAdjunto = new OpenFileDialog();
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
                                poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaFichaMedica"].ToString() + Name;
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
                var poLista = (List<PersonaFichaMedicaAdjunto>)bsArchivoAdjunto.DataSource;

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
                var poLista = (List<PersonaFichaMedicaAdjunto>)bsArchivoAdjunto.DataSource;

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
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var poLista = (List<PersonaFichaMedicaAdjunto>)bsArchivoAdjunto.DataSource;

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
        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;

            bsArchivoAdjunto.DataSource = new List<PersonaFichaMedicaAdjunto>();
            gcArchivoAdjunto.DataSource = bsArchivoAdjunto;

            dgvArchivoAdjunto.Columns["IdPersonaFichaMedicaAdjunto"].Visible = false;
            dgvArchivoAdjunto.Columns["IdPersonaFichaMedica"].Visible = false;
            dgvArchivoAdjunto.Columns["RutaDestino"].Visible = false;
            dgvArchivoAdjunto.Columns["RutaOrigen"].Visible = false;
            dgvArchivoAdjunto.Columns["ArchivoAdjunto"].Visible = false;
            dgvArchivoAdjunto.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvArchivoAdjunto.Columns["NombreOriginal"].Caption = "Archivo Adjunto";

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvArchivoAdjunto.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);
            clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvArchivoAdjunto.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnDownload, dgvArchivoAdjunto.Columns["Descargar"], "Descargar", Diccionario.ButtonGridImage.download_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowArchivo, dgvArchivoAdjunto.Columns["Visualizar"], "Visualizar", Diccionario.ButtonGridImage.show_16x16);

        }


        private void lLimpiar(bool tbLimpiarPersona = true)
        {
            if (tbLimpiarPersona)
            {
                if ((cmbPersona.Properties.DataSource as IList).Count > 0) cmbPersona.ItemIndex = 0;
            }
            
            if ((cmbTipo.Properties.DataSource as IList).Count > 0) cmbTipo.ItemIndex = 0;
            if ((cmbTipoSangre.Properties.DataSource as IList).Count > 0) cmbTipoSangre.ItemIndex = 0;
            if ((cmbEstadoIMC.Properties.DataSource as IList).Count > 0) cmbEstadoIMC.ItemIndex = 0;

            txtNo.Text = "";
            txtPeso.EditValue = "0";
            txtEstatura.EditValue = "0";
            txtIMC.EditValue = "0";
            txtParametroAbdominal.EditValue = "0";
            txtPresionArterial.Text = "";
            txtFrecuenciaCardiaca.EditValue = "0";
            txtTemperatura.EditValue = "0";
            txtSaturacion.EditValue = "0";
            chbAtencionPrioritaria.Checked = false;
            txtDiagnostico.EditValue = "";
            txtAntecedentesFamiliares.EditValue = "";
            txtAlergias.EditValue = "";
            txtMedicacionContinua.EditValue = "";
            dtpPeriodo.DateTime = DateTime.Now;
            txtObservaciones.EditValue = "";
            txtCIE10.EditValue = "";
            txtAntecedentesQuirurgicos.Text = "";
            

            bsArchivoAdjunto.DataSource = new List<PersonaFichaMedicaAdjunto>();
            gcArchivoAdjunto.DataSource = bsArchivoAdjunto;
        }

        private void lBuscar()
        {
            var poListaObject = loLogicaNegocio.goListarFichaMedica(lIdPersona);
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[]
                                {
                                    new DataColumn("No"),
                                    new DataColumn("Periodo", typeof(DateTime)),
                                    new DataColumn("Persona"),
                                    new DataColumn("Tipo"),
                                });

            poListaObject.ForEach(a =>
            {
                DataRow row = dt.NewRow();
                row["No"] = a.IdPersonaFichaMedica;
                row["Periodo"] = a.Periodo;
                row["Persona"] = a.DesPersona;
                row["Tipo"] = a.DesPersona;
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

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                pbCargado = false;
                var poObject = loLogicaNegocio.goConsultarFichaMedica(Convert.ToInt32(txtNo.Text.Trim()));
                cmbPersona.EditValue = poObject.IdPersona.ToString();
                cmbTipo.EditValue = poObject.CodigoTipo;
                cmbEstadoIMC.EditValue = poObject.CodigoEstadoIMC;
                cmbTipoSangre.EditValue = poObject.CodigoTipoSangre;
                txtNo.EditValue = poObject.IdPersonaFichaMedica;
                txtPeso.EditValue = poObject.Peso;
                txtEstatura.EditValue = poObject.Estatura;
                txtIMC.EditValue = poObject.IMC;
                txtParametroAbdominal.EditValue = poObject.ParametroAbdominal;
                txtPresionArterial.Text = poObject.PresionArterial;
                txtFrecuenciaCardiaca.EditValue = poObject.FrecuenciaCardiaca;
                txtTemperatura.EditValue = poObject.Temperatura;
                txtSaturacion.EditValue = poObject.Saturacion;
                chbAtencionPrioritaria.Checked = poObject.AtencionPrioritaria;
                txtDiagnostico.EditValue = poObject.Diagnostico;
                txtAntecedentesFamiliares.EditValue = poObject.AntecedentesFamiliares;
                txtAlergias.EditValue = poObject.Alergias;
                txtMedicacionContinua.EditValue = poObject.MedicacionContinua;
                dtpPeriodo.DateTime = poObject.Periodo;
                txtObservaciones.EditValue = poObject.Observaciones;
                txtCIE10.EditValue = poObject.CIE10;
                txtAntecedentesQuirurgicos.EditValue = poObject.AntecedentesQuirurgicos;

                bsArchivoAdjunto.DataSource = poObject.PersonaFichaMedicaAdjunto;
                gcArchivoAdjunto.DataSource = bsArchivoAdjunto;

                pbCargado = true;
            }
        }

        #endregion

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

        private void cmbTipo_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbPersona_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    var pId = loLogicaNegocio.giGetIdFichaMedica(int.Parse(cmbPersona.EditValue.ToString()));
                    if (pId != 0)
                    {
                        txtNo.Text = pId.ToString();
                        lConsultar();
                    }
                    else
                    {
                        lLimpiar(false);
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
