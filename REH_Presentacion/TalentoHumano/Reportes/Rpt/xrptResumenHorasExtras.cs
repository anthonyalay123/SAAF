using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace REH_Presentacion.TalentoHumano.Reportes.Rpt
{
    public partial class xrptResumenHorasExtras : DevExpress.XtraReports.UI.XtraReport
    {
        public xrptResumenHorasExtras(bool tbAutoriza = false)
        {
            InitializeComponent();
            xrlAutoriza.Visible = tbAutoriza;
        }

    }
}
