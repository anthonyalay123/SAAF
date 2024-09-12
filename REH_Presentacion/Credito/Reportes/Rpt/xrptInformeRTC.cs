using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace REH_Presentacion.Credito.Reportes.Rpt
{
    public partial class xrptInformeRTC : DevExpress.XtraReports.UI.XtraReport
    {
        public xrptInformeRTC()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRTCUbiCultivos reportSource = subreport.ReportSource as xrptInformeRTCUbiCultivos;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport2_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRTCPrincipalesClientes reportSource = subreport.ReportSource as xrptInformeRTCPrincipalesClientes;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport3_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRTCPrincipalesProveedores reportSource = subreport.ReportSource as xrptInformeRTCPrincipalesProveedores;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport4_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRTCProductosComercializar reportSource = subreport.ReportSource as xrptInformeRTCProductosComercializar;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport5_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRTCCroquis reportSource = subreport.ReportSource as xrptInformeRTCCroquis;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }
    }
}
