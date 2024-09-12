using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
using REH_Presentacion.Comun;
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
using VTA_Negocio;

namespace REH_Presentacion.Ventas.Parametrizadores
{
    public partial class frmGrupoTipoProducto : frmBaseTrxDev
    {
        #region Variables
        clsNGrupoTipoProducto loLogicaNegocio = new clsNGrupoTipoProducto();
        BindingSource bsDatos = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        RepositoryItemButtonEdit rpiBtnDet = new RepositoryItemButtonEdit();
        private List<Combo> loComboZona = new List<Combo>();
        private List<Combo> loComboGrupo = new List<Combo>();
        private List<Combo> loComboTipoProducto = new List<Combo>();
        #endregion

        public frmGrupoTipoProducto()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        private void frmGrupoTipoProductocs_Load(object sender, EventArgs e)
        {
            try
            {

                loComboZona = loLogicaNegocio.goConsultarZonasSAAF();
                loComboGrupo = loLogicaNegocio.goSapConsultaGrupos();
                loComboTipoProducto = loLogicaNegocio.goConsultarComboTipoProducto();
                lCargarEventosBotones();
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboZona, "IdZona", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboGrupo , "ItmsGrpCod", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loComboTipoProducto , "CodigoTipoProducto", true);
                clsComun.gLLenarComboGrid(ref dgvDatos, loLogicaNegocio.goConsultarComboTipoProducto(), "General", true);
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
                    List<GrupoTipoProducto> poLista = (List<GrupoTipoProducto>)bsDatos.DataSource;

                    DialogResult dialogResult = XtraMessageBox.Show("¿Está seguro de guardar cambios?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string psMsg = loLogicaNegocio.gsGuardarTipoProducto(poLista, clsPrincipal.gsUsuario, clsPrincipal.gsTerminal);
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
                var poLista = (List<GrupoTipoProducto>)bsDatos.DataSource;

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
                var poLista = (List<GrupoTipoProducto>)bsDatos.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    //var poDetalle = poLista[piIndex].FacturaComisionDetalle == null ? new List<FacturaComisionDetalle>() : poLista[piIndex].FacturaComisionDetalle.ToList();

                    //frmComisionDetalle frmComisionDetalle = new frmComisionDetalle();
                    //frmComisionDetalle.ComisionDetalle = poDetalle.Select(x => new ComisionDetalleBase()
                    //{
                    //    CodigoComision = x.CodigoComision,
                    //    CodeZona = x.CodeZona,
                    //    CodigoToleraciaContado = x.CodigoToleraciaContado,
                    //    PorcentajeComision = x.PorcentajeComision,
                    //    IdCab = x.IdFacturaComision,
                    //    IdDet = x.IdFacturaComisionDetalle
                    //}).ToList();
                    //frmComisionDetalle.ShowDialog();

                    //if (frmComisionDetalle.pbAcepto)
                    //{
                    //    poLista[piIndex].FacturaComisionDetalle = frmComisionDetalle.ComisionDetalle.Select(x => new FacturaComisionDetalle()
                    //    {
                    //        CodigoComision = x.CodigoComision,
                    //        CodeZona = x.CodeZona,
                    //        CodigoToleraciaContado = x.CodigoToleraciaContado,
                    //        PorcentajeComision = x.PorcentajeComision,
                    //        IdFacturaComision = x.IdCab,
                    //        IdFacturaComisionDetalle = x.IdDet
                    //    }).ToList();
                    //}
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

                frmCombo frmTipoComision = new frmCombo();
                frmTipoComision.lsNombre = "Grupo";
                frmTipoComision.loCombo = loComboGrupo;
                frmTipoComision.ShowDialog();

                if (frmTipoComision.lsSeleccionado != null)
                {
                    string psCodeGrupo = frmTipoComision.lsSeleccionado;

                    foreach (var item in loComboZona.Where(x => x.Codigo != Diccionario.Seleccione))
                    {
                        bsDatos.AddNew();
                        ((List<GrupoTipoProducto>)bsDatos.DataSource).LastOrDefault().IdZona = int.Parse(item.Codigo);
                        ((List<GrupoTipoProducto>)bsDatos.DataSource).LastOrDefault().ItmsGrpCod = short.Parse(psCodeGrupo);
                        string psTipoProducto = "CIC";

                        if (item.Descripcion.Contains("BAN"))
                        {
                            psTipoProducto = "BAN";
                        }
                        else if (item.Descripcion.Contains("GUAYA"))
                        {
                            psTipoProducto = "GYE";
                        }
                        else if (item.Descripcion.Contains("SIE") || item.Descripcion.Contains("AUSTRO") ||
                            item.Descripcion.Contains("FLORES") || item.Descripcion.Contains("MACAR") ||
                                item.Descripcion.Contains("QUIT"))
                        {
                            psTipoProducto = "SIE";
                        }
                        else if (item.Descripcion.Contains("KIT"))
                        {
                            psTipoProducto = "KIT";
                        }

                        ((List<GrupoTipoProducto>)bsDatos.DataSource).LastOrDefault().CodigoTipoProducto = psTipoProducto;
                        ((List<GrupoTipoProducto>)bsDatos.DataSource).LastOrDefault().General = psTipoProducto;
                        dgvDatos.Focus();
                        dgvDatos.FocusedRowHandle = dgvDatos.RowCount - 1;
                        dgvDatos.FocusedColumn = dgvDatos.Columns[0];
                        dgvDatos.ShowEditor();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Valida datos del formulario previo a Guardarlos
        /// </summary>
        /// <returns></returns>
        private string lsEsValido()
        {

            string psMsg = string.Empty;
            dgvDatos.PostEditor();
            List<GrupoTipoProducto> poLista = (List<GrupoTipoProducto>)bsDatos.DataSource;

            var poListaRecorrida = new List<GrupoTipoProducto>();
            int fila = 1;
            foreach (var item in poLista)
            {
                string psZona = loComboZona.Where(x => x.Codigo == item.IdZona.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                string psGrupo = loComboGrupo.Where(x => x.Codigo == item.ItmsGrpCod.ToString()).Select(x => x.Descripcion).FirstOrDefault();
                string psTipoProducto = loComboTipoProducto.Where(x => x.Codigo == item.CodigoTipoProducto).Select(x => x.Descripcion).FirstOrDefault();

                if (item.IdZona.ToString() == Diccionario.Seleccione)
                {
                    psMsg = string.Format("{0}Seleccionar Zona en la Fila # {1}\n", psMsg, fila);
                }

                if (item.ItmsGrpCod.ToString() == Diccionario.Seleccione)
                {
                    psMsg = string.Format("{0}Seleccionar Grupo en la Fila # {1}\n", psMsg, fila);
                }

                if (item.CodigoTipoProducto == Diccionario.Seleccione)
                {
                    psMsg = string.Format("{0}Seleccionar Tipo de Producto en la Fila # {1}\n", psMsg, fila);
                }

                if (item.General == Diccionario.Seleccione)
                {
                    psMsg = string.Format("{0}Seleccionar opción General en la Fila # {1}\n", psMsg, fila);
                }

                else if (poListaRecorrida.Where(x => x.IdZona == item.IdZona && x.ItmsGrpCod == item.ItmsGrpCod).Count() > 0)
                {
                    psMsg = string.Format("{0}Eliminar Fila # {1}, Ya existe una parametrización por la Zona: {2}, y el Grupo: {3}\n", psMsg, fila, psZona, psGrupo);
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

            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnConsultar_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;

            bsDatos = new BindingSource();
            bsDatos.DataSource = new List<GrupoTipoProducto>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.OptionsBehavior.Editable = true;


            dgvDatos.Columns["IdGrupoItemTipoProductoSaaf"].Visible = false;
            
            dgvDatos.Columns["ItmsGrpCod"].Caption = "Producto";
            dgvDatos.Columns["CodigoTipoProducto"].Caption = "Grupo Tipo Producto";
            dgvDatos.Columns["General"].Caption = "Grupo General";
            dgvDatos.Columns["IdZona"].Caption = "Zona SIRA";

            clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDatos.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16);
            //clsComun.gDibujarBotonGrid(rpiBtnDet, dgvDatos.Columns["Det"], "Detalle", Diccionario.ButtonGridImage.inserttable_16x16);

            dgvDatos.Columns["IdZona"].Width = 200;
            dgvDatos.Columns["ItmsGrpCod"].Width = 200;
            dgvDatos.Columns["CodigoTipoProducto"].Width = 150;
            dgvDatos.Columns["General"].Width = 100;

        }

        private void lBuscar()
        {
            bsDatos.DataSource = loLogicaNegocio.goConsultarGrupoTipoProducto();
            gcDatos.DataSource = bsDatos;

        }
    }
}
