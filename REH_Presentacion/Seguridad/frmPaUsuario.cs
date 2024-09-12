using DevExpress.XtraEditors;
using REH_Negocio;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using REH_Negocio.Parametrizadores;
using System.Collections;
using GEN_Entidad;
using REH_Presentacion.Comun;
using GEN_Entidad.Entidades;
using DevExpress.XtraEditors.Mask;

namespace REH_Presentacion.Parametrizadores
{
    public partial class frmPaUsuario : frmBaseDev
    {
        #region variables
        private bool lbCargado = false;
        clsNUsuario loLogicaNegocio;
        #endregion


        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmPaUsuario()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Evento que se ejecuta cuando se levanta el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPaUsuario_Load(object sender, EventArgs e)
        {

            loLogicaNegocio = new clsNUsuario();
            try
            {
                lbCargado = false;

                clsComun.gLLenarCombo(ref cmbIdPerfil, loLogicaNegocio.goConsultarComboPerfil(), true);
                clsComun.gLLenarCombo(ref cmbDepartamento, loLogicaNegocio.goConsultarComboDepartamento(), true);
                clsComun.gLLenarCombo(ref cmbIdPersona, loLogicaNegocio.goConsultarComboIdPersona(), true);
                clsComun.gLLenarCombo(ref cmbUsuarioSap, loLogicaNegocio.goSapConsultaComboUsuarios(), true);
                clsComun.gLLenarCombo(ref cmbBodegaEPP, loLogicaNegocio.goConsultarComboBodegaEPP(), true);

                tmInicio.Properties.Mask.MaskType = MaskType.DateTime;
                tmInicio.Properties.Mask.EditMask = "HH:mm";
                tmInicio.MaskBox.Mask.UseMaskAsDisplayFormat = true;
                tmFin.Properties.Mask.MaskType = MaskType.DateTime;
                tmFin.Properties.Mask.EditMask = "HH:mm";
                tmFin.MaskBox.Mask.UseMaskAsDisplayFormat = true;
                txtCantMinCotizaciones.Text = "0";
                lCargarEventosBotones();

                lbCargado = true;

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
                if (lbEsValido())
                {
                    Usuario poObject = new Usuario();
                    poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                    poObject.Codigo = txtCodigo.Text.Trim();
                    poObject.Clave = gsEncriptar(txtClave.Text.Trim()); 
                    poObject.Descripcion = txtDescripcion.Text.Trim();
                    poObject.IdPerfil = Convert.ToInt32(cmbIdPerfil.EditValue);
                    poObject.Fecha = DateTime.Now;
                    poObject.Usuario = clsPrincipal.gsUsuario;
                    poObject.Terminal = string.Empty;
                    poObject.CodigoDepartamento = cmbDepartamento.EditValue.ToString();
                    poObject.IdPersona = Int32.Parse(cmbIdPersona.EditValue.ToString());
                    poObject.TamanoMB = decimal.Parse(txtMb.EditValue.ToString());
                    poObject.AprobacionFinalCotizacion = chkboxAprobacion.Checked;
                    poObject.HoraInicioNotificacion =  TimeSpan.Parse(tmInicio.Text);
                    poObject.HoraFinNotificacion = TimeSpan.Parse(tmFin.Text);
                    poObject.MinFrecuenciaNotificacion = int.Parse(txtFrecuencia.EditValue.ToString());
                    poObject.Correo = txtCorreo.Text.Trim();
                    poObject.MontoMax = decimal.Parse(txtMontoMaxCompra.EditValue.ToString());
                    poObject.EditaProveedorFormaPago = chbEditaProveedorFP.Checked;
                    poObject.EditaTipoOrdenPago = chbEditaTipoOrdenPago.Checked;
                    poObject.CantMinCotizaciones = int.Parse(txtCantMinCotizaciones.EditValue.ToString());
                    poObject.VisualizaZonaOrdenPago = chbVisualizaZonaOrdenPago.Checked;
                    poObject.EnviarDesdeCorreoCorporativo = chbEnviarCorreoCorporativo.Checked;
                    poObject.ClaveDesdeCorreoCorporativo = gsEncriptar(txtClaveCorreoCorporativo.Text.Trim());
                    poObject.CorreoCorporativo = txtCorreoCorporativo.Text;
                    poObject.CodigoUsuarioSap = cmbUsuarioSap.EditValue.ToString();
                    poObject.BodegaEPP = int.Parse(cmbBodegaEPP.EditValue.ToString());
                    poObject.ControlaDuplicidadGuias = chbControlaDuplicidadGuias.Checked;

                    if (chbEnviarCorreoCorporativo.Checked)
                    {
                        if (string.IsNullOrEmpty(txtCorreo.Text))
                        {
                            XtraMessageBox.Show("Ingrese el correo corporativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (string.IsNullOrEmpty(txtClaveCorreoCorporativo.Text))
                        {
                            XtraMessageBox.Show("Ingrese la clave del correo corporativo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    var msg = loLogicaNegocio.gbGuardar(poObject);
                    if (string.IsNullOrEmpty(msg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                    else
                    {
                        XtraMessageBox.Show(msg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                List<Usuario> poListaObject = loLogicaNegocio.goListarMaestro(txtDescripcion.Text.Trim());
                DataTable dt = new DataTable();
                clsComun.gLLenarCombo(ref cmbDepartamento, loLogicaNegocio.goConsultarComboDepartamento(), true);
                clsComun.gLLenarCombo(ref cmbIdPersona, loLogicaNegocio.goConsultarComboIdPersona(), true);
                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Código"),
                                    new DataColumn("Descripción"),
                                    new DataColumn("Estado")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Código"] = a.Codigo;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                    row["Descripción"] = a.Descripcion;

                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    txtCodigo.Text = pofrmBuscar.lsCodigoSeleccionado;
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
                txtCodigo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Primero, txtCodigo.Text.Trim());
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
                txtCodigo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Anterior, txtCodigo.Text.Trim());
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
                txtCodigo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Siguiente, txtCodigo.Text.Trim());
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
                txtCodigo.Text = loLogicaNegocio.goBuscarCodigo(Diccionario.BuscarCodigo.Tipo.Ultimo, txtCodigo.Text.Trim());
                lConsultar();
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


        #endregion


        #region metodos

        private bool lbEsValido()
        {
            if (cmbIdPerfil.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione el tipo de Perfil.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (String.IsNullOrEmpty(txtDescripcion.Text.Trim()))
            {
                XtraMessageBox.Show("El nombre no puede estar vacio", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (String.IsNullOrEmpty(txtClave.Text.Trim()))
            {
                XtraMessageBox.Show("Ingrese una clave", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void lLimpiar()
        {
            lbCargado = false;
            txtCodigo.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtClave.Text = string.Empty;
            chbEnviarCorreoCorporativo.Checked = false;
            txtClaveCorreoCorporativo.Text = string.Empty;
            cmbEstado.ItemIndex = 0;
            txtFechaHoraIngreso.Text = String.Empty;
            txtFechaHoraModificacion.Text = String.Empty;
            txtTerminalIngreso.Text = String.Empty;
            txtTerminalModificacion.Text = String.Empty;
            txtUsuarioIngreso.Text = String.Empty;
            txtUsuarioModificacion.Text = String.Empty;
            txtMb.EditValue = string.Empty;
            chkboxAprobacion.Checked = false;
            chbEditaProveedorFP.Checked = false;
            txtFrecuencia.EditValue = string.Empty;
            txtCantMinCotizaciones.Text = "0";
            tmFin.EditValue = new TimeSpan();
            tmInicio.EditValue = new TimeSpan();
            if ((cmbIdPerfil.Properties.DataSource as IList).Count > 0) cmbIdPerfil.ItemIndex = 0;
            if ((cmbDepartamento.Properties.DataSource as IList).Count > 0) cmbDepartamento.ItemIndex = 0;
            if ((cmbIdPersona.Properties.DataSource as IList).Count > 0) cmbIdPersona.ItemIndex = 0;
            if ((cmbUsuarioSap.Properties.DataSource as IList).Count > 0) cmbUsuarioSap.ItemIndex = 0;
            if ((cmbBodegaEPP.Properties.DataSource as IList).Count > 0) cmbBodegaEPP.ItemIndex = 0;
            txtCorreo.Text = string.Empty;
            txtMontoMaxCompra.Text = string.Empty;
            chbVisualizaZonaOrdenPago.Checked = false;
            txtCorreoCorporativo.Text = "";
            chbControlaDuplicidadGuias.Checked = true;
            lbCargado = true;


        }
        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
           // if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
           // if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
           if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;

            txtCantMinCotizaciones.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscarMaestroUsuario(txtCodigo.Text.Trim());
                if (poObject != null)
                {
                    lbCargado = false;
                    txtCodigo.Text = poObject.Codigo;
                    txtDescripcion.Text = poObject.Descripcion;
                    cmbEstado.EditValue = poObject.CodigoEstado;
                    cmbIdPerfil.EditValue = !string.IsNullOrEmpty(poObject.IdPerfil.ToString()) ? poObject.IdPerfil.ToString() : Diccionario.Seleccione;
                    txtClave.EditValue = gsDesencriptar(poObject.Clave);
                    txtFechaHoraIngreso.Text = poObject.Fecha.ToString();
                    txtFechaHoraModificacion.Text = poObject.FechaMod.ToString();
                    txtUsuarioIngreso.Text = poObject.Usuario;
                    txtUsuarioModificacion.Text = poObject.UsuarioMod;
                    cmbIdPersona.EditValue = poObject.IdPersona.ToString();
                    txtMb.EditValue = poObject.TamanoMB.ToString();
                    chkboxAprobacion.Checked = poObject.AprobacionFinalCotizacion;
                    txtFrecuencia.EditValue = poObject.MinFrecuenciaNotificacion;
                    tmInicio.EditValue = poObject.HoraInicioNotificacion;
                    tmFin.EditValue = poObject.HoraFinNotificacion;
                    txtCorreo.Text = poObject.Correo;
                    txtMontoMaxCompra.Text = poObject.MontoMax.ToString();
                    chbEditaProveedorFP.Checked = poObject.EditaProveedorFormaPago;
                    chbEditaTipoOrdenPago.Checked = poObject.EditaTipoOrdenPago;
                    txtCantMinCotizaciones.EditValue = poObject.CantMinCotizaciones;
                    chbVisualizaZonaOrdenPago.Checked = poObject.VisualizaZonaOrdenPago;
                    chbEnviarCorreoCorporativo.Checked = poObject.EnviarDesdeCorreoCorporativo;
                    txtClaveCorreoCorporativo.Text = string.IsNullOrEmpty(poObject.ClaveDesdeCorreoCorporativo) ? "" : gsDesencriptar(poObject.ClaveDesdeCorreoCorporativo);
                    txtCorreoCorporativo.Text = poObject.CorreoCorporativo;
                    cmbUsuarioSap.EditValue = string.IsNullOrEmpty(poObject.CodigoUsuarioSap) ? Diccionario.Seleccione : poObject.CodigoUsuarioSap;
                    cmbBodegaEPP.EditValue = poObject.BodegaEPP.ToString();
                    chbControlaDuplicidadGuias.Checked = poObject.ControlaDuplicidadGuias;

                    if (poObject.CodigoDepartamento != null)
                    {
                        cmbDepartamento.EditValue = poObject.CodigoDepartamento.ToString();
                    }
                    else
                    {
                        poObject.CodigoDepartamento = Diccionario.Seleccione;
                    }
                    
                    //cmbDepartamento.EditValue = !string.IsNullOrEmpty(poObject.CodigoDepartamento.ToString()) ? poObject.CodigoDepartamento.ToString() : Diccionario.Seleccione;

                    tmInicio.Properties.Mask.MaskType = MaskType.DateTime;
                    tmInicio.Properties.Mask.EditMask = "HH:mm";
                    tmInicio.MaskBox.Mask.UseMaskAsDisplayFormat = true;
                    tmFin.Properties.Mask.MaskType = MaskType.DateTime;
                    tmFin.Properties.Mask.EditMask = "HH:mm";
                    tmFin.MaskBox.Mask.UseMaskAsDisplayFormat = true;

                    lbCargado = true;
                }
            }
        }
        /// <summary>
        /// Metodo que sirve para llenar el formulario al salir del txtCodigo usuario si es que existe un usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Función que devuelve una cadena encriptada
        /// </summary>
        /// <param name="tsCadena">texto a encriptar</param>
        /// <returns>texto encriptado</returns>
        public static string gsEncriptar(string tsCadena)
        {
            string result = string.Empty;
            byte[] encryted = Encoding.Unicode.GetBytes(tsCadena);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        /// <summary>
        /// Función que devuelve una cadena desencriptada
        /// </summary>
        /// <param name="tsCadena">texto a desencriptar</param>
        /// <returns>valor desencriptado</returns>
        public static string gsDesencriptar(string tsCadena)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(tsCadena);
            result = Encoding.Unicode.GetString(decryted);
            return result;
        }


        #endregion

        private void chbEnviarCorreoCorporativo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbCargado)
                {
                    if (chbEnviarCorreoCorporativo.Checked)
                    {
                        txtClaveCorreoCorporativo.Enabled = true;
                        txtCorreoCorporativo.Enabled = true;
                    }
                    else
                    {
                        txtClaveCorreoCorporativo.Enabled = false;
                        txtCorreoCorporativo.Enabled = false;
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
