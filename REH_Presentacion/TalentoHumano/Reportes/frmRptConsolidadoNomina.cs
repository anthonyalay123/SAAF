using DevExpress.XtraGrid.Views.Grid;
using REH_Negocio;
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
using COM_Negocio;
using REH_Presentacion.Reportes;

namespace REH_Presentacion.TalentoHumano.Reportes
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 09/11/2020
    /// Formulario de gestor de consultas
    /// </summary>
    public partial class frmRptConsolidadoNomina : frmBaseTrxDev
    {

        #region Variables

        clsNGestorConsulta loLogicaNegocio;
        string lsQuery;
        string lsTituloReporte;
        string lsDataSet;
        bool lbLandSpace;
        Nullable<int> liFixedColumn;
        public int lIdGestorConsulta;
        #endregion

        #region Eventos
        public frmRptConsolidadoNomina()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNGestorConsulta();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                //cmbReporte.Focus();
                //clsComun.gLLenarCombo(ref cmbReporte, loLogicaNegocio.goConsultarComboReportes());
                var piGestor = loLogicaNegocio.giConsultaId(int.Parse(Tag.ToString().Split(',')[0]));
                if (piGestor != 0) lIdGestorConsulta = piGestor;
                lCargarEventosBotones();
                lCargarControles();
                if (!pnc1.Visible)
                {
                    lBuscar();
                }
                //loLogicaNegocio.ProbarConexion();
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
                // lLimpiar();
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

                dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    if (xtraTabControl1.SelectedTabPageIndex == 0)
                    {
                        DataTable poLista = (DataTable)bsDatos.DataSource;
                        if (poLista != null && poLista.Rows.Count > 0)
                        {
                            string psFilter = "Files(*.xlsx;)|*.xlsx;";
                            clsComun.gSaveFile(gcDatos, "Consolidado_Nomina_Todos" + ".xlsx", psFilter);
                        }
                        else
                        {
                            XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else if (xtraTabControl1.SelectedTabPageIndex == 1)
                    {
                        DataTable poLista = (DataTable)bsActivos.DataSource;
                        if (poLista != null && poLista.Rows.Count > 0)
                        {
                            string psFilter = "Files(*.xlsx;)|*.xlsx;";
                            clsComun.gSaveFile(gcActivos, "Consolidado_Nomina_Activos" + ".xlsx", psFilter);
                        }
                        else
                        {
                            XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else if (xtraTabControl1.SelectedTabPageIndex == 2)
                    {
                        DataTable poLista = (DataTable)bsInactivos.DataSource;
                        if (poLista != null && poLista.Rows.Count > 0)
                        {
                            string psFilter = "Files(*.xlsx;)|*.xlsx;";
                            clsComun.gSaveFile(gcInactivos, "Consolidado_Nomina_Inactivos" + ".xlsx", psFilter);
                        }
                        else
                        {
                            XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// Evento del botón Exportar, Exporta a TXT.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportarTxt_Click(object sender, EventArgs e)
        {
            try
            {

                dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    DataTable poLista = (DataTable)bsDatos.DataSource;
                    if (poLista != null && poLista.Rows.Count > 0)
                    {
                        List<CuerpoRegionTxt> poListaMov = new List<CuerpoRegionTxt>();
                        List<string> psCodigos = new List<string>();

                        foreach (DataRow item in poLista.Rows)
                        {
                            CuerpoRegionTxt cuerpo = new CuerpoRegionTxt();
                            cuerpo.Cuerpo = item["Cuerpo"].ToString();
                            cuerpo.Region = item["CodigoRegionIess"].ToString();

                            if (!psCodigos.Contains(item["CodigoRegionIess"].ToString()))
                            {
                                psCodigos.Add(item["CodigoRegionIess"].ToString());
                            }
                            poListaMov.Add(cuerpo);
                        }

                        foreach (var item in psCodigos)
                        {
                            List<CuerpoRegionTxt> poListaNew = poListaMov.Where(x => x.Region == item).Select(x => new CuerpoRegionTxt { Cuerpo = x.Cuerpo }).ToList();

                            try
                            {

                                SaveFileDialog sfd = new SaveFileDialog();
                                sfd.Filter = ".TXT Files (*.txt)|*.txt";
                                sfd.FileName = "Ajuste_Iess_" + txtPar1.Text.Trim() + "_" + txtPar1.Text.Trim() + "_" + item + ".txt";
                                if (sfd.ShowDialog() == DialogResult.OK)
                                {
                                    string path = Path.GetDirectoryName(sfd.FileName);
                                    string filename = Path.GetFileNameWithoutExtension(sfd.FileName);
                                    string psRuta = sfd.FileName;
                                    //Pass the filepath and filename to the StreamWriter Constructor
                                    StreamWriter sw = new StreamWriter(psRuta);
                                    int piTotFilas = poListaNew.Count();
                                    int piCont = 0;
                                    foreach (var reigstro in poListaNew)
                                    {
                                        piCont++;
                                        if (piCont == piTotFilas)
                                        {
                                            sw.Write(reigstro.Cuerpo);
                                        }
                                        else
                                        {
                                            sw.WriteLine(reigstro.Cuerpo);
                                        }

                                    }

                                    sw.Close();

                                }

                            }
                            catch (Exception exp)
                            {
                                XtraMessageBox.Show(exp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }

                        XtraMessageBox.Show("Guardado Exitosamente", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// Evento del botón Exportar, Exporta a Xml.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportarXml_Click(object sender, EventArgs e)
        {
            try
            {

                dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    DataTable poLista = loLogicaNegocio.gdtConsultarRDEPXml(int.Parse(txtPar1.EditValue.ToString()));
                    if (poLista != null && poLista.Rows.Count > 0)
                    {
                        List<CuerpoRegionTxt> poListaMov = new List<CuerpoRegionTxt>();

                        foreach (DataRow item in poLista.Rows)
                        {
                            CuerpoRegionTxt cuerpo = new CuerpoRegionTxt();
                            cuerpo.Cuerpo = item["Cuerpo"].ToString();
                            poListaMov.Add(cuerpo);
                        }

                        try
                        {

                            SaveFileDialog sfd = new SaveFileDialog();
                            sfd.Filter = ".XML Files (*.xml)|*.xml";
                            sfd.FileName = "RDEP_" + txtPar1.Text.Trim() + ".xml";
                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                string path = Path.GetDirectoryName(sfd.FileName);
                                string filename = Path.GetFileNameWithoutExtension(sfd.FileName);
                                string psRuta = sfd.FileName;
                                //Pass the filepath and filename to the StreamWriter Constructor
                                StreamWriter sw = new StreamWriter(psRuta);
                                int piTotFilas = poListaMov.Count();

                                sw.WriteLine("<?xml version=\"1.0\" encoding=\"ISO-8859-1\" standalone =\"yes\"?>");
                                sw.WriteLine("<rdep>");
                                sw.WriteLine("<numRuc>" + clsPrincipal.gsRucEmpresa + "</numRuc>");
                                sw.WriteLine("<anio>" + txtPar1.Text + "</anio>");
                                sw.WriteLine("<retRelDep>");


                                foreach (var reigstro in poListaMov)
                                {
                                    sw.WriteLine(reigstro.Cuerpo);
                                }

                                sw.WriteLine("</retRelDep>");
                                sw.Write("</rdep>");

                                sw.Close();
                            }

                        }
                        catch (Exception exp)
                        {
                            XtraMessageBox.Show(exp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        XtraMessageBox.Show("Guardado Exitosamente", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        /// <summary>
        /// Evento del botón Imprimir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            
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


        private void btnRechazar_Click(object sender, EventArgs e)
        {
            try
            {


                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                DataTable poLista = (DataTable)bsDatos.DataSource;
                int pIdTra = int.Parse(poLista.Rows[piIndex][0].ToString());

                if (pIdTra != 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var result = XtraInputBox.Show("Ingrese comentario", "Rechazar", "");
                        if (string.IsNullOrEmpty(result))
                        {
                            XtraMessageBox.Show("Debe agregar comentario para poder rechazar", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var psMsg = new clsNOrdenPago().gsRechazarFacturaPago(pIdTra, result, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, "No es posible eliminar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        lConsultarBoton();
                    }
                }
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
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                DataTable poLista = (DataTable)bsDatos.DataSource;
                int pIdTra = int.Parse(poLista.Rows[piIndex][0].ToString());
                var pIdMenu = int.Parse(Tag.ToString().Split(',')[0]);

                if (pIdMenu == 200) // Listado de facturas de pago aprobadas
                {
                    if (pIdTra != 0)
                    {
                        DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {
                            new clsNOrdenPago().gsEliminarFacturaPago(pIdTra, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                            XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lConsultarBoton();
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
            if (tstBotones.Items["btnExportarTxt"] != null) tstBotones.Items["btnExportarTxt"].Click += btnExportarTxt_Click;
            if (tstBotones.Items["btnExportarXml"] != null) tstBotones.Items["btnExportarXml"].Click += btnExportarXml_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Click += btnRechazar_Click;

            //txtPar1.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar2.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar3.KeyDown += new KeyEventHandler(EnterEqualTab);
            //txtPar4.KeyDown += new KeyEventHandler(EnterEqualTab);


        }

        private bool lbEsValido()
        {
            //if (cmbReporte.EditValue.ToString() == Diccionario.Seleccione)
            //{
            //    XtraMessageBox.Show("Seleccione Empleado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}

            return true;
        }

        private void lLimpiar(bool tbSoloDetalle = false)
        {
            if (!tbSoloDetalle)
            {
                lsQuery = string.Empty;
            }

            lblPar1.Visible = false;
            lblPar2.Visible = false;
            txtPar1.Visible = false;
            txtPar2.Visible = false;
        }

        private DataTable ldtValidaQuery(string tsTipo)
        {
            DataTable dt = new DataTable();

            string psQuery = lsQuery;
            bool pbValido = true;
            string[] psArreglo;
            int piParametro = 0;
            if (psQuery.ToUpper().Contains("@USUARIO"))
            {
                string[] separatingStrings = { "@USUARIO" };
                psArreglo = psQuery.ToUpper().Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                if (psArreglo[1].Contains("#PARAMETRO1"))
                {
                    piParametro = 1;
                }
                else if (psArreglo[1].Contains("#PARAMETRO2"))
                {
                    piParametro = 2;
                }
                else if (psArreglo[1].Contains("#PARAMETRO3"))
                {
                    piParametro = 3;
                }
                else if (psArreglo[1].Contains("#PARAMETRO4"))
                {
                    piParametro = 4;
                }

            }


            if (psQuery.Contains("#PARAMETRO1"))
            {
                if (piParametro == 1 && txtPar1.Visible == false)
                {
                    psQuery = psQuery.Replace("#PARAMETRO1", clsPrincipal.gsUsuario);
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtPar1.Text))
                    {
                        psQuery = psQuery.Replace("#PARAMETRO1", txtPar1.Text.ToString());
                    }
                    else
                    {
                        XtraMessageBox.Show("Es necesario ingresar dato:" + lblPar1.Text, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pbValido = false;
                    }
                }

            }

            if (psQuery.Contains("#PARAMETRO2"))
            {
                if (piParametro == 2 && txtPar2.Visible == false)
                {
                    psQuery = psQuery.Replace("#PARAMETRO2", clsPrincipal.gsUsuario);
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtPar2.Text))
                    {
                        psQuery = psQuery.Replace("#PARAMETRO2", txtPar2.Text.ToString());
                    }
                    else
                    {
                        XtraMessageBox.Show("Es necesario ingresar dato:" + lblPar2.Text, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pbValido = false;
                    }
                }

            }

            if (psQuery.Contains("#PARAMETRO3"))
            {
                psQuery = psQuery.Replace("#PARAMETRO3", tsTipo);
            }

            dt = loLogicaNegocio.goConsultaDataTable(psQuery);

            return dt;
        }

        private void lBuscar()
        {
            /***********************************************************************************/
            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();

            DataTable dt1 = ldtValidaQuery("T");

            bsDatos.DataSource = dt1;
            gcDatos.DataSource = bsDatos;

            clsComun.gFormatearColumnasGrid(dgvDatos);
            clsComun.gOrdenarColumnasGridFull(dgvDatos);
            /***********************************************************************************/
            /***********************************************************************************/
            gcActivos.DataSource = null;
            dgvActivos.Columns.Clear();

            DataTable dt2 = ldtValidaQuery("A");

            bsActivos.DataSource = dt1;
            gcActivos.DataSource = bsActivos;

            clsComun.gFormatearColumnasGrid(dgvActivos);
            clsComun.gOrdenarColumnasGridFull(dgvActivos);
            /***********************************************************************************/
            /***********************************************************************************/
            gcInactivos.DataSource = null;
            dgvInactivos.Columns.Clear();

            DataTable dt3 = ldtValidaQuery("I");

            bsInactivos.DataSource = dt3;
            gcInactivos.DataSource = bsInactivos;

            clsComun.gFormatearColumnasGrid(dgvInactivos);
            clsComun.gOrdenarColumnasGridFull(dgvInactivos);
        }

        private void lCargarControles()
        {
            lLimpiar();
            if (lIdGestorConsulta != 0)
            {
                var poGestorConsulta = loLogicaNegocio.goGestorConsulta(lIdGestorConsulta);

                if (poGestorConsulta != null)
                {
                    lbLandSpace = poGestorConsulta.botonImprimir;

                    //if (!poGestorConsulta.botonImprimir)
                    //{
                    //    if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Visible = false;
                    //}

                    lsQuery = poGestorConsulta.Query;
                    lsTituloReporte = poGestorConsulta.TituloReporte;
                    lsDataSet = poGestorConsulta.DataSet;
                    liFixedColumn = poGestorConsulta.FixedColumn;

                    var poLista = loLogicaNegocio.goGestorConsultaDetalle(lIdGestorConsulta);
                    if (poLista != null && poLista.Count > 0)
                    {
                        int piContador = 0;

                        foreach (var item in poLista.OrderBy(x => x.Orden))
                        {
                            piContador++;

                            if (piContador == 1)
                            {
                                lFormatControl(lblPar1, txtPar1, item);
                            }
                            else if (piContador == 2)
                            {
                                lFormatControl(lblPar2, txtPar2, item);
                            }
                            
                        }
                    }
                    else
                    {
                        pnc1.Visible = false;
                        this.gcDatos.Location = new System.Drawing.Point(5, 5);
                        this.gcDatos.Size = new System.Drawing.Size(895, 441);
                    }

                }
                else
                {
                    lsQuery = string.Empty;
                }
            }
        }

        private void lFormatControl(Label lbl, TextEdit txt, GestorConsultaDetalle item)
        {
            lbl.Visible = true;
            lbl.Text = item.Nombre;
            txt.Visible = true;
            txt.Properties.MaxLength = item.Longitud;
            if (item.TipoDato.ToUpper() == "DATE" || item.TipoDato.ToUpper() == "DATETIME")
            {
                txt.Properties.Mask.EditMask = "d";
                txt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                if (lbl.Text.ToLower().Contains("ini"))
                {
                    txt.EditValue = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, 1);
                }
                else
                {
                    txt.EditValue = DateTime.Now.Date;
                }


            }
            else if (item.TipoDato.ToUpper() == "DECIMAL")
            {
                txt.Properties.Mask.EditMask = "n2";
                //txt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                //txt.Properties.Mask.UseMaskAsDisplayFormat = true;
                //txt.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            }
            else if (item.TipoDato.ToUpper() == "INT")
            {
                txt.KeyPress += new KeyPressEventHandler(SoloNumeros);
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

    }
}
