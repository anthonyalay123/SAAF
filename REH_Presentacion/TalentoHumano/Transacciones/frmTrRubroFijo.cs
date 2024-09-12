using DevExpress.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.TalentoHumano.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 03/02/2022
    /// Formulario para parametrizar los valores en rubros 
    /// </summary>
    public partial class frmTrRubroFijo : frmBaseTrxDev
    {
        #region Variables
        clsNNomina loLogicaNegocio;
        BindingSource bsDatos;
        RepositoryItemButtonEdit rpiBtnDel;
        #endregion

        #region Eventos
        public frmTrRubroFijo()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNNomina();
            bsDatos = new BindingSource();
            rpiBtnDel = new RepositoryItemButtonEdit();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        /// <summary>
        /// Inicializador del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTrAlimentacion_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboRubroEditable(), "CodigoRubro", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboTipoRol(), "CodigoTipoRol", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboIdPersona(), "IdPersonaString", true);
                lConsultar();
                

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Nuevo, Generalmente Limpia el formulario 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Generar, Genera Novedad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                dgvDatos.PostEditor();
                string psMsgValida = ""; //lsEsValido();

                List<RubroFijo> poLista = (List<RubroFijo>)bsDatos.DataSource;

                if (lbEsValido())
                {
                    if (poLista.Count > 0)
                    {
                        string psMsg = loLogicaNegocio.gsGuardarRubroFijo(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
                        if (string.IsNullOrEmpty(psMsg))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lConsultar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMsg, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No existen datos a guardar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                
                 lConsultar();
                

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                bsDatos.AddNew();
                ((List<RubroFijo>)bsDatos.DataSource).LastOrDefault().IdPersonaString = Diccionario.Seleccione;
                ((List<RubroFijo>)bsDatos.DataSource).LastOrDefault().CodigoRubro = Diccionario.Seleccione;
                ((List<RubroFijo>)bsDatos.DataSource).LastOrDefault().CodigoTipoRol = Diccionario.Seleccione;
                dgvDatos.Focus();
                dgvDatos.ShowEditor();
                dgvDatos.UpdateCurrentRow();
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
                var poLista = (List<RubroFijo>)bsDatos.DataSource;

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
                MessageBox.Show(ex.Message);
            }
        }
        
        #endregion

        #region Métodos
        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnBuscar"] != null) tstBotones.Items["btnBuscar"].Click += btnBuscar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;

            bsDatos = new BindingSource();
            bsDatos.DataSource = new List<RubroFijo>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsBehavior.Editable = true;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;
            dgvDatos.Columns["IdRubroFijo"].Visible = false;
            dgvDatos.Columns["IdPersona"].Visible = false;
            dgvDatos.Columns["Identificacion"].Visible = false;
            dgvDatos.Columns["Nombre"].Visible = false;

            dgvDatos.Columns["Identificacion"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Nombre"].OptionsColumn.AllowEdit = false;

            dgvDatos.Columns["IdPersonaString"].Caption = "Nombre";
            dgvDatos.Columns["CodigoRubro"].Caption = "Rubro";
            dgvDatos.Columns["CodigoTipoRol"].Caption = "Tipo Rol";

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 50);


            //for (int i = 0; i < dgvDatos.Columns.Count; i++)
            //{

            //    if (dgvDatos.Columns[i].ColumnType == typeof(decimal))
            //    {
            //        var psNameColumn = dgvDatos.Columns[i].FieldName;

            //        //dgvParametrizaciones.Columns[i].UnboundType = UnboundColumnType.Decimal;
            //        dgvDatos.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //        dgvDatos.Columns[i].DisplayFormat.FormatString = "c2";
            //        dgvDatos.Columns[psNameColumn].Summary.Add(SummaryItemType.Sum, psNameColumn, "{0:c2}");

            //        GridGroupSummaryItem item1 = new GridGroupSummaryItem();
            //        item1.FieldName = psNameColumn;
            //        item1.SummaryType = SummaryItemType.Sum;
            //        item1.DisplayFormat = "{0:n2}";
            //        item1.ShowInGroupColumnFooter = dgvDatos.Columns[psNameColumn];
            //        dgvDatos.GroupSummary.Add(item1);

            //    }

            //}

        }

        private bool lbEsValido()
        {
            List<RubroFijo> poLista = (List<RubroFijo>)bsDatos.DataSource;

            if (poLista.Count() == 0)
            {
                XtraMessageBox.Show("No existen registros para guardar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            foreach (var item in poLista)
            {
                if (item.CodigoRubro == Diccionario.Seleccione || item.CodigoTipoRol == Diccionario.Seleccione || item.IdPersonaString == Diccionario.Seleccione)
                {
                    XtraMessageBox.Show("Debe seleccionar Tipo Rol / Nombre / Rubro. Son obligatorios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        public void lConsultar()
        {

            var poLIsta = loLogicaNegocio.goConsultarListaRubroFijo();
            bsDatos.DataSource = poLIsta;
            gcDatos.DataSource = bsDatos;
            //clsComun.gOrdenarColumnasGridFullEditable(dgvDatos);
        }

        private void lLimpiar(bool tbTodo = true)
        {

            var poLIsta = new List<RubroFijo>();
            bsDatos.DataSource = poLIsta;
            gcDatos.DataSource = poLIsta;
        }
        #endregion

       
    }

    
}
