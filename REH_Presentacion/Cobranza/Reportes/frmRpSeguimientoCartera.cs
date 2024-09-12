using DevExpress.XtraGrid.Views.Grid;
using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraGrid.Columns;
using System.IO;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
using GEN_Entidad.Entidades;
using DevExpress.XtraReports.UI;
using REH_Presentacion.Ventas.Reportes.Rpt;
using System.Net.Mail;
using DevExpress.XtraPrinting;
using System.Configuration;
using DevExpress.XtraEditors.Repository;
using COB_Negocio;
using GEN_Entidad.Entidades.Cobranza;

namespace REH_Presentacion.Cobranza.Reportes
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 07/04/2020
    /// Formulario de reporte de Cumplimiento de Kilos Litros
    /// </summary>
    public partial class frmRpSeguimientoCartera : frmBaseTrxDev
    {

        #region Variables

        clsNSeguimiento loLogicaNegocio;
        string lsQuery;
        string lsTituloReporte;
        string lsDataSet;
        bool lbLandSpace;
        Nullable<int> liFixedColumn;
        public int lIdGestorConsulta;
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();
        RepositoryItemButtonEdit rpiBtnAdd = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        #endregion

        #region Eventos
        public frmRpSeguimientoCartera()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNSeguimiento();
            rpiBtnAdd.ButtonClick += rpiBtnAdd_ButtonClick;
            rpiMedDescripcion.WordWrap = true;
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {

                cmbZonas.Properties.DataSource = loLogicaNegocio.goConsultarZonasSAP();
                cmbZonas.Properties.ValueMember = "Codigo";
                cmbZonas.Properties.DisplayMember = "Descripcion";
                cmbZonas.Properties.PopupSizeable = true;

                lCargarEventosBotones();

                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboMotivoSeguimientoCompromiso(), "CodigoMotivo", true);
                clsComun.gLLenarComboGrid(ref dgvSeguimiento, loLogicaNegocio.goConsultarComboMotivoSeguimientoCompromiso(), "CodigoMotivo", false);
                clsComun.gLLenarComboGrid(ref dgvSeguimiento, loLogicaNegocio.goConsultarComboSINO(), "CompromisoCumplido", true, false,10);


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
                SendKeys.Send("{TAB}");
                lConsultarBoton();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Consultar, Consulta Query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            //SendKeys.Send("{TAB}");
            lConsultarBoton();
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
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                {
                    dgvDatos.PostEditor();
                    var poLista = (List<SeguimientoCompromiso>)bsDatos.DataSource;
                    if (poLista.Count() > 0)
                    {
                        string psFilter = "Files(*.xlsx;)|*.xlsx;";
                        clsComun.gSaveFile(gcDatos, Text + "_Preliminar.xlsx", psFilter);
                    }
                }
                else
                {
                    dgvSeguimiento.PostEditor();
                    var poLista = (List<SeguimientoCompromiso>)bsGestion.DataSource;
                    if (poLista.Count() > 0)
                    {
                        string psFilter = "Files(*.xlsx;)|*.xlsx;";
                        clsComun.gSaveFile(gcSeguimiento, Text + "_Compromisos.xlsx", psFilter);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Exportar, Exporta a Pdf.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            try
            {

                dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    DataTable poLista = (DataTable)bsDatos.DataSource;
                    if (poLista != null && poLista.Rows.Count > 0)
                    {
                        string psFilter = "Files(*.PDF;)|*.PDF;";
                        clsComun.gSaveFile(gcDatos, Text + ".pdf", psFilter, "PDF");
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rpiBtnAdd_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (DataTable)bsDatos.DataSource;
                if (poLista.Rows.Count > 0)
                {
                    var pId = int.Parse(poLista.Rows[piIndex][0].ToString());
                    // clsSpreadSheet.gPlantillaDERP(int.Parse(txtPar1.Text), int.Parse(poLista.Rows[piIndex][0].ToString()));
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
        /// Evento del botón Imprimir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
        }

        private void txtPar1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData.Equals(Keys.Enter))
                {
                    SendKeys.Send("{TAB}");
                    lConsultarBoton();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvSeguimiento.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<SeguimientoCompromiso>)bsGestion.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    var poEntidad = poLista[piIndex];
                    var count = poLista.Where(x => x.DocEntry == poEntidad.DocEntry).Count();

                    if (poEntidad.NumCompromiso == count)
                    {
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsGestion.DataSource = poLista;
                        dgvSeguimiento.RefreshData();
                    }
                    else
                    {
                        XtraMessageBox.Show(string.Format("No es posible eliminar, debe eliminar desde el más reciente hasta el más antiguo existe una gestión posterior a este registro"), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnExportarPdf"] != null) tstBotones.Items["btnExportarPdf"].Click += btnExportarPdf_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;

            //txtPar1.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar2.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar3.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar4.KeyDown += new KeyEventHandler(EnterEqualTab);

            bsDatos.DataSource = new List<SeguimientoCompromiso>();
            gcDatos.DataSource = bsDatos;

            bsGestion.DataSource = new List<SeguimientoCompromiso>();
            gcSeguimiento.DataSource = bsGestion;

            dgvDatos.Columns["IdSeguimientoCompromiso"].Visible = false;
            dgvDatos.Columns["CodigoEstado"].Visible = false;
            dgvDatos.Columns["CodCliente"].Visible = false;
            dgvDatos.Columns["CodZona"].Visible = false;
            dgvDatos.Columns["FechaPedido"].Visible = false;
            dgvDatos.Columns["CompromisoCumplido"].Visible = false;
            dgvDatos.Columns["FechaGestion"].Visible = false;
            dgvDatos.Columns["Observaciones"].Visible = false;
            dgvDatos.Columns["Motivo"].Visible = false;
            dgvDatos.Columns["DocEntry"].Visible = false;
            dgvDatos.Columns["DocNum"].Visible = false;
            //dgvDatos.Columns["FechaCompromiso"].Visible = false;
            dgvDatos.Columns["ValorPedido"].Visible = false;
            dgvDatos.Columns["Del"].Visible = false;

            dgvDatos.Columns["Compromiso"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Cliente"].ColumnEdit = rpiMedDescripcion;

            dgvDatos.Columns["Cliente"].Width = 150;
            dgvDatos.Columns["NumCompromiso"].Width = 80;
            dgvDatos.Columns["Factura"].Width = 60;
            dgvDatos.Columns["FechaEmision"].Width = 70;
            dgvDatos.Columns["FechaVencimiento"].Width = 80;
            dgvDatos.Columns["Saldo"].Width = 70;
            dgvDatos.Columns["DiasMora"].Width = 50;
            dgvDatos.Columns["FechaCompromiso"].Width = 80;
            dgvDatos.Columns["Compromiso"].Width = 350;


            dgvDatos.Columns["CodigoMotivo"].Caption = "Motivo";

            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                if (dgvDatos.Columns[i].FieldName == "FechaCompromiso" || dgvDatos.Columns[i].FieldName == "Compromiso" || dgvDatos.Columns[i].FieldName == "CodigoMotivo")
                {
                    dgvDatos.Columns[i].OptionsColumn.ReadOnly = false;
                }
                else
                {
                    dgvDatos.Columns[i].OptionsColumn.ReadOnly = true;
                }
            }

            clsComun.gFormatearColumnasGrid(dgvDatos);

            dgvSeguimiento.Columns["IdSeguimientoCompromiso"].Visible = false;
            dgvSeguimiento.Columns["CodigoEstado"].Visible = false;
            dgvSeguimiento.Columns["CodCliente"].Visible = false;
            dgvSeguimiento.Columns["CodZona"].Visible = false;
            dgvSeguimiento.Columns["Motivo"].Visible = false;
            dgvSeguimiento.Columns["NumCompromiso"].Visible = false;
            dgvSeguimiento.Columns["DocEntry"].Visible = false;
            dgvSeguimiento.Columns["DocNum"].Visible = false;
            dgvSeguimiento.Columns["FechaPedido"].Visible = false;
            dgvSeguimiento.Columns["ValorPedido"].Visible = false;

            dgvSeguimiento.Columns["CodigoMotivo"].Caption = "Motivo";
            dgvSeguimiento.Columns["CompromisoCumplido"].Caption = "Cumplió";

            dgvSeguimiento.Columns["Compromiso"].ColumnEdit = rpiMedDescripcion;
            dgvSeguimiento.Columns["Cliente"].ColumnEdit = rpiMedDescripcion;
            dgvSeguimiento.Columns["Observaciones"].ColumnEdit = rpiMedDescripcion;

            dgvSeguimiento.Columns["FechaGestion"].Width = 70;
            dgvSeguimiento.Columns["Cliente"].Width = 150;
            dgvSeguimiento.Columns["Factura"].Width = 60;
            dgvSeguimiento.Columns["FechaEmision"].Width = 70;
            dgvSeguimiento.Columns["FechaVencimiento"].Width = 80;
            dgvSeguimiento.Columns["Saldo"].Width = 70;
            dgvSeguimiento.Columns["DiasMora"].Width = 50;
            dgvSeguimiento.Columns["FechaCompromiso"].Width = 70;
            dgvSeguimiento.Columns["Compromiso"].Width = 180;
            dgvSeguimiento.Columns["CompromisoCumplido"].Width = 55;
            dgvSeguimiento.Columns["Observaciones"].Width = 180;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvSeguimiento.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 30);

            for (int i = 0; i < dgvSeguimiento.Columns.Count; i++)
            {
                if (dgvSeguimiento.Columns[i].FieldName == "CompromisoCumplido" || dgvSeguimiento.Columns[i].FieldName == "Observaciones")
                {
                    dgvSeguimiento.Columns[i].OptionsColumn.ReadOnly = false;
                }
                else
                {
                    dgvSeguimiento.Columns[i].OptionsColumn.ReadOnly = true;
                }
            }

            clsComun.gFormatearColumnasGrid(dgvSeguimiento);
        }

        private bool lbEsValido()
        {


            return true;
        }

        private void lLimpiar(bool tbSoloDetalle = false)
        {
            if (!tbSoloDetalle)
            {
                gcDatos.DataSource = null;
                dgvDatos.Columns.Clear();
                dtpFechaCorte.Value = DateTime.Now;
                lsQuery = string.Empty;

            }


        }

        private void lBuscar()
        {
            lBuscarDatos();
            lBuscarSeguimiento();
        }

        private void lBuscarDatos()
        {
            string psZonas = cmbZonas.EditValue.ToString().Replace(" ", "");
            bsDatos.DataSource = loLogicaNegocio.goConsultaPreliminar(dtpFechaCorte.Value, psZonas);
            dgvDatos.RefreshData();
        }

        private void lBuscarSeguimiento()
        {
            string psZonas = cmbZonas.EditValue.ToString().Replace(" ", "");
            var psLista = new List<string>();
            var array = psZonas.Split(',');
            foreach (var item in array)
            {
                psLista.Add(item);
            }

            bsGestion.DataSource = loLogicaNegocio.goConsultar(psLista);
            dgvSeguimiento.RefreshData();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {

                    if (xtraTabControl1.SelectedTabPageIndex == 0)
                    {
                        dgvDatos.PostEditor();
                        var poLista = (List<SeguimientoCompromiso>)bsDatos.DataSource;
                        string psMsg = loLogicaNegocio.gsGuardarNuevos(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lBuscarDatos();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        dgvSeguimiento.PostEditor();
                        var poSeguimiento = (List<SeguimientoCompromiso>)bsGestion.DataSource;
                        string psZonas = cmbZonas.EditValue.ToString().Replace(" ", "");
                        var psLista = new List<string>();
                        var array = psZonas.Split(',');
                        foreach (var item in array)
                        {
                            psLista.Add(item);
                        }
                        string psMsg = loLogicaNegocio.gsGuardarSeguimiento(psLista, poSeguimiento, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);

                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lBuscarSeguimiento();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        
        private void lConsultarBoton()
        {
            try
            {
                //SendKeys.Send("{TAB}");
                Cursor.Current = Cursors.WaitCursor;

                if (lbEsValido())
                {
                    lBuscar();
                }
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void frmRpCumplimientoKilosLitros_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lConsultarBoton();
            }
        }

        private void lEnvioAutomatio()
        {

            Cursor.Current = Cursors.WaitCursor;


            Cursor.Current = Cursors.Default;


        }

        private void gcDatos_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                lConsultarBoton();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarSeguimiento(string tsCodCliente)
        {
            //var poLista
        }
    }
}
