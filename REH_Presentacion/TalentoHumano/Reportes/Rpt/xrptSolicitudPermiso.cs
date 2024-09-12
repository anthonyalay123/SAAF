using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using DevExpress.XtraEditors;
using System.Collections.Generic;
using GEN_Entidad;
using System.Linq;

namespace REH_Presentacion.TalentoHumano.Reportes.Rpt
{
    /// <summary>
    /// Clase de Reporteador Dinámico
    /// 26/10/2021 VAR
    /// </summary>
    public partial class xrptSolicitudPermiso : DevExpress.XtraReports.UI.XtraReport
    {

    
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public xrptSolicitudPermiso()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento que se dispara para dibujar el reporte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrptGeneral_BeforePrint(object sender, CancelEventArgs e)
        {

        }

    }
}
