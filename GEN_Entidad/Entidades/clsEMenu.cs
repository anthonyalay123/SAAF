using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
    {/// <summary>
     /// Autor: Guillermo Murillo
     /// Fecha: 02/7/2021
     /// Clase del Objeto Menu
     /// </summary>
    public class Menu : Maestro
    {   
        public int? IdMenuPadre { get; set; }
        public string NombreForma { get; set; }
        public string NombreFormulario { get; set; }
        public int Orden { get; set; }
        public int IdGestorConsulta { get; set; }
    }

    public class MenuTodo
    {
        public int IdMenu { get; set; }
        public string Nombre { get; set; }
        public int? IdAccion { get; set; }
        public int? IdMenuPadre { get; set; }
        //public bool EsAccion { get; set; }
        //public string NombreForma { get; set; }
        //public bool Checked { get; set; }
    }

    public class RibbonFavorito
    {
        public int IdRibbonFavoritos { get; set; }
        public string CodigoUsuario { get; set; }
        public string Nombre { get; set; }
        public string NombreControl { get; set; }
        public string NombreForma { get; set; }
        public string Icono { get; set; }
    }

}
