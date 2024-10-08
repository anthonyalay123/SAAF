﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.DataAccess.ObjectBinding;

namespace REH_Presentacion.Credito.Reportes.Rpt
{
    public partial class xrptEstadoSituacionCliente : DevExpress.XtraReports.UI.XtraReport
    {
        public xrptEstadoSituacionCliente()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptEstadoSituacionClienteFacChePos reportSource = subreport.ReportSource as xrptEstadoSituacionClienteFacChePos;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;

            //XRSubreport subreport3 = sender as XRSubreport;
            //xrptEstadoSituacionClienteRecComPag reportSource3 = subreport1.ReportSource as xrptEstadoSituacionClienteRecComPag;
            //reportSource3.DataSource = (System.Data.DataSet)this.DataSource;

            //XRSubreport subreport4 = sender as XRSubreport;
            //xrptEstadoSituacionClienteCheklist reportSource4 = subreport1.ReportSource as xrptEstadoSituacionClienteCheklist;
            //reportSource4.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport2_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptEstadoSituacionClienteFacSinChe reportSource = subreport.ReportSource as xrptEstadoSituacionClienteFacSinChe;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport3_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptEstadoSituacionClienteRecComPag reportSource = subreport.ReportSource as xrptEstadoSituacionClienteRecComPag;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }
    }
}
