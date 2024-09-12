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
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraEditors;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 13/05/2020
    /// Formulario para generación de Novedades de Nómina 
    /// </summary>
    public partial class frmTrFondoReserva : frmBaseTrxDev
    {

        #region Variables
        clsNNomina loLogicaNegocio;
        private bool pbCargado = false;
        public int lIdPeriodo;
        #endregion

        #region Eventos
        public frmTrFondoReserva()
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
        /// Evento del botón Generar, Genera Novedad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    List<FondoReserva> poLista = (List<FondoReserva>)bsDatos.DataSource; 
                    if (poLista.Count > 0)
                    {
                        if (loLogicaNegocio.gGuardaFondoReserva(int.Parse(cmbPeriodo.EditValue.ToString()), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, poLista))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen Novedades Editables", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Evento del botón Importar, Carga Datos en formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                List<FondoReserva> poLista = new List<FondoReserva>();

                OpenFileDialog ofdRuta = new OpenFileDialog();
                ofdRuta.Title = "Seleccione Archivo";
                ofdRuta.Filter = "Files(*.csv;)|*.csv;";

                if (ofdRuta.ShowDialog() == DialogResult.OK)
                {
                    if (!ofdRuta.FileName.Equals(""))
                    {
                        DataTable dt = clsComun.ConvertCSVtoDataTable(ofdRuta.FileName);

                        foreach(DataRow item in dt.Rows)
                        {
                            FondoReserva poItem = new FondoReserva();
                            poItem.IdPeriodo = int.Parse(cmbPeriodo.EditValue.ToString());
                            poItem.Cedula = item[0].ToString().Trim();
                            poItem.Nombre = item[1].ToString().Trim();
                            poItem.TieneDerecho = item[6].ToString().Trim();
                            poItem.TieneSolicitud = item[3].ToString().Trim();
                            poItem.CodigoEstado = Diccionario.Pendiente;
                            poItem.DescEstado = Diccionario.DesPendiente;
                            poLista.Add(poItem);
                        }

                        bsDatos.DataSource = poLista.ToList();
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
                    lBuscar();
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
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Click += btnImportar_Click;

            bsDatos.DataSource = new List<FondoReserva>();
            gcDatos.DataSource = bsDatos;
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
            bsDatos.DataSource = loLogicaNegocio.gConsultarFondoReserva(int.Parse(cmbPeriodo.EditValue.ToString()));
        }
        
        #endregion

    }
}
