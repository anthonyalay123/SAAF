using REH_Negocio;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GEN_Entidad;
using REH_Presentacion.Comun;
using DevExpress.XtraEditors;
using DevExpress.Data;
using DevExpress.XtraGrid;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad.Entidades;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using System.Drawing.Printing;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 11/09/2020
    /// Formulario para ingresar licencias y permisos
    /// </summary>
    public partial class frmTrLiquidacion : frmBaseTrxDev
    {

        #region Variables
        clsNNomina loLogicaNegocio = new clsNNomina();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDel1 = new RepositoryItemButtonEdit();
        private bool pbCargado = false;
        public int lid = 0;
        private static PrinterSettings prnSettings;

        #endregion

        public frmTrLiquidacion()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnDel1.ButtonClick += rpiBtnDel1_ButtonClick;
        }

        /// <summary>
        /// Evento que se dispara cuando carga el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPaRubroTipoRol_Load(object sender, EventArgs e)
        {
            try
            {
                cmbEmpleado.Focus();
                clsComun.gLLenarCombo(ref cmbEmpleado, loLogicaNegocio.goConsultarComboIdPersona(), true);
                clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoFinContrato(), true);
                clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstado(), true);
                cmbEstado.EditValue = Diccionario.Pendiente;
                lLimpiar();

                bsRubrosManuales.DataSource = new List<RubroManual>();
                gcRubrosManuales.DataSource = bsRubrosManuales;
                clsComun.gLLenarComboGrid(ref dgvRubrosManuales, loLogicaNegocio.goConsultarComboRubroEditable(), "Rubro", true);
                dgvRubrosManuales.OptionsView.ColumnAutoWidth = true;

                bsRolComisiones.DataSource = new List<RolManual>();
                gcRol.DataSource = bsRolComisiones;
                clsComun.gLLenarComboGrid(ref dgvRol, loLogicaNegocio.goConsultarComboPeriodoComisiones(), "IdPeriodo", true);
                dgvRol.OptionsView.ColumnAutoWidth = true;

                lCargarEventosBotones();
                pbCargado = true;

                if (lid != 0)
                {
                    txtNo.Text = lid.ToString();
                    lConsultar();
                }

                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = false;
                if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Enabled = true;
                if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Enabled = true;
                if (tstBotones.Items["btnPreliminar"] != null) tstBotones.Items["btnPreliminar"].Enabled = true;

                xtraTabControl1.TabPages[2].PageVisible = false;
                txtValorComisiones.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
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
                piIndex = dgvRubrosManuales.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<RubroManual>)bsRubrosManuales.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsRubrosManuales.DataSource = poLista;
                    dgvRubrosManuales.RefreshData();
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
        private void rpiBtnDel1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {

                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvRol.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<RolManual>)bsRolComisiones.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsRolComisiones.DataSource = poLista;
                    dgvRol.RefreshData();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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


        /// <summary>
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbEsValido())
                {
                    // Actualizar datos de rubros manuales
                    dgvRubrosManuales.PostEditor();
                    var poLista = (List<RubroManual>)bsRubrosManuales.DataSource;
                    loLogicaNegocio.gsGuardarRubrosManuales(int.Parse(lblIdEmpleadoContrato.Text), int.Parse(cmbEmpleado.EditValue.ToString()), poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    dgvRol.PostEditor();
                    var poLista1 =  (List<RolManual>)bsRolComisiones.DataSource;
                    loLogicaNegocio.gsGuardarRolManuales(int.Parse(lblIdEmpleadoContrato.Text), int.Parse(cmbEmpleado.EditValue.ToString()), poLista1, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    lCalcular(false);
                    //lConsultar();

                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                    if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = false;
                    if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Enabled = true;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Enabled = true;

                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Preliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPreliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbEsValido())
                {
                    //Actualizar datos de rubros manuales
                    dgvRubrosManuales.PostEditor();
                    var poLista = (List<RubroManual>)bsRubrosManuales.DataSource;
                    loLogicaNegocio.gsGuardarRubrosManuales(int.Parse(lblIdEmpleadoContrato.Text), int.Parse(cmbEmpleado.EditValue.ToString()), poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    dgvRol.PostEditor();
                    var poLista1 = (List<RolManual>)bsRolComisiones.DataSource;
                    loLogicaNegocio.gsGuardarRolManuales(int.Parse(lblIdEmpleadoContrato.Text), int.Parse(cmbEmpleado.EditValue.ToString()), poLista1, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    var ds = lCalcular(false);
                    lImprimir(ds);
                    //lConsultar();

                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                    if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                    if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = false;
                    if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Enabled = false;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Enabled = false;
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // <summary>
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbEsValido())
                {
                    // Actualizar datos de rubros manuales
                    dgvRubrosManuales.PostEditor();
                    var poLista = (List<RubroManual>)bsRubrosManuales.DataSource;
                    loLogicaNegocio.gsGuardarRubrosManuales(int.Parse(lblIdEmpleadoContrato.Text), int.Parse(cmbEmpleado.EditValue.ToString()), poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    dgvRol.PostEditor();
                    var poLista1 = (List<RolManual>)bsRolComisiones.DataSource;
                    loLogicaNegocio.gsGuardarRolManuales(int.Parse(lblIdEmpleadoContrato.Text), int.Parse(cmbEmpleado.EditValue.ToString()), poLista1, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    lCalcular(true);
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
                if (!string.IsNullOrEmpty(txtNo.Text))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var psMsg = loLogicaNegocio.gsEliminarLiquidacion(int.Parse(txtNo.Text), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Evento del botón Imprimir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                lImprimir();

                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
                if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = true;
                if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Enabled = true;
                if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Enabled = true;

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
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Click += btnCalcular_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnPreliminar"] != null) tstBotones.Items["btnPreliminar"].Click += btnPreliminar_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvRubrosManuales.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);
            clsComun.gDibujarBotonGrid(rpiBtnDel1, dgvRol.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);

            //clsComun.gFormatearColumnasGrid(dgvRubrosManuales);

            dgvRubrosManuales.Columns["Valor"].UnboundType = UnboundColumnType.Decimal;
            dgvRubrosManuales.Columns["Valor"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvRubrosManuales.Columns["Valor"].DisplayFormat.FormatString = "c2";
            dgvRubrosManuales.Columns["Valor"].Summary.Add(SummaryItemType.Sum, "Valor", "{0:c2}");
            dgvRubrosManuales.Columns["Id"].Visible = false;

            dgvRol.Columns["Valor"].UnboundType = UnboundColumnType.Decimal;
            dgvRol.Columns["Valor"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvRol.Columns["Valor"].DisplayFormat.FormatString = "c2";
            dgvRol.Columns["Valor"].Summary.Add(SummaryItemType.Sum, "Valor", "{0:c2}");
            dgvRol.Columns["Id"].Visible = false;
            //dgvRol.Columns["Anio"].Visible = false;
            dgvRol.Columns["Mes"].Visible = false;

            dgvRol.Columns["IdPeriodo"].Caption = "Rol";

            LValidarBotones();
        }

        private void lImprimir(DataSet toDt =  null)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {

                if (!string.IsNullOrEmpty(txtNo.Text) || toDt != null)
                {
                    DataSet ds = new DataSet();
                    DataSet dsFull = new DataSet();
                    if (toDt == null)
                    {
                        dsFull = loLogicaNegocio.gdtRptLiquidacion(int.Parse(txtNo.Text));
                    }
                    else
                    {
                        dsFull = toDt;
                    }

                    DataTable dtCab = dsFull.Tables[0];
                    dtCab.TableName = "Cab";
                    ds.Merge(dtCab);

                    DataTable dtDetRol = dsFull.Tables[2];
                    dtDetRol.TableName = "DetRol";
                    ds.Merge(dtDetRol);

                    DataTable dtDetBSDT = dsFull.Tables[3];
                    dtDetBSDT.TableName = "DetBSDT";
                    ds.Merge(dtDetBSDT);

                    DataTable dtDetBSDC = dsFull.Tables[4];
                    dtDetBSDC.TableName = "DetBSDC";
                    ds.Merge(dtDetBSDC);


                    DataTable dtDetRes = dsFull.Tables[1];
                    dtDetRes.TableName = "DetRes";
                    ds.Merge(dtDetRes);

                    DataTable dtDetVac = dsFull.Tables[5];
                    dtDetVac.TableName = "DetVac";
                    ds.Merge(dtDetVac);

                    if (dtCab.Rows.Count > 0)
                    {
                        xrptLiquidacionResumen xrpt = new xrptLiquidacionResumen();
                        
                        xrpt.DataSource = ds;
                        xrpt.RequestParameters = false;
                        
                        //using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                        //{
                        //    printTool.ShowRibbonPreviewDialog();
                        //}


                        xrptLiquidacionResumenDetalle xrptde = new xrptLiquidacionResumenDetalle();

                        xrptde.DataSource = ds;
                        xrptde.RequestParameters = false;

                        //using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrptde))
                        //{
                        //    printTool.ShowRibbonPreviewDialog();
                        //}


                        XtraReport[] reports;
                        reports = new XtraReport[] { xrpt, xrptde };

                        ReportPrintTool pt1 = new ReportPrintTool(xrpt);
                        pt1.PrintingSystem.StartPrint += new PrintDocumentEventHandler(PrintingSystem_StartPrint);

                        foreach (XtraReport report in reports)
                        {
                            ReportPrintTool pts = new ReportPrintTool(report);
                            pts.PrintingSystem.StartPrint +=
                                new PrintDocumentEventHandler(reportsStartPrintEventHandler);
                        }

                        //pt1.PrintDialog();
                        foreach (XtraReport report in reports)
                        {

                            ReportPrintTool pts = new ReportPrintTool(report);
                            pts.ShowRibbonPreview();

                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos para consultar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existen datos guardados para imprimir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Eventos necesarios para la presentación de reportes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void PrintingSystem_StartPrint(object sender, PrintDocumentEventArgs e)
        {
            prnSettings = e.PrintDocument.PrinterSettings;
        }

        /// <summary>
        /// Eventos necesarios para la presentación de reportes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void reportsStartPrintEventHandler(object sender, PrintDocumentEventArgs e)
        {
            int pageCount = e.PrintDocument.PrinterSettings.ToPage;
            e.PrintDocument.PrinterSettings = prnSettings;

            // The following line is required if the number of pages for each report varies, 
            // and you consistently need to print all pages.
            e.PrintDocument.PrinterSettings.ToPage = pageCount;
        }


        private void lListar()
        {

            if (!string.IsNullOrEmpty(lblIdEmpleadoContrato.Text))
            {
                bsRubrosManuales.DataSource = loLogicaNegocio.ListarRubroManual(int.Parse(lblIdEmpleadoContrato.Text));
                gcRubrosManuales.DataSource = bsRubrosManuales;

                bsRolComisiones.DataSource = loLogicaNegocio.ListarRolManual(int.Parse(lblIdEmpleadoContrato.Text));
                gcRol.DataSource = bsRolComisiones;
            }
            else
            {
                bsRubrosManuales.DataSource = new List<RubroManual>();
                gcRubrosManuales.DataSource = bsRubrosManuales;

                bsRolComisiones.DataSource = new List<RolManual>();
                gcRol.DataSource = bsRolComisiones;
            }



            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();

            DataTable dt = loLogicaNegocio.ldtLiquidacion(int.Parse(txtNo.Text));
            //List<Rubro> poRubros = loLogicaNegocio.goConsultaRubro();

            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;

            dgvDatos.PopulateColumns();


            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                dgvDatos.Columns[i].OptionsColumn.ReadOnly = true;
                // Quita numeral del nombre de la columna, sirve para convertir a número
                if (dgvDatos.Columns[i].CustomizationSearchCaption.Contains("#")) dgvDatos.Columns[i].Caption = dgvDatos.Columns[i].CustomizationSearchCaption.Replace("#", "").Trim();

                if (i > 0)
                {
                    if (dt.Columns[i].DataType == typeof(decimal))
                    {
                        var psNameColumn = dgvDatos.Columns[i].FieldName;
                        if (psNameColumn.ToUpper().Contains("%") || psNameColumn.ToUpper().Contains("PORC"))
                        {
                            dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                            dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            dgvDatos.Columns[i].DisplayFormat.FormatString = "p2";

                            //dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:0.00}");


                            //GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                            //item1.FieldName = psNameColumn;
                            //item1.SummaryType = SummaryItemType.Sum;
                            //item1.DisplayFormat = "{0:0.00}";
                            //item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                            //dgvDatos.GroupSummary.Add(item1);
                        }
                        else if (psNameColumn.ToUpper().Contains("PESO"))
                        {
                            dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                            dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            dgvDatos.Columns[i].DisplayFormat.FormatString = "{0:##,##0.0000}";

                        }
                        else if (psNameColumn.ToUpper().Contains("#") || psNameColumn.ToUpper().Contains("CANT") || psNameColumn.ToUpper().Contains("UNI"))
                        {
                            dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                            dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            dgvDatos.Columns[i].DisplayFormat.FormatString = "n2";
                            dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:n2}");


                            GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                            item1.FieldName = psNameColumn;
                            item1.SummaryType = SummaryItemType.Sum;
                            item1.DisplayFormat = "{0:n2}";
                            item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                            dgvDatos.GroupSummary.Add(item1);
                        }
                        else
                        {
                            dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                            dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            dgvDatos.Columns[i].DisplayFormat.FormatString = "c2";
                            dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");


                            GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                            item1.FieldName = psNameColumn;
                            item1.SummaryType = SummaryItemType.Sum;
                            item1.DisplayFormat = "{0:c2}";
                            item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                            dgvDatos.GroupSummary.Add(item1);
                        }
                    }
                    else if (dt.Columns[i].DataType == typeof(int))
                    {
                        var psNameColumn = dgvDatos.Columns[i].FieldName;

                        if (psNameColumn.ToUpper().Contains("#") || psNameColumn.ToUpper().Contains("CANT") || psNameColumn.ToUpper().Contains("UNI"))
                        {
                            dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                            dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            dgvDatos.Columns[i].DisplayFormat.FormatString = "n";
                            dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:#,#}");


                            GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                            item1.FieldName = psNameColumn;
                            item1.SummaryType = SummaryItemType.Sum;
                            item1.DisplayFormat = "{0:#,#}";
                            item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                            dgvDatos.GroupSummary.Add(item1);
                        }
                    }

                }

            }


            dgvDatos.OptionsBehavior.Editable = false;
            dgvDatos.OptionsCustomization.AllowColumnMoving = false;
            dgvDatos.OptionsView.ColumnAutoWidth = false;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            //dgvDatos.OptionsView.BestFitMode = GridBestFitMode.Full;
            dgvDatos.BestFitColumns();

            xtraTabControl1.SelectedTabPageIndex = 0;

        }

        private DataSet lCalcular(bool tbGrabar = false)
        {
            DataSet dataSet = new DataSet();
            var dt = loLogicaNegocio.gCalcular(int.Parse(cmbEmpleado.EditValue.ToString()), cmbMotivo.EditValue.ToString(), dtpFechaFinContrato.Value,clsPrincipal.gsUsuario,txtObservacion.Text, tbGrabar,Convert.ToDecimal(txtValorComisiones.EditValue.ToString()), out dataSet);

            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;

            dgvDatos.PopulateColumns();

            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                dgvDatos.Columns[i].OptionsColumn.ReadOnly = true;
                // Quita numeral del nombre de la columna, sirve para convertir a número
                if (dgvDatos.Columns[i].CustomizationSearchCaption.Contains("#")) dgvDatos.Columns[i].Caption = dgvDatos.Columns[i].CustomizationSearchCaption.Replace("#", "").Trim();

                if (i > 0)
                {
                    if (dt.Columns[i].DataType == typeof(decimal))
                    {
                        var psNameColumn = dgvDatos.Columns[i].FieldName;
                        if (psNameColumn.ToUpper().Contains("%") || psNameColumn.ToUpper().Contains("PORC"))
                        {
                            dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                            dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            dgvDatos.Columns[i].DisplayFormat.FormatString = "p2";

                            //dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:0.00}");

                            //GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                            //item1.FieldName = psNameColumn;
                            //item1.SummaryType = SummaryItemType.Sum;
                            //item1.DisplayFormat = "{0:0.00}";
                            //item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                            //dgvDatos.GroupSummary.Add(item1);

                        }
                        else if (psNameColumn.ToUpper().Contains("PESO"))
                        {
                            dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                            dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            dgvDatos.Columns[i].DisplayFormat.FormatString = "{0:##,##0.0000}";
                        }
                        else if (psNameColumn.ToUpper().Contains("#") || psNameColumn.ToUpper().Contains("CANT") || psNameColumn.ToUpper().Contains("UNI"))
                        {
                            dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                            dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            dgvDatos.Columns[i].DisplayFormat.FormatString = "n2";
                            dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:n2}");


                            GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                            item1.FieldName = psNameColumn;
                            item1.SummaryType = SummaryItemType.Sum;
                            item1.DisplayFormat = "{0:n2}";
                            item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                            dgvDatos.GroupSummary.Add(item1);
                        }
                        else
                        {
                            dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                            dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            dgvDatos.Columns[i].DisplayFormat.FormatString = "c2";
                            dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");

                            GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                            item1.FieldName = psNameColumn;
                            item1.SummaryType = SummaryItemType.Sum;
                            item1.DisplayFormat = "{0:c2}";
                            item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                            dgvDatos.GroupSummary.Add(item1);
                        }
                    }
                    else if (dt.Columns[i].DataType == typeof(int))
                    {
                        var psNameColumn = dgvDatos.Columns[i].FieldName;

                        if (psNameColumn.ToUpper().Contains("#") || psNameColumn.ToUpper().Contains("CANT") || psNameColumn.ToUpper().Contains("UNI"))
                        {
                            dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                            dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            dgvDatos.Columns[i].DisplayFormat.FormatString = "n";
                            dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:#,#}");


                            GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                            item1.FieldName = psNameColumn;
                            item1.SummaryType = SummaryItemType.Sum;
                            item1.DisplayFormat = "{0:#,#}";
                            item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                            dgvDatos.GroupSummary.Add(item1);
                        }
                    }
                }
            }

            
            dgvDatos.OptionsBehavior.Editable = false;
            dgvDatos.OptionsCustomization.AllowColumnMoving = false;
            dgvDatos.OptionsView.ColumnAutoWidth = false;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            //dgvDatos.OptionsView.BestFitMode = GridBestFitMode.Full;
            dgvDatos.BestFitColumns();

            xtraTabControl1.SelectedTabPageIndex = 0;

            return dataSet;
        }


        private bool lbEsValido()
        {
            
            if (cmbEmpleado.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Empleado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (cmbMotivo.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Motivo de finiquito de contrato.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!(cmbEstado.EditValue.ToString() == Diccionario.Pendiente || cmbEstado.EditValue.ToString() == Diccionario.Corregir))
            {
                XtraMessageBox.Show("No es posible calcular, tiene un estado:." + cmbEstado.Text, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(lblIdEmpleadoContrato.Text))
            {
                XtraMessageBox.Show("Persona seleccionado no tiene un contrato activo para calcular:." + cmbEstado.Text, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
           
            return true;
        }

        private void lLimpiar(bool Empleado = true)
        {
            pbCargado = false;
            if (Empleado)
            {
                if ((cmbEmpleado.Properties.DataSource as IList).Count > 0) cmbEmpleado.ItemIndex = 0;
                lblIdEmpleadoContrato.Text = "";
                txtNo.Text = "";
            }
            txtValorComisiones.EditValue = 0;
            if ((cmbMotivo.Properties.DataSource as IList).Count > 0) cmbMotivo.ItemIndex = 0;
            cmbEstado.EditValue = Diccionario.Pendiente;
            dtpFechaFinContrato.Value = DateTime.Now;
            cmbEmpleado.Focus();
            txtObservacion.Text = "";
            //lId = 0;
            gcDatos.DataSource = null;
            pbCargado = true;
            

            bsRubrosManuales.DataSource = new List<RubroManual>();
            gcRubrosManuales.DataSource = bsRubrosManuales;

            bsRolComisiones.DataSource = new List<RolManual>();
            gcRol.DataSource = bsRolComisiones;

            LValidarBotones();

            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = false;
            if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Enabled = true;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Enabled = true;
            if (tstBotones.Items["btnPreliminar"] != null) tstBotones.Items["btnPreliminar"].Enabled = true;
            


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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Primero, txtNo.Text.Trim()).ToString();
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Anterior, txtNo.Text.Trim()).ToString();
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Siguiente, txtNo.Text.Trim()).ToString();
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Ultimo, txtNo.Text.Trim()).ToString();
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LValidarBotones()
        {
            if (cmbEstado.EditValue.ToString() == Diccionario.Pendiente || cmbEstado.EditValue.ToString() == Diccionario.Corregir)
            {
                if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Enabled = true;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
            }
            else
            {
                if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Enabled = false;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
            }

            if (cmbEstado.EditValue.ToString() == Diccionario.Rechazado)
            {
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
            }
        }
        private void lBuscar()
        {
            //lConsultar();
            List<Liquidacion> poListaObject = loLogicaNegocio.goListarLiquidaciones();
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[]
                                {
                                    new DataColumn("Id",typeof(int)),
                                    new DataColumn("Fecha Cálculo",typeof(DateTime)),
                                    new DataColumn("Nombre"),
                                    new DataColumn("Fecha Inicio Contrato",typeof(DateTime)),
                                    new DataColumn("Fecha Fin Contrato",typeof(DateTime)),
                                    new DataColumn("Motivo"),
                                    new DataColumn("Total"),
                                    new DataColumn("Estado"),
                                });

            poListaObject.ForEach(a =>
            {
                DataRow row = dt.NewRow();
                row["Id"] = a.IdLiquidacion;
                row["Fecha Cálculo"] = a.FechaCalculo.ToString("dd/MM/yyyy");
                row["Nombre"] = a.Nombre;
                row["Fecha Inicio Contrato"] = a.FechaInicioContrato.ToString("dd/MM/yyyy");
                row["Fecha Fin Contrato"] = a.FechaFinContrato.ToString("dd/MM/yyyy");
                row["Motivo"] = a.DesMotivo;
                row["Total"] = a.Total.ToString("c2");
                row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                dt.Rows.Add(row);
            });

            frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
            if (pofrmBuscar.ShowDialog() == DialogResult.OK)
            {
                txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                lConsultar();
            }
        }

        private void lConsultar(bool Empleado = true)
        {
            pbCargado = false;
            Liquidacion poObject = new Liquidacion();
            if (!string.IsNullOrEmpty(txtNo.Text))
            {
                poObject = loLogicaNegocio.goConsularLiquidacionXIdLiquidacion(int.Parse(txtNo.Text));
            }
            else if (!string.IsNullOrEmpty(lblIdEmpleadoContrato.Text))
            {
                poObject = loLogicaNegocio.goConsularLiquidacionXIdEmpleado(int.Parse(lblIdEmpleadoContrato.Text));
            }
            else
            {
                lLimpiar();
            }

            if (poObject != null && poObject.IdLiquidacion > 0)
            {
                txtNo.Text = poObject.IdLiquidacion.ToString();
                txtObservacion.Text = poObject.Observacion;
                lblIdEmpleadoContrato.Text = poObject.IdEmpleadoContrato.ToString();
                cmbEmpleado.EditValue = poObject.IdPersona.ToString();
                cmbMotivo.EditValue = poObject.Motivo;
                dtpFechaFinContrato.Value = poObject.FechaFinContrato;
                cmbEstado.EditValue = poObject.CodigoEstado;
                txtValorComisiones.EditValue = poObject.ValorRolComisiones;
                lListar();

                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
                if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = true;
                if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Enabled = true;
                if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Enabled = true;
                if (tstBotones.Items["btnPreliminar"] != null) tstBotones.Items["btnPreliminar"].Enabled = false;

                LValidarBotones();
            }
            else
            {
                lLimpiar(Empleado);
            }
            pbCargado = true ;

        }


        #endregion

        private void frmTrLiquidacion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                bsRubrosManuales.AddNew();
                ((List<RubroManual>)bsRubrosManuales.DataSource).LastOrDefault().Rubro = Diccionario.Seleccione;
                dgvRubrosManuales.Focus();
                dgvRubrosManuales.ShowEditor();
                dgvRubrosManuales.UpdateCurrentRow();
                dgvRubrosManuales.RefreshData();
                dgvRubrosManuales.FocusedColumn = dgvRubrosManuales.VisibleColumns[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbEmpleado_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    txtNo.Text = "";
                    lblIdEmpleadoContrato.Text = loLogicaNegocio.giConsultaIdEmpleadoCotrato(int.Parse(cmbEmpleado.EditValue.ToString())).ToString();
                    lConsultar(false);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddRol_Click(object sender, EventArgs e)
        {
            try
            {
                bsRolComisiones.AddNew();
                ((List<RolManual>)bsRolComisiones.DataSource).LastOrDefault().Id = 0;
                dgvRol.Focus();
                dgvRol.ShowEditor();
                dgvRol.UpdateCurrentRow();
                dgvRol.RefreshData();
                dgvRol.FocusedColumn = dgvRol.VisibleColumns[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gcDatos_Click(object sender, EventArgs e)
        {

        }

        private void btnDescargar_Click(object sender, EventArgs e)
        {
            try
            {
                clsSpreadSheet.gPlantillaRDEP(dtpFechaFinContrato.Value.Year, int.Parse(cmbEmpleado.EditValue.ToString()));

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
