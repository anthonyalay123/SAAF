using GEN_Entidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace REH_Presentacion
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // Set Valores en las Variables
            Diccionario.Config.FormatoFechaBD = ConfigurationManager.AppSettings["FormatoFechaBD"].ToString();
            Diccionario.Config.FormatoFechaHoraBD = ConfigurationManager.AppSettings["FormatoFechaHoraBD"].ToString();
            Diccionario.Config.FormatoFechaSistema = ConfigurationManager.AppSettings["FormatoFechaSistema"];

            // Globalización Ecuador
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-Ec"); // Lenguaje en español
            CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = "."; // Separador decimal '.'
            customCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"; //CultureInfo.InvariantCulture.DateTimeFormat; // Set Formato dd/MNM/yyyy
            Thread.CurrentThread.CurrentCulture = customCulture;

            //DateTime fecha = DateTime.Now;
            //decimal i = 1000000M;

            //CultureInfo us = new CultureInfo("en-US");
            //Console.WriteLine(i.ToString("c"));
            //Console.WriteLine(fecha.ToString());
            //CultureInfo es = new CultureInfo("es-ES");
            //Console.WriteLine(i.ToString("c", es));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmPrincipal());

           

        }
    }
}
