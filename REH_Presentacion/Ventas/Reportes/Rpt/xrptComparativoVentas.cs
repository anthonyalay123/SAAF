using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace REH_Presentacion.Ventas.Reportes.Rpt
{
    public partial class xrptComparativoVentas : DevExpress.XtraReports.UI.XtraReport
    {
        
        double lTotal1 = 0;
        double lTotal2 = 0;
        double lTotal3 = 0;
        double lPresupuestoCurso = 0;
        double lPresupuestoProyec = 0;
        List<string> psTipoProducto = new List<string>();
        

        public xrptComparativoVentas()
        {
            InitializeComponent();
        }


        #region PorcCre1
        private void xrLabel54_SummaryReset(object sender, EventArgs e)
        {
            lTotal1 = 0;
        }

        private void xrLabel54_SummaryRowChanged(object sender, EventArgs e)
        {
            lTotal1 += Convert.ToDouble(GetCurrentColumnValue("Total1"));
        }

        private void xrLabel28_SummaryReset(object sender, EventArgs e)
        {
            lTotal2 = 0;
        }

        private void xrLabel28_SummaryRowChanged(object sender, EventArgs e)
        {
            lTotal2 += Convert.ToDouble(GetCurrentColumnValue("Total2"));
        }

        private void xrLabel30_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            //e.Result = lTotal2 == 0 ? 0 : Math.Round(1 - (lTotal1 / lTotal2), 4);
            e.Result = lTotal1 == 0 ? 0 : Math.Round(((lTotal2- lTotal1) / lTotal1), 4);
            e.Handled = true;
        }
        #endregion

        #region PorcCre2
        private void xrLabel29_SummaryReset(object sender, EventArgs e)
        {
            lTotal3 = 0;
        }

        private void xrLabel29_SummaryRowChanged(object sender, EventArgs e)
        {
            lTotal3 += Convert.ToDouble(GetCurrentColumnValue("Total3"));
        }

        private void xrLabel33_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            //e.Result = lTotal3 == 0 ? 0 : Math.Round(1 - (lTotal2 / lTotal3), 4);
            e.Result = lTotal2 == 0 ? 0 : Math.Round(((lTotal3- lTotal2) / lTotal2), 4);
            e.Handled = true;
        }

        #endregion

        #region PorcCumpCurso
        private void xrLabel19_SummaryReset(object sender, EventArgs e)
        {
            lPresupuestoCurso = 0;
        }

        private void xrLabel19_SummaryRowChanged(object sender, EventArgs e)
        {
            lPresupuestoCurso += Convert.ToDouble(GetCurrentColumnValue("PresupuestoCurso"));
        }

        private void xrLabel20_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = lPresupuestoCurso == 0 ? 0 : Math.Round(lTotal3 / lPresupuestoCurso, 4);
            e.Handled = true;
        }
        #endregion

        #region PorcCumpAnual
        private void xrLabel22_SummaryReset(object sender, EventArgs e)
        {
            lPresupuestoProyec = 0;
            psTipoProducto = new List<string>();
        }

        private void xrLabel22_SummaryRowChanged(object sender, EventArgs e)
        {
            lPresupuestoProyec += Convert.ToDouble(GetCurrentColumnValue("PresupuestoProyec"));
            psTipoProducto.Add(GetCurrentColumnValue("TipoProducto").ToString());
        }

        private void xrLabel24_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = (lPresupuestoCurso + lPresupuestoProyec) == 0 ? 0 : Math.Round(lTotal3 / (lPresupuestoCurso + lPresupuestoProyec), 4);
            e.Handled = true;
        }

        #endregion

        private void xrLabel23_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            DataTable dtResult = new DataTable();
            var dt = ((System.Data.DataSet)this.DataSource).Tables[0];
            List<DataRow> poResultado = dt.AsEnumerable().Where(x => psTipoProducto.Contains(x.Field<string>("TipoProducto"))).ToList();
            if (poResultado.Count > 0)
                dtResult = poResultado.CopyToDataTable();
            else
            {
                dtResult = dt;
            }

            decimal pdcVentas = 0M, pdcRentabilidad = 0M;
            foreach (DataRow item in dtResult.Rows)
            {
                pdcVentas += Convert.ToDecimal(string.IsNullOrEmpty(item["VentasMesAct"].ToString()) ? 0 : item["VentasMesAct"]);
                pdcRentabilidad += Convert.ToDecimal(string.IsNullOrEmpty(item["RentabilidadMesAct"].ToString()) ? 0 : item["RentabilidadMesAct"]);
            }

            e.Result = pdcVentas == 0 ? 0 : Math.Round(pdcRentabilidad / pdcVentas, 4);
            e.Handled = true;
        }

        private void xrLabel37_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            DataTable dtResult = new DataTable();
            var dt = ((System.Data.DataSet)this.DataSource).Tables[0];
            List<DataRow> poResultado = dt.AsEnumerable().Where(x => psTipoProducto.Contains(x.Field<string>("TipoProducto"))).ToList();
            if (poResultado.Count > 0)
                dtResult = poResultado.CopyToDataTable();
            else
            {
                dtResult = dt;
            }

            decimal pdcVentas = 0M, pdcRentabilidad = 0M;
            foreach (DataRow item in dtResult.Rows)
            {
                pdcVentas += Convert.ToDecimal(string.IsNullOrEmpty(item["VentasMesAnt"].ToString()) ? 0 : item["VentasMesAnt"]);
                pdcRentabilidad += Convert.ToDecimal(string.IsNullOrEmpty(item["RentabilidadMesAnt"].ToString()) ? 0 : item["RentabilidadMesAnt"]);
            }

            e.Result = pdcVentas == 0 ? 0 : Math.Round(pdcRentabilidad / pdcVentas, 4);
            e.Handled = true;
        }

        private void xrLabel25_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            DataTable dtResult = new DataTable();
            var dt = ((System.Data.DataSet)this.DataSource).Tables[0];
            List<DataRow> poResultado = dt.AsEnumerable().Where(x => psTipoProducto.Contains(x.Field<string>("TipoProducto"))).ToList();
            if (poResultado.Count > 0)
                dtResult = poResultado.CopyToDataTable();
            else
            {
                dtResult = dt;
            }

            decimal pdcVentas = 0M, pdcRentabilidad = 0M;
            foreach (DataRow item in dtResult.Rows)
            {
                pdcVentas += Convert.ToDecimal(string.IsNullOrEmpty(item["Total3"].ToString()) ? 0 : item["Total3"]);
                pdcRentabilidad += Convert.ToDecimal(string.IsNullOrEmpty(item["RentabilidadAcum"].ToString()) ? 0 : item["RentabilidadAcum"]);
            }

            e.Result = pdcVentas == 0 ? 0 : Math.Round(pdcRentabilidad / pdcVentas, 4);
            e.Handled = true;
        }

        private void xrLabel18_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            
            var dt = ((System.Data.DataSet)this.DataSource).Tables[0];
            
            decimal pdcVentas = 0M, pdcRentabilidad = 0M;
            foreach (DataRow item in dt.Rows)
            {
                pdcVentas += Convert.ToDecimal(string.IsNullOrEmpty(item["VentasMesAct"].ToString()) ? 0 : item["VentasMesAct"]);
                pdcRentabilidad += Convert.ToDecimal(string.IsNullOrEmpty(item["RentabilidadMesAct"].ToString()) ? 0 : item["RentabilidadMesAct"]);
            }

            e.Result = pdcVentas == 0 ? 0 : Math.Round(pdcRentabilidad / pdcVentas, 4);
            e.Handled = true;
        }
        
        private void xrLabel27_SummaryGetResult_1(object sender, SummaryGetResultEventArgs e)
        {
            var dt = ((System.Data.DataSet)this.DataSource).Tables[0];

            decimal pdcVentas = 0M, pdcRentabilidad = 0M;
            foreach (DataRow item in dt.Rows)
            {
                pdcVentas += Convert.ToDecimal(string.IsNullOrEmpty(item["VentasMesAnt"].ToString()) ? 0 : item["VentasMesAnt"]);
                pdcRentabilidad += Convert.ToDecimal(string.IsNullOrEmpty(item["RentabilidadMesAnt"].ToString()) ? 0 : item["RentabilidadMesAnt"]);
            }

            e.Result = pdcVentas == 0 ? 0 : Math.Round(pdcRentabilidad / pdcVentas, 4);
            e.Handled = true;
        }

        private void xrLabel38_SummaryGetResult_1(object sender, SummaryGetResultEventArgs e)
        {
            var dt = ((System.Data.DataSet)this.DataSource).Tables[0];

            decimal pdcVentas = 0M, pdcRentabilidad = 0M;
            foreach (DataRow item in dt.Rows)
            {
                pdcVentas += Convert.ToDecimal(string.IsNullOrEmpty(item["Total3"].ToString()) ? 0 : item["Total3"]);
                pdcRentabilidad += Convert.ToDecimal(string.IsNullOrEmpty(item["RentabilidadAcum"].ToString()) ? 0 : item["RentabilidadAcum"]);
            }

            e.Result = pdcVentas == 0 ? 0 : Math.Round(pdcRentabilidad / pdcVentas, 4);
            e.Handled = true;
        }
    }
}
