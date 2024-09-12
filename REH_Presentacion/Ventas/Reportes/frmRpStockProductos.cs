using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using GEN_Entidad;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;
using VTA_Negocio;
using static GEN_Entidad.Diccionario;

namespace REH_Presentacion.Ventas.Reportes
{
    public partial class frmRpStockProductos : frmBaseTrxDev
    {
        clsNVentas loLogicaNegocio;

        public frmRpStockProductos()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNVentas();
        }

        private void frmRpStockProductos_Load(object sender, EventArgs e)
        {
            try
            {
                //var dtBodega = loLogicaNegocio.gdtSapListarBodegas("'01','03','04','06','08','30','32','34'");
                //foreach (DataRow item in dtBodega.Rows)
                //{
                //    string psBodega = string.Format("{0} - {1}", item[0].ToString(), item[1].ToString());
                //    CheckedListBoxItem checkedListBoxItem = new CheckedListBoxItem();
                //    checkedListBoxItem.Value = psBodega;
                //    clbBodega.Items.Add(checkedListBoxItem);
                //}

                string psBodegasExcluir = "";
                //psBodegasExcluir = loLogicaNegocio.goConsultarParametroVta().BodegasExluirStockProductoBatch;
                clsComun.gLLenarCombo(ref cmbBodega, loLogicaNegocio.goSapListarBodegas());

                lCargarEventosBotones();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Nuevo, Generalmente Limpia el formulario 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                lLimpiar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Consultar, Consulta Query
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            lBuscar();
        }

        /// <summary>
        /// Evento del botón Exportar, Exporta a Excel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {

                dgvDatos.PostEditor();
                DataTable poLista = (DataTable)bsDatos.DataSource;
                if (poLista != null && poLista.Rows.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcDatos, Text + ".xlsx", psFilter);
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnEnviarCorreo"] != null) tstBotones.Items["btnEnviarCorreo"].Click += btnEnviarCorreo_Click;
        }

        private void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                string psBodega = cmbBodega.EditValue.ToString().Replace(" ", "");

                if (!string.IsNullOrEmpty(psBodega))
                {
                    using (frmSeleccionarEmpleados frm = new frmSeleccionarEmpleados())
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            
                            List<string> listaCorreo = frm.EmpleadosSeleccionados;

                            string sCorreos = clsComun.gConcatenarCorreos(listaCorreo, ";");


                            if (!string.IsNullOrEmpty(sCorreos))
                            {
                                lEnviarCorreoStockBodega(sCorreos, ConfigurationManager.AppSettings["FileTmpCom"].ToString() + "StockBodegas.xlsx");
                                lBuscar();
                            }
                            else
                            {
                                XtraMessageBox.Show("No se seleccionaron empleados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("Seleccione Zonas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
        }

        private void lLimpiar()
        {
            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();

            //foreach (CheckedListBoxItem item in clbBodega.Items)
            //{
            //    item.CheckState = CheckState.Unchecked;
            //}
        }

        private void lBuscar(bool tbProcesoBatch = false)
        {

            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();

            //foreach (var item in clbBodega.Items.GetCheckedValues())
            //{
            //    var psBodega = item.ToString().Split('-')[0].Trim();
            //    psListaBodega.Add(psBodega);
            //}

            List<string> psListaBodega = new List<string>();

            string psBodega = cmbBodega.EditValue.ToString().Replace(" ", "");

            if (!string.IsNullOrEmpty(psBodega))
            {
                string[] bodegas = psBodega.Split(',');

                foreach (var bodega in bodegas)
                {
                    psListaBodega.Add(bodega.Trim());
                }
            }

            DataTable dt = loLogicaNegocio.gdtStockProductos(psListaBodega, tbProcesoBatch);

            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;
            dgvDatos.PopulateColumns();
            dgvDatos.FixedLineWidth = 3;
            dgvDatos.Columns[0].Fixed = FixedStyle.Left;
            dgvDatos.Columns[1].Fixed = FixedStyle.Left;
            dgvDatos.Columns[2].Fixed = FixedStyle.Left;

            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {

                if (i > 1)
                {
                    if (dt.Columns[i].DataType == typeof(int))
                    {
                        var psNameColumn = dgvDatos.Columns[i].FieldName;

                        dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                        dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        dgvDatos.Columns[i].DisplayFormat.FormatString = "{0:#,#}";
                        dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:#,#}");

                        GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                        item1.FieldName = psNameColumn;
                        item1.SummaryType = SummaryItemType.Sum;
                        item1.DisplayFormat = "{0:#,#}";
                        item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                        dgvDatos.GroupSummary.Add(item1);
                    }
                }
            }
            dgvDatos.BestFitColumns();
        }

        public void lEnviarCorreoStockBodega(string tsCorreos, string tsRuta, string tsCopia = null)
        {

            if (!string.IsNullOrEmpty(tsCorreos))
            {
                string psRuta = "";
                List<Attachment> listAdjuntosEmail = null;
                lBuscar(true);
                listAdjuntosEmail = new List<Attachment>();
                psRuta = tsRuta;
                gcDatos.ExportToXlsx(psRuta);

                if (File.Exists(psRuta))
                    listAdjuntosEmail.Add(new Attachment(psRuta));

                string psCuerpo = "<p>Estimados:<br/><br/>A continuación, se adjunta el reporte del stock de productos en <b>unidades por presentación</b> de todas las bodegas.<br/><br/>Saludos Cordiales.</p>";

                //clsComun.EnviarPorCorreo("varevalo@afecor.com", "Stock de Bodegas", psCuerpo, listAdjuntosEmail);
                clsComun.EnviarPorCorreo(tsCorreos, "Stock de Bodegas", psCuerpo, listAdjuntosEmail,false,tsCopia);
                if (File.Exists(psRuta)) File.Delete(psRuta);
            }
        }
    }
}
