using DevExpress.XtraEditors;
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using GEN_Entidad.Entidades.SHEQ;
using GEN_Negocio;
using REH_Presentacion.Formularios;
using reporte;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.SHEQ.Reportes
{
    public partial class frmListadoItems : frmBaseTrxDev
    {
        BindingSource bsDatos = new BindingSource();
        clsNBase loLogicaNegocio = new clsNBase();
        public frmListadoItems()
        {
            InitializeComponent();
        }

        private void frmListadoItems_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
                lConsultar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();

            if (tstBotones.Items["btnRefrescar"] != null) tstBotones.Items["btnRefrescar"].Click += btnRefrescar_Click;
            if (tstBotones.Items["btnExportar"] != null) tstBotones.Items["btnExportar"].Click += btnExportarExcel_Click;

            bsDatos.DataSource = new List<ItemEPP>();
            gcDatos.DataSource = bsDatos;

            dgvDatos.Columns["IdItemEPP"].Caption = "Item";
            dgvDatos.Columns["IdItemEPP"].Width = 5;
            dgvDatos.Columns["Descripcion"].Width = 150;
            dgvDatos.Columns["Costo"].Width = 10;
            dgvDatos.Columns["GrabaIva"].Width = 10;

            dgvDatos.Columns["IdItemEPP"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Descripcion"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["Costo"].OptionsColumn.AllowEdit = false;
            dgvDatos.Columns["GrabaIva"].OptionsColumn.AllowEdit = false;

        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            bsDatos.DataSource = new List<ItemEPP>();
            gcDatos.DataSource = bsDatos;
            lConsultar();
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {

                dgvDatos.PostEditor();

                List<GEN_Entidad.Entidades.SHEQ.ItemEPP> poLista = bsDatos.DataSource as List<GEN_Entidad.Entidades.SHEQ.ItemEPP>;
                if (poLista != null && poLista.Count > 0)
                {
                    DataTable dataTable = loLogicaNegocio.ConvertToDataTable(poLista);

                    string psFilter = "Files(*.xlsx;)|*.xlsx;";
                    clsComun.gSaveFile(gcDatos, "ITEM EPP.xlsx", psFilter);
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a exportar.", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lConsultar()
        {
            var poObject = loLogicaNegocio.goConsultarItemEpp();

            bsDatos.DataSource = poObject;
            dgvDatos.RefreshData();

        }

    }
}
