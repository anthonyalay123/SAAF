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
using DevExpress.XtraGrid;
using DevExpress.XtraEditors;
using REH_Presentacion.Reportes;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 07/06/2021
    /// Formulario para consulta de Vacaciones
    /// </summary>
    public partial class frmTrVacaciones : frmBaseTrxDev
    {

        #region Variables
        clsNVacacion loLogicaNegocio;
        public int lIdPeriodo;
        public bool lbNominaCerrada = false;
        #endregion

        #region Eventos
        public frmTrVacaciones()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNVacacion();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                lBuscar();
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
                if (lbEsValido(false))
                {
                    lBuscar();
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Grabar, Graba las celdas editables.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    List<PlantillaVacacion> poLista = (List<PlantillaVacacion>)bsDatos.DataSource;
                    if (poLista.Count > 0)
                    {
                        if (loLogicaNegocio.gbGuardarDiasObservaciones(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lBuscar();
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen Datos", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }
        
        
        /// <summary>
        /// Exportar datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                var poLista = (List<PlantillaVacacion>)bsDatos.DataSource;
                if (poLista.Count > 0)
                {
                    clsComun.gSaveFile(gcDatos, Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        
        #endregion

        #region Métodos
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
        }

        private bool lbEsValido(bool tbGenerar = true)
        {

            return true;
        }

        private void lLimpiar()
        {
            
        }

        private void lBuscar()
        {

            var poLista = loLogicaNegocio.goConsultaVacaciones();
            bsDatos.DataSource = poLista;
            gcDatos.DataSource = bsDatos;

            dgvDatos.PopulateColumns();
            dgvDatos.Columns[1].Visible = false; // Cédula
            dgvDatos.Columns[2].Visible = false; // Cédula
            //dgvDatos.Columns[1].Width = 110; // Identificación
            dgvDatos.Columns[3].Width = 200; // Nombre
            dgvDatos.FixedLineWidth = 2;
            dgvDatos.Columns[0].Fixed = FixedStyle.Left;
            dgvDatos.Columns[1].Fixed = FixedStyle.Left;
            dgvDatos.Columns[2].Fixed = FixedStyle.Left;
            dgvDatos.Columns[3].Fixed = FixedStyle.Left;

            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                // Las últimas 3 Columnas se habilitan para editar (DiasLiqPendGozar, DiasGozaPendLiq, Observaciones
                if (i < (dgvDatos.Columns.Count -3))
                {
                    dgvDatos.Columns[i].OptionsColumn.AllowEdit = false;
                } 
                else
                {
                    dgvDatos.Columns[i].OptionsColumn.AllowEdit = true;
                }

                if (i > 2)
                {
                    string psNameColumn = dgvDatos.Columns[i].FieldName;
                    if (dgvDatos.Columns[i].ColumnType == typeof(int) || dgvDatos.Columns[i].ColumnType == typeof(double))
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
                    else if (dgvDatos.Columns[i].ColumnType == typeof(decimal))
                    {
                        if (psNameColumn.ToLower().Contains("propo") || psNameColumn.ToLower().Contains("saldodias"))
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

                }
                
            }

            dgvDatos.BestFitColumns();


         
        }
        #endregion

    }
}
