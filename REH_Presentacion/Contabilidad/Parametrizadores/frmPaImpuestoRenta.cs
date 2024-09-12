using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList.Data;
using GEN_Entidad;
using GEN_Entidad.Entidades;
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
using VTA_Negocio;

namespace REH_Presentacion.Contabilidad.Parametrizadores
{
    public partial class frmPaImpuestoRenta : frmBaseTrxDev
    {
        
        clsNImpuestoRenta loLogicaNegocio;
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel;
        private bool lbCargado;
        public frmPaImpuestoRenta()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNImpuestoRenta();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        private void frmPaImpuestoRenta_Load(object sender, EventArgs e)
        {
            lbCargado = false;
           
            bsDatos.DataSource = new List<ImpuestoRenta>();
            gcParametrizaciones.DataSource = bsDatos;
            gcParametrizaciones.RefreshDataSource();
            lColumnas();
         
            lCargarEventosBotones();
            lbCargado = true;
        }

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                string anio = txtAnio.EditValue.ToString();
                if (anio.Length==4  )
            {
                    string psValidar = "";
                    if (string.IsNullOrEmpty(psValidar))
                    {
                        var poLista = (List<ImpuestoRenta>)bsDatos.DataSource;
                        var poListaAgregar = new List<ImpuestoRenta>();

                        if (Convert.ToDecimal(txtFraccionBasica.Text) >= Convert.ToDecimal(txtExcesoHasta.Text))
                        {
                            XtraMessageBox.Show("El valor de la Fraccion basica es mayor o igual al exceso ", "No es posible agregar!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;

                        }
                        if (!lvalidar(Convert.ToDecimal(txtFraccionBasica.Text), poLista))
                        {
                            XtraMessageBox.Show("EL VALOR: " + txtFraccionBasica.Text + " YA ESTA EN EL RANGO", "No es posible agregar!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (!lvalidar(Convert.ToDecimal(txtExcesoHasta.Text), poLista))
                        {
                            XtraMessageBox.Show("EL VALOR: " + txtExcesoHasta.Text + " YA ESTA EN EL RANGO", "No es posible agregar!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        ImpuestoRenta poItem = new ImpuestoRenta();
                        poItem.Anio = Convert.ToInt32(txtAnio.Text);
                        poItem.FraccionBasica = Convert.ToDecimal(txtFraccionBasica.Text);
                        poItem.ExcesoHasta = Convert.ToDecimal(txtExcesoHasta.Text);
                        poItem.ImpuestoaRenta = Convert.ToDecimal(txtImpuestoRenta.Text);
                        poItem.PorcentajeExcedente = Convert.ToDecimal(txtPorcentajeExcedente.Text);


                        poListaAgregar.Add(poItem);
                        string psMsg = lsValidaDuplicados(poLista, poListaAgregar);
                        if (!string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(psMsg, "No es posible agregar duplicados!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        poLista.Add(poItem);

                        bsDatos.DataSource = poLista;
                        gcParametrizaciones.RefreshDataSource();
                        dgvParametrizaciones.BestFitColumns();

                    }
                    else
                    {
                        XtraMessageBox.Show(psValidar, "No es posible agregar!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                
                }
                else
                {
                    XtraMessageBox.Show("Año incorrecto", "No es posible agregar!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        
           

        }
        private string lsValidaDuplicados(List<ImpuestoRenta> toListaGrid, List<ImpuestoRenta> toListaInsertar)
        {
            string psMsg = string.Empty;
            
            foreach (var item in toListaInsertar)
            {

                var piRegistro = toListaGrid.Where(x => x.FraccionBasica == item.FraccionBasica && x.ExcesoHasta == item.ExcesoHasta
                && x.ImpuestoaRenta == item.ImpuestoaRenta && x.PorcentajeExcedente == item.PorcentajeExcedente ).Count();
 


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
                var poLista = (List<ImpuestoRenta>)bsDatos.DataSource;

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

            dgvParametrizaciones.Columns["Anio"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["FraccionBasica"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["ExcesoHasta"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["CodigoEstado"].Visible = false;

            dgvParametrizaciones.Columns["IdImpuestoRenta"].Visible = false;

            dgvParametrizaciones.Columns["Anio"].Caption = "Año";
            dgvParametrizaciones.Columns["ImpuestoaRenta"].Caption = "Impuesto a la renta";
            ldolar("FraccionBasica");
            ldolar("ExcesoHasta");
            ldolar("ImpuestoaRenta");



        }
        private void ldolar(string psNameColumn)
        {

          //  dgvParametrizaciones.Columns[psNameColumn].UnboundType = UnboundColumnType.Decimal;
   
            dgvParametrizaciones.Columns[psNameColumn].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvParametrizaciones.Columns[psNameColumn].DisplayFormat.FormatString = "c2";

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
            txtAnio.EditValue = string.Empty;
            txtFraccionBasica.EditValue = string.Empty;
            txtExcesoHasta.EditValue = string.Empty;
            txtImpuestoRenta.EditValue = string.Empty;
            txtPorcentajeExcedente.EditValue = string.Empty;
         


            bsDatos.DataSource = new List<ImpuestoRenta>();
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
                List<ImpuestoRenta> poListaObject = loLogicaNegocio.goListarMaestro("");
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                   
                                    new DataColumn("Año"),
                                    new DataColumn("Estado")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                 
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                    row["Año"] = a.Anio;

                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    txtAnio.Text = pofrmBuscar.lsCodigoSeleccionado;
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
            if (!string.IsNullOrEmpty(txtAnio.Text.ToString()))
            {
                var poObject = loLogicaNegocio.goBuscarMaestro(txtAnio.Text.ToString());
                //if (poObject.Count >0)
                //{
                  
                    bsDatos.DataSource = poObject;
                    gcParametrizaciones.RefreshDataSource();
                
                //}
              
            }

        }

        private void txtAnio_EditValueChanged(object sender, EventArgs e)
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
                if (!string.IsNullOrEmpty(txtAnio.Text.ToString()))
                {


                    var poLista = (List<ImpuestoRenta>)bsDatos.DataSource;

                    var guardado = loLogicaNegocio.gsGuardar(poLista, Int32.Parse(txtAnio.Text.ToString()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                var poLista = (List<ImpuestoRenta>)bsDatos.DataSource;
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
                        var poLista = (List<ImpuestoRenta>)bsDatos.DataSource;
                        var poListaImportada = new List<ImpuestoRenta>();

                        string psMsgImport = string.Empty;
                        int piContador = 1;
                        foreach (DataRow item in dt.Rows)
                        {

                            if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                            {
                                ImpuestoRenta poItem = new ImpuestoRenta();
                                poItem.Anio = int.Parse(item[0].ToString().Trim());
                                poItem.FraccionBasica = Convert.ToDecimal(item[1].ToString().Trim());
                                poItem.ExcesoHasta = Convert.ToDecimal(item[2].ToString().Trim());
                                poItem.ImpuestoaRenta = Convert.ToDecimal(item[3].ToString().Trim());
                                poItem.PorcentajeExcedente = Convert.ToDecimal(item[4].ToString().Trim());
                             

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
                    bs.DataSource = new List<ImpuestoRentaExcel>();
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


        bool lvalidar(decimal num, List<ImpuestoRenta> poLista)
        {
            bool pbreturn = true;
            if (poLista.Count > 0)
            {
                foreach (var valor in poLista)
                {
                    if (num >= valor.FraccionBasica && num <= valor.ExcesoHasta)
                    {
                        pbreturn = false;
                    }
                }
            }
           
            return pbreturn;
        }
        //string lvalidar()
        //{
        //    string psReturn = "" ;
        //    if (string.IsNullOrEmpty(txtFraccionBasica.EditValue.ToString()) || Convert.ToDecimal(txtExcesoHasta.EditValue)<0)
        //    {
        //        psReturn += "Falta Fraccion Basica\n";
        //    }
        //    if (string.IsNullOrEmpty(txtExcesoHasta.EditValue.ToString()) || Convert.ToDecimal(txtExcesoHasta.EditValue) < 0)
        //    {
        //        psReturn += "Falta Exceso hasta\n";
        //    }
        //    if (string.IsNullOrEmpty(txtImpuestoRenta.EditValue.ToString()) || Convert.ToDecimal(txtImpuestoRenta.EditValue) < 0)
        //    {
        //        psReturn += "Falta Impuesto a la renta\n";
        //    }
        //    if (string.IsNullOrEmpty(txtPorcentajeExcedente.EditValue.ToString()) || Convert.ToDecimal(txtPorcentajeExcedente.EditValue) < 0)
        //    {
        //        psReturn += "Falta Porcentaje excedente\n";
        //    }



        //    return psReturn;
        //}


    }
}

