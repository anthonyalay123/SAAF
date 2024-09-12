using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Credito
{
    public class PlantillaSeguro
    {
        
        public int IdPlantillaSeguro { get; set; }
        public string CodigoEstado { get; set; }
        public string Pais { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Telefono { get; set; }
        public string PersonaContacto { get; set; }
        public string CorreoElectronico { get; set; }
        public decimal CupoSolicitado { get; set; }
        public decimal ProyeccionVentasAnuales { get; set; }
        public int PlazoCreditoSolicitado { get; set; }
        public Nullable<int> IdProcesoCredito { get; set; }
        public DateTime Fecha { get; set; }

        public string ArchivoAdjunto { get; set; }
        public string RutaOrigen { get; set; }
        public string RutaDestino { get; set; }
        public string NombreOriginal { get; set; }


        public DateTime FechaRespuesta { get; set; }
        public decimal CreditoAprobado { get; set; }
        public int PlazoAprobado { get; set; }
        public string Estado { get; set; }
        public string ObservacionesSeguro { get; set; }
        public string Comentarios { get; set; }
        
    }

    public class PlantillaSeguroExport
    {
        public string Pais { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Telefono { get; set; }
        public string PersonaDeContacto { get; set; }
        public string CorreoElectronico { get; set; }
        public decimal LimiteDeCredito { get; set; }
        public decimal EstimadoVentasAnuales { get; set; }
        public int PlazoDeVentas { get; set; }
        public string Garantias { get; set; }
        public string ValorDeLaGarantia { get; set; }
        public string TiempoQueNegociaConElComprador { get; set; }


    }
}
