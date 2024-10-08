﻿using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Cobranza;
using GEN_Entidad.Entidades.Ventas;
using REH_Presentacion.Formularios;
using REH_Presentacion.Ventas.Parametrizadores.PopUp;
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
    public partial class frmPrGrupoExclusion : frmBaseTrxDev
    {
        #region Variables
        clsNRebate loLogicaNegocio = new clsNRebate();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDet = new RepositoryItemButtonEdit();
        private List<Combo> loComboCliente = new List<Combo>();
        bool pbCargado = true;
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmPrGrupoExclusion()
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
                loComboCliente = loLogicaNegocio.goSapConsultaGrupos();
                lCargarEventosBotones();
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboCliente, "ItmsGrpCod", true);
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
                    List<GrupoItemExclusion> poLista = (List<GrupoItemExclusion>)bsDatos.DataSource;

                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de guardar cambios?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string psMsg = loLogicaNegocio.gsGuardarGrupoItemExclusion(poLista, txtAnio.Text, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                var poLista = (List<GrupoItemExclusion>)bsDatos.DataSource;

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
                var poLista = (List<GrupoItemExclusion>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    // Tomamos la entidad de la fila seleccionada
                    var poDetalle = poLista[piIndex].GrupoItemExclusionDetalle == null ? new List<GrupoItemExclusionDetalle>() : poLista[piIndex].GrupoItemExclusionDetalle.ToList();

                    frmZonaDetalle frmComisionDetalle = new frmZonaDetalle();
                    frmComisionDetalle.Detalle = poDetalle;
                    frmComisionDetalle.ShowDialog();

                    if (frmComisionDetalle.pbAcepto)
                    {
                        poLista[piIndex].GrupoItemExclusionDetalle = frmComisionDetalle.Detalle;
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
                ((List<GrupoItemExclusion>)bsDatos.DataSource).LastOrDefault().ItmsGrpCod = 0;
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
            List<GrupoItemExclusion> poLista = (List<GrupoItemExclusion>)bsDatos.DataSource;

            if (txtAnio.Text.Length != 4)
            {
                psMsg = string.Format("Ingrese el año\n", psMsg);
            }

            var poListaRecorrida = new List<GrupoItemExclusion>();
            int fila = 1;
            foreach (var item in poLista)
            {
                string psCliente = loComboCliente.Where(x => x.Codigo.Contains(item.ItmsGrpCod.ToString())).Select(x => x.Descripcion).FirstOrDefault();
                if (item.ItmsGrpCod == 0)
                {
                    psMsg = string.Format("{0}Seleccionar el Grupo en la Fila # {1}\n", psMsg, fila);
                }

                else if (poListaRecorrida.Where(x => x.ItmsGrpCod == item.ItmsGrpCod).Count() > 0)
                {
                    psMsg = string.Format("{0}Eliminar Fila # {1}, Ya existe una parametrización del Grupo: {2}\n", psMsg, fila, psCliente);
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
            bsDatos.DataSource = new List<GrupoItemExclusion>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsBehavior.Editable = true;


            dgvDatos.Columns["CodigoEstado"].Visible = false;
            dgvDatos.Columns["IdGrupoItemExclusion"].Visible = false;
            dgvDatos.Columns["ItmsGrpNam"].Visible = false;
            dgvDatos.Columns["GrupoItemExclusionDetalle"].Visible = false;

            dgvDatos.Columns["ItmsGrpCod"].Caption = "Item - Grupo";
            dgvDatos.Columns["ItmsGrpCod"].Width = 500;

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);
            clsComun.gDibujarBotonGrid(rpiBtnDet, dgvDatos.Columns["Det"], "Detalle", Diccionario.ButtonGridImage.inserttable_16x16);

            txtAnio.KeyDown += new KeyEventHandler(EnterEqualTab);
            txtAnio.KeyPress += new KeyPressEventHandler(SoloNumeros);
        }

        private void lBuscar()
        { 
            bsDatos.DataSource = loLogicaNegocio.goConsultarGrupoItemExclusion(txtAnio.Text);
            gcDatos.DataSource = bsDatos;

        }
        #endregion

        private void txtAnio_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (pbCargado)
                {
                    lBuscar();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
