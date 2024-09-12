using COM_Negocio;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
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

namespace REH_Presentacion.Compras.Transaccion
{
    public partial class frmTrGuiaRemision : frmBaseTrxDev
    {
        BindingSource bsDatos;
        BindingSource bsArchivoAdjunto;
        clsNOrdenPago loLogicaNegocio = new clsNOrdenPago();
        RepositoryItemMemoEdit rpiMedDescripcion;
        RepositoryItemButtonEdit rpiBtnDel;

        RepositoryItemButtonEdit rpiBtnAdd = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDownload = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShowArchivo = new RepositoryItemButtonEdit();

        bool pbCargado;
        public int lid = 0;
        public GuiaRemision poGuiaRemision = null;
        private string lsPrefijo;

        public frmTrGuiaRemision()
        {
            bsDatos = new BindingSource();
            bsArchivoAdjunto = new BindingSource();
            InitializeComponent();
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.MaxLength = 200;
            rpiMedDescripcion.WordWrap = true;

            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnAdd.ButtonClick += rpiBtnAdd_ButtonClick;
            rpiBtnShowArchivo.ButtonClick += rpiBtnShowArchivo_ButtonClick;
            rpiBtnDownload.ButtonClick += rpiBtnDownload_ButtonClick;
        }


        /// <summary>
        /// Evento de inicialización del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTrOrdenPago_Load(object sender, EventArgs e)
        {
            dtpFechaInicioTraslado.DateTime = DateTime.Now;
            dtpFechaFinTraslado.DateTime = DateTime.Now;

            lCargarEventosBotones();
            bsDatos.DataSource = new List<GuiaRemisionDetalle>();
            gcDatos.DataSource = bsDatos;

            bsArchivoAdjunto.DataSource = new List<GuiaRemisionAdjunto>();
            gcArchivoAdjunto.DataSource = bsArchivoAdjunto;

            pbCargado = false;
            clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstadoCotizacion(), true);
            clsComun.gLLenarCombo(ref cmbCliente, loLogicaNegocio.goSapConsultaClientesTodos(), true);
            clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoTraslado(), true);
            clsComun.gLLenarCombo(ref cmbTransportista, loLogicaNegocio.goSapConsultaComboTransportistas(), true);
            clsComun.gLLenarCombo(ref cmbTransporte, loLogicaNegocio.goSapConsultaComboTransporte(), true);
            clsComun.gLLenarCombo(ref cmbVendedor, loLogicaNegocio.goSapConsultVendedoresTodos(), true);
            clsComun.gLLenarCombo(ref cmbZonaVendedor, loLogicaNegocio.goConsultarZonasSAP(), true);
            clsComun.gLLenarCombo(ref cmbZonaTransporte, loLogicaNegocio.goConsultarZonasSAP(), true);
            clsComun.gLLenarCombo(ref cmbClaseArticulo, loLogicaNegocio.goConsultarComboClaseProducto(), false);
            clsComun.gLLenarCombo(ref cmbMotivoGuia, loLogicaNegocio.goConsultarComboMotivoGuiaManuales(), true);
            clsComun.gLLenarCombo(ref cmbExterna, loLogicaNegocio.goConsultarComboSINO(), true);
            clsComun.gLLenarCombo(ref cmbTipoTransporte, loLogicaNegocio.goConsultarComboTipoTransporte(), true);
            clsComun.gLLenarCombo(ref cmbProveedor, loLogicaNegocio.goSapConsultaProveedoresTodos(), true);


            clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goSapConsultaItems(), "ItemCode", true);
            var poLista = loLogicaNegocio.goSapConsultaComboCatalogoBodegas();
            clsComun.gLLenarCombo(ref cmbAlmacenOrigen, poLista, true);
            clsComun.gLLenarCombo(ref cmbAlmacenDestino, poLista, false);
            clsComun.gLLenarComboGrid(ref dgvDatos, poLista, "AlmacenOrigen", false);
            clsComun.gLLenarComboGrid(ref dgvDatos, poLista, "AlmacenDestino", false);

            pbCargado = true;
            cmbEstado.EditValue = Diccionario.Pendiente;
            lColumnas();

            lLimpiar();

            if (poGuiaRemision != null)
            {
                txtNo.Text = lid.ToString();
                lLlenarDatos(poGuiaRemision);
                //grp1.Enabled = false;
                //grp2.Enabled = false;
            }

            lblDocEntry.Text = "";
            lblFolioNum.Text = "";
            //lblCodAlmacen.Text = "";

            lOcultaDetalle();
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
                        bool Entro = false;
                        if (cmbAlmacenOrigen.EditValue.ToString() == Diccionario.Seleccione)
                        {
                            XtraMessageBox.Show("Seleccione Almacen de Origen", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Entro = true;
                        }

                        //if (cmbAlmacenDestino.EditValue.ToString() == Diccionario.Seleccione)
                        //{
                        //    XtraMessageBox.Show("Seleccione Almacen Destino", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    Entro = true;
                        //}

                        if (Entro)
                        {
                            return;
                        }

                        bsDatos.AddNew();
                        //cmbProveedor.ReadOnly = true;
                        dgvDatos.Focus();
                        dgvDatos.ShowEditor();
                        dgvDatos.UpdateCurrentRow();
                        var poLista = (List<GuiaRemisionDetalle>)bsDatos.DataSource;
                        poLista.LastOrDefault().ItemCode = Diccionario.Seleccione;
                        poLista.LastOrDefault().AlmacenOrigen = cmbAlmacenOrigen.EditValue.ToString();
                        poLista.LastOrDefault().AlmacenDestino = cmbAlmacenDestino.EditValue.ToString();

                        dgvDatos.RefreshData();
                        dgvDatos.FocusedColumn = dgvDatos.VisibleColumns[0];
                        lBloquearTipoItem();
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
                    piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<GuiaRemisionDetalle>)bsDatos.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        poLista.RemoveAt(piIndex);
                        bsDatos.DataSource = poLista;
                        dgvDatos.RefreshData();
                        lBloquearTipoItem();
                    }
                }
                if (xtraTabControl1.SelectedTabPageIndex == 1)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvArchivoAdjunto.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<GuiaRemisionAdjunto>)bsArchivoAdjunto.DataSource;

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
                var msg = lsEsValido();
                if (string.IsNullOrEmpty(msg))
                {
                    dgvDatos.PostEditor();
                    dgvArchivoAdjunto.PostEditor();
                    var poLista = (List<GuiaRemisionDetalle>)bsDatos.DataSource;

                    int TotalOri = poLista.Select(x => x.CantidadOriginal).Sum();
                    int TotalCant = poLista.Select(x => x.Cantidad).Sum();
                    if (TotalOri == TotalCant)
                    {
                        DialogResult dialog = XtraMessageBox.Show("La cantidad ingresada es por el total de las guía, ¿Está seguro de continuar?", Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialog != DialogResult.Yes)
                        {
                            return;
                        }                            
                    }


                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        
                        GuiaRemision PoObject = new GuiaRemision();
                        if (!string.IsNullOrEmpty(txtNo.Text))
                        {
                            PoObject.IdGuiaRemision = int.Parse(txtNo.Text);
                        }
                        if (!string.IsNullOrEmpty(lblDocEntry.Text))
                        {
                            PoObject.DocEntry = !string.IsNullOrEmpty(lblDocEntry.Text) ? int.Parse(lblDocEntry.Text) : 0;
                            PoObject.Tipo = lblDocEntry.Tag == null ? "" : lblDocEntry.Tag.ToString();
                        }
                        else
                        {
                            PoObject.Tipo = "Manual";
                        }

                        if (!string.IsNullOrEmpty(lblDocNum.Text))
                        {
                            PoObject.DocNum = int.Parse(lblDocNum.Text);
                        }

                        //if (String.IsNullOrEmpty(lblCodAlmacen.Text))
                        //{
                        //    lblCodAlmacen.Text = poLista.FirstOrDefault().AlmacenOrigen;
                        //}

                        PoObject.UsuarioSap = clsPrincipal.gsCodigoUsuarioSap;
                        PoObject.CodAlmacen = cmbAlmacenOrigen.EditValue.ToString();
                        PoObject.CodAlmacenDestino = cmbAlmacenDestino.EditValue.ToString() == Diccionario.Seleccione ? null : cmbAlmacenDestino.EditValue.ToString();
                        PoObject.PuntoLlegada = txtPuntoLlegada.Text;
                        PoObject.PuntoPartida = txtPuntoPartida.Text;
                        PoObject.Numero = txtNumero.EditValue.ToString();
                        PoObject.Local = PoObject.Numero.Substring(0, 3);
                        PoObject.PuntoEmision = PoObject.Numero.Substring(4, 3);
                        PoObject.Secuencia = PoObject.Numero.Substring(8, 9);
                        PoObject.CodCliente = cmbCliente.EditValue.ToString();
                        PoObject.Cliente = cmbCliente.Text;
                        PoObject.CodProveedor = cmbProveedor.EditValue.ToString();
                        PoObject.Proveedor = cmbProveedor.Text;
                        PoObject.CodigoMotivoTraslado = cmbMotivo.EditValue.ToString();
                        PoObject.MotivoTraslado = cmbMotivo.Text;
                        PoObject.NumBultos = string.IsNullOrEmpty(txtNumeroBultos.EditValue.ToString()) ? 0 : int.Parse(txtNumeroBultos.EditValue.ToString());
                        PoObject.CodVendedor = int.Parse(cmbVendedor.EditValue.ToString());
                        PoObject.Vendedor = cmbVendedor.Text;
                        PoObject.CodZonaVendedor = cmbZonaVendedor.EditValue.ToString();
                        PoObject.ZonaVendedor = cmbZonaVendedor.Text;
                        PoObject.CodZonaTransporte = cmbZonaTransporte.EditValue.ToString();
                        PoObject.ZonaTransporte = cmbZonaTransporte.Text;
                        PoObject.CodigoEstado = cmbEstado.EditValue.ToString();
                        PoObject.IdentificacionTransportista = txtIdentificacionTransportista.Text;
                        PoObject.IdentificacionCliente = txtIdentificacionCliente.Text;

                        PoObject.CodigoTransportista = cmbTransportista.EditValue.ToString();
                        PoObject.Transportista = cmbTransportista.Text;
                        PoObject.CodigoTransporte = cmbTransporte.EditValue.ToString();
                        PoObject.Transporte = cmbTransporte.Text;
                        PoObject.ValorBulto = string.IsNullOrEmpty(txtValorPorBulto.EditValue.ToString()) ? 0M : decimal.Parse(txtValorPorBulto.EditValue.ToString());

                        PoObject.FechaInicioTraslado = dtpFechaInicioTraslado.DateTime;
                        PoObject.FechaTerminoTraslado = dtpFechaFinTraslado.DateTime;
                        PoObject.FechaEmision = DateTime.Now;

                        PoObject.FechaContabilizacion = dtpFechaContabilizacion.DateTime;
                        PoObject.FechaDocumento = dtpFechaDocumento.DateTime;
                        PoObject.FechaEntrega = dtpFechaEntrega.DateTime;
                        PoObject.Total = string.IsNullOrEmpty(txtTotal.EditValue.ToString()) ? 0M : decimal.Parse(txtTotal.EditValue.ToString());
                        PoObject.NumeroGuia = lblNumeroGuia.Text;

                        PoObject.TipoItem = cmbClaseArticulo.EditValue.ToString();
                        PoObject.CodigoMotivoGuia = cmbMotivoGuia.EditValue.ToString();
                        PoObject.Externa = cmbExterna.EditValue.ToString();
                        PoObject.TipoTransporte = cmbTipoTransporte.EditValue.ToString();


                        if (string.IsNullOrEmpty(lblFolioNum.Text))
                        {
                            PoObject.FolioNum = null;
                        }
                        else
                        {
                            PoObject.FolioNum = int.Parse(lblFolioNum.Text);
                        }
                        PoObject.Prefijo = lsPrefijo;
                        PoObject.Comentario = txtComentario.Text;
                        PoObject.IngresoManual = true;
                        PoObject.GuiaRemisionDetalle = poLista;

                        PoObject.GuiaRemisionAdjunto = (List<GuiaRemisionAdjunto>)bsArchivoAdjunto.DataSource;

                        string psMsg = loLogicaNegocio.gsGuardarDesdeGuia(PoObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                else
                {
                    XtraMessageBox.Show(msg, "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                int piIndex = dgvArchivoAdjunto.FocusedRowHandle;
                var poLista = (List<GuiaRemisionAdjunto>)bsArchivoAdjunto.DataSource;

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rpiBtnDownload_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {

                int piIndex = dgvArchivoAdjunto.FocusedRowHandle;
                var poLista = (List<GuiaRemisionAdjunto>)bsArchivoAdjunto.DataSource;

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
        /// Evento del boton de Agregar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnAdd_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {

                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvArchivoAdjunto.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<GuiaRemisionAdjunto>)bsArchivoAdjunto.DataSource;


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
                                poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaGuiaRemision"].ToString() + Name;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private string lsEsValido()

        {
            
            string psMsg = "";

            if (txtNumero.EditValue == null)
            {
                psMsg = psMsg + "Ingrese la secuencia física. \n";
            }
            else if (string.IsNullOrEmpty(txtNumero.EditValue.ToString()))
            {
                psMsg = psMsg + "Ingrese la secuencia física. \n";
            }
            else if (txtNumero.EditValue.ToString().Contains("_"))
            {
                psMsg = psMsg + "Secuencia física ingresada no es valida. \n";
            }

            if (cmbMotivoGuia.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + "Seleccione el motivo de guía. \n";
            }

            if (cmbExterna.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + "Seleccione si es externa. \n";
            }

            if (cmbTipoTransporte.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + "Seleccione el tipo de transporte. \n";
            }

            if (cmbProveedor.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = psMsg + "Seleccione Proveedor. \n";
            }

            return psMsg;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                var menu = Tag.ToString().Split(',');
                var poListaObject = loLogicaNegocio.goListarGuiaRemision(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Usuario"),
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Cliente"),
                                    new DataColumn("Motivo"),
                                    new DataColumn("Número"),

                                    //new DataColumn("Estado"),
                                    //new DataColumn("Comentario Aprobador")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdGuiaRemision;
                    row["Usuario"] = a.Usuario;
                    row["Fecha"] = a.FechaEmision;
                    row["Cliente"] = a.Cliente; 
                    row["Motivo"] = a.MotivoTraslado;
                    row["Número"] = a.Numero;
                    //row["Estado"] = a.DesEstado;
                    //row["Comentario Aprobador"] = a.ComentarioAprobador;
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
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

        private void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = loLogicaNegocio.gdtConsultaGuiaRemisionSap(clsPrincipal.gsUsuario);

                List<string> strings = new List<string>();
                strings.Add("DocEntry");
                strings.Add("CodZona");
                strings.Add("FechaEntrega");



                frmBusqueda pofrmBuscar = new frmBusqueda(dt,null, strings) { Text = "Listado de Registros" };
                pofrmBuscar.Width = 1300;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    lLimpiar();

                    if (!string.IsNullOrEmpty(pofrmBuscar.lsCuartaColumna))
                    {
                        pbCargado = false;
                        DataSet ds = loLogicaNegocio.gdtConsultaGuiaRemisionDetalleSap(pofrmBuscar.lsCodigoSeleccionado, int.Parse(pofrmBuscar.lsSegundaColumna));

                        txtNo.Text = "";
                        cmbAlmacenOrigen.EditValue = ds.Tables[0].Rows[0]["CodAlmacen"].ToString();
                        cmbAlmacenDestino.EditValue = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CodAlmacenDestino"].ToString()) ? Diccionario.Seleccione : ds.Tables[0].Rows[0]["CodAlmacenDestino"].ToString();
                        lblDocEntry.Text = ds.Tables[0].Rows[0]["DocEntry"].ToString();
                        lblDocEntry.Tag = ds.Tables[0].Rows[0]["Tipo"].ToString();
                        lblDocNum.Text = ds.Tables[0].Rows[0]["DocNum"].ToString();
                        lblNumeroGuia.Text = ds.Tables[0].Rows[0]["Numero"].ToString();
                        cmbCliente.EditValue = ds.Tables[0].Rows[0]["CodCliente"].ToString();
                        cmbProveedor.EditValue = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CodProveedor"].ToString()) ? Diccionario.Seleccione : ds.Tables[0].Rows[0]["CodProveedor"].ToString();
                        txtIdentificacionCliente.EditValue = ds.Tables[0].Rows[0]["IdentificacionCliente"].ToString();
                        dtpFechaInicioTraslado.DateTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["FechaInicioTraslado"].ToString());
                        dtpFechaFinTraslado.DateTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["FechaFinTraslado"].ToString());
                        cmbMotivo.EditValue = ds.Tables[0].Rows[0]["Motivo"].ToString().Substring(0, 1);
                        txtPuntoPartida.EditValue = ds.Tables[0].Rows[0]["PuntoPartida"].ToString();
                        cmbTransporte.EditValue = ds.Tables[0].Rows[0]["CodTransporte"].ToString();
                        cmbTransportista.EditValue = ds.Tables[0].Rows[0]["CodTransportista"].ToString();
                        txtIdentificacionTransportista.EditValue = ds.Tables[0].Rows[0]["IdentificacionTransportista"].ToString();
                        txtPuntoLlegada.EditValue = ds.Tables[0].Rows[0]["PuntoLlegada"].ToString();
                        cmbVendedor.EditValue = ds.Tables[0].Rows[0]["CodVendedor"].ToString();
                        cmbZonaVendedor.EditValue = ds.Tables[0].Rows[0]["CodZona"].ToString();
                        cmbZonaTransporte.EditValue = ds.Tables[0].Rows[0]["CodZona"].ToString();
                        txtNumeroBultos.EditValue = ds.Tables[0].Rows[0]["NumeroBultos"].ToString();
                        dtpFechaContabilizacion.DateTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["FechaContabilizacion"].ToString());
                        dtpFechaEntrega.DateTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["FechaEntrega"].ToString());
                        dtpFechaDocumento.DateTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["FechaDocumento"].ToString());
                        //txtTotal.EditValue = ds.Tables[0].Rows[0]["Total"].ToString();
                        txtTotal.EditValue = 0;

                        cmbClaseArticulo.EditValue = "P";
                        cmbMotivoGuia.EditValue = Diccionario.Seleccione;
                        cmbExterna.EditValue = !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Externa"].ToString()) ? ds.Tables[0].Rows[0]["Externa"].ToString().ToUpper() : Diccionario.Seleccione;
                        cmbTipoTransporte.EditValue = !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TipoTransporte"].ToString()) ? ds.Tables[0].Rows[0]["TipoTransporte"].ToString() : Diccionario.Seleccione;

                        if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FolioNum"].ToString()))
                        {
                            lblFolioNum.Text = null;
                        }
                        else
                        {
                            lblFolioNum.Text = ds.Tables[0].Rows[0]["FolioNum"].ToString();
                        }

                        lsPrefijo = ds.Tables[0].Rows[0]["Prefijo"].ToString();
                        txtComentario.Text = ds.Tables[0].Rows[0]["Comentario"].ToString();

                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            bsDatos.AddNew();
                            //cmbProveedor.ReadOnly = true;
                            dgvDatos.Focus();
                            dgvDatos.ShowEditor();
                            dgvDatos.UpdateCurrentRow();
                            var poLista = (List<GuiaRemisionDetalle>)bsDatos.DataSource;
                            poLista.LastOrDefault().ItemCode = item["ItemCode"].ToString();
                            poLista.LastOrDefault().LineNum = int.Parse(item["LineNum"].ToString());
                            poLista.LastOrDefault().CantidadOriginal = int.Parse(item["CantidadOriginal"].ToString());
                            poLista.LastOrDefault().CantidadTomada = int.Parse(item["CantidadTomada"].ToString());
                            poLista.LastOrDefault().AlmacenOrigen = item["AlmacenOrigen"].ToString();
                            poLista.LastOrDefault().AlmacenDestino = item["AlmacenDestino"].ToString();


                            dgvDatos.RefreshData();
                            dgvDatos.FocusedColumn = dgvDatos.VisibleColumns[1];
                        }

                        cmbClaseArticulo.ReadOnly = true;

                        lControlarControles();

                        pbCargado = true;
                    }
                    else
                    {
                        var msg = string.Format("No es posible seleccionar guías sin relación del transportista con su proveedor. Comunicarse con Contabilidad");
                        XtraMessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                        var psMsg = loLogicaNegocio.gEliminarMaestroGuiaRemision(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


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
                    var poProveedor = (List<GuiaRemisionDetalle>)bsDatos.DataSource;
                    var piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                    int lIdOrdenCompra = int.Parse(txtNo.Text);
                    int lIdProveedor = int.Parse(cmbCliente.EditValue.ToString());
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
                    clsComun.gLLenarCombo(ref cmbCliente, loLogicaNegocio.goConsultarComboProveedorId(), true);

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

        private void lColumnas()
        {
            dgvDatos.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            dgvDatos.OptionsView.RowAutoHeight = true;

            dgvDatos.Columns["IdGuiaRemisionDetalle"].Visible = false;
            dgvDatos.Columns["IdGuiaRemision"].Visible = false;
            dgvDatos.Columns["ItemName"].Visible = false;
            dgvDatos.Columns["GuiaRemision"].Visible = false;
            dgvDatos.Columns["LineNum"].Visible = false;
            dgvDatos.Columns["TipoItem"].Visible = false;
            dgvDatos.Columns["Prorrateo"].Visible = false;

            if (new clsNParametroCompras().goBuscarEntidad().PermiteSeleccionarDiferentesBodegasEnGuias)
            {
                dgvDatos.Columns["AlmacenOrigen"].OptionsColumn.AllowEdit = true;
                dgvDatos.Columns["AlmacenDestino"].OptionsColumn.AllowEdit = true;
            }
            else
            {
                dgvDatos.Columns["AlmacenOrigen"].OptionsColumn.AllowEdit = false;
                dgvDatos.Columns["AlmacenDestino"].OptionsColumn.AllowEdit = false;
            }

            dgvDatos.Columns["NombreAlmacenOrigen"].Visible = false;
            dgvDatos.Columns["NombreAlmacenDestino"].Visible = false;

            dgvDatos.Columns["CodigoAlmacenOrigen"].Visible = false;
            dgvDatos.Columns["CodigoAlmacenDestino"].Visible = false;

            dgvDatos.Columns["CantidadTomada"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["CantidadOriginal"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["ItemCodeString"].Caption = "ItemCode";
            dgvDatos.Columns["ItemCode"].Caption = "Descripción";
            dgvDatos.Columns["CantidadOriginal"].Caption = "Cantidad Total en Guia";
            dgvDatos.Columns["CantidadTomada"].Caption = "Cantidad Despachada";
            dgvDatos.Columns["Cantidad"].Caption = "Cantidad a Despachar";
            dgvDatos.Columns["Saldo"].Caption = "Cantidad Pendiente";

            dgvDatos.Columns["CodigoAlmacenOrigen"].Caption = "Codigo";
            dgvDatos.Columns["CodigoAlmacenDestino"].Caption = "Codigo";
            dgvDatos.Columns["AlmacenOrigen"].Caption = "Almacen Origen";



            dgvDatos.Columns["ItemCodeString"].Width = 50;
            dgvDatos.Columns["ItemCode"].Width = 200;
            dgvDatos.Columns["CodigoAlmacenOrigen"].Width = 30;
            dgvDatos.Columns["CodigoAlmacenDestino"].Width = 30;

            //dgvDatos.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;


            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 30);


            dgvArchivoAdjunto.Columns["IdGuiaRemisionAdjunto"].Visible = false;
            dgvArchivoAdjunto.Columns["IdGuiaRemision"].Visible = false;
            //dgvArchivoAdjunto.Columns["NombreOriginal"].Visible = false;
            dgvArchivoAdjunto.Columns["RutaDestino"].Visible = false;
            dgvArchivoAdjunto.Columns["RutaOrigen"].Visible = false;
            dgvArchivoAdjunto.Columns["ArchivoAdjunto"].Visible = false;
            dgvArchivoAdjunto.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvArchivoAdjunto.Columns["NombreOriginal"].Caption = "Archivo Adjunto";

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvArchivoAdjunto.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);
            clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvArchivoAdjunto.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnDownload, dgvArchivoAdjunto.Columns["Descargar"], "Descargar", Diccionario.ButtonGridImage.download_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowArchivo, dgvArchivoAdjunto.Columns["Visualizar"], "Visualizar", Diccionario.ButtonGridImage.show_16x16);

            xtraTabControl1.SelectedTabPageIndex = 0;
        }

        private void lLimpiar()
        {
            pbCargado = false;
            txtNo.Text = string.Empty;
            lblFecha.Text = string.Empty;
            lblDocEntry.Text = string.Empty;
            lblDocEntry.Tag = string.Empty;
            lblDocNum.Text = string.Empty;
            lblNumeroGuia.Text = string.Empty;

            txtNumero.EditValue = string.Empty;
            txtPuntoLlegada.Text = string.Empty;
            txtIdentificacionCliente.Text = string.Empty;
            txtIdentificacionTransportista.Text = string.Empty;
            txtNumeroBultos.EditValue = 0;
            txtPuntoPartida.Text = string.Empty;
            txtValorPorBulto.EditValue = 0;

            dtpFechaInicioTraslado.DateTime = DateTime.Now;
            dtpFechaFinTraslado.DateTime = DateTime.Now;

            //cmbProveedor.EditValue = Diccionario.Seleccione;
            if ((cmbTransporte.Properties.DataSource as IList).Count > 0) cmbTransporte.ItemIndex = 0;
            if ((cmbTransportista.Properties.DataSource as IList).Count > 0) cmbTransportista.ItemIndex = 0;
            if ((cmbCliente.Properties.DataSource as IList).Count > 0) cmbCliente.ItemIndex = 0;
            if ((cmbMotivo.Properties.DataSource as IList).Count > 0) cmbMotivo.ItemIndex = 0;
            if ((cmbVendedor.Properties.DataSource as IList).Count > 0) cmbVendedor.ItemIndex = 0;
            if ((cmbZonaVendedor.Properties.DataSource as IList).Count > 0) cmbZonaVendedor.ItemIndex = 0;
            if ((cmbZonaTransporte.Properties.DataSource as IList).Count > 0) cmbZonaTransporte.ItemIndex = 0;
            if ((cmbMotivoGuia.Properties.DataSource as IList).Count > 0) cmbMotivoGuia.ItemIndex = 0;
            
            if ((cmbProveedor.Properties.DataSource as IList).Count > 0) cmbProveedor.ItemIndex = 0;
            if ((cmbTipoTransporte.Properties.DataSource as IList).Count > 0) cmbTipoTransporte.ItemIndex = 0;
            if ((cmbExterna.Properties.DataSource as IList).Count > 0) cmbExterna.ItemIndex = 0;
            if ((cmbAlmacenOrigen.Properties.DataSource as IList).Count > 0) cmbAlmacenOrigen.ItemIndex = 0;
            if ((cmbAlmacenDestino.Properties.DataSource as IList).Count > 0) cmbAlmacenDestino.ItemIndex = 0;

            cmbEstado.EditValue = Diccionario.Pendiente;

            dtpFechaContabilizacion.DateTime = DateTime.Now;
            dtpFechaEntrega.DateTime = DateTime.Now;
            dtpFechaDocumento.DateTime = DateTime.Now;
            txtTotal.EditValue = 0;
            lblFolioNum.Text = null;
            lsPrefijo = string.Empty;
            txtComentario.Text = string.Empty;

            cmbClaseArticulo.ReadOnly = false;


            bsDatos.DataSource = new List<GuiaRemisionDetalle>();
            gcDatos.DataSource = bsDatos;

            bsArchivoAdjunto.DataSource = new List<GuiaRemisionAdjunto>();
            gcArchivoAdjunto.DataSource = bsArchivoAdjunto;

            lControlarControles();
            lBloquearTipoItem();

            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Enabled = true;

            pbCargado = true;

            xtraTabControl1.SelectedTabPageIndex = 0;
            if ((cmbClaseArticulo.Properties.DataSource as IList).Count > 0) cmbClaseArticulo.ItemIndex = 0;


        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscarGuiaRemision(Convert.ToInt32(txtNo.Text.Trim()));
                lLlenarDatos(poObject);
            }
        }

        private void lLlenarDatos(GuiaRemision poObject)
        {
            pbCargado = false;

            if (poObject.IngresoManual)
            {
                txtNumero.Text = poObject.Numero;
            }
            

            txtPuntoLlegada.Text = poObject.PuntoLlegada;
            txtPuntoPartida.Text = poObject.PuntoPartida;
            cmbEstado.EditValue = poObject.CodigoEstado;
            cmbCliente.EditValue = poObject.CodCliente;
            cmbProveedor.EditValue = poObject.CodProveedor;
            cmbTransportista.EditValue = poObject.CodigoTransportista;
            cmbTransporte.EditValue = poObject.CodigoTransporte;
            cmbMotivo.EditValue = poObject.CodigoMotivoTraslado;
            cmbVendedor.EditValue = poObject.CodVendedor.ToString();
            cmbZonaTransporte.EditValue = poObject.CodZonaTransporte;
            cmbZonaVendedor.EditValue = poObject.CodZonaVendedor;
            
            txtNumeroBultos.Text = poObject.NumBultos.ToString();
            lblFecha.Text = poObject.FechaEmision.ToString("dd/MM/yyyy");
            cmbAlmacenOrigen.EditValue = poObject.CodAlmacen;
            cmbAlmacenDestino.EditValue = poObject.CodAlmacenDestino;

            lblDocEntry.Text = poObject.DocEntry.ToString();
            lblDocEntry.Tag = poObject.Tipo;
            lblDocNum.Text = poObject.DocNum.ToString();
            lblNumeroGuia.Text = poObject.NumeroGuia;

            txtIdentificacionCliente.Text = poObject.IdentificacionCliente;
            txtIdentificacionTransportista.Text = poObject.IdentificacionTransportista;
            txtValorPorBulto.EditValue = poObject.ValorBulto;

            dtpFechaInicioTraslado.DateTime = poObject.FechaInicioTraslado;
            dtpFechaFinTraslado.DateTime = poObject.FechaTerminoTraslado;

            dtpFechaContabilizacion.DateTime = poObject.FechaContabilizacion;
            dtpFechaEntrega.DateTime = poObject.FechaEntrega ?? DateTime.MinValue;
            dtpFechaDocumento.DateTime = poObject.FechaDocumento;
            txtTotal.EditValue = poObject.Total;
            lblFolioNum.Text = poObject.FolioNum?.ToString();
            lsPrefijo = poObject.Prefijo;
            txtComentario.Text = poObject.Comentario;

            cmbMotivoGuia.EditValue = poObject.CodigoMotivoGuia;
            pbCargado = true;
            cmbClaseArticulo.EditValue = poObject.TipoItem;
            pbCargado = false;
            cmbExterna.EditValue = poObject.Externa;
            cmbTipoTransporte.EditValue = poObject.TipoTransporte;


            var poDetalle = poObject.GuiaRemisionDetalle;
            bsDatos.DataSource = poDetalle;
            dgvDatos.RefreshData();

            var poAdjuntos = poObject.GuiaRemisionAdjunto;
            bsArchivoAdjunto.DataSource = poAdjuntos;
            dgvArchivoAdjunto.RefreshData();

            if (poObject.IngresoManual)
            {
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
                if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Enabled = true;
            }
            else
            {
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Enabled = false;
            }

            cmbClaseArticulo.ReadOnly = true;

            lControlarControles();

            pbCargado = true;
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Click += btnImportar_Click;

            txtNumeroBultos.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtValorPorBulto.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtTotal.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
        }

        private void frmTrOrdenPago_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

        private void btnComentarios_Click(object sender, EventArgs e)
        {
            try
            {
                //int tId = string.IsNullOrEmpty(txtNo.Text) ? 0 : int.Parse(txtNo.Text);
                //var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.OrdenPago, tId);

                //frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Comentarios" };
                //pofrmBuscar.ShowDialog();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbTrasnportista_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    txtIdentificacionTransportista.Text = loLogicaNegocio.gsGetIdentificacionTransportista(cmbTransportista.EditValue.ToString());
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCompletarPuntoPartida_Click(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    //if (cmbTrasnportista.EditValue.ToString() != Diccionario.Seleccione)
                    //{
                    //    var dt = loLogicaNegocio.goConsultaDataTable("");
                    //}
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbCliente_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    var poObject = loLogicaNegocio.goDatosClienteGuiaRemision(cmbCliente.EditValue.ToString());
                    txtIdentificacionCliente.EditValue = poObject.IdentificacionCliente;
                    txtPuntoLlegada.Text = poObject.PuntoLlegada;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lControlarControles()
        {
            if (poGuiaRemision == null)
            {
                if (!string.IsNullOrEmpty(lblDocEntry.Text))
                {
                    //txtPuntoLlegada.ReadOnly = true;
                    //txtPuntoPartida.ReadOnly = true;
                    cmbCliente.ReadOnly = true;
                    //cmbTrasnportista.ReadOnly = true;
                    //cmbTransporte.ReadOnly = true;
                    cmbMotivo.ReadOnly = true;
                    //txtTotal.ReadOnly = true;
                    cmbVendedor.ReadOnly = true;
                    cmbAlmacenDestino.ReadOnly = true;
                    cmbAlmacenOrigen.ReadOnly = true;
                    //txtNumero.ReadOnly = true;

                    //txtIdentificacionCliente.ReadOnly = true;
                    //txtIdentificacionTransportista.ReadOnly = true;

                    //dtpFechaInicioTraslado.ReadOnly = true;
                    //dtpFechaFinTraslado.ReadOnly = true;

                    btnAddFila.Enabled = false;
                    //btnCompletarPuntoPartida.Enabled = false;

                    dgvDatos.Columns["ItemCode"].OptionsColumn.AllowEdit = false;
                    dgvDatos.Columns["Saldo"].OptionsColumn.AllowEdit = false;
                    dgvDatos.Columns["AlmacenOrigen"].OptionsColumn.AllowEdit = false;
                    dgvDatos.Columns["AlmacenDestino"].OptionsColumn.AllowEdit = false;
                }
                else
                {
                    //txtPuntoLlegada.ReadOnly = false;
                    //txtPuntoPartida.ReadOnly = false;
                    cmbCliente.ReadOnly = false;
                    //txtTotal.ReadOnly = false;
                    cmbVendedor.ReadOnly = false;
                    cmbAlmacenDestino.ReadOnly = false;
                    cmbAlmacenOrigen.ReadOnly = false;
                    //cmbTrasnportista.ReadOnly = false;
                    //cmbTransporte.ReadOnly = false;
                    cmbMotivo.ReadOnly = false;
                    //txtNumero.ReadOnly = false;

                    //txtIdentificacionCliente.ReadOnly = false;
                    //txtIdentificacionTransportista.ReadOnly = false;

                    //dtpFechaInicioTraslado.ReadOnly = false;
                    //dtpFechaFinTraslado.ReadOnly = false;

                    btnAddFila.Enabled = true;
                    //btnCompletarPuntoPartida.Enabled = true;

                    dgvDatos.Columns["ItemCode"].OptionsColumn.AllowEdit = true;
                    dgvDatos.Columns["Saldo"].OptionsColumn.AllowEdit = true;
                    dgvDatos.Columns["AlmacenOrigen"].OptionsColumn.AllowEdit = true;
                    dgvDatos.Columns["AlmacenDestino"].OptionsColumn.AllowEdit = true;
                }
            }
            else
            {
                tstBotones.Enabled = false;
                txtNumeroBultos.ReadOnly = true;
                cmbZonaTransporte.ReadOnly = true;
                txtValorPorBulto.ReadOnly = true;
                dgvDatos.Columns["Cantidad"].OptionsColumn.AllowEdit = false;
                dgvDatos.Columns["Del"].OptionsColumn.AllowEdit = false;
                dgvDatos.Columns["ItemCode"].OptionsColumn.AllowEdit = false;
                dgvDatos.Columns["AlmacenOrigen"].OptionsColumn.AllowEdit = false;
                dgvDatos.Columns["AlmacenDestino"].OptionsColumn.AllowEdit = false;
                txtNumero.ReadOnly = true;
                txtPuntoLlegada.ReadOnly = true;
                txtPuntoPartida.ReadOnly = true;
                cmbCliente.ReadOnly = true;
                cmbTransportista.ReadOnly = true;
                cmbTransporte.ReadOnly = true;
                cmbMotivo.ReadOnly = true;
                txtNumero.ReadOnly = true;
                cmbAlmacenDestino.ReadOnly = true;
                cmbAlmacenOrigen.ReadOnly = true;
                txtIdentificacionCliente.ReadOnly = true;
                txtIdentificacionTransportista.ReadOnly = true;
                dtpFechaInicioTraslado.ReadOnly = true;
                dtpFechaFinTraslado.ReadOnly = true;
                dtpFechaContabilizacion.ReadOnly = true;
                dtpFechaDocumento.ReadOnly = true;
                dtpFechaEntrega.ReadOnly = true;
                txtTotal.ReadOnly = true;
                cmbVendedor.ReadOnly = true;
                txtComentario.ReadOnly = true;
                cmbTipoTransporte.ReadOnly = true;
                cmbClaseArticulo.ReadOnly = true;
                cmbExterna.ReadOnly = true;
                cmbMotivoGuia.ReadOnly = true;
                btnAddFila.Enabled = false;
                cmbProveedor.ReadOnly = true;
            }
        }

        private void cmbVendedor_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    string psCadena = loLogicaNegocio.gsGetCodZonaDelVendedor(cmbVendedor.EditValue.ToString());
                    cmbZonaVendedor.EditValue = psCadena;
                    cmbZonaTransporte.EditValue = psCadena;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbClaseArticulo_EditValueChanged(object sender, EventArgs e)
        {
            lOcultaDetalle();
        }

        private void lOcultaDetalle()
        {
            try
            {
                if (pbCargado)
                {
                    dgvDatos.Columns["ItemCodeString"].Visible = false;
                    dgvDatos.Columns["ItemCode"].Visible = false;
                    dgvDatos.Columns["DescripcionServicio"].Visible = false;
                    //dgvDatos.Columns["CodigoAlmacenOrigen"].Visible = false;
                    dgvDatos.Columns["AlmacenOrigen"].Visible = false;
                    //dgvDatos.Columns["CodigoAlmacenDestino"].Visible = false;
                    dgvDatos.Columns["AlmacenDestino"].Visible = false;
                    dgvDatos.Columns["Cantidad"].Visible = false;
                    dgvDatos.Columns["CantidadOriginal"].Visible = false;
                    dgvDatos.Columns["CantidadTomada"].Visible = false;
                    dgvDatos.Columns["Saldo"].Visible = false;
                    dgvDatos.Columns["Del"].Visible = false;

                    if (cmbClaseArticulo.EditValue.ToString() == "P")
                    {
                        dgvDatos.Columns["ItemCodeString"].Visible = true;
                        dgvDatos.Columns["ItemCode"].Visible = true;
                        dgvDatos.Columns["DescripcionServicio"].Visible = false;
                        //dgvDatos.Columns["CodigoAlmacenOrigen"].Visible = true;
                        dgvDatos.Columns["AlmacenOrigen"].Visible = true;
                        //dgvDatos.Columns["CodigoAlmacenDestino"].Visible = true;
                        dgvDatos.Columns["AlmacenDestino"].Visible = true;
                        dgvDatos.Columns["Cantidad"].Visible = true;
                        dgvDatos.Columns["CantidadOriginal"].Visible = true;
                        dgvDatos.Columns["CantidadTomada"].Visible = true;
                        dgvDatos.Columns["Saldo"].Visible = true;
                        dgvDatos.Columns["Del"].Visible = true;
                    }
                    else
                    {
                        dgvDatos.Columns["ItemCodeString"].Visible = false;
                        dgvDatos.Columns["ItemCode"].Visible = false;
                        dgvDatos.Columns["DescripcionServicio"].Visible = true;
                        //dgvDatos.Columns["CodigoAlmacenOrigen"].Visible = true;
                        dgvDatos.Columns["AlmacenOrigen"].Visible = true;
                        //dgvDatos.Columns["CodigoAlmacenDestino"].Visible = true;
                        dgvDatos.Columns["AlmacenDestino"].Visible = true;
                        dgvDatos.Columns["Cantidad"].Visible = true;
                        dgvDatos.Columns["CantidadOriginal"].Visible = true;
                        dgvDatos.Columns["CantidadTomada"].Visible = true;
                        dgvDatos.Columns["Saldo"].Visible = true;
                        dgvDatos.Columns["Del"].Visible = true;

                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void lBloquearTipoItem()
        {
            var poLista = (List<GuiaRemisionDetalle>)bsDatos.DataSource;

            if (poLista.Count > 0)
            {
                cmbClaseArticulo.ReadOnly = true;

            }
            else
            {
                cmbClaseArticulo.ReadOnly = false;
            }

        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (poGuiaRemision == null)
                {
                    if (xtraTabControl1.SelectedTabPageIndex == 1)
                    {
                        btnAddFila.Enabled = true;
                    }
                    else
                    {
                        btnAddFila.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbProveedor_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    //clsComun.gLLenarCombo(ref cmbTransportista, new List<Combo>(), true);
                    //if (cmbProveedor.EditValue.ToString() != Diccionario.Activo)
                    //{
                    //    var poLista = loLogicaNegocio.gsGetComboTransportistaDesdeProveedor(cmbProveedor.EditValue.ToString());
                    //    clsComun.gLLenarCombo(ref cmbTransportista, poLista, true);
                    //}
                    

                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
