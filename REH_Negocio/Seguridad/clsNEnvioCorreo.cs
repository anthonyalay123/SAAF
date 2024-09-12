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
    public class clsNEnvioCorreo : clsNBase
    {


        public List<EnvioCorreo> goListarMaestro(string tsDescripcion = "")
        {
            return loBaseDa.Find<GENPENVIOCORREO>().Where(x => x.CodigoEstado != Diccionario.Inactivo)
                   .Select(x => new EnvioCorreo
                   {
                       Codigo = x.Codigo,
                       Asunto = x.Asunto,
                       Cuerpo = x.Cuerpo,
                       CodigoEstado = x.CodigoEstado,
                       CCUsuario = x.CCUsuario,
                    

                   }).ToList();
        }

        public EnvioCorreo goBuscarMaestro(string tsCodigo)
        {
            return loBaseDa.Find<GENPENVIOCORREO>().Where(x => x.Codigo.ToString() == tsCodigo && x.CodigoEstado!= Diccionario.Inactivo)
                .Select(x => new EnvioCorreo
                {
                    Codigo = x.Codigo,
                    Asunto = x.Asunto,
                    Cuerpo = x.Cuerpo,
                    CodigoEstado = x.CodigoEstado,
                    CCUsuario = x.CCUsuario,
                    Fecha = x.FechaIngreso,
                    Transaccion = x.Transaccion,
                    Usuario = x.UsuarioIngreso,
                    Terminal = x.TerminalIngreso,
                    FechaMod = x.FechaModificacion,
                    UsuarioMod = x.UsuarioModificacion,
                    TerminalMod = x.TerminalModificacion,
                    CCCorreo = x.CCCorreo
                

                }).FirstOrDefault();
        }

        public string gsGuardar(EnvioCorreo toObject)
        {
            loBaseDa.CreateContext();
            string psResult = "";
            psResult = lValidar(toObject);
            string psCodigo = string.Empty;
            if (psResult=="")
            {
                if (!string.IsNullOrEmpty(toObject.Codigo)) psCodigo = toObject.Codigo;
                var poObject = loBaseDa.Get<GENPENVIOCORREO>().Where(x => x.Codigo.ToString() == psCodigo).FirstOrDefault();
                if (poObject != null)
                {
                   // poObject.Codigo = toObject.Codigo;
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Asunto = toObject.Asunto;
                    poObject.Cuerpo = toObject.Cuerpo;
                    poObject.CCUsuario = toObject.CCUsuario;
                    poObject.Transaccion = toObject.Transaccion;
                    poObject.FechaModificacion = toObject.Fecha;
                    poObject.UsuarioModificacion = toObject.Usuario;
                    poObject.TerminalModificacion = toObject.Terminal;
                    poObject.CCCorreo = toObject.CCCorreo;

                    // Insert Auditoría
                    //   loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, toObject.Fecha, toObject.Usuario, toObject.Terminal);

                }
                else
                {

                    poObject = new GENPENVIOCORREO();
                    loBaseDa.CreateNewObject(out poObject);

                    poObject.Codigo = toObject.Codigo;
                    poObject.CodigoEstado = toObject.CodigoEstado;
                    poObject.Asunto = toObject.Asunto;
                    poObject.Cuerpo = toObject.Cuerpo;
                    poObject.CCUsuario = toObject.CCUsuario;
                    poObject.Transaccion = toObject.Transaccion;
                    poObject.UsuarioIngreso = toObject.Usuario;
                    poObject.FechaIngreso = toObject.Fecha;
                    poObject.TerminalIngreso = toObject.Terminal;
                    poObject.CCCorreo = toObject.CCCorreo;



                    // Insert Auditoría
                    //  loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Insert, toObject.Fecha, toObject.Usuario, toObject.Terminal);

                }

                loBaseDa.SaveChanges();
              
            }

            return psResult;
        }

        private string lValidar(EnvioCorreo toObject)
        {
            string psResult = "";
            if (string.IsNullOrEmpty(toObject.Codigo))
            {
                psResult = psResult + "Falta ingresar el Codigo.\n";
            }
            if (string.IsNullOrEmpty(toObject.Transaccion))
            {
                psResult = psResult + "Falta ingresar la Transaccion.\n";
            }
            if (string.IsNullOrEmpty(toObject.Cuerpo))
            {
                psResult = psResult + "Falta ingresar el el cuerpo del correo.\n";
            }
            if (string.IsNullOrEmpty(toObject.Asunto))
            {
                psResult = psResult + "Falta ingresar el el asunto del correo.\n";
            }
            return psResult;

        }
    }
}
