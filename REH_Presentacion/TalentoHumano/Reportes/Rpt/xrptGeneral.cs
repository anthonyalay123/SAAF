using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraEditors;
using System.Collections.Generic;
using GEN_Entidad;
using System.Linq;

namespace REH_Presentacion.TalentoHumano.Reportes.Rpt
{
    /// <summary>
    /// Clase de Reporteador Dinámico
    /// 26/10/2021 VAR
    /// </summary>
    public partial class xrptGeneral : DevExpress.XtraReports.UI.XtraReport
    {

        /// <summary>
        /// Datatable que se envía por parámetro
        /// </summary>
        public DataTable dt;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public xrptGeneral()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento que se dispara para dibujar el reporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrptGeneral_BeforePrint(object sender, CancelEventArgs e)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    // Crea Tabla Detalle
                    lCrearTablaDetalle(dt);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error");
            }
        }

        /// <summary>
        /// Función que crea la tabla en el reporte
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="useStyles"></param>
        /// <param name="y"></param>
        private void lCrearTablaDetalle(DataTable dt, bool useStyles = false, float y = 0)
        {
            //Sección detalle
            DetailBand detail = Detail;
            detail.HeightF = 0;
            
            //Tabla a dibujar 
            XRTable detailTable = new XRTable();
            detail.Controls.Add(detailTable);

            //Inicio de Dibujar la tabla
            detailTable.BeginInit();
            
            //Variable de fila de cabecera
            XRTableRow headerRow = new XRTableRow();
            detailTable.Rows.Add(headerRow);

            //Cantidad de Columnas
            int colCount = dt.Columns.Count;

            //Se teermina el ancho de la página y los valores de ancho para cada columna
            float pageWidth = (this.PageWidth - (this.Margins.Left + this.Margins.Right));
            var poListaColumns = lColumnasWidth(dt, Convert.ToInt32(pageWidth));

            //float colWidth = pageWidth / colCount;
            float colWidth = 0;
            float pfLocationX = 0;
            
            //Creating an XRTableCell for each column in the corresponding DataTable
            for (int i = 0; i < colCount; i++)
            {
                string psNameColumn = dt.Columns[i].Caption;
                colWidth = poListaColumns.Where(x => x.index == i).Select(x => x.Width).FirstOrDefault();
                XRTableCell headerCell = new XRTableCell();
                headerRow.Cells.Add(headerCell);
                headerCell.WidthF = colWidth;
                headerCell.Text = psNameColumn;
                headerCell.Borders = DevExpress.XtraPrinting.BorderSide.All;
                headerCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                headerCell.Font = new Font(headerCell.Font.Name, 7.5F, FontStyle.Bold);

                //Cuando el tipo de dato sea decimal se agrega campo sumatoria
                if (dt.Columns[i].DataType == typeof(decimal))
                {
                    if (!psNameColumn.ToUpper().Contains("%") && !psNameColumn.ToUpper().Contains("PORC"))
                    {
                        // Se agrega campo sumatoria
                        XRLabel xrLabel = new XRLabel();
                        ReportFooter.Controls.AddRange(new XRControl[] {
                        xrLabel});
                        xrLabel.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                        xrLabel.Font = new Font("Times New Roman", 7.5F, FontStyle.Bold);
                        xrLabel.LocationFloat = new DevExpress.Utils.PointFloat(pfLocationX, 0);
                        xrLabel.SizeF = new SizeF(colWidth, 0);
                        //Decimal TotalPrice = Convert.ToDecimal(dt.Compute("SUM("+ psNameColumn + ")", string.Empty));
                        //var sumObject = dt.AsEnumerable().Where(row => row.Field<decimal>(psNameColumn) > 0).Sum(row => row.Field<decimal>(psNameColumn));
                        decimal total = 0M;
                        foreach (DataRow item in dt.Rows)
                        {
                            if (!string.IsNullOrEmpty(item[psNameColumn].ToString()))
                            {
                                total += Convert.ToDecimal(item[psNameColumn].ToString());
                            }
                        }
                        xrLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

                        if (psNameColumn.ToUpper().Contains("#") || psNameColumn.ToUpper().Contains("CANT") || psNameColumn.ToUpper().Contains("UNI"))
                        {
                            xrLabel.Text = string.Format("{0:n2}", Convert.ToDecimal(total));
                        }
                        else if (psNameColumn.ToUpper().Contains("PESO"))
                        {
                            xrLabel.Text = string.Format("{0:##,##0.0000}", Convert.ToDecimal(total));
                        }
                        else
                        {
                            xrLabel.Text = string.Format("{0:c2}", Convert.ToDecimal(total));
                        }
                    }
                  
                }
                pfLocationX += colWidth;
            }

            //Se dibuja las celdas detalle en la tabla
            foreach (DataRow item in dt.Rows)
            {
                XRTableRow detailRow = new XRTableRow();
                detailTable.Rows.Add(detailRow);

                for (int i = 0; i < colCount; i++)
                {
                    string psValor = item[i].ToString();
                    string psNameColumn = poListaColumns.Where(x => x.index == i).Select(x => x.Columna).FirstOrDefault();
                    colWidth = poListaColumns.Where(x => x.index == i).Select(x => x.Width).FirstOrDefault();

                    XRTableCell detailCell = new XRTableCell();
                    detailRow.Cells.Add(detailCell);
                    detailCell.WidthF = colWidth;
                    detailCell.Font = new Font("Times New Roman", 7.5F);
                    detailCell.Borders = DevExpress.XtraPrinting.BorderSide.All;
                    detailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

                    if (dt.Columns[i].DataType == typeof(decimal) && !string.IsNullOrEmpty(psValor))
                    {
                        if (psNameColumn.ToUpper().Contains("%") || psNameColumn.ToUpper().Contains("PORC"))
                        {
                            detailCell.Text = string.Format("{0:p2}", Convert.ToDecimal(psValor));
                        }
                        else if (psNameColumn.ToUpper().Contains("#") || psNameColumn.ToUpper().Contains("CANT") || psNameColumn.ToUpper().Contains("UNI"))
                        {
                            detailCell.Text = string.Format("{0:n2}", Convert.ToDecimal(psValor));
                        }
                        else if (psNameColumn.ToUpper().Contains("PESO"))
                        {
                            detailCell.Text = string.Format("{0:##,##0.0000}", Convert.ToDecimal(psValor));
                        }
                        else
                        {
                            detailCell.Text = string.Format("{0:c2}", Convert.ToDecimal(psValor));
                        }
                    }
                    else if (dt.Columns[i].DataType == typeof(int) && !string.IsNullOrEmpty(psValor))
                    {
                        if (psNameColumn.ToUpper().Contains("#") || psNameColumn.ToUpper().Contains("CANT") || psNameColumn.ToUpper().Contains("UNI"))
                        {
                            detailCell.Text = string.Format("{0:#,#}", Convert.ToInt32(psValor));
                        }
                        else
                        {
                            detailCell.Text = psValor;
                        }
                    }
                    else if (dt.Columns[i].DataType == typeof(DateTime) && !string.IsNullOrEmpty(psValor))
                    {
                        detailCell.Text = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(psValor));
                    }
                    else
                    {
                        detailCell.Text = psValor;
                    }

                }
            }

            //Finalizamos la edición de la tabla detalle
            detailTable.EndInit();


            //Ubicamos la tabla en el repote
            detailTable.LocationF = new PointF(0, y);
            //Se define el ancho de la Columna
            detailTable.WidthF = pageWidth;

            if (useStyles)
            {
                detailTable.EvenStyleName = "EvenStyle";
                detailTable.OddStyleName = "OddStyle";
            }



        }

        /// <summary>
        /// Determina la lista de columnas que existen para dibujar obteniendo tambien datos de su Index - Ancho de la Columna y Longitud de Caracteres
        /// </summary>
        /// <param name="dt"> DataTable con la información a mostrar en el reporte</param>
        /// <param name="pageWidth"> Ancho de la Página menos los margenes </param>
        /// <returns></returns>
        private List<tmpColumnaWidth> lColumnasWidth(DataTable dt, int pageWidth)
        {
            // Lista de Columans con sus propiedades
            List<tmpColumnaWidth> poLista = new List<tmpColumnaWidth>();

            // Determina las columnas a Dibujar con el tamaño máximo de Longitud por cada columna
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string psColumn = dt.Columns[i].Caption;
                foreach (DataRow item in dt.Rows)
                {
                    int len = item[psColumn].ToString().Length;

                    var poRegistro = poLista.Where(x => x.Columna == psColumn).FirstOrDefault();
                    
                    if (poRegistro == null)
                    {
                        poRegistro = new tmpColumnaWidth();
                        poRegistro.Len = len;
                        poRegistro.Columna = psColumn;
                        poLista.Add(poRegistro);
                    }
                    else
                    {
                        if (poRegistro.Len < len)
                        {
                            poRegistro.Len = len;
                        }
                    }
                }
            }

            // Pone el Indice por Columna y en caso de tener una longitud de caracteres menor a 10 le pone 10
            int Index = 0;
            foreach (var item in poLista)
            {
                item.index = Index;
                if (item.Len < 10)
                {
                    item.Len = 10;
                }
                Index++;
            }

            // Prorrateo de columnas en base a la longitud de sus carecteres y se determina el ancho que dete dener cada columna
            decimal piTotal = poLista.Sum(x => x.Len);
            foreach (var item in poLista)
            {
                decimal pdcPorc = item.Len / piTotal;
                decimal pdcValor = (pdcPorc * pageWidth);
                int piValor = int.Parse(Math.Round(pdcValor,0).ToString());
                item.Width = piValor;
            }

            // Retorna la Lista
            return poLista;
        }
        
        /// <summary>
        /// Entidad que se usa para Obtener atributos de la columna
        /// </summary>
        public class tmpColumnaWidth
        {
            public string Columna { get; set; }
            public float Width { get; set; }
            public int Len { get; set; }
            public int index { get; set; }
        }
    }
}
