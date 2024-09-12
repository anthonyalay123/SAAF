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
    public partial class frmMontoPagare : Form
    {
        public bool pbAcepto = false;
        public int tIdCheckList = 0;
        public decimal ldcCupoSolicitado = 0M;
        public ProcesoCreditoDetalle poDetalle = new ProcesoCreditoDetalle();
        public bool lbRevision = false;
        public bool lbCerrado = false;

        public frmMontoPagare()
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
                    poDetalle.MontoReferencial = Convert.ToDecimal(txtMontoPagare.EditValue);
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
            if (txtMontoPagare.EditValue.ToString() != "0")
            {
                if (Convert.ToDecimal(txtMontoPagare.EditValue) < ldcCupoSolicitado)
                {
                    psResult = string.Format("{0}Ingresar monto de pagaré mayor o igual al cupo solicitado: {1} \n", psResult, ldcCupoSolicitado.ToString("c2"));
                }
            }


            return psResult;
        }

        private void frmFechaVigencia_Load(object sender, EventArgs e)
        {
            try
            {
                if (poDetalle.MontoReferencial != null)
                {
                    txtMontoPagare.EditValue = poDetalle.MontoReferencial;
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
    }
}
