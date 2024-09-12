using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using REH_Presentacion.TalentoHumano.Reportes.Rpt;
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

namespace REH_Presentacion.TalentoHumano.Transacciones
{

    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 12/04/2022
    /// Formulario para ingresar préstamos internos
    /// </summary>
    public partial class frmTrSolicitudPrestamo : frmBaseTrxDev
    {

        #region Variables
        clsNMovimientos loLogicaNegocio;
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel;
        bool pbCargado = false;
        bool pbOrdernar = true;
        private int lId = 0;
        string lsTipo = ""; //P - Prestamo, A- Anticipo, D - Descuento
        #endregion

        public frmTrSolicitudPrestamo()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNMovimientos();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        private void frmTrSolicitudPrestamo_Load(object sender, EventArgs e)
        {
            if (Text.ToUpper().Contains("ANT"))
            {
                lsTipo = Diccionario.ListaCatalogo.TipoMovimientoClass.Anticipos;
                lblValorPrestamo.Text = "Valor Solicitado";
                group.Text = "Solicitud de Anticipo";
                clsComun.gLLenarCombo(ref cmbTipo, loLogicaNegocio.goConsultarComboTipoPrestamoAnticipoDescuento(lsTipo), true);
            }
            else if (Text.ToUpper().Contains("DESC"))
            {
                lsTipo = Diccionario.ListaCatalogo.TipoMovimientoClass.DescuentosProgramados;
                lblValorPrestamo.Text = "Valor Solicitado";
                group.Text = "Descuentos Varios";
                clsComun.gLLenarCombo(ref cmbTipo, loLogicaNegocio.goConsultarComboTipoPrestamoAnticipoDescuento(lsTipo), true);
                lblMotivo.Visible = false;
                cmbMotivo.Visible = false;
            }
            else
            {
                group.Text = "Solicitud de Préstamo Interno";
                lsTipo = Diccionario.ListaCatalogo.TipoMovimientoClass.PrestamoInterno;
                clsComun.gLLenarCombo(ref cmbTipo, loLogicaNegocio.goConsultarComboTipoPrestamoAnticipoDescuento(lsTipo), false);
                lblTipo.Visible = false;
                cmbTipo.Visible = false;
            }

            rdbPlazo.Checked = true;
            lCargarEventosBotones();
            clsComun.gLLenarCombo(ref cmbEmpleado, loLogicaNegocio.goConsultarComboEmpleado(), true);
            clsComun.gLLenarCombo(ref cmbMotivo, loLogicaNegocio.goConsultarComboMotivoPrestamoInterno(), true);
            clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboCodigoPeriodo().Where(x=>!x.Codigo.Contains("J")).ToList(), "Rol", false);
            clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboEstado(), "Estado", false);
            lLimpiar();
            pbCargado = true;

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
                    dgvDatos.PostEditor();
                    lOrdenar();

                    var poObject = new PrestamoInterno();
                    poObject.IdPrestamoInterno = string.IsNullOrEmpty(txtNo.Text) ? 0 : Convert.ToInt32(txtNo.Text);
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.CapacidadEndeudamiento = Convert.ToDecimal(txtCapacidadDeuda.EditValue);
                    poObject.Ingresos = Convert.ToDecimal(txtIngresos.EditValue);
                    poObject.Egresos = Convert.ToDecimal(txtEgresos.EditValue);
                    poObject.ValorCredito = Convert.ToDecimal(txtValor.EditValue);
                    poObject.Plazo = Convert.ToInt32(txtPlazo.EditValue);
                    poObject.Ciudad = txtCiudad.Text;
                    poObject.FechaInicioContrato = dtpFechaIngreso.Value;
                    poObject.FechaInicioPago = dtpFechaInicioPago.Value;
                    poObject.FechaSolicitud = dtpFechaSolicitud.Value;
                    poObject.IdPersona = int.Parse(cmbEmpleado.EditValue.ToString());
                    poObject.CodigoMotivoPrestamoInterno = cmbMotivo.EditValue.ToString();
                    poObject.Observacion = txtObservacion.Text.Trim();
                    poObject.CodigoTipoPrestamo = cmbTipo.EditValue.ToString();

