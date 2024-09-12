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
using GEN_Entidad;
using DevExpress.XtraEditors;
using REH_Negocio;
using GEN_Negocio;

namespace REH_Presentacion.Parametrizadores
{
    /// <summary>
    /// Formulario para registrar la cuenta contable de SAP al Empleado
    /// </summary>
    public partial class frmPaCuentaContable : frmBaseTrxDev
    {



        #region Variables
        clsNEmpleado loLogicaNegocio;
        public int lIdPersona ;
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        public frmPaCuentaContable()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNEmpleado();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se levanta el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPaCuentaContable_Load(object sender, System.EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                lCargarGrid();

                if (lIdPersona > 0)
                {
                    txtNumeroIdentificacion.Text = new clsNEmpleado().gsConsultaIdentifiacion(lIdPersona);
                    lConsultar();
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
                    if (loLogicaNegocio.gbGuardarCuentaContable(txtNumeroIdentificacion.Text.Trim(), txtCuentaContable.Text.Trim(), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                        if (lIdPersona > 0)
                        {
                            Close();
                        }
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
                List<Persona> poListaObject = loLogicaNegocio.goListar();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Identificación"),
                                    new DataColumn("Nombre"),
                                    new DataColumn("Estado Contrato"),
                                    new DataColumn("Cuenta Contable"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Identificación"] = a.NumeroIdentificacion;
                    row["Nombre"] = a.NombreCompleto;
                    row["Estado Contrato"] = a.EstadoContrato;
                    row["Cuenta Contable"] = a.Empleado.CuentaContable;
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Persona" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    txtNumeroIdentificacion.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gcDatos_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (dgvDatos.FocusedRowHandle >= 0)
                {
                    txtCuentaContable.Text = dgvDatos.GetDataRow(dgvDatos.FocusedRowHandle).ItemArray[0].ToString();
                    lConsultarNombreCuenta();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private void txtCuentaContable_Leave(object sender, EventArgs e)
        {
            try
            {
                lConsultarNombreCuenta();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Limpia controles del formulario
        /// </summary>
        private void lLimpiar()
        {
            txtCuentaContable.Text = string.Empty;
            txtEmpleado.Text = string.Empty;
            txtNumeroIdentificacion.Text = string.Empty;
            txtNombreCuenta.Text = string.Empty;
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
        }

        /// <summary>
        /// Valida datos previo a guardar
        /// </summary>
        /// <returns></returns>
        private bool lbEsValido()
        {

            if (String.IsNullOrEmpty(txtNumeroIdentificacion.Text.Trim()))
            {
                XtraMessageBox.Show("Debe Seleccionar algún Empleado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (String.IsNullOrEmpty(txtCuentaContable.Text.Trim()))
            {
                XtraMessageBox.Show("La cuenta contable NO puede estar vacío", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Consulta Entidad
        /// </summary>
        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNumeroIdentificacion.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscar(txtNumeroIdentificacion.Text.Trim());
                if (poObject != null && poObject.IdPersona != 0)
                {
                    txtCuentaContable.EditValue = poObject.Empleado.CuentaContable;
                    txtNumeroIdentificacion.Text = poObject.NumeroIdentificacion;
                    txtEmpleado.Text = poObject.NombreCompleto;
                    lConsultarNombreCuenta();
                }
                else
                {
                    lLimpiar();
                }
            }

        }

        private void lConsultarNombreCuenta()
        {
            if (!string.IsNullOrEmpty(txtCuentaContable.Text.Trim()))
            {
                var poObject =  new clsNCuentaContable().gdtConsultaSAPPlanCuentas(txtCuentaContable.Text.Trim());
                if (poObject != null && poObject.Rows.Count > 0)
                {
                    txtNombreCuenta.Text = poObject.Rows[0]["Descripcion"].ToString();
                }
                else
                {
                    txtNombreCuenta.Text = string.Empty;
                }
            }

        }


        private void lCargarGrid()
        {
            gcDatos.DataSource = null;

            DataTable dt = loLogicaNegocio.gdtConsultaSAPPlanCuentas();

            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;
        }


        #endregion

       
    }
}
