using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using GEN_Entidad;
using GEN_Entidad.Entidades.Credito;
using GEN_Entidad.Entidades.Ventas;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
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
using VTA_Negocio;

namespace REH_Presentacion.Ventas.Transacciones
{
    /// <summary>
    /// Formulario que permite guardar los clientes Rebate
    /// Autor: Víctor Arévalo
    /// Fecha: 21/12/2021
    /// </summary>
    public partial class frmTrRebate : frmBaseTrxDev
    {

        #region Variables
        clsNRebate loLogicaNegocio;
        RepositoryItemButtonEdit rpiBtnDel;
        BindingSource bsDatos;
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();
        BindingSource bsRebate;

        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDelAdj = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnAdd = new RepositoryItemButtonEdit();

        decimal ldcPorcRebate;

        #endregion

        #region Eventos

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmTrRebate()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNRebate();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnShowComentarios.ButtonClick += rpiBtnShowComentarios_ButtonClick;

            rpiBtnDelAdj.ButtonClick += rpiBtnDelAdj_ButtonClick;
            rpiBtnAdd.ButtonClick += rpiBtnAdd_ButtonClick;
            rpiBtnShow.ButtonClick += rpiBtnView_ButtonClick;

        }

        /// <summary>
        /// Evento del boton de eliminar en el Grid, elimina la fila seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDelAdj_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvRebate.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<RebateDetalleGrid>)bsRebate.DataSource;

