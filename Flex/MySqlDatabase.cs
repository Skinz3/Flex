using Flex.Attributes;
using Flex.Entities;
using Flex.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Flex
{
    public class MySqlDatabase
    {
        private MySqlConnection MySqlConnection
        {
            get;
            set;
        }
        private Assembly EntitiesAssembly
        {
            get;
            set;
        }
        private Dictionary<Type, ITable> Tables
        {
            get;
            set;
        }

        public MySqlDatabase(Assembly entitiesAssembly, string databaseName, string host = "127.0.0.1", string user = "root", string password = "")
        {
            EntitiesAssembly = entitiesAssembly;
            string connectionString = string.Format("Server={0};UserId={1};Password={2};Database={3};SslMode=none;", host, user, password, databaseName);
            MySqlConnection = new MySqlConnection(connectionString);
            UseConnection();
            Build();
        }
        public MySqlDatabase(string databaseName, string host = "127.0.0.1", string user = "root", string password = "") : this(Assembly.GetEntryAssembly(), databaseName, host, user, password)
        {

        }

        /// <summary>
        /// Rename ?
        /// </summary>
        private void Build()
        {
            Tables = new Dictionary<Type, ITable>();

            var tableTypes = EntitiesAssembly.GetTypes().Where(x => x.HasInterface<IEntity>()).ToArray();
            foreach (var type in tableTypes)
            {
                TableAttribute entityAttribute = type.GetCustomAttribute<TableAttribute>();
                Type genericType = typeof(Table<>).MakeGenericType(new Type[] { type });
                ITable table = (ITable)Activator.CreateInstance(genericType, new object[] { this, entityAttribute.TableName });

                Tables.Add(type, table);
            }
        }

        public void Drop<T>() where T : IEntity
        {
            var table = GetTable<T>();
            table.Drop();
            Tables.Remove(typeof(T));
        }

        public Table<T> GetTable<T>() where T : IEntity
        {
            return (Table<T>)Tables[typeof(T)];
        }
        public IEnumerable<ITable> GetTables()
        {
            return Tables.Values;
        }
       
        internal MySqlConnection UseConnection()
        {
            if (!MySqlConnection.Ping())
            {
                MySqlConnection.Close();
                MySqlConnection.Open();
            }

            return MySqlConnection;
        }

    }
}
