using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Compras
{

    public class OrdenCompra
    {
        public int IdOrdenCompra { get; set; }
        public string CodigoEstado { get; set; }
        [MaxLength(150)]
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Terminal { get; set; }
        public List<ProveedoresOrdenCompra> Proveedores { get; set; }
    }


    public class ProveedoresOrdenCompra
    {
        public bool Sel { get; set; }
        public int IdOrdenCompra { get; set; }
        public int IdOrdenCompraProveedor { get; set; }

        public int IdProveedor { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        [StringLength(500)]
        public string Observacion { get; set; }
        public bool RequiereAnticipo { get; set; }
        [MaxLength(10)]
        public decimal ValorAnticipo { get; set; }
        public string FormaPago { get; set; }
        public DateTime FechaEntregaEstimada { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public string CodEstado { get; set; }
        public bool CorreoEnviado { get; set; }
        public string EmailEnviado { get { return CorreoEnviado ? "SI" : "NO"; } }
        public string Visualizar { get; set; }
        public string Mail { get; set; }
        public List<ItemsOrdenCompra> Items { get; set; }

     
    }
    public class ItemsOrdenCompra
    {
        //public int idCotizacion { get; set; }
        public int IdOrdenCompraItem { get; set; }
        public string IdentificacionProveedor { get; set; }
        public string Proveedor { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public decimal Valor { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }

    }
    public class BandejaOrdenCompra
    {
        //public int idCotizacion { get; set; }
        public int IdOrdenCompra { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
       
        public string Visualizar { get; set; }


    }

    public class BandejaProveedoresCompraAnticipo
    {
        //public int idCotizacion { get; set; }
        public bool Sel { get; set; }
        public int IdOrdenCompraProveedor { get; set; }
        public string Nombre { get; set; }
        public string Observacion { get; set; }
        public decimal ValorAnticipo { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }

       // public string Visualizar { get; set; }


    }

    public class BandejaRecibirProductos
    {
        //public int idCotizacion { get; set; }
        public bool Sel { get; set; }
        public int No { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        //public string Observacion { get; set; }
        //public DateTime Fecha { get; set; }
        //public string Estado { get; set; }

        // public string Visualizar { get; set; }
    }

    public class BandejaRecibirProductosItems
    {
        //public int idCotizacion { get; set; }
        public int IdOrdenCompra { get; set; }
        public int IdOrdenCompraProveedorItem { get; set; }
        public int IdOrdenCompraProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        //public string Observacion { get; set; }
        public bool RecibiConforme { get; set; }
        public string RecibiConformeObservacion { get; set; }
      //  public DateTime Fecha { get; set; }
        //public string Estado { get; set; }

        // public string Visualizar { get; set; }
    }

    public class ListadoProveedor
    {
        public int idOrdenCompra { get; set; }
        public int IdProveedor { get; set; }

        public string IdentificacionProveedor { get; set; }
        public string Nombre { get; set; }

        public string Observacion { get; set; }
        public decimal Total { get; set; }
        public decimal ValorAnticipo { get; set; }
        public decimal Saldo { get { return Total - ValorAnticipo; } }
        public string Estado { get; set; }
        public string Ver { get; set; }
        public string Imprimir { get; set; }
    }




}
