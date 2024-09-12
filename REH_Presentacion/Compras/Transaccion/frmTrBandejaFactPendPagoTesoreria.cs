using COM_Negocio;
using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using GEN_Entidad.Entidades.Ventas;
using REH_Presentacion.Compras.Transaccion.Modal;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VTA_Negocio;

namespace REH_Presentacion.Compras.Transaccion
{

    public partial class frmTrBandejaFactPendPagoTesoreria : frmBaseTrxDev
    {

        #region Variables
        clsNOrdenPago loLogicaNegocio;
        RepositoryItemButtonEdit rpiBtnDel;
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        RepositoryItemMemoEdit rpiMedDescripcion = new RepositoryItemMemoEdit();
        RepositoryItemButtonEdit rpiBtnAdd = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDownload = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShowArchivo = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnShowOP = new RepositoryItemButtonEdit();
        BindingSource bsHistorial;
        BindingSource bsRebate;
        List<int> lIdEliminar = new List<int>();
        FacturaAprobadasDetalleGrid poEntidadAdj = new FacturaAprobadasDetalleGrid();
        List<FacturaAprobadasDetalleGrid> loListaGlobal = new List<FacturaAprobadasDetalleGrid>();
        RepositoryItemButtonEdit rpiBtnDet = new RepositoryItemButtonEdit();
        private decimal customSum = 0;
        private decimal customSumGroup = 0;
        bool pbCargado = false;

        #endregion

