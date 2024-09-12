using DevExpress.Charts.Native;
using DevExpress.XtraPrinting;
using REH_Presentacion;
using REH_Presentacion.Ventas.Reportes.Rpt;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using VTA_Negocio;
using static GEN_Entidad.Diccionario;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using REH_Presentacion.Ventas.Reportes;
using GEN_Entidad;
using REH_Presentacion.Cobranza.Reportes;

namespace VTA_EnvioKilosLitros
{
    class clsPrincipal
    {
        static void Main(string[] args)
        {
            var path = Application.StartupPath + "\\log.txt";
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("Inicio");

            clsNZonas loLogicaNegocio = new clsNZonas();

            try
            {

                DateTime pdFecha = DateTime.Now;

                string psDiaSemana = pdFecha.DayOfWeek.ToString();
                DateTime pdFechaUltimoDia = new DateTime(pdFecha.Year, pdFecha.Month, 1).AddMonths(1).AddDays(-1);
                
                
                if (psDiaSemana == "Monday" || pdFecha.Day == pdFechaUltimoDia.Day)
                {
                    DateTime pdFechaInicio = new DateTime(pdFecha.Year, pdFecha.Month, 1);
                    DateTime pdFechaFin = pdFecha;

                    string psRuta = "";
                    var poListaZonas = loLogicaNegocio.goConsultarZonaGrupo();
                    var poListaEmpleados = loLogicaNegocio.goConsultaEmpleados();

                    foreach (var item in poListaZonas)
                    {
                        
                        System.Data.DataSet ds1 = new System.Data.DataSet();
                        string psParametro1 = "";
                        var dt1 = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC [dbo].[VTA_SP_CUMPLIMIENTO_KILOS_LITROS_X_ZONA_AF] '{0}', '{1}', {2} ", pdFechaInicio.Date, pdFechaFin.Date, item.IdZonaGrupo));
                        dt1.TableName = "KilosLitros";
                        ds1.Merge(dt1);
                        if (dt1.Rows.Count > 0)
                        {

                            string psCorreoPara = "";
                            string psCorreoCC = "";

                            foreach (var det in item.ZonaGrupoEnvioCorreo)
                            {
                                var psCorreo = poListaEmpleados.Where(x => x.IdPersona == det.IdPersona).Select(x => x.Correo).FirstOrDefault();

                                if (!string.IsNullOrEmpty(psCorreo))
                                {
                                    if (det.TipoDestinatario == "PAR")
                                    {
                                        psCorreoPara = string.Format("{0}{1};", psCorreoPara,psCorreo.ToLower());
                                    }
                                    else if (det.TipoDestinatario == "COP")
                                    {
                                        psCorreoCC = string.Format("{0}{1};", psCorreoCC, psCorreo.ToLower());
                                    }
                                }
                            }

                            if (psCorreoPara.Length > 0)
                            {
                                psCorreoPara = psCorreoPara.Substring(0, psCorreoPara.Length - 1);
                            }

                            if (psCorreoCC.Length > 0)
                            {
                                psCorreoCC = psCorreoCC.Substring(0, psCorreoCC.Length - 1);
                            }

                            var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC [VTA_SP_CUMPLIMIENTO_KILOS_LITROS_X_ZONA_AF_CORREO] '{0}', '{1}', {2}", pdFechaInicio.Date, pdFechaFin.Date, item.IdZonaGrupo));

                            if (!string.IsNullOrEmpty(psCorreoPara) || !string.IsNullOrEmpty(psCorreoCC))
                            {
                                if (string.IsNullOrEmpty(psCorreoPara))
                                {
                                    psCorreoPara = psCorreoCC;
                                }

                                psParametro1 = string.Format("VENTAS DEL {0} AL {1} (AF)", pdFechaInicio.Date.ToString("dd/MM/yyyy"), pdFechaFin.Date.ToString("dd/MM/yyyy"));

                                xrptKilosLitrosZona xrpt = new xrptKilosLitrosZona();

                                //Se establece el origen de datos del reporte (El dataset previamente leído)
                                xrpt.Parameters["Titulo"].Value = psParametro1;
                                xrpt.DataSource = ds1;
                                //Se invoca la ventana que muestra el reporte.
                                xrpt.RequestParameters = false;
                                xrpt.Parameters["Titulo"].Visible = false;

                                // Specify export options.
                                PdfExportOptions pdfExportOptions = new PdfExportOptions()
                                {
                                    PdfACompatibility = PdfACompatibility.PdfA1b
                                };

                                // Specify the path for the exported PDF file.  
                                string pdfExportFile =
                                   @"\\192.168.11.9\adjuntos\SAAF\COM\TMP\Presupuesto Kilos-Listros vs Real.pdf"; //+ "_" + pdFechaInicio.Date.ToString("ddMMyyyy") + "_" + pdFechaFin.Date.ToString("ddMMyyyy") + ".pdf";

                                // Export the report.
                                xrpt.ExportToPdf(pdfExportFile, pdfExportOptions);

                                psRuta = pdfExportFile;

                                List<Attachment> listAdjuntosEmail = new List<Attachment>();
                                if (File.Exists(psRuta))
                                    listAdjuntosEmail.Add(new Attachment(psRuta));
                                
                                //clsComun.EnviarPorCorreo("varevalo@afecor.com",dt.Rows[0]["Asunto"].ToString(), dt.Rows[0]["Cuerpo"].ToString(), listAdjuntosEmail, false, "varevalo@afecor.com;jordonez@afecor.com");
                                clsComun.EnviarPorCorreo(psCorreoPara, dt.Rows[0]["Asunto"].ToString(), dt.Rows[0]["Cuerpo"].ToString(), listAdjuntosEmail,false,psCorreoCC);

                                if (File.Exists(psRuta)) File.Delete(psRuta);

                                //break;
                            }

                        }
                        
                    }
                }

                if (psDiaSemana == "Monday" || psDiaSemana == "Wednesday")
                {
                    // Envío de stock de productos automático
                    var dtCorreos = loLogicaNegocio.goConsultaDataTable("EXEC VTASPEMPLEADOSCORREO");
                    string psCorreos = "";
                    foreach (DataRow item in dtCorreos.Rows)
                    {
                        psCorreos += item["CorreoLaboral"].ToString() + ";";
                    }

                    var dtCorreosCopia = loLogicaNegocio.goConsultaDataTable("EXEC VTASPEMPLEADOSCORREO 'COPIA'");
                    string psCorreosCopia = "";
                    foreach (DataRow item in dtCorreosCopia.Rows)
                    {
                        psCorreosCopia += item["CorreoLaboral"].ToString() + ";";
                    }

                    if (!string.IsNullOrEmpty(psCorreos))
                    {
                        psCorreos = psCorreos.Substring(0, psCorreos.Length - 1);
                        psCorreosCopia = psCorreosCopia.Substring(0, psCorreosCopia.Length - 1);
                        DataTable dt = loLogicaNegocio.gdtStockProductos(null, true);
                        frmRpStockProductos frm = new frmRpStockProductos();
                        frm.Tag = 1 + "," + "";
                        frm.Show();
                        frm.lEnviarCorreoStockBodega(psCorreos, @"\\192.168.11.9\adjuntos\SAAF\COM\TMP\StockBodegas.xlsx", psCorreosCopia);
                    }

                    // Envío de Cartera por Zona automático
                    var poZonas = loLogicaNegocio.goConsultarZonasSAP();
                    string psZonas = "";
                    foreach (var item in poZonas)
                    {
                        psZonas += item.Codigo + ",";
                    }

                    if (!string.IsNullOrEmpty(psZonas))
                    {
                        psZonas = psZonas.Substring(0, psZonas.Length - 1);

                        frmRpEnviarCarteraZona frm = new frmRpEnviarCarteraZona();
                        frm.Tag = 1 + "," + "";
                        frm.Show();
                        frm.lEnviarCorreoCarteraPorZona(psZonas, @"\\192.168.11.9\adjuntos\SAAF\COM\TMP\CarteraPorZona.xlsx", true);
                    }
                }
            }
            catch (Exception ex)
            {
                loLogicaNegocio.EnviarPorCorreo("varevalo@afecor.com;jordonez@afecor.com", "Error en Envío de Correo - Ventas Kilos-Litros", ex.Message, null, false, "", "");
            }

            sw.Close();

        }
    }
}
