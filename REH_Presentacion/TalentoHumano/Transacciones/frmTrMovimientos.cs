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
using GEN_Entidad.Entidades;
using DevExpress.XtraEditors.Repository;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 15/03/2021
    /// Formulario para registrar el cálculo de utilidades 
    /// </summary
    public partial class frmTrMovimientos : frmBaseTrxDev
    {

        #region Variables
        clsNMovimientos loLogicaNegocio;
        private bool pbCargado = false;
        private bool pbAgregarSumatoria = false;
        public int lIdPeriodo;
        RepositoryItemButtonEdit rpiBtnDel;
        #endregion

        #region Eventos
        public frmTrMovimientos()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNMovimientos();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }
        /// <summary>
        /// Evento que inicia el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                
                clsComun.gLLenarCombo(ref cmbTipoMovimiento, loLogicaNegocio.goConsultarComboTipoMovientos(), true);
                clsComun.gLLenarCombo(ref cmbEmpleado, loLogicaNegocio.goConsultarCombPersonas(), true);
                pbCargado = true;
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
                if (lbEsValido())
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
        /// Evento del botón Grabar, Graba detalle de empleados externos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                dgvDatos.PostEditor();
                string psMsgValida = lsEsValido();

                List<MovimientosGrid> poLista = (List<MovimientosGrid>)bsDatos.DataSource;
                List<int> pListaId = new List<int>();
                if (poLista.Count > 0)
                {
                    string psMsg = loLogicaNegocio.gsGuardarMovimientos(cmbTipoMovimiento.EditValue.ToString(), poLista.Where(x=>x.IdNomina == 0 && x.IdReferencial == 0).ToList(), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, pListaId);
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lBuscar();
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a guardar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;

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
                if (lbEsValido())
                {
                    OpenFileDialog ofdRuta = new OpenFileDialog();
                    ofdRuta.Title = "Seleccione Archivo";
                    //(*.jpg; *.jpeg; *.png)| *.jpg; *.jpeg; *.png; "
                    ofdRuta.Filter = "Files(*.xls; *.xlsx;)|*.xls; *.xlsx;";

                    if (ofdRuta.ShowDialog() == DialogResult.OK)
                    {
                        if (!ofdRuta.FileName.Equals(""))
                        {
                            var poLista = (List<MovimientosGrid>)bsDatos.DataSource;
                            DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);
                            var poListaImportada = new List<MovimientosGrid>();

                            foreach (DataRow item in dt.Rows)
                            {
                                if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                                {
                                    MovimientosGrid poItem = new MovimientosGrid();
                                    //poItem.Anio = int.Parse(item[0].ToString().Trim());
                                    //poItem.Mes = int.Parse(item[1].ToString().Trim());
                                    poItem.Cedula = item[0].ToString().Trim();
                                    poItem.Empleado = item[1].ToString().Trim();
                                    poItem.Valor = Decimal.Parse(item[3].ToString().Trim());
                                    poItem.Observacion = item[2].ToString().Trim();
                                    poItem.CodigoTipoMovimiento = cmbTipoMovimiento.EditValue.ToString();
                                    poItem.FechaIngreso = DateTime.Now.Date;
                                    poListaImportada.Add(poItem);
                                }
                            }

                            string psMsg = lsValidaDuplicados(poLista, poListaImportada);
                            if (!string.IsNullOrEmpty(psMsg))
                            {
                                XtraMessageBox.Show(psMsg, "No es posible importar datos duplicados!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            poLista.AddRange(poListaImportada);

                            bsDatos.DataSource = poLista;
                            dgvDatos.BestFitColumns();
                            XtraMessageBox.Show("Importado Exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                    }

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
        private void btnImportarNC_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbEsValido())
                {
                    OpenFileDialog ofdRuta = new OpenFileDialog();
                    ofdRuta.Title = "Seleccione Archivo";
                    //(*.jpg; *.jpeg; *.png)| *.jpg; *.jpeg; *.png; "
                    ofdRuta.Filter = "Files(*.xls; *.xlsx;)|*.xls; *.xlsx;";

                    if (ofdRuta.ShowDialog() == DialogResult.OK)
                    {
                        if (!ofdRuta.FileName.Equals(""))
                        {
                            var poLista = (List<MovimientosGrid>)bsDatos.DataSource;
                            DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);
                            var poListaImportada = new List<MovimientosGrid>();

                            foreach (DataRow item in dt.Rows)
                            {
                                try
                                {
                                    if (item[4].ToString().Length == 10)
                                    {
                                        MovimientosGrid poItem = new MovimientosGrid();
                                        //poItem.Anio = int.Parse(item[0].ToString().Trim());
                                        //poItem.Mes = int.Parse(item[1].ToString().Trim());
                                        poItem.Cedula = item[4].ToString().Trim();
                                        poItem.Empleado = item[5].ToString().Trim();
                                        poItem.Valor = (Math.Abs(Decimal.Parse(item[10].ToString().Trim())) * -1);
                                        poItem.Observacion = "NC - " + item[3].ToString().Trim();
                                        poItem.CodigoTipoMovimiento = cmbTipoMovimiento.EditValue.ToString();
                                        poItem.FechaIngreso = DateTime.Now.Date;
                                        //poItem.IdReferencial = int.Parse(item[3].ToString().Trim());
                                        poListaImportada.Add(poItem);
                                    }
                                }
                                catch (Exception)
                                {
                                    
                                }
                                
                            }

                            string psMsg = lsValidaDuplicados(poLista, poListaImportada);
                            if (!string.IsNullOrEmpty(psMsg))
                            {
                                XtraMessageBox.Show(psMsg, "No es posible importar datos duplicados!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                            poLista.AddRange(poListaImportada);

                            bsDatos.DataSource = poLista;
                            dgvDatos.BestFitColumns();
                            XtraMessageBox.Show("Importado Exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
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
                bs.DataSource = new List<MovimientosExcel>();

                // Exportar Datos
                clsComun.gSaveFile(gc, "Plantilla_" + Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");

                gc.Visible = false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Imprimir, Presenta por pantalla Reporte.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTipoMovimiento.EditValue.ToString() != Diccionario.Seleccione)
                {
                    DataSet ds = new DataSet();
                    var dt = loLogicaNegocio.gdtConsultarMovimientoResumen(cmbTipoMovimiento.EditValue.ToString());
                    dt.TableName = "SaldoMovimiento";
                    ds.Merge(dt);
                    if (dt.Rows.Count > 0)
                    {
                        xrptSaldoMovimientos xrpt = new xrptSaldoMovimientos();
                        xrpt.DataSource = ds;

                        //Se establece el origen de datos del reporte (El dataset previamente leído)
                        xrpt.Parameters["parameter1"].Value = loLogicaNegocio.tsTituloReporte();
                        xrpt.DataSource = ds;
                        //Se invoca la ventana que muestra el reporte.
                        xrpt.RequestParameters = false;
                        xrpt.Parameters["parameter1"].Visible = false;


                        using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                        {
                            printTool.ShowRibbonPreviewDialog();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                List<MovimientosGrid> poLista = (List<MovimientosGrid>)bsDatos.DataSource;
                var poListaMov = poLista.Select(x => new MovimientosExport()
                {
                    Cedula = x.Cedula,
                    Empleado = x.Empleado,
                    FechaIngreso = x.FechaIngreso,
                    Observacion = x.Observacion,
                    TipoMovimiento = x.CodigoTipoMovimiento,
                    Valor = x.Valor
                }).ToList();


                if (poListaMov.Count > 0)
                {
                    GridControl gc = new GridControl();
                    BindingSource bs = new BindingSource();
                    GridView dgv = new GridView();
                    
                    gc.DataSource = bs;
                    gc.MainView = dgv;
                    gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                    dgv.GridControl = gc;

                    this.Controls.Add(gc);
                    bs.DataSource = poListaMov;

                    clsComun.gSaveFile(gc, "Registro_Movimientos.xlsx", "Files(*.xlsx;)|*.xlsx;");
                    gc.Visible = false;
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar en la tabla # 1.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                dgvResumen.PostEditor();
                List<SpConsultaMovimientosResumen> poListaResumen = (List<SpConsultaMovimientosResumen>)bsResumen.DataSource;
                if (poListaResumen.Count > 0)
                {
                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcResumen, "Saldo_Movimientos.xlsx", psFilter);
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar en la tabla # 2.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        /// <summary>
        /// Evento del boton de eliminar en el Grid, elimina la fila seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<MovimientosGrid>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];
                    if (poEntidad.IdNomina == 0 && poEntidad.IdReferencial == 0)
                    {
                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsDatos.DataSource = poLista;
                        dgvDatos.RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbPeriodo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    cmbTipoMovimiento.ReadOnly = true;
                    lBuscar();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        #endregion

        #region Métodos
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Click += btnImportar_Click;
            if (tstBotones.Items["btnImportarNC"] != null) tstBotones.Items["btnImportarNC"].Click += btnImportarNC_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnPlantilla"] != null) tstBotones.Items["btnPlantilla"].Click += btnPlantilla_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;


            cmbEmpleado.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbTipoMovimiento.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtValor.KeyDown += new KeyEventHandler(EnterEqualTab);

            bsDatos.DataSource = new List<MovimientosGrid>();
            gcDatos.DataSource = bsDatos;

            bsResumen.DataSource = new List<SpConsultaMovimientosResumen>();
            gcResumen.DataSource = bsResumen;

            dgvResumen.OptionsView.ShowAutoFilterRow = true;
            dgvResumen.OptionsBehavior.Editable = false;

            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            dgvDatos.OptionsBehavior.Editable = true;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Eliminar"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);

            dgvDatos.Columns["IdMovimiento"].Visible = false;
            dgvDatos.Columns["CodigoTipoMovimiento"].Visible = false;
            dgvDatos.Columns["IdNomina"].Visible = false;
            dgvDatos.Columns["Anio"].Visible = false;
            dgvDatos.Columns["Mes"].Visible = false;
            dgvDatos.Columns["IdReferencial"].Visible = false;

            dgvDatos.Columns["CodigoRubro"].Visible = false;
            dgvDatos.Columns["Rubro"].Visible = false;
            dgvDatos.Columns["CodigoTipoPrestamo"].Visible = false;
            dgvDatos.Columns["TipoPrestamo"].Visible = false;

            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                var psNameColumn = dgvDatos.Columns[i].FieldName;
                if (psNameColumn != "Valor" && psNameColumn != "Observacion" && psNameColumn != "Eliminar")
                {
                    dgvDatos.Columns[i].OptionsColumn.AllowEdit = false;
                }
            }
            

            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {

                if (dgvDatos.Columns[i].ColumnType == typeof(decimal))
                {
                    var psNameColumn = dgvDatos.Columns[i].FieldName;

                    //dgvDatos.Columns[i].UnboundType = UnboundColumnType.Decimal;
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


            for (int i = 0; i < dgvResumen.Columns.Count; i++)
            {

                if (dgvResumen.Columns[i].ColumnType == typeof(decimal))
                {
                    var psNameColumn = dgvResumen.Columns[i].FieldName;

                    //dgvResumen.Columns[i].UnboundType = UnboundColumnType.Decimal;
                    dgvResumen.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    dgvResumen.Columns[i].DisplayFormat.FormatString = "c2";
                    dgvResumen.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");

                    GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                    item1.FieldName = psNameColumn;
                    item1.SummaryType = SummaryItemType.Sum;
                    item1.DisplayFormat = "{0:c2}";
                    item1.ShowInGroupColumnFooter = dgvResumen.Columns[psNameColumn];
                    dgvResumen.GroupSummary.Add(item1);

                }

            }

            txtValor.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtAnio.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtMes.KeyPress += new KeyPressEventHandler(SoloNumeros);


        }

        private bool lbEsValido()
        {
            
            if (cmbTipoMovimiento.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Tipo Movimiento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            
           
            return true;
        }

        private void lLimpiar()
        {
            cmbTipoMovimiento.ReadOnly = false;
            pbCargado = false;
            if ((cmbTipoMovimiento.Properties.DataSource as IList).Count > 0) cmbTipoMovimiento.ItemIndex = 0;
            if ((cmbEmpleado.Properties.DataSource as IList).Count > 0) cmbEmpleado.ItemIndex = 0;
            pbCargado = true;

            bsDatos.DataSource = new List<MovimientosGrid>();
            gcDatos.DataSource = bsDatos;

            bsResumen.DataSource = new List<SpConsultaMovimientosResumen>();
            gcResumen.DataSource = bsResumen;

        }

        private void lBuscar()
        {
            var poLista = loLogicaNegocio.goConsultarMovimientos(cmbTipoMovimiento.EditValue.ToString());
            bsDatos.DataSource = poLista;
            //gcDatos.DataSource = bsDatos;

            //dgvDatos.OptionsView.ColumnAutoWidth = false;
            //dgvDatos.OptionsView.BestFitMode = GridBestFitMode.Full;
            dgvDatos.BestFitColumns();

            var poLista2 = loLogicaNegocio.goConsultarMovimientoResumen(cmbTipoMovimiento.EditValue.ToString());
            bsResumen.DataSource = poLista2;

            //dgvResumen.OptionsView.ColumnAutoWidth = false;
            //dgvResumen.OptionsView.BestFitMode = GridBestFitMode.Full;
            dgvResumen.BestFitColumns();

        }

        private void lChangeValue()
        {
            if (pbCargado)
            {
                lBuscar();
            }
        }

        #endregion

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                if(lbEsValido())
                {
                    string psMsgValida = lsEsValido();
                    if (string.IsNullOrEmpty(psMsgValida))
                    {

                        var poLista = (List<MovimientosGrid>)bsDatos.DataSource;
                        var poListaAgregar = new List<MovimientosGrid>();
                        MovimientosGrid poItem = new MovimientosGrid();
                        //poItem.Anio = int.Parse(txtAnio.Text.Trim());
                        //poItem.Mes = int.Parse(txtMes.Text.Trim());
                        poItem.Valor = Decimal.Parse(txtValor.Text.Trim());
                        poItem.CodigoTipoMovimiento = cmbTipoMovimiento.EditValue.ToString();
                        poItem.Cedula = cmbEmpleado.EditValue.ToString();
                        poItem.Empleado = cmbEmpleado.Text;

                        poListaAgregar.Add(poItem);
                        string psMsg = lsValidaDuplicados(poLista, poListaAgregar);
                        if (!string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(psMsg, "No es posible agregar duplicados!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        poLista.Add(poItem);
                        txtValor.Text = "0";
                        if ((cmbEmpleado.Properties.DataSource as IList).Count > 0) cmbEmpleado.ItemIndex = 0;

                        bsDatos.DataSource = poLista;
                        dgvDatos.BestFitColumns();
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsgValida, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string lsValidaDuplicados(List<MovimientosGrid> toListaGrid, List<MovimientosGrid> toListaInsertar)
        {
            string psMsg = string.Empty;

            //foreach (var item in toListaInsertar)
            //{
            //    var piRegistro = toListaGrid.Where(x => x.Observacion == item.Observacion && x.Valor == item.Valor && x.Cedula == item.Cedula).Count();
            //    if (piRegistro > 0)
            //    {
            //        psMsg = string.Format("{0}Empleado: {1} con la observación: {2} y el Valor: {3} Ya están ingresados. \n", psMsg, item.Empleado, item.Observacion, item.Valor);
            //    }
            //}

            return psMsg;
        }


        private string lsEsValido(bool tbGenerar = true)
        {

            string psMsg = string.Empty;

            //if (string.IsNullOrEmpty(txtAnio.Text))
            //{
            //    psMsg = string.Format("{0}Ingrese el Año. \n", psMsg);
            //}
            //if (string.IsNullOrEmpty(txtMes.Text))
            //{
            //    psMsg = string.Format("{0}Ingrese el Mes. \n", psMsg);
            //}
            if (string.IsNullOrEmpty(txtValor.Text))
            {
                psMsg = string.Format("{0}Ingrese el Valor. \n", psMsg);
            }
            if (cmbEmpleado.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Empleado. \n", psMsg);
            }
            
            return psMsg;
        }

        private void dgvDatos_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                var poLista = (List<MovimientosGrid>)bsDatos.DataSource;
                var piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                if (poLista[piIndex].IdNomina > 0 || poLista[piIndex].IdReferencial > 0)
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
