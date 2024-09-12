using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using REH_Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GEN_Entidad;
using System.Data;
using System.IO;
using System.Data.OleDb;
using DevExpress.XtraGrid;
using DevExpress.XtraReports.UI;
using System.Drawing.Printing;
using DevExpress.XtraPrinting;
using REH_Presentacion.Reportes;
using COM_Negocio;
using REH_Presentacion.Compras.Reportes;
using DevExpress.XtraEditors.Controls;
using static GEN_Entidad.Diccionario;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using DevExpress.XtraGrid.Views.Grid;
using Microsoft.Office.Interop.Excel;
using DevExpress.Data;
using System.Text;
using REH_Presentacion.Cobranza.Reportes.Rpt;
using COB_Negocio;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Columns;

namespace REH_Presentacion
{
    public static class clsComun
    {
        
        private static PrinterSettings prnSettings;

        /// <summary>
        /// Método Común que Elimina caracteres especiales
        /// </summary>
        /// <param name="tsCadena"></param>
        /// <returns></returns>
        public static string gRemoverCaracteresEspeciales(string tsCadena)
        {
            //return Regex.Replace(str, "[^a-zA-Z0-9_.- ]+", "", RegexOptions.Compiled);
            return Regex.Replace(tsCadena, @"[^0-9A-Za-z ]", "", RegexOptions.None);
        }
        
        /// <summary>
        /// Método Común que Elimina caracteres especiales
        /// </summary>
        /// <param name="tsCadena"></param>
        /// <returns></returns>
        public static string gRemoverCaracteresEspecialesNumero(string tsCadena)
        {
            //return Regex.Replace(str, "[^a-zA-Z0-9_.- ]+", "", RegexOptions.Compiled);
            return Regex.Replace(tsCadena, @"[^0-9A-Za-z. ]", "", RegexOptions.None);
        }

        /// <summary>
        /// Función genérica para llenar un combo
        /// </summary>
        /// <param name="toCmb"></param>
        /// <param name="toLista"></param>
        /// <param name="tbAgregaSeleccione"></param>
        public static void gLLenarCombo(ref LookUpEdit toCmb, List<Combo> toLista, bool tbAgregaSeleccione = false, bool tbAgregaTodas = false, string tsAgregarRegistro = "", bool tbAgregaTodos = false)
        {
            var poLista = toLista;
            if (!string.IsNullOrEmpty(tsAgregarRegistro)) poLista.Insert(0, new Combo { Codigo = "N", Descripcion = tsAgregarRegistro });
            if (tbAgregaSeleccione) poLista.Insert(0, new Combo { Codigo = Diccionario.Seleccione, Descripcion = Diccionario.DesSeleccione });
            if (tbAgregaTodas) poLista.Insert(0, new Combo { Codigo = Diccionario.Seleccione, Descripcion = Diccionario.DesTodas });
            if (tbAgregaTodos) poLista.Insert(0, new Combo { Codigo = Diccionario.Seleccione, Descripcion = Diccionario.DesTodos });
            toCmb.Properties.DataSource = poLista;
            toCmb.Properties.ValueMember = "Codigo";
            toCmb.Properties.DisplayMember = "Descripcion";
            toCmb.Properties.ForceInitialize();
            toCmb.Properties.PopulateColumns();
            toCmb.Properties.Columns["Codigo"].Visible = false;
            if ((toCmb.Properties.DataSource as IList).Count > 0) toCmb.ItemIndex = 0;
        }

        public static string gConcatenarCorreos(List<string> listaCorreos, string tipoSigno)
        {
            return string.Join(tipoSigno, listaCorreos);
        }


        /// <summary>
        /// Función genérica para llenar un Checked combo
        /// </summary>
        /// <param name="toCmb"></param>
        /// <param name="toLista"></param>
        /// <param name="tbAgregaSeleccione"></param>
        public static void gLLenarCombo(ref CheckedComboBoxEdit toCmb, List<Combo> toLista, bool tbAgregaSeleccione = false, bool tbAgregaTodas = false, string tsAgregarRegistro = "", bool tbAgregaTodos = false)
        {
            var poLista = toLista;
            if (!string.IsNullOrEmpty(tsAgregarRegistro)) poLista.Insert(0, new Combo { Codigo = "N", Descripcion = tsAgregarRegistro });
            if (tbAgregaSeleccione) poLista.Insert(0, new Combo { Codigo = Diccionario.Seleccione, Descripcion = Diccionario.DesSeleccione });
            if (tbAgregaTodas) poLista.Insert(0, new Combo { Codigo = Diccionario.Seleccione, Descripcion = Diccionario.DesTodas });
            if (tbAgregaTodos) poLista.Insert(0, new Combo { Codigo = Diccionario.Seleccione, Descripcion = Diccionario.DesTodos });
            toCmb.Properties.DataSource = poLista;
            toCmb.Properties.ValueMember = "Codigo";
            toCmb.Properties.DisplayMember = "Descripcion";
            toCmb.Properties.PopupSizeable = true;
        }

        /// <summary>
        /// Función genérica para llenar un Grid combo
        /// </summary>
        /// <param name="toCmb"></param>
        /// <param name="toLista"></param>
        /// <param name="tbAgregaSeleccione"></param>
        public static void gLLenarGridCombo(ref GridLookUpEdit toGcmb, List<Combo> toLista, bool tbAgregaSeleccione = false, bool tbAgregaTodos = false)
        {
            var poLista = toLista;
            if (tbAgregaSeleccione) poLista.Insert(0, new Combo { Codigo = Diccionario.Seleccione, Descripcion = Diccionario.DesSeleccione });
            if (tbAgregaTodos) poLista.Insert(0, new Combo { Codigo = Diccionario.Seleccione, Descripcion = Diccionario.DesTodas });
            toGcmb.Properties.DataSource = poLista;
            toGcmb.Properties.ValueMember = "Codigo";
            toGcmb.Properties.DisplayMember = "Descripcion";
            //toGcmb.Properties.ForceInitialize();
            //toCmb.Properties.PopulateColumns();
            //toCmb.Properties.Columns["Codigo"].Visible = false;
            //if ((toCmb.Properties.DataSource as IList).Count > 0) toCmb.ItemIndex = 0;
        }

        public static void gLLenarComboGrid(ref DevExpress.XtraGrid.Views.Grid.GridView toDgv, List<Combo> toLista, string tsColumna, bool tbAgregaSeleccione = false, bool tbAgregaTodos = false, int PopupFormMinSizeWith = 0)
        {
            RepositoryItemLookUpEdit poCmb = new RepositoryItemLookUpEdit();
            var poLista = toLista;
            if (tbAgregaSeleccione) poLista.Insert(0, new Combo { Codigo = Diccionario.Seleccione, Descripcion = Diccionario.DesSeleccione });
            if (tbAgregaTodos) poLista.Insert(0, new Combo { Codigo = Diccionario.Seleccione, Descripcion = Diccionario.DesTodas });
            poCmb.DataSource = poLista;
            poCmb.ValueMember = "Codigo";
            poCmb.DisplayMember = "Descripcion";
            poCmb.ForceInitialize();
            poCmb.PopulateColumns();
            poCmb.ShowFooter = false;
            poCmb.ShowHeader = false;
            poCmb.NullText = "";
            poCmb.Columns["Codigo"].Visible = false;
            poCmb.PopupWidth = 10;
            if (PopupFormMinSizeWith > 0) poCmb.PopupFormMinSize = new System.Drawing.Size(PopupFormMinSizeWith, 0);
            poCmb.PopupFilterMode = PopupFilterMode.Contains;
            //Añadir Combo a Grid
            toDgv.Columns[tsColumna].ColumnEdit = poCmb;
        }

        public static void gLLenarComboGridOut(ref DevExpress.XtraGrid.Views.Grid.GridView toDgv, List<Combo> toLista, string tsColumna, out RepositoryItemLookUpEdit toCmb, bool tbAgregaSeleccione = false)
        {
            toCmb = new RepositoryItemLookUpEdit();
            var poLista = toLista;
            if (tbAgregaSeleccione) poLista.Insert(0, new Combo { Codigo = Diccionario.Seleccione, Descripcion = Diccionario.DesSeleccione });
            toCmb.DataSource = poLista;
            toCmb.ValueMember = "Codigo";
            toCmb.DisplayMember = "Descripcion";
            toCmb.ForceInitialize();
            toCmb.PopulateColumns();
            toCmb.ShowFooter = false;
            toCmb.ShowHeader = false;
            toCmb.NullText = "";
            toCmb.Columns["Codigo"].Visible = false;
            toCmb.PopupWidth = 10;
            //Añadir Combo a Grid
            toDgv.Columns[tsColumna].ColumnEdit = toCmb;
        }

