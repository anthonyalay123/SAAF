using GEN_Negocio;
using System;
using System.Data;
using System.Threading;

namespace GEN_EnvioCorreoCumpleaños
{
    class clsPrincipal
    {
        static void Main(string[] args)
        {
            var clsBase = new clsNBase();

            try
            {
                int cont = 0, acum = 0;
                DataTable dt = new DataTable();
                
                if (DateTime.Now.Day == 1)
                {
                    /// CUMPLEAÑOS DEL MES
                    dt = clsBase.goConsultaDataTable("EXEC [REHSPENVIOCUMPLEANOSMES] 'Cumpleaños del Mes','1'");
                    cont = 0; acum = 0;
                    if (dt != null)
                    {
                        cont = dt.Rows.Count;
                        foreach (DataRow item in dt.Rows)
                        {
                            string psRecipients = item[0].ToString();
                            string psSubject = item[1].ToString();
                            string psBody = item[2].ToString();

                            clsBase.EnviarPorCorreo(psRecipients, psSubject, psBody, null, false, "", "", true);
                            Thread.Sleep(5 * 1000);

                            acum++;

                            Console.WriteLine(string.Format("Enviado {0} de {1}", acum, cont));
                        }
                    }
                    
                    /// ANIVERSARIOS DEL MES
                    dt = clsBase.goConsultaDataTable("EXEC [REHSPENVIOANIVERSARIOMES]");
                    cont = 0; acum = 0;
                    if (dt != null)
                    {
                        cont = dt.Rows.Count;
                        foreach (DataRow item in dt.Rows)
                        {
                            string psRecipients = item[0].ToString();
                            string psSubject = item[1].ToString();
                            string psBody = item[2].ToString();

                            if (!string.IsNullOrEmpty(psRecipients))
                            {
                                clsBase.EnviarPorCorreo(psRecipients, psSubject, psBody, null, false, "", "", true);
                                Thread.Sleep(5 * 1000);
                            }

                            acum++;

                            Console.WriteLine(string.Format("Enviado {0} de {1}", acum, cont));
                        }
                    }
                }
                
                /// CUMPLEAÑOS DEL DÍA
                dt = clsBase.goConsultaDataTable("EXEC [REHSPENVIOCUMPLEANOSDIA] 'Cumpleaños del Dia','1'");
                cont = 0; acum = 0;

                if (dt != null) 
                {
                    cont = dt.Rows.Count;
                    foreach (DataRow item in dt.Rows)
                    {
                        string psRecipients = item[0].ToString();
                        string psSubject = item[1].ToString();
                        string psBody = item[2].ToString();

                        clsBase.EnviarPorCorreo(psRecipients, psSubject, psBody, null, false, "", "", true);
                        Thread.Sleep(5 * 1000);

                        acum++;

                        Console.WriteLine(string.Format("Enviado {0} de {1}", acum, cont));
                    }
                }

                /// ANIVERSARIOS DEL DIA
                dt = clsBase.goConsultaDataTable("EXEC [REHSPENVIOANIVERSARIODIA]");
                cont = 0; acum = 0;
                if (dt != null)
                {
                    cont = dt.Rows.Count;
                    foreach (DataRow item in dt.Rows)
                    {
                        string psRecipients = item[0].ToString();
                        string psSubject = item[1].ToString();
                        string psBody = item[2].ToString();

                        if (!string.IsNullOrEmpty(psRecipients))
                        {
                            clsBase.EnviarPorCorreo(psRecipients, psSubject, psBody, null, false, "", "", true);
                            Thread.Sleep(5 * 1000);
                        }
                        
                        acum++;

                        Console.WriteLine(string.Format("Enviado {0} de {1}", acum, cont));
                    }
                }


            }
            catch (Exception ex)
            {
                clsBase.EnviarPorCorreo("jordonez@afecor.com;varevalo@afecor.com;", "Error en Envío de Correo Cumpleaños", ex.Message, null, false, "", "");
            }
            
            Environment.Exit(0);
        }
    }
}
