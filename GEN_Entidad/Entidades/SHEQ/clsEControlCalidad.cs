using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.SHEQ
{
    public class ControlCalidad
    {
        public int IdControlCalidad { get; set; }
        public Nullable<int> IdReferenciaForm { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Presentacion { get; set; }
        public string Empaque { get; set; }
        public string Lote { get; set; }
        public string TipoEnvasado { get; set; }
        public string Trazabilidad { get; set; }
        public DateTime FechaElaboracion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Produccion { get; set; }
        public string ObservacionesProduccion { get; set; }
        public int NumeroPersonalLinea { get; set; }
        public string UsoMascarilla { get; set; }
        public string UsoGuantes { get; set; }
        public string UsoGafas { get; set; }
        public string PisoLimpio { get; set; }
        public string MesaLimpia { get; set; }
        public string SoporteLimpio { get; set; }
        public string UsoLlenadora { get; set; }
        public string UsoSelladora { get; set; }
        public string UsoBalanza { get; set; }
        public string MaterialEmpaque { get; set; }
        public string UsoCaja { get; set; }
        public string UsoEtiqueta { get; set; }
        public string FechaCodEtiqueta { get; set; }
        public string LoteCodEtiqueta { get; set; }
        public string PvpCodEtiqueta { get; set; }
        public string RevisionDensidad { get; set; }
        public string RevisionVolumen { get; set; }
        public string RevisionColor { get; set; }
        public string ObservacionesPreparacionArea { get; set; }
        public string ControlMateriaPrima { get; set; }
        public string ResponsableLinea { get; set; }
        public string Imprimir { get; set; }


        public virtual ICollection<ControlCalidadDetalle> ControlCalidadDetalle { get; set; }
    }

    public class ControlCalidadDetalle
    {
        public int IdControlCalidadDetalle { get; set; }
        public int IdControlCalidad { get; set; }
        public int Muestra { get; set; }
        public DateTime FechaRevision { get; set; }
        public TimeSpan HoraRevision { get; set; }
        public string VolumenEnvasado { get; set; }
        public decimal PesoUnidad { get; set; }
        public decimal PesoCaja { get; set; }
        public string TorqueTapado { get; set; }
        public string Sellado { get; set; }
        public string Calibracion1 { get; set; }
        public string Calibracion2 { get; set; }
        public string Calibracion3 { get; set; }
        public string Etiquetado { get; set; }
        public int CajasPallet { get; set; }
        public string Def { get; set; }
        public string Observaciones { get; set; }
        public string Del { get; set; }
    }

    public class ControlCalidadImport
    {
        public int IdReferenciaForm { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public string Producto { get; set; }
        public string Presentacion { get; set; }
        public string ProductoRelacionado { get; set; }
        public string Empaque { get; set; }
        public string Lote { get; set; }
        public string TipoEnvasado { get; set; }
        public string Trazabilidad { get; set; }
        public DateTime FechaElaboracion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Produccion { get; set; }
        public string ObservacionesProduccion { get; set; }
        public int NumeroPersonalLinea { get; set; }
        public string UsoMascarilla { get; set; }
        public string UsoGuantes { get; set; }
        public string UsoGafas { get; set; }
        public string PisoLimpio { get; set; }
        public string MesaLimpia { get; set; }
        public string SoporteLimpio { get; set; }
        public string UsoLlenadora { get; set; }
        public string UsoSelladora { get; set; }
        public string UsoBalanza { get; set; }
        public string MaterialEmpaque { get; set; }
        public string UsoCaja { get; set; }
        public string UsoEtiqueta { get; set; }
        public string FechaCodEtiqueta { get; set; }
        public string LoteCodEtiqueta { get; set; }
        public string PvpCodEtiqueta { get; set; }
        public string RevisionDensidad { get; set; }
        public string RevisionVolumen { get; set; }
        public string RevisionColor { get; set; }
        public string ObservacionesPreparacionArea { get; set; }
        public string ControlMateriaPrima { get; set; }
        public string ResponsableLinea { get; set; }

        public int Muestra1 { get; set; }
        public DateTime FechaRevision1 { get; set; }
        public TimeSpan HoraRevision1 { get; set; }
        public decimal VolumenEnvasado1 { get; set; }
        public decimal PesoUnidad1 { get; set; }
        public decimal PesoCaja1 { get; set; }
        public string TorqueTapado1 { get; set; }
        public string Sellado1 { get; set; }
        public string Calibracion11 { get; set; }
        public string Calibracion21 { get; set; }
        public string Calibracion31 { get; set; }
        public string Etiquetado1 { get; set; }
        public int CajasPallet1 { get; set; }
        public string Observaciones1 { get; set; }

        public int Muestra2 { get; set; }
        public DateTime FechaRevision2 { get; set; }
        public TimeSpan HoraRevision2 { get; set; }
        public decimal VolumenEnvasado2 { get; set; }
        public decimal PesoUnidad2 { get; set; }
        public decimal PesoCaja2 { get; set; }
        public string TorqueTapado2 { get; set; }
        public string Sellado2 { get; set; }
        public string Calibracion12 { get; set; }
        public string Calibracion22 { get; set; }
        public string Calibracion32 { get; set; }
        public string Etiquetado2 { get; set; }
        public int CajasPallet2 { get; set; }
        public string Observaciones2 { get; set; }

        public int Muestra3 { get; set; }
        public DateTime FechaRevision3 { get; set; }
        public TimeSpan HoraRevision3 { get; set; }
        public decimal VolumenEnvasado3 { get; set; }
        public decimal PesoUnidad3 { get; set; }
        public decimal PesoCaja3 { get; set; }
        public string TorqueTapado3 { get; set; }
        public string Sellado3 { get; set; }
        public string Calibracion13 { get; set; }
        public string Calibracion23 { get; set; }
        public string Calibracion33 { get; set; }
        public string Etiquetado3 { get; set; }
        public int CajasPallet3 { get; set; }
        public string Observaciones3 { get; set; }

        public int Muestra4 { get; set; }
        public DateTime FechaRevision4 { get; set; }
        public TimeSpan HoraRevision4 { get; set; }
        public decimal VolumenEnvasado4 { get; set; }
        public decimal PesoUnidad4 { get; set; }
        public decimal PesoCaja4 { get; set; }
        public string TorqueTapado4 { get; set; }
        public string Sellado4 { get; set; }
        public string Calibracion14 { get; set; }
        public string Calibracion24 { get; set; }
        public string Calibracion34 { get; set; }
        public string Etiquetado4 { get; set; }
        public int CajasPallet4 { get; set; }
        public string Observaciones4 { get; set; }

        public int Muestra5 { get; set; }
        public DateTime FechaRevision5 { get; set; }
        public TimeSpan HoraRevision5 { get; set; }
        public decimal VolumenEnvasado5 { get; set; }
        public decimal PesoUnidad5 { get; set; }
        public decimal PesoCaja5 { get; set; }
        public string TorqueTapado5 { get; set; }
        public string Sellado5 { get; set; }
        public string Calibracion15 { get; set; }
        public string Calibracion25 { get; set; }
        public string Calibracion35 { get; set; }
        public string Etiquetado5 { get; set; }
        public int CajasPallet5 { get; set; }
        public string Observaciones5 { get; set; }

        public string Del { get; set; }

    }
}
