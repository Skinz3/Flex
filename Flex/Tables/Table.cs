using Flex.Atomics;
using Flex.Attributes;
using Flex.Exceptions;
using Flex.Expressions;
using Flex.Extensions;
using Flex.IO;
using Flex.Optimizations;
using Flex.Schedulers;
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
        private Type Type = typeof(T);

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

        public EntityScheduler<T> Scheduler
        {
            get;
            private set;
        }
        internal PropertyInfo PrimaryProperty
        {
            get;
            private set;
        }
        internal LambdaAccessor PrimaryAccessor
        {
            get;
            private set;
        }
        internal LambdaActivator.ObjectActivator Activator
        {
            get;
            private set;
        }
        internal PrimaryAttribute PrimaryAttribute
        {
            get;
            set;
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
            this.Scheduler = new EntityScheduler<T>(this);
            this.Name = tableName;
            this.Build();
        }

       
        private void Build()
        {
            this.Properties = Type.GetProperties().Where(x => !x.HasAttribute<TransientAttribute>()).OrderBy(x => x.MetadataToken).ToArray();
            this.UpdateProperties = Properties.Where(x => x.HasAttribute<UpdateAttribute>()).ToArray();
            this.BlobProperties = Properties.Where(x => x.HasAttribute<BlobAttribute>()).ToArray();

            var primaryProperties = Properties.Where(x => x.HasAttribute<PrimaryAttribute>());

            if (primaryProperties.Count() != 1)
            {
                throw new InvalidMappingException("Entity " + typeof(T).Name + " must own one primary property.");
            }

            PrimaryProperty = primaryProperties.First();

            PrimaryAttribute = PrimaryProperty.GetCustomAttribute<PrimaryAttribute>();

            PrimaryAccessor = new LambdaAccessor(PrimaryProperty);

            this.NotNullProperties = Properties.Where(x => x.HasAttribute<NotNullAttribute>()).ToArray();

            var ctor = Type.GetConstructors().FirstOrDefault(x => x.GetParameters().Length == 0);

            if (ctor == null)
            {
                throw new NotSupportedException("Entity " + Type.Name + " must own an empty constructor.");
            }

            this.Activator = LambdaActivator.GetActivator(ctor);
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
            Writer.Update(new[] { entity });
        }
        public void Update(IEnumerable<T> entities)
        {
            Writer.Update(entities);
        }

        public void Drop()
        {
            Database.Provider.NonQuery(string.Format(SQLQueries.DROP, Name));
        }

        public int Delete(T entity)
        {
            var primaryKey = PrimaryAccessor.Get(entity);
            string query = string.Format(SQLQueries.DELETE, Name) + string.Format(SQLQueries.WHERE_CLAUSE, PrimaryProperty.Name + "=" + primaryKey);
            return Database.Provider.NonQuery(query);
        }
        public int Delete(IEnumerable<T> entities)
        {
            return Writer.Delete(entities);
        }
        public int Delete(Expression<Func<T, bool>> expression)
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Translate(expression);
            return Database.Provider.NonQuery(string.Format(SQLQueries.DELETE_WHERE, Name, builder.WhereClause));
        }
        public int DeleteAll()
        {
            return Database.Provider.NonQuery(string.Format(SQLQueries.DELETE, Name));
        }
        public IEnumerable<T> Select()
        {
            return Reader.Read(string.Format(SQLQueries.SELECT, Name));
        }
        public IEnumerable<T> Select(Expression<Func<T, bool>> expression)
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Translate(expression);
            return Reader.Read(string.Format(SQLQueries.SELECT, Name) + string.Format(SQLQueries.WHERE_CLAUSE, builder.WhereClause));
        }
        public TOut Max<TOut>(Expression<Func<T, TOut>> expression)
        {
            QueryBuilder builder = new QueryBuilder();
            builder.Translate(expression);
            return Database.Provider.Scalar<TOut>(string.Format(SQLQueries.MAX, Name, builder.WhereClause));
        }
        internal object Max(string fieldName)
        {
            return Database.Provider.Scalar<object>(string.Format(SQLQueries.MAX, Name, fieldName));
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
                    sb.Append(SQLQueries.NOT_NULL);
                }

                /*if (property == PrimaryProperty && PrimaryAttribute.GenerationType == GenerationType.AutoIncrement)
                {
                    sb.Append(" ");
                    sb.Append(SQLConstants.AutoIncrement);
                } */

                if (i < Properties.Length - 1 || PrimaryProperty != null)
                {
                    sb.Append(',');
                }
            }

            if (PrimaryProperty != null)
            {
                sb.Append(string.Format(SQLQueries.PRIMARY_KEY, PrimaryProperty.Name));
            }

            Database.Provider.NonQuery(string.Format(SQLQueries.CREATE_TABLE, Name, sb.ToString()));
        }

        public long Count()
        {
            return Database.Provider.Scalar<long>(string.Format(SQLQueries.COUNT, Name));
        }

        public IScheduler GetScheduler()
        {
            return Scheduler;
        }

    }
}
