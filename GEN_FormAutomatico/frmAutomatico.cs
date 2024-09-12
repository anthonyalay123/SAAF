using System;
using System.IO;
using System.Windows.Forms;

namespace VTA_AutomaticoVentas
{
    public partial class frmAutomatico : Form
    {
        public frmAutomatico()
        {
            InitializeComponent();
        }

        private void frmAutomaticoVentas_Load(object sender, EventArgs e)
        {
            
            var path = Application.StartupPath + "\\log.txt";
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("Inicio");

            /// Codigo para generación Automática

            sw.WriteLine("Inicio");
            sw.Close();
            Close();
        }
    }
}
