using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Flex.Entities
{
    public class Table<T> : ITable where T : IEntity
    {
        public string Name
        {
            get;
            private set;
        }

        private PropertyInfo[] Properties
        {
            get;
            set;
        }
        private MySqlDatabase Database
        {
            get;
            set;
        }
        public Table(MySqlDatabase database, string tableName, PropertyInfo[] properties)
        {
            this.Database = database;
            this.Name = tableName;
            this.Properties = properties;
        }

        // DatabaseReader
        // DatabaseWritter
        public void Insert(IEntity entity)
        {

        }
        public void Update(IEntity entity)
        {

        }
        public void Delete(IEntity entity)
        {

        }
        public void DeleteAll()
        {

        }
        public IEnumerable<IEntity> Select()
        {
            return null;
        }
        public IEnumerable<IEntity> Select(Predicate<IEntity> entity)
        {
            return null;
        }
        public IEntity SelectOne(Predicate<IEntity> predicate)
        {
            return null;
        }

        private void Create()
        {

        }

        public long Count()
        {
            return 0;
        }
    }
}
