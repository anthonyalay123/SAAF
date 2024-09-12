using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    public class EmpleadoDocumento
    {
        public int IdEmpleadoDocumento { get; set; }
        public string CodigoEstado { get; set; }
        public int IdPersona { get; set; }
        public string RutaDestino { get; set; }
        public string RutaOrigen { get; set; }
        public string NombreArchivo { get; set; }
        public string Descripcion { get; set; }
        public string ArchivoAdjunto { get; set; }
        
        public string Cargar { get; set; }
        public string Add { get; set; }
        public string Visualizar { get; set; }
        public string Descargar { get; set; }
        public string Del { get; set; }

        public Empleado Empleado { get; set; }
    }

    public class EmpleadoDocumentoGrid
    {
        public int IdEmpleadoDocumento { get; set; }
        public string NombreArchivo { get; set; }
        public string NombreArchivoSistema { get; set; }
        public string ArchivoAdjunto { get; set; }
        public string Descripcion { get; set; }
        public string RutaOrigen { get; set; }
        public string RutaDestino { get; set; }
        public string Add { get; set; }
        public string Visualizar { get; set; }
        public string Descargar { get; set; }
        public string Del { get; set; }
    }
}
