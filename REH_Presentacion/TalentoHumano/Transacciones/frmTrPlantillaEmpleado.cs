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
    /// Fecha: 17/08/2020
    /// Formulario para Carga de Plantilla Empleado 
    /// </summary>
    public partial class frmTrPlantillaEmpleado : frmBaseTrxDev
    {

        #region Variables
        clsNPlantilla loLogicaNegocio;
        #endregion

        #region Eventos
        public frmTrPlantillaEmpleado()
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
                
                List<PlantillaEmpleado> poLista = (List<PlantillaEmpleado>)bsDatos.DataSource;
                List<Persona> poListaPersona = new List<Persona>();
                if (poLista.Count > 0)
                {
                    

                    if (lbEsValido(poLista))
                    {
                        var poListaEstadoCivil = loLogicaNegocio.goConsultarComboEstadoCivil();
                        var poListaGenero = loLogicaNegocio.goConsultarComboGenero();
                        var poListaTipoEmpleado = loLogicaNegocio.goConsultarComboTipoEmpleado();
                        var poListaEstado = loLogicaNegocio.goConsultarComboEstadoRegistro();
                        var poListaSucursal = loLogicaNegocio.goConsultarComboSucursal();
                        var poListaTipoContrato = loLogicaNegocio.goConsultarComboTipoContrato();
                        var poListaDepartamento = loLogicaNegocio.goConsultarComboDepartamento();
                        var poListaTipoComision = loLogicaNegocio.goConsultarComboTipoComision();
                        var poListaCentroCosto = loLogicaNegocio.goConsultarComboCentroCosto();
                        var poListaCargoLaboral = loLogicaNegocio.goConsultarComboCargo();
                        var poListaBanco = loLogicaNegocio.goConsultarComboBanco();
                        var poListaFormaPago = loLogicaNegocio.goConsultarComboFormaPago();
                        var poListaTipoCuenta = loLogicaNegocio.goConsultarComboTipoCuentaBancaria();
                        var poListaRegion = loLogicaNegocio.goConsultarComboRegion();

                        foreach (var item in poLista)
                        {
                            Persona poPersona = new Persona();
                            poPersona.CodigoEstado = Diccionario.Activo;
                            poPersona.CodigoTipoPersona = Diccionario.ListaCatalogo.TipoPersonaClass.Natural;
                            poPersona.CodigoTipoIdentificacion = Diccionario.ListaCatalogo.TipoIdentificacionClass.Cedula;
                            poPersona.IdPersona = 0;
                            poPersona.NumeroIdentificacion = item.Identificacion;
                            poPersona.ApellidoPaterno = item.ApellidoPaterno;
                            poPersona.ApellidoMaterno = item.ApellidoMaterno;
                            poPersona.PrimerNombre = item.PrimerNombre;
                            poPersona.SegundoNombre = item.SegundoNombre;
                            poPersona.NombreCompleto = item.ApellidoPaterno + " " + item.ApellidoMaterno + " " + item.PrimerNombre + " " + item.SegundoNombre;
                            poPersona.Correo = item.CorreoPersonal;
                            poPersona.CodigoEstadoCivil = poListaEstadoCivil.Where(x=>x.Descripcion == item.EstadoCivil).Select(x=>x.Codigo).FirstOrDefault();
                            poPersona.CodigoGenero = poListaGenero.Where(x => x.Descripcion == item.Genero).Select(x => x.Codigo).FirstOrDefault();
                            poPersona.LugarNacimiento = item.LugarNacimiento;
                            poPersona.FechaNacimiento = item.FechaNacimiento;
                            poPersona.CodigoNivelEducacion = null;
                            poPersona.Peso = null;
                            poPersona.Estatura = null;
                            poPersona.CodigoColorPiel = null;
                            poPersona.CodigoColorOjos = null;
                            poPersona.CodigoTipoSangre = null;
                            poPersona.CodigoTipoLicencia = null;
                            poPersona.CodigoRegion = poListaRegion.Where(x => x.Descripcion == item.Region).Select(x => x.Codigo).FirstOrDefault(); ;
                            poPersona.FechaExpiracionLicencia = null;
                            poPersona.NumeroLicencia = string.Empty;
                            poPersona.Fecha = DateTime.Now;
                            poPersona.Usuario = clsPrincipal.gsUsuario;
                            poPersona.Terminal = clsPrincipal.gsTerminal;
                            
                            Empleado poEmpleado = new Empleado();
                            poEmpleado.CodigoEstado = Diccionario.Activo;
                            poEmpleado.CodigoTipoEmpleado = poListaTipoEmpleado.Where(x => x.Descripcion == item.TipoEmpleado).Select(x => x.Codigo).FirstOrDefault();
                            poEmpleado.Correo = item.CorreoLaboral;
                            poEmpleado.NumeroSeguroSocial = string.Empty;
                            poEmpleado.CodigoTipoVivienda = null;
                            poEmpleado.CodigoTipoMaterialVivienda = null;
                            poEmpleado.ValorArriendo = null;
                            poEmpleado.NumeroLibretaMilitar = null;

                            //poEmpleado.EmpleadoCargaFamiliar = (List<EmpleadoCargaFamiliar>)bsCargaFamiliar.DataSource;
                            
                            EmpleadoContrato poContrato = new EmpleadoContrato();
                            poContrato.IdEmpleadoContrato = 0;
                            poContrato.CodigoEstado = poListaEstado.Where(x => x.Descripcion == item.EstadoContrato).Select(x => x.Codigo).FirstOrDefault();
                            poContrato.CodigoSucursal = poListaSucursal.Where(x => x.Descripcion == item.Sucursal).Select(x => x.Codigo).FirstOrDefault();
                            poContrato.CodigoDepartamento = poListaDepartamento.Where(x => x.Descripcion == item.Departamento).Select(x => x.Codigo).FirstOrDefault();
                            poContrato.CodigoTipoContrato = poListaTipoContrato.Where(x => x.Descripcion == item.TipoContrato).Select(x => x.Codigo).FirstOrDefault();
                            poContrato.CodigoCentroCosto = poListaCentroCosto.Where(x => x.Descripcion == item.CentrodeCosto).Select(x => x.Codigo).FirstOrDefault();
                            poContrato.IdPersonaJefe = null;
                            poContrato.CodigoCargoLaboral = poListaCargoLaboral.Where(x => x.Descripcion == item.CargoLaboral).Select(x => x.Codigo).FirstOrDefault();
                            if (!string.IsNullOrEmpty(item.TipodeComision))
                                poContrato.CodigoTipoComision = poListaTipoComision.Where(x => x.Descripcion == item.TipodeComision).Select(x => x.Codigo).FirstOrDefault();
                            else
                                poContrato.CodigoTipoComision = null;
                            poContrato.PorcentajeComision = item.PorcentajedeComision;
                            poContrato.Sueldo = item.Sueldo;
                            poContrato.PorcentajePQ = item.PorcentajePrimeraQuincena;
                            poContrato.InicioHorarioLaboral = null;
                            poContrato.FinHorarioLaboral = null;
                            poContrato.FechaInicioContrato = item.FechaInicioContrato;
                            poContrato.FechaFinContrato = item.FechaFindeContrato;
                            poContrato.CodigoMotivoFinContrato = null;
                            poContrato.CodigoBanco = poListaBanco.Where(x => x.Descripcion == item.Banco).Select(x => x.Codigo).FirstOrDefault();
                            if (poContrato.CodigoBanco == null) poContrato.CodigoBanco = string.Empty;
                            poContrato.CodigoFormaPago = poListaFormaPago.Where(x => x.Descripcion == item.FormadePago).Select(x => x.Codigo).FirstOrDefault();
                            if (poContrato.CodigoFormaPago == null) poContrato.CodigoFormaPago = string.Empty;
                            poContrato.CodigoTipoCuentaBancaria = poListaTipoCuenta.Where(x => x.Descripcion == item.TipoCuenta).Select(x => x.Codigo).FirstOrDefault(); ;
                            if (poContrato.CodigoTipoCuentaBancaria == null) poContrato.CodigoTipoCuentaBancaria = string.Empty;
                            poContrato.NumeroCuenta = item.Cuenta;
                            if (poContrato.NumeroCuenta == null) poContrato.NumeroCuenta = string.Empty;
                            poContrato.ValorAlimentacion = item.ValorAlimentacion;
                            poContrato.ValorMovilizacion = item.ValorMovilizacion;
                            poContrato.AplicaAlimentacion = item.ValorAlimentacion > 0M ? true : false;
                            poContrato.AplicaMovilizacion = item.ValorMovilizacion > 0M ? true : false;
                            poContrato.AplicaHorasExtras = item.AplicaHorasExtras;
                            poContrato.AplicaIessConyugue = item.AplicaIessConyugue;
                            poContrato.AcumulaD3 = item.AcumulaDecimoTercero;
                            poContrato.AcumulaD4 = item.AcumulaDecimoCuarto;
                            poContrato.EsJefe = item.EsJefe;
                            poContrato.EsJubilado = item.EsJubilado;

                            poContrato.SolicitudFondoReserva = item.SolicitudFondoReserva;
                            poContrato.DerechoFondoReserva = item.DerechoFondoReserva;

                            poEmpleado.EmpleadoContrato = new List<EmpleadoContrato>();
                            poEmpleado.EmpleadoContrato.Add(poContrato);
                            poPersona.Empleado = poEmpleado;

                            poListaPersona.Add(poPersona);
                        }

                        if (loLogicaNegocio.gGuardar(clsPrincipal.gsUsuario, clsPrincipal.gsTerminal, poListaPersona))
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lLimpiar();
                        }
                        else
                        {
                            XtraMessageBox.Show(Diccionario.MsgRegistroNoGuardado, Diccionario.MsgTituloRegistroGuardado, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<PlantillaEmpleado> poLista = new List<PlantillaEmpleado>();

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
                            PlantillaEmpleado poItem = new PlantillaEmpleado();
                            poItem.Identificacion = item[0].ToString().Trim();
                            poItem.ApellidoPaterno = item[1].ToString().Trim();
                            poItem.ApellidoMaterno= item[2].ToString().Trim();
                            poItem.PrimerNombre = item[3].ToString().Trim();
                            poItem.SegundoNombre= item[4].ToString().Trim();
                            poItem.CorreoPersonal = item[5].ToString().Trim();
                            poItem.CorreoLaboral = item[6].ToString().Trim();
                            poItem.Genero = item[7].ToString().Trim();
                            poItem.LugarNacimiento = item[8].ToString().Trim();
                            poItem.FechaNacimiento = DateTime.Parse(item[9].ToString().Trim());
                            poItem.Region = item[10].ToString().Trim();
                            poItem.TipoEmpleado = item[11].ToString().Trim();
                            poItem.Sucursal = item[12].ToString().Trim();
                            poItem.Departamento = item[13].ToString().Trim();
                            poItem.TipoContrato = item[14].ToString().Trim();
                            poItem.CentrodeCosto = item[15].ToString().Trim();
                            poItem.CargoLaboral = item[16].ToString().Trim();
                            poItem.Sueldo = decimal.Parse(item[17].ToString().Trim());
                            poItem.PorcentajePrimeraQuincena = decimal.Parse(item[18].ToString().Trim()) * 100;
                            poItem.FechaInicioContrato = DateTime.Parse(item[19].ToString().Trim());
                            if (!string.IsNullOrEmpty(item[20].ToString().Trim())) poItem.FechaFindeContrato = DateTime.Parse(item[20].ToString().Trim());
                            if (!string.IsNullOrEmpty(item[21].ToString().Trim()))  poItem.TipodeComision = item[21].ToString().Trim();
                            if (!string.IsNullOrEmpty(item[21].ToString().Trim()))  poItem.PorcentajedeComision = decimal.Parse(item[22].ToString().Trim()) * 100;
                            poItem.AplicaHorasExtras = item[23].ToString().Trim() == "SI" ? true : false;
                            poItem.Banco = item[24].ToString().Trim();
                            poItem.FormadePago = item[25].ToString().Trim();
                            poItem.TipoCuenta = item[26].ToString().Trim();
                            poItem.Cuenta = item[27].ToString().Trim();
                            poItem.AplicaIessConyugue = item[28].ToString().Trim() == "SI" ? true : false;
                            poItem.AcumulaDecimoTercero = item[29].ToString().Trim() == "SI" ? true : false;
                            poItem.AcumulaDecimoCuarto = item[30].ToString().Trim() == "SI" ? true : false;
                            poItem.EsJefe = item[31].ToString().Trim() == "SI" ? true : false;
                            poItem.EsJubilado = item[32].ToString().Trim() == "SI" ? true : false;
                            poItem.ValorAlimentacion = decimal.Parse(item[33].ToString().Trim());
                            poItem.ValorMovilizacion = decimal.Parse(item[34].ToString().Trim());
                            poItem.EstadoCivil = item[35].ToString().Trim();
                            poItem.SolicitudFondoReserva = item[36].ToString().Trim() == "SI" ? true : false;
                            poItem.DerechoFondoReserva = item[37].ToString().Trim() == "SI" ? true : false;
                            poItem.EstadoContrato = item[38].ToString().Trim();

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

            bsDatos.DataSource = new List<PlantillaEmpleado>();
            gcDatos.DataSource = bsDatos;
        }

        private bool lbEsValido(List<PlantillaEmpleado> toLista)
        {
            if(toLista.Count > 0)
            {
                List<string> psLista = new List<string>();
                string psMensaje = string.Empty;
                var poLostaCedula = new clsNEmpleado().goListar().Select(x => x.NumeroIdentificacion).ToList();
                // Inicio Valida Cédula
                psLista = toLista.Where(x => poLostaCedula.Contains(x.Identificacion)).Select(x => x.Identificacion).Distinct().ToList();
                if (psLista.Count > 0)
                {
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    XtraMessageBox.Show("Ya Existen ingresadas al Sistema los siguientes números de identificación: \n" + psMensaje, "¡Debe eliminar registros para importar!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                psLista = new List<string>();
                foreach(var item in toLista)
                {
                    if (!clsComun.gVerificaIdentificacion(item.Identificacion))
                    {
                        psLista.Add(item.Identificacion);
                    }
                }

                if (psLista.Count > 0)
                {
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    XtraMessageBox.Show("Números de Identificación con formato NO VALIDO: \n" + psMensaje, "¡Debe Corregir registros para importar!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                
                //Fin


                var poListaEstadoCivil = loLogicaNegocio.goConsultarComboEstadoCivil().Select(x=>x.Descripcion).ToList();
                var poListaGenero = loLogicaNegocio.goConsultarComboGenero().Select(x => x.Descripcion).ToList();
                var poListaTipoEmpleado = loLogicaNegocio.goConsultarComboTipoEmpleado().Select(x => x.Descripcion).ToList();
                var poListaEstado = loLogicaNegocio.goConsultarComboEstadoRegistro().Select(x => x.Descripcion).ToList();
                var poListaSucursal = loLogicaNegocio.goConsultarComboSucursal().Select(x => x.Descripcion).ToList();
                var poListaTipoContrato = loLogicaNegocio.goConsultarComboTipoContrato().Select(x => x.Descripcion).ToList();
                var poListaDepartamento = loLogicaNegocio.goConsultarComboDepartamento().Select(x => x.Descripcion).ToList();
                var poListaTipoComision = loLogicaNegocio.goConsultarComboTipoComision().Select(x => x.Descripcion).ToList();
                var poListaCentroCosto = loLogicaNegocio.goConsultarComboCentroCosto().Select(x => x.Descripcion).ToList();
                var poListaCargoLaboral = loLogicaNegocio.goConsultarComboCargo().Select(x => x.Descripcion).ToList();
                var poListaBanco = loLogicaNegocio.goConsultarComboBanco().Select(x => x.Descripcion).ToList();
                var poListaFormaPago = loLogicaNegocio.goConsultarComboFormaPago().Select(x => x.Descripcion).ToList();
                var poListaTipoCuenta = loLogicaNegocio.goConsultarComboTipoCuentaBancaria().Select(x => x.Descripcion).ToList();
                var poListaRegion  = loLogicaNegocio.goConsultarComboRegion().Select(x => x.Descripcion).ToList();


                int piContador;
                bool pbReturn = true;
                // Inicio Valida e inserta datos de ESTADO CIVIL
                psLista = toLista.Where(x => !poListaEstadoCivil.Contains(x.EstadoCivil)).Select(x => x.EstadoCivil).Distinct().ToList();
                if(psLista.Count > 0)
                {  
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    DialogResult dialogResult = XtraMessageBox.Show("No extisten los siguientes registros: \n" + psMensaje, "¿Crear Registros de Estado Civil?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        piContador = 0;
                        foreach (var item in psLista)
                        {
                            piContador++;
                            string psCodigo = item.Substring(0, 1) + piContador.ToString("00");
                            new clsNCatalogo().gbGuardar(new Catalogo()
                            {
                                Codigo = psCodigo,
                                CodigoGrupo = Diccionario.ListaCatalogo.EstadoCivil,
                                Descripcion = item,
                                CodigoEstado = Diccionario.Activo,
                                Usuario = clsPrincipal.gsUsuario,
                                Fecha = DateTime.Now,
                                Terminal = clsPrincipal.gsTerminal
                            });
                        }
                    }
                    else
                        pbReturn = false;
                 
                }
                // Fin
                // Inicio Valida e inserta datos de GÉNERO    
                psLista = toLista.Where(x => !poListaGenero.Contains(x.Genero)).Select(x => x.Genero).Distinct().ToList();
                if (psLista.Count > 0)
                {
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    DialogResult dialogResult = XtraMessageBox.Show("No extisten los siguientes registros: \n" + psMensaje, "¿Crear Registros de Género?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        piContador = 0;
                        foreach (var item in psLista)
                        {
                            piContador++;
                            string psCodigo = item.Substring(0, 1) + piContador.ToString("00");
                            new clsNCatalogo().gbGuardar(new Catalogo()
                            {
                                Codigo = psCodigo,
                                CodigoGrupo = Diccionario.ListaCatalogo.Genero,
                                Descripcion = item,
                                CodigoEstado = Diccionario.Activo,
                                Usuario = clsPrincipal.gsUsuario,
                                Fecha = DateTime.Now,
                                Terminal = clsPrincipal.gsTerminal
                            });
                        }
                    }
                    else
                        pbReturn = false;
                }
                // Fin
                // Inicio Valida e inserta datos de REGION    
                psLista = toLista.Where(x => !poListaRegion.Contains(x.Region)).Select(x => x.Region).Distinct().ToList();
                if (psLista.Count > 0)
                {
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    DialogResult dialogResult = XtraMessageBox.Show("No extisten los siguientes registros: \n" + psMensaje, "¿Crear Registros de Región?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        piContador = 0;
                        foreach (var item in psLista)
                        {
                            piContador++;
                            string psCodigo = item.Substring(0, 1) + piContador.ToString("00");
                            new clsNCatalogo().gbGuardar(new Catalogo()
                            {
                                Codigo = psCodigo,
                                CodigoGrupo = Diccionario.ListaCatalogo.Region,
                                Descripcion = item,
                                CodigoEstado = Diccionario.Activo,
                                Usuario = clsPrincipal.gsUsuario,
                                Fecha = DateTime.Now,
                                Terminal = clsPrincipal.gsTerminal
                            });
                        }
                    }
                    else
                        pbReturn = false;
                }
                // Fin
                // Inicio Valida e inserta datos de TIPO EMPLEADO    
                psLista = toLista.Where(x => !poListaTipoEmpleado.Contains(x.TipoEmpleado)).Select(x => x.TipoEmpleado).Distinct().ToList();
                if (psLista.Count > 0)
                {
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    DialogResult dialogResult = XtraMessageBox.Show("No extisten los siguientes registros: \n" + psMensaje, "¿Crear Registros de Tipo de Empleado?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        piContador = 0;
                        foreach (var item in psLista)
                        {
                            piContador++;
                            string psCodigo = item.Substring(0, 1) + piContador.ToString("00");
                            new clsNCatalogo().gbGuardar(new Catalogo()
                            {
                                Codigo = psCodigo,
                                CodigoGrupo = Diccionario.ListaCatalogo.TipoEmpleado,
                                Descripcion = item,
                                CodigoEstado = Diccionario.Activo,
                                Usuario = clsPrincipal.gsUsuario,
                                Fecha = DateTime.Now,
                                Terminal = clsPrincipal.gsTerminal
                            });
                        }
                    }
                    else
                        pbReturn = false;
                }
                // Fin
                // Inicio Valida e inserta datos de SUCURSAL    
                psLista = toLista.Where(x => !poListaSucursal.Contains(x.Sucursal)).Select(x => x.Sucursal).Distinct().ToList();
                if (psLista.Count > 0)
                {
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    DialogResult dialogResult = XtraMessageBox.Show("No extisten los siguientes registros: \n" + psMensaje, "¿Crear Registros de Sucursal?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        piContador = 0;
                        foreach (var item in psLista)
                        {
                            piContador++;
                            string psCodigo = item.Substring(0, 1) + piContador.ToString("00");
                            new clsNCatalogo().gbGuardar(new Catalogo()
                            {
                                Codigo = psCodigo,
                                CodigoGrupo = Diccionario.ListaCatalogo.Sucursal,
                                Descripcion = item,
                                CodigoEstado = Diccionario.Activo,
                                Usuario = clsPrincipal.gsUsuario,
                                Fecha = DateTime.Now,
                                Terminal = clsPrincipal.gsTerminal
                            });
                        }
                    }
                    else
                        pbReturn = false;
                }
                // Fin
                // Inicio Valida e inserta datos de TIPO CONTRATO    
                psLista = toLista.Where(x => !poListaTipoContrato.Contains(x.TipoContrato)).Select(x => x.TipoContrato).Distinct().ToList();
                if (psLista.Count > 0)
                {
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    DialogResult dialogResult = XtraMessageBox.Show("No extisten los siguientes registros: \n" + psMensaje, "¿Crear Registros de Tipo de Contrato?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {

                        foreach (var item in psLista)
                        {
                            new clsNTipoContrato().gbGuardar(new Maestro()
                            {
                                Descripcion = item,
                                CodigoEstado = Diccionario.Activo,
                                Usuario = clsPrincipal.gsUsuario,
                                Fecha = DateTime.Now,
                                Terminal = clsPrincipal.gsTerminal
                            });
                        }
                    }
                    else
                        pbReturn = false;
    
                }
                // Fin
                // Inicio Valida e inserta datos de DEPARTAMENTO    
                psLista = toLista.Where(x => !poListaDepartamento.Contains(x.Departamento)).Select(x => x.Departamento).Distinct().ToList();
                if (psLista.Count > 0)
                {
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    DialogResult dialogResult = XtraMessageBox.Show("No extisten los siguientes registros: \n" + psMensaje, "¿Crear Registros de Departamento?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        piContador = 0;
                        foreach (var item in psLista)
                        {
                            piContador++;
                            string psCodigo = item.Substring(0, 1) + piContador.ToString("00");
                            new clsNCatalogo().gbGuardar(new Catalogo()
                            {
                                Codigo = psCodigo,
                                CodigoGrupo = Diccionario.ListaCatalogo.Departamento,
                                Descripcion = item,
                                CodigoEstado = Diccionario.Activo,
                                Usuario = clsPrincipal.gsUsuario,
                                Fecha = DateTime.Now,
                                Terminal = clsPrincipal.gsTerminal
                            });
                        }
                    }
                    else
                        pbReturn = false;
                }
                // Fin
                // Inicio Valida e inserta datos de TIPO COMISION    
                psLista = toLista.Where(x => !poListaTipoComision.Contains(x.TipodeComision)).Where(x=>x.TipodeComision != null).Select(x => x.TipodeComision).Distinct().ToList();
                if (psLista.Count > 0)
                {
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    DialogResult dialogResult = XtraMessageBox.Show("No extisten los siguientes registros: \n" + psMensaje, "¿Crear Registros de Tipo de Comisión?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        piContador = 0;
                        foreach (var item in psLista)
                        {
                            new clsNTipoComision().gbGuardar(new TipoComision()
                            {
                                Descripcion = item,
                                CodigoEstado = Diccionario.Activo,
                                Usuario = clsPrincipal.gsUsuario,
                                Fecha = DateTime.Now,
                                Terminal = clsPrincipal.gsTerminal
                            });
                        }
                    }
                    else
                        pbReturn = false;
                }
                // Fin
                // Inicio Valida e inserta datos de CENTRO DE COSTO    
                psLista = toLista.Where(x => !poListaCentroCosto.Contains(x.CentrodeCosto)).Where(x => x.CentrodeCosto != null).Select(x => x.CentrodeCosto).Distinct().ToList();
                if (psLista.Count > 0)
                {
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    DialogResult dialogResult = XtraMessageBox.Show("No extisten los siguientes registros: \n" + psMensaje, "¿Crear Registros de Centro de Costo?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                      
                        foreach (var item in psLista)
                        {
                            new clsNCentroCosto().gbGuardar(new Maestro()
                            {
                                Descripcion = item,
                                CodigoEstado = Diccionario.Activo,
                                Usuario = clsPrincipal.gsUsuario,
                                Fecha = DateTime.Now,
                                Terminal = clsPrincipal.gsTerminal
                            });
                        }
                    }
                    else
                        pbReturn = false;
                }
                // Fin
                // Inicio Valida e inserta datos de CARGO LABORAL    
                psLista = toLista.Where(x => !poListaCargoLaboral.Contains(x.CargoLaboral)).Where(x => x.CargoLaboral != null).Select(x => x.CargoLaboral).Distinct().ToList();
                if (psLista.Count > 0)
                {
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    DialogResult dialogResult = XtraMessageBox.Show("No extisten los siguientes registros: \n" + psMensaje, "¿Crear Registros de Cargo Laboral?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        foreach (var item in psLista)
                        {
                            new clsNCargoLaboral().gbGuardar(new Maestro()
                            {
                                Descripcion = item,
                                CodigoEstado = Diccionario.Activo,
                                Usuario = clsPrincipal.gsUsuario,
                                Fecha = DateTime.Now,
                                Terminal = clsPrincipal.gsTerminal
                            });
                        }
                    }
                    else
                        pbReturn = false;
                }
                // Fin
                // Inicio Valida e inserta datos de BANCO    
                psLista = toLista.Where(x => !poListaBanco.Contains(x.Banco)).Where(x => !string.IsNullOrEmpty(x.Banco)).Select(x => x.Banco).Distinct().ToList();
                if (psLista.Count > 0)
                {
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    DialogResult dialogResult = XtraMessageBox.Show("No extisten los siguientes registros: \n" + psMensaje, "¿Crear Registros de Banco?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        piContador = 0;
                        foreach (var item in psLista)
                        {
                            piContador++;
                            string psCodigo = item.Substring(0, 1) + piContador.ToString("00");
                            new clsNCatalogo().gbGuardar(new Catalogo()
                            {
                                Codigo = psCodigo,
                                CodigoGrupo = Diccionario.ListaCatalogo.Banco,
                                Descripcion = item,
                                CodigoEstado = Diccionario.Activo,
                                Usuario = clsPrincipal.gsUsuario,
                                Fecha = DateTime.Now,
                                Terminal = clsPrincipal.gsTerminal
                            });
                        }
                    }
                    else
                        pbReturn = false;
                }
                // Fin
                // Inicio Valida e inserta datos de FORMA DE PAGO    
                psLista = toLista.Where(x => !poListaFormaPago.Contains(x.FormadePago)).Where(x => !string.IsNullOrEmpty(x.FormadePago)).Select(x => x.FormadePago).Distinct().ToList();
                if (psLista.Count > 0)
                {
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    DialogResult dialogResult = XtraMessageBox.Show("No extisten los siguientes registros: \n" + psMensaje, "¿Crear Registros de Forma de Pago?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        piContador = 0;
                        foreach (var item in psLista)
                        {
                            piContador++;
                            string psCodigo = item.Substring(0, 1) + piContador.ToString("00");
                            new clsNCatalogo().gbGuardar(new Catalogo()
                            {
                                Codigo = psCodigo,
                                CodigoGrupo = Diccionario.ListaCatalogo.FormaPago,
                                Descripcion = item,
                                CodigoEstado = Diccionario.Activo,
                                Usuario = clsPrincipal.gsUsuario,
                                Fecha = DateTime.Now,
                                Terminal = clsPrincipal.gsTerminal
                            });
                        }
                    }
                    else
                        pbReturn = false;
                }
                // Fin
                // Inicio Valida e inserta datos de TIPO DE CUENTA 
                psLista = toLista.Where(x => !poListaTipoCuenta.Contains(x.TipoCuenta)).Where(x => !string.IsNullOrEmpty(x.TipoCuenta)).Select(x => x.TipoCuenta).Distinct().ToList();
                if (psLista.Count > 0)
                {
                    psMensaje = string.Empty;
                    psLista.ForEach(x => psMensaje = psMensaje + x + "\n");
                    DialogResult dialogResult = XtraMessageBox.Show("No extisten los siguientes registros: \n" + psMensaje, "¿Crear Registros de Tipo de Cuenta Bancaria?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        piContador = 0;
                        foreach (var item in psLista)
                        {
                            piContador++;
                            string psCodigo = item.Substring(0, 1) + piContador.ToString("00");
                            new clsNCatalogo().gbGuardar(new Catalogo()
                            {
                                Codigo = psCodigo,
                                CodigoGrupo = Diccionario.ListaCatalogo.TipoCuentaBancaria,
                                Descripcion = item,
                                CodigoEstado = Diccionario.Activo,
                                Usuario = clsPrincipal.gsUsuario,
                                Fecha = DateTime.Now,
                                Terminal = clsPrincipal.gsTerminal
                            });
                        }
                    }
                    else
                        pbReturn = false;
                }
                // Fin



                return pbReturn;
            }
            else
            {
                return false;
            }
            
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
