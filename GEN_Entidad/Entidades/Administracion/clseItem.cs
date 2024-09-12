using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Administracion
{
    public class Item
    {
        public string Codigo { get; set; }
        public string CodigoEstado { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string Terminal { get; set; }
        public string Usuario { get; set; }
        public DateTime? FechaMod { get; set; }
        public string TerminalMod { get; set; }
        public string UsuarioMod { get; set; }

        public string Tipo { get; set; }

        public string Subtipo { get; set; }
        public int Cantidad { get; set; }

        public int? IdPresentacion { get; set; }

        public decimal Costo { get; set; }
        public bool GrabaIva { get; set; }

    }
}
