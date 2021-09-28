using Flex.Attributes;
using Flex.Entities;
using Flex.Extensions;
using Flex.SQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Flex.IO
{
    public class TableWriter<T> where T : IEntity
    {
        private Table<T> Table
        {
            get;
            set;
        }
        private PropertyInfo[] AddProperties
        {
            get;
            set;
        }
        private PropertyInfo[] UpdateProperties
        {
            get;
            set;
        }
        private PropertyInfo PrimaryProperty
        {
            get;
            set;
        }
        public TableWriter(Table<T> table)
        {
            this.Table = table;

            Type type = typeof(T);

            this.AddProperties = type.GetProperties().Where(property => !property.HasAttribute<TransientAttribute>()).OrderBy(x => x.MetadataToken).ToArray();
            this.UpdateProperties = AddProperties.Where(x => x.HasAttribute<UpdateAttribute>()).ToArray();

            IEnumerable<PropertyInfo> primaryProperties = AddProperties.Where(x => x.HasAttribute<PrimaryAttribute>());

            if (primaryProperties.Count() > 1)
            {
                throw new Exception("Entity " + table.Name + " has more then one primary properties. This is not handled currently.");
            }

            this.PrimaryProperty = primaryProperties.FirstOrDefault();
        }

        public void Insert(IEnumerable<T> entities)
        {
            DbCommand command = Table.Database.CreateSqlCommand();
 
            List<string> queries = new List<string>();

            int id = 0;

            foreach (var entity in entities)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");

                foreach (var property in AddProperties)
                {
                    sb.Append(string.Format(Table.Database.ParameterPrefix + "{0}{1},", property.Name, id));
                    DbParameter parameter = Table.Database.CreateSqlParameter(Table.Database.ParameterPrefix + property.Name + id, ConvertProperty(property.GetValue(entity)));
                    command.Parameters.Add(parameter);
                }

                sb = sb.Remove(sb.Length - 1, 1);
                sb.Append(")");

                queries.Add(sb.ToString());
                id++;
            }

            command.CommandText = string.Format(SQLConstants.Insert, Table.Name, string.Format("{0}", string.Join(",", queries)));

            command.ExecuteNonQuery();
        }

        private object ConvertProperty(object value)
        {
            if (value == null)
            {
                return "";
            }
            return value.ToString();
        }
    }
}
