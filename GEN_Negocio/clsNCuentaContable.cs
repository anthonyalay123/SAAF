using GEN_Entidad;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Negocio
{
    public class clsNCuentaContable : clsNBase
    {

        
        #region Cuentas Contables por Centro de Costo y Rubro

        public string gbGuardarCuentaContableCentroCostoRubro(string tsCodigoCentroCosto, string tsCodigoRubro, string tsCuentaContable, string tsUsuario, string tsTerminal)
        {
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<GENPRUBROCENTROCOSTOCC>().Where(x => x.CodigoCentroCosto == tsCodigoCentroCosto && x.CodigoRubro == tsCodigoRubro).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CuentaContable = tsCuentaContable;
                poObject.FechaModificacion = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);

                loBaseDa.SaveChanges();
            }
            else
            {
                poObject = new GENPRUBROCENTROCOSTOCC();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CuentaContable = tsCuentaContable;
                poObject.CodigoRubro = tsCodigoRubro;
                poObject.CodigoCentroCosto = tsCodigoCentroCosto;
                poObject.CodigoEstado = Diccionario.Activo;
                poObject.Descripcion = string.Empty;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioIngreso = tsUsuario;
                poObject.TerminalIngreso = tsTerminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
            }


            loBaseDa.SaveChanges();

            return psResult;
        }

        public DataTable gdtConsultaParametrizacionesCentroCostoRubro()
        {
            return loBaseDa.DataTable("SELECT CC.CodigoCentroCosto CodigoCC, CC.Descripcion CentroCosto, R.CodigoRubro, R.Descripcion Rubro,	RCC.CuentaContable FROM 	GENPRUBROCENTROCOSTOCC RCC (NOLOCK)	INNER JOIN REHPRUBRO R (NOLOCK) ON RCC.CodigoRubro = R.CodigoRubro AND R.CodigoEstado = 'A'	INNER JOIN GENMCENTROCOSTO CC (NOLOCK) ON RCC.CodigoCentroCosto = CC.CodigoCentroCosto AND CC.CodigoEstado = 'A' WHERE 	RCC.CodigoEstado = 'A'");
        }

        public string gsConsultarCuentaContableRubroCentroCosto(string tsCodigoRubro, string tsCodigoCentroCosto)
        {
            return loBaseDa.Find<GENPRUBROCENTROCOSTOCC>().Where(x => x.CodigoRubro == tsCodigoRubro && x.CodigoCentroCosto == tsCodigoCentroCosto).Select(x => x.CuentaContable).FirstOrDefault();
        }
        #endregion

        #region Cuentas Contables por Centro de Costo y Tipo de Beneficio Social
        public string gbGuardarCuentaContableCentroCostoTipoBeneficioSocial(string tsCodigoCentroCosto, string tsCodigoRubro, string tsCuentaContable, string tsUsuario, string tsTerminal)
        {
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<GENPTIPOBSCENTROCOSTOCC>().Where(x => x.CodigoCentroCosto == tsCodigoCentroCosto && x.Codigo == tsCodigoRubro).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CuentaContable = tsCuentaContable;
                poObject.FechaModificacion = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);

                loBaseDa.SaveChanges();
            }
            else
            {
                poObject = new GENPTIPOBSCENTROCOSTOCC();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CuentaContable = tsCuentaContable;
                poObject.Codigo = tsCodigoRubro;
                poObject.CodigoCentroCosto = tsCodigoCentroCosto;
                poObject.CodigoEstado = Diccionario.Activo;
                poObject.Descripcion = string.Empty;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioIngreso = tsUsuario;
                poObject.TerminalIngreso = tsTerminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
            }


            loBaseDa.SaveChanges();

            return psResult;
        }

        public DataTable gdtConsultaParametrizacionesCentroCostoTipoBS()
        {
            return loBaseDa.DataTable("SELECT CC.CodigoCentroCosto CodigoCC, CC.Descripcion CentroCosto, R.Codigo, R.Descripcion TipoBS,	RCC.CuentaContable FROM GENPTIPOBSCENTROCOSTOCC RCC (NOLOCK)	INNER JOIN GENMCATALOGO R (NOLOCK) ON r.CodigoGrupo = '026' AND RCC.Codigo = R.Codigo AND R.CodigoEstado = 'A'	INNER JOIN GENMCENTROCOSTO CC (NOLOCK) ON RCC.CodigoCentroCosto = CC.CodigoCentroCosto AND CC.CodigoEstado = 'A' WHERE 	RCC.CodigoEstado = 'A'");
        }

        public string gsConsultarCuentaContableTipoBSCentroCosto(string tsCodigoRubro, string tsCodigoCentroCosto)
        {
            return loBaseDa.Find<GENPTIPOBSCENTROCOSTOCC>().Where(x => x.Codigo == tsCodigoRubro && x.CodigoCentroCosto == tsCodigoCentroCosto).Select(x => x.CuentaContable).FirstOrDefault();
        }
        #endregion


        #region Cuentas Contables por Tipo de Transporte y Zona SAP
        public string gsGuardarCuentaContableTipoTransporteZona(string tsCodigoTipoTransporte, string tsCodigoZona, string tsCuentaContable, string tsUsuario, string tsTerminal)
        {
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<GENPTIPOTRANSPORTEZONACC>().Where(x => x.CodigoTipoTransporte == tsCodigoTipoTransporte && x.CodZona == tsCodigoZona).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CuentaContable = tsCuentaContable;
                poObject.FechaModificacion = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);

                loBaseDa.SaveChanges();
            }
            else
            {
                poObject = new GENPTIPOTRANSPORTEZONACC();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CuentaContable = tsCuentaContable;
                poObject.CodZona = tsCodigoZona;
                poObject.CodigoTipoTransporte = tsCodigoTipoTransporte;
                poObject.CodigoEstado = Diccionario.Activo;
                poObject.Descripcion = string.Empty;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioIngreso = tsUsuario;
                poObject.TerminalIngreso = tsTerminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
            }


            loBaseDa.SaveChanges();

            return psResult;
        }

        public DataTable gdtConsultaParametrizacionesTipoTranspoteZona()
        {
            return loBaseDa.DataTable("SELECT R.Codigo, R.Descripcion TipoTransporte, CC.Code CodigoZona, CC.[Name] Zona, RCC.CuentaContable FROM [dbo].[GENPTIPOTRANSPORTEZONACC] RCC (NOLOCK) INNER JOIN GENMCATALOGO R (NOLOCK) ON R.CodigoGrupo = '038' AND RCC.CodigoTipoTransporte = R.Codigo AND R.CodigoEstado = 'A' INNER JOIN SBO_AFECOR.dbo.[@AFE_ZONAS] CC (NOLOCK) ON RCC.CodZona COLLATE DATABASE_DEFAULT = CC.Code WHERE RCC.CodigoEstado = 'A'");
        }

        public string gsConsultarCuentaContableTipoTransporteZona(string tsCodigoZona, string tsCodigoTipoTransporte)
        {
            return loBaseDa.Find<GENPTIPOTRANSPORTEZONACC>().Where(x => x.CodZona == tsCodigoZona && x.CodigoTipoTransporte == tsCodigoTipoTransporte).Select(x => x.CuentaContable).FirstOrDefault();
        }

        public string gEliminarParametrizacionCuentaContableTipoTransporteZona(string tsCodigoTipoTransporte, string tsCodigoZona,string tsUsuario, string tsTerminal)
        {
            string psMsg = "";

            var poObject = loBaseDa.Get<GENPTIPOTRANSPORTEZONACC>().Where(x => x.CodigoEstado == Diccionario.Activo 
            && x.CodigoTipoTransporte == tsCodigoTipoTransporte && x.CodZona == tsCodigoZona).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                poObject.FechaModificacion = DateTime.Now;

                if (string.IsNullOrEmpty(psMsg))
                {
                    loBaseDa.SaveChanges();
                }
            }

            return psMsg;
        }

        public string gEliminarParametrizacionCuentaContableTipoTransporteAlmacen(string tsCodigoTipoTransporte, string tsCodigoAlmacen, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";

            var poObject = loBaseDa.Get<GENPTIPOTRANSPORTEALMACENCC>().Where(x => x.CodigoEstado == Diccionario.Activo
            && x.CodigoTipoTransporte == tsCodigoTipoTransporte && x.CodAlmacen == tsCodigoAlmacen).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                poObject.FechaModificacion = DateTime.Now;

                if (string.IsNullOrEmpty(psMsg))
                {
                    loBaseDa.SaveChanges();
                }
            }

            return psMsg;
        }


        #endregion

        #region Cuentas Contables por Tipo de Transporte y Almacen SAP
        public string gsGuardarCuentaContableTipoTransporteAlmacen(string tsCodigoTipoTransporte, string tsCodigoAlmacen, string tsCuentaContable, string tsUsuario, string tsTerminal)
        {
            string psResult = string.Empty;

            var poObject = loBaseDa.Get<GENPTIPOTRANSPORTEALMACENCC>().Where(x => x.CodigoTipoTransporte == tsCodigoTipoTransporte && x.CodAlmacen == tsCodigoAlmacen).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CuentaContable = tsCuentaContable;
                poObject.FechaModificacion = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);

                loBaseDa.SaveChanges();
            }
            else
            {
                poObject = new GENPTIPOTRANSPORTEALMACENCC();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CuentaContable = tsCuentaContable;
                poObject.CodAlmacen = tsCodigoAlmacen;
                poObject.CodigoTipoTransporte = tsCodigoTipoTransporte;
                poObject.CodigoEstado = Diccionario.Activo;
                poObject.Descripcion = string.Empty;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioIngreso = tsUsuario;
                poObject.TerminalIngreso = tsTerminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
            }


            loBaseDa.SaveChanges();

            return psResult;
        }

        public DataTable gdtConsultaParametrizacionesTipoTranspoteAlmacen()
        {
            return loBaseDa.DataTable("SELECT R.Codigo, R.Descripcion TipoTransporte, CC.WhsCode CodigoAlmacen, CC.WhsName Almacen, RCC.CuentaContable FROM [dbo].[GENPTIPOTRANSPORTEALMACENCC] RCC (NOLOCK) INNER JOIN GENMCATALOGO R (NOLOCK) ON R.CodigoGrupo = '038' AND RCC.CodigoTipoTransporte = R.Codigo AND R.CodigoEstado = 'A' INNER JOIN [SBO_AFECOR].DBO.[OWHS] CC (NOLOCK) ON RCC.CodAlmacen COLLATE DATABASE_DEFAULT = CC.WhsCode WHERE RCC.CodigoEstado = 'A'");
        }

        public string gsConsultarCuentaContableTipoTransporteAlmacen(string tsCodigoAlmacen, string tsCodigoTipoTransporte)
        {
            return loBaseDa.Find<GENPTIPOTRANSPORTEALMACENCC>().Where(x => x.CodAlmacen == tsCodigoAlmacen && x.CodigoTipoTransporte == tsCodigoTipoTransporte).Select(x => x.CuentaContable).FirstOrDefault();
        }

        #endregion










    }
}
