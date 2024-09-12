using DevExpress.XtraGrid.Views.Grid;
using REH_Negocio;
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
using System.Configuration;
using DevExpress.XtraEditors.Repository;
using System.IO;
using DevExpress.XtraEditors;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 11/06/2020
    /// Formulario para generación de pagos de Nómina
    /// </summary>
    public partial class frmTrGeneraPago : frmBaseTrxDev
    {

        #region Variables
        clsNGeneraPago loLogicaNegocio;
        private bool pbCargado = false;
        public int lIdPeriodo;
        #endregion

        #region Eventos
        public frmTrGeneraPago()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNGeneraPago();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbPeriodo, loLogicaNegocio.goConsultarComboPeriodo(), true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboBanco(), "CodigoBanco");
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboFormaPago(), "CodigoFormaPago");
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboTipoCuentaBancaria(), "CodigoTipoCuenta");


                pbCargado = true;
                
                // Carga Periodo Enviado
                if(lIdPeriodo > 0)
                {
                    cmbPeriodo.EditValue = lIdPeriodo.ToString();
                    if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Visible = false;
                    if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Visible = false;

                    cmbPeriodo.Enabled = false;
                }

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
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbEsValido(false))
                {
                    lBuscar();
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Generar, Genera archivo de pago
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    List<SpGeneraPago> poLista = (List<SpGeneraPago>)bsDatos.DataSource;
                    if (poLista.Where(x => x.Seleccionado).Count() > 0)
                    {                       

                        poLista.ForEach(x => x.NombreCompleto = clsComun.gRemoverCaracteresEspeciales(x.NombreCompleto));

                        string psPath = ConfigurationManager.AppSettings["CarpetaArchivoBanco"].ToString();
                        decimal pdcValorMaximoArchivoBanco = Decimal.Parse(ConfigurationManager.AppSettings["ValorMaximoArchivoPago"]);
                        string psMensaje;
                        string psMensajeTotal = string.Empty;

                        decimal pdcAcumulador = 0;
                        bool pbEntra = false;
                        List<SpGeneraPago> poListaNew = new List<SpGeneraPago>();
                        foreach (var item in poLista.Where(x => x.Seleccionado))
                        {
                            pbEntra = false;
                            pdcAcumulador += item.Valor;
                            if (pdcAcumulador > pdcValorMaximoArchivoBanco)
                            {
                                psMensaje = string.Empty;
                                loLogicaNegocio.gbGenerar(int.Parse(cmbPeriodo.EditValue.ToString()), poListaNew, psPath, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, out psMensaje);
                                poListaNew = new List<SpGeneraPago>();
                                pdcAcumulador = 0;
                                psMensajeTotal = psMensajeTotal + psMensaje + "\n";
                                pbEntra = true;
                            }
                            if (pbEntra)
                            {
                                pdcAcumulador += item.Valor;
                                pbEntra = false;
                            }
                            poListaNew.Add(item);
                        }

                        if(!pbEntra)
                        {
                            loLogicaNegocio.gbGenerar(int.Parse(cmbPeriodo.EditValue.ToString()), poListaNew, psPath, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, out psMensaje);
                            psMensajeTotal = psMensajeTotal + psMensaje + "\n";
                        }
                        XtraMessageBox.Show(psMensajeTotal, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos seleccionados a generar", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void cmbPeriodo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    if (cmbPeriodo.EditValue.ToString() != Diccionario.Seleccione)
                    {
                        
                        string psEstadoNomina = new clsNNomina().gsGetEstadoNomina(int.Parse(cmbPeriodo.EditValue.ToString()));
                        if (psEstadoNomina == Diccionario.Activo)
                        {
                            lblEstado.Text = Diccionario.DesActivo;
                        }
                        else if (psEstadoNomina == Diccionario.Pendiente)
                        {
                            lblEstado.Text = Diccionario.DesPendiente;
                        }
                        else
                        {
                            lblEstado.Text = string.Empty;
                        }
                    }
                    else
                    {
                        lblEstado.Text = string.Empty;
                    }
                    lBuscar();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTodos_Click(object sender, EventArgs e)
        {
            try
            {
                List<SpGeneraPago> poLista = (List<SpGeneraPago>)bsDatos.DataSource;
                int piContTrue = poLista.Where(x => x.Seleccionado).Count();
                int piContFalse = poLista.Where(x => !x.Seleccionado).Count();

                if (piContTrue == 0 && piContFalse > 0)
                {
                    poLista.ForEach(x => x.Seleccionado = true);
                }
                else 
                {
                    poLista.ForEach(x => x.Seleccionado = false);
                }

                bsDatos.DataSource = poLista.ToList();
               
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
        }
        

        private bool lbEsValido(bool tbGenerar = true)
        {
            if (cmbPeriodo.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Periodo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private void lLimpiar()
        {
            if ((cmbPeriodo.Properties.DataSource as IList).Count > 0) cmbPeriodo.ItemIndex = 0;
            lblEstado.Text = string.Empty;
        }

        private void lBuscar()
        {
            gcDatos.DataSource = null;
            bsDatos.DataSource = loLogicaNegocio.gdtGeneraPago(int.Parse(cmbPeriodo.EditValue.ToString()));
            gcDatos.DataSource = bsDatos;
            dgvDatos.Columns[0].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[1].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[2].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns[4].OptionsColumn.AllowEdit = false;
            clsComun.gFormatearColumnasGrid(dgvDatos);
        }


        #endregion

        private void dgvDatos_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //try
            //{
            //    if (e.Column.FieldName == "CodigoFormaPago")
            //    {
            //        if (e.CellValue != null)
            //        {
            //            if (e.CellValue.ToString() != "TRA")
            //            {
            //                e.Appearance.BackColor = Color.FromArgb(112, 196, 95);
            //            }
            //            else
            //            {
            //                e.Appearance.BackColor = Color.Transparent;
            //            }
            //        }
            //    }
            //}

            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void dgvDatos_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                var quantity = dgvDatos.GetRowCellValue(e.RowHandle, "CodigoFormaPago");

                if (quantity != null)
                {
                    if (quantity.ToString() != "TRA")
                    {
                        e.Appearance.BackColor = Color.FromArgb(255, 255, 0);
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.Transparent;
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
