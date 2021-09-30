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

                string prefix = "";

                foreach (var property in Table.Properties)
                {
                    queryContent.Append(prefix);
                    prefix = ",";

                    object value = ConvertProperty(property, property.GetValue(entity));

                    if (value is byte[])
                    {
                        queryContent.Append(string.Format(Table.Database.Provider.ParameterPrefix + "{0}{1}", property.Name, id));
                        string name = Table.Database.Provider.ParameterPrefix + property.Name + id;
                        DbParameter parameter = command.CreateParameter();
                        parameter.ParameterName = name;
                        parameter.Value = value;
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        queryContent.Append("'" + value + "'");
                    }
                }

                queryContent.Append(")");
                queryContent.Append(',');


                id++;
            }

            queryContent = queryContent.Remove(queryContent.Length - 1, 1);

            var properties = string.Join(',', Table.Properties.Select(x => x.Name));

            command.CommandText = string.Format(SQLQueries.INSERT, Table.Name, properties, string.Format("{0}", queryContent.ToString()));
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

                    var primary = Table.PrimaryAccessor.Get(entity);
                    command.CommandText += string.Format(SQLQueries.UPDATE, Table.Name, text, Table.PrimaryProperty.Name, primary.ToString()) + ";";

                    i++;
                }

                return command.ExecuteNonQuery();
            }

        }

        public int Delete(IEnumerable<T> entities) 
        {
            var uids = entities.Select(x => Table.PrimaryAccessor.Get(x)).ToArray();
            string queryContent = string.Join(",", uids);
            return Table.Database.Provider.NonQuery(string.Format(SQLQueries.DELETE_IN, Table.Name, queryContent));
        }

        private object ConvertProperty(PropertyInfo property, object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                value = ((DateTime)value).ToString(SQLQueries.DATE_FORMAT);
            }
            else if (Table.BlobProperties.Contains(property) || property.PropertyType.IsCollection())
            {
                return ProtoSerializer.Serialize(value);
            }
            return value.ToString();
        }
    }
}
