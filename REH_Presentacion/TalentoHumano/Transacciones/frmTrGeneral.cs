using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraEditors;
using System.Threading;
using System.Net.Mail;
using System.Configuration;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 03/06/2020
    /// Formulario que centraliza todos los procesos de Nómina
    /// </summary>
    public partial class frmTrGeneral : frmBaseTrxVerDev
    {

        #region Variables
        clsNNomina loLogicaNegocio;
        
        //private bool pbCargado = false;
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmTrGeneral()
        {
            InitializeComponent();
            bsDatos.DataSource = new List<Nomina>();
            loLogicaNegocio = new clsNNomina();
        }

        /// <summary>
        /// Evento de inicialización del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTrGeneral_Load(object sender, EventArgs e)
        {
            try
            {
                lCargar();
                lCargarEventosBotones();
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
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                List<Nomina> poDatos = (List<Nomina>)bsDatos.DataSource;
                List<Nomina> poLista = new List<Nomina>();
                if (poDatos != null) poLista = poDatos;
                string psMensaje;
                List<int> piPeriodosEnGird = poDatos.Where(x=>x.IdNomina == 0).Select(x => x.IdPeriodo).ToList();
                int pIdPeriodo = loLogicaNegocio.gIdPeriodoSiguiente(out psMensaje, piPeriodosEnGird);
                if (string.IsNullOrEmpty(psMensaje))
                {
                    Periodo poPeriodo = loLogicaNegocio.goConsultarPeriodo(pIdPeriodo);
                    if (poPeriodo!= null)
                    {
                        poLista.Add(new Nomina()
                        {
                            IdNomina = 0,
                            IdPeriodo = poPeriodo.IdPeriodo,
                            CodigoTipoRol = poPeriodo.CodigoTipoRol,
                            CodigoPeriodo = poPeriodo.Codigo,
                            DescripcionTipoRol = poPeriodo.TipoRol,
                            FechaIngreso = DateTime.Now.Date,
                            FechaInicio = poPeriodo.FechaInicio,
                            FechaFin = poPeriodo.FechaFin,
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
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<Nomina>)bsDatos.DataSource;
                int pIdPeriodo = poLista[piIndex].IdPeriodo;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;
                decimal pdcTotal = poLista[piIndex].Total;
                if (psCodigoEstado == Diccionario.Pendiente  || (string.IsNullOrEmpty(psCodigoEstado) && pdcTotal == 0M))
                {
                    if (pIdPeriodo > 0)
                    {
                        // Valida si el tipo de rol es comisiones
                        var psCodigoTipoRol = loLogicaNegocio.goConsultarPeriodo(pIdPeriodo).CodigoTipoRol;

                        if (psCodigoTipoRol == Diccionario.Tablas.TipoRol.Comisiones)
                        {
                            var poComision = new clsNComision().gsGetEstadoComision(pIdPeriodo);

                            if(poComision != Diccionario.Aprobado)
                            {
                                DialogResult dialogResult = XtraMessageBox.Show("No existen Comisiones Ingresadas/Aprobadas, por Cobranza, ¿Desea Continuar? ", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                                if (dialogResult != DialogResult.Yes)
                                {
                                    return;
                                }
                            }
                        }
                        
                        string psMsg = loLogicaNegocio.gsGenerarNomina(pIdPeriodo, clsPrincipal.gsUsuario);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            lbValidaNomina(pIdPeriodo);

                            string psNetoNegativo = loLogicaNegocio.gsValidaNetoRecibirNegativos(pIdPeriodo);
                            if (!string.IsNullOrEmpty(psNetoNegativo))
                            {
                                XtraMessageBox.Show(psNetoNegativo, "No es posible continuar!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            

                            lCargar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Periodo Seleccionado no es valido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("Eliminar cálculo para poder volver a recalcular", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        private bool lbValidaNomina(int tiPeriodo)
        {
            bool result = true;
            var dt = loLogicaNegocio.gdtValidaNomina(tiPeriodo);
            string psMensajeAdvertencia = "";
            string psMensajeError = "";

            if (dt != null)
            {
                foreach (DataRow item in dt.Rows)
                {
                    if (Convert.ToBoolean(item[1].ToString()))
                    {
                        result = false;
                        psMensajeError = psMensajeError + item[0].ToString();
                    }
                    else
                    {
                        psMensajeAdvertencia = psMensajeAdvertencia + item[0].ToString();
                    }

                }

            }

            if (!string.IsNullOrEmpty(psMensajeAdvertencia))
            {
                XtraMessageBox.Show(psMensajeAdvertencia, "ADVERTENCIA!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (!string.IsNullOrEmpty(psMensajeError))
            {
                XtraMessageBox.Show(psMensajeError, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        /// <summary>
        /// Evento del botón consultar Roles de Pagos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultaRoles_Click(object sender, EventArgs e)
        {
            try
            {
                var poForm = loLogicaNegocio.goConsultarMenu(Diccionario.Tablas.Menu.ConsultaRol);
                if(poForm != null)
                {
                    int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                    var poLista = (List<Nomina>)bsDatos.DataSource;
                    int pIdPeriodo = poLista[piIndex].IdPeriodo;

                    if (pIdPeriodo > 0)
                    {
                        frmTrConsultaRol poFrm = new frmTrConsultaRol();
                        poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                        poFrm.Text = poForm.Nombre;
                        poFrm.lIdPeriodo = pIdPeriodo;
                        poFrm.ShowInTaskbar = true;
                        poFrm.MdiParent = this.ParentForm;
                        poFrm.Show();

                        //poFrm.ShowDialog();

                    }
                    else
                    {
                        XtraMessageBox.Show("No existe detalle de Nómina", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No tiene permiso para realizar esta acción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }  
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                var poLista = (List<Nomina>)bsDatos.DataSource;
                int pIdPeriodo = poLista[piIndex].IdPeriodo;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;

                if (psCodigoEstado == Diccionario.Pendiente)
                {
                    //string psNetoNegativo = loLogicaNegocio.gsValidaNetoRecibirNegativos(pIdPeriodo);
                    //if (!string.IsNullOrEmpty(psNetoNegativo))
                    //{
                    //    XtraMessageBox.Show(psNetoNegativo, "No es posible continuar!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    return;
                    //}

                    if (lbValidaNomina(pIdPeriodo))
                    {
                        DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de cerrar Nómina?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {
                            string psMsg = loLogicaNegocio.gsCerrarNomina(pIdPeriodo, clsPrincipal.gsUsuario);

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
        /// Evento del botón Novedades, Para guardar Novedades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNovedades_Click(object sender, EventArgs e)
        {
            try
            {
                var poForm = loLogicaNegocio.goConsultarMenu(Diccionario.Tablas.Menu.Novedad);
                if (poForm != null)
                {
                    int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                    var poLista = (List<Nomina>)bsDatos.DataSource;
                    int pIdPeriodo = poLista[piIndex].IdPeriodo;
                    string psCodigoEstado = poLista[piIndex].CodigoEstado;

                    
                    if (pIdPeriodo > 0)
                    {
                        frmTrNovedad poFrm = new frmTrNovedad();
                        poFrm.lIdPeriodo = pIdPeriodo;
                        poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                        poFrm.Text = poForm.Nombre;
                        poFrm.lbNominaCerrada = psCodigoEstado == Diccionario.Cerrado ? true : false;
                        poFrm.ShowInTaskbar = true;
                        poFrm.MdiParent = this.ParentForm;
                        poFrm.Show();
                        //poFrm.ShowDialog();
                    }
                    else
                    {
                        XtraMessageBox.Show("Periodo Seleccinado no valido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                }
                else
                {
                    XtraMessageBox.Show("No tiene permiso para realizar esta acción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                

               
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                var poLista = (List<Nomina>)bsDatos.DataSource;
                int pIdPeriodo = poLista[piIndex].IdPeriodo;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;

                if (psCodigoEstado == Diccionario.Cerrado)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de Reversar Nómina?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //string psValidaReverso = loLogicaNegocio.gsValidaCerrarRol(pIdPeriodo);
                        //if (!string.IsNullOrEmpty(psValidaReverso))
                        //{
                        //    XtraMessageBox.Show(psValidaReverso, "No es posible continuar!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //    return;
                        //}

                        string psMsg = loLogicaNegocio.gsReversarNomina(pIdPeriodo, clsPrincipal.gsUsuario);
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
                    XtraMessageBox.Show("Nómina debe tener estado Cerrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Evento del botón Pago, Generá Archivo de Pagos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerarPagos_Click(object sender, EventArgs e)
        {
            try
            {
                var poForm = loLogicaNegocio.goConsultarMenu(Diccionario.Tablas.Menu.GenerarPago);
                if (poForm != null)
                {
                    int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                    var poLista = (List<Nomina>)bsDatos.DataSource;
                    int pIdPeriodo = poLista[piIndex].IdPeriodo;
                    string psCodigoEstado = poLista[piIndex].CodigoEstado;

                    if (psCodigoEstado == Diccionario.Cerrado)
                    {
                        if (pIdPeriodo > 0)
                        {

                            frmTrGeneraPago poFrm = new frmTrGeneraPago();
                            poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            poFrm.Text = poForm.Nombre;
                            poFrm.lIdPeriodo = pIdPeriodo;
                            poFrm.ShowInTaskbar = true;
                            poFrm.MdiParent = this.ParentForm;
                            poFrm.Show();
                            //poFrm.ShowDialog();

                        }
                        else
                        {
                            XtraMessageBox.Show("Periodo Seleccinado no valido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Nómina no está Cerrada, No es posible Generar Pagos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No tiene permiso para realizar esta acción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Fondo de Reserva, Actualiza datos del contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFondoReserva_Click(object sender, EventArgs e)
        {
            try
            {
                var poForm = loLogicaNegocio.goConsultarMenu(Diccionario.Tablas.Menu.FondoReserva);
                if (poForm != null)
                {
                    int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                    var poLista = (List<Nomina>)bsDatos.DataSource;
                    int pIdPeriodo = poLista[piIndex].IdPeriodo;
                    string psCodigoEstado = poLista[piIndex].CodigoEstado;

                    if (psCodigoEstado != Diccionario.Cerrado)
                    {
                        if (pIdPeriodo > 0)
                        {
                            frmTrFondoReserva poFrm = new frmTrFondoReserva();
                            poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            poFrm.Text = poForm.Nombre;
                            poFrm.lIdPeriodo = pIdPeriodo;
                            poFrm.ShowInTaskbar = true;
                            poFrm.MdiParent = this.ParentForm;
                            poFrm.Show();
                            //poFrm.ShowDialog();
                        }
                        else
                        {
                            XtraMessageBox.Show("Periodo Seleccinado no valido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Nómina está Cerrada, No es posible ingresar Fondos de Reserva", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No tiene permiso para realizar esta acción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Eliminar Cálculo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminarCalculo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {

                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<Nomina>)bsDatos.DataSource;
                int pIdPeriodo = poLista[piIndex].IdPeriodo;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;

                if (psCodigoEstado == Diccionario.Pendiente)
                {
                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de Eliminar Cálculo?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (loLogicaNegocio.gbEliminarCalculo(pIdPeriodo, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lCargar();
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var poLista = (List<Nomina>)bsDatos.DataSource;
                int pIdPeriodo = poLista[piIndex].IdPeriodo;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;
                decimal pdcTotal = poLista[piIndex].Total;
                if ((psCodigoEstado == Diccionario.Pendiente && pdcTotal == 0M) || (string.IsNullOrEmpty(psCodigoEstado) && pdcTotal == 0M))
                {
                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de Eliminar Nómina?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (loLogicaNegocio.gbEliminarNomina(pIdPeriodo, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lCargar();
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("No es posible Eliminar una Nómina Calculada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnBatchIess_Click(object sender, EventArgs e)
        {
            try
            {
                //int piIndex = dgvDatos.FocusedRowHandle;
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<Nomina>)bsDatos.DataSource;
                string psCodigoPeriodo = poLista[piIndex].CodigoPeriodo;
                string psCodigoTipoRol = poLista[piIndex].CodigoTipoRol;
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImpuestaRenta_Click(object sender, EventArgs e)
        {
            try
            {
                //int piIndex = dgvDatos.FocusedRowHandle;
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<Nomina>)bsDatos.DataSource;
                string psCodigoPeriodo = poLista[piIndex].CodigoPeriodo;
                string psCodigoTipoRol = poLista[piIndex].CodigoTipoRol;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;

                if (psCodigoTipoRol == Diccionario.Tablas.TipoRol.FinMes)
                {
                    if (psCodigoEstado == Diccionario.Pendiente)
                    {

                        DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de generar Impuesto a la Renta?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {
                            var piAnio = int.Parse(psCodigoPeriodo.Substring(1, 4));
                            var piMes = int.Parse(psCodigoPeriodo.Substring(5, 2));
                            DataTable dt = loLogicaNegocio.gdtImpuestoRenta(piAnio, piMes);
                            if (dt.Rows.Count > 0)
                            {
                                //dt.Columns.Remove("idPersona");
                                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Impuesto a la Renta" };
                                pofrmBuscar.Width = 1300;
                                pofrmBuscar.ShowDialog();

                            }
                        }

                        
                    }
                    else
                    {
                        XtraMessageBox.Show("No es posible Generar Consulta, Solo en Rol con estado PENDIENTE", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                }
                else
                {
                    XtraMessageBox.Show("No es posible Generar Consulta, Solo en Rol de FIN DE MES", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnCerrarPeriodoProvision_Click(object sender, EventArgs e)
        {
            try
            {
                //int piIndex = dgvDatos.FocusedRowHandle;
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<Nomina>)bsDatos.DataSource;
                string psCodigoPeriodo = poLista[piIndex].CodigoPeriodo;
                string psCodigoTipoRol = poLista[piIndex].CodigoTipoRol;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;
                int pIdPeriodo = poLista[piIndex].IdPeriodo;

                if (psCodigoTipoRol == Diccionario.Tablas.TipoRol.FinMes)
                {
                    if (psCodigoEstado == Diccionario.Cerrado)
                    {

                        DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de cerrar periodo para que contabilidad pueda generar el diario de provisión por liquidados?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {

                            string psMsg = loLogicaNegocio.gsCerrarPeriodoParaContabilidadEnLiquidados(pIdPeriodo, clsPrincipal.gsUsuario);

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
                        XtraMessageBox.Show("No es posible continuar, solo roles con estado CERRADO", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }

                }
                else
                {
                    XtraMessageBox.Show("No es posible continuar, esta opción solo es para el rol de FIN DE MES", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnContabilizar_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<Nomina>)bsDatos.DataSource;
                int pIdPeriodo = poLista[piIndex].IdPeriodo;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;


                if (pIdPeriodo > 0)
                {
                    clsComun.gImprimirRolIngresosEgresos(pIdPeriodo);   
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
        
        /// <summary>
        /// Evento del botón Fondo de Enviar Correo, Permite seleccionar los empleados a los que se les va a enviar el correo con su Rol de Pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnviarCorreo_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                var poForm = loLogicaNegocio.goConsultarMenu(Diccionario.Tablas.Menu.EnviarCorreoRol);
                //if (poForm != null)
                //{
                int piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                var poLista = (List<Nomina>)bsDatos.DataSource;
                int pIdPeriodo = poLista[piIndex].IdPeriodo;
                string psCodigoEstado = poLista[piIndex].CodigoEstado;

                if (psCodigoEstado == Diccionario.Cerrado)
                {
                    if (pIdPeriodo > 0)
                    {
                        DialogResult dialogResult = XtraMessageBox.Show("Se enviará el rol de pago a cada colaborador, ¿Desea Continuar? ", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {
                            
                            var dt1 = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC COBSPCONSULTACALCULOCOMISIONESDETALLE {0}", pIdPeriodo));

                            var poListaComi = new List<DetalleComisiones>();
                            foreach (DataRow item in dt1.Rows)
                            {
                                var poDet = new DetalleComisiones();
                                poDet.Id = Convert.ToInt32(item["Id"].ToString());
                                poDet.Empleado = item["Empleado"].ToString();
                                poDet.Zona = item["Zona"].ToString();
                                poDet.CodCliente = item["CodCliente"].ToString();
                                poDet.Cliente = item["Cliente"].ToString();
                                poDet.Facturador = item["Titular"].ToString();
                                poDet.CodComision = item["CodComision"].ToString();
                                poDet.Factura = item["Factura"].ToString();
                                poDet.Vendedor = item["Vendedor"].ToString();
                                poDet.FechaEmisiónFact = Convert.ToDateTime(item["FechaEmision"].ToString());
                                poDet.FechaVenceFact = Convert.ToDateTime(item["FechaVencimiento"].ToString());
                                poDet.DiasCrédito = Convert.ToInt32(item["DiasDocumento"].ToString());
                                poDet.NumDocPagoEnSAP = Convert.ToInt32(item["NumDocPago"].ToString());
                                poDet.FechaRegistroPago = Convert.ToDateTime(item["FechaContabilizacion"].ToString());
                                poDet.FechaEfectivaPago = Convert.ToDateTime(item["FechaEfectiva"].ToString());
                                poDet.DiasPago = Convert.ToInt32(item["DiasPago"].ToString());
                                poDet.Banco = item["Banco"].ToString();
                                poDet.ValorPago = Convert.ToDecimal(item["ValorTotal"].ToString());
                                poDet.PorcComisión = Convert.ToDecimal(item["% Comisión"].ToString());
                                poDet.ValorComisión = Convert.ToDecimal(item["Comisión"].ToString());

                                poListaComi.Add(poDet);
                            }
                            
                            Cursor.Current = Cursors.WaitCursor;
                            var dt = loLogicaNegocio.gdtEnvioCorreoRol(pIdPeriodo);
                            var poListaNomEmp = loLogicaNegocio.goConsultarNominaempleado(pIdPeriodo);
                            int pIdNomina = poLista[piIndex].IdNomina;
                            
                            foreach (DataRow item in dt.Rows)
                            {
                                
                                string psRuta = "";
                                List<Attachment> listAdjuntosEmail = null;
                                var poListaDet = poListaComi.Where(x => x.Id == int.Parse(item[0].ToString())).ToList();
                                if (poListaDet.Count > 0)
                                {
                                    listAdjuntosEmail = new List<Attachment>();
                                    psRuta = ConfigurationManager.AppSettings["FileTmpCom"].ToString() + "DetalleComisiones.xlsx";

                                    
                                    try
                                    {
                                        BindingSource bs = new BindingSource();
                                        bs.DataSource = poListaDet;
                                        gcExport.DataSource = bs;
                                        dgvExport.Columns["Id"].Visible = false;
                                        dgvExport.BestFitColumns();
                                        dgvExport.OptionsView.BestFitMode = DevExpress.XtraGrid.Views.Grid.GridBestFitMode.Full;
                                        // Exportar Datos
                                        gcExport.ExportToXlsx(psRuta);
                                    }
                                    catch (Exception ex)
                                    {
                                        
                                    }
                                    
                                    if (File.Exists(psRuta))
                                        listAdjuntosEmail.Add(new Attachment(psRuta));
                                }
                                

                                int pIdNominaEmpleado = poListaNomEmp.Where(x => x.IdPersona == int.Parse(item[0].ToString())).Select(x => x.IdBiometrico).FirstOrDefault()??0;
                                //clsComun.EnviarPorCorreo(item[2].ToString(), "Notificación Rol de Pago", item[1].ToString());
                                if (!string.IsNullOrEmpty(item[2].ToString()))
                                {
                                    //clsComun.EnviarPorCorreo(item[2].ToString(), "Notificación Rol de Pago", item[1].ToString(), null, false, "", "", "", "", pIdNominaEmpleado);
                                    clsComun.EnviarPorCorreo(item[2].ToString(), "Notificación Rol de Pago", item[1].ToString(), listAdjuntosEmail, false, "", "", "", "", pIdNominaEmpleado);
                                    if (File.Exists(psRuta)) File.Delete(psRuta);
                                }
                                
                                //Thread.Sleep(5 * 1000);
                            }
                            Cursor.Current = Cursors.Default;
                            XtraMessageBox.Show("Ejecutado exitosamente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                       
                    }
                    else
                    {
                        XtraMessageBox.Show("Periodo Seleccinado no valido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("Nómina NO está Cerrada, No es posible Enviar Correos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //}
                //else
                //{
                //    XtraMessageBox.Show("No tiene permiso para realizar esta acción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                
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
            if (tstBotones.Items["btnCalcular"] != null) tstBotones.Items["btnCalcular"].Click += btnCalcular_Click;
            if (tstBotones.Items["btnConsultarRol"] != null) tstBotones.Items["btnConsultarRol"].Click += btnConsultaRoles_Click;
            if (tstBotones.Items["btnCerrarRol"] != null) tstBotones.Items["btnCerrarRol"].Click += btnCerrarNomina_Click;
            if (tstBotones.Items["btnReversarRol"] != null) tstBotones.Items["btnReversarRol"].Click += btnReversarNomina_Click;
            if (tstBotones.Items["btnNovedades"] != null) tstBotones.Items["btnNovedades"].Click += btnNovedades_Click;
            if (tstBotones.Items["btnGenerarPagos"] != null) tstBotones.Items["btnGenerarPagos"].Click += btnGenerarPagos_Click;
            if (tstBotones.Items["btnEliminarCalculo"] != null) tstBotones.Items["btnEliminarCalculo"].Click += btnEliminarCalculo_Click;
            if (tstBotones.Items["btnBorrar"] != null) tstBotones.Items["btnBorrar"].Click += btnEliminarNomina_Click;
            if (tstBotones.Items["btnFondoReserva"] != null) tstBotones.Items["btnFondoReserva"].Click += btnFondoReserva_Click;
            if (tstBotones.Items["btnBatchIess"] != null) tstBotones.Items["btnBatchIess"].Click += btnBatchIess_Click;
            if (tstBotones.Items["btnContabilizar"] != null) tstBotones.Items["btnContabilizar"].Click += btnContabilizar_Click;
            if (tstBotones.Items["btnEnviarCorreo"] != null) tstBotones.Items["btnEnviarCorreo"].Click += btnEnviarCorreo_Click;
            if (tstBotones.Items["btnImprimir"] != null) tstBotones.Items["btnImprimir"].Click += btnImprimir_Click;
            if (tstBotones.Items["btnImpuestaRenta"] != null) tstBotones.Items["btnImpuestaRenta"].Click += btnImpuestaRenta_Click;
            if (tstBotones.Items["btnCerrarPeriodoProvision"] != null) tstBotones.Items["btnCerrarPeriodoProvision"].Click += btnCerrarPeriodoProvision_Click;

        }

        /// <summary>
        /// Carga registros de nómina
        /// </summary>
        private void lCargar()
        {
            bsDatos.DataSource = loLogicaNegocio.goConsultarNomina();
            dgvDatos.Columns["Total"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dgvDatos.Columns["Total"].DisplayFormat.FormatString = "c2";
            dgvDatos.Columns["DescripcionTipoRol"].Width = 150;
        }

        #endregion


    }
}
