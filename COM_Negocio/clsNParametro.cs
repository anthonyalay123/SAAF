using GEN_Entidad;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM_Negocio
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 11/05/2020
    /// Clase Unidad de Trabajo o Lógica de Negocio
    /// </summary>
    public class clsNParametroCompras : clsNBase
    {
        /// <summary>
        /// Buscar Entidad
        /// </summary>
        /// <returns></returns>
        public ParametroCompras goBuscarEntidad()
        {
            return loBaseDa.Find<COMPPARAMETRO>()
                //.Where(x => x.Codigo == tsCodigo)
                .Select(x => new ParametroCompras
                {
                    Codigo = x.Id.ToString(),
                    CodigoEmpresaMultiCash = x.CodigoEmpresaMultiCash,
                    CuentaBancariaEmpresa = x.CuentaBancariaEmpresa,
                    CodigoInstitucionFinancieraMultiCash = x.CodigoInstitucionFinancieraMultiCash,
                    Semana = x.Semana,
                    FechaConsultaGuiasDesde = x.FechaConsultaGuiasDesde,
                    PermiteSeleccionarDiferentesBodegasEnGuias = x.PermiteSeleccionarDiferentesBodegasEnGuias??false
                }).FirstOrDefault();
        }
        
        /// <summary>
        /// Guardar Objeto 
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public bool gbGuardar(ParametroCompras toObject, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            bool pbResult = false;
            int piCodigo = 0;
            if (!string.IsNullOrEmpty(toObject.Codigo)) piCodigo = int.Parse(toObject.Codigo);
            var poObject = loBaseDa.Get<COMPPARAMETRO>().Where(x => x.Id == piCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Activo;
                poObject.CodigoEmpresaMultiCash = toObject.CodigoEmpresaMultiCash;
                poObject.CuentaBancariaEmpresa = toObject.CuentaBancariaEmpresa;
                poObject.CodigoInstitucionFinancieraMultiCash = toObject.CodigoInstitucionFinancieraMultiCash;
                poObject.Semana = toObject.Semana;

                poObject.UsuarioModificacion = tsUsuario;
                poObject.FechaModificacion = DateTime.Now;
                poObject.TerminalModificacion = tsTerminal;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, tsUsuario, tsTerminal);
                pbResult = true;
            }
            else
            {
                poObject = new COMPPARAMETRO();
                loBaseDa.CreateNewObject(out poObject);

                poObject.CodigoEstado = Diccionario.Activo;
                poObject.CodigoEmpresaMultiCash = toObject.CodigoEmpresaMultiCash;
                poObject.CuentaBancariaEmpresa = toObject.CuentaBancariaEmpresa;
                poObject.CodigoInstitucionFinancieraMultiCash = toObject.CodigoInstitucionFinancieraMultiCash;
                poObject.Semana = toObject.Semana;

                poObject.UsuarioIngreso = tsUsuario;
                poObject.FechaIngreso = DateTime.Now;
                poObject.TerminalIngreso = tsTerminal;
                
                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, DateTime.Now, tsUsuario, tsTerminal);
                pbResult = true;
            }

            loBaseDa.SaveChanges();
            return pbResult;
        }
    }
}
