using CRE_Negocio.Transacciones;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Entidad.Entidades.Credito;
using REH_Presentacion.Comun;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Credito.PopUp
{
    public partial class frmEnvioCorreo : Form
    {
        BindingSource bsDestinatarios = new BindingSource();
        RepositoryItemButtonEdit rpiBtnDel = new RepositoryItemButtonEdit();
        clsNProcesoCredito loLogicaNegocio = new clsNProcesoCredito();
        clsNPlantillaSeguro loLogicaNegocioSeg = new clsNPlantillaSeguro();

        public int tId;
        public string Cuerpo;

        public frmEnvioCorreo()
        {
            InitializeComponent();
            rpiBtnDel.ButtonClick += rpiBtnDel_ButtonClick;
        }

        private void frmEnvioCorreo_Load(object sender, EventArgs e)
        {
            try
            {

                //if (clsPrincipal.gbEnviarDesdeCorreoCorporativo)
                //{
                //    chbEnviar.Visible = true;
                //}
                //else
                //{
                //    chbEnviar.Visible = false;
                //}

                chbEnviar.Checked = true;

                txtNo.Text = tId.ToString();

                bsDestinatarios.DataSource = new List<Destinatarios>();
                gcDestinatarios.DataSource = bsDestinatarios;

                clsComun.gDibujarBotonGrid(rpiBtnDel, dgvDestinatarios.Columns["Del"], "Eliminar", Diccionario.ButtonGridImage.trash_16x16, 10);

                clsComun.gLLenarComboGrid(ref dgvDestinatarios, loLogicaNegocio.goConsultarComboTipoDestinatario(),"Tipo");

                //clsComun.gLLenarCombo(ref cmbInicioTextoCorreo, loLogicaNegocio.goConsultarComboInicioCorreo(), false);

                dgvDestinatarios.Columns["Tipo"].Width = 10;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddFila_Click(object sender, EventArgs e)
        {
            try
            {
                bsDestinatarios.AddNew();
                dgvDestinatarios.Focus();
                dgvDestinatarios.ShowEditor();
                dgvDestinatarios.UpdateCurrentRow();
                var poLista = (List<Destinatarios>)bsDestinatarios.DataSource;
                poLista.LastOrDefault().Tipo = "PAR";
                poLista.LastOrDefault().Correo = txtDestinatarios.Text.Trim();
                dgvDestinatarios.RefreshData();
                dgvDestinatarios.FocusedColumn = dgvDestinatarios.VisibleColumns[1];
                txtDestinatarios.Text = "";
                txtDestinatarios.Focus();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                piIndex = dgvDestinatarios.GetFocusedDataSourceRowIndex();
                // Tomamos la lista del Grid
                var poLista = (List<Destinatarios>)bsDestinatarios.DataSource;

                if (poLista.Count > 0 && piIndex >= 0)
                {
                    // Tomamos la entidad de la fila seleccionada
                    var poEntidad = poLista[piIndex];

                    // Eliminar Fila seleccionada de mi lista
                    //poLista[piIndex].CodigoEstado = Diccionario.Eliminado;
                    poLista.RemoveAt(piIndex);

                    // Asigno mi nueva lista al Binding Source
                    bsDestinatarios.DataSource = poLista;
                    dgvDestinatarios.RefreshData();
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                
                var poListaObject = loLogicaNegocio.goConsultarVtEmpleados().Where(x => x.FechaFinContrato == null && !string.IsNullOrEmpty(x.CorreoLaboral)).OrderBy(x=>x.NombreCompleto).ToList();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Correo"),
                                    new DataColumn("Empleado"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Correo"] = a.CorreoLaboral;
                    row["Empleado"] = a.NombreCompleto;
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Empleados" };
                pofrmBuscar.Width = 600;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    bsDestinatarios.AddNew();
                    dgvDestinatarios.Focus();
                    dgvDestinatarios.ShowEditor();
                    dgvDestinatarios.UpdateCurrentRow();
                    var poLista = (List<Destinatarios>)bsDestinatarios.DataSource;
                    poLista.LastOrDefault().Tipo = "PAR";
                    poLista.LastOrDefault().Correo = pofrmBuscar.lsCodigoSeleccionado;
                    poLista.LastOrDefault().Persona = pofrmBuscar.lsSegundaColumna;
                    dgvDestinatarios.RefreshData();
                    dgvDestinatarios.FocusedColumn = dgvDestinatarios.VisibleColumns[1];
                    txtDestinatarios.Text = "";
                    txtDestinatarios.Focus();


                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {

                
                string psDestinatario = "";
                string psAsunto = "";
                string psCuerpo = Cuerpo;
                string psCC = "";

                var poListaDestinatarios = (List<Destinatarios>)bsDestinatarios.DataSource;
                if (poListaDestinatarios.Count > 0)
                {
                    if (!string.IsNullOrEmpty(txtAsunto.Text.Trim()))
                    {

                        DialogResult dialogResult2 = XtraMessageBox.Show("¿Está seguro de enviar el correo?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult2 == DialogResult.Yes)
                        {

                            foreach (var item in poListaDestinatarios)
                            {
                                if (item.Tipo == "PAR")
                                {
                                    psDestinatario = string.Format("{0}{1};", psDestinatario, item.Correo);
                                }
                                else if (item.Tipo == "COP")
                                {
                                    psCC = string.Format("{0}{1};", psCC, item.Correo);
                                }
                            }

                            if (psDestinatario.Length > 0)
                            {
                                psDestinatario = psDestinatario.Substring(0, (psDestinatario.Length - 1));
                            }

                            psAsunto = txtAsunto.Text;

                            if (tId != 0)
                            {
                                var dt = loLogicaNegocio.goConsultaDataTable(string.Format("EXEC CRESPCUERPORESOLUCION {0}", txtNo.Text));
                                if (dt.Rows.Count > 0)
                                {
                                    psCuerpo = string.Format("{0}<br/>{1}", ConstruirHtml(), dt.Rows[0][0].ToString());
                                }
                                
                                string psRuta = "";
                                List<Attachment> listAdjuntosEmail = new List<Attachment>();

                                var pIdSol = loLogicaNegocioSeg.gIdPlantillaSeguro(Convert.ToInt32(txtNo.Text.Trim()));
                                var poObjectSeg = loLogicaNegocioSeg.goConsultar(pIdSol);
                                if (poObjectSeg != null)
                                {
                                    psRuta = poObjectSeg.RutaDestino + poObjectSeg.ArchivoAdjunto;
                                }

                                if (File.Exists(psRuta))
                                    listAdjuntosEmail.Add(new Attachment(psRuta));


                                var poObject = loLogicaNegocio.goConsultar(Convert.ToInt32(txtNo.Text.Trim()));

                                foreach (var item in poObject.ProcesoCreditoResolucionAdjunto)
                                {
                                    psRuta = item.RutaDestino + item.ArchivoAdjunto;

                                    if (File.Exists(psRuta))
                                        listAdjuntosEmail.Add(new Attachment(psRuta));
                                }

                                loLogicaNegocio.EnviarPorCorreo(psDestinatario, psAsunto, psCuerpo, listAdjuntosEmail, false, psCC, "", true, "", 0, chbEnviar.Checked ? clsPrincipal.gsUsuario : "");
                                XtraMessageBox.Show("Correo enviado exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                psCuerpo = string.Format("{0}", ConstruirHtml());

                                loLogicaNegocio.EnviarPorCorreo(psDestinatario, psAsunto, psCuerpo, null, false, psCC, "", true, "", 0, chbEnviar.Checked ? clsPrincipal.gsUsuario : "");
                                XtraMessageBox.Show("Correo enviado exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show("Ingrese el Asunto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("No existen correos destinatarios", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMostrarAsunto_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                var poListaObject = loLogicaNegocio.goConsultarVtEmpleados().Where(x => x.FechaFinContrato == null && !string.IsNullOrEmpty(x.CorreoLaboral)).OrderBy(x => x.NombreCompleto).ToList();
                DataTable dt = new DataTable();

                dt.Columns.AddRange(new DataColumn[]
                                    {
                                    new DataColumn("Correo"),
                                    new DataColumn("Empleado"),
                                    });

                poListaObject.ForEach(a =>
                {
                    DataRow row = dt.NewRow();
                    row["Correo"] = a.CorreoLaboral;
                    row["Empleado"] = a.NombreCompleto;
                    dt.Rows.Add(row);
                });

                frmBusqueda pofrmBuscar = new frmBusqueda(dt) { Text = "Listado de Empleados" };
                pofrmBuscar.Width = 600;
                if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                {
                    bsDestinatarios.AddNew();
                    dgvDestinatarios.Focus();
                    dgvDestinatarios.ShowEditor();
                    dgvDestinatarios.UpdateCurrentRow();
                    var poLista = (List<Destinatarios>)bsDestinatarios.DataSource;
                    poLista.LastOrDefault().Tipo = "PAR";
                    poLista.LastOrDefault().Correo = pofrmBuscar.lsCodigoSeleccionado;
                    poLista.LastOrDefault().Persona = pofrmBuscar.lsSegundaColumna;
                    dgvDestinatarios.RefreshData();
                    dgvDestinatarios.FocusedColumn = dgvDestinatarios.VisibleColumns[1];
                    txtDestinatarios.Text = "";
                    txtDestinatarios.Focus();


                }

                */
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ConstruirHtml()
        {
            string html = "";
            html = string.Format("{0}<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">\n", html);
            html = string.Format("{0}<html>\n", html);
            html = string.Format("{0}<head>\n", html);
            html = string.Format("{0}<title></title>\n", html);
            html = string.Format("{0}</head>\n", html);
            html = string.Format("{0}<body>\n", html);
            html = string.Format("{0}{1}\n", html, txtTexto.Text.Replace("\n", "<br/>"));
            html = string.Format("{0}</body>", html);
            html = string.Format("{0}</html>\n", html);
            return html;
        }
    }
}
