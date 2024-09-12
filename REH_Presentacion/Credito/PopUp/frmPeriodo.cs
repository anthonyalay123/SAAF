using DevExpress.XtraEditors;
using GEN_Entidad.Entidades.Credito;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Credito.PopUp
{
    public partial class frmPeriodo : Form
    {
        public bool pbAcepto = false;
        public int tIdCheckList = 0;
        public ProcesoCreditoDetalle poDetalle = new ProcesoCreditoDetalle();
        public string lsNombre = "";
        public string lsTipoProceso = "";
        public bool lbRevision = false;
        public bool lbCerrado = false;

        public frmPeriodo()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            pbAcepto = false;
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                string psMsg = EsValido();
                if (string.IsNullOrEmpty(psMsg))
                {
                    if (string.IsNullOrEmpty(txtPeriodo.Text))
                    {
                        poDetalle.Periodo = null;
                    }
                    else
                    {
                        poDetalle.Periodo = int.Parse(txtPeriodo.Text);
                    }
                    pbAcepto = true;
                    Close();
                }
                else
                {
                    XtraMessageBox.Show(psMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string EsValido()
        {
            string psResult = "";
            if (!string.IsNullOrEmpty(txtPeriodo.Text))
            {
                if (tIdCheckList == 7)
                {
                    if (string.IsNullOrEmpty(txtPeriodo.Text))
                    {
                        psResult = string.Format("{0}Ingresar Periodo\n", psResult);
                    }
                    if (txtPeriodo.Text != (DateTime.Now.Year - 1).ToString())
                    {
                        XtraMessageBox.Show("El periodo declarado no esta vigente, revisar y adjuntar la última declaración.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
            if (tIdCheckList == 10)
                {
                    if (string.IsNullOrEmpty(txtPeriodo.Text))
                    {
                        psResult = string.Format("{0}Ingresar Periodo\n", psResult);
                    }

                    if (lsTipoProceso == "NUE")
                    {
                        if (txtPeriodo.Text != (DateTime.Now.Year).ToString())
                        {
                            psResult = string.Format("{0}Para clientes nuevos debe ser del año en curso.\n", psResult);
                            //XtraMessageBox.Show("El periodo declarado no esta vigente, revisar y adjuntar la última declaración.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        if (txtPeriodo.Text != (DateTime.Now.Year).ToString() && txtPeriodo.Text != (DateTime.Now.Year - 1).ToString())
                        {
                            psResult = string.Format("{0}No es posible continuar, Periodos vigentes: {1} y {2}.\n", psResult, (DateTime.Now.Year - 1), (DateTime.Now.Year));
                        }
                    }
                }
            }


            return psResult;

        }

        private void frmFechaVigencia_Load(object sender, EventArgs e)
        {
            try
            {
                if (poDetalle.Periodo != null)
                {
                    txtPeriodo.EditValue = poDetalle.Periodo;
                }

                if (!string.IsNullOrEmpty(lsNombre))
                {
                    lblNombre.Text = lsNombre;
                }

                if (lbCerrado)
                {
                    btnAceptar.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPeriodo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
