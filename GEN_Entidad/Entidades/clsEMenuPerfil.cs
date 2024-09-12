using System;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 19/02/2020
    /// Clase del Objeto Menú Perfíl
    /// </summary>
    public class MenuPerfil
    {
        public int IdMenu { get; set; }
        public int IdPerfil { get; set; }
        public string CodigoEstado { get; set; }
        public string Nombre { get; set; }
        public int IdMenuPadre { get; set; }
        public string NombreForma { get; set; }
        public string NombreFormulario { get; set; }
        public string Icono { get; set; }
        public int Orden { get; set; }
        
    }
}