                    poObject.PrestamoInternoDetalle = new List<PrestamoInternoDetalle>();
                    foreach (var item in (List<TablaAmortizacionPI>)bsDatos.DataSource)
                    {
                        var poDetalle = new PrestamoInternoDetalle();
                        poDetalle.Anio = item.Año;
                        poDetalle.Mes = item.Mes;
                        poDetalle.CodigoEstado = item.Estado;
                        poDetalle.CodigoTipoRol = "";
                        poDetalle.CodigoPeriodo = item.Rol;
                        poDetalle.Cuota = item.Cuota;
                        poDetalle.IdPrestamoInternoDetalle = item.Id;
                        poDetalle.Observacion = "";
                        poDetalle.Valor = item.Valor;
                        poObject.PrestamoInternoDetalle.Add(poDetalle);
                    }

                    int pId = 0;
                    var psMsg = loLogicaNegocio.gsGuardarPrestamoInterno(poObject, lsTipo, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, out pId);
                    if (string.IsNullOrEmpty(psMsg))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lLimpiar();
                        lImprimir(pId);
                    }
                    else
                    {
                        XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (!string.IsNullOrEmpty(txtNo.Text))
                {


                    DialogResult dialogResult = XtraMessageBox.Show(Diccionario.MsgRegistroAEliminar, Diccionario.MsgTituloRegistroAEliminar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var psMsg = loLogicaNegocio.gEliminarMaestro(int.Parse(txtNo.Text), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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

                if (!string.IsNullOrEmpty(txtNo.Text))
                {
                    lImprimir(int.Parse(txtNo.Text));

                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del boton de eliminar en el Grid, elimina la fila seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<TablaAmortizacionPI>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    if (poLista[piIndex].Estado == Diccionario.Cerrado)
                    {
                        XtraMessageBox.Show("No es posible eliminar cuota, ya ha sido tomada en Rol.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        poLista.RemoveAt(piIndex);
                        bsDatos.DataSource = poLista;
                        dgvDatos.RefreshData();
                        lCalculoAutomatico();
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
            txtValorCuotaFija.KeyPress += new KeyPressEventHandler(SoloNumerosSimbolo);

            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnEliminar"] != null) tstBotones.Items["btnEliminar"].Click += btnEliminar_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;

            bsDatos.DataSource = new List<TablaAmortizacionPI>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsBehavior.AllowSortAnimation = DevExpress.Utils.DefaultBoolean.False;

            dgvDatos.Columns["Id"].Visible = false;
            dgvDatos.Columns["AñoMes"].Visible = false;
            dgvDatos.Columns["Mes"].Visible = false;
            dgvDatos.Columns["Año"].Visible = false;
            dgvDatos.Columns["Periodo"].Visible = false;
            dgvDatos.Columns["Descripcion"].Visible = false;
            dgvDatos.Columns["Estado"].Visible = false;

            dgvDatos.Columns["Cuota"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Estado"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["Cuota"].Width = 5;
            dgvDatos.Columns["Año"].Width = 5;
            dgvDatos.Columns["Mes"].Width = 5;
            

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);

            /********************************************************************************************************************/
            dgvDatos.Columns["Valor"].UnboundType = UnboundColumnType.Decimal;
            dgvDatos.Columns["Valor"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Valor"].DisplayFormat.FormatString = "c2";
            dgvDatos.Columns["Valor"].Summary.Add(SummaryItemType.Sum, "Valor", "{0:c2}");

            GridGroupSummaryItem item1 = new GridGroupSummaryItem();
            item1.FieldName = "Valor";
            item1.SummaryType = SummaryItemType.Sum;
            item1.DisplayFormat = "{0:c2}";
            item1.ShowInGroupColumnFooter = dgvDatos.Columns["Valor"];
            dgvDatos.GroupSummary.Add(item1);
            /********************************************************************************************************************/

            txtValor.Properties.Mask.EditMask = "c";
            txtValor.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtValor.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtIngresos.Properties.Mask.EditMask = "c";
            txtIngresos.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtIngresos.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtEgresos.Properties.Mask.EditMask = "c";
            txtEgresos.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtEgresos.Properties.Mask.UseMaskAsDisplayFormat = true;

            txtCapacidadDeuda.Properties.Mask.EditMask = "c";
            txtCapacidadDeuda.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txtCapacidadDeuda.Properties.Mask.UseMaskAsDisplayFormat = true;

            clsComun.gOrdenarColumnasGrid(dgvDatos);
        }

        private bool lbEsValido()
        {

            //if (cmbEmpleado.EditValue.ToString() == Diccionario.Seleccione)
            //{
            //    XtraMessageBox.Show("Seleccione Empleado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}

            //if (cmbTipo.EditValue.ToString() == Diccionario.Seleccione)
            //{
            //    XtraMessageBox.Show("Seleccione Tipo Vacación.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}

            //if (dtpFechaInicio.Value > dtpFechaFin.Value)
            //{
            //    XtraMessageBox.Show("Rango de fechas inconsistentes, la fecha de inicio no puede ser mayor a la fecha fin.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}

            return true;
        }

        private void lLimpiar()
        {
            pbCargado = false;
            txtValor.EditValue = 0M;
            txtIngresos.EditValue = 0M;
            txtEgresos.EditValue = 0M;
            txtCapacidadDeuda.EditValue = 0M;
            txtPlazo.EditValue = 0;
            bsDatos.DataSource = new List<TablaAmortizacionPI>();
            gcDatos.DataSource = bsDatos;
            txtNo.Text = "";
            cmbEmpleado.ReadOnly = false;
            if ((cmbEmpleado.Properties.DataSource as IList).Count > 0) cmbEmpleado.ItemIndex = 0;
            txtCiudad.Text = "";
            lblEstado.Text = "";
            dtpFechaIngreso.Value = DateTime.Now;
            dtpFechaInicioPago.Value = DateTime.Now;
            dtpFechaSolicitud.Value = DateTime.Now;
            if ((cmbMotivo.Properties.DataSource as IList).Count > 0) cmbMotivo.ItemIndex = 0;
            txtObservacion.Text = "";
            pbCargado = true ;
            txtValorCuotaFija.Text = "";
            btnCalcular.Enabled = true;
            txtValor.ReadOnly = false;
            txtPlazo.ReadOnly = false;
            dtpFechaInicioPago.Enabled = true;
        }

        private void lBuscar()
        {
            List<PrestamoInterno> poListaObject = loLogicaNegocio.goListarPrestamos(lsTipo);
            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[]
                                {
                                    new DataColumn("Id",typeof(int)),
                                    new DataColumn("Empleado"),
                                    new DataColumn("Fecha Solicitud",typeof(DateTime)),
                                    new DataColumn("Valor", typeof(decimal)),
                                    new DataColumn("Observación"),
                                    new DataColumn("Estado")
                                });

            poListaObject.ForEach(a =>
            {
                DataRow row = dt.NewRow();
                row["Id"] = a.IdPrestamoInterno;
                row["Empleado"] = a.DesPersona;
                row["Fecha Solicitud"] = a.FechaSolicitud.ToString("dd/MM/yyyyy");
                row["Valor"] = a.ValorCredito;
                row["Observación"] = a.Observacion;
                row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                dt.Rows.Add(row);
            });

            frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de " + Text };
            pofrmBuscar.ClientSize = new Size(1200, 275);
            if (pofrmBuscar.ShowDialog() == DialogResult.OK)
            {
                txtNo.Text = pofrmBuscar.lsCodigoSeleccionado;
                lConsultar();
            }
        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtNo.Text))
            {
                var poObject = loLogicaNegocio.goBuscarEntidadPrestamo(int.Parse(txtNo.Text));
                if (poObject != null)
                {
                    pbCargado = false;
                    cmbEmpleado.EditValue = poObject.IdPersona.ToString();
                    cmbMotivo.EditValue = poObject.CodigoMotivoPrestamoInterno;
                    cmbTipo.EditValue = poObject.CodigoTipoPrestamo;
                    dtpFechaSolicitud.Value = poObject.FechaSolicitud;
                    dtpFechaIngreso.Value = poObject.FechaInicioContrato;
                    dtpFechaInicioPago.Value = poObject.FechaInicioPago;
                    txtCapacidadDeuda.EditValue = poObject.CapacidadEndeudamiento;
                    txtCiudad.Text = poObject.Ciudad;
                    txtEgresos.EditValue = poObject.Egresos;
                    txtIngresos.EditValue = poObject.Ingresos;
                    txtObservacion.Text = poObject.Observacion;
                    txtPlazo.EditValue = poObject.Plazo;
                    txtValor.EditValue = poObject.ValorCredito;
                    
                    lblEstado.Text = Diccionario.gsGetDescripcion(poObject.CodigoEstado);
                    
                    cmbEmpleado.ReadOnly = true;
                    
                    var poLista = new List<TablaAmortizacionPI>();

                    foreach (var item in poObject.PrestamoInternoDetalle.OrderBy(x=>x.Cuota))
                    {
                        var poDetalle = new TablaAmortizacionPI();
                        poDetalle.Id = item.IdPrestamoInternoDetalle;
                        poDetalle.Cuota = item.Cuota;
                        poDetalle.Año = item.Anio;
                        poDetalle.Mes = item.Mes;
                        poDetalle.Rol = item.CodigoPeriodo;
                        poDetalle.Estado = item.CodigoEstado;
                        poDetalle.Valor = item.Valor;
                        poLista.Add(poDetalle);

                    }
                    bsDatos.DataSource = poLista;
                    gcDatos.DataSource = bsDatos;

                    if (poObject.PrestamoInternoDetalle.Where(x=>x.CodigoEstado == Diccionario.Cerrado).Count() > 0)
                    {
                        btnCalcular.Enabled = false;
                        txtValor.ReadOnly = true;
                        txtPlazo.ReadOnly = true;
                        dtpFechaInicioPago.Enabled = false;
                    }
                    else
                    {
                        btnCalcular.Enabled = true;
                        txtValor.ReadOnly = false;
                        txtPlazo.ReadOnly = false;
                        dtpFechaInicioPago.Enabled = true;
                    }

                    pbCargado = true;
                }
                else
                {
                    lLimpiar();
                }
            }
        }


        #endregion

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            bsDatos.AddNew();
            ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Estado = Diccionario.Activo;
            ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Cuota = ((List<TablaAmortizacionPI>)bsDatos.DataSource).Max(x => x.Cuota) + 1;

            int Año, Mes;
            lGetAñoMes(DateTime.Now, out Año, out Mes);
            ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Año = Año;
            ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Mes = Mes;
            ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Rol = string.Format("M{0}{1}", Año, Mes.ToString("00"));

            dgvDatos.Focus();
            dgvDatos.ShowEditor();
            dgvDatos.UpdateCurrentRow();
            dgvDatos.RefreshData();
            lCalculoAutomatico();
        }

        private void lGetAñoMes(DateTime tdFecha, out int Año, out int Mes)
        {
            Año = 0;
            Mes = 0;

            var piAño = ((List<TablaAmortizacionPI>)bsDatos.DataSource).Max(x => x.Año);
            var piMes = ((List<TablaAmortizacionPI>)bsDatos.DataSource).Where(x=>x.Año == piAño).Max(x => x.Mes);

            if (piAño == 0 || piMes == 0)
            {
                Año = tdFecha.Year;
                Mes = tdFecha.Month;
            }
            else if (piMes == 12)
            {
                Año = piAño + 1;
                Mes = 1;
            }
            else
            {
                Año = piAño;
                Mes = piMes + 1;
            }
        }

        private void frmTrSolicitudPrestamo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }
        }

        private void cmbEmpleado_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    DateTime dateTime;
                    var poCapacidadEndeudamiento = loLogicaNegocio.goCapacidadEndeudamiento(int.Parse(cmbEmpleado.EditValue.ToString()),out dateTime);
                    if (poCapacidadEndeudamiento != null)
                    {
                        txtIngresos.EditValue = poCapacidadEndeudamiento.Ingresos;
                        txtEgresos.EditValue = poCapacidadEndeudamiento.Egresos;
                        txtCapacidadDeuda.EditValue = poCapacidadEndeudamiento.CapacidadEndeudamiento;
                        dtpFechaIngreso.Value = dateTime;
                    }
                    else
                    {
                        txtIngresos.EditValue = 0;
                        txtEgresos.EditValue = 0;
                        txtCapacidadDeuda.EditValue = 0;
                        dtpFechaIngreso.Value = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Calcular()
        {
            if (lbValidarPrevioCalculo())
            {
                txtValorCuotaFija.Text = "";

                pbOrdernar = false;
                var poLista = (List<TablaAmortizacionPI>)bsDatos.DataSource;

                bsDatos.DataSource = new List<TablaAmortizacionPI>();
                gcDatos.DataSource = bsDatos;

                decimal pdcValorCuota = Math.Round(Convert.ToDecimal(txtValor.EditValue) / Convert.ToDecimal(txtPlazo.EditValue), 2);
                decimal Acum = 0;
                for (int i = 1; i <= int.Parse(txtPlazo.EditValue.ToString()); i++)
                {

                    bsDatos.AddNew();
                    ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Estado = Diccionario.Activo;
                    ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Cuota = i;
                    if (i == int.Parse(txtPlazo.EditValue.ToString()))
                    {
                        ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Valor = Convert.ToDecimal(txtValor.EditValue) - Acum;
                    }
                    else
                    {
                        ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Valor = pdcValorCuota;
                    }



                    int Año, Mes;
                    lGetAñoMes(dtpFechaInicioPago.Value, out Año, out Mes);
                    ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Año = Año;
                    ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Mes = Mes;
                    ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Rol = string.Format("M{0}{1}", Año, Mes.ToString("00"));

                    dgvDatos.Focus();
                    dgvDatos.ShowEditor();
                    dgvDatos.UpdateCurrentRow();
                    dgvDatos.RefreshData();

                    Acum += pdcValorCuota;
                }

                if (pdcValorCuota > Convert.ToDecimal(txtCapacidadDeuda.EditValue))
                {
                    XtraMessageBox.Show("El valor de la cuota supera su capacidad de endeudamiento.", "Advertencia!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                pbOrdernar = true;
            }
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdbPlazo.Checked)
                {
                    Calcular();
                }
                else
                {
                    CalcularCuotaFija();
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool lbValidarPrevioCalculo()
        {
            if (string.IsNullOrEmpty(txtValor.EditValue.ToString()))
            {
                XtraMessageBox.Show("Ingrese el valor del préstamo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (Convert.ToDecimal(txtValor.EditValue) <= 0)
            {
                XtraMessageBox.Show("El valor del préstamo debe ser mayor a 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtPlazo.EditValue.ToString()))
            {
                XtraMessageBox.Show("Ingrese el plazo del préstamo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (Convert.ToInt32(txtPlazo.EditValue) <= 0)
            {
                XtraMessageBox.Show("El plazo debe ser mayor a 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            return true;
        }

        private bool lbValidarPrevioCalculoPorCapacidadEndeudamiento()
        {
            if (string.IsNullOrEmpty(txtValor.EditValue.ToString()))
            {
                XtraMessageBox.Show("Ingrese el valor del crédito", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (Convert.ToDecimal(txtValor.EditValue) <= 0)
            {
                XtraMessageBox.Show("El valor del crédito debe ser mayor a 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtValorCuotaFija.EditValue.ToString()))
            {
                XtraMessageBox.Show("Ingrese el valor de cuota fija", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (Convert.ToInt32(txtValorCuotaFija.EditValue) <= 0)
            {
                XtraMessageBox.Show("El valor de cuota fija debe ser mayor a 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            return true;
        }

        private void lImprimir(int lId)
        {
            DataSet ds = new DataSet();
            var dtCabVac = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPRPTSOLICITUDPRESTAMOINTERNOCAB {0}", lId));
            dtCabVac.TableName = "PrestamoInterno";

            var dtCabDet = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC REHSPRPTSOLICITUDPRESTAMOINTERNODET {0}", lId));
            dtCabDet.TableName = "PrestamoInternoDetalle";

            ds.Merge(dtCabVac);
            ds.Merge(dtCabDet);
            if (dtCabVac.Rows.Count > 0)
            {
                xrptSolicitudPrestamo xrpt = new xrptSolicitudPrestamo();

                //xrpt.Parameters["Titulo"].Value = "SOLICITUD DE PRÉSTAMO";
                xrpt.Parameters["Titulo"].Value = lsTipo == Diccionario.ListaCatalogo.TipoMovimientoClass.DescuentosProgramados ? cmbTipo.Text : "SOLICITUD DE " + cmbTipo.Text;
                xrpt.DataSource = ds;
                //Se invoca la ventana que muestra el reporte.
                xrpt.RequestParameters = false;
                xrpt.Parameters["Titulo"].Visible = false;               


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

        private void lCalculoAutomatico()
        {
            int piPlazo = 0;

            foreach (var item in (List<TablaAmortizacionPI>)bsDatos.DataSource)
            {
                piPlazo++;
                item.Cuota = piPlazo;
            }

            txtPlazo.EditValue = piPlazo;
        }
        
        private void lOrdenar()
        {
          
            int piPlazo = 0;
            var poLista = (List<TablaAmortizacionPI>)bsDatos.DataSource;

            foreach (var item in poLista.OrderBy(x => x.AñoMes))
            {
                piPlazo++;
                item.Cuota = piPlazo;

                if (item.Rol == Diccionario.Tablas.TipoRol.DecimoTercero)
                {
                    item.Mes = 12;
                }
                else if (item.Rol == Diccionario.Tablas.TipoRol.DecimoCuarto)
                {
                    var poEmpleado = loLogicaNegocio.goBuscarEmpleado(int.Parse(cmbEmpleado.EditValue.ToString()));
                    item.Mes = poEmpleado.MesDecimoCuarto;
                }
                else if (item.Rol == Diccionario.Tablas.TipoRol.Utilidades)
                {
                    item.Mes = 4;
                }
            }

            txtPlazo.EditValue = piPlazo;
            pbOrdernar = false;
            bsDatos.DataSource = poLista.OrderBy(x=>x.Cuota).ToList();
            dgvDatos.RefreshData();
            pbOrdernar = true;
                    
        }

        private void CalcularCuotaFija()
        {
           
            if (lbValidarPrevioCalculoPorCapacidadEndeudamiento())
            {
                pbOrdernar = false;
                var poLista = (List<TablaAmortizacionPI>)bsDatos.DataSource;

                bsDatos.DataSource = new List<TablaAmortizacionPI>();
                gcDatos.DataSource = bsDatos;

                decimal pdcValorCuota = Math.Round(Convert.ToDecimal(txtValorCuotaFija.EditValue), 2);
                decimal Acum = 0;
                int Cuota = 0;
                while (Math.Round(Convert.ToDecimal(txtValor.EditValue), 2) > Acum)
                {
                    Cuota++;
                    bsDatos.AddNew();
                    ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Estado = Diccionario.Activo;
                    ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Cuota = Cuota;
                    if ((Acum + pdcValorCuota) > Convert.ToDecimal(txtValor.EditValue))
                    {
                        ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Valor = Convert.ToDecimal(txtValor.EditValue) - Acum;
                    }
                    else
                    {
                        ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Valor = pdcValorCuota;
                    }



                    int Año, Mes;
                    lGetAñoMes(dtpFechaInicioPago.Value, out Año, out Mes);
                    ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Año = Año;
                    ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Mes = Mes;
                    ((List<TablaAmortizacionPI>)bsDatos.DataSource).LastOrDefault().Rol = string.Format("M{0}{1}", Año, Mes.ToString("00"));

                    dgvDatos.Focus();
                    dgvDatos.ShowEditor();
                    dgvDatos.UpdateCurrentRow();
                    dgvDatos.RefreshData();

                    Acum += pdcValorCuota;
                }

                txtPlazo.EditValue = Cuota;

                if (pdcValorCuota > Convert.ToDecimal(txtCapacidadDeuda.EditValue))
                {
                    XtraMessageBox.Show("El valor de la cuota supera su capacidad de endeudamiento.", "Advertencia!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                pbOrdernar = true;
            }

          
        }

        private void rdbPlazo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbPlazo.Checked)
            {
                txtPlazo.Enabled = true;
                txtValorCuotaFija.Text = "";
                txtValorCuotaFija.Enabled = false;
            }
            else
            {
                txtPlazo.Enabled = false;
                //txtPlazo.Text = "0";
                txtValorCuotaFija.Enabled = true;
            }
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            try
            {
                lOrdenar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDatos_CustomRowCellEditForEditing(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Rol")
                {
                    int piIndex;
                    // Tomamos la fila seleccionada
                    piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                    var poLista = (List<TablaAmortizacionPI>)bsDatos.DataSource;
                    if (poLista[piIndex].Rol == Diccionario.Tablas.TipoRol.DecimoTercero)
                    {
                        poLista[piIndex].Mes = 12;
                    }
                    else if (poLista[piIndex].Rol == Diccionario.Tablas.TipoRol.DecimoCuarto)
                    {
                        var poEmpleado = loLogicaNegocio.goBuscarEmpleado(int.Parse(cmbEmpleado.EditValue.ToString()));
                        poLista[piIndex].Mes = poEmpleado.MesDecimoCuarto;
                    }
                    else if (poLista[piIndex].Rol == Diccionario.Tablas.TipoRol.Utilidades)
                    {
                        poLista[piIndex].Mes = 4;
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
