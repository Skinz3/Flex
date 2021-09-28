using Flex.Attributes;
using Flex.Expressions;
using Flex.Extensions;
using Flex.IO;
using Flex.Pool;
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

        internal Database Database
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
        internal PropertyInfo[] BlobProperties
        {
            get;
            private set;
        }
        internal PropertyInfo[] UpdateProperties
        {
            get;
            private set;
        }
        internal PropertyInfo[] AutoIncrementProperties
        {
            get;
            private set;
        }
        internal PropertyInfo[] NotNullProperties
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
        public Table(Database database, string tableName)
        {
            this.Database = database;
            this.Reader = new TableReader<T>(this);
            this.Writer = new TableWriter<T>(this);
            this.Name = tableName;
            this.Build();
        }

        private void Build()
        {
            this.Properties = typeof(T).GetProperties().Where(x => !x.HasAttribute<TransientAttribute>()).OrderBy(x => x.MetadataToken).ToArray();
            this.PrimaryProperty = Properties.FirstOrDefault(x => x.HasAttribute<PrimaryAttribute>());
            this.UpdateProperties = Properties.Where(x => x.HasAttribute<UpdateAttribute>()).ToArray();
            this.BlobProperties = Properties.Where(x => x.HasAttribute<BlobAttribute>()).ToArray();
            this.AutoIncrementProperties = Properties.Where(x => x.HasAttribute<AutoIncrementAttribute>()).ToArray();
            this.NotNullProperties = Properties.Where(x => x.HasAttribute<NotNullAttribute>()).ToArray();
        }

        public void Insert(T entity)
        {
            Writer.Insert(new[] { entity });
        }
        public void Insert(IEnumerable<T> entities)
        {
            Writer.Insert(entities);
        }
        public void Update(T entity)
        {

        }
        public void Delete(T entity)
        {

        }
        public int DeleteAll()
        {
            return Database.Provider.NonQuery(string.Format(SQLConstants.Delete, Name));
        }

        public void Drop()
        {
            Database.Provider.NonQuery(string.Format(SQLConstants.Drop, Name));
        }

        public IEnumerable<T> Select()
        {
            return Reader.Read(string.Format(SQLConstants.Select, Name));
        }
        public IEnumerable<T> Select(Expression<Func<T, bool>> expression)
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Translate(expression);
            return Reader.Read(string.Format(SQLConstants.SelectWhere, Name, builder.WhereClause));
        }
        public TOut Max<TOut>(Expression<Func<T, TOut>> expression)
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Translate(expression);
            return Database.Provider.Scalar<TOut>(string.Format(SQLConstants.Max, Name, builder.WhereClause));
        }
        internal object Max(string fieldName)
        {
            return Database.Provider.Scalar<object>(string.Format(SQLConstants.Max, Name, fieldName));
        }
        public void Create()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Properties.Length; i++)
            {
                PropertyInfo property = Properties[i];
                sb.Append(property.Name);
                sb.Append(" ");
                sb.Append(TypeMapping.GetSQLType(property));

                if (NotNullProperties.Contains(property))
                {
                    sb.Append(" ");
                    sb.Append(SQLConstants.NotNull);
                }

                if (AutoIncrementProperties.Contains(property))
                {
                    sb.Append(" ");
                    sb.Append(SQLConstants.AutoIncrement);
                }

                if (i < Properties.Length - 1 || PrimaryProperty != null)
                {
                    sb.Append(',');
                }
            }

            if (PrimaryProperty != null)
            {
                sb.Append(string.Format(SQLConstants.PrimaryKey, PrimaryProperty.Name));
            }

            Database.Provider.NonQuery(string.Format(SQLConstants.CreateTable, Name, sb.ToString()));
        }

        public long Count()
        {
            return Database.Provider.Scalar<long>(string.Format(SQLConstants.Count, Name));
        }


    }
}
