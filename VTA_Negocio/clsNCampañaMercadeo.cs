using GEN_Entidad;
using GEN_Entidad.Entidades.Ventas;
using GEN_Negocio;
using REH_Dato;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace VTA_Negocio
{
    public class clsNCampañaMercadeo : clsNBase
    {
        public string gsGuardar(CampanaPublicitaria toObject, string tsUsuario, out int tId)
        {
            tId = 0;

            loBaseDa.CreateContext();
            string psResult = string.Empty;

            psResult = lsEsValido(toObject);
            List<string> psListaAdjuntoEliminar = new List<string>();

            if (psResult == string.Empty)
            {

                var poObject = loBaseDa.Get<VTAPCAMPANAPUBLICITARIA>().Include(x => x.VTAPCAMPANAPUBLICITARIADESTINATARIO)
                    .Include(x => x.VTAPCAMPANAPUBLICITARIAADJUNTO).Include(x=>x.VTAPCAMPANAPUBLICITARIACUERPO)
                    .Where(x => x.IdCampanaPublicitaria == toObject.IdCampanaPublicitaria).FirstOrDefault();
                if (poObject != null)
                {
                    poObject.UsuarioModificacion = tsUsuario;
                }
                else
                {
                    poObject = new VTAPCAMPANAPUBLICITARIA();
                    loBaseDa.CreateNewObject(out poObject);
                    poObject.UsuarioIngreso = tsUsuario;
                    poObject.FechaIngreso = DateTime.Now;
                    poObject.TerminalIngreso = "";
                }

                poObject.Asunto = toObject.Asunto;
                poObject.CodigoEstado = Diccionario.Activo;
                poObject.CuerpoDelEmail = toObject.CuerpoDelEmail;
                poObject.Descripcion = toObject.Descripcion;
                poObject.IsBodyHtml = true;
                poObject.NombrePresentar = toObject.NombrePresentar;
                poObject.Observacion = toObject.Observacion;


                List<int> poListaIdPe = toObject.CampanaPublicitariaDestinatario.Select(x => x.IdCampanaPublicitariaDestinatario).ToList();
                List<int> piListaEliminar = poObject.VTAPCAMPANAPUBLICITARIADESTINATARIO.Where(x => !poListaIdPe.Contains(x.IdCampanaPublicitariaDestinatario)).Select(x => x.IdCampanaPublicitariaDestinatario).ToList();
                //Recorrer la base de dato modificando el codigo estado a eliminado
                foreach (var poItem in poObject.VTAPCAMPANAPUBLICITARIADESTINATARIO.Where(x => piListaEliminar.Contains(x.IdCampanaPublicitariaDestinatario)))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    //poItem.FechaModificacion = DateTime.Now;
                    //poItem.TerminalModificacion = tsTerminal;
                }

                if (toObject.CampanaPublicitariaDestinatario != null)
                {
                    foreach (var factura in toObject.CampanaPublicitariaDestinatario)
                    {
                        var poObjectItem = poObject.VTAPCAMPANAPUBLICITARIADESTINATARIO.Where(x => x.IdCampanaPublicitariaDestinatario == factura.IdCampanaPublicitariaDestinatario && factura.IdCampanaPublicitariaDestinatario != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                        }
                        else
                        {
                            poObjectItem = new VTAPCAMPANAPUBLICITARIADESTINATARIO();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = "";
                            poObject.VTAPCAMPANAPUBLICITARIADESTINATARIO.Add(poObjectItem);
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.EmailCC = factura.EmailCC;
                        poObjectItem.EmailDestinatario = factura.EmailDestinatario;
                        poObjectItem.Enviado = factura.Enviado;
                        
                    }
                }

                poListaIdPe = toObject.CampanaPublicitariaAdjunto.Select(x => x.IdCampanaPublicitariaAdjunto).ToList();
                piListaEliminar = poObject.VTAPCAMPANAPUBLICITARIAADJUNTO.Where(x => !poListaIdPe.Contains(x.IdCampanaPublicitariaAdjunto)).Select(x => x.IdCampanaPublicitariaAdjunto).ToList();
                //Recorrer la base de dato modificando el codigo estado a eliminado
                foreach (var poItem in poObject.VTAPCAMPANAPUBLICITARIAADJUNTO.Where(x => piListaEliminar.Contains(x.IdCampanaPublicitariaAdjunto)))
                {
                    psListaAdjuntoEliminar.Add(poItem.ArchivoAdjunto);
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    //poItem.FechaModificacion = DateTime.Now;
                    //poItem.TerminalModificacion = tsTerminal;
                }

                if (toObject.CampanaPublicitariaAdjunto != null)
                {
                    foreach (var factura in toObject.CampanaPublicitariaAdjunto)
                    {
                        var poObjectItem = poObject.VTAPCAMPANAPUBLICITARIAADJUNTO.Where(x => x.IdCampanaPublicitariaAdjunto == factura.IdCampanaPublicitariaAdjunto && factura.IdCampanaPublicitariaAdjunto != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                        }
                        else
                        {
                            poObjectItem = new VTAPCAMPANAPUBLICITARIAADJUNTO();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = "";
                            poObject.VTAPCAMPANAPUBLICITARIAADJUNTO.Add(poObjectItem);
                        }

                        if (poObjectItem.ArchivoAdjunto != factura.ArchivoAdjunto && !string.IsNullOrEmpty(poObjectItem.ArchivoAdjunto))
                        {
                            psListaAdjuntoEliminar.Add(poObjectItem.ArchivoAdjunto);
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.Ruta = "";
                        poObjectItem.ArchivoAdjunto = factura.ArchivoAdjunto.Trim();
                        poObjectItem.NombreOriginal = factura.NombreOriginal.Trim();
                    }
                }

                poListaIdPe = toObject.CampanaPublicitariaCuerpo.Select(x => x.IdCampanaPublicitariaCuerpo).ToList();
                piListaEliminar = poObject.VTAPCAMPANAPUBLICITARIACUERPO.Where(x => !poListaIdPe.Contains(x.IdCampanaPublicitariaCuerpo)).Select(x => x.IdCampanaPublicitariaCuerpo).ToList();
                //Recorrer la base de dato modificando el codigo estado a eliminado
                foreach (var poItem in poObject.VTAPCAMPANAPUBLICITARIACUERPO.Where(x => piListaEliminar.Contains(x.IdCampanaPublicitariaCuerpo)))
                {
                    poItem.CodigoEstado = Diccionario.Eliminado;
                    poItem.UsuarioModificacion = tsUsuario;
                    //poItem.FechaModificacion = DateTime.Now;
                    //poItem.TerminalModificacion = tsTerminal;
                }

                if (toObject.CampanaPublicitariaCuerpo != null)
                {
                    var poCombo = goComboTipoEtiqueta();
                    foreach (var factura in toObject.CampanaPublicitariaCuerpo)
                    {
                        var poObjectItem = poObject.VTAPCAMPANAPUBLICITARIACUERPO.Where(x => x.IdCampanaPublicitariaCuerpo == factura.IdCampanaPublicitariaCuerpo && factura.IdCampanaPublicitariaCuerpo != 0).FirstOrDefault();
                        if (poObjectItem != null)
                        {
                            poObjectItem.UsuarioModificacion = tsUsuario;
                            poObjectItem.FechaModificacion = DateTime.Now;
                            poObjectItem.TerminalModificacion = "";
                        }
                        else
                        {
                            poObjectItem = new VTAPCAMPANAPUBLICITARIACUERPO();
                            poObjectItem.UsuarioIngreso = tsUsuario;
                            poObjectItem.FechaIngreso = DateTime.Now;
                            poObjectItem.TerminalIngreso = "";
                            poObject.VTAPCAMPANAPUBLICITARIACUERPO.Add(poObjectItem);
                        }

                        poObjectItem.CodigoEstado = Diccionario.Activo;
                        poObjectItem.CodigoTipo = factura.CodigoTipo;
                        poObjectItem.Tipo = poCombo.Where(x => x.Codigo == factura.CodigoTipo).Select(x => x.Descripcion).FirstOrDefault();
                        poObjectItem.Descripcion = factura.Descripcion;
                    }
                }


                using (var poTran = new TransactionScope())
                {
                    loBaseDa.SaveChanges();

                    tId = poObject.IdCampanaPublicitaria;

                    //Guardar Documentos Adjuntos
                    foreach (var poItem in toObject.CampanaPublicitariaAdjunto)
                    {
                        if (!string.IsNullOrEmpty(poItem.ArchivoAdjunto) && !string.IsNullOrEmpty(poItem.RutaDestino))
                        {
                            if (poItem.RutaOrigen != poItem.RutaDestino)
                            {
                                if (poItem.RutaOrigen != null)
                                {
                                    File.Copy(poItem.RutaOrigen, poItem.RutaDestino);
                                    //Subir Archivo a ruta FTP
                                    SubirArchivoAFTP(Diccionario.Credenciales.Ftp.UrlRoot + Diccionario.CarpetaFtp.Ventas, Diccionario.Credenciales.Ftp.Usuario, Diccionario.Credenciales.Ftp.Contrasena, poItem.RutaDestino);
                                }

                            }
                        }
                    }

                    foreach (var psItem in psListaAdjuntoEliminar)
                    {
                        string eliminar = ConfigurationManager.AppSettings["CarpetaVtaCamPu"].ToString() + psItem;
                        File.Delete(eliminar);
                    }

                    poTran.Complete();
                }
            }
            return psResult;
        }

        public CampanaPublicitaria goConsultar(int tId)
        {
            return lCargarDatos(loBaseDa.Find<VTAPCAMPANAPUBLICITARIA>()
                .Include(x=>x.VTAPCAMPANAPUBLICITARIAADJUNTO).Include(x => x.VTAPCAMPANAPUBLICITARIADESTINATARIO)
                .Where(x=>x.IdCampanaPublicitaria == tId).ToList()).FirstOrDefault();
        }

        public List<CampanaPublicitaria> goListar()
        {
            return lCargarDatos(loBaseDa.Find<VTAPCAMPANAPUBLICITARIA>()
                .Include(x => x.VTAPCAMPANAPUBLICITARIAADJUNTO).Include(x => x.VTAPCAMPANAPUBLICITARIADESTINATARIO)
                .Where(x => x.CodigoEstado == Diccionario.Activo).ToList());
        }

        private List<CampanaPublicitaria> lCargarDatos(List<VTAPCAMPANAPUBLICITARIA> toBd)
        {
            List<CampanaPublicitaria> poResult = new List<CampanaPublicitaria>();
            foreach (var obj in toBd)
            {
                CampanaPublicitaria poCab = new CampanaPublicitaria();
                poCab.Asunto = obj.Asunto;
                poCab.CuerpoDelEmail = obj.CuerpoDelEmail;
                poCab.Descripcion = obj.Descripcion;
                poCab.IsBodyHtml = obj.IsBodyHtml;
                poCab.NombrePresentar = obj.NombrePresentar;
                poCab.Observacion = obj.Observacion;
                poCab.IdCampanaPublicitaria = obj.IdCampanaPublicitaria;

                foreach (var item in obj.VTAPCAMPANAPUBLICITARIADESTINATARIO.Where(x=>x.CodigoEstado == Diccionario.Activo))
                {
                    CampanaPublicitariaDestinatario poObjectItem = new CampanaPublicitariaDestinatario();
                    poObjectItem.EmailCC = item.EmailCC;
                    poObjectItem.EmailDestinatario = item.EmailDestinatario;
                    poObjectItem.Enviado = item.Enviado;
                    poObjectItem.IdCampanaPublicitaria = item.IdCampanaPublicitaria;
                    poObjectItem.IdCampanaPublicitariaDestinatario = item.IdCampanaPublicitariaDestinatario;
                    poCab.CampanaPublicitariaDestinatario.Add(poObjectItem);
                }

                foreach (var item in obj.VTAPCAMPANAPUBLICITARIAADJUNTO.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    CampanaPublicitariaAdjunto poObjectItem = new CampanaPublicitariaAdjunto();
                    poObjectItem.Ruta = item.Ruta;
                    poObjectItem.IdCampanaPublicitaria = item.IdCampanaPublicitaria;
                    poObjectItem.IdCampanaPublicitariaAdjunto = item.IdCampanaPublicitariaAdjunto;
                    poObjectItem.ArchivoAdjunto = item.ArchivoAdjunto;
                    poObjectItem.NombreOriginal = item.NombreOriginal;
                    poObjectItem.RutaDestino = ConfigurationManager.AppSettings["CarpetaVtaCamPu"].ToString();
                    poCab.CampanaPublicitariaAdjunto.Add(poObjectItem);
                }

                foreach (var item in obj.VTAPCAMPANAPUBLICITARIACUERPO.Where(x => x.CodigoEstado == Diccionario.Activo))
                {
                    CampanaPublicitariaCuerpo poObjectItem = new CampanaPublicitariaCuerpo();
                    poObjectItem.CodigoTipo = item.CodigoTipo;
                    poObjectItem.Tipo = item.Tipo;
                    poObjectItem.Descripcion = item.Descripcion;
                    poObjectItem.IdCampanaPublicitaria = item.IdCampanaPublicitaria;
                    poObjectItem.IdCampanaPublicitariaCuerpo = item.IdCampanaPublicitariaCuerpo;
                    poCab.CampanaPublicitariaCuerpo.Add(poObjectItem);
                }

                poResult.Add(poCab);
            }

            return poResult;
        }

        private string lsEsValido(CampanaPublicitaria toObject)
        {
            string psMsg = string.Empty;
            //if (string.IsNullOrEmpty(toObject.NombrePresentar))
            //{
            //    psMsg = psMsg + "Ingrese Nombre a Presentar \n";
            //}
            if (string.IsNullOrEmpty(toObject.Descripcion))
            {
                psMsg = psMsg + "Ingrese Descripcion \n";
            }
            if (string.IsNullOrEmpty(toObject.Asunto))
            {
                psMsg = psMsg + "Ingrese Asunto \n";
            }

            //if (string.IsNullOrEmpty(toObject.CuerpoDelEmail))
            //{
            //    psMsg = psMsg + "Ingrese el Cuerpo del Correo \n";
            //}

            int num = 1;
            foreach (var factura in toObject.CampanaPublicitariaDestinatario)
            {
                if (string.IsNullOrEmpty(factura.EmailDestinatario))
                {
                    psMsg = psMsg + "Falta agregar Correo Destinatario en la fila:" + num + "\n";
                }
                num = num + 1;
            }

            num = 0;

            foreach (var a in toObject.CampanaPublicitariaAdjunto)
            {
                num = num + 1;
                if (string.IsNullOrEmpty(a.ArchivoAdjunto))
                {
                    psMsg = psMsg + "Ingrese el Archivo Adjunto en la fila: " + num + "\n";
                }

            }
            
            return psMsg;
        }

        public void gActualizaEnviado(int tId)
        {
            var poObj = loBaseDa.Get<VTAPCAMPANAPUBLICITARIADESTINATARIO>().Where(x=>x.IdCampanaPublicitariaDestinatario == tId).FirstOrDefault();
            if (poObj != null)
            {
                poObj.Enviado = true;
                loBaseDa.SaveChanges();
            }
        }

    }
}
