using DevExpress.XtraGrid.Views.Grid;
using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 17/08/2020
    /// Formulario para Carga de Plantilla Empleado 
    /// </summary>
    public partial class frmTrPlantillaVacacion : frmBaseTrxDev
    {

        #region Variables
        clsNPlantilla loLogicaNegocio;
        #endregion

        #region Eventos
        public frmTrPlantillaVacacion()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNPlantilla();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
               
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
        /// Evento del botón Generar, Genera Novedad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                
                List<PlantillaVacacionExcel> poLista = (List<PlantillaVacacionExcel>)bsDatos.DataSource;
                if (poLista.Count > 0)
                {
                    string psMensaje = loLogicaNegocio.gsImportarVacaciones(clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, poLista);

                    if (string.IsNullOrEmpty(psMensaje))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                    else
                    {
                        XtraMessageBox.Show(psMensaje, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a importar", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Evento del botón Importar, Carga Datos en formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                //gcDatos.DataSource = null;
                //dgvDatos.Columns.Clear();
                //bsDatos.DataSource = new List<PlantillaVacacionExcel>();

                List<PlantillaVacacionExcel> poLista = new List<PlantillaVacacionExcel>();

                OpenFileDialog ofdRuta = new OpenFileDialog();
                ofdRuta.Title = "Seleccione Archivo";
                //(*.jpg; *.jpeg; *.png)| *.jpg; *.jpeg; *.png; "
                ofdRuta.Filter = "Files(*.xls; *.xlsx;)|*.xls; *.xlsx;";

                if (ofdRuta.ShowDialog() == DialogResult.OK)
                {
                    if (!ofdRuta.FileName.Equals(""))
                    {
                        DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);

                        foreach(DataRow item in dt.Rows)
                        {
                            PlantillaVacacionExcel poItem = new PlantillaVacacionExcel();
                            poItem.Periodo = int.Parse(item[0].ToString().Trim());
                            poItem.Cedula  = item[1].ToString().Trim();
                            poItem.Empleado = item[2].ToString().Trim();
                            poItem.FechaInicial = DateTime.Parse(item[3].ToString().Trim());
                            poItem.FechaFinal= DateTime.Parse(item[4].ToString().Trim());
                            poItem.DiasNormales = int.Parse(item[5].ToString().Trim());
                            poItem.DiasAdicionales = int.Parse(item[6].ToString().Trim());
                            poItem.TotalDias = int.Parse(item[7].ToString().Trim());
                            poItem.ValorDiasNormales = decimal.Parse(item[8].ToString().Trim());
                            poItem.ValorDiasAdicionales = decimal.Parse(item[9].ToString().Trim());
                            poItem.ValorTotalDias = decimal.Parse(item[10].ToString().Trim());
                            poItem.DiasNormalesDevengados = int.Parse(item[11].ToString().Trim());
                            poItem.DiasAdicionalesDevengados = int.Parse(item[12].ToString().Trim());
                            poItem.TotalDiasDevengados = int.Parse(item[13].ToString().Trim());
                            poItem.ValorDiasNormalesDevengados = decimal.Parse(item[14].ToString().Trim());
                            poItem.ValorDiasAdicionalesDevengados = decimal.Parse(item[15].ToString().Trim());
                            poItem.ValorTotalDiasDevengados = decimal.Parse(item[16].ToString().Trim());
                            poItem.SaldoDias = int.Parse(item[17].ToString().Trim());
                            poItem.SaldoValor = decimal.Parse(item[18].ToString().Trim());
                            poItem.DiasGozadosPorLiquidar = int.Parse(item[19].ToString().Trim());
                            poItem.DiasLiquidadosPorGozar = int.Parse(item[20].ToString().Trim());
                            poItem.Observaciones = item[21].ToString().Trim();
                            poLista.Add(poItem);
                        }

                        bsDatos.DataSource = poLista.ToList();


                        for (int i = 0; i < dgvDatos.Columns.Count; i++)
                        {

                            if (i > 1)
                            {
                                var psNameColumn = dgvDatos.Columns[i].FieldName;

                                if (dt.Columns[i].DataType == typeof(int) || dt.Columns[i].DataType == typeof(double))
                                {
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
                                else if (dt.Columns[i].DataType == typeof(decimal))
                                {
                                    dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                    dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                    dgvDatos.Columns[i].DisplayFormat.FormatString = "c2";
                                    dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");


                                    GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                                    item1.FieldName = psNameColumn;
                                    item1.SummaryType = SummaryItemType.Sum;
                                    item1.DisplayFormat = "{0:c2}";
                                    item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                                    dgvDatos.GroupSummary.Add(item1);
                                }
                            }
                        }
                        dgvDatos.BestFitColumns();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Plantilla, Exporta Plantilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlantilla_Click(object sender, EventArgs e)
        {
            try
            {
                GridControl gc = new GridControl();
                BindingSource bs = new BindingSource();
                GridView dgv = new GridView();

                gc.DataSource = bs;
                gc.MainView = dgv;
                gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                dgv.GridControl = gc;

                this.Controls.Add(gc);
                bs.DataSource = new List<PlantillaVacacionExcel>();

                // Exportar Datos
                clsComun.gSaveFile(gc, "Plantilla_" + Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");

                gc.Visible = false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Click += btnImportar_Click;
            if (tstBotones.Items["btnPlantilla"] != null) tstBotones.Items["btnPlantilla"].Click += btnPlantilla_Click;

            bsDatos.DataSource = new List<PlantillaVacacionExcel>();
            gcDatos.DataSource = bsDatos;
        }
        
        private void lLimpiar()
        {
            gcDatos.DataSource = null;
            dgvDatos.Columns.Clear();
        }

        private void lBuscar()
        {
            //bsDatos.DataSource = loLogicaNegocio.gConsultarFondoReserva(int.Parse(cmbPeriodo.EditValue.ToString()));
        }
        
        #endregion

    }
}
