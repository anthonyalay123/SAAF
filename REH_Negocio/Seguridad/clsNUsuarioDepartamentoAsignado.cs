using GEN_Entidad;
using GEN_Entidad.Entidades;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REH_Negocio.Seguridad
{
    public class clsNUsuarioDepartamentoAsignado : clsNBase
    {
        public List<Combo> goConsultarComboMenu()
        {
            return loBaseDa.Find<GENPMENU>().Where(x => x.CodigoEstado.ToString() == Diccionario.Activo && x.IdMenuPadre != null)
                   .Select(x => new Combo
                   {
                       Codigo = x.IdMenu.ToString(),
                       Descripcion = x.Nombre.ToString(),
                   }).OrderBy(x => x.Codigo).ToList();
        }

        public List<Combo> goConsultarComboUsuario()
        {
            return loBaseDa.Find<SEGMUSUARIO>().Where(x => x.CodigoEstado == Diccionario.Activo)
                 .Select(x => new Combo
                 {
                     Codigo = x.CodigoUsuario.ToString(),
                     Descripcion = x.NombreCompleto.ToString(),
                 }).OrderBy(x => x.Descripcion).ToList();
        }

        //public List<Combo> goConsultarComboDepartamento()
        //{
        //    return loBaseDa.Find<GENMCATALOGO>().Where(x => x.CodigoEstado == Diccionario.Activo)
        //         .Select(x => new Combo
        //         {
        //             Codigo = x.CodigoDepartamento.ToString(),
        //             Descripcion = x.Descripcion.ToString(),
        //         }).OrderBy(x => x.Descripcion).ToList();
        //}


        /// <summary>
        /// Guardar Objeto Maestro
        /// </summary>
        /// <param name="toObject"></param>
        /// <returns></returns>
        public string gsGuardar(UsuarioDepartamentoAsignado toObject, string usuario)
        {
            loBaseDa.CreateContext();
            string psResult = string.Empty;
            psResult = lsEsValido(toObject);
            string psCodigo = string.Empty;
            if (string.IsNullOrEmpty(psResult))
            {
               
                var poObject = loBaseDa.Get<SEGPUSUARIODEPARTAMENTOASIGNADO>().Where(x => x.IdUsuarioDepartamentoAsignado == toObject.idUsuarioDepartamentoAsignado).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.IdMenu = toObject.idMenu;
                    poObject.CodigoUsuario = toObject.CodigoUsuario;
                    poObject.CodigoDepartamento = toObject.CodigoDepartamento;
                    poObject.CodigoEstado = toObject.codigoEstado;
                    poObject.FechaModificacion = DateTime.Now;
                    poObject.UsuarioModificacion = usuario;
                    poObject.TerminalModificacion = "";
                }
                else
                {
                    poObject = new SEGPUSUARIODEPARTAMENTOASIGNADO();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.IdMenu = toObject.idMenu;

                    poObject.CodigoUsuario = toObject.CodigoUsuario;
                   
                    poObject.CodigoDepartamento = toObject.CodigoDepartamento;
                    poObject.CodigoEstado = toObject.codigoEstado;

                    poObject.UsuarioIngreso = usuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = "";
                }

                loBaseDa.SaveChanges();

            }
            return psResult;
        }


        public string lsEsValido(UsuarioDepartamentoAsignado toObject)
        {
            string msg = null;
            if (toObject.idMenu <=0)
            {
                msg = msg + "Ingrese el Menu";
            }
            if (toObject.CodigoUsuario == Diccionario.Seleccione || toObject.CodigoUsuario == null)
            {
                msg = msg + "Ingrese el Usuario Asignado";
            }
            if (toObject.CodigoDepartamento == Diccionario.Seleccione || toObject.CodigoDepartamento == null)
            {
                msg = msg + "Ingrese el Codigo Departamento";
            }
            return msg;
        }

        public List<UsuarioDepartamentoAsignado> goListarMaestro()
        {


            return (from SC in loBaseDa.Find<SEGPUSUARIODEPARTAMENTOASIGNADO>()
                        // Inner Join con la tabla GENMESTADO
                    join E in loBaseDa.Find<GENMESTADO>()
                    on new { SC.CodigoEstado }
                    equals new { E.CodigoEstado }


                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                    join CUSUARIO in loBaseDa.Find<SEGMUSUARIO>()
                    on new { cg = SC.CodigoUsuario }
                    equals new { cg = CUSUARIO.CodigoUsuario }

                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                    join D in loBaseDa.Find<GENMCATALOGO>()
                    on new { cg = SC.CodigoDepartamento, cd = Diccionario.ListaCatalogo.Departamento }
                    equals new { cg = D.Codigo, cd = D.CodigoGrupo }

                    // Inner Join con la tabla SEGMUSUARIO - Nombre Completo
                    join m in loBaseDa.Find<GENPMENU>()
                    on new { cg = SC.IdMenu }
                    equals new { cg = m.IdMenu }


                    // Condición de la consulta WHERE
                    where SC.CodigoEstado != Diccionario.Inactivo
                    // Selección de las columnas / Datos
                    select new UsuarioDepartamentoAsignado
                    {
                        CodigoUsuario = SC.CodigoUsuario,
                        CodigoDepartamento = D.Descripcion,
                        Descripcion = m.Nombre,
                        codigoEstado = SC.CodigoEstado,
                        Codigo = SC.IdUsuarioDepartamentoAsignado.ToString()

                    }).ToList();
        }

        public UsuarioDepartamentoAsignado goBuscarMaestro(int tsCodigo)
        {
            return loBaseDa.Find<SEGPUSUARIODEPARTAMENTOASIGNADO>().Where(x => x.CodigoEstado != Diccionario.Inactivo && x.IdUsuarioDepartamentoAsignado == tsCodigo)
                   .Select(x => new UsuarioDepartamentoAsignado
                   {
                       idUsuarioDepartamentoAsignado = x.IdUsuarioDepartamentoAsignado,
                       CodigoUsuario = x.CodigoUsuario,
                       CodigoDepartamento = x.CodigoDepartamento,
                       idMenu = x.IdMenu,
                       CodigoEstado = x.CodigoEstado,
                      
                       Usuario = x.UsuarioIngreso,
                       UsuarioMod = x.UsuarioModificacion,
                       Terminal = x.TerminalIngreso,
                       TerminalMod = x.TerminalModificacion,
                       Fecha = x.FechaIngreso,
                       FechaMod = x.FechaModificacion,

                   }).FirstOrDefault();
        }

    }

   
}
