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
using static GEN_Entidad.Diccionario;
using System.Drawing;

namespace REH_Presentacion.Cobranza.Reportes
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 18/07/2024
    /// Formulario de Reporte de cartera para envío de correo
    /// </summary>
    public partial class frmRpEnviarCarteraZona : frmBaseTrxDev
    {

        #region Variables

        clsNGestorConsulta loLogicaNegocio;
        private bool lbEnvioIndividual = true;
        private List<Combo> loComboZonas = new List<Combo>(); 
        #endregion

        #region Eventos
        public frmRpEnviarCarteraZona()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNGestorConsulta();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                loComboZonas = loLogicaNegocio.goConsultarZonasSAP();
                clsComun.gLLenarCombo(ref cmbZonas, loComboZonas);
                lCargarEventosBotones();
                lLimpiar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Enviar Correo para enviar por correo el detalle del grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string psZonas = cmbZonas.EditValue.ToString().Replace(" ", "");
                if (!string.IsNullOrEmpty(psZonas))
                {
                    DialogResult dialogResult2 = XtraMessageBox.Show("¿Está seguro de enviar correo?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult2 == DialogResult.Yes)
                    {
                        //lPrepararEnvioCorreoCarteraPorZona(psZonas, lbEnvioIndividual, ConfigurationManager.AppSettings["FileTmpCom"].ToString() + "CarteraPorZona.xlsx");
                        lEnviarCorreoCarteraPorZona(psZonas,ConfigurationManager.AppSettings["FileTmpCom"].ToString() + "CarteraPorZona.xlsx", false);
                    }
                }
                else
                {
                    XtraMessageBox.Show("Seleccione Zonas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;

        }

        public void lEnviarCorreoCarteraPorZona(string tsZonas, string tsRuta, bool EnviarConCopia)
        {
            string psRuta = "";
            List<Attachment> listAdjuntosEmail = null;

            List<string> zonas = tsZonas.Split(',').ToList();

            var gszonas = loLogicaNegocio.goConsultarZonasSAP(zonas);
            foreach (var zona in gszonas)
            {

                ldtConsultar(zona.Codigo);

                DataTable poLista = (DataTable)bsDatos.DataSource;
                if (poLista != null && poLista.Rows.Count > 0)
                {
                    string correos = loLogicaNegocio.obtenerCorreosCarteraZona(zona.Codigo, "PAR");
                    string copias = "";
                    if (EnviarConCopia)
                    {
                        copias = loLogicaNegocio.obtenerCorreosCarteraZona(zona.Codigo, "COP");
                    }

                    if (!string.IsNullOrEmpty(correos))
                    {
                        psRuta = tsRuta;
                        gcDatos.ExportToXlsx(psRuta);

                        listAdjuntosEmail = new List<Attachment>();

                        if (File.Exists(psRuta))
                            listAdjuntosEmail.Add(new Attachment(psRuta));

                        string psCuerpo = "<p>Estimado/a <br/><br/>A continuación, se adjunta el reporte de cartera correspondiente a la zona " + zona.Descripcion + ", con el objetivo de que usted pueda revisar y analizar el mismo.<br/><br/>Saludos Cordiales.</p>";

                        //clsComun.EnviarPorCorreo("varevalo@afecor.com", "Cartera por Zona: " + zona.Descripcion, psCuerpo, listAdjuntosEmail);
                        clsComun.EnviarPorCorreo(correos, "Cartera por Zona: " + zona.Descripcion, psCuerpo, listAdjuntosEmail, false, copias);

                        if (File.Exists(psRuta)) File.Delete(psRuta);
                    }
                }
            }
        }

        //}

        /// <summary>
        /// Evento del botón Consultar, Consulta Query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            lBuscar();
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
                dgvDatos.PostEditor();
                DataTable poLista = (DataTable)bsDatos.DataSource;
                if (poLista != null && poLista.Rows.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcDatos, Text + ".xlsx", psFilter);
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmRpCumplimientoKilosLitros_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                lBuscar();
            }
        }

        private void dgvDatos_PrintInitialize(object sender, DevExpress.XtraGrid.Views.Base.PrintInitializeEventArgs e)
        {
            (e.PrintingSystem as PrintingSystemBase).PageSettings.Landscape = true;
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnEnviarCorreo"] != null) tstBotones.Items["btnEnviarCorreo"].Click += btnEnviarCorreo_Click;
        }

        private void lLimpiar()
        {
            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();
        }

        private void ldtConsultar(string tsZonas)
        {

            var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC COBSPCONSULTACARTERAZONA '{0}','{1}'", DateTime.Now, tsZonas));

            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();
            dgvDatos.GroupSummary.Clear();

            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;

            clsComun.gFormatearColumnasGrid(dgvDatos);
            clsComun.gOrdenarColumnasGridFull(dgvDatos);

            dgvDatos.OptionsView.RowAutoHeight = true;
            dgvDatos.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;

            dgvDatos.OptionsView.ShowGroupedColumns = true;
            dgvDatos.Columns["Cliente"].GroupIndex = 0;
            dgvDatos.ExpandAllGroups();

            //dgvDatos.Columns["Cliente"].Visible = false;
            //dgvDatos.Columns["CodCliente"].Visible = false;

            dgvDatos.Appearance.GroupFooter.Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);

            dgvDatos.RefreshData();
        }

        private void lBuscar()
        {
            string psZonas = cmbZonas.EditValue.ToString().Replace(" ", "");
            ldtConsultar(psZonas);

        }

        
        #endregion

    }
}
