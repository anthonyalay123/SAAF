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
    public partial class frmZonaGrupoDetalle : Form
    {
        #region Variables
        clsNZonas loLogicaNegocio = new clsNZonas();
        public List<ZonaGrupoDetalle> Detalle = new List<ZonaGrupoDetalle>();
        public bool pbAcepto = false;
        private List<Combo> loZonasSap = new List<Combo>();
        private List<Combo> loAgrupacion = new List<Combo>();
        private List<Combo> loResponsable = new List<Combo>();
        private List<Combo> loVendedor = new List<Combo>();
        private List<Combo> loZonaGrupo = new List<Combo>();
        private List<Combo> loTitulares = new List<Combo>();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
       
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmZonaGrupoDetalle()
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
                this.Width = 1000;
                this.CenterToScreen();

                bsDatos = new BindingSource();
                bsDatos.DataSource = Detalle;
                gcDatos.DataSource = bsDatos;

                loZonasSap = loLogicaNegocio.goConsultarZonasSAP();
                loAgrupacion = loLogicaNegocio.goConsultarComboAgrupacionCobranza();
                loResponsable = loLogicaNegocio.goConsultarComboResponsableCobranza();
                loVendedor = loLogicaNegocio.goSapConsultVendedoresTodos();
                loZonaGrupo = loLogicaNegocio.goConsultarComboVendedorGrupoTodos();
                loTitulares = loLogicaNegocio.goSapConsultTitularesTodos();

                clsComun.gLLenarComboGrid(ref dgvDatos, loZonasSap, "Code", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loAgrupacion, "CodAgrupacion", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loResponsable, "CodResponsableCobranza", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loVendedor, "CodVendedor", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loTitulares, "CodTitular", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loVendedor, "CodRecaudador", false);
                clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);

                dgvDatos.Columns["IdZonaGrupoDetalle"].Visible = false;
                dgvDatos.Columns["IdZonaGrupo"].Visible = false;
                dgvDatos.Columns["Agrupacion"].Visible = false;
                dgvDatos.Columns["ResponsableCobranza"].Visible = false;
                dgvDatos.Columns["NomVendedor"].Visible = false;
                dgvDatos.Columns["Titular"].Visible = false;
                dgvDatos.Columns["Recaudador"].Visible = false;
                dgvDatos.Columns["Name"].Visible = false;

                dgvDatos.Columns["Code"].Caption = "Zona SAP";
                dgvDatos.Columns["CodAgrupacion"].Caption = "Agrupacion";

                if (clsPrincipal.gIdPerfil != 1) // Diferente a perfil administrador
                {
                    dgvDatos.Columns["CodResponsableCobranza"].Visible = false;
                    dgvDatos.Columns["DiasProAcepIni"].Visible = false;
                    dgvDatos.Columns["DiasProAcepFin"].Visible = false;
                    dgvDatos.Columns["DiasProGestIni"].Visible = false;
                    dgvDatos.Columns["DiasProGestFin"].Visible = false;
                    dgvDatos.Columns["DiasProNoAcepIni"].Visible = false;
                    dgvDatos.Columns["DiasProNoAcepFin"].Visible = false;
                    dgvDatos.Columns["CodVendedor"].Visible = false;
                    dgvDatos.Columns["CodTitular"].Visible = false;
                    dgvDatos.Columns["CodRecaudador"].Visible = false;

                    this.Width = 430;
                    this.CenterToScreen();
                }
                dgvDatos.Columns["Code"].Width = 200;
                dgvDatos.Columns["CodAgrupacion"].Width = 100;

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

                //var psLista = ((List<ZonaGrupoDetalle>)bsDatos.DataSource).Select(x=>x.Code.ToString()).Distinct();

                bool pbEntro = false;

                pbEntro = true;
                bsDatos.AddNew();
                ((List<ZonaGrupoDetalle>)bsDatos.DataSource).LastOrDefault().CodAgrupacion = "0";
                dgvDatos.Focus();
                dgvDatos.FocusedRowHandle = dgvDatos.RowCount - 1;
                dgvDatos.FocusedColumn = dgvDatos.Columns[0];
                dgvDatos.ShowEditor();

                //foreach (var item in loComboVendedores.Where(x => !psLista.Contains(x.Codigo) && x.Codigo != Diccionario.Seleccione))
                //{
                //    pbEntro = true;
                //    bsDatos.AddNew();
                //    ((List<ZonaGrupoDetalle>)bsDatos.DataSource).LastOrDefault().SlpCode = 0;
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
                    Detalle = (List<ZonaGrupoDetalle>)bsDatos.DataSource;
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
            var poLista = (List<ZonaGrupoDetalle>)bsDatos.DataSource;
            var poListaRecorrida = new List<ZonaGrupoDetalle>();
            int fila = 1;
            foreach (var item in poLista)
            {
                if (poListaRecorrida.Where(x=>x.Code == item.Code).Count() > 0)
                {
                    string psName = loZonasSap.Where(x => x.Codigo == item.Code).Select(x => x.Descripcion).FirstOrDefault();
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
                var poLista = (List<ZonaGrupoDetalle>)bsDatos.DataSource;

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
