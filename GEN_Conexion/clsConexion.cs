using System;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;

namespace GEN_Conexion
{
    public static class clsConexion
    {
        public static class Biometrico
        {
            public static string Servidor = "192.168.11.8";
            public static string BaseDatos = "ZKTEKO";
            public static string Usuario = "sa";
            public static string Contrasena = "@fec0r$!.*";
            public static string Integrador = "false";
        }
        
        public static string MySql = "Server=35.215.99.152; Database=dbgs38ldn85vm2; Uid=uxy36ugw725ug; Pwd=Sw28Cw37;";

        public static class Ftp
        {
            public static string Servidor = "35.215.99.152";
            public static string UrlRoot = "ftp://35.215.99.152/";
            public static string Puerto = "21";
            public static string Usuario = "jordonez@afecor.com";
            public static string Contrasena = "@fec0r$!.*";
            public static bool Cifrado = false;
        }

        // <summary>
        /// Conexión PRUEBAS
        /// </summary>v arevalo   
        /// <returns></returns>
        public static DbConnection GetConnection()
        {
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
            entityBuilder.Provider = "System.Data.SqlClient";
            entityBuilder.ProviderConnectionString = "data source=192.168.11.8;initial catalog=SAAF_PRUEBAS;user id=sa;password=@fec0r$!.*;MultipleActiveResultSets=True;App=EntityFramework";
            entityBuilder.Metadata = "res://*/edmSAAF.csdl|res://*/edmSAAF.ssdl|res://*/edmSAAF.msl";
            return new EntityConnection(entityBuilder.ToString());
        }

        //public static DbConnection GetConnection()
        //{
        //    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
        //    entityBuilder.Provider = "System.Data.SqlClient";
        //    entityBuilder.ProviderConnectionString = "data source=Desarrollo\\SQLEXPRESS;initial catalog=SAAF_PRO;Integrated Security=SSPI;persist security info=True;MultipleActiveResultSets=True;App=EntityFramework";
        //    entityBuilder.Metadata = "res://*/edmSAAF.csdl|res://*/edmSAAF.ssdl|res://*/edmSAAF.msl";
        //    return new EntityConnection(entityBuilder.ToString());
        //}

        /// <summary>
        /// Conexión Local GUILLERMO MURILLO
        /// </summary>
        /// <returns></returns>
        //public static DbConnection GetConnection()
        //{
        //    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
        //    entityBuilder.Provider = "System.Data.SqlClient";
        //    entityBuilder.ProviderConnectionString = "data source=PASANTE_TI\\SQLEXPRESS;initial catalog=SAAF;user id=sa;password=@fec0r$!.*;MultipleActiveResultSets=True;App=EntityFramework";
        //    entityBuilder.Metadata = "res://*/edmSAAF.csdl|res://*/edmSAAF.ssdl|res://*/edmSAAF.msl";
        //    return new EntityConnection(entityBuilder.ToString());
        //}

        /// <summary>
        /// Conexión Local VICTOR AREVALO
        /// </summary>
        /// <returns></returns>
        //public static DbConnection GetConnection()
        //{
        //    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
        //    entityBuilder.Provider = "System.Data.SqlClient";
        //    entityBuilder.ProviderConnectionString = "data source=VAPTIDUR\\SQLEXPRESS;initial catalog=SAAF_PRO;user id=sa;password=@fec0r$!.*;MultipleActiveResultSets=True;App=EntityFramework";
        //    entityBuilder.Metadata = "res://*/edmSAAF.csdl|res://*/edmSAAF.ssdl|res://*/edmSAAF.msl";
        //    return new EntityConnection(entityBuilder.ToString());
        //}

        //public static DbConnection GetConnection()
        //{
        //    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
        //    entityBuilder.Provider = "System.Data.SqlClient";
        //    entityBuilder.ProviderConnectionString = "data source=PASANTE_TI\\SQLEXPRESS;initial catalog=SAAF_PRO;user id=sa;password=@fec0r$!.*;MultipleActiveResultSets=True;App=EntityFramework";
        //    entityBuilder.Metadata = "res://*/edmSAAF.csdl|res://*/edmSAAF.ssdl|res://*/edmSAAF.msl";
        //    return new EntityConnection(entityBuilder.ToString());
        //}


        /// <summary>
        /// Conexión Local VICTOR AREVALO
        /// </summary>
        /// <returns></returns>
        //public static DbConnection GetConnection()
        //{
        //    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
        //    entityBuilder.Provider = "System.Data.SqlClient";
        //    entityBuilder.ProviderConnectionString = "data source=VAPTIDUR\\SQLEXPRESS;initial catalog=SAAF_PRO;user id=sa;password=12345;MultipleActiveResultSets=True;App=EntityFramework";
        //    entityBuilder.Metadata = "res://*/edmSAAF.csdl|res://*/edmSAAF.ssdl|res://*/edmSAAF.msl";
        //    return new EntityConnection(entityBuilder.ToString());
        //}

        /// <summary>
        /// Conexión Producción
        /// </summary>v arevalo   
        /// <returns></returns>
        //public static DbConnection GetConnection()
        //{
        //    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
        //    entityBuilder.Provider = "System.Data.SqlClient";
        //    entityBuilder.ProviderConnectionString = "data source=192.168.11.8;initial catalog=SAAF_PRO;user id=sa;password=@fec0r$!.*;MultipleActiveResultSets=True;App=EntityFramework";
        //    entityBuilder.Metadata = "res://*/edmSAAF.csdl|res://*/edmSAAF.ssdl|res://*/edmSAAF.msl";
        //    return new EntityConnection(entityBuilder.ToString());
        //}

        /// <summary>
        /// Conexión SRVDESARROLLO
        /// </summary>
        /// <returns></returns>
        //public static DbConnection GetConnection()
        //{
        //    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
        //    entityBuilder.Provider = "System.Data.SqlClient";
        //    entityBuilder.ProviderConnectionString = "data source=SRVDESARROLLO\\SQLEXPRESS;initial catalog=SAAF_PRO;user id=sa;password=@fec0r$!.*;MultipleActiveResultSets=True;App=EntityFramework";
        //    entityBuilder.Metadata = "res://*/edmSAAF.csdl|res://*/edmSAAF.ssdl|res://*/edmSAAF.msl";
        //    return new EntityConnection(entityBuilder.ToString());
        //}

        /// <summary>
        /// Conexión ANTHONY
        /// </summary>
        /// <returns></returns>
        //public static DbConnection GetConnection()
        //{
        //    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
        //    entityBuilder.Provider = "System.Data.SqlClient";
        //    entityBuilder.ProviderConnectionString = "data source=backuplaptop;initial catalog=SAAF_PRO;user id=sa;password=0979811755Aa@;MultipleActiveResultSets=True;App=EntityFramework";
        //    entityBuilder.Metadata = "res://*/edmSAAF.csdl|res://*/edmSAAF.ssdl|res://*/edmSAAF.msl";
        //    return new EntityConnection(entityBuilder.ToString());
        //}
    }


}
