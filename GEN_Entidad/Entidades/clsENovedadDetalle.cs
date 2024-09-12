using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 27/05/2020
    /// Clase del Objeto del nombre de la clase
    /// </summary>
    public class NovedadDetalle
    {
        public int IdNovedadDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int IdNovedad { get; set; }
        public int IdPersona { get; set; }
        public string CodigoRubro { get; set; }
        public decimal Valor { get; set; }
        public int Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Terminal { get; set; }
        public string Observaciones { get; set; }
    }

    public class ReporteNovedades
    {
        public ReporteNovedades()
        {
            this.ReporteNovedadesDetalle = new List<ReporteNovedadesDetalle>();
        }

        public int IdReporteNovedades { get; set; }
        public string CodigoEstado { get; set; }
        public string Descripcion { get; set; }
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public int Orden { get; set; }

        public virtual ICollection<ReporteNovedadesDetalle> ReporteNovedadesDetalle { get; set; }
    }

    public class ReporteNovedadesDetalle
    {
        public int IdReporteNovedadesDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int IdReporteNovedades { get; set; }
        public string CodigoRubro { get; set; }
        public string Rubro { get; set; }
    }
}
