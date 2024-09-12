using COM_Negocio;
using DevExpress.Utils.Drawing;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using GEN_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Compras.Transaccion
{
    /// <summary>
    /// Formulario de Solicitud de Compra
    /// </summary>
    public partial class frmTrSolicitudCompras : frmBaseTrxDev
    {
        #region Variables
        clsNSolicitudCompra loLogicaNegocio;
        BindingSource bsDatos;
        RepositoryItemButtonEdit rpiBtnDel;
        RepositoryItemButtonEdit rpiBtnAdd;
        RepositoryItemButtonEdit rpiBtnAddDetalle;
        RepositoryItemButtonEdit rpiBtnDownload;
        RepositoryItemButtonEdit rpiBtnShow;
        RepositoryItemButtonEdit rpiBtnShowDetalle;
        BindingSource bsArchivoAdjunto;
        bool lbCargado;
        bool anteriorSap;
        public int lId;
        public List<Combo> ItemsSAP;
        RepositoryItemMemoEdit rpiMedDescripcion;
        #endregion


        #region Eventos

        public frmTrSolicitudCompras()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNSolicitudCompra();
            bsDatos = new BindingSource();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnAdd = new RepositoryItemButtonEdit();
            rpiBtnAddDetalle = new RepositoryItemButtonEdit();
            rpiBtnDownload = new RepositoryItemButtonEdit();
            rpiBtnShow = new RepositoryItemButtonEdit();
            rpiBtnShowDetalle = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnAdd.ButtonClick += rpiBtnAdd_ButtonClick;
            rpiBtnAddDetalle.ButtonClick += rpiBtnAddDetalle_ButtonClick;
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnShowDetalle.ButtonClick += rpiBtnShowDetalle_ButtonClick;
            rpiBtnDownload.ButtonClick += rpiBtnDownload_ButtonClick;

            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.MaxLength = 120;
            rpiMedDescripcion.WordWrap = true;
            bsArchivoAdjunto = new BindingSource();

        }

        /// <summary>
        /// Evento load del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void frmTrSolicitudCompras_Load(object sender, EventArgs e)
        {
            try
            {
                var x = Tag;
                lbCargado = false;
                clsComun.gLLenarCombo(ref cmbTipoItem, loLogicaNegocio.goConsultarComboTipoItems(), true);
                clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstadoSolicituCompra(), true);
                ItemsSAP =loLogicaNegocio.goConsultarComboGridItemSap();
                cmbEstado.ReadOnly = true;
               
                bsDatos.DataSource = new List<SolicitudCompraDetalle>();
                gcDatos.DataSource = bsDatos;
                bsArchivoAdjunto.DataSource = new List<SolicitudCompraArchivoAdjunto>();
                gcArchivoAdjunto.DataSource = bsArchivoAdjunto;
                xtraTabControl1.SelectedTabPageIndex = 0;

                dtpFechaEntrega.EditValue = DateTime.Now;
                cmbEstado.EditValue = Diccionario.Pendiente;

                lColumnas();
                clsComun.gLLenarComboGrid(ref dgvDatos, ItemsSAP, "ItemSap", true);

                
                lCargarEventosBotones();

                if (lId != 0)
                {
                    txtNo.Text = lId.ToString();
                    lConsultar();
                }
                lValidarBotones();
                lbCargado = true;
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
                    var poLista = (List<SolicitudCompraDetalle>)bsDatos.DataSource;

                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        // Tomamos la entidad de la fila seleccionada
                        var poEntidad = poLista[piIndex];

                        // Eliminar Fila seleccionada de mi lista
                        //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source
                        bsDatos.DataSource = poLista;
                        dgvDatos.RefreshData();
                    }
                }
                if (xtraTabControl1.SelectedTabPageIndex == 1)
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvArchivoAdjunto.GetFocusedDataSourceRowIndex();
                    // Tomamos la lista del Grid
                    var poLista = (List<SolicitudCompraArchivoAdjunto>)bsArchivoAdjunto.DataSource;

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
                MessageBox.Show(ex.Message);
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
                var poLista = (List<SolicitudCompraArchivoAdjunto>)bsArchivoAdjunto.DataSource;


                // Presenta un dialogo para seleccionar las imagenes
                ofdArchicoAdjunto.Title = "Seleccione Archivo";
                ofdArchicoAdjunto.Filter = "Image Files( *.pdf; )|*.pdf|All files (*.*)|*.*";

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
                                poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaSolCompras"].ToString() + Name;
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

        /// <summary>
        /// Evento del boton de Agregar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnAddDetalle_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<SolicitudCompraDetalle>)bsDatos.DataSource;


                // Presenta un dialogo para seleccionar las imagenes
                ofdArchicoAdjunto.Title = "Seleccione Archivo";
                ofdArchicoAdjunto.Filter = "Image Files(*.jpg )|  *.jpg; |Pdf Files(*.pdf;) |*.pdf|All files (*.*)|*.*";

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
                                poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaSolCompras"].ToString() + Name;
                                // Asigno mi nueva lista al Binding Source
                                bsDatos.DataSource = poLista;
                                dgvDatos.RefreshData();
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

        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvArchivoAdjunto.FocusedRowHandle;
                var poLista = (List<SolicitudCompraArchivoAdjunto>)bsArchivoAdjunto.DataSource;

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
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShowDetalle_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvDatos.FocusedRowHandle;
                var poLista = (List<SolicitudCompraDetalle>)bsDatos.DataSource;

                if (!string.IsNullOrEmpty(poLista[piIndex].ArchivoAdjunto))
                {
                    string extension = System.IO.Path.GetExtension(poLista[piIndex].ArchivoAdjunto);

                    //Archivo PDF
                    if (extension.Equals(".pdf"))
                    {
                        frmVerPdf pofrmVerPdf = new frmVerPdf();
                        //Muestra archivo local
                        if (!string.IsNullOrEmpty(poLista[piIndex].RutaOrigen))
                        {
                            pofrmVerPdf.lsRuta = poLista[piIndex].RutaOrigen;
                            pofrmVerPdf.Show();
                            pofrmVerPdf.SetDesktopLocation(0, 0);
                            pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;
                        }
                        //Muestra archivo ya subido
                        else
                        {

                           // Process.Start(ConfigurationManager.AppSettings["CarpetaSolCompras"].ToString() + poLista[piIndex].ArchivoAdjunto);
                            pofrmVerPdf.lsRuta = ConfigurationManager.AppSettings["CarpetaSolCompras"].ToString() + poLista[piIndex].ArchivoAdjunto;
                            pofrmVerPdf.Show();
                            pofrmVerPdf.SetDesktopLocation(0, 0);
                            pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;
                        }
                    }

                    //Archivo JPG
                    if (extension.Equals(".jpg"))
                    {
                        //Muestra archivo local
                        if (!string.IsNullOrEmpty(poLista[piIndex].RutaOrigen))
                        {
                            Process.Start(poLista[piIndex].RutaOrigen);
                        }
                        //Muestra archivo ya subido
                        else
                        {
                            string psRuta = ConfigurationManager.AppSettings["CarpetaSolCompras"].ToString() + poLista[piIndex].ArchivoAdjunto;
                            Process.Start(psRuta);
                        }
                    }
                }

                else
                {
                    XtraMessageBox.Show("No hay archivo para mostrar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void rpiBtnDownload_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {

                int piIndex = dgvArchivoAdjunto.FocusedRowHandle;
                var poLista = (List<SolicitudCompraArchivoAdjunto>)bsArchivoAdjunto.DataSource;

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
        /// Evento del boton Grabar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                dgvArchivoAdjunto.PostEditor();
                SolicitudCompra poObject = new SolicitudCompra();
                if (!string.IsNullOrEmpty(txtNo.Text))
                {
                    poObject.IdSolicitudCompra = Int32.Parse(txtNo.Text.ToString());
                }
                
                if (!string.IsNullOrEmpty(dtpFechaEntrega.EditValue.ToString()))
                {
                    poObject.FechaEntrega = Convert.ToDateTime(dtpFechaEntrega.EditValue.ToString());
                }
                poObject.ArchivoAdjunto = (List<SolicitudCompraArchivoAdjunto>)bsArchivoAdjunto.DataSource;
                poObject.CodigoTipoItem = cmbTipoItem.EditValue.ToString();
                poObject.Observacion = txtObservacion.Text.ToString();
                poObject.Descripcion = txtDescripcion.Text.ToString();
                poObject.CodigoEstado = Diccionario.Pendiente;
             
                poObject.Usuario = clsPrincipal.gsUsuario;
                poObject.Terminal = clsPrincipal.gsTerminal;
                poObject.SolicitudCompraDetalle = (List<SolicitudCompraDetalle>)bsDatos.DataSource;
                int pId = 0;
                string psMsg = loLogicaNegocio.gsGuardar(poObject, out pId);
                if (string.IsNullOrEmpty(psMsg))
                {
                    txtNo.EditValue = pId;
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    
                    lImprimir();
                    lLimpiar();
                }
                else
                {
                    XtraMessageBox.Show(psMsg,  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


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

                var menu = Tag.ToString().Split(',');
                List<SolicitudCompra> poListaObject = loLogicaNegocio.goListarMaestro(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Usuario"),
                                    new DataColumn("Fecha Solicitud",typeof(DateTime)),
                                    new DataColumn("Descripción"),
                                    new DataColumn("Tipo Item"),
                                    new DataColumn("Estado"),
                                    new DataColumn("Comentario Aprobador")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdSolicitudCompra;
                    row["Usuario"] = a.DesUsuario;
                    row["Fecha Solicitud"] = a.FechaIngreso;
                    row["Descripción"] = a.Descripcion;
                    row["Tipo Item"] = a.DesTipoItem;
                    row["Estado"] = a.DesEstado;
                    row["Comentario Aprobador"] = a.ComentarioAprobador;

                    dt.Rows.Add(row);
                });

                var psListaColumnRepItem = new List<GridBusqueda>();
                psListaColumnRepItem.Add(new GridBusqueda() { Columna = "Descripción", Ancho = 50 });
                frmBusqueda pofrmBuscar = new frmBusqueda(dt, psListaColumnRepItem) { Text = "Listado de Registros" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar();
                    
                }
                lValidarBotones();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {

                if (xtraTabControl1.SelectedTabPageIndex == 0)
                {
                    bsDatos.AddNew();
                    dgvDatos.Focus();
                    dgvDatos.ShowEditor();
                    dgvDatos.UpdateCurrentRow();
                    dgvDatos.RefreshData();
                    dgvDatos.FocusedColumn = dgvDatos.VisibleColumns[0];
                }
                if (xtraTabControl1.SelectedTabPageIndex == 1)
                {
                    bsArchivoAdjunto.AddNew();
                    dgvArchivoAdjunto.Focus();
                    dgvArchivoAdjunto.ShowEditor();
                    dgvArchivoAdjunto.UpdateCurrentRow();
                    dgvArchivoAdjunto.RefreshData();
                    dgvDatos.FocusedColumn = dgvDatos.VisibleColumns[0];
                }



            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Eliminar, Cambia a estado eliminado un registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
                {
                    loLogicaNegocio.gActualizarEstadoSolicitud(Convert.ToInt16(txtNo.Text.Trim()), Diccionario.Aprobado,txtObservacion.Text, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.Aprobado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lLimpiar();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCorregir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
                {
                    loLogicaNegocio.gActualizarEstadoSolicitud(Convert.ToInt16(txtNo.Text.Trim()), Diccionario.Corregir, txtObservacion.Text, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.Aprobado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lLimpiar();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnRechazar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
                {
                    loLogicaNegocio.gActualizarEstadoSolicitud(Convert.ToInt16(txtNo.Text.Trim()), Diccionario.Negado, txtObservacion.Text, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.Aprobado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lLimpiar();
                }
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
                if (txtNo.EditValue != null)
                {
                    lImprimir();
                }
                else
                {
                    XtraMessageBox.Show("No existe detalles guardados para imprimir.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (cmbTipoItem.EditValue != "0")
                {
                    List<SolicitudCompraDetalle> poLista = (List<SolicitudCompraDetalle>)bsDatos.DataSource;
                    if (!loLogicaNegocio.gItemSap(cmbTipoItem.EditValue.ToString()))
                    {
                        XtraMessageBox.Show("El Formato de Excel para importar dete tener las siguientes columnas: ( 'Cantidad', 'Descripción', 'Observación' )", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        OpenFileDialog ofdRuta = new OpenFileDialog();
                        ofdRuta.Title = "Seleccione Archivo";
                        //(*.jpg; *.jpeg; *.png)| *.jpg; *.jpeg; *.png; "
                        ofdRuta.Filter = "Files(*.xls; *.xlsx;)|*.xls; *.xlsx;";

                        if (ofdRuta.ShowDialog() == DialogResult.OK)
                        {
                            if (!ofdRuta.FileName.Equals(""))
                            {

                                DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);

                                foreach (DataRow item in dt.Rows)
                                {
                                    if (!string.IsNullOrEmpty(item[0].ToString().Trim()))
                                    {
                                        SolicitudCompraDetalle poItem = new SolicitudCompraDetalle();
                                        poItem.Cantidad = Int32.Parse(item[0].ToString().Trim());
                                        poItem.Descripcion = item[1].ToString().Trim();
                                        poItem.Observacion = item[2].ToString().Trim();
                                        poItem.CodigoEstado = Diccionario.Activo;

                                        poLista.Add(poItem);
                                    }
                                }
                            }
                            //bsDatos.DataSource = new List<SolicitudCompraDetalle>();
                            //gcDatos.DataSource = null;
                            bsDatos.DataSource = poLista.ToList();
                            gcDatos.DataSource = bsDatos;

                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("El Formato de Excel para importar dete tener las siguientes columnas: ( 'Codigo Item SAP', 'Cantidad', 'Descripción', 'Observación' )", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        OpenFileDialog ofdRuta = new OpenFileDialog();
                        ofdRuta.Title = "Seleccione Archivo";
                        //(*.jpg; *.jpeg; *.png)| *.jpg; *.jpeg; *.png; "
                        ofdRuta.Filter = "Files(*.xls; *.xlsx;)|*.xls; *.xlsx;";

                        if (ofdRuta.ShowDialog() == DialogResult.OK)
                        {
                            if (!ofdRuta.FileName.Equals(""))
                            {

                                DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);
                                string ItemsNoSeleccionados = "";
                                foreach (DataRow item in dt.Rows)
                                {
                                    if (!string.IsNullOrEmpty(item[1].ToString().Trim()))
                                    {
                                        var y = item[0].ToString().Trim();
                                        var f = ItemsSAP.Where(m => m.Codigo == y).Select(x => x.Codigo).FirstOrDefault();
                                        if (ItemsSAP.Select(x => x.Codigo).Where(x => x == f).FirstOrDefault() != null)
                                        {
                                            SolicitudCompraDetalle poItem = new SolicitudCompraDetalle();
                                            poItem.ItemSap = item[0].ToString().Trim();
                                            poItem.Cantidad = Int32.Parse(item[1].ToString().Trim());
                                            poItem.Descripcion = ItemsSAP.Where(m => m.Codigo == y).Select(x => x.Descripcion).FirstOrDefault();
                                            poItem.Observacion = item[3].ToString().Trim();
                                            poItem.CodigoEstado = Diccionario.Activo;

                                            poLista.Add(poItem);
                                        }
                                        else
                                        {
                                            ItemsNoSeleccionados = ItemsNoSeleccionados + " " + item[0].ToString().Trim() + "\n";
                                        }
                                        bsDatos.DataSource = poLista.ToList();
                                        gcDatos.DataSource = bsDatos;



                                    }
                                }
                                if (ItemsNoSeleccionados != "")
                                {
                                    XtraMessageBox.Show("Los Codigos: \n" + ItemsNoSeleccionados + " no se han encotrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }


                        }

                        // dgvDatos.BestFitColumns();
                    }
                }
                else
                {
                    XtraMessageBox.Show("Debe seleccionar un Tipo de Item primero!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        #region metodos



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


        private void lColocarbotonAdd(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Agregar";
            colXmlDown.ColumnEdit = rpiBtnAdd;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnAdd.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnAdd.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/open_16x16.png");
            rpiBtnAdd.TextEditStyle = TextEditStyles.HideTextEditor;
           colXmlDown.Width = 50;
        }
        private void lColocarbotonDescargar(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Descargar";
            colXmlDown.ColumnEdit = rpiBtnDownload;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnDownload.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnDownload.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/download_16x16.png");

            rpiBtnDownload.TextEditStyle = TextEditStyles.HideTextEditor;
           // colXmlDown.Width = 50;
        }
        private void lColocarbotonVisualizar(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Visualizar";
            colXmlDown.ColumnEdit = rpiBtnShow;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnShow.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnShow.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/show_16x16.png");
            rpiBtnShow.TextEditStyle = TextEditStyles.HideTextEditor;
          // colXmlDown.Width = 50;
        }
        private void lColocarbotonVisualizarDEtalle(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Ver";
            colXmlDown.ColumnEdit = rpiBtnShowDetalle;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnShowDetalle.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnShowDetalle.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/show_16x16.png");
            rpiBtnShowDetalle.TextEditStyle = TextEditStyles.HideTextEditor;
            // colXmlDown.Width = 50;
        }

        private void cmbTipoItem_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbCargado)
                {
                    var x = loLogicaNegocio.goDiasFechaEntrega(cmbTipoItem.EditValue.ToString());
                    dtpFechaEntrega.EditValue = (DateTime.Now).AddDays(x);
                    if (loLogicaNegocio.gItemSap(cmbTipoItem.EditValue.ToString()))
                    {
                        dgvDatos.Columns["ItemSap"].Visible = true;
                        dgvDatos.Columns["Descripcion"].Visible = false;
                       
                        dgvDatos.Columns["Descripcion"].OptionsColumn.AllowEdit = false;
                        var poLista = (List<SolicitudCompraDetalle>)bsDatos.DataSource;
                       
                        anteriorSap = true;
                        foreach (var item in poLista)
                        {
                            item.ItemSap = "0";
                            if (anteriorSap)
                            {
                                item.Descripcion = "";
                            }
                        }
                    }
                    else
                    {
                        dgvDatos.Columns["ItemSap"].Visible = false;
                        dgvDatos.Columns["Descripcion"].Visible = true;
                        dgvDatos.Columns["Descripcion"].OptionsColumn.AllowEdit = true;
                        dgvDatos.Columns["Descripcion"].VisibleIndex = 1;
                        var poLista = (List<SolicitudCompraDetalle>)bsDatos.DataSource;
                      
                        if (poLista.Count > 0)
                        {
                            foreach (var item in poLista)
                            {
                                item.ItemSap = "0";
                                if (anteriorSap)
                                {
                                    item.Descripcion = "";
                                }

                                anteriorSap = false;
                            }
                        }
                    }
                    gcDatos.RefreshDataSource();
                }


            }
            catch (Exception)
            {

                throw;
            }
        }



        public void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscarSolicitudCompra(Convert.ToInt32(txtNo.Text.Trim()));
                if (poObject != null)
                {
                    txtNo.Text = poObject.IdSolicitudCompra.ToString();
                    lblFecha.Text = poObject.FechaIngreso.ToString("dd/MM/yyyy");
                    txtObservacion.Text = poObject.Observacion;
                    cmbEstado.EditValue = poObject.CodigoEstado;
                    cmbTipoItem.EditValue = poObject.CodigoTipoItem;
                    dtpFechaEntrega.EditValue = poObject.FechaEntrega;
                    txtDescripcion.EditValue = poObject.Descripcion;
                    poObject.SolicitudCompraDetalle = loLogicaNegocio.goBuscarSolicitudCompraDetalles(poObject.IdSolicitudCompra);
                    bsDatos.DataSource = new List<SolicitudCompraDetalle>();
                    bsDatos.DataSource = poObject.SolicitudCompraDetalle;
                    gcDatos.DataSource = bsDatos.DataSource;
                    if (loLogicaNegocio.goBuscarSolicitudCompraArchivoAdjunto(poObject.IdSolicitudCompra)!= null)
                    {
                        poObject.ArchivoAdjunto = loLogicaNegocio.goBuscarSolicitudCompraArchivoAdjunto(poObject.IdSolicitudCompra);
                        bsArchivoAdjunto.DataSource = new List<SolicitudCompraArchivoAdjunto>();
                        bsArchivoAdjunto.DataSource = poObject.ArchivoAdjunto;
                        gcArchivoAdjunto.DataSource = bsArchivoAdjunto.DataSource;
                    }
                    
                }

            }

        }

        private void lLimpiar()
        {
            lblFecha.Text = "";
            txtNo.Text = string.Empty;
            dtpFechaEntrega.Text = string.Empty;
            cmbTipoItem.ItemIndex = 0;
            cmbEstado.ItemIndex = 0;
            txtObservacion.Text = String.Empty;
            txtDescripcion.Text = String.Empty;
            bsDatos.DataSource = new List<SolicitudCompraDetalle>();
            gcDatos.DataSource = null;
            gcDatos.DataSource = bsDatos;
            bsArchivoAdjunto.DataSource = new List<SolicitudCompraArchivoAdjunto>();
            gcArchivoAdjunto.DataSource = null;
            gcArchivoAdjunto.DataSource = bsArchivoAdjunto;
            cmbEstado.EditValue = Diccionario.Pendiente;
            dtpFechaEntrega.EditValue = DateTime.Now;
            dgvDatos.Columns["ItemSap"].Visible = false;
            dgvDatos.Columns["Descripcion"].OptionsColumn.AllowEdit = true;
            lValidarBotones();
        }

        
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Click += btnAprobar_Click;
            if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Click += btnCorregir_Click;
            if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Click += btnRechazar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Click += btnImportar_Click;
        }

        private void lColumnas()
        {
            try
            {   //datagrid view 1  Detalles
                for (int i = 0; i < dgvDatos.Columns.Count; i++)
                {
                    dgvDatos.Columns[i].Visible = false;
                }

                dgvDatos.Columns["Del"].Visible = true;
                dgvDatos.Columns["VerArchivo"].Visible = true;
                dgvDatos.Columns["AgregarArchivo"].Visible = true;
                dgvDatos.Columns["NombreOriginal"].Visible = true;
                dgvDatos.Columns["Observacion"].Visible = true;
                dgvDatos.Columns["Descripcion"].Visible = true;
                dgvDatos.Columns["Cantidad"].Visible = true;


                dgvDatos.Columns["NombreOriginal"].Caption = "Archivo";
                dgvDatos.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
                dgvDatos.Columns["Descripcion"].Width = 150;
                dgvDatos.Columns["ItemSap"].Width = 150;
                dgvDatos.Columns["Cantidad"].Width = 50;
                dgvDatos.Columns["Del"].Width = 60;
                
                
                dgvDatos.OptionsView.RowAutoHeight = true;



                lColocarbotonVisualizarDEtalle(dgvDatos.Columns["VerArchivo"]);
                lColocarbotonAddDetalleFoto(dgvDatos.Columns["AgregarArchivo"]);
                dgvDatos.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
                dgvDatos.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
                //datagrid view 2  Archivo Adjunto
                for (int i = 0; i < dgvArchivoAdjunto.Columns.Count; i++)
                {
                    dgvArchivoAdjunto.Columns[i].Visible = false;
                }
                dgvArchivoAdjunto.Columns["Del"].Visible = true;
                dgvArchivoAdjunto.Columns["Visualizar"].Visible = true;
                dgvArchivoAdjunto.Columns["Descargar"].Visible = true;
                dgvArchivoAdjunto.Columns["ArchivoAdjunto"].Visible = true;
                dgvArchivoAdjunto.Columns["Descripcion"].Visible = true;
                dgvArchivoAdjunto.Columns["Add"].Visible = true;
                dgvArchivoAdjunto.Columns["ArchivoAdjunto"].OptionsColumn.AllowEdit = false;
                dgvArchivoAdjunto.Columns["Descripcion"].Width = 170;
                dgvArchivoAdjunto.Columns["ArchivoAdjunto"].Width = 120;
                dgvArchivoAdjunto.OptionsView.RowAutoHeight = true;
                dgvArchivoAdjunto.Columns["ArchivoAdjunto"].Caption = "Archivo adjunto";
                dgvArchivoAdjunto.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
                //Colocar boton Delete 
                clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);
                clsComun.gDibujarBotonGrid(rpiBtnDel, dgvArchivoAdjunto.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);
                clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvArchivoAdjunto.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16);
                clsComun.gDibujarBotonGrid(rpiBtnDownload, dgvArchivoAdjunto.Columns["Descargar"], "Descargar", Diccionario.ButtonGridImage.download_16x16);
                clsComun.gDibujarBotonGrid(rpiBtnShow, dgvArchivoAdjunto.Columns["Visualizar"], "Visualizar", Diccionario.ButtonGridImage.show_16x16);

                //lColocarbotonDelete(dgvDatos.Columns["Del"]);
                //lColocarbotonDelete(dgvArchivoAdjunto.Columns["Del"]);
                //lColocarbotonAdd(dgvArchivoAdjunto.Columns["Add"]);
                //lColocarbotonDescargar(dgvArchivoAdjunto.Columns["Descargar"]);
                //lColocarbotonVisualizar(dgvArchivoAdjunto.Columns["Visualizar"]);

                dgvDatos.Columns["Del"].Width = 50;
                dgvDatos.Columns["AgregarArchivo"].Width = 50;
                dgvDatos.Columns["VerArchivo"].Width = 50;
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void lValidarBotones()
        {
            if ( cmbEstado.EditValue.ToString() == Diccionario.Negado || cmbEstado.EditValue.ToString() == Diccionario.Cotizado || cmbEstado.EditValue.ToString() == Diccionario.Cerrado || cmbEstado.EditValue.ToString() == Diccionario.Aprobado)
            {
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Enabled = false;
                if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = false;
            }
            else
            {
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
                if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Enabled = true;
                if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = true;
            }

            if (txtNo.Text != "")
            {
                if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = true;
                if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Enabled = true;

            }
            else
            {
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = false;
                
            }

            if (txtNo.Text != "")
            {
                var poObject = loLogicaNegocio.goBuscarSolicitudCompra(Convert.ToInt32(txtNo.Text.Trim()));
                if (poObject != null)
                {
                    if (cmbEstado.EditValue.ToString() == Diccionario.Aprobado || cmbEstado.EditValue.ToString() == Diccionario.Negado || 
                        cmbEstado.EditValue.ToString() == Diccionario.Cotizado || cmbEstado.EditValue.ToString() == Diccionario.Corregir ||
                        cmbEstado.EditValue.ToString() == Diccionario.Cerrado)
                    {
                        if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Enabled = false;
                        if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Enabled = false;
                        if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Enabled = false;
                        if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = false;

                    }
                    else
                    {
                        if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Enabled = true;
                        if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Enabled = true;
                        if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Enabled = true;
                        if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = true;
                    }

                    if (poObject.UsuarioAprobacion == clsPrincipal.gsUsuario && cmbEstado.EditValue.ToString() == Diccionario.Aprobado)
                    {
                        if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
                    }

                    

                }
            }
            else
            {
                if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Enabled = false;
                if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Enabled = true;
                if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Enabled = false;
                if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = false;
            }
        }

        private void lImprimir()
        {
            if (txtNo.EditValue.ToString() != "")
            {
                clsComun.gImprimirSolicitudCompra(int.Parse(txtNo.EditValue.ToString()));
             }
            else
            {
                XtraMessageBox.Show("No existe detalles guardados para imprimir.", Diccionario.MsgRegistroNoGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmTrSolicitudCompras_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

        private void lColocarbotonAddDetalleFoto(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Agregar";
            colXmlDown.ColumnEdit = rpiBtnAddDetalle;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnAddDetalle.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnAddDetalle.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/open_16x16.png");
            rpiBtnAddDetalle.TextEditStyle = TextEditStyles.HideTextEditor;
            colXmlDown.Width = 48;
        }


        #endregion

        private void dgvDatos_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            
        }

        private void dgvDatos_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "ItemSap")
            {
                int piIndex = dgvDatos.FocusedRowHandle;
                var poLista = (List<SolicitudCompraDetalle>)bsDatos.DataSource;
                foreach (var item in ItemsSAP)
                {
                    if (item.Codigo== dgvDatos.GetRowCellValue(piIndex, "ItemSap").ToString())
                    {
                        poLista[piIndex].Descripcion = item.Descripcion;
                    }
                }
                gcDatos.RefreshDataSource();
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



    }
}
