﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace REH_Presentacion.TalentoHumano.Reportes.Rpt
{
    public partial class xrptLiquidacionResumenDetalle : DevExpress.XtraReports.UI.XtraReport
    {
        public xrptLiquidacionResumenDetalle()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptLiquidacionDetRol reportSource = subreport.ReportSource as xrptLiquidacionDetRol;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport2_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptLiquidacionDetBSDT reportSource = subreport.ReportSource as xrptLiquidacionDetBSDT;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport3_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptLiquidacionDetBSDC reportSource = subreport.ReportSource as xrptLiquidacionDetBSDC;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport4_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptLiquidacionDetRes reportSource = subreport.ReportSource as xrptLiquidacionDetRes;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport5_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptLiquidacionDetVac reportSource = subreport.ReportSource as xrptLiquidacionDetVac;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }
    }
}
