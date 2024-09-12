using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTA_Negocio
{
   public class clsNImpuestoRentaDeducible : clsNBase
    {

        /// <summary>
        /// Listar Maestros
        /// </summary>
        /// <param name="tsDescripcion"></param>
        /// <returns></returns>
        public List<Persona> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<GENMPERSONA>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Persona
                   {
                       IdPersona = x.IdPersona,
                       NombreCompleto = x.NombreCompleto,
                       CodigoEstado = x.CodigoEstado,
                    
                   }).ToList();
        }

        public List<ImpuestoRentaDeducibleGrid> goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<REHTIMPUESTORENTADEDUCIBLE>().Where(x => x.IdPersona.ToString() == tsCodigo && x.CodigoEstado != Diccionario.Inactivo)
                .Select(x => new ImpuestoRentaDeducibleGrid
                {
                    IdImpuestoRentaDeducible = x.IdImpuestoRentaDeducible,
                    anio = x.Anio,
                    Vivienda = x.Vivienda,
                    Educacion = x.Educacion,
                    Salud = x.Salud,
                    Vestimenta = x.Vestimenta,
                    Alimentacion = x.Alimentacion,
                    Turismo = x.Turismo
              
                }).ToList();
        }

        public string gsGuardar(List<ImpuestoRentaDeducibleGrid> toObject,int idPersona, string psUsuario, string psTerminal)
        {
            string psReturn = string.Empty;
            loBaseDa.CreateContext();
            bool pbResult = false;
            string psCodigo = string.Empty;

            var poLista= loBaseDa.Get<REHTIMPUESTORENTADEDUCIBLE>().Where(x => x.IdPersona == idPersona && x.CodigoEstado!=Diccionario.Inactivo).ToList();

            List<int> poListaIdModificarProveedor = toObject.Select(x => x.IdImpuestoRentaDeducible).ToList();
            List<int> piListaIdEliminarProveedor = poLista.Where(x => x.CodigoEstado != Diccionario.Eliminado && !poListaIdModificarProveedor.Contains(x.IdImpuestoRentaDeducible)).Select(x => x.IdImpuestoRentaDeducible).ToList();

            foreach (var poImpuesto in poLista.Where(x => x.CodigoEstado != Diccionario.Inactivo && piListaIdEliminarProveedor.Contains(x.IdImpuestoRentaDeducible)))
            {
                poImpuesto.CodigoEstado = Diccionario.Inactivo;
                poImpuesto.UsuarioModificacion = psUsuario;
                poImpuesto.FechaModificacion = DateTime.Now;
                poImpuesto.TerminalModificacion = psTerminal;
            }
            foreach (var Impuesto in toObject)
            {
                if (!string.IsNullOrEmpty(Impuesto.IdImpuestoRentaDeducible.ToString())) psCodigo = Impuesto.IdImpuestoRentaDeducible.ToString();
                var poObject = poLista.Where(x => x.IdImpuestoRentaDeducible.ToString() == psCodigo).FirstOrDefault();
                if (poObject != null)
                {
                    // poObject.IdPerfil = toObject.IdPerfil ;
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.Anio = Impuesto.anio;
                    poObject.Vivienda = Impuesto.Vivienda;
                    poObject.Educacion = Impuesto.Educacion;
                    poObject.Salud = Impuesto.Salud;
                    poObject.Vestimenta = Impuesto.Vestimenta;
                    poObject.Alimentacion = Impuesto.Alimentacion;
                    poObject.Turismo = Impuesto.Turismo;
                    poObject.Total = Impuesto.Total;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = psUsuario;
                    poObject.TerminalModificacion = psTerminal;
                    //poObject.CodigoFlujoCompras = toObject.CodigoFlujoCompras;


                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, psUsuario, psTerminal);

                    pbResult = true;
                }
                else
                {

                    poObject = new REHTIMPUESTORENTADEDUCIBLE();

                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.IdPersona = idPersona;
                    poObject.Anio = Impuesto.anio;
                    poObject.Vivienda = Impuesto.Vivienda;
                    poObject.Educacion = Impuesto.Educacion;
                    poObject.Salud = Impuesto.Salud;
                    poObject.Vestimenta = Impuesto.Vestimenta;
                    poObject.Alimentacion = Impuesto.Alimentacion;
                    poObject.Turismo = Impuesto.Turismo;
                    poObject.Total = Impuesto.Total;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.UsuarioIngreso = psUsuario;
                    poObject.TerminalIngreso = psTerminal;




                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, psUsuario, psTerminal);

                    pbResult = true;
                    loBaseDa.SaveChanges();

                }

            } 

                loBaseDa.SaveChanges();

            return psReturn;
        }






    }
   


}
