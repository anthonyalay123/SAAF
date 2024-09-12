using GEN_Entidad;
using GEN_Entidad.Entidades.Administracion;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Negocio
{
    public class clsNPresentacion : clsNBase
    {

        public List<Maestro> goListarPresentacion(string tsDescripcion = "")
        {
            return loBaseDa.Find<ADMMPRESENTACION>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                   .Select(x => new Maestro
                   {
                       Codigo = x.IdPresentacion.ToString(),
                       CodigoEstado = x.CodigoEstado,
                       Descripcion = x.Nombre,
                       Fecha = x.FechaIngreso,
                       Usuario = x.UsuarioIngreso,
                       Terminal = x.TerminalIngreso,
                       FechaMod = x.FechaModificacion,
                       UsuarioMod = x.UsuarioModificacion,
                       TerminalMod = x.TerminalModificacion,
                   }).ToList();
        }

        public Maestro goBuscarPresentacion(int tsCodigo)
        {
            return loBaseDa.Find<ADMMPRESENTACION>().Where(x => x.IdPresentacion == tsCodigo)
                .Select(x => new Maestro
                {
                    Codigo = x.IdPresentacion.ToString(),
                    CodigoEstado = x.CodigoEstado,
                    Descripcion = x.Nombre,
                    Fecha = x.FechaIngreso,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    Cantidad = x.Cantidad
                }).FirstOrDefault();
        }

        public bool gbGuardarPresentacion(Maestro toObject)
        {
            loBaseDa.CreateContext();
            bool pbResult = false;
            int psCodigo = 0;
            if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = int.Parse(toObject.Codigo);
            var poObject = loBaseDa.Get<ADMMPRESENTACION>().Where(x => x.IdPresentacion == psCodigo).FirstOrDefault();
            if (poObject != null)
            {
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Nombre = toObject.Descripcion;
                poObject.FechaModificacion = toObject.Fecha;
                poObject.UsuarioModificacion = toObject.Usuario;
                poObject.TerminalModificacion = toObject.Terminal;
                poObject.Cantidad = toObject.Cantidad;

                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }
            else
            {
                poObject = new ADMMPRESENTACION();
                loBaseDa.CreateNewObject(out poObject);
                poObject.CodigoEstado = toObject.CodigoEstado;
                poObject.Nombre = toObject.Descripcion;
                poObject.FechaIngreso = toObject.Fecha;
                poObject.UsuarioIngreso = toObject.Usuario;
                poObject.TerminalIngreso = toObject.Terminal;
                poObject.Cantidad = toObject.Cantidad;
                // Insert Auditoría
                loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);
                pbResult = true;
            }

            loBaseDa.SaveChanges();
            return pbResult;
        }

        public void gEliminarPresentacion(int tsCodigo, string tsUsuario, string tsTerminal)
        {
            var poObject = loBaseDa.Get<ADMMPRESENTACION>().Where(x => x.IdPresentacion == tsCodigo).FirstOrDefault();
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
