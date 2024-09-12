using DevExpress.XtraEditors.Repository;
using GEN_Entidad.Entidades.Ventas;
using GEN_Entidad;
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
using DevExpress.XtraEditors;

namespace REH_Presentacion.Ventas.Parametrizadores.PopUp
{
    public partial class frmCarteraZonaSAPCorreoPersona : Form
    {
        #region Variables
        clsNCarteraZonaSAP loLogicaNegocio = new clsNCarteraZonaSAP();
        public List<CarteraZonaSAPEnvioCorreo> Detalle = new List<CarteraZonaSAPEnvioCorreo>();
        public bool pbAcepto = false;
        private List<Combo> loComboPersona = new List<Combo>();
        private List<Combo> loComboTipoDestinatario = new List<Combo>();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();

        #endregion

        public frmCarteraZonaSAPCorreoPersona()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        private void frmCarteraZonaSAPCorreoPersona_Load(object sender, EventArgs e)
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

                dgvDatos.Columns["IdCarteraZonaSAPEnvioCorreo"].Visible = false;
                dgvDatos.Columns["IdZonaSAPGrupo"].Visible = false;
                dgvDatos.Columns["CorreoManual"].Visible = false;
                dgvDatos.Columns["Codigo"].Visible = false;

                dgvDatos.Columns["IdPersona"].Caption = "Empleado";

                dgvDatos.Columns["TipoDestinatario"].Width = 80;
                dgvDatos.Columns["IdPersona"].Width = 200;

                clsComun.gOrdenarColumnasGrid(dgvDatos);
                clsComun.gFormatearColumnasGrid(dgvDatos);

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rpiBtnDel_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int piIndex;
                // Tomamos la fila seleccionada
                piIndex = dgvDatos.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<CarteraZonaSAPEnvioCorreo>)bsDatos.DataSource;

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

        private string EsValido()
        {
            string psResult = "";

            dgvDatos.PostEditor();
            var poLista = (List<CarteraZonaSAPEnvioCorreo>)bsDatos.DataSource;
            var poListaRecorrida = new List<CarteraZonaSAPEnvioCorreo>();
            int fila = 1;
            foreach (var item in poLista)
            {
                if (poListaRecorrida.Where(x => x.IdPersona == item.IdPersona).Count() > 0)
                {
                    string psName = loComboPersona.Where(x => x.Codigo == item.IdPersona.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                    psResult = string.Format("{0}Eliminar Fila # {1}, Ya existe una parametrización del Vendedor: {2}\n", psResult, fila, psName);
                }

                poListaRecorrida.Add(item);
                fila++;
            }

            return psResult;
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
                    Detalle = (List<CarteraZonaSAPEnvioCorreo>)bsDatos.DataSource;
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

        private void btnAddManualmente_Click(object sender, EventArgs e)
        {
            try
            {

                bool pbEntro = false;

                pbEntro = true;
                bsDatos.AddNew();
                ((List<CarteraZonaSAPEnvioCorreo>)bsDatos.DataSource).LastOrDefault().IdPersona = 0;
                ((List<CarteraZonaSAPEnvioCorreo>)bsDatos.DataSource).LastOrDefault().TipoDestinatario = "PAR";
                dgvDatos.Focus();
                dgvDatos.FocusedRowHandle = dgvDatos.RowCount - 1;
                dgvDatos.FocusedColumn = dgvDatos.Columns[0];
                dgvDatos.ShowEditor();


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
    }
}
