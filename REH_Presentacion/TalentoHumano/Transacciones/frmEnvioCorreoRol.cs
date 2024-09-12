using REH_Presentacion.Comun;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmpleadoNomina
{
    public partial class frmTrEnvioCorreoRol : Form
    {

        private readonly int liCodigoEstadoActivo = 1;
        private readonly string lsSeleccione = "Seleccione...";
        public string lsCodigoRecibido;
        public string lsCodigo;
        public int lIdPeriodo;
        public string lsTipoRol;



        public frmTrEnvioCorreoRol()
        {
            InitializeComponent();
        }

        private void frmEnvioCorreoRol_Load(object sender, EventArgs e)
        {

            try
            {
                Dictionary<string, string> ListaGenero = new Dictionary<string, string>();
                ListaGenero.Add("0", lsSeleccione);
                ListaGenero.Add("001", "PRIMERA QUINCENA");
                ListaGenero.Add("002", "SEGUNDA QUINCENA");
                ListaGenero.Add("005", "COMISIONES");
                cmbTipoRol.DataSource = new BindingSource(ListaGenero, null);
                cmbTipoRol.ValueMember = "Key";
                cmbTipoRol.DisplayMember = "Value";


                //var poListaGenero = dbContext.RRHHFechaRol.Where(x => x.Estado == liCodigoEstadoActivo).ToList().Where(x=> x.FechaIni.Value > DateTime.Now.AddMonths(-6))
                //    .Select(x => new comboLon { Codigo = x.IdFechaRol, Descripcion = x.FechaIni.Value.Year.ToString() + x.FechaIni.Value.Month.ToString("00") +", "+ x.Mes + "-" + x.Anio })
                //    .ToList().OrderBy(x => x.Descripcion).ToList();
                //poListaGenero.Insert(0, new comboLon() { Codigo = 0, Descripcion = lsSeleccione });
                //cmbFechaRol.DataSource = poListaGenero;
                //cmbFechaRol.DisplayMember = "Descripcion";
                //cmbFechaRol.ValueMember = "Codigo";

                if(string.IsNullOrEmpty(lsCodigoRecibido))
                {
                    rdbEmpleado.Enabled = false;
                    rdbTodos.Checked = true;
                }
                else
                {
                    rdbEmpleado.Checked = true;
                }
            }
            catch (Exception ex )
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbValida())
                {
                    var poLista = dbContext.rpt_RRHH_RolQuincenaEmpleadosEnviaCorreo(2, int.Parse(cmbTipoRol.SelectedValue.ToString()), int.Parse(cmbFechaRol.SelectedValue.ToString()), false,0, GetCodEmpleado()).ToList();
                    if(poLista.Count > 0)
                    {
                        var piCant = poLista.Where(x => string.IsNullOrEmpty(x.Correo)).Count();
                        if (piCant > 0) MessageBox.Show("Existen " + piCant.ToString() + " Registros que no tienen correo, para visualizar, Clic en Preliminar", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        DialogResult dialogResult = MessageBox.Show("¿Está seguro de enviar correo a los empleados.?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                        if (dialogResult == DialogResult.Yes)
                        {
                            dbContext.Database.CommandTimeout = int.MaxValue;
                            dbContext.rpt_RRHH_RolQuincenaEmpleadosEnviaCorreo(2, int.Parse(cmbTipoRol.SelectedValue.ToString()), int.Parse(cmbFechaRol.SelectedValue.ToString()), true, 0, GetCodEmpleado()).ToList();
                            MessageBox.Show("Enviado Exitosamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No existe información con esos parámetros", "Importante", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool lbValida()
        {
            bool poResult = true;

            if (cmbTipoRol.SelectedValue.ToString() == "0")
            {
                MessageBox.Show("Debe seleccionar Tipo Rol.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (cmbFechaRol.SelectedValue.ToString() == "0")
            {
                MessageBox.Show("Debe seleccionar Fecha Rol.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return poResult;
        }

        private int GetCodEmpleado()
        {
            int piCodEmpleado = 0;
            if(rdbEmpleado.Checked)
            {
                if(!string.IsNullOrEmpty(lsCodigoRecibido))
                {
                    piCodEmpleado = int.Parse(lsCodigoRecibido);
                }
                else
                {
                    MessageBox.Show("Ocurrió un error por favor cierre la pantalla y vuelva a intentarlo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 999999999;
                }
            }
            return piCodEmpleado;
        }
        private void btnPreliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbValida())
                {
                    var poListaObject = dbContext.rpt_RRHH_RolQuincenaEmpleadosEnviaCorreo(2, int.Parse(cmbTipoRol.SelectedValue.ToString()), int.Parse(cmbFechaRol.SelectedValue.ToString()), false, 0, GetCodEmpleado()).ToList();

                    DataTable dt = new DataTable();

                    dt.Columns.AddRange(new DataColumn[]
                                        {
                                    new DataColumn("Id"),
                                    new DataColumn("Cédula"),
                                    new DataColumn("Nombre"),
                                    new DataColumn("Tiene Correo"),
                                    new DataColumn("Correo"),
                                    new DataColumn("Departamento")
                                        });
                    poListaObject.ForEach(a =>
                    {
                        DataRow row = dt.NewRow();
                        row["Id"] = a.IdEmpleado;
                        row["Cédula"] = a.Cedula;
                        row["Nombre"] = a.Nombres;
                        row["Tiene Correo"] = string.IsNullOrEmpty(a.Correo) ? "No" : "Si";
                        row["Correo"] = a.Correo;
                        row["Departamento"] = a.DEPARTAMENTO;
                        dt.Rows.Add(row);
                    });

                    frmBuscar pofrmBuscar = new frmBuscar(dt) { Text = "Listado de Empleados" };

                    if (pofrmBuscar.ShowDialog() == DialogResult.OK)
                    {
                        DataGridViewRow row = pofrmBuscar.FilaSeleccionada;
                        lsCodigo = row.Cells[0].Value.ToString();
                        DialogResult = DialogResult.OK;
                        Close();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBitacora_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (lbValida())
                {
                    frmRptLogBitacora rpt = new frmRptLogBitacora();
                    rpt.lIdFechaRol = int.Parse(cmbFechaRol.SelectedValue.ToString());
                    rpt.lIdTipoRol = int.Parse(cmbTipoRol.SelectedValue.ToString());
                    rpt.lIdEmpleado = GetCodEmpleado();
                    rpt.Show();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
