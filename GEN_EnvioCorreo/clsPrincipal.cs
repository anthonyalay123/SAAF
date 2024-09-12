using GEN_Entidad.Entidades.General;
using GEN_Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GEN_EnvioCorreo
{
    class clsPrincipal
    {
        static void Main(string[] args)
        {
            var clsBase = new clsNBase();
            
            try
            {
                List<EnvioSms> poLista = new List<EnvioSms>(); 
                var dt = clsBase.goConsultaDataTable("EXEC [SBO_AFECOR].[dbo].[SP_AFE_ENVIO_SMS_PR_SN_NEW]");
                int cont = 0, acum = 0;
                if (dt != null) cont = dt.Rows.Count;

                var poListaDocEntry = clsBase.piListaDocEntry();

                foreach (DataRow item in dt.Rows)
                {

                    int piDocEntry = int.Parse(item["DocEntry"].ToString());

                    if (!poListaDocEntry.Contains(piDocEntry))
                    {
                        var poRegistro = new EnvioSms();
                        poRegistro.DocEntry = int.Parse(item[0].ToString());
                        poRegistro.Hora = int.Parse(item[1].ToString());
                        poRegistro.Time = int.Parse(item[2].ToString());
                        poRegistro.DocDate = Convert.ToDateTime(item[3].ToString());
                        poRegistro.CardCode = item[4].ToString();
                        poRegistro.CardName = item[5].ToString();
                        poRegistro.DocTotal = Convert.ToDecimal(item[6].ToString());
                        poRegistro.FormaPago = item[7].ToString();
                        poRegistro.Movil = item[8].ToString();
                        poRegistro.Destinatario = item[9].ToString();
                        poRegistro.TextoEmail = item[10].ToString();
                        poRegistro.PrevioEnvio = true;
                        poRegistro.MsgError = "";
                        poRegistro.FechaIngreso = DateTime.Now;

                        try
                        {
                            clsBase.EnviarPorCorreo(item["Destinatario"].ToString(), "SQL Server Message", item["TextoEmail"].ToString(), null, false, "", "", false);
                            //Thread.Sleep(5 * 1000);
                            poRegistro.PostEnvio = true;
                        }
                        catch (Exception e)
                        {
                            poRegistro.MsgError = e.Message.ToString();
                        }

                        poLista.Add(poRegistro);
                    }
                    
                    acum++;

                    Console.WriteLine(string.Format("Enviado {0} de {1}", acum, cont));
                }

                clsBase.gGuardaLogEnvioSms(poLista);
                   
            }
            catch (Exception ex)
            {
                clsBase.EnviarPorCorreo("jordonez@afecor.com;varevalo@afecor.com;", "Error en Envío de Correo SMS", ex.Message, null, false, "", "");
            }
           

            Environment.Exit(0);
        }
    }
}
