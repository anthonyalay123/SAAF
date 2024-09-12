using DevExpress.XtraEditors;
using REH_Negocio;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace REH_Presentacion.Formularios
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 17/01/2020
    /// Formulario Genérico para Maestros y parametrizadores
    /// </summary>
    public partial class frmBaseDev : Form
    {
        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmBaseDev()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Inicializa eventos del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmBase_Load(object sender, EventArgs e)
        {

            try
            {

                txtDescripcion.Focus();

                var estado = new[]
                    {
                    new {Text="Activo",Value="A"},
                    new {Text="Inactivo",Value="I"}
                };

                cmbEstado.Properties.DataSource = estado;
                cmbEstado.Properties.ValueMember = "Value";
                cmbEstado.Properties.DisplayMember = "Text";
                cmbEstado.Properties.PopulateColumns();
                cmbEstado.Properties.Columns["Value"].Visible = false;
                if ((cmbEstado.Properties.DataSource as IList).Count > 0) cmbEstado.ItemIndex = 0;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Método Genérico que inicializa el evento de botón "Salir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Método para que cuando de enter realice un tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EnterEqualTab(object sender, KeyEventArgs e)
        {
            clsComun.gEnterEqualTab(sender, e);
        }

        /// <summary>
        /// Método que solo te permite ingresar números
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            clsComun.gSoloNumeros(sender, e);
        }

        /// <summary>
        /// Método que solo te permite ingresar números con decimales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SoloNumerosSimbolo(object sender, KeyPressEventArgs e)
        {
            clsComun.gSoloNumerosSimbolo(sender, e);
        }

        /// <summary>
        /// Métido que solo te permite ingresar letras
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SoloLetras(object sender, KeyPressEventArgs e)
        {
            clsComun.gSoloLetras(sender, e);
        }

        /// <summary>
        /// Metodo que inhabilia y habilitia el custom de un datetimepicker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FechaValue_Changed(object sender, EventArgs e)
        {
            clsComun.gFechaValue_Changed(sender, e);
        }

        /// <summary>
        /// Crea dinámicamente los botones al formulario maestro que se desea abrir
        /// </summary>
        public void gCrearBotones()
        {
            // Se obtiene el Id del Menú
            int pIdMenu = int.Parse(Tag.ToString().Split(',')[0]);
            //var poLista =  .gsAccionPerfil.Where(x => x.IdMenu == pIdMenu).OrderBy(x => x.Orden).ToList();
            var poLista = new clsNUsuario().goAccionPerfil(clsPrincipal.gIdPerfil, pIdMenu).OrderBy(x => x.Orden).ToList();
            foreach (var item in poLista)
            {
                var button = new ToolStripButton();
                string ruta = Application.StartupPath + "\\Imagenes\\" + item.Icono;
                if (File.Exists(ruta)) button.Image = Image.FromFile(ruta);
                button.ImageTransparentColor = Color.Magenta;
                button.Name = item.NombreControl;
                button.Size = new Size(49, 35);
                if (item.MostrarTexto) button.Text = item.NombreAccion;
                if (item.NombreControl == "btnSalir") button.Click += new EventHandler(btnSalir_Click);
                tstBotones.Items.Add(button);
            }
        }

        #endregion
    }
}
