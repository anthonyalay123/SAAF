using System;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 28/07/2020
    /// Clase del Objeto Acción Perfil
    /// </summary>
    public class MenuAccionPerfil
    {
        public int IdMenu { get; set; }
        public int IdAccion { get; set; }
        public int IdPerfil { get; set; }
        public string NombreAccion { get; set; }
        public string CodigoEstado { get; set; }
        public string Icono { get; set; }
        public string NombreControl { get; set; }
        public int Orden { get; set; }
        public bool MostrarTexto { get; set; }

    }

    public class MenuAccionPerfilBandeja
    {
        public int IdMenu { get; set; }
        public string Menu { get; set; }
        public int IdAccion { get; set; }
        public string NombreAccion { get; set; }
        public int IdPerfil { get; set; }
        public string Perfil { get; set; }
        public string CodigoEstado { get; set; }
        public string Estado { get; set; }
    }


}

