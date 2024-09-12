using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REH_Negocio.Parametrizadores
{

   public class clsNDescuentoDiscapacidadIr : clsNBase
    {
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(DescuentoDiscapacitadoIr toObject)
        {
            loBaseDa.CreateContext();
            string psReturn = string.Empty;

            psReturn = lValidar(toObject);
            if (string.IsNullOrEmpty(psReturn))
            {
                string psCodigo = string.Empty;
                if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
                List<string> psCodigoEstadoExlusion = new List<string>();
                psCodigoEstadoExlusion.Add(Diccionario.Eliminado);
                psCodigoEstadoExlusion.Add(Diccionario.Inactivo);
                var poObject = loBaseDa.Get<REHPDESCUENTODISCAPACITADOIR>().Where(x => x.IdDescuentoDiscapacitadoIR.ToString() == psCodigo).FirstOrDefault();

                if (poObject != null)
                {

                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Anio = toObject.Anio;
                    poObject.PorcentajeInicial = toObject.PorcentajeInicial;
                    poObject.PorcentajeFinal = toObject.PorcentajeFinal;
                    poObject.PorcentajeDescuento = toObject.PorcentajeDescuento;
                    poObject.FechaModificacion = toObject.Fecha;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);

                }

                else
                {

                    poObject = new REHPDESCUENTODISCAPACITADOIR();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Anio = toObject.Anio;
                    poObject.PorcentajeInicial = toObject.PorcentajeInicial;
                    poObject.PorcentajeFinal = toObject.PorcentajeFinal;
                    poObject.PorcentajeDescuento = toObject.PorcentajeDescuento;
                    poObject.UsuarioIngreso = toObject.Usuario;
                    poObject.FechaIngreso = toObject.Fecha;
                    poObject.TerminalIngreso = toObject.Terminal;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                }

                loBaseDa.SaveChanges();
            }
            
            return psReturn;

        }


        private string lValidar(DescuentoDiscapacitadoIr toObject)
        {
            string psReturn = string.Empty;
            if (toObject.PorcentajeDescuento<0)
            {
                psReturn += "Falta la descripción \n";
            }
         
            if (toObject.PorcentajeInicial<0)
            {
                psReturn += "Falta el Porcentaje Inicial \n";
            }
            if (toObject.PorcentajeFinal<0)
            {
                psReturn += "Falta el porcentaje Final \n";
            }
            if (toObject.Anio == 0)
            {
                psReturn += "Falta el año \n";
            }


            return psReturn;
        }


        public List<DescuentoDiscapacitadoIr> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<REHPDESCUENTODISCAPACITADOIR>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new DescuentoDiscapacitadoIr
                   {
                       Codigo = x.IdDescuentoDiscapacitadoIR.ToString(),
                       Anio = x.Anio,
                       CodigoEstado = x.CodigoEstado,
                   }).ToList();
        }


        public DescuentoDiscapacitadoIr goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<REHPDESCUENTODISCAPACITADOIR>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdDescuentoDiscapacitadoIR.ToString() == tsCodigo)
                   .Select(x => new DescuentoDiscapacitadoIr
                   {
                       Codigo = x.IdDescuentoDiscapacitadoIR.ToString(),
                       Anio = x.Anio,
                       PorcentajeInicial = x.PorcentajeInicial,
                       CodigoEstado = x.CodigoEstado,
                       PorcentajeFinal = x.PorcentajeFinal,
                       Usuario = x.UsuarioIngreso,
                       UsuarioMod = x.UsuarioModificacion,
                       Terminal = x.TerminalIngreso,
                       TerminalMod = x.TerminalModificacion,
                       Fecha = x.FechaIngreso,
                       FechaMod = x.FechaModificacion,
                       PorcentajeDescuento = x.PorcentajeDescuento,
                

                   }).FirstOrDefault();
        }


        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestro(string tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<REHPDESCUENTODISCAPACITADOIR>().Where(x => x.IdDescuentoDiscapacitadoIR.ToString() == tsCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = Diccionario.Eliminado;
                poObject.FechaIngreso = DateTime.Now;
                poObject.UsuarioModificacion = tsUsuario;
                poObject.TerminalModificacion = tsTerminal;
                loBaseDa.SaveChanges();
            }
        }



    }
}
