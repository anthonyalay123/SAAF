using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GEN_Entidad;
using REH_Presentacion.TalentoHumano.Parametrizadores;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace REH_Presentacion.Parametrizadores
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 09/06/2020
    /// Formulario para administrar Empleados y Contratos
    /// </summary>
    public partial class frmPaEmpleado : frmBaseTrxDev
    {
        #region Variables
        clsNEmpleado loLogicaNegocio;
        RepositoryItemLookUpEdit poCmbProvincia;
        RepositoryItemLookUpEdit poCmbPais;
        RepositoryItemButtonEdit rpiBtnAdd;
        RepositoryItemButtonEdit rpiBtnDownload;
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnDel;

        RepositoryItemButtonEdit rpiBtnDelEdu = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDelCap = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDelCarFam = new RepositoryItemButtonEdit();

        private bool pbCargar;
        RepositoryItemButtonEdit rpiBtnDelFP;
        BindingSource bsDatosCuentaBancaria;
        BindingSource bsDescuentos = new BindingSource();
        BindingSource bsHistorialPersonalizado = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDelDes = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDelHis = new RepositoryItemButtonEdit();

        BindingSource bsEstudios = new BindingSource();
        BindingSource bsCapacitaciones = new BindingSource();
        BindingSource bsCargaFamiliar = new BindingSource();

        private List<Combo> loComboTipoRol = new List<Combo>();
        private List<Combo> loComboRubro = new List<Combo>();
        private bool lbPerfilMedico = false;
        #endregion

        #region Eventos

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmPaEmpleado()
        {
            InitializeComponent();
            lCargarEventos();
            loLogicaNegocio = new clsNEmpleado();
            tabEmp.Select();
            bsDatosCuentaBancaria = new BindingSource();

            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnAdd = new RepositoryItemButtonEdit();
            rpiBtnDownload = new RepositoryItemButtonEdit();
            rpiBtnShow = new RepositoryItemButtonEdit();
            rpiBtnDelFP = new RepositoryItemButtonEdit();

            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnDelFP.ButtonClick += rpiBtnDelFP_ButtonClick;
            rpiBtnAdd.ButtonClick += rpiBtnAdd_ButtonClick;
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnDownload.ButtonClick += rpiBtnDownload_ButtonClick;
            rpiBtnDelDes.ButtonClick += rpiBtnDelDes_ButtonClick;
            rpiBtnDelHis.ButtonClick += rpiBtnDelHis_ButtonClick;
            rpiBtnDelEdu.ButtonClick += rpiBtnDelEdu_ButtonClick;
            rpiBtnDelCap.ButtonClick += rpiBtnDelCap_ButtonClick;
            rpiBtnDelCarFam.ButtonClick += rpiBtnDelCarFam_ButtonClick;

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

                if (tbcDatosEmpleado.SelectedTabPageIndex == 0)
                {
                    int piIndex;
                    //// Tomamos la fila seleccionada
                    //piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                    //// Tomamos la lista del Grid
                    //var poLista = (List<SolicitudCompraDetalle>)bsDatos.DataSource;

                    //if (poLista.Count > 0 && piIndex >= 0)
                    //{
                    //    // Tomamos la entidad de la fila seleccionada
                    //    var poEntidad = poLista[piIndex];

                    //    // Eliminar Fila seleccionada de mi lista
                    //    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    //    poLista.RemoveAt(piIndex);

                    //    // Asigno mi nueva lista al Binding Source
                    //    bsDatos.DataSource = poLista;
                    //    dgvDatos.RefreshData();
                    //}
                }
                else if (tbcDatosEmpleado.SelectedTabPageIndex == 1)
                {
                    int piIndex;
                    //// Tomamos la fila seleccionada
                    //piIndex = dgvArchivoAdjunto.GetFocusedDataSourceRowIndex();
                    //// Tomamos la lista del Grid
                    //var poLista = (List<SolicitudCompraArchivoAdjunto>)bsArchivoAdjunto.DataSource;

                    //if (poLista.Count > 0 && piIndex >= 0)
                    //{
                    //    // Tomamos la entidad de la fila seleccionada
                    //    var poEntidad = poLista[piIndex];

                    //    // Eliminar Fila seleccionada de mi lista
                    //    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    //    poLista.RemoveAt(piIndex);

                    //    // Asigno mi nueva lista al Binding Source
                    //    bsArchivoAdjunto.DataSource = poLista;
                    //    dgvArchivoAdjunto.RefreshData();
                    //}
                }
                else if (tbcDatosEmpleado.SelectedTabPageIndex == 2)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvDocumentos.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<EmpleadoDocumento>)bsDocumentos.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsDocumentos.DataSource = poLista;
                        dgvDocumentos.RefreshData();
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
        private void rpiBtnDelHis_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {

                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvHistorial.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<PersonaHistorial>)bsHistorialPersonalizado.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsHistorialPersonalizado.DataSource = poLista;
                    dgvHistorial.RefreshData();
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
        private void rpiBtnDelEdu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {

                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvEstudios.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<PersonaEducacion>)bsEstudios.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsEstudios.DataSource = poLista;
                    dgvEstudios.RefreshData();
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
        private void rpiBtnDelCap_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvCapacitaciones.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<PersonaCapacitacion>)bsCapacitaciones.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsCapacitaciones.DataSource = poLista;
                    dgvCapacitaciones.RefreshData();
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
        private void rpiBtnDelCarFam_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvCargaFamiliar.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<EmpleadoCargaFamiliar>)bsCargaFamiliar.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsCargaFamiliar.DataSource = poLista;
                    dgvCargaFamiliar.RefreshData();
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
        private void rpiBtnDelFP_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvCuentasBancarias.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<EmpleadoContratoCuentaBancaria>)bsDatosCuentaBancaria.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsDatosCuentaBancaria.DataSource = poLista;
                    dgvCuentasBancarias.RefreshData();
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
        private void rpiBtnDelDes_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDescuentos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<EmpleadoDescuentoPrestamo>)bsDescuentos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsDescuentos.DataSource = poLista;
                    dgvDescuentos.RefreshData();
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
        private void rpiBtnAdd_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDocumentos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<EmpleadoDocumento>)bsDocumentos.DataSource;

                OpenFileDialog ofdArchicoAdjunto = new OpenFileDialog();
                // Presenta un dialogo para seleccionar las imagenes
                ofdArchicoAdjunto.Title = "Seleccione Archivo pdf";
                ofdArchicoAdjunto.Filter = "Image Files( *.pdf; )|  *.pdf; ";

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

                                poLista[piIndex].Cargar = Diccionario.Cargar;
                                poLista[piIndex].ArchivoAdjunto = Name;
                                poLista[piIndex].RutaOrigen = ofdArchicoAdjunto.FileName;
                                string psFileName = file.Name.ToLower();
                                if (psFileName.Contains(".pdf"))
                                {
                                    psFileName = psFileName.Replace(".pdf", "");
                                }
                                poLista[piIndex].NombreArchivo = psFileName;
                                poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaCompartidaArchivoEmpleado"].ToString() + Name;
                                // Asigno mi nueva lista al Binding Source
                                bsDocumentos.DataSource = poLista;
                                dgvDocumentos.RefreshData();

                                //int piIndex = dgvDocumentos.FocusedRowHandle;
                                //var poLista = (List<EmpleadoDocumento>)bsDocumentos.DataSource;
                                //poLista[piIndex].Cargar = Diccionario.Cargar;
                                //poLista[piIndex].RutaOrigen = ofdRuta.FileName;
                                //poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaCompartidaArchivoEmpleado"].ToString() + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff") + Path.GetExtension(ofdRuta.FileName);
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
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDocumentos.FocusedRowHandle;
                var poLista = (List<EmpleadoDocumento>)bsDocumentos.DataSource;

                string psRuta = File.Exists(poLista[piIndex].RutaDestino) ? poLista[piIndex].RutaDestino : poLista[piIndex].RutaOrigen;

                if (!string.IsNullOrEmpty(psRuta))
                {
                    if (File.Exists(psRuta))
                    {
                        frmVerPdf pofrmVerPdf = new frmVerPdf();
                        pofrmVerPdf.lsRuta = psRuta;
                        pofrmVerPdf.Show();
                    }
                    else
                    {
                        XtraMessageBox.Show("No existe archivo en ruta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existe ruta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private void rpiBtnDownload_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {

                int piIndex = dgvDocumentos.FocusedRowHandle;
                var poLista = (List<EmpleadoDocumento>)bsDocumentos.DataSource;

                string psRuta = poLista[piIndex].RutaDestino + poLista[piIndex].ArchivoAdjunto;
                string psNombreArchivo = poLista[piIndex].NombreArchivo;

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
        /// Eveto que se ejecuta al inciar el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPaEmpleado_Load(object sender, EventArgs e)
        {
            try
            {

                lCargarEventosBotones();

                clsComun.gLLenarCombo(ref cmbEstadoCivil, loLogicaNegocio.goConsultarComboEstadoCivil(), true);
                var poListaGenero = loLogicaNegocio.goConsultarComboGenero();
                clsComun.gLLenarCombo(ref cmbGenero, poListaGenero, true);
                clsComun.gLLenarCombo(ref cmbNivelEducacion, loLogicaNegocio.goConsultarComboNivelEducaciona(), true);
                clsComun.gLLenarCombo(ref cmbTitulo, loLogicaNegocio.goConsultarComboTitulo(), true);
                clsComun.gLLenarCombo(ref cmbColorPiel, loLogicaNegocio.goConsultarComboColorPiel(), true);
                clsComun.gLLenarCombo(ref cmbColorOjos, loLogicaNegocio.goConsultarComboColorOjos(), true);
                clsComun.gLLenarCombo(ref cmbTipoSangre, loLogicaNegocio.goConsultarComboTipoSangre(), true);
                clsComun.gLLenarCombo(ref cmbTipoLicencia, loLogicaNegocio.goConsultarComboTipoLicencia(), true);
                clsComun.gLLenarCombo(ref cmbTipoEmpleado, loLogicaNegocio.goConsultarComboTipoEmpleado(), false);
                clsComun.gLLenarCombo(ref cmbTipoVivienda, loLogicaNegocio.goConsultarComboTipoVivienda(), true);
                clsComun.gLLenarCombo(ref cmbTipoMaterialVivienda, loLogicaNegocio.goConsultarComboTipoMaterialVivienda(), true);
                clsComun.gLLenarCombo(ref cmbRegion, loLogicaNegocio.goConsultarComboRegion(), true);
                clsComun.gLLenarCombo(ref cmbEstadoEmpleado, loLogicaNegocio.goConsultarComboEstadoRegistro());
                clsComun.gLLenarCombo(ref cmbSucursal, loLogicaNegocio.goConsultarComboSucursal(), true);
                clsComun.gLLenarCombo(ref cmbDepartamento, loLogicaNegocio.goConsultarComboDepartamento(), true);
                clsComun.gLLenarCombo(ref cmbTipoContrato, loLogicaNegocio.goConsultarComboTipoContrato(), true);
                clsComun.gLLenarCombo(ref cmbCentroCosto, loLogicaNegocio.goConsultarComboCentroCosto(), true);
                clsComun.gLLenarCombo(ref cmbJefeInmediato, loLogicaNegocio.goConsultarComboJefeInmediato(), true);
                clsComun.gLLenarCombo(ref cmbCargo, loLogicaNegocio.goConsultarComboCargo(), true);
                clsComun.gLLenarCombo(ref cmbTipoComision, loLogicaNegocio.goConsultarComboTipoComision(), true);
                clsComun.gLLenarCombo(ref cmbMotivoFinContrato, loLogicaNegocio.goConsultarComboMotivoFinContrato(), true);
                var poComboBanco = loLogicaNegocio.goConsultarComboBanco();
                clsComun.gLLenarCombo(ref cmbBanco, poComboBanco, true);
                var poComboFormaPago = loLogicaNegocio.goConsultarComboFormaPago();
                clsComun.gLLenarCombo(ref cmbFormaPago, poComboFormaPago, true);
                var poTipoCuentaBancaria = loLogicaNegocio.goConsultarComboTipoCuentaBancaria();
                clsComun.gLLenarCombo(ref cmbTipoCuentaBancaria, poTipoCuentaBancaria, true);
                clsComun.gLLenarCombo(ref cmbEstadoContrato, loLogicaNegocio.goConsultarComboEstadoRegistro());
                clsComun.gLLenarCombo(ref cmbTipoDiscapacidad, loLogicaNegocio.goConsultarComboTipoDiscapacidad(), true);
                clsComun.gLLenarCombo(ref cmbReferenciaBiometrico, loLogicaNegocio.goBioConsultaEmpleadoBiometrico(), true);
                var poListaEstado = loLogicaNegocio.goConsultarComboEstadoRegistro(true);

                loComboTipoRol = loLogicaNegocio.goConsultarComboTipoRol();
                loComboRubro = loLogicaNegocio.goConsultarComboRubroDescuentoPrestamos();

                clsComun.gLLenarComboGrid(ref dgvCuentasBancarias, loComboTipoRol, "CodigoTipoRol", true);
                clsComun.gLLenarComboGrid(ref dgvCuentasBancarias, poComboBanco, "CodigoBanco", false);
                clsComun.gLLenarComboGrid(ref dgvCuentasBancarias, poComboFormaPago, "CodigoFormaPago", false);
                clsComun.gLLenarComboGrid(ref dgvCuentasBancarias, poTipoCuentaBancaria, "CodigoTipoCuentaBancaria", false);

                clsComun.gLLenarComboGrid(ref dgvDescuentos, loComboTipoRol, "CodigoTipoRol", false);
                clsComun.gLLenarComboGrid(ref dgvDescuentos, loComboRubro, "CodigoRubro", true);

                clsComun.gLLenarComboGridOut(ref dgvDireccion, loLogicaNegocio.goConsultarComboPais(), "IdPais", out poCmbPais, true);
                poCmbPais.EditValueChanged += cmbPais_EditValueChanged;

                clsComun.gLLenarComboGridOut(ref dgvDireccion, loLogicaNegocio.goConsultarComboProvincia(), "IdProvincia",out poCmbProvincia, true);
                poCmbProvincia.EditValueChanged += cmbProvincia_EditValueChanged;

                clsComun.gLLenarComboGrid(ref dgvDireccion, loLogicaNegocio.goConsultarComboCanton(), "IdCanton", true);

                clsComun.gLLenarComboGrid(ref dgvDireccion, poListaEstado, "CodigoEstado");

                clsComun.gLLenarComboGrid(ref dgvCargaFamiliar, loLogicaNegocio.goConsultarComboTipoCargaFamiliar(), "CodigoTipoCargaFamiliar", true);
                clsComun.gLLenarComboGrid(ref dgvCargaFamiliar, poListaEstado, "CodigoEstado", false);
                clsComun.gLLenarComboGrid(ref dgvCargaFamiliar, poListaGenero, "CodigoTipoGenero", false);
                clsComun.gLLenarComboGrid(ref dgvContacto, loLogicaNegocio.goConsultarComboParentezco(), "CodigoParentezco", false);
                clsComun.gLLenarComboGrid(ref dgvContacto, loLogicaNegocio.goConsultarComboTipoContacto(), "CodigoTipoContacto", false);
                clsComun.gLLenarComboGrid(ref dgvContacto, poListaEstado, "CodigoEstado", false);
                clsComun.gLLenarComboGrid(ref dgvDocumentos, poListaEstado, "CodigoEstado", false);

                clsComun.gLLenarComboGrid(ref dgvEstudios, loLogicaNegocio.goConsultarComboNivelEducaciona(), "CodigoTipoEducacion", true);
                clsComun.gLLenarCombo(ref cmbProvincia, loLogicaNegocio.goConsultarComboProvincia(), true);
                clsComun.gLLenarCombo(ref cmbCiudad, loLogicaNegocio.goConsultarComboCanton(), true);
                clsComun.gLLenarCombo(ref cmbParroquia, loLogicaNegocio.goConsultarComboParroquia(), true);
                clsComun.gLLenarCombo(ref cmbCaracteristicasVivienda, loLogicaNegocio.goConsultarComboCaracteristicasVivienda(), true);


                //RepositoryItemButtonEdit btnCargar = new RepositoryItemButtonEdit();
                //btnCargar.ButtonClick += btnCargar_Click;
                //dgvDocumentos.Columns["Cargar"].ColumnEdit = btnCargar;
                //dgvDocumentos.Columns["Cargar"].ShowButtonMode = ShowButtonModeEnum.ShowAlways;

                //RepositoryItemButtonEdit btnVisualizar = new RepositoryItemButtonEdit();
                //btnVisualizar.ButtonClick += btnVisualizar_Click;
                //dgvDocumentos.Columns["Visualizar"].ColumnEdit = btnVisualizar;
                //dgvDocumentos.Columns["Visualizar"].ShowButtonMode = ShowButtonModeEnum.ShowAlways;

                //RepositoryItemButtonEdit btnDescargar = new RepositoryItemButtonEdit();
                //btnDescargar.ButtonClick += btnDescargar_Click;
                //dgvDocumentos.Columns["Descargar"].ColumnEdit = btnDescargar;
                //dgvDocumentos.Columns["Descargar"].ShowButtonMode = ShowButtonModeEnum.ShowAlways;

                lLimpiar();
                
                pbCargar = true;

                txtNumeroIdentificacion.Focus();

                lblCuentaContable.Visible = false;
                txtCuentaContable.Visible = false;

                if (!clsPrincipal.gbSuperUsuario)
                {
                    tabEmp.TabPages[6].PageVisible = false;
                    lblCuentaContable.Visible = true;
                    txtCuentaContable.Visible = true;
                }

                if (clsPrincipal.gIdPerfil == 39)
                {
                    lbPerfilMedico = true;
                    //tabEmp.TabPages[0].PageVisible = false; Datos Personales
                    //tabEmp.TabPages[1].PageVisible = false; // Educacion
                    //tabEmp.TabPages[2].PageVisible = false; // Datos Familiares
                    tabEmp.TabPages[3].PageVisible = false; // Datos Empresa
                    //tabEmp.TabPages[4].PageVisible = false; // Información Complementaria
                    tabEmp.TabPages[5].PageVisible = false; // Historial
                    tabEmp.TabPages[6].PageVisible = false; // Bitacora de Cambios
                    
                    btnInactivarJubilado.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void cmbPais_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                dgvDireccion.SetFocusedRowCellValue("IdProvincia", 0);
                dgvDireccion.SetFocusedRowCellValue("IdCanton", 0);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void cmbProvincia_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                dgvDireccion.PostEditor();
                dgvDireccion.SetFocusedRowCellValue("IdProvincia",null);
                dgvDireccion.SetFocusedRowCellValue("IdCanton", 0);

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void cmbCanton_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                dgvDireccion.PostEditor();
                dgvDireccion.SetFocusedRowCellValue("IdCanton", null);

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDireccion_ShownEditor(object sender, EventArgs e)
        {
            ColumnView view = (ColumnView)sender;
            if (view.FocusedColumn.FieldName == "IdProvincia")
            {
                LookUpEdit cmb = (LookUpEdit)view.ActiveEditor;
                int piId = Convert.ToInt32(view.GetFocusedRowCellValue("IdPais"));
                clsComun.gLLenarCombo(ref cmb, loLogicaNegocio.goConsultarComboProvincia(piId), true);
                //cmb.Properties.DataSource = loLogicaNegocio.goConsultarComboProvincia(piId);
            }
            else if(view.FocusedColumn.FieldName == "IdCanton")
            {
                LookUpEdit cmb = (LookUpEdit)view.ActiveEditor;
                int piId = Convert.ToInt32(view.GetFocusedRowCellValue("IdProvincia"));
                clsComun.gLLenarCombo(ref cmb, loLogicaNegocio.goConsultarComboCanton(piId), true);
                //editor.Properties.DataSource = loLogicaNegocio.goConsultarComboCanton(piId);
            }
        }

        /// <summary>
        /// Carga un archivo en memoria
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCargar_Click(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                OpenFileDialog ofdRuta = new OpenFileDialog();
                ofdRuta.Title = "Seleccione Archivo";
                ofdRuta.Filter = "Image Files(*.pdf;)|*.pdf;";

                if (ofdRuta.ShowDialog() == DialogResult.OK)
                {
                    if (!ofdRuta.FileName.Equals(""))
                    {
                
                        int piIndex = dgvDocumentos.FocusedRowHandle;
                        var poLista = (List<EmpleadoDocumento>)bsDocumentos.DataSource;
                        poLista[piIndex].Cargar = Diccionario.Cargar;
                        poLista[piIndex].RutaOrigen = ofdRuta.FileName;
                        poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaCompartidaArchivoEmpleado"].ToString() + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff") + Path.GetExtension(ofdRuta.FileName);
                    }


                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Descarga un archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDescargar_Click(object sender, EventArgs e)
        {
            try
            {

                int piIndex = dgvDocumentos.FocusedRowHandle;
                var poLista = (List<EmpleadoDocumento>)bsDocumentos.DataSource;

                string psRuta = poLista[piIndex].RutaDestino;

                if (!string.IsNullOrEmpty(psRuta))
                {
                    if(File.Exists(psRuta))
                    {
                        //Creación de Carpeta en directorio de programa
                        string psPath = ConfigurationManager.AppSettings["CarpetaDescarga"].ToString();
                        if (!Directory.Exists(psPath))
                        {
                            Directory.CreateDirectory(psPath);
                        }
                        string psNombre = txtNombreCompleto.Text.Trim().Replace(" ", "_");
                        string psArchivo = poLista[piIndex].Descripcion.Replace(" ", "_");
                        string psExtension = Path.GetExtension(psRuta);

                        string psRutaDestino = psPath + psNombre + "_" + psArchivo + psExtension;
                        if (File.Exists(psRutaDestino))
                        {
                            File.Delete(psRutaDestino);
                        }

                        File.Copy(psRuta, psRutaDestino);

                        XtraMessageBox.Show("Archivo descargado en: " + psRutaDestino, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        XtraMessageBox.Show("Debe guardar cambios para descargar, Archivo no existe en Base de Datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("Debe guardar cambios para descargar, Archivo no existe en Base de Datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Visualiza un archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            try
            {
                int piIndex = dgvDocumentos.FocusedRowHandle;
                var poLista = (List<EmpleadoDocumento>)bsDocumentos.DataSource;

                string psRuta = File.Exists(poLista[piIndex].RutaDestino) ? poLista[piIndex].RutaDestino : poLista[piIndex].RutaOrigen;

                if (!string.IsNullOrEmpty(psRuta))
                {
                    if (File.Exists(psRuta))
                    {
                        frmVerPdf pofrmVerPdf = new frmVerPdf();
                        pofrmVerPdf.lsRuta = psRuta;
                        pofrmVerPdf.Show();
                    }
                    else
                    {
                        XtraMessageBox.Show("No existe archivo en ruta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existe ruta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                dgvDireccion.PostEditor();
                dgvCargaFamiliar.PostEditor();
                dgvContacto.PostEditor();
                dgvDocumentos.PostEditor();
                dgvCuentasBancarias.PostEditor();
                dgvDescuentos.PostEditor();
                dgvHistorial.PostEditor();
                dgvEstudios.PostEditor();
                dgvCapacitaciones.PostEditor();

                bool ValidaDatosContrato = true;
                if (cmbEstadoEmpleado.EditValue.ToString() == Diccionario.Inactivo) ValidaDatosContrato = false;
                if (btnCrearNuevoContrato.Enabled && lblEstadoPrincipal.Text == Diccionario.DesInactivo && !chbCrearNuevoContrato.Checked) ValidaDatosContrato = false;
                
                if (lbEsValido(ValidaDatosContrato))
                {
                    Persona poPersona = new Persona();
                    poPersona.CodigoEstado = Diccionario.Activo;
                    poPersona.CodigoTipoPersona = Diccionario.ListaCatalogo.TipoPersonaClass.Natural;
                    poPersona.CodigoTipoIdentificacion = Diccionario.ListaCatalogo.TipoIdentificacionClass.Cedula;
                    poPersona.IdPersona = !string.IsNullOrEmpty(lblIdPersona.Text) ? int.Parse(lblIdPersona.Text) : 0;
                    poPersona.RutaImagenCopy = ofdFoto.FileName;
                    poPersona.NameImagenCopy = ofdFoto.SafeFileName;
                    poPersona.RutaImagen = lblFoto.Text;
                    poPersona.NameImagen = lblFoto.Tag?.ToString();
                    poPersona.NumeroIdentificacion = txtNumeroIdentificacion.Text.Trim();
                    poPersona.ApellidoPaterno = txtApellidoPaterno.Text.Trim();
                    poPersona.ApellidoMaterno = txtApellidoMaterno.Text.Trim();
                    poPersona.PrimerNombre = txtPrimerNombre.Text.Trim();
                    poPersona.SegundoNombre = txtSegundoNombre.Text.Trim();
                    poPersona.NombreCompleto = txtNombreCompleto.Text.Trim();
                    poPersona.Correo = txtCorreoPersonal.Text.Trim();
                    poPersona.CodigoEstadoCivil = cmbEstadoCivil.EditValue.ToString();
                    poPersona.CodigoGenero = cmbGenero.EditValue.ToString();
                    poPersona.LugarNacimiento = txtLugarNacimiento.Text.Trim();
                    poPersona.FechaNacimiento = dtpFechaNacimiento.Value;
                    poPersona.CodigoNivelEducacion = cmbNivelEducacion.EditValue.ToString() != Diccionario.Seleccione ? cmbNivelEducacion.EditValue.ToString() : null;
                    poPersona.CodigoTitulo = cmbTitulo.EditValue.ToString() != Diccionario.Seleccione ? cmbTitulo.EditValue.ToString() : null;
                    poPersona.Titulo = txtTitulo.Text;
                    poPersona.NumeroRegistroProfesional = txtNumeroRegistroProfesional.Text.Trim();
                    if (!string.IsNullOrEmpty(txtPeso.Text.Trim())) poPersona.Peso = decimal.Parse(txtPeso.Text.Trim()); else poPersona.Peso = null;
                    if (!string.IsNullOrEmpty(txtEstatura.Text.Trim())) poPersona.Estatura = decimal.Parse(txtEstatura.Text.Trim()); else poPersona.Estatura = null;
                    if (!txtPorcentajeDiscapacidad.Enabled) txtPorcentajeDiscapacidad.Text = null;
                    if (!string.IsNullOrEmpty(txtPorcentajeDiscapacidad.Text.Trim())) poPersona.PorcentajeDiscapacidad = decimal.Parse(txtPorcentajeDiscapacidad.EditValue.ToString()); else poPersona.PorcentajeDiscapacidad = null;
                    poPersona.CodigoColorPiel = cmbColorPiel.EditValue.ToString() != Diccionario.Seleccione ? cmbColorPiel.EditValue.ToString() : null;
                    poPersona.CodigoColorOjos = cmbColorOjos.EditValue.ToString() != Diccionario.Seleccione ? cmbColorOjos.EditValue.ToString() : null;
                    poPersona.CodigoTipoSangre = cmbTipoSangre.EditValue.ToString() != Diccionario.Seleccione ? cmbTipoSangre.EditValue.ToString() : null;
                    poPersona.CodigoTipoLicencia = cmbTipoLicencia.EditValue.ToString() != Diccionario.Seleccione ? cmbTipoLicencia.EditValue.ToString() : null;
                    poPersona.CodigoRegion = cmbRegion.EditValue.ToString() != Diccionario.Seleccione ? cmbRegion.EditValue.ToString() : null;
                    poPersona.CodigoTipoDiscapacidad = cmbTipoDiscapacidad.EditValue.ToString() != Diccionario.Seleccione ? cmbTipoDiscapacidad.EditValue.ToString() : null;
                    if (cmbReferenciaBiometrico.EditValue.ToString() != Diccionario.Seleccione)
                        poPersona.IdBiometrico = int.Parse(cmbReferenciaBiometrico.EditValue.ToString());
                    else
                        poPersona.IdBiometrico = null;

                    if (dtpFechaExpiracionLicencia.Checked) poPersona.FechaExpiracionLicencia = dtpFechaExpiracionLicencia.Value; else poPersona.FechaExpiracionLicencia = null;
                    poPersona.NumeroLicencia = txtNumeroLicencia.Text.Trim();
                    poPersona.Fecha = DateTime.Now;
                    poPersona.Usuario = clsPrincipal.gsUsuario;
                    poPersona.Terminal = string.Empty;
                    poPersona.DescuentaIR = chbDescuentaIR.Checked;
                    poPersona.EnfermedadCatastrofica = chbTieneEnfermedadCatastrofica.Checked;

                    if (cmbProvincia.EditValue.ToString() != Diccionario.Seleccione)
                        poPersona.IdProvincia = int.Parse(cmbProvincia.EditValue.ToString());
                    else
                        poPersona.IdProvincia = null;

                    if (cmbParroquia.EditValue.ToString() != Diccionario.Seleccione)
                        poPersona.IdParroquia = int.Parse(cmbParroquia.EditValue.ToString());
                    else
                        poPersona.IdParroquia = null;

                    if (cmbCiudad.EditValue.ToString() != Diccionario.Seleccione)
                        poPersona.IdCanton = int.Parse(cmbCiudad.EditValue.ToString());
                    else
                        poPersona.IdCanton = null;

                    poPersona.Direccion = txtDireccion.Text;
                    poPersona.TelefonoCelular = txtTelefonoCelular.Text;
                    poPersona.TelefonoConvencional = txtTelefonoConvencional.Text;
                    poPersona.ContactoCasoEmergencia = txtContactoEmergencia.Text;
                    poPersona.TelefonoCasoEmergencia = txtTelefonoContactoCasoEmergencia.Text;

                    poPersona.PersonaDireccion = (List<PersonaDireccion>)bsDireccion.DataSource;
                    poPersona.PersonaEducacion = (List<PersonaEducacion>)bsEstudios.DataSource;
                    poPersona.PersonaCapacitacion = (List<PersonaCapacitacion>)bsCapacitaciones.DataSource;

                    Empleado poEmpleado = new Empleado();
                    poEmpleado.CodigoRegionIess = txtCodigoRegionIess.Text.Trim();
                    poEmpleado.CodigoEstado = cmbEstadoEmpleado.EditValue.ToString();
                    poEmpleado.CodigoTipoEmpleado = cmbTipoEmpleado.EditValue.ToString();
                    poEmpleado.Correo = txtCorreoLaboral.Text.Trim();
                    poEmpleado.CuentaContable = txtCuentaContable.Text.Trim();
                    poEmpleado.NumeroSeguroSocial = txtNumeroSeguroSocial.Text.Trim();
                    poEmpleado.CodigoSectorial = txtCodigoSectorial.Text.Trim();
                    if (cmbTipoVivienda.EditValue.ToString() != Diccionario.Seleccione) poEmpleado.CodigoTipoVivienda = cmbTipoVivienda.EditValue.ToString(); else poEmpleado.CodigoTipoVivienda = null;
                    if (cmbCaracteristicasVivienda.EditValue.ToString() != Diccionario.Seleccione) poEmpleado.CaracteristicasVivienda = cmbCaracteristicasVivienda.EditValue.ToString(); else poEmpleado.CaracteristicasVivienda = null;
                    if (cmbTipoMaterialVivienda.EditValue.ToString() != Diccionario.Seleccione) poEmpleado.CodigoTipoMaterialVivienda = cmbTipoMaterialVivienda.EditValue.ToString(); else poEmpleado.CodigoTipoMaterialVivienda = null;
                    if (!string.IsNullOrEmpty(txtValorArriendo.Text.Trim())) poEmpleado.ValorArriendo = decimal.Parse(txtValorArriendo.Text.Trim()); else poEmpleado.ValorArriendo = null;
                    if (!string.IsNullOrEmpty(txtNumeroLibretaMilitar.Text.Trim())) poEmpleado.NumeroLibretaMilitar = txtNumeroLibretaMilitar.Text.Trim(); else poEmpleado.NumeroLibretaMilitar = null;
                    poEmpleado.AporteIessConyuge = chbAplicaIessConyugue.Checked;
                    poEmpleado.AplicaHorasExtrasAntesLlegada = chbAplicaHorasExtrasAntesLlegada.Checked;
                    poEmpleado.AplicaTiempoGraciaPostSalida = chbAplicaTiempoGraciaPostSalida.Checked;
                    poEmpleado.MostrarEnAsistencia = chbMostrarAsistencia.Checked;
                    poEmpleado.CalculaIRComisiones = chbCalculaIRComisiones.Checked;

                    poEmpleado.EmpleadoCargaFamiliar = (List<EmpleadoCargaFamiliar>)bsCargaFamiliar.DataSource;
                    poEmpleado.EmpleadoContacto = (List<EmpleadoContacto>)bsContactos.DataSource;
                    if(ValidaDatosContrato)
                    {
                        EmpleadoContrato poContrato = new EmpleadoContrato();
                        poContrato.IdEmpleadoContrato = chbCrearNuevoContrato.Checked ? 0 : !string.IsNullOrEmpty(lblIdContrato.Text) ? int.Parse(lblIdContrato.Text) : 0;;
                        poContrato.CodigoEstado = cmbEstadoContrato.EditValue.ToString();
                        poContrato.CodigoSucursal = cmbSucursal.EditValue.ToString();
                        poContrato.CodigoDepartamento = cmbDepartamento.EditValue.ToString();
                        poContrato.CodigoTipoContrato = cmbTipoContrato.EditValue.ToString();
                        poContrato.CodigoCentroCosto = cmbCentroCosto.EditValue.ToString();
                        if (cmbJefeInmediato.EditValue.ToString() != Diccionario.Seleccione) poContrato.IdPersonaJefe = int.Parse(cmbJefeInmediato.EditValue.ToString()); else poContrato.IdPersonaJefe = null;
                        poContrato.CodigoCargoLaboral = cmbCargo.EditValue.ToString();
                        if (cmbTipoComision.EditValue.ToString() != Diccionario.Seleccione) poContrato.CodigoTipoComision = cmbTipoComision.EditValue.ToString(); else poContrato.CodigoTipoComision = null;
                        if (!string.IsNullOrEmpty(txtPorcentajeComision.Text.Trim())) poContrato.PorcentajeComision = decimal.Parse(txtPorcentajeComision.EditValue.ToString()); else poContrato.PorcentajeComision = null;
                        if (!string.IsNullOrEmpty(txtPorcentajeComisionAdicional.Text.Trim())) poContrato.PorcentajeComisionAdicional = decimal.Parse(txtPorcentajeComisionAdicional.EditValue.ToString()); else poContrato.PorcentajeComisionAdicional = null;
                        poContrato.Sueldo = decimal.Parse(txtSueldo.Text.Trim());
                        poContrato.PorcentajePQ = decimal.Parse(txtPorcentajePrimeraQuincena.Text.Trim());
                        if (dtpInicioHorarioLaboral.Checked) poContrato.InicioHorarioLaboral = dtpInicioHorarioLaboral.Value.TimeOfDay; else poContrato.InicioHorarioLaboral = null;
                        if (dtpFinHorarioLaboral.Checked) poContrato.FinHorarioLaboral = dtpFinHorarioLaboral.Value.TimeOfDay; else poContrato.FinHorarioLaboral = null;
                        poContrato.FechaInicioContrato = dtpFechaInicioContrato.Value;
                        if (dtpFechaFinContrato.Checked) poContrato.FechaFinContrato = dtpFechaFinContrato.Value; else poContrato.FechaFinContrato = null;
                        if (dtpFechaIngresoMandato8.Checked) poContrato.FechaIngresoMandato8 = dtpFechaIngresoMandato8.Value; else poContrato.FechaIngresoMandato8 = null;
                        if (cmbMotivoFinContrato.EditValue.ToString() != Diccionario.Seleccione) poContrato.CodigoMotivoFinContrato = cmbMotivoFinContrato.EditValue.ToString(); else poContrato.CodigoMotivoFinContrato = null;
                        poContrato.CodigoBanco = cmbBanco.EditValue.ToString();
                        poContrato.CodigoFormaPago = cmbFormaPago.EditValue.ToString();
                        if (cmbTipoCuentaBancaria.EditValue.ToString() != Diccionario.Seleccione) poContrato.CodigoTipoCuentaBancaria = cmbTipoCuentaBancaria.EditValue.ToString(); else poContrato.CodigoTipoCuentaBancaria = null;
                        poContrato.NumeroCuenta = txtNumeroCuenta.Text.Trim();
                        if (!string.IsNullOrEmpty(txtValorAlimentacion.Text.Trim())) poContrato.ValorAlimentacion = decimal.Parse(txtValorAlimentacion.Text.Trim()); else poContrato.ValorAlimentacion = null;
                        if (!string.IsNullOrEmpty(txtValorMovilizacion.Text.Trim())) poContrato.ValorMovilizacion = decimal.Parse(txtValorMovilizacion.Text.Trim()); else poContrato.ValorMovilizacion = null;
                        poContrato.AplicaAlimentacion = chbAplicaAlimentacion.Checked;
                        poContrato.AplicaMovilizacion = chbAplicaMovilizacion.Checked;
                        poContrato.AplicaHorasExtras = chbAplicaHorasExtras.Checked;
                        poContrato.AplicaIessConyugue = chbAplicaIessConyugue.Checked;
                        poContrato.AcumulaD3 = chbAcumulaD3.Checked;
                        poContrato.AcumulaD4 = chbAcumulaD4.Checked;
                        poContrato.EsJefe = chbEsJefe.Checked;
                        poContrato.EsJubilado = chbEsJubilado.Checked;
                        poContrato.DerechoFondoReserva = chbDerechoFondoReserva.Checked;
                        poContrato.SolicitudFondoReserva = chbSolicitudFondoReserva.Checked;

                        poContrato.EmpleadoContratoCuentaBancaria = (List<EmpleadoContratoCuentaBancaria>)bsDatosCuentaBancaria.DataSource;

                        poEmpleado.EmpleadoContrato = new List<EmpleadoContrato>();
                        poEmpleado.EmpleadoContrato.Add(poContrato);
                    }
                    
                    poEmpleado.EmpleadoDocumento = (List<EmpleadoDocumento>)bsDocumentos.DataSource;
                    poPersona.Empleado = poEmpleado;

                    bool pbGuardaHistorial = false;
                    string result = "";
                    var poEmpleadoPantalla = poEmpleado.EmpleadoContrato?.FirstOrDefault();
                    if (poEmpleadoPantalla != null)
                    {

                        var poEmpleadoConsulta = loLogicaNegocio.goBuscar(txtNumeroIdentificacion.Text.Trim())?.Empleado?.EmpleadoContrato?.FirstOrDefault();
                        if (poEmpleadoConsulta != null)
                        {
                            if (poEmpleadoPantalla.CodigoSucursal != poEmpleadoConsulta.CodigoSucursal || poEmpleadoPantalla.CodigoDepartamento != poEmpleadoConsulta.CodigoDepartamento ||
                                poEmpleadoPantalla.CodigoTipoContrato != poEmpleadoConsulta.CodigoTipoContrato || poEmpleadoPantalla.CodigoCargoLaboral != poEmpleadoConsulta.CodigoCargoLaboral ||
                                poEmpleadoPantalla.Sueldo != poEmpleadoConsulta.Sueldo || poEmpleadoPantalla.CodigoTipoComision != poEmpleadoConsulta.CodigoTipoComision ||
                                poEmpleadoPantalla.PorcentajeComision != poEmpleadoConsulta.PorcentajeComision || poEmpleadoPantalla.PorcentajeComisionAdicional != poEmpleadoConsulta.PorcentajeComisionAdicional ||
                                poEmpleadoPantalla.FechaInicioContrato != poEmpleadoConsulta.FechaInicioContrato || poEmpleadoPantalla.FechaFinContrato != poEmpleadoConsulta.FechaFinContrato ||
                                poEmpleadoPantalla.CodigoBanco != poEmpleadoConsulta.CodigoBanco || poEmpleadoPantalla.CodigoTipoCuentaBancaria != poEmpleadoConsulta.CodigoTipoCuentaBancaria ||
                                poEmpleadoPantalla.NumeroCuenta != poEmpleadoConsulta.NumeroCuenta)
                            {
                                pbGuardaHistorial = true;
                            }
                        }
                        else
                        {
                            pbGuardaHistorial = true;
                        }

                        if (pbGuardaHistorial)
                        {
                            result = XtraInputBox.Show("Ingrese Observación para Historial", "Historial de ficha empleado", "");
                        }
                    }

                    //if (string.IsNullOrEmpty(result))
                    //{
                    //    XtraMessageBox.Show("Debe agregar comentario para poder corregir", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    return;
                    //}

                    if (loLogicaNegocio.gbGuardar(poPersona, (List<EmpleadoDescuentoPrestamo>)bsDescuentos.DataSource, (List<PersonaHistorial>)bsHistorialPersonalizado.DataSource, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, pbGuardaHistorial, result))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                    else
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        /// <summary>
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                List<Persona> poListaObject = loLogicaNegocio.goListar();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Identificación"),
                                    new DataColumn("Nombre"),
                                    new DataColumn("Fecha Nacimiento"),
                                    new DataColumn("Estado Contrato")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Identificación"] = a.NumeroIdentificacion;
                    row["Nombre"] = a.NombreCompleto;
                    row["Fecha Nacimiento"] = a.FechaNacimiento.ToString("dd/MM/yyyy");
                    row["Estado Contrato"] = a.EstadoContrato;

                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Persona" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    txtNumeroIdentificacion.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Eliminar, Cambia a estado eliminado un registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lblIdPersona.Text))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var msg = loLogicaNegocio.gsEliminarMaestro(int.Parse(lblIdPersona.Text), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(msg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            XtraMessageBox.Show(msg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        
                        lLimpiar();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtApellidoPaterno_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtNombreCompleto.Text = txtApellidoPaterno.Text.Trim() + " " + txtApellidoMaterno.Text.Trim() + " " + txtPrimerNombre.Text.Trim() + " " + txtSegundoNombre.Text.Trim();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtNumeroIdentificacion_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNombreCompleto.Text))
                {
                    DialogResult dialogResult = XtraMessageBox.Show("!Se va a cargar por pantalla datos del empleado!", "¿Desea Continuar?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        lConsultar();
                    }
                }
                else
                {
                    lConsultar();
                }
                

                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pctFoto_Click(object sender, EventArgs e)
        {
            try
            {

                // Presenta un dialogo para seleccionar las imagenes
                ofdFoto.Title = "Seleccione Imagen";
                ofdFoto.Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png;";

                if (ofdFoto.ShowDialog() == DialogResult.OK)
                {
                    if (!ofdFoto.FileName.Equals(""))
                    {
                        Image img = Image.FromFile(ofdFoto.FileName);

                        if (img.Width > 200 || img.Height > 260)
                        {
                            XtraMessageBox.Show(string.Format("No es posible cargar imagen, supera el límite permitido de 200X260. \nImagen tiene: Ancho: {0} - Alto: {1}.", img.Width, img.Height), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }


                        string Name = txtPrimerNombre.Text.Substring(0,1) + txtApellidoPaterno.Text + "_"+ DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff") + Path.GetExtension(ofdFoto.FileName);
                        lblFoto.Text = ConfigurationManager.AppSettings["CarpetaCompartidaFoto"].ToString() + Name;
                        lblFoto.Tag = Name;
                        lCargaFoto();
                    }


                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbpContactos.Focus())
                {
                    bsContactos.AddNew();
                    ((List<EmpleadoContacto>)bsContactos.DataSource).LastOrDefault().CodigoEstado = Diccionario.Activo;
                    dgvContacto.Focus();
                    dgvContacto.FocusedRowHandle = dgvDocumentos.RowCount - 1;
                    dgvContacto.FocusedColumn = dgvDocumentos.Columns[0];
                    dgvContacto.ShowEditor();
                }
                else if (tbpCargasFamiliares.Focus())
                {
                    bsCargaFamiliar.AddNew();
                    ((List<EmpleadoCargaFamiliar>)bsCargaFamiliar.DataSource).LastOrDefault().CodigoEstado = Diccionario.Activo;
                    ((List<EmpleadoCargaFamiliar>)bsCargaFamiliar.DataSource).LastOrDefault().CodigoTipoGenero = Diccionario.Seleccione;
                    ((List<EmpleadoCargaFamiliar>)bsCargaFamiliar.DataSource).LastOrDefault().CodigoTipoCargaFamiliar = Diccionario.Seleccione;
                    ((List<EmpleadoCargaFamiliar>)bsCargaFamiliar.DataSource).LastOrDefault().AplicaCargaFamiliar = true;
                    ((List<EmpleadoCargaFamiliar>)bsCargaFamiliar.DataSource).LastOrDefault().CargaFamiliarIR = true;
                    dgvCargaFamiliar.Focus();
                    dgvCargaFamiliar.FocusedRowHandle = dgvDocumentos.RowCount - 1;
                    dgvCargaFamiliar.FocusedColumn = dgvDocumentos.Columns[0];
                    dgvCargaFamiliar.ShowEditor();
                }
                else if (tbpDocumentosAdjuntos.Focus())
                {
                    bsDocumentos.AddNew();
                    ((List<EmpleadoDocumento>)bsDocumentos.DataSource).LastOrDefault().CodigoEstado = Diccionario.Activo;
                    dgvDocumentos.Focus();
                    dgvDocumentos.FocusedRowHandle = dgvDocumentos.RowCount - 1;
                    dgvDocumentos.FocusedColumn = dgvDocumentos.Columns[0];
                    dgvDocumentos.ShowEditor();
                }
                else if (tbDescuentos.Focus())
                {
                    bsDescuentos.AddNew();
                    ((List<EmpleadoDescuentoPrestamo>)bsDescuentos.DataSource).LastOrDefault().CodigoTipoRol = Diccionario.Seleccione;
                    ((List<EmpleadoDescuentoPrestamo>)bsDescuentos.DataSource).LastOrDefault().CodigoRubro = Diccionario.Seleccione;
                    dgvDescuentos.Focus();
                    dgvDescuentos.ShowEditor();
                    dgvDescuentos.UpdateCurrentRow();
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
                bsDireccion.AddNew();
                ((List<PersonaDireccion>)bsDireccion.DataSource).LastOrDefault().CodigoEstado = Diccionario.Activo;
                dgvDireccion.Focus();
                dgvDireccion.ShowEditor();
                dgvDireccion.UpdateCurrentRow();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbTipoComision_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargar)
                {
                    if (cmbTipoComision.EditValue.ToString() != Diccionario.Seleccione)
                    {
                        var poTipoComision = loLogicaNegocio.goConsultarTipoComision(cmbTipoComision.EditValue.ToString());
                        if (poTipoComision != null)
                        {
                            if (poTipoComision.AplicaPorcentajeGrupal)
                            {
                                txtPorcentajeComision.ReadOnly = true;
                                txtPorcentajeComision.Text = string.Empty;
                            }
                            else
                            {
                                txtPorcentajeComision.ReadOnly = false;
                                txtPorcentajeComision.Text = string.Empty;
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

        
        #endregion

        #region Métodos
        private void lCargarEventos()
        {
            //Validación-Eventos Datos Persona
            txtNumeroIdentificacion.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtNumeroIdentificacion.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtApellidoPaterno.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtApellidoPaterno.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtApellidoMaterno.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtApellidoMaterno.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtPrimerNombre.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtPrimerNombre.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtSegundoNombre.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtSegundoNombre.KeyPress += new KeyPressEventHandler(SoloLetras);
            txtNumeroRegistroProfesional.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtNumeroRegistroProfesional.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtPeso.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtPeso.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtEstatura.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtEstatura.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtNumeroLicencia.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtNumeroLicencia.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtPorcentajeDiscapacidad.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtPorcentajeDiscapacidad.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtCorreoPersonal.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtNombreCompleto.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbEstadoCivil.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbGenero.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtLugarNacimiento.KeyDown += new KeyEventHandler(EnterEqualTab);
            dtpFechaNacimiento.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbNivelEducacion.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbTitulo.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbColorPiel.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbColorOjos.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbTipoSangre.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbTipoLicencia.KeyDown += new KeyEventHandler(EnterEqualTab);
            dtpFechaExpiracionLicencia.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbReferenciaBiometrico.KeyDown += new KeyEventHandler(EnterEqualTab);

            //Validación-Eventos Datos Empleado
            txtCodigoRegionIess.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtCodigoRegionIess.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtNumeroSeguroSocial.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtNumeroSeguroSocial.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtCodigoSectorial.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtCodigoSectorial.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtValorArriendo.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtValorArriendo.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtNumeroLibretaMilitar.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtNumeroLibretaMilitar.KeyPress += new KeyPressEventHandler(SoloNumeros);

            cmbTipoEmpleado.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbTipoVivienda.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbTipoMaterialVivienda.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtCorreoLaboral.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtCuentaContable.KeyDown += new KeyEventHandler(EnterEqualTab);

            //Validación-Eventos Datos Empleado
            txtPorcentajeComision.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtPorcentajeComision.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtPorcentajeComisionAdicional.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtPorcentajeComisionAdicional.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtSueldo.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtSueldo.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtPorcentajePrimeraQuincena.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtPorcentajePrimeraQuincena.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtNumeroCuenta.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtNumeroCuenta.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtValorMovilizacion.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtValorMovilizacion.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtValorAlimentacion.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtValorAlimentacion.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            cmbSucursal.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbDepartamento.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbTipoContrato.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbCentroCosto.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbJefeInmediato.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbCargo.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbCargo.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbTipoComision.KeyDown += new KeyEventHandler(EnterEqualTab);
            dtpInicioHorarioLaboral.KeyDown += new KeyEventHandler(EnterEqualTab);
            dtpFinHorarioLaboral.KeyDown += new KeyEventHandler(EnterEqualTab);
            dtpFechaInicioContrato.KeyDown += new KeyEventHandler(EnterEqualTab);
            dtpFechaFinContrato.KeyDown += new KeyEventHandler(EnterEqualTab);
            dtpFechaIngresoMandato8.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbMotivoFinContrato.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbBanco.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbFormaPago.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbTipoCuentaBancaria.KeyDown += new KeyEventHandler(EnterEqualTab);
            chbAplicaAlimentacion.KeyDown += new KeyEventHandler(EnterEqualTab);
            chbAplicaMovilizacion.KeyDown += new KeyEventHandler(EnterEqualTab);
            chbAplicaHorasExtras.KeyDown += new KeyEventHandler(EnterEqualTab);
            chbAplicaIessConyugue.KeyDown += new KeyEventHandler(EnterEqualTab);
            chbAplicaHorasExtrasAntesLlegada.KeyDown += new KeyEventHandler(EnterEqualTab);
            chbAplicaTiempoGraciaPostSalida.KeyDown += new KeyEventHandler(EnterEqualTab);
            chbMostrarAsistencia.KeyDown += new KeyEventHandler(EnterEqualTab);
            chbAcumulaD3.KeyDown += new KeyEventHandler(EnterEqualTab);
            chbAcumulaD3.KeyDown += new KeyEventHandler(EnterEqualTab);
            chbEsJefe.KeyDown += new KeyEventHandler(EnterEqualTab);
            chbEsJubilado.KeyDown += new KeyEventHandler(EnterEqualTab);


            dtpFechaFinContrato.ValueChanged += new EventHandler(FechaValue_Changed);
            dtpFechaIngresoMandato8.ValueChanged += new EventHandler(FechaValue_Changed);
            dtpFechaExpiracionLicencia.ValueChanged += new EventHandler(FechaValue_Changed);
        }

        private void lLimpiar(bool tbLimpiarIdentificacion = true)
        {

            ofdFoto.FileName = "";
            pctFoto.Image = null;
            lblFoto.Text = string.Empty;
            lblFoto.Tag = string.Empty;
            lblIdPersona.Text = string.Empty;
            lblIdContrato.Text = string.Empty;
            if(tbLimpiarIdentificacion) txtNumeroIdentificacion.Text = string.Empty;
            txtCodigoRegionIess.Text = string.Empty;
            txtApellidoPaterno.Text = string.Empty;
            txtApellidoMaterno.Text = string.Empty;
            txtPrimerNombre.Text = string.Empty;
            txtSegundoNombre.Text = string.Empty;
            txtNombreCompleto.Text = string.Empty;
            txtCorreoPersonal.Text = string.Empty;
            txtLugarNacimiento.Text = string.Empty;
            txtNumeroRegistroProfesional.Text = string.Empty;
            txtPeso.Text = string.Empty;
            txtEstatura.Text = string.Empty;
            cmbColorOjos.Text = string.Empty;
            txtNumeroLicencia.Text = string.Empty;
            txtCorreoLaboral.Text = string.Empty;
            txtCuentaContable.Text = string.Empty;
            txtNumeroSeguroSocial.Text = string.Empty;
            txtCodigoSectorial.Text = string.Empty;
            txtValorArriendo.Text = string.Empty;
            txtNumeroLibretaMilitar.Text = string.Empty;
            txtPorcentajeComision.Text = string.Empty;
            txtPorcentajeComisionAdicional.Text = string.Empty;
            txtSueldo.Text = string.Empty;
            txtPorcentajePrimeraQuincena.Text = string.Empty;
            txtNumeroCuenta.Text = string.Empty;
            txtValorAlimentacion.Text = string.Empty;
            txtValorMovilizacion.Text = string.Empty;
            txtPorcentajeDiscapacidad.Text = string.Empty;
            txtTitulo.Text = string.Empty;

            if ((cmbEstadoCivil.Properties.DataSource as IList).Count > 0) cmbEstadoCivil.ItemIndex = 0;
            if ((cmbGenero.Properties.DataSource as IList).Count > 0) cmbGenero.ItemIndex = 0;
            if ((cmbNivelEducacion.Properties.DataSource as IList).Count > 0) cmbNivelEducacion.ItemIndex = 0;
            if ((cmbTitulo.Properties.DataSource as IList).Count > 0) cmbTitulo.ItemIndex = 0;
            if ((cmbColorPiel.Properties.DataSource as IList).Count > 0) cmbColorPiel.ItemIndex = 0;
            if ((cmbColorOjos.Properties.DataSource as IList).Count > 0) cmbColorOjos.ItemIndex = 0;
            if ((cmbTipoSangre.Properties.DataSource as IList).Count > 0) cmbTipoSangre.ItemIndex = 0;
            if ((cmbTipoLicencia.Properties.DataSource as IList).Count > 0) cmbTipoLicencia.ItemIndex = 0;
            if ((cmbTipoEmpleado.Properties.DataSource as IList).Count > 0) cmbTipoEmpleado.ItemIndex = 0;
            if ((cmbTipoVivienda.Properties.DataSource as IList).Count > 0) cmbTipoVivienda.ItemIndex = 0;
            if ((cmbCaracteristicasVivienda.Properties.DataSource as IList).Count > 0) cmbCaracteristicasVivienda.ItemIndex = 0;
            if ((cmbTipoMaterialVivienda.Properties.DataSource as IList).Count > 0) cmbTipoMaterialVivienda.ItemIndex = 0;
            if ((cmbSucursal.Properties.DataSource as IList).Count > 0) cmbSucursal.ItemIndex = 0;
            if ((cmbDepartamento.Properties.DataSource as IList).Count > 0) cmbDepartamento.ItemIndex = 0;
            if ((cmbTipoContrato.Properties.DataSource as IList).Count > 0) cmbTipoContrato.ItemIndex = 0;
            if ((cmbCentroCosto.Properties.DataSource as IList).Count > 0) cmbCentroCosto.ItemIndex = 0;
            if ((cmbJefeInmediato.Properties.DataSource as IList).Count > 0) cmbJefeInmediato.ItemIndex = 0;
            if ((cmbCargo.Properties.DataSource as IList).Count > 0) cmbCargo.ItemIndex = 0;
            if ((cmbTipoComision.Properties.DataSource as IList).Count > 0) cmbTipoComision.ItemIndex = 0;
            if ((cmbMotivoFinContrato.Properties.DataSource as IList).Count > 0) cmbMotivoFinContrato.ItemIndex = 0;
            if ((cmbBanco.Properties.DataSource as IList).Count > 0) cmbBanco.ItemIndex = 0;
            if ((cmbFormaPago.Properties.DataSource as IList).Count > 0) cmbFormaPago.ItemIndex = 0;
            if ((cmbTipoCuentaBancaria.Properties.DataSource as IList).Count > 0) cmbTipoCuentaBancaria.ItemIndex = 0;
            if ((cmbEstadoEmpleado.Properties.DataSource as IList).Count > 0) cmbEstadoEmpleado.ItemIndex = 0;
            if ((cmbEstadoContrato.Properties.DataSource as IList).Count > 0) cmbEstadoContrato.ItemIndex = 0;
            if ((cmbRegion.Properties.DataSource as IList).Count > 0) cmbRegion.ItemIndex = 0;
            if ((cmbTipoDiscapacidad.Properties.DataSource as IList).Count > 0) cmbTipoDiscapacidad.ItemIndex = 0;
            if ((cmbReferenciaBiometrico.Properties.DataSource as IList).Count > 0) cmbReferenciaBiometrico.ItemIndex = 0;



            chbAplicaAlimentacion.Checked = false;
            chbAplicaMovilizacion.Checked = false;
            chbAplicaHorasExtras.Checked = false;
            chbAplicaIessConyugue.Checked = false;
            chbAplicaHorasExtrasAntesLlegada.Checked = false;
            chbAplicaTiempoGraciaPostSalida.Checked = false;
            chbMostrarAsistencia.Checked = true;
            chbCalculaIRComisiones.Checked = true;
            chbAcumulaD3.Checked = false;
            chbAcumulaD4.Checked = false;
            chbEsJefe.Checked = false;
            chbEsJubilado.Checked = false;
            chbDescuentaIR.Checked = true;
            chbDerechoFondoReserva.Checked = false;
            chbSolicitudFondoReserva.Checked = false;
            dtpFechaNacimiento.Value = DateTime.Now;
            dtpFechaInicioContrato.Value = DateTime.Now;

            clsComun.gResetDtpCheck(ref dtpFechaExpiracionLicencia);
            clsComun.gResetDtpCheck(ref dtpFechaFinContrato);
            clsComun.gResetDtpCheck(ref dtpFechaIngresoMandato8);

            bsDireccion.DataSource = new List<PersonaDireccion>();
            bsCargaFamiliar.DataSource = new List<EmpleadoCargaFamiliar>();
            bsContactos.DataSource = new List<EmpleadoContacto>();
            bsDocumentos.DataSource = new List<EmpleadoDocumento>();

            gcBitacoraPersona.DataSource = null;
            gcBitacoraEmpleado.DataSource = null;
            gcBitacoraContrato.DataSource = null;


            var poLIsta = new List<EmpleadoContratoCuentaBancaria>();
            bsDatosCuentaBancaria.DataSource = poLIsta;
            gcCuentasBancarias.DataSource = bsDatosCuentaBancaria;

            bsDescuentos.DataSource = new List<EmpleadoDescuentoPrestamo>();
            gcDescuentos.DataSource = bsDescuentos;

            bsHistorialPersonalizado.DataSource = new List<PersonaHistorial>();
            gcHistorial.DataSource = bsHistorialPersonalizado;

            bsCapacitaciones.DataSource = new List<PersonaCapacitacion>();
            gcCapacitaciones.DataSource = bsCapacitaciones;

            bsEstudios.DataSource = new List<PersonaEducacion>();
            gcEstudios.DataSource = bsEstudios;

            bsCargaFamiliar.DataSource = new List<EmpleadoCargaFamiliar>();
            gcCargaFamiliar.DataSource = bsCargaFamiliar;

            lblEstadoPrincipal.Text = "";
            chbCrearNuevoContrato.Checked = false;
            btnCrearNuevoContrato.Enabled = false;

            txtDireccion.Text = "";
            txtTelefonoConvencional.Text = "";
            txtTelefonoCelular.Text = "";
            txtContactoEmergencia.Text = "";
            if ((cmbProvincia.Properties.DataSource as IList).Count > 0) cmbProvincia.ItemIndex = 0;
            if ((cmbCiudad.Properties.DataSource as IList).Count > 0) cmbCiudad.ItemIndex = 0;
            if ((cmbParroquia.Properties.DataSource as IList).Count > 0) cmbParroquia.ItemIndex = 0;

            btnInactivarJubilado.Visible = false;
            chbTieneEnfermedadCatastrofica.Checked = false;
        }

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


            bsDatosCuentaBancaria = new BindingSource();
            bsDatosCuentaBancaria.DataSource = new List<EmpleadoContratoCuentaBancaria>();
            gcCuentasBancarias.DataSource = bsDatosCuentaBancaria;

            dgvCuentasBancarias.OptionsBehavior.Editable = true;
            dgvCuentasBancarias.Columns["IdEmpleadoContratoCuenta"].Visible = false;
            dgvCuentasBancarias.Columns["IdEmpleadoContrato"].Visible = false;
            dgvCuentasBancarias.Columns["CodigoEstado"].Visible = false;

            clsComun.gDibujarBotonGrid(rpiBtnDelFP, dgvCuentasBancarias.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);

            dgvCuentasBancarias.Columns["CodigoTipoRol"].Caption = "Tipo Rol";
            dgvCuentasBancarias.Columns["CodigoBanco"].Caption = "Banco";
            dgvCuentasBancarias.Columns["CodigoTipoCuentaBancaria"].Caption = "Tipo Cuenta";
            dgvCuentasBancarias.Columns["CodigoFormaPago"].Caption = "Forma Pago";

            bsDescuentos.DataSource = new List<EmpleadoDescuentoPrestamo>();
            gcDescuentos.DataSource = bsDescuentos;
            dgvDescuentos.OptionsBehavior.Editable = true;

            bsHistorialPersonalizado.DataSource = new List<PersonaHistorial>();
            gcHistorial.DataSource = bsHistorialPersonalizado;

            clsComun.gDibujarBotonGrid(rpiBtnDelDes, dgvDescuentos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);

            dgvDescuentos.Columns["CodigoTipoRol"].Caption = "Tipo Rol";
            dgvDescuentos.Columns["AplicaDescuentoRol"].Caption = "Aplicar en Rol";
            dgvDescuentos.Columns["CodigoRubro"].Caption = "Rubro";

            bsDocumentos = new BindingSource();
            bsDocumentos.DataSource = new List<EmpleadoDocumento>();
            gcDocumentos.DataSource = bsDocumentos;

            dgvDocumentos.OptionsBehavior.Editable = true;
            dgvDocumentos.Columns["IdEmpleadoDocumento"].Visible = false;
            dgvDocumentos.Columns["RutaDestino"].Visible = false;
            dgvDocumentos.Columns["RutaOrigen"].Visible = false;
            dgvDocumentos.Columns["CodigoEstado"].Visible = false;
            dgvDocumentos.Columns["IdPersona"].Visible = false;
            dgvDocumentos.Columns["Cargar"].Visible = false;
            dgvDocumentos.Columns["Empleado"].Visible = false;
            dgvDocumentos.Columns["ArchivoAdjunto"].Visible = false;
            dgvDocumentos.Columns["Descargar"].Visible = false;
            
            dgvDocumentos.Columns["NombreArchivo"].OptionsColumn.AllowEdit = false;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDocumentos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);
            clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvDocumentos.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16, 50);
            clsComun.gDibujarBotonGrid(rpiBtnDownload, dgvDocumentos.Columns["Descargar"], "Descargar", Diccionario.ButtonGridImage.download_16x16, 50);
            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDocumentos.Columns["Visualizar"], "Visualizar", Diccionario.ButtonGridImage.show_16x16, 50);


            clsComun.gDibujarBotonGrid(rpiBtnDelHis, dgvHistorial.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 35);

            //bsHistorial.DataSource = new List<PersonaHistorial>();
            //gcHistorial.DataSource = bsHistorial;

            dgvHistorial.Columns["Banco"].Visible = false;
            dgvHistorial.Columns["TipoCuentaBancaria"].Visible = false;
            dgvHistorial.Columns["NumeroCuenta"].Visible = false;


            bsEstudios.DataSource = new List<PersonaEducacion>();
            gcEstudios.DataSource = bsEstudios;
            dgvEstudios.Columns["IdPersonaEducacion"].Visible = false;
            dgvEstudios.Columns["CodigoEstado"].Visible = false;
            dgvEstudios.Columns["IdPersona"].Visible = false;
            dgvEstudios.Columns["CodigoTipoEducacion"].Caption = "Tipo Educación";
            clsComun.gDibujarBotonGrid(rpiBtnDelEdu, dgvEstudios.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);

            bsCapacitaciones.DataSource = new List<PersonaCapacitacion>();
            gcCapacitaciones.DataSource = bsCapacitaciones;
            dgvCapacitaciones.Columns["IdPersonaCapacitacion"].Visible = false;
            dgvCapacitaciones.Columns["CodigoEstado"].Visible = false;
            dgvCapacitaciones.Columns["IdPersona"].Visible = false;
            dgvCapacitaciones.Columns["Fecha"].Visible = false;
            dgvCapacitaciones.Columns["Anio"].Caption = "Año";
            clsComun.gDibujarBotonGrid(rpiBtnDelCap, dgvCapacitaciones.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);

            bsCargaFamiliar.DataSource = new List<EmpleadoCargaFamiliar>();
            gcCargaFamiliar.DataSource = bsCargaFamiliar;
            dgvCargaFamiliar.Columns["IdEmpleadoCargaFamiliar"].Visible = false;
            dgvCargaFamiliar.Columns["CodigoEstado"].Visible = false;
            dgvCargaFamiliar.Columns["IdPersona"].Visible = false;
            dgvCargaFamiliar.Columns["Empleado"].Visible = false;
            dgvCargaFamiliar.Columns["Adjunto"].Visible = false;
            dgvCargaFamiliar.Columns["CodigoTipoCargaFamiliar"].Caption = "Parentezco";
            dgvCargaFamiliar.Columns["CodigoTipoGenero"].Caption = "Genero";
            dgvCargaFamiliar.Columns["NumeroIdentificacion"].Caption = "Identificación";
            dgvCargaFamiliar.Columns["NombreCompleto"].Caption = "Nombre";
            dgvCargaFamiliar.Columns["AplicaCargaFamiliar"].Caption = "Carga Familiar Utilidades";
            dgvCargaFamiliar.Columns["Fecha"].Caption = "Fecha Nacimiento o Matrimonio";
            dgvCargaFamiliar.Columns["Fecha"].Width = 150;
            clsComun.gDibujarBotonGrid(rpiBtnDelCarFam, dgvCargaFamiliar.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);

        }

        private bool lbEsValido(bool tbValidaDatosContrato)
        {
            
            if (string.IsNullOrEmpty(txtNumeroIdentificacion.Text.Trim()))
            {
                XtraMessageBox.Show("Número de Identificación no puede estar vacío.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            try
            {
                if (!clsComun.gVerificaIdentificacion(txtNumeroIdentificacion.Text.Trim()))
                {
                    XtraMessageBox.Show("Número de Identificación no tiene un formato válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            catch (Exception)
            {
                XtraMessageBox.Show("Número de Identificación inválido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            
            //if (loLogicaNegocio.gbExisteIdentificacion(txtNumeroIdentificacion.Text.Trim()))
            //{
            //    XtraMessageBox.Show("Número de Identificación ya ingresado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}
            

            if (string.IsNullOrEmpty(txtApellidoPaterno.Text.Trim()))
            {
                XtraMessageBox.Show("Apellido Paterno no puede estar vacío.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtPrimerNombre.Text.Trim()))
            {
                XtraMessageBox.Show("Primer Nombre no puede estar vacío.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            
            if (cmbEstadoCivil.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Estado Civil.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (cmbGenero.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Género.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (dtpFechaNacimiento.Value.Date == DateTime.Now.Date)
            {
                XtraMessageBox.Show("Ingrese Fecha de Nacimiento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtCodigoRegionIess.Text.Trim()))
            {
                XtraMessageBox.Show("Código Región Iess no puede estar vacío.", "Datos Empleado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //if (cmbTipoEmpleado.EditValue.ToString() == Diccionario.Seleccione)
            //{
            //    XtraMessageBox.Show("Seleccione Tipo de Empleado.", "Datos Empleado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}

            if (cmbRegion.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Región.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (tbValidaDatosContrato)
            {

                if (cmbSucursal.EditValue.ToString() == Diccionario.Seleccione)
                {
                    XtraMessageBox.Show("Seleccione Sucursal.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (cmbDepartamento.EditValue.ToString() == Diccionario.Seleccione)
                {
                    XtraMessageBox.Show("Seleccione Departamento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (cmbTipoContrato.EditValue.ToString() == Diccionario.Seleccione)
                {
                    XtraMessageBox.Show("Seleccione Tipo de Contrato.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (cmbCentroCosto.EditValue.ToString() == Diccionario.Seleccione)
                {
                    XtraMessageBox.Show("Seleccione Centro de Costo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (cmbCargo.EditValue.ToString() == Diccionario.Seleccione)
                {
                    XtraMessageBox.Show("Seleccione Cargo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }


                if (cmbBanco.EditValue.ToString() == Diccionario.Seleccione)
                {
                    XtraMessageBox.Show("Seleccione Banco.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (cmbFormaPago.EditValue.ToString() == Diccionario.Seleccione)
                {
                    XtraMessageBox.Show("Seleccione Forma de Pago.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (string.IsNullOrEmpty(txtSueldo.Text.Trim()))
                {
                    XtraMessageBox.Show("Sueldo no puede estar vacío.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (string.IsNullOrEmpty(txtPorcentajePrimeraQuincena.Text.Trim()))
                {
                    XtraMessageBox.Show("% Primera Quincena no puede estar vacía.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

            }


            //if (dtpFechaInicioContrato.Value.Date == DateTime.Now.Date)
            //{
            //    XtraMessageBox.Show("Ingrese Fecha Inicio de Contrato.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}

            //if (cmbEstadoContrato.EditValue.ToString() == Diccionario.Inactivo)
            //{
            //    if (!dtpFechaFinContrato.Checked)
            //    {
            //        XtraMessageBox.Show("Ingrese Fecha Fin de Contrato para Inactivar el mismo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return false;
            //    }
            //    else
            //    {
            //        if (cmbMotivoFinContrato.EditValue.ToString() == Diccionario.Seleccione)
            //        {
            //            XtraMessageBox.Show("Seleccione Motivo Fin de Contrato.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            return false;
            //        }
            //    }
            //}

            if (!string.IsNullOrEmpty(txtCorreoLaboral.Text.Trim()))
            {
                if (!txtCorreoLaboral.Text.Trim().ToLower().Contains("@afecor.com"))
                {
                    XtraMessageBox.Show("No es posible guardar un correo laboral que no tenga como dominio '@afecor.com'.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

            }

            if (dtpFechaFinContrato.Checked)
            {
                if (cmbMotivoFinContrato.EditValue.ToString() == Diccionario.Seleccione)
                {
                    XtraMessageBox.Show("Seleccione Motivo Fin de Contrato.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (dtpFechaFinContrato.Checked)
            {
                if (dtpFechaInicioContrato.Value.Date > dtpFechaFinContrato.Value.Date)
                {
                    XtraMessageBox.Show("Fecha de inicio de contrato no puede ser mayor que la fecha fin de contrato.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            List<PersonaDireccion> poDireccion = (List<PersonaDireccion>)bsDireccion.DataSource;
            if(poDireccion != null && poDireccion.Count() > 0)
            {
                int piFilas = poDireccion.Count();
                List<int> piIndexRemove = new List<int>();
                for (int i = 0; i < piFilas; i++)
                {
                    if (poDireccion[i].IdPais == 0 && poDireccion[i].IdProvincia == 0 && poDireccion[i].IdCanton == null 
                        && string.IsNullOrEmpty(poDireccion[i].direccion) && string.IsNullOrEmpty(poDireccion[i].Referencia) 
                        && string.IsNullOrEmpty(poDireccion[i].TelfonoCelular) && string.IsNullOrEmpty(poDireccion[i].TelfonoConvencional) 
                        && string.IsNullOrEmpty(poDireccion[i].CodigoEstado))
                    {
                        piIndexRemove.Add(i);
                    }
                }
          
                foreach(var piItem in piIndexRemove.OrderByDescending(x=>x))
                {
                    poDireccion.RemoveAt(piItem);
                }

                if (poDireccion.Where(x => x.IdPais == 0).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'País' Obligatorio, Tab Dirección", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (poDireccion.Where(x => x.IdProvincia == 0).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Provincia' Obligatorio, Tab Dirección", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (poDireccion.Where(x => string.IsNullOrEmpty(x.direccion)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Dirección' Obligatorio, Tab Dirección", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (poDireccion.Where(x => string.IsNullOrEmpty(x.CodigoEstado)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Estado' Obligatorio, Tab Dirección", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (poDireccion.Where(x=>x.Principal).Count() != 1 )
                {
                    XtraMessageBox.Show("Debe seleccionar solo un registro de dirección como principal.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            List<EmpleadoCargaFamiliar> poCargaFamiliar = (List<EmpleadoCargaFamiliar>)bsCargaFamiliar.DataSource;
            if (poCargaFamiliar != null && poCargaFamiliar.Count() > 0)
            {
                int piFilas = poCargaFamiliar.Count();
                List<int> piIndexRemove = new List<int>();
                for (int i = 0; i < piFilas; i++)
                {
                    if (poCargaFamiliar[i].Fecha.Date == DateTime.MinValue.Date && string.IsNullOrEmpty(poCargaFamiliar[i].CodigoTipoCargaFamiliar) 
                        && string.IsNullOrEmpty(poCargaFamiliar[i].NumeroIdentificacion) 
                        && string.IsNullOrEmpty(poCargaFamiliar[i].NombreCompleto) 
                        && string.IsNullOrEmpty(poCargaFamiliar[i].CodigoTipoGenero) 
                        && !poCargaFamiliar[i].Discapacitado 
                        && string.IsNullOrEmpty(poCargaFamiliar[i].CodigoEstado))
                    {
                        piIndexRemove.Add(i);
                    }
                }

                foreach (var piItem in piIndexRemove.OrderByDescending(x => x))
                {
                    poCargaFamiliar.RemoveAt(piItem);
                }

                if (poCargaFamiliar.Where(x => string.IsNullOrEmpty(x.CodigoTipoCargaFamiliar)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Tipo' Obligatorio, Tab Cargas Familiares", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                //if (poCargaFamiliar.Where(x => string.IsNullOrEmpty(x.NumeroIdentificacion)).Count() > 0)
                //{
                //    XtraMessageBox.Show("Dato 'Identificación' Obligatorio, Tab Cargas Familiares", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return false;
                //}

                if (poCargaFamiliar.Where(x => string.IsNullOrEmpty(x.NombreCompleto)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Nombre' Obligatorio, Tab Cargas Familiares", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (poCargaFamiliar.Where(x => x.Fecha == null).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Fecha' Obligatorio, Tab Cargas Familiares", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (poCargaFamiliar.Where(x => string.IsNullOrEmpty(x.CodigoTipoGenero)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Género' Obligatorio, Tab Cargas Familiares", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (poCargaFamiliar.Where(x => string.IsNullOrEmpty(x.CodigoEstado)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Estado' Obligatorio, Tab Cargas Familiares", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            List<EmpleadoContacto> poContacto = (List<EmpleadoContacto>)bsContactos.DataSource;
            if (poContacto != null && poContacto.Count() > 0)
            {
                int piFilas = poContacto.Count();
                List<int> piIndexRemove = new List<int>();
                for (int i = 0; i < piFilas; i++)
                {
                    if (string.IsNullOrEmpty(poContacto[i].CodigoTipoContacto) && string.IsNullOrEmpty(poContacto[i].CodigoParentezco) 
                        && string.IsNullOrEmpty(poContacto[i].NombreCompleto) && string.IsNullOrEmpty(poContacto[i].Direccion)
                        && string.IsNullOrEmpty(poContacto[i].TelefonoConvencional) && string.IsNullOrEmpty(poContacto[i].TelefonoCelular) 
                        && string.IsNullOrEmpty(poContacto[i].Correo) && string.IsNullOrEmpty(poContacto[i].Observacion) 
                        && string.IsNullOrEmpty(poContacto[i].CodigoEstado))
                    {
                        piIndexRemove.Add(i);
                    }
                }

                foreach (var piItem in piIndexRemove.OrderByDescending(x => x))
                {
                    poContacto.RemoveAt(piItem);
                }

                if (poContacto.Where(x => string.IsNullOrEmpty(x.CodigoTipoContacto)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Tipo Contacto' Obligatorio, Tab Contactos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (poContacto.Where(x => string.IsNullOrEmpty(x.CodigoParentezco)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Parentezco' Obligatorio, Tab Contactos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (poContacto.Where(x => string.IsNullOrEmpty(x.NombreCompleto)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Nombre' Obligatorio, Tab Contactos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (poContacto.Where(x => string.IsNullOrEmpty(x.Direccion)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Dirección' Obligatorio, Tab Contactos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (poContacto.Where(x => string.IsNullOrEmpty(x.TelefonoConvencional)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Convencional' Obligatorio, Tab Contactos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (poContacto.Where(x => string.IsNullOrEmpty(x.TelefonoCelular)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Celular' Obligatorio, Tab Contactos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (poContacto.Where(x => string.IsNullOrEmpty(x.CodigoEstado)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Estado' Obligatorio, Tab Contactos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            List<EmpleadoDocumento> poDocumentos = (List<EmpleadoDocumento>)bsDocumentos.DataSource;
            if (poDocumentos != null && poDocumentos.Count() > 0)
            {
                int piFilas = poDocumentos.Count();
                List<int> piIndexRemove = new List<int>();
                for (int i = 0; i < piFilas; i++)
                {
                    if (string.IsNullOrEmpty(poDocumentos[i].Descripcion) && string.IsNullOrEmpty(poDocumentos[i].CodigoEstado) 
                        && string.IsNullOrEmpty(poDocumentos[i].Cargar) && string.IsNullOrEmpty(poDocumentos[i].Descargar))
                       
                    {
                        piIndexRemove.Add(i);
                    }
                }

                foreach (var piItem in piIndexRemove.OrderByDescending(x => x))
                {
                    poDocumentos.RemoveAt(piItem);
                }
                if (poDocumentos.Where(x => string.IsNullOrEmpty(x.Descripcion)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Descripción' Obligatorio, Tab Documentos Adjuntos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (poDocumentos.Where(x => string.IsNullOrEmpty(x.CodigoEstado)).Count() > 0)
                {
                    XtraMessageBox.Show("Dato 'Estado' Obligatorio, Tab Documentos Adjuntos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (poDocumentos.Where(x => string.IsNullOrEmpty(x.Cargar)).Count() > 0)
                {
                    XtraMessageBox.Show("Debe Adjuntar Archvo, Dato 'Cargar' Obligatorio, Tab Contactos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (cmbTipoComision.EditValue.ToString() != Diccionario.Seleccione)
            {
                if (string.IsNullOrEmpty(txtPorcentajeComision.Text.Trim()))
                {

                    DialogResult dialogResult = XtraMessageBox.Show("No tiene definido el porcentaje de comisión", "¿Desea Continuar?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult != DialogResult.Yes)
                    {
                        return false;
                    }

                }
            }

            dgvDescuentos.PostEditor();
            List<EmpleadoDescuentoPrestamo> poLista = (List<EmpleadoDescuentoPrestamo>)bsDescuentos.DataSource;

            var poListaRecorrida = new List<EmpleadoDescuentoPrestamo>();
            int fila = 1;
            foreach (var item in poLista)
            {
                string psCliente = loComboTipoRol.Where(x => x.Codigo.Contains(item.CodigoTipoRol)).Select(x => x.Descripcion).FirstOrDefault();
                string psZona = loComboRubro.Where(x => x.Codigo.Contains(item.CodigoRubro)).Select(x => x.Descripcion).FirstOrDefault();

                if (item.CodigoTipoRol == Diccionario.Seleccione || item.CodigoRubro == Diccionario.Seleccione)
                {
                    XtraMessageBox.Show(string.Format("Verificar si están seleccionados el Tipo de Rol y el Rubro en la Fila # {0}.", fila), tbDescuentos.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                else if (poListaRecorrida.Where(x => x.CodigoTipoRol == item.CodigoTipoRol && x.CodigoRubro == item.CodigoRubro).Count() > 0)
                {
                    XtraMessageBox.Show(string.Format("Eliminar Fila # {0}, Ya existe una parametrización por: {1} y {2}.", fila, psCliente, psZona), tbDescuentos.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                poListaRecorrida.Add(item);
                fila++;
            }

            return true;
        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNumeroIdentificacion.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscar(txtNumeroIdentificacion.Text.Trim());
                if (poObject != null  && poObject.IdPersona != 0)
                {
                    lLimpiar();

                    pbCargar = false;

                    lblIdPersona.Text = poObject.IdPersona.ToString();
                    lblFoto.Text = ConfigurationManager.AppSettings["CarpetaCompartidaFoto"].ToString() + poObject.RutaImagen;
                    lblFoto.Tag = poObject.RutaImagen;
                    if (!string.IsNullOrEmpty(lblFoto.Text) && File.Exists(lblFoto.Text))
                    {
                        ofdFoto.FileName = lblFoto.Text;
                        lCargaFoto();
                    }
                    txtNumeroIdentificacion.Text = poObject.NumeroIdentificacion;
                    txtApellidoPaterno.Text = poObject.ApellidoPaterno;
                    txtApellidoMaterno.Text = poObject.ApellidoMaterno;
                    txtPrimerNombre.Text = poObject.PrimerNombre;
                    txtSegundoNombre.Text = poObject.SegundoNombre;
                    txtNombreCompleto.Text = poObject.NombreCompleto;
                    txtCorreoPersonal.Text = poObject.Correo;
                    cmbEstadoCivil.EditValue = poObject.CodigoEstadoCivil;
                    cmbGenero.EditValue = poObject.CodigoGenero;
                    txtLugarNacimiento.Text = poObject.LugarNacimiento;
                    dtpFechaNacimiento.Value = poObject.FechaNacimiento;
                    if(poObject.CodigoNivelEducacion != null) cmbNivelEducacion.EditValue = poObject.CodigoNivelEducacion;
                    if (poObject.CodigoTitulo != null) cmbTitulo.EditValue = poObject.CodigoTitulo;
                    if (poObject.Titulo != null) txtTitulo.Text = poObject.Titulo;
                    txtNumeroRegistroProfesional.Text = poObject.NumeroRegistroProfesional;
                    if (poObject.Peso != null) txtPeso.Text = poObject.Peso.ToString();
                    if (poObject.Estatura != null) txtEstatura.Text = poObject.Estatura.ToString();
                    if (poObject.CodigoColorPiel != null) cmbColorPiel.EditValue = poObject.CodigoColorPiel;
                    if (poObject.CodigoColorOjos != null) cmbColorOjos.EditValue = poObject.CodigoColorOjos;
                    if (poObject.CodigoTipoSangre != null) cmbTipoSangre.EditValue = poObject.CodigoTipoSangre;
                    if (poObject.CodigoTipoLicencia != null) cmbTipoLicencia.EditValue = poObject.CodigoTipoLicencia;
                    if (poObject.CodigoTipoLicencia != null) cmbTipoLicencia.EditValue = poObject.CodigoTipoLicencia;
                    if (poObject.CodigoRegion != null) cmbRegion.EditValue = poObject.CodigoRegion;
                    if (poObject.CodigoTipoDiscapacidad != null) cmbTipoDiscapacidad.EditValue = poObject.CodigoTipoDiscapacidad;
                    if (poObject.IdBiometrico != null) cmbReferenciaBiometrico.EditValue = poObject.IdBiometrico.ToString();
                    if (poObject.PorcentajeDiscapacidad != null) txtPorcentajeDiscapacidad.Text = poObject.PorcentajeDiscapacidad.ToString();
                    if (poObject.FechaExpiracionLicencia != null)
                    {
                        dtpFechaExpiracionLicencia.Checked = true;
                        dtpFechaExpiracionLicencia.Value = poObject.FechaExpiracionLicencia??DateTime.Now;
                    }
                    chbTieneEnfermedadCatastrofica.Checked = poObject.EnfermedadCatastrofica;
                    txtDireccion.Text = poObject.Direccion;
                    txtTelefonoConvencional.Text = poObject.TelefonoConvencional;
                    txtTelefonoCelular.Text = poObject.TelefonoCelular;
                    txtContactoEmergencia.Text = poObject.ContactoCasoEmergencia;
                    txtTelefonoContactoCasoEmergencia.Text = poObject.TelefonoCasoEmergencia;
                    pbCargar = true;
                    if (poObject.IdProvincia != null) cmbProvincia.EditValue = poObject.IdProvincia.ToString();
                    if (poObject.IdCanton != null) cmbCiudad.EditValue = poObject.IdCanton.ToString();
                    if (poObject.IdParroquia != null) cmbParroquia.EditValue = poObject.IdParroquia.ToString();
                    pbCargar = false;

                    txtNumeroLicencia.Text = poObject.NumeroLicencia;
                    bsDireccion.DataSource = poObject.PersonaDireccion;
                    bsCapacitaciones.DataSource = poObject.PersonaCapacitacion;
                    bsEstudios.DataSource = poObject.PersonaEducacion;

                    chbDescuentaIR.Checked = poObject.DescuentaIR;


                    txtCodigoRegionIess.Text = poObject.Empleado.CodigoRegionIess;
                    cmbTipoEmpleado.EditValue = poObject.Empleado.CodigoTipoEmpleado;
                    cmbEstadoEmpleado.EditValue = poObject.Empleado.CodigoEstado;
                    txtCorreoLaboral.Text = poObject.Empleado.Correo;
                    txtCuentaContable.Text = poObject.Empleado.CuentaContable;
                    txtNumeroSeguroSocial.Text = poObject.Empleado.NumeroSeguroSocial;
                    txtCodigoSectorial.Text = poObject.Empleado.CodigoSectorial;
                    if (poObject.Empleado.CodigoTipoVivienda != null) cmbTipoVivienda.EditValue = poObject.Empleado.CodigoTipoVivienda;
                    if (poObject.Empleado.CaracteristicasVivienda != null) cmbCaracteristicasVivienda.EditValue = poObject.Empleado.CaracteristicasVivienda;
                    if (poObject.Empleado.CodigoTipoMaterialVivienda != null) cmbTipoMaterialVivienda.EditValue = poObject.Empleado.CodigoTipoMaterialVivienda;
                    if (poObject.Empleado.ValorArriendo != null) txtValorArriendo.Text = poObject.Empleado.ValorArriendo.ToString();
                    if (poObject.Empleado.NumeroLibretaMilitar != null) txtNumeroLibretaMilitar.Text = poObject.Empleado.NumeroLibretaMilitar;
                    chbAplicaHorasExtrasAntesLlegada.Checked = poObject.Empleado.AplicaHorasExtrasAntesLlegada;
                    chbAplicaTiempoGraciaPostSalida.Checked = poObject.Empleado.AplicaTiempoGraciaPostSalida;
                    chbMostrarAsistencia.Checked = poObject.Empleado.MostrarEnAsistencia;
                    chbCalculaIRComisiones.Checked = poObject.Empleado.CalculaIRComisiones;

                    bsCargaFamiliar.DataSource = poObject.Empleado.EmpleadoCargaFamiliar;
                    bsContactos.DataSource = poObject.Empleado.EmpleadoContacto;
                    bsDocumentos.DataSource = poObject.Empleado.EmpleadoDocumento;

                    if (poObject.Empleado.EmpleadoContrato.Count > 0)
                    {
                        var poContrato = poObject.Empleado.EmpleadoContrato.Where(x=>x.CodigoEstado == Diccionario.Activo).FirstOrDefault();
                        if (poContrato == null)
                        {
                            poContrato = poObject.Empleado.EmpleadoContrato.FirstOrDefault();
                        }

                        lblEstadoPrincipal.Text = Diccionario.gsGetDescripcion(poContrato.CodigoEstado);

                        if (poContrato.CodigoEstado == Diccionario.Activo)
                        {
                            btnCrearNuevoContrato.Enabled = false;

                            if (poContrato.CodigoDepartamento == "JUB")
                            {
                                btnInactivarJubilado.Visible = true;
                            }
                        }
                        else
                        {
                            btnCrearNuevoContrato.Enabled = true;
                        }


                        lblIdContrato.Text = poContrato.IdEmpleadoContrato.ToString();
                        cmbEstadoContrato.EditValue = poContrato.CodigoEstado;
                        cmbSucursal.EditValue = poContrato.CodigoSucursal;
                        cmbDepartamento.EditValue = poContrato.CodigoDepartamento;
                        cmbTipoContrato.EditValue = poContrato.CodigoTipoContrato;
                        cmbCentroCosto.EditValue = poContrato.CodigoCentroCosto;
                        if (poContrato.IdPersonaJefe != null) cmbJefeInmediato.EditValue = poContrato.IdPersonaJefe.ToString();
                        cmbCargo.EditValue =  poContrato.CodigoCargoLaboral;
                        if (poContrato.CodigoTipoComision != null) cmbTipoComision.EditValue = poContrato.CodigoTipoComision;
                        if (poContrato.PorcentajeComision != null) txtPorcentajeComision.Text = poContrato.PorcentajeComision.ToString();
                        if (poContrato.PorcentajeComisionAdicional != null) txtPorcentajeComisionAdicional.Text = poContrato.PorcentajeComisionAdicional.ToString(); 
                        txtSueldo.Text = poContrato.Sueldo.ToString();
                        txtPorcentajePrimeraQuincena.Text = poContrato.PorcentajePQ.ToString();
                        if (poContrato.InicioHorarioLaboral != null)
                        {
                            var pdFecha = poContrato.InicioHorarioLaboral ?? DateTime.Now.TimeOfDay;
                            dtpInicioHorarioLaboral.Checked = true;
                            dtpInicioHorarioLaboral.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, pdFecha.Hours, pdFecha.Minutes, 0);
                        }
                        if (poContrato.FinHorarioLaboral != null)
                        {
                            var pdFecha = poContrato.FinHorarioLaboral ?? DateTime.Now.TimeOfDay;
                            dtpFinHorarioLaboral.Checked = true;
                            dtpFinHorarioLaboral.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, pdFecha.Hours, pdFecha.Minutes, 0);
                        }
                        if (poContrato.FechaFinContrato != null)
                        {
                            dtpFechaFinContrato.Checked = true;
                            dtpFechaFinContrato.Value = poContrato.FechaFinContrato ?? DateTime.Now;
                        }

                        if (poContrato.FechaIngresoMandato8 != null)
                        {
                            dtpFechaIngresoMandato8.Checked = true;
                            dtpFechaIngresoMandato8.Value = poContrato.FechaIngresoMandato8 ?? DateTime.Now;
                        }

                        dtpFechaInicioContrato.Value = poContrato.FechaInicioContrato;
                        if (poContrato.CodigoMotivoFinContrato != null) cmbMotivoFinContrato.EditValue = poContrato.CodigoMotivoFinContrato;
                        if (cmbMotivoFinContrato.EditValue.ToString() != Diccionario.Seleccione) poContrato.CodigoMotivoFinContrato = cmbMotivoFinContrato.EditValue.ToString(); else poContrato.CodigoMotivoFinContrato = null;
                        cmbBanco.EditValue = poContrato.CodigoBanco;
                        cmbFormaPago.EditValue = poContrato.CodigoFormaPago;
                        if (poContrato.CodigoTipoCuentaBancaria != null) cmbTipoCuentaBancaria.EditValue = poContrato.CodigoTipoCuentaBancaria;
                        txtNumeroCuenta.Text = poContrato.NumeroCuenta;
                        if (poContrato.ValorAlimentacion != null) txtValorAlimentacion.Text = poContrato.ValorAlimentacion.ToString();
                        if (poContrato.ValorMovilizacion != null) txtValorMovilizacion.Text = poContrato.ValorMovilizacion.ToString();

                        chbAplicaAlimentacion.Checked = poContrato.AplicaAlimentacion;
                        chbAplicaMovilizacion.Checked = poContrato.AplicaMovilizacion;
                        chbAplicaHorasExtras.Checked= poContrato.AplicaHorasExtras;
                        chbAplicaIessConyugue.Checked = poContrato.AplicaIessConyugue;
                        chbAcumulaD3.Checked = poContrato.AcumulaD3;
                        chbAcumulaD4.Checked = poContrato.AcumulaD4;
                        chbEsJefe.Checked = poContrato.EsJefe;
                        chbEsJubilado.Checked = poContrato.EsJubilado;
                        chbDerechoFondoReserva.Checked = poContrato.DerechoFondoReserva;
                        chbSolicitudFondoReserva.Checked = poContrato.SolicitudFondoReserva;

                        var poLIsta = poContrato.EmpleadoContratoCuentaBancaria;
                        bsDatosCuentaBancaria.DataSource = poLIsta;
                        gcCuentasBancarias.DataSource = bsDatosCuentaBancaria;

                        bsDescuentos.DataSource = loLogicaNegocio.goConsultarDespuestoPrestamo(poObject.IdPersona);
                        gcDescuentos.DataSource = bsDescuentos;


                    }
                    pbCargar = true;

                    lCargarLog(poObject.IdPersona);
                }
                else
                {
                    lLimpiar(false);
                }
            }
        }

        private void lCargaFoto()
        {
            try
            {
                if (!ofdFoto.FileName.Equals(""))
                {
                    pctFoto.Image = new Bitmap(ofdFoto.OpenFile());
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lCargarLog(int tIdPersona)
        {
            BindingSource bindingSourcePersona = new BindingSource(); 
            gcBitacoraPersona.DataSource = null;
            bindingSourcePersona.DataSource = null;
            DataTable dtPersona = loLogicaNegocio.goConsultaLogPersona(tIdPersona);
            bindingSourcePersona.DataSource = dtPersona;
            gcBitacoraPersona.DataSource = bindingSourcePersona;

            BindingSource bindingSourceEmpleado = new BindingSource();
            gcBitacoraEmpleado.DataSource = null;
            bindingSourceEmpleado.DataSource = null;
            DataTable dtEmpleado = loLogicaNegocio.goConsultaLogEmpleado(tIdPersona);
            bindingSourceEmpleado.DataSource = dtEmpleado;
            gcBitacoraEmpleado.DataSource = bindingSourceEmpleado;

            BindingSource bindingSourceEmpleadoContrato = new BindingSource();
            gcBitacoraContrato.DataSource = null;
            bindingSourceEmpleadoContrato.DataSource = null;
            DataTable dtEmpleadoContrato = loLogicaNegocio.goConsultaLogContrato(tIdPersona);
            bindingSourceEmpleadoContrato.DataSource = dtEmpleadoContrato;
            gcBitacoraContrato.DataSource = bindingSourceEmpleadoContrato;


            
            gcHistorial.DataSource = null;
            bsHistorialPersonalizado.DataSource = null;

            bsHistorialPersonalizado.DataSource = loLogicaNegocio.goConsultaHistorial(tIdPersona);
            gcHistorial.DataSource = bsHistorialPersonalizado;

            clsComun.gOrdenarColumnasGridFullEditable(dgvHistorial);

            for (int i = 0; i < dgvHistorial.Columns.Count; i++)
            {
                if (i == 1 || i == 2 || i == 3 || i == 4 || i == 5 || i == 6 || i == 8)
                {
                    dgvHistorial.Columns[i].OptionsColumn.ReadOnly = false;
                }
                else
                {
                    dgvHistorial.Columns[i].OptionsColumn.ReadOnly = true;
                }

                
            }


        }

        #endregion

        private void cmbTipoDiscapacidad_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbTipoDiscapacidad.EditValue != null)
                {
                    if (cmbTipoDiscapacidad.EditValue.ToString() != Diccionario.Seleccione)
                    {
                        txtPorcentajeDiscapacidad.Enabled = true;
                    }
                    else
                    {
                        txtPorcentajeDiscapacidad.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAñadirFilaFP_Click(object sender, EventArgs e)
        {
            try
            {
                bsDatosCuentaBancaria.AddNew();
                ((List<EmpleadoContratoCuentaBancaria>)bsDatosCuentaBancaria.DataSource).LastOrDefault().CodigoTipoRol = Diccionario.Seleccione;
                ((List<EmpleadoContratoCuentaBancaria>)bsDatosCuentaBancaria.DataSource).LastOrDefault().CodigoBanco = Diccionario.Seleccione;
                ((List<EmpleadoContratoCuentaBancaria>)bsDatosCuentaBancaria.DataSource).LastOrDefault().CodigoFormaPago = Diccionario.Seleccione;
                ((List<EmpleadoContratoCuentaBancaria>)bsDatosCuentaBancaria.DataSource).LastOrDefault().CodigoTipoCuentaBancaria = Diccionario.Seleccione;
                dgvCuentasBancarias.Focus();
                dgvCuentasBancarias.ShowEditor();
                dgvCuentasBancarias.UpdateCurrentRow();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chbCrearNuevoContrato_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbCrearNuevoContrato.Checked)
                {
                    clsComun.gResetDtpCheck(ref dtpFechaFinContrato);
                    if ((cmbMotivoFinContrato.Properties.DataSource as IList).Count > 0) cmbMotivoFinContrato.ItemIndex = 0;
                    cmbEstadoContrato.EditValue = Diccionario.Activo;
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCrearNuevoContrato_Click(object sender, EventArgs e)
        {
            try
            {
                
                clsComun.gResetDtpCheck(ref dtpFechaFinContrato);
                if ((cmbMotivoFinContrato.Properties.DataSource as IList).Count > 0) cmbMotivoFinContrato.ItemIndex = 0;
                cmbEstadoContrato.EditValue = Diccionario.Activo;
                chbSolicitudFondoReserva.Checked = false;
                chbDerechoFondoReserva.Checked = false;
                chbCrearNuevoContrato.Checked = true;
                btnCrearNuevoContrato.Enabled = false;
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFichaMedica_Click(object sender, EventArgs e)
        {
            try
            {
                string psForma = Diccionario.Tablas.Menu.frmPrFichaMedica;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                if (poForm != null)
                {
                    frmPrFichaMedica frm = new frmPrFichaMedica();
                    frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    frm.Text = poForm.Nombre;
                    frm.lIdPersona  = string.IsNullOrEmpty(lblIdPersona.Text) ? 0 : int.Parse(lblIdPersona.Text);
                    frm.ShowDialog();
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

        private void btnAddEstudios_Click(object sender, EventArgs e)
        {
            try
            {
                bsEstudios.AddNew();
                ((List<PersonaEducacion>)bsEstudios.DataSource).LastOrDefault().CodigoTipoEducacion = Diccionario.Seleccione;
                dgvEstudios.Focus();
                dgvEstudios.FocusedRowHandle = dgvEstudios.RowCount - 1;
                dgvEstudios.FocusedColumn = dgvEstudios.Columns[0];
                dgvEstudios.ShowEditor();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddCapacitaciones_Click(object sender, EventArgs e)
        {
            try
            {
                bsCapacitaciones.AddNew();
                dgvCapacitaciones.Focus();
                dgvCapacitaciones.FocusedRowHandle = dgvCapacitaciones.RowCount - 1;
                dgvCapacitaciones.FocusedColumn = dgvCapacitaciones.Columns[0];
                dgvCapacitaciones.ShowEditor();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInactivarJubilado_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNumeroIdentificacion.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de INACTIVAR JUBILADO?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var psMsg =  loLogicaNegocio.gsInactivarJubilado(txtNumeroIdentificacion.Text.Trim(), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show("Jubilado ha sido Inactivado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbProvincia_EditValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (pbCargar)
                {
                    clsComun.gLLenarCombo(ref cmbCiudad, loLogicaNegocio.goConsultarComboCanton(int.Parse(cmbProvincia.EditValue.ToString())), true);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbCiudad_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargar)
                {
                    clsComun.gLLenarCombo(ref cmbParroquia, loLogicaNegocio.goConsultarComboParroquia(int.Parse(cmbCiudad.EditValue.ToString())), true);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddPerDes_Click(object sender, EventArgs e)
        {
            try
            {
                bsDescuentos.AddNew();
                ((List<EmpleadoDescuentoPrestamo>)bsDescuentos.DataSource).LastOrDefault().CodigoTipoRol = Diccionario.Seleccione;
                ((List<EmpleadoDescuentoPrestamo>)bsDescuentos.DataSource).LastOrDefault().CodigoRubro = Diccionario.Seleccione;
                dgvDescuentos.Focus();
                dgvDescuentos.ShowEditor();
                dgvDescuentos.UpdateCurrentRow();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddFilaHistorial_Click(object sender, EventArgs e)
        {
            try
            {
                bsHistorialPersonalizado.AddNew();
                //((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().CodigoTipoRol = Diccionario.Seleccione;
                //((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().CodigoRubro = Diccionario.Seleccione;
                var poObject = loLogicaNegocio.goConsultarPersonaContrato(txtNumeroIdentificacion.Text);
                if (poObject != null)
                {
                    ((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().TipoComision = poObject.TipoComision;
                    ((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().Sueldo = poObject.Sueldo;
                    ((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().PorcComision = poObject.PorcentajeComision??0;
                    ((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().PorcComisionAdicional = poObject.PorcentajeComisionAdicional??0;
                    ((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().CargoLaboral = poObject.CargoLaboral;
                    ((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().Departamento = poObject.Departamento;
                    ((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().Sucursal = poObject.Sucursal;
                    ((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().Banco = poObject.Banco;
                    ((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().NumeroCuenta = poObject.NumeroCuenta;
                    ((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().TipoContrato = cmbTipoContrato.Text;
                    ((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().Responsable = clsPrincipal.gsDesUsuario;
                    ((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().FechaInicioContrato = poObject.FechaInicioContrato;
                    ((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().FechaCambio = DateTime.Now;

                    //((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource).LastOrDefault().TipoComision = poObject.TipoComision;
                }
                dgvHistorial.Focus();
                dgvHistorial.ShowEditor();
                dgvHistorial.UpdateCurrentRow();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPlantilla_Click(object sender, EventArgs e)
        {
            GridControl gc = new GridControl();
            try
            {
                BindingSource bs = new BindingSource();
                GridView dgv = new GridView();

                gc.DataSource = bs;
                gc.MainView = dgv;
                gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                dgv.GridControl = gc;
                this.Controls.Add(gc);
                bs.DataSource = new List<PersonaHistorialPlantilla>();
                dgv.BestFitColumns();
                dgv.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
                // Exportar Datos
                clsComun.gSaveFile(gc, "Plantilla_Historial.xlsx", "Files(*.xlsx;)|*.xlsx;");

                gc.Visible = false;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                gc.Visible = false;
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

                clsComun.gsMensajePrevioImportar();

                if (ofdRuta.ShowDialog() == DialogResult.OK)
                {
                    if (!ofdRuta.FileName.Equals(""))
                    {
                        DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);
                        var poLista = ((List<PersonaHistorial>)bsHistorialPersonalizado.DataSource);
                        var poListaImportada = new List<PersonaHistorial>();

                        List<string> psListaMsg = new List<string>();
                        
                        int fila = 2;
                        string psMsgLista = string.Empty;
                        foreach (DataRow item in dt.Rows)
                        {
                            if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                            {
                                string psMsgFila = "";
                                string psMsgOut = "";

                                //try
                                //{

                                PersonaHistorial poItem = new PersonaHistorial();
                                poItem.FechaCambio = clsComun.gdValidarRegistro("FechaCambio", "dt", item[0].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.Observacion = clsComun.gdValidarRegistro("Observacion", "s", item[1].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.Sucursal = clsComun.gdValidarRegistro("Sucursal", "s", item[2].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.Departamento = clsComun.gdValidarRegistro("Departamento", "s", item[3].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.TipoContrato = clsComun.gdValidarRegistro("Tipo Contrato", "s", item[4].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.CargoLaboral = clsComun.gdValidarRegistro("Cargo Laboral", "s", item[5].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.Sueldo = clsComun.gdValidarRegistro("Sueldo", "d", item[6].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.TipoComision = clsComun.gdValidarRegistro("Tipo Comision", "s", item[7].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.PorcComision = clsComun.gdValidarRegistro("Porc Comision", "d", item[8].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.PorcComisionAdicional = clsComun.gdValidarRegistro("Porc Comision Adicional", "d", item[9].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.FechaInicioContrato = clsComun.gdValidarRegistro("Fecha Inicio Contrato", "dt", item[10].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.FechaFinContrato = clsComun.gdValidarRegistro("Fecha Fin Contrato", "dt", item[11].ToString().Trim(), fila, false, ref psMsgOut);
                                poItem.Responsable = clsComun.gdValidarRegistro("Responsable", "s", item[12].ToString().Trim(), fila, true, ref psMsgOut);
                                
                                
                                fila++;

                                if (string.IsNullOrEmpty(psMsgOut))
                                {
                                    poListaImportada.Add(poItem);
                                }
                                else
                                {
                                    psMsgLista = psMsgLista + psMsgOut;
                                }

                                //}
                                //catch (Exception ex)
                                //{

                                //    throw;
                                //}
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

                        
                        poLista.AddRange(poListaImportada);

                        bsHistorialPersonalizado.DataSource = poLista;
                        dgvHistorial.BestFitColumns();
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

        private void btnDescargarFoto_Click(object sender, EventArgs e)
        {
            try
            {
                
                string psRuta = lblFoto.Text;
                
                if (!string.IsNullOrEmpty(psRuta))
                {
                    if (File.Exists(psRuta))
                    {
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "files (*.pdf)|*.pdf|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 2;
                        saveFileDialog1.RestoreDirectory = true;
                        saveFileDialog1.FileName = txtNombreCompleto.Text;
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
                    XtraMessageBox.Show("Consulte empleado para descargar foto", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void xtraTabPage1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void xtraTabPage6_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
