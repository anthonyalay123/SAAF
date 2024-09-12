
using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COM_Negocio
{
    public class clsNBandejaSolicitudCompra : clsNBase
    {

        //public List<BandejaSolicitudCompra> goListarBandeja()
        //{
        //    return (from SC in loBaseDa.Find<COMTSOLICITUDCOMPRA>()
        //                // Inner Join con la tabla GENMESTADO
        //            join E in loBaseDa.Find<GENMESTADO>()
        //            on new { SC.CodigoEstado } equals new { E.CodigoEstado }
        //            // Inner Join con la tabla GENMCATALOGO - Estado Solicitud de Compra
        //            join CES in loBaseDa.Find<GENMCATALOGO>()
        //            on new { cg = Diccionario.ListaCatalogo.EstadoSolicitudCompra, c = SC.CodigoEstadoSolicitud }
        //            equals new { cg = CES.CodigoGrupo, c = CES.Codigo }
        //            // Inner Join con la tabla GENMCATALOGO - Tipo Items
        //            join CTI in loBaseDa.Find<GENMCATALOGO>()
        //            on new { cg = Diccionario.ListaCatalogo.TipoItems, c = SC.CodigoTipoItem }
        //            equals new { cg = CTI.CodigoGrupo, c = CTI.Codigo }
        //            // Condición de la consulta WHERE
        //            where SC.CodigoEstado != Diccionario.Eliminado && SC.CodigoEstadoSolicitud == Diccionario.ListaCatalogo.TipoEstadoSolicitud.Pendiente
        //            // Selección de las columnas / Datos
        //            select new BandejaSolicitudCompra
        //            {
        //                Id = SC.IdSolicitudCompra,
        //                Estado = CES.Descripcion,
        //                Observacion = SC.Observacion,
        //                FechaEntrega = SC.FechaEntrega,
        //                Fecha = SC.FechaIngreso,

        //            }).ToList();
        //}

        //public string gsGuardar(int idSolicitud, string codigo, string usuario, string terminal)
        //{
        //    loBaseDa.CreateContext();
        //    string psResult = string.Empty;
           
        //    var poObject = loBaseDa.Get<COMTSOLICITUDCOMPRA>().Where(x => x.CodigoEstado != Diccionario.Eliminado && x.IdSolicitudCompra == idSolicitud).FirstOrDefault();
        //        poObject.CodigoEstado = codigo;
        //        poObject.UsuarioModificacion = usuario;
        //        poObject.TerminalModificacion = terminal;
        //        loBaseDa.Auditoria(poObject, Diccionario.Auditoria.TipoAccion.Update, DateTime.Now, usuario, terminal);
        //        loBaseDa.SaveChanges();
        //    return psResult;
        //}

        
    }
}
