using Flex.Attributes;
using Flex.Entities;
using Flex.Extensions;
using Flex.SQL;
using MySql.Data.MySqlClient;
using ProtoBuf;
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

        public TableWriter(Table<T> table)
        {
            this.Table = table;
        }

        public void Insert(IEnumerable<T> entities)
        {
            DbCommand command = Table.Database.Provider.CreateSqlCommand();

            List<string> queries = new List<string>();

            int id = 0;

            foreach (var entity in entities)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");

                foreach (var property in Table.Properties)
                {
                    sb.Append(string.Format(Table.Database.Provider.ParameterPrefix + "{0}{1},", property.Name, id));
                    object value = ConvertProperty(property, property.GetValue(entity));
                    DbParameter parameter = Table.Database.Provider.CreateSqlParameter(Table.Database.Provider.ParameterPrefix + property.Name + id, value);
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
        private object ConvertProperty(PropertyInfo property, object value)
        {
            if (value == null)
            {
                return null;
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                value = ((DateTime)value).ToString(SQLConstants.SqlDateFormat);
            }
            else if (Table.BlobProperties.Contains(property) || property.PropertyType.IsCollection())
            {
                return ProtoSerializer.Serialize(value);
            }
            return value.ToString();
        }
    }
}
