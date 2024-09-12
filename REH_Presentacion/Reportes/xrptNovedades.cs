﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace REH_Presentacion.Reportes
{
    public partial class xrptNovedades : DevExpress.XtraReports.UI.XtraReport
    {
        public xrptNovedades()
        {
            InitializeComponent();
        }

        private void xrptNovedades_BeforePrint(object sender, CancelEventArgs e)
        {
            var psValor = this.Parameters["mostrarTotal"].Value.ToString();
            if (psValor == "1")
            {
                xrLabel6.Visible = true;
                xrLabel7.Visible = true;
            }
            else
            {
                xrLabel6.Visible = false;
                xrLabel7.Visible = false;
            }
        }
    }
}
