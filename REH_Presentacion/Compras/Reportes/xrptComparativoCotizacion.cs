using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using COM_Negocio;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace REH_Presentacion.Compras.Reportes
{
    public partial class xrptComparativoCotizacion : DevExpress.XtraReports.UI.XtraReport
    {
        public int lIdCotizacion;

        public xrptComparativoCotizacion()
        {
            InitializeComponent();
        }

        private void xrptComparativoCotizacion_BeforePrint(object sender, CancelEventArgs e)
        {
            string dataMember = ""; string Color = ""; string backColor = "";
            bool useStyles = false;

          var  dt1 = new clsNCotizacion().goConsultaDataTable(string.Format("EXEC COMSPCONSULTAPIVOTPROVEEDOR {0}", lIdCotizacion));

            var dt2 = new clsNCotizacion().goConsultaDataTable(string.Format("EXEC COMSPCONSULTAPIVOTDETALLE {0}", lIdCotizacion));


            lCrearTablaDetalle(dt1);

            xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 75);
         

            lCrearTablaDetalle(dt2, false, 100);



        }

        private void lCrearTabla(DataTable dt, bool useStyles = false, float y = 0)
        {

            DetailBand detail = Detail;
            //this.Bands.Add(detail);
            detail.HeightF = 0;

            // Creating Header and Detail Tables
            XRTable headerTable = new XRTable();
            this.Bands[BandKind.ReportHeader].Controls.Add(headerTable);
            //this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            //headerTable});

            XRTable detailTable = new XRTable();
            detail.Controls.Add(detailTable);

            detailTable.BeginInit();
            headerTable.BeginInit();

            XRTableRow headerRow = new XRTableRow();
            headerTable.Rows.Add(headerRow);


            int colCount = dt.Columns.Count;

            float pageWidth = (this.PageWidth - (this.Margins.Left + this.Margins.Right));
            float colWidth = pageWidth / colCount;


            var poListaColumns = lColumnasWidth(dt, Convert.ToInt32(pageWidth));
            //Creating an XRTableCell for each column in the corresponding DataTable
            for (int i = 0; i < colCount; i++)
            {

                colWidth = poListaColumns.Where(x => x.index == i).Select(x => x.Width).FirstOrDefault();
                XRTableCell headerCell = new XRTableCell();
                headerRow.Cells.Add(headerCell);
                headerCell.WidthF = colWidth;
                headerCell.Text = dt.Columns[i].Caption;
                headerCell.Borders = DevExpress.XtraPrinting.BorderSide.All;

            }

            foreach (DataRow item in dt.Rows)
            {
                XRTableRow detailRow = new XRTableRow();
                detailTable.Rows.Add(detailRow);

                for (int i = 0; i < colCount; i++)
                {
                    string psValor = item[i].ToString();
                    XRTableCell detailCell = new XRTableCell();
                    detailRow.Cells.Add(detailCell);
                    detailCell.WidthF = colWidth;

                    detailCell.Borders = DevExpress.XtraPrinting.BorderSide.All;

                    if (item[i].ToString().Contains("."))
                    {
                        detailCell.Text = string.Format("{0:c2}", Convert.ToDecimal(psValor));
                    }
                    else
                    {
                        detailCell.Text = psValor;
                    }

                }
            }

            headerTable.EndInit();
            detailTable.EndInit();

            headerTable.LocationF = new PointF(0, this.Bands[BandKind.ReportHeader].HeightF);
            headerTable.WidthF = pageWidth;
            headerTable.Font = new Font(headerTable.Font, FontStyle.Bold);

            detailTable.LocationF = new PointF(0, y);
            detailTable.WidthF = pageWidth;
            //detailTable.BackColor = backColor;
            //Applying styles if necessary
            if (useStyles)
            {
                detailTable.EvenStyleName = "EvenStyle";
                detailTable.OddStyleName = "OddStyle";
            }
        }

        private void lCrearTablaDetalle(DataTable dt, bool useStyles = false, float y = 0)
        {

            DetailBand detail = Detail;
            //this.Bands.Add(detail);
            detail.HeightF = 0;

            // Creating Header and Detail Tables
            //XRTable headerTable = new XRTable();
            //this.Bands[BandKind.ReportHeader].Controls.Add(headerTable);
            //this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            //headerTable});

            XRTable detailTable = new XRTable();
            detail.Controls.Add(detailTable);

            detailTable.BeginInit();
           // headerTable.BeginInit();

            XRTableRow headerRow = new XRTableRow();
            //headerTable.Rows.Add(headerRow);
            detailTable.Rows.Add(headerRow);

            int colCount = dt.Columns.Count;

            float pageWidth = (this.PageWidth - (this.Margins.Left + this.Margins.Right));
            float colWidth = pageWidth / colCount;
            var poListaColumns = lColumnasWidth(dt, Convert.ToInt32(pageWidth));
            //Creating an XRTableCell for each column in the corresponding DataTable
            for (int i = 0; i < colCount; i++)
            {
                colWidth = poListaColumns.Where(x => x.index == i).Select(x => x.Width).FirstOrDefault();
                XRTableCell headerCell = new XRTableCell();
                headerRow.Cells.Add(headerCell);
                headerCell.WidthF = colWidth;
                headerCell.Text = dt.Columns[i].Caption;
                headerCell.Borders = DevExpress.XtraPrinting.BorderSide.All;
                headerCell.Font = new Font(headerCell.Font.Name, 12, FontStyle.Bold);
                headerCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            }

            foreach (DataRow item in dt.Rows)
            {
                XRTableRow detailRow = new XRTableRow();
                detailTable.Rows.Add(detailRow);

                for (int i = 0; i < colCount; i++)
                {
                    colWidth = poListaColumns.Where(x => x.index == i).Select(x => x.Width).FirstOrDefault();
                    string psValor = item[i].ToString();
                    XRTableCell detailCell = new XRTableCell();
                    detailRow.Cells.Add(detailCell);
                    detailCell.WidthF = colWidth;
                    detailCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    detailCell.Borders = DevExpress.XtraPrinting.BorderSide.All;

                    if (item[i].ToString().Contains("."))
                    {
                        try
                        {
                            detailCell.Text = string.Format("{0:c2}", Convert.ToDecimal(psValor));
                        }
                        catch (Exception)
                        {

                            detailCell.Text = psValor;
                        }
                       
                    }
                    else
                    {
                        detailCell.Text = psValor;
                    }

                }
            }

            //headerTable.EndInit();
            detailTable.EndInit();

            //headerTable.LocationF = new PointF(0, this.Bands[BandKind.ReportHeader].HeightF);
            //headerTable.WidthF = pageWidth;
            //headerTable.Font = new Font(headerTable.Font, FontStyle.Bold);

            detailTable.LocationF = new PointF(0, y);
            detailTable.WidthF = pageWidth;
            //detailTable.BackColor = backColor;
            //Applying styles if necessary
            if (useStyles)
            {
                detailTable.EvenStyleName = "EvenStyle";
                detailTable.OddStyleName = "OddStyle";
            }
        }

        private void lCrearTabla2(DataTable dt, bool useStyles = false, float y = 0)
        {

            DetailBand detail = Detail;
            //this.Bands.Add(detail);
            detail.HeightF = 0;

            // Creating Header and Detail Tables
            XRTable headerTable = new XRTable();
            detail.Controls.Add(headerTable);
            //this.Bands[BandKind.ReportHeader].Controls.Add(headerTable);
            //this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            //headerTable});

            XRTable detailTable = new XRTable();
            detail.Controls.Add(detailTable);

            detailTable.BeginInit();
            headerTable.BeginInit();

            XRTableRow headerRow = new XRTableRow();
            headerTable.Rows.Add(headerRow);


            int colCount = dt.Columns.Count;

            float pageWidth = (this.PageWidth - (this.Margins.Left + this.Margins.Right));
            float colWidth = pageWidth / colCount;
            var poListaColumns = lColumnasWidth(dt, Convert.ToInt32(pageWidth));
            //Creating an XRTableCell for each column in the corresponding DataTable
            for (int i = 0; i < colCount; i++)
            {
                colWidth = poListaColumns.Where(x => x.index == i).Select(x => x.Width).FirstOrDefault();
                XRTableCell headerCell = new XRTableCell();
                headerRow.Cells.Add(headerCell);
                headerCell.WidthF = colWidth;
                headerCell.Text = dt.Columns[i].Caption;
                headerCell.Borders = DevExpress.XtraPrinting.BorderSide.All;

            }

            foreach (DataRow item in dt.Rows)
            {
                XRTableRow detailRow = new XRTableRow();
                detailTable.Rows.Add(detailRow);

                for (int i = 0; i < colCount; i++)
                {
                    string psValor = item[i].ToString();
                    XRTableCell detailCell = new XRTableCell();
                    detailRow.Cells.Add(detailCell);
                    detailCell.WidthF = colWidth;

                    detailCell.Borders = DevExpress.XtraPrinting.BorderSide.All;

                    if (item[i].ToString().Contains("."))
                    {
                        detailCell.Text = string.Format("{0:c2}", Convert.ToDecimal(psValor));
                    }
                    else
                    {
                        detailCell.Text = psValor;
                    }

                }
            }

            headerTable.EndInit();
            detailTable.EndInit();

            headerTable.LocationF = new PointF(0, this.Bands[BandKind.ReportHeader].HeightF);
            headerTable.WidthF = pageWidth;
            headerTable.Font = new Font(headerTable.Font, FontStyle.Bold);

            detailTable.LocationF = new PointF(0, y);
            detailTable.WidthF = pageWidth;
            //detailTable.BackColor = backColor;
            //Applying styles if necessary
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
                int piValor = int.Parse(Math.Round(pdcValor, 0).ToString());
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
