using COM_Negocio;
using DevExpress.Data;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using REH_Presentacion.Compras.Reportes;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace REH_Presentacion.Compras.Transaccion
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmTrCotizacion : frmBaseTrxDev
    {

        #region Variables
        BindingSource bsDatos;
        BindingSource bsProveedor;
        BindingSource bsAdjunto;
        clsNCotizacion loLogicaNegocio = new clsNCotizacion();
        BindingSource bsCotizacionDetalle;
        RepositoryItemButtonEdit rpiBtnDel;
        RepositoryItemMemoEdit rpiMedDescripcion;
        RepositoryItemMemoEdit rpiMedProveedor;
        RepositoryItemButtonEdit rpiBtnDelProveedor;
        RepositoryItemButtonEdit rpiBtnDelAdjunto;
        RepositoryItemButtonEdit rpiBtnShowDetalle;
        RepositoryItemButtonEdit rpiBtnAdd;
        RepositoryItemButtonEdit rpiBtnDownload;
        RepositoryItemButtonEdit rpiBtnShow;
        public List <int> lIdSolicitudCompra;
        public int lIdCotizacion;
        int saberId = 0;
        
        List<CotizacionDetalle> plDetalleGlobal = new List<CotizacionDetalle>();
        List<CotizacionAdjunto> plAdjtuntoGlobal = new List<CotizacionAdjunto>();
        #endregion


        #region Eventos 

        public frmTrCotizacion()
        {

            InitializeComponent();
            bsDatos = new BindingSource();
            bsProveedor = new BindingSource();
            bsAdjunto = new BindingSource();
            bsCotizacionDetalle = new BindingSource();


            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDelDetalle_ButtonClick;
            rpiBtnDelProveedor = new RepositoryItemButtonEdit();
            rpiBtnDelProveedor.ButtonClick += rpiBtnDelProveedor_ButtonClick;
            rpiBtnDelAdjunto = new RepositoryItemButtonEdit();
            rpiBtnDelAdjunto.ButtonClick += rpiBtnDelAdjunto_ButtonClick;
            rpiBtnAdd = new RepositoryItemButtonEdit();
            rpiBtnShowDetalle = new RepositoryItemButtonEdit();
            rpiBtnShowDetalle.ButtonClick += rpiBtnShowDetalle_ButtonClick;


            rpiBtnDownload = new RepositoryItemButtonEdit();
            rpiBtnShow = new RepositoryItemButtonEdit();
            rpiBtnAdd.ButtonClick += rpiBtnAdd_ButtonClick;
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiBtnDownload.ButtonClick += rpiBtnDownload_ButtonClick;


            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.MaxLength = 120;
            rpiMedProveedor = new RepositoryItemMemoEdit();
            rpiMedProveedor.MaxLength = 100;
            rpiMedProveedor.WordWrap = true;
        }

        private void frmTrCotizacion_Load(object sender, EventArgs e)
        {
                try
            {
                clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstadoCotizacion(), true);
                bsDatos.DataSource = new List<CotizacionSolicitudCompra>();
                gcBandejaSolicitud.DataSource = bsDatos;
                bsProveedor.DataSource = new List<CotizacionProveedor>();
                gcProveedor.DataSource = bsProveedor;
                bsCotizacionDetalle.DataSource = new List<CotizacionDetalle>();
                gcCotizacionDetalle.DataSource = bsCotizacionDetalle;


                bsAdjunto.DataSource = new List<CotizacionAdjunto>();
                gcAdjunto.DataSource = bsAdjunto;

                //dgvAdjunto.Columns["Agregar"].Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/open_16x16.png");
                //dgvAdjunto.Columns["Agregar"].Caption = "";

                if (lIdSolicitudCompra != null)
                {
                    var Descripcion = loLogicaNegocio.BuscarDescripcionSolicitud(lIdSolicitudCompra);
                    string msgDescripcion = "Cotización de las Sol. Compra. No.";
                    var polista = new List<CotizacionSolicitudCompra>();
                    foreach (var x in lIdSolicitudCompra)
                    {
                        msgDescripcion = msgDescripcion + ", " + x;

                        var dt = loLogicaNegocio.listarSolicitudCompra(x);
                        polista.Add(dt);
                    }
                    txtDescripcion.EditValue = Descripcion;
                    bsDatos.DataSource = polista;
                    gcBandejaSolicitud.DataSource = bsDatos;
                    dgvBandejaSolicitud.RefreshData();

                }
                //if (lIdCotizacion!=0 && lIdSolicitudCompra.Count!=0)
                //{
                //    lIdCotizacion = loLogicaNegocio.goBuscarCotizacionPorSolicitudCompra(lIdSolicitudCompra).IdCotizacion;
                //}


                cmbEstado.EditValue = Diccionario.Pendiente;
                lCargarEventosBotones();
                if (lIdCotizacion != 0)
                {
                    txtNo.EditValue = lIdCotizacion;

                    lConsultar();

                }

                lColumnas();
                lValidarBotones();

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
                piIndex = dgvAdjunto.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<CotizacionAdjunto>)bsAdjunto.DataSource;

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
                                bsAdjunto.DataSource = poLista;
                                dgvAdjunto.RefreshData();
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


        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvAdjunto.FocusedRowHandle;
                var poLista = (List<CotizacionAdjunto>)bsAdjunto.DataSource;

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
                        psRuta = ConfigurationManager.AppSettings["CarpetaSolCompras"].ToString() + poLista[piIndex].ArchivoAdjunto;
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
        /// Evento del boton de Descargar en el Grid, descarga el archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDownload_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {

                int piIndex = dgvAdjunto.FocusedRowHandle;
                var poLista = (List<CotizacionAdjunto>)bsAdjunto.DataSource;

                string psRuta = ConfigurationManager.AppSettings["CarpetaSolCompras"].ToString() + poLista[piIndex].ArchivoAdjunto;
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

        #region Eliminar filas en Grid

        private void rpiBtnDelProveedor_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                //Eliminar Proveedor
                // Tomamos la fila seleccionada
                int piIndex = dgvProveedor.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<CotizacionProveedor>)bsProveedor.DataSource;


                if (piIndex >= 0)
                {
                    var borrar = poLista[piIndex].Id;
                    if (poLista.Count > 0 && piIndex >= 0)
                    {
                        //ELiminar globales
                        for (int i = 0; i < plDetalleGlobal.Count; i++)
                        {
                            if (borrar == plDetalleGlobal[i].IdCotizacionProveedor)
                            {
                                plDetalleGlobal.RemoveAt(i);
                                i = i - 1;
                            }
                        }
                        for (int i = 0; i < plAdjtuntoGlobal.Count; i++)
                        {
                            if (borrar == plAdjtuntoGlobal[i].IdCotizacionProveedor)
                            {
                                plAdjtuntoGlobal.RemoveAt(i);
                                i = i - 1;
                            }
                        }

                        // Eliminar Fila seleccionada de mi lista
                        poLista.RemoveAt(piIndex);

                        // Asigno mi nueva lista al Binding Source

                        //  MostrarDetalle(piIndex);
                        //  bsProveedor.DataSource = poLista;
                        dgvProveedor.RefreshData();

                    }


                    if (poLista.Count == 0)
                    {
                        plDetalleGlobal = new List<CotizacionDetalle>();
                        bsCotizacionDetalle.DataSource = new List<CotizacionDetalle>();
                        dgvCotizacionDetalle.RefreshData();
                    }
                }
                piIndex = dgvProveedor.GetFocusedDataSourceRowIndex();
                MostrarDetalle(piIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Evento del boton de eliminar en el Grid, elimina la fila seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDelDetalle_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {

                //Eliminar Detalle

                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvCotizacionDetalle.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<CotizacionDetalle>)bsCotizacionDetalle.DataSource;

                var borrar = poLista[piIndex].IdCotizacionProveedor;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    for (int i = 0; i < plDetalleGlobal.Count; i++)
                    {
                        if (borrar == plDetalleGlobal[i].IdCotizacionProveedor)
                        {
                            plDetalleGlobal.RemoveAt(i);
                            i = i - 1;
                        }

                    }
                    // Eliminar Fila seleccionada de mi lista

                    poLista.RemoveAt(piIndex);
                    foreach (var x in poLista)
                    {
                        plDetalleGlobal.Add(x);
                    }
                    // Asigno mi nueva lista al Binding Source
                    bsCotizacionDetalle.DataSource = poLista;
                    dgvCotizacionDetalle.RefreshData();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rpiBtnDelAdjunto_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                //Eliminar Adjunto
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvAdjunto.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid

                var poLista = (List<CotizacionAdjunto>)bsAdjunto.DataSource;
                var borrar = poLista[piIndex].IdCotizacionProveedor;
                if (poLista.Count > 0 && piIndex >= 0)
                {
                    for (int i = 0; i < plAdjtuntoGlobal.Count; i++)
                    {
                        if (borrar == plAdjtuntoGlobal[i].IdCotizacionProveedor)
                        {
                            plAdjtuntoGlobal.RemoveAt(i);
                            i = i - 1;
                        }
                    }
                    // Eliminar Fila seleccionada de mi lista
                    poLista.RemoveAt(piIndex);
                    foreach (var x in poLista)
                    {
                        plAdjtuntoGlobal.Add(x);
                    }
                    // Asigno mi nueva lista al Binding Source
                    bsAdjunto.DataSource = poLista;
                    dgvAdjunto.RefreshData();
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
        private void rpiBtnShowDetalle_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvCotizacionDetalle.FocusedRowHandle;
                var poLista = (List<CotizacionDetalle>)bsCotizacionDetalle.DataSource;

                if (!string.IsNullOrEmpty(poLista[piIndex].ArchivoAdjunto))
                {
                    string extension = System.IO.Path.GetExtension(poLista[piIndex].ArchivoAdjunto);

                    //Archivo PDF
                    if (extension.Equals(".pdf"))
                    {
                        frmVerPdf pofrmVerPdf = new frmVerPdf();
                      
                        //Muestra archivo ya subido
                      
                            pofrmVerPdf.lsRuta = ConfigurationManager.AppSettings["CarpetaSolCompras"].ToString() + poLista[piIndex].ArchivoAdjunto;
                            pofrmVerPdf.Show();
                            pofrmVerPdf.SetDesktopLocation(0, 0);
                      
                    }

                    //Archivo JPG
                    if (extension.Equals(".jpg"))
                    {
                        
                        //Muestra archivo ya subido
                       
                            string psRuta = ConfigurationManager.AppSettings["CarpetaSolCompras"].ToString() + poLista[piIndex].ArchivoAdjunto;
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

        #endregion

        private void chbCompletado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbCompletado.Checked)
                {
                    chbCompletado.Properties.Appearance.BackColor = System.Drawing.Color.PaleGreen;
                }
                else
                {
                    chbCompletado.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
                }
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
                var poListaSolicitudes = (List<CotizacionSolicitudCompra>)bsDatos.DataSource;
                if (poListaSolicitudes.Count > 0)
                {
                    bsProveedor.AddNew();
                    dgvProveedor.Focus();
                    dgvProveedor.ShowEditor();
                    var poLista = (List<CotizacionProveedor>)bsProveedor.DataSource;
                    poLista.LastOrDefault().Id = saberId;
                    poLista.LastOrDefault().FechaCotizacion = DateTime.Now;
                    poLista.LastOrDefault().DiasPago = 0;
                    poLista.LastOrDefault().DiasEntrega = 1;
                    saberId = saberId + 1;
                    dgvProveedor.Focus();
                    dgvProveedor.ShowEditor();
                    var piIndex = dgvProveedor.GetFocusedDataSourceRowIndex();

                    if (lIdSolicitudCompra.Count != 0)
                    {
                        List<CotizacionDetalle> poListaDetalleSolCompra = new List<CotizacionDetalle>();
                        //Ingreso detalle de SC al proveedor
                        foreach (var x in lIdSolicitudCompra)
                        {
                            poListaDetalleSolCompra.AddRange(loLogicaNegocio.goBuscarListarSolicitudCompraDetalle(x));
                        }

                        //Agrupar detalle por descripcion por si hay algun item repetido
                        poListaDetalleSolCompra = (
                        from p in poListaDetalleSolCompra
                        group p by p.Descripcion into g
                        select new CotizacionDetalle()
                        {
                            Descripcion = g.Key,
                            Cantidad = g.Sum(t => t.Cantidad),
                            ItemCode = g.Select(p => p.ItemCode).FirstOrDefault(),
                            ArchivoAdjunto = g.Select(p => p.ArchivoAdjunto).FirstOrDefault(),
                        }
                        ).ToList();

                        if (poLista.Count != 0)
                        {
                            if (piIndex >= 0)
                            {
                                //agregar los Items a la variable global
                                foreach (var y in poListaDetalleSolCompra)
                                {
                                    y.IdCotizacionProveedor = poLista[piIndex].Id;
                                    plDetalleGlobal.Add(y);
                                }
                                //dgvCotizacionDetalle.Focus();
                                //dgvCotizacionDetalle.ShowEditor();
                                //dgvCotizacionDetalle.UpdateCurrentRow();
                                //dgvCotizacionDetalle.RefreshData();
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("Debe agregar un proveedor primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        MostrarDetalle(piIndex);
                        dgvProveedor.UpdateCurrentRow();
                        dgvProveedor.RefreshData();


                    }
                    else
                    {
                        XtraMessageBox.Show("No hay solicitudes de compras", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    MostrarDetalle(piIndex);
                    dgvProveedor.UpdateCurrentRow();
                    dgvProveedor.RefreshData();
                }
                else
                {
                    XtraMessageBox.Show("No hay solicitudes de compras", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void AddFilaDetalle_Click(object sender, EventArgs e)
        {

            try
            {
                var poLista = (List<CotizacionProveedor>)bsProveedor.DataSource;

                if (poLista.Count != 0)
                {
                    var poListaDetalle = new List<CotizacionDetalle>();
                    var poListaAdjunto = new List<CotizacionAdjunto>();
                    var piIndex = dgvProveedor.GetFocusedDataSourceRowIndex();
                    if (xtraTabControl1.SelectedTabPageIndex == 0)
                    {

                        bsCotizacionDetalle.AddNew();
                        poListaDetalle = (List<CotizacionDetalle>)bsCotizacionDetalle.DataSource;
                        poListaDetalle.LastOrDefault().IdCotizacionProveedor = poLista[piIndex].Id;
                        plDetalleGlobal.Add(poListaDetalle.LastOrDefault());
                        bsCotizacionDetalle.DataSource = poListaDetalle;
                        dgvCotizacionDetalle.Focus();
                        dgvCotizacionDetalle.ShowEditor();
                        dgvCotizacionDetalle.UpdateCurrentRow();
                        dgvCotizacionDetalle.RefreshData();
                    }
                    if (xtraTabControl1.SelectedTabPageIndex == 1)
                    {
                        bsAdjunto.AddNew();
                        poListaAdjunto = (List<CotizacionAdjunto>)bsAdjunto.DataSource;
                        var poRegistroCoti = poListaAdjunto.LastOrDefault();
                        poRegistroCoti.IdCotizacionProveedor = poLista[piIndex].Id;
                        plAdjtuntoGlobal.Add(poRegistroCoti);
                        //var poListaTmpAdjunto = new List<CotizacionAdjunto>();
                        //foreach (var x in poListaAdjunto)
                        //{
                        //    poListaTmpAdjunto.Add(new CotizacionAdjunto()
                        //    {
                        //        Descripcion = x.Descripcion,
                        //        NombreOriginal = x.NombreOriginal,
                        //        RutaOrigen = x.RutaOrigen,
                        //        RutaDestino = x.RutaDestino,
                        //        IdCotizacionProveedor = x.IdCotizacionProveedor,
                        //        IdCotizacionAdjunto = x.IdCotizacionAdjunto,
                        //    });
                        //}

                        bsAdjunto.DataSource = poListaAdjunto;
                        //foreach (var item in poListaTmpAdjunto)
                        //{
                        //    plAdjtuntoGlobal.Add(item);
                        //}
                        dgvAdjunto.Focus();
                        dgvAdjunto.ShowEditor();
                        dgvAdjunto.UpdateCurrentRow();
                        dgvAdjunto.RefreshData();
                    }
                }

                else
                {
                    XtraMessageBox.Show("Debe agregar un proveedor primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //private void btnDetalle_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var solicitudCompra = (CotizacionSolicitudCompra)bsDatos.DataSource;
        //        var dt = loLogicaNegocio.goBuscarListarSolicitudCompraDetalle(solicitudCompra.No);
        //        var poProveedor = (List<CotizacionProveedor>)bsProveedor.DataSource;
        //        if (poProveedor.Count != 0)
        //        {
        //            var piIndex = dgvProveedor.GetFocusedDataSourceRowIndex();
        //            if (piIndex>=0)
        //            {
        //                foreach (var x in dt)
        //                {
        //                    x.IdCotizacionProveedor = poProveedor[piIndex].Id;
        //                    plDetalleGlobal.Add(x);
        //                }
        //                MostrarDetalle(piIndex);
        //                dgvCotizacionDetalle.Focus();
        //                dgvCotizacionDetalle.ShowEditor();
        //                dgvCotizacionDetalle.UpdateCurrentRow();
        //                dgvCotizacionDetalle.RefreshData();
        //            }    
        //        }
        //        else
        //        {
        //            XtraMessageBox.Show("Debe agregar un proveedor primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void dgvCotizacionDetalle_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {

                if (e.Column.Name == "colGrabaIva")
                {
                    dgvCotizacionDetalle.PostEditor();
                    //dgvCotizacionDetalle.RefreshData();
                    int piIndex = dgvCotizacionDetalle.FocusedRowHandle;
                    if (piIndex >= 0)
                    {


                        //clsPrincipal.gdcIva = 0.12;
                        decimal tiIva = Convert.ToDecimal(clsPrincipal.gdcIva);
                        var poLista = (List<CotizacionDetalle>)bsCotizacionDetalle.DataSource;


                        if (poLista[piIndex].GrabaIva == false)
                        {
                            poLista[piIndex].GrabaIva = true;
                            poLista[piIndex].ValorIva = decimal.Round(poLista[piIndex].Cantidad * poLista[piIndex].ValorUnitario * tiIva, 2);
                            poLista[piIndex].Total = decimal.Round(poLista[piIndex].Cantidad * poLista[piIndex].ValorUnitario + poLista[piIndex].ValorIva, 2);


                        }
                        else
                        {
                            poLista[piIndex].GrabaIva = false;
                            poLista[piIndex].ValorIva = 0;
                            poLista[piIndex].Total = decimal.Round(poLista[piIndex].Cantidad * poLista[piIndex].ValorUnitario, 2);
                        }





                        bsCotizacionDetalle.DataSource = poLista;
                        dgvCotizacionDetalle.RefreshData();
                        Cargartotal();
                    }
                }
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }






        }

        private void dgvCotizacionDetalle_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                dgvCotizacionDetalle.PostEditor();
                //   dgvCotizacionDetalle.RefreshData();
                int piIndex = dgvCotizacionDetalle.FocusedRowHandle;
                if (piIndex >= 0)
                {
                    var poLista = (List<CotizacionDetalle>)bsCotizacionDetalle.DataSource;
                    decimal tiIva = Convert.ToDecimal(clsPrincipal.gdcIva);
                    if (poLista[piIndex].ValorUnitario >= 0)
                    {
                        if (poLista[piIndex].GrabaIva == true)
                        {
                            poLista[piIndex].ValorIva = decimal.Round(poLista[piIndex].Cantidad * poLista[piIndex].ValorUnitario * tiIva, 2);
                        }
                        poLista[piIndex].SubTotal = decimal.Round(poLista[piIndex].Cantidad * poLista[piIndex].ValorUnitario,2);
                        poLista[piIndex].Total = decimal.Round(poLista[piIndex].Cantidad * poLista[piIndex].ValorUnitario + poLista[piIndex].ValorIva, 2);

                    }

                    bsCotizacionDetalle.DataSource = poLista;
                    dgvCotizacionDetalle.RefreshData();
                    Cargartotal();
                }
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void dgvProveedor_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                var poProveedor = (List<CotizacionProveedor>)bsProveedor.DataSource;
                var piIndex = dgvProveedor.GetFocusedDataSourceRowIndex();
                if (piIndex >= 0)
                {
                    for (int i = 0; i < poProveedor.Count; i++)
                    {
                        poProveedor[i].No = false;
                    }
                    poProveedor[piIndex].No = true;
                    MostrarDetalle(piIndex);
                }
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

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                AddFilaDetalle.Visible = true;
            }
            else
            {
                AddFilaDetalle.Visible = false;
            }
        }
        #endregion



        /// <summary>
        /// Evento del boton Grabar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvCotizacionDetalle.PostEditor();
                dgvProveedor.PostEditor();
                dgvAdjunto.PostEditor();
                Cotizacion poObject = new Cotizacion();
                poObject.Proveedores= (List<CotizacionProveedor>)bsProveedor.DataSource;

                foreach (var poObjectProveedor in poObject.Proveedores)
                {
                    poObjectProveedor.Detalles= plDetalleGlobal.Where(i => i.IdCotizacionProveedor == poObjectProveedor.Id).ToList();
                    poObjectProveedor.ArchivoAdjunto = plAdjtuntoGlobal.Where(i => i.IdCotizacionProveedor == poObjectProveedor.Id).ToList();
                }
               // poObject.IdSolicitud = lIdSolicitudCompra;
                if (!string.IsNullOrEmpty(txtNo.Text))
                {
                    poObject.IdCotizacion = Int32.Parse(txtNo.Text);
                }
                if (!string.IsNullOrEmpty(txtDescripcion.Text))
                {
                    poObject.Descripcion = txtDescripcion.EditValue.ToString();

                }
                poObject.Observacion = txtObservacion.Text;
                poObject.IdSolicitud = lIdSolicitudCompra;
                poObject.CodigoEstado = Diccionario.Pendiente;
                poObject.Usuario = clsPrincipal.gsUsuario;
                poObject.Terminal = clsPrincipal.gsTerminal;
                poObject.Completado = chbCompletado.Checked;
           
                int pId = 0;

                string psMsg = loLogicaNegocio.gsGuardar(poObject, out pId);
                if (string.IsNullOrEmpty(psMsg))
                {
                    
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);


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
                var poListaObject = loLogicaNegocio.goListarMaestro(clsPrincipal.gsUsuario, Int32.Parse(menu[0]));
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Usuario"),
                                    new DataColumn("Fecha Cotización", typeof(DateTime)),
                                    new DataColumn("Descripción"),
                                    new DataColumn("Estado"),
                                    new DataColumn("Comentario Aprobador")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdCotizacion;
                    row["Usuario"] = a.DesUsuario;
                    row["Fecha Cotización"] = a.FechaIngreso;
                    row["Descripción"] = a.Descripcion;
                    row["Estado"] = a.DesEstado;
                    row["Comentario Aprobador"] = a.ComentarioAprobador;

                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    lLimpiar();
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar();
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
                if (txtNo.EditValue != null)
                {

                    xrptComparativoCotizacion xrpt = new xrptComparativoCotizacion();
                    xrpt.lIdCotizacion = int.Parse(txtNo.EditValue.ToString());
                        
                    using (ReportPrintTool printTool = new ReportPrintTool(xrpt))
                    {
                        printTool.ShowRibbonPreviewDialog();
                    }
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

        private void dgvProveedor_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //   dgvCotizacionDetalle.RefreshData();
                int piIndex = dgvProveedor.FocusedRowHandle;
                if (piIndex >= 0 && e.Column.FieldName == "Identificacion")
                {
                    var poLista = (List<CotizacionProveedor>)bsProveedor.DataSource;
                    var proveedor = loLogicaNegocio.goBuscarProveedor(poLista[piIndex].Identificacion);
                    if (proveedor.Count > 0)
                    {
                        if (proveedor.Count > 1)
                        {
                            DataTable dt = new DataTable();

                            dt.Columns.AddRange(new DataColumn[] { new DataColumn("Id"), new DataColumn("Nombre"), new DataColumn("Correo")});

                            proveedor.ForEach(a =>
                            {
                                DataRow row = dt.NewRow();
                                row["Id"] = a.IdProveedor;
                                row["Nombre"] = a.Nombre;
                                row["Correo"] = a.Correo;
                                dt.Rows.Add(row);
                            });

                            frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Lista de proveedores con la misma identificación" };
                            if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                            {
                                var poRegistro = proveedor.Where(x => x.IdProveedor == int.Parse(pofrmBuscar.lsCodigoSeleccionado)).FirstOrDefault();

                                poLista[piIndex].IdProveedor = poRegistro.IdProveedor;
                                poLista[piIndex].Nombre = poRegistro.Nombre;
                                poLista[piIndex].Correo = poRegistro.Correo;
                            }
                            else
                            {
                                poLista[piIndex].IdProveedor = proveedor.FirstOrDefault().IdProveedor;
                                poLista[piIndex].Nombre = proveedor.FirstOrDefault().Nombre;
                                poLista[piIndex].Correo = proveedor.FirstOrDefault().Correo;
                            }
                        }
                        else
                        {
                            poLista[piIndex].IdProveedor = proveedor.FirstOrDefault().IdProveedor;
                            poLista[piIndex].Nombre = proveedor.FirstOrDefault().Nombre;
                            poLista[piIndex].Correo = proveedor.FirstOrDefault().Correo;
                        }
                    }
                    else
                    {
                        poLista[piIndex].IdProveedor = 0;
                        poLista[piIndex].Nombre = "";
                        poLista[piIndex].Correo = "";
                    }
                    
                    bsProveedor.DataSource = poLista;
                    dgvProveedor.RefreshData();

                }
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        #region Metodos
      


        #region DGV
        private void lColumnas()
        {
            //DgvSolicitudCompra;
            for (int i = 0; i < dgvBandejaSolicitud.Columns.Count; i++)
            {
                dgvBandejaSolicitud.Columns[i].OptionsColumn.AllowEdit = false;
            }

            dgvBandejaSolicitud.Columns["No"].Width = 25;
            dgvBandejaSolicitud.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaSolicitud.Columns["Descripcion"].Width = 125;
            dgvBandejaSolicitud.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaSolicitud.Columns["Observacion"].Width = 125;
            dgvBandejaSolicitud.Columns["Usuario"].ColumnEdit = rpiMedDescripcion;
            dgvBandejaSolicitud.Columns["Usuario"].Width = 125;

            //DgvProveedor
            for (int i = 0; i < dgvProveedor.Columns.Count; i++)
            {
                dgvProveedor.Columns[i].Visible = false;
            }

            MostrarColumnasProveedores();

            dgvProveedor.Columns["Nombre"].ColumnEdit = rpiMedDescripcion;
            dgvProveedor.Columns["Correo"].ColumnEdit = rpiMedDescripcion;
            //dgvProveedor.Columns["FormaPago"].ColumnEdit = rpiMedProveedor;
            (dgvProveedor.Columns["DiasEntrega"].RealColumnEdit as RepositoryItemTextEdit).MaxLength = 3;
            dgvProveedor.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True; 
            (dgvProveedor.Columns["Identificacion"].RealColumnEdit as RepositoryItemTextEdit).MaxLength = 15;
           
            

            dgvProveedor.Columns["Iva"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvProveedor.Columns["Iva"].DisplayFormat.FormatString = "{0:c2}";
            dgvProveedor.Columns["SubTotal"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvProveedor.Columns["SubTotal"].DisplayFormat.FormatString = "{0:c2}";
            dgvProveedor.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvProveedor.Columns["Total"].DisplayFormat.FormatString = "{0:c2}";

            dgvProveedor.Columns["ValorAnticipo"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvProveedor.Columns["ValorAnticipo"].DisplayFormat.FormatString = "{0:c2}";

            lColocarbotonDeleteProveedor(dgvProveedor.Columns["Delete"]);

            dgvProveedor.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            dgvProveedor.BestFitColumns();
            ColumnaProveedorWidth();

            //DgvDetalle
            for (int i = 0; i < dgvCotizacionDetalle.Columns.Count; i++)
            {
                dgvCotizacionDetalle.Columns[i].Visible = false;
            }
            MostrarColumnasDetalle();
            

            dgvCotizacionDetalle.Columns["SubTotal"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvCotizacionDetalle.Columns["SubTotal"].DisplayFormat.FormatString = "{0:c2}";
            dgvCotizacionDetalle.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;

            lColocarbotonDelete(dgvCotizacionDetalle.Columns["Delete"]);
            dgvCotizacionDetalle.Columns["ValorUnitario"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvCotizacionDetalle.Columns["ValorUnitario"].DisplayFormat.FormatString = "{0:c2}";
            dgvCotizacionDetalle.Columns["ValorIva"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvCotizacionDetalle.Columns["ValorIva"].DisplayFormat.FormatString = "{0:c2}";
            dgvCotizacionDetalle.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvCotizacionDetalle.Columns["Total"].DisplayFormat.FormatString = "{0:c2}";


            lColocarbotonVisualizarDEtalle(dgvCotizacionDetalle.Columns["VerArchivoAdjunto"]);

            dgvCotizacionDetalle.OptionsView.RowAutoHeight = true;
            dgvProveedor.OptionsView.RowAutoHeight = true;
            dgvProveedor.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvCotizacionDetalle.Columns["Descripcion"].ColumnEdit = rpiMedDescripcion;
            dgvCotizacionDetalle.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvCotizacionDetalle.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
            dgvCotizacionDetalle.BestFitColumns();
            ColumnaDetalleWidth();




            //DGV ADJUNTO
            for (int i = 0; i < dgvAdjunto.Columns.Count; i++)
            {
                dgvAdjunto.Columns[i].Visible = false;
            }
            MostrarColumnasAdjunto();

            lColocarbotonAdd(dgvAdjunto.Columns["Agregar"]);
            lColocarbotonVisualizar(dgvAdjunto.Columns["Visualizar"]);
            lColocarbotonDescargar(dgvAdjunto.Columns["Descargar"]);
            lColocarbotonDeleteAdjunto(dgvAdjunto.Columns["Eliminar"]);

            lColocarTotal("Total", dgvCotizacionDetalle);
            lColocarTotal("ValorIva", dgvCotizacionDetalle);
            lColocarTotal("SubTotal", dgvCotizacionDetalle);

        }

        private void ColumnaDetalleWidth()
        {
            dgvCotizacionDetalle.Columns["Descripcion"].Width = 230;
            dgvCotizacionDetalle.Columns["Observacion"].Width = 230;
            dgvCotizacionDetalle.Columns["Delete"].Width = 150;
            dgvCotizacionDetalle.Columns["VerArchivoAdjunto"].Width = 90;
            dgvCotizacionDetalle.Columns["Cantidad"].Width = 70;
            dgvCotizacionDetalle.Columns["ValorUnitario"].Width = 90;
            dgvCotizacionDetalle.Columns["GrabaIva"].Width = 60;
            dgvCotizacionDetalle.Columns["SubTotal"].Width = 100;
            dgvCotizacionDetalle.Columns["ValorIva"].Width = 100;
            dgvCotizacionDetalle.Columns["Total"].Width = 100;
            dgvCotizacionDetalle.Columns["ValorIva"].Caption = "Iva";
        }
        private void ColumnaProveedorWidth()
        {
            dgvProveedor.Columns["Delete"].Width = 25;
            dgvProveedor.Columns["No"].Width = 35;
            dgvProveedor.Columns["DiasPago"].Width = 65;
            dgvProveedor.Columns["Identificacion"].Width = 100;
            dgvProveedor.Columns["Nombre"].Width = 120;
            dgvProveedor.Columns["Correo"].Width = 120;
            dgvProveedor.Columns["Observacion"].Width = 210;
            dgvProveedor.Columns["DiasEntrega"].Width = 57;
            dgvProveedor.Columns["FechaCotizacion"].Width = 69;
        }
        private void MostrarColumnasProveedores()
        {
            dgvProveedor.Columns["Delete"].Visible = true;
            dgvProveedor.Columns["Total"].Visible = true;
            dgvProveedor.Columns["Iva"].Visible = true;
            dgvProveedor.Columns["SubTotal"].Visible = true;
            dgvProveedor.Columns["FechaCotizacion"].Visible = true;
            dgvProveedor.Columns["DiasEntrega"].Visible = true;
            dgvProveedor.Columns["DiasPago"].Visible = true;
            dgvProveedor.Columns["ValorAnticipo"].Visible = true;
            dgvProveedor.Columns["Observacion"].Visible = true;
            dgvProveedor.Columns["Correo"].Visible = true;
            dgvProveedor.Columns["Nombre"].Visible = true;
            dgvProveedor.Columns["Identificacion"].Visible = true;
            dgvProveedor.Columns["No"].Visible = true;
            dgvProveedor.Columns["No"].OptionsColumn.AllowEdit = false;
            dgvProveedor.Columns["Total"].OptionsColumn.AllowEdit = false;

            dgvProveedor.Columns["SubTotal"].OptionsColumn.AllowEdit = false;
            dgvProveedor.Columns["Iva"].OptionsColumn.AllowEdit = false;

            //dgvProveedor.Columns["RequiereAnticipo"].Visible = true;
            
            
        }
        private void MostrarColumnasDetalle()
        {
            dgvCotizacionDetalle.Columns["Delete"].Visible = true;
            dgvCotizacionDetalle.Columns["VerArchivoAdjunto"].Visible = true;
            dgvCotizacionDetalle.Columns["Total"].Visible = true;
            dgvCotizacionDetalle.Columns["ValorIva"].Visible = true;
            dgvCotizacionDetalle.Columns["SubTotal"].Visible = true;
            dgvCotizacionDetalle.Columns["GrabaIva"].Visible = true;
            dgvCotizacionDetalle.Columns["ValorUnitario"].Visible = true;
            dgvCotizacionDetalle.Columns["Observacion"].Visible = true;
            dgvCotizacionDetalle.Columns["Descripcion"].Visible = true;
            dgvCotizacionDetalle.Columns["Cantidad"].Visible = true;

            dgvCotizacionDetalle.Columns["Descripcion"].OptionsColumn.AllowEdit = false;
            dgvCotizacionDetalle.Columns["Cantidad"].OptionsColumn.AllowEdit = false;
            dgvCotizacionDetalle.Columns["SubTotal"].OptionsColumn.AllowEdit = false;
            dgvCotizacionDetalle.Columns["ValorIva"].OptionsColumn.AllowEdit = false;
            dgvCotizacionDetalle.Columns["Total"].OptionsColumn.AllowEdit = false;
        }
        private void MostrarColumnasAdjunto()
        {
            dgvAdjunto.Columns["Eliminar"].Visible = true;
            dgvAdjunto.Columns["Descargar"].Visible = true;
            dgvAdjunto.Columns["Visualizar"].Visible = true;
            dgvAdjunto.Columns["NombreOriginal"].Visible = true;
            dgvAdjunto.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvAdjunto.Columns["NombreOriginal"].Caption = "Nombre";
            dgvAdjunto.Columns["Descripcion"].Visible = true;
            dgvAdjunto.Columns["Agregar"].Visible = true;
        }


        private void lColocarbotonDeleteProveedor(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Eliminar";
            colXmlDown.ColumnEdit = rpiBtnDelProveedor;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnDelProveedor.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnDelProveedor.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/trash_16x16.png");
            rpiBtnDelProveedor.TextEditStyle = TextEditStyles.HideTextEditor;
            colXmlDown.Width = 40;


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
        private void lColocarbotonDeleteAdjunto(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Eliminar";
            colXmlDown.ColumnEdit = rpiBtnDelAdjunto;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnDelAdjunto.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnDelAdjunto.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/trash_16x16.png");
            rpiBtnDelAdjunto.TextEditStyle = TextEditStyles.HideTextEditor;
            //  colXmlDown.Width = 50;


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
        #endregion

        private void MostrarDetalle(int piIndex)
        {
            var poProveedor = (List<CotizacionProveedor>)bsProveedor.DataSource;
            var poListaDetalle = plDetalleGlobal.Where(i => i.IdCotizacionProveedor == poProveedor[piIndex].Id).ToList();
            var poListaAdjunto = plAdjtuntoGlobal.Where(i => i.IdCotizacionProveedor == poProveedor[piIndex].Id).ToList();
       
            bsCotizacionDetalle.DataSource = poListaDetalle;
            bsAdjunto.DataSource = poListaAdjunto;
            dgvProveedor.RefreshData();
            dgvCotizacionDetalle.RefreshData();
            dgvAdjunto.RefreshData();
        }

        private void lValidarBotones()
        {
            if (cmbEstado.EditValue.ToString() == Diccionario.Pendiente || cmbEstado.EditValue.ToString() == Diccionario.Corregir)
            {
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;
                if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Enabled = true;
                if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = true;
            }
            else
            {
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                if (tstBotones.Items["btnRechazar"] != null) tstBotones.Items["btnRechazar"].Enabled = false;
                if (tstBotones.Items["btnCorregir"] != null) tstBotones.Items["btnCorregir"].Enabled = false;
            }

            if (txtNo.Text != "")
            {
                if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = true;

            }
            else
            {
                if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Enabled = false;
            }

          
        }
        
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

        }
        private void lLimpiar()
        {
            plDetalleGlobal = new List<CotizacionDetalle>();
            plAdjtuntoGlobal = new List<CotizacionAdjunto>();
            bsDatos.DataSource = new List<CotizacionSolicitudCompra>();
            bsProveedor.DataSource = new List<CotizacionProveedor>();
            bsCotizacionDetalle.DataSource = new List<CotizacionDetalle>();
            bsAdjunto.DataSource = new List<CotizacionAdjunto>();
            cmbEstado.EditValue = Diccionario.Pendiente;
            txtDescripcion.Text = string.Empty;
            txtObservacion.Text = string.Empty;
            txtNo.Text = string.Empty;
            saberId = 0;
            lIdSolicitudCompra = null;
            lIdCotizacion = 0;
            lValidarBotones();
            chbCompletado.Checked = false;
            lblFecha.Text = "";
        }

        public void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscarCotizacion(Convert.ToInt32(txtNo.Text.Trim()));
                if (poObject != null)
                {
                    if (poObject.CodigoEstado== Diccionario.Aprobado || poObject.CodigoEstado== Diccionario.PreAprobado)
                    {
                        if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;
                    }
                    txtNo.Text = poObject.IdCotizacion.ToString();
                    
                   
                    lIdSolicitudCompra = loLogicaNegocio.goBuscarIdSolicitudesPorCotizacion(Convert.ToInt32(txtNo.Text.Trim()));
                 // var polista = loLogicaNegocio.goBuscarSolicitudPorCotizacion(Convert.ToInt32(txtNo.Text.Trim()));
                    if (lIdSolicitudCompra != null)
                    {
                        var polista = new List<CotizacionSolicitudCompra>();
                        foreach (var x in lIdSolicitudCompra)
                        {

                            var dt = loLogicaNegocio.listarSolicitudCompra(x);
                            polista.Add(dt);
                            
                        }
                       
                        txtDescripcion.EditValue = poObject.Descripcion;
                        txtObservacion.EditValue = poObject.Observacion;
                        cmbEstado.EditValue = poObject.CodigoEstado;
                        bsDatos.DataSource = polista;
                        gcBandejaSolicitud.DataSource = bsDatos;
                        dgvBandejaSolicitud.RefreshData();
                        chbCompletado.Checked = poObject.Completado;
                    }

                    //if (polista != null)
                    //{
                    //    bsDatos.DataSource = polista;
                    //    gcBandejaSolicitud.DataSource = bsDatos;
                    //    dgvBandejaSolicitud.RefreshData();

                    //}

                    lblFecha.Text = poObject.FechaIngreso.ToString("dd/MM/yyyy");
                    txtDescripcion.Text = poObject.Descripcion.ToString();

                    poObject.Proveedores = loLogicaNegocio.goBuscarCotizacionProveedores(poObject.IdCotizacion);

                    foreach (var x in poObject.Proveedores)
                    {
                        x.Id = x.IdCotizacionProveedor;
                        saberId = x.Id;
                        saberId = saberId + 1;
                        plDetalleGlobal.AddRange(loLogicaNegocio.goBuscarCotizacionCompraDetalles(x.IdCotizacionProveedor));
                        plAdjtuntoGlobal.AddRange(loLogicaNegocio.goBuscarCotizacionAdjunto(x.IdCotizacionProveedor));
                    }
                    
                    bsProveedor.DataSource = poObject.Proveedores;
                        // bsAdjunto.DataSource = plAdjtuntoGlobal;

                        gcProveedor.RefreshDataSource();
                        gcAdjunto.RefreshDataSource();
                        gcCotizacionDetalle.RefreshDataSource();
                        gcBandejaSolicitud.RefreshDataSource();
                        Cargartotal();
                    

                }

            }

            lValidarBotones();
        }

        private void Cargartotal()
        {

            var poListaProveedor = (List<CotizacionProveedor>)bsProveedor.DataSource;

            foreach (var proveedor in poListaProveedor)
            {
                proveedor.Total = 0;
                proveedor.Iva = 0;
                proveedor.SubTotal = 0;
                var polistaDetalle = plDetalleGlobal.Where(i => i.IdCotizacionProveedor == proveedor.Id).ToList();

                foreach (var item in polistaDetalle)
                {
                    proveedor.Total = decimal.Round(item.Total + proveedor.Total,2);
                    proveedor.SubTotal =decimal.Round(item.SubTotal + proveedor.SubTotal,2);
                    proveedor.Iva = decimal.Round(item.ValorIva + proveedor.Iva,2);
                }
            }

            dgvProveedor.RefreshData();
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
        private void lColocarbotonVisualizarDEtalle(DevExpress.XtraGrid.Columns.GridColumn colXmlDown)
        {
            colXmlDown.Caption = "Ver";
            colXmlDown.ColumnEdit = rpiBtnShowDetalle;
            colXmlDown.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            colXmlDown.OptionsColumn.AllowSize = false;

            rpiBtnShowDetalle.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            rpiBtnShowDetalle.Buttons[0].ImageOptions.Image = DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/show_16x16.png");
            rpiBtnShowDetalle.TextEditStyle = TextEditStyles.HideTextEditor;
            colXmlDown.Width = 80;
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
        void OnCopyItemClick( object sender, EventArgs e)
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





        #endregion

    }
}
