﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace REH_Presentacion.Reportes
{
    public partial class xrptRolMensual : DevExpress.XtraReports.UI.XtraReport
    {
        public xrptRolMensual()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptRolComisiones reportSource = subreport.ReportSource as xrptRolComisiones;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }
    }
}
