using REH_Presentacion.Formularios;
using REH_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraEditors;
using REH_Presentacion.Comun;
using REH_Negocio.Parametrizadores;

namespace REH_Presentacion.Parametrizadores
{
    /// <summary>
    /// Autor: Guillermo Murillo
    /// Fecha: 1/07/2021
    /// Formulario de parámetro de TipoPermiso
    /// </summary>
    public partial class frmPaTipoPermiso : frmBaseDev
    {

        #region Variables

        clsNTipoPermiso loLogicaNegocio;
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmPaTipoPermiso()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNTipoPermiso();
            lCargarEventos();
        }

        /// <summary>
        /// Ejecuta la accion de limpiar al presionar el boton Nuevo
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
        /// Ejecuta la busqueda al presionar el boton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                List<TipoPermiso> poListaObject = loLogicaNegocio.goListarMaestro(txtDescripcion.Text.Trim());

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
        /// Muestra el primer registro de la tabla
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


                    TipoPermiso poObject = new TipoPermiso();
                    poObject.Codigo = txtCodigo.Text.Trim();
                    poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                    poObject.Descripcion = txtDescripcion.Text.Trim();
                    poObject.Fecha = DateTime.Now;
                    poObject.Usuario = clsPrincipal.gsUsuario;
                    poObject.Terminal = string.Empty;
                    poObject.AfectaDiasLaborales = chbAfectaDias.Checked;
                    poObject.DiasMaximoCobertura = Convert.ToInt32(txtDiasMax.Text);
                    poObject.DiasCoberturaPorcentaje = Convert.ToInt32(txtPorcentajeDia.Text);
                    poObject.PorcentajeCobertura = Convert.ToDecimal(txtCobertura.Text);
                    poObject.AplicaLicencias = chbAplicaLicencias.Checked;
                    poObject.AplicaPermisosPorHoras = chbAplicaPermisosPorHoras.Checked;
                    if (!string.IsNullOrEmpty(txtMinDescontar.Text))
                    {
                        poObject.MinutosDescontar = int.Parse(txtMinDescontar.Text);
                    } 
                    poObject.AplicaDescuentoHaberes = chbAplicaDescuentoHaberes.Checked;


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
        /// Elimina logicamene el registro seleccionado
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
        /// Load del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPaTipoPermiso_Load(object sender, EventArgs e)
        {
            try
            {

                lCargarEventosBotones();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Metodos

        /// <summary>
        /// Carga los botonoes y los eventos de los botones agregar, eliminar, nuevo
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnPrimero"] != null) tstBotones.Items["btnPrimero"].Click += btnPrimero_Click;
            if (tstBotones.Items["btnAnterior"] != null) tstBotones.Items["btnAnterior"].Click += btnAnterior_Click;
            if (tstBotones.Items["btnSiguiente"] != null) tstBotones.Items["btnSiguiente"].Click += btnSiguiente_Click;
            if (tstBotones.Items["btnUltimo"] != null) tstBotones.Items["btnUltimo"].Click += btnUltimo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
        }

        /// <summary>
        /// valida que solo de puedn ingresar numeros
        /// </summary>
        private void lCargarEventos()
        {
            //Validación-Eventos Datos 
            txtCobertura.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtCobertura.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);
            txtPorcentajeDia.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtPorcentajeDia.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtDiasMax.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtDiasMax.KeyPress += new KeyPressEventHandler(SoloNumeros);
            txtDescripcion.KeyDown += new KeyEventHandler(EnterEqualTab);
            cmbEstado.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtMinDescontar.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtMinDescontar.KeyPress += new KeyPressEventHandler(SoloNumeros);
        }

        /// <summary>
        /// Metodo consultar
        /// </summary>
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
                    txtDiasMax.Text = poObject.DiasMaximoCobertura.ToString();
                    txtPorcentajeDia.Text = poObject.DiasCoberturaPorcentaje.ToString();
                    txtCobertura.Text = poObject.PorcentajeCobertura.ToString();
                    txtFechaHoraIngreso.Text = poObject.Fecha.ToString();
                    txtFechaHoraModificacion.Text = poObject.FechaMod.ToString();
                    chbAfectaDias.Checked = poObject.AfectaDiasLaborales;
                    chbAplicaLicencias.Checked = poObject.AplicaLicencias;
                    chbAplicaPermisosPorHoras.Checked = poObject.AplicaPermisosPorHoras;
                    txtTerminalModificacion.Text = poObject.TerminalMod;
                    txtUsuarioIngreso.Text = poObject.Usuario;
                    txtUsuarioModificacion.Text = poObject.UsuarioMod;
                    txtMinDescontar.Text = poObject.MinutosDescontar.ToString();
                    chbAplicaDescuentoHaberes.Checked = poObject.AplicaDescuentoHaberes;
                }
            }
        }

        /// <summary>
        /// Validar si hay algo escrito en los txt, y en los que no son necesario se coloca cero
        /// </summary>
        /// <returns></returns>
        private bool lbEsValido()
        {
            if (txtDiasMax.Text == "")
            {
                txtDiasMax.Text = "0";
            }
            if (txtPorcentajeDia.Text == "")
            {
                txtPorcentajeDia.Text = "0";
            }
            if (txtCobertura.Text == "")
            {
                txtCobertura.Text = "0";
            }
            if (String.IsNullOrEmpty(txtDescripcion.Text.Trim()))
            {
                XtraMessageBox.Show("La descripción no puede estar vacía", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Limpia todos los controles del formulario
        /// </summary>
        private void lLimpiar()
        {
            txtCodigo.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtCobertura.Text = string.Empty;
            txtPorcentajeDia.Text = string.Empty;
            txtDiasMax.Text = string.Empty;
            chbAfectaDias.Checked = false;
            chbAplicaLicencias.Checked = false;
            chbAplicaPermisosPorHoras.Checked = false;
            txtMinDescontar.Text = string.Empty;
            chbAplicaDescuentoHaberes.Checked = false;
        }

        #endregion

    }
}
