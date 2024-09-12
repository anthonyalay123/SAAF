using AFI_Negocio;
using DevExpress.XtraEditors;
using GEN_Entidad;
using GEN_Entidad.Entidades.ActivoFijo;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
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

namespace REH_Presentacion.ActivoFijo
{
    public partial class frmPrItemActivoFijo : frmBaseTrxDev
    {
        clsNActivoFijo loLogicaNegocio = new clsNActivoFijo();
        ItemActivoFijo poEntidadAdj = new ItemActivoFijo();
        private bool pbCargado = false;

        public frmPrItemActivoFijo()
        {
            InitializeComponent();
        }

        private void frmPrItemActivoFijo_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                clsComun.gLLenarCombo(ref cmbSucursal, loLogicaNegocio.goConsultarComboSucursal(), true);
                clsComun.gLLenarCombo(ref cmbCentroCosto, loLogicaNegocio.goConsultarComboCentroCosto(), true);
                clsComun.gLLenarCombo(ref cmbTipoActivoFijo, loLogicaNegocio.goConsultarComboTipoActivoFijo(), true);
                clsComun.gLLenarCombo(ref cmbPersona, loLogicaNegocio.goConsultarComboIdPersona(), true);
                clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstadoActivoFijo(), true);
                clsComun.gLLenarCombo(ref cmbAgrupacion, loLogicaNegocio.goConsultarComboAgrupacionActivoFijo(), true);
                clsComun.gLLenarCombo(ref cmbProveedor, loLogicaNegocio.goConsultarComboProveedorId(), true);
                lLimpiar();
                pbCargado = true;
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
        /// Evento del botón Grabar, Guarda datos del formulario a el objeto de BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {

                string psMsg = lsEsValido();  
                if (string.IsNullOrEmpty(psMsg))

