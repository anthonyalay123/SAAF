﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace REH_Presentacion.Credito.Reportes.Rpt
{
    public partial class xrptInfomeRevision : DevExpress.XtraReports.UI.XtraReport
    {
        public xrptInfomeRevision()
        {
            InitializeComponent();
        }

        private void xrSubreport1_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRevisionSCDireccion reportSource = subreport.ReportSource as xrptInformeRevisionSCDireccion;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport2_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRevisionSCNombramiento reportSource = subreport.ReportSource as xrptInformeRevisionSCNombramiento;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport3_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRevisionSCCertificado reportSource = subreport.ReportSource as xrptInformeRevisionSCCertificado;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport4_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRevisionSCBancario reportSource = subreport.ReportSource as xrptInformeRevisionSCBancario;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport5_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRevisionSCComerciales reportSource = subreport.ReportSource as xrptInformeRevisionSCComerciales;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport6_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRevisionBuro reportSource = subreport.ReportSource as xrptInformeRevisionBuro;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport7_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRevisionJudicial reportSource = subreport.ReportSource as xrptInformeRevisionJudicial;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport8_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRevisionSri reportSource = subreport.ReportSource as xrptInformeRevisionSri;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport9_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRevisionAccionistas reportSource = subreport.ReportSource as xrptInformeRevisionAccionistas;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }

        private void xrSubreport10_BeforePrint(object sender, CancelEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            xrptInformeRevisionAdmActuales reportSource = subreport.ReportSource as xrptInformeRevisionAdmActuales;
            reportSource.DataSource = (System.Data.DataSet)this.DataSource;
        }
    }
}
