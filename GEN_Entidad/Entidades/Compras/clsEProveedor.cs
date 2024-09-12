using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN_Entidad.Entidades.Compras
{
    public class PaProveedor : Maestro
    {
        public string Identificacion { get; set; }
        public string Correo { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string CodigoTipoIdentificacion { get; set; }
        public string CardCode { get; set; }
        
        public ICollection<ProveedorCuentaBancaria> ProveedorCuentaBancaria { get; set; }
    }

    public class ProveedorCuentaBancaria
    {
        public int IdProveedorCuenta { get; set; }
        public int IdProveedor { get; set; }
        public bool Principal { get; set; }
        public string CodigoTipoIdentificacion { get; set; }
        public string DesTipoIdentificacion { get; set; }
        [MaxLength(20)]
        public string Identificacion { get; set; }
        [MaxLength(40)]
        public string Nombre { get; set; }
        public string CodigoBanco { get; set; }
        public string DesBanco { get; set; }
        public string CodigoFormaPago { get; set; }
        public string DesFormaPago { get; set; }
        public string CodigoTipoCuentaBancaria { get; set; }
        public string DesTipoCuentaBancaria { get; set; }
        [MaxLength(20)]
        public string NumeroCuenta { get; set; }
        public string Del { get; set; }
        
    }
}
