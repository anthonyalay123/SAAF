using COM_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Cobranza.Transacciones.Modal
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 27/04/2022
    /// Formulario Genérico para busqueda de datos
    /// </summary>
    public partial class frmAddBorradorComisiones : Form
    {
        #region Variables

        public bool Cargar { get; set; }
        public int CodVendedor { get; set; }
        public string NomVendedor { get; set; }
        public string CodCliente { get; set; }
        public string GrupoCliente { get; set; }
        public string NumFactura { get; set; }
        public string Titular { get; set; }
        public string CodicionPago { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVecimineto { get; set; }
        public DateTime FechaContabilizacion { get; set; }
        public DateTime FechaEfectiva { get; set; }
        public int NumDocPago { get; set; }
        public string Recaudador { get; set; }
        public string TipoDoc { get; set; }
        public string NoCheque { get; set; }
        public string CodBanco { get; set; }
        public string NomBanco { get; set; }
        public decimal Valor { get; set; }
        public string NomEmpresa { get; set; }
        public string Zona { get; set; }
        public int DiasPago { get; set; }
        public decimal ValorPago { get; set; }
        clsNOrdenPago loLogicaNegocio = new clsNOrdenPago();

        public List<Combo> loVendedor = new List<Combo>();
        public List<Combo> loEmpresa = new List<Combo>();
        public List<Combo> loGrupoCliente = new List<Combo>();
        public List<Combo> loTitular = new List<Combo>();
        public List<Combo> loCondPago = new List<Combo>();
        public List<Combo> loRecaudador = new List<Combo>();
        public List<Combo> loTipoDoc = new List<Combo>();
        public List<Combo> loBanco = new List<Combo>();
        public List<Combo> loZona = new List<Combo>();
        #endregion

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="toResultado">Tabla de datos a mostrar</param>
        public frmAddBorradorComisiones()
        {
            InitializeComponent();
        }

        
        private void frmBusqueda_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbVendedor, loVendedor, !Cargar);
                clsComun.gLLenarCombo(ref cmbEmpresa, loEmpresa, !Cargar);
                clsComun.gLLenarCombo(ref cmbGrupoCliente, loGrupoCliente, !Cargar);
                clsComun.gLLenarCombo(ref cmbTitular, loTitular, !Cargar);
                clsComun.gLLenarCombo(ref cmbCondPago, loCondPago, !Cargar);
                clsComun.gLLenarCombo(ref cmbRecaudador, loRecaudador, !Cargar);
                clsComun.gLLenarCombo(ref cmbTipoDoc, loTipoDoc, !Cargar);
                clsComun.gLLenarCombo(ref cmbBanco, loBanco, !Cargar);
                clsComun.gLLenarCombo(ref cmbZona, loZona, !Cargar);

                txtValor.Properties.Mask.EditMask = "c";
                txtValor.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtValor.Properties.Mask.UseMaskAsDisplayFormat = true;
                txtValor.EditValue = ValorPago;

                dtpFechaEmision.EditValue = DateTime.Now;
                dtpFechaEfectiva.EditValue = DateTime.Now;
                dtpFechaContabilizacion.EditValue = DateTime.Now;
                dtpVecimiento.EditValue = DateTime.Now;

                if (Cargar)
                {
                    
                    dtpFechaEmision.EditValue = FechaEmision;
                    dtpFechaEfectiva.EditValue = FechaEfectiva;
                    dtpFechaContabilizacion.EditValue = FechaContabilizacion;
                    dtpVecimiento.EditValue = FechaVecimineto;

                    txtNunFactura.Text = NumFactura;
                    txtNumDocPago.Text = NumDocPago.ToString();
                    txtDiasPago.Text = DiasPago.ToString();
                    txtNoCheque.Text = NoCheque;
                    
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        
        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbValida())
                {
                    CodVendedor = int.Parse(cmbVendedor.EditValue.ToString());
                    NomVendedor = cmbVendedor.Text;
                    CodCliente = cmbEmpresa.EditValue.ToString();
                    NomEmpresa = cmbEmpresa.Text;
                    GrupoCliente = cmbGrupoCliente.EditValue.ToString();
                    NumFactura = txtNunFactura.Text.Trim();
                    Titular = cmbTitular.EditValue.ToString();
                    CodicionPago = cmbCondPago.EditValue.ToString();
                    FechaEmision = dtpFechaEmision.DateTime;
                    FechaVecimineto = dtpVecimiento.DateTime;
                    FechaContabilizacion = dtpFechaContabilizacion.DateTime;
                    FechaEfectiva = dtpFechaEfectiva.DateTime;
                    NumDocPago = int.Parse(txtNumDocPago.Text.Trim());
                    Recaudador = cmbRecaudador.EditValue.ToString();
                    TipoDoc = cmbTipoDoc.EditValue.ToString();
                    NoCheque = txtNoCheque.Text.Trim();
                    CodBanco = cmbBanco.EditValue.ToString() == Diccionario.Seleccione ? "" : cmbBanco.EditValue.ToString();
                    NomBanco = cmbBanco.Text == Diccionario.DesSeleccione ? "" : cmbBanco.Text;
                    Valor = decimal.Parse(txtValor.EditValue.ToString());
                    Zona = cmbZona.EditValue.ToString();
                    DiasPago = int.Parse(txtDiasPago.Text);
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool lbValida()
        {
            bool pbResult = true;
            if (cmbVendedor.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Vendedor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pbResult = false;
            }
            if (Convert.ToDecimal(txtValor.EditValue.ToString()) <= 0)
            {
                XtraMessageBox.Show("Ingrese el Valor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pbResult = false;
            }

            return pbResult;
        }

        private void frmAddFacturaPago_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
            }

            else if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

       
    }
}