                {
                    ItemActivoFijo poObject = new ItemActivoFijo();
                    poObject.IdItemActivoFijo = string.IsNullOrEmpty(txtNo.Text) ? 0 : Convert.ToInt32(txtNo.Text);
                    poObject.Descripcion = txtDescripcion.Text;
                    poObject.CodigoTipoActivoFijo = cmbTipoActivoFijo.EditValue.ToString();
                    poObject.CodigoSucursal = cmbSucursal.EditValue.ToString();
                    poObject.CodigoCentroCosto = cmbCentroCosto.EditValue.ToString();
                    poObject.FechaCompra = dtpFechaCompra.DateTime;
                    poObject.FechaActivacion = new DateTime(dtpFechaCompra.DateTime.AddMonths(1).Year, dtpFechaCompra.DateTime.AddMonths(1).Month,1); //dtpFechaActivacion.DateTime;
                    poObject.CostoCompra = Convert.ToDecimal(txtCostoCompra.EditValue);
                    poObject.ValorResidual = Convert.ToDecimal(txtValorResidual.EditValue);
                    poObject.ValorDepreciable = Convert.ToDecimal(txtValorDepreciable.EditValue);
                    poObject.DepreciacionAcumulada = Convert.ToDecimal(txtDepreciacionAcumulada.EditValue);
                    poObject.CostoActual = Convert.ToDecimal(txtCostoActual.EditValue);
                    poObject.Marca = txtMarca.Text;
                    poObject.Modelo = txtModelo.Text;
                    poObject.Serie = txtSerie.Text;
                    poObject.ProductId = txtProductId.Text;
                    poObject.NumFactura = txtNumeroFactura.Text;
                    poObject.CodigoEstadoActivoFijo = cmbEstado.EditValue.ToString();
                    poObject.ArchivoAdjunto = poEntidadAdj.ArchivoAdjunto;
                    poObject.NombreOriginal = poEntidadAdj.NombreOriginal;
                    poObject.RutaOrigen = poEntidadAdj.RutaOrigen;
                    poObject.RutaDestino = poEntidadAdj.RutaDestino;

                    if (cmbPersona.EditValue.ToString() == Diccionario.Seleccione)
                    {
                        poObject.Persona = null;
                        poObject.IdPersona = null;
                    }
                    else
                    {
                        poObject.Persona = cmbPersona.Text;
                        poObject.IdPersona = int.Parse(cmbPersona.EditValue.ToString());
                    }

                    if (cmbProveedor.EditValue.ToString() == Diccionario.Seleccione)
                    {
                        poObject.Persona = null;
                        poObject.IdPersona = null;
                    }
                    else
                    {
                        poObject.Proveedor = cmbProveedor.Text;
                        poObject.IdProveedor = int.Parse(cmbProveedor.EditValue.ToString());
                    }

                    if (cmbAgrupacion.EditValue.ToString() == Diccionario.Seleccione)
                    {
                        poObject.CodigoAgrupacion = null;
                        poObject.Agrupacion = null;
                    }
                    else
                    {
                        poObject.Agrupacion = cmbAgrupacion.Text;
                        poObject.CodigoAgrupacion = cmbAgrupacion.EditValue.ToString();
                    }

                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de guardar los datos?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        psMsg = loLogicaNegocio.gsGuardarItemActivoFijo(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
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
                        var msg = loLogicaNegocio.gEliminar(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(msg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(msg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string lsEsValido()
        {
            return "";
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
                var poListaObject = loLogicaNegocio.goListarItemActivoFijo();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("ID"),
                                    new DataColumn("Código"),
                                    new DataColumn("Ítem"),
                                    new DataColumn("Proveedor"),
                                    new DataColumn("Fecha Compra", typeof(DateTime)),
                                    new DataColumn("Estado"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["ID"] = a.IdItemActivoFijo;
                    row["Código"] = a.Codigo;
                    row["Ítem"] = a.Descripcion.Length > 100 ? a.Descripcion.Substring(0, 100) : a.Descripcion;
                    row["Proveedor"] = a.Proveedor;
                    row["Fecha Compra"] = a.FechaCompra;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);

                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Ítems" };
                pofrmBuscar.Width = 1200;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar();
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Primero, txtNo.Text.Trim());
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Anterior, txtNo.Text.Trim());
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Siguiente, txtNo.Text.Trim());
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Ultimo, txtNo.Text.Trim());
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;

            txtCostoCompra.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtCostoCompra.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            //txtCostoCompra.Properties.Mask.EditMask = "c2";
            //txtCostoCompra.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            //txtCostoCompra.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtCostoActual.Properties.Mask.EditMask = "c2";
            txtCostoActual.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtCostoActual.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtDepreciacionAcumulada.Properties.Mask.EditMask = "c2";
            txtDepreciacionAcumulada.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtDepreciacionAcumulada.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtValorDepreciable.Properties.Mask.EditMask = "c2";
            txtValorDepreciable.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtValorDepreciable.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtValorResidual.Properties.Mask.EditMask = "c2";
            txtValorResidual.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtValorResidual.Properties.Mask.UseMaskAsDisplayFormat = true;
        }

        private void lLimpiar()
        {
            pbCargado = false;
            if ((cmbCentroCosto.Properties.DataSource as IList).Count > 0) cmbCentroCosto.ItemIndex = 0;
            if ((cmbSucursal.Properties.DataSource as IList).Count > 0) cmbSucursal.ItemIndex = 0;
            if ((cmbTipoActivoFijo.Properties.DataSource as IList).Count > 0) cmbTipoActivoFijo.ItemIndex = 0;
            if ((cmbPersona.Properties.DataSource as IList).Count > 0) cmbPersona.ItemIndex = 0;
            if ((cmbProveedor.Properties.DataSource as IList).Count > 0) cmbProveedor.ItemIndex = 0;
            dtpFechaCompra.DateTime = DateTime.Now;
            dtpFechaActivacion.DateTime = DateTime.Now;
            txtCostoCompra.EditValue = 0;
            txtCostoActual.EditValue = 0;
            txtDepreciacionAcumulada.EditValue = 0;
            txtDescripcion.EditValue = "";
            txtValorDepreciable.EditValue = 0;
            txtNo.EditValue = "";
            txtValorResidual.EditValue = 0;
            dtpFechaCompra.DateTime = DateTime.Now;
            dtpFechaActivacion.DateTime = DateTime.Now;
            txtMarca.EditValue = "";
            txtModelo.EditValue = "";
            txtSerie.EditValue = "";
            txtProductId.EditValue = "";
            lblFecha.Text = "";
            if ((cmbEstado.Properties.DataSource as IList).Count > 0) cmbEstado.ItemIndex = 0;
            if ((cmbAgrupacion.Properties.DataSource as IList).Count > 0) cmbAgrupacion.ItemIndex = 0;
            txtNumeroFactura.Text = "";

            txtAdjunto.Text = "";
            txtAdjunto.Tag = "";

            poEntidadAdj.ArchivoAdjunto = "";
            poEntidadAdj.NombreOriginal = "";
            poEntidadAdj.RutaDestino = "";
            poEntidadAdj.RutaOrigen = "";
            pbCargado = true;
        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscarItemActivoFijo(int.Parse(txtNo.Text));
                if (poObject != null)
                {
                    pbCargado = false;
                    if ((cmbPersona.Properties.DataSource as IList).Count > 0) cmbPersona.ItemIndex = 0;
                    if ((cmbProveedor.Properties.DataSource as IList).Count > 0) cmbProveedor.ItemIndex = 0;
                    txtDescripcion.Text = poObject.Descripcion;
                    cmbTipoActivoFijo.EditValue = poObject.CodigoTipoActivoFijo;
                    cmbSucursal.EditValue = poObject.CodigoSucursal;
                    cmbCentroCosto.EditValue = poObject.CodigoCentroCosto;
                    dtpFechaCompra.DateTime = poObject.FechaCompra;
                    dtpFechaActivacion.DateTime = poObject.FechaActivacion;
                    txtCostoCompra.EditValue = poObject.CostoCompra;
                    txtValorResidual.EditValue = poObject.ValorResidual;
                    txtValorDepreciable.EditValue = poObject.ValorDepreciable;
                    txtDepreciacionAcumulada.EditValue = poObject.DepreciacionAcumulada;
                    txtCostoActual.EditValue = poObject.CostoActual;
                    txtMarca.EditValue = poObject.Marca;
                    txtModelo.EditValue = poObject.Modelo;
                    txtSerie.EditValue = poObject.Serie;
                    txtProductId.EditValue = poObject.ProductId;
                    lblFecha.Text = poObject.FechaRegistro.ToString("dd/MM/yyyy");
                    txtCodigo.Text = poObject.Codigo;
                    txtNumeroFactura.Text = poObject.NumFactura;
                    cmbEstado.EditValue = poObject.CodigoEstadoActivoFijo;
                    if (poObject.IdPersona != null)
                    {
                        cmbPersona.EditValue = poObject.IdPersona.ToString();
                    }
                    if (!string.IsNullOrEmpty(poObject.CodigoAgrupacion))
                    {
                        cmbAgrupacion.EditValue = poObject.CodigoAgrupacion.ToString();
                    }
                    if (poObject.IdProveedor != null)
                    {
                        cmbProveedor.EditValue = poObject.IdProveedor.ToString();
                    }
                    if (!string.IsNullOrEmpty(poObject.CodigoAgrupacion))
                    {
                        cmbAgrupacion.EditValue = poObject.CodigoAgrupacion.ToString();
                    }
                    
                    poEntidadAdj.ArchivoAdjunto = poObject.ArchivoAdjunto;
                    poEntidadAdj.NombreOriginal = poObject.NombreOriginal;
                    poEntidadAdj.RutaDestino = poObject.RutaDestino;

                    txtAdjunto.Text = poObject.NombreOriginal;
                    txtAdjunto.Tag = poObject.ArchivoAdjunto;
                    pbCargado = true;
                }
            }
        }

