using DevExpress.XtraEditors;
using GEN_Entidad.Entidades.Compras;
using GEN_Entidad;
using REH_Presentacion.Formularios;
using reporte;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using COM_Negocio;
using System.Collections;
using REH_Presentacion.Comun;

namespace REH_Presentacion.Compras.Transaccion
{
    public partial class frmTrSolicitudAnticipo : frmBaseTrxDev
    {
        #region Variables
        private bool pbCargado = false;
        private clsNAnticipo loLogicaNegocio = new clsNAnticipo();
        public int lid = 0;
        #endregion

        #region Eventos
        public frmTrSolicitudAnticipo()
        {
            InitializeComponent();
        }
        private void frmTrSolicitudAnticipo_Load(object sender, EventArgs e)
        {
            lCargarEventosBotones();

            clsComun.gLLenarCombo(ref cmbTipoSolicitudAnticipo, loLogicaNegocio.goConsultarComboTipoSolicitudAnticipo(), true);
            clsComun.gLLenarCombo(ref cmbProveedor, loLogicaNegocio.goConsultarComboProveedorId(), true);
            clsComun.gLLenarCombo(ref cmbSucursal, loLogicaNegocio.goConsultarComboSucursal(), true);
            clsComun.gLLenarCombo(ref cmbDepartamento, loLogicaNegocio.goConsultarComboDepartamento(), true);
            clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstado(), true);
            lLimpiar();

            if (lid != 0)
            {
                txtNo.Text = lid.ToString();
                lConsultar();
            }

