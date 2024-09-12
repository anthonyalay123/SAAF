using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;

namespace REH_Negocio
{
    /// <summary>
    /// Clase General de Negocio
    /// Victor Arévalo
    /// 06/02/2020
    /// </summary>
    public class clsNGeneral 
    {
        #region Variables
        private readonly clsDBase loBaseDa = new clsDBase();
        #endregion

        /// <summary>
        /// Consulta tipo de rol, para llenar combo
        /// </summary>
        /// <returns></returns>
        public List<Combo> goConsultarComboTipoRol()
        {
            return loBaseDa.Find<REHMTIPOROL>().Where(x => x.CodigoEstado != Diccionario.Eliminado)
                  .Select(x => new Combo
                  {
                      Codigo = x.CodigoTipoRol,
                      Descripcion = x.Descripcion
                  }).OrderBy(x=>x.Descripcion).ToList();
        }
    }
}