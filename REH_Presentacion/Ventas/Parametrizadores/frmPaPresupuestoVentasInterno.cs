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
    public partial class frmPaPresupuestoVentasInterno : frmBaseTrxDev
    {

        #region Variables
        clsNPresupuestoVentas loLogicaNegocio;
        BindingSource bsDatos;
        private bool lbCargado;
        RepositoryItemButtonEdit rpiBtnDel;
        List<Combo> loComboZona;
        List<Combo> loComboProducto;


        #endregion

        public frmPaPresupuestoVentasInterno()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNPresupuestoVentas();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        private void frmPaPresupuestoVentasRebate_Load(object sender, EventArgs e)
        {
            try
            {
                lbCargado = false;
                loComboZona = loLogicaNegocio.goConsultarZonasSAAF();
                loComboProducto = loLogicaNegocio.goSapConsultaItems();
                clsComun.gLLenarCombo(ref cmbProducto, loComboProducto, true);
                clsComun.gLLenarCombo(ref cmbZona, loComboZona, true);
                lbCargado = true;
                lCargarEventosBotones();
                lBuscar();
                txtAnio.Focus();

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
                    bs.DataSource = new List<PresupuestoVentasExcel>();
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

                clsComun.gsMensajePrevioImportar();

                if (ofdRuta.ShowDialog() == DialogResult.OK)
                {
                    if (!ofdRuta.FileName.Equals(""))
                    {
                        DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);
                        var poLista = (List<PresupuestoVentasGrid>)bsDatos.DataSource;
                        var poListaImportada = new List<PresupuestoVentasGrid>();

                        List<string> psListaMsg = new List<string>();

                        var poComboZona = new List<Combo>();
                        foreach (var item in loComboZona)
                        {
                            if (item.Descripcion.Contains("-"))
                            {
                                poComboZona.Add(new Combo() { Codigo = item.Codigo, Descripcion = item.Descripcion.Substring(5, item.Descripcion.Length-5)});
                            }
                            else
                            {
                                poComboZona.Add(new Combo() { Codigo = item.Codigo, Descripcion = item.Descripcion });
                            }
                            
                        } 

                        int fila = 2;
                        string psMsgLista = string.Empty;
                        foreach (DataRow item in dt.Rows)
                        {
                            if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                            {
                                string psMsgFila = "";
                                string psMsgOut = "";

                                //try
                                //{

                                PresupuestoVentasGrid poItem = new PresupuestoVentasGrid();
                                poItem.Periodo = clsComun.gdValidarRegistro("Periodo","i",item[0].ToString().Trim(),fila, true, ref psMsgOut);
                                poItem.Mes = clsComun.gdValidarRegistro("Mes", "i", item[1].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.Zona = clsComun.gdValidarRegistro("Zona", "s", item[2].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.ItemCode = clsComun.gdValidarCodigo(loComboProducto,"Producto", fila, item[3].ToString().Trim(), item[4].ToString().Trim(), ref psMsgOut);
                                poItem.ItemName = clsComun.gdValidarRegistro("Item Name", "s", item[4].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.Unidades = clsComun.gdValidarRegistro("Unidades", "i", item[5].ToString().Trim(), fila, true, ref psMsgOut);
                                poItem.PrecioReferencial = clsComun.gdValidarRegistro("Precio", "d", item[6].ToString().Trim(), fila, false, ref psMsgOut);
                                poItem.MedidaConversion = clsComun.gdValidarRegistro("Medida Conversión", "d", item[8].ToString().Trim(), fila, true, ref psMsgOut); 
                                poItem.TipoProducto = clsComun.gdValidarRegistro("Tipo Producto", "s", item[10].ToString().Trim(), fila, false, ref psMsgOut);
                                var pCode = clsComun.gdValidarRegistro(poComboZona, "Zona", "i", poItem.Zona, fila, true, ref psMsgOut);
                                if (pCode != null)
                                {
                                    poItem.IdZona = pCode;
                                }
                               

                                //string psCodeZona = loComboZona.Where(x => x.Descripcion == poItem.Zona).Select(x => x.Codigo).FirstOrDefault();
                                //if (!string.IsNullOrEmpty(psCodeZona))
                                //{
                                //    poItem.IdZona = int.Parse(psCodeZona);
                                //}
                                //else
                                //{
                                //    psMsgLista = string.Format("{0}No se encontró Zona con el nombre: '{1}'  en la fila {2}.\n", psMsgLista, poItem.Zona, fila);
                                //}

                                if (!string.IsNullOrEmpty(poItem.ItemCode))
                                {
                                    DataTable dtAtrItem = loLogicaNegocio.gdtSapConsultaAtributosItem(poItem.ItemCode);

                                    if (dtAtrItem != null && dtAtrItem.Rows.Count > 0)
                                    {
                                        poItem.Familia = dtAtrItem.Rows[0]["Familia"].ToString().Trim();
                                        poItem.ItmsGrpCod = short.Parse(dtAtrItem.Rows[0]["GroupCode"].ToString().Trim());
                                        poItem.Grupo = dtAtrItem.Rows[0]["GroupName"].ToString().Trim();
                                    }
                                    else
                                    {
                                        psMsgOut = string.Format("{0}No se encontró Familia/Grupo del Ítem: '{1}'  en la fila {2}.\n", psMsgOut, poItem.ItemCode, fila);
                                    }

                                    
                                }


                                fila++;
                                
                                if (string.IsNullOrEmpty(psMsgOut))
                                {
                                    poListaImportada.Add(poItem);
                                }
                                else
                                {
                                    psMsgLista = psMsgLista + psMsgOut;
                                }

                                //}
                                //catch (Exception ex)
                                //{

                                //    throw;
                                //}
                            }
                        }
                        if (!string.IsNullOrEmpty(psMsgLista))
                        {
                            psListaMsg.Add(psMsgLista);
                        }
                        

                        //if (!string.IsNullOrEmpty(psMsgLista))
                        //{
                        //    XtraMessageBox.Show(psMsgLista, "No es posible importar datos!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //    return;
                        //}

                        if (psListaMsg.Count > 0)
                        {
                            XtraMessageBox.Show("Se emitirá un archivo de errores", "No es posible importar datos!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clsComun.gsGuardaLogTxt(psListaMsg);
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
                
                List<PresupuestoVentasGrid> poLista = (List<PresupuestoVentasGrid>)bsDatos.DataSource;

                if (poLista.Count > 0)
                {
                    string psMsg = loLogicaNegocio.gsGuardarProductoInterno(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                var poLista = (List<PresupuestoVentasGrid>)bsDatos.DataSource;
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
                var poLista = (List<PresupuestoVentasGrid>)bsDatos.DataSource;

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
            bsDatos.DataSource = new List<PresupuestoVentasGrid>();
            gcParametrizaciones.DataSource = bsDatos;
            dgvParametrizaciones.Columns["IdPresupuestoVentas"].Visible = false;
            dgvParametrizaciones.Columns["Observacion"].Visible = false;
            dgvParametrizaciones.Columns["ItmsGrpCod"].Visible = false;
            
            dgvParametrizaciones.Columns["IdZona"].Visible = false;
            dgvParametrizaciones.OptionsBehavior.Editable = true;
            dgvParametrizaciones.Columns["Periodo"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["Mes"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["IdZona"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["Zona"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["Familia"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["ItemCode"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["ItemName"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["ItmsGrpCod"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["Grupo"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["Total"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["Valor"].OptionsColumn.AllowEdit = false;

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
            txtTipoProducto.KeyDown += new KeyEventHandler(EnterEqualTab);


            txtAnio.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAnio.KeyPress += new KeyPressEventHandler(SoloNumeros);

            txtMes.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtMes.KeyPress += new KeyPressEventHandler(SoloNumeros);

            txtAnio.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAnio.KeyPress += new KeyPressEventHandler(SoloNumeros);

            txtTipoProducto.KeyDown += new KeyEventHandler(EnterEqualTab);

            txtUnidades.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtUnidades.KeyPress += new KeyPressEventHandler(SoloNumeros);

            txtPrecio.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtPrecio.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            txtMedidaConversion.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtMedidaConversion.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);


        }

        private string lsEsValido(bool tbGenerar = true)
        {

            string psMsg = string.Empty;

            if (string.IsNullOrEmpty(txtAnio.Text))
            {
                psMsg = string.Format("{0}Ingrese el Periodo. \n", psMsg);
            }
            if (string.IsNullOrEmpty(txtMes.Text))
            {
                psMsg = string.Format("{0}Ingrese el Mes. \n", psMsg);
            }
            if (cmbZona.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Zona. \n", psMsg);
            }
            if (cmbProducto.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Producto. \n", psMsg);
            }
            
            if (string.IsNullOrEmpty(txtUnidades.Text)) txtUnidades.Text = "0";
            if (string.IsNullOrEmpty(txtPrecio.Text)) txtPrecio.Text = "0";
            if (string.IsNullOrEmpty(txtValor.Text)) txtValor.Text = "0";
            if (string.IsNullOrEmpty(txtMedidaConversion.Text)) txtMedidaConversion.Text = "0";
            if (string.IsNullOrEmpty(txtTotal.Text)) txtTotal.Text = "0";
           
            return psMsg;
        }

        private void lLimpiar()
        {
            if ((cmbProducto.Properties.DataSource as IList).Count > 0) cmbProducto.ItemIndex = 0;
            if ((cmbZona.Properties.DataSource as IList).Count > 0) cmbZona.ItemIndex = 0;
            txtUnidades.Text = "0";
            txtPrecio.Text = "0";
            txtValor.Text = "0";
            txtMedidaConversion.Text = "0";
            txtTotal.Text = "0";
            txtTipoProducto.Text = string.Empty;
            txtAnio.Text = string.Empty;
            bsDatos.DataSource = new List<PresupuestoVentasGrid>();
        }

        private void lBuscar()
        {

            if (!string.IsNullOrEmpty(txtAnio.Text))
            {
                int piPeriodo = int.Parse(txtAnio.Text);

                var poLIsta = loLogicaNegocio.goConsultarParametrizacionesInterno(piPeriodo);
                bsDatos.DataSource = poLIsta;
                gcParametrizaciones.DataSource = bsDatos;
            }
            else
            {
                var poLIsta = new List<PresupuestoVentasGrid>();
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
                    
                    var poLista = (List<PresupuestoVentasGrid>)bsDatos.DataSource;
                    var poListaAgregar = new List<PresupuestoVentasGrid>();
                    PresupuestoVentasGrid poItem = new PresupuestoVentasGrid();
                    poItem.Periodo = int.Parse(txtAnio.Text.Trim());
                    poItem.Mes = int.Parse(txtMes.Text.Trim());
                    //poItem.CodeVendedor = int.Parse(cmbFamilia.EditValue.ToString());
                    //poItem.NameVendedor = cmbFamilia.Text;
                    poItem.ItemCode = cmbProducto.EditValue.ToString();
                    poItem.ItemName = cmbProducto.Text;
                    poItem.TipoProducto = txtTipoProducto.Text;
                    poItem.IdZona = int.Parse(cmbZona.EditValue.ToString());
                    poItem.Zona = cmbZona.Text;


                    DataTable dtAtrItem = loLogicaNegocio.gdtSapConsultaAtributosItem(cmbProducto.EditValue.ToString());

                    poItem.Familia = dtAtrItem.Rows[0]["Familia"].ToString().Trim();
                    poItem.ItmsGrpCod = short.Parse(dtAtrItem.Rows[0]["GroupCode"].ToString().Trim());
                    poItem.Grupo = dtAtrItem.Rows[0]["GroupName"].ToString().Trim();

                    poItem.Unidades = int.Parse(txtUnidades.Text);
                    poItem.PrecioReferencial = Convert.ToDecimal(txtPrecio.Text);
                    poItem.MedidaConversion = Convert.ToDecimal(txtMedidaConversion.Text);
                    poItem.TipoProducto = txtTipoProducto.Text.Trim();

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
                    XtraMessageBox.Show("Agregado Exitosamente, Presione el botón Guardar para completar el registro", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        private string lsValidaDuplicados(List<PresupuestoVentasGrid> toListaGrid, List<PresupuestoVentasGrid> toListaInsertar)
        {
            string psMsg = string.Empty;

            foreach (var item in toListaInsertar)
            {
                var piRegistro = toListaGrid.Where(x => x.Periodo == item.Periodo && x.Mes == item.Mes && x.ItemCode == item.ItemCode && x.IdZona == item.IdZona).Count();
                if(piRegistro > 0)
                {
                    psMsg = string.Format("{0}Periodo-Mes: {1}-{2} con la zona: {3} y Item: {4} Ya están parametrizados. \n", psMsg, item.Periodo, item.Mes, item.Zona, item.ItemName);
                }
            }

            return psMsg;
        }

        private void lCalcular()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtUnidades.Text))
                {
                    if (!string.IsNullOrEmpty(txtPrecio.Text))
                    {
                        var pdcValor = Convert.ToDecimal(txtUnidades.Text) * Convert.ToDecimal(txtPrecio.Text);
                        txtValor.Text = pdcValor.ToString();
                    }
                    else
                    {
                        txtValor.Text = "0";
                    }

                    if (!string.IsNullOrEmpty(txtMedidaConversion.Text))
                    {
                        var pdcValor = Convert.ToDecimal(txtUnidades.Text) * Convert.ToDecimal(txtMedidaConversion.Text);
                        txtTotal.Text = pdcValor.ToString();
                    }
                    else
                    {
                        txtTotal.Text = "0";
                    }

                }
                else
                {
                    txtValor.Text = "0";
                    txtTotal.Text = "0";
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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

        private void txtUnidades_EditValueChanged(object sender, EventArgs e)
        {
            lCalcular();
        }

        private void btnSeleccionarTodos_Click(object sender, EventArgs e)
        {
            try
            {
                List<PresupuestoVentasGrid> poLista = (List<PresupuestoVentasGrid>)bsDatos.DataSource;

                var asd = dgvParametrizaciones.ActiveFilterString;

                //var a = (List<PresupuestoVentasGrid>)dgvParametrizaciones.DataSource;
                //var b = (List<PresupuestoVentasGrid>)gcParametrizaciones.DataSource;
                
                if (!string.IsNullOrEmpty(txtMes.Text))
                {
                    var sel = poLista.Select(x => x.Sel).FirstOrDefault();
                    foreach (var item in poLista.Where(x => x.Mes == int.Parse(txtMes.Text)).ToList())
                    {
                        item.Sel = !sel;
                    }
                }
                else
                {
                    var sel = poLista.Select(x => x.Sel).FirstOrDefault();
                    foreach (var item in poLista)
                    {
                        item.Sel = !sel;
                    }
                }
                
                bsDatos.DataSource = poLista;
                dgvParametrizaciones.RefreshData();
                

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarSeleccionados_Click(object sender, EventArgs e)
        {
            try
            {
                var poLista = (List<PresupuestoVentasGrid>)bsDatos.DataSource;
                foreach (PresupuestoVentasGrid item in poLista.Where(x => x.Sel).ToList())
                {
                    var po = poLista.Remove(item);
                }
                bsDatos.DataSource = poLista;
                dgvParametrizaciones.RefreshData();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
