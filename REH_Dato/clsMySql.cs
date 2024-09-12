using GEN_Conexion;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REH_Dato
{
    
    public class clsMySql
    {
        
        public clsMySql()
        {
            
        }

        public string ProbarConexion()
        {
            MySqlConnection MySqlConecction = new MySqlConnection(clsConexion.MySql);
            string psMsg = string.Empty;

            MySqlConecction.Open();
            psMsg = "Conectado";
            MySqlConecction.Close();
            MySqlConecction.Dispose();

            return psMsg;
        }

        public DataTable gdtConsultar(string tsQuery)
        {
            MySqlConnection MySqlConecction = new MySqlConnection(clsConexion.MySql);
            MySqlConecction.Open();
            MySqlCommand comando = new MySqlCommand(tsQuery, MySqlConecction);
            MySqlDataAdapter adaptador = new MySqlDataAdapter();
            adaptador.SelectCommand = comando;
            DataTable dt = new DataTable();
            adaptador.Fill(dt);
            MySqlConecction.Close();
            MySqlConecction.Dispose();
            return dt;
        }
    }
}