        #region Eventos

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmTrBandejaFactPendPagoTesoreria()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNOrdenPago();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
            rpiMedDescripcion.WordWrap = true;
            rpiBtnAdd.ButtonClick += rpiBtnAdd_ButtonClick;
            rpiBtnShowArchivo.ButtonClick += rpiBtnShowArchivo_ButtonClick;
            rpiBtnDownload.ButtonClick += rpiBtnDownload_ButtonClick;
            rpiBtnShowOP.ButtonClick += rpiBtnShowOP_ButtonClick;
            rpiBtnDet.ButtonClick += rpiBtnDet_ButtonClick;

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
                int piIndex = dgvRebate.GetFocusedDataSourceRowIndex();
                var poLista = (List<FacturaAprobadasDetalleGrid>)bsRebate.DataSource;
                if (piIndex >= 0)
                {
                    var poListaCuentas = loLogicaNegocio.goConsultarCuentasProveedor(poLista[piIndex].IdProveedor);
                    var poListaObject = poListaCuentas;
                    DataTable dt = new DataTable();

                    dt.Columns.AddRange(new DataColumn[]
                                        {
                                            new DataColumn("Id"),
                                            new DataColumn("Nombre"),
                                            new DataColumn("Banco"),
                                            new DataColumn("Tipo Cuenta"),
                                            new DataColumn("Cuenta")
                                        });

                    poListaObject.ForEach(a =>
                    {
                        DataRow row = dt.NewRow();
                        row["Id"] = a.IdProveedorCuenta;
                        row["Nombre"] = a.Nombre;
                        row["Banco"] = a.DesBanco;
                        row["Tipo Cuenta"] = a.DesTipoCuentaBancaria;
                        row["Cuenta"] = a.NumeroCuenta;

                        dt.Rows.Add(row);
                    });

                    frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Cuentas Bancarias del Proveedor" };
                    if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                    {
                        int pId = int.Parse(pofrmBuscar.lsCodigoSeleccionado);
                        var poRegistro = poListaCuentas.Where(x => x.IdProveedorCuenta == pId).FirstOrDefault();
                        if (poRegistro != null)
                        {
                            poLista[piIndex].CodigoBanco = poRegistro.CodigoBanco;
                            poLista[piIndex].CodigoFormaPago = poRegistro.CodigoFormaPago;
                            poLista[piIndex].CodigoTipoCuentaBancaria = poRegistro.CodigoTipoCuentaBancaria;
                            poLista[piIndex].CodigoTipoIdentificacion = poRegistro.CodigoTipoIdentificacion;
                            poLista[piIndex].IdentificacionCuenta = poRegistro.Identificacion;
                            poLista[piIndex].Nombre = poRegistro.Nombre;
                            poLista[piIndex].NumeroCuenta = poRegistro.NumeroCuenta;

                            dgvRebate.RefreshData();

                            clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);
                        }

                    }
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
        private void rpiBtnShowOP_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvRebate.GetFocusedDataSourceRowIndex();
                var poLista = (List<FacturaAprobadasDetalleGrid>)bsRebate.DataSource;
                if (piIndex >= 0)
                {
                    if (poLista[piIndex].IdOrdenPago != 0)
                    {

                        string psForma = Diccionario.Tablas.Menu.OrdenPago;
                        var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                        if (poForm != null)
                        {
                            frmTrOrdenPago poFrmMostrarFormulario = new frmTrOrdenPago();
                            poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            poFrmMostrarFormulario.Text = poForm.Nombre;
                            poFrmMostrarFormulario.ShowInTaskbar = true;
                            poFrmMostrarFormulario.MdiParent = this.ParentForm;
                            poFrmMostrarFormulario.lid = poLista[piIndex].IdOrdenPago;
                            poFrmMostrarFormulario.Show();
                        }
                        else
                        {
                            XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show(string.Format("Factura: '{0}', No viene de una orden de pago.", poLista[piIndex].NumDocumento), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Exportar, Exporta a TXT.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerarPagos_Click(object sender, EventArgs e)
        {
            try
            {

                dgvRebate.PostEditor();

                var poLista = (List<FacturaAprobadasDetalleGrid>)bsRebate.DataSource;
                poLista.ForEach(x => x.Sel = false);

                for (int i = 0; i < dgvRebate.SelectedRowsCount; i++)
                {
                    int pIdSel = dgvRebate.GetSelectedRows()[i];
                    //int pId = int.Parse(dgvRebate.GetRowCellValue(i, "IdFacturaPago").ToString());
                    //var asd = poLista.Where(x => x.IdFacturaPago == pId).FirstOrDefault();
                    if (pIdSel >= 0)
                    {
                        poLista[pIdSel].Sel = true;
                    }
                }

                poLista = ((List<FacturaAprobadasDetalleGrid>)bsRebate.DataSource).Where(x => x.Sel).ToList();

                string psValid = "";
                foreach (var item in poLista.Where(x => x.ValorPago != x.ValorPagoOriginal))
                {
                    if (string.IsNullOrEmpty(item.ComentarioAprobador))
                    {
                        psValid = string.Format("{0} Factura: {1} no es posible cambiar el valor orginal de: {2}. No existe comentario del aprobador", psValid, item.NumDocumento, item.ValorPagoOriginal.ToString("c2"));
                    }
                }
                if (string.IsNullOrEmpty(psValid))
                {

                    if (poLista.Count > 0)
                    {
                        string psMsg, psNameFile;
                        //var poListaNew = loLogicaNegocio.goArchivoPagosMultiCash(int.Parse(cmbSemana.EditValue.ToString()),poLista,clsPrincipal.gsUsuario,out psMsg, out psNameFile);
                        var poListaNew = loLogicaNegocio.goArchivoPagosMultiCash(poLista, clsPrincipal.gsUsuario, out psMsg, out psNameFile);

                        if (string.IsNullOrEmpty(psMsg))
                        {

                            try
                            {

                                SaveFileDialog sfd = new SaveFileDialog();
                                sfd.Filter = ".TXT Files (*.txt)|*.txt";
                                sfd.FileName = psNameFile;
                                if (sfd.ShowDialog() == DialogResult.OK)
                                {
                                    string path = Path.GetDirectoryName(sfd.FileName);
                                    string filename = Path.GetFileNameWithoutExtension(sfd.FileName);
                                    string psRuta = sfd.FileName;
                                    //Pass the filepath and filename to the StreamWriter Constructor
                                    StreamWriter sw = new StreamWriter(psRuta);
                                    int piTotFilas = poListaNew.Count();
                                    int piCont = 0;
                                    foreach (var reigstro in poListaNew)
                                    {

                                        piCont++;
                                        if (piCont == piTotFilas)
                                        {
                                            sw.Write(reigstro.Cuerpo);
                                        }
                                        else
                                        {
                                            sw.WriteLine(reigstro.Cuerpo);
                                        }

                                    }

                                    sw.Close();

                                    XtraMessageBox.Show("Guardado Exitosamente", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                            }
                            catch (Exception exp)
                            {
                                XtraMessageBox.Show(exp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    else
                    {
                        XtraMessageBox.Show("No existen registros seleccionados para generar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show(psValid, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Exportar, Exporta a Excel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbcDocumentosPagar.SelectedTabPageIndex == 0)
                {
                    List<FacturaAprobadasDetalleGrid> poLista = (List<FacturaAprobadasDetalleGrid>)bsRebate.DataSource;
                    if (poLista != null && poLista.Count > 0)
                    {
                        string psFilter = "Files(*.xlsx;)|*.xlsx;";
                        clsComun.gSaveFile(gcRebate, Text + ".xlsx", psFilter);
                    }
                }
                else
                {
                    List<FacturaAprobadasDetalleGrid> poLista = (List<FacturaAprobadasDetalleGrid>)bsHistorial.DataSource;
                    if (poLista != null && poLista.Count > 0)
                    {
                        string psFilter = "Files(*.xlsx;)|*.xlsx;";
                        clsComun.gSaveFile(gcHistorial, Text + ".xlsx", psFilter);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
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

        /// <summary>
        /// Evento del boton de Mostrar detalle de la fila seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDet_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                dgvHistorial.PostEditor();
                // Tomamos la fila seleccionada
                piIndex = dgvHistorial.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<FacturaAprobadasDetalleGrid>)bsHistorial.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poDetalle = loListaGlobal.Where(x => x.IdProveedor == poLista[piIndex].IdProveedor && x.IdGrupoPago == poLista[piIndex].IdGrupoPago).ToList();

                    frmDetalleFacturaProveedor frmComisionDetalle = new frmDetalleFacturaProveedor();
                    frmComisionDetalle.loLista = poDetalle;
                    frmComisionDetalle.ShowDialog();

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
                piIndex = dgvHistorial.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<FacturaAprobadasDetalleGrid>)bsHistorial.DataSource;


                // Presenta un dialogo para seleccionar las imagenes
                ofdArchivoAdjunto.Title = "Seleccione Archivo pdf";
                ofdArchivoAdjunto.Filter = "Image Files( *.pdf; )|  *.pdf; ";

                if (ofdArchivoAdjunto.ShowDialog() == DialogResult.OK)
                {
                    if (poLista.Count > 0 && piIndex >= 0)
                    {

                        if (!ofdArchivoAdjunto.FileName.Equals(""))
                        {
                            FileInfo file = new FileInfo(ofdArchivoAdjunto.FileName);
                            var piSize = file.Length;

                            if (piSize <= clsPrincipal.gdcTamanoMb * 1048576)
                            {
                                string Name = file.Name + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff") + Path.GetExtension(ofdArchivoAdjunto.FileName);
                                var poEntidad = poLista[piIndex];

                                poLista[piIndex].ArchivoAdjunto = Name;
                                poLista[piIndex].RutaOrigen = ofdArchivoAdjunto.FileName;
                                poLista[piIndex].NombreOriginal = file.Name;
                                poLista[piIndex].RutaDestino = ConfigurationManager.AppSettings["CarpetaOPBcoCompras"].ToString() + Name;
                                // Asigno mi nueva lista al Binding Source
                                bsHistorial.DataSource = poLista;
                                dgvHistorial.RefreshData();
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
        private void rpiBtnShowArchivo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvHistorial.GetFocusedDataSourceRowIndex();
                var poLista = (List<FacturaAprobadasDetalleGrid>)bsHistorial.DataSource;

                if (!string.IsNullOrEmpty(poLista[piIndex].ArchivoAdjunto))
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
                        pofrmVerPdf.lsRuta = poLista[piIndex].RutaDestino + poLista[piIndex].ArchivoAdjunto;
                        pofrmVerPdf.Show();
                        pofrmVerPdf.SetDesktopLocation(0, 0);

                        pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;
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

        /// <summary>
        /// Evento del boton de Descargar en el Grid, descarga el archivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDownload_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {

                int piIndex = dgvHistorial.GetFocusedDataSourceRowIndex();
                var poLista = (List<FacturaAprobadasDetalleGrid>)bsHistorial.DataSource;

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

        #endregion

        private void frmTrRebate_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();

                clsComun.gLLenarCombo(ref cmbGrupoPago, loLogicaNegocio.goConsultarComboFacturaGrupoPagoAprobadosTesoreria(), false, true);
                clsComun.gLLenarCombo(ref cmbGrupoPagoHistorico, loLogicaNegocio.goConsultarComboGrupoPagosHistorico());

                clsComun.gLLenarComboGrid(ref dgvHistorial, loLogicaNegocio.goConsultarComboEstado(), "CodigoEstado");
                //clsComun.gLLenarComboGrid(ref dgvHistorial, loLogicaNegocio.goConsultarComboSemanaPagosTodos(), "IdSemanaPago");

                clsComun.gLLenarComboGrid(ref dgvRebate, loLogicaNegocio.goConsultarComboEstado(), "CodigoEstado");
                clsComun.gLLenarComboGrid(ref dgvRebate, loLogicaNegocio.goConsultarComboBanco(), "CodigoBanco", true);
                clsComun.gLLenarComboGrid(ref dgvRebate, loLogicaNegocio.goConsultarComboFormaPago(), "CodigoFormaPago", true);
                clsComun.gLLenarComboGrid(ref dgvRebate, loLogicaNegocio.goConsultarComboTipoIdentificación(), "CodigoTipoIdentificacion", true);
                clsComun.gLLenarComboGrid(ref dgvRebate, loLogicaNegocio.goConsultarComboTipoCuentaBancaria(), "CodigoTipoCuentaBancaria", true);

                pbCargado = true;
                lBuscar();
                tbcDocumentosPagar_SelectedPageChanged(null, null);
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
                DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (dialogResult == DialogResult.Yes)
                {
                    var poLista = (List<FacturaAprobadasDetalleGrid>)bsHistorial.DataSource;
                    foreach (var item in poLista)
                    {
                        foreach (var fila in loListaGlobal.Where(x => x.IdProveedor == item.IdProveedor && x.IdGrupoPago == item.IdGrupoPago))
                        {
                            fila.Sel = true;
                            fila.ValorPago = item.ValorPago;
                            fila.ArchivoAdjunto = item.ArchivoAdjunto;
                            fila.RutaOrigen = item.RutaOrigen;
                            fila.NombreOriginal = item.NombreOriginal;
                            fila.RutaDestino = item.RutaDestino;
                        }

                    }
                    var psMsg = loLogicaNegocio.gGuardaAdjuntosArchivoPago(cmbGrupoPagoHistorico.EditValue.ToString(), poEntidadAdj, loListaGlobal, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lBuscarHistorial();
                        lBuscarAdjunto();
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

        #region Métodos

        private void lCargarEventosBotones()
        {
            gCrearBotones();

            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnGenerarPagos"] != null) tstBotones.Items["btnGenerarPagos"].Click += btnGenerarPagos_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;

            /*********************************************************************************************************************************************/

            bsRebate = new BindingSource();
            bsRebate.DataSource = new List<FacturaAprobadasDetalleGrid>();
            gcRebate.DataSource = bsRebate;

            dgvRebate.OptionsBehavior.Editable = true;
            dgvRebate.OptionsPrint.AutoWidth = false;

            dgvRebate.OptionsSelection.MultiSelect = true;
            dgvRebate.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            dgvRebate.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            dgvRebate.OptionsSelection.CheckBoxSelectorColumnWidth = 40;

            dgvRebate.OptionsCustomization.AllowSort = false;

            dgvRebate.OptionsView.ShowGroupedColumns = true;
            dgvRebate.Columns["Proveedor"].GroupIndex = 0;
            dgvRebate.ExpandAllGroups();

            dgvRebate.Appearance.GroupFooter.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dgvRebate.Appearance.FooterPanel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold);
            dgvRebate.Appearance.FooterPanel.Options.UseFont = true;
            dgvRebate.OptionsBehavior.Editable = true;

            dgvRebate.Columns["ArchivoAdjunto"].Visible = false;
            dgvRebate.Columns["RutaOrigen"].Visible = false;
            dgvRebate.Columns["RutaDestino"].Visible = false;
            dgvRebate.Columns["NombreOriginal"].Visible = false;
            dgvRebate.Columns["Sel"].Visible = false;
            dgvRebate.Columns["Det"].Visible = false;

            dgvRebate.Columns["IdOrdenPagoFactura"].Visible = false;
            dgvRebate.Columns["IdSemanaPago"].Visible = false;
            dgvRebate.Columns["IdFacturaPago"].Visible = false;
            dgvRebate.Columns["Identificacion"].Visible = false;
            //dgvRebate.Columns["Sel"].Visible = false;
            dgvRebate.Columns["Generado"].Visible = false;
            dgvRebate.Columns["Del"].Visible = false;
            dgvRebate.Columns["IdOrdenPago"].Visible = false;
            dgvRebate.Columns["Valor"].Visible = false;
            dgvRebate.Columns["Abono"].Visible = false;
            dgvRebate.Columns["Saldo"].Visible = false;
            dgvRebate.Columns["FechaEmision"].Visible = false;
            //dgvRebate.Columns["Ver"].Visible = false;
            dgvRebate.Columns["Aprobaciones"].Visible = false;
            dgvRebate.Columns["Aprobo"].Visible = false;
            dgvRebate.Columns["DocNum"].Visible = false;
            dgvRebate.Columns["Add"].Visible = false;
            dgvRebate.Columns["Descargar"].Visible = false;
            dgvRebate.Columns["Visualizar"].Visible = false;
            dgvRebate.Columns["FechaPago"].Visible = false;
            dgvRebate.Columns["IdProveedor"].Visible = false;
            dgvRebate.Columns["IdGrupoPago"].Visible = false;

            dgvRebate.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvRebate.Columns["Proveedor"].ColumnEdit = rpiMedDescripcion;
            dgvRebate.Columns["Aprobo"].ColumnEdit = rpiMedDescripcion;

            dgvRebate.Columns["DocNum"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Proveedor"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["FechaEmision"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["FechaVencimiento"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["NumDocumento"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Valor"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Abono"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Saldo"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Observacion"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CodigoEstado"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["ValorPagoOriginal"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["ValorPago"].OptionsColumn.AllowEdit = true;
            dgvRebate.Columns["ComentarioAprobador"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Aprobaciones"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Aprobo"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CodigoTipoIdentificacion"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["IdentificacionCuenta"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["Nombre"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CodigoBanco"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CodigoFormaPago"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["CodigoTipoCuentaBancaria"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["NumeroCuenta"].OptionsColumn.AllowEdit = false;
            dgvRebate.Columns["GrupoPago"].OptionsColumn.AllowEdit = false;

            dgvRebate.Columns["ValorPago"].Caption = "Valor a Pagar";
            //dgvRebate.Columns["Ver"].Caption = "Seleccionar Cuenta";
            dgvRebate.Columns["NumDocumento"].Caption = "# Factura";
            dgvRebate.Columns["CodigoEstado"].Caption = "Estado";
            dgvRebate.Columns["Aprobo"].Caption = "Aprobó";
            dgvRebate.Columns["CodigoTipoIdentificacion"].Caption = "Tipo Identifiación";
            dgvRebate.Columns["IdentificacionCuenta"].Caption = "Identificación";
            dgvRebate.Columns["Nombre"].Caption = "Nombre";
            dgvRebate.Columns["CodigoBanco"].Caption = "Banco";
            dgvRebate.Columns["CodigoFormaPago"].Caption = "Forma Pago";
            dgvRebate.Columns["CodigoTipoCuentaBancaria"].Caption = "Tipo Cuenta";
            dgvRebate.Columns["NumeroCuenta"].Caption = "Cuenta";

            dgvRebate.Columns["ValorPago"].UnboundType = UnboundColumnType.Decimal;
            dgvRebate.Columns["ValorPago"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvRebate.Columns["ValorPago"].DisplayFormat.FormatString = "c2";

            dgvRebate.Columns["ValorPago"].SummaryItem.SummaryType = SummaryItemType.Custom;
            dgvRebate.Columns["ValorPago"].SummaryItem.DisplayFormat = "{0:c2}";
            dgvRebate.Columns["ValorPago"].SummaryItem.Tag = 1;

            dgvRebate.GroupSummary.Clear();

            GridGroupSummaryItem sumColumn = new GridGroupSummaryItem();
            //sumColumn = dgvRebate.GroupSummary.Add(SummaryItemType.Sum, "ValorPago", dgvRebate.Columns["ValorPago"], "{0:c2}");
            sumColumn.ShowInGroupColumnFooter = dgvRebate.Columns["ValorPago"];
            sumColumn.FieldName = "ValorPago";
            sumColumn.SummaryType = SummaryItemType.Custom;
            sumColumn.Tag = 2;
            sumColumn.DisplayFormat = "{0:c2}";

            dgvRebate.GroupSummary.Add(sumColumn);

            dgvRebate.CustomSummaryCalculate += gridView1_CustomSummaryCalculate;
            dgvRebate.SelectionChanged += gridView1_SelectionChanged;

            dgvRebate.UpdateSummary();

            dgvRebate.FixedLineWidth = 3;
            dgvRebate.Columns["Sel"].Fixed = FixedStyle.Left;
            dgvRebate.Columns["Proveedor"].Fixed = FixedStyle.Left;
            dgvRebate.Columns["NumDocumento"].Fixed = FixedStyle.Left;
            //dgvRebate.Columns["Ver"].Fixed = FixedStyle.Right;

            clsComun.gDibujarBotonGrid(rpiBtnShow, dgvRebate.Columns["Ver"], "Seleccionar Cuenta", Diccionario.ButtonGridImage.show_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowOP, dgvRebate.Columns["VerOP"], "Orden Pago", Diccionario.ButtonGridImage.show_16x16);

            dgvRebate.OptionsView.RowAutoHeight = true;
            dgvRebate.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;

            dgvRebate.BestFitColumns();

            //dgvRebate.Columns["NumDocumento"].Width = 130;

            /*********************************************************************************************************************************************/

            bsHistorial = new BindingSource();
            bsHistorial.DataSource = new List<FacturaAprobadasDetalleGrid>();
            gcHistorial.DataSource = bsHistorial;

            dgvHistorial.OptionsBehavior.Editable = true;

            dgvHistorial.Columns["ArchivoAdjunto"].Visible = false;
            dgvHistorial.Columns["RutaOrigen"].Visible = false;
            dgvHistorial.Columns["RutaDestino"].Visible = false;
            //dgvHistorial.Columns["NombreOriginal"].Visible = false;
            dgvHistorial.Columns["Ver"].Visible = false;
            dgvHistorial.Columns["IdGrupoPago"].Visible = false;

            dgvHistorial.Columns["IdOrdenPagoFactura"].Visible = false;
            //dgvHistorial.Columns["IdSemanaPago"].Visible = false;
            dgvHistorial.Columns["IdFacturaPago"].Visible = false;
            dgvHistorial.Columns["Identificacion"].Visible = false;
            //dgvRebate.Columns["Sel"].Visible = false;
            dgvHistorial.Columns["Generado"].Visible = false;
            dgvHistorial.Columns["Del"].Visible = false;
            dgvHistorial.Columns["IdOrdenPago"].Visible = false;
            dgvHistorial.Columns["Valor"].Visible = false;
            dgvHistorial.Columns["Abono"].Visible = false;
            dgvHistorial.Columns["Saldo"].Visible = false;
            dgvHistorial.Columns["FechaEmision"].Visible = false;
            //dgvRebate.Columns["Ver"].Visible = false;
            dgvHistorial.Columns["Aprobaciones"].Visible = false;
            dgvHistorial.Columns["Aprobo"].Visible = false;
            dgvHistorial.Columns["DocNum"].Visible = false;
            dgvHistorial.Columns["Valor"].Visible = false;
            dgvHistorial.Columns["Abono"].Visible = false;
            dgvHistorial.Columns["Nombre"].Visible = false;
            dgvHistorial.Columns["CodigoBanco"].Visible = false;
            dgvHistorial.Columns["CodigoFormaPago"].Visible = false;
            dgvHistorial.Columns["CodigoTipoCuentaBancaria"].Visible = false;
            dgvHistorial.Columns["NumeroCuenta"].Visible = false;
            dgvHistorial.Columns["CodigoTipoIdentificacion"].Visible = false;
            dgvHistorial.Columns["IdentificacionCuenta"].Visible = false;
            dgvHistorial.Columns["IdProveedor"].Visible = false;
            dgvHistorial.Columns["Sel"].Visible = false;
            dgvHistorial.Columns["IdSemanaPago"].Visible = false;
            dgvHistorial.Columns["FechaVencimiento"].Visible = false;
            dgvHistorial.Columns["ComentarioAprobador"].Visible = false;
            dgvHistorial.Columns["Observacion"].Visible = false;
            dgvHistorial.Columns["CodigoEstado"].Visible = false;
            dgvHistorial.Columns["ComentarioAprobador"].Visible = false;
            dgvHistorial.Columns["VerOP"].Visible = false;

            dgvHistorial.Columns["Observacion"].ColumnEdit = rpiMedDescripcion;
            dgvHistorial.Columns["Proveedor"].ColumnEdit = rpiMedDescripcion;
            dgvHistorial.Columns["Aprobo"].ColumnEdit = rpiMedDescripcion;

            dgvHistorial.Columns["GrupoPago"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["DocNum"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["Proveedor"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["FechaEmision"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["FechaVencimiento"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["NumDocumento"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["Saldo"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["Observacion"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["CodigoEstado"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["ValorPago"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["Aprobaciones"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["Aprobo"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["NombreOriginal"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["FechaPago"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["IdSemanaPago"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["ComentarioAprobador"].OptionsColumn.AllowEdit = false;
            dgvHistorial.Columns["ValorPagoOriginal"].OptionsColumn.AllowEdit = false;

            dgvHistorial.Columns["ValorPago"].Caption = "Valor Pagado";
            dgvHistorial.Columns["NumDocumento"].Caption = "# Factura";
            dgvHistorial.Columns["CodigoEstado"].Caption = "Estado";
            dgvHistorial.Columns["Aprobo"].Caption = "Aprobó";
            dgvHistorial.Columns["NombreOriginal"].Caption = "Archivo";
            dgvHistorial.Columns["IdSemanaPago"].Caption = "Semana de pago";

            clsComun.gDibujarBotonGrid(rpiBtnDet, dgvHistorial.Columns["Det"], "Detalle", Diccionario.ButtonGridImage.inserttable_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnAdd, dgvHistorial.Columns["Add"], "Agregar", Diccionario.ButtonGridImage.open_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnDownload, dgvHistorial.Columns["Descargar"], "Descargar", Diccionario.ButtonGridImage.download_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnShowArchivo, dgvHistorial.Columns["Visualizar"], "Visualizar", Diccionario.ButtonGridImage.show_16x16);

            clsComun.gFormatearColumnasGrid(dgvHistorial);
            clsComun.gOrdenarColumnasGridFullEditable(dgvHistorial);

            dgvHistorial.FixedLineWidth = 2;
            dgvHistorial.Columns["Proveedor"].Fixed = FixedStyle.Left;
            dgvHistorial.Columns["FechaPago"].Fixed = FixedStyle.Left;

            /*********************************************************************************************************************************************/


            dtpFechaInicio.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaFin.EditValue = DateTime.Now;

        }

        void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            dgvRebate.UpdateSummary();
        }

        void gridView1_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            GridView view = sender as GridView;

            //Get the summary ID. 
            int summaryID = Convert.ToInt32((e.Item as GridSummaryItem).Tag);

            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Start)
            {
                customSum = 0;
                customSumGroup = 0;
            }

            // Calculation.
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Calculate)
            {
                if (dgvRebate.IsRowSelected(e.RowHandle))
                {
                    //customSum += decimal.Parse(e.FieldValue.ToString());

                    switch (summaryID)
                    {
                        case 1: // The total summary calculated against the 'UnitPrice' column. 
                                //int unitsInStock = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "UnitsInStock"));
                            customSum += decimal.Parse(e.FieldValue.ToString());
                            break;
                        case 2: // The group summary. 
                                //Boolean isDiscontinued = Convert.ToBoolean(e.FieldValue);
                            customSumGroup += decimal.Parse(e.FieldValue.ToString());
                            break;
                    }
                }


            }

            // Finalization. 
            if (e.SummaryProcess == DevExpress.Data.CustomSummaryProcess.Finalize)
            {
                //e.TotalValue = customSum;

                switch (summaryID)
                {
                    case 1:
                        e.TotalValue = customSum;
                        break;
                    case 2:
                        e.TotalValue = customSumGroup;
                        break;
                }

            }
        }

        private void lLimpiar()
        {
            //if ((cmbProducto.Properties.DataSource as IList).Count > 0) cmbProducto.ItemIndex = 0;
            //if ((cmbZona.Properties.DataSource as IList).Count > 0) cmbZona.ItemIndex = 0;
            //txtUnidades.Text = "0";
            //txtPrecio.Text = "0";
            //txtValor.Text = "0";
            //txtMedidaConversion.Text = "0";
            //txtTotal.Text = "0";
            //txtTipoProducto.Text = string.Empty;
            //txtAnio.Text = string.Empty;
            bsHistorial.DataSource = new List<FacturaAprobadasDetalleGrid>();
            bsRebate.DataSource = new List<FacturaAprobadasDetalleGrid>();
            gcRebate.DataSource = bsRebate;
            dtpFechaInicio.Enabled = true;
            dtpFechaFin.Enabled = true;
            dtpFechaInicio.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpFechaFin.EditValue = DateTime.Now;
            lIdEliminar = new List<int>();
        }

        private void lBuscar()
        {

            Cursor.Current = Cursors.WaitCursor;
            string psGrupoPago = cmbGrupoPago.EditValue.ToString();

            loListaGlobal = loLogicaNegocio.goConsultaFacturasPendientePagoTesoreria(psGrupoPago);

            bsRebate.DataSource = loListaGlobal;
            gcRebate.DataSource = bsRebate;
            dgvRebate.ExpandAllGroups();

            clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);

            lIdEliminar = new List<int>();

            lBuscarAdjunto();

            Cursor.Current = Cursors.Default;

        }

        private void lBuscarHistorial()
        {

            Cursor.Current = Cursors.WaitCursor;


            loListaGlobal = loLogicaNegocio.goConsultaHistorialFacturasPendientePagoTesoreria(dtpFechaInicio.DateTime, dtpFechaFin.DateTime);

            var poListaAgrupada = loListaGlobal.GroupBy(x => new
            {
                x.IdProveedor,
                x.Proveedor,
                x.IdGrupoPago,
                x.GrupoPago,
                x.ArchivoAdjunto,
                x.RutaOrigen,
                x.NombreOriginal,
                x.RutaDestino,
                x.FechaPago
            }).Select(y => new FacturaAprobadasDetalleGrid()
            {
                IdGrupoPago = y.Key.IdGrupoPago,
                GrupoPago = y.Key.GrupoPago,
                IdProveedor = y.Key.IdProveedor,
                Proveedor = y.Key.Proveedor,
                ValorPagoOriginal = y.Sum(x => x.ValorPagoOriginal),
                Valor = y.Sum(x => x.Valor),
                ValorPago = y.Sum(x => x.ValorPago),
                ComentarioAprobador = y.Max(x => x.ComentarioAprobador),
                ArchivoAdjunto = y.Key.ArchivoAdjunto,
                RutaOrigen = y.Key.RutaOrigen,
                NombreOriginal = y.Key.NombreOriginal,
                RutaDestino = y.Key.RutaDestino,
                FechaPago = y.Key.FechaPago

            }).ToList();

            foreach (var item in poListaAgrupada)
            {
                foreach (var fila in loListaGlobal.Where(x => x.IdProveedor == item.IdProveedor && x.IdGrupoPago == item.IdGrupoPago ))
                {
                    item.NumDocumento += fila.NumDocumento + ",";
                }

                item.NumDocumento = item.NumDocumento.Substring(0, item.NumDocumento.Length - 1);
            }

            bsHistorial.DataSource = poListaAgrupada;
            gcHistorial.DataSource = bsHistorial;

            clsComun.gOrdenarColumnasGridFullEditable(dgvHistorial);

            dgvHistorial.Columns["NumDocumento"].Width = 130;

            Cursor.Current = Cursors.Default;

        }

        private void lBuscarAdjunto()
        {
            txtNo.Text = "";
            txtNo.Tag = "";

            poEntidadAdj.ArchivoAdjunto = "";
            poEntidadAdj.NombreOriginal = "";
            poEntidadAdj.RutaDestino = "";
            poEntidadAdj.RutaOrigen = "";

            var poResult = loLogicaNegocio.gConsultaArchivoPagoResumen(cmbGrupoPagoHistorico.EditValue.ToString());
            if (poResult != null)
            {
                poEntidadAdj.ArchivoAdjunto = poResult.ArchivoAdjunto;
                poEntidadAdj.NombreOriginal = poResult.NombreOriginal;
                poEntidadAdj.RutaDestino = poResult.RutaDestino;

                txtNo.Text = poResult.NombreOriginal;
                txtNo.Tag = poResult.ArchivoAdjunto;
            }
        }

        #endregion


        private void lCalcularTotal()
        {
            var pdcTotal = ((List<FacturaAprobadasDetalleGrid>)bsRebate.DataSource).Where(x => x.Sel).Sum(x => x.ValorPago);
            lblTotal.Text = pdcTotal.ToString("c2");
        }

        private void dgvRebate_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Sel")
                {
                    dgvRebate.PostEditor();
                    lCalcularTotal();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddManualmente_Click(object sender, EventArgs e)
        {
            try
            {
                List<FacturaAprobadasDetalleGrid> poLista = (List<FacturaAprobadasDetalleGrid>)bsRebate.DataSource;

                var sel = poLista.Select(x => x.Sel).FirstOrDefault();
                foreach (var item in poLista)
                {
                    item.Sel = !sel;
                }


                bsRebate.DataSource = poLista;
                dgvRebate.RefreshData();
                lCalcularTotal();
                //clsComun.gOrdenarColumnasGridFullEditable(dgvRebate);

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdjuntar_Click(object sender, EventArgs e)
        {
            try
            {
                // Presenta un dialogo para seleccionar las imagenes
                ofdArchivo.Title = "Seleccione Archivo pdf";
                ofdArchivo.Filter = "Image Files( *.pdf; )|  *.pdf; ";

                if (ofdArchivo.ShowDialog() == DialogResult.OK)
                {


                    if (!ofdArchivo.FileName.Equals(""))
                    {
                        FileInfo file = new FileInfo(ofdArchivo.FileName);
                        var piSize = file.Length;

                        if (piSize <= clsPrincipal.gdcTamanoMb * 1048576)
                        {
                            string Name = file.Name + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff") + Path.GetExtension(ofdArchivo.FileName);
                            poEntidadAdj = new FacturaAprobadasDetalleGrid();

                            poEntidadAdj.ArchivoAdjunto = Name;
                            poEntidadAdj.RutaOrigen = ofdArchivo.FileName;
                            poEntidadAdj.NombreOriginal = file.Name;
                            poEntidadAdj.RutaDestino = ConfigurationManager.AppSettings["CarpetaOPBcoCompras"].ToString() + Name;


                            txtNo.Text = file.Name;
                            txtNo.Tag = Name;

                        }

                        else
                        {
                            XtraMessageBox.Show("El tamano máximo permitido es de: " + clsPrincipal.gdcTamanoMb + "mb", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(poEntidadAdj.ArchivoAdjunto))
                {
                    frmVerPdf pofrmVerPdf = new frmVerPdf();
                    //Muestra archivo local
                    if (!string.IsNullOrEmpty(poEntidadAdj.RutaOrigen))
                    {
                        pofrmVerPdf.lsRuta = poEntidadAdj.RutaOrigen;
                        pofrmVerPdf.Show();
                        pofrmVerPdf.SetDesktopLocation(0, 0);
                        pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;
                    }
                    //Muestra archivo ya subido
                    else
                    {
                        pofrmVerPdf.lsRuta = poEntidadAdj.RutaDestino + poEntidadAdj.ArchivoAdjunto;
                        pofrmVerPdf.Show();
                        pofrmVerPdf.SetDesktopLocation(0, 0);

                        pofrmVerPdf.Size = SystemInformation.PrimaryMonitorMaximizedWindowSize;
                    }

                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarAdjunto_Click(object sender, EventArgs e)
        {
            try
            {
                poEntidadAdj = new FacturaAprobadasDetalleGrid();
                //loLogicaNegocio.gEliminarchivoPagoResumen(cmbSemana.EditValue.ToString(), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                txtNo.Text = "";
                txtNo.Tag = "";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbSemana_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lBuscarAdjunto();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void tbcDocumentosPagar_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (tbcDocumentosPagar.SelectedTabPageIndex == 0)
            {
                if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Enabled = true;
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                if (tstBotones.Items["btnGenerarPagos"] != null) tstBotones.Items["btnGenerarPagos"].Enabled = true;
            }
            else
            {
                if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Enabled = false;
                if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                if (tstBotones.Items["btnGenerarPagos"] != null) tstBotones.Items["btnGenerarPagos"].Enabled = false;
            }
        }

        private void btnBuscarPorFechas_Click(object sender, EventArgs e)
        {
            try
            {
                lBuscarHistorial();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbGrupoPago_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lBuscar();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}