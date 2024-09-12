using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    public class Empleado
    {
        public int IdPersona { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoTipoEmpleado { get; set; }
        public string Correo { get; set; }
        public string CodigoTipoVivienda { get; set; }
        public string CodigoTipoMaterialVivienda { get; set; }
        public Nullable<decimal> ValorArriendo { get; set; }
        public string NumeroSeguroSocial { get; set; }
        public string NumeroLibretaMilitar { get; set; }
        public string CodigoSectorial { get; set; }
        public string CodigoRegionIess { get; set; }
        public string CuentaContable { get; set; }
        public bool AporteIessConyuge { get; set; }
        public bool AplicaHorasExtrasAntesLlegada { get; set; }
        public bool AplicaTiempoGraciaPostSalida { get; set; }
        public bool MostrarEnAsistencia { get; set; }
        public bool CalculaIRComisiones { get; set; }
        public int MesDecimoCuarto { get { return CodigoRegionIess == "0008" ? 8 : 3;   } }
        public string CaracteristicasVivienda { get; set; }

        public Persona Persona { get; set; }
        public ICollection<EmpleadoCargaFamiliar> EmpleadoCargaFamiliar { get; set; }
        public virtual ICollection<EmpleadoContacto> EmpleadoContacto { get; set; }
        public virtual ICollection<EmpleadoContrato> EmpleadoContrato { get; set; }
        public virtual ICollection<EmpleadoDocumento> EmpleadoDocumento { get; set; }

    }
}
