using COB_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Cobranza;
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

namespace REH_Presentacion.Cobranza.Parametrizadores
{
    /// <summary>
    /// Formulario que muestra el detalle de las comisiones
    /// </summary>
    public partial class frmEmpleadoComisionDetalleZona : Form
    {
        #region Variables
        clsNComisiones loLogicaNegocio = new clsNComisiones();
        public List<EmpleadoComisionZona> ComisionDetalle = new List<EmpleadoComisionZona>();
        public bool pbAcepto = false;
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
       
        private List<Combo> loCombo = new List<Combo>();
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmEmpleadoComisionDetalleZona()
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
                bsDatos = new BindingSource();
                bsDatos.DataSource = ComisionDetalle;
                gcDatos.DataSource = bsDatos;
                
                loCombo = loLogicaNegocio.goConsultarZonasSAP();

                clsComun.gLLenarComboGrid(ref dgvDatos, loCombo, "CodeZona", true);
                
                clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);

                dgvDatos.Columns["IdEmpleadoZonaSap"].Visible = false;
                dgvDatos.Columns["IdPersona"].Visible = false;
                dgvDatos.Columns["NameZona"].Visible = false;

                dgvDatos.Columns["CodeZona"].Caption = "Zona";

                dgvDatos.Columns["CodeZona"].Width = 200;

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
                frmCombo frmZona = new frmCombo();
                frmZona.lsNombre = "Zona";
                frmZona.loCombo = loCombo;
                frmZona.ShowDialog();
                string psCodeZona = frmZona.lsSeleccionado;

                bsDatos.AddNew();
                ((List<EmpleadoComisionZona>)bsDatos.DataSource).LastOrDefault().CodeZona = string.IsNullOrEmpty(psCodeZona) ? Diccionario.Seleccione : psCodeZona;
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
                    ComisionDetalle = (List<EmpleadoComisionZona>)bsDatos.DataSource;
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
            var poLista = (List<EmpleadoComisionZona>)bsDatos.DataSource;
            var poListaRecorrida = new List<EmpleadoComisionZona>();
            int fila = 1;
            foreach (var item in poLista)
            {
                string psZona = loCombo.Where(x => x.Codigo.Contains(item.CodeZona)).Select(x => x.Descripcion).FirstOrDefault();

                if (item.CodeZona == Diccionario.Seleccione)
                {
                    psResult = string.Format("{0}Verificar si están seleccionados la Zona en la Fila # {1}\n", psResult, fila);
                }

                else if (poListaRecorrida.Where(x=>x.CodeZona == item.CodeZona).Count() > 0)
                {
                    psResult = string.Format("{0}Eliminar Fila # {1}, Ya existe una parametrización por: {2}\n", psResult, fila, psZona);
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
                var poLista = (List<EmpleadoComisionZona>)bsDatos.DataSource;

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
