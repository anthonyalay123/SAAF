using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Formularios
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 17/01/2020
    /// Formulario Genérico para Maestros y parametrizadores
    /// </summary>
    public partial class frmBaseCopy : Form
    {
        #region Eventos
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public frmBaseCopy()
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
            txtDescripcion.Focus();
        }
        /// <summary>
        /// Método Genérico que inicializa el evento de botón "Nuevo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void btnNuevo_Click(object sender, EventArgs e)
        {
            int prueb = 0;
        }

        /// <summary>
        /// Método Genérico que inicializa el evento de botón "Grabar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void btnGrabar_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Método Genérico que inicializa el evento de botón "Buscar"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void btnBuscar_Click(object sender, EventArgs e)
        {

        }

        #endregion


    }
}
