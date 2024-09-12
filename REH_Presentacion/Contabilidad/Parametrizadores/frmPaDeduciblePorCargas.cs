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
    public partial class frmPaDeduciblePorCargas : frmBaseTrxDev
    {
        
        clsNImpuestoRenta loLogicaNegocio;
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel;
        private bool lbCargado;
        public frmPaDeduciblePorCargas()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNImpuestoRenta();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        private void frmPaImpuestoRenta_Load(object sender, EventArgs e)
        {
            lbCargado = false;
           
            bsDatos.DataSource = new List<ImpuestoRentaCargas>();
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
                if (anio.Length == 4  )
            {
                    string psValidar = "";
                    if (string.IsNullOrEmpty(psValidar))
                    {
                        var poLista = (List<ImpuestoRentaCargas>)bsDatos.DataSource;
                        var poListaAgregar = new List<ImpuestoRentaCargas>();

                        
                        if (!lvalidar(Convert.ToDecimal(txtCargas.Text), poLista))
                        {
                            XtraMessageBox.Show("EL VALOR: " + txtCargas.Text + " YA ESTA EN EL RANGO", "No es posible agregar!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        
                        ImpuestoRentaCargas poItem = new ImpuestoRentaCargas();
                        poItem.Anio = Convert.ToInt32(txtAnio.Text);
                        poItem.Cargas = Convert.ToInt32(txtCargas.Text);
                        poItem.GastoDeducibleMaximo = Convert.ToDecimal(txtDeducibleMaximo.Text);
                        poItem.RebajaIR = Convert.ToDecimal(txtRebajaIR.Text);


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
        private string lsValidaDuplicados(List<ImpuestoRentaCargas> toListaGrid, List<ImpuestoRentaCargas> toListaInsertar)
        {
            string psMsg = string.Empty;
            
            foreach (var item in toListaInsertar)
            {

                var piRegistro = toListaGrid.Where(x => x.Cargas == item.Cargas && x.GastoDeducibleMaximo == item.GastoDeducibleMaximo
                && x.GastoDeducibleMinimo == item.GastoDeducibleMinimo && x.RebajaIR == item.RebajaIR).Count();
 


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
                var poLista = (List<ImpuestoRentaCargas>)bsDatos.DataSource;

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
            //dgvParametrizaciones.Columns["Cargas"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["GastoDeducibleMinimo"].Visible = false;
            //dgvParametrizaciones.Columns["GastoDeducibleMaximo"].OptionsColumn.AllowEdit = false;
            dgvParametrizaciones.Columns["CodigoEstado"].Visible = false;

            dgvParametrizaciones.Columns["IdDeduccionImpuestoRentaCargas"].Visible = false;

            dgvParametrizaciones.Columns["Anio"].Caption = "Año";
            //dgvParametrizaciones.Columns["ImpuestoaRenta"].Caption = "Impuesto a la renta";
            ldolar("GastoDeducibleMaximo");
            ldolar("GastoDeducibleMinimo");
            ldolar("RebajaIR");



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
            txtCargas.EditValue = string.Empty;
            //txtDeducibleMinimo.EditValue = string.Empty;
            txtDeducibleMaximo.EditValue = string.Empty;
            txtRebajaIR.EditValue = string.Empty;
         


            bsDatos.DataSource = new List<ImpuestoRentaCargas>();
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
                List<ImpuestoRentaCargas> poListaObject = loLogicaNegocio.goListarMaestroDeducibleCargas("");
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
                var poObject = loLogicaNegocio.goBuscarMaestroDeducibleCargas(txtAnio.Text.ToString());
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
                    var poLista = (List<ImpuestoRentaCargas>)bsDatos.DataSource;

                    var guardado = loLogicaNegocio.gsGuardarDeducibleCargas(poLista, Int32.Parse(txtAnio.EditValue.ToString()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                var poLista = (List<ImpuestoRentaCargas>)bsDatos.DataSource;
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
                        var poLista = (List<ImpuestoRentaCargas>)bsDatos.DataSource;
                        var poListaImportada = new List<ImpuestoRentaCargas>();

                        string psMsgImport = string.Empty;
                        int piContador = 1;
                        foreach (DataRow item in dt.Rows)
                        {

                            if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                            {
                                ImpuestoRentaCargas poItem = new ImpuestoRentaCargas();
                                poItem.Anio = int.Parse(item[0].ToString().Trim());
                                poItem.Cargas = Convert.ToInt32(item[1].ToString().Trim());
                                poItem.GastoDeducibleMinimo = Convert.ToDecimal(item[2].ToString().Trim());
                                poItem.GastoDeducibleMaximo = Convert.ToDecimal(item[3].ToString().Trim());
                                poItem.RebajaIR = Convert.ToDecimal(item[4].ToString().Trim());
                             

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
                    bs.DataSource = new List<ImpuestoRentaCargasExcel>();
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


        bool lvalidar(decimal num, List<ImpuestoRentaCargas> poLista)
        {
            bool pbreturn = true;
            //if (poLista.Count > 0)
            //{
            //    foreach (var valor in poLista)
            //    {
            //        if (num >= valor.GastoDeducibleMinimo && num <= valor.GastoDeducibleMaximo)
            //        {
            //            pbreturn = false;
            //        }
            //    }
            //}
           
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

