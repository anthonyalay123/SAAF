
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraEditors;
using COM_Negocio;

namespace REH_Presentacion.Compras.Parametrizadores
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 11/05/2022
    /// Formulario de parámetros generales de compras
    /// </summary>
    public partial class frmPaParametro : frmBaseTrxDev
    {
        #region Variables
        clsNParametroCompras loLogicaNegocio;
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmPaParametro()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNParametroCompras();
        }

        private void frmParametro_Load(object sender, EventArgs e)
        {
            try
            {
                //clsComun.gLLenarCombo(ref cmbTipoPersona, loLogicaNegocio.goConsultarComboTipoPersona(), true);
                lCargarEventosBotones();
                lBuscar();
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
                lBuscar();
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
                
                if(lbEsValido())
                {
                    ParametroCompras poObject = new ParametroCompras();
                    poObject.Codigo = txtCodigo.Text.Trim();
                    poObject.CodigoEmpresaMultiCash = txtCodigoEmpresaMultiChash.Text.Trim();
                    poObject.CodigoInstitucionFinancieraMultiCash = txtCodigoIntitucionFinancieraMultiCash.Text.Trim();
                    poObject.CuentaBancariaEmpresa = txtCuentaBancariaEmpresa.Text.Trim();
                    poObject.Semana = txtSemana.Text.Trim();
                    poObject.FechaConsultaGuiasDesde = dtpFechaConsultaGuiaDesde.DateTime;
                    poObject.PermiteSeleccionarDiferentesBodegasEnGuias = chbPermitirSeleccionarDiferentesBodegasEnGuias.Checked;
                   
                    if (loLogicaNegocio.gbGuardar(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lBuscar();
                    }
                    else
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
                lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private new void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Char.IsDigit(e.KeyChar)|| Char.IsControl(e.KeyChar))
                {
                    e.Handled = false;
                }
               
                else
                 if (Char.IsSeparator(e.KeyChar))
                {
                    e.Handled = false;
                }
                else if (e.KeyChar =='(' || e.KeyChar ==')')
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private new void SoloNumerosSimbolo(object sender, KeyPressEventArgs e)
        {
            try
            {
                clsComun.gSoloNumerosSimbolo(sender, e);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private new void SoloLetras(object sender, KeyPressEventArgs e)
        {
            try
            {
                clsComun.gSoloLetras(sender, e);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;


            txtCodigoIntitucionFinancieraMultiCash.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtCuentaBancariaEmpresa.KeyPress += new KeyPressEventHandler(SoloNumeros);
        }

        private bool lbEsValido()
        {
            
            if (string.IsNullOrEmpty(txtCodigoEmpresaMultiChash.Text.Trim()))
            {
                XtraMessageBox.Show("Ingrese Código Empresa MultiCash", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtCodigoIntitucionFinancieraMultiCash.Text.Trim()))
            {
                XtraMessageBox.Show("Ingrese Código Institución Financiera MultiCash", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtCuentaBancariaEmpresa.Text.Trim()))
            {
                XtraMessageBox.Show("Ingrese Cuenta Bancaria", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
           

            return true;
        }

        private void lLimpiar()
        {
            txtCodigo.Text = string.Empty;
        }

        private void lBuscar()
        {
            var poObject = loLogicaNegocio.goBuscarEntidad();
            if (poObject != null)
            {
                txtCodigo.Text = poObject.Codigo;
                txtCodigoEmpresaMultiChash.Text = poObject.CodigoEmpresaMultiCash;
                txtCodigoIntitucionFinancieraMultiCash.Text = poObject.CodigoInstitucionFinancieraMultiCash;
                txtCuentaBancariaEmpresa.Text = poObject.CuentaBancariaEmpresa;
                txtSemana.Text = poObject.Semana;
                chbPermitirSeleccionarDiferentesBodegasEnGuias.Checked = poObject.PermiteSeleccionarDiferentesBodegasEnGuias;
                dtpFechaConsultaGuiaDesde.EditValue = poObject.FechaConsultaGuiasDesde;


                //tbcPrincipal.SelectedTabPageIndex = 1;
                //tbpHorasExtras.Focus();

                //tbcPrincipal.SelectedTabPageIndex = 0;

            }

            else
            {
                lLimpiar();
            }
           
        }

        #endregion
    }
}
