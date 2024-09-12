using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Cobranza;
using GEN_Entidad.Entidades.Ventas;
using REH_Presentacion.Comun;
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

namespace REH_Presentacion.Ventas.Parametrizadores.PopUp
{
    /// <summary>
    /// Formulario que muestra el detalle de las comisiones
    /// </summary>
    public partial class frmZonaGrupoCorreoPersona : Form
    {
        #region Variables
        clsNZonas loLogicaNegocio = new clsNZonas();
        public List<ZonaGrupoEnvioCorreo> Detalle = new List<ZonaGrupoEnvioCorreo>();
        public bool pbAcepto = false;
        private List<Combo> loComboPersona = new List<Combo>();
        private List<Combo> loComboTipoDestinatario = new List<Combo>();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
       
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmZonaGrupoCorreoPersona()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        /// <summary>
        /// Evento de inicialización del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmComisionDetalle_Load(object sender, EventArgs e)
        {
            try
            {
                
                this.CenterToScreen();

                bsDatos = new BindingSource();
                bsDatos.DataSource = Detalle;
                gcDatos.DataSource = bsDatos;
                loComboPersona = loLogicaNegocio.goConsultarComboIdPersona();
                loComboTipoDestinatario = loLogicaNegocio.goConsultarComboTipoDestinatario();
                    
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboTipoDestinatario, "TipoDestinatario");
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboPersona, "IdPersona", true);
                clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);

                dgvDatos.Columns["IdZonaGrupoEnvioCorreo"].Visible = false;
                dgvDatos.Columns["IdZonaGrupo"].Visible = false;
                dgvDatos.Columns["CorreoManual"].Visible = false;

                dgvDatos.Columns["IdPersona"].Caption = "Empleado";

                dgvDatos.Columns["TipoDestinatario"].Width = 80;
                dgvDatos.Columns["IdPersona"].Width = 200;

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
                //frmCombo frmZona = new frmCombo();
                //frmZona.lsNombre = "Zona";
                //frmZona.loCombo = loComboZona;
                //frmZona.ShowDialog();
                //string psCodeZona = frmZona.lsSeleccionado;

                //var psLista = ((List<ZonaGrupoEnvioCorreo>)bsDatos.DataSource).Select(x=>x.Code.ToString()).Distinct();

                bool pbEntro = false;

                pbEntro = true;
                bsDatos.AddNew();
                ((List<ZonaGrupoEnvioCorreo>)bsDatos.DataSource).LastOrDefault().IdPersona = 0;
                ((List<ZonaGrupoEnvioCorreo>)bsDatos.DataSource).LastOrDefault().TipoDestinatario = "PAR";
                dgvDatos.Focus();
                dgvDatos.FocusedRowHandle = dgvDatos.RowCount - 1;
                dgvDatos.FocusedColumn = dgvDatos.Columns[0];
                dgvDatos.ShowEditor();

                //foreach (var item in loComboVendedores.Where(x => !psLista.Contains(x.Codigo) && x.Codigo != Diccionario.Seleccione))
                //{
                //    pbEntro = true;
                //    bsDatos.AddNew();
                //    ((List<ZonaGrupoEnvioCorreo>)bsDatos.DataSource).LastOrDefault().SlpCode = 0;
                //    dgvDatos.Focus();
                //    dgvDatos.FocusedRowHandle = dgvDatos.RowCount - 1;
                //    dgvDatos.FocusedColumn = dgvDatos.Columns[0];
                //    dgvDatos.ShowEditor();
                //}

                if (!pbEntro)
                {
                    XtraMessageBox.Show("Todas las zonas han sido añadidas", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Acepta los datos ingresados/seleccionados y cierra el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                pbAcepto = true;
                dgvDatos.PostEditor();
                string psMsg = EsValido();
                if (string.IsNullOrEmpty(psMsg))
                {
                    Detalle = (List<ZonaGrupoEnvioCorreo>)bsDatos.DataSource;
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

        /// <summary>
        /// Cierra el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalir_Click(object sender, EventArgs e)
        {
            pbAcepto = false;
            Close();
        }

        /// <summary>
        /// Valida información del formulario, previo a aceptar
        /// </summary>
        /// <returns></returns>
        private string EsValido()
        {
            string psResult = "";

            dgvDatos.PostEditor();
            var poLista = (List<ZonaGrupoEnvioCorreo>)bsDatos.DataSource;
            var poListaRecorrida = new List<ZonaGrupoEnvioCorreo>();
            int fila = 1;
            foreach (var item in poLista)
            {
                if (poListaRecorrida.Where(x=>x.IdPersona == item.IdPersona).Count() > 0)
                {
                    string psName = loComboPersona.Where(x => x.Codigo == item.IdPersona.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                    psResult = string.Format("{0}Eliminar Fila # {1}, Ya existe una parametrización del Vendedor: {2}\n", psResult, fila, psName);
                }

                poListaRecorrida.Add(item);
                fila++;
            }

            return psResult;
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
                var poLista = (List<ZonaGrupoEnvioCorreo>)bsDatos.DataSource;

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
        #endregion
    }
}