        public static bool gValidaFormatoCorreo(string tsEmail)
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(tsEmail, expresion))
            {
                if (Regex.Replace(tsEmail, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static void gSoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        public static void gEnterEqualTab(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
                SendKeys.Send("{TAB}");
        }

        public static void gSoloNumerosSimbolo(object sender, KeyPressEventArgs e)
        {

            CultureInfo cc = System.Threading.Thread.CurrentThread.CurrentCulture;
            char psSimboloDecimal = Convert.ToChar(cc.NumberFormat.NumberDecimalSeparator);
            char psSimboloMenos = Convert.ToChar("-");

            if (!Char.IsDigit(e.KeyChar) &&
                e.KeyChar != (char)Keys.Back &&
                e.KeyChar != psSimboloDecimal &&
                e.KeyChar != psSimboloMenos)
            {
                e.Handled = true;
            }
            else
            {
                if (e.KeyChar == psSimboloDecimal)
                {
                    if (((TextEdit)sender).Text.Contains(psSimboloDecimal))
                        e.Handled = true;
                    else
                        e.Handled = false;
                }
            }

            //if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == '.')
            //{
            //    e.Handled = false;
            //}
            //else
            //{
            //    e.Handled = true;
            //}
        }

        public static void gSoloLetras(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }


            //if (Char.IsLetter(e.KeyChar))
            //{
            //    e.Handled = false;
            //}
            //if (Char.IsNumber(e.KeyChar))
            //{
            //    e.Handled = true;
            //}

        }

        public static bool gVerificaIdentificacion(string identificacion)
        {
            bool estado = false;

            try
            {

                var dt = new clsNUsuario().goConsultaDataTable(string.Format("EXEC GENSPVALIDADIGITOVERIFICADOR '{0}'", identificacion));

                if (string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                {
                    estado = true;
                }
                else
                {
                    estado = false;
                }
                //char[] valced = new char[13];
                //int provincia;
            //    if (identificacion.Length >= 10)
            //    {
            //        valced = identificacion.Trim().ToCharArray();
            //        provincia = int.Parse((valced[0].ToString() + valced[1].ToString()));
            //        if (provincia > 0 && provincia < 25)
            //        {
            //            if (int.Parse(valced[2].ToString()) < 6)
            //            {
            //                estado = VerificaCedula(valced);
            //            }
            //            else if (int.Parse(valced[2].ToString()) == 6)
            //            {
            //                estado = VerificaSectorPublico(valced);
            //            }
            //            else if (int.Parse(valced[2].ToString()) == 9)
            //            {

            //                estado = VerificaPersonaJuridica(valced);
            //            }
            //        }
            //    }
            }
            catch (Exception)
            {
                estado = false;
            }
            
            return estado;
        }

        public static void gFechaValue_Changed(object sender, EventArgs e)
        {
            if (((DateTimePicker)sender).ShowCheckBox == true)
            {
                if (((DateTimePicker)sender).Checked == false)
                {
                    ((DateTimePicker)sender).CustomFormat = " ";
                    ((DateTimePicker)sender).Format = DateTimePickerFormat.Custom;
                }
                else
                {
                    ((DateTimePicker)sender).Format = DateTimePickerFormat.Short;
                }
            }
            else
            {
                ((DateTimePicker)sender).Format = DateTimePickerFormat.Short;
            }
        }

        public static void gResetDtpCheck(ref DateTimePicker toDtp)
        {
            toDtp.Value = DateTime.Now;
            toDtp.Format = DateTimePickerFormat.Custom;
            toDtp.CustomFormat = " ";
            toDtp.Checked = false;
        }

        private static bool VerificaCedula(char[] validarCedula)
        {
            int aux = 0, par = 0, impar = 0, verifi;
            for (int i = 0; i < 9; i += 2)
            {
                aux = 2 * int.Parse(validarCedula[i].ToString());
                if (aux > 9)
                    aux -= 9;
                par += aux;
            }
            for (int i = 1; i < 9; i += 2)
            {
                impar += int.Parse(validarCedula[i].ToString());
            }

            aux = par + impar;
            if (aux % 10 != 0)
            {
                verifi = 10 - (aux % 10);
            }
            else
                verifi = 0;
            if (verifi == int.Parse(validarCedula[9].ToString()))
                return true;
            else
                return false;
        }

        public static bool VerificaPersonaJuridica(char[] validarCedula)
        {
            int aux = 0, prod, veri;
            veri = int.Parse(validarCedula[10].ToString()) + int.Parse(validarCedula[11].ToString()) + int.Parse(validarCedula[12].ToString());
            if (veri > 0)
            {
                int[] coeficiente = new int[9] { 4, 3, 2, 7, 6, 5, 4, 3, 2 };
                for (int i = 0; i < 9; i++)
                {
                    prod = int.Parse(validarCedula[i].ToString()) * coeficiente[i];
                    aux += prod;
                }
                if (aux % 11 == 0)
                {
                    veri = 0;
                }
                else if (aux % 11 == 1)
                {
                    return false;
                }
                else
                {
                    aux = aux % 11;
                    veri = 11 - aux;
                }

                if (veri == int.Parse(validarCedula[9].ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool VerificaSectorPublico(char[] validarCedula)
        {
            int aux = 0, prod, veri;
            veri = int.Parse(validarCedula[9].ToString()) + int.Parse(validarCedula[10].ToString()) + int.Parse(validarCedula[11].ToString()) + int.Parse(validarCedula[12].ToString());
            if (veri > 0)
            {
                int[] coeficiente = new int[8] { 3, 2, 7, 6, 5, 4, 3, 2 };

                for (int i = 0; i < 8; i++)
                {
                    prod = int.Parse(validarCedula[i].ToString()) * coeficiente[i];
                    aux += prod;
                }

                if (aux % 11 == 0)
                {
                    veri = 0;
                }
                else if (aux % 11 == 1)
                {
                    return false;
                }
                else
                {
                    aux = aux % 11;
                    veri = 11 - aux;
                }

                if (veri == int.Parse(validarCedula[8].ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Convierte un datatable a un archivo csv
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public static System.Data.DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            StreamReader sr = new StreamReader(strFilePath);
            string[] headers = sr.ReadLine().Split(',');
            System.Data.DataTable dt = new System.Data.DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static System.Data.DataTable ConvertExcelToDataTable(string FileName)
        {
            System.Data.DataTable dtResult = null;
            int totalSheet = 0; //No of sheets on excel file  
            using (OleDbConnection objConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';"))
            {
                objConn.Open();
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                DataSet ds = new DataSet();
                System.Data.DataTable dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = string.Empty;
                if (dt != null)
                {
                    var tempDataTable = (from dataRow in dt.AsEnumerable()
                                         where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                         select dataRow).CopyToDataTable();
                    dt = tempDataTable;
                    totalSheet = dt.Rows.Count;
                    sheetName = dt.Rows[0]["TABLE_NAME"].ToString();
                }
                cmd.Connection = objConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
                oleda = new OleDbDataAdapter(cmd);
                oleda.Fill(ds, "excelData");
                dtResult = ds.Tables["excelData"];
                objConn.Close();
                return dtResult; //Returning Dattable  
            }
        }

        /// <summary>
        /// Retorna la descripción del més
        /// </summary>
        /// <param name="tdFecha"></param>
        /// <returns></returns>
        public static string gsGetMes(DateTime tdFecha)
        {
            return gsGetMes(tdFecha.Month);
        }

        /// <summary>
        /// Retorna la descripción del més
        /// </summary>
        /// <param name="tdFecha"></param>
        /// <returns></returns>
        public static string gsGetMes(int tiMes)
        {
            string psReturn = string.Empty;
            switch (tiMes)
            {
                case 1:
                    psReturn = "ENERO";
                    break;
                case 2:
                    psReturn = "FEBRERO";
                    break;
                case 3:
                    psReturn = "MARZO";
                    break;
                case 4:
                    psReturn = "ABRIL";
                    break;
                case 5:
                    psReturn = "MAYO";
                    break;
                case 6:
                    psReturn = "JUNIO";
                    break;
                case 7:
                    psReturn = "JULIO";
                    break;
                case 8:
                    psReturn = "AGOSTO";
                    break;
                case 9:
                    psReturn = "SEPTIEMBRE";
                    break;
                case 10:
                    psReturn = "OCTUBRE";
                    break;
                case 11:
                    psReturn = "NOVIEMBRE";
                    break;
                case 12:
                    psReturn = "DICIEMBRE";
                    break;
                default:
                    break;
            }
            return psReturn;
        }

        /// <summary>
        /// Retorna la descripción del día
        /// </summary>
        /// <param name="tdFecha"></param>
        /// <returns></returns>
        public static string gsGetDia(DateTime tdFecha)
        {
            string psReturn = string.Empty;
            switch (tdFecha.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    psReturn = "Domingo";
                    break;
                case DayOfWeek.Monday:
                    psReturn = "Lunes";
                    break;
                case DayOfWeek.Tuesday:
                    psReturn = "Martes";
                    break;
                case DayOfWeek.Wednesday:
                    psReturn = "Miércoles";
                    break;
                case DayOfWeek.Thursday:
                    psReturn = "Jueves";
                    break;
                case DayOfWeek.Friday:
                    psReturn = "Viernes";
                    break;
                case DayOfWeek.Saturday:
                    psReturn = "Sábado";
                    break;
                default:
                    break;
            }
            return psReturn;
        }


        /// <summary>
        /// Exportar Datos convirtiendolos en un archivo
        /// </summary>
        /// <param name="tgcDatos">Grid Control que tiene los datos</param>
        /// <param name="tsFileName">Nombre del archivo a guardar</param>
        /// <param name="tsFilter">Formatos de archivos en los que se puede guardar</param>
        /// <param name="tsTypeFile">Tipo a Archivo a Exportar: "XLS" => EXCEL, "CSV" => CSV, "TXT" => TXT, "PDF" => PDF </param>
        public static void gSaveFile(GridControl tgcDatos, string tsFileName, string tsFilter, string tsTypeFile  = "XLS")
        {
            if (tgcDatos != null)
            {
                SaveFileDialog savefile = new SaveFileDialog();
                // set a default file name
                savefile.FileName = tsFileName;
                // set filters - this can be done in properties as well
                savefile.Filter = tsFilter;

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    string psPath = savefile.FileName;
                    if (tsTypeFile.ToUpper() == "XLS")
                    {
                        tgcDatos.ExportToXlsx(psPath);
                    }
                    else if (tsTypeFile.ToUpper() == "CSV")
                    {
                        tgcDatos.ExportToCsv(psPath);
                    }
                    else if (tsTypeFile.ToUpper() == "TXT")
                    {
                        tgcDatos.ExportToText(psPath);
                    }
                    else if (tsTypeFile.ToUpper() == "PDF")
                    {
                        tgcDatos.ExportToPdf(psPath);
                    }
                    else
                    {
                        tgcDatos.ExportToXlsx(psPath);
                    }

                    string psMensaje = "Archivo Creado, Ruta: " + psPath;
                    XtraMessageBox.Show(psMensaje, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Eventos necesarios para la presentación de reportes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void PrintingSystem_StartPrint(object sender, PrintDocumentEventArgs e)
        {
            prnSettings = e.PrintDocument.PrinterSettings;
        }
       
        /// <summary>
        /// Eventos necesarios para la presentación de reportes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void reportsStartPrintEventHandler(object sender, PrintDocumentEventArgs e)
        {
            int pageCount = e.PrintDocument.PrinterSettings.ToPage;
            e.PrintDocument.PrinterSettings = prnSettings;

            // The following line is required if the number of pages for each report varies, 
            // and you consistently need to print all pages.
            e.PrintDocument.PrinterSettings.ToPage = pageCount;
        }

        /// <summary>
        /// Presente el reporte de Rol de Ingresos y Egresos
        /// </summary>
        /// <param name="tIdPeriodo"></param>
        public static void gImprimirRolIngresosEgresos(int tIdPeriodo)
        {
            //Se crea una instancia del dataset tipado
            dsNomina dsIngresos = new dsNomina();
            var poLogicaNegocio = new clsNNomina();

            var poRubros = poLogicaNegocio.goConsultaRubro();
            var poPeriodo = poLogicaNegocio.goConsultarPeriodo(tIdPeriodo);

            string psParametro = "";

            if (poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.DecimoCuarto)
            {
                if (poPeriodo.FechaInicio.Month == 3)
                {
                    psParametro = string.Format("ROL {0} \n PERIODO {1}-{2} REGIÓN COSTA", poPeriodo.TipoRol, poPeriodo.Anio-1, poPeriodo.Anio);
                }
                else
                {
                    psParametro = string.Format("ROL {0} \n PERIODO {1}-{2} REGIÓN SIERRA", poPeriodo.TipoRol, poPeriodo.Anio - 1, poPeriodo.Anio);
                }
            }
            else if (poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.DecimoTercero)
            {
                psParametro = string.Format("ROL {0} \n PERIODO {1}-{2}", poPeriodo.TipoRol, poPeriodo.Anio - 1, poPeriodo.Anio);
            }
            else if (poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.FinMes)
            {
                psParametro = string.Format("ROL DE {0} / {1}", clsComun.gsGetMes(poPeriodo.FechaInicio), poPeriodo.Anio);
            }
            else
            {   
                psParametro = string.Format("{0} DE {1} / {2}", poPeriodo.TipoRol, clsComun.gsGetMes(poPeriodo.FechaInicio), poPeriodo.Anio);
            }
            
            if (poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.PrimeraQuincena || poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.Jubilados)
            {
                DataSet ds = new DataSet();
                var dt = poLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTAROLINGRESOEGRESO {0}", tIdPeriodo));
                dt.TableName = "RolPQ";
                if (ds.Tables["RolPQ"] != null) ds.Tables.Remove("RolPQ");
                string psColumn = "Sueldo Ganado";
                if (dt.Columns["ANTICIPO SUELDO"] != null)
                {
                    dt.Columns["ANTICIPO SUELDO"].ColumnName = "SUELDO GANADO";
                    psColumn = "Anticipo Sueldo";
                }
                
                ds.Merge(dt);
                if (dt.Rows.Count > 0)
                {
                    xrptRolIngresosEgresos xrpt = new xrptRolIngresosEgresos();

                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrpt.Parameters["Rol"].Value = psParametro;
                    xrpt.Parameters["SueldoGan"].Value = psColumn;
                    xrpt.DataSource = ds;
                    //Se invoca la ventana que muestra el reporte.
                    xrpt.RequestParameters = false;
                    xrpt.Parameters["Rol"].Visible = false;
                    xrpt.Parameters["SueldoGan"].Visible = false;

                    XtraReport[] reports;

                    reports = new XtraReport[] { xrpt };

                    ReportPrintTool pt1 = new ReportPrintTool(xrpt);
                    pt1.PrintingSystem.StartPrint += new PrintDocumentEventHandler(PrintingSystem_StartPrint);

                    foreach (XtraReport report in reports)
                    {
                        ReportPrintTool pts = new ReportPrintTool(report);
                        pts.PrintingSystem.StartPrint +=
                            new PrintDocumentEventHandler(reportsStartPrintEventHandler);
                    }

                    foreach (XtraReport report in reports)
                    {

                        ReportPrintTool pts = new ReportPrintTool(report);
                        pts.ShowRibbonPreview();

                    }

                }

                /*var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTACOMISIONESCOBRANZA {0}", tIdPeriodo));
                dt.TableName = "Comisiones";
                if (ds.Tables["Comisiones"] != null) ds.Tables.Remove("Comisiones");
                ds.Merge(dt);
                if (dt.Rows.Count > 0)
                {
                    var poComision = loLogicaNegocio.goConsultarComision(tIdPeriodo);
                    xrptComisionesCobranza xrpt = new xrptComisionesCobranza();

                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrpt.Parameters["Periodo"].Value = psParametro;
                    xrpt.Parameters["BaseComision"].Value = poComision.BaseComision;
                    xrpt.DataSource = ds;
                    //Se invoca la ventana que muestra el reporte.
                    xrpt.RequestParameters = false;
                    xrpt.Parameters["Periodo"].Visible = false;
                    xrpt.Parameters["BaseComision"].Visible = false;

                    using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                    {
                        printTool.ShowRibbonPreviewDialog();
                    }
                }
                */
            }
            else
            {
                var dtIngresos = poLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTAROLINGRESOS {0}", tIdPeriodo));
                dtIngresos.TableName = "RolIngresos";
                if (dsIngresos.Tables["RolIngresos"] != null) dsIngresos.Tables.Remove("RolIngresos");

                List<DictionaryDto> dictionaryIngresos = new List<DictionaryDto>();
                dictionaryIngresos.Add(new DictionaryDto() { Key = 4, Value = "040" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 5, Value = "001" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 6, Value = "041" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 7, Value = "057" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 8, Value = "002" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 9, Value = "003" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 10, Value = "004" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 11, Value = "008" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 12, Value = "020" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 13, Value = "019" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 14, Value = "014" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 15, Value = "006" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 16, Value = "052" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 17, Value = "045" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 18, Value = "046" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 19, Value = "043" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 20, Value = "044" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 21, Value = "037" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 22, Value = "005" });
                dictionaryIngresos.Add(new DictionaryDto() { Key = 23, Value = "059" });

                List<GrupoRubroIndex> poGrupoRubro = new List<GrupoRubroIndex>();
                if (poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.DecimoTercero)
                {
                    poGrupoRubro.Add(new GrupoRubroIndex() { Index = 13, CodigoRubro = "019", CodigoRubroReferencial = "009" });
                }
                else if (poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.PrimeraQuincena)
                {
                    poGrupoRubro.Add(new GrupoRubroIndex() { Index = 6, CodigoRubro = "041", CodigoRubroReferencial = "022" });
                }
                else if (poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.DecimoCuarto)
                {
                    poGrupoRubro.Add(new GrupoRubroIndex() { Index = 12, CodigoRubro = "020", CodigoRubroReferencial = "010" });
                }


                System.Data.DataTable dtDatosIngresos = new System.Data.DataTable();
                dtDatosIngresos.TableName = "RolIngresos";
                dtDatosIngresos.Columns.AddRange(new DataColumn[]
                {
                        new DataColumn("ID", typeof(int)),
                        new DataColumn("CEDULA", typeof(string)),
                        new DataColumn("EMPLEADO", typeof(string)),
                        new DataColumn("DEPARTAMENTO", typeof(string)),
                        new DataColumn("DIAS LABORADOS", typeof(int)),
                        new DataColumn("SUELDO", typeof(decimal)),
                        new DataColumn("SUELDO GANADO", typeof(decimal)),
                        new DataColumn("SUBSIDIO 25", typeof(decimal)),
                        new DataColumn("SUBSIDIO 50", typeof(decimal)),
                        new DataColumn("HORAS EXTRAS 50", typeof(decimal)),
                        new DataColumn("HORAS EXTRAS 100", typeof(decimal)),
                        new DataColumn("VACACIONES GANADAS", typeof(decimal)),
                        new DataColumn("PROVISION 14TO SUELDO", typeof(decimal)),
                        new DataColumn("PROVISION 13ER SUELDO", typeof(decimal)),
                        new DataColumn("FONDO DE RESERVA", typeof(decimal)),
                        new DataColumn("ALIMENTACION", typeof(decimal)),
                        new DataColumn("CELULAR", typeof(decimal)),
                        new DataColumn("OTROS INGRESOS IESS", typeof(decimal)),
                        new DataColumn("OTROS INGRESOS NO IESS", typeof(decimal)),
                        new DataColumn("TOTAL INGRESOS IESS", typeof(decimal)),
                        new DataColumn("TOTAL INGRESOS NO IESS", typeof(decimal)),
                        new DataColumn("TOTAL INGRESOS", typeof(decimal)),
                        new DataColumn("COMISIONES", typeof(decimal)),
                        new DataColumn("COMISION AD", typeof(decimal)),
                });

                var psColumnConsulta = new List<string>();

                foreach (var item in dtIngresos.Columns)
                {
                    psColumnConsulta.Add(item.ToString());
                }

                var psColumnReporte = new List<string>();

                foreach (DataRow item in dtIngresos.Rows)
                {

                    int piContColumn = 0;
                    int piCount = dtDatosIngresos.Columns.Count;
                    DataRow row = dtDatosIngresos.NewRow();
                    foreach (DataColumn column in dtDatosIngresos.Columns)
                    {
                        string psNombreColumnaDt = column.Caption.Trim();

                        string psCodigoRubroDs = dictionaryIngresos.Where(x => x.Key == piContColumn).Select(x => x.Value).FirstOrDefault();
                        try
                        {
                            if (!string.IsNullOrEmpty(psCodigoRubroDs))
                            {
                                psNombreColumnaDt = poRubros.Where(x => x.Codigo == psCodigoRubroDs).Select(x => x.Descripcion).FirstOrDefault();
                                var poObject = poGrupoRubro.Where(x => x.Index == piContColumn).FirstOrDefault();
                                if (poObject != null)
                                {
                                    psColumnReporte.Add(psNombreColumnaDt);
                                    psNombreColumnaDt = poRubros.Where(x => x.Codigo == poObject.CodigoRubroReferencial).Select(x => x.Descripcion).FirstOrDefault();
                                }
                            }

                            if (item[psNombreColumnaDt] != null)
                            {
                                if (!string.IsNullOrEmpty(item[psNombreColumnaDt].ToString()))
                                {
                                    row[piContColumn] = item[psNombreColumnaDt];
                                    psColumnReporte.Add(psNombreColumnaDt);
                                }
                                else
                                {
                                    row[piContColumn] = "0";
                                    psColumnReporte.Add(psNombreColumnaDt);
                                }
                            }
                            else
                            {
                                row[piContColumn] = "0";
                                psColumnReporte.Add(psNombreColumnaDt);
                            }
                        }
                        catch (Exception)
                        {
                            row[piContColumn] = "0";
                        }

                        piContColumn++;

                        if (piCount == piContColumn)
                        {
                            string psOtrosIngresosIess = poRubros.Where(x => x.Codigo == "045").Select(x => x.Descripcion).FirstOrDefault();
                            string psOtrosIngresosNoIess = poRubros.Where(x => x.Codigo == "046").Select(x => x.Descripcion).FirstOrDefault();
                            decimal pdcValorAdicionarIess = 0;
                            decimal pdcValorAdicionarNoIess = 0;
                            foreach (var nameColumn in psColumnConsulta.Where(x => !psColumnReporte.Contains(x)).Select(X => X).ToList())
                            {
                                var pbAportable = poRubros.Where(x => x.Descripcion == nameColumn).Select(x => x.Aportable).FirstOrDefault();

                                if (pbAportable)
                                {
                                    pdcValorAdicionarIess += Convert.ToDecimal(string.IsNullOrEmpty(item[nameColumn].ToString()) ? "0" : item[nameColumn].ToString()); ;
                                }
                                else
                                {
                                    pdcValorAdicionarNoIess += Convert.ToDecimal(string.IsNullOrEmpty(item[nameColumn].ToString()) ? "0" : item[nameColumn].ToString()); ;
                                }
                            }

                            row[psOtrosIngresosIess] = Convert.ToDecimal(row[psOtrosIngresosIess].ToString()) + pdcValorAdicionarIess;
                            row[psOtrosIngresosNoIess] = Convert.ToDecimal(row[psOtrosIngresosNoIess].ToString()) + pdcValorAdicionarNoIess;

                        }

                    }
                    dtDatosIngresos.Rows.Add(row);
                }

                dsIngresos.Merge(dtDatosIngresos);

                
                //using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrptNominaIngresos))
                //{
                //    printTool.ShowRibbonPreview();
                //}


                // EGRESOS
                //Se crea una instancia del dataset tipado
                Reportes.dsNomina dsEgresos = new Reportes.dsNomina();
                var dtEgresos = poLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTAROLEGRESOS {0}", tIdPeriodo));
                dtIngresos.TableName = "RolEgresos";
                if (dsIngresos.Tables["RolEgresos"] != null) dsIngresos.Tables.Remove("RolEgresos");

                Dictionary<int, string> dictionaryEgresos = new Dictionary<int, string>();
                dictionaryEgresos.Add(4, "040");
                dictionaryEgresos.Add(5, "043");
                dictionaryEgresos.Add(6, "044");
                dictionaryEgresos.Add(7, "037");
                dictionaryEgresos.Add(8, "032");
                dictionaryEgresos.Add(9, "034");
                dictionaryEgresos.Add(10, "016");
                dictionaryEgresos.Add(11, "055");
                dictionaryEgresos.Add(12, "058");
                dictionaryEgresos.Add(13, "053");
                dictionaryEgresos.Add(14, "054");
                dictionaryEgresos.Add(15, "047");
                dictionaryEgresos.Add(16, "049");
                dictionaryEgresos.Add(17, "048");
                dictionaryEgresos.Add(18, "050");
                dictionaryEgresos.Add(29, "056");
                dictionaryEgresos.Add(20, "051");
                dictionaryEgresos.Add(21, "038");
                dictionaryEgresos.Add(22, "039");



                System.Data.DataTable dtDatosEgresos = new System.Data.DataTable();
                dtDatosEgresos.TableName = "RolEgresos";
                dtDatosEgresos.Columns.AddRange(new DataColumn[]
                {
                        new DataColumn("ID", typeof(int)),
                        new DataColumn("CEDULA", typeof(string)),
                        new DataColumn("EMPLEADO", typeof(string)),
                        new DataColumn("DEPARTAMENTO", typeof(string)),
                        new DataColumn("DIAS LABORADOS", typeof(int)),
                        new DataColumn("TOTAL INGRESOS IESS", typeof(decimal)),
                        new DataColumn("TOTAL INGRESOS NO IESS", typeof(decimal)),
                        new DataColumn("TOTAL INGRESOS", typeof(decimal)),
                        new DataColumn("APORTE IESS EMPLEADO", typeof(decimal)),
                        new DataColumn("APORTE IESS CONYUGE EMPLEADO", typeof(decimal)),
                        new DataColumn("ANTICIPO SUELDO", typeof(decimal)),
                        new DataColumn("ANTICIPO ALIMENTACION", typeof(decimal)),
                        new DataColumn("DESCUENTO DE HABERES", typeof(decimal)),
                        new DataColumn("DESCUENTO CELULAR", typeof(decimal)),
                        new DataColumn("VACACIONES PAGADAS", typeof(decimal)),
                        new DataColumn("PRESTAMO HIPOTECARIO", typeof(decimal)),
                        new DataColumn("PRESTAMO QUIROGRAFARIO", typeof(decimal)),
                        new DataColumn("PRESTAMO INTERNO", typeof(decimal)),
                        new DataColumn("RETENCION JUDICIAL", typeof(decimal)),
                        new DataColumn("IMPUESTO A LA RENTA", typeof(decimal)),
                        new DataColumn("OTROS EGRESOS", typeof(decimal)),
                        new DataColumn("TOTAL EGRESOS", typeof(decimal)),
                        new DataColumn("NETO A RECIBIR", typeof(decimal)),
                });

                psColumnConsulta = new List<string>();

                foreach (var item in dtEgresos.Columns)
                {
                    psColumnConsulta.Add(item.ToString());
                }

                psColumnReporte = new List<string>();

                poGrupoRubro = new List<GrupoRubroIndex>();

                foreach (DataRow item in dtEgresos.Rows)
                {

                    // Para validar
                    var psVal = "";
                    if (int.Parse(item[0].ToString()) == 400)
                    {
                        psVal = "stop";
                    }

                    int piContColumn = 0;
                    int piCount = dtDatosEgresos.Columns.Count;
                    DataRow row = dtDatosEgresos.NewRow();
                    foreach (DataColumn column in dtDatosEgresos.Columns)
                    {
                        string psNombreColumnaDt = column.Caption.Trim();
                        string psCodigoRubroDs = dictionaryEgresos.Where(x => x.Key == piContColumn).Select(x => x.Value).FirstOrDefault();
                        try
                        {
                            if (!string.IsNullOrEmpty(psCodigoRubroDs))
                            {
                                psNombreColumnaDt = poRubros.Where(x => x.Codigo == psCodigoRubroDs).Select(x => x.Descripcion).FirstOrDefault();
                                var poObject = poGrupoRubro.Where(x => x.Index == piContColumn).FirstOrDefault();
                                if (poObject != null)
                                {
                                    psColumnReporte.Add(psNombreColumnaDt);
                                    psNombreColumnaDt = poRubros.Where(x => x.Codigo == poObject.CodigoRubroReferencial).Select(x => x.Descripcion).FirstOrDefault();
                                }
                            }

                            if (item[psNombreColumnaDt] != null)
                            {
                                if (!string.IsNullOrEmpty(item[psNombreColumnaDt].ToString()))
                                {
                                    row[piContColumn] = item[psNombreColumnaDt];
                                    psColumnReporte.Add(psNombreColumnaDt);
                                }
                                else
                                {
                                    row[piContColumn] = "0";
                                    psColumnReporte.Add(psNombreColumnaDt);
                                }
                            }
                            else
                            {
                                row[piContColumn] = "0";
                            }
                        }
                        catch (Exception)
                        {
                            row[piContColumn] = "0";
                        }
                        piContColumn++;

                        if (piCount == piContColumn)
                        {
                            string psOtrosEgresos = poRubros.Where(x => x.Codigo == "051").Select(x => x.Descripcion).FirstOrDefault();
                            decimal pdcValorAdicionar = 0;
                            foreach (var nameColumn in psColumnConsulta.Where(x => !psColumnReporte.Contains(x)).Select(X => X).ToList())
                            {
                                pdcValorAdicionar += Convert.ToDecimal(string.IsNullOrEmpty(item[nameColumn].ToString()) ? "0" : item[nameColumn].ToString());
                            }

                            row[psOtrosEgresos] = Convert.ToDecimal(row[psOtrosEgresos].ToString()) + pdcValorAdicionar;
                        }


                    }
                    dtDatosEgresos.Rows.Add(row);
                }

                dsEgresos.Merge(dtDatosEgresos);


                xrptRolIngresosTb xrptNominaIngresos = new xrptRolIngresosTb();

                //Se establece el origen de datos del reporte (El dataset previamente leído)
                xrptNominaIngresos.Parameters["Rol"].Value = psParametro;
                xrptNominaIngresos.DataSource = dsIngresos;
                //Se invoca la ventana que muestra el reporte.
                xrptNominaIngresos.RequestParameters = false;
                xrptNominaIngresos.Parameters["Rol"].Visible = false;


                xrptRolEgresosTb xrptNominaEgresos = new xrptRolEgresosTb();

                //Se establece el origen de datos del reporte (El dataset previamente leído)
                xrptNominaEgresos.Parameters["Rol"].Value = psParametro;
                xrptNominaEgresos.DataSource = dsEgresos;
                //Se invoca la ventana que muestra el reporte.
                xrptNominaEgresos.RequestParameters = false;
                xrptNominaEgresos.Parameters["Rol"].Visible = false;


                xrptRolIngresosComisiones xrptNominaIngresosCom = new xrptRolIngresosComisiones();

                //Se establece el origen de datos del reporte (El dataset previamente leído)
                xrptNominaIngresosCom.Parameters["Rol"].Value = psParametro;
                xrptNominaIngresosCom.DataSource = dsIngresos;
                //Se invoca la ventana que muestra el reporte.
                xrptNominaIngresosCom.RequestParameters = false;
                xrptNominaIngresosCom.Parameters["Rol"].Visible = false;


                xrptRolEgresosComisiones xrptNominaEgresosCom = new xrptRolEgresosComisiones();

                //Se establece el origen de datos del reporte (El dataset previamente leído)
                xrptNominaEgresosCom.Parameters["Rol"].Value = psParametro;
                xrptNominaEgresosCom.DataSource = dsEgresos;
                //Se invoca la ventana que muestra el reporte.
                xrptNominaEgresosCom.RequestParameters = false;
                xrptNominaEgresosCom.Parameters["Rol"].Visible = false;

                //using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrptNominaEgresos))
                //{
                //    printTool.ShowRibbonPreview();
                //}

                XtraReport[] reports;

                //Creación de reporte Décimo Tercero
                xrptDecimoTercero xrptDecimoTercero = new xrptDecimoTercero();
                xrptDecimoCuarto xrptDecimoCuarto = new xrptDecimoCuarto();

                xrptComisiones xrptComisiones = new xrptComisiones();
                dsNomina dsDecimo = new dsNomina();
                dsNomina dsComisiones = new dsNomina();
                if (poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.DecimoTercero || poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.DecimoCuarto)
                {
                    System.Data.DataTable dtDecimo = new System.Data.DataTable();

                    if (poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.DecimoTercero)
                        dtDecimo = poLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTADECIMOTERCERO {0}", tIdPeriodo));
                    else
                        dtDecimo = poLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTADECIMOCUARTO {0}", tIdPeriodo));

                    dtDecimo.TableName = "DecimoTercero";
                    if (dsDecimo.Tables["DecimoTercero"] != null) dsDecimo.Tables.Remove("DecimoTercero");

                    dsDecimo.Merge(dtDecimo);

                    if (poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.DecimoTercero)
                    {
                        //Se establece el origen de datos del reporte (El dataset previamente leído)
                        xrptDecimoTercero.Parameters["Rol"].Value = psParametro;
                        xrptDecimoTercero.DataSource = dsDecimo;
                        //Se invoca la ventana que muestra el reporte.
                        xrptDecimoTercero.RequestParameters = false;
                        xrptDecimoTercero.Parameters["Rol"].Visible = false;

                        reports = new XtraReport[] { xrptNominaIngresos, xrptNominaEgresos, xrptDecimoTercero };
                    }
                    else
                    {
                        //Se establece el origen de datos del reporte (El dataset previamente leído)
                        xrptDecimoCuarto.Parameters["Rol"].Value = psParametro;
                        xrptDecimoCuarto.DataSource = dsDecimo;
                        //Se invoca la ventana que muestra el reporte.
                        xrptDecimoCuarto.RequestParameters = false;
                        xrptDecimoCuarto.Parameters["Rol"].Visible = false;

                        reports = new XtraReport[] { xrptNominaIngresos, xrptNominaEgresos, xrptDecimoCuarto };

                    }



                }
                else if (poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.Comisiones)
                {
                    var dt = poLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTACOMISIONES {0}", tIdPeriodo));
                    dt.TableName = "Comisiones";
                    if (dsComisiones.Tables["Comisiones"] != null) dsComisiones.Tables.Remove("Comisiones");

                    dsComisiones.Merge(dt);

                    var poComision = new clsNComision().goConsultarComision(tIdPeriodo);
                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrptComisiones.Parameters["Rol"].Value = string.Format("DEL {0} DE {1} AL {2} DE {3}", poPeriodo.FechaInicioComi.Value.Day, clsComun.gsGetMes(poPeriodo.FechaInicioComi.Value.Month), poPeriodo.FechaFinComi.Value.Day, clsComun.gsGetMes(poPeriodo.FechaFinComi.Value.Month));
                    xrptComisiones.Parameters["TotalCobranza"].Value = poComision.BaseComision;
                    xrptComisiones.DataSource = dsComisiones;
                    //Se invoca la ventana que muestra el reporte.
                    xrptComisiones.RequestParameters = false;
                    xrptComisiones.Parameters["Rol"].Visible = false;
                    xrptComisiones.Parameters["TotalCobranza"].Visible = false;

                    reports = new XtraReport[] { xrptNominaIngresosCom, xrptNominaEgresosCom, xrptComisiones };
                }
                else if (poPeriodo.CodigoTipoRol == Diccionario.Tablas.TipoRol.FinMes)
                {
                    var dt = poLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPCONSULTACOMISIONES {0}", tIdPeriodo));
                    dt.TableName = "Comisiones";
                    if (dsComisiones.Tables["Comisiones"] != null) dsComisiones.Tables.Remove("Comisiones");

                    dsComisiones.Merge(dt);

                    var poComision = new clsNComision().goConsultarComision(tIdPeriodo);
                    //Se establece el origen de datos del reporte (El dataset previamente leído)
                    xrptComisiones.Parameters["Rol"].Value = string.Format("DEL {0} DE {1} AL {2} DE {3}", poPeriodo.FechaInicioComi.Value.Day, clsComun.gsGetMes(poPeriodo.FechaInicioComi.Value.Month), poPeriodo.FechaFinComi.Value.Day, clsComun.gsGetMes(poPeriodo.FechaFinComi.Value.Month));
                    xrptComisiones.Parameters["TotalCobranza"].Value = poComision?.BaseComision;
                    xrptComisiones.DataSource = dsComisiones;
                    //Se invoca la ventana que muestra el reporte.
                    xrptComisiones.RequestParameters = false;
                    xrptComisiones.Parameters["Rol"].Visible = false;
                    xrptComisiones.Parameters["TotalCobranza"].Visible = false;

                    if (dt.Rows.Count > 0)
                    {
                        reports = new XtraReport[] { xrptNominaIngresos, xrptNominaEgresos, xrptComisiones };
                    }
                    else
                    {
                        reports = new XtraReport[] { xrptNominaIngresos, xrptNominaEgresos };
                    }
                    
                }
                else
                {
                    reports = new XtraReport[] { xrptNominaIngresos, xrptNominaEgresos };
                }


                ReportPrintTool pt1 = new ReportPrintTool(xrptNominaIngresos);
                pt1.PrintingSystem.StartPrint += new PrintDocumentEventHandler(PrintingSystem_StartPrint);

                foreach (XtraReport report in reports)
                {
                    ReportPrintTool pts = new ReportPrintTool(report);
                    pts.PrintingSystem.StartPrint +=
                        new PrintDocumentEventHandler(reportsStartPrintEventHandler);
                }

                foreach (XtraReport report in reports)
                {
                    ReportPrintTool pts = new ReportPrintTool(report);
                    pts.ShowRibbonPreview();
                }
            }

            
        }
        

        /// <summary>
        /// Impresión de rol individual
        /// </summary>
        /// <param name="tIdPeriodo"></param>
        /// <param name="tsNumeroIdentificacion"></param>
        public static void gImprimirRolIndividual(int tIdPeriodo, string tsNumeroIdentificacion)
        {

            xrptRolIndividual xrpt = new xrptRolIndividual();
            DataSet ds = new DataSet();
            var poLogicaNegocio = new clsNNomina();
            var dsResult = poLogicaNegocio.gdsRol(tIdPeriodo, tsNumeroIdentificacion);
            dsResult.Tables[0].TableName = "RolCab";
            dsResult.Tables[1].TableName = "RolDet";
            
            
            ds.Merge(dsResult);
            xrpt.DataSource = ds;
            xrpt.RequestParameters = false;

            using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
            {
                printTool.ShowRibbonPreviewDialog();
            }
        }

        /// <summary>
        /// Impresión de rol individual
        /// </summary>
        /// <param name="tIdPeriodo"></param>
        /// <param name="tsNumeroIdentificacion"></param>
        public static void gImprimirRolMensual(int tIdPeriodo, string tsNumeroIdentificacion, string tsTipo)
        {

            xrptRolMensual xrpt = new xrptRolMensual();
            DataSet ds = new DataSet();
            var poLogicaNegocio = new clsNNomina();

            if (tsTipo == "C")
            {
                var dsResult = poLogicaNegocio.gdsRol(tIdPeriodo, tsNumeroIdentificacion);
                dsResult.Tables[0].TableName = "RolCab";
                dsResult.Tables[1].TableName = "RolDet";
                ds.Merge(dsResult);

                var poReg = poLogicaNegocio.goConsultarPeriodo(tIdPeriodo);
                var poRegCom = poLogicaNegocio.goConsultarPeriodo(string.Format("{2}{0}{1}", poReg.Anio, poReg.FechaInicio.Month.ToString("00"), tsTipo));

                var dsResultCom = poLogicaNegocio.gdsRol(poRegCom.IdPeriodo, tsNumeroIdentificacion);
                dsResultCom.Tables[0].TableName = "RolCab1";
                dsResultCom.Tables[1].TableName = "RolDet1";
                ds.Merge(dsResultCom);

            }
            else
            {
                var poReg = poLogicaNegocio.goConsultarPeriodo(tIdPeriodo);
                var poRegCom = poLogicaNegocio.goConsultarPeriodo(string.Format("{2}{0}{1}", poReg.Anio, poReg.FechaInicio.Month.ToString("00"), tsTipo));

                var dsResultCom = poLogicaNegocio.gdsRol(poRegCom.IdPeriodo, tsNumeroIdentificacion);
                dsResultCom.Tables[0].TableName = "RolCab";
                dsResultCom.Tables[1].TableName = "RolDet";
                ds.Merge(dsResultCom);

                var dsResult = poLogicaNegocio.gdsRol(tIdPeriodo, tsNumeroIdentificacion);
                dsResult.Tables[0].TableName = "RolCab1";
                dsResult.Tables[1].TableName = "RolDet1";
                ds.Merge(dsResult);


            }


            xrpt.DataSource = ds;
            xrpt.RequestParameters = false;

            using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
            {
                printTool.ShowRibbonPreviewDialog();
            }
        }

        /// <summary>
        /// Impresión de rol individual
        /// </summary>
        /// <param name="tIdPeriodo"></param>
        /// <param name="tsNumeroIdentificacion"></param>
        public static void gImprimirReportesCobranza(int tIdPeriodo)
        {
            var poLogicaNegocio = new clsNComisiones();
            var poPeriodo = poLogicaNegocio.goConsultarPeriodo(tIdPeriodo);
            //string psParametro = string.Format("{0} - {1}", clsComun.gsGetMes(poPeriodo.FechaInicio), poPeriodo.Anio);
            string psParametro = string.Format("DEL {0} DE {1} AL {2} DE {3}", poPeriodo.FechaInicioComi.Value.Day, clsComun.gsGetMes(poPeriodo.FechaInicioComi.Value.Month), poPeriodo.FechaFinComi.Value.Day, clsComun.gsGetMes(poPeriodo.FechaFinComi.Value.Month));
            /*************************************************************************************************************/
            xrptCobranzaZona xrpt = new xrptCobranzaZona();
            DataSet ds = new DataSet();
            
            var dsResult = poLogicaNegocio.gdsComisionesZona(tIdPeriodo);
            dsResult.Tables[0].TableName = "ComiZonaCab";
            dsResult.Tables[1].TableName = "ComiZonaDet";
            
            ds.Merge(dsResult);
            xrpt.DataSource = ds;
            xrpt.RequestParameters = false;

            xrpt.Parameters["Titulo"].Value = psParametro;
            xrpt.Parameters["Titulo"].Visible = false;
            /*************************************************************************************************************/
            xrptCobranzaZonaEmpleado xrptEmpleado = new xrptCobranzaZonaEmpleado();
            DataSet dsEmp = new DataSet();

            var dsResultEmp = poLogicaNegocio.gdsComisionesZonaEmpleado(tIdPeriodo);
            dsResultEmp.Tables[0].TableName = "ComiZonaEmpleado";

            dsEmp.Merge(dsResultEmp);
            xrptEmpleado.DataSource = dsEmp;
            xrptEmpleado.RequestParameters = false;

            xrptEmpleado.Parameters["Titulo"].Value = psParametro;
            xrptEmpleado.Parameters["Titulo"].Visible = false;
            /*************************************************************************************************************/
            xrptCobranzaEmpleado xrptEmple = new xrptCobranzaEmpleado();
            DataSet dsEm = new DataSet();

            var dsResultEm = poLogicaNegocio.gdsComisionesEmpleado(tIdPeriodo);
            dsResultEm.Tables[0].TableName = "Empleado";

            dsEm.Merge(dsResultEm);
            xrptEmple.DataSource = dsEm;
            xrptEmple.RequestParameters = false;

            xrptEmple.Parameters["Titulo"].Value = psParametro;
            xrptEmple.Parameters["Titulo"].Visible = false;
            /*************************************************************************************************************/
            XtraReport[] reports;

            reports = new XtraReport[] { xrpt, xrptEmpleado, xrptEmple };

            ReportPrintTool pt1 = new ReportPrintTool(xrpt);
            pt1.PrintingSystem.StartPrint += new PrintDocumentEventHandler(PrintingSystem_StartPrint);

            foreach (XtraReport report in reports)
            {
                ReportPrintTool pts = new ReportPrintTool(report);
                pts.PrintingSystem.StartPrint +=
                    new PrintDocumentEventHandler(reportsStartPrintEventHandler);
            }

            foreach (XtraReport report in reports)
            {
                ReportPrintTool pts = new ReportPrintTool(report);
                pts.ShowRibbonPreview();
            }

            //using (ReportPrintTool printTool = new ReportPrintTool(xrpt))
            //{
            //    printTool.ShowRibbonPreviewDialog();
            //}
        }

        public static void gImprimirCotizacionGanadora(int tIdCotizacion)
        {
            DataSet ds = new DataSet();
            var loLogicaNegocio = new clsNSolicitudCompra();
            var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC COMSPCONSULTACOTIZACIONGANADORA {0}", tIdCotizacion));
     
            dt.TableName = "CotizacionGanadora";
           
            ds.Merge(dt);
        
            if (dt.Rows.Count > 0)
            {
                xrptCotizacionGanadora xrpt = new xrptCotizacionGanadora();
                xrpt.DataSource = ds;
                xrpt.RequestParameters = false;

                using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                {
                    printTool.ShowRibbonPreviewDialog();
                }
            }
        }
       
        public static void gImprimirSolicitudCompra(int tIdSolicitud)
        {
            DataSet ds = new DataSet();
            var loLogicaNegocio = new clsNSolicitudCompra();
            var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC COMSPCONSULTASOLICITUDCOMPRA {0}", tIdSolicitud));
            var dtDetalle = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC COMSPCONSULTASOLICITUDCOMPRADETALLE {0}", tIdSolicitud));
            dt.TableName = "SolicitudCompra";
            dtDetalle.TableName = "SolicitudCompraDetalle";
            ds.Merge(dt);
            ds.Merge(dtDetalle);
            if (dt.Rows.Count > 0)
            {
                xrptSolicitudCompra xrpt = new xrptSolicitudCompra();
                xrpt.DataSource = ds;
                xrpt.RequestParameters = false;

                using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                {
                    printTool.ShowRibbonPreviewDialog();
                }
            }
        }

        public static void gImprimirOrdenCompra(int tIdOrdenCompra, string IdOrdenCompraProveedor)
        {
            DataSet ds = new DataSet();
            var loLogicaNegocio = new clsNSolicitudCompra();
            var dt = loLogicaNegocio.goConsultaDataTable("EXEC COMCONSULTAORDENCOMPRACABECERA " + "'" + tIdOrdenCompra + "'," + "'" + IdOrdenCompraProveedor + "'");
                dt.TableName = "CabeceraOrdenCompra";
            var dt2 = loLogicaNegocio.goConsultaDataTable("EXEC COMCONSULTAORDENCOMPRADETALLE " + "'" + tIdOrdenCompra + "'," + "'" + IdOrdenCompraProveedor + "'");
            dt2.TableName = "DetalleOrdenCompra";
            var dt3 = loLogicaNegocio.goConsultaDataTable("EXEC COMSPCONSULTAPARAMETRO");
            dt3.TableName = "Parametro";
            ds.Merge(dt);
            ds.Merge(dt2);
            ds.Merge(dt3);
            if (dt.Rows.Count > 0)
            {
                xrptOrdenCompraProveedor xrpt = new xrptOrdenCompraProveedor();
                xrpt.DataSource = ds;
                xrpt.RequestParameters = false;

                using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                {
                    printTool.ShowRibbonPreviewDialog();
                }
            }
        }
        public static void gImprimirOrdenPago(int tIdOrdenPago)
        {
            DataSet ds = new DataSet();
            var loLogicaNegocio = new clsNSolicitudCompra();
            var dt = loLogicaNegocio.goConsultaDataTable("EXEC COMSPCONSULTAORDENPAGO " + "'" + tIdOrdenPago + "'" );

            dt.TableName = "OrdenPago";
            ds.Merge(dt);
            if (dt.Rows.Count > 0)
            {
                xrptOrdenPago xrpt = new xrptOrdenPago();
                xrpt.DataSource = ds;
                xrpt.RequestParameters = false;

                using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                {
                    printTool.ShowRibbonPreviewDialog();
                }
            }
        }

        public static string gCorreoOrdenCompra(int tIdOrdenCompra, string IdOrdenCompraProveedor)
        {
            String Ruta = "";
            DataSet ds = new DataSet();
            var loLogicaNegocio = new clsNSolicitudCompra();
            var dt = loLogicaNegocio.goConsultaDataTable("EXEC COMCONSULTAORDENCOMPRACABECERA " + "'" + tIdOrdenCompra + "'," + "'" + IdOrdenCompraProveedor + "'");
            dt.TableName = "CabeceraOrdenCompra";
            var dt2 = loLogicaNegocio.goConsultaDataTable("EXEC COMCONSULTAORDENCOMPRADETALLE " + "'" + tIdOrdenCompra + "'," + "'" + IdOrdenCompraProveedor + "'");
            dt2.TableName = "DetalleOrdenCompra";
            var dt3 = loLogicaNegocio.goConsultaDataTable("EXEC COMSPCONSULTAPARAMETRO");
            dt3.TableName = "Parametro";
            ds.Merge(dt);
            ds.Merge(dt2);
            ds.Merge(dt3);
           
            if (dt.Rows.Count > 0)
            {
                xrptOrdenCompraProveedor xrpt = new xrptOrdenCompraProveedor();
                xrpt.DataSource = ds;
                xrpt.RequestParameters = false;

                // Specify export options.
                PdfExportOptions pdfExportOptions = new PdfExportOptions()
                {
                    PdfACompatibility = PdfACompatibility.PdfA1b
                };

                // Specify the path for the exported PDF file.  
                string pdfExportFile =
                    ConfigurationManager.AppSettings["FileTmpCom"].ToString() +
                   
                   "Afecor_OC_"+ tIdOrdenCompra  +
                    ".pdf";

                // Export the report.
                xrpt.ExportToPdf(pdfExportFile, pdfExportOptions);

                Ruta = pdfExportFile;
            }
            return Ruta;
        }

        public static void gObtenerDatosAtraso(TimeSpan toHoraEntrada, out TimeSpan toTiempoAtraso, out int tiMinutosAtraso)
        {
            toTiempoAtraso = Convert.ToDateTime("00:00:00").TimeOfDay;
            tiMinutosAtraso = 0;

            TimeSpan poHoraLlegada = Convert.ToDateTime("08:30:00").TimeOfDay;

            string psHora = toHoraEntrada.Hours.ToString("00") +":"+ toHoraEntrada.Minutes.ToString("00") +":"+ toHoraEntrada.Seconds.ToString("00");

            TimeSpan pdHoraEntrada = Convert.ToDateTime(psHora).TimeOfDay;
            TimeSpan poResult = pdHoraEntrada - poHoraLlegada;
            int piDif = poResult.Minutes;

            if(piDif > 0)
            {
                toTiempoAtraso = poResult;
                piDif = poResult.Minutes;
            }
        }
       

        /// <summary>
        /// Agregar boton delete a una Columna de un grid
        /// </summary>
        /// <param name="colXmlDown"></param>
        public static void gDibujarBotonGrid(RepositoryItemButtonEdit tbButton ,DevExpress.XtraGrid.Columns.GridColumn colXmlDown, string tsCaption, string tsRutaImage, int? tiWidth = null)
        {
            colXmlDown.Caption = tsCaption;
            colXmlDown.ColumnEdit = tbButton;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;
            colXmlDown.OptionsColumn.AllowEdit = true;

            tbButton.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            tbButton.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage(tsRutaImage);
            tbButton.TextEditStyle = TextEditStyles.HideTextEditor;
            
            if (tiWidth != null) colXmlDown.Width = tiWidth??0;


        }

        public static bool EnviarPorCorreo(string emailDestinatario, string Asunto, string cuerpoDelEmail, List<Attachment> adjuntos = null, 
            bool desplegarMensaje = false, string emailCC = "", string cuentaCorreoSaliente = "", string contraseñaCorreoSaliente = "", string nombrePresentar = "", int tIdEmpleadoNomina = 0)
        {
           
            var poParametro = new clsNParametro();
            poParametro.EnviarPorCorreo(emailDestinatario, Asunto, cuerpoDelEmail, adjuntos, desplegarMensaje, emailCC, cuentaCorreoSaliente,true,"", tIdEmpleadoNomina);
            
            if (desplegarMensaje)
            {
                MessageBox.Show("Correo enviado exitosamente.", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
                
            return true;

        }

        public static void gOrdenarColumnasGrid(GridView dgv)
        {
            dgv.OptionsCustomization.AllowColumnMoving = false;
            dgv.OptionsView.ColumnAutoWidth = true;
            dgv.OptionsView.ShowAutoFilterRow = true;
            //dgv.OptionsView.BestFitMode = GridBestFitMode.Full;
            dgv.BestFitColumns();
        }


        public static void gOrdenarColumnasGridFull(GridView dgv)
        {
            dgv.OptionsBehavior.Editable = false;
            dgv.OptionsCustomization.AllowColumnMoving = false;
            dgv.OptionsView.ColumnAutoWidth = false;
            dgv.OptionsView.ShowAutoFilterRow = true;
            dgv.OptionsView.BestFitMode = GridBestFitMode.Full;
            dgv.BestFitColumns();
            
        }

        public static void gOrdenarColumnasGridFullEditable(GridView dgv)
        {
            dgv.OptionsBehavior.Editable = true;
            dgv.OptionsCustomization.AllowColumnMoving = false;
            dgv.OptionsView.ColumnAutoWidth = false;
            dgv.OptionsView.ShowAutoFilterRow = true;
            dgv.OptionsView.BestFitMode = GridBestFitMode.Full;
            dgv.BestFitColumns();
        }

        public static void gOrdenarColumnasGridFullEditableNone(GridView dgv)
        {
            dgv.OptionsCustomization.AllowColumnMoving = false;
            dgv.OptionsView.ColumnAutoWidth = false;
            dgv.OptionsView.ShowAutoFilterRow = true;
            dgv.OptionsView.BestFitMode = GridBestFitMode.Full;
            dgv.BestFitColumns();
        }

        public static void gFormatearColumnasGrid(GridView dgvDatos, bool AddSummaryCustomPorcentaje = false)
        {
            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                // Quita numeral del nombre de la columna, sirve para convertir a número
                if (dgvDatos.Columns[i].CustomizationSearchCaption.Contains("#")) dgvDatos.Columns[i].Caption = dgvDatos.Columns[i].CustomizationSearchCaption.Replace("#", "").Trim();

                if (i > 0)
                {
                    if (dgvDatos.Columns[i].ColumnType == typeof(decimal))
                    {
                        var psNameColumn = dgvDatos.Columns[i].FieldName;
                        if (psNameColumn.ToUpper().Contains("%") || psNameColumn.ToUpper().Contains("PORCE"))
                        {
                            dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                            dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            dgvDatos.Columns[i].DisplayFormat.FormatString = "p2";

                            if (AddSummaryCustomPorcentaje)
                            {
                                dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Custom, psNameColumn, "{0:p2}");

                                GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                                item1.FieldName = psNameColumn;
                                item1.SummaryType = SummaryItemType.Custom;
                                item1.DisplayFormat = "{0:p2}";
                                item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                                dgvDatos.GroupSummary.Add(item1);
                            }
                            
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
                    else if (dgvDatos.Columns[i].ColumnType == typeof(int))
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

        public static void gFormatearColumnasBandedGrid(BandedGridView dgvDatos)
        {
            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                // Quita numeral del nombre de la columna, sirve para convertir a número
                if (dgvDatos.Columns[i].CustomizationSearchCaption.Contains("#")) dgvDatos.Columns[i].Caption = dgvDatos.Columns[i].CustomizationSearchCaption.Replace("#", "").Trim();

                if (i > 1)
                {
                    if (dgvDatos.Columns[i].ColumnType == typeof(decimal))
                    {
                        var psNameColumn = dgvDatos.Columns[i].FieldName;
                        if (psNameColumn.ToUpper().Contains("%") || psNameColumn.ToUpper().Contains("PORCE"))
                        {
                            dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                            dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            dgvDatos.Columns[i].DisplayFormat.FormatString = "p2";


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
                    else if (dgvDatos.Columns[i].ColumnType == typeof(int))
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

        public static void gsMensajePrevioImportar()
        {
            XtraMessageBox.Show("Antes de Importar asegurese de los siguientes puntos: \n" +
                    "- En las celdas numéricas no deben estar vacias, valor por defecto '0' \n" +
                    "- En las celdas númericas no deben tener formato alguno, por defecto general \n" +
                    "- El archivo debe ser excel y debe contener solo una hoja \n" +
                    "- Los nombres de las columnas deben estar como están en la plantilla", "Importante!!", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        public static void gsGuardaLogTxt(List<string> tsLista)
        {
            if (tsLista.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = ".TXT Files (*.txt)|*.txt";
                sfd.FileName = "Log_Errores.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string path = Path.GetDirectoryName(sfd.FileName);
                    string filename = Path.GetFileNameWithoutExtension(sfd.FileName);
                    string psRuta = sfd.FileName;
                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter sw = new StreamWriter(psRuta);

                    foreach (var reigstro in tsLista)
                    {
                        sw.WriteLine(reigstro);
                    }
                    
                    sw.Close();
                }
            }
        }

        /// <summary>
        /// Valida Registro para buscar un código de una lista
        /// </summary>
        /// <param name="toLista">Lista a Buscar</param>
        /// <param name="tsColumna">Columna del Excel</param>
        /// <param name="tsTipo">Tipo de Dato</param>
        /// <param name="tsValor">Valor de la Celda</param>
        /// <param name="tiFila">Fila del Excel</param>
        /// <param name="tbOgligatorio">Si es dato Obligatorio</param>
        /// <param name="tsMsg">Muestra el error de la fila</param>
        /// <returns></returns>
        public static dynamic gdValidarRegistro(string psColumna ,string tsTipo ,string tsValor, int tiFila, bool tbOgligatorio, ref string tsMsg)
        {
            
            if (tsTipo == "s")
            {
                if (!string.IsNullOrEmpty(tsValor))
                {
                    return tsValor;
                }
                else
                {
                    if (tbOgligatorio)
                    {
                        tsMsg  = string.Format("{0}En la Fila '{1}' No se encontró valor en la Columna '{2}'.\n",tsMsg, tiFila, psColumna);
                        return "";
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            else if (tsTipo == "d")
            {
                if (!string.IsNullOrEmpty(tsValor))
                {
                    try
                    {
                        tsValor = tsValor.Replace("$", "").Replace(",", "").Trim();
                        return Convert.ToDecimal(tsValor);
                    }
                    catch (Exception)
                    {
                        tsMsg = string.Format("{0}En la fila: '{1}' y en la columna '{2}' el valor '{3}' tiene un valor que no se puede convertir. Quitar formato de celda.\n", tsMsg, tiFila, psColumna, tsValor);
                        return 0M;
                    }
                    
                }
                else
                {
                    if (tbOgligatorio)
                    {
                        tsMsg = string.Format("{0}En la Fila '{1}' No se encontró valor en la Columna '{2}'.\n", tsMsg, tiFila, psColumna);
                        return 0M;
                    }
                    else
                    {
                        return 0M;
                    }
                }
            }
            else if (tsTipo == "i")
            {
                if (!string.IsNullOrEmpty(tsValor))
                {
                    try
                    {
                        return int.Parse(tsValor);
                    }
                    catch (Exception)
                    {
                        tsMsg = string.Format("{0}En la fila: '{1}' y en la columna '{2}' el valor '{3}' tiene un valor que no se puede convertir. Quitar formato de celda.\n", tsMsg, tiFila, psColumna, tsValor);
                        return 0;
                    }
                }
                else
                {
                    if (tbOgligatorio)
                    {
                        tsMsg = string.Format("{0}En la Fila '{1}' No se encontró valor en la Columna '{2}'.\n", tsMsg, tiFila, psColumna);
                        return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else if (tsTipo == "dt")
            {
                if (!string.IsNullOrEmpty(tsValor))
                {
                    
                    try
                    {
                        return Convert.ToDateTime(tsValor);
                    }
                    catch (Exception)
                    {
                        tsMsg = string.Format("{0}En la fila: '{1}' y en la columna '{2}' el valor '{3}' tiene un valor que no se puede convertir. Quitar formato de celda.\n", tsMsg, tiFila, psColumna, tsValor);
                        return null;
                    }
                }
                else
                {
                    if (tbOgligatorio)
                    {
                        tsMsg = string.Format("{0}En la Fila '{1}' No se encontró valor en la Columna '{2}'.\n", tsMsg, tiFila, psColumna);
                        return DateTime.MinValue;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else if (tsTipo == "t")
            {
                if (!string.IsNullOrEmpty(tsValor))
                {

                    try
                    {
                        return Convert.ToDateTime(tsValor).TimeOfDay;
                    }
                    catch (Exception)
                    {
                        tsMsg = string.Format("{0}En la fila: '{1}' y en la columna '{2}' el valor '{3}' tiene un valor que no se puede convertir. Quitar formato de celda.\n", tsMsg, tiFila, psColumna, tsValor);
                        return null;
                    }
                }
                else
                {
                    if (tbOgligatorio)
                    {
                        tsMsg = string.Format("{0}En la Fila '{1}' No se encontró valor en la Columna '{2}'.\n", tsMsg, tiFila, psColumna);
                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// Valida Registro para buscar un código de una lista
        /// </summary>
        /// <param name="toLista">Lista a Buscar</param>
        /// <param name="tsColumna">Columna del Excel</param>
        /// <param name="tsTipo">Tipo de Dato</param>
        /// <param name="tsValor">Valor de la Celda</param>
        /// <param name="tiFila">Fila del Excel</param>
        /// <param name="tbOgligatorio">Si es dato Obligatorio</param>
        /// <param name="tsMsg">Muestra el error de la fila</param>
        /// <returns></returns>
        public static dynamic gdValidarRegistro(List<Combo> toLista, string tsColumna, string tsTipo, string tsValor, int tiFila, bool tbOgligatorio, ref string tsMsg)
        {
            
            string psCode = toLista.Where(x => x.Descripcion == tsValor).Select(x => x.Codigo).FirstOrDefault();
            if (!string.IsNullOrEmpty(psCode))
            {
                if (tsTipo == "s")
                {
                    return psCode;   
                }
                else if (tsTipo == "d")
                {
                    return Convert.ToDecimal(psCode);
                }
                else if (tsTipo == "i")
                {
                    return int.Parse(psCode);
                }
            }
            else
            {
                tsMsg = string.Format("{0}No se encontró {1} con el nombre: '{2}' en la fila {3}.\n", tsMsg, tsColumna, tsValor, tiFila);
            }
            
            return null;
        }

        /// <summary>
        /// Valida Registro para buscar un código de una lista
        /// </summary>
        /// <param name="toLista">Lista a Buscar</param>
        /// <param name="tsColumna">Columna del Excel</param>
        /// <param name="tsTipo">Tipo de Dato</param>
        /// <param name="tsValor">Valor de la Celda</param>
        /// <param name="tiFila">Fila del Excel</param>
        /// <param name="tbOgligatorio">Si es dato Obligatorio</param>
        /// <param name="tsMsg">Muestra el error de la fila</param>
        /// <returns></returns>
        public static dynamic gdValidarRegistroContains(List<Combo> toLista, string tsColumna, string tsTipo, string tsValor, int tiFila, bool tbOgligatorio, ref string tsMsg)
        {

            string psCode = toLista.Where(x => x.Descripcion.Contains(tsValor)).Select(x => x.Codigo).FirstOrDefault();
            if (!string.IsNullOrEmpty(psCode))
            {
                if (tsTipo == "s")
                {
                    return psCode;
                }
                else if (tsTipo == "d")
                {
                    return Convert.ToDecimal(psCode);
                }
                else if (tsTipo == "i")
                {
                    return int.Parse(psCode);
                }
            }
            else
            {
                tsMsg = "";
            }

            return "0";
        }

        public static string gdValidarCodigo(List<Combo> toLista, string tsColumna, int tiFila, string tsCodigo, string tsNombre, ref string tsMsg)
        {
           
            string psCode = toLista.Where(x => x.Codigo == tsCodigo).Select(x => x.Codigo).FirstOrDefault();
            if (string.IsNullOrEmpty(psCode))
            {
                //Se aumenta un cero por si el valor al copiar y pegar en una celda se borró
                tsCodigo = "0" + tsCodigo;
                psCode = toLista.Where(x => x.Codigo == tsCodigo && tsCodigo != "0").Select(x => x.Codigo).FirstOrDefault();
                if (string.IsNullOrEmpty(psCode))
                {
                    tsMsg = string.Format("{0}No se encontró {1} '{2} - {3}':  en la fila {4}.\n", tsMsg, tsColumna, tsCodigo, tsNombre, tiFila);
                }
            }
            return psCode;
        }



        #region Métodos sin DevExpress

        /// <summary>
        /// Método Común que Exporta de una ruta de Excel a un Datatable
        /// </summary>
        /// <param name="ExcelFilePath"></param>
        /// <returns></returns>
        public static System.Data.DataTable gReadExcel(string tsExcelFilePath = null, string tsNombreHoja = "Hoja1")
        {
            //Create COM Objects. Create a COM object for everything that is referenced
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook xlWorkbook = xlApp.Workbooks.Open(tsExcelFilePath);

            _Worksheet xlWorksheet;

            try
            {
                xlWorksheet = xlWorkbook.Sheets[tsNombreHoja];
            }
            catch (Exception ex)
            {
                IndexOutOfRangeException poErrorNombreHoja = new IndexOutOfRangeException(String.Format("Error no existe la Hoja con nombre {0} en el archivo de Excel.", tsNombreHoja), ex);
                throw poErrorNombreHoja;
            }
            Range xlRange = xlWorksheet.UsedRange;

            int colCount = xlRange.Columns.Count;

            int rowIndex = 1;

            System.Data.DataTable dt = new System.Data.DataTable();
            for (int i = 1; i <= colCount; i++)
            {
                if (((Range)xlWorksheet.Cells[rowIndex, i]).Value2 != null)
                {
                    if (dt.Columns.Contains(Convert.ToString(((Range)xlWorksheet.Cells[rowIndex, i]).Value2)))
                    {
                        throw new Exception("ImportToExcel: se encontro columna repetida Nombre: " + Convert.ToString(((Range)xlWorksheet.Cells[rowIndex, i]).Value2) + " dentro del archivo !\n");
                    }
                    dt.Columns.Add(Convert.ToString(((Range)xlWorksheet.Cells[rowIndex, i]).Value2));
                }
            }
            rowIndex++;
            DataRow row = null;
            string psValor = ((Range)xlWorksheet.Cells[rowIndex, 1]).Value2 != null ? ((Range)xlWorksheet.Cells[rowIndex, 1]).Value2.ToString() : string.Empty;
            while (!string.IsNullOrEmpty(psValor))
            {
                row = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    row[i] = Convert.ToString(((Range)xlWorksheet.Cells[rowIndex, i + 1]).Value2);
                }
                dt.Rows.Add(row);
                rowIndex++;
                psValor = ((Range)xlWorksheet.Cells[rowIndex, 1]).Value2 != null ? ((Range)xlWorksheet.Cells[rowIndex, 1]).Value2.ToString() : string.Empty;
            }
            xlWorkbook.Close();
            xlApp.Quit();
            return dt;
        }
        /// <summary>
        /// Método Común que Exporta un Datatable en un Excel
        /// </summary>
        /// <param name="ExcelFilePath"></param>
        /// <returns></returns>
        public static void gExportToExcel(this System.Data.DataTable tdtDataTable, string tsExcelFilePath = null, string tsTitulo = null, string tsEncabezado = null, string tsFecha = null)
        {
            try
            {
                int ColumnsCount;

                if (tdtDataTable == null || (ColumnsCount = tdtDataTable.Columns.Count) == 0)
                    throw new Exception("ExportToExcel: No tiene datos la tabla!\n");

                Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application();
                Excel.Workbooks.Add();

                _Worksheet Worksheet = Excel.ActiveSheet;

                Range HeaderRange2 = Worksheet.get_Range((Range)(Worksheet.Cells[1, 1]), (Range)(Worksheet.Cells[1, ColumnsCount]));
                HeaderRange2.Cells[1, 1] = tsTitulo;

                HeaderRange2.Cells[2, 1] = tsEncabezado;
                HeaderRange2.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkRed);
                HeaderRange2.Font.Size = 14;
                HeaderRange2.Cells[2, 3] = tsFecha;
                HeaderRange2.Cells[2, 3].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                HeaderRange2.Cells[2, 3].Font.Size = 12;
                HeaderRange2.Font.Bold = true;

                HeaderRange2.Merge();

                HeaderRange2.HorizontalAlignment = Constants.xlCenter;

                object[] Header = new object[ColumnsCount];

                for (int i = 0; i < ColumnsCount; i++)
                    Header[i] = tdtDataTable.Columns[i].ColumnName;

                Range HeaderRange = Worksheet.get_Range((Range)(Worksheet.Cells[3, 1]), (Range)(Worksheet.Cells[3, ColumnsCount]));
                HeaderRange.Value = Header;
                HeaderRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                HeaderRange.Font.Bold = true;
                HeaderRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkRed);
                HeaderRange.Columns.AutoFit();

                int RowsCount = tdtDataTable.Rows.Count;
                object[,] Cells = new object[RowsCount + 4, ColumnsCount];

                for (int j = 0; j < RowsCount; j++)
                {
                    for (int i = 0; i < ColumnsCount; i++)
                    {
                        Cells[j, i] = tdtDataTable.Rows[j][i];
                    }

                }

                Worksheet.get_Range((Range)(Worksheet.Cells[4, 1]), (Range)(Worksheet.Cells[RowsCount + 6, ColumnsCount])).Value = Cells;

                if (tsExcelFilePath != null && tsExcelFilePath != "")
                {
                    try
                    {
                        Worksheet.SaveAs(tsExcelFilePath);
                        Excel.Quit();
                        MessageBox.Show("Excel guardado con exito!");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel no pudo ser guardado!"
                            + ex.Message);
                    }
                }
                else
                {
                    Excel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }

        /// <summary>
        /// Método Común que Exporta una lista de Data Tables en un Excel
        /// </summary>
        /// <param name="ExcelFilePath"></param>
        /// <returns></returns>
        public static void gExportToExcelVariosDataTable(this List<System.Data.DataTable> tdtDataTables, string tsExcelFilePath = null, string tsTitulo = null, string tsEncabezado = null, string tsFecha = null)
        {
            try
            {
                int ColumnsCount = 0;

                foreach (System.Data.DataTable tdtDataTable in tdtDataTables)
                {
                    if (tdtDataTable.Columns.Count > ColumnsCount && tdtDataTable != null)
                        ColumnsCount = tdtDataTable.Columns.Count;
                }

                Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application();
                Excel.Workbooks.Add();

                Microsoft.Office.Interop.Excel._Worksheet Worksheet = Excel.ActiveSheet;

                Microsoft.Office.Interop.Excel.Range HeaderRange2 = Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[1, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[1, ColumnsCount]));
                HeaderRange2.Cells[1, 1] = tsTitulo;

                HeaderRange2.Cells[2, 1] = tsEncabezado;
                HeaderRange2.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkRed);
                HeaderRange2.Font.Size = 14;
                HeaderRange2.Cells[2, 3] = tsFecha;
                HeaderRange2.Cells[2, 3].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                HeaderRange2.Cells[2, 3].Font.Size = 12;
                HeaderRange2.Font.Bold = true;

                HeaderRange2.Merge();

                HeaderRange2.HorizontalAlignment = Constants.xlCenter;

                foreach (System.Data.DataTable tdtDataTable in tdtDataTables)
                {

                    if (tdtDataTable == null || (ColumnsCount = tdtDataTable.Columns.Count) == 0)
                    {

                        object[] Header = new object[ColumnsCount];

                        for (int i = 0; i < ColumnsCount; i++)
                            Header[i] = tdtDataTable.Columns[i].ColumnName;

                        Microsoft.Office.Interop.Excel.Range HeaderRange = Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[3, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[3, ColumnsCount]));
                        HeaderRange.Value = Header;
                        HeaderRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        HeaderRange.Font.Bold = true;
                        HeaderRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkRed);
                        HeaderRange.Columns.AutoFit();

                        int RowsCount = tdtDataTable.Rows.Count;
                        object[,] Cells = new object[RowsCount + 4, ColumnsCount];

                        for (int j = 0; j < RowsCount; j++)
                        {
                            for (int i = 0; i < ColumnsCount; i++)
                            {
                                Cells[j, i] = tdtDataTable.Rows[j][i];
                            }

                        }

                        Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[4, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[RowsCount + 6, ColumnsCount])).Value = Cells;
                        if (tsExcelFilePath != null && tsExcelFilePath != "")
                        {
                            try
                            {
                                Worksheet.SaveAs(tsExcelFilePath);
                                Excel.Quit();
                                MessageBox.Show("Excel guardado con exito!");
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("ExportToExcel: Excel no pudo ser guardado!"
                                    + ex.Message);
                            }
                        }
                        else
                        {
                            Excel.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }
        /// <summary>
        /// Método Común que Obtiene Ruta para Descargar Archivos
        /// </summary>
        /// <param name="ExcelFilePath"></param>
        /// <returns></returns>
        public static string gObtenerRutaDescarga(string lsFileName)
        {
            string psRuta = string.Empty;
            string path = string.Empty;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog() { FileName = lsFileName, Filter = "excel archivos (*.xlsx)|*.xlsx", FilterIndex = 2 };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog1.ToString();
                psRuta = path.Replace("System.Windows.Forms.SaveFileDialog: Title: , FileName:", "");
                psRuta = psRuta.Trim();
                psRuta = psRuta + ".xlsx";
            }
            return psRuta;
        }
        /// <summary>
        /// Método Común que Obtiene Archivo para Subir
        /// </summary>
        public static string gObtenerArchivoCarga(string tsFiltro)
        {
            string psRuta = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = tsFiltro, FilterIndex = 1 };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                psRuta = openFileDialog.FileName;
            }
            return psRuta;
        }

  
        /// <summary>
        /// SUBIR ARCHIVO A SERVER FTP
        /// </summary>
        /// <param name="Server">Nombre del server ftp</param>
        /// <param name="Usuario"></param>
        /// <param name="Password"></param>
        /// <param name="ArchivoOrigen"></param>
        /// <param name="ArchivoDestino"></param>
        /// <returns></returns>
        public static bool SubirArchivoAFTP(string Server, string Usuario, string Password, string ArchivoOrigen, string ArchivoDestino = "")
        {
            try
            {
                if (ArchivoDestino.Length == 0)
                    ArchivoDestino = Path.GetFileName(ArchivoOrigen);

                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(String.Format(@"{0}/{1}", Server, ArchivoDestino));
                //MessageBox.Show(Server);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(Usuario, Password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = true;
                FileStream stream = File.OpenRead(ArchivoOrigen);
                //ZipStream = new GZipStream(stream,System.IO.Compression.CompressionMode.Compress);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();



                Stream reqStream = request.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Flush();
                reqStream.Close();
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                //Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
                response.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + (ex.InnerException != null ? ex.InnerException.Message : "") + " " + ArchivoOrigen + " " + ArchivoDestino);
                return false;
            }

        }

        

        /// <summary>
        /// DESCARGAR ARCHIVO A SERVER FTP
        /// </summary>
        /// <param name="Server">Nombre del server ftp</param>
        /// <param name="Usuario"></param>
        /// <param name="Password"></param>
        /// <param name="ArchivoOrigen"></param>
        /// <param name="ArchivoDestino"></param>
        /// <returns></returns>
        public static bool DescargarArchivoDFTP(string Server, string Usuario, string Password, string ArchivoOrigen, string NombreBaseD, string ServidorBD, string pathDestino = @"C:\USERS\PUBLIC")
        {
            try
            {
                using (WebClient request = new WebClient())
                {
                    request.Credentials = new NetworkCredential(Usuario, Password);
                    byte[] fileData = request.DownloadData(Server + "/" + ArchivoOrigen);



                    using (FileStream file = File.Create(pathDestino + @"\" + ArchivoOrigen))
                    {
                        file.Write(fileData, 0, fileData.Length);
                        file.Close();
                    }
                }



                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }



        }


      
        #endregion
    }
}
