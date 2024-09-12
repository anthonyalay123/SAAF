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
using DevExpress.XtraEditors;

namespace REH_Presentacion.Parametrizadores
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 07/02/2020
    /// Formulario parámetro de Periodos
    /// </summary>
    public partial class frmPaPeriodo : frmBaseTrxDev
    {
        #region Variables
        clsNPeriodo loLogicaNegocio;
        private bool pbCargado = false;
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmPaPeriodo()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNPeriodo();
        }

        /// <summary>
        /// Evento de inicialización del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPeriodos_Load(object sender, EventArgs e)
        {
            try
            {
                cmbAnio.Focus();

                var poListaAnio = new List<Combo>();
                Combo poItem = new Combo();
                poItem.Codigo = DateTime.Now.AddYears(+1).Year.ToString();
                poItem.Descripcion = DateTime.Now.AddYears(+1).Year.ToString();
                poListaAnio.Add(poItem);
                for (int i = 0; i < 2; i++)
                {
                    poItem = new Combo();
                    poItem.Codigo = DateTime.Now.AddYears(-i).Year.ToString();
                    poItem.Descripcion = DateTime.Now.AddYears(-i).Year.ToString();
                    poListaAnio.Add(poItem);
                }
                //poListaAnio.Insert(0, new Combo { Codigo = "0", Descripcion = Diccionario.DesSeleccione });

                clsComun.gLLenarCombo(ref cmbAnio, poListaAnio);
                clsComun.gLLenarCombo(ref cmbTipoRol, loLogicaNegocio.goConsultarComboTipoRol(), true);
                
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
                dgvDatos.PostEditor();
                List<Periodo> poDatos = (List<Periodo>)bsDatos.DataSource;
                if (poDatos.Count > 0)
                {
                    if (loLogicaNegocio.gbGuardar(poDatos, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal))
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No Existen datos a guardar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                lBuscar(true);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Generar, Genera Periodos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbEsValido())
                {
                    List<Periodo> poDatos = (List<Periodo>)bsDatos.DataSource;
                    DateTime pdFecha = new DateTime(int.Parse(cmbAnio.EditValue.ToString()), 1, 1);
                    DateTime pdFechaInicio = new DateTime();
                    DateTime pdFechaFin = new DateTime();

                    List<Periodo> poLista = new List<Periodo>();
                    bool pbDecimo = false;
                    string psCodigo = string.Empty;
                    if (poDatos != null) poLista = poDatos;
                    string psCodigoPeriodo = loLogicaNegocio.gsConsultaDatosTipoRol(cmbTipoRol.EditValue.ToString());
                    int piDiaCorteComi = new clsNParametro().goBuscarEntidad().DiaCorteHorasExtras??0;
                    for (int i = 0; i < 12; i++)
                    {
                        if (cmbTipoRol.Text.ToString().ToUpper().Contains("SEM"))
                        {
                            pdFechaInicio = pdFecha.AddMonths(i);
                            pdFechaFin = pdFecha.AddMonths(i).AddDays(7);
                            psCodigo = string.Format("{0}{1}", psCodigoPeriodo, pdFechaFin.ToString("yyyyMM"));
                            poLista.Add(new Periodo() { Anio = int.Parse(cmbAnio.EditValue.ToString()), Codigo = psCodigo, CodigoTipoRol = cmbTipoRol.EditValue.ToString(), FechaInicio = pdFechaInicio, FechaFin = pdFechaFin, TipoRol = cmbTipoRol.Text });
                        }
                        else if (cmbTipoRol.Text.ToString().ToUpper().Contains("PRI") || cmbTipoRol.Text.ToString().ToUpper().Contains("ANT"))
                        {
                            pdFechaInicio = pdFecha.AddMonths(i);
                            pdFechaFin = pdFecha.AddMonths(i).AddDays(14);
                            psCodigo = string.Format("{0}{1}", psCodigoPeriodo, pdFechaFin.ToString("yyyyMM"));
                            poLista.Add(new Periodo() { Anio = int.Parse(cmbAnio.EditValue.ToString()), Codigo = psCodigo, CodigoTipoRol = cmbTipoRol.EditValue.ToString(), FechaInicio = pdFechaInicio, FechaFin = pdFechaFin, TipoRol = cmbTipoRol.Text });
                        }
                        else if (cmbTipoRol.Text.ToString().ToUpper().Contains("SEG") || cmbTipoRol.Text.ToString().ToUpper().Contains("JUB"))
                        {
                            pdFechaInicio = pdFecha.AddMonths(i);
                            var FechaIniComi = new DateTime(pdFechaInicio.AddMonths(-1).Year, pdFechaInicio.AddMonths(-1).Month, piDiaCorteComi + 1);
                            var FechaIniFin = new DateTime(pdFechaInicio.Year, pdFechaInicio.Month, piDiaCorteComi);
                            pdFechaFin = pdFecha.AddMonths(i + 1).AddDays(-1);
                            psCodigo = string.Format("{0}{1}", psCodigoPeriodo, pdFechaFin.ToString("yyyyMM"));
                            poLista.Add(new Periodo() { Anio = int.Parse(cmbAnio.EditValue.ToString()), Codigo = psCodigo, CodigoTipoRol = cmbTipoRol.EditValue.ToString(), FechaInicio = pdFechaInicio, FechaFin = pdFechaFin, TipoRol = cmbTipoRol.Text, FechaInicioComi = FechaIniComi, FechaFinComi = FechaIniFin, FechaInicioHorasExtras = FechaIniComi, FechaFinHorasExtras = FechaIniFin });
                        }
                        else if (cmbTipoRol.Text.ToString().ToUpper().Contains("CUART") && !pbDecimo)
                        {
                            pdFechaInicio = new DateTime(pdFecha.AddYears(-1).Year, 3, 1);
                            pdFechaFin = pdFechaInicio.AddYears(1).AddDays(-1);
                            psCodigo = string.Format("{0}{1}", psCodigoPeriodo, pdFechaFin.AddDays(1).ToString("yyyyMM"));
                            poLista.Add(new Periodo() { Anio = int.Parse(cmbAnio.EditValue.ToString()), Codigo = psCodigo, CodigoTipoRol = cmbTipoRol.EditValue.ToString(), FechaInicio = pdFechaInicio, FechaFin = pdFechaFin, TipoRol = cmbTipoRol.Text });

                            DateTime pdFechaInicioSierra = new DateTime(pdFecha.AddYears(-1).Year, 8, 1);
                            DateTime pdFechaFinSierra = pdFechaInicioSierra.AddYears(1).AddDays(-1);
                            psCodigo = string.Format("{0}{1}", psCodigoPeriodo, pdFechaFinSierra.AddDays(1).ToString("yyyyMM"));
                            poLista.Add(new Periodo() { Anio = int.Parse(cmbAnio.EditValue.ToString()), Codigo = psCodigo, CodigoTipoRol = cmbTipoRol.EditValue.ToString(), FechaInicio = pdFechaInicioSierra, FechaFin = pdFechaFinSierra, TipoRol = cmbTipoRol.Text });
                            pbDecimo = true;
                        }
                        else if (cmbTipoRol.Text.ToString().Contains("TERCERO") && !pbDecimo)
                        {
                            pdFechaInicio = new DateTime(pdFecha.AddYears(-1).Year, 12, 1);
                            pdFechaFin = pdFechaInicio.AddYears(1).AddDays(-1);
                            psCodigo = string.Format("{0}{1}", psCodigoPeriodo, pdFechaFin.AddDays(1).ToString("yyyyMM"));
                            poLista.Add(new Periodo() { Anio = int.Parse(cmbAnio.EditValue.ToString()), Codigo = psCodigo, CodigoTipoRol = cmbTipoRol.EditValue.ToString(), FechaInicio = pdFechaInicio, FechaFin = pdFechaFin, TipoRol = cmbTipoRol.Text });
                            pbDecimo = true;
                        }
                        else if (cmbTipoRol.Text.ToString().Contains("COMISI") && !pbDecimo)
                        {
                            pdFechaInicio = pdFecha.AddMonths(i);
                            var FechaIniComi = new DateTime(pdFechaInicio.AddMonths(-1).Year, pdFechaInicio.AddMonths(-1).Month, piDiaCorteComi + 1);
                            var FechaIniFin = new DateTime(pdFechaInicio.Year, pdFechaInicio.Month, piDiaCorteComi);
                            pdFechaFin = pdFecha.AddMonths(i + 1).AddDays(-1);
                            psCodigo = string.Format("{0}{1}", psCodigoPeriodo, pdFechaFin.ToString("yyyyMM"));
                            //psCodigo = string.Format("{0}{1}", psCodigoPeriodo, pdFechaFin.AddDays(1).ToString("yyyyMM"));
                            poLista.Add(new Periodo() { Anio = int.Parse(cmbAnio.EditValue.ToString()), Codigo = psCodigo, CodigoTipoRol = cmbTipoRol.EditValue.ToString(), FechaInicio = pdFechaInicio, FechaFin = pdFechaFin, TipoRol = cmbTipoRol.Text, FechaInicioComi = FechaIniComi, FechaFinComi = FechaIniFin });
                        }
                        else if (cmbTipoRol.Text.ToString().Contains("UTILID") && !pbDecimo)
                        {
                            pdFechaInicio = new DateTime(pdFecha.AddYears(-1).Year, 4, 1);
                            pdFechaFin = pdFechaInicio.AddYears(1).AddDays(-1);
                            psCodigo = string.Format("{0}{1}", psCodigoPeriodo, pdFechaFin.AddDays(1).ToString("yyyyMM"));
                            poLista.Add(new Periodo() { Anio = int.Parse(cmbAnio.EditValue.ToString()), Codigo = psCodigo, CodigoTipoRol = cmbTipoRol.EditValue.ToString(), FechaInicio = pdFechaInicio, FechaFin = pdFechaFin, TipoRol = cmbTipoRol.Text });
                            pbDecimo = true;
                        }
                    }

                    bsDatos.DataSource = poLista.OrderBy(x => x.FechaFin).ToList();
                    dgvDatos.Columns["IdPeriodo"].Visible = false;
                    dgvDatos.Columns["CodigoTipoRol"].Visible = false;
                    dgvDatos.Columns["Anio"].Visible = false;
                    dgvDatos.Columns["FechaInicioComi"].Visible = false;
                    dgvDatos.Columns["FechaFinComi"].Visible = false;
                    dgvDatos.Columns["FechaInicioHorasExtras"].Visible = false;
                    dgvDatos.Columns["FechaFinHorasExtras"].Visible = false;
                    dgvDatos.Columns["CodigoEstadoNomina"].Visible = false;
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento al cambiar valor en el combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAnio_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    bsDatos.DataSource = null;
                    if (cmbAnio.EditValue.ToString() != "0")
                    {
                        bsDatos.DataSource = loLogicaNegocio.gsConsultaPeriodos(int.Parse(cmbAnio.EditValue.ToString()));
                        dgvDatos.Columns["IdPeriodo"].Visible = false;
                        dgvDatos.Columns["CodigoTipoRol"].Visible = false;
                        dgvDatos.Columns["Anio"].Visible = false;
                        dgvDatos.Columns["FechaInicioComi"].Visible = false;
                        dgvDatos.Columns["FechaFinComi"].Visible = false;
                        dgvDatos.Columns["FechaInicioHorasExtras"].Visible = false;
                        dgvDatos.Columns["FechaFinHorasExtras"].Visible = false;
                        dgvDatos.Columns["CodigoEstadoNomina"].Visible = false;
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
        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGenerar"] != null) tstBotones.Items["btnGenerar"].Click += btnGenerar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
        }

        private bool lbEsValido()
        {
            if (cmbAnio.EditValue.ToString() == Diccionario.Seleccione)
            {
                lMensajeSeleccioneAnio();
                return false;
            }

            if (cmbTipoRol.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Tipo Rol.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            List<Periodo> poDatos = (List<Periodo>)bsDatos.DataSource;
            if (poDatos != null && poDatos.Count > 0)
            {
                int piCont = poDatos.Where(x => x.FechaInicio.Year == int.Parse(cmbAnio.EditValue.ToString()) && x.TipoRol == cmbTipoRol.Text).Count();
                if (piCont > 0)
                {
                    XtraMessageBox.Show(string.Format("Ya existe para el año {0} y el tipo de rol {1} periodos generados.", cmbAnio.EditValue.ToString(), cmbTipoRol.Text), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        private void lLimpiar()
        {
            if ((cmbTipoRol.Properties.DataSource as IList).Count > 0) cmbTipoRol.ItemIndex = 0;
            if ((cmbAnio.Properties.DataSource as IList).Count > 0) cmbAnio.ItemIndex = 0;
            bsDatos.DataSource = null;
        }

        private void lBuscar(bool tbBotonBuscar = false)
        {
            bsDatos.DataSource = null;
            if (cmbAnio.EditValue.ToString() != "0")
            {
                bsDatos.DataSource = loLogicaNegocio.gsConsultaPeriodos(int.Parse(cmbAnio.EditValue.ToString()));
                dgvDatos.Columns["IdPeriodo"].Visible = false;
                dgvDatos.Columns["CodigoTipoRol"].Visible = false;
                dgvDatos.Columns["Anio"].Visible = false;
                dgvDatos.Columns["FechaInicioComi"].Visible = false;
                dgvDatos.Columns["FechaFinComi"].Visible = false;
                dgvDatos.Columns["CodigoEstadoNomina"].Visible = false;
                dgvDatos.Columns["FechaInicioHorasExtras"].Visible = false;
                dgvDatos.Columns["FechaFinHorasExtras"].Visible = false;
            }
            else
            {
                if (tbBotonBuscar) lMensajeSeleccioneAnio();
            }
        }

        private void lMensajeSeleccioneAnio()
        {
            XtraMessageBox.Show("Seleccione Año.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }
}
