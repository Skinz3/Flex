using Flex.Attributes;
using Flex.Expressions;
using Flex.Extensions;
using Flex.IO;
using Flex.SQL;
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
        internal PropertyInfo PrimaryProperty
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
        private TableWriter<T> Writer
        {
            get;
            set;
        }
        public Table(MySqlDatabase database, string tableName)
        {
            this.Database = database;
            this.Reader = new TableReader<T>(this);
            this.Name = tableName;
            this.Properties = typeof(T).GetProperties().Where(x => !x.HasAttribute<TransientAttribute>()).ToArray();
            this.PrimaryProperty = Properties.FirstOrDefault(x => x.HasAttribute<PrimaryAttribute>());
            this.Create();
        }

        public void Insert(T entity)
        {

        }
        public void Update(T entity)
        {

        }
        public void Delete(T entity)
        {

        }
        public int DeleteAll()
        {
            return NonQuery(string.Format(SQLConstants.Delete, Name));
        }

        public void Drop()
        {
            NonQuery(string.Format(SQLConstants.Drop, Name));
        }

        public IEnumerable<T> Select()
        {
            return Reader.Query(string.Format(SQLConstants.Select, Name));
        }
        public IEnumerable<T> Select(Expression<Func<T, bool>> expression)
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Translate(expression);
            return Reader.Query(string.Format(SQLConstants.SelectWhere, Name, builder.WhereClause));
        }

        public int NonQuery(string query)
        {
            MySqlConnection connection = Database.UseConnection();

            using (var command = new MySqlCommand(query, connection))
            {
                return command.ExecuteNonQuery();
            }
        }

        public TResult Scalar<TResult>(string query)
        {
            MySqlConnection connection = Database.UseConnection();

            using (var command = new MySqlCommand(query, connection))
            {
                return (TResult)command.ExecuteScalar();
            }
        }

        private void Create()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Properties.Length; i++)
            {
                PropertyInfo property = Properties[i];
                sb.Append(property.Name);
                sb.Append(" ");
                sb.Append(TypeMapping.GetSQLType(property));

                if (i < Properties.Length - 1 || PrimaryProperty != null)
                {
                    sb.Append(',');
                }

            }

            if (PrimaryProperty != null)
            {
                sb.Append("PRIMARY KEY(" + PrimaryProperty.Name + ")");

            }

            NonQuery(string.Format(SQLConstants.CreateTable, Name, sb.ToString()));
        }

        public long Count()
        {
            return Scalar<long>(string.Format(SQLConstants.Count, Name));
        }
    }
}
