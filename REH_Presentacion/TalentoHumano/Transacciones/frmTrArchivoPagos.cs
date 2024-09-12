using DevExpress.XtraEditors;
using GEN_Entidad;
using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.TalentoHumano.Transacciones
{
    public partial class frmTrArchivoPagos : frmBaseTrxDev
    {

        clsNNomina loLogicaNegocio;
        bool pbCargado;

        public frmTrArchivoPagos()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNNomina();
        }

        private void frmTrArchivoPagos_Load(object sender, EventArgs e)
        {
            try
            {
                pbCargado = false;
                clsComun.gLLenarCombo(ref cmbPersona, loLogicaNegocio.goConsultarComboIdPersona(), true);
                clsComun.gLLenarCombo(ref cmbBanco, loLogicaNegocio.goConsultarComboBanco(), true);
                clsComun.gLLenarCombo(ref cmbFormaPago, loLogicaNegocio.goConsultarComboFormaPago(), true);
                clsComun.gLLenarCombo(ref cmbTipoCuenta, loLogicaNegocio.goConsultarComboTipoCuentaBancaria(), true);
                lCargarEventosBotones();
                pbCargado = true;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Click += btnGenerar_Click;

            txtValor.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtNumeroCuenta.KeyPress += new KeyPressEventHandler(SoloNumeros);

            txtValor.Properties.Mask.EditMask = "c";
            txtValor.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtValor.Properties.Mask.UseMaskAsDisplayFormat = true;
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
                var dt = loLogicaNegocio.gdtConsultaPersonaCuentaBancaria();

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    string psCodigo = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar(int.Parse(psCodigo));
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lConsultar(int tIdEmpleadoContrato)
        {
            if (tIdEmpleadoContrato != 0)
            {
                var poEmpleadoContrato = new clsNEmpleado().goConsultarContratos(tIdEmpleadoContrato);
                if (poEmpleadoContrato != null)
                {
                    cmbPersona.EditValue = poEmpleadoContrato.IdPersona.ToString();
                    cmbBanco.EditValue = poEmpleadoContrato.CodigoBanco;
                    cmbFormaPago.EditValue = poEmpleadoContrato.CodigoFormaPago;
                    cmbTipoCuenta.EditValue = poEmpleadoContrato.CodigoTipoCuentaBancaria;
                    txtNumeroCuenta.Text = poEmpleadoContrato.NumeroCuenta;
                    txtValor.EditValue = 0.00M;
                }
            }
        }

        /// <summary>
        /// Evento del botón Generar, Genera archivo de pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {

                if (lbEsValido())
                {
                    List<SpGeneraPago> poLista = new List<SpGeneraPago>();

                    var poPersona = loLogicaNegocio.goConsultarPersona(int.Parse(cmbPersona.EditValue.ToString()));

                    poLista.Add(new SpGeneraPago
                    {
                        CodigoBanco = cmbBanco.EditValue.ToString(),
                        NumeroIdentificacion = poPersona.NumeroIdentificacion,
                        CodigoTipoIdentificacion = poPersona.CodigoTipoIdentificacion,
                        Banco = cmbBanco.Text,
                        CodigoFormaPago = cmbFormaPago.EditValue.ToString(),
                        FormaPago = cmbFormaPago.Text,
                        CodigoTipoCuenta = cmbTipoCuenta.EditValue.ToString(),
                        TipoCuenta = cmbTipoCuenta.Text,
                        IdPersona = poPersona.IdPersona,
                        NombreCompleto = clsComun.gRemoverCaracteresEspeciales(cmbPersona.Text),
                        NumeroCuenta = txtNumeroCuenta.Text,
                        Valor = decimal.Parse(txtValor.EditValue.ToString()),
                        ValorEntero = Convert.ToInt32((decimal.Parse(txtValor.EditValue.ToString()) * 100)),
                        Seleccionado = true
                    });
                    string psPath = ConfigurationManager.AppSettings["CarpetaArchivoBanco"].ToString();
                    string psMensajeTotal = string.Empty;
                    string psMensaje;
                    new clsNGeneraPago().gbGenerar(0, poLista, psPath, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, out psMensaje);
                    psMensajeTotal = psMensajeTotal + psMensaje + "\n";
                    XtraMessageBox.Show(psMensajeTotal, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }


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

        private void lLimpiar(bool tbTodo = true)
        {
            pbCargado = false;
            if (tbTodo)
            {
                if ((cmbPersona.Properties.DataSource as IList).Count > 0) cmbPersona.ItemIndex = 0;
            }

            if ((cmbBanco.Properties.DataSource as IList).Count > 0) cmbBanco.ItemIndex = 0;
            if ((cmbFormaPago.Properties.DataSource as IList).Count > 0) cmbFormaPago.ItemIndex = 0;
            if ((cmbTipoCuenta.Properties.DataSource as IList).Count > 0) cmbTipoCuenta.ItemIndex = 0;
            txtNumeroCuenta.Text = "";
            txtValor.EditValue = 0.00M;
            pbCargado = true;
        }

        private bool lbEsValido()
        {
            if (cmbPersona.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Persona.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (cmbBanco.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Banco.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (cmbFormaPago.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Forma de Pago.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (cmbTipoCuenta.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Tipo de Cuenta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtNumeroCuenta.Text.Trim()))
            {
                XtraMessageBox.Show("Ingrese el Número de Cuenta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtValor.Text.Trim()))
            {
                XtraMessageBox.Show("Ingrese el Valor.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                if (Convert.ToDecimal(txtValor.EditValue) == 0.00M)
                {
                    XtraMessageBox.Show("Ingrese valor mayor a '0.00'.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        private void cmbPersona_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lLimpiar(false);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
