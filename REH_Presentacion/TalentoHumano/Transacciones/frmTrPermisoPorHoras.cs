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
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
using DevExpress.XtraReports.UI;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 20/08/2021
    /// Formulario para ingresar licencias y permisos por horas
    /// </summary>
    public partial class frmTrPermisoPorHoras : frmBaseTrxDev
    {

        #region Variables
        clsNPermiso loLogicaNegocio;
        private bool pbCargado = false;
        //private bool pbCargado = false;
        private int lId = 0;
        #endregion

        public frmTrPermisoPorHoras()
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
                clsComun.gLLenarCombo(ref cmbEmpleado, loLogicaNegocio.goConsultarComboEmpleado(clsPrincipal.gsUsuario, int.Parse(Tag.ToString().Split(',')[0])), true);
                clsComun.gLLenarCombo(ref cmbTipo, loLogicaNegocio.goConsultarComboTipoPermisoPorHoras(), true);
                
                lLimpiar();
                pbCargado = true;
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
                    var poObject = new PermisoHoras();
                    poObject.Id = lId;
                    poObject.CodigoEstado = string.IsNullOrEmpty(lblEstado.Text) ? Diccionario.Pendiente : Diccionario.gsGetCodigo(lblEstado.Text);
                    poObject.CodigoTipoPermiso = cmbTipo.EditValue.ToString();
                    poObject.IdPersona = int.Parse(cmbEmpleado.EditValue.ToString());
                    poObject.Fecha = dtpFecha.Value;
                    poObject.TodoDia = chbTodoDia.Checked;

                    poObject.HoraInicio = dtpFechaInicio.Value.TimeOfDay;
                    poObject.HoraFin = dtpFechaFin.Value.TimeOfDay;
                    poObject.Observacion = txtObservacion.Text.Trim();

                    var psMsg = loLogicaNegocio.gsGuardarPorHoras(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, clsPrincipal.gIdPerfil);
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
                        
                        var psMsg = loLogicaNegocio.gsEliminarMaestroPorHoras(lId, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, int.Parse(Tag.ToString().Split(',')[0]));
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

        /// <summary>
        /// Evento del botón Nuevo, Generalmente Limpia el formulario 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {

                if (lId > 0)
                {
                    DataSet ds = new DataSet();
                    var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC [REHSPCONSULTAPERMISOSID] '{0}'", lId));
                    xrptSolicitudPermiso xrptGen = new xrptSolicitudPermiso();
                    dt.TableName = "SolicitudPermiso";
                    ds.Merge(dt);


                    //xrptGen.Parameters["parameter1"].Value = string.Format("SOLICITUD DE PERMISO # {0}", lId);
                    xrptGen.Parameters["parameter1"].Value = string.Format("SOLICITUD DE PERMISO");
                    xrptGen.DataSource = ds;
                    xrptGen.RequestParameters = false;
                    xrptGen.Parameters["parameter1"].Visible = false;
                    using (ReportPrintTool printTool = new ReportPrintTool(xrptGen))
                    {
                        printTool.ShowRibbonPreviewDialog();
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
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

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

            if (dtpFechaInicio.Value.TimeOfDay.Hours == 0)
            {
                XtraMessageBox.Show("Ingrese Hora de Inicio", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (dtpFechaFin.Value.TimeOfDay.Hours == 0)
            {
                XtraMessageBox.Show("Ingrese Hora de Fin", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            //var poFechaDif = dtpFechaFin.Value.TimeOfDay - dtpFechaInicio.Value.TimeOfDay;
            //if (poFechaDif.Minutes == 0)
            //{
            //    XtraMessageBox.Show("Ingrese Hora de Fin", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}

            return true;
        }

        private void lLimpiar()
        {
            if ((cmbEmpleado.Properties.DataSource as IList).Count > 0) cmbEmpleado.ItemIndex = 0;
            if ((cmbTipo.Properties.DataSource as IList).Count > 0) cmbTipo.ItemIndex = 0;
            //if ((cmbEmpleadoCubre.Properties.DataSource as IList).Count > 0) cmbEmpleadoCubre.ItemIndex = 0;
            //clsComun.gResetDtpCheck(ref dtpFechaFinPeriodoLactancia);
            dtpFecha.Value = DateTime.Now;
            dtpFechaInicio.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dtpFechaFin.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            cmbEmpleado.Focus();
            lId = 0;
            lblEstado.Text = "";
            txtObservacion.Text = "";
            chbTodoDia.Checked = false;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
        }

        private void lBuscar()
        {
            List<PermisoHoras> poListaObject = loLogicaNegocio.goListarPorHoras(clsPrincipal.gsUsuario, int.Parse(Tag.ToString().Split(',')[0]));
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[]
                                {
                                    new DataColumn("Id",typeof(int)),
                                    new DataColumn("Identificación"),
                                    new DataColumn("Nombre"),
                                    new DataColumn("Tipo"),
                                    new DataColumn("Estado"),
                                    new DataColumn("Fecha Solicitud", typeof(DateTime)),
                                    new DataColumn("Fecha Permiso", typeof(DateTime)),
                                    new DataColumn("Hora Inicio",typeof(TimeSpan)),
                                    new DataColumn("Hora Fin",typeof(TimeSpan))
                                });

            poListaObject.ForEach(a =>
            {
                DataRow row = dt.NewRow();
                row["Id"] = a.Id;
                row["Identificación"] = a.NumeroIdentificacion;
                row["Nombre"] = a.DesPersona;
                row["Tipo"] = a.DesTipoPermiso;
                row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                row["Fecha Solicitud"] = a.FechaInicio;
                row["Fecha Permiso"] = a.Fecha;
                row["Hora Inicio"] = a.HoraInicio;
                row["Hora Fin"] = a.HoraFin;
                dt.Rows.Add(row);
            });

            frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Permisos por Horas" };
            pofrmBuscar.Width = 1000;
            if (pofrmBuscar.ShowDialog() == DialogResult.OK)
            {
                lLimpiar();
                lId = int.Parse(pofrmBuscar.lsCodigoSeleccionado);
                lConsultar();
            }
        }

        private void lConsultar()
        {
            if (lId > 0)
            {
                var poObject = loLogicaNegocio.goBuscarMaestroPorHoras(lId);
                if (poObject != null)
                {
                    if (poObject.CodigoEstado != Diccionario.Pendiente)
                    {
                        if (clsPrincipal.gIdPerfil == 2 || clsPrincipal.gIdPerfil == 12)
                        {
                            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                        }
                        else
                        {
                            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                        }
                    }
                    else
                    {
                        if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
                    }

                    cmbEmpleado.EditValue = poObject.IdPersona.ToString() ;
                    cmbTipo.EditValue = poObject.CodigoTipoPermiso;
                    dtpFecha.Value = poObject.Fecha;
                    dtpFechaInicio.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, poObject.HoraInicio.Hours, poObject.HoraInicio.Minutes, poObject.HoraInicio.Seconds);
                    dtpFechaFin.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, poObject.HoraFin.Hours, poObject.HoraFin.Minutes, poObject.HoraFin.Seconds);
                    txtObservacion.Text = poObject.Observacion;
                    lblEstado.Text = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    pbCargado = false;
                    chbTodoDia.Checked = poObject.TodoDia;
                    pbCargado = true;
                    //if (poObject.IdPersonaCubre != null) cmbEmpleadoCubre.EditValue = poObject.IdPersonaCubre.ToString();
                    //if (poObject.FechaFinPermisoLactancia != null) dtpFechaFinPeriodoLactancia.Value = poObject.FechaFinPermisoLactancia??DateTime.Now;
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
               
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chbTodoDia_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    if (!chbTodoDia.Checked)
                    {
                        dtpFechaInicio.Enabled = true;
                        dtpFechaFin.Enabled = true;

                    }
                    else
                    {
                        dtpFechaInicio.Enabled = false;
                        dtpFechaFin.Enabled = false;

                        dtpFechaInicio.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 30, 0);
                        dtpFechaFin.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0);
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
