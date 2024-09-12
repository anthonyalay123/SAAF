using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using GEN_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Comun
{
    public partial class frmSeleccionarEmpleados : Form
    {
        clsNBase loLogicaNegocio = new clsNBase();
        public List<string> EmpleadosSeleccionados { get; set; }

       public frmSeleccionarEmpleados()
        {
            InitializeComponent();
            EmpleadosSeleccionados = new List<string>();
            CargarEmpleados();

            dgvDatos.OptionsSelection.MultiSelect = true;
            dgvDatos.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
            dgvDatos.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DevExpress.Utils.DefaultBoolean.True;
            dgvDatos.OptionsSelection.CheckBoxSelectorColumnWidth = 40;
            dgvDatos.OptionsView.ShowAutoFilterRow = true;
        }

        private void CargarEmpleados()
        {
            DataTable dtEmpleados = loLogicaNegocio.goConsultaDataTable("EXEC VTASPEMPLEADOSCORREO");

            gcDatos.DataSource = dtEmpleados;
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            EmpleadosSeleccionados.Clear(); 

            //foreach (int index in dgvDatos.GetSelectedRows())
            //{
            //    DataRow fila = dgvDatos.GetDataRow(index);
            //    if (fila != null)
            //    {
            //        EmpleadosSeleccionados.Add(fila["CorreoLaboral"].ToString());
            //    }
            //}

            EmpleadosSeleccionados.Add("pasante_sistemas@afecor.com");
            EmpleadosSeleccionados.Add("varevalo@afecor.com");
            EmpleadosSeleccionados.Add("jordonez@afecor.com");

            if (EmpleadosSeleccionados.Count > 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                XtraMessageBox.Show("No se seleccionaron empleados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        


    }
}
