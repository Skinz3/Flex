using Flex.Attributes;
using Flex.Entities;
using Flex.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Flex
{
    public abstract class Database
    {
        private Assembly EntitiesAssembly
        {
            get;
            set;
        }
        protected Dictionary<Type, ITable> Tables
        {
            get;
            private set;
        }

        public Database(Assembly entitiesAssembly)
        {
            this.EntitiesAssembly = entitiesAssembly;
            Build();
        }

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
        public void CreateAllTables()
        {
            foreach (var table in Tables.Values)
            {
                table.Create();
            }
        }
        public void Drop<T>() where T : IEntity
        {
            var table = GetTable<T>();
            table.Drop();
            Tables.Remove(typeof(T));
        }

        public abstract int NonQuery(string query);

        public abstract T Scalar<T>(string query);

        public abstract DbDataReader ExecuteReader(string query);

        public Table<T> GetTable<T>() where T : IEntity
        {
            return (Table<T>)Tables[typeof(T)];
        }
        public IEnumerable<ITable> GetTables()
        {
            return Tables.Values;
        }

     
    }
}
