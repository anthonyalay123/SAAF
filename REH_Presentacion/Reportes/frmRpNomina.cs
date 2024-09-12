using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using REH_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Reportes
{
    public partial class frmRpNomina : Form
    {
        public frmRpNomina()
        {
            InitializeComponent();

            //crystalReportViewer1 cw = new crystalReportViewer1();
            crystalReportViewer1.Dock = DockStyle.Fill;



            ReportDocument cryRpt = new ReportDocument();
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            Tables CrTables;





            ParameterField paramField = new ParameterField();
            ParameterFields paramFields = new ParameterFields();
            ParameterDiscreteValue paramDiscreteValue = new ParameterDiscreteValue();

            paramField.Name = "@ID";
            paramDiscreteValue.Value = 18;
            paramField.CurrentValues.Add(paramDiscreteValue);
            paramFields.Add(paramField);

            crystalReportViewer1.ParameterFieldInfo = paramFields;



            string path = @"C:\Users\vaptidur\source\repos\SAAF\REH_Presentacion\Reportes\rptNomina2.rpt";



            cryRpt.Load(path);


            ParameterFieldDefinitions crParameterFieldDefinitions;
            ParameterFieldDefinition crParameterFieldDefinition;
            ParameterValues crParameterValues = new ParameterValues();
            ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

            crParameterDiscreteValue.Value = Convert.ToInt32(1);
            crParameterFieldDefinitions = cryRpt.DataDefinition.ParameterFields;
            crParameterFieldDefinition = crParameterFieldDefinitions["@ID"];
            crParameterValues = crParameterFieldDefinition.CurrentValues;

            crParameterValues.Clear();
            crParameterValues.Add(crParameterDiscreteValue);
            crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);



            crConnectionInfo.ServerName = "VAPTIDUR";
            crConnectionInfo.DatabaseName = "SAAF";
            crConnectionInfo.UserID = "sa";
            crConnectionInfo.Password = "12345";

            CrTables = cryRpt.Database.Tables;


            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;

                CrTable.ApplyLogOnInfo(crtableLogoninfo);


            }
            cryRpt.SetParameterValue("@ID", 18);

            cryRpt.VerifyDatabase();

            cryRpt.Refresh();

            crystalReportViewer1.ReportSource = cryRpt;


            crystalReportViewer1.Refresh();


            /*
             ReportDocument rptDoc = new ReportDocument();
             string rptPath = @"C:\Users\vaptidur\source\repos\SAAF\REH_Presentacion\Reportes\rptNomina2.rpt";
             rptDoc.Load(rptPath);
             rptDoc.SetParameterValue("@ID", 18);
             crystalReportViewer1.ReportSource = rptDoc;
             */


            /*
            rptNomina21.SetParameterValue("@ID", 18);
            //rptNomina21.VerifyDatabase();
            rptNomina21.Refresh();
            */
            /*
            //create object of crystal report.
            var ds = bl.RptNomina(17, "");
            rptNomina objRpt = new rptNomina();
            objRpt.SetDataSource(ds);
            ParameterFields pfield = new ParameterFields();
            ParameterField ptitle = new ParameterField();
            ParameterDiscreteValue pvalue = new ParameterDiscreteValue();
            ptitle.ParameterFieldName = "date";
            pvalue.Value = txtcolor.Text;
            ptitle.CurrentValues.Add(pvalue);
            pfield.Add(ptitle);
            crystalReportViewer1.ParameterFieldInfo = pfield;
            crystalReportViewer1.ReportSource = objRpt;
            crystalReportViewer1.Refresh();
            //dsn.sour = bl.RptNomina(17, "");
            //dsNomina.Equals( de);
            */
        }
    }
}
