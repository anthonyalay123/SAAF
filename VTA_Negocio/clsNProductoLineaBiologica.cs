using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTA_Negocio
{
    /// <summary>
    /// Clase de negocio para administrar los productos de linea biológica
    /// </summary>
    public class clsNProductoLineaBiologica : clsNBase
    {

        public List<ProductoLineaBiologica> goListar()
        {
            loBaseDa.CreateContext();
            return loBaseDa.Find<VTAPPRODUCTOLINEABIOLOGICA>().Where(x => x.CodigoEstado == Diccionario.Activo).Select(x => new ProductoLineaBiologica()
            {
                IdProductoLineaBiologica = x.IdProductoLineaBiologica,
                ItmsGrpCod = x.ItmsGrpCod,
            }).ToList();
        }

        public string gsGuardar(List<ProductoLineaBiologica> toLista, string tsUsuario, string tsTerminal)
        {
            
            loBaseDa.CreateContext();

            string psMsg = lsValida(toLista);

            if (string.IsNullOrEmpty(psMsg))
            {
                var poLista = loBaseDa.Get<VTAPPRODUCTOLINEABIOLOGICA>().Where(x => x.CodigoEstado == Diccionario.Activo).ToList();

                var piListaIdBd = poLista.Select(x => x.IdProductoLineaBiologica);
                var piListaIdFrm = toLista.Select(x => x.IdProductoLineaBiologica);

                //Elimino lo que no viene desde el formulario
                foreach (var item in poLista.Where(x=> !piListaIdFrm.Contains(x.IdProductoLineaBiologica)))
                {
                    item.CodigoEstado = Diccionario.Eliminado;
                    item.UsuarioModificacion = tsUsuario;
                    item.TerminalModificacion = tsTerminal;
                    item.FechaModificacion = DateTime.Now;
                }

                var poGrupos = goSapConsultaGrupos();

                //Recorrer la lista que viene del formulario para insertar o actualizar registros
                foreach (var item in toLista)
                {
                    int pId = item.IdProductoLineaBiologica;

                    var poObject = poLista.Where(x => x.IdProductoLineaBiologica == pId && x.IdProductoLineaBiologica != 0).FirstOrDefault();

                    if (poObject != null)
                    {
                        poObject.UsuarioModificacion = tsUsuario;
                        poObject.TerminalModificacion = tsTerminal;
                        poObject.FechaModificacion = DateTime.Now;
                    }
                    else
                    {
                        poObject = new VTAPPRODUCTOLINEABIOLOGICA();
                        loBaseDa.CreateNewObject(out poObject);
                        poObject.UsuarioIngreso = tsUsuario;
                        poObject.TerminalIngreso = tsTerminal;
                        poObject.FechaIngreso = DateTime.Now;
                    }

                    poObject.CodigoEstado = Diccionario.Activo;
                    poObject.ItmsGrpCod = item.ItmsGrpCod;
                    poObject.ItmsGrpNam = poGrupos.Where(x => x.Codigo == item.ItmsGrpCod.ToString()).Select(x => x.Descripcion)?.FirstOrDefault();
                }

                loBaseDa.SaveChanges();
            }

            return psMsg;
        }

        private string lsValida(List<ProductoLineaBiologica> toLista)
        {
            string psMsg = "";
            int fila = 0;
            foreach (var item in toLista)
            {
                fila++;
                if (item.ItmsGrpCod.ToString() == Diccionario.Seleccione)
                {
                    psMsg = string.Format("{0}Debe seleccionar ítem en la fila {1}.\n", psMsg, fila);
                }
            }

            return psMsg;
        }
    }
}
