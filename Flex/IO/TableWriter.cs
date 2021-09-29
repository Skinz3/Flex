using Flex.Attributes;
using Flex.Entities;
using Flex.Exceptions;
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

        public int Insert(IEnumerable<T> entities)
        {
            if (entities.Count() == 0)
            {
                return 0;
            }

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

            return command.ExecuteNonQuery();
        }

        public int Update(IEnumerable<T> entities)
        {
            if (entities.Count() == 0)
            {
                return 0;
            }
            if (Table.UpdateProperties.Length == 0)
            {
                throw new InvalidMappingException("Unable to update elements. " + Table.Name + " has no update property.");
            }

            using (DbCommand command = Table.Database.Provider.CreateSqlCommand())
            {
                int i = 0;

                foreach (var entity in entities)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var property in Table.UpdateProperties)
                    {
                        sb.Append(string.Format("{2} = {3}{0}{1},", property.Name, i, property.Name, Table.Database.Provider.ParameterPrefix));

                        MySqlParameter parameter = new MySqlParameter(Table.Database.Provider.ParameterPrefix + property.Name + i,
                            ConvertProperty(property, property.GetValue(entity)));

                        command.Parameters.Add(parameter);
                    }

                    sb = sb.Remove(sb.Length - 1, 1);

                    var text = string.Format("{0}", string.Join(",", sb.ToString()));

                    var primary = Table.PrimaryProperty.GetValue(entity);
                    command.CommandText += string.Format(SQLConstants.Update, Table.Name, text, Table.PrimaryProperty.Name, primary.ToString()) + ";";

                    i++;
                }

                return command.ExecuteNonQuery();
            }

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