            pbCargado = true;

        }
        private void frmTrSolicitudAnticipo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }
        private void btnTrazabilidad_Click(object sender, EventArgs e)
        {
            int tId = string.IsNullOrEmpty(txtNo.Text) ? 0 : int.Parse(txtNo.Text);
            var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.SolicitudAnticipo, tId);

            frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Trazabilidad" };
            pofrmBuscar.ShowDialog();
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
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var poListaObject = loLogicaNegocio.goListarSolicitudAnticipo(clsPrincipal.gsUsuario, int.Parse(Tag.ToString().Split(',')[0]));
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("No"),
                                    new DataColumn("Usuario"),
                                    new DataColumn("Fecha", typeof(DateTime)),
                                    new DataColumn("Proveedor"),
                                    new DataColumn("Estado"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["No"] = a.IdSolicitudAnticipo;
                    row["Usuario"] = a.Usuario;
                    row["Fecha"] = a.FechaAnticipo;
                    row["Proveedor"] = a.Proveedor;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                pofrmBuscar.Width = 1200;
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
        private void btnGrabar_Click(object sender, EventArgs e)
        {

            try
            {
                var msg = lsEsValido();
                if (string.IsNullOrEmpty(msg))
                {

                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        SolicitudAnticipo poObject = new SolicitudAnticipo();
                        if (!string.IsNullOrEmpty(txtNo.Text))
                        {
                            poObject.IdSolicitudAnticipo = int.Parse(txtNo.Text);
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
                            poObject.CodigoEstado = Diccionario.Pendiente;
                        }
                        else
                        {
                            poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                        }

                        poObject.CodigoTipoAnticipo = cmbTipoSolicitudAnticipo.EditValue.ToString();
                        poObject.CodigoSucursal = cmbSucursal.EditValue.ToString();
                        poObject.Sucursal = cmbSucursal.Text;
                        poObject.IdProveedor = int.Parse(cmbProveedor.EditValue.ToString());
                        poObject.Proveedor = cmbProveedor.Text;
                        poObject.CodigoDepartamento = cmbDepartamento.EditValue.ToString();
                        poObject.Departamento = cmbDepartamento.Text;
                        poObject.Observacion = txtObservaciones.Text;
                        poObject.Valor = Convert.ToDecimal(txtValor.EditValue.ToString());
                        poObject.FechaAnticipo = dtpFechaAnticipo.DateTime;

                        string psMsg = loLogicaNegocio.gsGuardarSolicitudAnticipo(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, int.Parse(Tag.ToString().Split(',')[0]), psComentarioCorregir);
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
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.ToString()))
                {
                    //var poProveedor = (List<GuiaRemisionDetalle>)bsDatos.DataSource;
                    //var piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                    //int lIdOrdenCompra = int.Parse(txtNo.Text);
                    //int lIdProveedor = int.Parse(cmbCliente.EditValue.ToString());
                    //clsComun.gImprimirOrdenPago(lIdOrdenCompra);
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
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var psMsg = loLogicaNegocio.gsEliminarSolicitudAnticipo(Convert.ToInt16(txtNo.Text.Trim()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
        private void btnPrimero_Click(object sender, EventArgs e)
        {
            try
            {
                txtNo.Text = loLogicaNegocio.goBuscarCodigoSolicitudAnticipo(Diccionario.BuscarCodigo.Tipo.Primero, txtNo.Text.Trim()).ToString();
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigoSolicitudAnticipo(Diccionario.BuscarCodigo.Tipo.Anterior, txtNo.Text.Trim()).ToString();
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigoSolicitudAnticipo(Diccionario.BuscarCodigo.Tipo.Siguiente, txtNo.Text.Trim()).ToString();
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
                txtNo.Text = loLogicaNegocio.goBuscarCodigoSolicitudAnticipo(Diccionario.BuscarCodigo.Tipo.Ultimo, txtNo.Text.Trim()).ToString();
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Métodos
        private string lsEsValido()
        {
            string psMsg = "";

            if (cmbTipoSolicitudAnticipo.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Tipo de Solicitud.\n", psMsg);
            }
            if (cmbProveedor.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Proveedor.\n", psMsg);
            }
            if (cmbDepartamento.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Departamento.\n", psMsg);
            }
            if (cmbSucursal.EditValue.ToString() == Diccionario.Seleccione)
            {
                psMsg = string.Format("{0}Seleccione Sucursal.\n", psMsg);
            }
            if (Convert.ToDecimal(txtValor.EditValue.ToString()) == 0)
            {
                psMsg = string.Format("{0}Ingrese el valor.\n", psMsg);
            }

            return psMsg;
        }
        private void lLimpiar()
        {
            pbCargado = false;
            txtNo.Text = string.Empty;
            txtObservaciones.Text = string.Empty;
            txtValor.EditValue = 0;


            if ((cmbDepartamento.Properties.DataSource as IList).Count > 0) cmbDepartamento.ItemIndex = 0;
            if ((cmbEstado.Properties.DataSource as IList).Count > 0) cmbEstado.ItemIndex = 0;
            if ((cmbProveedor.Properties.DataSource as IList).Count > 0) cmbProveedor.ItemIndex = 0;
            if ((cmbSucursal.Properties.DataSource as IList).Count > 0) cmbSucursal.ItemIndex = 0;
            if ((cmbTipoSolicitudAnticipo.Properties.DataSource as IList).Count > 0) cmbTipoSolicitudAnticipo.ItemIndex = 0;

            cmbEstado.EditValue = Diccionario.Pendiente;
            dtpFechaAnticipo.DateTime = DateTime.Now;

            pbCargado = true;


        }
        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goConsultarSolicitudAnticipo(Convert.ToInt32(txtNo.Text.Trim()));
                if (poObject != null)
                {
                    txtNo.EditValue = poObject.IdSolicitudAnticipo;
                    txtObservaciones.EditValue = poObject.Observacion;
                    txtValor.EditValue = poObject.Valor;
                    dtpFechaAnticipo.DateTime = poObject.FechaAnticipo;

                    cmbDepartamento.EditValue = poObject.CodigoDepartamento;
                    cmbEstado.EditValue = poObject.CodigoEstado;
                    cmbProveedor.EditValue = poObject.IdProveedor;
                    cmbSucursal.EditValue = poObject.CodigoSucursal;
                    cmbTipoSolicitudAnticipo.EditValue = poObject.CodigoTipoAnticipo;
                }
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
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;
        }
        #endregion

    }
}
