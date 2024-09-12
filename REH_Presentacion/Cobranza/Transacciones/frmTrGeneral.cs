using COB_Negocio;
using DevExpress.XtraEditors;
using GEN_Entidad;
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

namespace REH_Presentacion.Cobranza.Transacciones
{
    /// <summary>
    /// Formulario para proceso de comisiones
    /// 19/07/2022
    /// Victor Arévalo
    /// </summary>
    public partial class frmTrGeneral : frmBaseTrxVerDev
    {

        #region Variables
        BindingSource bsDatos = new BindingSource();
        clsNComisiones loLogicaNegocio = new clsNComisiones();
        #endregion

        #region Eventos
        public frmTrGeneral()
        {
            InitializeComponent();
        }

        private void frmGeneral_Load(object sender, EventArgs e)
        {
            
            lCargarEventosBotones();
            lCargar();
        }

        /// <summary>
        /// Evento del botón Agregar, Agrega un nuevo periodo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                List<Comisiones> poDatos = (List<Comisiones>)bsDatos.DataSource;
                List<Comisiones> poLista = new List<Comisiones>();
                if (poDatos != null) poLista = poDatos;
                string psMensaje;
                List<int> piPeriodosEnGird = poDatos.Select(x => x.IdPeriodo).ToList();
                int pIdPeriodo = loLogicaNegocio.gIdPeriodoSiguienteComisiones(out psMensaje, piPeriodosEnGird, Diccionario.Tablas.TipoRol.FinMes);
                if (string.IsNullOrEmpty(psMensaje))
                {
                    Periodo poPeriodo = loLogicaNegocio.goConsultarPeriodo(pIdPeriodo);
                    if (poPeriodo != null)
                    {
                        poLista.Add(new Comisiones()
                        {
                            IdPeriodo = poPeriodo.IdPeriodo,
                            CodigoTipoRol = poPeriodo.CodigoTipoRol,
                            CodigoPeriodo = poPeriodo.Codigo,
                            DesTipoRol = poPeriodo.TipoRol,
                            FechaCreacion = DateTime.Now.Date,
                            FechaInicio = poPeriodo.FechaInicioComi??DateTime.Now,
                            FechaFin = poPeriodo.FechaFinComi ?? DateTime.Now,
                        });

                        bsDatos.DataSource = poLista.ToList();
                    }
                    else
                    {
                        XtraMessageBox.Show("No se encontró el periodo siguiente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    XtraMessageBox.Show(psMensaje, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Agregar, Agrega un nuevo periodo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<Comisiones>)bsDatos.DataSource;
                int pIdPeriodo = poLista[piIndex].IdPeriodo;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;
                if (pIdPeriodo > 0)
                {
                    var poForm = loLogicaNegocio.goConsultarMenu(Diccionario.Tablas.Menu.frmTrConsultarComisiones);
                    if (poForm != null)
                    {
                        frmTrConsultarComisiones poFrm = new frmTrConsultarComisiones();
                        poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                        poFrm.Text = poForm.Nombre;
                        poFrm.lIdPeriodo = pIdPeriodo;
                        poFrm.ldtFechaInicio = poLista[piIndex].FechaInicio;
                        poFrm.ldtFechaFin = poLista[piIndex].FechaFin;
                        poFrm.ShowInTaskbar = true;
                        poFrm.MdiParent = this.ParentForm;
                        poFrm.Show();

                    }
                    else
                    {
                        XtraMessageBox.Show("No tiene permiso para realizar esta acción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                }
                else
                {
                    XtraMessageBox.Show("Periodo Seleccionado no es valido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }


        /// <summary>
        /// Evento del botón Agregar, Agrega un nuevo periodo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<Comisiones>)bsDatos.DataSource;
                int pIdPeriodo = poLista[piIndex].IdPeriodo;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;
                decimal pdcTotal = poLista[piIndex].Total;
                if (psCodigoEstado == Diccionario.Pendiente || (string.IsNullOrEmpty(psCodigoEstado) && pdcTotal == 0M))
                {
                    if (pIdPeriodo > 0)
                    {

                        DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de calcular comisiones?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {
                            loLogicaNegocio.gCalcularComisiones(pIdPeriodo, clsPrincipal.gsUsuario);
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lCargar();
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Periodo Seleccionado no es valido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("Comisiones cerradas no se puede volver a calcular.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }


        /// <summary>
        /// Evento del botón Agregar, Agrega un nuevo periodo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrador_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                if (piIndex >= 0)
                {
                    var poLista = (List<Comisiones>)bsDatos.DataSource;
                    int pIdPeriodo = poLista[piIndex].IdPeriodo;
                    string psCodigoEstado = poLista[piIndex].CodigoEstado;
                    if (pIdPeriodo > 0)
                    {
                        var poForm = loLogicaNegocio.goConsultarMenu(Diccionario.Tablas.Menu.frmTrBorradorComisiones);
                        if (poForm != null)
                        {
                            frmTrBorradorComisiones poFrm = new frmTrBorradorComisiones();
                            poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            poFrm.Text = poForm.Nombre;
                            poFrm.lIdPeriodo = pIdPeriodo;
                            poFrm.ldtFechaInicio = poLista[piIndex].FechaInicio;
                            poFrm.ldtFechaFin = poLista[piIndex].FechaFin;
                            poFrm.ShowInTaskbar = true;
                            poFrm.MdiParent = this.ParentForm;
                            poFrm.Show();

                        }
                        else
                        {
                            XtraMessageBox.Show("No tiene permiso para realizar esta acción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }


                    }
                    else
                    {
                        XtraMessageBox.Show("Periodo Seleccionado no es valido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("Selecione registro de comisión", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Evento del botón Cerrar, Cierra la Nómina no se permite Volverla a Generar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCerrarNomina_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {

                //int piIndex = dgvDatos.GetFocusedRow;                
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<Comisiones>)bsDatos.DataSource;
                int pIdPeriodo = poLista[piIndex].IdPeriodo;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;

                if (psCodigoEstado == Diccionario.Pendiente)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de cerrar Rol?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {

                        if (pIdPeriodo > 0)
                        {
                            string psMsg = loLogicaNegocio.gsCerrarBorradorComisiones(pIdPeriodo, clsPrincipal.gsUsuario);
                            if (string.IsNullOrEmpty(psMsg))
                            {
                                XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lCargar();
                            }
                            else
                            {
                                XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("No existen datos a guardar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("Nómina debe tener estado Pendiente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Evento del botón Reverso de Nómina.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReversarNomina_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {

                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<Comisiones>)bsDatos.DataSource;
                int pIdPeriodo = poLista[piIndex].IdPeriodo;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;

                if (psCodigoEstado == Diccionario.Cerrado)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de Reversar Comisiones?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {


                        string psMsg = loLogicaNegocio.gsReversarComisiones(pIdPeriodo, clsPrincipal.gsUsuario);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lCargar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("Comisiones debe tener estado Cerrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }


        /// <summary>
        /// Evento del botón Eliminar Nómina.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminarNomina_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //int piIndex = dgvDatos.FocusedRowHandle;
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<Comisiones>)bsDatos.DataSource;
                int pIdPeriodo = poLista[piIndex].IdPeriodo;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;
                decimal pdcTotal = poLista[piIndex].Total;
                if ((psCodigoEstado == Diccionario.Pendiente && pdcTotal == 0M) || (string.IsNullOrEmpty(psCodigoEstado) && pdcTotal == 0M))
                {
                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de Borrar?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //if (loLogicaNegocio.gbEliminarComisiones(pIdPeriodo, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal))
                        //{
                        //    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lCargar();
                        //}
                        //else
                        //{
                        //    XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //}
                    }
                }
                else
                {
                    XtraMessageBox.Show("No es posible eliminar comisiones generadas", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<Comisiones>)bsDatos.DataSource;
                int pIdPeriodo = poLista[piIndex].IdPeriodo;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;


                if (pIdPeriodo > 0)
                {
                    clsComun.gImprimirReportesCobranza(pIdPeriodo);
                }
                else
                {
                    XtraMessageBox.Show("Periodo Seleccinado no valido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Cursor.Current = Cursors.Default;
        }


        #endregion

        #region Métodos
        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnAgregar"] != null) tstBotones.Items["btnAgregar"].Click += btnAgregar_Click;
            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnBorrador"] != null) tstBotones.Items["btnBorrador"].Click += btnBorrador_Click;
            if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Click += btnCalcular_Click;
            if (tstBotones.Items["btnCerrarRol"] != null) tstBotones.Items["btnCerrarRol"].Click += btnCerrarNomina_Click;
            if (tstBotones.Items["btnBorrar"] != null) tstBotones.Items["btnBorrar"].Click += btnEliminarNomina_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnReversarRol"] != null) tstBotones.Items["btnReversarRol"].Click += btnReversarNomina_Click;
            

            bsDatos.DataSource = new List<Comisiones>();
            gcPrincipal.DataSource = bsDatos;

            dgvDatos.Columns["CodigoEstado"].Visible = false;
            dgvDatos.Columns["Id"].Visible = false;
            dgvDatos.Columns["CodigoTipoRol"].Visible = false;
            dgvDatos.Columns["IdPeriodo"].Visible = false;
            dgvDatos.Columns["Id"].Visible = false;

            dgvDatos.Columns["DesTipoRol"].Caption = "Tipo Rol";
            dgvDatos.Columns["DesEstado"].Caption = "Estado";

        }

        /// <summary>
        /// Carga registros de nómina
        /// </summary>
        private void lCargar()
        {
            bsDatos.DataSource = loLogicaNegocio.goConsultar();
            dgvDatos.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Total"].DisplayFormat.FormatString = "c2";
            dgvDatos.Columns["Total"].Caption = "Total Cobranza";
            dgvDatos.Columns["TotalComisiones"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["TotalComisiones"].DisplayFormat.FormatString = "c2";
        }


        #endregion

      
    }
}