        private void lCalcular()
        {
            if (pbCargado)
            {
                if (string.IsNullOrEmpty(txtNo.Text))
                {
                    var poRegistro = loLogicaNegocio.goBuscarTipoActivoFijo(cmbTipoActivoFijo.EditValue.ToString());
                    if (poRegistro != null)
                    {
                        decimal pdcCostoCompra = decimal.Parse(txtCostoCompra.EditValue.ToString());
                        //decimal pdcValorResidual = Math.Round(pdcCostoCompra * (poRegistro.PorcentajeResidual / 100), 2);
                        decimal pdcValorResidual = poRegistro.ValorResidual;
                        decimal pdcValorDepreciable = pdcCostoCompra - pdcValorResidual;

                        txtValorResidual.EditValue = pdcValorResidual;
                        txtCostoActual.EditValue = pdcCostoCompra;
                        txtDepreciacionAcumulada.EditValue = 0;
                        txtValorDepreciable.EditValue = pdcValorDepreciable;

                    }
                }
                else
                {
                    if (loLogicaNegocio.giTieneDepreciaciones(int.Parse(txtNo.Text)) == 0)
                    {
                        if (loLogicaNegocio.gsUsuarioIngreso(int.Parse(txtNo.Text)) != "MIGRACION")
                        {
                            var poRegistro = loLogicaNegocio.goBuscarTipoActivoFijo(cmbTipoActivoFijo.EditValue.ToString());
                            if (poRegistro != null)
                            {
                                decimal pdcCostoCompra = decimal.Parse(txtCostoCompra.EditValue.ToString());
                                //decimal pdcValorResidual = Math.Round(pdcCostoCompra * (poRegistro.PorcentajeResidual / 100), 2);
                                decimal pdcValorResidual = poRegistro.ValorResidual;
                                decimal pdcValorDepreciable = pdcCostoCompra - pdcValorResidual;

                                txtValorResidual.EditValue = pdcValorResidual;
                                txtCostoActual.EditValue = pdcCostoCompra;
                                txtDepreciacionAcumulada.EditValue = 0;
                                txtValorDepreciable.EditValue = pdcValorDepreciable;

                            }
                        }

                    }
                }
            }
        }

