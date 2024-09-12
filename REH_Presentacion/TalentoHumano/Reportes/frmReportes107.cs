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
using System.Configuration;
using DevExpress.XtraEditors.Repository;
using REH_Presentacion.Comun;
//using _Excel = Microsoft.Office.Interop.Excel;


namespace REH_Presentacion.TalentoHumano.Reportes
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 09/11/2020
    /// Formulario de gestor de consultas
    /// </summary>
    public partial class frmReportes107 : frmBaseTrxDev
    {

        #region Variables

        clsNGestorConsulta loLogicaNegocio;
        string lsQuery;
        string lsTituloReporte;
        string lsDataSet;
        bool lbLandSpace;
        Nullable<int> liFixedColumn;
        public int lIdGestorConsulta;
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        #endregion

        #region Eventos
        public frmReportes107()
        {
            InitializeComponent();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
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


                //Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
                //Microsoft.Office.Interop.Excel.Workbook wb = default(Microsoft.Office.Interop.Excel.Workbook);

                //wb = xlapp.Workbooks.Open("C:\\Users\\vaptidur\\Downloads\\Plantilla_Formulario_107.xls");
                /*
                WorkBook xlsWorkbook = WorkBook.Create(ExcelFileFormat.XLS);
                xlsWorkbook.Metadata.Author = "IronXL";
                WorkSheet xlsSheet = xlsWorkbook.CreateWorkSheet("new_sheet");
                xlsSheet["A1"].Value = "Hello World";
                xlsSheet["A2"].Style.BottomBorder.SetColor("#ff6600");
                xlsSheet["A2"].Style.BottomBorder.Type = IronXL.Styles.BorderType.Double;
                xlsWorkbook.SaveAs("NewExcelFile.xls"); //Save the excel file
                */

                
                //var po = ws.Cells[0, 0];

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
                                sw.WriteLine("<tipoEmpleador>PRIVADO_MIXTO</tipoEmpleador>");
                                sw.WriteLine("<enteSegSocial>IESS</enteSegSocial>");
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

            try
            {
                List<int> piListaimprimir = new List<int>();
                piListaimprimir.Add(161); // Reportes de Cierre de Ventas Mensual
                piListaimprimir.Add(162); // Reportes de Cierre de Ventas Diario
                //if (piListaimprimir.Contains(int.Parse(Tag.ToString().Split(',')[0])))
                var pIdMenu = int.Parse(Tag.ToString().Split(',')[0]);
                if (pIdMenu == 161 || pIdMenu == 162 || pIdMenu == 169 || pIdMenu == 170 || pIdMenu == 307)
                {

                    DataTable poLista = (DataTable)bsDatos.DataSource;
                    if (poLista == null || poLista.Rows.Count == 0)
                    {
                        XtraMessageBox.Show("Debe consultar datos por pantalla para visualizar reporte.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    System.Data.DataSet ds1 = new System.Data.DataSet();
                    string psParametro1 = "";

                    //var dt1 = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC [dbo].[VTASPPRESUPUESTOVENTACLIENTE] '{0}', '{1}' ", txtPar1.Text.Substring(0, 10), txtPar2.Text.Substring(0, 10)));
                    var dt1 = ldtValidaQuery();
                    dt1.TableName = "CierreVentas";
                    ds1.Merge(dt1);
                    if (dt1.Rows.Count > 0)
                    {


                        if (pIdMenu == 162 || pIdMenu == 169)
                        {
                            var pdFechaFin = Convert.ToDateTime(txtPar1.Text.Substring(0, 10));
                            var pdFechaIni = pdFechaFin.AddDays((pdFechaFin.Day * -1) + 1);


                            /******** VALIDACIÓN DE TIPOS DE PRODUCTOS VACIOS ***************************************************************/
                            var dsVal = loLogicaNegocio.goConsultaDataSet(string.Format("EXEC [dbo].[VTASPPRESUPUESTOVENTACLIENTE] '{0}','{1}','0','0','1'", pdFechaIni.ToString("yyyyMMdd"), txtPar1.Text.Substring(0, 10)));
                            var msgVal = "";
                            if (dsVal.Tables[0].Rows.Count >0)
                            {
                                foreach (DataRow item in dsVal.Tables[0].Rows)
                                {
                                    //msgVal = string.Format("{0}Código: {1} - Vendedor: {2} de SAP NO tiene relacionado Vendedor en SAAF.\n",msgVal, item[0].ToString(), item[1].ToString());
                                    msgVal = string.Format("{0}Vendedor: {1} de SAP NO tiene relacionado Vendedor en SAAF. Ir a la opción 'Módulo de Ventas -> Parametrizadores -> Vendedores'.\n", msgVal, item[1].ToString());
                                }
                                
                            }
                            if (dsVal.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow item in dsVal.Tables[1].Rows)
                                {
                                    //msgVal = string.Format("{0}Código: {1} - Grupo de Producto: {2} de SAP NO tiene relacionado Tipo de Producto en SAAF.\n", msgVal, item[0].ToString(), item[1].ToString());
                                    msgVal = string.Format("{0}Grupo de Producto: {1} de SAP NO tiene relacionado Tipo de Producto en SAAF. Ir a la opción 'Módulo de Ventas -> Parametrizadores -> Grupo - Zona - Tipo Producto'\n", msgVal, item[1].ToString());
                                }
                                
                            }
                            if (!string.IsNullOrEmpty(msgVal))
                            {
                                XtraMessageBox.Show(msgVal, "IMPORTANTE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            /****************************************************************************************************************/


                            DataTable dt2 = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC [dbo].[VTASPRESUMENVENTASIVAKL] '{0}', '{1}' ", pdFechaIni.ToString("yyyyMMdd"), txtPar1.Text.Substring(0, 10)));

                            DateTime pdFecha = pdFechaFin;
                            psParametro1 = string.Format(lsTituloReporte, clsComun.gsGetDia(pdFecha), pdFecha.Day, clsComun.gsGetMes(pdFecha.Month).ToLower(), pdFecha.Year);

                            xrptCierreVentasDiario xrpt = new xrptCierreVentasDiario();

                            string psLeyenda = "";
                            DateTime pdFechaLeyenda = new DateTime(2022, 9, 5);
                            if (pdFechaLeyenda.Date >= pdFechaIni.Date && pdFechaLeyenda.Date <= pdFechaFin.Date)
                            {
                                psLeyenda = "Nota: Este reporte no está considerando la factura #10929 por un valor de $221,078.00 que fue emitida el 05/09/2022 por devolución de productos del proveedor FMC. Solicitado por la Gerencia Financiera.";
                            }

                            //Se establece el origen de datos del reporte (El dataset previamente leído)
                            xrpt.Parameters["Titulo"].Value = psParametro1;
                            xrpt.Parameters["KiloLitros"].Value = dt2.Rows[0][1].ToString();
                            xrpt.Parameters["Iva"].Value = dt2.Rows[0][0].ToString();
                            xrpt.Parameters["Leyenda"].Value = psLeyenda;
                            xrpt.DataSource = ds1;
                            //Se invoca la ventana que muestra el reporte.
                            xrpt.RequestParameters = false;
                            xrpt.Parameters["Titulo"].Visible = false;
                            xrpt.Parameters["KiloLitros"].Visible = false;
                            xrpt.Parameters["Iva"].Visible = false;
                            xrpt.Parameters["Leyenda"].Visible = false;

                            using (ReportPrintTool printTool = new ReportPrintTool(xrpt))
                            {
                                printTool.ShowRibbonPreviewDialog();
                            }
                        }
                        else if (pIdMenu == 307)
                        {
                            var pdFechaFin = Convert.ToDateTime(txtPar1.Text.Substring(0, 10));
                            var pdFechaIni = pdFechaFin.AddDays((pdFechaFin.Day * -1) + 1);

                            DataTable dt2 = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC [dbo].[VTASPRESUMENVENTASIVAKL] '{0}', '{1}' ", pdFechaIni.ToString("yyyyMMdd"), txtPar1.Text.Substring(0, 10)));

                            DateTime pdFecha = pdFechaFin;
                            psParametro1 = string.Format(lsTituloReporte, txtPar1.Text.Substring(0, 10), txtPar2.Text.Substring(0, 10));

                            xrptVentasDevolucion xrpt = new xrptVentasDevolucion();

                            string psLeyenda = "";
                            DateTime pdFechaLeyenda = new DateTime(2022, 9, 5);
                            if (pdFechaLeyenda.Date >= pdFechaIni.Date && pdFechaLeyenda.Date <= pdFechaFin.Date)
                            {
                                psLeyenda = "Nota: Este reporte no está considerando la factura #10929 por un valor de $221,078.00 que fue emitida el 05/09/2022 por devolución de productos del proveedor FMC. Solicitado por la Gerencia Financiera.";
                            }

                            //Se establece el origen de datos del reporte (El dataset previamente leído)
                            xrpt.Parameters["Titulo"].Value = psParametro1;
                            xrpt.Parameters["KiloLitros"].Value = dt2.Rows[0][1].ToString();
                            xrpt.Parameters["Iva"].Value = dt2.Rows[0][0].ToString();
                            xrpt.Parameters["Leyenda"].Value = psLeyenda;
                            xrpt.DataSource = ds1;
                            //Se invoca la ventana que muestra el reporte.
                            xrpt.RequestParameters = false;
                            xrpt.Parameters["Titulo"].Visible = false;
                            xrpt.Parameters["KiloLitros"].Visible = false;
                            xrpt.Parameters["Iva"].Visible = false;
                            xrpt.Parameters["Leyenda"].Visible = false;

                            using (ReportPrintTool printTool = new ReportPrintTool(xrpt))
                            {
                                printTool.ShowRibbonPreviewDialog();
                            }
                        }
                        else
                        {

                            /******** VALIDACIÓN DE TIPOS DE PRODUCTOS VACIOS ***************************************************************/
                            var dsVal = loLogicaNegocio.goConsultaDataSet(string.Format("EXEC [dbo].[VTASPPRESUPUESTOVENTACLIENTE] '{0}','{1}','0','0','1'", txtPar1.Text.Substring(0, 10), txtPar2.Text.Substring(0, 10)));
                            var msgVal = "";
                            if (dsVal.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow item in dsVal.Tables[0].Rows)
                                {
                                    msgVal = string.Format("{0}Código: {1} - Vendedor: {2} de SAP NO tiene relacionado Vendedor en SAAF.\n", msgVal, item[0].ToString(), item[1].ToString());
                                }

                            }
                            if (dsVal.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow item in dsVal.Tables[1].Rows)
                                {
                                    msgVal = string.Format("{0}Código: {1} - Grupo de Producto: {2} de SAP NO tiene relacionado Tipo de Producto en SAAF.\n", msgVal, item[0].ToString(), item[1].ToString());
                                }

                            }
                            if (!string.IsNullOrEmpty(msgVal))
                            {
                                XtraMessageBox.Show(msgVal, "IMPORTANTE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            /****************************************************************************************************************/


                            DataTable dt2 = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC [dbo].[VTASPRESUMENVENTASIVAKL] '{0}', '{1}' ", txtPar1.Text.Substring(0, 10), txtPar2.Text.Substring(0, 10)));

                            psParametro1 = string.Format(lsTituloReporte, txtPar1.Text.Substring(0, 10), txtPar2.Text.Substring(0, 10));

                            string psLeyenda = "";
                            DateTime pdFechaLeyenda = new DateTime(2022, 9, 5);
                            if (pdFechaLeyenda.Date >= Convert.ToDateTime(txtPar1.Text.Substring(0, 10)).Date && pdFechaLeyenda.Date <= Convert.ToDateTime(txtPar2.Text.Substring(0, 10)).Date)
                            {
                                psLeyenda = "Nota: Este reporte no está considerando la factura #10929 por un valor de $221,078.00 que fue emitida el 05/09/2022 por devolución de productos del proveedor FMC. Solicitado por la Gerencia Financiera.";
                            }


                            xrptCierreVentasMensual xrpt = new xrptCierreVentasMensual();

                            //Se establece el origen de datos del reporte (El dataset previamente leído)
                            xrpt.Parameters["Titulo"].Value = psParametro1;
                            xrpt.Parameters["KiloLitros"].Value = dt2.Rows[0][1].ToString();
                            xrpt.Parameters["Iva"].Value = dt2.Rows[0][0].ToString();
                            xrpt.Parameters["Leyenda"].Value = psLeyenda;
                            xrpt.DataSource = ds1;

                            //Se invoca la ventana que muestra el reporte.
                            xrpt.RequestParameters = false;
                            xrpt.Parameters["Titulo"].Visible = false;
                            xrpt.Parameters["KiloLitros"].Visible = false;
                            xrpt.Parameters["Iva"].Visible = false;
                            xrpt.Parameters["Leyenda"].Visible = false;

                            using (ReportPrintTool printTool = new ReportPrintTool(xrpt))
                            {
                                printTool.ShowRibbonPreviewDialog();
                            }

                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (pIdMenu == 248)
                {
                    System.Data.DataSet ds = new System.Data.DataSet();
                    var dt = loLogicaNegocio.goConsultaDataTable(string.Format("SELECT Observacion,Nombre,Departamento,Valor FROM [dbo].[REHLVACACIONEMPLEADO] WHERE ANIO = {0} AND MES = {1} ORDER BY Nombre", txtPar1.Text, txtPar2.Text));
                    dt.TableName = "Provision";
                    if (ds.Tables["Provision"] != null) ds.Tables.Remove("Provision");
                    ds.Merge(dt);
                    if (dt.Rows.Count > 0)
                    {
                        var dt2 = loLogicaNegocio.goConsultaDataTable(string.Format("SELECT ISNULL(SUM(Valor),0) Valor FROM REHLVACACIONEMPLEADO WHERE CodigoEstado = 'A' AND CONVERT(DATE,CONCAT(CONVERT(VARCHAR(4),Anio), '-',CONCAT(REPLICATE ('0', 2 - LEN(Mes)), CONVERT(VARCHAR(2),Mes)), '-01')) <= CONVERT(DATE,CONCAT(CONVERT(VARCHAR(4),{0}), '-',CONCAT(REPLICATE ('0', 2 - LEN({1})), CONVERT(VARCHAR(2),{1})), '-01'))", txtPar1.Text, txtPar2.Text));

                        xrptProvisionVacaciones xrpt = new xrptProvisionVacaciones();

                        //Se establece el origen de datos del reporte (El dataset previamente leído)
                        xrpt.Parameters["parameter1"].Value = string.Format("LISTADO DE PROVISIONES DE VACACIONES {0} - {1}",clsComun.gsGetMes(int.Parse(txtPar2.Text)),txtPar1.Text);
                        xrpt.Parameters["parameter2"].Value = dt2.Rows[0][0].ToString();
                        xrpt.DataSource = ds;
                        //Se invoca la ventana que muestra el reporte.
                        xrpt.RequestParameters = false;
                        xrpt.Parameters["parameter1"].Visible = false;
                        xrpt.Parameters["parameter2"].Visible = false;

                        using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                        {
                            printTool.ShowRibbonPreviewDialog();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (pIdMenu == 321)
                {
                    System.Data.DataSet ds = new System.Data.DataSet();
                    var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPVINCULACIONES '{0}','{1}' ", txtPar1.Text.Substring(0, 10), txtPar2.Text.Substring(0, 10)));
                    dt.TableName = "Det";
                    if (ds.Tables["Det"] != null) ds.Tables.Remove("Det");
                    ds.Merge(dt);
                    if (dt.Rows.Count > 0)
                    {
                        xrptVinculaciones xrptVinc = new xrptVinculaciones();
                        //Se establece el origen de datos del reporte (El dataset previamente leído)
                        xrptVinc.Parameters["Rol"].Value = string.Format("VINCULACIONES DEL {0} AL {1}", txtPar1.Text.Substring(0, 10), txtPar2.Text.Substring(0, 10));
                        xrptVinc.DataSource = ds;

                        //Se invoca la ventana que muestra el reporte.
                        xrptVinc.RequestParameters = false;
                        xrptVinc.Parameters["Rol"].Visible = false;

                        using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrptVinc))
                        {
                            printTool.ShowRibbonPreviewDialog();
                        }
                    }
                }
                else if (pIdMenu == 315)
                {
                    System.Data.DataSet ds = new System.Data.DataSet();
                    var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTAHISTORICOPROVISIONES   '{0}','{1}' ", txtPar1.Text, txtPar2.Text));
                    dt.TableName = "Vac";
                    if (ds.Tables["Vac"] != null) ds.Tables.Remove("Vac");
                    ds.Merge(dt);
                    if (dt.Rows.Count > 0)
                    {
                        xrptProvisionesVacacionesAcum xrpt = new xrptProvisionesVacacionesAcum();

                        //Se establece el origen de datos del reporte (El dataset previamente leído)
                        xrpt.Parameters["parameter1"].Value = string.Format("PROVISIÓN DE VACACIONES\n{0} / {1}", clsComun.gsGetMes(int.Parse(txtPar2.Text)), txtPar1.Text);
                        xrpt.Parameters["cab1"].Value = string.Format("Prov. Acum. {0}/{1}", clsComun.gsGetMes(new DateTime(int.Parse(txtPar1.Text), int.Parse(txtPar2.Text),1).AddMonths(-1).Month).ToLower(), new DateTime(int.Parse(txtPar1.Text), int.Parse(txtPar2.Text), 1).AddMonths(-1).Year);
                        xrpt.Parameters["cab2"].Value = string.Format("Provisión {0}", clsComun.gsGetMes(new DateTime(int.Parse(txtPar1.Text), int.Parse(txtPar2.Text), 1)).ToLower());
                        xrpt.Parameters["cab3"].Value = string.Format("Saldo acumulado {0}", clsComun.gsGetMes(new DateTime(int.Parse(txtPar1.Text), int.Parse(txtPar2.Text), 1)).ToLower());
                        xrpt.DataSource = ds;
                        //Se invoca la ventana que muestra el reporte.
                        xrpt.RequestParameters = false;
                        xrpt.Parameters["parameter1"].Visible = false;
                        xrpt.Parameters["cab1"].Visible = false;
                        xrpt.Parameters["cab2"].Visible = false;
                        xrpt.Parameters["cab3"].Visible = false;

                        using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                        {
                            printTool.ShowRibbonPreviewDialog();
                        }
                    }
                }
                else
                {
                    System.Data.DataSet ds = new System.Data.DataSet();
                    DataTable dt = ldtValidaQuery();
                    dt.TableName = lsDataSet;
                    ds.Merge(dt);

                    string psPar1 = "";
                    string psPar2 = "";
                    string psPar3 = "";
                    string psPar4 = "";

                    if (txtPar1.Properties.Mask.MaskType == DevExpress.XtraEditors.Mask.MaskType.DateTime)
                    {
                        if (txtPar1.Text.Length > 10)
                        {
                            psPar1 = txtPar1.Text.Substring(0, 10);
                        }
                        else
                        {
                            psPar1 = txtPar1.Text;
                        }

                    }
                    else
                    {
                        psPar1 = txtPar1.Text;
                    }

                    if (txtPar2.Properties.Mask.MaskType == DevExpress.XtraEditors.Mask.MaskType.DateTime)
                    {
                        if (txtPar2.Text.Length > 10)
                        {
                            psPar2 = txtPar2.Text.Substring(0, 10);
                        }
                        else
                        {
                            psPar2 = txtPar1.Text;
                        }

                    }
                    else
                    {
                        psPar2 = txtPar2.Text;
                    }

                    if (txtPar3.Properties.Mask.MaskType == DevExpress.XtraEditors.Mask.MaskType.DateTime)
                    {
                        if (txtPar3.Text.Length > 10)
                        {
                            psPar3 = txtPar3.Text.Substring(0, 10);
                        }
                        else
                        {
                            psPar3 = txtPar1.Text;
                        }

                    }
                    else
                    {
                        psPar3 = txtPar3.Text;
                    }

                    if (txtPar4.Properties.Mask.MaskType == DevExpress.XtraEditors.Mask.MaskType.DateTime)
                    {
                        if (txtPar4.Text.Length > 10)
                        {
                            psPar4 = txtPar4.Text.Substring(0, 10);
                        }
                        else
                        {
                            psPar4 = txtPar1.Text;
                        }

                    }
                    else
                    {
                        psPar4 = txtPar4.Text;
                    }


                    if (dt.Rows.Count > 0)
                    {
                        string psParametro = string.Empty;
                        if (lsTituloReporte.Contains("{3}"))
                        {
                            psParametro = string.Format(lsTituloReporte, psPar1, psPar2, psPar3, psPar4);
                        }
                        else if (lsTituloReporte.Contains("{2}"))
                        {
                            psParametro = string.Format(lsTituloReporte, psPar1, psPar2, psPar3);
                        }
                        else if (lsTituloReporte.Contains("{1}"))
                        {
                            psParametro = string.Format(lsTituloReporte, psPar1, psPar2);
                        }
                        else if (lsTituloReporte.Contains("{0}"))
                        {
                            psParametro = string.Format(lsTituloReporte, psPar1);
                        }
                        else
                        {
                            psParametro = lsTituloReporte;
                        }

                        xrptGeneral xrptGen = new xrptGeneral();
                        xrptGen.dt = dt;
                        xrptGen.Landscape = lbLandSpace;
                        xrptGen.Parameters["parameter1"].Value = psParametro;

                        xrptGen.RequestParameters = false;
                        xrptGen.Parameters["parameter1"].Visible = false;
                        using (ReportPrintTool printTool = new ReportPrintTool(xrptGen))
                        {
                            printTool.ShowRibbonPreviewDialog();
                        }

                        //xrptVacaciones xrpt = new xrptVacaciones();
                        //xrpt.Parameters["parameter1"].Value = psParametro;
                        //xrpt.DataSource = ds;
                        //xrpt.RequestParameters = false;
                        //xrpt.Parameters["parameter1"].Visible = false;
                        //using (ReportPrintTool printTool = new ReportPrintTool(xrpt))
                        //{
                        //    printTool.ShowRibbonPreviewDialog();
                        //}
                    }
                }

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

        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
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
                    clsSpreadSheet.gPlantillaRDEP(int.Parse(txtPar1.Text), int.Parse(poLista.Rows[piIndex]["IdPersona"].ToString()));
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
            lblPar3.Visible = false;
            lblPar4.Visible = false;
            txtPar1.Visible = false;
            txtPar2.Visible = false;
            txtPar3.Visible = false;
            txtPar4.Visible = false;
        }

        private DataTable ldtValidaQuery()
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
                if (piParametro == 3 && txtPar3.Visible == false)
                {
                    psQuery = psQuery.Replace("#PARAMETRO3", clsPrincipal.gsUsuario);
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtPar3.Text))
                    {
                        psQuery = psQuery.Replace("#PARAMETRO3", txtPar3.Text.ToString());
                    }
                    else
                    {
                        XtraMessageBox.Show("Es necesario ingresar dato:" + lblPar3.Text, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pbValido = false;
                    }
                }

            }

            if (psQuery.Contains("#PARAMETRO4"))
            {
                if (piParametro == 4 && txtPar4.Visible == false)
                {
                    psQuery = psQuery.Replace("#PARAMETRO4", clsPrincipal.gsUsuario);
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtPar4.Text))
                    {
                        psQuery = psQuery.Replace("#PARAMETRO4", txtPar4.Text.ToString());
                    }
                    else
                    {
                        XtraMessageBox.Show("Es necesario ingresar dato:" + lblPar4.Text, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        pbValido = false;
                    }
                }

            }

            if (pbValido)
            {
                if (int.Parse(Tag.ToString().Split(',')[0]) == 248)
                {
                    dt = loLogicaNegocio.goConsultaDataTable(string.Format("SELECT Observacion,Nombre,Departamento,Valor FROM [dbo].[REHLVACACIONEMPLEADO] WHERE ANIO = {0} AND MES = {1} ORDER BY Observacion,Nombre", txtPar1.Text, txtPar2.Text));
                }
                else
                {
                    dt = loLogicaNegocio.goConsultaDataTable(psQuery);
                }
            }

            return dt;
        }

        private void lBuscar()
        {
            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();

            DataTable dt = ldtValidaQuery();

            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;
            dgvDatos.PopulateColumns();

            if (dt.Rows.Count > 0)
            {
                if (liFixedColumn != null)
                {
                    int piCant = liFixedColumn ?? 0;
                    dgvDatos.FixedLineWidth = piCant;

                    for (int x = 0; x <= piCant; x++)
                    {
                        dgvDatos.Columns[x].Fixed = FixedStyle.Left;
                    }
                }


                for (int i = 0; i < dgvDatos.Columns.Count; i++)
                {
                    if (dgvDatos.Columns[i].Name == "colSel")
                    {
                        dgvDatos.Columns[i].OptionsColumn.ReadOnly = false;
                    }
                    else
                    {
                        dgvDatos.Columns[i].OptionsColumn.ReadOnly = true;
                    }
                    
                    // Quita numeral del nombre de la columna, sirve para convertir a número
                    if (dgvDatos.Columns[i].CustomizationSearchCaption.Contains("#")) dgvDatos.Columns[i].Caption = dgvDatos.Columns[i].CustomizationSearchCaption.Replace("#", "").Trim();

                    if (i > 1)
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
            }

            var pIdMenu = int.Parse(Tag.ToString().Split(',')[0]);
            if (pIdMenu == 161 || pIdMenu == 162 || pIdMenu == 169 || pIdMenu == 170 || pIdMenu == 307)
            {
                dgvDatos.Columns["Ord"].Visible = false;
                dgvDatos.Columns["Orden"].Visible = false;
            }

            if (pIdMenu == 154 || pIdMenu == 152 || pIdMenu == 136)
            {
                clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDatos.Columns["107"], "Formulario 107", Diccionario.ButtonGridImage.printer_16x16);
                dgvDatos.Columns["IdPersona"].Visible = false;
            }

            dgvDatos.OptionsBehavior.Editable = true;
            dgvDatos.OptionsCustomization.AllowColumnMoving = true;
            dgvDatos.OptionsView.ColumnAutoWidth = false;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            dgvDatos.OptionsView.BestFitMode = GridBestFitMode.Full;
            dgvDatos.BestFitColumns();
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
                            else if (piContador == 3)
                            {
                                lFormatControl(lblPar3, txtPar3, item);
                            }
                            else if (piContador == 4)
                            {
                                lFormatControl(lblPar4, txtPar4, item);
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

        private void btnTodos_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable poLista = (DataTable)bsDatos.DataSource;
                if (poLista != null && poLista.Rows.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcDatos, Text + ".xlsx", psFilter);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable poLista = (DataTable)bsDatos.DataSource;
                bool pbValor = false;
                if (poLista.Rows.Count > 0)
                {
                    pbValor = !bool.Parse(poLista.Rows[0][0].ToString());
                }
                foreach (DataRow item in poLista.Rows)
                {
                    item[0] = pbValor;
                }

                dgvDatos.RefreshData();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDescargar_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> pListId = new List<int>();
                DataTable poLista = (DataTable)bsDatos.DataSource;

                foreach (DataRow item in poLista.Rows)
                {
                    if (bool.Parse(item[0].ToString()))
                    {
                        pListId.Add(int.Parse(item["IdPersona"].ToString()));
                    }
                }

                if (pListId.Count > 0)
                {
                    clsSpreadSheet.gDescargarPlantillaRDEP(int.Parse(txtPar1.Text), pListId);
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
