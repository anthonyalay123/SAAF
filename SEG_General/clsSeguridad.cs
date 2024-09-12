using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG_General
{
    /// <summary>
    /// Clase General de Seguridad
    /// 15/01/2019 VAR
    /// </summary>
    public static class clsSeguridad
    {
        /// <summary>
        /// Función que devuelve una cadena encriptada
        /// </summary>
        /// <param name="tsCadena">texto a encriptar</param>
        /// <returns>texto encriptado</returns>
        public static string gsEncriptar(string tsCadena)
        {
            string result = string.Empty;
            byte[] encryted = Encoding.Unicode.GetBytes(tsCadena);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        /// <summary>
        /// Función que devuelve una cadena desencriptada
        /// </summary>
        /// <param name="tsCadena">texto a desencriptar</param>
        /// <returns>valor desencriptado</returns>
        public static string gsDesencriptar(string tsCadena)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(tsCadena);
            result = Encoding.Unicode.GetString(decryted);
            return result;
        }
    }
}
