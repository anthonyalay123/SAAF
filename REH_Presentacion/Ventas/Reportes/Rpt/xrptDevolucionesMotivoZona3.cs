﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace REH_Presentacion.Ventas.Reportes.Rpt
{
    public partial class xrptDevolucionesMotivoZona3 : DevExpress.XtraReports.UI.XtraReport
    {
        public int liMesMaximo = 0;
        public int liMesMinimo = 0;

        public xrptDevolucionesMotivoZona3()
        {
            InitializeComponent();
        }

        private void xrptDevolucionesMotivo_BeforePrint(object sender, CancelEventArgs e)
        {
            int Dif = 12 - liMesMaximo;
            int dist = 58;
            int cont = 0;
            int contIzq = 0;


            if (liMesMinimo > 1)
            {
                lblCabEne.Visible = false;
                colEne.Visible = false;
                ColSumEne.Visible = false;
                ColSum2Ene.Visible = false;
                colSumRepEne.Visible = false;
                contIzq++;
            }
            if (liMesMinimo > 2)
            {
                lblCabFeb.Visible = false;
                colFeb.Visible = false;
                ColSumFeb.Visible = false;
                ColSum2Feb.Visible = false;
                colSumRepFeb.Visible = false;
                contIzq++;
            }
            if (liMesMinimo > 3)
            {
                lblCabMar.Visible = false;
                colMar.Visible = false;
                ColSumMar.Visible = false;
                ColSum2Mar.Visible = false;
                colSumRepMar.Visible = false;
                contIzq++;
            }
            if (liMesMinimo > 4)
            {
                lblCabAbr.Visible = false;
                colAbr.Visible = false;
                ColSumAbr.Visible = false;
                ColSum2Abr.Visible = false;
                colSumRepAbr.Visible = false;
                contIzq++;
            }
            if (liMesMinimo > 5)
            {
                lblCabMay.Visible = false;
                colMay.Visible = false;
                ColSumMay.Visible = false;
                ColSum2May.Visible = false;
                colSumRepMay.Visible = false;
                contIzq++;
            }
            if (liMesMinimo > 6)
            {
                lblCabJun.Visible = false;
                colJun.Visible = false;
                ColSumJun.Visible = false;
                ColSum2Jun.Visible = false;
                colSumRepJun.Visible = false;
                contIzq++;
            }
            if (liMesMinimo > 7)
            {
                lblCabJul.Visible = false;
                colJul.Visible = false;
                ColSumJul.Visible = false;
                ColSum2Jul.Visible = false;
                colSumRepJul.Visible = false;
                contIzq++;
            }
            if (liMesMinimo > 8)
            {
                lblCabAgo.Visible = false;
                colAgo.Visible = false;
                ColSumAgo.Visible = false;
                ColSum2Ago.Visible = false;
                colSumRepAgo.Visible = false;
                contIzq++;
            }
            if (liMesMinimo > 9)
            {
                lblCabSep.Visible = false;
                colSep.Visible = false;
                ColSumSep.Visible = false;
                ColSum2Sep.Visible = false;
                colSumRepSep.Visible = false;
                contIzq++;
            }
            if (liMesMinimo > 10)
            {
                lblCabOct.Visible = false;
                colOct.Visible = false;
                ColSumOct.Visible = false;
                ColSum2Oct.Visible = false;
                colSumRepOct.Visible = false;
                contIzq++;
            }
            if (liMesMinimo > 11)
            {
                lblCabNov.Visible = false;
                colNov.Visible = false;
                ColSumNov.Visible = false;
                ColSum2Nov.Visible = false;
                colSumRepNov.Visible = false;
                contIzq++;
            }

            /************************************************************************************************************************/
            /************************************************************************************************************************/

            lblCabFeb.LocationFloat = new DevExpress.Utils.PointFloat(lblCabFeb.LocationF.X - (dist * contIzq), lblCabFeb.LocationF.Y);
            colFeb.LocationFloat = new DevExpress.Utils.PointFloat(colFeb.LocationF.X - (dist * contIzq), colFeb.LocationF.Y);
            ColSumFeb.LocationFloat = new DevExpress.Utils.PointFloat(ColSumFeb.LocationF.X - (dist * contIzq), ColSumFeb.LocationF.Y);
            ColSum2Feb.LocationFloat = new DevExpress.Utils.PointFloat(ColSum2Feb.LocationF.X - (dist * contIzq), ColSum2Feb.LocationF.Y);
            colSumRepFeb.LocationFloat = new DevExpress.Utils.PointFloat(colSumRepFeb.LocationF.X - (dist * contIzq), colSumRepFeb.LocationF.Y);

            lblCabMar.LocationFloat = new DevExpress.Utils.PointFloat(lblCabMar.LocationF.X - (dist * contIzq), lblCabMar.LocationF.Y);
            colMar.LocationFloat = new DevExpress.Utils.PointFloat(colMar.LocationF.X - (dist * contIzq), colMar.LocationF.Y);
            ColSumMar.LocationFloat = new DevExpress.Utils.PointFloat(ColSumMar.LocationF.X - (dist * contIzq), ColSumMar.LocationF.Y);
            ColSum2Mar.LocationFloat = new DevExpress.Utils.PointFloat(ColSum2Mar.LocationF.X - (dist * contIzq), ColSum2Mar.LocationF.Y);
            colSumRepMar.LocationFloat = new DevExpress.Utils.PointFloat(colSumRepMar.LocationF.X - (dist * contIzq), colSumRepMar.LocationF.Y);

            lblCabAbr.LocationFloat = new DevExpress.Utils.PointFloat(lblCabAbr.LocationF.X - (dist * contIzq), lblCabAbr.LocationF.Y);
            colAbr.LocationFloat = new DevExpress.Utils.PointFloat(colAbr.LocationF.X - (dist * contIzq), colAbr.LocationF.Y);
            ColSumAbr.LocationFloat = new DevExpress.Utils.PointFloat(ColSumAbr.LocationF.X - (dist * contIzq), ColSumAbr.LocationF.Y);
            ColSum2Abr.LocationFloat = new DevExpress.Utils.PointFloat(ColSum2Abr.LocationF.X - (dist * contIzq), ColSum2Abr.LocationF.Y);
            colSumRepAbr.LocationFloat = new DevExpress.Utils.PointFloat(colSumRepAbr.LocationF.X - (dist * contIzq), colSumRepAbr.LocationF.Y);

            lblCabMay.LocationFloat = new DevExpress.Utils.PointFloat(lblCabMay.LocationF.X - (dist * contIzq), lblCabMay.LocationF.Y);
            colMay.LocationFloat = new DevExpress.Utils.PointFloat(colMay.LocationF.X - (dist * contIzq), colMay.LocationF.Y);
            ColSumMay.LocationFloat = new DevExpress.Utils.PointFloat(ColSumMay.LocationF.X - (dist * contIzq), ColSumMay.LocationF.Y);
            ColSum2May.LocationFloat = new DevExpress.Utils.PointFloat(ColSum2May.LocationF.X - (dist * contIzq), ColSum2May.LocationF.Y);
            colSumRepMay.LocationFloat = new DevExpress.Utils.PointFloat(colSumRepMay.LocationF.X - (dist * contIzq), colSumRepMay.LocationF.Y);

            lblCabJun.LocationFloat = new DevExpress.Utils.PointFloat(lblCabJun.LocationF.X - (dist * contIzq), lblCabJun.LocationF.Y);
            colJun.LocationFloat = new DevExpress.Utils.PointFloat(colJun.LocationF.X - (dist * contIzq), colJun.LocationF.Y);
            ColSumJun.LocationFloat = new DevExpress.Utils.PointFloat(ColSumJun.LocationF.X - (dist * contIzq), ColSumJun.LocationF.Y);
            ColSum2Jun.LocationFloat = new DevExpress.Utils.PointFloat(ColSum2Jun.LocationF.X - (dist * contIzq), ColSum2Jun.LocationF.Y);
            colSumRepJun.LocationFloat = new DevExpress.Utils.PointFloat(colSumRepJun.LocationF.X - (dist * contIzq), colSumRepJun.LocationF.Y);

            lblCabJul.LocationFloat = new DevExpress.Utils.PointFloat(lblCabJul.LocationF.X - (dist * contIzq), lblCabJul.LocationF.Y);
            colJul.LocationFloat = new DevExpress.Utils.PointFloat(colJul.LocationF.X - (dist * contIzq), colJul.LocationF.Y);
            ColSumJul.LocationFloat = new DevExpress.Utils.PointFloat(ColSumJul.LocationF.X - (dist * contIzq), ColSumJul.LocationF.Y);
            ColSum2Jul.LocationFloat = new DevExpress.Utils.PointFloat(ColSum2Jul.LocationF.X - (dist * contIzq), ColSum2Jul.LocationF.Y);
            colSumRepJul.LocationFloat = new DevExpress.Utils.PointFloat(colSumRepJul.LocationF.X - (dist * contIzq), colSumRepJul.LocationF.Y);

            lblCabAgo.LocationFloat = new DevExpress.Utils.PointFloat(lblCabAgo.LocationF.X - (dist * contIzq), lblCabAgo.LocationF.Y);
            colAgo.LocationFloat = new DevExpress.Utils.PointFloat(colAgo.LocationF.X - (dist * contIzq), colAgo.LocationF.Y);
            ColSumAgo.LocationFloat = new DevExpress.Utils.PointFloat(ColSumAgo.LocationF.X - (dist * contIzq), ColSumAgo.LocationF.Y);
            ColSum2Ago.LocationFloat = new DevExpress.Utils.PointFloat(ColSum2Ago.LocationF.X - (dist * contIzq), ColSum2Ago.LocationF.Y);
            colSumRepAgo.LocationFloat = new DevExpress.Utils.PointFloat(colSumRepAgo.LocationF.X - (dist * contIzq), colSumRepAgo.LocationF.Y);

            lblCabSep.LocationFloat = new DevExpress.Utils.PointFloat(lblCabSep.LocationF.X - (dist * contIzq), lblCabSep.LocationF.Y);
            colSep.LocationFloat = new DevExpress.Utils.PointFloat(colSep.LocationF.X - (dist * contIzq), colSep.LocationF.Y);
            ColSumSep.LocationFloat = new DevExpress.Utils.PointFloat(ColSumSep.LocationF.X - (dist * contIzq), ColSumSep.LocationF.Y);
            ColSum2Sep.LocationFloat = new DevExpress.Utils.PointFloat(ColSum2Sep.LocationF.X - (dist * contIzq), ColSum2Sep.LocationF.Y);
            colSumRepSep.LocationFloat = new DevExpress.Utils.PointFloat(colSumRepSep.LocationF.X - (dist * contIzq), colSumRepSep.LocationF.Y);

            lblCabOct.LocationFloat = new DevExpress.Utils.PointFloat(lblCabOct.LocationF.X - (dist * contIzq), lblCabOct.LocationF.Y);
            colOct.LocationFloat = new DevExpress.Utils.PointFloat(colOct.LocationF.X - (dist * contIzq), colOct.LocationF.Y);
            ColSumOct.LocationFloat = new DevExpress.Utils.PointFloat(ColSumOct.LocationF.X - (dist * contIzq), ColSumOct.LocationF.Y);
            ColSum2Oct.LocationFloat = new DevExpress.Utils.PointFloat(ColSum2Oct.LocationF.X - (dist * contIzq), ColSum2Oct.LocationF.Y);
            colSumRepOct.LocationFloat = new DevExpress.Utils.PointFloat(colSumRepOct.LocationF.X - (dist * contIzq), colSumRepOct.LocationF.Y);

            lblCabNov.LocationFloat = new DevExpress.Utils.PointFloat(lblCabNov.LocationF.X - (dist * contIzq), lblCabNov.LocationF.Y);
            colNov.LocationFloat = new DevExpress.Utils.PointFloat(colNov.LocationF.X - (dist * contIzq), colNov.LocationF.Y);
            ColSumNov.LocationFloat = new DevExpress.Utils.PointFloat(ColSumNov.LocationF.X - (dist * contIzq), ColSumNov.LocationF.Y);
            ColSum2Nov.LocationFloat = new DevExpress.Utils.PointFloat(ColSum2Nov.LocationF.X - (dist * contIzq), ColSum2Nov.LocationF.Y);
            colSumRepNov.LocationFloat = new DevExpress.Utils.PointFloat(colSumRepNov.LocationF.X - (dist * contIzq), colSumRepNov.LocationF.Y);

            lblCabDic.LocationFloat = new DevExpress.Utils.PointFloat(lblCabDic.LocationF.X - (dist * contIzq), lblCabDic.LocationF.Y);
            colDic.LocationFloat = new DevExpress.Utils.PointFloat(colDic.LocationF.X - (dist * contIzq), colDic.LocationF.Y);
            ColSumDic.LocationFloat = new DevExpress.Utils.PointFloat(ColSumDic.LocationF.X - (dist * contIzq), ColSumDic.LocationF.Y);
            ColSum2Dic.LocationFloat = new DevExpress.Utils.PointFloat(ColSum2Dic.LocationF.X - (dist * contIzq), ColSum2Dic.LocationF.Y);
            colSumRepDic.LocationFloat = new DevExpress.Utils.PointFloat(colSumRepDic.LocationF.X - (dist * contIzq), colSumRepDic.LocationF.Y);

            /************************************************************************************************************************/
            /************************************************************************************************************************/

            if (liMesMaximo < 12)
            {
                lblCabDic.Visible = false;
                colDic.Visible = false;
                ColSumDic.Visible = false;
                ColSum2Dic.Visible = false;
                colSumRepDic.Visible = false;
                cont++;
            }
            if (liMesMaximo < 11)
            {
                lblCabNov.Visible = false;
                colNov.Visible = false;
                ColSumNov.Visible = false;
                ColSum2Nov.Visible = false;
                colSumRepNov.Visible = false;
                cont++;
            }
            if (liMesMaximo < 10)
            {
                lblCabOct.Visible = false;
                colOct.Visible = false;
                ColSumOct.Visible = false;
                ColSum2Oct.Visible = false;
                colSumRepOct.Visible = false;
                cont++;
            }
            if (liMesMaximo < 9)
            {
                lblCabSep.Visible = false;
                colSep.Visible = false;
                ColSumSep.Visible = false;
                ColSum2Sep.Visible = false;
                colSumRepSep.Visible = false;
                cont++;
            }
            if (liMesMaximo < 8)
            {
                lblCabAgo.Visible = false;
                colAgo.Visible = false;
                ColSumAgo.Visible = false;
                ColSum2Ago.Visible = false;
                colSumRepAgo.Visible = false;
                cont++;
            }
            if (liMesMaximo < 7)
            {
                lblCabJul.Visible = false;
                colJul.Visible = false;
                ColSumJul.Visible = false;
                ColSum2Jul.Visible = false;
                colSumRepJul.Visible = false;
                cont++;
            }
            if (liMesMaximo < 6)
            {
                lblCabJun.Visible = false;
                colJun.Visible = false;
                ColSumJun.Visible = false;
                ColSum2Jun.Visible = false;
                colSumRepJun.Visible = false;
                cont++;
            }
            if (liMesMaximo < 5)
            {
                lblCabMay.Visible = false;
                colMay.Visible = false;
                ColSumMay.Visible = false;
                ColSum2May.Visible = false;
                colSumRepMay.Visible = false;
                cont++;
            }
            if (liMesMaximo < 4)
            {
                lblCabAbr.Visible = false;
                colAbr.Visible = false;
                ColSumAbr.Visible = false;
                ColSum2Abr.Visible = false;
                colSumRepAbr.Visible = false;
                cont++;
            }
            if (liMesMaximo < 3)
            {
                lblCabMar.Visible = false;
                colMar.Visible = false;
                ColSumMar.Visible = false;
                ColSum2Mar.Visible = false;
                colSumRepMar.Visible = false;
                cont++;
            }
            if (liMesMaximo < 2)
            {
                lblCabFeb.Visible = false;
                colFeb.Visible = false;
                ColSumFeb.Visible = false;
                ColSum2Feb.Visible = false;
                colSumRepFeb.Visible = false;
                cont++;
            }

            
            lblCabTot.LocationFloat = new DevExpress.Utils.PointFloat(lblCabTot.LocationF.X - (dist * cont) - (dist * contIzq), lblCabTot.LocationF.Y);
            colTot.LocationFloat = new DevExpress.Utils.PointFloat(colTot.LocationF.X - (dist * cont) - (dist * contIzq), colTot.LocationF.Y);
            ColSumTot.LocationFloat = new DevExpress.Utils.PointFloat(ColSumTot.LocationF.X - (dist * cont) - (dist * contIzq), ColSumTot.LocationF.Y);
            ColSum2Tot.LocationFloat = new DevExpress.Utils.PointFloat(ColSum2Tot.LocationF.X - (dist * cont) - (dist * contIzq), ColSum2Tot.LocationF.Y);
            colSumRepTot.LocationFloat = new DevExpress.Utils.PointFloat(colSumRepTot.LocationF.X - (dist * cont) - (dist * contIzq), colSumRepTot.LocationF.Y);

            float anchoPagina = 1060;
            float anchoDetalle = lblCabTot.LocationF.X + dist;

            float mitadPagina = anchoPagina / 2;
            float mitadDetalle = anchoDetalle / 2;

            float InicioXPanel = mitadPagina - mitadDetalle;

            lblMotivoHijo.LocationF = new PointF(InicioXPanel+10, lblMotivoHijo.LocationF.Y);
            xrPanel1.LocationF = new PointF(InicioXPanel, xrPanel1.LocationF.Y);
            xrPanel2.LocationF = new PointF(InicioXPanel, xrPanel2.LocationF.Y);
            xrPanel3.LocationF = new PointF(InicioXPanel, xrPanel3.LocationF.Y);
            xrPanel4.LocationF = new PointF(InicioXPanel, xrPanel4.LocationF.Y);
            xrPanel5.LocationF = new PointF(InicioXPanel, xrPanel5.LocationF.Y);

            xrPanel1.SizeF = new SizeF(anchoDetalle, xrPanel1.SizeF.Height);
            xrPanel2.SizeF = new SizeF(anchoDetalle, xrPanel2.SizeF.Height);
            xrPanel3.SizeF = new SizeF(anchoDetalle, xrPanel3.SizeF.Height);
            xrPanel4.SizeF = new SizeF(anchoDetalle, xrPanel4.SizeF.Height);
            xrPanel5.SizeF = new SizeF(anchoDetalle, xrPanel5.SizeF.Height);


        }
    }
}
