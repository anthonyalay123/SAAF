using System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REH_Dato
{
    /// <summary>
    /// Autor: Victor Arévalo
    /// Fecha: 16/01/2020
    /// Clase repositorio 
    /// </summary>
    public class clsDBase
    {
        #region Variables
        private SAAFEntities loContext;
        #endregion
        /// <summary>
        /// Constructor de la clase, Inicializa el contexto
        /// </summary>
        public clsDBase()
        {
            loContext = new SAAFEntities();
            loContext.Database.CommandTimeout = 0;
        }
        /// <summary>
        /// Inicializa el contexto
        /// </summary>
        public void CreateContext()
        {
            loContext = new SAAFEntities();
            loContext.Database.CommandTimeout = 0;
        }

        /// <summary>
        /// Crea nuevo objeto al contexto
        /// </summary>
        /// <typeparam name="TEntry">Tipo de objeto del Modelo EF</typeparam>
        /// <param name="entity">Entidad de Modelo EF</param>
        public void CreateNewObject<TEntry>(out TEntry entity) where TEntry : class
        {
            dynamic poObj = Activator.CreateInstance<TEntry>();
            loContext.Set<TEntry>().Add(poObj);
            entity = poObj;
        }

        /// <summary>
        /// Consulta datos de un objeto del contexto
        /// </summary>
        /// <typeparam name="TEntry">Tipo de objeto del Modelo EF</typeparam>
        /// <returns>Retorna un Iqueryable del objeto consultado, Tracking: true</returns>
        public IQueryable<TEntry> Get<TEntry>() where TEntry : class
        {
            return loContext.Set<TEntry>();
        }

        /// <summary>
        /// Consulta datos de un objeto del contexto
        /// </summary>
        /// <typeparam name="TEntry">Tipo de objeto del Modelo EF</typeparam>
        /// <returns>Retorna un Iqueryable del objeto consultado, Tracking: false</returns>
        public IQueryable<TEntry> Find<TEntry>() where TEntry : class
        {
            return loContext.Set<TEntry>().AsNoTracking();
        }

        /// <summary>
        /// Consulta un script de BD
        /// </summary>
        /// <typeparam name="TEntry">Tipo de objeto del Modelo EF</typeparam>
        /// <param name="tsQuery">Script de consulta de BD</param>
        /// <param name="toParameters">Parámetros para condicionar la consulta</param>
        /// <returns></returns>
        public IQueryable<TEntry> SelectQuery<TEntry>(string tsQuery, params object[] toParameters) where TEntry : class
        {
            return loContext.Set<TEntry>().SqlQuery(tsQuery, toParameters).AsQueryable();
        }

        /// <summary>
        /// Ejecuta un Store Procedure
        /// </summary>
        /// <param name="tsQuery">Nombre del Sp a ejecutar con sus parámetros en caso de existir</param>
        /// <param name="toParameters">Parámetros del Sp</param>
        public List<TEntry> ExecStoreProcedure<TEntry>(string tsQuery, params object[] toParameters) where TEntry : class
        {
            return loContext.Database.SqlQuery<TEntry>(tsQuery, toParameters).ToList();
        }

        /// <summary>
        /// Ejecuta un query en base de datos
        /// </summary>
        /// <param name="tsQuery">Cadena con el script para ejecutar en base de BD</param>
        public void ExecuteQuery(string tsQuery)
        {
            loContext.Database.ExecuteSqlCommand(tsQuery);
        }

        /// <summary>
        /// Elimina una entidad del contexto
        /// </summary>
        /// <typeparam name="TEntry">Tipo de objeto del Modelo EF</typeparam>
        /// <param name="entity">Entidad de Modelo EF</param>
        public void Detach<TEntry>(ref TEntry entity) where TEntry : class, new()
        {
            loContext.Entry(entity).State = EntityState.Detached;
        }

        /// <summary>
        /// Cambia de estado a un objeto del Modelo EF
        /// </summary>
        /// <typeparam name="TEntry">Tipo de objeto del Modelo EF</typeparam>
        /// <param name="entity">Entidad de Modelo EF</param>
        /// <param name="state">Estado de objeto en el Modelo EF</param>
        public void ChangeState<TEntry>(ref TEntry entity, EntityState state) where TEntry : class
        {
            loContext.Entry(entity).State = state;
        }

        /// <summary>
        /// Consulta el estado de un objeto en el Modelo EF
        /// </summary>
        /// <typeparam name="TEntry">Tipo de objeto del Modelo EF</typeparam>
        /// <param name="entity">Entidad de Modelo EF</param>
        /// <returns>Devuelve el estado del objeto en el Modelo EF</returns>
        public EntityState State<TEntry>(TEntry entity) where TEntry : class
        {
            return loContext.Entry(entity).State;
        }

        /// <summary>
        /// Genera nueva secuencia
        /// </summary>
        /// <param name="tsNombreTabla">Nombre de la tabla</param>
        /// <returns>Secuencia a guardar</returns>
        public string GeneraSecuencia(string tsNombreTabla)
        {
            return loContext.GENSPGENERASECUENCIA(tsNombreTabla).FirstOrDefault();
        }

        
        /// <summary>
        /// Convierte un Objeto a un DataTable
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable<TEntry>(IList<TEntry> @entities) where TEntry : class
        {
            DataTable _dt = new DataTable();
            int _index = 0;
            string _nameEntry = typeof(TEntry).Name;
            ObjectContext ObContext = ((IObjectContextAdapter)loContext).ObjectContext;
            var poedmProperty = (from meta in ObContext.MetadataWorkspace.GetItems(DataSpace.CSpace).Where(m => m.BuiltInTypeKind == BuiltInTypeKind.EntityType)
                                 from p in (meta as EntityType).Properties.Where(p => p.DeclaringType.Name == _nameEntry)
                                 select p).ToList();

            foreach (EdmProperty edmProperty in poedmProperty)
            {
                _dt.Columns.Add(new DataColumn() { ColumnName = edmProperty.Name });
                _index++;
            }

            foreach (var _entry in entities)
            {
                DataRow _newRow = _dt.NewRow();
                foreach (DataColumn _columns in _dt.Columns)
                    _newRow[_columns.ColumnName] = _entry.GetType().GetProperty(_columns.ColumnName).GetValue(_entry, null);
                _dt.Rows.Add(_newRow);
            }
            entities = null;

            return _dt;
        }

        /// <summary>
        /// Retorna valores en String
        /// </summary>
        /// <typeparam name="TEntry"></typeparam>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public void Auditoria<TEntry>(TEntry Entity, string TsTipoAccion, DateTime tdFecha, string tsUsuario, string tsTerminal) where TEntry : class
        {
            string psData = string.Empty;
            DataTable _dt = new DataTable();
            string _nameEntry = typeof(TEntry).Name;

            ObjectContext ObContext = ((IObjectContextAdapter)loContext).ObjectContext;
            var poedmProperty = (from meta in ObContext.MetadataWorkspace.GetItems(DataSpace.CSpace).Where(m => m.BuiltInTypeKind == BuiltInTypeKind.EntityType)
                                 from p in (meta as EntityType).Properties.Where(p => p.DeclaringType.Name == _nameEntry)
                                 select p).ToList();

            foreach (EdmProperty edmProperty in poedmProperty)
            {
                psData = psData + edmProperty.Name + "=" + Entity.GetType().GetProperty(edmProperty.Name).GetValue(Entity, null) + ",";
            }
            if (psData.Length > 0) psData = psData.Substring(0, psData.Length - 1);
            //if (psData.Length > 800) psData = psData.Substring(0, 800);

            GENLAUDITORIA poAuditoria = new GENLAUDITORIA();
            CreateNewObject(out poAuditoria);
            poAuditoria.CodigoEstado = "A";
            poAuditoria.NombreTabla = _nameEntry;
            poAuditoria.Accion = TsTipoAccion;
            poAuditoria.Data = psData;
            poAuditoria.UsuarioIngreso = tsUsuario;
            poAuditoria.FechaIngreso = tdFecha;
            poAuditoria.TerminalIngreso = tsTerminal;
        }

        /// <summary>
        /// Guarda todos los cambios realizados en el contexto y los asienta en BD
        /// </summary>
        public void SaveChanges()
        {
            loContext.SaveChanges();
        }

        public DataTable DataTable(string sqlQuery,
                                     params DbParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            DbConnection connection = loContext.Database.Connection;
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);
            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.CommandTimeout = int.MaxValue;
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlQuery;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }

        public DataSet DataSet(string sqlQuery,
                                     params DbParameter[] parameters)
        {
            DataSet ds = new DataSet();
            DbConnection connection = loContext.Database.Connection;
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);
            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlQuery;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(ds);
                }
            }
            return ds;
        }


        //public DataSet DataSet(string sqlQuery,
        //                             params DbParameter[] parameters)
        //{
        //    DataSet dataSet = new DataSet();
        //    var dt = DataTable(sqlQuery, parameters);
        //    dataSet.Tables.Add(dt);
        //    return dataSet;
        //}

        /// <summary>
        /// Clase para recibir parámetros desde capa de negocio, cuando invoca a un SqlExec
        /// </summary>
        public class parameterSql {
            public string NameParameter {get; set;}
            public object Valor { get; set; }
        }


    }
}
