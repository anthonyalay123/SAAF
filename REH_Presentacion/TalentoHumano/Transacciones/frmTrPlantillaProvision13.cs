using DevExpress.XtraGrid.Views.Grid;
using REH_Negocio;
using REH_Presentacion.Comun;
using REH_Presentacion.Formularios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GEN_Entidad;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;
using DevExpress.XtraEditors;

namespace REH_Presentacion.Transacciones
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 31/08/2020
    /// Formulario para Carga de Plantilla Provisión
    /// </summary>
    public partial class frmTrPlantillaProvision13 : frmBaseTrxDev
    {

        #region Variables
        clsNPlantilla loLogicaNegocio;
        #endregion

        #region Eventos
        public frmTrPlantillaProvision13()
        {
            InitializeComponent();
            loLogicaNegocio = new clsNPlantilla();
        }

        private void frmTrNomina_Load(object sender, EventArgs e)
        {
            try
            {
                lCargarEventosBotones();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Evento del botón Nuevo, Generalmente Limpia el formulario 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                lLimpiar();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// Evento del botón Buscar, Consulta Registros guardados en BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, EventArgs e)
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
        /// Evento del botón Generar, Genera Novedad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                dgvDatos.PostEditor();
                
                List<PlantillaProvision> poLista = (List<PlantillaProvision>)bsDatos.DataSource;
                if (poLista.Count > 0)
                {
                   
                    if (lbEsValido())
                    {
                        string psMensaje = loLogicaNegocio.gImportarProvisionDecimoTercero(clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, poLista);
                        if (string.IsNullOrEmpty(psMensaje))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(psMensaje, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("No es posible importar datos!, Faltan maestros", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existen datos a importar", Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Evento del botón Importar, Carga Datos en formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                List<PlantillaProvision> poLista = new List<PlantillaProvision>();

                OpenFileDialog ofdRuta = new OpenFileDialog();
                ofdRuta.Title = "Seleccione Archivo";
                //(*.jpg; *.jpeg; *.png)| *.jpg; *.jpeg; *.png; "
                ofdRuta.Filter = "Files(*.xls; *.xlsx;)|*.xls; *.xlsx;";

                if (ofdRuta.ShowDialog() == DialogResult.OK)
                {
                    if (!ofdRuta.FileName.Equals(""))
                    {
                        DataTable dt = clsComun.ConvertExcelToDataTable(ofdRuta.FileName);

                        foreach(DataRow item in dt.Rows)
                        {
                            PlantillaProvision poItem = new PlantillaProvision();
                            poItem.Identificacion = item[0].ToString().Trim();
                            poItem.Nombre = item[1].ToString().Trim();
                            poItem.Anio = int.Parse(item[2].ToString().Trim());
                            poItem.Mes = int.Parse(item[3].ToString().Trim());
                            poItem.Valor = Math.Round(decimal.Parse(item[4].ToString().Trim()),2, MidpointRounding.AwayFromZero);
                            poItem.PagadoNomina = item[5].ToString().Trim().ToUpper() == "SI" ? true : false ;

                            poLista.Add(poItem);
                        }

                        bsDatos.DataSource = poLista.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa los botones del formulario si tiene permiso el usuario
        /// </summary>
        private void lCargarEventosBotones()
        {
            gCrearBotones();
            if (tstBotones.Items["btnNuevo"] != null) tstBotones.Items["btnNuevo"].Click += btnNuevo_Click;
            if (tstBotones.Items["btnGrabar"] != null) tstBotones.Items["btnGrabar"].Click += btnGrabar_Click;
            if (tstBotones.Items["btnImportar"] != null) tstBotones.Items["btnImportar"].Click += btnImportar_Click;

            bsDatos.DataSource = new List<PlantillaProvision>();
            gcDatos.DataSource = bsDatos;
        }

        private bool lbEsValido()
        {
            return true;
            
        }

        private void lLimpiar()
        {
            bsDatos.DataSource = null;
        }

        private void lBuscar()
        {
            //bsDatos.DataSource = loLogicaNegocio.gConsultarFondoReserva(int.Parse(cmbPeriodo.EditValue.ToString()));
        }
        
        #endregion

    }
}
