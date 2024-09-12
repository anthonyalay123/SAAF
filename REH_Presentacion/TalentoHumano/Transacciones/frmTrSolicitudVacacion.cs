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
using REH_Presentacion.Reportes;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 11/09/2020
    /// Formulario para ingresar licencias y permisos
    /// </summary>
    public partial class frmTrSolicitudVacacion : frmBaseTrxDev
    {

        #region Variables
        clsNVacacion loLogicaNegocio;
        List<Vacacion> loListaVacacion;
        public int lId = 0;
        bool pbCargado = false;
        #endregion

        public frmTrSolicitudVacacion()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNVacacion();
            loListaVacacion = new List<Vacacion>();
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
                clsComun.gLLenarCombo(ref cmbTipo, loLogicaNegocio.goConsultarComboTipoVacacion(), true);
                clsComun.gLLenarCombo(ref cmbEmpleadoCubre, loLogicaNegocio.goConsultarComboEmpleado(), true);

                lCargarEventosBotones();
                lLimpiar();
                pbCargado = true;
                
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
                lGrabar(false);
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
        private void btnGrabarAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                lGrabar(true);
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
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroEliminado, Diccionario.MsgTituloRegistroEliminado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Evento del botón Imprimir, Presenta por pantalla Reporte.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {

                if(lId > 0)
                {
                    lImprimir(lId);

                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lImprimir(int lId)
        {
            DataSet ds = new DataSet();
            var dtCabVac = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPRPTSOLICITUDVACACIONESCAB {0}", lId));
            dtCabVac.TableName = "CabVac";
   
            var dtCabDet = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPRPTSOLICITUDVACACIONESDET {0}", lId));
            dtCabDet.TableName = "DetVac";

            ds.Merge(dtCabVac);
            ds.Merge(dtCabDet);
            if (dtCabVac.Rows.Count > 0)
            {
                xrptSolicitudVacaciones xrpt = new xrptSolicitudVacaciones();

                xrpt.DataSource = ds;
                //Se invoca la ventana que muestra el reporte.
                xrpt.RequestParameters = false;

                using (DevExpress.XtraReports.UI.ReportPrintTool printTool = new DevExpress.XtraReports.UI.ReportPrintTool(xrpt))
                {
                    printTool.ShowRibbonPreviewDialog();
                }
            }
            else
            {
                XtraMessageBox.Show("No existen datos guardados para imprimir.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (tstBotones.Items["btnGrabarAprobar"] != null) tstBotones.Items["btnGrabarAprobar"].Click += btnGrabarAprobar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

            //dtpFechaFinPeriodoLactancia.ValueChanged += new EventHandler(FechaValue_Changed);
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
                XtraMessageBox.Show("Seleccione Tipo Vacación.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (dtpFechaInicio.Value > dtpFechaFin.Value)
            {
                XtraMessageBox.Show("Rango de fechas inconsistentes, la fecha de inicio no puede ser mayor a la fecha fin.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void lGrabar(bool tbAprobar)
        {
            if (lbEsValido())
            {

                var poObject = new SolicitudVacacion();
                poObject.Id = lId;
                //poObject.CodigoEstado = Diccionario.Pendiente;
                poObject.CodigoTipoVacacion = cmbTipo.EditValue.ToString();
                poObject.IdPersona = int.Parse(cmbEmpleado.EditValue.ToString());
                poObject.Observacion = txtObservacion.Text.Trim();
                poObject.Reemplazo = txtReemplazo.Text.Trim();
                poObject.PagarReemplazo = chPagarReemplazo.Checked;
                if (cmbEmpleadoCubre.EditValue.ToString() != Diccionario.Seleccione)
                {
                    poObject.IdPersonaReemplazo = int.Parse(cmbEmpleadoCubre.EditValue.ToString());
                    poObject.Reemplazo = cmbEmpleadoCubre.Text;
                }

                poObject.FechaInicio = dtpFechaInicio.Value;
                poObject.FechaFin = dtpFechaFin.Value;

                int pId = 0;
                var psMsg = loLogicaNegocio.gbGuardar(poObject, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, out pId);
                if (string.IsNullOrEmpty(psMsg))
                {
                    if (tbAprobar)
                    {
                        loLogicaNegocio.gsAprobar(pId, clsPrincipal.gsUsuario, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal,210);
                    }
                    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lLimpiar();
                    lImprimir(pId);
                }
                else
                {
                    XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void lLimpiar()
        {
            pbCargado = false;
            if ((cmbEmpleado.Properties.DataSource as IList).Count > 0) cmbEmpleado.ItemIndex = 0;
            if ((cmbTipo.Properties.DataSource as IList).Count > 0) cmbTipo.ItemIndex = 0;
            if ((cmbEmpleadoCubre.Properties.DataSource as IList).Count > 0) cmbEmpleadoCubre.ItemIndex = 0;
            //clsComun.gResetDtpCheck(ref dtpFechaFinPeriodoLactancia);
            dtpFechaInicio.Value = DateTime.Now.Date;
            dtpFechaFin.Value = DateTime.Now;
            cmbEmpleado.Focus();
            lId = 0;
            txtNo.Text = "";
            lblDiasTomar.Text = "0";
            lblCantidadMaxDiasTomar.Text = "0";
            lblEstado.Text = ":";
            loListaVacacion = new List<Vacacion>();

            txtObservacion.Text = string.Empty;
            txtReemplazo.Text = string.Empty;
            chPagarReemplazo.Checked = false;

            cmbEmpleado.ReadOnly = false;
            cmbEmpleadoCubre.ReadOnly = false;
            cmbTipo.ReadOnly = false;
            dtpFechaFin.Enabled = true;
            dtpFechaInicio.Enabled = true;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = true;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = true;

            if(clsPrincipal.gIdPerfil == 2 || clsPrincipal.gIdPerfil == 12)
            {
                cmbTipo.Enabled = true;
                cmbEmpleadoCubre.Enabled = true;
                chPagarReemplazo.Visible = true;
                btnAsignarReemplazo.Enabled = true;
            }
            else
            {
                cmbTipo.Enabled = false;
                cmbEmpleadoCubre.Enabled = false;
                chPagarReemplazo.Visible = false;
                if ((cmbTipo.Properties.DataSource as IList).Count > 0) cmbTipo.ItemIndex = 1;
                btnAsignarReemplazo.Enabled = false;
            }
            pbCargado = true;
        }

        private void lBuscar()
        {
            List<SolicitudVacacion> poListaObject = loLogicaNegocio.goListar(clsPrincipal.gsUsuario, int.Parse(Tag.ToString().Split(',')[0]));
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[]
                                {
                                    new DataColumn("Id"),
                                    new DataColumn("Identificación"),
                                    new DataColumn("Nombre"),
                                    new DataColumn("Tipo"),
                                    new DataColumn("Fecha Inicio",typeof(DateTime)),
                                    new DataColumn("Fecha Fin",typeof(DateTime)),
                                    new DataColumn("Días"),
                                    new DataColumn("Estado")
                                });

            poListaObject.ForEach(a =>
            {
                DataRow row = dt.NewRow();
                row["Id"] = a.Id;
                row["Identificación"] = a.NumeroIdentificacion;
                row["Nombre"] = a.DesPersona;
                row["Tipo"] = a.DesTipoVacacion;
                row["Fecha Inicio"] = a.FechaInicio;
                row["Fecha Fin"] = a.FechaFin;
                row["Días"] = a.Dias;
                row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                dt.Rows.Add(row);
            });

            frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Solicitud de Vacaciones" };
            pofrmBuscar.ClientSize = new Size(800, 275);
            if (pofrmBuscar.ShowDialog() == DialogResult.OK)
            {
                lId = Convert.ToInt32(pofrmBuscar.lsCodigoSeleccionado);
                txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
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
                    pbCargado = false;
                    cmbEmpleado.EditValue = poObject.IdPersona.ToString() ;
                    cmbTipo.EditValue = poObject.CodigoTipoVacacion;
                    dtpFechaInicio.Value = poObject.FechaInicio;
                    dtpFechaFin.Value = poObject.FechaFin;
                    lblDiasTomar.Text = poObject.Dias.ToString();
                    txtObservacion.Text = poObject.Observacion;
                    txtReemplazo.Text = poObject.Reemplazo;
                    lblEstado.Text = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    chPagarReemplazo.Checked = poObject.PagarReemplazo;

                    if (poObject.IdPersonaReemplazo != null)
                    {
                        cmbEmpleadoCubre.EditValue = poObject.IdPersonaReemplazo.ToString();
                    }
                    else
                    {
                        cmbEmpleadoCubre.EditValue = Diccionario.Seleccione;
                    }

                    lblFecha.Text = poObject.Fecha.ToString("dd/MM/yyyy");

                    //if (clsPrincipal.gIdPerfil == 2 || clsPrincipal.gIdPerfil == 12)
                    //{
                    //    cmbEmpleado.ReadOnly = true;
                    //    cmbEmpleadoCubre.ReadOnly = false;
                    //    cmbTipo.ReadOnly = true;
                    //    dtpFechaFin.Enabled = false;
                    //    dtpFechaInicio.Enabled = false;
                    //}
                    //else
                    //{
                    //    cmbEmpleado.ReadOnly = true;
                    //    cmbEmpleadoCubre.ReadOnly = true;
                    //    cmbTipo.ReadOnly = true;
                    //    dtpFechaFin.Enabled = false;
                    //    dtpFechaInicio.Enabled = false;
                    //}

                    cmbEmpleado.ReadOnly = true;
                    cmbEmpleadoCubre.ReadOnly = true;
                    cmbTipo.ReadOnly = true;
                    dtpFechaFin.Enabled = false;
                    dtpFechaInicio.Enabled = false;


                    if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Enabled = false;
                    //if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Enabled = false;

                    var poLista = new List<SolicitudVacacionDetalleGridView>();

                    foreach (var item in poObject.SolicitudVacacionDetalle)
                    {
                        var poDetalle = new SolicitudVacacionDetalleGridView();
                        poDetalle.Periodo = item.Periodo;
                        poDetalle.Dias = item.Dias;
                        poDetalle.Valor = item.Valor;
                        poDetalle.DiasSaldo = item.DiasSaldo;
                        poLista.Add(poDetalle);
                    }
                    bsDatos.DataSource = poLista;
                    gcDatos.DataSource = bsDatos;

                    BindingSource bindingSource = new BindingSource();
                    bindingSource.DataSource = loLogicaNegocio.gdtConsultaSaldoVacacion(int.Parse(cmbEmpleado.EditValue.ToString()), txtNo.Text);
                    gcPreliminar.DataSource = bindingSource;

                    lOcultarColumnas();
                    
                    pbCargado = true;
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

        private void dtpFechaFin_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                var poDay = dtpFechaFin.Value.Date.Subtract(dtpFechaInicio.Value.Date);
                lblDiasTomar.Text = (poDay.Days + 1).ToString();
                lCargarDetalle();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lCargarDetalle()
        {
            if (pbCargado)
            {
                var poLista = new List<SolicitudVacacionDetalleGridView>();
                int piContador = 0;
                int piValorTomar = int.Parse(lblDiasTomar.Text);
                if (piValorTomar > 0)
                {
                    foreach (var item in loListaVacacion.OrderBy(x => x.Periodo))
                    {
                        int piContadorFor = 0;
                        for (int i = 0; i < item.SaldoDias; i++)
                        {
                            piContador++;
                            piContadorFor++;
                            if (piContador == int.Parse(lblCantidadMaxDiasTomar.Text)) break;
                            if (piContador == piValorTomar) break;
                        }
                        if (piContadorFor > 0) poLista.Add(new SolicitudVacacionDetalleGridView() { Periodo = item.Periodo, Dias = piContadorFor, Valor = Math.Round((item.SaldoValor * piContadorFor) / item.SaldoDias, 2), DiasSaldo = item.SaldoDias - piContadorFor });
                        if (piContador == int.Parse(lblCantidadMaxDiasTomar.Text)) break;
                        if (piContador == piValorTomar) break;
                    }
                }

                bsDatos.DataSource = poLista;
                gcDatos.DataSource = bsDatos;

                lOcultarColumnas();
            }
        }

        private void lOcultarColumnas()
        {
            if (clsPrincipal.gIdPerfil == 2 || clsPrincipal.gIdPerfil == 12) //Perfíl Talento Humano - Jefe de Talento Humano
            {
                dgvDatos.Columns["Valor"].Visible = true;
                dgvPreliminar.Columns["Valor"].Visible = true;
            }
            else
            {
                dgvDatos.Columns["Valor"].Visible = false;
                dgvPreliminar.Columns["Valor"].Visible = false;
            }
        }

        private void cmbEmpleado_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lblCantidadMaxDiasTomar.Text = loLogicaNegocio.giConsultaSaldoVacacion(int.Parse(cmbEmpleado.EditValue.ToString())).ToString();
                    loListaVacacion = loLogicaNegocio.goConsultaVacacion(int.Parse(cmbEmpleado.EditValue.ToString()));

                    BindingSource bindingSource = new BindingSource();
                    bindingSource.DataSource = loLogicaNegocio.gdtConsultaSaldoVacacion(int.Parse(cmbEmpleado.EditValue.ToString()));
                    gcPreliminar.DataSource = bindingSource;

                    lCargarDetalle();
                    lOcultarColumnas();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnComentarios_Click(object sender, EventArgs e)
        {
            try
            {
                int tId = string.IsNullOrEmpty(txtNo.Text) ? 0 : int.Parse(txtNo.Text);
                var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.SolicitudVacaciones, tId);

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Comentarios" };
                pofrmBuscar.ShowDialog();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAsignarReemplazo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNo.Text))
                {
                    frmBusqueda pofrmBuscar = new frmBusqueda(loLogicaNegocio.goConsultaDataTable("SELECT 0 Id, 'NINGUNO...' Empleado, '' CargoLaboral,''Departamento union all SELECT IdPersona Id, NombreCompleto Empleado, CargoLaboral, Departamento FROM REHVTPERSONACONTRATO (NOLOCK)")) { Text = "Listado de Solicitud de Vacaciones" };
                    pofrmBuscar.ClientSize = new Size(1200, 400);
                    if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                    {

                        DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAGuardar, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {
                            loLogicaNegocio.gsActualizaReempleazo(int.Parse(txtNo.Text), int.Parse(pofrmBuscar.lsCodigoSeleccionado),clsPrincipal.gsUsuario,true);
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lConsultar();
                        }
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
