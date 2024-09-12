using AFI_Negocio;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using REH_Presentacion.Comun;
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

namespace REH_Presentacion.ActivoFijo
{
    public partial class frmTrDepreciacion : frmBaseTrxDev
    {
        clsNActivoFijo loLogicaNegocio = new clsNActivoFijo();
        BindingSource bsDatos = new BindingSource();
        int lId;

        public frmTrDepreciacion()
        {
            InitializeComponent();
        }

        private void frmTrDepreciacion_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                
                lLimpiar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

        private void btnGenerar_Click(object sender, EventArgs e)
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
                var poListaObject = loLogicaNegocio.goListarItemActivoFijo();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("ID"),
                                    new DataColumn("Ítem"),
                                    new DataColumn("Fecha Compra", typeof(DateTime)),
                                    new DataColumn("Estado"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["ID"] = a.IdItemActivoFijo;
                    row["Ítem"] = a.Descripcion;
                    row["Fecha Compra"] = a.FechaCompra;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);

                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Ítems" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    lId = int.Parse(pofrmBuscar.lsCodigoSeleccionado);
                    lConsultar();
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
            if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Click += btnGenerar_Click;

            
        }

        private void lLimpiar()
        {
            txtAnio.EditValue = "";
            txtMes.EditValue = "";
        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtAnio.Text.Trim()) && !string.IsNullOrEmpty(txtMes.Text.Trim()))
            {
                gcDatos.DataSource = null;
                dgvDatos.Columns.Clear();

                DataTable dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC ACTSPCONSULTADEPRECIACION {0}, {1}", txtAnio.Text, txtMes.Text));

                bsDatos.DataSource = dt;
                gcDatos.DataSource = bsDatos;
                dgvDatos.PopulateColumns();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvDatos.Columns.Count; i++)
                    {
                        dgvDatos.Columns[i].OptionsColumn.ReadOnly = true;
                        // Quita numeral del nombre de la columna, sirve para convertir a número
                        if (dgvDatos.Columns[i].CustomizationSearchCaption.Contains("#")) dgvDatos.Columns[i].Caption = dgvDatos.Columns[i].CustomizationSearchCaption.Replace("#", "").Trim();

                        if (i > 1)
                        {
                            if (dt.Columns[i].DataType == typeof(decimal))
                            {
                                var psNameColumn = dgvDatos.Columns[i].FieldName;
                                if (psNameColumn.ToUpper().Contains("%") || psNameColumn.ToUpper().Contains("PORC"))
                                {
                                    dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                    dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                    dgvDatos.Columns[i].DisplayFormat.FormatString = "p2";

                                    //dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:0.00}");


                                    //GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                                    //item1.FieldName = psNameColumn;
                                    //item1.SummaryType = SummaryItemType.Sum;
                                    //item1.DisplayFormat = "{0:0.00}";
                                    //item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                                    //dgvDatos.GroupSummary.Add(item1);
                                }
                                else if (psNameColumn.ToUpper().Contains("PESO"))
                                {
                                    dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                    dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                    dgvDatos.Columns[i].DisplayFormat.FormatString = "{0:##,##0.0000}";

                                }
                                else if (psNameColumn.ToUpper().Contains("#") || psNameColumn.ToUpper().Contains("CANT") || psNameColumn.ToUpper().Contains("UNI"))
                                {
                                    dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                    dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                    dgvDatos.Columns[i].DisplayFormat.FormatString = "n2";
                                    dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:n2}");


                                    GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                                    item1.FieldName = psNameColumn;
                                    item1.SummaryType = SummaryItemType.Sum;
                                    item1.DisplayFormat = "{0:n2}";
                                    item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
                                    dgvDatos.GroupSummary.Add(item1);
                                }
                                else
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
                            else if (dt.Columns[i].DataType == typeof(int))
                            {
                                var psNameColumn = dgvDatos.Columns[i].FieldName;

                                if (psNameColumn.ToUpper().Contains("#") || psNameColumn.ToUpper().Contains("CANT") || psNameColumn.ToUpper().Contains("UNI"))
                                {
                                    dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
                                    dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                    dgvDatos.Columns[i].DisplayFormat.FormatString = "n";
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
                    }
                }

                dgvDatos.OptionsBehavior.Editable = true;
                dgvDatos.OptionsCustomization.AllowColumnMoving = false;
                dgvDatos.OptionsView.ColumnAutoWidth = false;
                dgvDatos.OptionsView.ShowAutoFilterRow = true;
                dgvDatos.OptionsView.BestFitMode = GridBestFitMode.Full;
                dgvDatos.BestFitColumns();
            }
        }
    }
}
