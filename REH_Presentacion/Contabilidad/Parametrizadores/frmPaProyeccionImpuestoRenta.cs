using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
using REH_Negocio;
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

namespace REH_Presentacion.Contabilidad.Parametrizadores
{
    public partial class frmPaProyeccionImpuestoRenta : frmBaseTrxDev
    {

        #region Variables
        clsNImpuestoRenta loLogicaNegocio;
        BindingSource bsDatos;
        private bool lbCargado;
        RepositoryItemButtonEdit rpiBtnDel;
        
        List<Combo> loCombo;
        List<Persona> loListaPersona;


        #endregion

        public frmPaProyeccionImpuestoRenta()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNImpuestoRenta();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            loListaPersona = new clsNEmpleado().goListar();
        }

        private void frmPaPresupuestoVentasRebate_Load(object sender, EventArgs e)
        {
            try
            {
                lbCargado = false;
                loCombo = loLogicaNegocio.goConsultarComboEmpleado();
                clsComun.gLLenarCombo(ref cmbEmpleado, loCombo, true);
                //clsComun.gLLenarCombo(ref cmbZona, loComboZona, true);
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
                    bs.DataSource = new List<ProyeccionImpuestoRentaExcel>();
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
                if (!string.IsNullOrEmpty(txtAnio.Text))
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
                            var poLista = (List<ProyeccionImpuestoRentaGrid>)bsDatos.DataSource;
                            var poListaImportada = new List<ProyeccionImpuestoRentaGrid>();

                            List<string> psListaMsg = new List<string>();

                            var polistaIdPersona = loListaPersona.Select(x => new Combo() { Codigo = x.IdPersona.ToString(), Descripcion = x.NumeroIdentificacion }).ToList();
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

                                    ProyeccionImpuestoRentaGrid poItem = new ProyeccionImpuestoRentaGrid();
                                    poItem.Periodo = int.Parse(txtAnio.EditValue.ToString()); //clsComun.gdValidarRegistro("Periodo", "i", item[0].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.NumeroIdentificacion = clsComun.gdValidarRegistro("Identificacion", "s", item[0].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.NombreCompleto = clsComun.gdValidarRegistro("Empleado", "s", item[1].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.IdPersona = clsComun.gdValidarRegistro(polistaIdPersona, "Identificacion", "i", poItem.NumeroIdentificacion, fila, true, ref psMsgOut);
                                    poItem.Enero = clsComun.gdValidarRegistro("Enero", "d", item[2].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.Febrero = clsComun.gdValidarRegistro("Febrero", "d", item[3].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.Marzo = clsComun.gdValidarRegistro("Marzo", "d", item[4].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.Abril = clsComun.gdValidarRegistro("Abril", "d", item[5].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.Mayo = clsComun.gdValidarRegistro("Mayo", "d", item[6].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.Junio = clsComun.gdValidarRegistro("Junio", "d", item[7].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.Julio = clsComun.gdValidarRegistro("Julio", "d", item[8].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.Agosto = clsComun.gdValidarRegistro("Agosto", "d", item[9].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.Septiembre = clsComun.gdValidarRegistro("Septiembre", "d", item[10].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.Octubre = clsComun.gdValidarRegistro("Octubre", "d", item[11].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.Noviembre = clsComun.gdValidarRegistro("Noviembre", "d", item[12].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.Diciembre = clsComun.gdValidarRegistro("Diciembre", "d", item[13].ToString().Trim(), fila, true, ref psMsgOut);
                                    poItem.Utilidades = clsComun.gdValidarRegistro("Diciembre", "d", item[14].ToString().Trim(), fila, true, ref psMsgOut);


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

                            bsDatos.DataSource = poLista.ToList();
                            dgvParametrizaciones.BestFitColumns();
                            XtraMessageBox.Show("Importado Exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("Ingrese el Periodo", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                
                List<ProyeccionImpuestoRentaGrid> poLista = (List<ProyeccionImpuestoRentaGrid>)bsDatos.DataSource;

                if (poLista.Count > 0)
                {
                    string psMsg = loLogicaNegocio.gsGuardarProyeccionIR(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                var poLista = (List<ProyeccionImpuestoRentaGrid>)bsDatos.DataSource;
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
                var poLista = (List<ProyeccionImpuestoRentaGrid>)bsDatos.DataSource;

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
            bsDatos.DataSource = new List<ProyeccionImpuestoRentaGrid>();
            gcParametrizaciones.DataSource = bsDatos;
            dgvParametrizaciones.Columns["IdProyeccionImpuestoRenta"].Visible = false;
            dgvParametrizaciones.Columns["NumeroIdentificacion"].Visible = false;
            dgvParametrizaciones.Columns["IdPersona"].Visible = false;
            
            dgvParametrizaciones.OptionsBehavior.Editable = true;
            dgvParametrizaciones.Columns["NombreCompleto"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["Periodo"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["Total"].OptionsColumn.AllowEdit = false;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvParametrizaciones.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);
           
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

            cmbEmpleado.KeyDown += new KeyEventHandler(EnterEqualTab);
            
            txtAnio.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAnio.KeyPress += new KeyPressEventHandler(SoloNumeros);
            
            txtEnero.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtEnero.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtEnero.Properties.Mask.EditMask = "c2";
            txtEnero.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtEnero.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtFebrero.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtFebrero.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtFebrero.Properties.Mask.EditMask = "c2";
            txtFebrero.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtFebrero.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtMarzo.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtMarzo.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtMarzo.Properties.Mask.EditMask = "c2";
            txtMarzo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtMarzo.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtAbril.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAbril.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtAbril.Properties.Mask.EditMask = "c2";
            txtAbril.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAbril.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtMayo.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtMayo.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtMayo.Properties.Mask.EditMask = "c2";
            txtMayo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtMayo.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtJunio.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtJunio.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtJunio.Properties.Mask.EditMask = "c2";
            txtJunio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtJunio.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtJulio.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtJulio.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtJulio.Properties.Mask.EditMask = "c2";
            txtJulio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtJulio.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtAgosto.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAgosto.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtAgosto.Properties.Mask.EditMask = "c2";
            txtAgosto.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtAgosto.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtSeptiembre.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtSeptiembre.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtSeptiembre.Properties.Mask.EditMask = "c2";
            txtSeptiembre.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtSeptiembre.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtOctubre.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtOctubre.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtOctubre.Properties.Mask.EditMask = "c2";
            txtOctubre.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtOctubre.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtNoviembre.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtNoviembre.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtNoviembre.Properties.Mask.EditMask = "c2";
            txtNoviembre.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtNoviembre.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtDiciembre.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtDiciembre.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtDiciembre.Properties.Mask.EditMask = "c2";
            txtDiciembre.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtDiciembre.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtUtilidades.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtUtilidades.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtUtilidades.Properties.Mask.EditMask = "c2";
            txtUtilidades.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtUtilidades.Properties.Mask.UseMaskAsDisplayFormat = true;
        }

        private string lsEsValido(bool tbGenerar = true)
        {

            string psMsg = string.Empty;

            if (string.IsNullOrEmpty(txtAnio.Text))
            {
                psMsg = string.Format("{0}Ingrese el Periodo. \n", psMsg);
            }
            //if (string.IsNullOrEmpty(txtMes.Text))
            //{
            //    psMsg = string.Format("{0}Ingrese el Mes. \n", psMsg);
            //}
            //if (cmbZona.EditValue.ToString() == Diccionario.Seleccione)
            //{
            //    psMsg = string.Format("{0}Seleccione Zona. \n", psMsg);
            //}
            if (cmbEmpleado.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Empleado. \n", psMsg);
            }
            
            if (string.IsNullOrEmpty(txtEnero.Text)) txtEnero.Text = "0";
            if (string.IsNullOrEmpty(txtFebrero.Text)) txtFebrero.Text = "0";
            if (string.IsNullOrEmpty(txtMarzo.Text)) txtMarzo.Text = "0";
            if (string.IsNullOrEmpty(txtAbril.Text)) txtAbril.Text = "0";
            if (string.IsNullOrEmpty(txtMayo.Text)) txtMayo.Text = "0";
            if (string.IsNullOrEmpty(txtJunio.Text)) txtJunio.Text = "0";
            if (string.IsNullOrEmpty(txtJulio.Text)) txtJulio.Text = "0";
            if (string.IsNullOrEmpty(txtAgosto.Text)) txtAgosto.Text = "0";
            if (string.IsNullOrEmpty(txtSeptiembre.Text)) txtSeptiembre.Text = "0";
            if (string.IsNullOrEmpty(txtOctubre.Text)) txtOctubre.Text = "0";
            if (string.IsNullOrEmpty(txtNoviembre.Text)) txtNoviembre.Text = "0";
            if (string.IsNullOrEmpty(txtDiciembre.Text)) txtDiciembre.Text = "0";

            return psMsg;
        }

        private void lLimpiar()
        {
            if ((cmbEmpleado.Properties.DataSource as IList).Count > 0) cmbEmpleado.ItemIndex = 0;
            txtEnero.Text = "0";
            txtFebrero.Text = "0";
            txtMarzo.Text = "0";
            txtAbril.Text = "0";
            txtMayo.Text = "0";
            txtJunio.Text = "0";
            txtJulio.Text = "0";
            txtAgosto.Text = "0";
            txtSeptiembre.Text = "0";
            txtOctubre.Text = "0";
            txtNoviembre.Text = "0";
            txtDiciembre.Text = "0";
            txtAnio.Text = "";
            bsDatos.DataSource = new List<ProyeccionImpuestoRentaGrid>();
        }

        private void lBuscar()
        {

            if (!string.IsNullOrEmpty(txtAnio.Text))
            {
                int piPeriodo = int.Parse(txtAnio.Text);

                var poLIsta = loLogicaNegocio.goConsultarProyeccionIR(piPeriodo);
                bsDatos.DataSource = poLIsta;
                gcParametrizaciones.DataSource = bsDatos;
            }
            else
            {
                var poLIsta = new List<ProyeccionImpuestoRentaGrid>();
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
                    
                    var poLista = (List<ProyeccionImpuestoRentaGrid>)bsDatos.DataSource;
                    var poListaAgregar = new List<ProyeccionImpuestoRentaGrid>();
                    ProyeccionImpuestoRentaGrid poItem = new ProyeccionImpuestoRentaGrid();
                    poItem.Periodo = int.Parse(txtAnio.Text.Trim());
                    //poItem.Mes = int.Parse(txtMes.Text.Trim());
                    //poItem.CodeVendedor = int.Parse(cmbFamilia.EditValue.ToString());
                    //poItem.NameVendedor = cmbFamilia.Text;
                    poItem.IdPersona = int.Parse(cmbEmpleado.EditValue.ToString());
                    poItem.NombreCompleto = cmbEmpleado.Text;
                    poItem.NumeroIdentificacion = loListaPersona.Where(x => x.IdPersona == poItem.IdPersona).Select(x => x.NumeroIdentificacion).FirstOrDefault();
                    //poItem.TipoProducto = txtTipoProducto.Text;
                    //poItem.IdZona = int.Parse(cmbEmpleado.EditValue.ToString());
                    //poItem.Zona = cmbZona.Text;

                    poItem.Enero = Decimal.Parse(txtEnero.EditValue.ToString());
                    poItem.Febrero = Decimal.Parse(txtFebrero.EditValue.ToString());
                    poItem.Marzo = Decimal.Parse(txtMarzo.EditValue.ToString());
                    poItem.Abril = Decimal.Parse(txtAbril.EditValue.ToString());
                    poItem.Mayo = Decimal.Parse(txtMayo.EditValue.ToString());
                    poItem.Junio = Decimal.Parse(txtJunio.EditValue.ToString());
                    poItem.Julio = Decimal.Parse(txtJulio.EditValue.ToString());
                    poItem.Agosto = Decimal.Parse(txtAgosto.EditValue.ToString());
                    poItem.Septiembre = Decimal.Parse(txtSeptiembre.EditValue.ToString());
                    poItem.Octubre = Decimal.Parse(txtOctubre.EditValue.ToString());
                    poItem.Noviembre = Decimal.Parse(txtNoviembre.EditValue.ToString());
                    poItem.Diciembre = Decimal.Parse(txtDiciembre.EditValue.ToString());
                    poItem.Utilidades = Decimal.Parse(txtUtilidades.EditValue.ToString());

                    //poItem.TipoProducto = txtTipoProducto.Text.Trim();

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

        private string lsValidaDuplicados(List<ProyeccionImpuestoRentaGrid> toListaGrid, List<ProyeccionImpuestoRentaGrid> toListaInsertar)
        {
            string psMsg = string.Empty;

            foreach (var item in toListaInsertar)
            {
                var piRegistro = toListaGrid.Where(x => x.Periodo == item.Periodo && x.NumeroIdentificacion == item.NumeroIdentificacion).Count();
                if(piRegistro > 0)
                {
                    psMsg = string.Format("{0}Empleado: '{1}-{2}' en el periodo: {3}. Ya está parametrizado. \n", psMsg, item.NumeroIdentificacion, item.NombreCompleto, item.Periodo);
                }
            }

            return psMsg;
        }

        private void lCalcular()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtEnero.Text))
                {
                    if (!string.IsNullOrEmpty(txtFebrero.Text))
                    {
                        var pdcValor = Convert.ToDecimal(txtEnero.Text) * Convert.ToDecimal(txtFebrero.Text);
                        txtMarzo.Text = pdcValor.ToString();
                    }
                    else
                    {
                        txtMarzo.Text = "0";
                    }

                    if (!string.IsNullOrEmpty(txtAbril.Text))
                    {
                        var pdcValor = Convert.ToDecimal(txtEnero.Text) * Convert.ToDecimal(txtAbril.Text);
                        txtMayo.Text = pdcValor.ToString();
                    }
                    else
                    {
                        txtMayo.Text = "0";
                    }

                }
                else
                {
                    txtMarzo.Text = "0";
                    txtMayo.Text = "0";
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

}

        /// <summary>
        /// Agregar boton delete a una Columna de un grid
        ///// </summary>
        ///// <param name="colXmlDown"></param>
        //private void lColocarbotonDelete(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        //{
        //    colXmlDown.Caption = "Eliminar";
        //    colXmlDown.ColumnEdit = rpiBtnDel;
        //    colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
        //    colXmlDown.OptionsColumn.AllowSize = false;

        //    rpiBtnDel.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
        //    rpiBtnDel.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/trash_16x16.png");
        //    rpiBtnDel.TextEditStyle = TextEditStyles.HideTextEditor;
        //    //  colXmlDown.Width = 50;


        //}

        private void txtUnidades_EditValueChanged(object sender, EventArgs e)
        {
            lCalcular();
        }
    }
}
