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

            StringBuilder queryContent = new StringBuilder();

            int id = 0;

            foreach (var entity in entities)
            {
                queryContent.Append("(");

                foreach (var property in Table.Properties)
                {
                    queryContent.Append(string.Format(Table.Database.Provider.ParameterPrefix + "{0}{1},", property.Name, id));

                    object value = ConvertProperty(property, property.GetValue(entity));

                    DbParameter parameter = command.CreateParameter();

                    parameter.ParameterName = Table.Database.Provider.ParameterPrefix + property.Name + id;
                    parameter.Value = value;

                    command.Parameters.Add(parameter);
                }

                queryContent = queryContent.Remove(queryContent.Length - 1, 1);
                queryContent.Append(")");

                queryContent.Append(',');


                id++;
            }

            queryContent = queryContent.Remove(queryContent.Length - 1, 1);

            command.CommandText = string.Format(SQLConstants.Insert, Table.Name, string.Format("{0}", queryContent.ToString()));

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
