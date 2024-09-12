using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace REH_Presentacion.Ventas.Reportes.Rpt
{
    public partial class xrptKilosLitrosFamilia : DevExpress.XtraReports.UI.XtraReport
    {

        double TotalVentaNeta = 0;
        double TotalVentaNetaAnt = 0;
        double TotalPResupuesto = 0;

        double TotalVentaNetaG2 = 0;
        double TotalVentaNetaAntG2 = 0;
        double TotalPResupuestoG2 = 0;

        public xrptKilosLitrosFamilia()
        {
            InitializeComponent();
        }

        #region SubGrupo 1
        private void xrLabel20_SummaryReset(object sender, EventArgs e)
        {
            TotalVentaNeta = 0;
        }

        private void xrLabel20_SummaryRowChanged(object sender, EventArgs e)
        {
            TotalVentaNeta += Convert.ToDouble(GetCurrentColumnValue("#Ventas Lts-Kgs"));
        }

        private void xrLabel22_SummaryReset(object sender, EventArgs e)
        {
            TotalPResupuesto = 0;
        }

        private void xrLabel22_SummaryRowChanged(object sender, EventArgs e)
        {
            TotalPResupuesto += Convert.ToDouble(GetCurrentColumnValue("#Presupuesto"));
        }

        private void xrLabel24_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = TotalPResupuesto == 0 ? 0 : Math.Round(TotalVentaNeta / TotalPResupuesto, 4);
            e.Handled = true;
        }
        #endregion

        #region SubGrupo 2

        #endregion

        private void xrLabel38_SummaryReset(object sender, EventArgs e)
        {
            TotalVentaNetaG2 = 0;
        }

        private void xrLabel38_SummaryRowChanged(object sender, EventArgs e)
        {
            TotalVentaNetaG2 += Convert.ToDouble(GetCurrentColumnValue("#Ventas Lts-Kgs"));
        }

        private void xrLabel39_SummaryReset(object sender, EventArgs e)
        {
            TotalPResupuestoG2 = 0;
        }

        private void xrLabel39_SummaryRowChanged(object sender, EventArgs e)
        {
            TotalPResupuestoG2 += Convert.ToDouble(GetCurrentColumnValue("#Presupuesto"));
        }

        private void xrLabel40_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = TotalPResupuestoG2 == 0 ? 0 : Math.Round(TotalVentaNetaG2 / TotalPResupuestoG2, 4);
            e.Handled = true;
        }
    }
}
