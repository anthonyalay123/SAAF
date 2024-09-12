using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades;
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
using VTA_Negocio;

namespace REH_Presentacion.Contabilidad.Parametrizadores
{
    public partial class frmPaImpuestoRentaDeducible : frmBaseTrxDev
    {

        clsNImpuestoRentaDeducible loLogicaNegocio;
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel;
        private bool lbCargado;

        public frmPaImpuestoRentaDeducible()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNImpuestoRentaDeducible();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
           
        }

        private void frmPaImpuestoRentaDeducible_Load(object sender, EventArgs e)
        {
            lbCargado = false;
            clsComun.gLLenarCombo(ref cmbPersona, loLogicaNegocio.goConsultarComboIdPersona(), true);
            bsDatos.DataSource = new List<ImpuestoRentaDeducibleGrid>();
            gcParametrizaciones.DataSource = bsDatos;
            gcParametrizaciones.RefreshDataSource();
            lColumnas();
            txtAño.EditValue = DateTime.Now.Year;
            lCargarEventosBotones();
            lbCargado = true;
    }

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            var poLista = (List<ImpuestoRentaDeducibleGrid>)bsDatos.DataSource;
            var poListaAgregar = new List<ImpuestoRentaDeducibleGrid>();
            ImpuestoRentaDeducibleGrid poItem = new ImpuestoRentaDeducibleGrid();
            poItem.anio = Convert.ToInt32(txtAño.EditValue);
            poItem.Vivienda = Convert.ToDecimal(txtVivienda.EditValue);
            poItem.Educacion = Convert.ToDecimal(txtEducacion.EditValue);
            poItem.Salud = Convert.ToDecimal(txtSalud.EditValue);
            poItem.Vestimenta = Convert.ToDecimal(txtVestimenta.EditValue);
            poItem.Alimentacion = Convert.ToDecimal(txtAlimentacion.EditValue);
            poItem.Turismo = Convert.ToDecimal(txtTurismo.EditValue);

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

        private string lsValidaDuplicados(List<ImpuestoRentaDeducibleGrid> toListaGrid, List<ImpuestoRentaDeducibleGrid> toListaInsertar)
        {
            string psMsg = string.Empty;
          
            foreach (var item in toListaInsertar)
            {
               
                var piRegistro = toListaGrid.Where(x => x.anio == item.anio ).Count();
                if (piRegistro > 0)
                {
                    psMsg += "El registro que intenta ingresar ya esta parametrizado.";
                }
           
            }

            return psMsg;
        }

        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvParametrizaciones.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<ImpuestoRentaDeducibleGrid>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                 
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsDatos.DataSource = poLista;
                    dgvParametrizaciones.RefreshData();
                    gcParametrizaciones.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


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

        private void lColumnas()
        {
            lColocarbotonDelete(dgvParametrizaciones.Columns["Del"]);
            dgvParametrizaciones.Columns["Total"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["anio"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["anio"].Caption = "Año";
            dgvParametrizaciones.Columns["IdImpuestoRentaDeducible"].Visible = false;


            for (int i = 2; i < dgvParametrizaciones.Columns.Count; i++)
            {
                dgvParametrizaciones.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                dgvParametrizaciones.Columns[i].DisplayFormat.FormatString = "c2";
            }
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Click += btnImportar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportar_Click;
            if (tstBotones.Items["btnPlantilla"] != null) tstBotones.Items["btnPlantilla"].Click += btnPlantilla_Click;
            //if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

        }
        private void lLimpiar()
        {
            if ((cmbPersona.Properties.DataSource as IList).Count > 0) cmbPersona.ItemIndex = 0;
            txtAño.EditValue = DateTime.Now.Year;
            txtVivienda.EditValue = 0;
            txtEducacion.EditValue = 0;
            txtSalud.EditValue = 0;
            txtVestimenta.EditValue = 0;
            txtAlimentacion.EditValue = 0;
            txtTurismo.EditValue = 0;

 
            bsDatos.DataSource = new List<ImpuestoRentaDeducibleGrid>();
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



        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                List<Persona> poListaObject = loLogicaNegocio.goListarMaestro("");
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Código"),
                                    new DataColumn("Descripción"),
                                    new DataColumn("Estado")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Código"] = a.IdPersona;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                    row["Descripción"] = a.NombreCompleto;

                    dt.Rows.Add(row);
                });
                
                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    cmbPersona.EditValue = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void lConsultar()
        {
            if (cmbPersona.Text!=" ")
            {
                var poObject = loLogicaNegocio.goBuscarMaestro(cmbPersona.EditValue.ToString());
                if (poObject != null)
                {
                    bsDatos.DataSource = poObject;
                    gcParametrizaciones.RefreshDataSource();
                }
            }

        }

        private void cmbPersona_EditValueChanged(object sender, EventArgs e)
        {
            lConsultar();
        }
        /// <summary>
        /// Evento del botón Grabar, Guarda datos del formulario a el objeto de BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
               

                    dgvParametrizaciones.PostEditor();
                 

                //poObject.IdPerfil = Convert.ToInt32(txtCodigo.Text.Trim());
                var poLista = (List<ImpuestoRentaDeducibleGrid>)bsDatos.DataSource;

                var guardado = loLogicaNegocio.gsGuardar(poLista, Int32.Parse(cmbPersona.EditValue.ToString()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    if (string.IsNullOrEmpty(guardado))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                    else
                    {
                        XtraMessageBox.Show(guardado + " \n" + Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                var poLista = (List<ImpuestoRentaDeducibleGrid>)bsDatos.DataSource;
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
                        var poLista = (List<ImpuestoRentaDeducibleGrid>)bsDatos.DataSource;
                        var poListaImportada = new List<ImpuestoRentaDeducibleGrid>();

                        string psMsgImport = string.Empty;
                        int piContador = 1;
                        foreach (DataRow item in dt.Rows)
                        {

                            if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                            {
                                ImpuestoRentaDeducibleGrid poItem = new ImpuestoRentaDeducibleGrid();
                                poItem.anio = int.Parse(item[0].ToString().Trim());
                                poItem.Vivienda = Convert.ToDecimal(item[1].ToString().Trim());
                                poItem.Educacion = Convert.ToDecimal(item[2].ToString().Trim());
                                poItem.Salud = Convert.ToDecimal(item[3].ToString().Trim());
                                poItem.Vestimenta = Convert.ToDecimal(item[4].ToString().Trim());
                                poItem.Alimentacion = Convert.ToDecimal(item[5].ToString().Trim());
                                poItem.Turismo = Convert.ToDecimal(item[6].ToString().Trim());
                               
                                piContador++;

                           
                                
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
        /// Evento del botón Plantilla, Exporta Plantilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlantilla_Click(object sender, EventArgs e)
        {
            GridControl gc = new GridControl();
            try
            {
                // Crear Lista de la Plantilla a exportar
                if (lbCargado)
                {

                    //dt.Columns.Remove("Departamento");
                    // Grid Control y Binding Object

                    BindingSource bs = new BindingSource();
                    GridView dgv = new GridView();

                    gc.DataSource = bs;
                    gc.MainView = dgv;
                    gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { dgv });
                    dgv.GridControl = gc;
                    this.Controls.Add(gc);
                    bs.DataSource = new List<ImpuestoRentaDeducibleExcel>();
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
                gc.Visible = false;
            }
        }













    }



    }
