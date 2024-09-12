using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 15/03/2021
    /// Clase para la carga de empleados externos para el cálculo de utilidades
    /// </summary>
    public class PlantillaUtilidadEmpExterno
    {
        public int Id { get; set; }
        public string RucEmpresa { get; set; }
        public string Empresa { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Genero { get; set; }
        public int Dias { get; set; }
        public int CargaConyuge { get; set; }
        public int Discapacitados { get; set; }
        public int CargaHijos { get; set; }
        public Nullable<System.DateTime> FechaInicioContrato { get; set; }
        public Nullable<System.DateTime> FechaFinContrato { get; set; }
        public string Ubicacion { get; set; }
        public string Cargo { get; set; }
        public string CodigoIess { get; set; }
        public string Estado { get; set; }
        

    }

    public class PlantillaUtilidadEmpExternoExcel
    {
        public string RucEmpresa { get; set; }
        public string Empresa { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Genero { get; set; }
        public int Dias { get; set; }
        public int CargaConyuge { get; set; }
        public int Discapacitados { get; set; }
        public int CargaHijos { get; set; }
        public Nullable<System.DateTime> FechaInicioContrato { get; set; }
        public Nullable<System.DateTime> FechaFinContrato { get; set; }
        public string Ubicacion { get; set; }
        public string Cargo { get; set; }
        public string CodigoIess { get; set; }
    }
}
