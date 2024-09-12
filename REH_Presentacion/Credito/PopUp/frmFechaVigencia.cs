using DevExpress.XtraEditors;
using GEN_Entidad.Entidades.Credito;
using REH_Presentacion.Credito.Transacciones;
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
    public partial class frmFechaVigencia : Form
    {
        private frmTrProcesoCredito m_MainForm;

        public bool pbAcepto = false;
        public int tIdCheckList = 0;
        public ProcesoCreditoDetalle poDetalle = new ProcesoCreditoDetalle();
        public string lsNombre = "";
        public bool lbRevision = false;
        public bool lbCerrado = false;

        public frmFechaVigencia(frmTrProcesoCredito form = null)
        {
            m_MainForm = form;
            InitializeComponent();

            if (lbCerrado)
            {
                btnAceptar.Enabled = false;
            }
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
                    poDetalle.FechaReferencial = dtpFecha.DateTime;
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
            int DiasVigencia = 0;

            if (dtpFecha.DateTime != DateTime.MinValue)
            {
                if (dtpFecha.DateTime == null)
                {
                    psResult = string.Format("{0}Ingresar fecha de vigencia\n", psResult);
                }
                else if (dtpFecha.DateTime == DateTime.MinValue)
                {
                    //psResult = string.Format("{0}Ingresar fecha de vigencia\n", psResult);
                }

                DateTime pdFechaActual = DateTime.Now;

                if (tIdCheckList == 11) // Certf. Banco
                {
                    DiasVigencia = 180; // NO valida vigencia en fecha de actualización, que debe ser máximo de 5 años atrás.
                    if (pdFechaActual.Subtract(dtpFecha.DateTime).Days > DiasVigencia)
                    {
                        psResult = string.Format("{0}Fecha ingresada no está vigente, Fecha de Vigencia mínima valida {1}\n", psResult, pdFechaActual.AddDays(DiasVigencia * -1).ToString("dd/MM/yyyy"));
                    }
                }
                else if (tIdCheckList == 14) // Certif.Comercial
                {
                    DiasVigencia = 90; // NO valida vigencia que debe ser emitido hasta máximo 3 meses atrás. 
                    if (pdFechaActual.Subtract(dtpFecha.DateTime).Days > DiasVigencia)
                    {
                        psResult = string.Format("{0}Fecha ingresada no está vigente, Fecha de Vigencia mínima valida {1}\n", psResult, pdFechaActual.AddDays(DiasVigencia * -1).ToString("dd/MM/yyyy"));
                    }
                }
                else if (tIdCheckList == 8) // Escrit. Const. Empresa
                {

                }
                else if (tIdCheckList == 13) // Pagare
                {

                }
                else if (tIdCheckList == 10) // Predios/Certf.Bienes
                {
                    DiasVigencia = 365; // NO valida vigencia que debe ser emitido máximo hace 1 año atrás.
                    if (pdFechaActual.Subtract(dtpFecha.DateTime).Days > DiasVigencia)
                    {
                        psResult = string.Format("{0}Fecha ingresada no está vigente, Fecha de Vigencia mínima valida {1}\n", psResult, pdFechaActual.AddDays(DiasVigencia * -1).ToString("dd/MM/yyyy"));
                    }
                }
                else if (tIdCheckList == 7) // Ult. Imp. Rta./EEFF
                {
                    DiasVigencia = 510; // NO valida vigencia que debe ser del ultimo año declarado (año anterior y hasta Junio del año en curso, o sea 1 año 5 meses de vigencia).
                    if (pdFechaActual.Subtract(dtpFecha.DateTime).Days > DiasVigencia)
                    {
                        psResult = string.Format("{0}Fecha ingresada no está vigente, Fecha de Vigencia mínima valida {1}\n", psResult, pdFechaActual.AddDays(DiasVigencia * -1).ToString("dd/MM/yyyy"));
                    }
                }
                else if (tIdCheckList == 4) // Cédula Cónyuge
                {

                }
                else if (tIdCheckList == 15) // Plan Comercial o Acuerdo Comercial
                {

                }
            }



            return psResult;
        }

        private void frmFechaVigencia_Load(object sender, EventArgs e)
        {
            try
            {
                if (poDetalle.FechaReferencial != DateTime.MinValue)
                {
                    dtpFecha.DateTime = poDetalle.FechaReferencial;
                }
                if (!string.IsNullOrEmpty(lsNombre))
                {
                    lblNombre.Text = lsNombre;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
