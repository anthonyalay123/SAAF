using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Ventas
{

    public class PresupuestoVentas
    {
        public int IdPresupuestoVentas { get; set; }
        public string CodigoEstado { get; set; }
        public int Periodo { get; set; }
        public int Mes { get; set; }
        public int IdZona { get; set; }
        public string Zona { get; set; }
        public string Familia { get; set; }
        public short ItmsGrpCod { get; set; }
        public string ItmsGrpNam { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Unidades { get; set; }
        public decimal PrecioReferencial { get; set; }
        public decimal Valor { get; set; }
        public decimal MedidaConversion { get; set; }
        public decimal Total { get; set; }
        public string TipoProducto { get; set; }
        public string Observacion { get; set; }
    }

    public class PresupuestoVentasGrid
    {
        public bool Sel { get; set; }
        public int IdPresupuestoVentas { get; set; }
        public int Periodo { get; set; }
        public int Mes { get; set; }
        public int IdZona { get; set; }
        public string Zona { get; set; }
        public string Familia { get; set; }
        public short ItmsGrpCod { get; set; }
        public string Grupo { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Unidades { get; set; }
        public decimal PrecioReferencial { get; set; }
        public decimal Valor { get { return Math.Round(Unidades * PrecioReferencial, 2); } }
        public decimal MedidaConversion { get; set; }
        public decimal Total { get { return Math.Round(Unidades* MedidaConversion, 2); } }
        public string TipoProducto { get; set; }
        public string Observacion { get; set; }
        public string Del { get; set; }
    }

    public class PresupuestoVentasExcel
    {
        public int Periodo { get; set; }
        public int Mes { get; set; }
        //public int IdZona { get; set; }
        public string Zona { get; set; }
        //public string Familia { get; set; }
        //public short ItmsGrpCod { get; set; }
        //public string Grupo { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Unidades { get; set; }
        public decimal PrecioReferencial { get; set; }
        public decimal Valor { get { return Math.Round(Unidades * PrecioReferencial, 2); } }
        public decimal MedidaConversion { get; set; }
        public decimal Total { get { return Math.Round(Unidades * MedidaConversion, 2); } }
        public string TipoProducto { get; set; }
        //public string Observacion { get; set; }
    }



    public class PresupuestoKilosLitros
    {
        public int IdPresupuestoKilosLitros { get; set; }
        public string CodigoEstado { get; set; }
        public int Periodo { get; set; }
        public int Mes { get; set; }
        public int IdZona { get; set; }
        public string Zona { get; set; }
        public string Familia { get; set; }
        public short ItmsGrpCod { get; set; }
        public string ItmsGrpNam { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Unidades { get; set; }
        public decimal MedidaConversion { get; set; }
        public decimal Total { get; set; }
        public string Observacion { get; set; }
    }

    public class PresupuestoKilosLitrosGrid
    {
        public bool Sel { get; set; }
        public int IdPresupuestoKilosLitros { get; set; }
        public int Periodo { get; set; }
        public int Mes { get; set; }
        public int IdZona { get; set; }
        public string Zona { get; set; }
        public string Familia { get; set; }
        public short ItmsGrpCod { get; set; }
        public string Grupo { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Unidades { get; set; }
        public decimal MedidaConversion { get; set; }
        public decimal Total { get { return Math.Round(Unidades * MedidaConversion, 2); } }
        public string Observacion { get; set; }
        public string Del { get; set; }
    }

    public class PresupuestoKilosLitrosExcel
    {
        public int Periodo { get; set; }
        public int Mes { get; set; }
        public string Zona { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Unidades { get; set; }
        public decimal MedidaConversion { get; set; }
        public decimal Total { get { return Math.Round(Unidades * MedidaConversion, 2); } }
    }
}
