using Flex.Attributes;
using Flex.Expressions;
using Flex.IO;
using Flex.MySQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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

        internal MySqlDatabase Database
        {
            get;
            private set;
        }
        internal PropertyInfo[] Properties
        {
            get;
            private set;
        }

        private TableReader<T> Reader
        {
            get;
            set;
        }
        public Table(MySqlDatabase database, string tableName)
        {
            this.Database = database;
            this.Reader = new TableReader<T>(this);
            this.Name = tableName;
            this.Properties = typeof(T).GetProperties().Where(x => x.GetCustomAttribute<TransientAttribute>() == null).ToArray();
            this.Create();
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
            return Reader.Query(string.Format(QueryConstants.Select, Name));
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

        public int NonQuery(string query)
        {
            MySqlConnection connection = Database.UseConnection();

            using (var command = new MySqlCommand(query, connection))
            {
                return command.ExecuteNonQuery();
            }
        }
        public T Scalar<T>(string query)
        {
            MySqlConnection connection = Database.UseConnection();

            using (var command = new MySqlCommand(query, connection))
            {
                return (T)command.ExecuteScalar();
            }
        }

        private void Create()
        {
            string str = string.Empty;

            foreach (var property in this.Properties)
            {
                string pType = "varchar(255)";
                str += property.Name + " " + pType + ",";
            }

            str = str.Remove(str.Length - 1, 1);


            NonQuery(string.Format(QueryConstants.CreateTable, Name, str));
        }

        public long Count()
        {
            return Scalar<long>(string.Format(QueryConstants.Count, Name));
        }
    }
}
