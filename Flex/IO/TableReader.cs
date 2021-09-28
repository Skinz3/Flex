using Flex.Attributes;
using Flex.Entities;
using Flex.Exceptions;
using Flex.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Flex.IO
{
    public class TableReader<T> where T : IEntity
    {
        private Table<T> Table
        {
            get;
            set;
        }
        public TableReader(Table<T> table)
        {
            this.Table = table;
        }

        public IEnumerable<T> Read(string query)
        {
            List<T> results = new List<T>();

            using (DbCommand command = Table.Database.CreateSqlCommand())
            {
                command.CommandText = query;

                using (DbDataReader reader = command.ExecuteReader())
                {

                    if (reader.FieldCount != Table.Properties.Length)
                    {
                        throw new InvalidMappingException("The mapping of table '" + Table.Name + "' is not consistent with the SQL structure.");
                    }
                    while (reader.Read())
                    {
                        var obj = new object[Table.Properties.Length];

                        for (var i = 0; i < reader.FieldCount; i++) // O(2n) complexity necessary ? only one for loop ?
                        {
                            obj[i] = ConvertProperty(reader[i], Table.Properties[i]);
                        }

                        T entity = (T)Activator.CreateInstance(typeof(T));

                        for (int i = 0; i < Table.Properties.Length; i++)
                        {
                            Table.Properties[i].SetValue(entity, obj[i]);
                        }

                        results.Add(entity);
                    }
                }
            }

            return results;
        }

        private object ConvertProperty(object value, PropertyInfo property) // todo, Blob, Enum, boolean? blob if its collection.
        {
            if (property.PropertyType.IsCollection() || Table.BlobProperties.Contains(property))
            {
                return ProtoSerializer.Deserialize(property.PropertyType, (byte[])value);
            }
            return Convert.ChangeType(value, property.PropertyType, CultureInfo.InvariantCulture);
        }
    }
}
