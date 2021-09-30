using Flex.Attributes;
using Flex.Entities;
using Flex.Exceptions;
using Flex.Extensions;
using Flex.Providers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Flex
{
    public class Database
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
        public ISqlProvider Provider
        {
            get;
            private set;
        }
        public Database(ISqlProvider provider) : this(Assembly.GetEntryAssembly(), provider)
        {

        }
        public Database(Assembly entitiesAssembly, ISqlProvider provider)
        {

            this.EntitiesAssembly = entitiesAssembly;
            this.Provider = provider;
            this.Provider.Connect();
            Build();
            CreateAllTables();

        }

        private void Build()
        {
            Tables = new Dictionary<Type, ITable>();

            var tableTypes = EntitiesAssembly.GetTypes().Where(x => x.HasInterface<IEntity>()).ToArray();
            foreach (var type in tableTypes)
            {
                TableAttribute entityAttribute = type.GetCustomAttribute<TableAttribute>();

                if (entityAttribute == null)
                {
                    throw new InvalidMappingException("Missing Table attribute on class : " + type.Name);
                }

                Type genericType = typeof(Table<>).MakeGenericType(new Type[] { type });
                ITable table = (ITable)Activator.CreateInstance(genericType, new object[] { this, entityAttribute.TableName });

                Tables.Add(type, table);
            }


        }
        public void ApplySchedulers()
        {
            foreach (var table in GetTables())
            {
                table.GetScheduler().Apply();
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

        public Table<T> GetTable<T>() where T : IEntity
        {
            return (Table<T>)Tables[typeof(T)];
        }
        public ITable GetTable(string name)
        {
            return Tables.Values.FirstOrDefault(x => x.Name == name);
        }
        public IEnumerable<ITable> GetTables()
        {
            return Tables.Values;
        }

        public void CopyTo(Database target)
        {
            throw new NotImplementedException("This functionality is not available yet.");
        }
    }
}
