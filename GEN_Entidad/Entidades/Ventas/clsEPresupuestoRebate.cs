using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Ventas
{
    public class PresupuestoRebate
    {
        public int Periodo { get; set; }
        public int Trimestre { get; set; }
        //public int CodeVendedor { get; set; }
        //public string NameVendedor { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public string CodeCliente { get; set; }
        public string NameCliente { get; set; }
        public decimal Meta { get; set; }
        public string Observacion { get; set; }
    }

    public class PresupuestoRebateGC
    {
        public int Periodo { get; set; }
        public int Trimestre { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public int IdGrupoCliente { get; set; }
        public string NombreGrupo { get; set; }
        public decimal Meta { get; set; }
        public string Observacion { get; set; }
    }

    public class PresupuestoRebatePivotGrid
    {
        public int IdPresupuestoRebate { get; set; }
        public int Periodo { get; set; }
        //public int CodeVendedor { get; set; }
        //public string NameVendedor { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public string CodeCliente { get; set; }
        public string NameCliente { get; set; }
        public decimal Trimestre1 { get; set; }
        public decimal Trimestre2 { get; set; }
        public decimal Trimestre3 { get; set; }
        public decimal Trimestre4 { get; set; }
        public decimal Total { get { return Trimestre1 + Trimestre2 + Trimestre3 + Trimestre4; } }
        public string Observacion { get; set; }
        public string Carta { get; set; }
        public string Del { get; set; }
    }

    public class PresupuestoRebatePivotGridGC
    {
        public int IdPresupuestoRebateGC { get; set; }
        public int Periodo { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public int IdGrupoCliente { get; set; }
        public string NombreGrupo { get; set; }
        public decimal Trimestre1 { get; set; }
        public decimal Trimestre2 { get; set; }
        public decimal Trimestre3 { get; set; }
        public decimal Trimestre4 { get; set; }
        public decimal Total { get { return Trimestre1 + Trimestre2 + Trimestre3 + Trimestre4; } }
        public string Observacion { get; set; }
        public string Carta { get; set; }
        public string Del { get; set; }
    }

    public class PresupuestoRebatePivotExcel
    {
        public int Periodo { get; set; }
        //public int CodeVendedor { get; set; }
        //public string NameVendedor { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public string CodeCliente { get; set; }
        public string NameCliente { get; set; }
        public decimal Trimestre1 { get; set; }
        public decimal Trimestre2 { get; set; }
        public decimal Trimestre3 { get; set; }
        public decimal Trimestre4 { get; set; }
        public string Observacion { get; set; }
    }

    public class PresupuestoRebatePivotExcelGC
    {
        public int Periodo { get; set; }
        public string CodeZona { get; set; }
        public string NameZona { get; set; }
        public int IdGrupoCliente { get; set; }
        public string NombreGrupo { get; set; }
        public decimal Trimestre1 { get; set; }
        public decimal Trimestre2 { get; set; }
        public decimal Trimestre3 { get; set; }
        public decimal Trimestre4 { get; set; }
        public string Observacion { get; set; }
    }
}
