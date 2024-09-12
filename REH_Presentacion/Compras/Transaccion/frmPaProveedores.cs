using COM_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
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

namespace REH_Presentacion.Compras.Transaccion
{
    public partial class frmPaProveedores : frmBaseDev
    {
        clsNProveedor loLogicaNegocio = new clsNProveedor();
        BindingSource bsDatosCuentaBancaria = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDelFP = new RepositoryItemButtonEdit();
        bool pbCargado = false;

        public frmPaProveedores()
        {
            InitializeComponent();
            rpiBtnDelFP.ButtonClick += rpiBtnDelFP_ButtonClick;
        }

        private void frmPaProveedores_Load(object sender, EventArgs e)
        {
            lCargarEventosBotones();

            var poComboBanco = loLogicaNegocio.goConsultarComboBanco();
            //clsComun.gLLenarCombo(ref cmbBanco, poComboBanco, true);
            var poComboFormaPago = loLogicaNegocio.goConsultarComboFormaPago();
            //clsComun.gLLenarCombo(ref cmbFormaPago, poComboFormaPago, true);
            var poTipoCuentaBancaria = loLogicaNegocio.goConsultarComboTipoCuentaBancaria();
            //clsComun.gLLenarCombo(ref cmbTipoCuentaBancaria, poTipoCuentaBancaria, true);
            var poTipoIdentificacion = loLogicaNegocio.goConsultarComboTipoIdentificación();
            clsComun.gLLenarCombo(ref cmbTipoIdentificacion, poTipoIdentificacion, true);

            clsComun.gLLenarComboGrid(ref dgvCuentasBancarias, poTipoIdentificacion, "CodigoTipoIdentificacion", false);
            clsComun.gLLenarComboGrid(ref dgvCuentasBancarias, poComboBanco, "CodigoBanco", true);
            clsComun.gLLenarComboGrid(ref dgvCuentasBancarias, poComboFormaPago, "CodigoFormaPago", true);
            clsComun.gLLenarComboGrid(ref dgvCuentasBancarias, poTipoCuentaBancaria, "CodigoTipoCuentaBancaria", true);

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
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //var menu = Tag.ToString().Split(',');
            var poListaObject = loLogicaNegocio.goListarMaestro();
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[]
                                {
                                    new DataColumn("No."),
                                    new DataColumn("Identificación"),
                                    new DataColumn("Nombre"),
                                    new DataColumn("Estado")
                                });

            poListaObject.ForEach(a =>
            {
                DataRow row = dt.NewRow();
                row["No."] = a.Codigo;
                row["Identificación"] = a.Identificacion;
                row["Nombre"] = a.Descripcion;
                row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                dt.Rows.Add(row);
            });

            frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
            if (pofrmBuscar.ShowDialog() == DialogResult.OK)
            {
                lLimpiar();
                txtCodigo.Text = pofrmBuscar.lsCodigoSeleccionado;
                lConsultar();

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
                if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loLogicaNegocio.gEliminarMaestro(txtCodigo.Text.Trim(), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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

        /// <summary>
        /// Evento del botón Grabar, Guarda datos del formulario a el objeto de BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {

                dgvCuentasBancarias.PostEditor();
                PaProveedor poObject = new PaProveedor();

                if (txtCodigo != null)
                {
                    poObject.Codigo = txtCodigo.Text.ToString();
                }
                
                ////Validación de número de identificación
                //if (string.IsNullOrEmpty(txtIdentificacion.Text.Trim()))
                //{
                //    XtraMessageBox.Show("Número de Identificación no puede estar vacío.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                //try
                //{
                //    if (!clsComun.gVerificaIdentificacion(txtIdentificacion.Text.Trim()))
                //    {
                //        XtraMessageBox.Show("Número de Identificación no tiene un formato válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        return;
                //    }
                //}
                //catch (Exception)
                //{
                //    XtraMessageBox.Show("Número de Identificación inválido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                poObject.Descripcion = txtDescripcion.Text.Trim();
                poObject.Correo = txtCorreo.Text.Trim();
                poObject.Identificacion = txtIdentificacion.Text.Trim();
                poObject.Ciudad = txtCiudad.Text.Trim();
                poObject.Pais = txtPais.Text.Trim();
                poObject.Direccion = txtDireccion.Text.Trim();
                poObject.Usuario = clsPrincipal.gsUsuario;
                poObject.Terminal = string.Empty;
                poObject.Fecha = DateTime.Now;
                poObject.CodigoTipoIdentificacion = cmbTipoIdentificacion.EditValue.ToString();
                poObject.CardCode = lblCardCode.Text;

                poObject.ProveedorCuentaBancaria = (List<ProveedorCuentaBancaria>)bsDatosCuentaBancaria.DataSource;
                int tId = 0;    
                string psMsg = loLogicaNegocio.gsGuardar(poObject, out tId);
                if (string.IsNullOrEmpty(psMsg))
                {
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCodigo.Text = tId.ToString();
                    lConsultar();
                    //lLimpiar();
                }
                else
                {
                    XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void rpiBtnDelFP_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvCuentasBancarias.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<ProveedorCuentaBancaria>)bsDatosCuentaBancaria.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsDatosCuentaBancaria.DataSource = poLista;
                    dgvCuentasBancarias.RefreshData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }






        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();

            bsDatosCuentaBancaria.DataSource = new List<ProveedorCuentaBancaria>();
            gcCuentasBancarias.DataSource = bsDatosCuentaBancaria;

            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;


            dgvCuentasBancarias.OptionsBehavior.Editable = true;
            dgvCuentasBancarias.Columns["IdProveedorCuenta"].Visible = false;
            dgvCuentasBancarias.Columns["IdProveedor"].Visible = false;
            dgvCuentasBancarias.Columns["DesTipoIdentificacion"].Visible = false;
            dgvCuentasBancarias.Columns["DesBanco"].Visible = false;
            dgvCuentasBancarias.Columns["DesFormaPago"].Visible = false;
            dgvCuentasBancarias.Columns["DesTipoCuentaBancaria"].Visible = false;

            clsComun.gDibujarBotonGrid(rpiBtnDelFP, dgvCuentasBancarias.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);

            dgvCuentasBancarias.Columns["CodigoBanco"].Caption = "Banco";
            dgvCuentasBancarias.Columns["CodigoTipoCuentaBancaria"].Caption = "Tipo Cuenta";
            dgvCuentasBancarias.Columns["CodigoFormaPago"].Caption = "Forma Pago";
            dgvCuentasBancarias.Columns["CodigoTipoIdentificacion"].Caption = "Tipo Identificación";

            dgvCuentasBancarias.Columns["Principal"].Width = 50;

            if (!clsPrincipal.gbEditaProveedorFormaPago)
            {
                btnAñadirFilaFP.Enabled = false;
                dgvCuentasBancarias.Columns["Principal"].OptionsColumn.AllowEdit = false;
                dgvCuentasBancarias.Columns["Del"].OptionsColumn.AllowEdit = false;
                dgvCuentasBancarias.Columns["CodigoBanco"].OptionsColumn.AllowEdit = false;
                dgvCuentasBancarias.Columns["CodigoTipoCuentaBancaria"].OptionsColumn.AllowEdit = false;
                dgvCuentasBancarias.Columns["CodigoFormaPago"].OptionsColumn.AllowEdit = false;
                dgvCuentasBancarias.Columns["CodigoTipoIdentificacion"].OptionsColumn.AllowEdit = false;
                dgvCuentasBancarias.Columns["Identificacion"].OptionsColumn.AllowEdit = false;
                dgvCuentasBancarias.Columns["NumeroCuenta"].OptionsColumn.AllowEdit = false;
                dgvCuentasBancarias.Columns["Nombre"].OptionsColumn.AllowEdit = false;
            }

        }


        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscarMaestro(txtCodigo.Text.Trim());
                if (poObject != null)
                {
                    lCargarDatos(poObject);
                }
            }
        }

        private void lConsultarIdentificacion()
        {
            if (!string.IsNullOrEmpty(txtIdentificacion.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscarMaestroIdentifacion(txtIdentificacion.Text.Trim());
                if (poObject != null)
                {
                    lCargarDatos(poObject);
                }
            }
        }

        private void lCargarDatos(PaProveedor poObject)
        {
            txtCodigo.Text = poObject.Codigo;
            txtDescripcion.Text = poObject.Descripcion;
            cmbEstado.EditValue = poObject.CodigoEstado;
            txtDescripcion.EditValue = poObject.Descripcion;
            txtIdentificacion.EditValue = poObject.Identificacion;
            txtCorreo.EditValue = poObject.Correo;
            txtTerminalIngreso.EditValue = poObject.Terminal;
            txtTerminalModificacion.EditValue = poObject.TerminalMod;
            txtFechaHoraIngreso.Text = poObject.Fecha.ToString();
            txtFechaHoraModificacion.Text = poObject.FechaMod.ToString();
            txtUsuarioIngreso.Text = poObject.Usuario;
            txtUsuarioModificacion.Text = poObject.UsuarioMod;
            txtPais.Text = poObject.Pais;
            txtCiudad.Text = poObject.Ciudad;
            txtDireccion.Text = poObject.Direccion;
            lblCardCode.Text = poObject.CardCode;
            if (!string.IsNullOrEmpty(poObject.CodigoTipoIdentificacion))
            {
                cmbTipoIdentificacion.EditValue = poObject.CodigoTipoIdentificacion;
            }
            else
            {
                cmbTipoIdentificacion.EditValue = Diccionario.Seleccione;
            }
            
            txtIdentificacion.Enabled = false;

            bsDatosCuentaBancaria.DataSource = poObject.ProveedorCuentaBancaria;
            gcCuentasBancarias.DataSource = bsDatosCuentaBancaria;
            if (poObject.ProveedorCuentaBancaria.Count > 0)
            {
                clsComun.gOrdenarColumnasGridFullEditable(dgvCuentasBancarias);
            }
        }

        private void lLimpiar()
        {
            txtCodigo.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtIdentificacion.Text = string.Empty;
            txtIdentificacion.Enabled = true;
            cmbEstado.ItemIndex = 0;
            txtFechaHoraIngreso.Text = String.Empty;
            txtFechaHoraModificacion.Text = String.Empty;
            txtTerminalIngreso.Text = String.Empty;
            txtTerminalModificacion.Text = String.Empty;
            txtUsuarioIngreso.Text = String.Empty;
            txtUsuarioModificacion.Text = String.Empty;
            txtPais.Text = string.Empty;
            txtCiudad.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            lblCardCode.Text = string.Empty;
            if ((cmbEstado.Properties.DataSource as IList).Count > 0) cmbEstado.ItemIndex = 0;
            if ((cmbTipoIdentificacion.Properties.DataSource as IList).Count > 0) cmbTipoIdentificacion.ItemIndex = 0;

            bsDatosCuentaBancaria.DataSource = new List<ProveedorCuentaBancaria>();
            gcCuentasBancarias.DataSource = bsDatosCuentaBancaria;


        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            try
            {
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtIdentificacion_Leave(object sender, EventArgs e)
        {
            try
            {
                lConsultarIdentificacion();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmPaProveedores_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            /*
            var dt = loLogicaNegocio.goConsultaDataTable(
                "SELECT " +
                    "SN.LicTradNum Identificación, " +
                    "SN.CardName Nombre, " +
                    "SN.E_Mail Correo " +
                "FROM " +
                    "SBO_AFECOR.dbo.OCRD SN(NOLOCK) " +
                "WHERE " +
                    "SN.CardType = 'S' AND SN.validFor = 'Y' " +
                "ORDER BY " +
                    "SN.CardName "
                );

            frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Proveedores de SAP" };
            if (pofrmBuscar.ShowDialog() == DialogResult.OK)
            {
                lLimpiar();
                txtIdentificacion.Text = pofrmBuscar.lsCodigoSeleccionado;
                var psSegundaColumna = pofrmBuscar.lsSegundaColumna;
                lConsultarIdentificacion();

                dt = loLogicaNegocio.goConsultaDataTable(
                    "SELECT " +
                        "SN.LicTradNum Identificación, " +
                        "SN.CardName Nombre, " +
                        "SN.E_Mail Correo, " +
                        "SN.CardCode, " +
                        "CASE WHEN U_TIPO_RUC IN ('N') THEN 'CED' WHEN U_TIPO_RUC IN ('EX') THEN 'PAS' WHEN U_TIPO_RUC IN ('NR','P','J') THEN 'RUC' ELSE '0' END Codigo " +
                    "FROM " +
                        "SBO_AFECOR.dbo.OCRD SN(NOLOCK) " +
                    "WHERE " +
                        "SN.CardType = 'S' AND SN.validFor = 'Y' " +
                        "AND SN.CardCode LIKE '%" + txtIdentificacion.Text + "' " +
                    "ORDER BY " +
                        "SN.CardName "
                    );

                txtDescripcion.Text = dt.Rows[0][1].ToString();
                txtCorreo.Text = dt.Rows[0][2].ToString();
                lblCardCode.Text = dt.Rows[0][3].ToString();
                cmbTipoIdentificacion.EditValue = dt.Rows[0][4].ToString();
                btnGrabar_Click(null, null);
                
            }
            */

            loLogicaNegocio.gMigrarProveedoresSapSaaf();
            XtraMessageBox.Show("Proceso finalizado con éxito", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lLimpiar();
        }

        private void btnAñadirFilaFP_Click(object sender, EventArgs e)
        {
            try
            {
                bsDatosCuentaBancaria.AddNew();
                ((List<ProveedorCuentaBancaria>)bsDatosCuentaBancaria.DataSource).LastOrDefault().CodigoTipoIdentificacion = Diccionario.Seleccione;
                ((List<ProveedorCuentaBancaria>)bsDatosCuentaBancaria.DataSource).LastOrDefault().CodigoBanco = Diccionario.Seleccione;
                ((List<ProveedorCuentaBancaria>)bsDatosCuentaBancaria.DataSource).LastOrDefault().CodigoFormaPago = Diccionario.Seleccione;
                ((List<ProveedorCuentaBancaria>)bsDatosCuentaBancaria.DataSource).LastOrDefault().CodigoTipoCuentaBancaria = Diccionario.Seleccione;
                dgvCuentasBancarias.Focus();
                dgvCuentasBancarias.ShowEditor();
                dgvCuentasBancarias.UpdateCurrentRow();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCuentasBancarias_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Principal")
                {
                    dgvCuentasBancarias.PostEditor();
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvCuentasBancarias.GetFocusedDataSourceRowIndex();
                    var poLista = (List<ProveedorCuentaBancaria>)bsDatosCuentaBancaria.DataSource;

                    int i = 0;
                    foreach (var item in poLista)
                    {
                        if (i != piIndex)
                        {
                            if (item.Principal)
                            {
                                item.Principal = false;
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
    }
}
