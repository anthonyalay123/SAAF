using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
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
using VTA_Negocio;

namespace REH_Presentacion.Ventas.Parametrizadores
{
    public partial class frmPaPresupuestoCantidadesProducto : frmBaseTrxDev
    {

        #region Variables
        clsNKilosLitros loLogicaNegocio;
        BindingSource bsDatos;
        private bool lbCargado;
        RepositoryItemButtonEdit rpiBtnDel;
        List<Combo> loComboZona;
        List<Combo> loComboProducto;
        #endregion

        public frmPaPresupuestoCantidadesProducto()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNKilosLitros();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        private void frmPaPresupuestoVentasRebate_Load(object sender, EventArgs e)
        {
            try
            {
                lbCargado = false;
                loComboZona = loLogicaNegocio.goSapConsultaZonas();
                loComboProducto = loLogicaNegocio.goSapConsultaItems();
                clsComun.gLLenarCombo(ref cmbProducto, loComboProducto, true);
                clsComun.gLLenarCombo(ref cmbZona, loComboZona, true);
                lbCargado = true;
                lCargarEventosBotones();
                lBuscar();
                txtAnio.Focus();
                

                //lCargarEventosBotones();
                //lCargarGrid();
                //lCargarParametrizaciones();

                //if (!string.IsNullOrEmpty(lsCodigoRubro))
                //{
                //    cmbCentroCosto.EditValue = lsCodigoCentroCosto;
                //    cmbRubro.EditValue = lsCodigoRubro;
                //    lCargaCuentaContableRubroCentroCosto();
                //}
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
                if (lbCargado)
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
                    bs.DataSource = new List<PresupuestoCantidadesProductoExcel>();
                    dgv.BestFitColumns();
                    dgv.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
                    // Exportar Datos
                    clsComun.gSaveFile(gc, "Plantilla_" + Text + ".xlsx", "Files(*.xlsx;)|*.xlsx;");

                    gc.Visible = false;

                    

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
                        var poLista = (List<PresupuestoCantidadesProductoGrid>)bsDatos.DataSource;
                        var poListaImportada = new List<PresupuestoCantidadesProductoGrid>();

                        string psMsgImport = string.Empty;
                        int piContador = 1;
                        foreach (DataRow item in dt.Rows)
                        {

                            if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                            {
                                PresupuestoCantidadesProductoGrid poItem = new PresupuestoCantidadesProductoGrid();
                                poItem.Periodo = int.Parse(item[0].ToString().Trim());
                                poItem.CodeZona = item[1].ToString().Trim();
                                poItem.NameZona = item[2].ToString().Trim();
                                poItem.ItemCode = item[3].ToString().Trim();
                                poItem.ItemName = item[4].ToString().Trim();
                                poItem.Ene = Convert.ToDecimal(item[5].ToString().Trim());
                                poItem.Feb = Convert.ToDecimal(item[6].ToString().Trim());
                                poItem.Mar = Convert.ToDecimal(item[7].ToString().Trim());
                                poItem.Abr = Convert.ToDecimal(item[8].ToString().Trim());
                                poItem.May = Convert.ToDecimal(item[9].ToString().Trim());
                                poItem.Jun = Convert.ToDecimal(item[10].ToString().Trim());
                                poItem.Jul = Convert.ToDecimal(item[11].ToString().Trim());
                                poItem.Ago = Convert.ToDecimal(item[12].ToString().Trim());
                                poItem.Sep = Convert.ToDecimal(item[13].ToString().Trim());
                                poItem.Oct = Convert.ToDecimal(item[14].ToString().Trim());
                                poItem.Nov = Convert.ToDecimal(item[15].ToString().Trim());
                                poItem.Dic = Convert.ToDecimal(item[16].ToString().Trim());
                                poItem.Observacion = item[17].ToString().Trim();
                                piContador++;

                                if (loComboZona.Where(x=>x.Codigo == poItem.CodeZona).Count() > 0 && loComboProducto.Where(x => x.Codigo  == poItem.ItemCode).Count() > 0)
                                {
                                    poListaImportada.Add(poItem);
                                }
                                else
                                {

                                    if (loComboZona.Where(x => x.Codigo == poItem.CodeZona).Count() == 0)
                                    {
                                        psMsgImport = string.Format("{0}Fila #{1} Columna 'Codigo Zona' contiene un valor no encontrado en Base de Datos. Valor: '{2}'.\n", psMsgImport, piContador, poItem.CodeZona);
                                    }

                                    if (loComboProducto.Where(x => x.Codigo  == poItem.ItemCode).Count() == 0)
                                    {
                                        psMsgImport = string.Format("{0}Fila #{1} Columna 'Item' contiene un valor no encontradoen Base de Datos. Valor: '{2}'.\n", psMsgImport, piContador, poItem.CodeZona);
                                    }
                                }
                                
                            }

                        }

                        if (!string.IsNullOrEmpty(psMsgImport))
                        {
                            XtraMessageBox.Show(psMsgImport, "No se importaron los siguientes registros!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                
                List<PresupuestoCantidadesProductoGrid> poLista = (List<PresupuestoCantidadesProductoGrid>)bsDatos.DataSource;

                if (poLista.Count > 0)
                {
                    string psMsg = loLogicaNegocio.gsGuardarCantidadesProducto(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                var poLista = (List<PresupuestoCantidadesProductoGrid>)bsDatos.DataSource;
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
        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvParametrizaciones.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<PresupuestoCantidadesProductoGrid>)bsDatos.DataSource;

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
            bsDatos.DataSource = new List<PresupuestoCantidadesProductoGrid>();
            gcParametrizaciones.DataSource = bsDatos;
            dgvParametrizaciones.Columns["IdPresupuestoCantidadesProducto"].Visible = false;
            dgvParametrizaciones.OptionsBehavior.Editable = true;
            dgvParametrizaciones.Columns["Periodo"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["CodeZona"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["NameZona"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["ItemCode"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["ItemName"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["Total"].OptionsColumn.AllowEdit = false;

            lColocarbotonDelete(dgvParametrizaciones.Columns["Del"]);

            for (int i = 0; i < dgvParametrizaciones.Columns.Count; i++)
            {

                if (dgvParametrizaciones.Columns[i].ColumnType == typeof(decimal))
                {
                    var psNameColumn = dgvParametrizaciones.Columns[i].FieldName;

                    //dgvParametrizaciones.Columns[i].UnboundType = UnboundColumnType.Decimal;
                    dgvParametrizaciones.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    dgvParametrizaciones.Columns[i].DisplayFormat.FormatString = "n2";
                    dgvParametrizaciones.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:n2}");

                    GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                    item1.FieldName = psNameColumn;
                    item1.SummaryType = SummaryItemType.Sum;
                    item1.DisplayFormat = "{0:n2}";
                    item1.ShowInGroupColumnFooter = dgvParametrizaciones.Columns[psNameColumn];
                    dgvParametrizaciones.GroupSummary.Add(item1);
                    
                }

            }

            cmbProducto.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbZona.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtObservacion.KeyDown += new KeyEventHandler(EnterEqualTab);


            txtAnio.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAnio.KeyPress += new KeyPressEventHandler(SoloNumeros);

            txtEne.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtEne.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtFeb.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtFeb.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtMar.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtMar.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtAbr.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAbr.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtMay.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtMay.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtJun.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtJun.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtJul.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtJul.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtAgo.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAgo.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtSep.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtSep.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtOct.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtOct.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtNov.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtNov.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtDic.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtDic.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
        }

        private string lsEsValido(bool tbGenerar = true)
        {

            string psMsg = string.Empty;

            if (string.IsNullOrEmpty(txtAnio.Text))
            {
                psMsg = string.Format("{0}Ingrese el Periodo. \n", psMsg);
            }
            if (cmbZona.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Zona. \n", psMsg);
            }
            if (cmbProducto.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Producto. \n", psMsg);
            }
            
            if (string.IsNullOrEmpty(txtEne.Text)) txtEne.Text = "0";
            if (string.IsNullOrEmpty(txtFeb.Text)) txtFeb.Text = "0";
            if (string.IsNullOrEmpty(txtMar.Text)) txtMar.Text = "0";
            if (string.IsNullOrEmpty(txtAbr.Text)) txtAbr.Text = "0";
            if (string.IsNullOrEmpty(txtMay.Text)) txtMay.Text = "0";
            if (string.IsNullOrEmpty(txtJun.Text)) txtJun.Text = "0";
            if (string.IsNullOrEmpty(txtJul.Text)) txtJul.Text = "0";
            if (string.IsNullOrEmpty(txtAgo.Text)) txtAgo.Text = "0";
            if (string.IsNullOrEmpty(txtSep.Text)) txtSep.Text = "0";
            if (string.IsNullOrEmpty(txtOct.Text)) txtOct.Text = "0";
            if (string.IsNullOrEmpty(txtNov.Text)) txtNov.Text = "0";
            if (string.IsNullOrEmpty(txtDic.Text)) txtDic.Text = "0";
            return psMsg;
        }

        private void lLimpiar()
        {
            if ((cmbProducto.Properties.DataSource as IList).Count > 0) cmbProducto.ItemIndex = 0;
            if ((cmbZona.Properties.DataSource as IList).Count > 0) cmbZona.ItemIndex = 0;
            txtEne.Text = "0";
            txtFeb.Text = "0";
            txtMar.Text = "0";
            txtAbr.Text = "0";
            txtMay.Text = "0";
            txtJun.Text = "0";
            txtJul.Text = "0";
            txtAgo.Text = "0";
            txtSep.Text = "0";
            txtOct.Text = "0";
            txtNov.Text = "0";
            txtDic.Text = "0";
            txtObservacion.Text = string.Empty;
            txtAnio.Text = string.Empty;
            bsDatos.DataSource = new List<PresupuestoCantidadesProductoGrid>();
        }

        private void lBuscar()
        {

            if (!string.IsNullOrEmpty(txtAnio.Text))
            {
                int piPeriodo = int.Parse(txtAnio.Text);

                var poLIsta = loLogicaNegocio.goConsultarParametrizacionesCantidadesProducto(piPeriodo);
                bsDatos.DataSource = poLIsta;
                gcParametrizaciones.DataSource = poLIsta;
            }
            else
            {
                var poLIsta = new List<PresupuestoCantidadesProductoGrid>();
                bsDatos.DataSource = poLIsta;
                gcParametrizaciones.DataSource = poLIsta;
            }
           
            //dgvDatos.PopulateColumns();
            //dgvDatos.Columns[0].Visible = false; // IdPersona
            //dgvDatos.Columns[1].Width = 110; // Identificación
            //dgvDatos.Columns[2].Width = 200; // Nombre
            //dgvDatos.FixedLineWidth = 2;
            //dgvDatos.Columns[1].Fixed = FixedStyle.Left;
            //dgvDatos.Columns[2].Fixed = FixedStyle.Left;

            

            //List<string> psColumnas = new List<string>();

            //foreach (DataColumn column in dt.Columns)
            //{
            //    psColumnas.Add(column.ColumnName);
            //}

            //List<string> psRubrosNovedades = new List<string>();

            //foreach (string psItem in psColumnas)
            //{
            //    if (psItem.Contains("-"))
            //    {
            //        if (poRubros.Where(x => x.NovedadEditable && x.Codigo == psItem.Split('-')[0]).Count() == 0)
            //        {
            //            dgvDatos.Columns[psItem].OptionsColumn.AllowEdit = false;
            //        }
            //        dgvDatos.Columns[psItem].Caption = psItem.Substring(4);
            //    }
            //    else
            //    {
            //        dgvDatos.Columns[psItem].OptionsColumn.AllowEdit = false;
            //    }
            //}
            //// Habilitar edición para columna de OBSERVACIONES que debe estar al final
            //dgvDatos.Columns[psColumnas.Count - 1].OptionsColumn.AllowEdit = true;


            //dgvDatos.BestFitColumns();
        }

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {

                string psMsgValida = lsEsValido();
                if (string.IsNullOrEmpty(psMsgValida))
                {
                    
                    var poLista = (List<PresupuestoCantidadesProductoGrid>)bsDatos.DataSource;
                    var poListaAgregar = new List<PresupuestoCantidadesProductoGrid>();
                    PresupuestoCantidadesProductoGrid poItem = new PresupuestoCantidadesProductoGrid();
                    poItem.Periodo = int.Parse(txtAnio.Text.Trim());
                    //poItem.CodeVendedor = int.Parse(cmbFamilia.EditValue.ToString());
                    //poItem.NameVendedor = cmbFamilia.Text;
                    poItem.CodeZona = cmbZona.EditValue.ToString();
                    poItem.NameZona = cmbZona.Text;
                    poItem.ItemCode = cmbProducto.EditValue.ToString();
                    poItem.ItemName = cmbProducto.Text;
                    poItem.Ene = Convert.ToDecimal(txtEne.Text);
                    poItem.Feb = Convert.ToDecimal(txtFeb.Text);
                    poItem.Mar = Convert.ToDecimal(txtMar.Text);
                    poItem.Abr = Convert.ToDecimal(txtAbr.Text);
                    poItem.May = Convert.ToDecimal(txtMay.Text);
                    poItem.Jun = Convert.ToDecimal(txtJun.Text);
                    poItem.Jul = Convert.ToDecimal(txtJul.Text);
                    poItem.Ago = Convert.ToDecimal(txtAgo.Text);
                    poItem.Sep = Convert.ToDecimal(txtSep.Text);
                    poItem.Oct = Convert.ToDecimal(txtOct.Text);
                    poItem.Nov = Convert.ToDecimal(txtNov.Text);
                    poItem.Dic = Convert.ToDecimal(txtDic.Text);
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

        private string lsValidaDuplicados(List<PresupuestoCantidadesProductoGrid> toListaGrid, List<PresupuestoCantidadesProductoGrid> toListaInsertar)
        {
            string psMsg = string.Empty;

            foreach (var item in toListaInsertar)
            {
                var piRegistro = toListaGrid.Where(x => x.Periodo == item.Periodo && x.ItemCode == item.ItemCode && x.CodeZona == item.CodeZona).Count();
                if(piRegistro > 0)
                {
                    psMsg = string.Format("{0}Periodo: {1} con la zona: {2} y Item: {3} Ya están parametrizados. \n", psMsg, item.Periodo, item.NameZona, item.ItemName);
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
    }
}
