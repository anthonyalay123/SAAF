using DevExpress.XtraEditors;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using REH_Negocio.Seguridad;
using REH_Presentacion.Comun;
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

namespace REH_Presentacion.Seguridad
{
    public partial class frmPaEnvioCorreo : frmBaseTrxDev
    {
        clsNEnvioCorreo loLogicaNegocio = new clsNEnvioCorreo();

        public frmPaEnvioCorreo()
        {
            InitializeComponent();
          
        }

        private void frmEnvioCorreo_Load(object sender, EventArgs e)
        {
            clsComun.gLLenarCombo(ref cmbEstado, loLogicaNegocio.goConsultarComboEstadoRegistro());
            lCargarEventosBotones();
            cmbEstado.EditValue = 1;
        }
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


        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                EnvioCorreo poObject = new EnvioCorreo();


                poObject.Codigo = txtCodigo.Text.ToString();
                poObject.Asunto = txtAsunto.Text.ToString();
                poObject.CCUsuario = chbCCUsuario.Checked;
                poObject.Cuerpo = txtCuerpo.Text.ToString();
                poObject.CodigoEstado = cmbEstado.EditValue.ToString();
                poObject.Transaccion = txtTransaccion.Text.ToString();
                poObject.Usuario = clsPrincipal.gsUsuario;
                poObject.Terminal = string.Empty;
                poObject.Fecha = DateTime.Now;
                poObject.CCCorreo = txtCCCorreo.Text.ToString();

                string psMsg = loLogicaNegocio.gsGuardar(poObject);

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
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
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
                List<EnvioCorreo> poListaObject = loLogicaNegocio.goListarMaestro(txtCodigo.Text.Trim());
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Código"),
                                    new DataColumn("Descripción"),
                                    new DataColumn("Estado")
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Código"] = a.Codigo;
                    row["Estado"] = Diccionario.gsGetDescripcion(a.CodigoEstado);
                    row["Descripción"] = a.Asunto;

                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Registros" };
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    txtCodigo.Text = pofrmBuscar.lsCodigoSeleccionado;
                    lConsultar();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lConsultar()
        {
            if (!string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                var poObject = loLogicaNegocio.goBuscarMaestro(txtCodigo.Text.Trim());
                if (poObject != null)
                {
                    txtCodigo.Text = poObject.Codigo;
                    txtAsunto.Text = poObject.Asunto;
                    cmbEstado.EditValue = poObject.CodigoEstado;
                    txtTransaccion.Text = poObject.Transaccion;
                    txtCuerpo.Text = poObject.Cuerpo;
                    txtCCCorreo.Text = poObject.CCCorreo;
                    chbCCUsuario.Checked = poObject.CCUsuario;
                }
            }
        }

        private void lLimpiar()
        {
            txtCuerpo.Text = string.Empty;
            txtCodigo.Text = string.Empty;
            txtAsunto.Text = string.Empty;
            txtTransaccion.Text = string.Empty;
            txtCCCorreo.Text = string.Empty;
            cmbEstado.EditValue = 1;
            chbCCUsuario.EditValue = false;

        }

        
    }
}
