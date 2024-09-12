using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
using REH_Presentacion.Formularios;
using REH_Presentacion.Ventas.Reportes.Rpt;
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
using VTA_Negocio;

namespace REH_Presentacion.Ventas.Parametrizadores
{
    public partial class frmPaPresupuestoVentasRebateGC : frmBaseTrxDev
    {

        #region Variables
        clsNRebate loLogicaNegocio;
        BindingSource bsDatos;
        private bool lbCargado;
        List<Combo> loListaGrupoClientes;
        List<Combo> loListaZonas;
        RepositoryItemButtonEdit rpiBtnDel;
        RepositoryItemButtonEdit rpiBtnReport;
        #endregion

        public frmPaPresupuestoVentasRebateGC()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNRebate();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnReport = new RepositoryItemButtonEdit();
            rpiBtnReport.ButtonClick += rpiBtnReport_ButtonClick;
        }

        private void frmPaPresupuestoVentasRebate_Load(object sender, EventArgs e)
        {
            try
            {
                lbCargado = false;
                loListaGrupoClientes = loLogicaNegocio.goSapConsultaGrupoClientes();
                clsComun.gLLenarCombo(ref cmbCliente, loListaGrupoClientes, true);
                //clsComun.gLLenarCombo(ref cmbVendedor, loLogicaNegocio.goSapConsultVendedores(), true);
                loListaZonas = loLogicaNegocio.goSapConsultaZonas();
                clsComun.gLLenarCombo(ref cmbZona, loListaZonas, true);
                lbCargado = true;
                lCargarEventosBotones();
                lBuscar();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnReport_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                lImprimir();
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
        /// Evento del botón Plantilla, Exporta Plantilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlantilla_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear Lista de la Plantilla a exportar
                if (true)
                {
                    int piPeriodo = DateTime.Now.Year;
                    if (!string.IsNullOrEmpty(txtAnio.Text.Trim()))
                    {
                        piPeriodo = int.Parse(txtAnio.Text.Trim());
                    }

                    DataTable dt = loLogicaNegocio.gdtPlantillaPResupuestoRebateGC(piPeriodo);
                    if (dt.Rows.Count > 0)
                    {
                        //dt.Columns.Remove("Departamento");
                        // Grid Control y Binding Object
                        GridControl gc = new GridControl();
                        BindingSource bs = new BindingSource();
                        GridView dgv = new GridView();

                        gc.DataSource = bs;
                        gc.MainView = dgv;
                        gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                        dgv.GridControl = gc;
                        this.Controls.Add(gc);
                        bs.DataSource = dt;
                        dgv.BestFitColumns();
                        dgv.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
                        // Exportar Datos
                        clsComun.gSaveFile(gc, "Plantilla_" + Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");

                        gc.Visible = false;

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
        private void btnImportar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int cont = 0;
            try
            {
                OpenFileDialog ofdRuta = new OpenFileDialog();
                ofdRuta.Title = "Seleccione Archivo";
                //(*.jpg; *.jpeg; *.png)| *.jpg; *.jpeg; *.png; "
                ofdRuta.Filter = "Files(*.xls; *.xlsx;)|*.xls; *.xlsx;";

                XtraMessageBox.Show("Antes de Importar asegurese de los siguientes puntos: \n" +
                    "- En las celdas de valores no pueden estar vacias, valor por defecto '0' \n" +
                    "- El archivo debe ser excel y debe contener solo una hoja \n" +
                    "- Los nombres de las columnas deben estar como están en la plantilla", "Importante!!", MessageBoxButtons.OK, MessageBoxIcon.Information);


                if (ofdRuta.ShowDialog() == DialogResult.OK)
                {
                    if (!ofdRuta.FileName.Equals(""))
                    {
                        DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);
                        var poLista = (List<PresupuestoRebatePivotGridGC>)bsDatos.DataSource;
                        var poListaImportada = new List<PresupuestoRebatePivotGridGC>();

                        string psMsgLista = string.Empty;
                        int fila = 1;
                        foreach (DataRow item in dt.Rows)
                        {
                            fila++;
                            if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                            {
                                PresupuestoRebatePivotGridGC poItem = new PresupuestoRebatePivotGridGC();
                                poItem.Periodo = int.Parse(item[0].ToString().Trim());
                                //poItem.CodeZona = item[1].ToString().Trim();
                                poItem.NameZona = item[1].ToString().Trim();
                                string psCodeZona = loListaZonas.Where(x => x.Descripcion == poItem.NameZona).Select(x => x.Codigo).FirstOrDefault();
                                if (!string.IsNullOrEmpty(psCodeZona))
                                {
                                    poItem.CodeZona = psCodeZona;
                                }
                                else
                                {
                                    psMsgLista = string.Format("{0}No se encontró Zona con el nombre: '{1}'  en la fila {2}.\n", psMsgLista, poItem.NameZona, fila);
                                }
                                //poItem.IdGrupoCliente = int.Parse(item[3].ToString().Trim());
                                poItem.NombreGrupo = item[2].ToString().Trim();
                                string psCodeGC = loListaGrupoClientes.Where(x => x.Descripcion == poItem.NombreGrupo).Select(x => x.Codigo).FirstOrDefault();
                                if (!string.IsNullOrEmpty(psCodeGC))
                                {
                                    poItem.IdGrupoCliente = int.Parse(psCodeGC);
                                }
                                else
                                {
                                    psMsgLista = string.Format("{0}No se encontró Grupo Cliente con el nombre: '{1}'  en la fila {2}.\n", psMsgLista, poItem.NombreGrupo, fila);
                                }

                                poItem.Trimestre1 = Convert.ToDecimal(item[3].ToString().Trim());
                                poItem.Trimestre2 = Convert.ToDecimal(item[4].ToString().Trim());
                                poItem.Trimestre3 = Convert.ToDecimal(item[5].ToString().Trim());
                                poItem.Trimestre4 = Convert.ToDecimal(item[6].ToString().Trim());
                                poItem.Observacion = item[7].ToString().Trim();
                                poListaImportada.Add(poItem);
                            }

                        }

                        if (!string.IsNullOrEmpty(psMsgLista))
                        {
                            XtraMessageBox.Show(psMsgLista, "No es posible importar datos!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        string psMsg = lsValidaDuplicados(poLista, poListaImportada);
                        if (!string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(psMsg, "No es posible importar datos duplicados!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        poLista.AddRange(poListaImportada);

                        bsDatos.DataSource = poLista;
                        dgvParametrizaciones.BestFitColumns();
                        XtraMessageBox.Show("Importado Exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Evento del botón Generar, Genera Novedad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                dgvParametrizaciones.PostEditor();
                string psMsgValida = lsEsValido();
                
                List<PresupuestoRebatePivotGridGC> poLista = (List<PresupuestoRebatePivotGridGC>)bsDatos.DataSource;

                if (poLista.Count > 0)
                {
                    string psMsg = loLogicaNegocio.gsGuardarGC(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
        /// Exportar datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvParametrizaciones.PostEditor();
                var poLista = (List<PresupuestoRebatePivotGridGC>)bsDatos.DataSource;
                if (poLista.Count > 0)
                {
                    clsComun.gSaveFile(gcParametrizaciones, Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");
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

        /// <summary>
        /// Evento del boton de eliminar en el Grid, elimina la fila seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDel_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvParametrizaciones.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<PresupuestoRebatePivotGridGC>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsDatos.DataSource = poLista;
                    dgvParametrizaciones.RefreshData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            //if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Click += btnImportar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            if (tstBotones.Items["btnPlantilla"] != null) tstBotones.Items["btnPlantilla"].Click += btnPlantilla_Click;
            //if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

            bsDatos = new BindingSource();
            bsDatos.DataSource = new List<PresupuestoRebatePivotGridGC>();
            gcParametrizaciones.DataSource = bsDatos;
            dgvParametrizaciones.Columns["IdPresupuestoRebateGC"].Visible = false;
            dgvParametrizaciones.Columns["CodeZona"].Visible = false;
            dgvParametrizaciones.Columns["IdGrupoCliente"].Visible = false;
            dgvParametrizaciones.Columns["IdPresupuestoRebateGC"].Visible = false;
            dgvParametrizaciones.OptionsBehavior.Editable = true;
            dgvParametrizaciones.Columns["Periodo"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["CodeZona"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["NameZona"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["IdGrupoCliente"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["NombreGrupo"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["Total"].OptionsColumn.AllowEdit = false;

            lColocarbotonDelete(dgvParametrizaciones.Columns["Del"]);
            clsComun.gDibujarBotonGrid(rpiBtnReport, dgvParametrizaciones.Columns["Carta"], "Carta", Diccionario.ButtonGridImage.printer_16x16);

            dgvParametrizaciones.Columns["Del"].Width = 40;
            dgvParametrizaciones.Columns["Carta"].Width = 40;

            for (int i = 0; i < dgvParametrizaciones.Columns.Count; i++)
            {

                if (dgvParametrizaciones.Columns[i].ColumnType == typeof(decimal))
                {
                    var psNameColumn = dgvParametrizaciones.Columns[i].FieldName;

                    //dgvParametrizaciones.Columns[i].UnboundType = UnboundColumnType.Decimal;
                    dgvParametrizaciones.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    dgvParametrizaciones.Columns[i].DisplayFormat.FormatString = "c2";
                    dgvParametrizaciones.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");

                    GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                    item1.FieldName = psNameColumn;
                    item1.SummaryType = SummaryItemType.Sum;
                    item1.DisplayFormat = "{0:c2}";
                    item1.ShowInGroupColumnFooter = dgvParametrizaciones.Columns[psNameColumn];
                    dgvParametrizaciones.GroupSummary.Add(item1);
                    
                }

            }

            txtAnio.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAnio.KeyPress += new KeyPressEventHandler(SoloNumeros);

            txtObservacion.KeyDown += new KeyEventHandler(EnterEqualTab);

            txtTrimestre1.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtTrimestre1.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtTrimestre2.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtTrimestre2.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtTrimestre3.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtTrimestre3.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtTrimestre4.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtTrimestre4.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
        }

        private string lsEsValido(bool tbGenerar = true)
        {

            string psMsg = string.Empty;

            if (string.IsNullOrEmpty(txtAnio.Text))
            {
                psMsg = string.Format("{0}Ingrese el Periodo. \n", psMsg);
            }
            if (cmbCliente.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Cliente. \n", psMsg);
            }
            if (cmbZona.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Zona. \n", psMsg);
            }
            //if (cmbVendedor.EditValue.ToString() == Diccionario.Seleccione)
            //{
            //    psMsg = string.Format("{0}Seleccione Vendedor. \n", psMsg);
            //}
            
            if (string.IsNullOrEmpty(txtTrimestre1.Text)) txtTrimestre1.Text = "0";
            if (string.IsNullOrEmpty(txtTrimestre2.Text)) txtTrimestre2.Text = "0";
            if (string.IsNullOrEmpty(txtTrimestre3.Text)) txtTrimestre3.Text = "0";
            if (string.IsNullOrEmpty(txtTrimestre4.Text)) txtTrimestre4.Text = "0";

            return psMsg;
        }

        private void lLimpiar()
        {
            if ((cmbCliente.Properties.DataSource as IList).Count > 0) cmbCliente.ItemIndex = 0;
            //if ((cmbVendedor.Properties.DataSource as IList).Count > 0) cmbVendedor.ItemIndex = 0;
            if ((cmbZona.Properties.DataSource as IList).Count > 0) cmbZona.ItemIndex = 0;
            txtTrimestre1.Text = "0";
            txtTrimestre2.Text = "0";
            txtTrimestre3.Text = "0";
            txtTrimestre4.Text = "0";
            txtObservacion.Text = string.Empty;
            txtAnio.Text = string.Empty;
            bsDatos.DataSource = new List<PresupuestoRebatePivotGridGC>();
            gcParametrizaciones.DataSource = bsDatos;
        }

        private void lBuscar()
        {

            if (!string.IsNullOrEmpty(txtAnio.Text))
            {
                int piPeriodo = int.Parse(txtAnio.Text);

                var poLIsta = loLogicaNegocio.goConsultarParametrizacionesGC(piPeriodo);
                bsDatos.DataSource = poLIsta;
                gcParametrizaciones.DataSource = poLIsta;
                clsComun.gOrdenarColumnasGrid(dgvParametrizaciones);
            }
            else
            {
                var poLIsta = new List<PresupuestoRebatePivotGridGC>();
                bsDatos.DataSource = poLIsta;
                gcParametrizaciones.DataSource = poLIsta;
            }
           
            
        }

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {

                string psMsgValida = lsEsValido();
                if (string.IsNullOrEmpty(psMsgValida))
                {
                    
                    var poLista = (List<PresupuestoRebatePivotGridGC>)bsDatos.DataSource;
                    var poListaAgregar = new List<PresupuestoRebatePivotGridGC>();
                    PresupuestoRebatePivotGridGC poItem = new PresupuestoRebatePivotGridGC();
                    poItem.Periodo = int.Parse(txtAnio.Text.Trim());
                    poItem.CodeZona = cmbZona.EditValue.ToString();
                    poItem.NameZona = cmbZona.Text;
                    poItem.IdGrupoCliente = int.Parse(cmbCliente.EditValue.ToString());
                    poItem.NombreGrupo = cmbCliente.Text;
                    poItem.Trimestre1 = Convert.ToDecimal(txtTrimestre1.Text);
                    poItem.Trimestre2 = Convert.ToDecimal(txtTrimestre2.Text);
                    poItem.Trimestre3 = Convert.ToDecimal(txtTrimestre3.Text);
                    poItem.Trimestre4 = Convert.ToDecimal(txtTrimestre4.Text);
                    poItem.Observacion = txtObservacion.Text.Trim();
                    poListaAgregar.Add(poItem);
                    string psMsg = lsValidaDuplicados(poLista, poListaAgregar);
                    if (!string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(psMsg, "No es posible agregar duplicados!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    poLista.Add(poItem);

                    bsDatos.DataSource = poLista;
                    dgvParametrizaciones.BestFitColumns();
                }
                else
                {
                    XtraMessageBox.Show(psMsgValida, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtAnio_Leave(object sender, EventArgs e)
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

        private string lsValidaDuplicados(List<PresupuestoRebatePivotGridGC> toListaGrid, List<PresupuestoRebatePivotGridGC> toListaInsertar)
        {
            string psMsg = string.Empty;

            foreach (var item in toListaInsertar)
            {
                var piRegistro = toListaGrid.Where(x => x.Periodo == item.Periodo && x.IdGrupoCliente == item.IdGrupoCliente && x.CodeZona == item.CodeZona).Count();
                if(piRegistro > 0)
                {
                    psMsg = string.Format("{0}Zona: {1} y Cliente: {2} Ya están parametrizados. \n", psMsg, item.NameZona, item.NombreGrupo);
                }
            }

            return psMsg;
        }

        /// <summary>
        /// Agregar boton delete a una Columna de un grid
        /// </summary>
        /// <param name="colXmlDown"></param>
        private void lColocarbotonDelete(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Eliminar";
            colXmlDown.ColumnEdit = rpiBtnDel;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnDel.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnDel.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/trash_16x16.png");
            rpiBtnDel.TextEditStyle = TextEditStyles.HideTextEditor;
            //  colXmlDown.Width = 50;


        }

        private void lImprimir()
        {
            int piIndex = dgvParametrizaciones.GetFocusedDataSourceRowIndex();
            var poLista = (List<PresupuestoRebatePivotGridGC>)bsDatos.DataSource;
            if (poLista[piIndex].IdPresupuestoRebateGC.ToString() != "")
            {

                var poCartaRebate = loLogicaNegocio.goCartaRebate(poLista[piIndex].IdGrupoCliente.ToString(), int.Parse(txtAnio.Text) - 1);

                xrptCartaRebateNew xrpt = new xrptCartaRebateNew();
                xrpt.Parameters["Fecha"].Value = string.Format("Durán, {0} de {1} del {2}", DateTime.Now.Day, clsComun.gsGetMes(DateTime.Now.Month).ToLower(), DateTime.Now.Year);
                xrpt.Parameters["ValorRebate"].Value = poCartaRebate.ValorRebate;
                xrpt.Parameters["Cliente"].Value = poLista[piIndex].NombreGrupo;
                xrpt.Parameters["Meta"].Value = poCartaRebate.Meta;
                xrpt.Parameters["Plazo"].Value = poCartaRebate.Plazo;
                xrpt.Parameters["PresenteAño"].Value = poCartaRebate.PresenteAño;
                xrpt.Parameters["SiguienteAño"].Value = poCartaRebate.SiguienteAño;
                xrpt.Parameters["Q1"].Value = poCartaRebate.Q1;
                xrpt.Parameters["Q2"].Value = poCartaRebate.Q2;
                xrpt.Parameters["Q3"].Value = poCartaRebate.Q3;
                xrpt.Parameters["Q4"].Value = poCartaRebate.Q4;
                xrpt.Parameters["Ventas"].Value = poCartaRebate.Ventas;

                xrpt.RequestParameters = false;
                xrpt.Parameters["ValorRebate"].Visible = false;
                xrpt.Parameters["Fecha"].Visible = false;
                xrpt.Parameters["Cliente"].Visible = false;
                xrpt.Parameters["Meta"].Visible = false;
                xrpt.Parameters["Plazo"].Visible = false;
                xrpt.Parameters["PresenteAño"].Visible = false;
                xrpt.Parameters["SiguienteAño"].Visible = false;
                xrpt.Parameters["Q1"].Visible = false;
                xrpt.Parameters["Q2"].Visible = false;
                xrpt.Parameters["Q3"].Visible = false;
                xrpt.Parameters["Q4"].Visible = false;
                xrpt.Parameters["Ventas"].Visible = false;

                using (ReportPrintTool printTool = new ReportPrintTool(xrpt))
                {
                    printTool.ShowRibbonPreviewDialog();
                }

            }
            else
            {
                XtraMessageBox.Show("No existe detalles guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