                if (poLista[piIndex].CodigoEstado != Diccionario.Aprobado)
                {
                    if (poLista.Count > 0 && piIndex >= 0 && !string.IsNullOrEmpty(poLista[piIndex].NombreOriginal))
                    {
                        DialogResult dialogResult = XtraMessageBox.Show("¿Desea eliminar documento adjunto?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {
                            poLista[piIndex].ArchivoAdjunto = "";
                            poLista[piIndex].RutaOrigen = "";
                            poLista[piIndex].NombreOriginal = "";
                            poLista[piIndex].RutaDestino = "";
                            // Asigno mi nueva lista al Binding Source
                            bsRebate.DataSource = poLista;
                            dgvRebate.RefreshData();

                            //btnGrabar_Click(null, null);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No es posible eliminar archivo en registros aprobados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                piIndex = dgvRebate.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<RebateDetalleGrid>)bsRebate.DataSource;

                if (poLista[piIndex].CodigoEstado != Diccionario.Aprobado)
                {
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
                                    poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaRebates"].ToString() + Name;
                                    // Asigno mi nueva lista al Binding Source
                                    bsRebate.DataSource = poLista;
                                    dgvRebate.RefreshData();

                                    //btnGrabar_Click(null, null);
                                }
                                else
                                {
                                    XtraMessageBox.Show("El tamano máximo permitido es de: " + clsPrincipal.gdcTamanoMb + "mb", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No es posible añadir archivo en registros aprobados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void rpiBtnView_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvRebate.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<RebateDetalleGrid>)bsRebate.DataSource;
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
                piIndex = dgvRebate.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<RebateDetalleGrid>)bsDatos.DataSource;
                var poListaRebate = (List<RebateDetalleGrid>)bsRebate.DataSource;

                if (poListaRebate.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poListaRebate[piIndex];

                    poLista.Add(poEntidad);

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poListaRebate.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsRebate.DataSource = poListaRebate;
                    dgvRebate.RefreshData();

                    bsDatos.DataSource = poLista.ToList();
                    dgvDatos.RefreshData();
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
        /// Evento del botón Generar, Genera Novedad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                dgvRebate.PostEditor();
                //string psMsgValida = lsEsValido();

                List<RebateDetalleGrid> poLista = (List<RebateDetalleGrid>)bsRebate.DataSource;

                if (poLista.Count > 0)
                {
                    string psMsg = loLogicaNegocio.gsGuardarRebateCliente(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lBuscar();
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a guardar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Evento del botón Nuevo, Generalmente Limpia el formulario 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtAnio.Text.Trim()) && cmbTrimestre.EditValue.ToString() != Diccionario.Seleccione)
                {
                    var dt = loLogicaNegocio.gdtConsultaRebateTrimestre(int.Parse(txtAnio.Text), int.Parse(cmbTrimestre.EditValue.ToString()));
                    xrptGeneral xrptGen = new xrptGeneral();
                    xrptGen.dt = dt;
                    xrptGen.Landscape = false;
                    xrptGen.Parameters["parameter1"].Value = string.Format("CLIENTES REBATE AÑO: {0}, TRIMESTRE {1}", txtAnio.Text, cmbTrimestre.EditValue);

                    xrptGen.RequestParameters = false;
                    xrptGen.Parameters["parameter1"].Visible = false;
                    using (ReportPrintTool printTool = new ReportPrintTool(xrptGen))
                    {
                        printTool.ShowRibbonPreviewDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Exportar, Exporta a Excel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(txtAnio.Text) && cmbTrimestre.EditValue.ToString() != Diccionario.Seleccione)
                {
                    dgvDatos.PostEditor();
                    dgvRebate.PostEditor();

                    List<RebateDetalleGrid> poLista = (List<RebateDetalleGrid>)bsDatos.DataSource;
                    if (poLista != null && poLista.Count > 0)
                    {
                        string psFilter = "Files(*.xlsx;)|*.xlsx;";
                        clsComun.gSaveFile(gcDatos, "Rebate_Preliminar_" + txtAnio.Text + "_" + cmbTrimestre.Text + ".xlsx", psFilter);
                    }

                    List<RebateDetalleGrid> poListaProvision = (List<RebateDetalleGrid>)bsRebate.DataSource;
                    if (poListaProvision != null && poListaProvision.Count > 0)
                    {
                        string psFilter = "Files(*.xlsx;)|*.xlsx;";
                        clsComun.gSaveFile(gcRebate, "Rebate_Seleccionados_" + txtAnio.Text + "_" + cmbTrimestre.Text + ".xlsx", psFilter);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void frmTrRebate_Load(object sender, EventArgs e)
        {
            try
            {

                clsComun.gLLenarCombo(ref cmbTrimestre, loLogicaNegocio.goConsultarComboTrimestres(), true);
                lCargarEventosBotones();

                clsComun.gLLenarComboGrid(ref dgvRebate, loLogicaNegocio.goConsultarComboEstado(),"CodigoEstado");
                var poParametro = loLogicaNegocio.goConsultarParametroVta();
                ldcPorcRebate = poParametro != null ? poParametro.PorcBonificacionCumplimientoRebate: 0M;

                

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Métodos
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;

            /*********************************************************************************************************************************************/

            bsDatos = new BindingSource();
            bsDatos.DataSource = new List<RebateDetalleGrid>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["IdRebateClienteDetalle"].Visible = false;
            dgvDatos.Columns["Periodo"].Visible = false;
            dgvDatos.Columns["Trimestre"].Visible = false;
            dgvDatos.Columns["CodeZona"].Visible = false;
            dgvDatos.Columns["CodeCliente"].Visible = false;
            dgvDatos.Columns["Del"].Visible = false;
            dgvDatos.Columns["Carta"].Visible = false;
            dgvDatos.Columns["Generado"].Visible = false;
            //dgvDatos.Columns["Observacion"].Visible = false;
            dgvDatos.Columns["ValorRebate"].Visible = false;
            dgvDatos.Columns["CantFacturas"].Visible = false;
            dgvDatos.Columns["CantFacturasPagadas"].Visible = false;
            dgvDatos.Columns["CodigoEstado"].Visible = false;
            dgvDatos.Columns["PorcentajeRebate"].Visible = false;
            dgvDatos.Columns["RegistradoCobranza"].Visible = false;
            dgvDatos.Columns["FechaCobranza"].Visible = false;
            dgvDatos.Columns["VerComentarios"].Visible = false;
            
            dgvDatos.Columns["DelAdj"].Visible = false;
            dgvDatos.Columns["AddAdj"].Visible = false;
            dgvDatos.Columns["Ver"].Visible = false;
            dgvDatos.Columns["RutaDestino"].Visible = false;
            dgvDatos.Columns["RutaOrigen"].Visible = false;
            dgvDatos.Columns["ArchivoAdjunto"].Visible = false;
            dgvDatos.Columns["NombreOriginal"].Visible = false;

            dgvDatos.OptionsBehavior.Editable = true;
            dgvDatos.Columns["NameZona"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["NameCliente"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["CantFacturasPendientes"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[4].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[5].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[6].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[7].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[8].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[9].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[10].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[11].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["CantFacturas"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["CantFacturasPagadas"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["DiasMora"].OptionsColumn.AllowEdit = false;

            clsComun.gFormatearColumnasGrid(dgvDatos);

            dgvDatos.FixedLineWidth = 3;
            dgvDatos.Columns[0].Fixed = FixedStyle.Left;
            dgvDatos.Columns[5].Fixed = FixedStyle.Left;
            dgvDatos.Columns[7].Fixed = FixedStyle.Left;

            /*********************************************************************************************************************************************/

            bsRebate = new BindingSource();
            bsRebate.DataSource = new List<RebateDetalleGrid>();
            gcRebate.DataSource = bsRebate;

            dgvRebate.OptionsBehavior.Editable = true;
            dgvRebate.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;

            dgvRebate.Columns["IdRebateClienteDetalle"].Visible = false;
            dgvRebate.Columns["Periodo"].Visible = false;
            dgvRebate.Columns["Trimestre"].Visible = false;
            dgvRebate.Columns["CodeZona"].Visible = false;
            dgvRebate.Columns["CodeCliente"].Visible = false;
            dgvRebate.Columns["Carta"].Visible = false;
            //dgvRebate.Columns["Observacion"].Visible = false;
            dgvRebate.Columns["Sel"].Visible = false;
            dgvRebate.Columns["CantFacturas"].Visible = false;
            dgvRebate.Columns["CantFacturasPagadas"].Visible = false;
            //dgvRebate.Columns["CodigoEstado"].Visible = false;
            dgvRebate.Columns["RegistradoCobranza"].Visible = false;
            dgvRebate.Columns["FechaCobranza"].Visible = false;

            dgvRebate.Columns["NameZona"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["VentaNeta"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CantFacturas"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CantFacturasPagadas"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CantFacturasPendientes"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Presupuesto"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["NameCliente"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["PorcentCumplimientoMeta"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["PorcentMargenRentabilidad"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["DiasMora"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CodigoEstado"].OptionsColumn.AllowEdit = false;

            dgvRebate.Columns["PorcentajeRebate"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CodigoEstado"].Caption = "Estado";
            dgvRebate.Columns["PorcentajeRebate"].Caption = "%";

            clsComun.gFormatearColumnasGrid(dgvRebate);

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvRebate.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowComentarios, dgvRebate.Columns["VerComentarios"], "Trazabilidad", Diccionario.ButtonGridImage.showhidecomment_16x16, 30);
            
            clsComun.gDibujarBotonGrid(rpiBtnDelAdj, dgvRebate.Columns["DelAdj"], "Eliminar Adjunto", Diccionario.ButtonGridImage.trash_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvRebate.Columns["AddAdj"], "Agregar", Diccionario.ButtonGridImage.open_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvRebate.Columns["Ver"], "Visualizar", Diccionario.ButtonGridImage.show_16x16, 30);

            dgvRebate.Columns["RutaDestino"].Visible = false;
            dgvRebate.Columns["RutaOrigen"].Visible = false;
            dgvRebate.Columns["ArchivoAdjunto"].Visible = false;
            dgvRebate.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["NombreOriginal"].Caption = "Adjunto";

            dgvRebate.FixedLineWidth = 3;
            dgvRebate.Columns["CodigoEstado"].Fixed = FixedStyle.Left;
            dgvRebate.Columns["NameZona"].Fixed = FixedStyle.Left;
            dgvRebate.Columns["NameCliente"].Fixed = FixedStyle.Left;

            cmbTrimestre.KeyDown += new KeyEventHandler(EnterEqualTab);
            
            txtAnio.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAnio.KeyPress += new KeyPressEventHandler(SoloNumeros);
            
            
        }

        private void lLimpiar()
        {
            //if ((cmbProducto.Properties.DataSource as IList).Count > 0) cmbProducto.ItemIndex = 0;
            //if ((cmbZona.Properties.DataSource as IList).Count > 0) cmbZona.ItemIndex = 0;
            //txtUnidades.Text = "0";
            //txtPrecio.Text = "0";
            //txtValor.Text = "0";
            //txtMedidaConversion.Text = "0";
            //txtTotal.Text = "0";
            //txtTipoProducto.Text = string.Empty;
            //txtAnio.Text = string.Empty;
            //bsDatos.DataSource = new List<PresupuestoVentasGrid>();
        }

        private void lBuscar()
        {

            Cursor.Current = Cursors.WaitCursor;

            if (!string.IsNullOrEmpty(txtAnio.Text) && cmbTrimestre.EditValue.ToString() != Diccionario.Seleccione)
            {
                
                bsDatos.DataSource = loLogicaNegocio.goConsultaPreliminarClientesRebate(int.Parse(txtAnio.Text), int.Parse(cmbTrimestre.EditValue.ToString()));
                gcDatos.DataSource = bsDatos;

                clsComun.gOrdenarColumnasGridFullEditable(dgvDatos);

                bsRebate.DataSource = loLogicaNegocio.goConsultaClientesRebate(int.Parse(txtAnio.Text), int.Parse(cmbTrimestre.EditValue.ToString()));
                gcRebate.DataSource = bsRebate;

                clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);


            }

            Cursor.Current = Cursors.Default;

        }

        #endregion

       
        private void cmbTrimestre_EditValueChanged(object sender, EventArgs e)
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

        private void txtAnio_Leave(object sender, EventArgs e)
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

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {

                List<RebateDetalleGrid> poListaPreliminar = (List<RebateDetalleGrid>)bsDatos.DataSource;
                List<RebateDetalleGrid> poListaRebate = (List<RebateDetalleGrid>)bsRebate.DataSource;

                List<RebateDetalleGrid> poListaAgregar = new List<RebateDetalleGrid>();

                foreach (var item in poListaPreliminar.Where(x=>x.Sel).ToList())
                {
                    if (poListaRebate.Where(x=> x.Periodo == item.Periodo && x.Trimestre == item.Trimestre && x.CodeZona == item.CodeZona && x.CodeCliente == item.CodeCliente).Count() == 0)
                    {
                        var poRegistro = new RebateDetalleGrid();
                        poRegistro.Periodo = int.Parse(txtAnio.Text);
                        poRegistro.Trimestre = int.Parse(cmbTrimestre.EditValue.ToString());
                        poRegistro.CodeZona = item.CodeZona;
                        poRegistro.NameZona = item.NameZona;
                        poRegistro.CodeCliente = item.CodeCliente;
                        poRegistro.NameCliente = item.NameCliente;
                        poRegistro.Presupuesto = item.Presupuesto;
                        poRegistro.VentaNeta = item.VentaNeta;
                        poRegistro.PorcentCumplimientoMeta = item.PorcentCumplimientoMeta;
                        poRegistro.PorcentMargenRentabilidad = item.PorcentMargenRentabilidad;
                        poRegistro.DiasMora = item.DiasMora;
                        //poRegistro.ValorRebate = Math.Round(item.VentaNeta * ldcPorcRebate, 2);
                        poRegistro.PorcentajeRebate = ldcPorcRebate;
                        poRegistro.CantFacturas = item.CantFacturas;
                        poRegistro.CantFacturasPagadas = item.CantFacturasPagadas;
                        poRegistro.CantFacturasPendientes = item.CantFacturasPendientes;
                        poRegistro.CodigoEstado = Diccionario.Pendiente;

                        poListaRebate.Add(poRegistro);

                        poListaPreliminar.Remove(item);

                        bsDatos.DataSource = poListaPreliminar;
                        dgvDatos.RefreshData();

                        bsRebate.DataSource = poListaRebate;
                        dgvRebate.RefreshData();

                        clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);
                    }

                }

                //if (poLista.Count > 0 && piIndex >= 0)
                //{
                //    // Tomamos la entidad de la fila seleccionada
                //    var poEntidad = poLista[piIndex];

                //    // Eliminar Fila seleccionada de mi lista
                //    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                //    poLista.RemoveAt(piIndex);

                //    // Asigno mi nueva lista al Binding Source
                //    bsDatos.DataSource = poLista;
                //    dgvParametrizaciones.RefreshData();
                //}


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void rpiBtnShowComentarios_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvRebate.GetFocusedDataSourceRowIndex();
                var poLista = (List<RebateDetalleGrid>)bsRebate.DataSource;
                if (piIndex >= 0)
                {
                    var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.Rebate, poLista[piIndex].IdRebateClienteDetalle);

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Trazabilidad del Rebate: " };
                    pofrmBuscar.ShowDialog();

                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
