using CRE_Negocio.Transacciones;
using DevExpress.Xpo.DB;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
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

namespace REH_Presentacion.Credito.Reportes
{
    public partial class frmmRpResumenRequerimiento : Form
    {
        clsNProcesoCredito loLogicaNegocio = new clsNProcesoCredito();

        public frmmRpResumenRequerimiento()
        {
            InitializeComponent();
            bsDatos = new BindingSource();
        }

        private void frmmRpResumenRequerimiento_Load(object sender, EventArgs e)
        {
            try
            {
                dtpFechaDesde.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpFechaHasta.DateTime = DateTime.Now;

                List<spRequerimientos> poLista = new List<spRequerimientos>();

                bsDatos.DataSource = poLista;
                gcDatos.DataSource = bsDatos;

                dgvDatos.Columns["IdProcesoCredito"].Visible = false;
                //dgvDatos.Columns["RequerimientoDetalle"].Visible = false;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                List<spRequerimientos> poLista = loLogicaNegocio.goConsultarRequerimientos(dtpFechaDesde.DateTime,dtpFechaHasta.DateTime);

                foreach (var item in poLista)
                {

                    var poListaDetalle = loLogicaNegocio.goConsultarRequerimientoDetalle(item.IdProcesoCredito);

                    foreach (var det in poListaDetalle)
                    {
                        var poDet = new RequerimientoDetalle();
                        poDet.IdProcesoCreditoDetalle = det.IdProcesoCreditoDetalle;
                        poDet.IdProcesoCredito = det.IdProcesoCredito;
                        poDet.Documento = det.Documento;
                        poDet.Estado = det.Estado;
                        poDet.FechaCompromiso = det.FechaCompromiso;

                        var dt = loLogicaNegocio.goConsultarComentarioAprobadores(Diccionario.Tablas.Transaccion.Checklist, det.IdProcesoCreditoDetalle);
                        
                        foreach (DataRow dtRows in dt.Rows)
                        {
                            RequerimientoDetalleTrazabilidad poTra = new RequerimientoDetalleTrazabilidad();
                            //poTra.IdProcesoCreditoDetalle = det.IdProcesoCreditoDetalle;
                            poTra.Usuario = dtRows[1].ToString();
                            poTra.Fecha = Convert.ToDateTime(dtRows[2].ToString());
                            poTra.EstadoAnterior = dtRows[3].ToString();
                            poTra.EstadoPosterior = dtRows[4].ToString();
                            poTra.Comentario = dtRows[5].ToString();

                            poDet.RequerimientoDetalleTrazabilidad.Add(poTra);
                        }

                        item.RequerimientoDetalle.Add(poDet);
                    }
                }

                bsDatos.DataSource = poLista;
                gcDatos.DataSource = bsDatos;

                //var sd = gcDatos.LevelTree.Nodes[0];// .LevelTree["Level1"];
                //var sde = gcDatos.LevelTree.Parent;// .LevelTree["Level1"];
                //var asdad = gcDatos.LevelTree.Level;// .LevelTree["Level1"];
                //var adfsde = gcDatos.LevelTree.Parent.Nodes;// .LevelTree["Level1"];
                //GridView level1View = (GridView);

                //GridView gv = (GridView)dgvDatos.GetDetailView(7, 0);

                dgvDatos.Columns["Zona"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["Usuario"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["RTC"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["Cliente"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["TipoSN"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["Grupo"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["FechaApertura"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["EstatusSeguro"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["PlazoSap"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["CupoSap"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["FvctoCedulaTitular"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["AnioSolCredito"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["FechaUltActualizacionRuc"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["FvctoNombramRepLegal"].OptionsColumn.ReadOnly = true;
                //dgvDatos.Columns["Fecha"].OptionsColumn.ReadOnly = true;
                //dgvDatos.Columns["FechaVencimiento"].OptionsColumn.ReadOnly = true;
                //dgvDatos.Columns["DiasPorVencer"].OptionsColumn.ReadOnly = true;
                dgvDatos.Columns["TipoRequerimiento"].OptionsColumn.ReadOnly = true;

                //GridView level1View = gcDatos.LevelTree.Nodes[0] as GridView;


                string ps = "1";

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            dgvDatos.PostEditor();
            List<spRequerimientos> poLista = (List<spRequerimientos>)bsDatos.DataSource;
            if (poLista != null && poLista.Count > 0)
            {
                string psFilter = "Files(*.xlsx;)|*.xlsx;";
                clsComun.gSaveFile(gcDatos, Text + ".xlsx", psFilter);
            }
            else
            {
                XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgvDatos_Click(object sender, EventArgs e)
        {
            GridView gridView = (GridView)dgvDatos.GetDetailView(dgvDatos.FocusedRowHandle, 0);
            if (gridView != null)
            {
                //Grid level 1
                if (gridView.Columns["Documento"] != null)
                {
                    lColumnasLevel1(gridView);
                }
            }
        }

        private void gcDatos_Click(object sender, EventArgs e)
        {
            try
            {
                GridControl grid = sender as GridControl;
                Point p = new Point(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);
                GridView gridView = grid.GetViewAt(p) as GridView;

                if (gridView != null)
                {
                    //Grid level 1
                    if (gridView.Columns["Documento"] != null)
                    {
                        lColumnasLevel1(gridView);
                    }
                    //Grid level 1
                    else if (gridView.Columns["EstadoAnterior"] != null)
                    {
                        gridView.Columns["Usuario"].OptionsColumn.ReadOnly = true;
                        gridView.Columns["Fecha"].OptionsColumn.ReadOnly = true;
                        gridView.Columns["EstadoAnterior"].OptionsColumn.ReadOnly = true;
                        gridView.Columns["EstadoPosterior"].OptionsColumn.ReadOnly = true;
                        gridView.Columns["Comentario"].OptionsColumn.ReadOnly = true;

                        //gridView.Columns["Id"].Visible = false;
                        //gridView.Columns["IdProcesoCreditoDetalle"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lColumnasLevel1(GridView gridView)
        {
            gridView.Columns["Documento"].OptionsColumn.ReadOnly = true;
            gridView.Columns["Estado"].OptionsColumn.ReadOnly = true;
            gridView.Columns["FechaCompromiso"].OptionsColumn.ReadOnly = true;

            gridView.Columns["IdProcesoCreditoDetalle"].Visible = false;
            gridView.Columns["IdProcesoCredito"].Visible = false;
        }
    }
}
