using Flex.Attributes;
using Flex.Entities;
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

        public IEnumerable<T> Query(string query)
        {
            List<T> results = new List<T>();

            DbDataReader reader = Table.Database.ExecuteReader(query);

            if (reader.FieldCount != Table.Properties.Length)
            {
                throw new InvalidOperationException("Inccorect mapping.");
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

            return new List<T>();
        }

        private object ConvertProperty(object value, PropertyInfo property) // todo, Blob, Enum, boolean? blob if its collection.
        {
            return Convert.ChangeType(value, property.PropertyType, CultureInfo.InvariantCulture);
        }
    }
}
