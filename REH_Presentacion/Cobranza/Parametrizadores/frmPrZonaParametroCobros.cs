using COB_Negocio;
using COB_Negocio;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using GEN_Entidad;
using GEN_Entidad.Entidades.Cobranza;
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

namespace REH_Presentacion.Cobranza.Parametrizadores
{
    /// <summary>
    /// Formulario de Excepciones de clientes para cálculo de comisión
    /// </summary>
    public partial class frmPrZonaParametroCobros : frmBaseTrxDev
    {
        #region Variables
        clsNComisiones loLogicaNegocio = new clsNComisiones();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        
        private List<Combo> loComboVendedor = new List<Combo>();
        private List<Combo> loComboTitular = new List<Combo>();
        private List<Combo> loComboRecaudador = new List<Combo>();
        private List<Combo> loComboZona = new List<Combo>();
        private List<Combo> loComboAgrupacion = new List<Combo>();
        private List<Combo> loComboResponsable = new List<Combo>();
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmPrZonaParametroCobros()
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
                loComboVendedor = loLogicaNegocio.goSapConsultVendedoresTodos();
                loComboZona = loLogicaNegocio.goConsultarZonasSAP();
                loComboTitular = loLogicaNegocio.goSapConsultTitularesTodos();
                loComboAgrupacion = loLogicaNegocio.goConsultarComboAgrupacionCobranza();
                loComboResponsable = loLogicaNegocio.goConsultarComboResponsableCobranza();

                lCargarEventosBotones();
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboVendedor, "CodVendedor", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboZona, "Code", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboTitular, "CodTitular", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboVendedor, "CodRecaudador");
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboResponsable, "CodResponsableCobranza", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboAgrupacion, "CodAgrupacion", true);
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
                    List<ZonaGrupoDetalle> poLista = (List<ZonaGrupoDetalle>)bsDatos.DataSource;

                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de guardar cambios?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string psMsg = loLogicaNegocio.gsGuardarPrCobGrupoZonaDetalle(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                ((List<ZonaGrupoDetalle>)bsDatos.DataSource).LastOrDefault().Code = Diccionario.Seleccione;
                ((List<ZonaGrupoDetalle>)bsDatos.DataSource).LastOrDefault().CodVendedor = 0;
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
            List<ZonaGrupoDetalle> poLista = (List<ZonaGrupoDetalle>)bsDatos.DataSource;

            var poListaRecorrida = new List<ZonaGrupoDetalle>();
            int fila = 1;
            foreach (var item in poLista)
            {
                string psVendedor = "";

                if (item.CodVendedor != null)
                {
                    psVendedor = loComboVendedor.Where(x => x.Codigo.Contains(item.CodVendedor?.ToString())).Select(x => x.Descripcion).FirstOrDefault();
                }
                else
                {
                    var ds = "";
                }
                string psZona = loComboZona.Where(x => x.Codigo.Contains(item.Code)).Select(x => x.Descripcion).FirstOrDefault();

                if (item.Code == Diccionario.Seleccione || item.CodVendedor?.ToString() == Diccionario.Seleccione)
                {
                    psMsg = string.Format("{0}Verificar si están seleccionados la Zona y el Vendedor en la Fila # {1}\n", psMsg, fila);
                }

                else if (poListaRecorrida.Where(x => x.Code == item.Code  && x.CodVendedor  == item.CodVendedor).Count() > 0)
                {
                    psMsg = string.Format("{0}Eliminar Fila # {1}, Ya existe una parametrización por: {2} y {3}\n", psMsg, fila, psZona, psVendedor);
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
            bsDatos.DataSource = new List<ZonaGrupoDetalle>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsBehavior.Editable = true;


            dgvDatos.Columns["Name"].Visible = false;
            dgvDatos.Columns["NomVendedor"].Visible = false;
            dgvDatos.Columns["IdZonaGrupoDetalle"].Visible = false;
            dgvDatos.Columns["IdZonaGrupo"].Visible = false;
            dgvDatos.Columns["Del"].Visible = false;
            dgvDatos.Columns["Agrupacion"].Visible = false;
            dgvDatos.Columns["ResponsableCobranza"].Visible = false;
            dgvDatos.Columns["Titular"].Visible = false;
            dgvDatos.Columns["Recaudador"].Visible = false;

            dgvDatos.Columns["CodResponsableCobranza"].Visible = false;
            dgvDatos.Columns["DiasProAcepIni"].Visible = false;
            dgvDatos.Columns["DiasProAcepFin"].Visible = false;
            dgvDatos.Columns["DiasProGestIni"].Visible = false;
            dgvDatos.Columns["DiasProGestFin"].Visible = false;
            dgvDatos.Columns["DiasProNoAcepIni"].Visible = false;
            dgvDatos.Columns["DiasProNoAcepFin"].Visible = false;

            dgvDatos.Columns["CodVendedor"].Caption = "Vendedor";
            dgvDatos.Columns["Code"].Caption = "Zona";

            dgvDatos.Columns["CodAgrupacion"].Caption = "Agrupación";
            dgvDatos.Columns["CodResponsableCobranza"].Caption = "Responsable";
            dgvDatos.Columns["CodTitular"].Caption = "Titular";
            dgvDatos.Columns["CodRecaudador"].Caption = "Recaudador";

            
            dgvDatos.Columns["DiasProAcepIni"].Caption = "Días Aceptación Desde";
            dgvDatos.Columns["DiasProAcepFin"].Caption = "Días Aceptación Hasta";
            dgvDatos.Columns["DiasProGestIni"].Caption = "Días Gestión Desde";
            dgvDatos.Columns["DiasProGestFin"].Caption = "Días Gestión Hasta";
            dgvDatos.Columns["DiasProNoAcepIni"].Caption = "Días No Aceptación Desde";
            dgvDatos.Columns["DiasProNoAcepFin"].Caption = "Días No Aceptación Hasta";

            

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);

            dgvDatos.FixedLineWidth = 1;
            dgvDatos.Columns["Code"].Fixed = FixedStyle.Left;
            //dgvDatos.Columns["Proveedor"].Fixed = FixedStyle.Left;
            //dgvDatos.Columns["NumDocumento"].Fixed = FixedStyle.Left;

            //dgvDatos.Columns["CardCode"].Width = 400;
            //dgvDatos.Columns["CodeZona"].Width = 200;
        }

        private void lBuscar()
        { 
            bsDatos.DataSource = loLogicaNegocio.goConsultarPrCobGrupoZonaDetalle();
            gcDatos.DataSource = bsDatos;

            clsComun.gOrdenarColumnasGridFullEditable(dgvDatos);

        }
        #endregion
    }
}
