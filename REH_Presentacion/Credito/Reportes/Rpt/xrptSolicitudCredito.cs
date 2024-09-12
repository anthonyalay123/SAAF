using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace REH_Presentacion.Credito.Reportes.Rpt
{
    public partial class xrptSolicitudCredito : DevExpress.XtraReports.UI.XtraReport
    {
        public xrptSolicitudCredito()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptSolicitudCreditoPropiedades reportSource = subreport.ReportSource as xrptSolicitudCreditoPropiedades;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;

        }

        private void xrSubreport2_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptSolicitudCreditoOtrosActivos reportSource = subreport.ReportSource as xrptSolicitudCreditoOtrosActivos;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport3_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptSolicitudCreditoRefBancarias reportSource = subreport.ReportSource as xrptSolicitudCreditoRefBancarias;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport4_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptSolicitudCreditoRefComerciales reportSource = subreport.ReportSource as xrptSolicitudCreditoRefComerciales;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport5_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptSolicitudCreditoRefPersonales reportSource = subreport.ReportSource as xrptSolicitudCreditoRefPersonales;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }
    }
}
