using COM_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using REH_Presentacion.Compras.Maestros;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Compras.Transaccion.Modal
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 27/04/2022
    /// Formulario Genérico para busqueda de datos
    /// </summary>
    public partial class frmAddFacturaPago : Form
    {
        #region Variables
        public string lsIdentificacion { get; private set; }
        public string lsProveedor { get; private set; }
        public DateTime ldFechaEmision { get; private set; }
        public DateTime ldFechaVencimiento { get; private set; }
        public decimal ldcValor { get; private set; }
        public string lsObservacion { get; private set; }
        public string lsNumDocumento { get; private set; }
        public int lIdProveedor { get; private set; }
        public int lIdGrupoPago { get; private set; }
        public string lsGrupoPago { get; private set; }
        clsNOrdenPago loLogicaNegocio = new clsNOrdenPago();
        #endregion

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="toResultado">Tabla de datos a mostrar</param>
        public frmAddFacturaPago()
        {
            InitializeComponent();
        }

        
        private void frmBusqueda_Load(object sender, EventArgs e)
        {
            try
            {
                clsComun.gLLenarCombo(ref cmbProveedor, loLogicaNegocio.goConsultarComboProveedorId(), true);
                clsComun.gLLenarCombo(ref cmbGrupoPago, loLogicaNegocio.goConsultarComboGrupoPagos(), true);

                txtValor.Properties.Mask.EditMask = "c";
                txtValor.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
                txtValor.Properties.Mask.UseMaskAsDisplayFormat = true;
                txtValor.EditValue = 0;

                dtpFechaEmision.EditValue = DateTime.Now;
                dptFechaVencimiento.EditValue = DateTime.Now;
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
                    lIdProveedor = int.Parse(cmbProveedor.EditValue.ToString());
                    lsIdentificacion = loLogicaNegocio.GetIdentificacionProveedor(lIdProveedor);

                    lsProveedor = cmbProveedor.Text;
                    lsNumDocumento = txtNumDocumento.Text;
                    lsObservacion = txtObservacion.Text;
                    ldcValor = Convert.ToDecimal(txtValor.EditValue);
                    ldFechaEmision = dtpFechaEmision.DateTime;
                    ldFechaVencimiento = dptFechaVencimiento.DateTime;
                    lIdGrupoPago = int.Parse(cmbGrupoPago.EditValue.ToString());
                    lsGrupoPago = cmbGrupoPago.Text;
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
            if (cmbGrupoPago.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Grupo Pago.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pbResult = false;
            }

            if (cmbProveedor.EditValue.ToString() == Diccionario.Seleccione)
            {
                XtraMessageBox.Show("Seleccione Proveedor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnAddProveedor_Click(object sender, EventArgs e)
        {
            try
            {
                string psForma = Diccionario.Tablas.Menu.Proveedores;
                var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);

                if (poForm != null)
                {
                    frmPaProveedores poFrmMostrarFormulario = new frmPaProveedores();

                    poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                    poFrmMostrarFormulario.Text = poForm.Nombre;
                    poFrmMostrarFormulario.ShowInTaskbar = true;
                    //poFrmMostrarFormulario.MdiParent = this.ParentForm;

                    poFrmMostrarFormulario.ShowDialog();
                    clsComun.gLLenarCombo(ref cmbProveedor, loLogicaNegocio.goConsultarComboProveedor(), true);

                }
                else
                {
                    XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string psForma = Diccionario.Tablas.Menu.frmMaGrupoPago;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);

            if (poForm != null)
            {
                frmMaGrupoPago poFrmMostrarFormulario = new frmMaGrupoPago();

                poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrmMostrarFormulario.Text = poForm.Nombre;
                poFrmMostrarFormulario.ShowInTaskbar = true;
                //poFrmMostrarFormulario.MdiParent = this.ParentForm;

                poFrmMostrarFormulario.ShowDialog();
                clsComun.gLLenarCombo(ref cmbGrupoPago, loLogicaNegocio.goConsultarComboGrupoPagos(), true);

            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
