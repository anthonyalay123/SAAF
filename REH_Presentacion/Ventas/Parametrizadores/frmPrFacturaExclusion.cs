using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
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
using VTA_Negocio;

namespace REH_Presentacion.Ventas.Parametrizadores
{
    /// <summary>
    /// Formulario de Excepciones de clientes para cálculo de comisión
    /// </summary>
    public partial class frmPrFacturaExclusion : frmBaseTrxDev
    {
        #region Variables
        clsNFacturaExclusion loLogicaNegocio = new clsNFacturaExclusion();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        private List<Combo> loComboCliente = new List<Combo>();
        private List<Combo> loComboTipoDocumento = new List<Combo>();
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmPrFacturaExclusion()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        /// <summary>
        /// Evento de inicialización del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPrExcepcionClienteComision_Load(object sender, EventArgs e)
        {
            try
            {
                loComboCliente = loLogicaNegocio.goSapConsultaClientes();
                loComboTipoDocumento = loLogicaNegocio.goConsultarComboTipoDocumento();
                lCargarEventosBotones();
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboCliente, "CardCode", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboTipoDocumento,"TipoDocumento", true);
                lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento que consulta la información guardada y la muestra por pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                lBuscar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento de grabar datos en el sistema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                dgvDatos.PostEditor();
                string psMsgValida = lsEsValido();

                if (string.IsNullOrEmpty(psMsgValida))
                {
                    List<FacturaExclusion> poLista = (List<FacturaExclusion>)bsDatos.DataSource;

                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de guardar cambios?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string psMsg = loLogicaNegocio.gsGuardarFacturaExclusion(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lBuscar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show(psMsgValida, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Evento del boton de eliminar en el Grid, elimina la fila seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<FacturaExclusion>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsDatos.DataSource = poLista;
                    dgvDatos.RefreshData();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Añade una fila a el grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddManualmente_Click(object sender, EventArgs e)
        {
            try
            {
                bsDatos.AddNew();
                ((List<FacturaExclusion>)bsDatos.DataSource).LastOrDefault().CardCode = Diccionario.Seleccione;
                ((List<FacturaExclusion>)bsDatos.DataSource).LastOrDefault().TipoDocumento = Diccionario.Seleccione;
                dgvDatos.Focus();
                dgvDatos.FocusedRowHandle = dgvDatos.RowCount - 1;
                dgvDatos.FocusedColumn = dgvDatos.Columns[0];
                dgvDatos.ShowEditor();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Valida datos del formulario previo a Guardarlos
        /// </summary>
        /// <returns></returns>
        private string lsEsValido()
        {

            string psMsg = string.Empty;
            dgvDatos.PostEditor();
            List<FacturaExclusion> poLista = (List<FacturaExclusion>)bsDatos.DataSource;

            var poListaRecorrida = new List<FacturaExclusion>();
            int fila = 1;
            foreach (var item in poLista)
            {
                string psCliente = loComboCliente.Where(x => x.Codigo.Contains(item.CardCode)).Select(x => x.Descripcion).FirstOrDefault();
                string psTipoDocumento = loComboTipoDocumento.Where(x => x.Codigo.Contains(item.TipoDocumento)).Select(x => x.Descripcion).FirstOrDefault();

                if (item.CardCode == Diccionario.Seleccione)
                {
                    psMsg = string.Format("{0}Seleccionar el cliente en la Fila # {1}\n", psMsg, fila);
                }

                if (item.TipoDocumento == Diccionario.Seleccione)
                {
                    psMsg = string.Format("{0}Seleccionar el tipo de documento en la Fila # {1}\n", psMsg, fila);
                }

                if (item.NumFactura <= 0)
                {
                    psMsg = string.Format("{0}Ingrese el número de factura en la Fila # {1}\n", psMsg, fila);
                }

                else if (poListaRecorrida.Where(x => x.NumFactura == item.NumFactura && x.CardCode == item.CardCode && x.TipoDocumento == item.TipoDocumento).Count() > 0)
                {
                    psMsg = string.Format("{0}Eliminar Fila # {1}, Ya existe una parametrización por el cliente: {2}, Tipo Documento: {4} y la factura: {3}\n", psMsg, fila, psCliente, item.NumFactura, psTipoDocumento);
                }

                poListaRecorrida.Add(item);
                fila++;
            }

            
            return psMsg;
        }

        /// <summary>
        /// Carga botones y define forma del formulario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();

            if (tstBotones.Items["btnConsultar"] != null) tstBotones.Items["btnConsultar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;

            bsDatos = new BindingSource();
            bsDatos.DataSource = new List<FacturaExclusion>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsBehavior.Editable = true;


            dgvDatos.Columns["CodigoEstado"].Visible = false;
            dgvDatos.Columns["CardName"].Visible = false;
            dgvDatos.Columns["IdFacturaExclusion"].Visible = false;

            dgvDatos.Columns["CardCode"].Caption = "Cliente";
            dgvDatos.Columns["NumFactura"].Caption = "# Documento";

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);

            dgvDatos.Columns["CardCode"].Width = 400;
            dgvDatos.Columns["NumFactura"].Width = 80;
            dgvDatos.Columns["TipoDocumento"].Width = 110;
        }

        private void lBuscar()
        { 
            bsDatos.DataSource = loLogicaNegocio.goConsultarFacturaExclusion();
            gcDatos.DataSource = bsDatos;

        }
        #endregion
    }
}
