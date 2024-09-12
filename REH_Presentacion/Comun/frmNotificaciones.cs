using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using GEN_Entidad;
using GEN_Negocio;
using REH_Presentacion.Compras.Transaccion;
using REH_Presentacion.Credito.Transacciones;
using REH_Presentacion.SHEQ.Transacciones;
using REH_Presentacion.TalentoHumano.Transacciones;
using REH_Presentacion.Ventas.Transacciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REH_Presentacion.Comun
{
    public partial class frmNotificaciones : Form
    {
        public DataTable dt;
        public clsNBase loLogicaNegocio = new clsNBase();
        
        RepositoryItemMemoEdit rpiMedDescripcion;


        public frmNotificaciones()
        {
            InitializeComponent();
            rpiMedDescripcion = new RepositoryItemMemoEdit();
            rpiMedDescripcion.WordWrap = true;
            //DisableCloseButton(frmNotificaciones);
        }

        private void gcDatos_Load(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dt;
            gcDatos.DataSource = bs;
        }

        private void frmNotificaciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void frmNotificaciones_Shown(object sender, EventArgs e)
        {
            try
            {
                //BindingSource bs = new BindingSource();
                //bs.DataSource = dt;
                //gcDatos.DataSource = dt;
                BindingSource bs = new BindingSource();
                bs.DataSource = dt;
                gcDatos.DataSource = bs;
                lColumnas();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void gRefresh()
        {
            try
            {
                Invoke(new Action(() =>
                {
                    //gcDatos.DataSource = dt;
                    BindingSource bs = new BindingSource();
                    bs.DataSource = dt;
                    gcDatos.DataSource = bs;

                }));
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }

        }

        public void lColumnas()
        {
            for (int i = 0; i < dgvDatos.Columns.Count; i++)
            {
                dgvDatos.Columns[i].OptionsColumn.AllowEdit = false;

            }

            dgvDatos.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            dgvDatos.OptionsView.RowAutoHeight = true;

            // Crédito - Jefe de Crédito - Revisión de Crédito
            if (clsPrincipal.gIdPerfil == 13 || clsPrincipal.gIdPerfil == 34 || clsPrincipal.gIdPerfil == 37)
            {
                dgvDatos.Columns["Fecha"].Caption = "Fecha Requerimiento";
            }


            dgvDatos.Columns["Id"].Caption = "No";
            dgvDatos.Columns["IdMenu"].Visible = false;
            dgvDatos.Columns["Transaccion"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Descripción"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Usuario"].ColumnEdit = rpiMedDescripcion;
            dgvDatos.Columns["Proceso"].ColumnEdit = rpiMedDescripcion;

            dgvDatos.Columns["Id"].Width = 30;
            dgvDatos.Columns["Fecha"].Width = 65;
            dgvDatos.Columns["Transaccion"].Width = 100;
            dgvDatos.Columns["Usuario"].Width = 100;
            dgvDatos.Columns["Proceso"].Width = 100;
            dgvDatos.Columns["Estado"].Width = 60;

        }

        private void dgvDatos_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var Index = dgvDatos.GetFocusedDataSourceRowIndex();
                var polista = dt.AsEnumerable().ToList();
                var lid = polista[Index].ItemArray[0].ToString();

                var idMenu = polista[Index].ItemArray[7].ToString();
                var poMenu = loLogicaNegocio.goBuscarMenuId(idMenu);
                if (poMenu != null)
                {
                    if (poMenu.NombreForma == Diccionario.Tablas.Menu.SolicitudCompra)
                    {
                        lmostrarSolicitudCompra(int.Parse(lid));
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.BandejaSolicitudCompra)
                    {
                        lmostrarBandejaSolicitudCompras();
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.BandejaSolicitudCompraAprobadas)
                    {
                        lmostrarBandejaSolicituComprasAprobadas();
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.AprobacionCotizacion)
                    {
                        lmostrarCotizacionAprobacion(int.Parse(lid));
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.BandejaPermisoHoras)
                    {
                        lMostrarBandejaPermisosHoras();
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.OrdenPago)
                    {
                        lMostrarOrdenPago(int.Parse(lid));
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.frmTrBandejaOrdenPago)
                    {
                        lMostrarBandejaOrdenPago();
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.frmTrBandejaFactPendPagoContabilidad)
                    {
                        lMostrarFacturasIngresadasEnSAAF();
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.frmTrBandejaFactPendPago)
                    {
                        lMostrarDocumentosPorPagar();
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.frmTrBandejaFactPendPagoPorAprobar)
                    {
                        lMostrarListadoFacturasPendientePagoXAprobar();
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.frmTrBandejaFactPendPagoTesoreria)
                    {
                        lMostrarGenerarArchivoMultiCash();
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.frmTrBandejaFactPendPagoFinanciero)
                    {
                        lMostrarAprobacionArchivoMultiCash();
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.frmTrBandejaLiquidaciones)
                    {
                        lMostrarBandejaLiquidaciones();
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.frmTrBandejaSolicitudVacaciones)
                    {
                        lMostrarBandejaSolicitudVacaciones();
                    }
                    else if (int.Parse(idMenu) == 284) //Diccionario.Tablas.Menu.frmTrProcesoCredito
                    {
                        lMostrarRequerimientoCredito(int.Parse(lid));
                    }
                    else if (int.Parse(idMenu) == 291) //Diccionario.Tablas.Menu.frmTrProcesoCredito
                    {
                        lMostrarRequerimientoCreditoRevision(int.Parse(lid));
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.frmTrCambiarEstatus)
                    {
                        lMostrarResolucionCredito();
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.frmTrRecepcionIngreso)
                    {
                        lMostrarRecepcionIngreso();
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.frmTrBandejaAprobacionRebate)
                    {
                        lMostrarBandejaAprobacionRebate();
                    }
                    else if (poMenu.NombreForma == Diccionario.Tablas.Menu.frmTrBandejaAprobacionRebateAnual)
                    {
                        lMostrarBandejaAprobacionRebateAnual();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            }

            
        }

        private void lmostrarSolicitudCompra(int lid)
        {

            string psForma = Diccionario.Tablas.Menu.SolicitudCompra;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrSolicitudCompras poFrmMostrarFormulario = new frmTrSolicitudCompras();
                poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrmMostrarFormulario.Text = poForm.Nombre;
                poFrmMostrarFormulario.lId = lid;
                poFrmMostrarFormulario.ShowInTaskbar = true;

                poFrmMostrarFormulario.MdiParent = this.ParentForm;
                poFrmMostrarFormulario.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void lmostrarBandejaSolicitudCompras()
        {

            string psForma = Diccionario.Tablas.Menu.BandejaSolicitudCompra;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrBandejaSolicitudCompra poFrmMostrarFormulario = new frmTrBandejaSolicitudCompra();
                poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrmMostrarFormulario.Text = poForm.Nombre;
                poFrmMostrarFormulario.ShowInTaskbar = true;
            
                poFrmMostrarFormulario.MdiParent = this.ParentForm;
        
                poFrmMostrarFormulario.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void lmostrarBandejaSolicituComprasAprobadas()
        {
            string psForma = Diccionario.Tablas.Menu.BandejaSolicitudCompraAprobadas;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrSolicitudesCompraAprobadas poFrmMostrarFormulario = new frmTrSolicitudesCompraAprobadas();
                poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrmMostrarFormulario.Text = poForm.Nombre;
                poFrmMostrarFormulario.ShowInTaskbar = true;

                poFrmMostrarFormulario.MdiParent = this.ParentForm;
                poFrmMostrarFormulario.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void lmostrarCotizacionAprobacion(int lid)
        {
            string psForma = Diccionario.Tablas.Menu.AprobacionCotizacion;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrCotizacionAprobacion poFrmMostrarFormulario = new frmTrCotizacionAprobacion();
                poFrmMostrarFormulario.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrmMostrarFormulario.Text = poForm.Nombre;
                poFrmMostrarFormulario.ShowInTaskbar = true;
                poFrmMostrarFormulario.lIdCotizacion = lid;

                poFrmMostrarFormulario.MdiParent = this.ParentForm;
                poFrmMostrarFormulario.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lMostrarBandejaPermisosHoras()
        {
            string psForma = Diccionario.Tablas.Menu.BandejaPermisoHoras;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrBandejaPermisoHoras poFrm = new frmTrBandejaPermisoHoras();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lMostrarOrdenPago(int tId = 0)
        {
            string psForma = Diccionario.Tablas.Menu.OrdenPago;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrOrdenPago poFrm = new frmTrOrdenPago();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                if (tId != 0)
                {
                    poFrm.lid = tId;
                }
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        private void lMostrarBandejaOrdenPago()
        {
            string psForma = Diccionario.Tablas.Menu.frmTrBandejaOrdenPago;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrBandejaOrdenPago poFrm = new frmTrBandejaOrdenPago();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void lMostrarListadoFacturasPendientePagoXAprobar()
        {
            string psForma = Diccionario.Tablas.Menu.frmTrBandejaFactPendPagoPorAprobar;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrBandejaFactPendPagoPorAprobar poFrm = new frmTrBandejaFactPendPagoPorAprobar();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lMostrarFacturasIngresadasEnSAAF()
        {
            string psForma = Diccionario.Tablas.Menu.frmTrBandejaFactPendPagoContabilidad;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrBandejaFactPendPagoContabilidad poFrm = new frmTrBandejaFactPendPagoContabilidad();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lMostrarDocumentosPorPagar()
        {
            string psForma = Diccionario.Tablas.Menu.frmTrBandejaFactPendPago;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrBandejaFactPendPago poFrm = new frmTrBandejaFactPendPago();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        private void lMostrarGenerarArchivoMultiCash()
        {
            string psForma = Diccionario.Tablas.Menu.frmTrBandejaFactPendPagoTesoreria;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrBandejaFactPendPagoTesoreria poFrm = new frmTrBandejaFactPendPagoTesoreria();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lMostrarAprobacionArchivoMultiCash()
        {
            string psForma = Diccionario.Tablas.Menu.frmTrBandejaFactPendPagoFinanciero;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrBandejaFactPendPagoFinanciero poFrm = new frmTrBandejaFactPendPagoFinanciero();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lMostrarBandejaLiquidaciones()
        {
            string psForma = Diccionario.Tablas.Menu.frmTrBandejaLiquidaciones;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrBandejaLiquidaciones poFrm = new frmTrBandejaLiquidaciones();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lMostrarBandejaSolicitudVacaciones()
        {
            string psForma = Diccionario.Tablas.Menu.frmTrBandejaSolicitudVacaciones;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrBandejaSolicitudVacaciones poFrm = new frmTrBandejaSolicitudVacaciones();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lMostrarResolucionCredito()
        {
            string psForma = Diccionario.Tablas.Menu.frmTrCambiarEstatus;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrCambiarEstatus poFrm = new frmTrCambiarEstatus();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void lMostrarBandejaAprobacionRebate()
        {
            string psForma = Diccionario.Tablas.Menu.frmTrBandejaAprobacionRebate;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrBandejaAprobacionRebate poFrm = new frmTrBandejaAprobacionRebate();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lMostrarBandejaAprobacionRebateAnual()
        {
            string psForma = Diccionario.Tablas.Menu.frmTrBandejaAprobacionRebateAnual;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrBandejaAprobacionRebateAnual poFrm = new frmTrBandejaAprobacionRebateAnual();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void lMostrarRecepcionIngreso()
        {
            string psForma = Diccionario.Tablas.Menu.frmTrRecepcionIngreso;
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(psForma, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrRecepcionIngreso poFrm = new frmTrRecepcionIngreso();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado la forma: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, psForma), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lMostrarRequerimientoCreditoRevision(int tId = 0)
        {
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(291, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrProcesoCredito poFrm = new frmTrProcesoCredito();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                if (tId != 0)
                {
                    poFrm.lid = tId;
                }
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado Menú Id: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, 291), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lMostrarRequerimientoCredito(int tId = 0)
        {
            var poForm = loLogicaNegocio.goConsultarMenuPerfil(284, clsPrincipal.gIdPerfil);
            if (poForm != null)
            {
                frmTrProcesoCredito poFrm = new frmTrProcesoCredito();
                poFrm.Tag = poForm.IdMenu + "," + poForm.Nombre;
                poFrm.Text = poForm.Nombre;
                if (tId != 0)
                {
                    poFrm.lid = tId;
                }
                poFrm.ShowInTaskbar = true;

                poFrm.MdiParent = this.ParentForm;
                poFrm.Show();
            }
            else
            {
                XtraMessageBox.Show(string.Format("Perfil: '{0}', No tiene parametrizado Menú Id: '{1}'. Por favor comunicarse con Sistemas ", clsPrincipal.gsDesPerfil, 284), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
