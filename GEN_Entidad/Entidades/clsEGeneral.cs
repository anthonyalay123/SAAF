using System;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 07/12/2020
    /// Clase para crear objetos generales
    /// </summary>
    public class DictionaryDto
    {
        public int Key { get; set; }
        public string Value { get; set; }        
    }

    public class GrupoRubroIndex
    {
        public int Index { get; set; }
        public string CodigoRubro { get; set; }
        public string CodigoRubroReferencial { get; set; }
    }

    public class GridBusqueda
    {
        public int Ancho { get; set; }
        public string Columna { get; set; }
    }
}

