using CRE_Negocio.Transacciones;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
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
    public partial class frmCheckList : Form
    {
        #region Variables
        clsNProcesoCredito loLogicaNegocio = new clsNProcesoCredito();
        public List<ProcesoCreditoDetalleRevision> ComisionDetalle = new List<ProcesoCreditoDetalleRevision>();
        public bool pbAcepto = false;
        public int tId = 0;
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();

        private List<Combo> loCombo = new List<Combo>();
        
        #endregion

        public frmCheckList()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        private void frmCheckList_Load(object sender, EventArgs e)
        {
            try
            {
                bsDatos = new BindingSource();
                bsDatos.DataSource = loLogicaNegocio.goConsultarProcesoDetalleRevision(tId);
                gcDatos.DataSource = bsDatos;

                loCombo = loLogicaNegocio.goConsultarComboCheckList();
                clsComun.gLLenarComboGrid(ref dgvDatos, loCombo, "IdCheckList");

                //clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);

                dgvDatos.Columns["IdProcesoCreditoDetalle"].Visible = false;
                dgvDatos.Columns["IdProcesoCredito"].Visible = false;

                dgvDatos.Columns["Sel"].Caption = "Obligatorio";
                dgvDatos.Columns["IdCheckList"].Caption = "Documento";

                dgvDatos.Columns["Sel"].Width = 20;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                var poLista = (List<ProcesoCreditoDetalleRevision>)bsDatos.DataSource;

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

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                pbAcepto = true;
                dgvDatos.PostEditor();
                string psMsg = EsValido();
                if (string.IsNullOrEmpty(psMsg))
                {
                    ComisionDetalle = (List<ProcesoCreditoDetalleRevision>)bsDatos.DataSource;
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
            var poLista = (List<ProcesoCreditoDetalleRevision>)bsDatos.DataSource;
            var poListaRecorrida = new List<ProcesoCreditoDetalleRevision>();
            int fila = 1;
            foreach (var item in poLista)
            {
                //string psRango = loComboRango.Where(x => x.Codigo.Contains(item.CodigoToleraciaContado)).Select(x => x.Descripcion).FirstOrDefault();
                //string psZona = loComboZona.Where(x => x.Codigo.Contains(item.CodeZona)).Select(x => x.Descripcion).FirstOrDefault();

                //if (item.CodeZona == Diccionario.Seleccione || item.CodigoToleraciaContado == Diccionario.Seleccione || item.CodigoComision == Diccionario.Seleccione)
                //{
                //    psResult = string.Format("{0}Verificar si están seleccionados el Tipo de Comisión, la Zona y el Rango en la Fila # {1}\n", psResult, fila);
                //}

                //else if (poListaRecorrida.Where(x => x.CodeZona == item.CodeZona && x.CodigoComision == item.CodigoComision && x.CodigoToleraciaContado == item.CodigoToleraciaContado).Count() > 0)
                //{
                //    psResult = string.Format("{0}Eliminar Fila # {1}, Ya existe una parametrización por: {2}, {3} y {4}\n", psResult, fila, item.CodigoComision, psZona, psRango);
                //}

                poListaRecorrida.Add(item);
                fila++;
            }

            return psResult;
        }

        private void btnAddManualmente_Click(object sender, EventArgs e)
        {
            List<ProcesoCreditoDetalleRevision> poLista = (List<ProcesoCreditoDetalleRevision>)bsDatos.DataSource;

            var sel = poLista.Select(x => x.Sel).FirstOrDefault();
            foreach (var item in poLista)
            {
                item.Sel = !sel;
            }


            bsDatos.DataSource = poLista;
            dgvDatos.RefreshData();
        }
    }
}
