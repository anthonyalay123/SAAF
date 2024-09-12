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
using DevExpress.XtraEditors;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 13/05/2020
    /// Formulario para generación de Nómina 
    /// </summary>
    public partial class frmTrNomina : frmBaseTrxDev
    {

        #region Variables
        clsNNomina loLogicaNegocio;
        private bool pbCargado = false;
        #endregion

        #region Eventos
        public frmTrNomina()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNNomina();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbPeriodo, loLogicaNegocio.goConsultarComboPeriodo(), true);
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
        /// Evento del botón Generar, Genera Nómina.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbEsValido())
                {
                    //if (loLogicaNegocio.gbGenerarNomina(int.Parse(cmbPeriodo.EditValue.ToString()), clsPrincipal.gsUsuario))
                    //{
                    //    XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    lLimpiar();
                    //}
                    //else
                    //{
                    //    XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Aprobar, Aprueba Nómina.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbEsValido(false))
                {
                    if (lblEstado.Text == Diccionario.DesPendiente)
                    {
                        DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de aprobar Nómina?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {
                            if (loLogicaNegocio.gbAprobarNomina(int.Parse(cmbPeriodo.EditValue.ToString()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal))
                            {
                                XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lLimpiar();
                            }
                            else
                            {
                                XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("La Nómina debe estar en un estado de Pendiente para aprobar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        string psEstadoNomina = loLogicaNegocio.gsGetEstadoNomina(int.Parse(cmbPeriodo.EditValue.ToString()));
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
            if (tstBotones.Items["btnAprobar"] != null) tstBotones.Items["btnAprobar"].Click += btnAprobar_Click;
        }

        private bool lbEsValido(bool tbGenerar = true)
        {
            if (cmbPeriodo.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Periodo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if(tbGenerar)
            {
                string psEstadoNomina = loLogicaNegocio.gsGetEstadoNomina(int.Parse(cmbPeriodo.EditValue.ToString()));

                if (!string.IsNullOrEmpty(psEstadoNomina))
                {
                    if (psEstadoNomina == Diccionario.Activo)
                    {
                        XtraMessageBox.Show("Nómina ya generada. No es posible volver a generar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }

                    if (psEstadoNomina == Diccionario.Pendiente)
                    {
                        DialogResult dialogResult = XtraMessageBox.Show("Nómina ya generada con estado pendiente. ¿Desea sobreescribir la Nómina?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult != DialogResult.Yes)
                        {
                            return false;
                        } 
                    }
                }
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

            DataTable dt = loLogicaNegocio.gdtGetNomina(int.Parse(cmbPeriodo.EditValue.ToString()));
            
            frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
            if (pofrmBuscar.ShowDialog() == DialogResult.OK)
            {

            }
            
        }

        #endregion

    }
}
