using COM_Negocio;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSpreadsheet.Import.Xls;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using REH_Presentacion.Compras.Transaccion.Modal;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.Data.Filtering.Helpers.SubExprHelper.ThreadHoppingFiltering;

namespace REH_Presentacion.Compras.Transaccion
{
    public partial class frmTrOrdenPago : frmBaseTrxDev
    {
        BindingSource bsDatos;
        BindingSource bsArchivoAdjunto;
        clsNOrdenPago loLogicaNegocio = new clsNOrdenPago();
        RepositoryItemMemoEdit rpiMedDescripcion;
        RepositoryItemButtonEdit rpiBtnDel;
        RepositoryItemButtonEdit rpiBtnAdd;
        //RepositoryItemButtonEdit rpiBtnAddDetalle;
        RepositoryItemButtonEdit rpiBtnDownload;
        RepositoryItemButtonEdit rpiBtnReferencia;
        //RepositoryItemButtonEdit rpiBtnShowDetalle;
        RepositoryItemButtonEdit rpiBtnShowArchivo;
        RepositoryItemButtonEdit rpiBtnGuia =  new RepositoryItemButtonEdit();
        bool pbCargado;
        public int lid = 0;

        public frmTrOrdenPago()
        {
            bsDatos = new BindingSource();
            InitializeComponent();
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.MaxLength = 200;
            rpiMedDescripcion.WordWrap = true;
            bsArchivoAdjunto = new BindingSource();


            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnReferencia = new RepositoryItemButtonEdit();
            rpiBtnReferencia.ButtonClick += rpiBtnReferencia_ButtonClick;
            rpiBtnAdd = new RepositoryItemButtonEdit();
            rpiBtnAdd.ButtonClick += rpiBtnAdd_ButtonClick;

            rpiBtnShowArchivo = new RepositoryItemButtonEdit();
            rpiBtnShowArchivo.ButtonClick += rpiBtnShowArchivo_ButtonClick;
            
            rpiBtnDownload = new RepositoryItemButtonEdit();
            rpiBtnDownload.ButtonClick += rpiBtnDownload_ButtonClick;

            rpiBtnGuia.ButtonClick += rpiBtnGuia_ButtonClick;


        }


        /// <summary>
        /// Evento de inicialización del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTrOrdenPago_Load(object sender, EventArgs e)
        {
            
            lCargarEventosBotones();
            bsDatos.DataSource = new List<Factura>();
            gcFactura.DataSource = bsDatos;

            bsArchivoAdjunto.DataSource = new List<OrdenPagoAdjunto>();
            gcArchivoAdjunto.DataSource = bsArchivoAdjunto;

            pbCargado = false;
            clsComun.gLLenarCombo(ref cmbTipoCompra, loLogicaNegocio.goConsultarComboTipoCompra(), true);
            clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstadoCotizacion(), true);
            clsComun.gLLenarCombo(ref cmbProveedor, loLogicaNegocio.goConsultarComboProveedorId(), true);
            clsComun.gLLenarCombo(ref cmbTipoOrdenPago, loLogicaNegocio.goConsultarComboTipoOrdenPago(), true);
            clsComun.gLLenarComboGrid(ref dgvFactura, loLogicaNegocio.goConsultarComboTipoTransporte(), "TipoTransporte", true);
            clsComun.gLLenarComboGrid(ref dgvFactura, loLogicaNegocio.goConsultarZonasSAAFSinNumero(), "IdZonaGrupo", true);
            pbCargado = true;
            cmbEstado.EditValue = Diccionario.Pendiente;
            lColumnas();
            lValidarControles();
            if (lid != 0)
            {
                txtNo.Text = lid.ToString();
                lConsultar();
            }
            
        }

        /// <summary>
        /// Evento del boton de Agregar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnAdd_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {

