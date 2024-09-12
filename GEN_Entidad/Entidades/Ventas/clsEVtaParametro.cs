using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Ventas
{
    public class VtaParametro
    {
        public int DiasMaxMoraRebate { get; set; }
        public decimal PorcMinRentabilidadRebate { get; set; }
        public decimal PorcMinCumplimientoRebate { get; set; }
        public decimal PorcBonificacionCumplimientoRebate { get; set; }
        public decimal PorcMinRentabilidadRebateAnual { get; set; }
        public decimal PorcMinCumplimientoRebateAnual { get; set; }
        public decimal PorcBonificacionCumplimientoRebateAnual { get; set; }
        public string TextoEmailRebateTrimestral { get; set; }
        public string TextoEmailRebateAnual { get; set; }
        public string BodegasExluirStockProductoBatch { get; set; }
        

    }
}
