﻿using REH_Presentacion.Comun;
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
    /// Formulario para registrar la cuenta contable de SAP Rubro por Centro de Costo
    /// </summary>
    public partial class frmPaCuentaContableRubro : frmBaseTrxDev
    {

        #region Variables
        bool lbCargado;
        clsNCuentaContable loLogicaNegocio;
        public string lsCodigoRubro;
        public string lsCodigoCentroCosto;
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        public frmPaCuentaContableRubro()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNCuentaContable();
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
                lbCargado = false;
                clsComun.gLLenarCombo(ref cmbCentroCosto, loLogicaNegocio.goConsultarComboCentroCosto(), true);
                clsComun.gLLenarCombo(ref cmbRubro , loLogicaNegocio.goConsultarComboRubroCentroCosto(), true);
                lbCargado = true;
                lCargarEventosBotones();
                lCargarGrid();
                lCargarParametrizaciones();

                if (!string.IsNullOrEmpty(lsCodigoRubro))
                {
                    cmbCentroCosto.EditValue = lsCodigoCentroCosto;
                    cmbRubro.EditValue = lsCodigoRubro;
                    lCargaCuentaContableRubroCentroCosto();
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
                    var psResult = loLogicaNegocio.gbGuardarCuentaContableCentroCostoRubro(cmbCentroCosto.EditValue.ToString(), cmbRubro.EditValue.ToString(), txtCuentaContable.Text.Trim(), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    if (string.IsNullOrEmpty(psResult))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                        if (!string.IsNullOrEmpty(lsCodigoRubro))
                        {
                            Close();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show(psResult, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                //List<Persona> poListaObject = loLogicaNegocio.goListar();
                //DataTable dt = new DataTable();

                //dt.Columns.AddRange(new DataColumn[]
                //                    {
                //                    new DataColumn("Identificación"),
                //                    new DataColumn("Nombre"),
                //                    new DataColumn("Estado Contrato"),
                //                    new DataColumn("Cuenta Contable"),
                //                    });

                //poListaObject.ForEach(a =>
                //{
                //    DataRow row = dt.NewRow();
                //    row["Identificación"] = a.NumeroIdentificacion;
                //    row["Nombre"] = a.NombreCompleto;
                //    row["Estado Contrato"] = a.EstadoContrato;
                //    row["Cuenta Contable"] = a.Empleado.CuentaContable;
                //    dt.Rows.Add(row);
                //});

                //frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Persona" };
                //if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                //{
                //    //txtNumeroIdentificacion.Text = pofrmBuscar.lsCodigoSeleccionado;
                //    lConsultar();
                //}

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

        private void gcParametrizaciones_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (dgvParametrizaciones.FocusedRowHandle >= 0)
                {
                    cmbCentroCosto.EditValue = dgvParametrizaciones.GetDataRow(dgvParametrizaciones.FocusedRowHandle).ItemArray[0].ToString();
                    cmbRubro.EditValue = dgvParametrizaciones.GetDataRow(dgvParametrizaciones.FocusedRowHandle).ItemArray[2].ToString();
                    lCargaCuentaContableRubroCentroCosto();
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

        private void cmbCentroCosto_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbCargado)
                    lCargaCuentaContableRubroCentroCosto();
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
            if ((cmbCentroCosto.Properties.DataSource as IList).Count > 0) cmbCentroCosto.ItemIndex = 0;
            if ((cmbRubro.Properties.DataSource as IList).Count > 0) cmbRubro.ItemIndex = 0;
            txtNombreCuenta.Text = string.Empty;
            lCargarGrid();
            lCargarParametrizaciones();
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
            //if (tstBotones.Items["btnSalir"] != null) tstBotones.Items["btnSalir"].Click += btnSalir_Click;
        }

        /// <summary>
        /// Valida datos previo a guardar
        /// </summary>
        /// <returns></returns>
        private bool lbEsValido()
        {


            if (cmbCentroCosto.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Centro de Costo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (cmbRubro.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Rubro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (cmbCentroCosto.EditValue.ToString() != Diccionario.Seleccione && cmbRubro.EditValue.ToString() != Diccionario.Seleccione)
            {
                var poObject = loLogicaNegocio.goBuscar("");
                if (poObject != null && poObject.IdPersona != 0)
                {
                    txtCuentaContable.EditValue = poObject.Empleado.CuentaContable;
                    //cmbCentroCosto.EditValue = poObject.CodigoCentroCosto;
                    //cmbRubro.EditValue = poObject.CodigoRubro;
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
                var poObject = loLogicaNegocio.gdtConsultaSAPPlanCuentas(txtCuentaContable.Text.Trim());
                if (poObject != null && poObject.Rows.Count > 0)
                {
                    txtNombreCuenta.Text = poObject.Rows[0]["Descripcion"].ToString();
                }
                else
                {
                    txtNombreCuenta.Text = string.Empty;
                }
            }
            else
            {
                txtNombreCuenta.Text = string.Empty;
            }

        }


        private void lCargarGrid()
        {
            gcDatos.DataSource = null;

            DataTable dt = loLogicaNegocio.gdtConsultaSAPPlanCuentas();

            bsDatos.DataSource = dt;
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns[0].Width = 40; 
        }

        private void lCargarParametrizaciones()
        {
            gcParametrizaciones.DataSource = null;

            DataTable dt = loLogicaNegocio.gdtConsultaParametrizacionesCentroCostoRubro();

            bsParametrizaciones.DataSource = dt;
            gcParametrizaciones.DataSource = bsParametrizaciones;

            dgvParametrizaciones.Columns[0].Width = 35; // Codigo Centro Costo
            dgvParametrizaciones.Columns[2].Width = 35; // Codigo Rubro

        }

        private void lCargaCuentaContableRubroCentroCosto()
        {
            if (cmbRubro.EditValue.ToString() != Diccionario.Seleccione && cmbCentroCosto.EditValue.ToString() != Diccionario.Seleccione)
            {
                txtCuentaContable.Text = loLogicaNegocio.gsConsultarCuentaContableRubroCentroCosto(cmbRubro.EditValue.ToString(), cmbCentroCosto.EditValue.ToString());
                lConsultarNombreCuenta();
            }
        }



        #endregion

       
    }
}
