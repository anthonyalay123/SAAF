using COB_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Cobranza;
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

namespace REH_Presentacion.Cobranza.Parametrizadores
{
    /// <summary>
    /// Formulario de Excepciones de clientes para cálculo de comisión
    /// </summary>
    public partial class frmPrExcepcionClienteComision : frmBaseTrxDev
    {
        #region Variables
        clsNComisiones loLogicaNegocio = new clsNComisiones();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDet = new RepositoryItemButtonEdit();
        private List<Combo> loComboCliente = new List<Combo>();
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmPrExcepcionClienteComision()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
            rpiBtnDet.ButtonClick += rpiBtnDet_ButtonClick;
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
                lCargarEventosBotones();
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboCliente, "CardCode", true);
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
                    List<ClienteComision> poLista = (List<ClienteComision>)bsDatos.DataSource;

                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de guardar cambios?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string psMsg = loLogicaNegocio.gsGuardarClienteComision(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                var poLista = (List<ClienteComision>)bsDatos.DataSource;

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
        /// Evento del boton de Mostrar detalle de la fila seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpiBtnDet_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                dgvDatos.PostEditor();
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<ClienteComision>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poDetalle = poLista[piIndex].ClienteComisionDetalle == null ? new List<ClienteComisionDetalle>() : poLista[piIndex].ClienteComisionDetalle.ToList();

                    frmComisionDetalle frmComisionDetalle = new frmComisionDetalle();
                    frmComisionDetalle.ComisionDetalle = poDetalle.Select(x=> new ComisionDetalleBase()
                    {
                        CodigoComision = x.CodigoComision,
                        CodeZona = x.CodeZona,
                        CodigoToleraciaContado = x.CodigoToleraciaContado,
                        PorcentajeComision = x.PorcentajeComision,
                        IdCab = x.IdClienteComision,
                        IdDet = x.IdClienteComisionDetalle
                    }).ToList();
                    frmComisionDetalle.ShowDialog();

                    if (frmComisionDetalle.pbAcepto)
                    {
                        poLista[piIndex].ClienteComisionDetalle = frmComisionDetalle.ComisionDetalle.Select(x => new ClienteComisionDetalle()
                        {
                            CodigoComision = x.CodigoComision,
                            CodeZona = x.CodeZona,
                            CodigoToleraciaContado = x.CodigoToleraciaContado,
                            PorcentajeComision = x.PorcentajeComision,
                            IdClienteComision = x.IdCab,
                            IdClienteComisionDetalle = x.IdDet
                        }).ToList();
                    }
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
                ((List<ClienteComision>)bsDatos.DataSource).LastOrDefault().CardCode = Diccionario.Seleccione;
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
            List<ClienteComision> poLista = (List<ClienteComision>)bsDatos.DataSource;

            var poListaRecorrida = new List<ClienteComision>();
            int fila = 1;
            foreach (var item in poLista)
            {
                string psCliente = loComboCliente.Where(x => x.Codigo.Contains(item.CardCode)).Select(x => x.Descripcion).FirstOrDefault();
                if (item.CardCode == Diccionario.Seleccione)
                {
                    psMsg = string.Format("{0}Seleccionar el cliente en la Fila # {1}\n", psMsg, fila);
                }

                else if (poListaRecorrida.Where(x => x.CardCode == item.CardCode).Count() > 0)
                {
                    psMsg = string.Format("{0}Eliminar Fila # {1}, Ya existe una parametrización del cliente: {2}\n", psMsg, fila, psCliente);
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
            bsDatos.DataSource = new List<ClienteComision>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsBehavior.Editable = true;


            dgvDatos.Columns["CodigoEstado"].Visible = false;
            dgvDatos.Columns["CardName"].Visible = false;
            dgvDatos.Columns["ClienteComisionDetalle"].Visible = false;
            dgvDatos.Columns["IdClienteComision"].Visible = false;

            dgvDatos.Columns["CardCode"].Caption = "Cliente";
            dgvDatos.Columns["IdClienteComision"].Caption = "Id";

            dgvDatos.Columns["IdClienteComision"].OptionsColumn.AllowEdit = false;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnDet, dgvDatos.Columns["Det"], "Detalle", Diccionario.ButtonGridImage.inserttable_16x16);

            dgvDatos.Columns["CardCode"].Width = 500;
        }

        private void lBuscar()
        { 
            bsDatos.DataSource = loLogicaNegocio.goConsultarExcepcionClienteComision();
            gcDatos.DataSource = bsDatos;

        }
        #endregion
    }
}
