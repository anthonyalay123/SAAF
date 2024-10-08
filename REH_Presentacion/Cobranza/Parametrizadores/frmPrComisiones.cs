﻿using COB_Negocio;
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
    /// Formulario parametrizador de porcentajes de comisiones
    /// </summary>
    public partial class frmPrComisiones : frmBaseTrxDev
    {
        #region Variables
        clsNComisiones loLogicaNegocio = new clsNComisiones();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();

        private List<Combo> loComboZona = new List<Combo>();
        private List<Combo> loComboRango = new List<Combo>();
        #endregion

        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmPrComisiones()
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
                loComboZona = loLogicaNegocio.goConsultarZonasSAP();
                loComboRango = loLogicaNegocio.goConsultarToleranciaContado();

                lCargarEventosBotones();

                clsComun.gLLenarComboGrid(ref dgvDatos, loComboZona, "CodeZona", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboTipoCodigoComision(), "CodigoComision", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboRango, "CodigoToleranciaContado", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboGrupoFactura(), "CodigoGrupoFactura", true);
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
                    List<PrComisiones> poLista = (List<PrComisiones>)bsDatos.DataSource;

                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de guardar cambios?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string psMsg = loLogicaNegocio.gsGuardarPrComisiones(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                var poLista = (List<PrComisiones>)bsDatos.DataSource;

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
                ((List<PrComisiones>)bsDatos.DataSource).LastOrDefault().CodigoGrupoFactura = Diccionario.Seleccione;
                ((List<PrComisiones>)bsDatos.DataSource).LastOrDefault().CodigoComision = Diccionario.Seleccione;
                ((List<PrComisiones>)bsDatos.DataSource).LastOrDefault().CodeZona = Diccionario.Seleccione;
                ((List<PrComisiones>)bsDatos.DataSource).LastOrDefault().CodigoToleranciaContado = Diccionario.Seleccione;
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
            List<PrComisiones> poLista = (List<PrComisiones>)bsDatos.DataSource;

            var poListaRecorrida = new List<PrComisiones>();
            int fila = 1;
            foreach (var item in poLista)
            {
                string psZona = loComboZona.Where(x => x.Codigo.Contains(item.CodeZona)).Select(x => x.Descripcion).FirstOrDefault();
                string psRango = loComboRango.Where(x => x.Codigo.Contains(item.CodigoToleranciaContado)).Select(x => x.Descripcion).FirstOrDefault();

                if (item.CodigoComision == Diccionario.Seleccione || item.CodeZona == Diccionario.Seleccione || item.CodigoToleranciaContado == Diccionario.Seleccione || item.CodigoGrupoFactura == Diccionario.Seleccione)
                {
                    psMsg = string.Format("{0}Verificar si están seleccionados el Tipo de Comisión, la Zona, el Rango y Código Vendedor en la Fila # {1}\n", psMsg, fila);
                }

                else if (poListaRecorrida.Where(x => x.CodigoComision == item.CodigoComision && x.CodeZona == item.CodeZona && x.CodigoToleranciaContado == item.CodigoToleranciaContado && x.CodigoGrupoFactura == item.CodigoGrupoFactura).Count() > 0)
                {
                    psMsg = string.Format("{0}Eliminar Fila # {1}, Ya existe una parametrización por: {2}, {3}, {4} y {5}\n", psMsg, fila, item.CodigoComision,psZona,psRango, item.CodigoGrupoFactura);
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
            bsDatos.DataSource = new List<PrComisiones>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsBehavior.Editable = true;


            //dgvDatos.Columns["CodigoEstado"].Visible = false;
            dgvDatos.Columns["NameZona"].Visible = false;
            dgvDatos.Columns["Id"].Visible = false;

            dgvDatos.Columns["CodigoComision"].Caption = "Tipo Comisión";
            dgvDatos.Columns["CodeZona"].Caption = "Zona";
            dgvDatos.Columns["CodigoToleranciaContado"].Caption = "Rango";
            dgvDatos.Columns["CodigoGrupoFactura"].Caption = "Código Vendedor";

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);

            clsComun.gOrdenarColumnasGrid(dgvDatos);

            dgvDatos.Columns["CodeZona"].Width = 150;
            dgvDatos.Columns["CodigoToleranciaContado"].Width = 150;
            //dgvDatos.Columns["CodigoGrupoFactura"].Width = 100;

        }

        private void lBuscar()
        { 
            bsDatos.DataSource = loLogicaNegocio.goConsultarPrComisiones();
            gcDatos.DataSource = bsDatos;

        }
        #endregion
    }
}
