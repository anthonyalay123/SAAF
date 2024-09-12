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

namespace REH_Presentacion.Compras.Transaccion.Modal
{
    public partial class frmDetalleFacturaProveedor : Form
    {

        public List<FacturaAprobadasDetalleGrid> loLista = new List<FacturaAprobadasDetalleGrid>();
        RepositoryItemButtonEdit rpiBtnShow = new RepositoryItemButtonEdit();
        BindingSource bsDatos = new BindingSource();
        clsNOrdenPago loLogicaNegocio = new clsNOrdenPago();

        public frmDetalleFacturaProveedor()
        {
            InitializeComponent();
            rpiBtnShow.ButtonClick += rpiBtnShow_ButtonClick;
        }

        private void frmDetalleFacturaProveedor_Load(object sender, EventArgs e)
        {
            try
            {
                bsDatos.DataSource = loLista;
                gcBusqueda.DataSource = bsDatos;

                dgvBusqueda.Columns["ArchivoAdjunto"].Visible = false;
                dgvBusqueda.Columns["RutaOrigen"].Visible = false;
                dgvBusqueda.Columns["RutaDestino"].Visible = false;
                dgvBusqueda.Columns["NombreOriginal"].Visible = false;
                dgvBusqueda.Columns["Sel"].Visible = false;
                dgvBusqueda.Columns["Det"].Visible = false;

                dgvBusqueda.Columns["IdOrdenPagoFactura"].Visible = false;
                dgvBusqueda.Columns["IdSemanaPago"].Visible = false;
                dgvBusqueda.Columns["IdFacturaPago"].Visible = false;
                dgvBusqueda.Columns["Identificacion"].Visible = false;
                
                dgvBusqueda.Columns["Generado"].Visible = false;
                dgvBusqueda.Columns["Del"].Visible = false;
                dgvBusqueda.Columns["IdOrdenPago"].Visible = false;
                dgvBusqueda.Columns["Valor"].Visible = false;
                dgvBusqueda.Columns["Abono"].Visible = false;
                dgvBusqueda.Columns["Saldo"].Visible = false;
                dgvBusqueda.Columns["FechaEmision"].Visible = false;
                
                dgvBusqueda.Columns["Aprobaciones"].Visible = false;
                dgvBusqueda.Columns["Aprobo"].Visible = false;
                dgvBusqueda.Columns["DocNum"].Visible = false;
                dgvBusqueda.Columns["Add"].Visible = false;
                dgvBusqueda.Columns["Descargar"].Visible = false;
                dgvBusqueda.Columns["Visualizar"].Visible = false;
                dgvBusqueda.Columns["FechaPago"].Visible = false;
                dgvBusqueda.Columns["IdProveedor"].Visible = false;
                dgvBusqueda.Columns["IdGrupoPago"].Visible = false;
                dgvBusqueda.Columns["FechaVencimiento"].Visible = false;
                dgvBusqueda.Columns["FechaVencimiento"].Visible = false;
                dgvBusqueda.Columns["ComentarioAprobador"].Visible = false;
                dgvBusqueda.Columns["CodigoEstado"].Visible = false;
                dgvBusqueda.Columns["Observacion"].Visible = false;
                dgvBusqueda.Columns["VerOP"].Visible = false;

                dgvBusqueda.Columns["CodigoTipoIdentificacion"].Visible = false;
                dgvBusqueda.Columns["IdentificacionCuenta"].Visible = false;
                dgvBusqueda.Columns["Nombre"].Visible = false;
                dgvBusqueda.Columns["CodigoBanco"].Visible = false;
                dgvBusqueda.Columns["CodigoFormaPago"].Visible = false;
                dgvBusqueda.Columns["CodigoTipoCuentaBancaria"].Visible = false;
                dgvBusqueda.Columns["NumeroCuenta"].Visible = false;
                dgvBusqueda.Columns["GrupoPago"].Visible = false;

                dgvBusqueda.Columns["Proveedor"].OptionsColumn.AllowEdit = false;
                dgvBusqueda.Columns["ValorPagoOriginal"].OptionsColumn.AllowEdit = false;
                dgvBusqueda.Columns["ValorPago"].OptionsColumn.AllowEdit = false;
                dgvBusqueda.Columns["NumDocumento"].OptionsColumn.AllowEdit = false;
                dgvBusqueda.Columns["GrupoPago"].OptionsColumn.AllowEdit = false;
                dgvBusqueda.Columns["NumDocumento"].Caption = "Factura";

                clsComun.gDibujarBotonGrid(rpiBtnShow, dgvBusqueda.Columns["Ver"], "Orden Pago", Diccionario.ButtonGridImage.show_16x16);


                clsComun.gFormatearColumnasGrid(dgvBusqueda);
                clsComun.gOrdenarColumnasGridFullEditable(dgvBusqueda);

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del boton de Visulizar en el Grid, adjunta archivo en el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnShow_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex = dgvBusqueda.GetFocusedDataSourceRowIndex();
                var poLista = (List<FacturaAprobadasDetalleGrid>)bsDatos.DataSource;
                if (piIndex >= 0)
                {
                    if (poLista[piIndex].IdOrdenPago != 0)
                    {

                        string psForma = Diccionario.Tablas.Menu.OrdenPago;
                        var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
                        if (poForm != null)
                        {
                            frmTrOrdenPago poFrmMostrarFormulario = new frmTrOrdenPago();
                            poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                            poFrmMostrarFormulario.Text = poForm.Nombre;
                            poFrmMostrarFormulario.ShowInTaskbar = true;
                            poFrmMostrarFormulario.MdiParent = this.ParentForm;
                            poFrmMostrarFormulario.lid = poLista[piIndex].IdOrdenPago;
                            poFrmMostrarFormulario.Show();
                        }
                        else
                        {
                            XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show(string.Format("Factura: '{0}', No viene de una orden de pago.", poLista[piIndex].NumDocumento), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
