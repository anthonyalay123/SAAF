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
    /// Fecha: 27/02/2020
    /// Formulario parámetro de Periodos
    /// </summary>
    public partial class frmPaRubroTipoRol : frmBaseTrxDev
    {

        #region Variables
        clsNRubroTipoRol loLogicaNegocio;
        private bool pbCargado = false;
        #endregion

        public frmPaRubroTipoRol()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNRubroTipoRol();
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
                cmbTipoRol.Focus();
                clsComun.gLLenarCombo(ref cmbTipoRol, loLogicaNegocio.goConsultarComboTipoRol(), true);

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
                dgvDatos.PostEditor();
                if (lbEsValido())
                {
                    List<RubroTipoRol> poDatos = (List<RubroTipoRol>)bsDatos.DataSource;

                    if (poDatos.Count > 0)
                    {
                        if (loLogicaNegocio.gbGuardar(poDatos,cmbTipoRol.EditValue.ToString(), clsPrincipal.gsUsuario, clsPrincipal.gsTerminal))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lBuscar();
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
        /// Carga rubros tipos rol por tipo de rol seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTipoRol_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lBuscar();
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
        }
        private bool lbEsValido()
        {
            
            if (cmbTipoRol.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Tipo Rol.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            List<RubroTipoRol> poDatos = (List<RubroTipoRol>)bsDatos.DataSource;
            if (poDatos != null && poDatos.Count == 0)
            {
                XtraMessageBox.Show("Seleccione Rubros.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            string psMensaje = string.Empty;
            var poRubros = loLogicaNegocio.goConsultaRubro();
            var psRubros = new List<string>();

            foreach(var item in poDatos.Where(x=>x.Aplica).OrderBy(x=>x.Orden).ToList())
            {
                var poRubro = poRubros.Where(x => x.Codigo == item.CodigoRubro).FirstOrDefault();
                var psRubroArray = poRubro.Formula.Split('{');

                foreach(var fila in psRubroArray)
                {

                    if(fila.Length > 3)
                    {
                        string psRubrotmp = fila.Substring(0, 3);

                        if (!psRubros.Contains(psRubrotmp))
                        {
                            psMensaje += "- Código de Rubro: " + poRubro.Codigo + ", Depende del Código del Rubro: " + psRubrotmp + " Verifique que esté seleccionado y ordenado correctamente. \n";
                        }
                    }
                }
                psRubros.Add(poRubro.Codigo);
            }

            if (!string.IsNullOrEmpty(psMensaje))
            {
                XtraMessageBox.Show(psMensaje, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void lLimpiar()
        {
            if ((cmbTipoRol.Properties.DataSource as IList).Count > 0) cmbTipoRol.ItemIndex = 0;
            bsDatos.DataSource = loLogicaNegocio.goConsultaRubros();
            cmbTipoRol.Focus();
        }

        private void lBuscar(bool tbBotonBuscar = false)
        {
            if (cmbTipoRol.EditValue.ToString() != "0")
            {
                string psValor = cmbTipoRol.EditValue.ToString();
                pbCargado = false;
                lLimpiar();
                cmbTipoRol.EditValue = psValor;
                var poDatosTipoRubro = loLogicaNegocio.goConsultaRubroTipoRol(cmbTipoRol.EditValue.ToString());
                pbCargado = true;
                List<RubroTipoRol> poDatos = (List<RubroTipoRol>)bsDatos.DataSource;
                poDatos.ForEach(x => x.Aplica = false);
                foreach (var poItem in poDatosTipoRubro)
                {
                    var poRegistro = poDatos.Where(x => x.CodigoRubro == poItem.CodigoRubro).FirstOrDefault();
                    if (poRegistro != null)
                    {
                        poRegistro.Aplica = true;
                        poRegistro.Orden = poItem.Orden;
                    }
                }
                bsDatos.DataSource = poDatos.ToList();

            }
        }
        
        #endregion
        
    }
}
