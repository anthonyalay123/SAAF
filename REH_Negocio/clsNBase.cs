using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using GEN_Entidad;
using System.Data;
using System.Data.SqlClient;

namespace REH_Negocio
{
    /// <summary>
    /// Clase base de Lógica de Negocio para Formularios
    /// Victor Arévalo
    /// 06/02/2020
    /// </summary>
    public class clsNBase
    {
        #region Variables
        public readonly clsDBase loBaseDa = new clsDBase();
        #endregion


        /// <summary>
        /// Consulta tipo de rol, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoRol()
        {
            return loBaseDa.Find<REHMTIPOROL>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoTipoRol,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Estado Empleado, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboEstadoRegistroEmpleado()
        {
            List<Combo> poLista = new List<Combo>();
            poLista.Add(new Combo() { Codigo = Diccionario.Activo, Descripcion = Diccionario.DesActivo });
            poLista.Add(new Combo() { Codigo = Diccionario.Inactivo, Descripcion = Diccionario.DesInactivo });
            poLista.Add(new Combo() { Codigo = Diccionario.Jubilado, Descripcion = Diccionario.DesJubilado });
            return poLista;
        }

        /// <summary>
        /// Consulta Estado, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboEstadoRegistro(bool tbAddEstadoEliminado = false)
        {
            List<Combo> poLista = new List<Combo>();
            poLista.Add(new Combo() { Codigo = Diccionario.Activo, Descripcion = Diccionario.DesActivo });
            poLista.Add(new Combo() { Codigo = Diccionario.Inactivo, Descripcion = Diccionario.DesInactivo });
            if (tbAddEstadoEliminado) poLista.Add(new Combo() { Codigo = Diccionario.Eliminado, Descripcion = Diccionario.DesEliminado });
            return poLista;
        }

        /// <summary>
        /// Consulta Empleados Activos
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboEmpleado()
        {
            return (from a in loBaseDa.Find<GENMPERSONA>()
                    join b in loBaseDa.Find<REHPEMPLEADO>() on a.IdPersona equals b.IdPersona 
                    join c in loBaseDa.Find<REHPEMPLEADOCONTRATO>() on b.IdPersona equals c.IdPersona
                    where a.CodigoEstado == Diccionario.Activo && b.CodigoEstado == Diccionario.Activo && c.CodigoEstado == Diccionario.Activo
                    select new Combo
                    {
                        Codigo = a.IdPersona.ToString(),
                        Descripcion = a.NombreCompleto
                    }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Contratos Activos
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<EmpleadoContrato> goConsultarContratos()
        {
            return (from a in loBaseDa.Find<REHPEMPLEADOCONTRATO>()
                    where a.CodigoEstado == Diccionario.Activo
                    select new EmpleadoContrato
                    {
                        IdEmpleadoContrato = a.IdEmpleadoContrato,
                        IdPersona = a.IdPersona,
                        CodigoTipoComision = a.CodigoTipoComision
                    }).ToList();
        }

        /// <summary>
        /// Consulta de Perdido
        /// </summary>
        /// <param name="tIdPeriodo">Id Periodo</param>
        /// <returns></returns>
        public Periodo goConsultarPeriodo(int tIdPeriodo)
        {
            return (from a in loBaseDa.Find<REHPPERIODO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.IdPeriodo == tIdPeriodo
                    select new Periodo()
                    {
                        IdPeriodo = a.IdPeriodo,
                        FechaInicio = a.FechaInicio,
                        FechaFin = a.FechaFin,
                        CodigoTipoRol = a.CodigoTipoRol,
                        TipoRol = b.Descripcion,
                        Codigo = a.Codigo,
                        Anio = a.Anio,
                        
                    }).FirstOrDefault();
        }


        /// <summary>
        /// Consulta tipo de Permiso, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoPermiso()
        {
            return loBaseDa.Find<REHPTIPOPERMISO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoTipoPermiso,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        #region PARÁMETRO
        public int gIdPeriodoSiguiente(out string tsMensaje, List<int> tiCodigoPeriodos = null)
        {
            int piResult = 0;
            tsMensaje = string.Empty;

            var poPeriodos = loBaseDa.Find<REHPPERIODO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                .Select(x => new { x.IdPeriodo, x.Anio, x.FechaInicio, x.FechaFin }).ToList();

            if (poPeriodos.Count == 0)
            {
                tsMensaje = "No existen periodos parametrizados.";
                return piResult;
            } 

            var poParametro = loBaseDa.Find<GENPPARAMETRO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                .Select(x => new { x.AnioInicioNomina, x.MesInicioNomina }).FirstOrDefault();

            if (poParametro == null || poParametro.AnioInicioNomina == null || poParametro.MesInicioNomina == null)
            {
                tsMensaje = "No existen parametrizados en año y mes de inicio de Nómina.";
                return piResult;
            }
                

            var poTipoRol = loBaseDa.Find<REHMTIPOROL>().Where(x => x.CodigoEstado == Diccionario.Activo)
                .Select(x => new { x.CodigoTipoRol, x.OrdenPagoMensual }).ToList();

            if (poTipoRol.Count == 0)
            {
                tsMensaje = "No existen Tipo de Roles parametrizados.";
                return piResult;
            }

            List<int> piPeriodosEnNomina = loBaseDa.Find<REHTNOMINA>().Where(x => !Diccionario.EstadosNoIncluyentesNomina.Contains(x.CodigoEstado)).Select(x=>x.IdPeriodo).Distinct().ToList();
            if( tiCodigoPeriodos != null) piPeriodosEnNomina.AddRange(tiCodigoPeriodos);
            DateTime pdFecha = new DateTime(poParametro.AnioInicioNomina ?? 0, poParametro.MesInicioNomina ?? 0, 1);
            var poPeriodosValidos = poPeriodos.Where(x => x.FechaFin >= pdFecha && !piPeriodosEnNomina.Contains(x.IdPeriodo)).OrderBy(x=>x.FechaFin).ToList();

            if(poPeriodosValidos.Count > 0)
            {
                piResult = poPeriodosValidos.FirstOrDefault().IdPeriodo;
            }
            else
            {
                tsMensaje = "No existen periodos vigentes.";
                return piResult;
            }
            
            return piResult;
        }
        #endregion

        #region CATALOGO
        /// <summary>
        /// Consulta Catálogo de maestros.
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarCatalogo(string tsCodigoGrupo)
        {
            return loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo == tsCodigoGrupo && x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.Codigo,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de maestros.
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarCatalogoGrupo()
        {
            return loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoGrupo == Diccionario.ListaCatalogo.GrupoCatalogo 
                                                && x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.Codigo,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de Periodo
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboPeriodo(string tsCodigoTipoRol = "")
        {
            return (from a in loBaseDa.Find<REHPPERIODO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.CodigoEstado == Diccionario.Activo
                    && tsCodigoTipoRol == "" ? true : a.CodigoTipoRol == tsCodigoTipoRol
                    orderby a.FechaFin
                    select new Combo()
                    {
                        Codigo = a.IdPeriodo.ToString(),
                        Descripcion = a.Codigo +" - "+b.Descripcion
                    }).ToList();
        }

        /// <summary>
        /// Consulta Catálogo de Periodo
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboPeriodosCerrados(string tsCodigoTipoRol = "")
        {
            return (from a in loBaseDa.Find<REHPPERIODO>()
                    join b in loBaseDa.Find<REHMTIPOROL>() on a.CodigoTipoRol equals b.CodigoTipoRol
                    where a.CodigoEstado == Diccionario.Activo && a.CodigoEstadoNomina == Diccionario.Cerrado
                    && tsCodigoTipoRol == "" ? true : a.CodigoTipoRol == tsCodigoTipoRol
                    orderby a.FechaFin descending
                    select new Combo()
                    {
                        Codigo = a.IdPeriodo.ToString(),
                        Descripcion = a.Codigo + " - " + b.Descripcion
                    }).ToList();
        }


        /// <summary>
        /// Consulta Catálogo de Tipo de Persona
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoPersona()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoPersona);
        }

        /// <summary>
        /// Consulta Catálogo de Tipo de Identificación
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoIdentificación()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoIdentificacion);
        }

        /// <summary>
        /// Consulta Catálogo de Nivel de Educación
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboNivelEducaciona()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.NivelEducacion);
        }
        /// <summary>
        /// Consulta Catálogo Región
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboRegion()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Region);
        }
        /// <summary>
        /// Consulta Catálogo Tipo Discapacidad
        /// </summary>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoDiscapacidad()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoDiscapacidad);
        }
        /// <summary>
        /// Consulta Catálogo de Color de Piel
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboColorPiel()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.ColorPiel);
        }
        /// <summary>
        /// Consulta Catálogo Color de Ojos
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboColorOjos()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.ColorOjos);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Sangre
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoSangre()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoSangre);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Licencia
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoLicencia()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoLicencia);
        }
        /// <summary>
        /// Consulta Catálogo de Género
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboGenero()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Genero);
        }
        /// <summary>
        /// Consulta Catálogo de Estado Civil
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboEstadoCivil()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.EstadoCivil);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Carga Familiar
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoCargaFamiliar()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoCargaFamiliar);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Contacto
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoContacto()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoContacto);
        }
        /// <summary>
        /// Consulta Catálogo de Parentezco
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboParentezco()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Parentezco);
        }
        /// <summary>
        /// Consulta Catálogo de Sucursal
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboSucursal()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Sucursal);
        }
        /// <summary>
        /// Consulta Catálogo de Departamento
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboDepartamento ()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Departamento);
        }
        /// <summary>
        /// Consulta Catálogo de Motivo Fin Contrato
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboMotivoFinContrato()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.MotivoFinContrato);
        }
        /// <summary>
        /// Consulta Catálogo de Banco
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboBanco()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.Banco);
        }
        /// <summary>
        /// Consulta Catálogo de Forma de Pago
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboFormaPago()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.FormaPago);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Cuenta Bamcaria
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoCuentaBancaria()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoCuentaBancaria);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Empleado
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoEmpleado()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoEmpleado);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Vivienda
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoVivienda()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoVivienda);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Material de Vivienda
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoMaterialVivienda()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoMaterialVivienda);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Contrato
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoContrato()
        {

            return loBaseDa.Find<REHMTIPOCONTRATO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoTipoContrato,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
            //return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoContrato);
        }
        /// <summary>
        /// Consulta Catálogo de Tipo de Caracteristica Rubro
        /// </summary>
        /// <param name="tsCodigoGrupo">Código del grupo del maestro a Consultar</param>
        /// <returns>Listado de Maestro</returns>
        public List<Combo> goConsultarComboTipoCatalogoRubro()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoCategoriaRubro);
        }
        /// <summary>
        /// Consulta catálogo de Tipo de Contabilización
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoContabilizacion()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoContabilizacion);
        }
        /// <summary>
        /// Consulta catálogo de Tipo de vacación
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoVacacion()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoVacacion);
        }

        /// <summary>
        /// Consulta catálogo de Tipo de Beneficio Social
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoBeneficioSocial()
        {
            return goConsultarCatalogo(Diccionario.ListaCatalogo.TipoBeneficioSocial);
        }
        #endregion

        /// <summary>
        /// Consulta tipo de Rubro, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoRubro()
        {
            return loBaseDa.Find<REHMTIPORUBRO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoTipoRubro,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }
        /// <summary>
        /// Consulta Centro de Costo, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboRubroCentroCosto()
        {
            var poLista = new List<Combo>();
            var dt = loBaseDa.DataTable("EXEC REHSPCMBRUBROCENTROCOSTO");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var poRegistro = new Combo();
                poRegistro.Codigo = dt.Rows[i][0].ToString(); 
                poRegistro.Descripcion = dt.Rows[i][1].ToString();

                poLista.Add(poRegistro);
            }

            return poLista;
        }

        /// <summary>
        /// Consulta Centro de Costo, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboCentroCosto()
        {
            return loBaseDa.Find<GENMCENTROCOSTO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoCentroCosto,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Cargo Laboral, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboCargo()
        {
            return loBaseDa.Find<REHMCARGOLABORAL>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoCargoLaboral,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }
        /// <summary>
        /// Consulta Cargo Laboral, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoComision()
        {
            return loBaseDa.Find<REHMTIPOCOMISION>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoTipoComision,
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta País, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboPais()
        {
            return loBaseDa.Find<GENPPAIS>().Where(x => x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.IdPais.ToString(),
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Provincia, para llenar combo
        /// </summary>
        /// <param name="tIdPais">Id del País, parámetro opcional</param>
        /// <returns></returns>
        public List<Combo> goConsultarComboProvincia(int? tIdPais = null)
        {
            return loBaseDa.Find<GENPPROVINCIA>().Where(x => (tIdPais == null || x.IdPais == tIdPais)  && x.CodigoEstado == Diccionario.Activo)
            //return loBaseDa.Find<GENPPROVINCIA>().Where(x =>x.IdPais == tIdPais && x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.IdProvincia.ToString(),
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta Cantón, para llenar combo
        /// </summary>
        /// <param name="tIdCanton">Id de la Provincia, parámetro opcional</param>
        /// <returns></returns>
        public List<Combo> goConsultarComboCanton(int? tIdProvincia = null)
        {
            return loBaseDa.Find<GENPCANTON>().Where(x => (tIdProvincia == null || x.IdProvincia == tIdProvincia) && x.CodigoEstado == Diccionario.Activo)
            //return loBaseDa.Find<GENPCANTON>().Where(x => x.IdProvincia == tIdProvincia && x.CodigoEstado == Diccionario.Activo)
                  .Select(x => new Combo
                  {
                      Codigo = x.IdCanton.ToString(),
                      Descripcion = x.Descripcion
                  }).OrderBy(x => x.Descripcion).ToList();
        }

        /// <summary>
        /// Consulta de Rubros
        /// </summary>
        /// <returns></returns>
        public List<Rubro> goConsultaRubro()
        {
            return loBaseDa.Find<REHPRUBRO>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x =>
              new Rubro
              {
                  Codigo = x.CodigoRubro,
                  CodigoTipoRubro = x.CodigoTipoRubro,
                  NovedadEditable = x.NovedadEditable ?? false,
                  Aportable = x.Iess ?? false,
                  Descripcion = x.Descripcion,
                  EsEntero = x.EsEntero ?? false,
                  Orden = x.Orden ?? 0,
                  Formula = x.Formula ?? "",
                  CodigoCategoriaRubro = x.CodigoCategoriaRubro
              }
            ).ToList();
        }

        /// <summary>
        /// Retorna la lista de acciones parametrizados por perfil del usuario
        /// </summary>
        /// <param name="tIdPerfil"></param>
        /// <returns></returns>
        public List<MenuAccionPerfil> goAccionPerfil(int tIdPerfil)
        {
            return (from a in loBaseDa.Find<GENPMENUACCIONPERFIL>()
                    join b in loBaseDa.Find<GENPACCION>() on a.IdAccion equals b.IdAccion
                    where a.IdPerfil == tIdPerfil && a.CodigoEstado == Diccionario.Activo

                    select new MenuAccionPerfil()
                    {
                        IdAccion = a.IdAccion,
                        IdPerfil = a.IdPerfil,
                        IdMenu = a.IdMenu,
                        CodigoEstado = a.CodigoEstado,
                        NombreAccion = b.Nombre,
                        Orden = b.Orden,
                        Icono = b.Icono,
                        NombreControl = b.NombreControl,
                        MostrarTexto = b.MostrarTexto
                    }).ToList();
        }

        /// <summary>
        /// Retorna Objeto Ménu
        /// </summary>
        /// <param name="tsNombreForma"> Nombre del formulario</param>
        /// <returns></returns>
        public MenuPerfil goConsultarMenu(string tsNombreForma)
        {
            return loBaseDa.Find<GENPMENU>().Where(x => x.NombreForma == tsNombreForma).Select(x => new MenuPerfil()
            {
                IdMenu = x.IdMenu,
                Nombre = x.Nombre
            }).FirstOrDefault();
        }

        public TipoComision goConsultarTipoComision(string tsCodigo)
        {
            return loBaseDa.Find<REHMTIPOCOMISION>().Where(x => x.CodigoTipoComision == tsCodigo)
                .Select(x => new TipoComision
                {
                    Codigo = x.CodigoTipoComision,
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Descripcion,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    Porcentaje = x.Porcentaje ?? 0M,
                    AplicaPorcentajeGrupal = x.AplicaPorcentajeGrupal ?? false,
                    ValidaDiasTrabajados = x.ValidaDiasTrabajados ?? false,
                    EditableCobranza = x.EditableCobranza ?? false,
                    PorcentajeTotalMaximo = x.PorcentajeTotalMaximo
                }).FirstOrDefault();
        }

        public void goActualizarPorcentajeComision(string tsCodigo)
        {
            var poListaComision = loBaseDa.Find<REHMTIPOCOMISION>().Where(x => x.CodigoTipoComision == tsCodigo && x.AplicaPorcentajeGrupal == true).ToList();
            foreach (var poComision in poListaComision)
            {
                //if (poComision.PorcentajeTotalMaximo != null)
                //{
                var poLista = loBaseDa.Get<REHPEMPLEADOCONTRATO>().Where(x => x.CodigoEstado == Diccionario.Activo && x.CodigoTipoComision == tsCodigo).ToList();

                if (poLista.Count() > 0)
                {
                    int piTotal = poLista.Count;
                    decimal piPorcentajeTotal = poComision.Porcentaje??0;
                    decimal pdcPorcentajeComisionEmpleado = Math.Truncate((piPorcentajeTotal / piTotal) * 1000000) / 1000000;

                    foreach (var item in poLista)
                    {
                        if (pdcPorcentajeComisionEmpleado != item.PorcentajeComision)
                        {
                            item.PorcentajeComision = pdcPorcentajeComisionEmpleado;
                            item.FechaModificacion = DateTime.Now;

                        }
                    }
                }

                //}
            }
        }

        public DataTable goConsultaDataTable(string tsCadena)
        {
            return loBaseDa.DataTable(tsCadena);
        }

        public DataTable goConsultaLogPersona(int tIdPersona)
        {
            return loBaseDa.DataTable("EXEC REHSPCONSULTALOGPERSONA @IdPersona = @paramIdPersona ",
                                           new SqlParameter("paramIdPersona", SqlDbType.VarChar) { Value = tIdPersona }
                                         ); 
        }

        public DataTable goConsultaLogEmpleado(int tIdPersona)
        {
            return loBaseDa.DataTable("EXEC REHSPCONSULTALOGEMPLEADO @IdPersona = @paramIdPersona ",
                                           new SqlParameter("paramIdPersona", SqlDbType.VarChar) { Value = tIdPersona }
                                         );
        }

        public DataTable goConsultaLogContrato(int tIdPersona)
        {
            return loBaseDa.DataTable("EXEC REHSPCONSULTALOGCONTRATO @IdPersona = @paramIdPersona ",
                                           new SqlParameter("paramIdPersona", SqlDbType.VarChar) { Value = tIdPersona }
                                         );
        }
    }

}