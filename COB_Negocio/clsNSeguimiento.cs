using GEN_Entidad;
using GEN_Entidad.Entidades.Cobranza;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COB_Negocio
{
    public class clsNSeguimiento : clsNBase
    {
        public List<SeguimientoCompromiso> goConsultar(List<string> tsLista)
        {
            return loBaseDa.Find<COBTSEGUIMIENTOCOMPROMISO>().Where(x => x.CodigoEstado == Diccionario.Activo && tsLista.Contains(x.CodZona)).Select(x => new SeguimientoCompromiso()
            {
                IdSeguimientoCompromiso = x.IdSeguimientoCompromiso,
                CodigoEstado = x.CodigoEstado,
                CodCliente = x.CodCliente,
                Cliente = x.Cliente,
                CodZona = x.CodZona,
                Zona = x.Zona,
                FechaPedido = x.FechaPedido,
                NumCompromiso = x.NumCompromiso,
                ValorPedido = x.ValorPedido,
                Factura = x.Factura,
                FechaEmision = x.FechaEmision,
                FechaVencimiento = x.FechaVencimiento,
                FechaCompromiso = x.FechaCompromiso,
                DiasMora = x.DiasMora,
                Compromiso = x.Compromiso,
                CompromisoCumplido = x.CompromisoCumplido,
                Observaciones = x.Observaciones,
                CodigoMotivo = x.CodigoMotivo,
                Saldo = x.Saldo,
                DocNum = x.DocNum,
                DocEntry = x.DocEntry,
                FechaGestion = x.FechaIngreso,
            }).ToList();
        }


        public List<SeguimientoCompromiso> goConsultaPreliminar(DateTime tdFechaCorte, string tsZona)
        {
            return loBaseDa.ExecStoreProcedure<SeguimientoCompromiso>(string.Format("EXEC COBSPCONSULTACARTERAZONA '{0}','{1}','1'", tdFechaCorte.Date, tsZona));
        }

        public string gsGuardar(List<SeguimientoCompromiso> toListaNuevos, List<SeguimientoCompromiso> toListaSeguimiento, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";
            loBaseDa.CreateContext();

            foreach (var item in toListaNuevos.Where(x => !string.IsNullOrEmpty(x.Compromiso) && x.FechaCompromiso == null))
            {
                psMsg = string.Format("{0}Cliente {1} con la factura {2} no tiene fecha de compromiso\n", psMsg, item.Cliente, item.Factura);
            }

            foreach (var item in toListaNuevos.Where(x => string.IsNullOrEmpty(x.Compromiso) && x.FechaCompromiso != null))
            {
                psMsg = string.Format("{0}Cliente {1} con la factura {2} no tiene compromiso\n", psMsg, item.Cliente, item.Factura);
            }

            var poLista = toListaNuevos.Where(x => !string.IsNullOrEmpty(x.Compromiso) && x.FechaCompromiso != null).ToList();

            foreach (var item in poLista)
            {
                if (item.CodigoMotivo == Diccionario.Seleccione)
                {
                    psMsg = string.Format("{0}Cliente {1} con la factura {2} no tiene seleccionado el motivo\n",psMsg,item.Cliente, item.Factura);
                }
            }

            if (string.IsNullOrEmpty(psMsg))
            {

                foreach (var item in poLista)
                {
                    COBTSEGUIMIENTOCOMPROMISO poRegistro = new COBTSEGUIMIENTOCOMPROMISO();
                    loBaseDa.CreateNewObject(out poRegistro);

                    poRegistro.CodigoEstado = Diccionario.Activo;
                    poRegistro.CodCliente = item.CodCliente;
                    poRegistro.Cliente = item.Cliente;
                    poRegistro.CodZona = item.CodZona;
                    poRegistro.Zona = item.Zona;
                    poRegistro.FechaPedido = item.FechaPedido;
                    poRegistro.NumCompromiso = loBaseDa.Find<COBTSEGUIMIENTOCOMPROMISO>().Where(x=>x.CodigoEstado == Diccionario.Activo && x.DocEntry == item.DocEntry).Count() + 1;
                    poRegistro.ValorPedido = item.ValorPedido;
                    poRegistro.Factura = string.IsNullOrEmpty(item.Factura) ? "" : item.Factura;
                    poRegistro.FechaEmision = item.FechaEmision;
                    poRegistro.FechaVencimiento = item.FechaVencimiento;
                    poRegistro.FechaCompromiso = item.FechaCompromiso ?? DateTime.Now;
                    poRegistro.DiasMora = item.DiasMora;
                    poRegistro.Compromiso = item.Compromiso;
                    poRegistro.CompromisoCumplido = "0";
                    poRegistro.CodigoMotivo = item.CodigoMotivo;
                    poRegistro.UsuarioIngreso = tsUsuario;
                    poRegistro.FechaIngreso = DateTime.Now;
                    poRegistro.TerminalIngreso = tsTerminal;
                    poRegistro.DocEntry = item.DocEntry;
                    poRegistro.DocNum = item.DocNum;
                    poRegistro.Saldo = item.Saldo;
                }

                foreach (var item in toListaSeguimiento)
                {
                    var poRegistro = loBaseDa.Get<COBTSEGUIMIENTOCOMPROMISO>().Where(x => x.IdSeguimientoCompromiso == item.IdSeguimientoCompromiso).FirstOrDefault();
                    if (poRegistro != null)
                    {
                        poRegistro.Observaciones = item.Observaciones;
                        poRegistro.CompromisoCumplido = item.CompromisoCumplido;
                        poRegistro.UsuarioModificacion = tsUsuario;
                        poRegistro.FechaModificacion = DateTime.Now;
                        poRegistro.TerminalModificacion = tsTerminal;
                    }
                }

                loBaseDa.SaveChanges();
            }

            return psMsg;

        }

        public string gsGuardarSeguimiento(List<string> tsListaZona, List<SeguimientoCompromiso> toListaSeguimiento, string tsUsuario, string tsTerminal)
        {
            loBaseDa.CreateContext();
            string psMsg = "";
            
            if (string.IsNullOrEmpty(psMsg))
            {
                var poLista = loBaseDa.Get<COBTSEGUIMIENTOCOMPROMISO>().Where(x => x.CodigoEstado == Diccionario.Activo && tsListaZona.Contains(x.CodZona)).ToList();

                List<int> poListaIdPe = toListaSeguimiento.Select(x => x.IdSeguimientoCompromiso).ToList();
                List<int> piListaEliminar = poLista.Where(x => x.CodigoEstado == Diccionario.Activo && tsListaZona.Contains(x.CodZona) && !poListaIdPe.Contains(x.IdSeguimientoCompromiso)).Select(x => x.IdSeguimientoCompromiso).ToList();
                //Recorrer la base de dato modificando el codigo estado a eliminado
                foreach (var poItem in poLista.Where(x => piListaEliminar.Contains(x.IdSeguimientoCompromiso)))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    poItem.FechaModificacion = DateTime.Now;
                    poItem.TerminalModificacion = tsTerminal;
                }
                
                foreach (var item in toListaSeguimiento)
                {
                    var poRegistro = poLista.Where(x => x.IdSeguimientoCompromiso == item.IdSeguimientoCompromiso).FirstOrDefault();
                    if (poRegistro != null)
                    {
                        poRegistro.Observaciones = item.Observaciones;
                        poRegistro.CompromisoCumplido = item.CompromisoCumplido;
                        poRegistro.UsuarioModificacion = tsUsuario;
                        poRegistro.FechaModificacion = DateTime.Now;
                        poRegistro.TerminalModificacion = tsTerminal;
                    }
                }

                loBaseDa.SaveChanges();
            }

            return psMsg;

        }

        public string gsGuardarNuevos(List<SeguimientoCompromiso> toListaNuevos, string tsUsuario, string tsTerminal)
        {
            string psMsg = "";
            loBaseDa.CreateContext();

            foreach (var item in toListaNuevos.Where(x => !string.IsNullOrEmpty(x.Compromiso) && x.FechaCompromiso == null))
            {
                psMsg = string.Format("{0}Cliente {1} con la factura {2} no tiene fecha de compromiso\n", psMsg, item.Cliente, item.Factura);
            }

            foreach (var item in toListaNuevos.Where(x => string.IsNullOrEmpty(x.Compromiso) && x.FechaCompromiso != null))
            {
                psMsg = string.Format("{0}Cliente {1} con la factura {2} no tiene compromiso\n", psMsg, item.Cliente, item.Factura);
            }

            var poLista = toListaNuevos.Where(x => !string.IsNullOrEmpty(x.Compromiso) && x.FechaCompromiso != null).ToList();

            foreach (var item in poLista)
            {
                if (item.CodigoMotivo == Diccionario.Seleccione)
                {
                    psMsg = string.Format("{0}Cliente {1} con la factura {2} no tiene seleccionado el motivo\n", psMsg, item.Cliente, item.Factura);
                }
            }

            if (string.IsNullOrEmpty(psMsg))
            {
                foreach (var item in poLista)
                {
                    COBTSEGUIMIENTOCOMPROMISO poRegistro = new COBTSEGUIMIENTOCOMPROMISO();
                    loBaseDa.CreateNewObject(out poRegistro);

                    poRegistro.CodigoEstado = Diccionario.Activo;
                    poRegistro.CodCliente = item.CodCliente;
                    poRegistro.Cliente = item.Cliente;
                    poRegistro.CodZona = item.CodZona;
                    poRegistro.Zona = item.Zona;
                    poRegistro.FechaPedido = item.FechaPedido;
                    poRegistro.NumCompromiso = item.NumCompromiso;
                    poRegistro.ValorPedido = item.ValorPedido;
                    poRegistro.Factura = string.IsNullOrEmpty(item.Factura) ? "" : item.Factura;
                    poRegistro.FechaEmision = item.FechaEmision;
                    poRegistro.FechaVencimiento = item.FechaVencimiento;
                    poRegistro.FechaCompromiso = item.FechaCompromiso ?? DateTime.Now;
                    poRegistro.DiasMora = item.DiasMora;
                    poRegistro.Compromiso = item.Compromiso;
                    poRegistro.CompromisoCumplido = "0";
                    poRegistro.CodigoMotivo = item.CodigoMotivo;
                    poRegistro.UsuarioIngreso = tsUsuario;
                    poRegistro.FechaIngreso = DateTime.Now;
                    poRegistro.TerminalIngreso = tsTerminal;
                    poRegistro.DocEntry = item.DocEntry;
                    poRegistro.DocNum = item.DocNum;
                    poRegistro.Saldo = item.Saldo;
                }

                loBaseDa.SaveChanges();
            }

            return psMsg;

        }
    }
}
