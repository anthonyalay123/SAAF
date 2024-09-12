using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraEditors;

namespace REH_Presentacion.Maestros
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 24/01/2020
    /// Formulario para dato maestro "Tipo Contrato"
    /// </summary>
    public partial class frmMaTipoContrato : frmBaseDev
    {
        #region Variables
        clsNTipoContrato loLogicaNegocio;
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmMaTipoContrato()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNTipoContrato();
        }

        /// <summary>
        /// Evento que se dispara cuando carga el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMaTipoContrato_Load(object sender, EventArgs e)
        {
            try
            {
                gCrearBotones();
                lCargarEventosBotones();
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
                    Maestro poObject = new Maestro();

                    poObject.Codigo = txtCodigo.Text.Trim();
                    poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                    poObject.Descripcion = txtDescripcion.Text.Trim();
                    poObject.Fecha = DateTime.Now;
                    poObject.Usuario = clsPrincipal.gsUsuario;
                    poObject.Terminal = string.Empty;
                    poObject.AsumeIessEmpresa = chbAsumeIessEmpresa.Checked;
                    poObject.PagoUtilidades = chbAplicaPagoUtilidades.Checked;
                    poObject.PagoVacacionesMensual = chbAplicaPagoVacacionesMensual.Checked;
                    poObject.ProvicionVacaciones = chbProvisionVacaciones.Checked;
                    poObject.ProvisionD3 = chbProvisionD3.Checked;
                    poObject.ProvisionD4 = chbProvisionD4.Checked;
                    poObject.PorcAportePersonal = Convert.ToDecimal(txtAportePersonal.Text);
                    poObject.PorcAportePatronal = Convert.ToDecimal(txtAportePatronal.Text);
                    poObject.PorcAporteSecap = Convert.ToDecimal(txtAporteSecap.Text);

                    if (loLogicaNegocio.gbGuardar(poObject))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
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
                List<Maestro> poListaObject = loLogicaNegocio.goListarMaestro(txtDescripcion.Text.Trim());
                DataTable dt = new DataTable();

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

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Tipo Contrato" };

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

        #region Métodos

        private void lLimpiar()
        {
            txtCodigo.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            cmbEstado.ItemIndex = 0;
            txtFechaHoraIngreso.Text = String.Empty;
            txtFechaHoraModificacion.Text = String.Empty;
            txtTerminalIngreso.Text = String.Empty;
            txtTerminalModificacion.Text = String.Empty;
            txtUsuarioIngreso.Text = String.Empty;
            txtUsuarioModificacion.Text = String.Empty;
            chbAsumeIessEmpresa.Checked = false;
            chbAplicaPagoUtilidades.Checked = false;
            chbAplicaPagoVacacionesMensual.Checked = false;
            chbAplicaPagoUtilidades.Checked = false;
            chbProvisionVacaciones.Checked = false;
            chbProvisionD3.Checked = false;
            chbProvisionD4.Checked = false;
            txtAportePersonal.Text ="0" ;
            txtAportePatronal.Text ="0";
            txtAporteSecap.Text = "0";


        }

        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
        }
        private bool lbEsValido()
        {
            if (String.IsNullOrEmpty(txtDescripcion.Text.Trim()))
            {
                XtraMessageBox.Show("La descripción no puede estar vacía", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscarMaestro(txtCodigo.Text.Trim());
                if (poObject != null)
                {
                    txtCodigo.Text = poObject.Codigo;
                    txtDescripcion.Text = poObject.Descripcion;
                    cmbEstado.EditValue = poObject.CodigoEstado;
                    txtFechaHoraIngreso.Text = poObject.Fecha.ToString();
                    txtFechaHoraModificacion.Text = poObject.FechaMod.ToString();
                    txtTerminalIngreso.Text = poObject.Terminal;
                    txtTerminalModificacion.Text = poObject.TerminalMod;
                    txtUsuarioIngreso.Text = poObject.Usuario;
                    txtUsuarioModificacion.Text = poObject.UsuarioMod;
                    chbAsumeIessEmpresa.Checked = poObject.AsumeIessEmpresa;
                    chbAplicaPagoUtilidades.Checked = poObject.PagoUtilidades;
                    chbAplicaPagoVacacionesMensual.Checked = poObject.PagoVacacionesMensual;
                    chbProvisionVacaciones.Checked = poObject.ProvicionVacaciones;
                    chbProvisionD3.Checked = poObject.ProvisionD3;
                    chbProvisionD4.Checked = poObject.ProvisionD4;
                    txtAportePersonal.Text = poObject.PorcAportePersonal.ToString();
                    txtAportePatronal.Text = poObject.PorcAportePatronal.ToString();
                    txtAporteSecap.Text = poObject.PorcAporteSecap.ToString();
                 


                }
            }
        }
        #endregion

       
    }
}
