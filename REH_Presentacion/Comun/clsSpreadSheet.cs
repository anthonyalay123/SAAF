using DevExpress.Spreadsheet;
using DevExpress.XtraEditors;
using GEN_Entidad;
using REH_Negocio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Comun
{
    public static class clsSpreadSheet
    {

        public static void gPlantillaRDEP(int Anio, int IdPersona)
        {
            Cursor.Current = Cursors.WaitCursor;

            Combo FilePdf;
            Combo FileXlsx;
            GuardaFormulario107(Anio, IdPersona, out FilePdf, out FileXlsx);
            frmVerPdf pofrmVerPdf = new frmVerPdf();
            pofrmVerPdf.lsRuta = FilePdf.Codigo;
            pofrmVerPdf.Show();
            pofrmVerPdf.SetDesktopLocation(0, 0);
            pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;

            /*
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = dt.Rows[0][5].ToString() + "_" + dt.Rows[0][6].ToString() + "_107";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(saveFileDialog1.FileName + Path.GetExtension(ArchivoXlsx)))
                {
                    File.Delete(saveFileDialog1.FileName + Path.GetExtension(ArchivoXlsx));
                }
                File.Copy(ArchivoXlsx, saveFileDialog1.FileName + Path.GetExtension(ArchivoXlsx));
                XtraMessageBox.Show("Archivo Guardado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            */

            Cursor.Current = Cursors.Default;
        }

        public static void gDescargarPlantillaRDEP(int Anio, List<int> IdPersona)
        {
            Cursor.Current = Cursors.WaitCursor;

            List<Combo> FilesPdfs = new List<Combo>();
            List<Combo> FilesXlsxs = new List<Combo>();

            foreach (var item in IdPersona)
            {
                Combo FilePdf;
                Combo FileXlsx;
                GuardaFormulario107(Anio, item, out FilePdf, out FileXlsx);

                FilesPdfs.Add(FilePdf);
                FilesXlsxs.Add(FileXlsx);
            }

            if (FilesPdfs.Count > 0)
            {
                var folderBrowserDialog1 = new FolderBrowserDialog();

                // Show the FolderBrowserDialog.
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string folderName = folderBrowserDialog1.SelectedPath;

                    foreach (var item in FilesPdfs)
                    {
                        string fileNew = folderName + "\\" + item.Descripcion;
                        if (File.Exists(fileNew))
                        {
                            File.Delete(fileNew);
                        }
                        File.Copy(item.Codigo, fileNew);
                    }

                    XtraMessageBox.Show("Archivo(s) Descargado(s)", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            

            /*
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = dt.Rows[0][5].ToString() + "_" + dt.Rows[0][6].ToString() + "_107";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(saveFileDialog1.FileName + Path.GetExtension(ArchivoXlsx)))
                {
                    File.Delete(saveFileDialog1.FileName + Path.GetExtension(ArchivoXlsx));
                }
                File.Copy(ArchivoXlsx, saveFileDialog1.FileName + Path.GetExtension(ArchivoXlsx));
                XtraMessageBox.Show("Archivo Guardado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            */

            Cursor.Current = Cursors.Default;
        }

        private static void GuardaFormulario107(int Anio, int IdPersona, out Combo FilePdf, out Combo FileXlsx)
        {
            FilePdf = new Combo();
            FileXlsx = new Combo();

            string psFile = ConfigurationManager.AppSettings["FilePlantillaRDEP"].ToString();
            string Carpeta = ConfigurationManager.AppSettings["CarpetaPlantillaRDEP"].ToString();

            var loLogica = new clsNNomina();

            var dt = loLogica.goConsultaDataTable(string.Format("EXEC REHSPGCRDEP '{0}','0','1','{1}'", Anio, IdPersona));

            if (dt.Rows.Count > 0)
            {

                // Create an instance of a workbook.
                Workbook workBook = new Workbook();

                workBook.LoadDocument(psFile, DocumentFormat.Xlsx);

                // Access the first worksheet in the workbook.
                Worksheet workSheet = workBook.Worksheets[0];

                workSheet["O4:R5"].Value = Anio.ToString();
                workSheet["Z5:AC5"].Value = DateTime.Now.Year;
                workSheet["AD5:AF5"].Value = DateTime.Now.Month;
                workSheet["AH5:AI5"].Value = DateTime.Now.Day;
                workSheet["B11:N11"].Value = dt.Rows[0][4].ToString();
                workSheet["P11:AI11"].Value = dt.Rows[0][5].ToString() + " " + dt.Rows[0][6].ToString();
                workSheet.Range["V14:AI14"].Value = Convert.ToDouble(dt.Rows[0][15].ToString());
                workSheet.Range["V15:AI15"].Value = Convert.ToDouble(dt.Rows[0][16].ToString());
                workSheet.Range["V16:AI16"].Value = Convert.ToDouble(dt.Rows[0][17].ToString());
                workSheet.Range["V17:AI17"].Value = Convert.ToDouble(dt.Rows[0][18].ToString());
                workSheet.Range["V18:AI18"].Value = Convert.ToDouble(dt.Rows[0][20].ToString());
                workSheet.Range["V19:AI19"].Value = Convert.ToDouble(dt.Rows[0][21].ToString());
                workSheet.Range["V20:AI20"].Value = Convert.ToDouble(dt.Rows[0][22].ToString());
                workSheet.Range["V21:AI21"].Value = Convert.ToDouble(dt.Rows[0][24].ToString());
                workSheet.Range["V22:AI22"].Value = Convert.ToDouble(dt.Rows[0][27].ToString());
                workSheet.Range["V23:AI23"].Value = Convert.ToDouble(dt.Rows[0][28].ToString());
                workSheet.Range["V24:AI24"].Value = Convert.ToDouble(dt.Rows[0][29].ToString());
                workSheet.Range["V25:AI25"].Value = Convert.ToDouble(dt.Rows[0][34].ToString());
                workSheet.Range["V26:AI26"].Value = Convert.ToDouble(dt.Rows[0][30].ToString());
                workSheet.Range["V27:AI27"].Value = Convert.ToDouble(dt.Rows[0][31].ToString());
                workSheet.Range["V28:AI28"].Value = Convert.ToDouble(dt.Rows[0][32].ToString());
                workSheet.Range["V29:AI29"].Value = Convert.ToDouble(dt.Rows[0][33].ToString());

                workSheet.Range["V30:AI30"].Value = Convert.ToDouble(dt.Rows[0][35].ToString());
                workSheet.Range["V31:AI31"].Value = Convert.ToDouble(dt.Rows[0][36].ToString());
                workSheet.Range["V32:AI32"].Value = Convert.ToDouble(dt.Rows[0][19].ToString());
                workSheet.Range["V33:AI33"].Value = Convert.ToDouble(dt.Rows[0][37].ToString());
                workSheet.Range["V34:AI34"].Value = Convert.ToDouble(dt.Rows[0][39].ToString());
                workSheet.Range["V35:AI35"].Value = Convert.ToDouble(dt.Rows[0][40].ToString());
                workSheet.Range["V36:AI36"].Value = Convert.ToDouble(dt.Rows[0][41].ToString());

                workSheet.Range["V37:AI37"].Value = Convert.ToDouble(dt.Rows[0][42].ToString());
                workSheet.Range["V38:AI38"].Value = Convert.ToDouble(dt.Rows[0][43].ToString());
                workSheet.Range["V39:AI39"].Value = Convert.ToDouble(dt.Rows[0][44].ToString());
                workSheet.Range["V40:AI40"].Value = Convert.ToDouble((Convert.ToDecimal(dt.Rows[0][15].ToString()) + Convert.ToDecimal(dt.Rows[0][16].ToString()) + Convert.ToDecimal(dt.Rows[0][17].ToString()) + Convert.ToDecimal(dt.Rows[0][33].ToString())).ToString());

                workSheet.Range["V38:AI38"].NumberFormat = "$#,##0.00";

                //var psFileXlsx = Carpeta + dt.Rows[0][5].ToString() + "_" + dt.Rows[0][6].ToString() + "_107.xlsx";
                //workBook.SaveDocument(psFileXlsx, DocumentFormat.OpenXml);

                //FileXlsx.Codigo = psFileXlsx;
                //FileXlsx.Descripcion = dt.Rows[0][5].ToString() + "_" + dt.Rows[0][6].ToString() + "_107.dpf";

                //workSheet.PrintOptions.PrintGridlines = true;
                var  psFilePdf = Carpeta + dt.Rows[0][5].ToString() + "_" + dt.Rows[0][6].ToString() + "_107.dpf";
                workBook.ExportToPdf(psFilePdf);

                FilePdf.Codigo = psFilePdf;
                FilePdf.Descripcion = dt.Rows[0][5].ToString() + "_" + dt.Rows[0][6].ToString() + "_107.dpf";
            }
        }

    }
}
