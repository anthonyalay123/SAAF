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
    public partial class frmComisionDetalle : Form
    {
        #region Variables
        clsNComisiones loLogicaNegocio = new clsNComisiones();
        public List<ComisionDetalleBase> ComisionDetalle = new List<ComisionDetalleBase>();
        public bool pbAcepto = false;
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        
        private List<Combo> loComboTipoCodigoComision = new List<Combo>();
        private List<Combo> loComboZona = new List<Combo>();
        private List<Combo> loComboRango = new List<Combo>();
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmComisionDetalle()
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

                loComboTipoCodigoComision = loLogicaNegocio.goConsultarComboTipoCodigoComision();
                loComboRango = loLogicaNegocio.goConsultarToleranciaContado();
                loComboZona = loLogicaNegocio.goConsultarZonasSAP();

                clsComun.gLLenarComboGrid(ref dgvDatos, loComboTipoCodigoComision, "CodigoComision", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboZona, "CodeZona", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboRango, "CodigoToleraciaContado", true);

                clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);

                dgvDatos.Columns["IdCab"].Visible = false;
                dgvDatos.Columns["IdDet"].Visible = false;

                dgvDatos.Columns["CodigoComision"].Caption = "Tipo Comisión";
                dgvDatos.Columns["CodeZona"].Caption = "Zona";
                dgvDatos.Columns["CodigoToleraciaContado"].Caption = "Rango";
                dgvDatos.Columns["PorcentajeComision"].Caption = "% Comisión";

                dgvDatos.Columns["CodeZona"].Width = 200;
                dgvDatos.Columns["CodigoToleraciaContado"].Width = 200;

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
                frmCombo frmTipoComision = new frmCombo();
                frmTipoComision.lsNombre = "Tipo Comisión";
                frmTipoComision.loCombo = loComboTipoCodigoComision;
                frmTipoComision.ShowDialog();
                string psCodeTipoComision = frmTipoComision.lsSeleccionado;

                frmCombo frmZona = new frmCombo();
                frmZona.lsNombre = "Zona";
                frmZona.loCombo = loComboZona;
                frmZona.ShowDialog();
                string psCodeZona = frmZona.lsSeleccionado;

                foreach (var item in loComboRango.Where(x=>x.Codigo != Diccionario.Seleccione))
                {
                    bsDatos.AddNew();
                    ((List<ComisionDetalleBase>)bsDatos.DataSource).LastOrDefault().CodeZona = string.IsNullOrEmpty(psCodeZona) ? Diccionario.Seleccione : psCodeZona;
                    ((List<ComisionDetalleBase>)bsDatos.DataSource).LastOrDefault().CodigoComision = string.IsNullOrEmpty(psCodeTipoComision) ? Diccionario.Seleccione : psCodeTipoComision;
                    ((List<ComisionDetalleBase>)bsDatos.DataSource).LastOrDefault().CodigoToleraciaContado = item.Codigo;
                    dgvDatos.Focus();
                    dgvDatos.FocusedRowHandle = dgvDatos.RowCount - 1;
                    dgvDatos.FocusedColumn = dgvDatos.Columns[0];
                    dgvDatos.ShowEditor();
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
                    ComisionDetalle = (List<ComisionDetalleBase>)bsDatos.DataSource;
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
            var poLista = (List<ComisionDetalleBase>)bsDatos.DataSource;
            var poListaRecorrida = new List<ComisionDetalleBase>();
            int fila = 1;
            foreach (var item in poLista)
            {
                string psRango = loComboRango.Where(x => x.Codigo.Contains(item.CodigoToleraciaContado)).Select(x => x.Descripcion).FirstOrDefault();
                string psZona = loComboZona.Where(x => x.Codigo.Contains(item.CodeZona)).Select(x => x.Descripcion).FirstOrDefault();

                if (item.CodeZona == Diccionario.Seleccione || item.CodigoToleraciaContado == Diccionario.Seleccione || item.CodigoComision == Diccionario.Seleccione)
                {
                    psResult = string.Format("{0}Verificar si están seleccionados el Tipo de Comisión, la Zona y el Rango en la Fila # {1}\n", psResult, fila);
                }

                else if (poListaRecorrida.Where(x=>x.CodeZona == item.CodeZona && x.CodigoComision == item.CodigoComision && x.CodigoToleraciaContado == item.CodigoToleraciaContado).Count() > 0)
                {
                    psResult = string.Format("{0}Eliminar Fila # {1}, Ya existe una parametrización por: {2}, {3} y {4}\n", psResult, fila, item.CodigoComision,psZona, psRango);
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
                var poLista = (List<ComisionDetalleBase>)bsDatos.DataSource;

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
