using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 06/02/2020
    /// Clase del Objeto Combo
    /// </summary>
    public class Combo
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    }

    public class Dictionary
    {
        public int Id { get; set; }
        public int Orden { get; set; }
    }

    public class RubroManual
    {
        public int Id { get; set; }
        public string Rubro { get; set; }
        [MaxLength(100)]
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }
        public string Del { get; set; }
    }

    public class RolManual
    {
        public int Id { get; set; }
        public int IdPeriodo { get; set; }
        public int Mes { get; set; }
        public decimal Valor { get; set; }
        public string Observacion { get; set; }
        public string Del { get; set; }
    }
}
