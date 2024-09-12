using REH_Negocio;
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
using REH_Presentacion.Comun;
using DevExpress.XtraEditors;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 11/09/2020
    /// Formulario para ingresar licencias y permisos
    /// </summary>
    public partial class frmTrPermiso : frmBaseTrxDev
    {

        #region Variables
        clsNPermiso loLogicaNegocio;
        //private bool pbCargado = false;
        private int lId = 0;
        #endregion

        public frmTrPermiso()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNPermiso();
        }

        /// <summary>
        /// Evento que se dispara cuando carga el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPaRubroTipoRol_Load(object sender, EventArgs e)
        {
            try
            {
                cmbEmpleado.Focus();
                clsComun.gLLenarCombo(ref cmbEmpleado, loLogicaNegocio.goConsultarComboEmpleado(), true);
                clsComun.gLLenarCombo(ref cmbTipo, loLogicaNegocio.goConsultarComboTipoPermisoLicencia(), true);
                clsComun.gLLenarCombo(ref cmbEmpleadoCubre, loLogicaNegocio.goConsultarComboEmpleado(), true);

                lLimpiar();
                //pbCargado = true;
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
                SendKeys.Send("{TAB}");
                //dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    //List<RubroTipoRol> poDatos = (List<RubroTipoRol>)bsDatos.DataSource;

                    var poObject = new Permiso();
                    poObject.Id = lId;
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.CodigoTipoPermiso = cmbTipo.EditValue.ToString();
                    poObject.IdPersona = int.Parse(cmbEmpleado.EditValue.ToString());
                    if (cmbEmpleadoCubre.Enabled == false || cmbEmpleadoCubre.EditValue.ToString() == Diccionario.Seleccione)
                        poObject.IdPersonaCubre = null;
                    else
                        poObject.IdPersonaCubre = int.Parse(cmbEmpleadoCubre.EditValue.ToString());

                    //if (cmbEmpleadoCubre.Enabled == false || cmbEmpleadoCubre.EditValue.ToString() == Diccionario.Seleccione)
                    //    poObject.IdPersonaCubre = null;
                    //else
                    //    poObject.IdPersonaCubre = int.Parse(cmbEmpleadoCubre.EditValue.ToString());


                    poObject.FechaInicio = dtpFechaInicio.Value;
                    poObject.FechaFin = dtpFechaFin.Value;
                    poObject.Observacion = txtObservacion.Text.Trim();

                    var psMsg = loLogicaNegocio.gsGuardar(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        /// <summary>
        /// Evento del botón Eliminar, Cambia a estado eliminado un registro guardado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lId > 0)
                {
                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var psMsg = loLogicaNegocio.gsEliminarMaestro(lId, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if(string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;

            dtpFechaFinPeriodoLactancia.ValueChanged += new EventHandler(FechaValue_Changed);
        }

        private bool lbEsValido()
        {
            
            if (cmbEmpleado.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Empleado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (cmbTipo.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Tipo Permiso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //if (cmbEmpleado.EditValue.ToString() == cmbEmpleado.EditValue.ToString())
            //{
            //    XtraMessageBox.Show("Seleccione otro Empleado que cubre, No se puede seleccionar el mismo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}

            if (dtpFechaInicio.Value > dtpFechaFin.Value)
            {
                XtraMessageBox.Show("Rango de fechas inconsistentes, la fecha de inicio no puede ser mayor a la fecha fin.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }


            if (loLogicaNegocio.gbPermisosExistentes(int.Parse(cmbEmpleado.EditValue.ToString()), dtpFechaInicio.Value.Date, dtpFechaFin.Value.Date, lId))
            {
                XtraMessageBox.Show("Fechas coinciden con un ingreso previo, por favor revisar no es posible guardar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void lLimpiar()
        {
            if ((cmbEmpleado.Properties.DataSource as IList).Count > 0) cmbEmpleado.ItemIndex = 0;
            if ((cmbTipo.Properties.DataSource as IList).Count > 0) cmbTipo.ItemIndex = 0;
            if ((cmbEmpleadoCubre.Properties.DataSource as IList).Count > 0) cmbEmpleadoCubre.ItemIndex = 0;
            clsComun.gResetDtpCheck(ref dtpFechaFinPeriodoLactancia);
            dtpFechaInicio.Value = DateTime.Now;
            dtpFechaFin.Value = DateTime.Now;
            cmbEmpleado.Focus();
            txtObservacion.Text = "";
            lId = 0;
        }

        private void lBuscar()
        {
            List<Permiso> poListaObject = loLogicaNegocio.goListar();
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[]
                                {
                                    new DataColumn("Id",typeof(int)),
                                    new DataColumn("Identificación"),
                                    new DataColumn("Nombre"),
                                    new DataColumn("Tipo"),
                                    new DataColumn("Fecha Registro",typeof(DateTime)),
                                    new DataColumn("Fecha Inicio",typeof(DateTime)),
                                    new DataColumn("Fecha Fin",typeof(DateTime))
                                });

            poListaObject.ForEach(a =>
            {
                DataRow row = dt.NewRow();
                row["Id"] = a.Id;
                row["Identificación"] = a.NumeroIdentificacion;
                row["Nombre"] = a.DesPersona;
                row["Tipo"] = a.DesTipoPermiso;
                row["Fecha Registro"] = a.Fecha.ToString("dd/MM/yyyy");
                row["Fecha Inicio"] = a.FechaInicio.ToString("dd/MM/yyyy");
                row["Fecha Fin"] = a.FechaFin.ToString("dd/MM/yyyy");
                dt.Rows.Add(row);
            });

            frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Permisos" };
            if (pofrmBuscar.ShowDialog() == DialogResult.OK)
            {
                lId = Convert.ToInt32(pofrmBuscar.lsCodigoSeleccionado);
                lConsultar();
            }
        }

        private void lConsultar()
        {
            if (lId > 0)
            {
                var poObject = loLogicaNegocio.goBuscarMaestro(lId);
                if (poObject != null)
                {
                    cmbEmpleado.EditValue = poObject.IdPersona.ToString() ;
                    cmbTipo.EditValue = poObject.CodigoTipoPermiso;
                    dtpFechaInicio.Value = poObject.FechaInicio;
                    dtpFechaFin.Value = poObject.FechaFin;
                    if (poObject.IdPersonaCubre != null) cmbEmpleadoCubre.EditValue = poObject.IdPersonaCubre.ToString();
                    if (poObject.FechaFinPermisoLactancia != null) dtpFechaFinPeriodoLactancia.Value = poObject.FechaFinPermisoLactancia??DateTime.Now;
                }
                else
                {
                    lLimpiar();
                }
            }
        }


        #endregion

        private void cmbTipo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                dtpFechaFinPeriodoLactancia.Enabled = false;
                cmbEmpleadoCubre.Enabled = false;
                if(cmbTipo.EditValue.ToString() == Diccionario.Tablas.TipoPermiso.Vacaciones)
                {
                    cmbEmpleadoCubre.Enabled = false;
                }
                else if(cmbTipo.EditValue.ToString() == Diccionario.Tablas.TipoPermiso.PermisoLactancia)
                {
                    dtpFechaFinPeriodoLactancia.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
