using Flex.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        public void Insert(T entity)
        {

        }
        public void Update(T entity)
        {

        }
        public void Delete(T entity)
        {

        }
        public void DeleteAll()
        {

        }
        public IEnumerable<T> Select()
        {
            return null;
        }
        public IEnumerable<T> Select(Func<T, bool> func)
        {
            Expression<Func<T, bool>> expr = (entity) => func(entity);
            QueryBuilder b = new QueryBuilder();
            b.Visit(expr);

            var test = b.WhereClause;

            return null;
        }
        public IEntity SelectOne(Predicate<T> predicate)
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
