using GEN_Entidad;
using GEN_Entidad.Entidades.Compras;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM_Negocio
{
    public class clsNLogistica : clsNBase
    {
        public List<Logistica> goListar()
        {
            return loBaseDa.Find<COMTLOGISTICA>().Select(x => new Logistica()
            {
                Bultos = x.Bultos,
                CodigoConcepto = x.CodigoConcepto,
                CodigoEstado = x.CodigoEstado,
                DiasCredito = x.DiasCredito,
                Factura = x.Factura,
                FechaVencimiento = x.FechaVencimiento,
                FechaViaje = x.FechaViaje,
                IdLogistica  = x.IdLogistica,
                IdProveedor = x.IdProveedor,
                Observacion = x.Observacion,
                //Total = x.Total,
                Valor = x.Valor
            }).ToList();
        }

        public string gsGuardar(List<Logistica> toLista, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";
            loBaseDa.CreateContext();

            int fila = 1;
            foreach (var item in toLista)
            {

                if (item.IdProveedor == 0)
                {
                    psMsg = psMsg + "Falta seleccionar proveedor en la fila:" + fila + "\n";
                }
                if (string.IsNullOrEmpty(item.Factura))
                {
                    psMsg = psMsg + "Falta ingresar No. Factura en la fila:" + fila + "\n";
                }
                if (item.FechaViaje == DateTime.MinValue)
                {
                    psMsg = psMsg + "Falta ingresar fecha de Viaje en la fila: " + fila + "\n";
                }
                if (item.CodigoConcepto == Diccionario.Seleccione)
                {
                    psMsg = psMsg + "Falta seleccionar concepto en la fila:" + fila + "\n";
                }
                if (item.Valor <= 0)
                {
                    psMsg = psMsg + "Falta ingresar valor de Factura en la fila " + fila + "\n";
                }
                if (item.Bultos <= 0)
                {
                    psMsg = psMsg + "Falta ingresar bultos en la fila " + fila + "\n";
                }
                if (item.FechaVencimiento == DateTime.MinValue)
                {
                    psMsg = psMsg + "Falta ingresar fecha de vencimiento en la fila: " + fila + "\n";
                }
                if (item.DiasCredito <= 0)
                {
                    psMsg = psMsg + "Falta ingresar días de crédito en la fila " + fila + "\n";
                }

                fila++;
            }

            if (string.IsNullOrEmpty(psMsg))
            {
                List<int> poListaIdPe = toLista.Select(x => x.IdLogistica).ToList();
                List<int> piListaEliminar = loBaseDa.Find<COMTLOGISTICA>().Where(x => !poListaIdPe.Contains(x.IdLogistica)).Select(x => x.IdLogistica).ToList();
                //Recorrer la base de dato modificando el codigo estado a eliminado
                foreach (var poItem in loBaseDa.Get<COMTLOGISTICA>().Where(x => piListaEliminar.Contains(x.IdLogistica)))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }

                if (toLista != null)
                {
                    foreach (var item in toLista)
                    {

                        var poObjectItem = loBaseDa.Get<COMTLOGISTICA>().Where(x => x.IdLogistica == item.IdLogistica && item.IdLogistica != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.TerminalModificacion = tsTerminal;
                        }
                        else
                        {
                            poObjectItem = new COMTLOGISTICA();
                            loBaseDa.CreateNewObject(out poObjectItem);
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.TerminalIngreso = tsTerminal;
                        }

                        poObjectItem.CodigoEstado = item.CodigoEstado;
                        poObjectItem.Bultos = item.Bultos;
                        poObjectItem.CodigoConcepto = item.CodigoConcepto;
                        poObjectItem.DiasCredito = item.DiasCredito;
                        poObjectItem.Factura = item.Factura;
                        poObjectItem.FechaViaje = item.FechaViaje;
                        poObjectItem.IdProveedor = item.IdProveedor;
                        poObjectItem.Observacion = item.Observacion;
                        poObjectItem.Valor = item.Valor;
                        poObjectItem.Total = item.Total;
                        poObjectItem.FechaVencimiento = item.FechaVencimiento;
                    }
                }

                loBaseDa.SaveChanges();
            }

           

            return psMsg;
        }
    }
}
