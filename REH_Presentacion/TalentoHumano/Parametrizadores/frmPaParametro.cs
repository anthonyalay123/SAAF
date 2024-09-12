using REH_Negocio;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraEditors;

namespace REH_Presentacion.Parametrizadores
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 14/02/2020
    /// Formulario de parámetros generales
    /// </summary>
    public partial class frmPaParametro : frmBaseTrxDev
    {
        #region Variables
        clsNParametro loLogicaNegocio;
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmPaParametro()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNParametro();
        }

        private void frmParametro_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbTipoPersona, loLogicaNegocio.goConsultarComboTipoPersona(), true);
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
                    Parametro poObject = new Parametro();
                    poObject.Codigo = txtCodigo.Text.Trim();
                    poObject.CodigoTipoPersona = cmbTipoPersona.EditValue.ToString();
                    poObject.Ruc = txtRuc.Text.Trim();
                    poObject.Nombre = txtNombre.Text.Trim();
                    poObject.RepresentanteLegal = txtRepresentanteLegal.Text.Trim();
                    poObject.Direccion = txtDireccion.Text.Trim();
                    poObject.Telefono = txtTelefono.Text.Trim();
                    poObject.Fax = txtFax.Text.Trim();
                    poObject.Movil = txtMovil.Text.Trim();
                    poObject.Correo = txtCorreo.Text.Trim();
                    poObject.SitioWeb = txtSitioWeb.Text.Trim();
                    poObject.CodigoSri = txtCodigoSri.Text.Trim();
                    poObject.CodigoIess = txtCodigoIess.Text.Trim();
                    poObject.SueldoBasico = Convert.ToDecimal(txtSueldoBasico.EditValue.ToString().Trim());
                    poObject.SueldoBasicoAnterior = Convert.ToDecimal(txtSueldoBasicoAnterior.EditValue.ToString().Trim());
                    poObject.PorcAportePatronal = Convert.ToDecimal(txtPorcAportePatronal.EditValue.ToString().Trim());
                    poObject.PorcAportePersonal = Convert.ToDecimal(txtPorcAportePersonal.EditValue.ToString().Trim());
                    poObject.PeriodoNomina = int.Parse(txtPeriodoNomina.Text.Trim());
                    poObject.AnioInicioNomina = int.Parse(txtAnioInicioNomina.Text.Trim());
                    poObject.MesInicioNomina = int.Parse(txtMesInicioNomina.Text.Trim());
                    poObject.MinGraciaPostSalida = int.Parse(txtMinGraciaPostSalida.Text.Trim());
                    poObject.TieneConfiguradoCorreo = chkEnvioCorreo.Checked;
                    poObject.CorreoUsuario = txtCorreoUsuario.Text.Trim();
                    poObject.Contrasena = txtContrasenaUsuario.Text.Trim();
                    poObject.ServidorSMTP = txtServidorSMTP.Text.Trim();
                    poObject.Ciudad = txtCiudad.Text.Trim();
                    poObject.CantidadDiasApron = int.Parse(txtCantidadDiasAprobacion.Text.Trim());
                    poObject.Pais = txtPais.Text.Trim();
                    if (!string.IsNullOrEmpty(txtPuerto.Text.Trim())) poObject.PuertoSMTP = int.Parse(txtPuerto.Text.Trim());
                    poObject.UsaSSL = chkUsaSSL.Checked;
                    poObject.AutentificarSMTP = chkAutenticarSMTP.Checked;

                    poObject.Conexion = txtConexion.Text.Trim();
                    poObject.PorcRecargoJornadaNocturna = Convert.ToDecimal(txtPorcentajeRecargoNocturno.EditValue.ToString().Trim());
                    poObject.PorcHoraExtraSuplementaria50 = Convert.ToDecimal(txtPorcentajeHoraSuplementaria.EditValue.ToString().Trim());
                    poObject.PorcHoraExtraSuplementaria100 = Convert.ToDecimal(txtPorcentajeHoraExtraordinaria.EditValue.ToString().Trim());
                    poObject.PorcHoraExtraFds_Feriado = Convert.ToDecimal(txtPorcentajeFdsFeriado.EditValue.ToString().Trim());
                    poObject.HorasLaborablesDia = int.Parse(txtHorasLaborablesDia.Text.Trim());
                    poObject.DiasLaborablesMes = int.Parse(txtDiasLaborablesMes.Text.Trim());
                    poObject.DiaCorteHorasExtras = int.Parse(txtDiaCorteHoraExtra.Text.Trim());
                    poObject.DiaInicioCorteHorasExtras = int.Parse(txtDiaInicioCorteHoraExtra.Text.Trim());
                    poObject.IntervaloAjusteProvision = Convert.ToDecimal(txtIntervaloAjusteProvision.Text.Trim());

                    poObject.HoraInicioJornadaDiurna = dtpInicioJornadaDiurna.Value.TimeOfDay;
                    poObject.HoraFinJornadaDiurna = dtpFinJornadaDiurna.Value.TimeOfDay;
                    poObject.HoraGeneralEntrada = dtpHoraGeneralEntrada.Value.TimeOfDay;
                    poObject.HoraGeneralSalida = dtpHoraGeneralSalida.Value.TimeOfDay;
                    poObject.TiempoAlmuerzo = dtpTiempoAlmuerzo.Value.TimeOfDay;
                    poObject.HoraInicioGraciaPost = dtpHoraInicioTiempoGraciaPost.Value.TimeOfDay;
                    poObject.HoraFinGraciaPost = dtpHoraFinTiempoGraciaPost.Value.TimeOfDay;
                    poObject.MontoMaxDeduccionGP = string.IsNullOrEmpty(txtMontoMaxDeduccionGP.Text) ? 0M : Convert.ToDecimal(txtMontoMaxDeduccionGP.EditValue);
                    poObject.EdadTerceraEdad = string.IsNullOrEmpty(txtEdadTerceraEdad.Text) ? 99 : int.Parse(txtEdadTerceraEdad.EditValue.ToString());

                    poObject.CerrarSistema = chbCerrarSistema.Checked;
                    poObject.HoraCierreSistema = dtpHoraCierreSistema.Value.TimeOfDay;
                    poObject.MensajePrevioCierreSistema = txtTextoCierreSistema.Text;

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

        

        private void chkEnvioCorreo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkEnvioCorreo.Checked)
                    grbEnvioCorreo.Enabled = true;
                else
                    grbEnvioCorreo.Enabled = false;
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


            txtDiasLaborablesMes.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtHorasLaborablesDia.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtDiaCorteHoraExtra.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtDiaInicioCorteHoraExtra.KeyPress += new KeyPressEventHandler(SoloNumeros);
        }

        private bool lbEsValido()
        {
            if (cmbTipoPersona.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Tipo Persona.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtRuc.Text.Trim()))
            {
                XtraMessageBox.Show("Ingrese Ruc.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else
            {
                if (!clsComun.gVerificaIdentificacion(txtRuc.Text.Trim()))
                {
                    XtraMessageBox.Show("Ruc ingresado no tiene formato valido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            
            if (string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                XtraMessageBox.Show("Ingrese Nombre.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtDireccion.Text.Trim()))
            {
                XtraMessageBox.Show("Ingrese Dirección.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtSueldoBasico.EditValue.ToString().Trim()))
            {
                XtraMessageBox.Show("Ingrese Sueldo Básico.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtSueldoBasicoAnterior.EditValue.ToString().Trim()))
            {
                XtraMessageBox.Show("Ingrese Sueldo Básico Anterior.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtPorcAportePatronal.EditValue.ToString().Trim()))
            {
                XtraMessageBox.Show("Ingrese porcentaje aporte patronal.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            
            if (string.IsNullOrEmpty(txtPorcAportePersonal.EditValue.ToString().Trim()))
            {
                XtraMessageBox.Show("Ingrese porcentaje aporte personal.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(txtPeriodoNomina.Text.Trim()))
            {
                XtraMessageBox.Show("Ingrese Periodo Nómina.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!string.IsNullOrEmpty(txtCorreo.Text.Trim()))
            {
                if(!clsComun.gValidaFormatoCorreo(txtCorreo.Text.Trim()))
                {
                    XtraMessageBox.Show("Correo ingresado no tiene formato valido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (chkEnvioCorreo.Checked)
            {
                if (string.IsNullOrEmpty(txtCorreoUsuario.Text.Trim()))
                {
                    XtraMessageBox.Show("Ingrese usuario de correo electrónico.", "Envío de Correo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                else
                {
                    if (!clsComun.gValidaFormatoCorreo(txtCorreoUsuario.Text.Trim()))
                    {
                        XtraMessageBox.Show("Usuario de correo ingresado no tiene formato valido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }

                if (string.IsNullOrEmpty(txtContrasenaUsuario.Text.Trim()))
                {
                    XtraMessageBox.Show("Ingrese contraseña.", "Envío de Correo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (string.IsNullOrEmpty(txtConfContrasenaUsuario.Text.Trim()))
                {
                    XtraMessageBox.Show("Ingrese confirmación de contraseña.", "Envío de Correo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (txtConfContrasenaUsuario.Text.Trim() != txtContrasenaUsuario.Text.Trim())
                {
                    XtraMessageBox.Show("La contraseña ingresada no coincide con la confirmación de contraseña.", "Envío de Correo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (string.IsNullOrEmpty(txtServidorSMTP.Text.Trim()))
                {
                    XtraMessageBox.Show("Ingrese Servidor SMTP.", "Envío de Correo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (string.IsNullOrEmpty(txtPuerto.Text.Trim()))
                {
                    XtraMessageBox.Show("Ingrese Puerto.", "Envío de Correo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        private void lLimpiar()
        {
            txtCodigo.Text = string.Empty;
            txtRuc.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtRepresentanteLegal.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtFax.Text = string.Empty;
            txtMovil.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtSitioWeb.Text = string.Empty;
            txtCodigoSri.Text = string.Empty;
            txtCodigoIess.Text = string.Empty;
            txtSueldoBasico.Text = string.Empty;
            txtSueldoBasicoAnterior.Text = string.Empty;
            txtPorcAportePatronal.Text = string.Empty;
            txtPorcAportePersonal.Text = string.Empty;
            txtPeriodoNomina.Text = string.Empty;
            chkEnvioCorreo.Checked = false;
            txtCorreoUsuario.Text = string.Empty;
            txtContrasenaUsuario.Text = string.Empty;
            txtConfContrasenaUsuario.Text = string.Empty;
            txtServidorSMTP.Text = string.Empty;
            txtPuerto.Text = string.Empty;
            chkUsaSSL.Checked = false;
            chkAutenticarSMTP.Checked = false;
            txtAnioInicioNomina.Text = string.Empty;
            txtMesInicioNomina.Text = string.Empty;
            txtMinGraciaPostSalida.Text = string.Empty;
            if ((cmbTipoPersona.Properties.DataSource as IList).Count > 0) cmbTipoPersona.ItemIndex = 0;
            txtTextoCierreSistema.Text = string.Empty;
            chbCerrarSistema.Checked = false;
        }

        private void lBuscar()
        {
            var poObject = loLogicaNegocio.goBuscarEntidad();
            if (poObject != null)
            {
                txtCodigo.Text = poObject.Codigo;
                cmbTipoPersona.EditValue = poObject.CodigoTipoPersona;
                txtRuc.Text = poObject.Ruc;
                txtNombre.Text = poObject.Nombre;
                txtRepresentanteLegal.Text = poObject.RepresentanteLegal;
                txtDireccion.Text = poObject.Direccion;
                txtTelefono.Text = poObject.Telefono;
                txtFax.Text = poObject.Fax;
                txtMovil.Text = poObject.Movil;
                txtCorreo.Text = poObject.Correo;
                txtSitioWeb.Text = poObject.SitioWeb;
                txtCodigoSri.Text = poObject.CodigoSri;
                txtCodigoIess.Text = poObject.CodigoIess;
                txtSueldoBasico.Text = poObject.SueldoBasico.ToString();
                txtSueldoBasicoAnterior.Text = poObject.SueldoBasicoAnterior.ToString();
                txtPorcAportePatronal.Text = poObject.PorcAportePatronal.ToString();
                txtPorcAportePersonal.Text = poObject.PorcAportePersonal.ToString();
                txtPeriodoNomina.Text = poObject.PeriodoNomina.ToString();
                txtAnioInicioNomina.Text = poObject.AnioInicioNomina.ToString();
                txtMesInicioNomina.Text = poObject.MesInicioNomina.ToString();
                txtMinGraciaPostSalida.Text = poObject.MinGraciaPostSalida.ToString();
                chkEnvioCorreo.Checked = poObject.TieneConfiguradoCorreo;
                txtCorreoUsuario.Text = poObject.CorreoUsuario;
                txtContrasenaUsuario.Text = poObject.Contrasena;
                txtConfContrasenaUsuario.Text = poObject.Contrasena;
                txtPais.Text = poObject.Pais;
                txtCiudad.Text = poObject.Ciudad;
                txtServidorSMTP.Text = poObject.ServidorSMTP;
                txtCantidadDiasAprobacion.EditValue = poObject.CantidadDiasApron;
                if(poObject.PuertoSMTP != null) txtPuerto.Text = poObject.PuertoSMTP.ToString();
                if (poObject.UsaSSL != null) chkUsaSSL.Checked = (bool)poObject.UsaSSL;
                if (poObject.AutentificarSMTP != null) chkAutenticarSMTP.Checked = (bool)poObject.AutentificarSMTP;

                txtConexion.Text = poObject.Conexion;
                txtPorcentajeRecargoNocturno.EditValue = poObject.PorcRecargoJornadaNocturna;
                txtPorcentajeHoraSuplementaria.EditValue = poObject.PorcHoraExtraSuplementaria50;
                txtPorcentajeHoraExtraordinaria.EditValue = poObject.PorcHoraExtraSuplementaria100;
                txtPorcentajeFdsFeriado.EditValue = poObject.PorcHoraExtraFds_Feriado;
                txtDiasLaborablesMes.EditValue = poObject.DiasLaborablesMes;
                txtHorasLaborablesDia.EditValue = poObject.HorasLaborablesDia;
                txtDiaCorteHoraExtra.EditValue = poObject.DiaCorteHorasExtras;
                txtDiaInicioCorteHoraExtra.EditValue = poObject.DiaInicioCorteHorasExtras;
                txtIntervaloAjusteProvision.EditValue = poObject.IntervaloAjusteProvision;
                txtTextoCierreSistema.Text = poObject.MensajePrevioCierreSistema;
                chbCerrarSistema.Checked = poObject.CerrarSistema;

                tbcPrincipal.SelectedTabPageIndex = 3;
                tbpHorasExtras.Focus();
  
                if (poObject.HoraInicioJornadaDiurna != null)
                {
                    var pdFecha = poObject.HoraInicioJornadaDiurna ?? DateTime.Now.TimeOfDay;
                    dtpInicioJornadaDiurna.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, pdFecha.Hours, pdFecha.Minutes, pdFecha.Seconds);
                    dtpInicioJornadaDiurna.Refresh();
                }

                if (poObject.HoraFinJornadaDiurna != null)
                {
                    var pdFecha = poObject.HoraFinJornadaDiurna ?? DateTime.Now.TimeOfDay;
                    dtpFinJornadaDiurna.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, pdFecha.Hours, pdFecha.Minutes, pdFecha.Seconds);
                    dtpFinJornadaDiurna.Refresh();
                }

                if (poObject.HoraGeneralEntrada != null)
                {
                    var pdFecha = poObject.HoraGeneralEntrada ?? DateTime.Now.TimeOfDay;
                    dtpHoraGeneralEntrada.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, pdFecha.Hours, pdFecha.Minutes, pdFecha.Seconds);
                    dtpHoraGeneralEntrada.Refresh();
                }

                if (poObject.HoraGeneralSalida != null)
                {
                    var pdFecha = poObject.HoraGeneralSalida ?? DateTime.Now.TimeOfDay;
                    dtpHoraGeneralSalida.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, pdFecha.Hours, pdFecha.Minutes, pdFecha.Seconds);
                    dtpHoraGeneralSalida.Refresh();
                }

                if (poObject.TiempoAlmuerzo != null)
                {
                    var pdFecha = poObject.TiempoAlmuerzo ?? DateTime.Now.TimeOfDay;
                    dtpTiempoAlmuerzo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, pdFecha.Hours, pdFecha.Minutes, pdFecha.Seconds);
                    dtpTiempoAlmuerzo.Refresh();
                }

                if (poObject.HoraInicioGraciaPost != null)
                {
                    var pdFecha = poObject.HoraInicioGraciaPost ?? DateTime.Now.TimeOfDay;
                    dtpHoraInicioTiempoGraciaPost.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, pdFecha.Hours, pdFecha.Minutes, pdFecha.Seconds);
                    dtpHoraInicioTiempoGraciaPost.Refresh();
                }

                if (poObject.HoraFinGraciaPost != null)
                {
                    var pdFecha = poObject.HoraFinGraciaPost ?? DateTime.Now.TimeOfDay;
                    dtpHoraFinTiempoGraciaPost.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, pdFecha.Hours, pdFecha.Minutes, pdFecha.Seconds);
                    dtpHoraFinTiempoGraciaPost.Refresh();
                }

                tbcPrincipal.SelectedTabPageIndex = 5;

                if (poObject.HoraCierreSistema != null)
                {
                    var pdFecha = poObject.HoraCierreSistema ?? DateTime.Now.TimeOfDay;
                    dtpHoraCierreSistema.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, pdFecha.Hours, pdFecha.Minutes, pdFecha.Seconds);
                    dtpHoraCierreSistema.Refresh();
                }

                tbcPrincipal.SelectedTabPageIndex = 0;

                if (poObject.MontoMaxDeduccionGP != null)
                {
                    txtMontoMaxDeduccionGP.EditValue = poObject.MontoMaxDeduccionGP;
                }

                if (poObject.EdadTerceraEdad != null)
                {
                    txtEdadTerceraEdad.EditValue = poObject.EdadTerceraEdad;
                }
            }

            else
            {
                lLimpiar();
            }
           
        }

        #endregion
    }
}
