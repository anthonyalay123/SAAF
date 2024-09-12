using CRE_Negocio.Transacciones;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Credito;
using REH_Presentacion.Credito.Reportes.Rpt;
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

namespace REH_Presentacion.Credito.Transacciones
{
    public partial class frmTrBandejaInformeRTC : frmBaseTrxDev
    {
        clsNProcesoCredito loLogicaNegocio = new clsNProcesoCredito();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnReport = new RepositoryItemButtonEdit();
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();
        RepositoryItemButtonEdit rpiBtnShowComentarios = new RepositoryItemButtonEdit();

        public frmTrBandejaInformeRTC()
        {
            InitializeComponent();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnReport.ButtonClick += rpiBtnReport_ButtonClick;
        }

        private void frmTrBandejaInformeRTC_Load(object sender, EventArgs e)
        {
            try
            {
                dtpFechaInicial.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpFechaFinal.DateTime = DateTime.Now;
                
                lCargarEventosBotones();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<BandejaInformeRTC>)bsDatos.DataSource;

                string psForma = Diccionario.Tablas.Menu.frmTrInformeRTC;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                if (poForm != null)
                {
                    var poObj = loLogicaNegocio.goConsultar(poLista[piIndex].Id);

                    frmTrInformeRTC frm = new frmTrInformeRTC();
                    frm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    frm.Text = poForm.Nombre;
                    frm.lIdProceso = poObj.IdProcesoCredito;
                    frm.lsCardCode = poObj.CodigoCliente;
                    frm.lsTipoProceso = poObj.CodigoTipoSolicitud;
                    frm.lsCliente = poObj.Cliente;
                    frm.ldcCupoSAP = poObj.CupoSap;
                    frm.ldcCupoSolicitado = poObj.CupoSolicitado;
                    frm.liPlazoSolicitado = poObj.PlazoSolicitado??0;
                    frm.lsTipoPersona = poObj.CodigoTipoPersona;
                    frm.ShowDialog();
                    loLogicaNegocio.gsActualzarRequerimientoCredito(poObj.IdProcesoCredito, "", clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    lListar();
                }
                else
                {
                    XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnReport_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                var poLista = (List<BandejaInformeRTC>)bsDatos.DataSource;
                var piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                if (poLista[piIndex].IdInf != 0)
                {
                    var ds = loLogicaNegocio.goConsultaDataSet("EXEC CRESPRPTINFORMERTC " + "'" + poLista[piIndex].IdInf + "'");

                    ds.Tables[0].TableName = "Cab";
                    ds.Tables[1].TableName = "UbiCul";
                    ds.Tables[2].TableName = "PriCli";
                    ds.Tables[3].TableName = "PriPro";
                    ds.Tables[4].TableName = "ProCom";
                    ds.Tables[5].TableName = "Croquis";

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        xrptInformeRTC xrpt = new xrptInformeRTC();
                        xrpt.DataSource = ds;
                        xrpt.RequestParameters = false;

                        using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                        {
                            printTool.ShowRibbonPreviewDialog();
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existen datos para imprimir", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;

            bsDatos.DataSource = new List<BandejaInformeRTC>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["IdInf"].Visible = false;

            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvDatos.Columns["Transaccion"], "Transacción", Diccionario.ButtonGridImage.show_16x16, 40);
            clsComun.gDibujarBotonGrid(rpiBtnReport, dgvDatos.Columns["Imprimir"], "Imprimir", Diccionario.ButtonGridImage.printer_16x16, 40);

            dgvDatos.Columns["Id"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["Usuario"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["Fecha"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["TipoSolicitud"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["Cliente"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["RepresentanteLegal"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["EstadoRequerimiento"].OptionsColumn.ReadOnly = true;
            dgvDatos.Columns["EstadoInforme"].OptionsColumn.ReadOnly = true;
            
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            try
            {
                lListar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lListar()
        {
            Cursor.Current = Cursors.WaitCursor;

            var menu = Tag.ToString().Split(',');
            var poObject = loLogicaNegocio.goConsultarBandejaRTC(dtpFechaInicial.DateTime, dtpFechaFinal.DateTime, clsPrincipal.gsUsuario);
            if (poObject != null)
            {
                bsDatos.DataSource = poObject;
                gcDatos.DataSource = bsDatos.DataSource;
                dgvDatos.Columns[0].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            }

            clsComun.gOrdenarColumnasGridFullEditable(dgvDatos);

            Cursor.Current = Cursors.Default;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                lListar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
