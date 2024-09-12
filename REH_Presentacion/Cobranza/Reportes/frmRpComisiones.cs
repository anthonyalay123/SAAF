using COB_Negocio;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.Xpo.DB;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using REH_Presentacion.Cobranza.Reportes.Rpt;



namespace REH_Presentacion.Cobranza.Reportes
{
    public partial class frmRpComisiones : frmBaseTrxDev
    {
        BindingSource bsDatos = new BindingSource();
        clsNComisiones loLogicaNegocio = new clsNComisiones();
        Nullable<int> liFixedColumn;
        public frmRpComisiones()
        {
            InitializeComponent();
            cmbTipo.EditValueChanged += cmbTipo_EditValueChanged;
        }

        private void frmRpComisiones_Load(object sender, EventArgs e)
        {
            clsComun.gLLenarCombo(ref cmbPeriodo, loLogicaNegocio.goConsultarComboCobranzaPeriodo());
            clsComun.gLLenarCombo(ref cmbTipo, loLogicaNegocio.goConsultarComboTipoCobranzaComisiones());
            lCargarEventosBotones();
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            //if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            lBuscar();
        }

        private void cmbTipo_EditValueChanged(object sender, EventArgs e)
        {
            lBuscar();
        }

        private void lBuscar()
        {

            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();

            DataTable dt = ldtValidaQuery();

            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;
            dgvDatos.PopulateColumns();

            clsComun.gFormatearColumnasGrid(dgvDatos);
            clsComun.gOrdenarColumnasGrid(dgvDatos);

            if (cmbTipo.EditValue.ToString() == "1")
            {
                dgvDatos.Columns["Zona"].GroupIndex = 0;
                dgvDatos.Columns["CodigoComision"].Caption = "Nomenclatura";
                dgvDatos.Columns["Empleado"].VisibleIndex = 1;
                dgvDatos.Columns["CodigoComision"].VisibleIndex = 2;
            }
            else if (cmbTipo.EditValue.ToString() == "3")
            {
                dgvDatos.Columns["Agrupacion"].GroupIndex = 0;
            }

            dgvDatos.ExpandAllGroups();

        }

        private DataTable ldtValidaQuery()
        {
            DataTable dt = new DataTable();
            
            if (cmbTipo.EditValue.ToString() == "1") {
                dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC COBSPRPTCOMISIONESZONAEMPLEADO '{0}'", cmbPeriodo.EditValue.ToString()));
            } 
            else if (cmbTipo.EditValue.ToString() == "2")
            {
                dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC COBSPRPTCOMISIONESEMPLEADO '{0}'", cmbPeriodo.EditValue.ToString()));
            } else
            {
                var ds = loLogicaNegocio.goConsultaDataSet(string.Format("EXEC COBSPRPTCOMISIONESZONA '{0}'", cmbPeriodo.EditValue.ToString()));
                
                if (ds.Tables.Count > 1)
                {
                    dt = ds.Tables[1];
                }
            }

            return dt;
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                int tIdPeriodo = int.Parse(cmbPeriodo.EditValue.ToString());

                if (tIdPeriodo > 0)
                {
                    var poLogicaNegocio = new clsNComisiones();
                    var poPeriodo = poLogicaNegocio.goConsultarPeriodo(tIdPeriodo);
                    //string psParametro = string.Format("{0} - {1}", clsComun.gsGetMes(poPeriodo.FechaInicio), poPeriodo.Anio);
                    string psParametro = string.Format("DEL {0} DE {1} AL {2} DE {3}", poPeriodo.FechaInicioComi.Value.Day, clsComun.gsGetMes(poPeriodo.FechaInicioComi.Value.Month), poPeriodo.FechaFinComi.Value.Day, clsComun.gsGetMes(poPeriodo.FechaFinComi.Value.Month));
                    xrptCobranzaZonaEmpleado xrptEmpleado = new xrptCobranzaZonaEmpleado();
                    xrptCobranzaEmpleado xrptEmple = new xrptCobranzaEmpleado();
                    xrptCobranzaZona xrpt = new xrptCobranzaZona();
                    if (cmbTipo.EditValue.ToString() == "1")
                    {
                        /*************************************************************************************************************/
                        System.Data.DataSet dsEmp = new System.Data.DataSet();

                        var dsResultEmp = poLogicaNegocio.gdsComisionesZonaEmpleado(tIdPeriodo);
                        dsResultEmp.Tables[0].TableName = "ComiZonaEmpleado";

                        dsEmp.Merge(dsResultEmp);
                        xrptEmpleado.DataSource = dsEmp;
                        xrptEmpleado.RequestParameters = false;

                        xrptEmpleado.Parameters["Titulo"].Value = psParametro;
                        xrptEmpleado.Parameters["Titulo"].Visible = false;

                        MostrarReporte(xrptEmpleado);
                    }
                    else if (cmbTipo.EditValue.ToString() == "2")
                    {
                        /*************************************************************************************************************/
                        System.Data.DataSet dsEm = new System.Data.DataSet();

                        var dsResultEm = poLogicaNegocio.gdsComisionesEmpleado(tIdPeriodo);
                        dsResultEm.Tables[0].TableName = "Empleado";

                        dsEm.Merge(dsResultEm);
                        xrptEmple.DataSource = dsEm;
                        xrptEmple.RequestParameters = false;

                        xrptEmple.Parameters["Titulo"].Value = psParametro;
                        xrptEmple.Parameters["Titulo"].Visible = false;

                        MostrarReporte(xrptEmple);
                    }
                    else if (cmbTipo.EditValue.ToString() == "3")
                    {
                        /*************************************************************************************************************/
                        System.Data.DataSet ds = new System.Data.DataSet();

                        var dsResult = poLogicaNegocio.gdsComisionesZona(tIdPeriodo);
                        dsResult.Tables[0].TableName = "ComiZonaCab";
                        dsResult.Tables[1].TableName = "ComiZonaDet";

                        ds.Merge(dsResult);
                        xrpt.DataSource = ds;
                        xrpt.RequestParameters = false;

                        xrpt.Parameters["Titulo"].Value = psParametro;
                        xrpt.Parameters["Titulo"].Visible = false;

                        MostrarReporte(xrpt);
                    }

                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarReporte(XtraReport report)
        {
            ReportPrintTool pts = new ReportPrintTool(report);
            pts.PrintingSystem.StartPrint += new PrintDocumentEventHandler(clsComun.PrintingSystem_StartPrint);
            pts.PrintingSystem.StartPrint += new PrintDocumentEventHandler(clsComun.reportsStartPrintEventHandler);
            pts.ShowRibbonPreview();
        }



    }
}