        private void txtCostoCompra_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                lCalcular();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void frmPrItemActivoFijo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }   
        }

        private void cmbTipoActivoFijo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lCalcular();
                }
                
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
                bool pbEntro = false;
                var carp = ConfigurationManager.AppSettings["CarpetaOPCompras"].ToString();
                var dt2 = loLogicaNegocio.goConsultaDataTable(string.Format("SELECT NombreOriginal Archivo, Factura, FechaFactura, CONCAT('{1}',ArchivoAdjunto) VerAdjunto  FROM COMTORDENPAGO O INNER JOIN COMTORDENPAGOFACTURA OD (NOLOCK) ON O.IdOrdenPago = OD.IdOrdenPago WHERE O.CodigoEstado = 'S' AND OD.CodigoEstado = 'S' AND O.IdProveedor = {0}", cmbProveedor.EditValue.ToString(), carp));

                if (dt2.Rows.Count > 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("¿Desea visualizar las facturas del módulo Ordenes de Pagos?", "Buscar facturas en Orden de Pago", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        frmBusqueda frm = new frmBusqueda(dt2) { Text = "Facturas" };
                        frm.Width = 1200;
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            string Name = DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss_fff")+ "_" + frm.lsCodigoSeleccionado;
                            poEntidadAdj = new ItemActivoFijo();

                            poEntidadAdj.ArchivoAdjunto = Name;
                            poEntidadAdj.RutaOrigen = frm.lsCuartaColumna;
                            poEntidadAdj.NombreOriginal = frm.lsCodigoSeleccionado;
                            poEntidadAdj.RutaDestino = ConfigurationManager.AppSettings["CarpetaAfiIaf"].ToString() + Name;

                            txtNumeroFactura.Text = frm.lsSegundaColumna;
                            txtAdjunto.Text = frm.lsCodigoSeleccionado;
                            txtAdjunto.Tag = Name;

                            pbEntro = true;


                        }
                    }
                }

                if (!pbEntro)
                {
                    // Presenta un dialogo para seleccionar las imagenes
                    OpenFileDialog ofdArchivo = new OpenFileDialog();
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
                                poEntidadAdj = new ItemActivoFijo();

                                poEntidadAdj.ArchivoAdjunto = Name;
                                poEntidadAdj.RutaOrigen = ofdArchivo.FileName;
                                poEntidadAdj.NombreOriginal = file.Name;
                                poEntidadAdj.RutaDestino = ConfigurationManager.AppSettings["CarpetaAfiIaf"].ToString() + Name;


                                txtAdjunto.Text = file.Name;
                                txtAdjunto.Tag = Name;

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
                poEntidadAdj = new ItemActivoFijo();
                txtAdjunto.Text = "";
                txtAdjunto.Tag = "";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text))
                {
                    txtCodigo.Text = "";
                    txtNo.Text = "";
                    lblFecha.Text = "";
                    lCalcular();
                    XtraMessageBox.Show("Registro duplicado con exito", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("Cargue un registro por pantalla para duplicar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
