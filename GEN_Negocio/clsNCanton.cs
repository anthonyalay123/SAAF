﻿using GEN_Entidad;
using GEN_Entidad.Entidades.General;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Negocio
{
    public class clsNCanton : clsNBase
    {
        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(Canton toObject)
        {
            loBaseDa.CreateContext();
            string psReturn = string.Empty;

            psReturn = lValidar(toObject);
            if (string.IsNullOrEmpty(psReturn))
            {
                //  string psCodigo = string.Empty;
                //if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.CodigoTipoComision;
                List<string> psCodigoEstadoExlusion = new List<string>();
                psCodigoEstadoExlusion.Add(Diccionario.Eliminado);
                psCodigoEstadoExlusion.Add(Diccionario.Inactivo);
                var poObject = loBaseDa.Get<GENPCANTON>().Where(x => x.IdCanton.ToString() == toObject.Codigo && !psCodigoEstadoExlusion.Contains(x.CodigoEstado)).FirstOrDefault();

                if (poObject != null)
                {

                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.IdProvincia = toObject.IdProvincia;
                    poObject.Descripcion = toObject.Descripcion;
                    poObject.FechaModificacion = toObject.Fecha;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;

                    // Insert Auditoría
                    loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);

                }

                else
                {

                    poObject = new GENPCANTON();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.IdProvincia = toObject.IdProvincia;
                    poObject.Descripcion = toObject.Descripcion;
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


        private string lValidar(Canton toObject)
        {
            string psReturn = string.Empty;
            if (string.IsNullOrEmpty(toObject.Descripcion))
            {
                psReturn += "Falta la descripcion \n";
            }
            if (toObject.IdProvincia <= 0)
            {
                psReturn += "Falta la Provincia \n";
            }



            return psReturn;

        }

        public List<Canton> goListarMaestro(string tsDescripcion = "")
        {
            return (from SC in loBaseDa.Find<GENPCANTON>()

                        // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Eliminado
                    // Selección de las columnas / Datos
                    select new Canton
                    {
                        Codigo = SC.IdCanton.ToString(),
                        Descripcion = SC.Descripcion,

                        CodigoEstado = SC.CodigoEstado
                    }).ToList();
        }

        public Canton goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<GENPCANTON>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdCanton.ToString() == tsCodigo)
                   .Select(x => new Canton
                   {
                       Codigo = x.IdCanton.ToString(),
                       IdProvincia = x.IdProvincia,
                       Descripcion = x.Descripcion,
                       CodigoEstado = x.CodigoEstado,

                       Usuario = x.UsuarioIngreso,
                       UsuarioMod = x.UsuarioModificacion,
                       Terminal = x.TerminalIngreso,
                       TerminalMod = x.TerminalModificacion,
                       Fecha = x.FechaIngreso,
                       FechaMod = x.FechaModificacion,

                   }).FirstOrDefault();
        }


        /// <summary>
        /// Elimina Registro Maestro
        /// </summary>
        /// <param name="tsCodigo">código de la entidad</param>
        public void gEliminarMaestro(string tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<GENPCANTON>().Where(x => x.IdCanton.ToString() == tsCodigo).FirstOrDefault();
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
