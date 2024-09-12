using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 18/08/2021
    /// Clase de Entidad para Sp del mismo nombre REHSPCONSULTAASISTENCIA
    /// </summary>
    public class SpAsistenciaDetalle
    {
        public int IdAsistenciaDetalle { get; set; }
        public int IdAsistencia { get; set; }
        public int IdPersona { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public string Departamento { get; set; }
        public Nullable<System.TimeSpan> HoraLlegada { get; set; }
        public Nullable<System.TimeSpan> HoraSalida { get; set; }
        public Nullable<System.TimeSpan> TiempoAtraso { get; set; }
        public int MinutosAtraso { get; set; }
        public bool Asistencia { get; set; }
        public string Permiso { get; set; }
        public string DesPermiso { get; set; }
        public bool AplicaHE { get; set; }
        public bool AplicaHEAntesEntrada { get; set; }
        public int MinutosExtras { get; set; }
        public bool AplicaTiempoGraciaPostSalida { get; set; }
        public string CodigoDepartamento { get; set; }
        public string Observacion { get; set; }
        public string UsuarioJefatura { get; set; }

        //public SpAsistenciaDetalle Clone()
        //{
        //    return this.MemberwiseClone();
        //}

        public SpAsistenciaDetalle Clone()
        {
            var t = new SpAsistenciaDetalle();
            var type = t.GetType();

            foreach (var prop in type.GetProperties())
            {
                var p = type.GetProperty(prop.Name);
                p.SetValue(t, p.GetValue(this));
            }

            return t;
        }
    }



    public class SpAsistenciaDetalleExport
    {
        public string NumeroIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public string Departamento { get; set; }
        public Nullable<System.TimeSpan> HoraLlegada { get; set; }
        public Nullable<System.TimeSpan> HoraSalida { get; set; }
        public Nullable<System.TimeSpan> TiempoAtraso { get; set; }
        public bool Asistencia { get; set; }
        public string Novedad { get; set; }
        public bool AplicaHE { get; set; }
        public bool AplicaHEAntesEntrada { get; set; }
        public bool AplicaTiempoGraciaPostSalida { get; set; }

    }


    public class SpAsistenciaDetalleBiometrico
    {
        public DateTime Fecha { get; set; }
        public string Empleado { get; set; }
        public TimeSpan HoraLlegada { get; set; }
        public TimeSpan HoraSalida { get; set; }
    }

    public class SpDetalleMarcaciones
    {
        public DateTime Fecha { get; set; }
        public string Empleado { get; set; }
        public TimeSpan Marcacion { get; set; }
    }

    public class AsistenciaDefinitiva
    {
        public string Empleado { get; set; }
        public string Departamento { get; set; }
        public string HoraLlegada { get; set; }
        public string HoraSalida { get; set; }
        public string Atraso { get; set; }
        public string Asistencia { get; set; }
        public string TipoPermiso { get; set; }
        public string AplicaHE { get; set; }
        public int MinutosExtras { get; set; }

    }

    public class SpPermisoPorHoras
    {
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public string Empleado { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string CodigoTipoPermiso { get; set; }
        public string Novedad { get; set; }
        
    }

    public class SpPermisoPorHorasExport
    {
        public string Empleado { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string Novedad { get; set; }

    }

}

