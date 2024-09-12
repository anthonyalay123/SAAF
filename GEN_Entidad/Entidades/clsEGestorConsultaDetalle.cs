using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades
{
    public class GestorConsultaDetalle
    {
        public int IdDetalle { get; set; }
        public string CodigoEstado { get; set; }
        public int Id { get; set; }
        public int Orden { get; set; }
        public string Nombre { get; set; }
        public string TipoDato { get; set; }
        public string Formato { get; set; }
        public string PlaceHolder { get; set; }
        public int Longitud { get; set; }
    }

    public class GestorConsulta
    {
        public int Id { get; set; }
        public string CodigoEstado { get; set; }
        public string Nombre { get; set; }
        public string Query { get; set; }
        public string Observacion { get; set; }
        public bool BotonImprimir { get; set; }
        public string TituloReporte { get; set; }
        public string DataSet { get; set; }
    }
}
