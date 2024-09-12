using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Entidad.Entidades.SHEQ;
using REH_Negocio.Seguridad;
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

namespace REH_Presentacion.Seguridad
{
    public partial class frmPaAsignarAccionPorMenu : frmBaseTrxDev
    {
        clsNMenuAccionPerfil loLogicaNegocio = new clsNMenuAccionPerfil();
        public frmPaAsignarAccionPorMenu()
        {
            InitializeComponent();
            LoadTreeListData();

            treeList1.OptionsBehavior.Editable = true;  
            treeList1.OptionsView.ShowCheckBoxes = true;

            cmbPerfil.EditValueChanged += cmbPerfil_EditValueChanged;

            treeList1.AfterCheckNode += TreeList1_AfterCheckNode;
        }

        private void frmPaAsignarMenuUsuario_Load(object sender, EventArgs e)
        {
            lCargarEventosBotones();
            clsComun.gLLenarCombo(ref cmbPerfil, loLogicaNegocio.goConsultarComboPerfil(), true);
        }

        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
        }

        private void TreeList1_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            // If the node's checked state is changed, update its children accordingly
            SelecionarMenusHijosConPadre(e.Node, e.Node.Checked);
        }

        private void SelecionarMenusHijosConPadre(TreeListNode node, bool isChecked)
        {
            foreach (TreeListNode childNode in node.Nodes)
            {
                childNode.Checked = isChecked;
                // Recursively update the check state for all children
                SelecionarMenusHijosConPadre(childNode, isChecked);
            }
        }

        private void LoadTreeListData()
        {
            var menuItems = loLogicaNegocio.goConsultarMenuAccion();

            treeList1.DataSource = menuItems;
            treeList1.ParentFieldName = "IdMenuPadre";
            treeList1.KeyFieldName = "IdMenu";

            treeList1.Columns["IdAccion"].Visible = false;
            treeList1.OptionsBehavior.Editable = false;

            treeList1.Columns["Nombre"].OptionsColumn.ReadOnly = true;
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                var menus = ObtenerMenusAccionSelecionados(treeList1.Nodes);
                int perfil = int.Parse(cmbPerfil.EditValue.ToString());

                string psMsg = loLogicaNegocio.gsGuardarMenusAccionParaPerfil(perfil, menus, clsPrincipal.gsUsuario);

                XtraMessageBox.Show("Datos guardados exitosamente", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<int> ObtenerMenusAccionSelecionados(TreeListNodes nodes)
        {
            List<int> checkedMenuIds = new List<int>();

            foreach (TreeListNode node in nodes)
            {
                if (node.Checked)
                {
                    checkedMenuIds.Add((int)node.GetValue("IdMenu"));
                }

                if (node.HasChildren)
                {
                    checkedMenuIds.AddRange(ObtenerMenusAccionSelecionados(node.Nodes));
                }
            }

            return checkedMenuIds;
        }

        private void cmbPerfil_EditValueChanged(object sender, EventArgs e)
        {
            var idMenu = loLogicaNegocio.goConsultarMenuPorPerfil(int.Parse(cmbPerfil.EditValue.ToString()));

            foreach (TreeListNode node in treeList1.Nodes)
            {
                CheckNode(node, idMenu);
            }
        }

        private void CheckNode(TreeListNode node, List<int> idMenu)
        {
            if (idMenu.Contains((int)node.GetValue("IdMenu")))
            {
                node.Checked = true;
            }
            else
            {
                node.Checked = false;
            }

            foreach (TreeListNode childNode in node.Nodes)
            {
                CheckNode(childNode, idMenu);
            }
        }
    }
}
