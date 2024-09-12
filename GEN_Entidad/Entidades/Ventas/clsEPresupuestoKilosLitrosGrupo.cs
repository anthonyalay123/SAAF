using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Ventas
{

    public class PresupuestoKilosLitrosGrupo : PresupuestoKilosLitrosBase
    {
        public int IdPresupuestoKilosLitrosGrupo { get; set; }
        public short GroupCode { get; set; }
        public string GroupName { get; set; }
    }

    public class PresupuestoKilosLitrosGrupoGrid
    {
        public int IdPresupuestoKilosLitrosGrupo { get; set; }
        public int Periodo { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public short GroupCode { get; set; }
        public string GroupName { get; set; }
        public decimal Ene { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Abr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Ago { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dic { get; set; }
        public decimal Total { get { return Ene + Feb + Mar + Abr + May + Jun + Jul + Ago + Sep + Oct + Nov + Dic; } }
        public string Observacion { get; set; }
        public string Del { get; set; }
    }

    public class PresupuestoKilosLitrosGrupoExcel
    {
        public int Periodo { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public short GroupCode { get; set; }
        public string GroupName { get; set; }
        public decimal Ene { get; set; }
        public decimal Feb { get; set; }
        public decimal Mar { get; set; }
        public decimal Abr { get; set; }
        public decimal May { get; set; }
        public decimal Jun { get; set; }
        public decimal Jul { get; set; }
        public decimal Ago { get; set; }
        public decimal Sep { get; set; }
        public decimal Oct { get; set; }
        public decimal Nov { get; set; }
        public decimal Dic { get; set; }
        public string Observacion { get; set; }
    }

    //public class PresupuestoKilosLitrosGrupoGrid : PresupuestoKilosLitrosGridBase
    //{
    //    public int IdPresupuestoKilosLitrosGrupo { get; set; }
    //    public short GroupCode { get; set; }
    //    public string GroupName { get; set; }
    //}

    //public class PresupuestoKilosLitrosGrupoExcel : PresupuestoKilosLitrosExcelBase
    //{
    //    public short GroupCode { get; set; }
    //    public string GroupName { get; set; }
    //}


}