                if (xtraTabControl1.SelectedTabPageIndex == 0)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvFactura.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<Factura>)bsDatos.DataSource;


                    // Presenta un dialogo para seleccionar las imagenes
                    ofdArchicoAdjunto.Title = "Seleccione Archivo";
                    ofdArchicoAdjunto.Filter = "Image Files( *.pdf; )|*.pdf;";

                    if (ofdArchicoAdjunto.ShowDialog() == DialogResult.OK)
                    {
                        if (poLista.Count > 0 && piIndex >= 0)
                        {

                            if (!ofdArchicoAdjunto.FileName.Equals(""))
                            {
                                FileInfo file = new FileInfo(ofdArchicoAdjunto.FileName);
                                var piSize = file.Length;

                                if (piSize <= clsPrincipal.gdcTamanoMb * 1048576)
                                {
                                    string Name = file.Name + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff") + Path.GetExtension(ofdArchicoAdjunto.FileName);
                                    var poEntidad = poLista[piIndex];

                                    poLista[piIndex].ArchivoAdjunto = Name;
                                    poLista[piIndex].RutaOrigen = ofdArchicoAdjunto.FileName;
                                    poLista[piIndex].NombreOriginal = file.Name;
                                    poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaOPCompras"].ToString() + Name;
                                    // Asigno mi nueva lista al Binding Source
                                    bsDatos.DataSource = poLista;
                                    dgvFactura.RefreshData();
                                }

                                else
                                {
                                    XtraMessageBox.Show("El tamano máximo permitido es de: " + clsPrincipal.gdcTamanoMb + "mb", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
                else if (xtraTabControl1.SelectedTabPageIndex == 1)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvArchivoAdjunto.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<OrdenPagoAdjunto>)bsArchivoAdjunto.DataSource;


                    // Presenta un dialogo para seleccionar las imagenes
                    ofdArchicoAdjunto.Title = "Seleccione Archivo";
                    ofdArchicoAdjunto.Filter = "Image Files( *.*; ) | *.*";

                    if (ofdArchicoAdjunto.ShowDialog() == DialogResult.OK)
                    {
                        if (poLista.Count > 0 && piIndex >= 0)
                        {

                            if (!ofdArchicoAdjunto.FileName.Equals(""))
                            {
                                FileInfo file = new FileInfo(ofdArchicoAdjunto.FileName);
                                var piSize = file.Length;

                                if (piSize <= clsPrincipal.gdcTamanoMb * 1048576)
                                {
                                    string Name = file.Name + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff") + Path.GetExtension(ofdArchicoAdjunto.FileName);
                                    var poEntidad = poLista[piIndex];

                                    poLista[piIndex].ArchivoAdjunto = Name;
                                    poLista[piIndex].RutaOrigen = ofdArchicoAdjunto.FileName;
                                    poLista[piIndex].NombreOriginal = file.Name;
                                    poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaOPCompras"].ToString() + Name;
                                    // Asigno mi nueva lista al Binding Source
                                    bsArchivoAdjunto.DataSource = poLista;
                                    dgvArchivoAdjunto.RefreshData();
                                }

                                else
                                {
                                    XtraMessageBox.Show("El tamano máximo permitido es de: " + clsPrincipal.gdcTamanoMb + "mb", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Evento del boton de Agregar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShowArchivo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvFactura.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<Factura>)bsDatos.DataSource;
                    if (!string.IsNullOrEmpty(poLista[piIndex].ArchivoAdjunto))
                    {
                        string psRuta = "";
                       
                        //Muestra archivo local
                        if (!string.IsNullOrEmpty(poLista[piIndex].RutaOrigen))
                        {
                            psRuta = poLista[piIndex].RutaOrigen;
                        }
                        //Muestra archivo ya subido
                        else
                        {
                            psRuta = ConfigurationManager.AppSettings["CarpetaOPCompras"].ToString() + poLista[piIndex].ArchivoAdjunto;
                        }

                        if (psRuta.ToLower().Contains(".pdf"))
                        {
                            frmVerPdf pofrmVerPdf = new frmVerPdf();
                            pofrmVerPdf.lsRuta = psRuta;
                            pofrmVerPdf.Show();
                            pofrmVerPdf.SetDesktopLocation(0, 0);
                            pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;
                        }
                        else
                        {
                            Process.Start(psRuta);
                        }


                    }

                    else
                    {
                        XtraMessageBox.Show("No hay archivo para mostrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (xtraTabControl1.SelectedTabPageIndex == 1)
                {
                    int piIndex = dgvArchivoAdjunto.FocusedRowHandle;
                    var poLista = (List<OrdenPagoAdjunto>)bsArchivoAdjunto.DataSource;

                    if (!string.IsNullOrEmpty(poLista[piIndex].ArchivoAdjunto))
                    {
                        string psRuta = "";
                        //Muestra archivo local
                        if (!string.IsNullOrEmpty(poLista[piIndex].RutaOrigen))
                        {
                            psRuta = poLista[piIndex].RutaOrigen;
                        }
                        //Muestra archivo ya subido
                        else
                        {
                            psRuta = poLista[piIndex].RutaDestino + poLista[piIndex].ArchivoAdjunto;
                        }

                        if (psRuta.ToLower().Contains(".pdf"))
                        {
                            frmVerPdf pofrmVerPdf = new frmVerPdf();
                            pofrmVerPdf.lsRuta = psRuta;
                            pofrmVerPdf.Show();
                            pofrmVerPdf.SetDesktopLocation(0, 0);
                            pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;
                        }
                        else
                        {
                            Process.Start(psRuta);
                        }
                    }

                    else
                    {
                        XtraMessageBox.Show("No hay archivo para mostrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        /// <summary>
        /// Agregar referencia de Orden de compra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnReferencia_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {

                var poLista = (List<Factura>)bsDatos.DataSource;
                var piIndex = dgvFactura.GetFocusedDataSourceRowIndex();
                List<int> id = new List<int>();
                
                if (!string.IsNullOrEmpty(poLista[piIndex].Observacion))
                {
                    var separar = poLista[piIndex].Observacion.Split(new char[] { ',' });

                    foreach (var num in separar)
                    {
                        if (num != " ")
                        {
                            id.Add(int.Parse(num));
                          
                        }
                    }
                }
                List<int> idUsado = new List<int>();
                foreach (var x in poLista)
                {
                    if (!string.IsNullOrEmpty(x.Observacion))
                    {
                        var separar = x.Observacion.Split(new char[] { ',' });

                        foreach (var num in separar)
                        {
                            if (num != " ")
                            {
                                idUsado.Add(int.Parse(num));

                            }
                        }
                    }
                }
                foreach (var item in id)
                {
                    idUsado.Remove(item);
                }


                var poListaObject = loLogicaNegocio.goBuscarListarProveedores(int.Parse(cmbProveedor.EditValue.ToString()), id);
               

                frmBusquedaOC pofrmBuscar = new frmBusquedaOC(poListaObject, idUsado) { Text = "Listado de Registros" };
                pofrmBuscar.dgvBusqueda.Columns["IdProveedor"].Caption = "No.";

                pofrmBuscar.dgvBusqueda.Columns["Estado"].Visible = false;
                lColocarTotal("ValorAnticipo", pofrmBuscar.dgvBusqueda);
                lColocarTotal("Saldo", pofrmBuscar.dgvBusqueda);
                lColocarTotal("Total", pofrmBuscar.dgvBusqueda);
                pofrmBuscar.dgvBusqueda.OptionsBehavior.Editable = true;
                for (int i = 0; i < pofrmBuscar.dgvBusqueda.Columns.Count; i++)
                {
                    pofrmBuscar.dgvBusqueda.Columns[i].OptionsColumn.AllowEdit = false;
                }
                pofrmBuscar.dgvBusqueda.Columns["Sel"].OptionsColumn.AllowEdit = true;

                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    string final = string.Empty;
                    //lLimpiar();

                    poLista[piIndex].Observacion = pofrmBuscar.lsCodigoSeleccionado;
                    //poLista[piIndex].SaldoOc = pofrmBuscar.ldSaldo;
                    bsDatos.DataSource = poLista;
                    gcFactura.RefreshDataSource();
                  
                }

                lCalcularSaldoFila();
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Agregar fila a grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (cmbProveedor.EditValue.ToString() != Diccionario.Seleccione)
                {
                    if (xtraTabControl1.SelectedTabPageIndex == 0)
                    {
                        bsDatos.AddNew();
                        //cmbProveedor.ReadOnly = true;
                        dgvFactura.Focus();
                        dgvFactura.ShowEditor();
                        dgvFactura.UpdateCurrentRow();
                        var poLista = (List<Factura>)bsDatos.DataSource;
                        if (txtObservacion.Text.Length > 100)
                        {
                            poLista.LastOrDefault().Descripcion = txtObservacion.Text.Substring(0, 100);
                        }
                        else
                        {
                            poLista.LastOrDefault().Descripcion = txtObservacion.Text;
                        }
                        
                        poLista.LastOrDefault().FechaFactura = DateTime.Now;
                        poLista.LastOrDefault().FechaVencimiento = DateTime.Now;
                        poLista.LastOrDefault().TipoTransporte = Diccionario.Seleccione;
                        poLista.LastOrDefault().IdZonaGrupo = 0;
                        dgvFactura.RefreshData();
                        dgvFactura.FocusedColumn = dgvFactura.VisibleColumns[0];
                    }
                    if (xtraTabControl1.SelectedTabPageIndex == 1)
                    {
                        bsArchivoAdjunto.AddNew();
                        dgvArchivoAdjunto.Focus();
                        dgvArchivoAdjunto.ShowEditor();
                        dgvArchivoAdjunto.UpdateCurrentRow();
                        dgvArchivoAdjunto.RefreshData();
                        dgvArchivoAdjunto.FocusedColumn = dgvArchivoAdjunto.VisibleColumns[0];
                    }

                    
                }
                else
                {
                    XtraMessageBox.Show("Seleccione un proveedor primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

        /// <summary>
        /// Evento del boton de Descargar en el Grid, descarga el archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDownload_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {

                int piIndex = dgvArchivoAdjunto.FocusedRowHandle;
                var poLista = (List<OrdenPagoAdjunto>)bsArchivoAdjunto.DataSource;

                string psRuta = poLista[piIndex].RutaDestino + poLista[piIndex].ArchivoAdjunto;
                string psNombreArchivo = poLista[piIndex].NombreOriginal;

                if (!string.IsNullOrEmpty(psRuta))
                {
                    if (File.Exists(psRuta))
                    {
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "files (*.pdf)|*.pdf|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 2;
                        saveFileDialog1.RestoreDirectory = true;
                        saveFileDialog1.FileName = psNombreArchivo.Split('.')[0];
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            File.Copy(psRuta, saveFileDialog1.FileName + Path.GetExtension(psRuta));
                            XtraMessageBox.Show("Archivo descargado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Debe guardar cambios para descargar, Archivo no existe en Base de Datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("Debe guardar cambios para descargar, Archivo no existe en Base de Datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvFactura.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<Factura>)bsDatos.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de eliminar registro?", "AVISO", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {
                            List<int> id = new List<int>();

                            if (!string.IsNullOrEmpty(poLista[piIndex].Observacion))
                            {
                                var separar = poLista[piIndex].Observacion.Split(new char[] { ',' });

                                foreach (var num in separar)
                                {
                                    if (num != " ")
                                    {
                                        id.Add(int.Parse(num));
                                    }
                                }
                            }

                            // Eliminar Fila seleccionada de mi lista
                            //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                            poLista.RemoveAt(piIndex);

                            // Asigno mi nueva lista al Binding Source

                            if (poLista.Count == 0)
                            {
                                //cmbProveedor.ReadOnly = false;
                            }

                            bsDatos.DataSource = poLista;
                            dgvFactura.RefreshData();
                        }
                    }


                    lCalcularSaldoFila();
                }
                if (xtraTabControl1.SelectedTabPageIndex == 1)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvArchivoAdjunto.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<OrdenPagoAdjunto>)bsArchivoAdjunto.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsArchivoAdjunto.DataSource = poLista;
                        dgvArchivoAdjunto.RefreshData();
                    }
                }

                

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {

            try
            {
                dgvFactura.PostEditor();
                dgvArchivoAdjunto.PostEditor();
                var poLista = (List<Factura>)bsDatos.DataSource;

                string psMsgAlerta = "";
                int num = 0;
                foreach (var factura in poLista)
                {
                    num++;

                    if (factura.GuiaRemision != null && factura.GuiaRemision.Count() > 0)
                    {
                        var poListaGuias = factura.GuiaRemision;
                        decimal pdcCantidadTotalBultos = poListaGuias.Select(x => x.NumBultos).Sum();

                        decimal pdcValorPorBulto = 0M;
                        if (pdcCantidadTotalBultos > 0)
                        {
                            pdcValorPorBulto = factura.Valor / pdcCantidadTotalBultos;
                        }

                        foreach (var po in poListaGuias)
                        {
                            po.ValorBulto = pdcValorPorBulto;
                            po.Total = po.TotalFlete;
                        }
                    }
                    
                    ////Validación de que no cuadra el valor de la factura con las guias ingresadas
                    //if (factura.TipoTransporte != Diccionario.Seleccione)
                    //{
                    //    decimal pdcValorTotalGuias = factura.GuiaRemision.Sum(x => x.Total);
                    //    if (pdcValorTotalGuias != factura.Valor)
                    //    {
                    //        psMsgAlerta = string.Format("{0}En la factura No. {1}. El valor de la factura no cuadra con el valor total de Guías. Factura: {2} vs Guías: {3}.\n", psMsgAlerta, factura.NoFactura, factura.Valor.ToString("c2"), pdcValorTotalGuias.ToString("c2"));
                    //    }
                    //}
                }

                if (!string.IsNullOrEmpty(psMsgAlerta))
                {
                    XtraMessageBox.Show(psMsgAlerta, "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    
                    OrdenPago PoObject = new OrdenPago();
                    PoObject.Observacion = txtObservacion.Text;
                    if (!string.IsNullOrEmpty(txtNo.Text))
                    {
                        PoObject.IdOrdenPago = int.Parse(txtNo.Text);
                    }

                    string psComentarioCorregir = "";
                    bool pbCorregido = false;
                    if (cmbEstado.EditValue.ToString() == Diccionario.Corregir)
                    {
                        pbCorregido = true;

                        psComentarioCorregir = XtraInputBox.Show("Ingrese observación de su correción", "Correción", "");
                        if (string.IsNullOrEmpty(psComentarioCorregir))
                        {
                            XtraMessageBox.Show("Debe agregar la obsevación de su correción", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        PoObject.CodigoEstado = Diccionario.Pendiente;
                        PoObject.ComentarioAprobador = "";
                    }
                    else
                    {
                        PoObject.CodigoEstado = cmbEstado.EditValue.ToString();
                    }
                    
                    PoObject.Facturas = poLista;
                    PoObject.IdProveedor = int.Parse(cmbProveedor.EditValue.ToString());
                    PoObject.FormaPago = txtFormaPago.Text;
                    PoObject.CodigoTipoCompra = cmbTipoCompra.EditValue.ToString();
                    PoObject.OrdenPagoAdjunto = (List<OrdenPagoAdjunto>)bsArchivoAdjunto.DataSource;
                    PoObject.CodigoTipoOrdenPago = cmbTipoOrdenPago.EditValue.ToString();
                    

                    PoObject.Valor = 0;
                    foreach (var factura in poLista)
                    {
                        PoObject.Valor = factura.Valor + PoObject.Valor;
                        PoObject.TotalOrdenCompra = factura.SaldoOc + PoObject.TotalOrdenCompra;
                    }

                    string psMsg = loLogicaNegocio.gsGuardar(PoObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, pbCorregido, psComentarioCorregir);
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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

                var menu = Tag.ToString().Split(',');
                var poListaObject = loLogicaNegocio.goListarMaestro(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Usuario"),
                                    new DataColumn("Fecha Orden Pago", typeof(DateTime)),
                                    new DataColumn("Proveedor"),
                                    new DataColumn("Descripción"),
                                    new DataColumn("Facturas"),
                                    new DataColumn("Estado"),
                                    //new DataColumn("Comentario Aprobador")

                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdCotizacion;
                    row["Usuario"] = a.DesUsuario;
                    row["Fecha Orden Pago"] = a.FechaIngreso;
                    row["Proveedor"] = a.Proveedor;
                    row["Descripción"] = a.Descripcion;
                    row["Facturas"] = a.Comentario;
                    row["Estado"] = a.DesEstado;
                    //row["Comentario Aprobador"] = a.ComentarioAprobador;
                    dt.Rows.Add(row);
                });

                List<GridBusqueda> poAjustarColumnas = new List<GridBusqueda>();
                poAjustarColumnas.Add(new GridBusqueda { Ancho = 30, Columna = "No" });
                poAjustarColumnas.Add(new GridBusqueda { Ancho = 150, Columna = "Usuario" });
                poAjustarColumnas.Add(new GridBusqueda { Ancho = 100, Columna = "Fecha Orden Pago" });
                poAjustarColumnas.Add(new GridBusqueda { Ancho = 200, Columna = "Proveedor" });
                poAjustarColumnas.Add(new GridBusqueda { Ancho = 300, Columna = "Descripción" });
                poAjustarColumnas.Add(new GridBusqueda { Ancho = 100, Columna = "Facturas" });
                poAjustarColumnas.Add(new GridBusqueda { Ancho = 100, Columna = "Estado" });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt, poAjustarColumnas) { Text = "Listado de Registros" };
                pofrmBuscar.Width = 1200;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    lLimpiar();
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar();
                    //cmbProveedor.ReadOnly = true;

                }
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
                lLimpiar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loLogicaNegocio.gEliminarMaestro(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.ToString()))
                {
                    var poProveedor = (List<Factura>)bsDatos.DataSource;
                    var piIndex = dgvFactura.GetFocusedDataSourceRowIndex();
                    int lIdOrdenCompra = int.Parse(txtNo.Text);
                    int lIdProveedor = int.Parse(cmbProveedor.EditValue.ToString());
                    clsComun.gImprimirOrdenPago(lIdOrdenCompra);
                }
                else
                {
                    XtraMessageBox.Show("No existe detalles guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.ToString()))
                {

                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de orden de pago por correo?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var poCorreo = loLogicaNegocio.Correo(Diccionario.Correo.OrdenPagoContabilidad);

                        var poObject = loLogicaNegocio.goBuscarOrdenPago(Convert.ToInt32(txtNo.Text.Trim()));
                        string PsProveedor = loLogicaNegocio.goConsultarComboProveedorId().Where(x => x.Codigo == poObject.IdProveedor.ToString()).Select(x => x.Descripcion).FirstOrDefault();

                        string psTexto = poCorreo.Cuerpo;

                        psTexto = psTexto.Replace("#USUARIO", clsPrincipal.gsDesUsuario);
                        psTexto = psTexto.Replace("#DEPARTAMENTO", clsPrincipal.gsDesDepartamento);
                        psTexto = psTexto.Replace("#NOORDEN", poObject.IdOrdenPago.ToString());
                        psTexto = psTexto.Replace("#PROVEEDOR", PsProveedor);
                        psTexto = psTexto.Replace("#TOTAL", poObject.Valor.ToString("c2"));

                        bool pbResult = false;
                        if (poCorreo.CCUsuario)
                        {
                            pbResult = clsComun.EnviarPorCorreo(poCorreo.CCCorreo, poCorreo.Asunto, psTexto, null, true, clsPrincipal.gsCorreo);
                        }
                        else
                        {
                            pbResult = clsComun.EnviarPorCorreo(poCorreo.CCCorreo, poCorreo.Asunto, psTexto, null, true);
                        }
                    }
                    

                    XtraMessageBox.Show("Correo enviado exitosamente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("No existe detalles guardados para enviar por correo.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbTipoCompra_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    if (cmbTipoCompra.EditValue.ToString() == "001")
                    {
                        dgvFactura.Columns["Add"].Visible = false;
                        dgvFactura.Columns["Observacion"].Visible = false;
                        dgvFactura.Columns["SaldoOc"].Visible = false;
                        //dgvFactura.Columns["Add"].OptionsColumn.AllowEdit = false;
                    }
                    else
                    {
                        //dgvFactura.Columns["Add"].OptionsColumn.AllowEdit = true;
                        dgvFactura.Columns["SaldoOc"].Visible = true;
                        dgvFactura.Columns["Observacion"].Visible = true;
                        dgvFactura.Columns["Add"].Visible = true;
                        dgvFactura.Columns["Add"].VisibleIndex = 11;
                        dgvFactura.Columns["Observacion"].VisibleIndex = 12;
                        dgvFactura.Columns["SaldoOc"].VisibleIndex = 13;
                    }

                    
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                string psForma = Diccionario.Tablas.Menu.Proveedores;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);

                if (poForm != null)
                {
                    frmPaProveedores poFrmMostrarFormulario = new frmPaProveedores();

                    poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    poFrmMostrarFormulario.Text = poForm.Nombre;
                    poFrmMostrarFormulario.ShowInTaskbar = true;
                    //poFrmMostrarFormulario.MdiParent = this.ParentForm;

                    poFrmMostrarFormulario.ShowDialog();
                    clsComun.gLLenarCombo(ref cmbProveedor, loLogicaNegocio.goConsultarComboProveedorId(), true);

                }
                else
                {
                    XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lValidarControles()
        {
            if (clsPrincipal.gbEditaTipoOrdenPago)
            {
                cmbTipoOrdenPago.Visible = true;
                lblTipo.Visible = true;
            }
            else
            {
                cmbTipoOrdenPago.Visible = false;
                lblTipo.Visible = false;
            }
        }

        private void lColumnas()
        {
            dgvFactura.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            dgvFactura.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            dgvFactura.OptionsView.RowAutoHeight = true;

            // Consultar si el usuario tiene parametrizado para visualizar la ZONA
            var poUsuario = loLogicaNegocio.goBuscarMaestroUsuario(clsPrincipal.gsUsuario);
            if (poUsuario != null)
            {
                if (!poUsuario.VisualizaZonaOrdenPago)
                {
                    dgvFactura.Columns["IdZonaGrupo"].Visible = false;
                }
            }

            dgvFactura.Columns["IdFactura"].Visible = false;
            dgvFactura.Columns["CodigoEstado"].Visible = false;
            dgvFactura.Columns["IdOrdenPago"].Visible = false;
            dgvFactura.Columns["ArchivoAdjunto"].Visible = false;
            dgvFactura.Columns["RutaOrigen"].Visible = false;
            dgvFactura.Columns["RutaDestino"].Visible = false;
            dgvFactura.Columns["ZonaGrupo"].Visible = false;
            dgvFactura.Columns["GuiaRemisionFactura"].Visible = false;
            dgvFactura.Columns["GuiaRemision"].Visible = false;
            
            dgvFactura.Columns["Add"].Visible = false;
            dgvFactura.Columns["Observacion"].Visible = false;
            dgvFactura.Columns["SaldoOc"].Visible = false;

            dgvFactura.Columns["Observacion"].OptionsColumn.AllowEdit = false;
            dgvFactura.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;

            //dgvFactura.Columns["AgregarFactura"].Caption = "Adjuntar Factrua";
            dgvFactura.Columns["NombreOriginal"].Caption = "Adjunto";
            dgvFactura.Columns["VerFactura"].Caption = "Ver Factura";

            dgvFactura.Columns["SaldoOc"].OptionsColumn.AllowEdit = false;
            dgvFactura.Columns["FechaVencimiento"].OptionsColumn.AllowEdit = false;
            dgvFactura.Columns["SaldoOc"].Caption = "Total OC";
            dgvFactura.Columns["Observacion"].Caption = "Referencia OC";
            dgvFactura.Columns["Valor"].Caption = "Total Factura";
            dgvFactura.Columns["Dias"].Caption = "Dias Crédito";

            dgvFactura.Columns["TipoTransporte"].Caption = "Tipo Transporte";
            dgvFactura.Columns["IdZonaGrupo"].Caption = "Zona";

            dgvFactura.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
            dgvFactura.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;

            dgvArchivoAdjunto.Columns["IdOrdenPagoAdjunto"].Visible = false;
            dgvArchivoAdjunto.Columns["IdOrdenPago"].Visible = false;
            //dgvArchivoAdjunto.Columns["NombreOriginal"].Visible = false;
            dgvArchivoAdjunto.Columns["RutaDestino"].Visible = false;
            dgvArchivoAdjunto.Columns["RutaOrigen"].Visible = false;
            dgvArchivoAdjunto.Columns["ArchivoAdjunto"].Visible = false;
            dgvArchivoAdjunto.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvArchivoAdjunto.Columns["NombreOriginal"].Caption = "Archivo Adjunto";

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvFactura.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 30);
            clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvFactura.Columns["AgregarFactura"], "Factura", Diccionario.ButtonGridImage.open_16x16, 40);
            clsComun.gDibujarBotonGrid(rpiBtnReferencia, dgvFactura.Columns["Add"], "Agregar Ref. OC", Diccionario.ButtonGridImage.add_16x16, 50);
            clsComun.gDibujarBotonGrid(rpiBtnShowArchivo, dgvFactura.Columns["VerFactura"], "Ver", Diccionario.ButtonGridImage.show_16x16, 50);
            clsComun.gDibujarBotonGrid(rpiBtnGuia, dgvFactura.Columns["Guia"], "Gúias", Diccionario.ButtonGridImage.inserttable_16x16,30);

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvArchivoAdjunto.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);
            clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvArchivoAdjunto.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnDownload, dgvArchivoAdjunto.Columns["Descargar"], "Descargar", Diccionario.ButtonGridImage.download_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowArchivo, dgvArchivoAdjunto.Columns["Visualizar"], "Visualizar", Diccionario.ButtonGridImage.show_16x16);

            lColocarTotal("Valor", dgvFactura);
            lColocarTotal("SaldoOc", dgvFactura);

            dgvFactura.Columns["FechaFactura"].Width = 75;
            dgvFactura.Columns["FechaVencimiento"].Width = 80;
            dgvFactura.Columns["Descripcion"].Width = 200;
            dgvFactura.Columns["IdFactura"].Width = 35;
            dgvFactura.Columns["Dias"].Width = 60;
            dgvFactura.Columns["IdZonaGrupo"].Width = 120;
            dgvFactura.Columns["NoFactura"].Width = 55;
            dgvFactura.Columns["TipoTransporte"].Width = 75;
            dgvFactura.Columns["Valor"].Width = 70;
            dgvFactura.Columns["Del"].Width = 45;
            dgvFactura.Columns["VerFactura"].Width = 45;
            dgvFactura.Columns["AgregarFactura"].Width = 65;


            xtraTabControl1.SelectedTabPageIndex = 0;
        }
       
        private void lLimpiar()
        {
            //cmbProveedor.ReadOnly = false;
            txtNo.Text = string.Empty;
            lblFecha.Text = "";
            //txtDocEntry.Text = string.Empty;
            txtObservacion.Text = string.Empty;
            txtFormaPago.Text = string.Empty;
            //cmbProveedor.EditValue = Diccionario.Seleccione;
            if ((cmbTipoCompra.Properties.DataSource as IList).Count > 0) cmbTipoCompra.ItemIndex = 0;
            if ((cmbTipoOrdenPago.Properties.DataSource as IList).Count > 0) cmbTipoOrdenPago.ItemIndex = 0;
            cmbEstado.EditValue = Diccionario.Pendiente;
            //bsDatos.DataSource = new List<Factura>();
            clsComun.gLLenarCombo(ref cmbProveedor, loLogicaNegocio.goConsultarComboProveedorId(), true);

            bsDatos.DataSource = new List<Factura>();
            gcFactura.DataSource = bsDatos;

            bsArchivoAdjunto.DataSource = new List<OrdenPagoAdjunto>();
            gcArchivoAdjunto.DataSource = bsArchivoAdjunto;
            ValidarBotones();
         
        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscarOrdenPago(Convert.ToInt32(txtNo.Text.Trim()));
                txtObservacion.Text = poObject.Observacion;
                cmbEstado.EditValue = poObject.CodigoEstado;
                cmbProveedor.EditValue = poObject.IdProveedor.ToString();
                cmbTipoCompra.EditValue = poObject.CodigoTipoCompra;
                cmbTipoOrdenPago.EditValue = poObject.CodigoTipoOrdenPago;
                txtFormaPago.Text = poObject.FormaPago;
                lblFecha.Text = poObject.Fecha.ToString("dd/MM/yyyy");
                //txtDocEntry.EditValue = poObject.DocEntry;

                var poObjectFactura = loLogicaNegocio.goBuscarOrdenPagoFactura(Convert.ToInt32(txtNo.Text.Trim()));
                bsDatos.DataSource = poObjectFactura;
                dgvFactura.RefreshData();

                var poAdjunto = loLogicaNegocio.goBuscarOrdenPagoArchivoAdjunto(Convert.ToInt32(txtNo.Text.Trim()));
                if (poAdjunto !=  null)
                {
                    bsArchivoAdjunto.DataSource = poAdjunto;
                    dgvArchivoAdjunto.RefreshData();
                }
                else
                {
                    poAdjunto = new List<OrdenPagoAdjunto>();
                    bsArchivoAdjunto.DataSource = poAdjunto;
                    dgvArchivoAdjunto.RefreshData();
                }
                
                
                ValidarBotones();
            }
        }

        public void lColocarTotal(string psNameColumn, DevExpress.XtraGrid.Views.Grid.GridView dgv)
        {
            //  var psNameColumn = "Total";
            //dgvCotizacionDetalle.Columns[psNameColumn].UnboundType = UnboundColumnType.Decimal;
            dgv.Columns[psNameColumn].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgv.Columns[psNameColumn].DisplayFormat.FormatString = "c2";
            dgv.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");


            GridGroupSummaryItem item1 = new GridGroupSummaryItem();
            item1.FieldName = psNameColumn;
            item1.SummaryType = SummaryItemType.Sum;
            item1.DisplayFormat = "{0:c2}";
            item1.ShowInGroupColumnFooter = dgv.Columns[psNameColumn];
            dgv.GroupSummary.Add(item1);

        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnEnviarCorreo"] != null) tstBotones.Items["btnEnviarCorreo"].Click += btnEnviarCorreo_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;
        }

        private void ValidarBotones()
        {
            var poProveedor = (List<Factura>)bsDatos.DataSource;
            if (cmbEstado.EditValue.ToString() == Diccionario.Pendiente || cmbEstado.EditValue.ToString() == Diccionario.Corregir)
            {
                if (tstBotones.Items["btnGrabar"] != null)   tstBotones.Items["btnGrabar"].Enabled = true;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;

                dgvFactura.Columns["FechaFactura"].OptionsColumn.AllowEdit = true;
                dgvFactura.Columns["FechaVencimiento"].OptionsColumn.AllowEdit = true;
                dgvFactura.Columns["Valor"].OptionsColumn.AllowEdit = true;
                dgvFactura.Columns["AgregarFactura"].OptionsColumn.AllowEdit = true;
                dgvFactura.Columns["Del"].OptionsColumn.AllowEdit = true;
                dgvFactura.Columns["Add"].OptionsColumn.AllowEdit = true;
                dgvFactura.Columns["NoFactura"].OptionsColumn.AllowEdit = true;
                dgvFactura.Columns["Descripcion"].OptionsColumn.AllowEdit = true;

                dgvArchivoAdjunto.Columns["Descripcion"].OptionsColumn.AllowEdit = true;
                dgvArchivoAdjunto.Columns["Del"].OptionsColumn.AllowEdit = true;
                dgvArchivoAdjunto.Columns["Add"].OptionsColumn.AllowEdit = true;

                cmbTipoCompra.ReadOnly = false;
                cmbTipoOrdenPago.ReadOnly = false;
                txtFormaPago.ReadOnly = false;
                txtObservacion.ReadOnly = false;
                cmbProveedor.ReadOnly = false;
                btnAddFila.Enabled = true;
            }
            else
            {
                if (tstBotones.Items["btnGrabar"] != null)  tstBotones.Items["btnGrabar"].Enabled = false;
                if (tstBotones.Items["btnEliminar"] != null)  tstBotones.Items["btnEliminar"].Enabled = false;

                dgvFactura.Columns["FechaFactura"].OptionsColumn.AllowEdit = false;
                dgvFactura.Columns["FechaVencimiento"].OptionsColumn.AllowEdit = false;
                dgvFactura.Columns["Valor"].OptionsColumn.AllowEdit = false;
                dgvFactura.Columns["AgregarFactura"].OptionsColumn.AllowEdit = false;
                dgvFactura.Columns["Del"].OptionsColumn.AllowEdit = false;
                dgvFactura.Columns["Add"].OptionsColumn.AllowEdit = false;
                dgvFactura.Columns["NoFactura"].OptionsColumn.AllowEdit = false;
                dgvFactura.Columns["Descripcion"].OptionsColumn.AllowEdit = false;
                

                dgvArchivoAdjunto.Columns["Descripcion"].OptionsColumn.AllowEdit = false;
                dgvArchivoAdjunto.Columns["Del"].OptionsColumn.AllowEdit = false;
                dgvArchivoAdjunto.Columns["Add"].OptionsColumn.AllowEdit = false;

                

                cmbTipoCompra.ReadOnly = true;
                cmbTipoOrdenPago.ReadOnly = true;
                txtFormaPago.ReadOnly = true;
                txtObservacion.ReadOnly = true;
                cmbProveedor.ReadOnly = true;
                btnAddFila.Enabled = false;

            }
            if (poProveedor.Count>0)
            {
                //cmbProveedor.ReadOnly = true;
            }
        }

       
        private void lCalcularSaldoFila()
        {
            //Ordenes de Compra Proveedor
            var polistaProveedor = loLogicaNegocio.goBuscarListarProveedores(cmbProveedor.EditValue.ToString());
            //Todas las facturas del Grid
            var poListaFactura = (List<Factura>)bsDatos.DataSource;
            //Guarda OC ya ingresadas en el grid
            List<int> piOrdenesCompraEnGrid = new List<int>();

            foreach (var item in poListaFactura)
            {
                item.SaldoOc = 0;
                if (!string.IsNullOrEmpty(item.Observacion))
                {
                    foreach (var array in item.Observacion.Split(','))
                    {
                        if (array.Trim().Length > 0)
                        {
                            int piOc = int.Parse(array.Trim());
                            if (!piOrdenesCompraEnGrid.Contains(piOc))
                            {
                                var pdcSaldo = polistaProveedor.Where(x => x.IdProveedor == piOc).Select(x => x.Saldo).FirstOrDefault();
                                item.SaldoOc += pdcSaldo;
                                piOrdenesCompraEnGrid.Add(piOc);
                            }
                        }
                    }
                }
                //psCadenaOC = string.Format("{0}{1}", psCadenaOC, item.Observacion);
            }
        }

        GridView DGVCopiarPortapapeles;
        private void GridView1_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            if (e.MenuType != DevExpress.XtraGrid.Views.Grid.GridMenuType.Row)
                return;
            var menuItemCopyCellValue = new DevExpress.Utils.Menu.DXMenuItem("Copiar", new EventHandler(OnCopyItemClick) /*, assign an icon, if necessary */);
            DGVCopiarPortapapeles = sender as GridView;
            e.Menu.Items.Add(menuItemCopyCellValue);
        }
        void OnCopyItemClick(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(DGVCopiarPortapapeles.GetFocusedValue()?.ToString());
            }
            catch (Exception)
            {

                try
                {
                    Clipboard.SetText(DGVCopiarPortapapeles.GetFocusedValue()?.ToString());
                }
                catch (Exception)
                {

                    Clipboard.SetText(" ");
                }

            }

        }

        private void frmTrOrdenPago_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

        private void dgvFactura_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //try
            //{
            //    if (pbCargado)
            //    {

            //        if (e.Column.FieldName == "Dias")
            //        {
            //            pbCargado = false;
            //            dgvFactura.PostEditor();
            //            int piIndex = dgvFactura.GetFocusedDataSourceRowIndex();
            //            var poLista = (List<Factura>)bsDatos.DataSource;
            //            poLista[piIndex].FechaVencimiento = poLista[piIndex].FechaFactura.AddDays(poLista[piIndex].Dias);
            //            pbCargado = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void dgvFactura_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                if (pbCargado)
                {

                    if (e.Column.FieldName == "Dias" || e.Column.FieldName == "FechaFactura")
                    {
                        pbCargado = false;
                        //dgvFactura.PostEditor();
                        int piIndex = dgvFactura.GetFocusedDataSourceRowIndex();
                        var poLista = (List<Factura>)bsDatos.DataSource;
                        poLista[piIndex].FechaVencimiento = poLista[piIndex].FechaFactura.AddDays(poLista[piIndex].Dias);
                        pbCargado = true;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvFactura_ShowingEditor(object sender, CancelEventArgs e)
        {
            var psNameColumn = dgvFactura.FocusedColumn.FieldName;
            var poLista = (List<Factura>)bsDatos.DataSource;
            var piIndex = dgvFactura.GetFocusedDataSourceRowIndex();
            if (poLista[piIndex].CodigoEstado == Diccionario.Aprobado)
            {
                if (psNameColumn != "VerFactura" && psNameColumn != "Guia")
                {
                    e.Cancel = true;
                }
            }            
        }

        private void btnComentarios_Click(object sender, EventArgs e)
        {
            try
            {
                int tId = string.IsNullOrEmpty(txtNo.Text) ? 0 : int.Parse(txtNo.Text);
                var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.OrdenPago, tId);

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Comentarios" };
                pofrmBuscar.ShowDialog();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del boton de Mostrar detalle de la fila seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnGuia_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                dgvFactura.PostEditor();
                // Tomamos la fila seleccionada
                piIndex = dgvFactura.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<Factura>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poDetalle = poLista[piIndex].GuiaRemision == null ? new List<GuiaRemision>() : poLista[piIndex].GuiaRemision.ToList();

                    frmFacturaGuias frm = new frmFacturaGuias();
                    frm.lsCodCliente = loLogicaNegocio.GetCardCodeProveedor(int.Parse(cmbProveedor.EditValue.ToString()));
                    frm.Detalle = poDetalle;
                    frm.ldcTotalFactura = poLista[piIndex].Valor;
                    frm.ShowDialog();

                    if (frm.pbAcepto)
                    {
                        poLista[piIndex].GuiaRemision = frm.Detalle;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Primero, Consulta el primer registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrimero_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Primero, txtNo.Text.Trim()).ToString();
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Anterior, Consulta el anterior registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Anterior, txtNo.Text.Trim()).ToString();
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón siguiente, Consulta el siguiente registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Siguiente, txtNo.Text.Trim()).ToString();
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Último, Consulta el Último registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUltimo_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Ultimo, txtNo.Text.Trim()).ToString();
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
